using System;
using HamsterCheese;

//Readed Player List
static List<PlayerData> playerDatas = new List<PlayerData>(); 

// Update Your Cheat 
static void UpdateCheat()
{
   while (true)
   { 
       foreach (var data in playerDatas)
       {
           Console.WriteLine("Find Player Name :: " + Utils.ReadString(data.PlayerInfo.Value.PlayerName));
       } 
       System.Threading.Thread.Sleep(100); 
   }
}

// Update Player List EveryGame.
static void OnDetectJoinNewGame()
{
   playerDatas = HamsterCheese.AmongUsMemory.Cheese.GetAllPlayers();
}

static void Main(string[] args)
{
   // Cheat Init
   if (HamsterCheese.AmongUsMemory.Cheese.Init())
   { 
       // Update Player Data When Join New Map.
       HamsterCheese.AmongUsMemory.Cheese.ObserveShipStatus(OnDetectJoinNewGame);

       // Start Your Cheat 
       CancellationTokenSource cts = new CancellationTokenSource();
       Task.Factory.StartNew(
           UpdateCheat
       , cts.Token); 
   }

   System.Threading.Thread.Sleep(1000000);
}