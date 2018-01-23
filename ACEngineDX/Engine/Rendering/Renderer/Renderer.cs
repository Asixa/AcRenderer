using System;
using ACEngine.Engine.Scene;
using ACEngine.Math;
using ACEngine;
using ACEngine.Engine;

namespace ACEngine.Engine.Rendering.Renderer
{
    public class Renderer:Component
    {
        public int type;
        public virtual void Render(){}

        public static Vector3 GetRelativePositionToVertex(Vector3 t, Vector3 zP, Vector3 zR, bool debug = false)
        {
            var rot = zR / Mathx.Rad2Deg;
            float x = t.x, y = t.y, z = t.z;
            float y0 = y, x0 = x, z0 = z;
            //y
            var a = rot.y;
            z0 = z * Mathx.cos(a) - x * Mathx.sin(a);
            x0 = z * Mathx.sin(a) + x * Mathx.cos(a);
            y = y0;
            z = z0;
            //x
            a = rot.x;
            y0 = y * Mathx.cos(a) - z * Mathx.sin(a);
            z0 = y * Mathx.sin(a) + z * Mathx.cos(a);
            x = x0;
            y = y0;
            //z
            a = rot.z;
            x0 = x * Mathx.cos(a) - y * Mathx.sin(a);
            y0 = x * Mathx.sin(a) + y * Mathx.cos(a);
            if (debug) Console.WriteLine("相对" + t + " 结果" + (new Vector3(x0, y0, z0) + zP));
            return new Vector3(x0, y0, z0) + zP;
        }

        public static Vector3 GetRelativePosition(Vector3 t, Vector3 zP, Vector3 zR, bool debug = false)
        {
            var rot = zR / Mathx.Rad2Deg;

            float x = t.x, y = t.y, z = t.z;
            float y0 = y, x0 = x, z0 = z;
            //y
            var a = rot.y;
            z0 = z * Mathx.cos(a) - x * Mathx.sin(a);
            x0 = z * Mathx.sin(a) + x * Mathx.cos(a);
            y = y0;
            z = z0;
            //x
            a = rot.x;
            y0 = y * Mathx.cos(a) - z * Mathx.sin(a);
            z0 = y * Mathx.sin(a) + z * Mathx.cos(a);
            x = x0;
            y = y0;
            //z
            a = rot.z;
            x0 = x * Mathx.cos(a) - y * Mathx.sin(a);
            y0 = x * Mathx.sin(a) + y * Mathx.cos(a);
            return new Vector3(x0, y0, z0);
        }

        public static Vector3 GetPTC(Vector3 vW)
        {
            var camera = SceneManager.Current.main_camera.transform;
            return GetRelativePosition(vW - camera.position, camera.position, camera.rotation, true);
        }
    }
}
