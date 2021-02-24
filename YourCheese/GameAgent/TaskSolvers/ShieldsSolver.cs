using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class ShieldsSolver : TaskSolver
    {
        private Dictionary<Vector2, Vector2> shieldsLocation = new Dictionary<Vector2, Vector2>()
        {
            { new Vector2(746, 298), new Vector2(746, 403) },
            { new Vector2(964, 175), new Vector2(964, 284) },
            { new Vector2(1172, 302), new Vector2(1172, 402) },
            { new Vector2(734, 554), new Vector2(734, 654) },
            { new Vector2(964, 426), new Vector2(964, 526) },
            { new Vector2(1172, 557), new Vector2(1172, 657) },
            { new Vector2(964, 682), new Vector2(964, 782) }
        };

        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();
            foreach (KeyValuePair<Vector2, Vector2> entry in shieldsLocation)
            {
                if (screen.GetPixel((int)entry.Key.x, (int)entry.Key.y).G < 50)
                {
                    taskInput.mouseClick(entry.Value);
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        public void abort()
        {
            
        }
    }
}
