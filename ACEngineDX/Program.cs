using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ACEngineDX.Base;
using ACEngine;
using ACEngine.Engine;
using ACEngine.Engine.Rendering;
using ACEngine.Engine.Scene;
using ACEngineDX.Assets;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using Color = System.Drawing.Color;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using Random = ACEngineDX.Math.Random;

namespace ACEngineDX
{
    public class Program:D2DWindow
    {
        [STAThread]
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run(new DxConfiguration("ACEngine-Acform 硬件加速", 512, 512));
        }
        public static Program main;
        public int Width => 512;
        public int Height => 512;

        public byte[] _memory;
        private Bitmap _backBufferBmp;

        public SolidColorBrush LineBrush;
        public Size2 BufferSize=new Size2(512,512);
        public RawColor4 backgroundColor=new RawColor4(0, 0.5f, 1, 1);
        public BitmapProperties BufferProperties;

        public Canvas canvas;



        public void Update()
        {
            Input.Check();
            SceneManager.Current.OnUpdate();
            CheckForChangeMode();
          
        }

        public void Start()
        {
            LineBrush = new SolidColorBrush(RenderTarget2D, new RawColor4());
            BufferProperties = new BitmapProperties(RenderTarget2D.PixelFormat);
            SceneManager.Current = new Scene();
            SceneManager.Current.Start();
            canvas = new Canvas();
            _memory = new byte[Width * Height * 4];
            _backBufferBmp = new Bitmap(RenderTarget2D, BufferSize, BufferProperties);

            Input.Init();
            form.AllowDrop = true;
            form.DragEnter += DragEnter;
            form.DragDrop += DragDrop;
        }

        #region 绘制

        public void SetPixel(int x, int y, Color color)
        {
            var i = Width * 4 * y + x * 4;
            _memory[i] = color.B;
            _memory[i + 1] = color.G;
            _memory[i + 2] = color.R;
            _memory[i + 3] = color.A;
        }

        public void DrawLine(int xs, int xe, int y, Color c)
        {
            LineBrush.Color = new RawColor4(c.R, c.G, c.B, c.A);
            RenderTarget2D.DrawLine(new RawVector2(xs, y), new RawVector2(xe, y), LineBrush);
        }

        public void DrawLine(Vertex f, Vertex t)
        {
            LineBrush.Color = new RawColor4(0, 0, 0, 1);
            RenderTarget2D.DrawLine(new RawVector2(f.point.x, f.point.y), new RawVector2(t.point.x, t.point.y), LineBrush,3);
        } //=> DrawLine(f, t, Color.Black);

        public void DrawLine(Vertex f, Vertex t, Color c)
        {
            LineBrush.Color = new RawColor4(c.R, c.G, c.B, c.A);
            RenderTarget2D.DrawLine(new RawVector2(f.point.x, f.point.y), new RawVector2(t.point.x, t.point.y), LineBrush);
        }
        #endregion

        #region 拖放
        private void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            Behavior.Spawn(GameObject.Create(new ACEngine.Engine.Rendering.Mesh(ObjModelLoader.ObjLoader.load(path))), new ACEngine.Math.Vector3(0, 0, 2), new ACEngine.Math.Vector3(0, 0, 0)).AddComponent(new MouseMove());
        }
        #endregion

        #region 继承函数

        protected override void Initialize(DxConfiguration demoConfiguration)
        {
            main = this;
            base.Initialize(demoConfiguration);
            Start();
        }

        protected override void Draw(DxTime time)
        {
            base.Draw(time);
            RenderTarget2D.Clear(backgroundColor);
            Array.Clear(_memory, 0, _memory.Length);
            Update();

            canvas.Draw();

            _backBufferBmp.CopyFromMemory(_memory, Width * 4);
            RenderTarget2D.DrawBitmap(_backBufferBmp, 1f, BitmapInterpolationMode.Linear);
            //_backBufferBmp.Dispose();
        }

        #endregion

        public void CheckForChangeMode()
        {
            if (Input.GetKeyDown(Key.Space))
            {
                if ((int) canvas.draw_mode == 4) canvas.draw_mode = 0;
                else canvas.draw_mode++;
            }
        }
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


    }
}
