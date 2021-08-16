using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System.Linq;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace SalmonGL
{
    public class TextureAtlas
    {
        Texture2D texture;

        List<NamedImage> images = new List<NamedImage>();
        
        public Dictionary<string, TextureRegion> regions;
        public void AddImage(string path, string name)
        {
            images.Add(new NamedImage(name, Image.Load<Rgba32>(path)));
            
        }
        public void AddImage(Image<Rgba32> image, string name) 
        { 
            images.Add(new NamedImage(name, image));
            
        } 
        public void CreateAtlas()
        {
            int layers = (int)Math.Ceiling(Math.Sqrt(images.Count));
            var sorted = new List<NamedImage>[layers];
            for (int i = 0; i < layers; i++)
            {
                sorted[i] = new List<NamedImage>();
            }
            images = images.OrderBy(i => i.image.Height).ToList();
            
            int k = 0;
            
            for(int i = 0; i < layers; i++)
            {
                for(int j = 0; j < layers && k < images.Count; j++, k++)
                {
                    sorted[i].Add(images[k]);
                }
            }
            var offsets = new Dictionary<string, Vector2>();
            int y = 0;
            int x;
            int maxX = 0;
            for(int i = 0; i < layers; i++)
            {
                x = 0;
                int maxHeight = 0;
                foreach(NamedImage image in sorted[i])
                {
                    offsets.Add(image.name, new Vector2(x, y));
                    x += image.image.Width;
                    if (image.image.Height > maxHeight) maxHeight = image.image.Height;
                }
                if (x > maxX) maxX = x;
                y += maxHeight;
            }
            var atlas = new Image<Rgba32>(maxX, y);
            var offsetArray = offsets.Values.ToArray();
            for(int i = 0; i < images.Count; i++)
            {
                Point offset = new Point((int)offsetArray[i].X, (int)offsetArray[i].Y);
                atlas.Mutate(im => im.DrawImage(images[i].image, offset, 1));
            }            
            Vector2 atlasSize = new Vector2(atlas.Width, atlas.Height);
            foreach(NamedImage image in images)
            {
                Vector2 UL = Vector2.Divide(offsets[image.name], atlasSize);
                Vector2 BR = UL + Vector2.Divide(new Vector2(image.image.Width, image.image.Height), atlasSize);
                regions.Add(image.name, new TextureRegion(UL, BR));
            }

            texture = new Texture2D();
            texture.LoadImage(atlas);            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        private class NamedImage
        {
            public string name;
            public Image<Rgba32> image;
            public NamedImage(string name, Image<Rgba32> image)
            {
                this.name = name;
                this.image = image;
            }
        }
    }
    public struct TextureRegion
    {
        public Vector2 UL;
        public Vector2 BR;
        public Vector2 UR => new Vector2(UL.Y, BR.X);
        public Vector2 BL => new Vector2(UL.X, BR.Y);

        public TextureRegion(Vector2 ul, Vector2 br)
        {
            UL = ul;
            BR = br;
        }
        public Vector2[] Corners { 
            get
            {
                return new Vector2[] { UL, UR, BR, BL };
            } 
        }
    }
}
