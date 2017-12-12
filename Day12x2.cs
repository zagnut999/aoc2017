using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class Day12x2
    {
        /*
        http://adventofcode.com/2017/day/12
        --- Day 12: Digital Plumber ---

Walking along the memory banks of the stream, you find a small village that is experiencing a little confusion: some programs can't communicate with each other.

Programs in this village communicate using a fixed system of pipes. Messages are passed between programs using these pipes, but most programs aren't connected to each other directly. Instead, programs pass messages between each other until the message reaches the intended recipient.

For some reason, though, some of these messages aren't ever reaching their intended recipient, and the programs suspect that some pipes are missing. They would like you to investigate.

You walk through the village and record the ID of each program and the IDs with which it can communicate directly (your puzzle input). Each program has one or more programs with which it can communicate, and these pipes are bidirectional; if 8 says it can communicate with 11, then 11 will say it can communicate with 8.

You need to figure out how many programs are in the group that contains program ID 0.

For example, suppose you go door-to-door like a travelling salesman and record the following list:

0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5
In this example, the following programs are in the group that contains program ID 0:

Program 0 by definition.
Program 2, directly connected to program 0.
Program 3 via program 2.
Program 4 via program 2.
Program 5 via programs 6, then 4, then 2.
Program 6 via programs 4, then 2.
Therefore, a total of 6 programs are in this group; all but program 1, which has a pipe that connects it to itself.

How many programs are in the group that contains program ID 0?

Your puzzle answer was 239.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

There are more programs than just the ones in the group containing program ID 0. The rest of them have no way of reaching that group, and still might have no way of reaching each other.

A group is a collection of programs that can all communicate via pipes either directly or indirectly. The programs you identified just a moment ago are all part of the same group. Now, they would like you to determine the total number of groups.

In the example above, there were 2 groups: one consisting of programs 0,2,3,4,5,6, and the other consisting solely of program 1.

How many groups are there in total?
 */

        [Fact]
        public void Actual()
        {
            var expected = 215;
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\Day12.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            } 

            var actual = FindConnectionsGroups(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void First()
        {
            var expected = 2;
            var input = new List<string>{"0 <-> 2", "1 <-> 1", "2 <-> 0, 3, 4", "3 <-> 2, 4", "4 <-> 2, 3, 6", "5 <-> 6", "6 <-> 4, 5"};

            var actual = FindConnectionsGroups(input);

            Assert.Equal(expected, actual);
        }

        private int FindConnectionsGroups(List<string> inputs)
        {
            var connections = BuildConnections(inputs);
            var connectionGroups = new Dictionary<int, List<int>>();

            foreach (var key in connections.Keys)
            {
                if (connectionGroups.Count == 0)
                {
                    connectionGroups.Add(key, new List<int>{key});
                    continue;
                }

                if (connectionGroups.Values.Any(x => x.Any(y=> y == key)))
                    continue;

                var added = false;
                foreach (var target in connectionGroups.Keys)
                {
                    var success = Check(key, target, connections, new List<int>());
                    if (success)
                    {
                        connectionGroups[target].Add(key);
                        added = true;
                        break;
                    }
                }
                if (!added)
                    connectionGroups.Add(key, new List<int>{key});
            }

            return connectionGroups.Count();
        }

        private bool Check(int key, int target, Dictionary<int,List<int>> connections, List<int> visited)
        {
            visited.Add(key);
            if (key == target)
                return true;
            
            if (connections.ContainsKey(key))
                foreach (var newKey in connections[key])
                {
                    if (visited.Contains(newKey))
                        continue;

                    var result = Check(newKey, target, connections, visited);
                    if (result)
                        return true;
                }

            return false;
        }

        private Dictionary<int, List<int>> BuildConnections(List<string> inputs)
        {
            var connections = new Dictionary<int, List<int>>();
            var regex = new Regex(@"(\d+)<->([\d,]+)");
            foreach(var input in inputs)
            {
                var line = input.Replace(" ", "");
                var match = regex.Match(line);
                if (match.Success)
                {
                    var key = int.Parse(match.Groups[1].Value);
                    var values = match.Groups[2].Value.Split(',');
                    foreach (var value in values)
                    {
                        var valueInt = int.Parse(value);
                        if (connections.ContainsKey(key))
                            connections[key].Add(valueInt);
                        else
                            connections.Add(key, new List<int> {valueInt});
                    }
                }
            }
            return connections;
        }
    }
}
