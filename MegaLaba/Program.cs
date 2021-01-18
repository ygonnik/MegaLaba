using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace MegaLaba
{
    class Program
    {
        //поля с объектами
        static GameEngine gameEngine;

        static UIController uIController;

        static void Main(string[] args)
        {
            TakeSettings("C:\\Users\\ygonnik\\source\\repos\\MegaLaba\\MegaLaba\\bin\\Debug\\megalaba.xlsx");
            Initialize();
            gameEngine.Run();
        }

        /// <summary>
        /// Инициализация полей
        /// </summary>
        public static void Initialize()
        {

            gameEngine = new GameEngine();

            uIController = new UIController();

            uIController.LeftArrowPressed += (obj, arg) => gameEngine.MovePlayerLeft();
            uIController.RightArrowPressed += (obj, arg) => gameEngine.MovePlayerRight();
            uIController.SpacebarPressed += (obj, arg) => gameEngine.ShotPlayer();

            Thread uIthread1 = new Thread(uIController.StartListening);
            uIthread1.Start();
        }

        public static void TakeSettings(string path)
        {
            List<dynamic> list;
            object temp;
            Type GameSettingsT = Type.GetTypeFromProgID("ComObjects.GameSettings");
            object gameSettings = Activator.CreateInstance(GameSettingsT);
            temp = GameSettingsT.InvokeMember("GetSettings", System.Reflection.BindingFlags.InvokeMethod, null, gameSettings, new object[] { path });
            list = temp as List<dynamic>;

            GameSettings.ConsoleWidth = Convert.ToInt32(list[0]);
            GameSettings.ConsoleHeight = Convert.ToInt32(list[1]);
            GameSettings.NumberOfSwarmRows = Convert.ToInt32(list[2]);
            GameSettings.NumberOfSwarm = Convert.ToInt32(list[3]);
            GameSettings.SwarmStartX = Convert.ToInt32(list[4]);
            GameSettings.SwarmStartY = Convert.ToInt32(list[5]);
            GameSettings.Invader = Convert.ToChar(list[6]);
            GameSettings.SwarmSpeed = Convert.ToInt32(list[7]);
            GameSettings.PlayerStartX = Convert.ToInt32(list[8]);
            GameSettings.PlayerStartY = Convert.ToInt32(list[9]);
            GameSettings.Player = Convert.ToChar(list[10]);
            GameSettings.PlayerLifes = Convert.ToInt32(list[11]);
            GameSettings.MaxPlayerLifes = Convert.ToInt32(list[12]);
            GameSettings.PlayerLifeRecoveryTime = Convert.ToInt32(list[13]);
            GameSettings.GroundStartX = Convert.ToInt32(list[14]);
            GameSettings.GroundStartY = Convert.ToInt32(list[15]);
            GameSettings.Ground = Convert.ToChar(list[16]);
            GameSettings.NumberOfGroundRows = Convert.ToInt32(list[17]);
            GameSettings.NumberOfGround = Convert.ToInt32(list[18]);
            GameSettings.Missle = Convert.ToChar(list[19]);
            GameSettings.MissleDamage = Convert.ToInt32(list[20]);
            GameSettings.MissleSpeed = Convert.ToInt32(list[21]);
            GameSettings.MissleFrequency = Convert.ToInt32(list[22]);
            GameSettings.MaxInvaderMissles = Convert.ToInt32(list[23]);
            GameSettings.GameSpeed = Convert.ToInt32(list[24]);
        }

    }
}