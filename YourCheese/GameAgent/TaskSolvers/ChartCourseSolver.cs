using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class ChartCourseSolver : TaskSolver
    {
        private int xOffset = 464;
        private int yOffset = 263;

        public void Solve(DirectBitmap screen)
        {
            screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(xOffset, yOffset, 990, 554));
            TaskInput taskInput = new TaskInput();

            List<Vector2> points = new List<Vector2>();
            int dist = 1;
            
            for (int x = 7; x < screen.Width - 11; x += 4)
            {
                for (int y = 7; y < screen.Height - 11; y += 4)
                {
                    
                    if (isPoint(screen, x, y, dist))
                    {
                        dist = 3;
                        points.Add(new Vector2(x + xOffset, y + yOffset));
                        y += 70;
                        x += 70;
                    }
                }
            }

            if (points.Count < 5)
            {
                return;
            }

            for (int i=0; i < 4; i++)
            {
                var origin = points[i];
                var destination = points[i + 1];
                bool isUp = destination.y < origin.y;
                double yDiff = 50;
                if (isUp) yDiff = -50;
                taskInput.dragMouseLinear(origin, new Vector2(destination.x + 50, destination.y + yDiff), 250);
            }

            System.Threading.Thread.Sleep(50);
            taskInput.closeTask();
        }

        private bool isPoint(DirectBitmap screen, int x, int y, int dist)
        {
            for (int n = -dist; n < dist; n++)
            {
                for (int m = -dist; m < dist; m++)
                {
                    if (screen.GetPixel(x + n, y + m).R < 240)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void abort()
        {

        }
    }
}
