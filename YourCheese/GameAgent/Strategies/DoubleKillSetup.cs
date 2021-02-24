using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.Strategies
{
    class DoubleKillSetup : Strategy
    {

        SkeldMap map;
        Navigator navigator;
        GameDataContainer gameState;
        PlayerInformation partner;

        public DoubleKillSetup(Navigator navigator, SkeldMap map, GameDataContainer gameState)
        {
            this.navigator = navigator;
            this.map = map;
            this.gameState = gameState;
            partner = gameState.getTheOtherImposter();
        }


        public double getConfidence()
        {
            return 1;
        }

        public void setConfidence(double t)
        {

        }

        public void run()
        {
            var targets = acquireTargets();
            if (targets.Count != 2)
            {
                return;
            }
            while ((Vector2.Distance(map.gamePosToMeshPos(partner.position), map.gamePosToMeshPos(targets[0].position)) < 12 && Vector2.Distance(map.gamePosToMeshPos(partner.position), map.gamePosToMeshPos(targets[1].position)) < 25
                || Vector2.Distance(map.gamePosToMeshPos(partner.position), map.gamePosToMeshPos(targets[1].position)) < 12 && Vector2.Distance(map.gamePosToMeshPos(partner.position), map.gamePosToMeshPos(targets[0].position)) < 25)
                && (!targets[0].isDead && !targets[1].isDead && !partner.isDead))
            {
                while (partner.killTimer < 1)
                {
                    targets = acquireTargets();
                    PlayerInformation partnerTarget = gameState.getClosestCrewmate(partner.colorId);
                    PlayerInformation myTarget;
                    foreach (var player in targets)
                    {
                        if (player.colorId != partnerTarget.colorId)
                        {
                            myTarget = player;
                            if (Vector2.Distance(map.gamePosToMeshPos(myTarget.position), map.gamePosToMeshPos(navigator.botPos)) > 10)
                            navigator.setDestination(myTarget.position);
                        }
                    }
                }
            }

        }

        public List<PlayerInformation> acquireTargets()
        {
            List<PlayerInformation> targets;
            var visiblePlayers = gameState.getVisiblePlayers();
            visiblePlayers.Remove(partner);
            targets = visiblePlayers;
            return targets;
        }

        public void update(GameDataContainer gameDataContainer)
        {
            gameState = gameDataContainer;
            partner = gameState.getTheOtherImposter();
        }

        public String getAsString()
        {
            return "Grouping with people";
        }

        public void abort()
        {
            navigator.abort();
        }

        public String getMode()
        {
            return "Setting up for a doublekill";
        }

        public void setNavigator(Navigator navigator)
        {
            this.navigator = navigator;
        }
    }
}
