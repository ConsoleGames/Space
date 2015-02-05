using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public struct Vector2
    {
        public readonly int X;

        public readonly int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2(Vector2 old, int deltaX = 0, int deltaY = 0)
        {
            X = old.X + deltaX;
            Y = old.Y + deltaY;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
                return false;

            var vectorObj = (Vector2)obj;

            return vectorObj.X == X && vectorObj.Y == Y;
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
            return X + "/" + Y;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left, deltaX: right.X, deltaY: right.Y);
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left, deltaX: -right.X, deltaY: -right.Y);
        }

        public static Vector2 operator *(Vector2 left, int right)
        {
            return new Vector2(left.X * right, left.Y * right);
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !left.Equals(right);
        }
    }
}