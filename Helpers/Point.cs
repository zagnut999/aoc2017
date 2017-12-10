namespace AdventOfCode
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point()
        {

        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point Clone()
        {
            return new Point(X, Y);
        }

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            if (point == null)
                return false;

            return point.X == this.X && point.Y == this.Y;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();
            return hash;
        }
        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}