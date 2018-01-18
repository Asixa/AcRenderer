using ACEngine.Engine.Rendering;
using ACEngine.Engine.Scene;
using ACEngine.Math;

namespace ACEngine.Engine
{
    public class MeshRenderer :Component
    {
        public GameObject gameObject;
        public Transform transform => gameObject.transform;
        public Mesh mesh;
        public Material material;

        public Vertex GetCameraPosition(int i)=>new Vertex(mesh.vertices[i])
            {
                point = GetRelativePosition(transform.position, SceneManager.Current.main_camera.transform) -
                        SceneManager.Current.main_camera.transform.position+
                        GetRelativePosition(mesh.vertices[i].point, new Transform
                        {
                            position = transform.position,
                            rotation = transform.rotation + SceneManager.Current.main_camera.transform.rotation
                        })
            };

        public static Vector3 GetRelativePosition(Vector3 t, Transform zero)
        {
            var rot = zero.rotation/Mathx.Rad2Deg;
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

            return new Vector3(x0, y0,z0);
        }
    }

    public struct Triangle
    {
        public int p1, p2,p3;
        public Triangle(int ll1, int ll2,int ll3)
        {
            p1 = ll1;
            p2 = ll2;
            p3 = ll3;
        }
    }
}
