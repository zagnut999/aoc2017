using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode
{
    public class Day14x1
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

Your puzzle input is jzgqcdpd.*/
        [Fact]
        public void First()
        {
            var expected = 8108;
            var input = "flqrgnkx";

            var actual = FindCount(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 8074;
            var input = "jzgqcdpd";

            var actual = FindCount(input);

            Assert.Equal(expected, actual);
        }

        private int FindCount(string input)
        {
            var hashes = new List<string>();
            for (var i = 0; i < 128; i++)
            {
                var hash = CalculateKnotHash($"{input}-{i}");
                hashes.Add(hash);
            }

            var result = hashes
                            .Select(ToBinaryFromHex)
                            .Aggregate((x, collector )=> collector += x)
                            .Select(x => x == '1' ? 1 : 0)
                            .Sum();

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
