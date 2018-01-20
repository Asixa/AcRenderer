using System;
using ACEngine.Engine.Rendering;

namespace ACEngine.Math
{
    class Mathx
    {
        public const float Rad2Deg =57.29578f;
        public static float sin(float v)=>(float) System.Math.Sin(Convert.ToDouble(v));
        public static float cos(float v) => (float)System.Math.Cos(Convert.ToDouble(v));
        public static float sqrt(float v) => (float) System.Math.Sqrt(Convert.ToDouble(v));
        public static float atan(float v) => (float)System.Math.Atan(Convert.ToDouble(v));
        public static float asin(float v) => (float)System.Math.Asin(Convert.ToDouble(v));
        public static float acos(float v) => (float)System.Math.Acos(Convert.ToDouble(v));

        public static int Range(int v, int min, int max)
        {
            if (v <= min)
            {
                return min;
            }
            return v >= max ? max : v;
        }
        public static float Range(float v, float min, float max)
        {
            if (v <= min)
            {
                return min;
            }
            return v >= max ? max : v;
        }
        public static int GetQuadrant(float x, float y)
        {
            if (x > 0)
            {
                if (y > 0) return 1;
                if (y <0) return 4;
            }
            else
            {
                if (y > 0) return 2;
                if (y < 0) return 3;
            }
            return -1;
        }

        public static Color32 Lerp(Color32 a, Color32 b, float t)
        {
            if (t <= 0)
            {
                return a;
            }
            if (t >= 1)
            {
                return b;
            }
            return t * b + (1 - t) * a;
        }

        public static float Lerp(float a, float b, float t)
        {
            if (t <= 0)
            {
                return a;
            }
            if (t >= 1)
            {
                return b;
            }
            return b * t + (1 - t) * a;
        }

        public static void ScreenSpaceLerpVertex(ref Vertex v, Vertex v1, Vertex v2, float t)
        {
            v.onePerZ = Lerp(v1.onePerZ, v2.onePerZ, t);
            //
            v.u =Lerp(v1.u, v2.u, t);
            v.v =Lerp(v1.v, v2.v, t);
            //
            v.vcolor = Lerp(v1.vcolor, v2.vcolor, t);
            //
            v.lightingColor =Lerp(v1.lightingColor, v2.lightingColor, t);
        }
    }
}
