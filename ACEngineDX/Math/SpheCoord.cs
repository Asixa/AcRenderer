using ACEngine.Math;

namespace ACEngine.Mathf
{
    class SpheCoord
    {
        public float p, x, y;
        public SpheCoord() { }

        public SpheCoord(float p1, float x1, float y1)
        {
            p = p1;
            x = x1;
            y = y1;
        }

        public override string ToString()
        {
            return "<" + x + "," + y + "," + p + ">";
        }

        public Vector3 ToVector3() => new Vector3
        {
            x = p * Mathx.sin(y) * Mathx.cos(x),
            y = p * Mathx.sin(y) * Mathx.sin(x),
            z = p * Mathx.cos(y)
        };

        public static SpheCoord ToSpheCoord(Vector3 v3) 
        {
            var p=Mathx.sqrt(v3.x*v3.x+v3.y+v3.y+v3.z*v3.z);
            var r = Mathx.sqrt(v3.x * v3.x + v3.y + v3.y);
            var x = Mathx.atan(v3.y / v3.x);
            var y = Mathx.asin(r / p);
            return new SpheCoord(r,x,y);
        }

    }
}
