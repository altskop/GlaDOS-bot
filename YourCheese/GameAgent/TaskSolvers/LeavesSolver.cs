using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class LeavesSolver : TaskSolver
    {

        private int xOffset = 696;
        private int yOffset = 98;
        private Vector2 destination = new Vector2(616, 545);
        private bool varAbort = false;

        public void Solve(DirectBitmap screen)
        {
            screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(xOffset, yOffset, 703, 883));
            TaskInput taskInput = new TaskInput();

            List<Vector2> leaves = new List<Vector2>();
            leaves.Add(Vector2.Zero);
            while (leaves.Count > 0 && !varAbort && inTask(screen))
            {
                screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(xOffset, yOffset, 703, 883));
                leaves.Clear();
                for (int x = 16; x < screen.Width - 25; x += 20)
                {
                    for (int y = 16; y < screen.Height - 25; y += 20)
                    {
                        if (isLeaf(screen, x, y))
                        {
                            leaves.Add(new Vector2(x, y));
                            x += 20;
                            y += 20;
                        }
                    }
                }

                foreach (var leaf in leaves)
                {
                    taskInput.dragMouseLinear(new Vector2(leaf.x + xOffset, leaf.y + yOffset), destination, 150);
                    System.Threading.Thread.Sleep(20);
                }
                System.Threading.Thread.Sleep(500);
            }
            System.Threading.Thread.Sleep(200);
            taskInput.closeTask();
        }

        private bool isLeaf(DirectBitmap screen, int x, int y)
        {
            for (int n = -15; n < 15; n++)
            {
                for (int m = -15; m < 15; m++)
                {
                    if (screen.GetPixel(x+n, y + m).B > 120)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool inTask(DirectBitmap screen)
        {
            return (screen.GetPixel(9, 1).B > 180 && screen.GetPixel(698, 1).B > 180 && screen.GetPixel(701, 881).B > 180);
        }

        public void abort()
        {
            varAbort = true;
        }
    }

}
