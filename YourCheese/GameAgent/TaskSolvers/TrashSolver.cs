using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class TrashSolver : TaskSolver
    {
        public void Solve(DirectBitmap screen)
        {

            TaskInput taskInput = new TaskInput();
            taskInput.dragMouseNoRelease(new Vector2(1255, 417), new Vector2(1255, 673));
            System.Threading.Thread.Sleep(2000);
            taskInput.releaseMouse();
        }

        public void abort()
        {

        }
    }
}
