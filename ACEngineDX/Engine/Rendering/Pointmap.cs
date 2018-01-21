﻿//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Bitmap = SharpDX.WIC.Bitmap;

//namespace ACEngineDX.Engine.Rendering
//{
//    public class PointBitmap
//    {
//        Bitmap source = null;
//        IntPtr Iptr = IntPtr.Zero;
//        BitmapData bitmapData = null;

//        public int Depth { get; private set; }
//        public int Width { get; private set; }
//        public int Height { get; private set; }

//        public PointBitmap(Bitmap source)
//        {
//            this.source = source;
//        }

//        public void LockBits()
//        {source.NativePointer=new IntPtr();
//            try
//            {
//                // Get width and height of bitmap
//                Width = source.Width;
//                Height = source.Height;

//                // get total locked pixels count
//                int PixelCount = Width * Height;

//                // Create rectangle to lock
//                Rectangle rect = new Rectangle(0, 0, Width, Height);

//                // get source bitmap pixel format size
//                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

//                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
//                if (Depth != 8 && Depth != 24 && Depth != 32)
//                {
//                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
//                }

//                // Lock bitmap and return bitmap data
//                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
//                    source.PixelFormat);

//                //得到首地址
//                unsafe
//                {
//                    Iptr = bitmapData.Scan0;
//                    //二维图像循环

//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public void UnlockBits()
//        {
//            try
//            {
//                source.UnlockBits(bitmapData);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public Color GetPixel(int x, int y)
//        {
//            unsafe
//            {
//                byte* ptr = (byte*)Iptr;
//                ptr = ptr + bitmapData.Stride * y;
//                ptr += Depth * x / 8;
//                Color c = Color.Empty;
//                if (Depth == 32)
//                {
//                    int a = ptr[3];
//                    int r = ptr[2];
//                    int g = ptr[1];
//                    int b = ptr[0];
//                    c = Color.FromArgb(a, r, g, b);
//                }
//                else if (Depth == 24)
//                {
//                    int r = ptr[2];
//                    int g = ptr[1];
//                    int b = ptr[0];
//                    c = Color.FromArgb(r, g, b);
//                }
//                else if (Depth == 8)
//                {
//                    int r = ptr[0];
//                    c = Color.FromArgb(r, r, r);
//                }
//                return c;
//            }
//        }

//        public void SetPixel(int x, int y, Color c)
//        {
//            unsafe
//            {
//                byte* ptr = (byte*)Iptr;
//                ptr = ptr + bitmapData.Stride * y;
//                ptr += Depth * x / 8;
//                if (Depth == 32)
//                {
//                    ptr[3] = c.A;
//                    ptr[2] = c.R;
//                    ptr[1] = c.G;
//                    ptr[0] = c.B;
//                }
//                else if (Depth == 24)
//                {
//                    ptr[2] = c.R;
//                    ptr[1] = c.G;
//                    ptr[0] = c.B;
//                }
//                else if (Depth == 8)
//                {
//                    ptr[2] = c.R;
//                    ptr[1] = c.G;
//                    ptr[0] = c.B;
//                }
//            }
//        }
//    }
//}
