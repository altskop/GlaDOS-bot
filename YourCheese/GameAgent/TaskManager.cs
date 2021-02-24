using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using WindowsInput;
using WindowsInput.Native;

namespace YourCheese
{

    public class GameTask
    {
        public Vector2 position;
        public string name;
        public bool isDone = false;
        public int fakeTime;

        public GameTask(Vector2 position, string name, int fakeTime)
        {
            this.position = position;
            this.name = name;
            this.fakeTime = fakeTime;
        }
    }

    class TaskLocationResolver
    {
        public static Dictionary<Vector2, GameTask> taskLocations = new Dictionary<Vector2, GameTask>()
        {
            {new Vector2(151, 412), new GameTask(new Vector2(44, 290), "Manifolds", 1100)}, // manifolds
            {new Vector2(187, 541), new GameTask(new Vector2(77,377), "Simon Says", 6000)}, // reactor
            {new Vector2(251, 845), new GameTask(new Vector2(143, 596), "Lower Engine Calibration", 1100)}, // lower engine
            {new Vector2(342, 831), new GameTask(new Vector2(171, 582), "Lower Gas", 3300)}, // lower gas
            {new Vector2(256, 350), new GameTask(new Vector2(144, 237), "Upper Engine Calibration", 1100)}, // upper engine
            {new Vector2(334, 334), new GameTask(new Vector2(172, 227), "Upper Gas", 3300)}, // upper gas
            {new Vector2(367, 185), new GameTask(new Vector2(198, 130), "Power Switch in Upper Reactor", 800)}, // power switch upper reactor
            {new Vector2(543, 431), new GameTask(new Vector2(336, 313), "Power Switch in Security", 800)}, // power switch security
            {new Vector2(307, 688), new GameTask(new Vector2(175, 497), "Power Switch in Lower Engine", 800)}, // power switch lower engine
            {new Vector2(755, 531), new GameTask(new Vector2(483, 356), "Medscan", 10)}, // medbay
            {new Vector2(809, 483), new GameTask(new Vector2(503, 346), "Inspect sample in medbay", 1000)}, // med analyzer
            {new Vector2(638, 614), new GameTask(new Vector2(411, 443), "Download in electrical", 9500)}, //download electrical
            {new Vector2(688, 602), new GameTask(new Vector2(422, 441), "Power switch in electrical", 800)}, // electrical wires + power switch
            {new Vector2(821, 613), new GameTask(new Vector2(523, 450), "Wires in electrical", 900)}, // wires
            {new Vector2(759, 613), new GameTask(new Vector2(468, 440), "Electrical distributor", 2500)}, // electrical distributor
            {new Vector2(943, 880), new GameTask(new Vector2(600, 624), "Gas", 5000)}, // gas
            {new Vector2(1076, 1006), new GameTask(new Vector2(712, 706), "Bottom Trash", 3000)}, // bottom trash
            {new Vector2(998, 675), new GameTask(new Vector2(644, 480), "Wires in storage", 900)}, // wires storage
            {new Vector2(1291, 664), new GameTask(new Vector2(875, 443), "Card swipe", 1600)}, // card swipe
            {new Vector2(836, 98), new GameTask(new Vector2(552, 64), "Wires in cafe", 900)}, // wires top left cafe
            {new Vector2(1179, 108), new GameTask(new Vector2(787, 71), "Download in cafe", 10000)}, // download cafe
            {new Vector2(1225, 164), new GameTask(new Vector2(813, 98), "Trash in cafe", 3500)}, // trash cafe
            {new Vector2(1097, 571), new GameTask(new Vector2(736, 414), "Wires in admin", 900)}, // admin wire
            {new Vector2(1158, 572), new GameTask(new Vector2(770, 406), "Upload", 10000)}, // upload admin
            {new Vector2(1199, 887), new GameTask(new Vector2(823, 642), "Download in comms", 10000)}, // comms download
            {new Vector2(1299, 889), new GameTask(new Vector2(873, 640), "Power switch in comms", 900)}, // switch in comms
            {new Vector2(1383, 870), new GameTask(new Vector2(930, 630), "Shields", 900)}, // shields
            {new Vector2(1491, 727), new GameTask(new Vector2(1004, 519), "Power switch in shields", 900)}, // shields switch
            {new Vector2(1631, 471), new GameTask(new Vector2(1124, 341), "Navigation wires", 900)}, // nav wires
            {new Vector2(1735, 411), new GameTask(new Vector2(1195, 295), "Navigation download", 10000)}, // nav download
            {new Vector2(1780, 456), new GameTask(new Vector2(1227, 328), "Rocket in nav", 3100)}, // rocket in nav
            {new Vector2(1803, 516), new GameTask(new Vector2(1227, 328), "Cross in nav", 800)}, // nav cross
            {new Vector2(1313, 437), new GameTask(new Vector2(874, 302), "leaves", 3500)}, // leaves in o2
            {new Vector2(1240, 462), new GameTask(new Vector2(854, 315), "Trash in o2", 3500)}, // trash in o2
            {new Vector2(1391, 416), new GameTask(new Vector2(946, 299), "Power switch in o2", 900)}, // switch in o2
            {new Vector2(1415, 228), new GameTask(new Vector2(969, 173), "Asteroids", 15000)}, // weapons
            {new Vector2(1395, 152), new GameTask(new Vector2(955, 105), "Download in weapons", 10000)}, // download in weaponss
            {new Vector2(1513, 230), new GameTask(new Vector2(1025, 152), "Power switch in weapons", 900)} // switch in weapons
        };
    }

    public class TaskManager
    {
        public static InputSimulator inputSimulator = new InputSimulator();
        public List<GameTask> getTaskPositions()
        {
            List<GameTask> tasks = new List<GameTask>();
            // open map
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
            System.Threading.Thread.Sleep(200);
            DirectBitmap mapCapture = GameCapture.getGameScreen();
            System.Threading.Thread.Sleep(150);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);

            foreach (KeyValuePair<Vector2, GameTask> entry in TaskLocationResolver.taskLocations)
            {
                // do something with entry.Value or entry.Key
                int blueColor = mapCapture.GetPixel((int)entry.Key.x, (int)entry.Key.y).B;
                int greenColor = mapCapture.GetPixel((int)entry.Key.x, (int)entry.Key.y).G;
                int redColor = mapCapture.GetPixel((int)entry.Key.x, (int)entry.Key.y).R;
                if (blueColor < 142 && greenColor > 95 && redColor > 95)
                {
                    tasks.Add(entry.Value);
                }
            }
            mapCapture.Dispose();
            return tasks;
        }
    }
}
