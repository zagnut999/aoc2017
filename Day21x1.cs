using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class Day21x1
    {
        /*
        --- Day 21: Fractal Art ---

You find a program trying to generate some art. It uses a strange process that involves repeatedly enhancing the detail of an image through a set of rules.

The image consists of a two-dimensional square grid of pixels that are either on (#) or off (.). The program always begins with this pattern:

.#.
..#
###
Because the pattern is both 3 pixels wide and 3 pixels tall, it is said to have a size of 3.

Then, the program repeats the following process:

If the size is evenly divisible by 2, break the pixels up into 2x2 squares, and convert each 2x2 square into a 3x3 square by following the corresponding enhancement rule.
Otherwise, the size is evenly divisible by 3; break the pixels up into 3x3 squares, and convert each 3x3 square into a 4x4 square by following the corresponding enhancement rule.
Because each square of pixels is replaced by a larger one, the image gains pixels and so its size increases.

The artist's book of enhancement rules is nearby (your puzzle input); however, it seems to be missing rules. The artist explains that sometimes, one must rotate or flip the input pattern to find a match. (Never rotate or flip the output pattern, though.) Each pattern is written concisely: rows are listed as single units, ordered top-down, and separated by slashes. For example, the following rules correspond to the adjacent patterns:

../.#  =  ..
          .#

                .#.
.#./..#/###  =  ..#
                ###

                        #..#
#..#/..../#..#/.##.  =  ....
                        #..#
                        .##.
When searching for a rule to use, rotate and flip the pattern as necessary. For example, all of the following patterns match the same rule:

.#.   .#.   #..   ###
..#   #..   #.#   ..#
###   ###   ##.   .#.
Suppose the book contained the following two rules:

../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#
As before, the program begins with this pattern:

.#.
..#
###
The size of the grid (3) is not divisible by 2, but it is divisible by 3. It divides evenly into a single square; the square matches the second rule, which produces:

#..#
....
....
#..#
The size of this enhanced grid (4) is evenly divisible by 2, so that rule is used. It divides evenly into four squares:

#.|.#
..|..
--+--
..|..
#.|.#
Each of these squares matches the same rule (../.# => ##./#../...), three of which require some flipping and rotation to line up with the rule. The output for the rule is the same in all four cases:

##.|##.
#..|#..
...|...
---+---
##.|##.
#..|#..
...|...
Finally, the squares are joined into a new grid:

##.##.
#..#..
......
##.##.
#..#..
......
Thus, after 2 iterations, the grid contains 12 pixels that are on.

How many pixels stay on after 5 iterations?
         */

        // [Fact]
        // public void First()
        // {
        //     var expected = 12;
        //     var iterations = 2;
        //     var input = new List<string> {"../.# => ##./#../...",
        //                                     ".#./..#/### => #..#/..../..../#..#"};

        //     var actual = FindActive(input, iterations);

        //     Assert.Equal(expected, actual);
        // }

        // [Fact]
        // public void Actual()
        // {
        //     var expected = 12;
        //     var iterations = 5;
        //     var input = new List<string>();
        //     using(var file = new System.IO.StreamReader(@"..\..\..\Day21.txt"))
        //     {  
        //         string line;
        //         while((line = file.ReadLine()) != null)  
        //         {
        //             input.Add(line);
        //         }
        //         file.Close();
        //     }

        //     var actual = FindActive(input, iterations);

        //     Assert.Equal(expected, actual);
        // }

        private int FindActive(List<string> input, int interations = 5)
        {
            var rules = GetRules(input);
            var initial = ".#./..#/###";

            var result = string.Empty;
            var current = initial;
            for (var i = 0; i < interations; i++)
            {
                var lines = current.Split('/').ToList();
                List<string> newFrames;
                if (lines.Count % 3 == 0)
                {
                    newFrames = BreakUp(lines,3);
                } 
                else if (lines.Count % 2 == 0)
                {
                    newFrames = BreakUp(lines,2);
                }
                else throw new ApplicationException($"{current} is not a valid size");

                current = ApplyEnhancements(newFrames, rules);
            }

            return current.Count(x => x == '#');
        }

        private string ApplyEnhancements(List<string> frames, Dictionary<string, string> enhancements)
        {
            var result = string.Empty;
            var postEnhancements = new List<string>();
            for (var i =0; i < frames.Count; i++)
            {
                var postEnhancement = string.Empty;
                var frame = frames[i];
                var flipFrame = Flip(frame);
                var found = false;
                for (var j=0; j < 4; j++)
                {
                    if (enhancements.ContainsKey(frame))
                    {
                        postEnhancement = enhancements[frame];
                        found = true;
                        break;
                    } 
                    else if (enhancements.ContainsKey(flipFrame))
                    {
                        postEnhancement = enhancements[flipFrame];
                        found = true;
                        break;
                    }
                    frame = Rotate(frame);
                    flipFrame = Flip(frame);
                }
                if (!found)
                {
                    throw new ArgumentException($"{frames[i]} not found in rules");
                }

                postEnhancements.Add(postEnhancement);
            }
            
            var count = postEnhancements.Count;
            if (count == 1)
            {
                result = postEnhancements[0];
            }
            else 
            {
                var frameLength = (int)Math.Sqrt(count);
                var resultLength = (int)Math.Sqrt(postEnhancements[0].Replace("/","").Length) * frameLength;
                var target = new char[resultLength,resultLength];
                for (var frameRowIndex = 0; frameRowIndex < frameLength; frameRowIndex++)
                {
                    var frameRow = postEnhancements.GetRange( frameRowIndex * frameLength , frameLength);
                    
                    for (var frameColumn =0 ; frameColumn <frameRow.Count; frameColumn++)
                    {
                        var frame = frameRow[frameColumn].Split('/').ToList();
                        for (var column = 0; column < frame.Count; column++)
                        {
                            for (var row = 0 ; row < frame.Count; row++)
                            {
                                var targetRow = frameRowIndex * frame.Count + row;
                                var targetColumn = frameColumn * frame.Count + column;

                                target[targetRow,targetColumn] = frame[row][column];
                            }
                        }
                    }
                    
                }
                for (var row = 0; row < resultLength; row++)
                {
                    for (var column = 0; column < resultLength; column++)
                    {
                        result += target[row,column];
                    }
                    result += '/';
                }
                result = result.TrimEnd('/');

            }

            return  result;
        }

        private Dictionary<string, string> GetRules(List<string> input)
        {
            var rules = new Dictionary<string, string>();
            var regex = new Regex(@"^(.*) => (.*)$");
            foreach (var line in input)
            {
                var match = regex.Match(line);
                if (!match.Success)
                    throw new ArgumentException($"GetRules '{line}' is not valid");

                var key = match.Groups[1].Value;
                var value = match.Groups[2].Value;
                rules.Add(key, value);
            }
            return rules;
        }

        private List<string> BreakUp(List<string> lines, int size)
        {
            var newFrames = new List<string>();
            var columns = lines[0].Length;
            for (var row = 0; row < lines.Count; row += size)
            {
                for (var column = 0; column < columns; column += size)
                {
                    var newFrame = string.Empty;
                    for (var y = 0; y < size ; y ++)
                    {
                        for (var x = 0; x < size; x++)
                        {
                            newFrame += lines[row + y][column + x];
                        }
                        newFrame += '/';
                    }
                    
                    newFrame =  newFrame.TrimEnd('/');
                    newFrames.Add(newFrame);
                }
            }

            return newFrames;
        }


        [Fact]
        public void Rotate2Test()
        {
            var expected = "BD/AC";
            var input = "AB/CD";

            var actual = Rotate(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Rotate3Test()
        {
            var expected = "BCF/AEI/DGH";
            var input = "ABC/DEF/GHI";

            var actual = Rotate(input);

            Assert.Equal(expected, actual);
        }

        private string Flip(string original)
        {
            var result = "";
            if (original.Length == 5)
            {
                // 01234
                // AB/CD
                // CD/AB
                result = $"{original[3]}{original[4]}/{original[0]}{original[1]}";
            }
            else if (original.Length == 11)
            {
                // 0123456789*
                // ABC/DEF/GHI
                // 89* 456 012
                // GHI/DEF/ABC
                result = $"{original[8]}{original[9]}{original[10]}/{original[4]}{original[5]}{original[6]}/{original[0]}{original[1]}{original[2]}";
            }
            else 
                throw new ArgumentException ($"'{original}' is an invalid size");
                
            return result;
        }

        private string Rotate(string original)
        {
            var result = "";
            if (original.Length == 5)
            {
                // 01234
                // AB/CD
                result = $"{original[1]}{original[4]}/{original[0]}{original[3]}";
            }
            else if (original.Length == 11)
            {
                // 0123456789*
                // ABC/DEF/GHI
                // 126 05* 489
                // BCF/AEI/DGH
                result = $"{original[1]}{original[2]}{original[6]}/{original[0]}{original[5]}{original[10]}/{original[4]}{original[8]}{original[9]}";
            }
            else 
                throw new ArgumentException ($"'{original}' is an invalid size");
                
            return result;
        }
    }
}
