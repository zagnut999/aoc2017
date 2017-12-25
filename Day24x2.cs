using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode
{
    public class Day24x2
    {
        /*
        --- Day 24: Electromagnetic Moat ---

The CPU itself is a large, black building surrounded by a bottomless pit. Enormous metal tubes extend outward from the side of the building at regular intervals and descend down into the void. There's no way to cross, but you need to get inside.

No way, of course, other than building a bridge out of the magnetic components strewn about nearby.

Each component has two ports, one on each end. The ports come in all different types, and only matching types can be connected. You take an inventory of the components by their port types (your puzzle input). Each port is identified by the number of pins it uses; more pins mean a stronger connection for your bridge. A 3/7 component, for example, has a type-3 port on one side, and a type-7 port on the other.

Your side of the pit is metallic; a perfect surface to connect a magnetic, zero-pin port. Because of this, the first port you use must be of type 0. It doesn't matter what type of port you end with; your goal is just to make the bridge as strong as possible.

The strength of a bridge is the sum of the port types in each component. For example, if your bridge is made of components 0/3, 3/7, and 7/4, your bridge has a strength of 0+3 + 3+7 + 7+4 = 24.

For example, suppose you had the following components:

0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10
With them, you could make the following valid bridges:

0/1
0/1--10/1
0/1--10/1--9/10
0/2
0/2--2/3
0/2--2/3--3/4
0/2--2/3--3/5
0/2--2/2
0/2--2/2--2/3
0/2--2/2--2/3--3/4
0/2--2/2--2/3--3/5
(Note how, as shown by 10/1, order of ports within a component doesn't matter. However, you may only use each port on a component once.)

Of these bridges, the strongest one is 0/1--10/1--9/10; it has a strength of 0+1 + 1+10 + 10+9 = 31.

What is the strength of the strongest bridge you can make with the components you have available?

Your puzzle answer was 1906.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

The bridge you've built isn't long enough; you can't jump the rest of the way.

In the example above, there are two longest bridges:

0/2--2/2--2/3--3/4
0/2--2/2--2/3--3/5
Of them, the one which uses the 3/5 component is stronger; its strength is 0+2 + 2+2 + 2+3 + 3+5 = 19.

What is the strength of the longest bridge you can make? If you can make multiple bridges of the longest length, pick the strongest one.
 */
        [Fact]
        public void First()
        {
            var expected = 19;
            var input = new List<string> { "0/2", "2/2", "2/3", "3/4", "3/5", "0/1", "10/1", "9/10" };

            var actual = FindStrongest(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 1824;
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\Day24.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            }
            var actual = FindStrongest(input);
            
            Assert.Equal(expected, actual);
        }

        private int FindStrongest(List<string> input)
        {
            var nodes = new List<Node>();
            input.ForEach(x => nodes.Add(new Node(x)));

            var result = Search(new List<Node>(), nodes, 0);
            
            var longestLength =0;
            longestLength = result.Select(x => x.Count).Max();
            var longestResults = result.Where(x => x.Count == longestLength);

            var strongest = 0;
            List<Node> strongestPath;
            foreach (var path in longestResults)
            {
                var sum = path.Select(x => x.Value1 + x.Value2).Sum();
                if (sum > strongest)
                {
                    strongest = sum;
                    strongestPath = path;
                }
            }

            return strongest;
        }

        private List<List<Node>> Search(List<Node> current, List<Node> left, int connector)
        {
            var result = new List<List<Node>>();

            result.Add(current);
            foreach (var node in left.Where(x=> x.Value1 == connector || x.Value2 == connector))
            {
                var newConnector = node.Value1 == connector ? node.Value2 : node.Value1;
                var newCurrent = current.ToList();
                newCurrent.Add(node);
                var newLeft = left.ToList();
                newLeft.Remove(node);
                var newResult = Search(newCurrent, newLeft, newConnector);
                result.AddRange(newResult);
            }

            return result;
        }

        private class Node
        {
            public string Name { get; }
            public int Value1 { get; set; }
            public int Value2 { get; set; }
            
            public Node (string name)
            {
                Name = name;

                var values = name.Split('/');

                Value1 = int.Parse(values[0]);
                Value2 = int.Parse(values[1]);
            }

            public override string ToString()
            {
                return Name;
            }

            public override bool Equals(object obj)
            {
                var node = obj as Node;
                if (node == null)
                    return false;

                return Name.Equals(node.Name);
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

            public Node Clone()
            {
                return new Node(Name); 
            }

        }
    }
}
