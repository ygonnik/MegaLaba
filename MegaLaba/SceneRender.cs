using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MegaLaba
{
    /// <summary>
    /// Класс отрисовки элементов
    /// </summary>
    class SceneRender
    {
        int windowWidth; // Ширина окна
        int windowHeight; // Высота окна

        char[,] window; // Матрица окна

        public SceneRender()
        {
            windowHeight = GameSettings.ConsoleHeight;
            windowWidth = GameSettings.ConsoleWidth;
            window = new char[GameSettings.ConsoleHeight, GameSettings.ConsoleWidth];

            Console.WindowHeight = GameSettings.ConsoleHeight; // Установка высоты консоли
            Console.WindowWidth = GameSettings.ConsoleWidth; // Установка ширины консоли

            Console.CursorVisible = false; // Отменить видимость курсора на экране
            Console.SetCursorPosition(0, 0); // Позиция курсора
        }

        /// <summary>
        /// Рендер сцены
        /// </summary>
        public void Render(Scene scene)
        {
            AddInvadersForRendering(scene.invaders);
            AddGroundForRendering(scene.ground);
            AddPlayerMisslesForRendering(scene.playerMissle);
            AddInvadersMisslesForRendering(scene.invaderMissle);

            AddPlayerForRendering(scene.player);

            string render = $"Ваши жизни: {GameSettings.PlayerLifes}\r\n";

            for (int y = 0; y < windowHeight; y++)
            {
                for (int x = 0; x < windowWidth; x++)
                {
                    render += window[y, x];
                }
                render += "\r\n";
            }
            Console.WriteLine(render); //вывод на экран
            Console.SetCursorPosition(0, 0);
        }

        public void AddGameObjectForRendering(GameObject gameObject) //метод, размещающий объект
        {
            if (gameObject.GameObjectLocation.Y < window.GetLength(0) &&
                gameObject.GameObjectLocation.X < window.GetLength(1))
            {
                window[gameObject.GameObjectLocation.Y, gameObject.GameObjectLocation.X] = gameObject.Figure;
            }
        }

        public void AddPlayerForRendering(object gameObject) //метод, размещающий объект
        {
            int size = Marshal.SizeOf(typeof(int));
            int xPlayer, yPlayer;
            using (var accessorPlayer = GameEngine.PlayerLocation.CreateViewAccessor(0, GameEngine.length, MemoryMappedFileAccess.Read))
            {
                accessorPlayer.Read(0, out xPlayer);
                accessorPlayer.Read(size, out yPlayer);
            }
            object gameObjectFigure;
            Scene.PlayerT.GetProperty("X").SetValue(gameObject, xPlayer);
            Scene.PlayerT.GetProperty("Y").SetValue(gameObject, yPlayer);
            gameObjectFigure = Scene.PlayerT.GetProperty("Figure").GetValue(gameObject);

            if (yPlayer < window.GetLength(0) &&
                xPlayer < window.GetLength(1))
            {
                window[yPlayer, xPlayer] = Convert.ToChar(gameObjectFigure);
            }
        }

        public void AddInvadersForRendering(List<GameObject> gameObjects)//метод, размещающий коллекции
        {
            GameEngine.mutex.WaitOne();
            foreach (GameObject gameObject in gameObjects)
                AddGameObjectForRendering(gameObject);
            GameEngine.mutex.ReleaseMutex();
        }

        public void AddPlayerMisslesForRendering(List<GameObject> gameObjects)//метод, размещающий коллекции
        {
            GameEngine.autoResetEvent.WaitOne();
            foreach (GameObject gameObject in gameObjects)
                AddGameObjectForRendering(gameObject);
            GameEngine.autoResetEvent.Set();
        }

        public void AddInvadersMisslesForRendering(List<GameObject> gameObjects)//метод, размещающий коллекции
        {
            GameEngine.semaphore.WaitOne();
            foreach (GameObject gameObject in gameObjects)
                AddGameObjectForRendering(gameObject);
            GameEngine.semaphore.Release();
        }

        public void AddGroundForRendering(List<GameObject> gameObjects)//метод, размещающий коллекции
        {
            foreach (GameObject gameObject in gameObjects)
                AddGameObjectForRendering(gameObject);
        }

        public void ClearScene() //метод очищения экрана
        {
            for (int y = 0; y < windowHeight; y++)
            {
                for (int x = 0; x < windowWidth; x++)
                {
                    window[y, x] = ' ';
                }
            }
        }

        public void RenderGameOver()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
            Console.WriteLine("Game Over");
        }

    }
}