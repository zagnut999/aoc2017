using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class Day12x1
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
 */

        [Fact]
        public void Actual()
        {
            var expected = 239;
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

            var actual = FindConnectionsToZero(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void First()
        {
            var expected = 6;
            var input = new List<string>{"0 <-> 2", "1 <-> 1", "2 <-> 0, 3, 4", "3 <-> 2, 4", "4 <-> 2, 3, 6", "5 <-> 6", "6 <-> 4, 5"};

            var actual = FindConnectionsToZero(input);

            Assert.Equal(expected, actual);
        }

        private int FindConnectionsToZero(List<string> inputs)
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
                        
                        if (connections.ContainsKey(valueInt))
                            connections[valueInt].Add(key);
                        else
                            connections.Add(valueInt, new List<int> {key});
                    }
                }
            }

            var reachedZero = 0;

            foreach (var key in connections.Keys)
            {
                var success = Check(key, connections, new List<int>());
                if (success)
                    reachedZero++;
            }

            return reachedZero;
        }

        private bool Check(int key, Dictionary<int,List<int>> connections, List<int> visited)
        {
            visited.Add(key);
            if (key == 0)
                return true;
            
            if (connections.ContainsKey(key))
                foreach (var newKey in connections[key])
                {
                    if (visited.Contains(newKey))
                        continue;

                    var result = Check(newKey, connections, visited);
                    if (result)
                        return true;
                }

            return false;
        }
    }
}
