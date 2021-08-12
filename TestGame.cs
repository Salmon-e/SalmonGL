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
        RenderBatch<PosColor> renderBatch;
        Shader shader;
        struct PosColor
        {
            Vector2 position;
            Vector4 color;            
            public PosColor(Vector2 position, Color4 color)
            {                
                this.position = position + new Vector2(50, 50);
                this.color = new Vector4(color.R, color.G, color.B, color.A);                
            }
        }
        PosColor[] verts = {
            new PosColor(new Vector2(-100, 100),  Color4.Red),
            new PosColor(new Vector2(100, 100),   Color4.White),
            new PosColor(new Vector2(-100, -100), Color4.White),
            new PosColor(new Vector2(-100, -100), Color4.White),
            new PosColor(new Vector2(100, -100),  Color4.Green),
            new PosColor(new Vector2(100, 100),   Color4.White)
        };
        
        public TestGame() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {

        }
        protected override void OnLoad()
        {
            base.OnLoad();
            RenderTime = 1f / 60;
            shader = new Shader("vert.glsl", "frag.glsl");            
            renderBatch = new RenderBatch<PosColor>(shader, new VertexFootprint("f2-position f4-inColor"), BufferUsageHint.DynamicDraw, true);
            renderBatch.AddVertices(verts);            
            
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

            renderBatch.Render(false);
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
