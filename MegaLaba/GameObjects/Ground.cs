using System.Collections.Generic;

namespace MegaLaba
{
    /// <summary>
    /// Класс земли
    /// </summary>
    class Ground : GameObject
    {
        public Ground(GameObjectLocation objectPlace)
        {
            Figure = GameSettings.Ground;
            GameObjectLocation = objectPlace;
        }

        public static List<GameObject> GetGround()
        {
            List<GameObject> ground = new List<GameObject>();

            int startX = GameSettings.GroundStartX;
            int startY = GameSettings.GroundStartY;

            for (int y = 0; y < GameSettings.NumberOfGroundRows; y++) //по строкам
            {
                for (int x = 0; x < GameSettings.NumberOfGround; x++) //по эл-м
                {
                    GameObjectLocation objectPlace = new GameObjectLocation() { X = startX + x, Y = startY + y };
                    GameObject groundObj = new Ground(objectPlace);
                    ground.Add(groundObj);
                }
            }
            return ground;
        }
    }
}
