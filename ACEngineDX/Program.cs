using System;
using System.Windows.Forms;
using AcForm;
using ACEngine.Assets;
using ACEngine.Engine;
using ACEngine.Engine.Rendering;
using ACEngine.Engine.Scene;
using ACEngine.Math;
using SharpDX.DirectInput;
using Color = System.Drawing.Color;
namespace ACEngine
{
    public class Program:DxWindow
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var program = new Program();
            program.Run(new DxConfiguration("ACEngine-Acform 硬件加速", 512, 512));
        }
        public static Program main;
        public Canvas canvas;

        public override void Update()
        {
            SceneManager.Current.OnUpdate();
            CheckForChangeMode();
            ProgramControl();
            form.Text ="[ "+(int)FramePerSecond+ " FPS]"+(canvas.FlipNormal ? "[反向]" : "") +" "+ Config.Title + " 模式:"+canvas.draw_mode;
            canvas.Draw();
        }

        public override void Start()
        {
            main = this;

            backgroundColor=new DxColor(71/255f, 71/255f, 71/255f);
            SceneManager.Current = new Scene();
            SceneManager.Current.Start();
            canvas = new Canvas();
            form.AllowDrop = true;
            form.DragEnter += DragEnter;
            form.DragDrop += DragDrop;
        }

        #region 绘制

        public void DrawLine(int xs, int xe, int y, Color c)
        {
            DrawLine(xs,  y, xe, y, Mathx.ToDxColor(c));
        }


        public void DrawLine(Vertex f, Vertex t)
        {
            DrawLine((int)f.point.x, (int)f.point.y, (int)t.point.x, (int)t.point.y, Mathx.ToDxColor(Color.Black), 3);
        } 

        public void DrawLine(Math.Vector3 f, Math.Vector3 t, Color c,int w=2)
        {
            DrawLine((int)f.x, (int)f.y, (int)t.x, (int)t.y,Mathx.ToDxColor(c), w);
        }
        #endregion

        #region 拖放
        private void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void DragDrop(object sender, DragEventArgs e)
        {
            var path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            try
            {
                Behavior.Spawn(GameObject.Create(new Mesh(ObjModelLoader.ObjLoader.load(path))), new Vector3(0, 0, 2), new Vector3(0, 0, 0)).AddComponent(new MouseMove());
            }
            catch (Exception)
            {
                // ignored
            }
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

        public void ProgramControl()
        {
            if (Input.GetKeyDown(Key.F5))
            {
                if (SceneManager.Current.ObjectInScene.Count > 1)
                {
                    SceneManager.Current.ObjectInScene.Remove(SceneManager.Current.ObjectInScene[1]);
                }

            }
            if (Input.GetKeyDown(Key.F1))
            {
                canvas.FlipNormal = !canvas.FlipNormal;
            }
        }
    }
}
