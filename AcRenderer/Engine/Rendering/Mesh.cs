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

    }
}