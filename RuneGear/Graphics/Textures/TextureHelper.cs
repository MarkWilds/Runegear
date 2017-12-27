using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using RuneGear.General;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace RuneGear.Graphics.Textures
{
    public static class Texture2DHelper
    {
        public static Texture2D ToTexture2D(this TextureItem textureItem)
        {
            Bitmap image = new Bitmap(textureItem.FilePath);
            Texture2D newTexture = new Texture2D(textureItem.Id)
            {
                Width = image.Width,
                Height = image.Height
            };
            newTexture.Bind();

            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);
            image.Dispose();
            SetDefaultTextureParameters();

            newTexture.Unbind();

            return newTexture;
        }

        private static void SetDefaultTextureParameters()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }
    }
}
