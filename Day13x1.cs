using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class Day13x1
    {
        /*--- Day 13: Packet Scanners ---

You need to cross a vast firewall. The firewall consists of several layers, each with a security scanner that moves back and forth across the layer. To succeed, you must not be detected by a scanner.

By studying the firewall briefly, you are able to record (in your puzzle input) the depth of each layer and the range of the scanning area for the scanner within it, written as depth: range. Each layer has a thickness of exactly 1. A layer at depth 0 begins immediately inside the firewall; a layer at depth 1 would start immediately after that.

For example, suppose you've recorded the following:

0: 3
1: 2
4: 4
6: 4
This means that there is a layer immediately inside the firewall (with range 3), a second layer immediately after that (with range 2), a third layer which begins at depth 4 (with range 4), and a fourth layer which begins at depth 6 (also with range 4). Visually, it might look like this:

 0   1   2   3   4   5   6
[ ] [ ] ... ... [ ] ... [ ]
[ ] [ ]         [ ]     [ ]
[ ]             [ ]     [ ]
                [ ]     [ ]
Within each layer, a security scanner moves back and forth within its range. Each security scanner starts at the top and moves down until it reaches the bottom, then moves up until it reaches the top, and repeats. A security scanner takes one picosecond to move one step. Drawing scanners as S, the first few picoseconds look like this:


Picosecond 0:
 0   1   2   3   4   5   6
[S] [S] ... ... [S] ... [S]
[ ] [ ]         [ ]     [ ]
[ ]             [ ]     [ ]
                [ ]     [ ]

Picosecond 1:
 0   1   2   3   4   5   6
[ ] [ ] ... ... [ ] ... [ ]
[S] [S]         [S]     [S]
[ ]             [ ]     [ ]
                [ ]     [ ]

Picosecond 2:
 0   1   2   3   4   5   6
[ ] [S] ... ... [ ] ... [ ]
[ ] [ ]         [ ]     [ ]
[S]             [S]     [S]
                [ ]     [ ]

Picosecond 3:
 0   1   2   3   4   5   6
[ ] [ ] ... ... [ ] ... [ ]
[S] [S]         [ ]     [ ]
[ ]             [ ]     [ ]
                [S]     [S]
Your plan is to hitch a ride on a packet about to move through the firewall. The packet will travel along the top of each layer, and it moves at one layer per picosecond. Each picosecond, the packet moves one layer forward (its first move takes it into layer 0), and then the scanners move one step. If there is a scanner at the top of the layer as your packet enters it, you are caught. (If a scanner moves into the top of its layer while you are there, you are not caught: it doesn't have time to notice you before you leave.) If you were to do this in the configuration above, marking your current position with parentheses, your passage through the firewall would look like this:

Initial state:
 0   1   2   3   4   5   6
[S] [S] ... ... [S] ... [S]
[ ] [ ]         [ ]     [ ]
[ ]             [ ]     [ ]
                [ ]     [ ]

Picosecond 0:
 0   1   2   3   4   5   6
(S) [S] ... ... [S] ... [S]
[ ] [ ]         [ ]     [ ]
[ ]             [ ]     [ ]
                [ ]     [ ]

 0   1   2   3   4   5   6
( ) [ ] ... ... [ ] ... [ ]
[S] [S]         [S]     [S]
[ ]             [ ]     [ ]
                [ ]     [ ]


Picosecond 1:
 0   1   2   3   4   5   6
[ ] ( ) ... ... [ ] ... [ ]
[S] [S]         [S]     [S]
[ ]             [ ]     [ ]
                [ ]     [ ]

 0   1   2   3   4   5   6
[ ] (S) ... ... [ ] ... [ ]
[ ] [ ]         [ ]     [ ]
[S]             [S]     [S]
                [ ]     [ ]


Picosecond 2:
 0   1   2   3   4   5   6
[ ] [S] (.) ... [ ] ... [ ]
[ ] [ ]         [ ]     [ ]
[S]             [S]     [S]
                [ ]     [ ]

 0   1   2   3   4   5   6
[ ] [ ] (.) ... [ ] ... [ ]
[S] [S]         [ ]     [ ]
[ ]             [ ]     [ ]
                [S]     [S]


Picosecond 3:
 0   1   2   3   4   5   6
[ ] [ ] ... (.) [ ] ... [ ]
[S] [S]         [ ]     [ ]
[ ]             [ ]     [ ]
                [S]     [S]

 0   1   2   3   4   5   6
[S] [S] ... (.) [ ] ... [ ]
[ ] [ ]         [ ]     [ ]
[ ]             [S]     [S]
                [ ]     [ ]


Picosecond 4:
 0   1   2   3   4   5   6
[S] [S] ... ... ( ) ... [ ]
[ ] [ ]         [ ]     [ ]
[ ]             [S]     [S]
                [ ]     [ ]

 0   1   2   3   4   5   6
[ ] [ ] ... ... ( ) ... [ ]
[S] [S]         [S]     [S]
[ ]             [ ]     [ ]
                [ ]     [ ]


Picosecond 5:
 0   1   2   3   4   5   6
[ ] [ ] ... ... [ ] (.) [ ]
[S] [S]         [S]     [S]
[ ]             [ ]     [ ]
                [ ]     [ ]

 0   1   2   3   4   5   6
[ ] [S] ... ... [S] (.) [S]
[ ] [ ]         [ ]     [ ]
[S]             [ ]     [ ]
                [ ]     [ ]


Picosecond 6:
 0   1   2   3   4   5   6
[ ] [S] ... ... [S] ... (S)
[ ] [ ]         [ ]     [ ]
[S]             [ ]     [ ]
                [ ]     [ ]

 0   1   2   3   4   5   6
[ ] [ ] ... ... [ ] ... ( )
[S] [S]         [S]     [S]
[ ]             [ ]     [ ]
                [ ]     [ ]
In this situation, you are caught in layers 0 and 6, because your packet entered the layer when its scanner was at the top when you entered it. You are not caught in layer 1, since the scanner moved into the top of the layer once you were already there.

The severity of getting caught on a layer is equal to its depth multiplied by its range. (Ignore layers in which you do not get caught.) The severity of the whole trip is the sum of these values. In the example above, the trip severity is 0*3 + 6*4 = 24.

Given the details of the firewall you've recorded, if you leave immediately, what is the severity of your whole trip?
 */
        [Fact]
        public void First()
        {
            var expected = 0;
            var input = new List<string>{"0: 3", "1: 2","4: 4","6: 4"};

            var actual = CalculateSeverity(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 0;
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\Day13.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            }

            var actual = 0;

            Assert.Equal(expected, actual);
        }

        private int CalculateSeverity(List<string> input)
        {
            var initial = Parse(input);

            var timer = 0;

            var max = initial.Keys.Max();

            for (var wall = 0; wall < max; wall++)
            {
                
                timer++;
            }

            return 0;
        }

        private Dictionary<int,int> Parse(List<string> inputs)
        {
            var result = new Dictionary<int,int>();
            var regex = new Regex(@"(\d+): (\d+)");
            foreach (var input in inputs)
            {
                var match = regex.Match(input);
                if (match.Success)
                {
                    var key = int.Parse(match.Groups[1].Value);
                    var value = int.Parse(match.Groups[2].Value);
                    result.Add(key,value);
                }
                else 
                {
                    throw new ArgumentException($"input invalid {input}");
                }
            }

            return result;
        }
    }
}
