using System.Collections.Generic;
using System.Drawing;
using ACEngine.Engine.Rendering;
using ACEngine.Engine.Rendering.Renderer;
using ACEngine.Math;
using ACEngine.Engine;

namespace ACEngine.Engine
{
    public class GameObject
    {
        public List<Base> components=new List<Base>();
        public Transform transform;
        public Renderer renderer;
        public Camera camera;
        public GameObject()
        {
            transform=new Transform();
            components.Add(transform);
        }

        public object AddComponent(Behavior com)
        {
            com.gameObject = this;
            components.Add(com);
            return com;
        }

        public static GameObject Create(Mesh mesh)
        {
            var g = new GameObject();
            g.renderer=new MeshRenderer(mesh,g);
            return g;
        }

        public static GameObject CreateLine(Vector3 f, Vector3 t, Color c)
        {
            var g = new GameObject {renderer = new LineRenderer(f,t,c)};
            return g;
        }
    }
}
