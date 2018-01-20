
using System;
using ACEngine.Engine;

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

           
        }
    }
}
