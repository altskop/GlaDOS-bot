using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class SteeringSolver : TaskSolver
    {
        public void Solve(DirectBitmap screen)
        {

            TaskInput taskInput = new TaskInput();
            taskInput.mouseClick(new Vector2(958, 538));
        }

        public void abort()
        {

        }
    }
}
