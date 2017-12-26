using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode
{
    public class Day14x2
    {
        /* 
        --- Day 14: Disk Defragmentation ---

Suddenly, a scheduled job activates the system's disk defragmenter. Were the situation different, you might sit and watch it for a while, but today, you just don't have that kind of time. It's soaking up valuable system resources that are needed elsewhere, and so the only option is to help it finish its task as soon as possible.

The disk in question consists of a 128x128 grid; each square of the grid is either free or used. On this disk, the state of the grid is tracked by the bits in a sequence of knot hashes.

A total of 128 knot hashes are calculated, each corresponding to a single row in the grid; each hash contains 128 bits which correspond to individual grid squares. Each bit of a hash indicates whether that square is free (0) or used (1).

The hash inputs are a key string (your puzzle input), a dash, and a number from 0 to 127 corresponding to the row. For example, if your key string were flqrgnkx, then the first row would be given by the bits of the knot hash of flqrgnkx-0, the second row from the bits of the knot hash of flqrgnkx-1, and so on until the last row, flqrgnkx-127.

The output of a knot hash is traditionally represented by 32 hexadecimal digits; each of these digits correspond to 4 bits, for a total of 4 * 32 = 128 bits. To convert to bits, turn each hexadecimal digit to its equivalent binary value, high-bit first: 0 becomes 0000, 1 becomes 0001, e becomes 1110, f becomes 1111, and so on; a hash that begins with a0c2017... in hexadecimal would begin with 10100000110000100000000101110000... in binary.

Continuing this process, the first 8 rows and columns for key flqrgnkx appear as follows, using # to denote used squares, and . to denote free ones:

##.#.#..-->
.#.#.#.#   
....#.#.   
#.#.##.#   
.##.#...   
##..#..#   
.#...#..   
##.#.##.-->
|      |   
V      V   
In this example, 8108 squares are used across the entire 128x128 grid.

Given your actual key string, how many squares are used?

Your puzzle input is jzgqcdpd.

Your puzzle answer was 8074.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

Now, all the defragmenter needs to know is the number of regions. A region is a group of used squares that are all adjacent, not including diagonals. Every used square is in exactly one region: lone used squares form their own isolated regions, while several adjacent squares all count as a single region.

In the example above, the following nine regions are visible, each marked with a distinct digit:

11.2.3..-->
.1.2.3.4   
....5.6.   
7.8.55.9   
.88.5...   
88..5..8   
.8...8..   
88.8.88.-->
|      |   
V      V   
Of particular interest is the region marked 8; while it does not appear contiguous in this small view, all of the squares marked 8 are connected when considering the whole 128x128 grid. In total, in this example, 1242 regions are present.

How many regions are present given your key string?

Your puzzle input is still jzgqcdpd.
*/
        [Fact]
        public void First()
        {
            var expected = 1242;
            var input = "flqrgnkx";

            var actual = FindRegions(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 1212;
            var input = "jzgqcdpd";

            var actual = FindRegions(input);

            Assert.Equal(expected, actual);
        }

        private int FindRegions(string input)
        {
            var grid = FindBinary(input);
            
            var points = new List<Point>();
            for (var y = 0; y < grid.Count; y++)
            {
                var line = grid[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '1')
                    {
                        points.Add(new Point(x,y));
                    }
                }
            }

            var visited = new List<Point>();
            var regions = new Dictionary<Point, List<Point>>();
            
            Point nextPoint = null;
            nextPoint = points.First();
            while(nextPoint != null)
            {
                var current = nextPoint.Clone();
                regions.Add(current, new List<Point>());
                Search(current, current, points, regions, visited);

                nextPoint = points.FirstOrDefault(x => !visited.Contains(x));
            }

            return regions.Count();
        }

        private void Search(Point current, Point currentRegion, List<Point> points, Dictionary<Point, List<Point>> regions, List<Point> visited)
        {
            regions[currentRegion].Add(current);
            visited.Add(current);

            var point = new Point(current.X + 1, current.Y);
            if (points.Contains(point) && !visited.Contains(point))
            {
                Search(point, currentRegion, points, regions, visited);
            }
            point = new Point(current.X - 1, current.Y);
            if (points.Contains(point) && !visited.Contains(point))
            {
                Search(point, currentRegion, points, regions, visited);
            }
            point = new Point(current.X, current.Y + 1);
            if (points.Contains(point) && !visited.Contains(point))
            {
                Search(point, currentRegion, points, regions, visited);
            }
            point = new Point(current.X, current.Y - 1);
            if (points.Contains(point) && !visited.Contains(point))
            {
                Search(point, currentRegion, points, regions, visited);
            }
        }

        private List<string> FindBinary(string input)
        {
            var hashes = new List<string>();
            for (var i = 0; i < 128; i++)
            {
                var hash = CalculateKnotHash($"{input}-{i}");
                hashes.Add(hash);
            }

            var result = hashes
                            .Select(ToBinaryFromHex)
                            .ToList();

            return result;
        }


        [Fact]
        public void H2B1()
        {
            var input = "A";
            var expect = "1010";

            var actual = ToBinaryFromHex(input);

            Assert.Equal(expect, actual);
        }

        [Fact]
        public void H2B2()
        {
            var input = "AA";
            var expect = "10101010";

            var actual = ToBinaryFromHex(input);

            Assert.Equal(expect, actual);
        }

        [Fact]
        public void H2B3()
        {
            var input = "11";
            var expect = "00010001";

            var actual = ToBinaryFromHex(input);

            Assert.Equal(expect, actual);
        }

        private string ToBinaryFromHex(string hex)
        {
            var temp = hex.Select(x => Convert.ToString(Convert.ToInt64(x.ToString(), 16), 2).PadLeft(4,'0')).ToList();
            var result = string.Join("", temp);
            return result;
        }

        private string CalculateKnotHash(string input)
        {
            var listSize = 256;
            int currentPosition =0;
            int skipSize =0;
            
            var lengths = input.Select( x => (int)x).ToList();
            var standardLengthSuffixValues = new List<int> {17, 31, 73, 47, 23};
            lengths.AddRange(standardLengthSuffixValues);
            
            var sparse = new List<int>();

            var values = new List<int>();
                for (var i = 0; i < listSize; i++)
                    values.Add(i);

            for (var round = 0; round < 64; round++)
            {
                
                
                foreach (var length in lengths)
                {
                    var subSet = new List<int>();
                    for (var i = currentPosition; i < currentPosition+ length; i++)
                    { 
                        subSet.Add(values[i % listSize]);
                    }
                    subSet.Reverse();

                    for (var i = 0; i< subSet.Count; i++)
                        values[(i+currentPosition) % listSize] = subSet[i];
                    
                    currentPosition = (currentPosition + length + skipSize++) % listSize;
                }
                
            }
            sparse = values;

            var dense = new List<int>();
            for (var i = 0; i<16; i++)
            {
                var index = 0 + i * 16;
                var value = sparse[index];

                for (var j = index + 1; j < index + 16; j++)
                {
                    value = value ^ sparse[j];
                }

                dense.Add(value);
            }

            var output = dense.ConvertAll(x => x.ToString("x2")).Aggregate( (working, x) => working += x ) ;

            return output;
        }

    }
}
