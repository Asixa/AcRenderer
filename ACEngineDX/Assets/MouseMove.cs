
using System;
using AcForm;
using ACEngine.Engine;
using ACEngine.Engine.Scene;
using SharpDX.DirectInput;

namespace ACEngine.Assets
{
    class MouseMove:Behavior
    {
        public override void Update()
        {

            if (Input.GetMouse(0))
            {
                transform.rotation.x -= Input.mouse_axis.y *1f;
                transform.rotation.y += Input.mouse_axis.x * 1f;
            }
            float speed = 0.5f;
            if (Input.GetKey(Key.LeftShift)) speed = 1;
           SceneManager.Current.main_camera.transform.position.z += speed*Input.GetMouseWhell();
        }
    }
}
