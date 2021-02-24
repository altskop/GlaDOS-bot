using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class InspectSampleSolver : TaskSolver
    {
        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();
            taskInput.mouseClick(new Vector2(1261, 936));
            System.Threading.Thread.Sleep(300);
            taskInput.closeTask();
        }

        public void abort()
        {

        }
    }
}
