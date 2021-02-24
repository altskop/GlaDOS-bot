
using HamsterCheese.AmongUsMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YourCheese.GameAgent.Forms;
using System.Windows.Forms;
using YourCheese.GameAgent.Conversation;
using YourCheese.GameAgent;

namespace YourCheese
{
    class Program
    {
        static int tableWidth = 170;
        public static int MEMORY_POLLING_PERIOD = 90;

        static SkeldMap skeld;
        static BehaviorDriver behaviorDriver;
        static BotStatus botStatusForm;
        static EventGenerator eventGenerator;
        static List<PlayerData> playerDatas = new List<PlayerData>(); 
        static void UpdateCheat()
        {
            /*while (true)
            {
                Console.Clear();
                AmongUsClient resultInst = HamsterCheese.AmongUsMemory.Cheese.getAmongUsClient();
                //Console.WriteLine(resultInst.timer + " | " + resultInst.IsGamePublic + " | " + resultInst.GameState + " | " + resultInst.SpawnRadius + " | " + resultInst.mode);
                System.Threading.Thread.Sleep(2000);
            }*/
            
            while (true)
            {
                Console.Clear();
                ShipStatus shipStatus = HamsterCheese.AmongUsMemory.Cheese.shipStatus;
                //PrintRow($"Timer: {shipStatus.Timer}", $"EmergencyCooldown: {shipStatus.EmergencyCooldown}", $"Type: {shipStatus.Type}");

                GameDataContainer gameData = new GameDataContainer();
                gameData.emergencyCooldown = shipStatus.EmergencyCooldown;
                List<PlayerInformation> players = new List<PlayerInformation>();
                
                Console.WriteLine("Test Read Player Datas..");
                PrintRow("Name", "Position", "Color", "isDead", "Emergencies", "inVent", "isImposter", "killTimer");
                PrintLine();

                foreach (var data in playerDatas)
                {
                    var Name = HamsterCheese.AmongUsMemory.Utils.ReadString(data.PlayerInfo.Value.PlayerName);
                    if (data.IsLocalPlayer)
                    {
                        gameData.botPlayer = new PlayerInformation(data.Position, Name, data.PlayerInfo.Value.ColorId, data.PlayerInfo.Value.IsDead, data.remainingEmergencies(), data.inVent(), data.PlayerInfo.Value.IsImpostor, data.getKillTimer());
                        LightSource ls = data.LightSource;
                        gameData.lightRadius = ls.LightRadius;
                    }
                    else
                    {
                        PlayerInformation player = new PlayerInformation(data.Position, Name, data.PlayerInfo.Value.ColorId, data.PlayerInfo.Value.IsDead, data.remainingEmergencies(), data.inVent(), data.PlayerInfo.Value.IsImpostor, data.getKillTimer());
                        players.Add(player);
                    }

                    //PrintRow($"{(data.IsLocalPlayer == true ? "Me->" : "")}{data.PlayerControllPTROffset}", $"{Name}", $"{data.Instance.OwnerId}", $"{data.Instance.PlayerId}", $"{data.Instance.SpawnId}", $"{data.Instance.SpawnFlags}");
                }

                gameData.players = players;

                Console.ForegroundColor = ConsoleColor.Green;
                PrintRow($"{gameData.botPlayer.name}", $"{gameData.botPlayer.position.x},{gameData.botPlayer.position.y}", $"{gameData.botPlayer.color}", $"{gameData.botPlayer.isDead.ToString()}", $"{gameData.botPlayer.remainingEmergencies.ToString()}", $"{gameData.botPlayer.inVent.ToString()}", $"{gameData.botPlayer.isImposter.ToString()}", $"{gameData.botPlayer.killTimer.ToString()}");
                Console.ForegroundColor = ConsoleColor.White;

                foreach (var player in players)
                {
                    PrintRow($"{player.name}", $"{player.position.x},{player.position.y}", $"{player.color}", $"{player.isDead.ToString()}", $"{player.remainingEmergencies.ToString()}", $"{player.inVent.ToString()}", $"{player.isImposter.ToString()}",$"{player.killTimer.ToString()}");
                    Console.ForegroundColor = ConsoleColor.White;

                    PrintLine();
                }
                PrintRow($"Light level: {gameData.lightRadius}");
                PrintRow($"OriginalPos: {gameData.botPlayer.position.x},{gameData.botPlayer.position.y}");
                Vector2 meshPos = skeld.gamePosToMeshPos(gameData.botPlayer.position);
                PrintRow($"MeshPos: {meshPos.x},{meshPos.y}");
                Vector2 gaemPos = skeld.meshPosToGamePos(meshPos);
                PrintRow($"GaemPos: {gaemPos.x},{gaemPos.y}");
                PrintRow($"Region: {skeld.getLocationRegionName(meshPos)}");
                PrintRow($"Timer: {shipStatus.Timer}");

                GameUpdate gameUpdate = eventGenerator.getGameUpdate(gameData);
                if (gameUpdate.gameDataContainer != null && gameData.players.Count > 0)
                    behaviorDriver.update(gameUpdate);
                botStatusForm.update(behaviorDriver);

                System.Threading.Thread.Sleep(MEMORY_POLLING_PERIOD);
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            skeld = new SkeldMap();
            behaviorDriver = new BehaviorDriver(skeld);
            eventGenerator = new EventGenerator();
            Application.EnableVisualStyles();
            botStatusForm = new BotStatus();
            Task.Run(() => Application.Run(botStatusForm));
            System.Threading.Thread.Sleep(3500);
            // Memory Init
            if (HamsterCheese.AmongUsMemory.Cheese.Init())
            { 
                // Update Player Data When Every Game
                HamsterCheese.AmongUsMemory.Cheese.ObserveShipStatus((x) =>
                {
                    
                    //stop observe state for init. 
                    foreach(var player in playerDatas) 
                        player.StopObserveState(); 


                    playerDatas = HamsterCheese.AmongUsMemory.Cheese.GetAllPlayers();
                    
                  
                 
                    foreach (var player in playerDatas)
                    {
                        player.onDie += (pos, colorId) => {
                            Console.WriteLine("OnPlayerDied! Color ID :" + colorId);
                        }; 
                        // player state check
                        player.StartObserveState();
                    }

                    CancellationTokenSource cts2 = new CancellationTokenSource();
                    Task.Factory.StartNew(
                        behaviorDriver.run
                    , cts2.Token);

                });

                // Cheat Logic
                CancellationTokenSource cts = new CancellationTokenSource();
                Task.Factory.StartNew(
                    UpdateCheat
                , cts.Token);
                
                //Task.Factory.StartNew(() => behaviorDriver.run());
            }

            System.Threading.Thread.Sleep(10000000);
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);

            
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        } 
    }
}


