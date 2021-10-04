using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Mathematics;
namespace SalmonGL
{
    public class FrameBuffer
    {
        public Texture2D texture;
        int _fbo;
        public FrameBuffer(int width, int height)
        {
            _fbo = GL.GenFramebuffer();
            texture = new Texture2D();
            texture.WriteBytes(null, width, height, PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte);
            Enable();
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture.texture, 0);
            Disable();
        }
        public void Enable()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        }
        public void Disable()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }        
    }
}
