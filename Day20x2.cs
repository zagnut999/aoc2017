using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode
{
    public class Day20x2
    {
        /*
        --- Day 20: Particle Swarm ---

Suddenly, the GPU contacts you, asking for help. Someone has asked it to simulate too many particles, and it won't be able to finish them all in time to render the next frame at this rate.

It transmits to you a buffer (your puzzle input) listing each particle in order (starting with particle 0, then particle 1, particle 2, and so on). For each particle, it provides the X, Y, and Z coordinates for the particle's position (p), velocity (v), and acceleration (a), each in the format <X,Y,Z>.

Each tick, all particles are updated simultaneously. A particle's properties are updated in the following order:

Increase the X velocity by the X acceleration.
Increase the Y velocity by the Y acceleration.
Increase the Z velocity by the Z acceleration.
Increase the X position by the X velocity.
Increase the Y position by the Y velocity.
Increase the Z position by the Z velocity.
Because of seemingly tenuous rationale involving z-buffering, the GPU would like to know which particle will stay closest to position <0,0,0> in the long term. Measure this using the Manhattan distance, which in this situation is simply the sum of the absolute values of a particle's X, Y, and Z position.

For example, suppose you are only given two particles, both of which stay entirely on the X-axis (for simplicity). Drawing the current states of particles 0 and 1 (in that order) with an adjacent a number line and diagram of current X positions (marked in parenthesis), the following would take place:

p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>    -4 -3 -2 -1  0  1  2  3  4
p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>                         (0)(1)

p=< 4,0,0>, v=< 1,0,0>, a=<-1,0,0>    -4 -3 -2 -1  0  1  2  3  4
p=< 2,0,0>, v=<-2,0,0>, a=<-2,0,0>                      (1)   (0)

p=< 4,0,0>, v=< 0,0,0>, a=<-1,0,0>    -4 -3 -2 -1  0  1  2  3  4
p=<-2,0,0>, v=<-4,0,0>, a=<-2,0,0>          (1)               (0)

p=< 3,0,0>, v=<-1,0,0>, a=<-1,0,0>    -4 -3 -2 -1  0  1  2  3  4
p=<-8,0,0>, v=<-6,0,0>, a=<-2,0,0>                         (0)   
At this point, particle 1 will never be closer to <0,0,0> than particle 0, and so, in the long run, particle 0 will stay closest.

Which particle will stay closest to position <0,0,0> in the long term?
Your puzzle answer was 243.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

To simplify the problem further, the GPU would like to remove any particles that collide. Particles collide if their positions ever exactly match. Because particles are updated simultaneously, more than two particles can collide at the same time and place. Once particles collide, they are removed and cannot collide with anything else after that tick.

For example:

p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>    
p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>    -6 -5 -4 -3 -2 -1  0  1  2  3
p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>    (0)   (1)   (2)            (3)
p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>

p=<-3,0,0>, v=< 3,0,0>, a=< 0,0,0>    
p=<-2,0,0>, v=< 2,0,0>, a=< 0,0,0>    -6 -5 -4 -3 -2 -1  0  1  2  3
p=<-1,0,0>, v=< 1,0,0>, a=< 0,0,0>             (0)(1)(2)      (3)   
p=< 2,0,0>, v=<-1,0,0>, a=< 0,0,0>

p=< 0,0,0>, v=< 3,0,0>, a=< 0,0,0>    
p=< 0,0,0>, v=< 2,0,0>, a=< 0,0,0>    -6 -5 -4 -3 -2 -1  0  1  2  3
p=< 0,0,0>, v=< 1,0,0>, a=< 0,0,0>                       X (3)      
p=< 1,0,0>, v=<-1,0,0>, a=< 0,0,0>

------destroyed by collision------    
------destroyed by collision------    -6 -5 -4 -3 -2 -1  0  1  2  3
------destroyed by collision------                      (3)         
p=< 0,0,0>, v=<-1,0,0>, a=< 0,0,0>
In this example, particles 0, 1, and 2 are simultaneously destroyed at the time and place marked X. On the next tick, particle 3 passes through unharmed.

How many particles are left after all collisions are resolved?

         */
        [Fact]
        public void First()
        {
            var expected = 1;
            var input = new List<string>{"p=<-6,0,0>, v=<3,0,0>, a=<0,0,0>","p=<-4,0,0>, v=<2,0,0>, a=<0,0,0>","p=<-2,0,0>, v=<1,0,0>, a=<0,0,0>","p=<3,0,0>, v=<-1,0,0>, a=<0,0,0>"};

            var actual = CountRemaining(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Actual()
        {
            var expected = 648;
            var input = new List<string>();
            using(var file = new System.IO.StreamReader(@"..\..\..\Day20.txt"))
            {  
                string line;
                while((line = file.ReadLine()) != null)  
                {
                    input.Add(line);
                }
                file.Close();
            }

            var actual = CountRemaining(input);

            Assert.Equal(expected, actual);
        }


        private int CountRemaining(List<string> input)
        {
            var vectors = Parse(input);

            int closest = -1;
            
            var lastChanged = 0;
            var step = 0;

            while(step - lastChanged < 1000)
            {
                int closestValue = int.MaxValue;
                var innerClosest = -1;
                for(var i = 0; i < vectors.Count; i++)
                {
                    vectors[i].Step();
                    if (vectors[i].DistanceFromZero < closestValue)
                    {
                        innerClosest = i;
                        closestValue = vectors[i].DistanceFromZero;
                    }
                }
                if (closest != innerClosest)
                {
                    closest = innerClosest;
                    lastChanged = step;
                }
                step++;

                
                RemoveCollisions(vectors);
            }
            return vectors.Count();
        }

        private void RemoveCollisions(List<Vector> vectors)
        {
            var toRemove = new HashSet<Vector>();
            for (int i =0 ; i< vectors.Count; i++)
            {
                var vectori = vectors[i];
                for (int j = i + 1; j < vectors.Count; j++ )
                {
                    var vectorj = vectors[j];
                    if (vectori.P.X == vectorj.P.X && vectori.P.Y == vectorj.P.Y && vectori.P.Z == vectorj.P.Z)
                    {
                        toRemove.Add(vectori);
                        toRemove.Add(vectorj);
                    }
                }
            }
            vectors.RemoveAll(x => toRemove.Contains(x));
        }

        private List<Vector> Parse(List<string> input)
        {
            var regex = new Regex(@"^p=<([\d-,]+)>, v=<([\d-,]+)>, a=<([\d-,]+)>$");
            var vectors = new List<Vector>();
            foreach(var line in input)
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    var vector = new Vector();
                    vector.P = new Point3d(match.Groups[1].Value);
                    vector.V = new Point3d(match.Groups[2].Value);
                    vector.A = new Point3d(match.Groups[3].Value);
                    vectors.Add(vector);
                }
                else
                {
                    throw new ArgumentException($"{line} failed to parse");
                }
            }
            return vectors;
        }

        private class Vector
        {
            public Point3d P { get; set; }
            public Point3d V { get; set; }
            public Point3d A { get; set; }

            public void Step()
            {
                V.X += A.X;
                V.Y += A.Y;
                V.Z += A.Z;
                P.X += V.X;
                P.Y += V.Y;
                P.Z += V.Z;
            }

            public int DistanceFromZero { get {
                return Math.Abs(P.X) + Math.Abs(P.Y) + Math.Abs(P.Z);
            } }

            public override string ToString()
            {
                return $"p=<{P}>, v=<{V}>, a=<{A}>";
            }
        }

        private class Point3d {
            public Point3d() { }
            public Point3d(string input)
            {
                var split = input.Split(',');
                X = int.Parse(split[0]);
                Y = int.Parse(split[1]);
                Z = int.Parse(split[2]);
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public override string ToString()
            {
                return $"{X},{Y},{Z}";
            }
        }

    }
}
