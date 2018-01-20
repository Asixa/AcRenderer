using System;
using System.Drawing;
using ACEngine.Engine;
using ACEngine.Engine.Rendering;
using ACEngine.Engine.Scene;
using ACEngine.Math;
using ACEngineDX;

namespace ACEngine
{
    public class Canvas
    {
        public enum DrawMode
        {
            Wireframe,
            WireframeWithoutCulling,
            GouraudShading,
            Tex_Wire,
            NoShading
        }

        public bool FlipNormal = false;
        public DrawMode draw_mode = DrawMode.Wireframe;
        private static int Width => 512;
        private static int Height => 512;
        private Color currentColor;


        public void SetPixel(int x, int y, Color color) => Program.main.SetPixel(x, y, color);

        public void Draw()
        {
            foreach (var t in SceneManager.Current.ObjectInScene)
            {
                if (t.meshRenderer == null) continue;
                var renderer = t.meshRenderer;
                renderer.CaculateCameraTransform();
       
                if(renderer.transform.positionToCamera.z<=0)continue;
                var c = 1;
                for (var i = 0; i + 2 < renderer.mesh.vertices.Length; i += 3)
                {
                    c++;
                    if (c > 27) c = 2;
                    currentColor = Color.FromArgb(100 + ((int) (c / 2)) * 10, 100 + ((int) (c / 2)) * 10,
                        100 + ((int) (c / 2)) * 10);
                    DrawTriangle(renderer, i);
                }
            }
        }
    

        private bool BackFaceCulling(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            if (draw_mode == DrawMode.WireframeWithoutCulling)return true;
            var camera_position = SceneManager.Current.main_camera.transform.position;
            var v1 = p2 - p1;
            var v2 = p3 - p2;
            var normal = Vector3.Cross(v1, v2);
            var view_dir = p1 - camera_position; 
            return FlipNormal? Vector3.Dot(normal, view_dir) <= 0: Vector3.Dot(normal, view_dir) > 0;
        }

        public Vector2 ToScreen(Vector3 pos) => new Vector2((int)(pos.x * (1 / pos.z) * Width + Width/2), (int)(pos.y * (1 / pos.z) * Height + Height/2));
        public Vertex ToScreen(Vertex p)
        {

            var result = ToScreen(p.point);
            return new Vertex(p){point=new Vector3(result.x, result.y,0)};
        }

        public void FillLine_Gouraud(Color32 lc, Color32 rc, int xs, int xe, int y,int debug)

        {   float dx = System.Math.Abs(xe - xs);
            for (var x = xs; x <= xe; x ++)
            {
                if (x >= Width || y >= Height || x <= 0 || y <= 0)
                {
                    continue;
                };
                    var factor = System.Math.Abs((x - xs + 1) / (dx + 1));
                    var color = Mathx.Lerp(lc, rc, factor);
                Program.main.SetPixel(x, y, color.ToColor());
            }
        }

        public void DrawTriangle(MeshRenderer renderer, int i)
        {
            if (!BackFaceCulling(renderer.GetCameraPosition(i).point, renderer.GetCameraPosition(i + 1).point,
                renderer.GetCameraPosition(i + 2).point)) return;
            Triangle(ToScreen(renderer.GetCameraPosition(i)), ToScreen(renderer.GetCameraPosition(i + 1)), ToScreen(renderer.GetCameraPosition(i + 2)));      
        }

        public void Triangle(Vertex p0, Vertex p1, Vertex p2)
        {
            if (draw_mode == DrawMode.Wireframe || draw_mode == DrawMode.WireframeWithoutCulling || draw_mode == DrawMode.Tex_Wire||draw_mode == DrawMode.GouraudShading)
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
                    return;}
                //插值求中间点x
                var middlex = (middle.point.y - top.point.y) * (bottom.point.x - top.point.x) / (bottom.point.y - top.point.y) + top.point.x;
                float dy = middle.point.y - top.point.y;
                float t = dy / (bottom.point.y - top.point.y);//插值生成左右顶点
                Vertex newMiddle = new Vertex(new Vector3(middlex, middle.point.y, 0));
                var f = (top.point.y - middle.point.y) / (top.point.y - bottom.point.y);
                newMiddle.vcolor = Mathx.Lerp(top.vcolor, bottom.vcolor, f);
                Vertex left = middlex > middle.point.x ? middle : newMiddle;
                Vertex right = middlex < middle.point.x ? middle : newMiddle;
                ////平底
                TriangleD(top, left, right);
                ////平顶
                TriangleT(bottom, left, right);
            }
        }

        public void TriangleD(Vertex top,  Vertex down_left, Vertex down_right)
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
                    FillLine_Gouraud(left_color, right_color, (int) xs, (int) xe, (int) y, 1);
                }
                else
                {
                    Program.main.DrawLine((int)xs, (int)xe, (int)y,currentColor);
                }

                xs += dxy_left;
                xe += dxy_right;
            }
        }

        public void TriangleT(Vertex down,Vertex up_left, Vertex up_right)
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
                    FillLine_Gouraud(left_color, right_color, (int) (xs + 0.5f), (int) (xe + 0.5f), (int) y, 0);
                }
                else
                {
                    Program.main.DrawLine((int)xs, (int)xe, (int)y, currentColor);
                }
                xs += dxy_left;
                xe += dxy_right;
            }
        }

    }
}
