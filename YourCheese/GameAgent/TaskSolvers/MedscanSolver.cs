using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class MedscanSolver : TaskSolver
    {
        public void Solve(DirectBitmap screen)
        {
            System.Threading.Thread.Sleep(8000);
        }

        public void abort()
        {

        }
    }
}
