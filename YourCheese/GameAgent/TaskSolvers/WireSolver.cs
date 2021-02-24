using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.TaskSolvers
{
    class WireSolver : TaskSolver
    {

        Vector2[] leftSide = new Vector2[] {new Vector2(520, 273), new Vector2(516, 459), new Vector2(517, 645), new Vector2(517, 831)};
        Vector2[] rightSide = new Vector2[] { new Vector2(1335, 273), new Vector2(1335, 459), new Vector2(1335, 645), new Vector2(1335, 831) };

        public void Solve(DirectBitmap screen)
        {
            Dictionary<string, Vector2> leftSideWires = new Dictionary<string, Vector2>();
            Dictionary<string, Vector2> rightSideWires = new Dictionary<string, Vector2>();
            TaskInput taskInput = new TaskInput();

            foreach (Vector2 location in leftSide)
            {
                leftSideWires.Add(getColor(screen.GetPixel((int)location.x, (int)location.y)), location);
            }

            foreach (Vector2 location in rightSide)
            {
                rightSideWires.Add(getColor(screen.GetPixel((int)location.x, (int)location.y)), location);
            }

            foreach (KeyValuePair<string, Vector2> entry in leftSideWires)
            {
                taskInput.dragMouse(entry.Value, rightSideWires[entry.Key]);
            }

        }

        private string getColor(System.Drawing.Color color)
        {
            if(color.R > 245 && color.G > 225 && color.B < 15)
            {
                return "yellow";
            }
            else if (color.R > 245 && color.G < 15 && color.B > 225)
            {
                return "magenta";
            }
            else if (color.R > 245 && color.G < 15 && color.B < 15)
            {
                return "red";
            }
            else
            {
                return "blue";
            }
        }

        public void abort()
        {

        }
    }
}
