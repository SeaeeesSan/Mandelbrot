using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Mandelbrot_Set
{
    static class Program
    {
        private static int _resolution = 5000;
        private static int _limit = 100;

        static void Main(string[] args)
        {
            Camera cam = new Camera(0, 0, 3);
            Bitmap result = CalcImage(_resolution,_limit,cam);
            result.Save($@"D:\man.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private static Color GetColor(int i)
        {
            if(i == -1) return Color.Black;
            double n = (double)i / _limit;
            Color c1 = Color.Orange;
            Color c2 = Color.Blue;

            int r = ((int)((c2.R - c1.R)*n + c1.R)).Round(255,0);
            int g = ((int)((c2.G - c1.G)*n + c1.G)).Round(255,0);
            int b = ((int)((c2.B - c1.B)*n + c1.B)).Round(255,0);
            //r = r > 255 ? r : 255;

            return Color.FromArgb(r,g,b);
        }

        private static int Round(this int target,int max,int min)
        {
            if (target > max) target = max;
            if (target < min) target = min;
            return target;
        }

        static Bitmap CalcImage(int resolution, int limit, Camera camera)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Bitmap bitmap = new Bitmap(resolution,resolution);

            PixelFormat pixelFormat = PixelFormat.Format32bppRgb;
            int pixelSize = 3;
            BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, resolution, resolution),
                ImageLockMode.ReadOnly,
                bitmap.PixelFormat
            );

            if (bmpData.Stride < 0)
            {
                bitmap.UnlockBits(bmpData);
                throw new Exception();
            }

            IntPtr ptr = bmpData.Scan0;
            byte[] pixels = new byte[bmpData.Stride * bitmap.Height];
            Marshal.Copy(ptr, pixels, 0, pixels.Length);

            //for (int i = 0; i < _resolution; i++){
            Parallel.For(0,resolution, i =>
            {
                Parallel.For(0, resolution, j =>
                {
                    Complex c = new Complex((((double)i/_resolution)*camera.FoV) + camera.X - camera.FoV/2, (((double)j/_resolution)*camera.FoV) - camera.Y - camera.FoV/2);
                    Color col = GetColor(c.CalcJulia(limit));
                    int pos = j*bmpData.Stride + i*4;
                    pixels[pos] = col.B;
                    pixels[pos + 1] = col.G;
                    pixels[pos + 2] = col.R;
                    pixels[pos + 3] = 255;
                });
            });
            /*
            for (int i = 0; i < _resolution; i++)
            {
                for (int j = 0; j < _resolution; j++)
                {
                    //Complex c = new Complex((((double)i / _resolution) * 2.5) - 1.8, (((double)j / _resolution) * 2.5) - 1.25);
                    Complex c = new Complex((((double)i / _resolution) * camera.FoV) + camera.X - camera.FoV / 2 , (((double)j / _resolution) * camera.FoV) - camera.Y - camera.FoV / 2);
                    result.SetPixel(i,j,GetColor(c.CalcMandelbrot(_limit)));
                }
            }
            */
            //return result;

            System.Runtime.InteropServices.Marshal.Copy(pixels,0,ptr,pixels.Length);
            bitmap.UnlockBits(bmpData);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            return bitmap;
        }

        static int CalcJulia(this Complex zComplex, int limit)
        {
            //-0.747593803839+0.083586811697 i
            Complex cComplex = new Complex(-0.747593803839,0.083586811697);
            for (int i = 0; i < limit; i++)
            {
                zComplex = zComplex.Square() + cComplex;
                //発散
                if (zComplex.Abs() > 2.0) return i;
            }
            //発散しない
            return -1;
        }

        static int CalcMandelbrot(this Complex cComplex, int limit)
        {
            //-0.747593803839+0.083586811697 i
            Complex zComplex = new Complex(0,0);
            for (int i = 0; i < limit; i++)
            {
                zComplex = zComplex.Square() + cComplex;
                if (zComplex.Abs() > 2.0) return i;
            }
            //発散しない
            return -1;
        }

        static int CalcMultibrot(this Complex cComplex, int limit)
        {
            //-0.747593803839+0.083586811697 i
            Complex zComplex = new Complex(0,0);
            for (int i = 0; i < limit; i++)
            {
                zComplex = zComplex.Cube() + cComplex;
                if (zComplex.Abs() > 2.0) return i;
            }
            //発散しない
            return -1;
        }

        static int CalcTricorn(this Complex cComplex, int limit)
        {
            //-0.747593803839+0.083586811697 i
            Complex zComplex = new Complex(0,0);
            for (int i = 0; i < limit; i++)
            {
                zComplex = zComplex.Conjugate().Square() + cComplex;
                if (zComplex.Abs() > 2.0) return i;
            }
            //発散しない
            return -1;
        }
    }
}
