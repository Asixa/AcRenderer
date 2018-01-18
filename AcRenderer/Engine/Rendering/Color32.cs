using ACEngine.Math;
namespace ACEngine.Engine.Rendering
{
    public class Color32
    {
        private float _r;
        private float _b;
        private float _g;

        public float r
        {
            get => Mathx.Range(_r, 0, 1);
            set => _r = Mathx.Range(value, 0, 1);
        }

        public float g
        {
            get => Mathx.Range(_g, 0, 1);
            set => _g = value;
        }

        public float b
        {
            get => Mathx.Range(_b, 0, 1);
            set => _b = value;
        }

        public Color32(float r, float g, float b)
        {
            this._r = Mathx.Range(r, 0, 1);
            this._g = Mathx.Range(g, 0, 1);
            this._b = Mathx.Range(b, 0, 1);
        }

        public Color32(System.Drawing.Color c)
        {
            this._r = Mathx.Range((float)c.R / 255, 0, 1);
            this._g = Mathx.Range((float)c.G / 255, 0, 1);
            this._b = Mathx.Range((float)c.B / 255, 0, 1);
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

        public static Color32 operator *(Color32 a, Color32 b) => new Color32(a.r * b.r, a.g * b.g, a.b * b.b);
        public static Color32 operator *(float a, Color32 b) => new Color32(a * b.r, a * b.g, a * b.b);
        public static Color32 operator *(Color32 a, float b)=>new Color32(a.r * b, a.g * b, a.b * b);
        public static Color32 operator +(Color32 a, Color32 b)=>new Color32(a.r + b.r, a.g + b.g, a.b + b.b);

    }
}
