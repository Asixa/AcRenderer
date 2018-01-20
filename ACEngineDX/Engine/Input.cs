
using System;
using System.Collections.Generic;
using System.Linq;
using ACEngine.Math;
using SharpDX.DirectInput;

namespace ACEngine.Engine
{
    class Input
    {
        private static DirectInput directInput;
        private static Keyboard keyboard;
        private static Mouse mouse;
        private static KeyboardUpdate[] Keydata;
        private static MouseUpdate[] Mousedata;
        private static List<Key> HoldingKey=new List<Key>();
        private static List<int> HoldingMouse = new List<int>();
        public  static Vector2 MouseAsix;

        public static void Init()
        {
            directInput = new DirectInput();
            keyboard = new Keyboard(directInput);
            mouse=new Mouse(directInput);
            keyboard.Properties.BufferSize = 128;
            keyboard.Acquire();
            mouse.Properties.BufferSize = 128;
            mouse.Acquire();
        }

        public static void Check()
        {
                keyboard.Poll();
                Keydata = keyboard.GetBufferedData();
                foreach (var state in Keydata) CheckHoldingKey(state);

                mouse.Poll();
                Mousedata= mouse.GetBufferedData();
                GetMouseAsix();
                foreach (var t in Mousedata)CheckHoldingKey(t);
        }

        public static void CheckHoldingKey(KeyboardUpdate key)
        {
            if(key.IsPressed)HoldingKey.Add(key.Key);
            else if (key.IsReleased) HoldingKey.Remove(key.Key);
        }
        public static void CheckHoldingKey(MouseUpdate m)
        {
            if(!m.IsButton)return;
            if (m.Value==128) HoldingMouse.Add((int)m.Offset-12);
            else if (m.Value == 0) HoldingMouse.Remove((int)m.Offset - 12);
        }

        public static bool GetKeyDown(Key k)
        {
            return Keydata.Where(t => t.IsPressed).Any(t => t.Key == k);
        }
        public static bool GetKeyUp(Key k)
        {
            return Keydata.Where(t => t.IsReleased).Any(t => t.Key == k);
        }
        public static bool GetKey(Key k)
        {
            return HoldingKey.Contains(k);
        }

        private static void GetMouseAsix()
        {
            MouseAsix = new Vector2(0,0);
            foreach (var t in Mousedata)
            {
                switch (t.Offset)
                {
                    case MouseOffset.X:
                        MouseAsix.x += t.Value;
                        break;
                    case MouseOffset.Y:
                        MouseAsix.y += t.Value;
                        break;
                    case MouseOffset.Z:
                        break;
                    case MouseOffset.Buttons0:
                        break;
                    case MouseOffset.Buttons1:
                        break;
                    case MouseOffset.Buttons2:
                        break;
                    case MouseOffset.Buttons3:
                        break;
                    case MouseOffset.Buttons4:
                        break;
                    case MouseOffset.Buttons5:
                        break;
                    case MouseOffset.Buttons6:
                        break;
                    case MouseOffset.Buttons7:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static bool GetMouseDown(int id)
        {
            return Mousedata.Where(t => t.Offset == (MouseOffset)(id+12)).Any(t => t.Value == 128);
        }
        public static bool GetMouseUp(int id)
        {
            return Mousedata.Where(t => t.Offset == (MouseOffset)(id + 12)).Any(t => t.Value == 0);
        }

        public static bool GetMouse(int id)=>HoldingMouse.Contains(id);
    }
}
