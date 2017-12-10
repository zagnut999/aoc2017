using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class xxDay01x2
    {
        /*
        --- Day 1: No Time for a Taxicab ---

Santa's sleigh uses a very high-precision clock to guide its movements, and the clock's oscillator is regulated by stars. Unfortunately, the stars have been stolen... by the Easter Bunny. To save Christmas, Santa needs you to retrieve all fifty stars by December 25th.

Collect stars by solving puzzles. Two puzzles will be made available on each day in the advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

You're airdropped near Easter Bunny Headquarters in a city somewhere. "Near", unfortunately, is as close as you can get - the instructions on the Easter Bunny Recruiting Document the Elves intercepted start here, and nobody had time to work them out further.

The Document indicates that you should start at the given coordinates (where you just landed) and face North. Then, follow the provided sequence: either turn left (L) or right (R) 90 degrees, then walk forward the given number of blocks, ending at a new intersection.

There's no time to follow such ridiculous instructions on foot, though, so you take a moment and work out the destination. Given that you can only walk on the street grid of the city, how far is the shortest path to the destination?

For example:

Following R2, L3 leaves you 2 blocks East and 3 blocks North, or 5 blocks away.
R2, R2, R2 leaves you 2 blocks due South of your starting position, which is 2 blocks away.
R5, L5, R5, R3 leaves you 12 blocks away.
How many blocks away is Easter Bunny HQ?

--- Part Two ---

Then, you notice the instructions continue on the back of the Recruiting Document. Easter Bunny HQ is actually at the first location you visit twice.

For example, if your instructions are R8, R4, R4, R8, the first location you visit twice is 4 blocks away, due East.

How many blocks away is the first location you visit twice?
 */

        [Fact]
        public void First()
        {
            var expected = 4;
            var input = new List<string> {"R8", "R4", "R4", "R8"};

            var actual = Distance(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 161;
            var input = Input;

            var actual = Distance(input);

            Assert.Equal(expected, actual);
        }

        private int Distance(List<string> input)
        {
            var state = new State();
            foreach (var step in input)
            {
                state.Move(step);
            }

            return State.Distance(state.FirstCross);
        }

        private class State {
            public Point Point { get; set; } = new Point();
            public Direction Facing { get; set; } = Direction.N;
            public List<Point> Path {get;set;} = new List<Point>();

            public Point FirstCross {get;set;} = null;

            public void Move(string input)
            {
                var regex = new Regex(@"^(\w)(\d+)$");
                var match =regex.Match(input);
                if (!match.Success)
                {
                    throw new ArgumentException($"Input invalid ('{input}')");
                }

                var directionString = match.Groups[1].Value;
                var blocks = int.Parse(match.Groups[2].Value.ToString());

                Turn(directionString);
                Walk(blocks);
            }

            private void Turn(string value)
            {
                switch (value)
                {
                    case "R" : Facing = Change(+1); break;
                    case "L" : Facing = Change(-1); break;
                    default: throw new ArgumentException($"Turn invalid {value}");
                }
            }

            private Direction Change(int value)
            {
                if (value == -1 && Facing == Direction.N)
                    return Direction.W;
                return (Direction)(((int)Facing + value) % 4);
            }

            private void Walk(int blocks)
            {
                for (var step = 0; step < blocks; step++)
                {
                    switch (Facing)
                    {
                        case Direction.N: Point.X += 1; break;
                        case Direction.S: Point.X -= 1; break;
                        case Direction.E: Point.Y += 1; break;
                        case Direction.W: Point.Y -= 1; break;
                        default: throw new ArgumentException($"Facing invalid: {Facing}");
                    }
                    var newPoint = Point.Clone();
                    if (Path.Contains(newPoint) && FirstCross == null)
                        FirstCross = newPoint;
                    Path.Add(newPoint);
                }
            }

            public int Distance()
            {
                return Distance(Point);
            }

            public static int Distance(Point point)
            {
                return Math.Abs(point.X) + Math.Abs(point.Y);
            }
        }

        private static List<string> Input = new List<string> {"L1","R3","R1","L5","L2","L5","R4","L2","R2","R2","L2","R1","L5","R3","L4","L1","L2","R3","R5","L2","R5","L1","R2","L5","R4","R2","R2","L1","L1","R1","L3","L1","R1","L3","R5","R3","R3","L4","R4","L2","L4","R1","R1","L193","R2","L1","R54","R1","L1","R71","L4","R3","R191","R3","R2","L4","R3","R2","L2","L4","L5","R4","R1","L2","L2","L3","L2","L1","R4","R1","R5","R3","L5","R3","R4","L2","R3","L1","L3","L3","L5","L1","L3","L3","L1","R3","L3","L2","R1","L3","L1","R5","R4","R3","R2","R3","L1","L2","R4","L3","R1","L1","L1","R5","R2","R4","R5","L1","L1","R1","L2","L4","R3","L1","L3","R5","R4","R3","R3","L2","R2","L1","R4","R2","L3","L4","L2","R2","R2","L4","R3","R5","L2","R2","R4","R5","L2","L3","L2","R5","L4","L2","R3","L5","R2","L1","R1","R3","R3","L5","L2","L2","R5"};
    }
}
