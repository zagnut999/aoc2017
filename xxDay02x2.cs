using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode
{
    public class xxDay02x2
    {
        /*
        --- Day 2: Bathroom Security ---

You arrive at Easter Bunny Headquarters under cover of darkness. However, you left in such a rush that you forgot to use the bathroom! Fancy office buildings like this one usually have keypad locks on their bathrooms, so you search the front desk for the code.

"In order to improve security," the document you find says, "bathroom codes will no longer be written down. Instead, please memorize and follow the procedure below to access the bathrooms."

The document goes on to explain that each button to be pressed can be found by starting on the previous button and moving to adjacent buttons on the keypad: U moves up, D moves down, L moves left, and R moves right. Each line of instructions corresponds to one button, starting at the previous button (or, for the first line, the "5" button); press whatever button you're on at the end of each line. If a move doesn't lead to a button, ignore it.

You can't hold it much longer, so you decide to figure out the code as you walk to the bathroom. You picture a keypad like this:

1 2 3
4 5 6
7 8 9
Suppose your instructions are:

ULL
RRDDD
LURDL
UUUUD
You start at "5" and move up (to "2"), left (to "1"), and left (you can't, and stay on "1"), so the first button is 1.
Starting from the previous button ("1"), you move right twice (to "3") and then down three times (stopping at "9" after two moves and ignoring the third), ending up with 9.
Continuing from "9", you move left, up, right, down, and left, ending with 8.
Finally, you move up four times (stopping at "2"), then down once, ending with 5.
So, in this example, the bathroom code is 1985.

Your puzzle input is the instructions from the document you found at the front desk. What is the bathroom code?
Your puzzle answer was 53255.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

You finally arrive at the bathroom (it's a several minute walk from the lobby so visitors can behold the many fancy conference rooms and water coolers on this floor) and go to punch in the code. Much to your bladder's dismay, the keypad is not at all like you imagined it. Instead, you are confronted with the result of hundreds of man-hours of bathroom-keypad-design meetings:

    1
  2 3 4
5 6 7 8 9
  A B C
    D
You still start at "5" and stop when you're at an edge, but given the same instructions as above, the outcome is very different:

You start at "5" and don't move at all (up and left are both edges), ending at 5.
Continuing from "5", you move right twice and down three times (through "6", "7", "B", "D", "D"), ending at D.
Then, from "D", you move five more times (through "D", "B", "C", "C", "B"), ending at B.
Finally, after five more moves, you end at 3.
So, given the actual keypad layout, the code would be 5DB3.

Using the same instructions in your puzzle input, what is the correct bathroom code?

 */
        [Fact]
        public void First()
        {
            var expected = new Point(-1,1);
            var input = "ULL";

            var actual = FindButton(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Second()
        {
            var expected = new Point(1,-1);
            var start = new Point(-1,1);
            var input = "RRDDD";

            var actual = FindButton(input, start);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Third()
        {
            var expected = new Point(0,-1);
            var start = new Point(1,-1);
            var input = "LURDL";

            var actual = FindButton(input, start);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Forth()
        {
            var expected =  new Point(0,1);
            var start = new Point(0,-1);
            var input = "UUUUD";

            var actual = FindButton(input, start);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Fifth()
        {
            var expected = "2CB3";
            var input = new List<string> {"ULL","RRDDD", "LURDL", "UUUUD"};

            var actual = FindPassword(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = "7423A";
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\xxDay02.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            } 

            var actual = FindPassword(input);

            Assert.Equal(expected, actual);
        }

        /*
                1
              2 3 4
            5 6 7 8 9
              A B C
                D
                */
        private Dictionary<Point, string> KeyPad = new Dictionary<Point, string>
        {
            {new Point(0,2), "1"},
            {new Point(-1,1), "2"},{new Point(0,1), "3"},{new Point(1,1), "4"},
            {new Point(-2,0), "5"},{new Point(-1,0), "6"},{new Point(0,0), "7"}, {new Point(1,0), "8"},{new Point(2,0), "9"},
            {new Point(-1,-1), "A"},{new Point(0,-1), "B"},{new Point(1,-1), "C"},
            {new Point(0,-2), "D"},
        };

        private string FindPassword(List<string> inputs)
        {
            var output = string.Empty;

            var previousResult = new Point(0,0);
            foreach (var input in inputs)
            {
                previousResult = FindButton(input, previousResult);
                output += KeyPad[previousResult];
            }
            return output;
        }

        private Point FindButton(string input, Point start = null)
        {
            if (start == null)
                start = new Point(0,0);
        
            foreach (var step in input)
            {
                switch (step)
                {
                    case 'U':
                    {
                        var temp = start.Clone();
                        temp.Y++;
                        if (KeyPad.ContainsKey(temp))
                        {
                            start = temp;
                        }
                        break;
                    }
                    case 'D':
                    {
                        var temp = start.Clone();
                        temp.Y--;
                        if (KeyPad.ContainsKey(temp))
                        {
                            start = temp;
                        }
                        break;
                    }
                    case 'L':
                    {
                        var temp = start.Clone();
                        temp.X--;
                        if (KeyPad.ContainsKey(temp))
                        {
                            start = temp;
                        }
                        break;
                    }
                    case 'R':
                    {
                        var temp = start.Clone();
                        temp.X++;
                        if (KeyPad.ContainsKey(temp))
                        {
                            start = temp;
                        }
                        break;
                    }
                    default:
                        throw new ArgumentException($"input is invalid ({input}, {step})");
                }
            }

            return start;
        }
    }
}
