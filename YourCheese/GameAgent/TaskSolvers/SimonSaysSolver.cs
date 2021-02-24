using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class SimonSaysSolver : TaskSolver
    {
        /*private Dictionary<Vector2, Vector2> buttonLocations = new Dictionary<Vector2, Vector2>()
        {
            { new Vector2(523, 466), new Vector2(1137, 466) }, { new Vector2(652, 466), new Vector2(1264, 466) }, { new Vector2(772, 466), new Vector2(1384, 466) },
            { new Vector2(523, 583), new Vector2(1137, 583) }, { new Vector2(652, 583), new Vector2(1264, 583) }, { new Vector2(772, 583), new Vector2(1384, 583) },
            { new Vector2(523, 715), new Vector2(1137, 715) }, { new Vector2(652, 715), new Vector2(1264, 715) }, { new Vector2(772, 715), new Vector2(1384, 715) }

        };*/

        private bool varAbort = false;
        private Dictionary<Vector2, Vector2> buttonLocations = new Dictionary<Vector2, Vector2>()
        {
            { new Vector2(162, 268), new Vector2(1137, 466) }, { new Vector2(299, 268), new Vector2(1264, 466) }, { new Vector2(415, 268), new Vector2(1384, 466) },
            { new Vector2(162, 398), new Vector2(1137, 583) }, { new Vector2(299, 398), new Vector2(1264, 583) }, { new Vector2(415, 398), new Vector2(1384, 583) },
            { new Vector2(162, 525), new Vector2(1137, 715) }, { new Vector2(299, 525), new Vector2(1264, 715) }, { new Vector2(415, 525), new Vector2(1384, 715) }

        };

        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();

            // first reset the state
            foreach (KeyValuePair<Vector2, Vector2> entry in buttonLocations)
            {
                taskInput.mouseClick(entry.Value);
                System.Threading.Thread.Sleep(20);
            }

            System.Threading.Thread.Sleep(460);


            for (int i=0; i<5; i++)
            {
                List<Vector2> buttons = new List<Vector2>();

                while (buttons.Count < i+1)
                {
                    if (varAbort) return;
                    Vector2 button = Vector2.Zero;
                    while (button.x == 0)
                    {
                        screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(357, 196, 599, 676));
                        button = indicatedButton(screen);
                    }
                    buttons.Add(button);

                    Vector2 emptyScreen = button;
                    while (emptyScreen.x != 0)
                    {
                        screen = GameCapture.getGameScreen(new System.Drawing.Rectangle(357, 196, 599, 676));
                        emptyScreen = indicatedButton(screen);
                    }
                }

                System.Threading.Thread.Sleep(50);

                foreach (var button in buttons)
                {
                    System.Threading.Thread.Sleep(20);
                    taskInput.mouseClick(button); 
                }
            }
        }

        private Vector2 indicatedButton(DirectBitmap screen)
        {
            foreach (KeyValuePair<Vector2, Vector2> entry in buttonLocations)
            {
                if (screen.GetPixel((int)entry.Key.x, (int)entry.Key.y).B > 50)
                {
                    return entry.Value;
                }
                //else if (screen.GetPixel((int)entry.Key.x, (int)entry.Key.y).R > 50)
                //{
                //    throw new Exception();
                //}
            }
            return Vector2.Zero;
        }

        public void abort()
        {
            varAbort = true;
        }
    }
}
