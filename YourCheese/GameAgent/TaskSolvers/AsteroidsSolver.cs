using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class AsteroidsSolver : TaskSolver
    {

        private int xOffset = 1138;
        private int yOffset = 81;
        private bool varAbort = false;

        public void Solve(DirectBitmap screen)
        {
            screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(xOffset, yOffset, 225, 863));
            TaskInput taskInput = new TaskInput();

            while (screen.GetPixel(223, 9).G > 50 && screen.GetPixel(223, 9).B < 70 && !varAbort)
            {
                List<Vector2> asteroids = new List<Vector2>();
                screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(xOffset, yOffset, 225, 863));

                for (int x = 0; x < 178; x += 10)
                {
                    for (int y = 64; y < 863 - 40; y += 10)
                    {
                        if (isAsteroid(screen, x, y))
                        {
                            asteroids.Add(new Vector2(x, y));
                        }
                    }
                }
                
                foreach (var asteroid in asteroids)
                {
                    taskInput.mouseClick(new Vector2(asteroid.x + xOffset - 5, asteroid.y + yOffset));
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

        private bool isAsteroid(DirectBitmap screen, int x, int y)
        {
            for (int n = -15; n < 15; n++)
            {
                for (int m = -15; m < 15; m++)
                {
                    if (screen.GetPixel(x + n, y + m).G < 70 && screen.GetPixel(x + n, y + m).G > 50 && screen.GetPixel(x + n, y + m).R < 30 && screen.GetPixel(x + n, y + m).R > 10)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void abort()
        {
            varAbort = true;
        }
    }
}
