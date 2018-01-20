﻿using System;
using System.Windows.Forms;
using ACEngine.Engine;

namespace ACEngine.Assets
{
    class RotateMove : Behavior
    {
        private float MoveSpeed = 0.08f;
        public int MoveDir = 1;
        public override void Update()
        {
            if (Input.GetKeyDown(Keys.C))
            {
                Console.WriteLine(233);
            }
            transform.rotation.y += 1.6f;
            transform.rotation.x += 1.6f;
            transform.rotation.z += 1.6f;
            if (MoveDir == 1 && transform.position.z > 20) MoveDir = -1;
            else if (MoveDir == -1 && transform.position.z < 2) MoveDir = 1;
            transform.position.z += MoveSpeed * MoveDir;

           
        }

    }
}