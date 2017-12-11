using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode
{
    public class Day11x1
    {
        /*
        --- Day 11: Hex Ed ---

Crossing the bridge, you've barely reached the other side of the stream when a program comes up to you, clearly in distress. "It's my child process," she says, "he's gotten lost in an infinite grid!"

Fortunately for her, you have plenty of experience with infinite grids.

Unfortunately for you, it's a hex grid.

The hexagons ("hexes") in this grid are aligned such that adjacent hexes can be found to the north, northeast, southeast, south, southwest, and northwest:

  \ n  /
nw +--+ ne
  /    \
-+      +-
  \    /
sw +--+ se
  / s  \
You have the path the child process took. Starting where he started, you need to determine the fewest number of steps required to reach him. (A "step" means to move from the hex you are in to any adjacent hex.)

For example:

ne,ne,ne is 3 steps away.
ne,ne,sw,sw is 0 steps away (back where you started).
ne,ne,s,s is 2 steps away (se,se).
se,sw,se,sw,sw is 3 steps away (s,s,sw). 

https://www.redblobgames.com/grids/hexagons/
*/
        [Fact]
        public void First()
        {
            var expected = 3;
            var input = "ne,ne,ne";

            var actual = FindShortest(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Second()
        {
            var expected = 0;
            var input = "ne,ne,sw,sw";

            var actual = FindShortest(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Third()
        {
            var expected = 2;
            var input = "ne,ne,s,s";

            var actual = FindShortest(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Forth()
        {
            var expected = 3;
            var input = "se,sw,se,sw,sw";

            var actual = FindShortest(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Fifth()
        {
            var expected = 3;
            var input = "ne,se,ne";

            var actual = FindShortest(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 784;
            string input = "";
            using(var file = new System.IO.StreamReader(@"..\..\..\Day11.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input = line;
                }
                file.Close();
            } 

            var actual = FindShortest(input);

            Assert.Equal(expected, actual);
        }
        

        private int FindShortest(string path)
        {
            var route = PlotRoute(path);
            var current = route[route.Count- 1].Clone();
            var distance = 0;
            var start = route[0].Clone();
            //Find shortest back to 0,0
            while (!current.Point.Equals(start.Point))
            {
                if (current.Point.X == 0)
                {
                    // need to move n/s
                    if (current.Point.Y > 0)
                        current = current.Move("s");
                    else
                        current = current.Move("n");
                }
                else
                {
                    if (current.Point.Y >= 0)
                        if (current.Point.X >= 0)
                            current = current.Move("sw");
                        else
                            current = current.Move("se");
                    else
                        if (current.Point.X > 0)
                            current = current.Move("nw");
                        else
                            current = current.Move("ne");
                }
                
                distance++;
            }
            return distance;
        }

        private List<Node> PlotRoute(string path)
        {
            var steps = path.Split(',');
            var current = new Node();

            var route = new List<Node> {current};
            foreach (var step in steps)
            {
                current = current.Move(step);
                route.Add(current);
            }

            return route;
        }

        private class Node
        {
            public Point Point { get; set; } = new Point();

            public Node Move(string direction)
            {
                var newNode = Clone();

                /*
                     0 0 0
                       0 
                     0 0 0      
                    no direct east or west
                 */

                switch (direction)
                {
                    case "nw":
                        if (IsEven(newNode))
                        {
                            newNode.Point.X += -1;
                            newNode.Point.Y += 0; 
                        }
                        else
                        {
                            newNode.Point.X += -1;
                            newNode.Point.Y += 1; 
                        }
                        
                        break;
                    case "n": 
                        newNode.Point.X += 0;
                        newNode.Point.Y += 1; 
                        break;
                    case "ne":
                        if (IsEven(newNode))
                        {
                            newNode.Point.X += 1;
                            newNode.Point.Y += 0; 
                        }
                        else
                        {
                            newNode.Point.X += 1;
                            newNode.Point.Y += 1; 
                        }
                        break;
                    case "sw": 
                        if (IsEven(newNode))
                        {
                            newNode.Point.X += -1;
                            newNode.Point.Y += -1; 
                        }
                        else
                        {
                            newNode.Point.X += -1;
                            newNode.Point.Y += 0; 
                        }
                        break;
                    case "s": 
                        newNode.Point.X += 0;
                        newNode.Point.Y += -1; 
                        break;
                    case "se": 
                        if (IsEven(newNode))
                        {
                            newNode.Point.X += 1;
                            newNode.Point.Y += -1; 
                        }
                        else
                        {
                            newNode.Point.X += 1;
                            newNode.Point.Y += 0; 
                        } 
                        break;
                    default: throw new ArgumentException($"Invalid direction {direction}");
                }
                return newNode;
            }

            public Node Clone()
            {
                return new Node { Point= Point.Clone()};
            }

            private bool IsEven(Node node)
            {
                return node.Point.X % 2 == 0;
            }
        }
    }
}
