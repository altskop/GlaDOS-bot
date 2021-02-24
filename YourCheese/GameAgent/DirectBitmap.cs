using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace YourCheese
{
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, System.Drawing.Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public System.Drawing.Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            System.Drawing.Color result = System.Drawing.Color.FromArgb(col);

            return result;
        }

        public int[][] getBits()
        {
            int[][] bitsArray = new int[Height][];

            for (int y=0; y<Height; y++)
            {
                bitsArray[y] = new int[Width];
                for (int x=0; x<Width; x++)
                {
                    int index = x + (y * Width);
                    bitsArray[y][x] = Bits[index];
                }
            }
            return bitsArray;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }

        public Bitmap getAsNewBitmap()
        {
            Int32 width = Width;
            Int32 height = Height;
            Int32 stride = width * 4;
            Int32 byteIndex = 0;
            Byte[] dataBytes = new Byte[height * stride];
            for (Int32 y = 0; y < height; y++)
            {
                for (Int32 x = 0; x < width; x++)
                {
                    // UInt32 0xAARRGGBB = Byte[] { BB, GG, RR, AA }
                    int index = x + (y * Width);
                    UInt32 val = (UInt32)Bits[index];
                    // This code clears out everything but a specific part of the value
                    // and then shifts the remaining piece down to the lowest byte
                    dataBytes[byteIndex + 0] = (Byte)(val & 0x000000FF); // B
                    dataBytes[byteIndex + 1] = (Byte)((val & 0x0000FF00) >> 08); // G
                    dataBytes[byteIndex + 2] = (Byte)((val & 0x00FF0000) >> 16); // R
                    dataBytes[byteIndex + 3] = (Byte)((val & 0xFF000000) >> 24); // A
                                                                                    // More efficient than multiplying
                    byteIndex += 4;
                }
            }
            return BuildImage(dataBytes, width, height, stride, PixelFormat.Format32bppArgb, null, null);
        }

        public static Bitmap BuildImage(Byte[] sourceData, Int32 width, Int32 height, Int32 stride, PixelFormat pixelFormat, System.Drawing.Color[] palette, System.Drawing.Color? defaultColor)
        {
            Bitmap newImage = new Bitmap(width, height, pixelFormat);
            BitmapData targetData = newImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, newImage.PixelFormat);
            Int32 newDataWidth = ((Image.GetPixelFormatSize(pixelFormat) * width) + 7) / 8;
            // Compensate for possible negative stride on BMP format.
            Boolean isFlipped = stride < 0;
            stride = Math.Abs(stride);
            // Cache these to avoid unnecessary getter calls.
            Int32 targetStride = targetData.Stride;
            Int64 scan0 = targetData.Scan0.ToInt64();
            for (Int32 y = 0; y < height; y++)
                Marshal.Copy(sourceData, y * stride, new IntPtr(scan0 + y * targetStride), newDataWidth);
            newImage.UnlockBits(targetData);
            // Fix negative stride on BMP format.
            if (isFlipped)
                newImage.RotateFlip(RotateFlipType.Rotate180FlipX);
            // For indexed images, set the palette.
            if ((pixelFormat & PixelFormat.Indexed) != 0 && palette != null)
            {
                ColorPalette pal = newImage.Palette;
                for (Int32 i = 0; i < pal.Entries.Length; i++)
                {
                    if (i < palette.Length)
                        pal.Entries[i] = palette[i];
                    else if (defaultColor.HasValue)
                        pal.Entries[i] = defaultColor.Value;
                    else
                        break;
                }
                newImage.Palette = pal;
            }
            return newImage;
        }
    }
}