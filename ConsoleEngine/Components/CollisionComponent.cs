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

        private readonly Action<Entity, Entity, Vector2> hitTarget;
        private readonly Func<IEnumerable<Entity>> getChecks;

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitTarget">First parameter is the target Entity, second the Entity being checked, third the position where they collided.</param>
        /// <param name="getChecks">Has to return the Entities that get checked.</param>
        public CollisionComponent(Action<Entity, Entity, Vector2> hitTarget, Func<IEnumerable<Entity>> getChecks)
        {
            this.hitTarget = hitTarget;
            this.getChecks = getChecks;
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
            foreach (var check in getChecks())
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