using System;
using ACEngine.Engine;
using SharpDX.DirectInput;

namespace ACEngineDX.Assets
{
    class ObjControl:Behavior
    {
        public override void Update()
        {
            if (Input.GetKey(Key.Q)) transform.rotation.z++;
            if (Input.GetKey(Key.E)) transform.rotation.z--;
            if (Input.GetKey(Key.W)) transform.position.z+=0.05f;
            if (Input.GetKey(Key.S)) transform.position.z -= 0.05f;
            if (Input.GetKey(Key.A)) transform.position.x -= 0.05f;
            if (Input.GetKey(Key.D)) transform.position.x += 0.05f;
        }
    }
}
