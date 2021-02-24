using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YourCheese
{

    public class Region
    {
        public String name;
        public Vector2[] points;

        public bool isPointIn(Vector2 targetPoint)
        {
            bool result = false;
            int j = points.Count() - 1;
            for (int i = 0; i < points.Count(); i++)
            {
                if (points[i].y < targetPoint.y && points[j].y >= targetPoint.y || points[j].y < targetPoint.y && points[i].y >= targetPoint.y)
                {
                    if (points[i].x + (targetPoint.y - points[i].y) / (points[j].y - points[i].y) * (points[j].x - points[i].x) < targetPoint.x)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public Vector2 getCentroid()
        {
            float accumulatedArea = 0.0f;
            float centerX = 0.0f;
            float centerY = 0.0f;

            for (int i = 0, j = points.Count() - 1; i < points.Count(); j = i++)
            {
                float temp = points[i].x * points[j].y - points[j].x * points[i].y;
                accumulatedArea += temp;
                centerX += (points[i].x + points[j].x) * temp;
                centerY += (points[i].y + points[j].y) * temp;
            }

            if (Math.Abs(accumulatedArea) < 1E-7f)
                return new Vector2(0, 0);  // Avoid division by zero

            accumulatedArea *= 3f;
            return new Vector2(centerX / accumulatedArea, centerY / accumulatedArea);
        }

        public double distanceFromCenter(Vector2 target)
        {
            return AuUtils.vectorDistance(target, getCentroid());
        }
    }

    public class Vertex {

        public List<Vertex> visiblePoints;
        public float x, y;

        public Vertex()
        {

        }

        public Vertex(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.visiblePoints = new List<Vertex>();
        }

        public Vertex(Vector2 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
        }

        public Vertex(Waypoint waypoint)
        {
            this.x = waypoint.x;
            this.y = waypoint.y;
        }

        public Vertex(PathfindingNode waypoint)
        {
            this.x = waypoint.X;
            this.y = waypoint.Y;
        }

        public static float DistanceSquared(Vertex pos1, Vertex pos2)
        {
            return ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y));
        }

        public static float Distance(Vertex pos1, Vertex pos2)
        {
            return (float)Math.Sqrt(DistanceSquared(pos1, pos2));
        }

        public static Vertex Zero
        {
            get
            {
                return new Vertex(0, 0);
            }
        }

        public static Vertex operator +(Vertex f1, Vertex f2)
        {
            Vertex result = new Vertex(f1.x + f2.x, f1.y + f2.y);
            return result;
        }

        public static Vertex operator /(Vertex f1, float num)
        {
            Vertex result = new Vertex(f1.x / num, f1.y / num);
            return result;
        }

    }

    public class Polygon
    {
        public Vertex[] points;
        public bool inside;

        public bool Inside(Vector2 targetPoint) {
            bool result = false;
            int j = points.Count() - 1;
            for (int i = 0; i < points.Count(); i++)
            {
                if (points[i].y < targetPoint.y && points[j].y >= targetPoint.y || points[j].y < targetPoint.y && points[i].y >= targetPoint.y)
                {
                    if (points[i].x + (targetPoint.y - points[i].y) / (points[j].y - points[i].y) * (points[j].x - points[i].x) < targetPoint.x)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public bool Inside(Vertex targetPoint)
        {
            bool result = false;
            int j = points.Count() - 1;
            for (int i = 0; i < points.Count(); i++)
            {
                if (points[i].y < targetPoint.y && points[j].y >= targetPoint.y || points[j].y < targetPoint.y && points[i].y >= targetPoint.y)
                {
                    if (points[i].x + (targetPoint.y - points[i].y) / (points[j].y - points[i].y) * (points[j].x - points[i].x) < targetPoint.x)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }

    public class SkeldMap
    {

        public Vector2 playerPos;

        public byte[,] walkableMesh;
        public List<Region> regions;
        public List<Polygon> polygons;
        public List<Vertex> waypoints;
        public List<Vertex> places;  

        // hardcoded to Skeld for now
        //
        Vector2 topImg = new Vector2(571, 35);
        Vector2 topPoint = new Vector2(-4.186444, 6.023194);
        Vector2 leftImg = new Vector2(28, 292);
        Vector2 leftPoint = new Vector2(-22.76394, -2.692804);
        Vector2 rightImg = new Vector2(1209, 291);
        Vector2 rightPoint = new Vector2(17.4329, -2.755548);
        Vector2 botImg = new Vector2(725, 719);
        Vector2 botPoint = new Vector2(0.7558017, -17.09207);

        public SkeldMap()
        {
            Console.WriteLine("Reading the map...");
            //loadMeshFromImage();
            loadRegions();
            Console.WriteLine("Loading the map polygons and waypoints...");
            loadPolygons();
            loadWaypoints();
            loadPlaces();
            Console.WriteLine("Constructing visibility graph...");
            constructVisibilityGraph();
        }

        private void loadMeshFromImage()
        {
            Bitmap walkableMeshImage = new Bitmap("C:/Studio/Skeld/WalkableMesh.png");

            walkableMesh = new byte[walkableMeshImage.Width, walkableMeshImage.Height];
            
            for (int y = 0; y < walkableMeshImage.Height; y++)
            {
                for (int x = 0; x < walkableMeshImage.Width; x++)
                {
                    System.Drawing.Color pixelColor = walkableMeshImage.GetPixel(x, y);
                    if (pixelColor.GetBrightness() < 0.02)
                    {
                        walkableMesh[x, y] = 0;
                    }
                    else
                    {
                        walkableMesh[x, y] = 1;
                    }
                }
            }
        }

        private void loadPolygons()
        {
            using (StreamReader r = new StreamReader("C:/Studio/Skeld/polygons.json"))
            {
                string json = r.ReadToEnd();
                polygons = JsonConvert.DeserializeObject<List<Polygon>>(json);
            }
        }

        private void loadWaypoints()
        {
            using (StreamReader r = new StreamReader("C:/Studio/Skeld/waypoints.json"))
            {
                string json = r.ReadToEnd();
                waypoints = JsonConvert.DeserializeObject<List<Vertex>>(json);
            }
        }

        private void loadPlaces()
        {
            using (StreamReader r = new StreamReader("C:/Studio/Skeld/places.json"))
            {
                string json = r.ReadToEnd();
                places = JsonConvert.DeserializeObject<List<Vertex>>(json);
            }
        }

        private void constructVisibilityGraph()
        {
            foreach (var polygon in polygons)
            {
                foreach (var point in polygon.points)
                {
                    point.visiblePoints = new List<Vertex>();
                    foreach (var targetPolygon in polygons)
                    {
                        foreach (var targetPoint in targetPolygon.points)
                        {
                            if (targetPoint != point)
                            {
                                if (PolyPathfinder.InLineOfSight(polygons, point, targetPoint))
                                {
                                    point.visiblePoints.Add(targetPoint);
                                }
                            }
                        }
                    }
                    foreach (var waypoint in waypoints)
                    {
                        if (PolyPathfinder.InLineOfSight(polygons, point, waypoint))
                        {
                            point.visiblePoints.Add(waypoint);
                        }
                    }
                }
            }
        }

        private void loadRegions()
        {
            using (StreamReader r = new StreamReader("C:/Studio/Skeld/regions.json"))
            {
                string json = r.ReadToEnd();
                regions = JsonConvert.DeserializeObject<List<Region>>(json);
            }
        }

        public Vector2 gamePosToMeshPos(Vector2 gamePos)
        {
            float xPercent = (gamePos.x - leftPoint.x) / (rightPoint.x - leftPoint.x);
            float yPercent = (gamePos.y - topPoint.y) / (botPoint.y - topPoint.y);
            float xPos = leftImg.x + ((rightImg.x - leftImg.x) * xPercent);
            float yPos = topImg.y + ((botImg.y - topImg.y) * yPercent);
            return new Vector2(xPos, yPos);
        }

        public Vector2 meshPosToGamePos(Vector2 meshPos)
        {
            float xPercent = (meshPos.x - leftImg.x) / (rightImg.x - leftImg.x);
            float yPercent = (meshPos.y - topImg.y) / (botImg.y - topImg.y);
            float xPos = leftPoint.x + ((rightPoint.x - leftPoint.x) * xPercent);
            float yPos = topPoint.y + ((botPoint.y - topPoint.y) * yPercent);
            return new Vector2(xPos, yPos);
        }

        public String getLocationRegionName(Vector2 pos)
        {
            foreach (Region region in regions){
                if (region.isPointIn(pos))
                {
                    return region.name;
                }
            }
            // Not sure? Let's find the closest region. Computationally expensive but hopefully won't have to do often
            double distance = 99999;
            String name = "not sure";
            foreach (Region region in regions)
            {
                double temp = region.distanceFromCenter(pos);
                if (temp < distance)
                {
                    distance = temp;
                    name = region.name;
                }
            }
            return name;
        }
    }
}
