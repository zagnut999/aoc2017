using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class Day16x2
    {
        /*
        --- Day 16: Permutation Promenade ---

You come upon a very unusual sight; a group of programs here appear to be dancing.

There are sixteen programs in total, named a through p. They start by standing in a line: a stands in position 0, b stands in position 1, and so on until p, which stands in position 15.

The programs' dance consists of a sequence of dance moves:

Spin, written sX, makes X programs move from the end to the front, but maintain their order otherwise. (For example, s3 on abcde produces cdeab).
Exchange, written xA/B, makes the programs at positions A and B swap places.
Partner, written pA/B, makes the programs named A and B swap places.
For example, with only five programs standing in a line (abcde), they could do the following dance:

s1, a spin of size 1: eabcd.
x3/4, swapping the last two programs: eabdc.
pe/b, swapping programs e and b: baedc.
After finishing their dance, the programs end up in order baedc.

You watch the dance for a while and record their dance moves (your puzzle input). In what order are the programs standing after their dance?

Your puzzle answer was bijankplfgmeodhc.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

Now that you're starting to get a feel for the dance moves, you turn your attention to the dance as a whole.

Keeping the positions they ended up in from their previous dance, the programs perform it again and again: including the first dance, a total of one billion (1000000000) times.

In the example above, their second dance would begin with the order baedc, and use the same dance moves:

s1, a spin of size 1: cbaed.
x3/4, swapping the last two programs: cbade.
pe/b, swapping programs e and b: ceadb.
In what order are the programs standing after their billion dances?
         */
        [Fact]
        public void First()
        {
            var lineup = "abcde";
            var input = new List<string>{"s1"};
            var expected = "eabcd";

            var actual = Dance(input, lineup);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Second()
        {
            var lineup = "eabcd";
            var input = new List<string>{"x3/4"};
            var expected = "eabdc";

            var actual = Dance(input, lineup);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Third()
        {
            var lineup = "eabdc";
            var input = new List<string>{"pe/b"};
            var expected = "baedc";

            var actual = Dance(input, lineup);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Forth()
        {
            var expected = "baedc";
            var input = new List<string>{"s1", "x3/4", "pe/b"};
            var lineup = "abcde";

            var actual = Dance(input, lineup);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = "bijankplfgmeodhc";
            var input = new List<string>();
            var lineup = "abcdefghijklmnop";
            using(var file = new System.IO.StreamReader(@"..\..\..\Day16.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    foreach (var move in line.Split(','))
                        input.Add(move);
                }
                file.Close();
            }

            var actual = lineup;
            for(var x = 0L; x < 1000000000;x++)
            {
                Console.WriteLine(x); 
                actual = Dance(input, actual);
            }

            Assert.Equal(expected, actual);
        }

        private string Dance(List<string> input, string lineup)
        {
            foreach (var move in input)
            {
                switch (move[0])
                {
                    case 's': lineup = Spin(move, lineup);break;
                    case 'x': lineup = SwapIndex(move, lineup);break;
                    case 'p': lineup = SwapName(move, lineup);break;
                    default: throw new ArgumentException($"Illegal move {move}");
                }
            }
            return lineup;
        }

        private string Spin(string move, string lineup)
        {
            var regex = new Regex(@"s(\d+)");
            var match = regex.Match(move);
            if (match.Success)
            {
                var spin = int.Parse(match.Groups[1].Value);
                var lineupLength = lineup.Length;
                var newLineup = lineup.ToCharArray();
                for (var i = 0; i < lineupLength; i++)
                {
                    if (spin > lineupLength || spin < -lineupLength)
                        throw new ArgumentException($"spin is out of bounds {spin}");
                    var newIndex = (i+lineupLength + spin) % lineupLength;

                    newLineup[newIndex] = lineup[i];
                }
                return newLineup.Select(x => x.ToString()).Aggregate((accum, next) => accum + next);
            }
            return "";
        }

        private string SwapIndex(string move, string lineup)
        {
            var regex = new Regex(@"x(\d+)/(\d+)");
            var match = regex.Match(move);
            if (match.Success)
            {
                var first = int.Parse(match.Groups[1].Value);
                var second = int.Parse(match.Groups[2].Value);
                var newLineup = lineup.ToCharArray();
                newLineup[first] = lineup[second];
                newLineup[second] = lineup[first];
                return newLineup.Select(x => x.ToString()).Aggregate((accum, next) => accum + next);
            }
            return "";
        }

        private string SwapName(string move, string lineup)
        {
            var regex = new Regex(@"p(\w+)/(\w+)");
            var match = regex.Match(move);
            if (match.Success)
            {
                var first = match.Groups[1].Value;
                var second = match.Groups[2].Value;
                var newLineup = lineup.ToCharArray();
                var firstIndex = lineup.IndexOf(first);
                var secondIndex = lineup.IndexOf(second);
                newLineup[firstIndex] = lineup[secondIndex];
                newLineup[secondIndex] = lineup[firstIndex];
                return newLineup.Select(x => x.ToString()).Aggregate((accum, next) => accum + next);
            }
            return "";
        }


    }
}
