using System.Collections.Generic;
using ACEngine.Assets;
using ACEngine.Engine.Rendering;
using ACEngine.Math;
using ACEngineDX.Assets;

namespace ACEngine.Engine.Scene
{
    internal class Scene
    {
        public List<GameObject> ObjectInScene = new List<GameObject>();
        public GameObject main_camera;
        public void OnUpdate()
        {
            foreach (var t in ObjectInScene)
            {
                foreach (var c in t.components)
                {
                    c.Update();
                }
            }
        }

        public void OnStart()
        {
            foreach (var t in ObjectInScene)
            {
                foreach (var c in t.components)
                {
                    c.Start();
                }
            }
        }

        public void Start()
        {
            main_camera = Camera.CreateNew();
            main_camera.AddComponent(new ObjControl());
            ObjectInScene.Add(main_camera);
            main_camera.transform.position=new Vector3(0,0,0);

            //Behavior.Spawn(GameObject.Create(new Mesh(Model.Plane)), new Vector3(0, 2, 5), new Vector3(0, 0, 0)).AddComponent(new Rotate());

            //Behavior.Spawn(GameObject.Create(new Mesh(Model.Cube)), new Vector3(0, 0, 5), new Vector3(0, 0, 0)).AddComponent(new MouseMove());
            //Behavior.Spawn(GameObject.Create(new Mesh(Model.Cube)), new Vector3(2, 0, 5), new Vector3(0, 0, 0));//.AddComponent(new Rotate());
            //Behavior.Spawn(GameObject.Create(new Mesh(Model.Cube)), new Vector3(-2, 0, 5), new Vector3(0, 0, 0));//.AddComponent(new Rotate());
            //Behavior.Spawn(GameObject.Create(new Mesh(Model.Cube)), new Vector3(0, 2, 5), new Vector3(0, 0, 0));//.AddComponent(new Rotate());
            //Behavior.Spawn(GameObject.Create(new Mesh(Model.Cube)), new Vector3(0, -2, 5), new Vector3(0, 0, 0));//.AddComponent(new Rotate());
           // Behavior.Spawn(GameObject.Create(new Mesh(Model.Cube)), new Vector3(0, 0, 20), new Vector3(0, 0, 0)).AddComponent(new MouseMove());

            OnStart();
        }
    }
}
