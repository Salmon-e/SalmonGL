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
        private int _ebo = -1;
        public BufferUsageHint usageHint;
        public VertexFootprint footprint;
        public uint[] indices;
        public bool UseEbo => _ebo != -1;
        public VertexBuffer(VertexFootprint footprint, BufferUsageHint usageHint, bool useEbo = true)
        {
            _vbo = GL.GenBuffer();
            _vao = GL.GenVertexArray();
            if (useEbo)
            {
                _ebo = GL.GenBuffer();
            }
            this.footprint = footprint;
            this.usageHint = usageHint;            
        }
        public void VertexAttribPointers(Shader shader)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BindVertexArray(_vao);
            int offset = 0;
            for(int i = 0; i < footprint.types.Length; i++)
            {
                int location = GL.GetAttribLocation(shader.Program, footprint.names[i]);
                GL.VertexAttribPointer(location, footprint.counts[i], footprint.types[i], false, footprint.size, offset);
                GL.EnableVertexAttribArray(location);
                offset += VertexFootprint.TypeSizes[footprint.types[i]] * footprint.counts[i];
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        public void Enable()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            if (_ebo != -1) GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BindVertexArray(_vao);
        }
        public void Disable()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        public void BufferData<T>(T[] vertices, bool collapse = true) where T : struct
        {
            if (collapse && _ebo != -1)
            {
                List<T> copy = new List<T>(vertices);
                List<T> trimmed = new List<T>();
                List<uint> indices = new List<uint>();
                foreach(T v in copy)
                {
                    if (!trimmed.Contains(v))
                    {
                        trimmed.Add(v);
                    }
                }
                foreach(T v in copy)
                {
                    indices.Add((uint)trimmed.IndexOf(v));
                }
                this.indices = indices.ToArray();
                vertices = trimmed.ToArray();                
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), usageHint);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
            int vertexCount = vertices.Length;
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, footprint.size * vertexCount, vertices, usageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void BufferData<T>(T[] vertices, uint[] indices, int vertexCount = -1) where T : struct
        {
            if (vertexCount == -1) vertexCount = vertices.Length;

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, footprint.size * vertexCount, vertices, usageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, usageHint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}
