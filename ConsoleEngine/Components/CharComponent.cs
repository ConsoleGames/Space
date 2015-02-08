using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class CharComponent : Component
    {
        public char Char { get; set; }

        public CharComponent(char @char)
        {
            Char = @char;
        }

        public override bool Equals(object obj)
        {
            var charObj = obj as CharComponent;

            return !object.ReferenceEquals(charObj, null) && charObj.Char == Char;
        }

        public override int GetHashCode()
        {
            return Char.GetHashCode();
        }

        public override string ToString()
        {
            return Char.ToString();
        }

        public static bool operator ==(CharComponent left, CharComponent right)
        {
            return !object.ReferenceEquals(left, null) && left.Equals(right);
        }

        public static bool operator !=(CharComponent left, CharComponent right)
        {
            return !(left == right);
        }
    }
}