using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class DistributorSolver : TaskSolver
    {

        private Dictionary<Vector2, Vector2> switchLocation = new Dictionary<Vector2, Vector2>()
        {
            { new Vector2(115, 32), new Vector2(1237, 311) },
            { new Vector2(115, 298), new Vector2(1237, 582) },
            { new Vector2(115, 569), new Vector2(1237, 837) }            
        };

        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();
            foreach (KeyValuePair<Vector2, Vector2> entry in switchLocation)
            {
                //GameCapture.getGameScreenAsImage(new System.Drawing.Rectangle(1109, 204, 257, 608));
                screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(1109, 204, 257, 608));
                var pixel = screen.GetPixel((int)entry.Key.x, (int)entry.Key.y);
                while (pixel.R < 5 &&
                    pixel.G < 5 &&
                    pixel.B < 5)
                {
                    //System.Threading.Thread.Sleep(2);
                    screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(1109, 204, 257, 608)); 
                }
                taskInput.mouseClick(entry.Value);
            }
            taskInput.closeTask();
        }

        public void abort()
        {

        }
    }
}
