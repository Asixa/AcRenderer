using System;
using System.Drawing;
using ACEngine;
using ACEngine.Engine;
using ACEngine.Engine.Rendering;
using ACEngine.Engine.Scene;
using ACEngine.Math;
using ACEngineDX.Engine.Rendering.Renderer;

namespace ACEngineDX.Engine
{
    public class MeshRenderer :Renderer
    {
        public GameObject gameObject;
        public Transform transform => gameObject.transform;
        public Mesh mesh;
        public Material material;

        public MeshRenderer(Mesh m,GameObject g)
        {
            mesh = m;
            gameObject = g;
        }

        public override void Render()
        {
            var canvas = Program.main.canvas;
            CaculateCameraTransform();
            if ( transform.positionToCamera.z <= 0) return;
            var c = 1;
            for (var i = 0; i + 2 < mesh.vertices.Length; i += 3)
            {
                c++;
                if (c > 8) c = 2;
                canvas.currentColor = Color.FromArgb(100 + ((int)(c / 2)) * 20, 100 + ((int)(c / 2)) * 20,
                    100 + ((int)(c / 2)) * 20);
                canvas.DrawTriangle(this, i);
            }
        }

        public Vertex GetCameraPosition(int i)=>new Vertex(mesh.vertices[i])
            {
                point = GetRelativePositionToVertex(mesh.vertices[i].point,
                    transform.positionToCamera,
                    transform.RotationToCamera)
            };

        public void CaculateCameraTransform()
        {
            var camera = SceneManager.Current.main_camera.transform;
            transform.positionToCamera = GetRelativePosition(transform.position - camera.position, camera.position, camera.rotation, true);
            transform.RotationToCamera = transform.rotation +camera.rotation;
        }

    

    }

    public struct Triangle
    {
        public int p1, p2,p3;
        public Triangle(int ll1, int ll2,int ll3)
        {
            p1 = ll1;
            p2 = ll2;
            p3 = ll3;
        }
    }
}
