using System.Drawing;
namespace Mandelbrot_Set
{
    public static class Utils
    {
        public static Color GetColor(int i, int limit)
        {
            if (i == -1) return Color.Black;
            var d = (double)i / limit;

            var color1 = Color.Orange;
            var color2 = Color.Blue;

            var r = ((int)((color2.R - color1.R) * d + color1.R)).Round();
            var g = ((int)((color2.G - color1.G) * d + color1.G)).Round();
            var b = ((int)((color2.B - color1.B) * d + color1.B)).Round();
            var a = ((int)((color2.A - color1.A) * d + color1.A)).Round();

            return Color.FromArgb(a, r, g, b);
        }

        private static int Round(this int value)
        {
            if (value > 255) value = 255;
            if (value < 0) value = 0;
            return value;
        }
    }
}
