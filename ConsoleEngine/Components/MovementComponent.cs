using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class MovementComponent : Component
    {
        public Vector2 Velocity { get; set; }

        public Vector2 Position { get; set; }

        private Vector2 lastPosition;

        private LineSegment lastMove
        {
            get { return new LineSegment(lastPosition, Velocity); }
        }

        public Vector2 NextPosition
        {
            get { return Position + Velocity; }
        }

        public MovementComponent(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public MovementComponent(Vector2 position)
            : this(position, new Vector2())
        { }

        public override string ToString()
        {
            return "Coordinates: " + Position + ", Velocity: " + Velocity;
        }

        public override void Update()
        {
            lastPosition = Position;
            Position = NextPosition;
        }

        public static bool HaveCollided(MovementComponent left, MovementComponent right, ref Vector2 solution)
        {
            return LineSegment.Intersect(left.lastMove, right.lastMove, ref solution);
        }
    }
}