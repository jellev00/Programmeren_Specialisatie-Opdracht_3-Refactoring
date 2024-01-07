namespace EscapeFromTheWoods.Objects
{
    public class MapBoundary
    {
        public MapBoundary(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public int DeltaX { get => MaxX - MinX; }
        public int DeltaY { get => MaxY - MinY; }

        public bool WithinBounds(double x, double y)
        {
            if ((x < MinX) || (x > MaxX) || (y < MinY) || (y > MaxY))
            {
                return false;
            }
            return true;
        }
    }
}
