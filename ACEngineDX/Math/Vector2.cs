using System.Collections.Generic;
using System.Linq;
using ObjModelLoader;

namespace ACEngine.Math
{
    public class Vector2
    {
        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()=>"<" + x + "," + y + ">";

        public static Vector2 FromObj(ObjVector2 v) => new Vector2(v.x, v.y);

        public static List<Vector2> FromObj(ObjVector2[] v) => v.Select(FromObj).ToList();

        public Vector3 v3()=>new Vector3(x,y,0);
    }
}
