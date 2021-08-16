using System;

namespace SalmonGL
{
    class Program
    {
        static void Main(string[] args)
        {
            TextureAtlas atlas = new TextureAtlas();
            atlas.AddImage("green.png", "green");
            atlas.AddImage("blue.png", "blue");           
            atlas.AddImage("red.png", "red");

            atlas.CreateAtlas();
            TestGame game = new TestGame();
            game.Run();
           
        }
    }
}
