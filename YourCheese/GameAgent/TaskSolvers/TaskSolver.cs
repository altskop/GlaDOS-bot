using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    public interface TaskSolver
    {
        void Solve(DirectBitmap screen);
        void abort();
    }
}
