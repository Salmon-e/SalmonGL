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
    internal class TestGame : GameWindow
    {
        RenderBatch<Vertex> renderBatch, frameBatch;
        Shader shader, frameShader;
        FrameBuffer frameBuffer;
        struct Vertex
        {
            Vector2 position;
            Vector4 color;
            Vector2 texCoord;
            public Vertex(Vector2 position, Color4 color, Vector2 texCoord)
            {                
                this.position = position;
                this.color = new Vector4(color.R, color.G, color.B, color.A);
                this.texCoord = texCoord;
            }
        }
        Vertex[] verts = {
            new Vertex(new Vector2(-400, 400),  Color4.Red  , new Vector2(0, 1)),
            new Vertex(new Vector2(400, 400),   Color4.White, new Vector2(1, 1)),
            new Vertex(new Vector2(-400, -400), Color4.White, new Vector2(0, 0)),
            new Vertex(new Vector2(-400, -400), Color4.White, new Vector2(0, 0)),
            new Vertex(new Vector2(400, -400),  Color4.Green, new Vector2(1, 0)),
            new Vertex(new Vector2(400, 400),   Color4.White, new Vector2(1, 1))
        };        
        public TestGame() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            
        }
        protected override void OnLoad()
        {
            base.OnLoad();           
            RenderTime = 1f / 60;
            shader = new Shader("vert.glsl", "frag.glsl");
            frameShader = new Shader("vert.glsl", "frag.glsl");
            renderBatch = new RenderBatch<Vertex>(shader, new VertexFootprint("f2-position f4-inColor f2-texCoord"), BufferUsageHint.DynamicDraw, true);
            renderBatch.AddVertices(verts);            
            Texture2D texture = new Texture2D("C:/bunny.png");              
            renderBatch.AddTexture(texture);            
            shader.SetUniform("texture0", texture);

            frameBuffer = new FrameBuffer((int)ClientSize.ToVector2().X, (int)ClientSize.ToVector2().Y);
            frameBatch = new RenderBatch<Vertex>(frameShader, new VertexFootprint("f2-position f4-inColor f2-texCoord"), BufferUsageHint.DynamicDraw, true);
            frameBatch.AddVertices(verts);
            frameBatch.AddTexture(frameBuffer.texture);
            frameShader.SetUniform("texture0", frameBuffer.texture);

            GL.ClearColor(Color4.CornflowerBlue);
        }              
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            shader.SetUniform("resolution", e.Size.ToVector2());
            frameShader.SetUniform("resolution", e.Size.ToVector2());

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            frameBuffer.Enable();
            renderBatch.Render(false);
            frameBuffer.Disable();

            frameBatch.Render(false);
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
