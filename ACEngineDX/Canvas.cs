using System;
using System.Drawing;
using AcForm;
using ACEngine.Engine.Rendering;
using ACEngine.Engine.Rendering.Renderer;
using ACEngine.Engine.Scene;
using ACEngine.Math;
using ACEngine;
using ACEngine.Engine;

namespace ACEngine
{
    public class Canvas
    {
        public enum DrawMode
        {
            Wireframe,
            WireframeWithoutCulling,
            GouraudShading,
            Texture,
            Tex_Wire,
            NoShading
        }

        public bool FlipNormal = false;
        public DrawMode draw_mode = DrawMode.Texture;
        private static int Width => 512;
        private static int Height => 512;
        public Color currentColor;
        public Material currentMaterial;
        public Light _light = new Light();
        public Vector3 lightpos=new Vector3(3,-3,2);

        public void Draw()
        {
            foreach (var t in SceneManager.Current.ObjectInScene)
            {
                t.renderer?.Render();
            }
        }
    
        private bool BackFaceCulling(Vector3 p1, Vector3 p2, Vector3 p3,out Vector3 normal)
        {

            if (draw_mode == DrawMode.WireframeWithoutCulling)
            {
                normal = new Vector3(); return true;
            }
            var camera_position = SceneManager.Current.main_camera.transform.position;
            var v1 = p2 - p1;
            var v2 = p3 - p2;
            normal = Vector3.Cross(v1, v2);
            var view_dir = p1 - camera_position; 
            return FlipNormal? Vector3.Dot(normal, view_dir) <= 0: Vector3.Dot(normal, view_dir) > 0;
        }

        public void FillLine(Color32 lc, Color32 rc, int xs, int xe, int y,int debug)

        {   float dx = System.Math.Abs(xe - xs);
            for (var x = xs; x <= xe; x ++)
            {
                if (x >= Width || y >= Height || x <= 0 || y <= 0)
                {
                    continue;
                };
                    var factor = System.Math.Abs((x - xs + 1) / (dx + 1));
                    var color = Mathx.Lerp(lc, rc, factor);
                Program.main.SetPixel(x, y, color.ToDX());
            }
        }

        public void VFillLine(Vertex left,Vertex right,int y)
        {
            //int y = (int)((left.point.y+right.point.y)/2);
            float dx = System.Math.Abs(left.point.x - right.point.x);
            for (var x = (int)left.point.x; x <= right.point.x; x++)
            {
                if (x >= Width || y >= Height || x <= 0 || y <= 0)
                {
                    continue;
                };
                var factor = System.Math.Abs((x - left.point.x + 1) / (dx + 1));
                var color = Mathx.Lerp(left.vcolor, right.vcolor, factor);
                var lcolor = Mathx.Lerp(left.lightingColor, right.lightingColor, factor);
                float u = Mathx.Lerp(left.u, right.u, factor),
                      v=Mathx.Lerp(left.v,right.v,factor);

                var c =new Color32(ReadTexture(u, v, currentMaterial.texture));
                Program.main.SetPixel(x, y, (c*lcolor).ToDX());
            }
        }

        #region 绘制三角形

        public void DrawTriangle(MeshRenderer renderer, int i)
        {
          

            var p1= renderer.GetCameraPosition(i); 
            var p2 = renderer.GetCameraPosition(i+1); 
            var p3 = renderer.GetCameraPosition(i+2);
            if (!BackFaceCulling(p1.point, p2.point,p3.point,out Vector3 normal)) return;
            p1.normal = p2.normal = p3.normal = normal;
            BakeLight(ref p1); BakeLight(ref p2); BakeLight(ref p3);
            Triangle(ToScreen(p1), ToScreen(p2), ToScreen(p3));
        }

      
        public void BakeLight(ref Vertex v)
        {
            Vector3 worldPoint = v.point;//世界空间顶点位置
            Vector3 normal= v.normal;   //模型空间法线乘以世界矩阵的逆转置得到世界空间法线
            Color32 emissiveColor =currentMaterial.emissive;//自发光
            Color32 ambientColor = new Color32(0.2f,0.2f,0.3f);//环境光 

            Vector3 inLightDir = (lightpos- worldPoint);
            float diffuse = Vector3.Dot(normal, inLightDir);
            if (diffuse < 0)
            {
                diffuse = 0;
            }
            Color32 diffuseColor = currentMaterial.diffuse * diffuse * _light.lightColor;//漫反射
            //
            Vector3 inViewDir = (SceneManager.Current.main_camera.transform.position - worldPoint);
            Vector3 h = ( inLightDir);
            float specular = 0;
            if (diffuse != 0)

            {//防止出现光源在物体背面产生高光的情况
                specular = (float)System.Math.Pow(Mathx.Range(Vector3 .Dot(h, normal), 0, 1), currentMaterial.shininess);
            }
            Color32 specularColor =currentMaterial.specular * specular * _light.lightColor;//镜面高光
            //
            v.lightingColor = emissiveColor + ambientColor + diffuseColor + specularColor;
        }

