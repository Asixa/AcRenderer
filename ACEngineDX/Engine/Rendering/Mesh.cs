using System;
using ACEngine.Math;
using ObjModelLoader;
using Random = ACEngine.Math.Random;

namespace ACEngine.Engine.Rendering
{
    public class Mesh
    {
        public Vertex[] vertices;
        public Mesh(Model model)
        {
            vertices = new Vertex[model.indexs.Count];
            for (var i = 0; i < model.indexs.Count; i++)
            {
                var index = model.indexs[i];
                var point = model.points[index];
                vertices[i] = new Vertex(point, model.norlmas[i], model.uvs[i].x, model.uvs[i].y,model.vertColors[index]);
            }
        }
        public Mesh(ObjMesh model)
        {
            vertices = new Vertex[model.TriangleArray.Length];
            for (var i = 0; i < model.TriangleArray.Length; i++)
            {
                var color = Math.Random.Range(0, 255) / 255f;
                var index = model.TriangleArray[i];
                var point = model.VertexArray[index];
                vertices[i] = new Vertex(Vector3.FromObj(point),
                    Vector3.FromObj(model.NormalArray[index]),
                    model.UVArray[index].x,
                    model.UVArray[index].y, new Vector3(color, color, color));
            }
        }
    }
}