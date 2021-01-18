using System.Collections.Generic;

namespace MegaLaba
{
    /// <summary>
    /// Класс корабля пришельца
    /// </summary>
    class Invader : GameObject
    {
        public Invader(GameObjectLocation objectPlace)
        {
            Figure = GameSettings.Invader;
            GameObjectLocation = objectPlace;
        }

        public static List<GameObject> GetInvaders()
        {
            List<GameObject> swarm = new List<GameObject>();

            int startX = GameSettings.SwarmStartX;
            int startY = GameSettings.SwarmStartY;

            for (int y = 0; y < GameSettings.NumberOfSwarmRows; y++) // По координате У
            {
                for (int x = 0; x < GameSettings.NumberOfSwarm; x++) // По координате Х
                {
                    GameObjectLocation objectPlace = new GameObjectLocation() { X = startX + x, Y = startY + y };
                    GameObject invader = new Invader(objectPlace);
                    swarm.Add(invader);
                }
            }

            return swarm;
        }
    }
}