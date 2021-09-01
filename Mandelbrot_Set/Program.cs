using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Mandelbrot_Set
{
    static class Program
    {
        private const int Resolution = 1000;
        private const int Limit = 100;

        static void Main(string[] args)
        {
            var pos = new Vector2(-0.5f, 0.0f);
            var result = CalcImage(Resolution, Limit, pos, 2);
            result.Save($@"D:\mandel.png", ImageFormat.Png);
        }

        private static Bitmap CalcImage(int resolution, int limit, Vector2 position, double foV)
        {
            var bitmap = new Bitmap(resolution, resolution);
            var bmpData = bitmap.LockBits(
                new Rectangle(0, 0, resolution, resolution),
                ImageLockMode.ReadOnly,
                bitmap.PixelFormat
            );

            var ptr = bmpData.Scan0;
            var pixels = new byte[bmpData.Stride*bitmap.Height];
            Marshal.Copy(ptr, pixels, 0, pixels.Length);

            Parallel.For(0, resolution, i =>
            {
                Parallel.For(0, resolution, j =>
                {
                    var c = new Complex((double)i / Resolution * foV + position.X - foV / 2, (double)j / Resolution * foV - position.Y - foV / 2);
                    var col = Gradation.GetColor(CalcMandelbrot(c,limit), limit);
                    var pos = j * bmpData.Stride + i * 4;

                    pixels[pos] = col.B;
                    pixels[pos + 1] = col.G;
                    pixels[pos + 2] = col.R;
                    pixels[pos + 3] = col.A;
                });
            });

            Marshal.Copy(pixels, 0, ptr, pixels.Length);
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }

        private static int CalcMandelbrot(Complex cComplex, int limit)
        {
            var zComplex = Complex.Zero;
            for (int i = 0; i < limit; i++)
            {
                zComplex = Complex.Pow(zComplex, 2) + cComplex;
                //発散
                if (zComplex.Magnitude > 2.0) return i;
            }
            //発散しない
            return -1;
        }

        private static int CalcJulia(Complex zComplex, Complex cComplex, int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                zComplex = Complex.Pow(zComplex, 2) + cComplex;
                //発散
                if (zComplex.Magnitude > 2.0) return i;
            }
            //発散しない
            return -1;
        }

        private static int CalcTricorn(Complex cComplex, int limit)
        {
            var zComplex = Complex.Zero;
            for (int i = 0; i < limit; i++)
            {
                zComplex = Complex.Pow(Complex.Conjugate(zComplex), 2) + cComplex;
                //発散
                if (zComplex.Magnitude > 2.0) return i;
            }
            //発散しない
            return -1;
        }
    }
}
