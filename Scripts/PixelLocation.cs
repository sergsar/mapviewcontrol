namespace MapViewScripts
{
    public class PixelLocation
    {
        public int X { get; set; }
        public int Z { get; set; }

        public PixelLocation() { }

        public PixelLocation(int x, int z)
        {
            X = x;
            Z = z;
        }

        public static PixelLocation operator +(PixelLocation a, PixelLocation b)
        {
            return new PixelLocation(a.X + b.X, a.Z + b.Z);
        }
        public static PixelLocation operator -(PixelLocation a, PixelLocation b)
        {
            return new PixelLocation(a.X - b.X, a.Z - b.Z);
        }

        public static PixelLocation operator *(PixelLocation a, int d)
        {
            return new PixelLocation(a.X * d, a.Z * d);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Z);
        }
    }
}