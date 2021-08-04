using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace SalmonGL
{
    public class TestGame : GameWindow
    {
        VertexBuffer buffer;
        Shader shader;
        Vector3[] verts =
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 1)
        };
        public TestGame() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {

        }
        protected override void OnLoad()
        {
            base.OnLoad();
            shader = new Shader("vert.glsl", "frag.glsl");
            buffer = new VertexBuffer(new VertexFootprint("f3-position"), BufferUsageHint.StaticDraw);
            buffer.BufferData(verts);
            buffer.VertexAttribPointers(shader);
            GL.ClearColor(Color4.CornflowerBlue);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Activate();
            buffer.Enable();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
