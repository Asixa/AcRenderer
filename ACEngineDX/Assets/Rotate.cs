using ACEngine.Engine;

namespace ACEngine.Assets
{
    class Rotate:Behavior
    {
        public override void Update()
        {
            transform.rotation.y += 1f;
            
        }
    }
}
