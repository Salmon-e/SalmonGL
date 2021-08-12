using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
namespace SalmonGL
{
    public class RenderBatch<T> where T : struct
    {
        public VertexBuffer vertexBuffer;
        public Shader shader;
        bool _hasBuffered = false;
        public delegate void OnRebuffer(RenderBatch<T> batch);
        OnRebuffer onRebuffer = (batch) => { };
        public T[] vertices;
        private List<T> queuedVertices;

        public RenderBatch(Shader shader, VertexFootprint footprint, BufferUsageHint usageHint, bool useEbo = true)
        {
            vertexBuffer = new VertexBuffer(footprint, usageHint, useEbo);
            this.shader = shader;
            queuedVertices = new List<T>();
        }
        public void AddVertices(T[] newVertices)
        {
            queuedVertices.AddRange(newVertices);
        }
        public void Rebuffer(bool collapse = true)
        {
            vertices = queuedVertices.ToArray();
            queuedVertices.Clear();
            vertexBuffer.BufferData(vertices, collapse);
            vertexBuffer.VertexAttribPointers(shader);
            _hasBuffered = true;
            onRebuffer(this);
        }
        public void Render(bool rebuffer = true)
        {
            if (rebuffer || !_hasBuffered) Rebuffer();
            shader.Enable();
            vertexBuffer.Enable();

            if (vertexBuffer.UseEbo)
            {
                GL.DrawElements(PrimitiveType.Triangles, vertexBuffer.indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            }
        }
    }
}
