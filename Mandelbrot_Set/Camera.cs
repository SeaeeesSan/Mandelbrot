namespace Mandelbrot_Set
{
    public class Camera
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double FoV { get; private set; }

        public Camera(double x, double y, double foV)
        {
            X = x;
            Y = y;
            FoV = foV;
        }
    }
}
