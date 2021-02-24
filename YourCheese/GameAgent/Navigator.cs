using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace YourCheese
{
    public class Waypoint
    {
        public float x, y;

        public Waypoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Waypoint(Vector2 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
        }

        public Waypoint(Vertex vector)
        {
            this.x = vector.x;
            this.y = vector.y;
        }

        public bool isReached(Vector2 currentPos, bool last)
        {
            int distance = 6;
            if (last) {
                return Math.Abs(currentPos.x - x) < distance && Math.Abs(currentPos.y - y) < distance;
            }
            distance = 15;
            return Vector2.Distance(currentPos, new Vector2(x, y)) < distance;
        }
    }
    public class Navigator
    {
        private SkeldMap map;
        public Vector2 botPos;
        public NavigationInput navigationInput = new NavigationInput();
        bool abortBool = false;

        public Navigator(SkeldMap map)
        {
            this.map = map;
            navigationInput.polygons = map.polygons;
        }

        public void setDestination(Vector2 target)
        {
            List<Waypoint> route = getWaypoints(target);
            if (route == null) 
                return;
            try
            {
                walkTheRoute(route);
            } catch (NavigationError e)
            {
                setDestination(target);
            }
        }

        public void followPlayer(Vector2 target)
        {
            List<Waypoint> route = getWaypoints(target);
            if (route == null) return;
            try
            {
                if (route.Count > 5)
                route = route.GetRange(0, 5);
                walkTheRoute(route);
            }
            catch (NavigationError e)
            {
                return;
            }
        }

        public void walkTheRoute(List<Waypoint> route)
        {
            int i = 0;
            while (!abortBool && i < route.Count)
            {
                var waypoint = route[i];
                Waypoint nextWaypoint = null;
                if (i + 1 < route.Count)
                {
                    nextWaypoint = route[i + 1];
                }
                navigationInput.getToPos(waypoint, nextWaypoint);
                navigationInput.releaseInput();
                i++;
            }
        }
        
        public List<Waypoint> getWaypoints(Vector2 target)
        {
            PolyPathfinder pathFinder = new PolyPathfinder(map.polygons, map.waypoints);
            Vertex initialPoint = new Vertex((int)Math.Round(botPos.x), (int)Math.Round(botPos.y));
            Vertex targetPoint = new Vertex((int)Math.Round(target.x), (int)Math.Round(target.y));
            List<Waypoint> waypoints = pathFinder.findPath(initialPoint, targetPoint);
            return waypoints;
        }

        public void updateBotPos(Vector2 pos)
        {
            Vector2 previousPos = botPos;
            botPos = pos;
            this.navigationInput.updatePosition(botPos);
        }

        public void stop()
        {
            navigationInput.releaseInput();
        }

        public void abort()
        {
            navigationInput.abort = true;
            abortBool = true;
        }
    }
}