        public void Triangle(Vertex p0, Vertex p1, Vertex p2)
        {
            if (draw_mode == DrawMode.Wireframe || draw_mode == DrawMode.WireframeWithoutCulling || draw_mode == DrawMode.Tex_Wire || draw_mode == DrawMode.GouraudShading)
            {
                Program.main.DrawLine(p0, p1);
                Program.main.DrawLine(p1, p2);
                Program.main.DrawLine(p2, p0);
                if (draw_mode == DrawMode.Wireframe || draw_mode == DrawMode.WireframeWithoutCulling) return;
            }

          


            if (p0.point.y == p1.point.y) // 0 1 为平
            {
                if (p0.point.y < p2.point.y)  //平顶 2为底
                {
                    TriangleT(p2, p0, p1);
                }
                else //平底 2为顶
                {
                    TriangleD(p2, p0, p1);
                }
            }
            else if (p0.point.y == p2.point.y) //0 2 为平
            {
                if (p0.point.y < p1.point.y)//平1顶
                {
                    TriangleT(p1, p0, p2);
                }
                else//平1底
                {
                    TriangleD(p1, p0, p2);
                }
            }
            else if (p1.point.y == p2.point.y) //1,2 为平
            {
                if (p1.point.y < p0.point.y)
                {//平0顶
                    TriangleT(p0, p1, p2);
                }
                else
                {//平0底
                    TriangleD(p0, p1, p2);
                }
            }
            else
            {//分割三角形
                Vertex top, bottom, middle;
                if (p0.point.y > p1.point.y && p1.point.y > p2.point.y)
                {
                    top = p2;
                    middle = p1;
                    bottom = p0;
                }
                else if (p2.point.y > p1.point.y && p1.point.y > p0.point.y)
                {
                    top = p0;
                    middle = p1;
                    bottom = p2;
                }
                else if (p1.point.y > p0.point.y && p0.point.y > p2.point.y)
                {
                    top = p2;
                    middle = p0;
                    bottom = p1;
                }
                else if (p2.point.y > p0.point.y && p0.point.y > p1.point.y)
                {
                    top = p1;
                    middle = p0;
                    bottom = p2;
                }
                else if (p0.point.y > p2.point.y && p2.point.y > p1.point.y)
                {
                    top = p1;
                    middle = p2;
                    bottom = p0;
                }
                else if (p1.point.y > p2.point.y && p2.point.y > p0.point.y)
                {
                    top = p0;
                    middle = p2;
                    bottom = p1;
                }
                else
                {
                    //三点共线
                    return;
                }
                //插值求中间点x
                var middlex = (middle.point.y - top.point.y) * (bottom.point.x - top.point.x) / (bottom.point.y - top.point.y) + top.point.x;
                float dy = middle.point.y - top.point.y;
                float t = dy / (bottom.point.y - top.point.y);//插值生成左右顶点
                Vertex newMiddle = new Vertex(new Vector3(middlex, middle.point.y, 0));
                var f = (top.point.y - middle.point.y) / (top.point.y - bottom.point.y);
                newMiddle = Mathx.Lerp(top, bottom, f);
                Vertex left = middlex > middle.point.x ? middle : newMiddle;
                Vertex right = middlex < middle.point.x ? middle : newMiddle;
                ////平底
                TriangleD(top, left, right);
                ////平顶
                TriangleT(bottom, left, right);
            }
        }

