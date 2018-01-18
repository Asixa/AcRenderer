using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using ACEngine.Engine;
using ACEngine.Engine.Scene;
namespace ACEngine
{
    public partial class Form1 : Form
    {
        public static Form1 main;
        public Bitmap frame_buff;
        public Graphics frame_graphics;
        Graphics g = null;
        public Canvas canvas;
        private System.Timers.Timer main_timer;


        public Form1()
        {
            main = this;
            InitializeComponent();

            frame_buff = new Bitmap(Width, Height);
            frame_graphics = Graphics.FromImage(frame_buff);
            canvas = new Canvas(frame_buff, frame_graphics);

            KeyPreview = true;

            SceneManager.Current = new Scene();
            SceneManager.Current.Start();
            
            main_timer = new System.Timers.Timer(1000 / 1000f);
            main_timer.Elapsed += Tick;
            main_timer.AutoReset = true;
            main_timer.Enabled = true;
            main_timer.Start();
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            var t1 = DateTime.Now;
            lock (frame_buff)
            {
                frame_graphics.Clear(Color.DeepSkyBlue);
                SceneManager.Current.OnUpdate();
                if (g == null) g = CreateGraphics();
                canvas.Draw();
                g.DrawImage(frame_buff, 0, 0);
            }
            Input.key.Clear(); ;
            Time.deltatime = (DateTime.Now - t1).Milliseconds;

            main_timer.Interval = Time.deltatime/10;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.key.Add(e.KeyCode);
            if (e.KeyCode == Keys.Space)
            {
                switch (canvas.draw_mode)
                {
                    case Canvas.DrawMode.Wireframe:
                        canvas.draw_mode =Canvas.DrawMode.WireframeWithoutCulling;
                        break;
                    case Canvas.DrawMode.WireframeWithoutCulling:
                        canvas.draw_mode = Canvas.DrawMode.GouraudShading;
                        break;
                    case Canvas.DrawMode.GouraudShading:
                        canvas.draw_mode = Canvas.DrawMode.NoShading;
                        break;
                    case Canvas.DrawMode.Tex_Wire:
                        canvas.draw_mode = Canvas.DrawMode.Wireframe;
                        break;case Canvas.DrawMode.NoShading:
                        canvas.draw_mode = Canvas.DrawMode.Tex_Wire;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
          ///  Console.WriteLine("D2D2");
//_D.Render();
        }
    }
}
