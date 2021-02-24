using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese
{
    public struct PlayerInformation
    {
        public Vector2 position;
        public String name;
        public String color;
        public byte colorId;
        public bool isDead;
        public bool inVent;
        public bool isImposter; // only used for bot player and their partner during imp rounds
        public float killTimer;
        public uint remainingEmergencies;

        public PlayerInformation(Vector2 position, String name, byte color, byte isDead, uint remainingEmergencies, bool inVent)
        {
            this.position = position;
            this.name = name;
            this.color = ColorDecoder.decodeColor(color);
            this.colorId = color;
            this.isDead = System.Convert.ToBoolean(isDead);
            this.remainingEmergencies = remainingEmergencies;
            this.inVent = inVent;
            this.isImposter = false;
            this.killTimer = 0;
        }

        public PlayerInformation(Vector2 position, String name, byte color, byte isDead, uint remainingEmergencies, bool inVent, byte isImposter, float killTimer)
        {
            this.position = position;
            this.name = name;
            this.color = ColorDecoder.decodeColor(color);
            this.colorId = color;
            this.isDead = System.Convert.ToBoolean(isDead);
            this.remainingEmergencies = remainingEmergencies;
            this.inVent = inVent;
            this.isImposter = System.Convert.ToBoolean(isImposter);
            this.killTimer = killTimer;
        }

        public static PlayerInformation Zero
        {
            get
            {
                return new PlayerInformation(Vector2.Zero, "Test", 4, 1, 0, false);
            }
        }
    }

    class ColorDecoder
    {
        public static String decodeColor(byte colorByte)
        {
            switch (colorByte)
            {
                case 0:
                    return "Red";
                case 1:
                    return "Blue";
                case 2:
                    return "Green";
                case 3:
                    return "Pink";
                case 4:
                    return "Orange";
                case 5:
                    return "Yellow";
                case 6:
                    return "Black";
                case 7:
                    return "White";
                case 8:
                    return "Purple";
                case 9:
                    return "Brown";
                case 10:
                    return "Cyan";
                case 11:
                    return "Lime";
                default:
                    return colorByte.ToString();
            }       

        }
    }

    public class GameDataContainer
    {
        public PlayerInformation botPlayer;
        public List<PlayerInformation> players;
        public float lightRadius;
        public float emergencyCooldown;

        public List<PlayerInformation> getImposters()
        {
            List<PlayerInformation> imposters = new List<PlayerInformation>();
            foreach (PlayerInformation player in players)
            {
                if (player.isImposter && !player.isDead)
                {
                    imposters.Add(player);
                }
            }
            return imposters;
        }

        public PlayerInformation getTheOtherImposter()
        {
            List<PlayerInformation> imposters = getImposters();
            foreach (PlayerInformation player in imposters)
            {
                if (player.colorId != botPlayer.colorId)
                {
                    return player;
                }
            }
            return new PlayerInformation();
            throw new Exception();
        }

        public List<PlayerInformation> getLivingPlayers()
        {
            List<PlayerInformation> alivePlayers = new List<PlayerInformation>();
            foreach (PlayerInformation player in players)
            {
                if (!player.isDead)
                {
                    alivePlayers.Add(player);
                }
            }
            if (!botPlayer.isDead)
            {
                alivePlayers.Add(botPlayer);
            }
            return alivePlayers;
        }

        public List<PlayerInformation> getLivingPlayersThatArentBot()
        {
            List<PlayerInformation> alivePlayers = new List<PlayerInformation>();
            foreach (PlayerInformation player in players)
            {
                if (!player.isDead && player.colorId != botPlayer.colorId)
                {
                    alivePlayers.Add(player);
                }
            }
            return alivePlayers;
        }

        public List<PlayerInformation> getLivingCrewmatesThatArentBot()
        {
            List<PlayerInformation> alivePlayers = new List<PlayerInformation>();
            foreach (PlayerInformation player in players)
            {
                if (!player.isDead && player.colorId != botPlayer.colorId && !player.isImposter)
                {
                    alivePlayers.Add(player);
                }
            }
            return alivePlayers;
        }

        public List<PlayerInformation> getDeadPlayers()
        {
            List<PlayerInformation> deadPlayers = new List<PlayerInformation>();
            foreach (PlayerInformation player in players)
            {
                if (player.isDead)
                {
                    deadPlayers.Add(player);
                }
            }
            return deadPlayers;
        }

        public PlayerInformation getPlayerByColor(byte colorId)
        {
            foreach (PlayerInformation player in players){
                if (player.colorId == colorId)
                {
                    return player;
                }
            }
            return new PlayerInformation();
        }

        public PlayerInformation getClosestPlayer(byte colorId)
        {
            byte closestColor = colorId;
            double distance = 9999999;
            Vector2 currentPos = getPlayerByColor(colorId).position;
            foreach (PlayerInformation player in players)
            {
                if (player.colorId != colorId)
                {
                    double temp_distance = AuUtils.vectorDistance(currentPos, player.position);
                    if (temp_distance < distance)
                    {
                        closestColor = player.colorId;
                        distance = temp_distance;
                    }
                }
            }
            return getPlayerByColor(closestColor);
        }

        public PlayerInformation getClosestCrewmate(byte colorId)
        {
            byte closestColor = colorId;
            double distance = 9999999;
            Vector2 currentPos = getPlayerByColor(colorId).position;
            foreach (PlayerInformation player in players)
            {
                if (player.colorId != colorId && !player.isImposter)
                {
                    double temp_distance = AuUtils.vectorDistance(currentPos, player.position);
                    if (temp_distance < distance)
                    {
                        closestColor = player.colorId;
                        distance = temp_distance;
                    }
                }
            }
            return getPlayerByColor(closestColor);
        }

        public double getClosestCrewmateToPoint(Vector2 position)
        {
            byte closestColor;
            double distance = 9999999;
            foreach (PlayerInformation player in players)
            {
                if (!player.isImposter && !player.isDead)
                {
                    double temp_distance = AuUtils.vectorDistance(position, player.position);
                    if (temp_distance < distance)
                    {
                        closestColor = player.colorId;
                        distance = temp_distance;
                    }
                }
            }
            return distance;
        }

        public PlayerInformation getClosestImposter(byte colorId)
        {
            byte closestColor = colorId;
            double distance = 9999999;
            Vector2 currentPos = getPlayerByColor(colorId).position;
            foreach (PlayerInformation player in getImposters())
            {
                if (player.colorId != colorId)
                {
                    double temp_distance = AuUtils.vectorDistance(currentPos, player.position);
                    if (temp_distance < distance)
                    {
                        closestColor = player.colorId;
                        distance = temp_distance;
                    }
                }
            }
            return getPlayerByColor(closestColor);
        }

        public List<PlayerInformation> getNearbyPlayers(byte colorId)
        {
            double radius = 1.5;
            Vector2 currentPos = getPlayerByColor(colorId).position;
            List<PlayerInformation> nearbyPlayers = new List<PlayerInformation>();
            foreach (PlayerInformation player in players)
            {
                if (player.colorId != colorId)
                {
                    double distance = AuUtils.vectorDistance(currentPos, player.position);
                    if (distance < radius)
                    {
                        nearbyPlayers.Add(player);
                    }
                }
            }
            return nearbyPlayers;
        }

        public List<PlayerInformation> getVisiblePlayers()
        {
            Vector2 currentPos = botPlayer.position;
            List<PlayerInformation> visiblePlayers = new List<PlayerInformation>();
            foreach (var player in players)
            {
                if (player.colorId != botPlayer.colorId)
                {
                    if (player.position.isVisible(currentPos, lightRadius))
                    {
                        visiblePlayers.Add(player);
                    }
                }
            }
            return visiblePlayers;
        }
    }
}
