using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace SalmonGL
{
    public class VertexBuffer
    {
        private int _vbo;
        private int _vao;
        public BufferUsageHint usageHint;
        public VertexFootprint footprint;
        public VertexBuffer(VertexFootprint footprint, BufferUsageHint usageHint)
        {
            _vbo = GL.GenBuffer();
            _vao = GL.GenVertexArray();
            this.footprint = footprint;
            this.usageHint = usageHint;            
        }
        public void VertexAttribPointers(Shader shader)
        {
            GL.BindVertexArray(_vao);
            for(int i = 0; i < footprint.types.Length; i++)
            {
                int location = GL.GetAttribLocation(shader.Program, footprint.names[i]);
                GL.VertexAttribPointer(location, footprint.counts[i], footprint.types[i], false, 0, 0);
                GL.EnableVertexAttribArray(location);
            }
        }
        public void Enable()
        {
            GL.BindVertexArray(_vao);
        }
        public void BufferData<T>(T[] vertices, int vertexCount = -1) where T : struct
        {
            if (vertexCount == -1) vertexCount = vertices.Length;
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, footprint.size * vertexCount, vertices, usageHint);
        }
    }
}
