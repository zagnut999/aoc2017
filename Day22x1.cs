using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode
{
    public class Day22x1
    {
        /*
        --- Day 22: Sporifica Virus ---

Diagnostics indicate that the local grid computing cluster has been contaminated with the Sporifica Virus. The grid computing cluster is a seemingly-infinite two-dimensional grid of compute nodes. Each node is either clean or infected by the virus.

To prevent overloading the nodes (which would render them useless to the virus) or detection by system administrators, exactly one virus carrier moves through the network, infecting or cleaning nodes as it moves. The virus carrier is always located on a single node in the network (the current node) and keeps track of the direction it is facing.

To avoid detection, the virus carrier works in bursts; in each burst, it wakes up, does some work, and goes back to sleep. The following steps are all executed in order one time each burst:

If the current node is infected, it turns to its right. Otherwise, it turns to its left. (Turning is done in-place; the current node does not change.)
If the current node is clean, it becomes infected. Otherwise, it becomes cleaned. (This is done after the node is considered for the purposes of changing direction.)
The virus carrier moves forward one node in the direction it is facing.
Diagnostics have also provided a map of the node infection status (your puzzle input). Clean nodes are shown as .; infected nodes are shown as #. This map only shows the center of the grid; there are many more nodes beyond those shown, but none of them are currently infected.

The virus carrier begins in the middle of the map facing up.

For example, suppose you are given a map like this:

..#
#..
...
Then, the middle of the infinite grid looks like this, with the virus carrier's position marked with [ ]:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . # . . .
. . . #[.]. . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
The virus carrier is on a clean node, so it turns left, infects the node, and moves left:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . # . . .
. . .[#]# . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
The virus carrier is on an infected node, so it turns right, cleans the node, and moves up:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . .[.]. # . . .
. . . . # . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
Four times in a row, the virus carrier finds a clean, infects it, turns left, and moves forward, ending in the same place and still facing up:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . #[#]. # . . .
. . # # # . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
Now on the same node as before, it sees an infection, which causes it to turn right, clean the node, and move forward:

. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
. . # .[.]# . . .
. . # # # . . . .
. . . . . . . . .
. . . . . . . . .
. . . . . . . . .
After the above actions, a total of 7 bursts of activity had taken place. Of them, 5 bursts of activity caused an infection.

After a total of 70, the grid looks like this, with the virus carrier facing up:

. . . . . # # . .
. . . . # . . # .
. . . # . . . . #
. . # . #[.]. . #
. . # . # . . # .
. . . . . # # . .
. . . . . . . . .
. . . . . . . . .
By this time, 41 bursts of activity caused an infection (though most of those nodes have since been cleaned).

After a total of 10000 bursts of activity, 5587 bursts will have caused an infection.

Given your actual map, after 10000 bursts of activity, how many bursts cause a node to become infected? (Do not count nodes that begin infected.)
         */
        [Fact]
        public void First()
        {
            /*
            ..#
            #..
            ... 
            */
            var expected = 5;
            var input = new List<string>{"..#","#..","..."};

            var initialState = Initialize(input);
            var state = Run(initialState, 7);
            var actual = state.NewInfections;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Second()
        {
            /*
            ..#
            #..
            ... 
            */
            var expected = 41;
            var input = new List<string>{"..#","#..","..."};

            var initialState = Initialize(input);
            var state = Run(initialState, 70);
            var actual = state.NewInfections;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Third()
        {
            /*
            ..#
            #..
            ... 
            */
            var expected = 5587;
            var input = new List<string>{"..#","#..","..."};

            var initialState = Initialize(input);
            var state = Run(initialState, 10000);
            var actual = state.NewInfections;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 5176;
            var iterations = 10000;
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\Day22.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            }
            var initialState = Initialize(input);
            var state = Run(initialState, iterations);
            var actual = state.NewInfections;

            Assert.Equal(expected, actual);
        }
        private State Run(State state, int steps)
        {
            for (var i = 0; i < steps; i++)
            {
                state.Move();
                //Console.WriteLine(state);
            }
            return state;
        }

        private class State 
        {
            public Direction Facing { get; set; }
            public Point Current {get;set;}
            public int NewInfections {get;set;}
            public Dictionary<Point, bool> Map { get;set;} = new Dictionary<Point, bool>();

            public void Move()
            {
                var isCurrentCellInfected = Map[Current];
                if (isCurrentCellInfected)
                {
                    //turn right
                    switch (Facing)
                    {
                        case Direction.N: Facing = Direction.E; break;
                        case Direction.E: Facing = Direction.S; break;
                        case Direction.S: Facing = Direction.W; break;
                        case Direction.W: Facing = Direction.N; break;
                        default:
                            throw new ArgumentException($"'{Facing}' is invalid.");
                    }
                }
                else
                {
                    //turn left
                    switch (Facing)
                    {
                        case Direction.N: Facing = Direction.W; break;
                        case Direction.E: Facing = Direction.N; break;
                        case Direction.S: Facing = Direction.E; break;
                        case Direction.W: Facing = Direction.S; break;
                        default: throw new ArgumentException($"'{Facing}' is invalid.");
                    }
                }
                if (!isCurrentCellInfected)
                    NewInfections++;
                
                Map[Current] = !isCurrentCellInfected;
                Current = Current.Clone();
                switch (Facing)
                {
                    case Direction.N: Current.Y--; break;
                    case Direction.E: Current.X++; break;
                    case Direction.S: Current.Y++; break;
                    case Direction.W: Current.X--; break;
                    default: throw new ArgumentException($"'{Facing}' is invalid.");
                }

                if (!Map.ContainsKey(Current))
                {
                    Map.Add(Current, false);
                }
            }


            public override string ToString()
            {
                var minX = Map.Keys.Select(p => p.X).Min();
                var minY = Map.Keys.Select(p => p.Y).Min();
                var maxX = Map.Keys.Select(p => p.X).Max();
                var maxY = Map.Keys.Select(p => p.Y).Max();

                var map = new List<string>();
                for (var y = minY; y <= maxY; y++)
                {
                    var newLine = string.Empty;
                    
                    for (var x = minX; x <= maxX; x++)
                    {
                        var point = new Point(x,y);
                        
                        var isCurrent = false;
                        if (point.Equals(Current))
                            isCurrent = true;

                        if (Map.ContainsKey(point))
                        {
                            if (isCurrent)
                                newLine += Map[point] ? "[#]" : "[.]";
                            else
                                newLine += Map[point] ? " # " : " . ";
                        }
                        else
                        {
                            newLine += " . ";
                        }
                    }
                    map.Add(newLine);
                }
                var output = string.Empty;
                foreach (var line in map)
                {
                    output +=line;
                    output += '\n';//"/";
                }
                output = output.TrimEnd('/');
                return $"Facing: {Facing.ToString()} Current:{Current} Map:\n{output}";
            }
        }

        private State Initialize(List<string> input)
        {
            var center = input[0].Length / 2; // zero indexed
            var newState = new State {Facing = Direction.N, Current = new Point(center, center)};
            
            for (var y = 0; y < input.Count; y++)
            {
                var line = input[y];
                for (var x = 0; x < line.Length; x++)
                {
                    newState.Map.Add(new Point(x,y), line[x]=='#');
                }
            }
            return newState;
        }
    }
}
