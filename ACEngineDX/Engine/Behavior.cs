using ACEngine.Engine.Rendering.Renderer;
using ACEngine.Engine.Scene;
using ACEngine.Math;
using ACEngine.Engine;

namespace ACEngine.Engine
{
    public class Behavior:Base
    {
        public GameObject gameObject;
        public Transform transform=>gameObject.transform;
        
        public MeshRenderer meshRenderer;

        public static GameObject Spawn(GameObject obj, Vector3 pos, Vector3 rot)
        {
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            SceneManager.Current.ObjectInScene.Add(obj);
            return obj;
        }

    }
}
