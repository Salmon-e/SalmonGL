using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Linq;

namespace SalmonGL
{
    public class TestGame : GameWindow
    {
        VertexBuffer buffer;
        Shader shader;
        Vector2[] verts = {
            new Vector2(-100, 100),
            new Vector2(100, 100),
            new Vector2(-100, -100),
            new Vector2(-100, -100),
            new Vector2(100, -100),
            new Vector2(100, 100)
        };
        
        public TestGame() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {

        }
        protected override void OnLoad()
        {
            base.OnLoad();
            Random r = new Random();            
            shader = new Shader("vert.glsl", "frag.glsl");
            shader.SetUniform("resolution", Size.ToVector2());
            buffer = new VertexBuffer(new VertexFootprint("f2-position"), BufferUsageHint.StaticDraw);
            buffer.BufferData(verts, true);            
            buffer.VertexAttribPointers(shader);
            GL.ClearColor(Color4.CornflowerBlue);
        }        
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);           
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            shader.SetUniform("resolution", e.Size.ToVector2());
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Activate();
            buffer.Enable();
            GL.DrawElements(PrimitiveType.Triangles, buffer.indices.Length, DrawElementsType.UnsignedInt, 0);
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
