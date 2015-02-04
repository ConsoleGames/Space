using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public struct Coordinate
    {
        public readonly int X;

        public readonly int Y;

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinate(Coordinate old, int deltaX = 0, int deltaY = 0)
        {
            X = old.X + deltaX;
            Y = old.Y + deltaY;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate))
                return false;

            var velocityObj = (Coordinate)obj;

            return velocityObj.X == X && velocityObj.Y == Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return X * 13 + Y;
            }
        }

        public static Coordinate operator +(Coordinate left, Velocity right)
        {
            return new Coordinate(left, deltaX: right.X, deltaY: right.Y);
        }

        public override string ToString()
        {
            return "Velocity: " + X + "/" + Y;
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !left.Equals(right);
        }
    }
}