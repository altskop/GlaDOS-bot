using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YourCheese
{

    class GameCapture
    {
        public static DirectBitmap getGameScreen()
        {
            Rectangle bounds = new Rectangle(0, 0, 1920, 1080);
            using (DirectBitmap bitmap = new DirectBitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap.Bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Bitmap.Save("C:/Studio/templates/CURRENTLY_ORIGINAL.png", ImageFormat.Jpeg);
                
                return bitmap;
            }
        }

        public static DirectBitmap getGameScreen(Rectangle bounds)
        {
            using (DirectBitmap bitmap = new DirectBitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap.Bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }

                return bitmap;
            }
        }

        public static Bitmap getGameScreenAsImage()
        {
            Rectangle bounds = new Rectangle(0, 0, 1920, 1080);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height,
                               PixelFormat.Format32bppArgb);

            var g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            bitmap.Save("C:/Studio/templates/CURRENTLY_ORIGINAL.png");

            return bitmap;
            
        }

        public static Bitmap getGameScreenAsImage(Rectangle bounds)
        {
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height,
                               PixelFormat.Format32bppArgb);

            var g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            bitmap.Save("D:/Studio/Programming/HK47/AmongUsMemory-master/YourCheese/GameAgent/TaskSolvers/templates/CURRENTLY_ORIGINAL.png");

            return bitmap;

        }

    }
}
