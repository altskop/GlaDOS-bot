using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class InspectSampleReadySolver : TaskSolver
    {

        private Dictionary<Vector2, Vector2> vialLocation = new Dictionary<Vector2, Vector2>() 
        {
            { new Vector2(735, 591), new Vector2(731, 848) },
            { new Vector2(849, 591), new Vector2(849, 848) },
            { new Vector2(962, 591), new Vector2(962, 848) },
            { new Vector2(1074, 591), new Vector2(1074, 848) },
            { new Vector2(1189, 591), new Vector2(1189, 848) }
        };

        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();
            foreach (KeyValuePair<Vector2, Vector2> entry in vialLocation)
            {
                if (screen.GetPixel((int)entry.Key.x, (int)entry.Key.y).R > 220)
                {
                    taskInput.mouseClick(entry.Value);
                }
            }
        }

        public void abort()
        {

        }
    }
}
