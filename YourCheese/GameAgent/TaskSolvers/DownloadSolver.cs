using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class DownloadSolver : TaskSolver
    {
        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();
            taskInput.mouseClick(new Vector2(969, 664));
            System.Threading.Thread.Sleep(10000);
        }

        public void abort()
        {

        }
    }
}
