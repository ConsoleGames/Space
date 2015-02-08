using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public struct LineSegment
    {
        public readonly Vector2 Length;
        public readonly Vector2 Start;

        public Vector2 End
        {
            get { return Start + Length; }
        }

        public LineSegment(Vector2 start, Vector2 length)
        {
            Start = start;
            Length = length;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LineSegment))
                return false;

            var lineObj = (LineSegment)obj;

            return lineObj.Start == Start && lineObj.Length == Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Start.GetHashCode() * 13 + Length.GetHashCode();
            }
        }

        public override string ToString()
        {
            return "Start: " + Start.ToString() + ", End: " + End.ToString();
        }

        public static bool Intersect(LineSegment left, LineSegment right, ref Vector2 solution)
        {
            if (left.Start == right.Start)
            {
                solution = left.Start;
                return true;
            }
            else if (left.Start + left.Length == right.Start + right.Length)
            {
                solution = left.Start + left.Length;
                return true;
            }

            if (left.Start.Y == right.Start.Y && left.Length.Y == 0 && right.Length.Y == 0)
            {
                var leftEnd = left.Start + left.Length;
                var rightEnd = right.Start + right.Length;

                if ((left.Start.X < right.Start.X && leftEnd.X > rightEnd.X) || (left.Start.X > right.Start.X && leftEnd.X < rightEnd.X))
                {
                    var difference = Math.Abs(leftEnd.X - rightEnd.X);
                    solution = new Vector2(Math.Min(left.Start.X, right.Start.X) + difference, left.Start.Y);
                    return true;
                }
            }
            else if (left.Start.X == right.Start.X && left.Length.X == 0 && right.Length.X == 0)
            {
                var leftEnd = left.Start + left.Length;
                var rightEnd = right.Start + right.Length;

                if ((left.Start.Y < right.Start.Y && leftEnd.Y > rightEnd.Y) || (left.Start.Y > right.Start.Y && leftEnd.Y < rightEnd.Y))
                {
                    var difference = Math.Abs(leftEnd.Y - rightEnd.Y);
                    solution = new Vector2(left.Start.X, Math.Min(left.Start.Y, right.Start.Y) + difference);
                    return true;
                }
            }
            return false;

            //float d = left.Length.X * right.Length.Y - left.Length.Y * right.Length.X;

            //if (d == 0)
            //    return false;

            //var leftToRightStart = left.Start - right.Start;
            //var r = (float)(leftToRightStart.Y * right.Length.X - leftToRightStart.X * right.Length.Y) / d;
            //var s = (float)(leftToRightStart.Y * left.Length.X - leftToRightStart.X * left.Length.Y) / d;

            //if (r < 0 && r > 1 && s < 0 && s > 1)
            //    return false;

            //solution = left.Start + left.Length * (int)r;
            //return true;
        }

        public static bool operator ==(LineSegment left, LineSegment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LineSegment left, LineSegment right)
        {
            return !left.Equals(right);
        }
    }
}