
using System;
using ACEngine.Engine;
using SharpDX.DirectInput;

namespace ACEngineDX.Assets
{
    class MouseMove:Behavior
    {
        public override void Update()
        {

            if (Input.GetMouse(0))
            {
                transform.rotation.x -= Input.MouseAsix.y *1f;
                transform.rotation.y += Input.MouseAsix.x * 1f;
            }
            float speed = 0.5f;
            if (Input.GetKey(Key.LeftShift)) speed = 1;
            transform.position.z -= speed*Input.GetMouseWhell();
        }
    }
}
