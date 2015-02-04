using ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    internal class PlayerControlOptionsProvider : ControlOptionsProvider<PlayerControlOptions>
    {
        public override bool CanGetControlOptions
        {
            get { return Console.KeyAvailable; }
        }

        public override PlayerControlOptions GetControlOption()
        {
            if (!CanGetControlOptions)
                return PlayerControlOptions.None;

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    return PlayerControlOptions.Up;

                case ConsoleKey.DownArrow:
                    return PlayerControlOptions.Down;

                default:
                    return PlayerControlOptions.None;
            }
        }
    }
}