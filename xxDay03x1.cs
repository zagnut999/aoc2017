using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class xxDay03x1
    {
        /*
        Now that you can think clearly, you move deeper into the labyrinth of hallways and office furniture that makes up this part of Easter Bunny HQ. This must be a graphic design department; the walls are covered in specifications for triangles.

Or are they?

The design document gives the side lengths of each triangle it describes, but... 5 10 25? Some of these aren't triangles. You can't help but mark the impossible ones.

In a valid triangle, the sum of any two sides must be larger than the remaining side. For example, the "triangle" given above is impossible, because 5 + 10 is not larger than 25.

In your puzzle input, how many of the listed triangles are possible?

 */
        [Fact]
        public void First()
        {
            var expected = false;
            var input = "5,10,25";

            var actual = CheckLength(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 869;
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
            foreach(var input in inputs)
            {
                if (CheckLength(input))
                {
                    result++;
                }
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
