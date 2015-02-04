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

        private readonly Action<Entity, Entity> hitTarget;

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitTarget">First parameter is the target Entity, second the Entity being checked.</param>
        /// <param name="targets"></param>
        public CollisionComponent(Action<Entity, Entity> hitTarget, params Entity[] targets)
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

            var entityPosition = targetEntity.GetComponent<MovementComponent>().Position;
            foreach (var target in checks)
            {
                if (!target.HasComponent<MovementComponent>())
                    continue;

                if (entityPosition == target.GetComponent<MovementComponent>().Position)
                    hitTarget(targetEntity, target);
            }
        }
    }
}