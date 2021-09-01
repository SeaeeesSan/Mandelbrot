using System.Drawing;
using System.Drawing.Imaging;
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
            var cam = new Camera(-0.5, 0, 2);
            var result = CalcImage(Resolution, Limit, cam);
            result.Save($@"D:\mandel.png", ImageFormat.Png);
        }

        private static Bitmap CalcImage(int resolution, int limit, Camera camera)
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
                    var c = new Complex((double)i / Resolution * camera.FoV + camera.X - camera.FoV / 2, (double)j / Resolution * camera.FoV - camera.Y - camera.FoV / 2);
                    var col = Utils.GetColor(CalcMandelbrot(c,limit), limit);
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
            Complex zComplex = new Complex(0, 0);
            for (int i = 0; i < limit; i++)
            {
                zComplex = zComplex.Square() + cComplex;
                //発散
                if (zComplex.Abs() > 2.0) return i;
            }
            //発散しない
            return -1;
        }

        private static int CalcJulia(Complex zComplex, Complex cComplex, int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                zComplex = zComplex.Square() + cComplex;
                //発散
                if (zComplex.Abs() > 2.0) return i;
            }
            //発散しない
            return -1;
        }

        private static int CalcTricorn(Complex cComplex, int limit)
        {
            Complex zComplex = new Complex(0, 0);
            for (int i = 0; i < limit; i++)
            {
                zComplex = zComplex.Conjugate().Square() + cComplex;
                //発散
                if (zComplex.Abs() > 2.0) return i;
            }
            //発散しない
            return -1;
        }
    }
}
