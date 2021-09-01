using System;
namespace Mandelbrot_Set
{
    public class Complex
    {
        private double X { get; set; }
        private double Y { get; set; }

        public Complex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Complex Square()
        {
            return new Complex(X * X - Y * Y, X * Y * 2);
        }

        public Complex Conjugate()
        {
            return new Complex(X, -Y);
        }

        public double Abs()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public static Complex operator + (Complex z, Complex w)
        {
            return new Complex(z.X + w.X, z.Y + w.Y);
        }

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
