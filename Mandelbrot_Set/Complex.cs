using System;
namespace Mandelbrot_Set
{
    public class Complex
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Complex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Complex() { }

        public Complex Square()
        {
            return new Complex(X*X - Y*Y, X*Y*2);
        }

        public Complex Cube()
        {
            //(x+yi)^3 = xxx + 3xxyi - 3xyy - yyyi
            return new Complex(X*X*X-3*X*Y*Y, 3*X*X*Y-Y*Y*Y);
        }

        public Complex Conjugate()
        {
            return new Complex(X, -Y);
        }

        public double Abs()
        {
            return Math.Sqrt(X*X + Y*Y);
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
