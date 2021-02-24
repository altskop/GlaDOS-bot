using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.Strategies
{
    class HuntingStrategy : Strategy
    {
        PlayerInformation target;
        SkeldMap map;
        Navigator navigator;
        GameDataContainer gameState;
        BehaviorDriver behaviorDriver;
        double confidence = 1;
        String mode = "Hunting";
        bool murdered = false;

        public HuntingStrategy(Navigator navigator, SkeldMap map, GameDataContainer gameState, BehaviorDriver behaviorDriver)
        {
            this.navigator = navigator;
            this.map = map;
            this.gameState = gameState;
            this.behaviorDriver = behaviorDriver;
        }

        public double getConfidence() 
        {
            return confidence;
        }

        public void setConfidence(double t) 
        {
            confidence = t;
        }

        public void run() 
        {
            while (confidence > 0 && gameState.botPlayer.killTimer == 0)
            {
                acquireTarget();
                while (targetStillValid())
                {
                    navigator.followPlayer(map.gamePosToMeshPos(target.position));
                    if (Vector2.Distance(navigator.botPos, map.gamePosToMeshPos(target.position)) < 20)
                    {
                        new TaskInput().pressQ();
                        murdered = true;
                    }
                }
            }
            if (murdered)
            {
                Vector2 escapePoint = Vector2.Zero;
                foreach (var point in map.places)
                {
                    Vector2 pointVector = new Vector2(point.x, point.y);
                    List<Waypoint> escapeRoute = navigator.getWaypoints(pointVector);
                    int i = 0;
                    bool safeRoute = true;
                    foreach (var waypoint in escapeRoute)
                    {
                        double closestCrewmateDistance = gameState.getClosestCrewmateToPoint(new Vector2(waypoint.x, waypoint.y));
                        if (closestCrewmateDistance < 150 - (i * 10))
                        {
                            safeRoute = false;
                            break;
                        }
                    }
                    if (safeRoute)
                    {
                        escapePoint = pointVector;
                        break;
                    }
                }
                if (escapePoint.IsGarbage() && Vector2.Distance(map.gamePosToMeshPos(gameState.getTheOtherImposter().position), navigator.botPos) > 50)
                {
                    // self report
                    behaviorDriver.reportedBody = target;
                    new TaskInput().pressR();
                }
                else
                {
                    // run for your life
                    mode = "Escaping murder scene";
                    navigator.setDestination(escapePoint);
                }
            }
        }

        public void update(GameDataContainer gameState)
        {
            this.gameState = gameState;
            this.target = gameState.getPlayerByColor(target.colorId);
        }

        void acquireTarget()
        {
            foreach (var player in gameState.getLivingCrewmatesThatArentBot())
            {
                PlayerInformation closestCrewmate = gameState.getClosestPlayer(player.colorId);
                float distance = Vector2.Distance(map.gamePosToMeshPos(closestCrewmate.position), map.gamePosToMeshPos(player.position));
                if (distance > 80 && !player.isDead)
                {
                    target = player;
                    return;
                }
            }
            setConfidence(0);
        }

        bool targetStillValid()
        {
            PlayerInformation closestCrewmate = gameState.getClosestPlayer(target.colorId);
            float distance = Vector2.Distance(map.gamePosToMeshPos(closestCrewmate.position), map.gamePosToMeshPos(target.position));
            return (distance > 60 && !target.isDead);
        }

        public String getAsString() 
        {
            return "Looking for bodies";
        }


        public void abort()
        {
            confidence = 0;
            navigator.abort();
        }

        public String getMode()
        {
            return mode;
        }

        public void setNavigator(Navigator navigator)
        {
            this.navigator = navigator;
        }
    }
}
