
using ACEngine.Math;
using ACEngine.Mathf;

namespace ACEngine.Engine.Rendering
{
    public class Vertex
    {
        /// <summary>
        /// 顶点位置
        /// </summary>
        public Vector3 point;
        /// <summary>
        /// 纹理坐标
        /// </summary>
        public float u;
        public float v;
        /// <summary>
        /// 顶点色
        /// </summary>
        public Color32 vcolor;
        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 normal;
        /// <summary>
        /// 光照颜色
        /// </summary>
        public Color32 lightingColor;
        /// <summary>
        /// 1/z，用于顶点信息的透视校正
        /// </summary>
        public float onePerZ;

        public Vertex(Vector3 point, Vector3 normal, float u, float v, Vector3 color)
        {
            this.point = point;
            this.normal = normal;
            vcolor = new Color32(color.x, color.y, color.z);
            onePerZ = 1;
            this.u = u;
            this.v = v;
            lightingColor = new Color32(1, 1, 1);
        }

        public Vertex()
        {
            point = new Vector3();
            vcolor = new Color32(0, 0, 0);
            lightingColor = new Color32(0, 0, 0);
            u = v = 0;
            normal = new Vector3();
            onePerZ = 1;
        }
        public Vertex(Vector3 v3)
        {
            point = v3;
            vcolor = new Color32(0, 0, 0);
            lightingColor = new Color32(0, 0, 0);
            u = v = 0;
            normal = new Vector3();
            onePerZ = 1;
        }

        public Vertex(Vertex v)
        {
            point = v.point;
            normal = v.normal;
            this.vcolor = v.vcolor;
            onePerZ = 1;
            this.u = v.u;
            this.v = v.v;
            this.lightingColor = v.lightingColor;
        }
    }
}
