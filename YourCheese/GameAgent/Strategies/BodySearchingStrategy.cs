using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.Strategies
{
    class BodySearchingStrategy : Strategy
    {

        double confidence = 1.2;
        SkeldMap map;
        Navigator navigator;
        List<Vertex> points = new List<Vertex>();

        public BodySearchingStrategy(Navigator navigator, SkeldMap map)
        {
            this.navigator = navigator;
            this.map = map;
        }

        public void run()
        {
            points.AddRange(map.places);
            while (new Random().NextDouble() < confidence)
            {
                if (points.Count == 0)
                {
                    points.AddRange(map.places);
                }
                var nextPoint = getClosestPoint(points);
                navigator.setDestination(new Vector2(nextPoint.x, nextPoint.y));
                points.Remove(nextPoint);
                confidence -= 0.05;
            }
        }

        public double getConfidence()
        {
            return confidence;
        }

        public void setConfidence(double t)
        {
            this.confidence = t;
        }

        private Vertex getClosestPoint(List<Vertex> points)
        {
            float distance = 99999;
            Vertex closestPoint = null;
            foreach (var point in points)
            {
                float temp = Vector2.Distance(this.map.gamePosToMeshPos(navigator.botPos), new Vector2(point.x, point.y));
                if (temp < distance)
                {
                    distance = temp;
                    closestPoint = point;
                }
            }
            return closestPoint;
        }

        public void update(GameDataContainer gameDataContainer)
        {

        }

        public String getAsString()
        {
            return "Searching for bodies";
        }

        public void abort()
        {
            confidence = 0;
            navigator.abort();
        }

        public String getMode()
        {
            return "Searching for bodies";
        }

        public void setNavigator (Navigator navigator)
        {
            this.navigator = navigator;
        }
    }
}
