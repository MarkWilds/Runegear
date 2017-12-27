using System;
using OpenTK;

namespace RuneGear.Geometry
{
    public class Line : ICloneable
    {
        public Vector3 Start { get; set; }

        public Vector3 End { get; set; }

        public Line()
        {
            Start = Vector3.Zero;
            End = Vector3.Zero;
        }

        public Line(Line line)
        {
            Line newLine = (Line)line.Clone();
            Start = newLine.Start;
            End = newLine.End;
        }

        public Line(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }

        public object Clone()
        {
            Line newLine = new Line
            {
                Start = Start,
                End = End
            };

            return newLine;
        }
    }
}
