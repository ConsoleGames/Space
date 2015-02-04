using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class RenderComponent : Component
    {
        private Entity target;

        public override void Setup(Entity entity)
        {
            target = entity;
        }

        public override void Update()
        {
            if (!target.HasComponent<MovementComponent>() || !target.HasComponent<CharComponent>())
                return;

            var movement = target.GetComponent<MovementComponent>();
            Console.SetCursorPosition(movement.Position.X, movement.Position.Y);
            Console.Write(target.GetComponent<CharComponent>().Char);
        }
    }
}