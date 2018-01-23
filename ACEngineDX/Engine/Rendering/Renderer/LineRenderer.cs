using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACEngine.Math;

namespace ACEngine.Engine.Rendering.Renderer
{
    class LineRenderer:Renderer
    {
        public Vector3 s, e;
        public Color color;
        public LineRenderer(Vector3 f,Vector3 t,Color c)
        {
            s = f;
            e = t;
            color = c;
        }

        public override void Render()
        {
            var canvas = Program.main.canvas;
            
            Vector3 start= GetPTC(s),end=GetPTC(e);
            Console.WriteLine(canvas.ToScreen(start) + " "+ canvas.ToScreen(end));
            Program.main.DrawLine(canvas.ToScreen(start).v3(),canvas.ToScreen(end).v3(),color,2);
        }
    }
}
