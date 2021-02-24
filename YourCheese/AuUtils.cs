using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese
{
    class AuUtils
    {
        public static bool arePositionsClose(Vector2 pos1, Vector2 pos2)
        {
            return (vectorDistance(pos1, pos2) < 0.5);
        }

        public static double vectorDistance(Vector2 pos1, Vector2 pos2) 
        {
            return Math.Sqrt(((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y)));
        }
    }
}
