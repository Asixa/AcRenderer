using ACEngine.Engine;

namespace ACEngine.Assets
{
    class RotateMove : Behavior
    {
        private float MoveSpeed = 0.00f;
        public int MoveDir = 1;
        public override void Update()
        {
            transform.rotation.y += 1.6f;
            transform.rotation.x += 1.6f;
            transform.rotation.z += 1.6f;
            if (MoveDir == 1 && transform.position.z > 10) MoveDir = -1;
            else if (MoveDir == -1 && transform.position.z < 2) MoveDir = 1;
            transform.position.z += MoveSpeed * MoveDir;
        }

    }
}
