using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MegaLaba
{
    /// <summary>
    /// Класс игрового движка (для логики перемещения объектов)
    /// </summary>
    class GameEngine
    {
        private bool IsGameRunning;
        private SceneRender sceneRender; // Рендер сцены
        private Scene scene; // Сцена

        private bool CanShoot = true; // может ли гг стрелять
        private System.Timers.Timer CanShootTimer;
        private System.Timers.Timer LifeRecoveryTimer;

        public static Mutex mutex = new Mutex();
        public static AutoResetEvent autoResetEvent = new AutoResetEvent(true);
        public static Semaphore semaphore = new Semaphore(1, 1);

        public static int length = 100;

        public static MemoryMappedFile PlayerLocation = MemoryMappedFile.CreateNew("Player", length);


        /// <summary>
        /// Инициализация Игровых настроек, Сцены, Рендер
        /// </summary>
        /// <param name="GameSettings"></param>
        public GameEngine()
        {
            IsGameRunning = true;
            scene = new Scene();
            sceneRender = new SceneRender();
            CanShootTimer = new System.Timers.Timer(GameSettings.MissleFrequency);
            CanShootTimer.Elapsed += ChangeCanShootFlagToTrue;
            CanShootTimer.AutoReset = true;
            CanShootTimer.Enabled = true;

            LifeRecoveryTimer = new System.Timers.Timer(GameSettings.PlayerLifeRecoveryTime);
            LifeRecoveryTimer.Elapsed += RecoveryLife;
            LifeRecoveryTimer.AutoReset = true;
            LifeRecoveryTimer.Enabled = false;
        }

        public void Run()
        {
            Thread tSwarmMove = new Thread(InvadersMove);
            tSwarmMove.Start();
            Thread tPlayerMissleMove = new Thread(PlayerMissleMove);
            tPlayerMissleMove.Start();
            Thread tInvaderMissleMove = new Thread(InvaderMissleMove);
            tInvaderMissleMove.Start();
            Thread tShotInvader = new Thread(ShotInvader);
            tShotInvader.Start();
            int size = Marshal.SizeOf(typeof(int));
            using (var accessorPlayer = PlayerLocation.CreateViewAccessor(0, length, MemoryMappedFileAccess.Write))
            {
                accessorPlayer.Write(0, GameSettings.PlayerStartX);
                accessorPlayer.Write(size, GameSettings.PlayerStartY);
            }
            do
            {
                sceneRender.Render(scene);
                Thread.Sleep(GameSettings.GameSpeed); //сокращение частоты состояния сцены
                sceneRender.ClearScene();

            } while (IsGameRunning);

            sceneRender.RenderGameOver();
        }

        public void MovePlayerLeft()//вычисление состояния с измененим движения влево
        {

            object x = Scene.PlayerT.GetProperty("X").GetValue(scene.player);
            if (Convert.ToInt32(x) > 1)
                Scene.PlayerT.GetProperty("X").SetValue(scene.player, Convert.ToInt32(x) - 1);
            using (var accessorPlayer = PlayerLocation.CreateViewAccessor(0, length, MemoryMappedFileAccess.Write))
            {
                accessorPlayer.Write(0, Convert.ToInt32(x) - 1);
            }
        }

        public void MovePlayerRight()//вычисление состояния с измененим движения вправо
        {
            object x = Scene.PlayerT.GetProperty("X").GetValue(scene.player);
            if (Convert.ToInt32(x) > 1)
                Scene.PlayerT.GetProperty("X").SetValue(scene.player, Convert.ToInt32(x) + 1);
            using (var accessorPlayer = PlayerLocation.CreateViewAccessor(0, length, MemoryMappedFileAccess.Write))
            {
                accessorPlayer.Write(0, Convert.ToInt32(x) + 1);
            }
        }

        public void InvadersMove()//метод для перемещения захватчиков
        {
            do
            {
                object y = Scene.PlayerT.GetProperty("Y").GetValue(scene.player);
                for (int i = 0; i < scene.invaders.Count; i++)
                {
                    GameObject invader = scene.invaders[i];
                    invader.GameObjectLocation.Y++;
                    if (invader.GameObjectLocation.Y == Convert.ToInt32(y))
                        IsGameRunning = false;
                }
                Thread.Sleep(GameSettings.SwarmSpeed);

            } while (IsGameRunning);
        }



        public void ShotPlayer()
        {
            if (CanShoot == true)
            {
                object x = Scene.PlayerT.GetProperty("X").GetValue(scene.player);
                object y = Scene.PlayerT.GetProperty("Y").GetValue(scene.player);
                GameObjectLocation location = new GameObjectLocation()
                {
                    X = Convert.ToInt32(x),
                    Y = Convert.ToInt32(y)
                };
                PlayerMissle missle = new PlayerMissle(location);
                autoResetEvent.WaitOne();
                scene.playerMissle.Add(missle);
                autoResetEvent.Set();
                CanShoot = false;
            }
        }

        public void ShotInvader()
        {
            Random rnd = new Random();
            int temp;
            IEnumerable<GameObject> invadersPlaces;
            List<GameObject> list = new List<GameObject>();
            InvaderMissle missle;
            do
            {
                if (scene.invaderMissle.Count < GameSettings.MaxInvaderMissles)
                {
                    do
                    {
                        temp = rnd.Next(0, scene.invaders.Count);
                        missle = new InvaderMissle(scene.invaders[temp].GameObjectLocation);
                        mutex.WaitOne();
                        invadersPlaces = (from a in scene.invaders
                                          where a.GameObjectLocation.Equals(missle.GameObjectLocation)
                                          select a);
                        list = invadersPlaces.ToList();
                        mutex.ReleaseMutex();
                        if (list.Count == 0)
                        {
                            semaphore.WaitOne();
                            scene.invaderMissle.Add(missle);
                            semaphore.Release();

                        }
                    } while (list.Count != 0);
                }
            } while (IsGameRunning);

        }

        public void PlayerMissleMove()
        {
            do
            {
                for (int x = 0; x < scene.playerMissle.Count; x++)
                {
                    GameObject missle = scene.playerMissle[x];
                    if (missle.GameObjectLocation.Y == 1)
                    {
                        autoResetEvent.WaitOne();
                        scene.playerMissle.RemoveAt(x);
                        autoResetEvent.Set();
                    }

                    missle.GameObjectLocation.Y--;
                    for (int i = 0; i < scene.invaders.Count; i++)
                    {
                        GameObject invader = scene.invaders[i];
                        if (missle.GameObjectLocation.Equals(invader.GameObjectLocation))
                        {
                            mutex.WaitOne();
                            autoResetEvent.WaitOne();
                            scene.invaders.RemoveAt(i);
                            scene.playerMissle.RemoveAt(x);
                            mutex.ReleaseMutex();
                            autoResetEvent.Set();
                            break;
                        }
                    }
                }
                Thread.Sleep(GameSettings.MissleSpeed);
            } while (IsGameRunning);

        }

        public void InvaderMissleMove()
        {
            do
            {
                object X = Scene.PlayerT.GetProperty("X").GetValue(scene.player);
                object Y = Scene.PlayerT.GetProperty("Y").GetValue(scene.player);
                GameObjectLocation location = new GameObjectLocation()
                {
                    X = Convert.ToInt32(X),
                    Y = Convert.ToInt32(Y)
                };
                for (int x = 0; x < scene.invaderMissle.Count; x++)
                {
                    GameObject missle = scene.invaderMissle[x];
                    if (missle.GameObjectLocation.Y == GameSettings.GroundStartY)
                    {
                        semaphore.WaitOne();
                        scene.invaderMissle.RemoveAt(x);
                        semaphore.Release();
                    }

                    missle.GameObjectLocation.Y++;
                    if (missle.GameObjectLocation.Equals(location))
                    {
                        if (GameSettings.PlayerLifes - GameSettings.MissleDamage < 1)
                        {
                            sceneRender.RenderGameOver();
                            IsGameRunning = false;
                            break;
                        }
                        else
                        {
                            GameSettings.PlayerLifes -= GameSettings.MissleDamage;
                            LifeRecoveryTimer.Enabled = true;
                        }
                    }
                }
                Thread.Sleep(GameSettings.MissleSpeed);
            } while (IsGameRunning);

        }

        public void ChangeCanShootFlagToTrue(Object source, ElapsedEventArgs e)
        {
            CanShoot = true;
        }

        public void RecoveryLife(Object source, ElapsedEventArgs e)
        {
            if (GameSettings.PlayerLifes < GameSettings.MaxPlayerLifes)
            {
                GameSettings.PlayerLifes += 1;
                if (GameSettings.PlayerLifes == GameSettings.MaxPlayerLifes)
                    LifeRecoveryTimer.Enabled = false;
            }
        }
    }
}
