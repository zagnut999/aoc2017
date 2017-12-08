using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class Day08x2
    {
        /*
        --- Day 8: I Heard You Like Registers ---

You receive a signal directly from the CPU. Because of your recent assistance with jump instructions, it would like you to compute the result of a series of unusual register instructions.

Each instruction consists of several parts: the register to modify, whether to increase or decrease that register's value, the amount by which to increase or decrease it, and a condition. If the condition fails, skip the instruction without modifying the register. The registers all start at 0. The instructions look like this:

b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10
These instructions would be processed as follows:

Because a starts at 0, it is not greater than 1, and so b is not modified.
a is increased by 1 (to 1) because b is less than 5 (it is 0).
c is decreased by -10 (to 10) because a is now greater than or equal to 1 (it is 1).
c is increased by -20 (to -10) because c is equal to 10.
After this process, the largest value in any register is 1.

You might also encounter <= (less than or equal to) or != (not equal to). However, the CPU doesn't have the bandwidth to tell you what all the registers are named, and leaves that to you to determine.

What is the largest value in any register after completing the instructions in your puzzle input?
--- Part Two ---

To be safe, the CPU also needs to know the highest value held in any register during this process so that it can decide how much memory to allocate to these operations. For example, in the above instructions, the highest value ever held was 10 (in register c after the third instruction was evaluated).
         */
        [Fact]
        public void First()
        {
            var expected = 10;
            var input = new List<string>{"b inc 5 if a > 1", "a inc 1 if b < 5", "c dec -10 if a >= 1", "c inc -20 if c == 10"};

            var actual = Execute(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 4644;
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\Day08.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            } 

            var actual = Execute(input);

            Assert.Equal(expected, actual);
        }

        private int Execute(List<string> input)
        {
            var registers = new Dictionary<string, int>();
            var commands = ParseCommands(input);
            int maxValue = 0;

            //initialize registers
            foreach (var command in commands)
            {
                if (!registers.ContainsKey(command.Register))
                {
                    registers.Add(command.Register, 0);
                }
            }

            // Execute commands
            foreach (var command in commands)
            {
                if (command.Condition.Comparer(registers[command.Condition.Register], command.Condition.Value))
                {
                    if (command.Action == Action.Inc)
                    {
                        registers[command.Register] += command.Value;
                    }
                    else
                    {
                        registers[command.Register] -= command.Value;
                    }
                }

                var newMax = registers.Values.Max();
                if (newMax > maxValue)
                    maxValue = newMax;
            }

            return maxValue;
        }

        [Fact]
        public void ParseCommand()
        {
            var input = "b inc 5 if a > 1";

            var commands = ParseCommands(new List<string> {input});

            Assert.Single(commands);
        }

        private List<Command> ParseCommands(List<string> inputs)
        {
            var commands = new List<Command>();

            Regex InputRegex = new Regex(@"^(\w+) (inc|dec) (-?\d+) if (\w+) (\=\=|!=|<|<=|>|>=) (-?\d+)$");
            foreach (var input in inputs)
            {
                var command = new Command();

                var match = InputRegex.Match(input);
                if (!match.Success)
                    throw new ApplicationException($"Match Failed {input}");

                command.Register = match.Groups[1].Value;
                command.Action = match.Groups[2].Value == "inc" ? Action.Inc : Action.Dec;
                command.Value = int.Parse(match.Groups[3].Value);

                Func<int,int, bool> comparer;
                switch (match.Groups[5].Value) {
                    case "==" : comparer = (x,y) => x == y ; break;
                    case "!=" : comparer = (x,y) => x != y; break;
                    case ">": comparer = (x,y) => x > y; break;
                    case ">=" : comparer = (x,y) => x >= y; break;
                    case "<" : comparer = (x,y) => x < y; break;
                    case "<=" : comparer = (x,y) => x <= y; break;
                    default : throw new ArgumentException($"Invalid comparer: {match.Groups[5].Value}");
                };
                command.Condition = new Condition { Register=match.Groups[4].Value, Comparer = comparer, Value = int.Parse(match.Groups[6].Value)};
                commands.Add(command);
            }

            return commands;
        }

        private class Command 
        {
            public string Register { get; set; }
            public Action Action { get; set; }
            public int Value { get; set; }
            public Condition Condition { get; set; }
        }

        private class Condition{
            public string Register { get; set; }
            public Func<int,int,bool> Comparer { get; set; }
            public int Value { get; set; }
        }
        private enum Action {
            Inc,
            Dec,
        }
        private enum Comparer {
            LessThan,
            LessThanOrEqual,
            GreaterThan,
            GreaterThanOrEqual,
            Equal,
            NotEqual
        }
    }
}
