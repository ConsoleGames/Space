using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Components
{
    public class ControlComponent<TControlOption> : Component
    {
        private Entity target;
        private ControlOptionsProvider<TControlOption> controlOptionsProvider;
        private Dictionary<TControlOption, Action<Entity>> controls;

        public ControlComponent(Dictionary<TControlOption, Action<Entity>> controls, ControlOptionsProvider<TControlOption> controlOptionsProvider)
        {
            this.controls = controls;
            this.controlOptionsProvider = controlOptionsProvider;
        }

        public override void Setup(Entity entity)
        {
            target = entity;
        }

        public override void Update()
        {
            controls[controlOptionsProvider.GetControlOption()](target);
        }
    }
}