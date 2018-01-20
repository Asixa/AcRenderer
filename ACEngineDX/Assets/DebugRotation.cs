using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACEngine.Engine;

namespace ACEngine.Assets
{
    class DebugRotation:Behavior
    {
        public override void Update()
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                transform.rotation.z += 1;
            }
        }
    }
}
