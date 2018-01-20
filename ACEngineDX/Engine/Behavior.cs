using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACEngine.Engine.Scene;
using ACEngine.Math;
using ACEngine.Mathf;

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
