using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace YourCheese
{
    class TaskInput
    {
        private InputSimulator inputSimulator = new InputSimulator();

        private static double MONITOR_X_OFFSET = 0;

        public void mouseClick(Vector2 position)
        {
            moveMouse(position);
            inputSimulator.Mouse.LeftButtonClick();
        }

        public void mouseDown(Vector2 position)
        {
            moveMouse(position);
            inputSimulator.Mouse.LeftButtonDown();
        }

        public void releaseMouse()
        {
            inputSimulator.Mouse.LeftButtonUp();
        }

        public void pressE()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
        }

        public void pressR()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_R);
        }

        public void pressQ()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_Q);
            System.Threading.Thread.Sleep(50);
        }

        public void pressEsc()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
        }

        public void closeTask()
        {
            mouseClick(new Vector2(1700, 300));
        }

        public void dragMouse(Vector2 position, Vector2 destination)
        {
            moveMouse(position);
            System.Threading.Thread.Sleep(20);
            inputSimulator.Mouse.LeftButtonDown();
            System.Threading.Thread.Sleep(20);
            moveMouse(destination);
            System.Threading.Thread.Sleep(20);
            inputSimulator.Mouse.LeftButtonUp();
            System.Threading.Thread.Sleep(20);
        }

        public void dragMouseNoRelease(Vector2 position, Vector2 destination)
        {
            moveMouse(position);
            System.Threading.Thread.Sleep(20);
            inputSimulator.Mouse.LeftButtonDown();
            System.Threading.Thread.Sleep(20);
            moveMouse(destination);
            System.Threading.Thread.Sleep(20);
        }

        public void dragMouseLinear(Vector2 position, Vector2 destination, float milliseconds)
        {
            var distance = Vector2.Distance(position, destination);
            int steps = (int) Math.Round(milliseconds / 20);

            Vector2[] points = Vector2.pointsInBetween(position, destination, steps);

            moveMouse(position);
            System.Threading.Thread.Sleep(20);
            inputSimulator.Mouse.LeftButtonDown();
            System.Threading.Thread.Sleep(20);

            foreach (var point in points)
            {
                moveMouse(point);
                System.Threading.Thread.Sleep(20);
            }

            moveMouse(destination);
            System.Threading.Thread.Sleep(20);
            inputSimulator.Mouse.LeftButtonUp();

        }

        private void moveMouse(Vector2 destination)
        {
            var x = destination.x * 65535 / Screen.PrimaryScreen.Bounds.Width;
            var y = destination.y * 65535 / Screen.PrimaryScreen.Bounds.Height;
            inputSimulator.Mouse.MoveMouseTo(Convert.ToDouble(x), Convert.ToDouble(y));
        }
    }
}
