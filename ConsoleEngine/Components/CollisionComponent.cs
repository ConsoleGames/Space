using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class CollisionComponent : Component
    {
        private Entity targetEntity;
        private readonly Entity[] checks;

        private readonly Action<Entity, Entity, Vector2> hitTarget;

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitTarget">First parameter is the target Entity, second the Entity being checked, third the position where they collided.</param>
        /// <param name="targets"></param>
        public CollisionComponent(Action<Entity, Entity, Vector2> hitTarget, params Entity[] targets)
        {
            this.hitTarget = hitTarget;
            this.checks = targets;
        }

        public override void Setup(Entity entity)
        {
            this.targetEntity = entity;
        }

        public override void Update()
        {
            if (!targetEntity.HasComponent<MovementComponent>())
                return;

            var entityMovement = targetEntity.GetComponent<MovementComponent>();
            foreach (var check in checks)
            {
                if (!check.HasComponent<MovementComponent>())
                    continue;

                Vector2 solution = new Vector2(-1, -1);
                if (MovementComponent.HaveCollided(entityMovement, check.GetComponent<MovementComponent>(), ref solution))
                    hitTarget(targetEntity, check, solution);
            }
        }
    }
}