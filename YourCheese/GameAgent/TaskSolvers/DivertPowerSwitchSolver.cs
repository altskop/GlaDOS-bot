using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class DivertPowerSwitchSolver : TaskSolver
    {

        public void Solve(DirectBitmap screen)
        {
            TaskInput taskInput = new TaskInput();
            taskInput.mouseClick(new Vector2(958, 541));
        }

        public void abort()
        {

        }
    }
}
