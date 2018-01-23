using System;
using System.Drawing;
namespace ACEngine.Engine.Rendering
{
    public class Material
    {
        public Bitmap texture;
        public Color32 emissive=new Color32(0,0,0), specular=new Color32(1,1,1), diffuse=new Color32(1,1,1) ;
        public float  shininess=0.5f;
        public Material()
        {
            texture = new Bitmap(Image.FromFile(Environment.CurrentDirectory + "/texture.jpg"), 256, 256);
        }
    }
}
