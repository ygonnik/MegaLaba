using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaLaba
{
    class InvaderMissle : GameObject
    {
        public InvaderMissle(GameObjectLocation objectPlace)
        {
            GameObjectLocation misslePlace = new GameObjectLocation() { X = objectPlace.X, Y = objectPlace.Y + 1 };
            Figure = GameSettings.Missle;
            GameObjectLocation = misslePlace;
        }
    }
}
