using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class RenderComponent : Component
    {
        private Entity entity;

        public override void Setup(Entity entity)
        {
            this.entity = entity;
        }

        public override void Update()
        {
            if (!entity.HasComponent<MovementComponent>() || !entity.HasComponent<CharComponent>())
                return;

            var movement = entity.GetComponent<MovementComponent>();
            Console.SetCursorPosition(movement.Position.X, movement.Position.Y);
            Console.Write(entity.GetComponent<CharComponent>().Char);
        }
    }
}