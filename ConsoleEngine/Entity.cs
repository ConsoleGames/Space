using ConsoleEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public class Entity
    {
        private readonly Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public Entity(params Component[] components)
        {
            AddComponents(components);
        }

        public TComponent GetComponent<TComponent>() where TComponent : Component
        {
            var componentType = typeof(TComponent);

            if (!components.ContainsKey(componentType))
                throw new Exception("Entity doesn't have Component!");

            return (TComponent)components[componentType];
        }

        public bool HasComponent<TComponent>() where TComponent : Component
        {
            return components.ContainsKey(typeof(TComponent));
        }

        public void AddComponents(params Component[] components)
        {
            foreach (var component in components)
            {
                var componentType = component.GetType();

                if (this.components.ContainsKey(componentType))
                    throw new ArgumentException("Entity already has this component!", "component");

                component.Setup(this);
                this.components.Add(componentType, component);
            }
        }

        public void Update()
        {
            foreach (var component in components.Values)
                component.Update();
        }
    }
}