using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class CoordinateComponent : Component
    {
        public uint X { get; set; }

        public uint Y { get; set; }

        public CoordinateComponent(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            var coordObj = obj as CoordinateComponent;

            return !object.ReferenceEquals(coordObj, null) && coordObj.X == X && coordObj.Y == Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return X.GetHashCode() * 13 + Y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return X + "/" + Y;
        }

        public static bool operator ==(CoordinateComponent left, CoordinateComponent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CoordinateComponent left, CoordinateComponent right)
        {
            return !left.Equals(right);
        }
    }
}