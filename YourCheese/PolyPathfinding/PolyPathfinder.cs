using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese
{
    public class PathfindingNode
    {
        public int X;
        public int Y;
        public int F = 0;
        public int G;
        public int H;
        public PathfindingNode parent = null;
        public List<Vertex> visiblePoints;

        public PathfindingNode(Vertex node)
        {
            this.X = (int)Math.Round(node.x);
            this.Y = (int)Math.Round(node.y);
            this.visiblePoints = node.visiblePoints;
        }

        public PathfindingNode(PathfindingNode parent, Vertex node, PathfindingNode start, PathfindingNode end)
        {
            this.X = (int) Math.Round(node.x);
            this.Y = (int) Math.Round(node.y);
            this.H = Math.Abs(X - end.X) + Math.Abs(Y - end.Y);
            this.G = Math.Abs(X - start.X) + Math.Abs(Y - start.Y);
            this.F = 0;
            this.visiblePoints = node.visiblePoints;
            this.parent = parent;
        }

        public void updateF(PathfindingNode start, PathfindingNode end)
        {
            this.H = Math.Abs(X - end.X) + Math.Abs(Y - end.Y);
            this.G = Math.Abs(X - start.X) + Math.Abs(Y - start.Y);
            this.F = 0;
        }

        public void findVisibleNodes(List<Polygon> polygons)
        {
            Vertex point = new Vertex(X, Y);
            this.visiblePoints = new List<Vertex>();
            foreach (var targetPolygon in polygons)
            {
                foreach (var targetPoint in targetPolygon.points)
                {
                    if (PolyPathfinder.InLineOfSight(polygons, point, targetPoint))
                    {
                        this.visiblePoints.Add(targetPoint);
                    }
                }
            }
        }
    }

    class PolyPathfinder
    {

        private List<Polygon> polygons;
        private List<Vertex> waypoints;

        public PolyPathfinder(List<Polygon> polygons, List<Vertex> points)
        {
            this.polygons = polygons;
            this.waypoints = points;
        }

        public static bool LineSegmentsCross(Vertex a, Vertex b, Vertex c, Vertex d)
        {
            float denominator = ((b.x - a.x) * (d.y - c.y)) - ((b.y - a.y) * (d.x - c.x));

            if (denominator == 0)
            {
                return false;
            }

            float numerator1 = ((a.y - c.y) * (d.x - c.x)) - ((a.x - c.x) * (d.y - c.y));

            float numerator2 = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));

            if (numerator1 == 0 || numerator2 == 0)
            {
                return false;
            }

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r > 0 && r < 1) && (s > 0 && s < 1);
        }

        public static bool IsVertexConcave(IList<Vertex> vertices, int vertex)
        {
            Vertex current = vertices[vertex];
            Vertex next = vertices[(vertex + 1) % vertices.Count];
            Vertex previous = vertices[vertex == 0 ? vertices.Count - 1 : vertex - 1];

            Vertex left = new Vertex(current.x - previous.x, current.y - previous.y);
            Vertex right = new Vertex(next.x - current.x, next.y - current.y);

            float cross = (left.x * right.y) - (left.y * right.x);

            return cross < 0;
        }

        public static Waypoint generateWaypoint(float x, float y, List<Polygon> polygons, int offset)
        {
            Waypoint newWaypoint;
            float offsetCheck = offset / 2;
            if (!Inside(polygons, new Vertex(x + offsetCheck, y+ offsetCheck)))
            {
                newWaypoint = new Waypoint(x - offset, y - offset);
            }
            else if (!Inside(polygons, new Vertex(x - offsetCheck, y + offsetCheck)))
            {
                newWaypoint = new Waypoint(x + offset, y - offset);
            }
            else if (!Inside(polygons, new Vertex(x + offsetCheck, y - offsetCheck)))
            {
                newWaypoint = new Waypoint(x - offset, y + offset);
            }
            else if (!Inside(polygons, new Vertex(x - offsetCheck, y - offsetCheck)))
            {
                newWaypoint = new Waypoint(x + offset, y + offset);
            }
            else newWaypoint = new Waypoint(x, y);
            if (!Inside(polygons, new Vertex(newWaypoint.x, newWaypoint.y)) && offset >= 5)
            {
                return generateWaypoint(x, y, polygons, offset - 5);
            }
            return newWaypoint;
        }

        public static bool Inside(List<Polygon> polygons, Vertex position)
        {
            foreach (var polygon in polygons)
            {
                if (!Inside(polygon, position)) return false;
            } 
            return true;
        }

        public static bool Inside(Polygon polygon, Vertex position, bool toleranceOnOutside = true)
        {
            Vertex point = position;

            const float epsilon = 0.5f;

            bool inside = false;

            // Must have 3 or more edges
            if (polygon.points.Length < 3) return false;

            Vertex oldPoint = polygon.points[polygon.points.Length - 1];
            float oldSqDist = Vertex.DistanceSquared(oldPoint, point);

            for (int i = 0; i < polygon.points.Length; i++)
            {
                Vertex newPoint = polygon.points[i];
                float newSqDist = Vertex.DistanceSquared(newPoint, point);

                if (oldSqDist + newSqDist + 2.0f * System.Math.Sqrt(oldSqDist * newSqDist) - Vertex.DistanceSquared(newPoint, oldPoint) < epsilon)
                   return toleranceOnOutside;

                Vertex left;
                Vertex right;
                if (newPoint.x > oldPoint.x)
                {
                    left = oldPoint;
                    right = newPoint;
                }
                else
                {
                    left = newPoint;
                    right = oldPoint;
                }

                if (left.x < point.x && point.x <= right.x && (point.y - left.y) * (right.x - left.x) < (right.y - left.y) * (point.x - left.x))
                    inside = !inside;

                oldPoint = newPoint;
                oldSqDist = newSqDist;
            }
            if (polygon.inside)
            {
                return inside;
            }
            return !inside;

        }

        public static bool InLineOfSight(List<Polygon> polygons, Vertex start, Vertex end)
        {
            // In LOS if it's the same start and end location
            if (Vertex.Distance(start, end) < 2) return true;

            foreach (var polygon in polygons)
            {
                
                // Not in LOS if any of the ends is outside the polygon
                if (!Inside(polygon, start) || !Inside(polygon, end)) { return false; }

                // Not in LOS if any edge is intersected by the start-end line segment          
                var n = polygon.points.Length;
                for (int i = 0; i < n; i++)
                {
                    if (polygon.points[i] == start || polygon.points[i] == end || polygon.points[(i + 1) % n] == start || polygon.points[(i + 1) % n] == end)
                    {
                        continue;
                    }
                    if (LineSegmentsCross(start, end, polygon.points[i], polygon.points[(i + 1) % n]))
                    {
                        return false;
                    }
                }
                // Finally the middle point in the segment determines if in LOS or not
                //foreach (var point in Vector2.pointsInBetween(new Vector2(start.x, start.y), new Vector2(end.x, end.y), 7))
                var point = (start + end) / 2f;
                if (!Inside(polygon, point)) return false;
            }
            return true;
        }

        public Vertex findClosestVertex(Vertex vertex, List<Polygon> polygons)
        {
            Vertex closest = null;
            float distance = 99999;
            foreach (Polygon polygon in polygons)
            {
                foreach (Vertex point in polygon.points)
                {
                    float newDistance = Vertex.DistanceSquared(point, vertex);
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        closest = point;
                    }
                }
            }
            return new Vertex(generateWaypoint(closest.x, closest.y, polygons, 20));
        }

        public Vertex findClosestVertex(Vertex vertex, List<Vertex> points)
        {
            Vertex closest = null;
            float distance = 99999;
            foreach (Vertex point in points)
            {
                float newDistance = Vertex.DistanceSquared(point, vertex);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closest = point;
                }
            }
            if (distance < 40) return new Vertex(generateWaypoint(closest.x, closest.y, polygons, 20));
            return new Vertex(generateWaypoint(vertex.x, vertex.y, polygons, 20));
        }

        public List<Waypoint> findPath(Vertex start, Vertex end)
        {
            List<Waypoint> waypoints = new List<Waypoint>();
            // First verify if both the start and end points of the path are inside the polygon
            foreach (Polygon polygon in polygons)
            {
                if (!Inside(polygon, start)) { start = findClosestVertex(start, polygons); }
                if (!Inside(polygon, end)) { end = findClosestVertex(start, polygons); }
                //if (!Inside(polygon, start)) { return null; }
                //if (!Inside(polygon, end)) { return null; }
            }
            
            // Then start by checking if both points are in line-of-sight.
            if (InLineOfSight(polygons, start, end))
            {
                waypoints.Add(new Waypoint(start));
                waypoints.Add(new Waypoint(end));
                return waypoints;
            }

            var openList = new List<PathfindingNode>();
            var closedList = new List<PathfindingNode>();
            int g = 0;

            // start by adding the original position to the open list
            PathfindingNode startNode = new PathfindingNode(start);
            startNode.findVisibleNodes(polygons);
            PathfindingNode endNode = new PathfindingNode(end);
            endNode.findVisibleNodes(polygons);
            startNode.updateF(startNode, endNode);
            openList.Add(startNode);

            
            while (openList.Count > 0)
            {
                Console.WriteLine($"openList.Count: {openList.Count}");
                var checkTile = openList.OrderBy(x => x.F).First();
                // algorithm's logic goes here
                if (endNode.visiblePoints.Any(x => x.x == checkTile.X && x.y == checkTile.Y))
                {
                    waypoints.Add(new Waypoint(end));
                    while(checkTile.parent != null)
                    {
                        //waypoints.Add(new Waypoint(checkTile.X, checkTile.Y));
                        var resultingVertex = findClosestVertex(new Vertex(checkTile), this.waypoints);
                        var waypoint = generateWaypoint(resultingVertex.x, resultingVertex.y, polygons, 20);
                        waypoints.Add(waypoint);
                        
                        Vertex middlePoint = findClosestVertex(((new Vertex(checkTile) + new Vertex(checkTile.parent)) / 2f), this.waypoints);
                        if (Inside(polygons, middlePoint))
                        {
                            waypoint = generateWaypoint(middlePoint.x, middlePoint.y, polygons, 15);
                            waypoints.Add(waypoint);
                        }
                        
                        checkTile = checkTile.parent;
                    }
                    waypoints.Reverse();
                    return waypoints;
                    //We can actually loop through the parents of each tile to find our exact path which we will show shortly. 
                    //return;
                }

                closedList.Add(checkTile);
                openList.Remove(checkTile);

                var walkableTiles = checkTile.visiblePoints;

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (closedList.Any(x => x.X == walkableTile.x && x.Y == walkableTile.y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (openList.Any(x => x.X == walkableTile.x && x.Y == walkableTile.y))
                    {
                        var existingTile = openList.First(x => x.X == walkableTile.x && x.Y == walkableTile.y);
                        if (existingTile.F > checkTile.F)
                        {
                            openList.Remove(existingTile);
                            openList.Add(new PathfindingNode(checkTile, walkableTile, startNode, endNode));
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        openList.Add(new PathfindingNode(checkTile, walkableTile, startNode, endNode));
                    }
                }
            }
            Console.WriteLine("Couldn't build a path");
            return null;

        }
    }
}
