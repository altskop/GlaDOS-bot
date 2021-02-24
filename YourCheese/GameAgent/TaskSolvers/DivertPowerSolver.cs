using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class DivertPowerSolver : TaskSolver
    {

        private Dictionary<Vector2, Vector2> switchLocation = new Dictionary<Vector2, Vector2>()
        {
            { new Vector2(599, 773), new Vector2(593, 648) },
            { new Vector2(692, 773), new Vector2(687, 648) },
            { new Vector2(788, 773), new Vector2(782, 648) },
            { new Vector2(889, 773), new Vector2(881, 648) },
            { new Vector2(984, 773), new Vector2(977, 648) },
            { new Vector2(1079, 773), new Vector2(1071, 648) },
            { new Vector2(1177, 773), new Vector2(1169, 648) },
            { new Vector2(1274, 773), new Vector2(1269, 648) }
        };

        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();
            foreach (KeyValuePair<Vector2, Vector2> entry in switchLocation)
            {
                if (screen.GetPixel((int)entry.Key.x, (int)entry.Key.y).R > 220)
                {
                    taskInput.dragMouseLinear(entry.Key, entry.Value, 200);
                }
            }
            
        }

        public void abort()
        {

        }
    }
}
