using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamsterCheese.AmongUsMemory
{
    class ClientData
    {
        public static int gameState(IntPtr pointer)
        {
            var targetPointer = Utils.GetMemberPointer(pointer, typeof(AmongUsClient), "GameState");
            int val = Cheese.mem.ReadByte(targetPointer.GetAddress());
            return val;
        }

        public static int state(IntPtr pointer)
        {
            var targetPointer = Utils.GetMemberPointer(pointer, typeof(AmongUsClient), "mode");
            int val = Cheese.mem.ReadByte(targetPointer.GetAddress());
            return val;
        }

        public static int gameMode(IntPtr pointer)
        {
            var targetPointer = Utils.GetMemberPointer(pointer, typeof(AmongUsClient), "GameMode");
            int val = Cheese.mem.ReadByte(targetPointer.GetAddress());
            return val;
        }
    }
}
