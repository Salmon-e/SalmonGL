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
        RenderBatch<Vertex> renderBatch;
        Shader shader;
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
            TextureAtlas atlas = new TextureAtlas();
            atlas.AddImage("green.png", "green");
            atlas.AddImage("blue.png", "blue");
            atlas.AddImage("red.png", "red");

            atlas.CreateAtlas();
            RenderTime = 1f / 60;
            shader = new Shader("vert.glsl", "frag.glsl");            
            renderBatch = new RenderBatch<Vertex>(shader, new VertexFootprint("f2-position f4-inColor f2-texCoord"), BufferUsageHint.DynamicDraw, true);
            renderBatch.AddVertices(verts);            
            Texture2D texture = new Texture2D("C:/bunny.png");   
            Texture2D texture2 = atlas.texture;
            renderBatch.AddTexture(texture);
            renderBatch.AddTexture(texture2);
            shader.SetUniform("texture0", texture);
            shader.SetUniform("texture1", texture2);
            GL.ClearColor(Color4.CornflowerBlue);
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
