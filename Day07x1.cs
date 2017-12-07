using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode
{
    public class Day07x1
    {
        /*
        --- Day 7: Recursive Circus ---

Wandering further through the circuits of the computer, you come upon a tower of programs that have gotten themselves into a bit of trouble. A recursive algorithm has gotten out of hand, and now they're balanced precariously in a large tower.

One program at the bottom supports the entire tower. It's holding a large disc, and on the disc are balanced several more sub-towers. At the bottom of these sub-towers, standing on the bottom disc, are other programs, each holding their own disc, and so on. At the very tops of these sub-sub-sub-...-towers, many programs stand simply keeping the disc below them balanced but with no disc of their own.

You offer to help, but first you need to understand the structure of these towers. You ask each program to yell out their name, their weight, and (if they're holding a disc) the names of the programs immediately above them balancing on that disc. You write this information down (your puzzle input). Unfortunately, in their panic, they don't do this in an orderly fashion; by the time you're done, you're not sure which program gave which information.

For example, if your list is the following:

pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)
...then you would be able to recreate the structure of the towers that looks like this:

                gyxo
              /     
         ugml - ebii
       /      \     
      |         jptl
      |        
      |         pbga
     /        /
tknk --- padx - havc
     \        \
      |         qoyq
      |             
      |         ktlj
       \      /     
         fwft - cntj
              \     
                xhth
In this example, tknk is at the bottom of the tower (the bottom program), and is holding up ugml, padx, and fwft. Those programs are, in turn, holding up other programs; in this example, none of those programs are holding up any other programs, and are all the tops of their own towers. (The actual tower balancing in front of you is much larger.)

Before you're ready to help them, you need to make sure your information is correct. What is the name of the bottom program?
         */
        [Fact]
        public void First()
        {
            var expected = "tknk";
            var input = new List<string> {"pbga (66)","xhth (57)","ebii (61)","havc (66)","ktlj (57)","fwft (72) -> ktlj, cntj, xhth","qoyq (66)","padx (45) -> pbga, havc, qoyq","tknk (41) -> ugml, padx, fwft","jptl (61)","ugml (68) -> gyxo, ebii, jptl","gyxo (61)","cntj (57)"};

            var actual = FindRoot(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = "ykpsek";
            var input = new List<string>();

            using(var file = new System.IO.StreamReader(@"..\..\..\Day07.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            } 

            var actual = FindRoot(input);

            Assert.Equal(expected, actual);
        }

        private string FindRoot(List<string> input)
        {
            var nodes = Parse(input);
            var root = nodes.First( x => x.Parent == null);
            return root.Name;
        }

        private List<Node> Parse(List<string> input)
        {
            var nodes = new List<Node>();
            var nodeMap = new Dictionary<Node, List<string>>();
            foreach(var line in input)
            {
                var linePlus = line.Replace(" ", "").Replace("(", ";").Replace(")","").Replace("->",";").Split(";");
                var name = linePlus[0];
                var weight = int.Parse(linePlus[1]);
                var nodesList = linePlus.Length == 3 ? linePlus[2].Split(",").ToList() : null;
                var node = new Node {Name = name, Weight=weight};
                nodeMap.Add(node, nodesList);
                nodes.Add(node);
            }

            foreach(var node in nodeMap.Keys)
            {
                var nodeNameList = nodeMap[node];
                if (nodeNameList == null)
                    continue;

                foreach (var nodeName in nodeNameList)
                {
                    var child = nodes.First(x => x.Name == nodeName);
                    if (child.Parent != null)
                        throw new ApplicationException($"Parent is not null! {child.Parent}");
                    child.Parent = node;
                    node.Children.Add(child);
                }
            }

            return nodes;
        }

        private class Node
        {
            public string Name { get; set; }
            public int Weight { get; set; }
            public List<Node> Children  { get; set; } = new List<Node>();
            public Node Parent { get; set; }
            public override string ToString()
            {
                return $"{Name} ({Weight}) -> {Children}";
            }
            public override bool Equals(object obj)
            {
                var node = obj as Node;
                if (node == null)
                    return false;
                
                return Name == node.Name;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
        }
    }
}
