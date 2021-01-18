using System;
using System.Collections.Generic;

namespace MegaLaba
{
    /// <summary>
    /// Класс сцены
    /// </summary>
    class Scene
    {
        /// <summary>
        /// Коллекция объектов кораблей пришельцев (рой)
        /// </summary>
        public List<GameObject> invaders;

        /// <summary>
        /// Коолекция объектов земли
        /// </summary>
        public List<GameObject> ground;

        /// <summary>
        /// Пользовательский корабль
        /// </summary>
        public object player;

        /// <summary>
        /// Коллекция объектов ракет
        /// </summary>
        public List<GameObject> playerMissle;
        public List<GameObject> invaderMissle;

        public static Type PlayerT = Type.GetTypeFromProgID("ComObjects.Player");

        public Scene() //инициализация коллекции
        {
            invaders = Invader.GetInvaders();
            ground = Ground.GetGround();
            
            player = Activator.CreateInstance(PlayerT, GameSettings.PlayerStartX, GameSettings.PlayerStartY, GameSettings.Player);
            playerMissle = new List<GameObject>();
            invaderMissle = new List<GameObject>(); 
        }
    }
}