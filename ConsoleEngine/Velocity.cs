using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public struct Velocity
    {
        public readonly int X;

        public readonly int Y;

        public Velocity(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Velocity))
                return false;

            var velocityObj = (Velocity)obj;

            return velocityObj.X == X && velocityObj.Y == Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return X * 13 + Y;
            }
        }

        public override string ToString()
        {
            return "Velocity: " + X + "/" + Y;
        }

        public static bool operator ==(Velocity left, Velocity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Velocity left, Velocity right)
        {
            return !left.Equals(right);
        }
    }
}