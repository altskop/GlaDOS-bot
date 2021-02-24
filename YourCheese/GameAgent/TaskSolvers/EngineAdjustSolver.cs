using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class EngineAdjustSolver : TaskSolver
    {

        public List<Vector2> points = new List<Vector2>() 
        {
            new Vector2(1321, 169),
            new Vector2(1299, 255),
            new Vector2(1279, 279),
            new Vector2(1275, 299),
            new Vector2(1260, 365),
            new Vector2(1250, 428),
            new Vector2(1243, 490),
            new Vector2(1242, 549),
            new Vector2(1244, 612),
            new Vector2(1254, 671),
            new Vector2(1262, 720),
            new Vector2(1274, 770),
            new Vector2(1285, 811),
            new Vector2(1295, 840),
            new Vector2(1308, 878),
            new Vector2(1320, 907)
        };

        private Vector2 destination = new Vector2(1213, 531);

        public void Solve(DirectBitmap screen)
        {
            Vector2 start = new Vector2();
            foreach (var point in points)
            {
                if ((screen.GetPixel((int)point.x, (int)point.y).R > 80 &&
                    screen.GetPixel((int)point.x, (int)point.y).G > 80 &&
                    screen.GetPixel((int)point.x, (int)point.y).B > 80))
                {
                    start = point;
                    break;
                }
            }
            new TaskInput().dragMouseLinear(start, destination, 400);
            new TaskInput().closeTask();
        }

        public void abort()
        {

        }

    }
}
