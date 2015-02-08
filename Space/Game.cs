using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    public static class Game
    {
        public static int FieldHeight
        {
            get { return Console.WindowHeight - 3; }
            set { Console.WindowHeight = value + 3; }
        }

        public static int FieldWidth
        {
            get { return Console.WindowWidth; }
            set
            {
                Console.WindowWidth = value;
                SeparationLine = "".PadLeft(value, '-');
            }
        }

        public static string SeparationLine { get; private set; }

        public static int Points { get; set; }

        public static int Shots { get; set; }

        public static int Asteroids { get { return (Game.FieldWidth * Game.FieldHeight) / 60; } }
    }
}