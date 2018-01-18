using System;
using System.Windows.Forms;
using ACEngine.Engine;

namespace ACEngine.Assets
{
    class CameraMove:Behavior
    {
        public override void Update()
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                transform.position.x+= 0.01f;
                Console.WriteLine(transform.position);
            }
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                transform.position.y += 0.01f;
            }
            if (Input.GetKeyDown(Keys.W))
            {
                transform.position.z++;
                Console.WriteLine("w");
            }
            if (Input.GetKeyDown(Keys.S))
            {
                Console.WriteLine("S");
                transform.position.z--;
            }
        }
    }
}
