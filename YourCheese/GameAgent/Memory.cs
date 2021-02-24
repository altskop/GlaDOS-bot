using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourCheese.GameAgent.Strategies;

namespace YourCheese.GameAgent
{
    public class RoundMemory
    {
        public List<Strategy> strategies = new List<Strategy>();
        public List<GameTask> completedTasks = new List<GameTask>();
        public List<Event> witnessedEvents = new List<Event>();
        public Dictionary<PlayerInformation, int> playerSightMap = new Dictionary<PlayerInformation, int>();
        int totalUpdates = 0;

        public void update(List<Event> events, List<PlayerInformation> visiblePlayers)
        {
            foreach (var myEvent in events)
            {
                if (myEvent is VentEvent || myEvent is DeathEvent)
                {
                    witnessedEvents.Add(myEvent);
                }
            }
            if (playerSightMap.Count > 0)
            foreach (var player in visiblePlayers)
            {
                if (playerSightMap.ContainsKey(player))
                {
                    playerSightMap[player] += 1;
                }
            }
            totalUpdates += 1;
        }

        public void addNewStrategy(Strategy strategy)
        {
            if (strategies.Count < 2)
            strategies.Add(strategy);
        }

        public void generatePlayerSightings(List<PlayerInformation> players)
        {
            playerSightMap = new Dictionary<PlayerInformation, int>();
            foreach (var player in players)
            {
                playerSightMap.Add(player, 0);
            }
        }

        public void addTask(GameTask gameTask)
        {
            if (!completedTasks.Contains(gameTask))
            {
                completedTasks.Add(gameTask);
            }
        }

        public void addTasks(List<GameTask> gameTasks)
        {
            foreach (var task in gameTasks)
            {
                addTask(task);
            }
        }

        public Dictionary<PlayerInformation, double> getPlayerSightings()
        {
            var result = new Dictionary<PlayerInformation, double>();
            foreach (KeyValuePair<PlayerInformation, int> entry in playerSightMap)
            {
                result[entry.Key] = (double) entry.Value / totalUpdates;
            }
            return result;
        }

        public List<PlayerInformation> getTrustedPlayers()
        {
            List<PlayerInformation> trustedPlayers = new List<PlayerInformation>();
            foreach (KeyValuePair<PlayerInformation, double> entry in getPlayerSightings())
            {
                if (entry.Value > 0.75)
                {
                    trustedPlayers.Add(entry.Key);
                }
            }
            return trustedPlayers;
        }

        public void refresh()
        {
            strategies = new List<Strategy>();
            completedTasks = new List<GameTask>();
            witnessedEvents = new List<Event>();
            playerSightMap = new Dictionary<PlayerInformation, int>();
            totalUpdates = 0;
        }

    }

    class GameMemory
    {

    }
}
