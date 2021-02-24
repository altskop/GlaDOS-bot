using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourCheese.GameAgent;
using YourCheese.GameAgent.Conversation;
using YourCheese.GameAgent.Strategies;

namespace YourCheese
{
    public class BehaviorDriver
    {
        SkeldMap map;
        public PlayerInformation botInfo;
        Navigator navigator;
        public Strategy currentStrategy;
        Strategy queuedStrategy;
        RoundMemory roundMemory;
        GameDataContainer gameDataContainer;
        List<PlayerInformation> visiblePlayers;
        public bool inEmergencyMeeting = false;
        bool talked = false;

        public PlayerInformation reportedBody = PlayerInformation.Zero;
        PlayerInformation imposterPartner;
        int remainingTasks = 10;

        public BehaviorDriver(SkeldMap map)
        {
            this.map = map;
            this.navigator = new Navigator(map);
            this.roundMemory = new RoundMemory();
        }

        public void run()
        {
            while (gameDataContainer == null)
            {
                System.Threading.Thread.Sleep(1500);
            }
            while (gameDataContainer.players.Count == 0)
            {
                System.Threading.Thread.Sleep(1500);
            }
            System.Threading.Thread.Sleep(8000);
            inEmergencyMeeting = false;
            talked = false;
            roundMemory.generatePlayerSightings(gameDataContainer.players);
            while (true)
            {
                while (!inEmergencyMeeting && !botInfo.isDead)
                {
                    talked = false;
                    currentStrategy = selectStrategy();
                    currentStrategy.run();
                    roundMemory.addNewStrategy(currentStrategy);
                    if (currentStrategy is TaskDoingStrategy)
                    {
                        TaskDoingStrategy taskDoingStrategy = (TaskDoingStrategy)currentStrategy;
                        roundMemory.addTasks(taskDoingStrategy.doneTasks);
                        remainingTasks = taskDoingStrategy.taskPositions.Count;
                    }
                    else if (currentStrategy is TaskFakingStrategy)
                    {
                        TaskFakingStrategy taskDoingStrategy = (TaskFakingStrategy)currentStrategy;
                        roundMemory.addTasks(taskDoingStrategy.doneTasks);
                        remainingTasks = taskDoingStrategy.taskPositions.Count;
                    }
                }
                while (!inEmergencyMeeting && botInfo.isDead && !botInfo.isImposter && remainingTasks > 0)
                {
                    var taskDoingStrategy = new TaskDoingStrategy(navigator, map);
                    taskDoingStrategy.run();
                    remainingTasks = taskDoingStrategy.taskPositions.Count;
                }
                while (inEmergencyMeeting && !botInfo.isDead)
                {
                    if (!talked)
                    {
                        new MeetingTalker(botInfo, gameDataContainer).tellTheMemory(roundMemory, reportedBody, map);
                        talked = true;
                        roundMemory.refresh();
                        this.navigator = new Navigator(map);
                        new VotingDriver().vote(this.gameDataContainer.getLivingPlayers().Count);
                    }
                    System.Threading.Thread.Sleep(500);
                }
                while (inEmergencyMeeting && botInfo.isDead)
                {
                    System.Threading.Thread.Sleep(500);
                    roundMemory.refresh();
                }
                while (botInfo.isDead && botInfo.isImposter)
                {
                    System.Threading.Thread.Sleep(500);
                }
                while (botInfo.isDead && remainingTasks == 0)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        public Strategy selectStrategy()
        {
            refreshNav();
            if (queuedStrategy != null)
            {
                var strat = queuedStrategy;
                queuedStrategy.setNavigator(navigator);
                queuedStrategy = null;
                return strat;
            }

            if (currentStrategy is PanicStrategy)
            {
                return currentStrategy;
            }

            if (!botInfo.isImposter)
            {
                int choice = new Random().Next(4);
                switch (choice)
                {
                    case 0: //return new BodySearchingStrategy(navigator, map);
                    case 1: return new FollowingStrategy(navigator, map, gameDataContainer.getLivingPlayersThatArentBot()[new Random().Next(gameDataContainer.getLivingPlayersThatArentBot().Count)]);
                    case 2: 
                    case 3:
                    default: return new TaskDoingStrategy(navigator, map);
                }
                     
            }
            else
            {
                int choice = new Random().Next(4);
                switch (choice)
                {
                    case 0:
                    case 1: return new FollowingStrategy(navigator, map, gameDataContainer.getLivingPlayersThatArentBot()[new Random().Next(gameDataContainer.getLivingPlayersThatArentBot().Count)]);
                    case 2:
                    case 3:
                    default: return new TaskFakingStrategy(navigator, map, roundMemory.completedTasks);
                }
            }
            throw new Exception();
        }

        public void updateBotInfo(PlayerInformation botInfo)
        {
            if (!botInfo.position.IsGarbage())
            {
                this.botInfo = botInfo;
                this.navigator.updateBotPos(this.map.gamePosToMeshPos(botInfo.position));
            }
        }

        private void processEvents(List<Event> events, List<PlayerInformation> nearbyPlayers)
        {
            if (!botInfo.isImposter)
            {
                foreach (var player in visiblePlayers)
                {
                    if (player.isDead)
                    {
                        reportedBody = player;
                        new TaskInput().pressR();
                    }
                }
                foreach (var myEvent in events)
                {
                    if (myEvent is VentEvent)
                    {
                        overrideStrategy(new PanicStrategy(navigator, myEvent));
                    }
                }
            }
            else
            {
                
                if (visiblePlayers.Count == 3 && visiblePlayers.Contains(gameDataContainer.getTheOtherImposter()) && gameDataContainer.getTheOtherImposter().killTimer < 3 && botInfo.killTimer < 2)
                {
                    overrideStrategy(new DoubleKillSetup(navigator, map, gameDataContainer));
                }
                if (botInfo.killTimer < 1 && !(currentStrategy is HuntingStrategy))
                {
                    overrideStrategy(new HuntingStrategy(navigator, map, gameDataContainer, this));
                }
                foreach (var myEvent in events)
                {
                    if (myEvent is DeathEvent)
                    {
                        if (currentStrategy is DoubleKillSetup)
                        {
                            new TaskInput().pressQ();
                        }
                    }
                }
            }
        }

        private void overrideStrategy(Strategy newStrategy)
        {
            if (currentStrategy != null)
            currentStrategy.abort();
            queuedStrategy = newStrategy;
        }

        private bool isInEmergencyMeeting(List<Event> events)
        {
            //if (gameDataContainer.emergencyCooldown > -15)
            foreach (var myEvent in events)
            {
                if (myEvent is EmergencyMeeting)
                {
                    return true;
                }
            }
            return false;
        }

        private void refreshNav()
        {
            this.navigator.stop();
            this.navigator = new Navigator(map);
        }

        public void update(GameUpdate gameUpdate)
        {
            
            updateBotInfo(gameUpdate.gameDataContainer.botPlayer);
            visiblePlayers = gameUpdate.visiblePlayers;
            roundMemory.update(gameUpdate.events, gameUpdate.visiblePlayers);
            if (gameDataContainer != null)
            {
                if (!inEmergencyMeeting)
                {
                    inEmergencyMeeting = isInEmergencyMeeting(gameUpdate.events);
                }
                else
                {
                    var closestPlayer = gameUpdate.gameDataContainer.getLivingPlayersThatArentBot()[0];
                    if (closestPlayer.position != gameDataContainer.getPlayerByColor(closestPlayer.colorId).position)
                        inEmergencyMeeting = false;
                }
            }
            //inEmergencyMeeting = (gameDataContainer.emergencyCooldown < gameUpdate.gameDataContainer.emergencyCooldown);
            gameDataContainer = gameUpdate.gameDataContainer;
            if (currentStrategy != null)
            currentStrategy.update(gameDataContainer);
            processEvents(gameUpdate.events, gameUpdate.gameDataContainer.getNearbyPlayers(botInfo.colorId));
            if (inEmergencyMeeting)
            {
                overrideStrategy(null);
            }
        }


        
    }
}
