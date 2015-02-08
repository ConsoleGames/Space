using ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    internal class PlayerControlOptionsProvider : ControlOptionsProvider<ShipControlOptions>
    {
        public override ShipControlOptions GetControlOption()
        {
            if (!Console.KeyAvailable)
                return ShipControlOptions.None;

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    return ShipControlOptions.Up;

                case ConsoleKey.DownArrow:
                    return ShipControlOptions.Down;

                case ConsoleKey.Spacebar:
                    return ShipControlOptions.Shoot;

                default:
                    return ShipControlOptions.None;
            }
        }
    }
}