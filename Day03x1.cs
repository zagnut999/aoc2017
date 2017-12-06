using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day03x1
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
         */
        
        private readonly ITestOutputHelper output;
        public Day03x1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void First()
        {
            var expected = 0;
            var input = 1;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Second()
        {
            var expected = 3;
            var input = 12;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Third()
        {
            var expected = 2;
            var input = 23;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Forth()
        {
            var expected = 31;
            var input = 1024;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 438;
            var input = 265149;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestNine()
        {
            var expected = 2;
            var input = 9;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test4()
        {
            var expected = 1;
            var input = 4;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test5()
        {
            var expected = 2;
            var input = 5;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Test16()
        {
            var expected = 3;
            var input = 16;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test10()
        {
            var expected = 3;
            var input = 10;

            var actual = ManhattanDistance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Diagonal()
        {
            for (int x = 3;x < 20 ; x +=2)
            {
                var expected = x - 1;
                var input = x * x;

                var actual = ManhattanDistance(input);

                Assert.Equal(expected, actual);
            }
            
        }

        private int ManhattanDistance(int location)
        {
            if (location == 1)
                return 0;

            int x = 3;

            while ((x*x) < location)
            {
                x += 2;
            }

            var previousL =  (x-2)*(x-2);
            var i = location - previousL; 
            var l = (x*x) - previousL;
            int l1 = 0, l2 = 0;
            for (l2 = l; l2 > 0; l2 = l1)
            {
                l1 = l2 - x + 1;

                if (l2 >= i && l1 < i)
                    break;
            }
            var m = (l1 + l2) / 2;

            var j = Math.Abs( m - i); // Distance off axis

            var d = (x - 1) / 2; // shortest distance from center for this row

            return j + d;   
        }
    }
}
