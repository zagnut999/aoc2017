using System;
using System.Collections.Generic;
using Xunit;

namespace AdventOfCode
{

    public class PointTests
    {
        [Fact]
        public void First()
        {
            var p1 = new Point(-1,-1);
            var p2 = new Point(-1,-1);

            Assert.Equal(p1, p2);
        }

        [Fact]
        public void Second()
        {
            var p1 = new Point(-1,-1);
            var p2 = new Point(-1,-1);

            Assert.True(p1 == p2);
        }

        [Fact]
        public void Thrid()
        {
            var p1 = new Point(-1,-1);
            var p2 = new Point(-1,-1);
            var dict = new Dictionary<Point, bool>();
            dict.Add(p1, true);

            Assert.True(dict.ContainsKey(p2));
        }

        [Fact]
        public void Forth()
        {
            var p1 = new Point(-1,-1);
            var p2 = new Point(-1,-1);
            var dict = new Dictionary<Point, bool>();
            dict.Add(p1, true);

            Exception ex = Assert.Throws<ArgumentException>(() => dict.Add(p1, false));

            Assert.Equal("An item with the same key has already been added. Key: (-1,-1)", ex.Message);
        }
    }

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

        public static bool operator == (Point p1, Point p2)
        {
            if (null == (object)p1)
                return (null == (object)p2);

            return p1.Equals(p2);
        }

        public static bool operator != (Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }
}