using AcForm;
using ACEngine.Engine;
using SharpDX.DirectInput;

namespace ACEngine.Assets
{
    class ObjControl:Behavior
    {
        public override void Update()
        {
            float move = 0.01f;

            if (Input.GetKey(Key.W)) transform.position-=transform.rotation*move;
            if (Input.GetKey(Key.S)) transform.position+= transform.rotation * move;


            if (Input.GetMouse(0))
            {
                transform.rotation.x += Input.mouse_axis.y * 1f;
                transform.rotation.y -= Input.mouse_axis.x * 1f;
            }
        }
    }
}
