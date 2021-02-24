using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using YourCheese.GameAgent.TaskSolvers;

namespace YourCheese
{
    class TaskIdentifier
    {

        private Bitmap screen;
        private TaskSolver taskSolver;

        public void doTask()
        {
            new TaskInput().pressE();
            System.Threading.Thread.Sleep(600);
            screen = GameCapture.getGameScreenAsImage();
            taskSolver = getTaskSolver();
            if (taskSolver != null)
            {
                taskSolver.Solve(GameCapture.getGameScreen());
            }
            screen.Dispose();
            System.Threading.Thread.Sleep(500);
        }

        public void abort()
        {
            taskSolver.abort();
        }

        public TaskSolver getTaskSolver()
        {
            if (containsImage("asteroids", new Rectangle(939, 523, 41, 38)))
            {
                return new AsteroidsSolver();
            }
            else if (containsImage("card_swipe", new Rectangle(502, 84, 274, 132)))
            {
                return new CardSwipeSolver();
            }
            else if (containsImage("chart_course", new Rectangle(389, 196, 199, 162)))
            {
                return new ChartCourseSolver();
            }
            else if (containsImage("distributor", new Rectangle(875, 437, 140, 195)))
            {
                return new DistributorSolver();
            }
            else if (containsImage("divert_power", new Rectangle(800, 150, 192, 84)))
            {
                return new DivertPowerSolver();
            }
            else if (containsImage("divert_power_switch", new Rectangle(465, 463, 166, 149)))
            {
                return new DivertPowerSwitchSolver();
            }
            else if (containsImage("download", new Rectangle(390, 654, 207, 195)))
            {
                return new DownloadSolver();
            }
            else if (containsImage("engine_adjust", new Rectangle(1045, 854, 137, 139)))
            {
                return new EngineAdjustSolver();
            }
            else if (containsImage("gas_fill", new Rectangle(1350, 764, 225, 228)))
            {
                return new GasSolver();
            }
            else if (containsImage("inspect_sample", new Rectangle(1141, 898, 159, 78)))
            {
                return new InspectSampleSolver();
            }
            else if (containsImage("inspect_sample_ready", new Rectangle(1141, 898, 159, 78)))
            {
                return new InspectSampleReadySolver();
            }
            else if (containsImage("leaves", new Rectangle(501, 290, 193, 516)))
            {
                return new LeavesSolver();
            }
            else if (containsImage("manifolds", new Rectangle(507, 315, 103, 95)))
            {
                return new ManifoldSolver();
            }
            else if (containsImage("medscan", new Rectangle(662, 114, 170, 89)))
            {
                return new MedscanSolver();
            }
            else if (containsImage("shields", new Rectangle(511, 93, 162, 197)))
            {
                return new ShieldsSolver();
            }
            else if (containsImage("simon_says", new Rectangle(424, 263, 117, 148)))
            {
                return new SimonSaysSolver();
            }
            else if (containsImage("steering", new Rectangle(508, 88, 177, 166)))
            {
                return new SteeringSolver();
            }
            else if (containsImage("trash", new Rectangle(1154, 355, 246, 249)))
            {
                return new TrashSolver();
            }
            else if (containsImage("wires", new Rectangle(908, 77, 210, 45)))
            {
                return new WireSolver();
            }

            return null;
        }

        public bool containsImage(string templateName, Rectangle rect)
        {
            //String filename = "D:/Studio/Programming/HK47/AmongUsMemory-master/YourCheese/GameAgent/TaskSolvers/templates/" + templateName + ".png";
            //using (Bitmap loadedNeedle = new Bitmap(filename))
            //{
            //    DirectBitmap needle = new DirectBitmap(loadedNeedle.Width, loadedNeedle.Height);
            //    using (Graphics g = Graphics.FromImage(needle.Bitmap))
            //    {
            //        g.DrawImage(loadedNeedle, new Point(0,0));
            //    }
            //    bool result = Find(this.screen, needle) != null;
            //    return result;
            //}



            Bitmap croppedImage = screen.Clone(rect, screen.PixelFormat);
            croppedImage.Save(Constants.FILE_LOCATION + "/templates/CURRENTLY_SCANNED.png");
            Image<Bgr, byte> Image1 = croppedImage.ToImage<Bgr, byte>(); //Your first image

            String filename = Constants.FILE_LOCATION + "/templates/" + templateName + ".jpg";
            Image<Bgr, byte> Image2 = new Image<Bgr, byte>(filename); //Your second image

            double Threshold = 0.7; //set it to a decimal value between 0 and 1.00, 1.00 meaning that the images must be identical

            Image<Gray, float> Matches = Image1.MatchTemplate(Image2, TemplateMatchingType.CcoeffNormed);

            for (int y = 0; y < Matches.Data.GetLength(0); y++)
            {
                for (int x = 0; x < Matches.Data.GetLength(1); x++)
                {
                    if (Matches.Data[y, x, 0] >= Threshold) //Check if its a valid match
                    {
                        //Image2 found within Image1
                        return true;
                    }
                }
            }
            return false;
        }

        public Point? Find(DirectBitmap haystack, DirectBitmap needle)
        {
            if (null == haystack || null == needle)
            {
                return null;
            }
            if (haystack.Width < needle.Width || haystack.Height < needle.Height)
            {
                return null;
            }

            int[][] haystackArray = haystack.getBits();
            int[][] needleArray = needle.getBits();

            FindMatch(haystackArray.Take(haystack.Height - needle.Height), needleArray[0]);
            foreach (var firstLineMatchPoint in FindMatch(haystackArray.Take(haystack.Height - needle.Height), needleArray[0]))
            {
                if (IsNeedlePresentAtLocation(haystackArray, needleArray, firstLineMatchPoint, 1))
                {
                    return firstLineMatchPoint;
                }
            }

            return null;
        }

        private IEnumerable<Point> FindMatch(IEnumerable<int[]> haystackLines, int[] needleLine)
        {
            var y = 0;
            foreach (var haystackLine in haystackLines)
            {
                for (int x = 0, n = haystackLine.Length - needleLine.Length; x < n; ++x)
                {
                    if (ContainSameElements(haystackLine, x, needleLine, 0, needleLine.Length))
                    {
                        yield return new Point(x, y);
                    }
                }
                y += 1;
            }
        }

        private bool ContainSameElements(int[] first, int firstStart, int[] second, int secondStart, int length)
        {
            for (int i = 0; i < length; ++i)
            {
                if (first[i + firstStart] != second[i + secondStart])
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsNeedlePresentAtLocation(int[][] haystack, int[][] needle, Point point, int alreadyVerified)
        {
            //we already know that "alreadyVerified" lines already match, so skip them
            for (int y = alreadyVerified; y < needle.Length; ++y)
            {
                if (!ContainSameElements(haystack[y + point.Y], point.X, needle[y], 0, needle[y].Length))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