        public void TriangleD(Vertex top, Vertex down_left, Vertex down_right)
        {
            if (down_left.point.x > down_right.point.x)
            {
                var t = down_left;
                down_left = down_right;
                down_right = t;
            }
            var dxy_left = (down_left.point.x - top.point.x) / (down_left.point.y - top.point.y);
            var dxy_right = (down_right.point.x - top.point.x) / (down_right.point.y - top.point.y);
            float xs = top.point.x, xe = top.point.x;

            for (var y = top.point.y; y <= down_right.point.y; y++)
            {
                if (draw_mode == DrawMode.GouraudShading)
                {
                    var v = (xe - xs == 0) ? 0 : (xe - xs) / (down_right.point.x - down_left.point.x);

                    Color32 left_color = Mathx.Lerp(top.vcolor, down_left.vcolor, v),
                        right_color = Mathx.Lerp(top.vcolor, down_right.vcolor, v);
                    FillLine(left_color, right_color, (int)xs, (int)xe, (int)y, 1);
                }
                else if(draw_mode == DrawMode.Texture)
                {
                    var v = (xe - xs == 0) ? 0 : (xe - xs) / (down_right.point.x - down_left.point.x);
                    var left = Mathx.Lerp(top, down_left, v);
                    var right = Mathx.Lerp(top, down_right, v);
                    VFillLine(left, right, (int)y);
                }
                else
                {
                    Program.main.DrawLine((int)xs, (int)xe, (int)y, currentColor);
                }

                xs += dxy_left;
                xe += dxy_right;
            }
        }

        public void TriangleT(Vertex down, Vertex up_left, Vertex up_right)
        {
            if (up_left.point.x > up_right.point.x)
            {
                var t = up_left;
                up_left = up_right;
                up_right = t;
            }

            var dxy_left = (down.point.x - up_left.point.x) / (down.point.y - up_left.point.y);
            var dxy_right = (down.point.x - up_right.point.x) / (down.point.y - up_right.point.y);
            float xs = up_left.point.x, xe = up_right.point.x;

            for (var y = up_left.point.y; y <= down.point.y; y++)
            {
                if (draw_mode == DrawMode.GouraudShading)
                {
                    var v = (xe - xs == 0) ? 0 : (xe - xs) / (up_right.point.x - up_left.point.x);
                    Color32 left_color = Mathx.Lerp(down.vcolor, up_left.vcolor, v),
                        right_color = Mathx.Lerp(down.vcolor, up_right.vcolor, v);
                    FillLine(left_color, right_color, (int)(xs + 0.5f), (int)(xe + 0.5f), (int)y, 0);
                }
                else if (draw_mode == DrawMode.Texture)
                {
                    var v = (xe - xs == 0) ? 0 : (xe - xs) / (up_right.point.x - up_left.point.x);
                    var left = Mathx.Lerp(down, up_left, v);
                    var right = Mathx.Lerp(down, up_right, v);
                    VFillLine(left, right,(int)y);
                }
                else
                {
                    Program.main.DrawLine((int)xs, (int)xe, (int)y, currentColor);
                }
                xs += dxy_left;
                xe += dxy_right;
            }
        }

        #endregion
        
        #region 摄像机投影
        public Vector2 ToScreen(Vector3 pos) => new Vector2((int)(pos.x * (1 / pos.z) * Width + Width / 2), (int)(pos.y * (1 / pos.z) * Height + Height / 2));
        public Vertex ToScreen(Vertex p) => new Vertex(p) { point = ToScreen(p.point).v3() };
        #endregion


        public Color ReadTexture(float uIndex, float vIndex, Bitmap _texture) => _texture.GetPixel((int)(Mathx.Range(uIndex,0,1)*(_texture.Width-1)), (int)(Mathx.Range(vIndex, 0, 1) *(_texture.Height-1)));

        //public Color ReadTexture(float uIndex, float vIndex, Bitmap _texture)
        //{
        //    var x = uIndex * _texture.Width;
        //    var y = vIndex * _texture.Height;
        //}
    }
}
