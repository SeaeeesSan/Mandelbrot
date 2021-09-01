namespace Mandelbrot_Set
{
    public class Camera
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double FoV { get; set; }

        public Camera()
        {
            X = 1.25;
            Y = 1.25;
            FoV = 2.5;
        }

        public Camera(double x, double y, double foV)
        {
            X = x;
            Y = y;
            FoV = foV;
        }

        public Camera(double x, double y)
        {
            X = x;
            Y = y;
            FoV = 2.5;
        }
    }
}
