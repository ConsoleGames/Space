using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class MovementComponent : Component
    {
        public Velocity Velocity { get; set; }

        public Coordinate Position { get; set; }

        public MovementComponent(Coordinate position, Velocity velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public MovementComponent(Coordinate position)
            : this(position, new Velocity())
        { }

        public override string ToString()
        {
            return Position + ", " + Velocity;
        }

        public override void Update()
        {
            Position += Velocity;
        }
    }
}