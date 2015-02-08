using ConsoleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    internal class AIControlOptionsProvider : ControlOptionsProvider<ShipControlOptions>
    {
        private Random random = new Random();

        public override ShipControlOptions GetControlOption()
        {
            var rand = random.Next(0, 15);
            return (ShipControlOptions)(rand > 3 ? 0 : rand);
        }
    }
}