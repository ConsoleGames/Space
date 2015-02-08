using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class BoundsCheckComponent<TCheckedComponent> : Component where TCheckedComponent : Component
    {
        private Entity target;
        private Action<Entity, TCheckedComponent> checkAndFix;

        public BoundsCheckComponent(Action<Entity, TCheckedComponent> checkAndFix)
        {
            this.checkAndFix = checkAndFix;
        }

        public override void Setup(Entity entity)
        {
            target = entity;
        }

        public override void Update()
        {
            if (!target.HasComponent<TCheckedComponent>())
                return;

            checkAndFix(target, target.GetComponent<TCheckedComponent>());
        }
    }
}