using System;
using System.Collections.Generic;

namespace YourCheese
{
    public class EventGenerator
    {
        private GameDataContainer previousData;
        private GameDataContainer currentData;

        private List<DeathEvent> processDeaths()
        {
            List<DeathEvent> deathEvents = new List<DeathEvent>();
            int previouslyDeadNumber = previousData.getDeadPlayers().Count;
            int currentlyDeadNumber = currentData.getDeadPlayers().Count;
            if (previouslyDeadNumber < currentlyDeadNumber)
            {
                List<byte> previouslyDeadColors = new List<byte>();
                foreach (PlayerInformation player in previousData.getDeadPlayers())
                {
                    previouslyDeadColors.Add(player.colorId);
                }

                List<byte> deadColors = new List<byte>();
                foreach (PlayerInformation player in currentData.getDeadPlayers())
                {
                    deadColors.Add(player.colorId);
                }

                foreach (byte colorId in deadColors)
                {
                    if (!previouslyDeadColors.Contains(colorId))
                    {
                        PlayerInformation victim = currentData.getPlayerByColor(colorId);
                        if (!previousData.getPlayerByColor(victim.colorId).position.IsGarbage())
                        {
                            DeathEvent deathEvent = new DeathEvent(victim, findKiller(victim), currentData.getNearbyPlayers(colorId));
                            if (currentData.botPlayer.position.isVisible(deathEvent.position, currentData.lightRadius))
                                deathEvents.Add(deathEvent);
                        }
                    }
                }
            }
            
            return deathEvents;
        }

        private PlayerInformation findKiller(PlayerInformation victim)
        {
            List<PlayerInformation> previousKillers = previousData.getImposters();
            List<PlayerInformation> currentKillers = currentData.getImposters();
            if (previousKillers.Count != currentKillers.Count) return PlayerInformation.Zero;
            List<int> updatedCooldowns = new List<int>();
            for (int i=0; i<currentKillers.Count; i++)
            {
                if (previousKillers[i].killTimer < currentKillers[i].killTimer)
                {
                    updatedCooldowns.Add(i);
                }
            }
            if(updatedCooldowns.Count == 1)
            {
                return currentKillers[updatedCooldowns[0]];
            }
            // else: both updated
            return currentData.getClosestImposter(victim.colorId);
        }
        
        private List<EmergencyMeeting> processEmergencyMeeting()
        {
            List<EmergencyMeeting> emergencyMeetings = new List<EmergencyMeeting>();
            List<TeleportEvent> teleportEvents = new List<TeleportEvent>();
            List<PlayerInformation> livingPlayers = currentData.getLivingPlayers();
            foreach (PlayerInformation player in livingPlayers)
            {
                Vector2 currentPos = player.position;
                Vector2 oldPos = previousData.getPlayerByColor(player.colorId).position;
                if (Vector2.Distance(currentPos, oldPos) > 3 && !oldPos.IsGarbage() && !currentPos.IsGarbage())
                {
                    teleportEvents.Add(new TeleportEvent(oldPos, currentPos));
                }
            }
            bool playerFar = false;
            
            if (teleportEvents.Count >= Math.Min(livingPlayers.Count-1, 2) && !playerFar)
            {
                foreach (var player in currentData.getLivingPlayers())
                {
                    if (Vector2.Distance(player.position, new Vector2(0, 0)) > 6)
                    {
                        playerFar = true;
                    }
                }
                if (!playerFar)
                {
                    emergencyMeetings.Add(new EmergencyMeeting());
                }
            }
            return emergencyMeetings;
        }

        public List<VentEvent> processVentEvents()
        {
            List<VentEvent> ventEvents = new List<VentEvent>();
            foreach (PlayerInformation player in previousData.getImposters())
            {
                if (player.inVent != currentData.getPlayerByColor(player.colorId).inVent)
                {
                    var ventEvent = new VentEvent(player, player.position);
                    if (currentData.botPlayer.position.isVisible(player.position, currentData.lightRadius))
                        ventEvents.Add(ventEvent);
                }
            }
            return ventEvents;
        }

        public List<Event> processEvents()
        {
            List<Event> events = new List<Event>();
            if (currentData.players.Count > 0 && previousData != null)
            {
                events.AddRange(processDeaths());
                events.AddRange(processEmergencyMeeting());
                events.AddRange(processVentEvents());
            }
            return events;
        }

        public GameUpdate getGameUpdate(GameDataContainer newData)
        {
            if (currentData == null)
            {
                currentData = newData;
                var gameUpdate = new GameUpdate();
                gameUpdate.visiblePlayers = newData.getVisiblePlayers();
                gameUpdate.events = new List<Event>();
                gameUpdate.gameDataContainer = newData;
                return gameUpdate;
            }
            else
            {
                previousData = currentData;
                currentData = newData;
                var gameUpdate = new GameUpdate();
                gameUpdate.events = processEvents();
                gameUpdate.visiblePlayers = newData.getVisiblePlayers();
                gameUpdate.gameDataContainer = newData;
                return gameUpdate;
            }
        }
    }

    public class GameUpdate
    {
        public List<Event> events;
        public List<PlayerInformation> visiblePlayers;
        public GameDataContainer gameDataContainer;
    }

    public class Event
    {
        public String getAsString() { return ""; }
    }

    public class DeathEvent : Event
    {
        public Vector2 position;
        public PlayerInformation victim;
        public PlayerInformation killer;
        public List<PlayerInformation> witnesses;

        public DeathEvent(PlayerInformation victim, PlayerInformation killer, List<PlayerInformation> witnesses)
        {
            this.position = victim.position;
            this.victim = victim;
            this.killer = killer;
            this.witnesses = witnesses;
        }

        public String getAsString() { return "I watched "+killer.color+" murder "+victim.color; }
    }

    public class VentEvent : Event
    {
        public Vector2 position;
        public PlayerInformation venter;

        public VentEvent(PlayerInformation venter, Vector2 position)
        {
            this.position = position;
            this.venter = venter;
        }

        public String getAsString() { return venter.color + " vented right in front of me!"; }
    }

    public class TeleportEvent 
    {
        private Vector2 previousPosition;
        private Vector2 newPosition;

        public TeleportEvent(Vector2 previous, Vector2 newPos)
        {
            this.previousPosition = previous;
            this.newPosition = newPos;
        }
    }

    public class EmergencyMeeting : Event
    {

    }
}
