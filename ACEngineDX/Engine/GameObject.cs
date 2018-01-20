using System;
using System.Collections.Generic;
using ACEngine.Engine.Rendering;

namespace ACEngine.Engine
{
    public class GameObject
    {
        public List<Base> components=new List<Base>();
        public Transform transform;
        public MeshRenderer meshRenderer;
        public Camera camera;
        public GameObject()
        {
            transform=new Transform();
            meshRenderer = new MeshRenderer {gameObject = this};
            components.Add(transform);
            components.Add(meshRenderer);
        }

        public object AddComponent(Behavior com)
        {
            com.gameObject = this;
            components.Add(com);
            return com;
        }

        public static GameObject Create(Mesh mesh)
        {
            var g = new GameObject {meshRenderer = {mesh = mesh}};
            return g;
        }


    }
}
