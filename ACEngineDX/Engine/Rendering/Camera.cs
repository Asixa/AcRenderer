using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACEngine.Engine
{
    public class Camera :Component
    {
        public float LineOfSight = 1;
        public GameObject gameObject;
        public Transform transform => gameObject.transform;
        public static GameObject CreateNew()
        {
            return new GameObject{camera = new Camera(),renderer = null};
        }
    }
}
