using System;
using ACEngineDX.Base;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
namespace ACEngineDX
{
    public class Program:D2DWindow
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>

        private Bitmap _bitmap;
        public static Bitmap LoadFromFile(RenderTarget renderTarget, string file)
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                var stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (var y = 0; y < bitmap.Height; y++)
                    {
                        var offset = bitmapData.Stride * y;
                        for (var x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            var B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            var G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            var R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            var A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            var rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }

        protected override void Initialize(DxConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);
            _bitmap = LoadFromFile(RenderTarget2D, "1.png");
        }

        protected override void Draw(DxTime time)
        {
            base.Draw(time);

            // Draw the TextLayout

            RenderTarget2D.DrawBitmap(_bitmap, 1.0f, BitmapInterpolationMode.Linear);
        }


        [STAThread]
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run(new DxConfiguration("ACEngine",512,512));
        }
    }
}
