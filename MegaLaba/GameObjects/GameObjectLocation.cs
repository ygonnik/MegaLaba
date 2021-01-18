using System;
using System.Runtime.InteropServices;

namespace MegaLaba
{
    /// <summary>
    /// Класс координат 
    /// </summary>
    class GameObjectLocation
    {
        public int X { get; set; }

        public int Y { get; set; }

        public override bool Equals(object place)
        {
            GameObjectLocation location = place as GameObjectLocation;
            if (location != null && X == location.X && Y == location.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}