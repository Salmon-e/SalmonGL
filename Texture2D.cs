using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace SalmonGL
{
    public class Texture2D
    {
        public int texture;
        public TextureUnit currentUnit = TextureUnit.Texture0;        
        public Texture2D(string path, TextureWrapMode wrap = TextureWrapMode.Repeat, TextureMagFilter magfilter = TextureMagFilter.Nearest, TextureMinFilter minFilter = TextureMinFilter.Nearest) : this(wrap, magfilter, minFilter)
        {
            LoadImage(Image.Load<Rgba32>(path));
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        public Texture2D(TextureWrapMode wrap = TextureWrapMode.Repeat, TextureMagFilter magfilter = TextureMagFilter.Nearest, TextureMinFilter minFilter = TextureMinFilter.Nearest)
        {
            texture = GL.GenTexture();
            Enable();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magfilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrap);
        }
        public void WriteBytes(byte[] data, int width, int height, PixelInternalFormat internalFormat, PixelFormat format, PixelType type)
        {
            Enable();
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, format, type, data);
        }
        public void LoadImage(Image<Rgba32> image)
        {
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            List<byte> data = new List<byte>();

            for (int y = 0; y < image.Height; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (int x = 0; x < image.Width; x++)
                {
                    data.AddRange(new byte[] { row[x].R, row[x].G, row[x].B, row[x].A });
                }
            }
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data.ToArray());
        }        
        public void Enable(TextureUnit unit = TextureUnit.Texture0)
        {            
            GL.ActiveTexture(unit);
            currentUnit = unit;
            GL.BindTexture(TextureTarget.Texture2D, texture);           
        }
    }
}
