using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day03x2
    {
        /*        
        --- Day 3: Spiral Memory ---

        You come across an experimental new kind of memory stored on an infinite two-dimensional grid.

        Each square on the grid is allocated in a spiral pattern starting at a location marked 1 and then counting up while spiraling outward. For example, the first few squares are allocated like this:

        17  16  15  14  13
        18   5   4   3  12
        19   6   1   2  11
        20   7   8   9  10
        21  22  23---> ...
        While this is very space-efficient (no squares are skipped), requested data must be carried back to square 1 (the location of the only access port for this memory system) by programs that can only move up, down, left, or right. They always take the shortest path: the Manhattan Distance between the location of the data and square 1.

        For example:

        Data from square 1 is carried 0 steps, since it's at the access port.
        Data from square 12 is carried 3 steps, such as: down, left, left.
        Data from square 23 is carried only 2 steps: up twice.
        Data from square 1024 must be carried 31 steps.
        How many steps are required to carry the data from the square identified in your puzzle input all the way to the access port?

        Your puzzle input is 265149.

        --- Part Two ---

        As a stress test on the system, the programs here clear the grid and then store the value 1 in square 1. Then, in the same allocation order as shown above, they store the sum of the values in all adjacent squares, including diagonals.

        So, the first few squares' values are chosen as follows:

        Square 1 starts with the value 1.
        Square 2 has only one adjacent filled square (with value 1), so it also stores 1.
        Square 3 has both of the above squares as neighbors and stores the sum of their values, 2.
        Square 4 has all three of the aforementioned squares as neighbors and stores the sum of their values, 4.
        Square 5 only has the first and fourth squares as neighbors, so it gets the value 5.
        Once a square is written, its value does not change. Therefore, the first few squares would receive the following values:

        147  142  133  122   59
        304    5    4    2   57
        330   10    1    1   54
        351   11   23   25   26
        362  747  806--->   ...
        What is the first value written that is larger than your puzzle input?

        Your puzzle input is still 265149.
         */
        
        private readonly ITestOutputHelper output;
        public Day03x2(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void First()
        {
            var expected = 1;
            var input = 1;

            var actual = SumNeighbors(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Second()
        {
            var expected = 1;
            var input = 2;

            var actual = SumNeighbors(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Third()
        {
            var expected = 2;
            var input = 3;

            var actual = SumNeighbors(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Forth()
        {
            var expected = 4;
            var input = 4;

            var actual = SumNeighbors(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Fifth()
        {
            var expected = 5;
            var input = 5;

            var actual = SumNeighbors(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sixth()
        {
            var expected = 10;
            var input = 6;

            var actual = SumNeighbors(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Seventh()
        {
            var expected = 11;
            var input = 7;

            var actual = SumNeighbors(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TwentyThird()
        {
            var expected = 806;
            var input = 23;

            var actual = SumNeighbors(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 266330;
            var input = 265149;

            var actual = SumNeighbors(input, true);

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetNodes_1()
        {
            var expectedCount = 1;
            var expectedPoint = new Point(0,0);
            var expectedValue = 1;
            var expectedIndex = 1;

            var actual = GetNodes(1);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[0].Point);
            Assert.Equal(expectedValue, actual[0].Value);
            Assert.Equal(expectedIndex, actual[0].Index);
        }

        [Fact]
        public void GetNodes_2()
        {
            var expectedCount = 2;
            var expectedPoint = new Point(1,0);
            var expectedIndex = 2;

            var actual = GetNodes(2);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[1].Point);
            Assert.Equal(expectedIndex, actual[1].Index);
        }

        [Fact]
        public void GetNodes_4()
        {
            var expectedCount = 4;
            var expectedPoint = new Point(0,1);
            var expectedIndex = 4;

            var actual = GetNodes(4);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[3].Point);
            Assert.Equal(expectedIndex, actual[3].Index);
        }

        [Fact]
        public void GetNodes_5()
        {
            var expectedCount = 5;
            var expectedPoint = new Point(-1,1);
            var expectedIndex = expectedCount;

            var actual = GetNodes(expectedCount);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[expectedCount-1].Point);
            Assert.Equal(expectedIndex, actual[expectedCount-1].Index);
        }

        [Fact]
        public void GetNodes_6()
        {
            var expectedCount = 6;
            var expectedPoint = new Point(-1,0);
            var expectedIndex = expectedCount;

            var actual = GetNodes(expectedCount);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[expectedCount-1].Point);
            Assert.Equal(expectedIndex, actual[expectedCount-1].Index);
        }

        [Fact]
        public void GetNodes_9()
        {
            var expectedCount = 9;
            var expectedPoint = new Point(1,-1);
            var expectedIndex = 9;

            var actual = GetNodes(9);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[8].Point);
            Assert.Equal(expectedIndex, actual[8].Index);
        }

        [Fact]
        public void GetNodes_11()
        {
            var expectedCount = 11;
            var expectedPoint = new Point(2,0);
            var expectedIndex = expectedCount;

            var actual = GetNodes(expectedCount);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[expectedCount-1].Point);
            Assert.Equal(expectedIndex, actual[expectedCount-1].Index);
        }

        [Fact]
        public void GetNodes_15()
        {
            var expectedCount = 15;
            var expectedPoint = new Point(0,2);
            var expectedIndex = expectedCount;

            var actual = GetNodes(expectedCount);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[expectedCount-1].Point);
            Assert.Equal(expectedIndex, actual[expectedCount-1].Index);
        }

        [Fact]
        public void GetNodes_25()
        {
            var expectedCount = 25;
            var expectedPoint = new Point(2,-2);
            var expectedIndex = expectedCount;

            var actual = GetNodes(expectedCount);

            Assert.Equal(expectedCount, actual.Count);
            Assert.Equal(expectedPoint, actual[expectedCount-1].Point);
            Assert.Equal(expectedIndex, actual[expectedCount-1].Index);
        }

        private int SumNeighbors(int location, bool isActual = false)
        {
            if (location == 1)
                return 1;

            var nodes = GetNodes(location);

            for (var i = 0; i < location; i++)
            {
                var node = nodes[i];
                var x1 = node.Point.X -1;
                var x2 = node.Point.X + 1;
                var y1 = node.Point.Y -1;
                var y2 = node.Point.Y + 1;
                var surroundingNodes = nodes.Where( z => x1 <= z.Point.X && z.Point.X <= x2 && y1 <= z.Point.Y && z.Point.Y <= y2).ToList();
                node.Value = surroundingNodes.Select(z => z.Value).Sum();

                if (isActual && node.Value > location)
                {
                    return node.Value;
                }

            }

            return nodes.Last().Value;
        }

        private List<Node> GetNodes(int location)
        {
            var list = new List<Node> {  new Node {Index = 1, Point=new Point(0,0), Value = 1} };

            if (location > 1)
            {
                // Populate nodes
                for (var i = 2; i <= location; i++)
                {
                    var current = new Node { Index = i};
                    var previous = list[i-2]; //zero based

                    var x = current.Ring;
                    var xPrevious = previous.Ring;

                    if (x != xPrevious)
                    {
                        current.Point = new Point(previous.X + 1, previous.Y);
                    }
                    else
                    {
                        var lastRing = Squared(x - 2);
                        var l = current.Index - lastRing; //relative location on current ring
                        
                        // Get Side (right, top, left, bottom) and set point
                        if (1 <= l && l < x)
                        {
                            current.Point = new Point(previous.X, previous.Y + 1);
                        }
                        else if (x <= l && l < (x*2 - 1))
                        {
                            current.Point = new Point(previous.X - 1, previous.Y);
                        }
                        else if ((x*2 -1) <= l && l < (x*3 -2))
                        {
                            current.Point = new Point(previous.X, previous.Y - 1);
                        }
                        else
                        {
                            current.Point = new Point(previous.X + 1, previous.Y);
                        }
                    }

                    list.Add(current);
                }
            }
            return list;
        }

        private static int Squared(int x) => x*x;

        private class Point
        {
            public int X { get;}
            public int Y { get;}
            public Point(int x, int y)
            {
                X = x;
                Y = y;
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
        private class Node 
        {
            public int Index { get; set; }
            public int Ring { get {return GetRing(Index);} }
            
            public Point Point {get;set;}
            public int X { get { return Point.X; } }
            public int Y { get { return Point.Y; } }
            
            public int Value { get; set; }

            public override string ToString()
            {
                return $"I:{Index} P:{Point} V:{Value}";
            }

            private int GetRing(int location)
            {
                if (location == 1)
                    return 1;
                int x = 3;

                while ((x*x) < location)
                {
                    x += 2;
                }
                return x;
            }
        }
    }
}
