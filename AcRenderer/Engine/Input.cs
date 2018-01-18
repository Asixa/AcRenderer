
using System.Collections.Generic;
using System.Windows.Forms;

namespace ACEngine.Engine
{
    class Input
    {
        public static List<Keys> key=new List<Keys>();

        public static bool GetKeyDown(Keys k)=> Control.ModifierKeys==(k);
    }
}
