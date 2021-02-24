using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class GasSolver : TaskSolver
    {
        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();
            taskInput.mouseDown(new Vector2(1459, 880));
            System.Threading.Thread.Sleep(6500);
            taskInput.releaseMouse();
        }

        public void abort()
        {

        }
    }
}
