using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace YourCheese
{
    public class NavigationError: Exception
    {

    }
    public class NavigationInput
    {
        public List<Polygon> polygons;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 previousVelocity;
        public int iterationsLost = 0;
        public static InputSimulator inputSimulator = new InputSimulator();

        public bool abort = false;

        public void getToPos(Waypoint waypoint, Waypoint nextWaypoint)
        {
            while (!waypoint.isReached(position, (nextWaypoint==null)) && !abort)
            {
                if (nextWaypoint != null)
                {
                    if (PolyPathfinder.InLineOfSight(polygons, new Vertex(position), new Vertex(nextWaypoint))){
                        break;
                    }
                }
                Console.WriteLine($"Current pos: {position.x}, {position.y}");
                Console.WriteLine($"Current destination: {waypoint.x}, {waypoint.y}");
                releaseInput();

                float distance = 8;
                if (nextWaypoint == null)
                {
                    distance = 2;
                }

                if (PolyPathfinder.InLineOfSight(polygons, new Vertex(position), new Vertex(waypoint)))
                {
                    if (velocity.x > 3 || velocity.y > 3)
                    {
                        iterationsLost = 0;
                    }
                    if (!(Math.Abs(position.y - waypoint.y) > (Math.Abs(position.x - waypoint.x) + 70)))
                        getToX(waypoint.x, distance);
                    if (!(Math.Abs(position.x - waypoint.x) > (Math.Abs(position.y - waypoint.y) + 70)))
                        getToY(waypoint.y, distance);
                } 
                else
                {
                    iterationsLost += 1;
                    getToX(waypoint.x, 2);
                    getToY(waypoint.y, 2);
                }

                if (velocity.x <= 3 && velocity.y <= 3)
                {
                    iterationsLost += 1;
                }

                if (iterationsLost >= 500)
                {
                    //releaseInput();
                    throw new NavigationError();
                }

                interpolatePosition();
                System.Threading.Thread.Sleep(10);
            }
        }

        public void releaseInput()
        {
            releaseKey(VirtualKeyCode.VK_A);
            releaseKey(VirtualKeyCode.VK_D);
            releaseKey(VirtualKeyCode.VK_W);
            releaseKey(VirtualKeyCode.VK_S);
        }

        public void getToX(float x, float margin)
        {
            if (Math.Abs(position.x - x) > margin)
            {
                if (position.x > x)
                {
                    // go left
                    holdKey(VirtualKeyCode.VK_A);
                }
                else
                {
                    // go right
                    holdKey(VirtualKeyCode.VK_D);
                }
            }
            else
            {
                releaseKey(VirtualKeyCode.VK_A);
                releaseKey(VirtualKeyCode.VK_D);
            }
        }

        public void getToY(float y, float margin)
        {
            if (Math.Abs(position.y - y) > margin)
            {
                if (position.y > y)
                {
                    // go down
                    holdKey(VirtualKeyCode.VK_W);
                }
                else
                {
                    // go up
                    holdKey(VirtualKeyCode.VK_S);
                }
            }
            else
            {
                releaseKey(VirtualKeyCode.VK_W);
                releaseKey(VirtualKeyCode.VK_S);
            }
        }

        private async void holdKey(VirtualKeyCode key)
        {
            inputSimulator.Keyboard.KeyDown(key);
        }

        private void releaseKey(VirtualKeyCode key)
        {
            inputSimulator.Keyboard.KeyUp(key);
        }

        private void interpolatePosition()
        {
            this.position.x += this.velocity.x*10;
            this.position.y += this.velocity.y*10;
        }

        public void updatePosition(Vector2 position)
        {
            this.previousVelocity = velocity;
            this.velocity = new Vector2((position.x - this.position.x)/Program.MEMORY_POLLING_PERIOD, (position.y - this.position.y) / Program.MEMORY_POLLING_PERIOD);
            this.position = position;
        }
    }
}
