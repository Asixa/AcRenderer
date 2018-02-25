using AcForm;
using ACEngine.Math;

namespace ACEngine.Engine.Rendering
{
    public class Color32
    {


        public float r;

        public float g;

        public float b;

        public Color32(float r, float g, float b)
        {
            this.r = Mathx.Range(r, 0, 1);
            this.g = Mathx.Range(g, 0, 1);
            this.b = Mathx.Range(b, 0, 1);
        }

        public Color32(System.Drawing.Color c)
        {
            this.r = Mathx.Range((float)c.R / 255, 0, 1);
            this.g = Mathx.Range((float)c.G / 255, 0, 1);
            this.b = Mathx.Range((float)c.B / 255, 0, 1);
        }

        public Color32()
        {
            r = g = b = 0;
        }

        public override string ToString()
        {
            return "<" + r + "," + g + "," + b + ">";
        }

        /// <summary>
        /// 转换为系统的color
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color ToColor()
        {
            var r = this.r * 255;
            var g = this.g * 255;
            var b = this.b * 255;
            return System.Drawing.Color.FromArgb((int)r, (int)g, (int)b);
        }
        public DxColor ToDX()=> new DxColor(r,g,b);

        public static Color32 operator *(Color32 a, Color32 b) => new Color32(a.r * b.r, a.g * b.g, a.b * b.b);
        public static Color32 operator *(float a, Color32 b) => new Color32(a * b.r, a * b.g, a * b.b);
        public static Color32 operator *(Color32 a, float b)=>new Color32(a.r * b, a.g * b, a.b * b);
        public static Color32 operator +(Color32 a, Color32 b)=>new Color32(a.r + b.r, a.g + b.g, a.b + b.b);

    }
}
