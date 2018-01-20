using ACEngine.Math;

namespace ACEngine.Engine
{
    public class Transform : Component
    {
        public Vector3 
            position = new Vector3(), 
            positionToCamera=new Vector3(),
            localScale=new Vector3(),
            rotation = new Vector3(),
            RotationToCamera=new Vector3();
    }
}
