using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class xxDay03x2
    {
        /*
        Now that you can think clearly, you move deeper into the labyrinth of hallways and office furniture that makes up this part of Easter Bunny HQ. This must be a graphic design department; the walls are covered in specifications for triangles.

Or are they?

The design document gives the side lengths of each triangle it describes, but... 5 10 25? Some of these aren't triangles. You can't help but mark the impossible ones.

In a valid triangle, the sum of any two sides must be larger than the remaining side. For example, the "triangle" given above is impossible, because 5 + 10 is not larger than 25.

In your puzzle input, how many of the listed triangles are possible?

Your puzzle answer was 869.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

Now that you've helpfully marked up their design documents, it occurs to you that triangles are specified in groups of three vertically. Each set of three numbers in a column specifies a triangle. Rows are unrelated.

For example, given the following specification, numbers with the same hundreds digit would be part of the same triangle:

101 301 501
102 302 502
103 303 503
201 401 601
202 402 602
203 403 603
In your puzzle input, and instead reading by columns, how many of the listed triangles are possible?
 */
        [Fact]
        public void First()
        {
            var expected = 6;
            var input = new List<string>{"101,301,501","102,302,502", "103,303,503", "201,401,601", "202,402,602", "203,403,603"};

            var actual = CheckLengths(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 1544;
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\xxDay03.txt"))
            {  
                string line;
                var regex = new Regex(" +");
                while((line = file.ReadLine()) != null)  
                {
                    line = regex.Replace(line, ",");
                    input.Add(line);
                }
                file.Close();
            } 

            var actual = CheckLengths(input);

            Assert.Equal(expected, actual);
        }

        private int CheckLengths(List<string> inputs)
        {
            var result = 0;
            var sides = new List<string>();
            for(var x= 0; x < inputs.Count; x += 3)
            {
                var side1 = inputs[x].Split(',').Select(s => int.Parse(s)).ToList();
                var side2 = inputs[x+1].Split(',').Select(s => int.Parse(s)).ToList();
                var side3 = inputs[x+2].Split(',').Select(s => int.Parse(s)).ToList();

                sides.Add($"{side1[0]},{side2[0]},{side3[0]}");
                sides.Add($"{side1[1]},{side2[1]},{side3[1]}");
                sides.Add($"{side1[2]},{side2[2]},{side3[2]}");
            }

            foreach (var side in sides)
            {
                if (CheckLength(side))
                    result++;
            }
            return result;
        }

        private bool CheckLength(string input)
        {
            var result = false;
            var sides = input.Split(',').Select(x => int.Parse(x)).OrderBy(x => x).ToList();

            if (sides[0] + sides[1]> sides[2])
                result = true;
            
            return result;
        }
    }
}
