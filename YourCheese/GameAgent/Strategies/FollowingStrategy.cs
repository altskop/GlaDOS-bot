using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.Strategies
{
    class FollowingStrategy : Strategy
    {
        SkeldMap map;
        Navigator navigator;
        PlayerInformation targetPlayer;
        double confidence = 2;
        bool found = false;
        List<Vertex> points = new List<Vertex>();

        public FollowingStrategy(Navigator navigator, SkeldMap map, PlayerInformation player)
        {
            this.navigator = navigator;
            this.map = map;
            this.targetPlayer = player;
        }

        public void run()
        {
            while (new Random().NextDouble() < confidence)
            {
                var targetPos = map.gamePosToMeshPos(targetPlayer.position);
                var distance = Vector2.Distance(navigator.botPos, targetPos);
                
                if (distance > 15)
                {
                    // follow them
                    navigator.followPlayer(targetPos);
                    confidence -= 0.005;
                    found = true;
                }
                else
                {
                    // stand still
                    System.Threading.Thread.Sleep(300);
                    confidence -= 0.005;
                    found = true;
                }
                /*
                else
                {
                    // search for them
                    if (points.Count == 0)
                    {
                        points.AddRange(map.places);
                    }
                    var nextPoint = getClosestPoint(points);
                    navigator.followPlayer(new Vector2(nextPoint.x, nextPoint.y));
                    distance = Vector2.Distance(navigator.botPos, targetPos);
                    if (Vector2.Distance(navigator.botPos, new Vector2(nextPoint.x, nextPoint.y)) < 15)
                    {
                        points.Remove(nextPoint);
                    }
                    if (distance < 20)
                    {
                        found = true;
                    }
                    if (found)
                    {
                        confidence -= 0.005;
                    }
                    confidence -= 0.0003;
                }
                */
            }
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

        public double getConfidence()
        {
            return confidence;
        }

        public void setConfidence(double t)
        {
            this.confidence = t;
        }

        public String getAsString()
        {
            return $"Following {targetPlayer.name}";
        }

        public String getMode()
        {
            return $"Following {targetPlayer.name}";
        }

        public void abort()
        {
            confidence = 0;
            navigator.abort();
        }

        public void update(GameDataContainer gameDataContainer)
        {
            targetPlayer = gameDataContainer.getPlayerByColor(targetPlayer.colorId);
        }

        public void setNavigator(Navigator navigator)
        {
            this.navigator = navigator;
        }
    }
}
