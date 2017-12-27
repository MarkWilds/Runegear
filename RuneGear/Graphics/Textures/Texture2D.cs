using System;
using OpenTK.Graphics.OpenGL4;

namespace RuneGear.Graphics.Textures
{
    public class Texture2D : IDisposable
    {
        public Texture2D(string identifier, bool generateId = true)
        {
            Identifier = identifier;

            if(generateId)
                GlIdentifier = GL.GenTexture();
        }

        public string Identifier { get; private set; }

        public int GlIdentifier { get;}

        public int Width { get; set; }

        public int Height { get; set; }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D,GlIdentifier);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Dispose()
        {
            GL.DeleteTexture(GlIdentifier);
        }
    }
}
