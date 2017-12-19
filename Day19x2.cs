using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode
{
    public class Day19x2
    {
        /*
        --- Day 19: A Series of Tubes ---

Somehow, a network packet got lost and ended up here. It's trying to follow a routing diagram (your puzzle input), but it's confused about where to go.

Its starting point is just off the top of the diagram. Lines (drawn with |, -, and +) show the path it needs to take, starting by going down onto the only line connected to the top of the diagram. It needs to follow this path until it reaches the end (located somewhere within the diagram) and stop there.

Sometimes, the lines cross over each other; in these cases, it needs to continue going the same direction, and only turn left or right when there's no other option. In addition, someone has left letters on the line; these also don't change its direction, but it can use them to keep track of where it's been. For example:

     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ 

Given this diagram, the packet needs to take the following path:

Starting at the only line touching the top of the diagram, it must go down, pass through A, and continue onward to the first +.
Travel right, up, and right, passing through B in the process.
Continue down (collecting C), right, and up (collecting D).
Finally, go all the way left through E and stopping at F.
Following the path to the end, the letters it sees on its path are ABCDEF.

The little packet looks up at you, hoping you can help it find the way. What letters will it see (in the order it would see them) if it follows the path? (The routing diagram is very wide; make sure you view it without line wrapping.)

Your puzzle answer was GPALMJSOY.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---
The packet is curious how many steps it needs to go.

For example, using the same routing diagram from the example above...

     |          
     |  +--+    
     A  |  C    
 F---|--|-E---+ 
     |  |  |  D 
     +B-+  +--+ 

...the packet would go:

6 steps down (including the first line at the top of the diagram).
3 steps right.
4 steps up.
3 steps right.
4 steps down.
3 steps right.
2 steps up.
13 steps left (including the F it stops on).
This would result in a total of 38 steps.

How many steps does the packet need to go?
 */

        [Fact]
        public void First()
        {
            var expected = 38;
            var input = new List<string>{
                    "     |          ",
                    "     |  +--+    ",
                    "     A  |  C    ",
                    " F---|----E|--+ ", 
                    "     |  |  |  D ",
                    "     +B-+  +--+ "};

            var actual = FindRoute(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 16204;
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\Day19.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            }

            var actual = FindRoute(input);

            Assert.Equal(expected, actual);
        }

        private int FindRoute(List<string> input)
        {
            Point current = new Point();
            char currentValue;
            bool hasNextValue = true;
            var steps = 0;
            // 1 = north, 2 = east, 3 = south, 4 = west
            int direction = 0;

            // find initial x,y, direction
            direction = 3;
            current.Y = 0;
            current.X = input[current.Y].IndexOf('|');
            currentValue = '|';

            do
            {
                switch(currentValue)
                {
                    case ' ': 
                        hasNextValue = false;
                        break;
                    default: 
                        direction = Move(currentValue, current, direction, input);
                        steps++;
                        break;
                }
                currentValue = input[current.Y][current.X];
            }
            while (hasNextValue);


            return steps;
        }

        private int Move(char currentValue, Point current, int direction, List<string> input)
        {
            var newDirection = direction;
            switch(currentValue)
            {
                // case '-':
                // case '|': 
                //     if (direction == 2)
                //         current.X++;
                //     else if (direction == 4)
                //         current.X--;
                //     else if (direction == 1)
                //         current.Y--; // note this is opposite
                //     else if (direction == 3)
                //         current.Y++;
                //     else
                //         throw new ArgumentException($"{direction} and {currentValue} not valid combo");
                //     break;
                case '+': 
                    if (direction == 2 || direction == 4)
                    {
                        if (input.Count > (current.Y+1) && input[current.Y+1][current.X] != ' ')
                        {
                            current.Y++;
                            newDirection = 3;
                        }
                        else if (0 <= (current.Y-1) && input[current.Y - 1][current.X] != ' ')
                        {
                            current.Y--;
                            newDirection = 1;
                        }
                        else 
                        {
                            throw new ArgumentException($"No path found.");
                        }
                    }
                    else if (direction == 1 ||direction == 3)
                    {
                        if (input[current.Y].Length > (current.X +1) && input[current.Y][current.X + 1] != ' ')
                        {
                            current.X++;
                            newDirection = 2;
                        }
                        else if (0 <= (current.X - 1) && input[current.Y][current.X - 1] != ' ')
                        {
                            current.X--;
                            newDirection = 4;
                        }
                        else 
                        {
                            throw new ArgumentException($"No path found.");
                        }
                        
                    }    
                    break;
                case ' ': 
                    throw new ArgumentException($"Current value should not be a space.");
                default: 
                    if (direction == 2)
                        current.X++;
                    else if (direction == 4)
                        current.X--;
                    else if (direction == 3)
                        current.Y++;
                    else if (direction == 1)
                        current.Y--;
                    break;
            }

            return newDirection;
        }
    }
}
