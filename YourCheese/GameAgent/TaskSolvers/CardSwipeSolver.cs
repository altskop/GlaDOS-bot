using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class CardSwipeSolver : TaskSolver
    {
        public void Solve(DirectBitmap screen)
        {

            TaskInput taskInput = new TaskInput();
            taskInput.mouseClick(new Vector2(684, 816));
            System.Threading.Thread.Sleep(1300);
            taskInput.dragMouseLinear(new Vector2(523, 409), new Vector2(1468, 409), 1300);
        }

        public void abort()
        {

        }
    }
}
