using System;
using System.Drawing;

namespace RuneGear.Utilities.Extensions
{
    public static class ColorExtensions
    {
        private static readonly Random rnd;

        static ColorExtensions()
        {
            rnd = new Random();
        }

        /// <summary>
        /// Get a random color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color GetRandomColor(this Color color)
        {
            return Color.FromArgb(255, rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
        }

        /// <summary>
        /// Brush colours only vary from shades of green and blue
        /// </summary>
        public static Color GetRandomBrushColor(this Color color)
        {
            return Color.FromArgb(255, 0, rnd.Next(128, 256), rnd.Next(128, 256));
        }

        /// <summary>
        /// Group colours only vary from shades of green and red
        /// </summary>
        public static Color GetRandomGroupColor(this Color color)
        {
            return Color.FromArgb(255, rnd.Next(128, 256), rnd.Next(128, 256), 0);
        }

        /// <summary>
        /// Get a random light color
        /// </summary>
        public static Color GetRandomLightColor(this Color color)
        {
            return Color.FromArgb(255, rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));
        }

        /// <summary>
        /// Get a random dark color
        /// </summary>
        public static Color GetRandomDarkColor(this Color color)
        {
            return Color.FromArgb(255, rnd.Next(0, 128), rnd.Next(0, 128), rnd.Next(0, 128));
        }
    }
}
