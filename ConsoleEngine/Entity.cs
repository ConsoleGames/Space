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

        public void AddComponents(params Component[] components)
        {
            foreach (var component in components)
            {
                var componentType = component.GetType();

                if (this.components.ContainsKey(componentType))
                    throw new ArgumentException("Entity already has this component!", "component");

                this.components.Add(componentType, component);
            }
        }

        public void Update()
        {
            foreach (var component in components.Values)
                component.Update();
        }

        public void Render()
        {
            if (!components.ContainsKey(typeof(MovementComponent)) || !components.ContainsKey(typeof(CharComponent)))
                return;

            var movement = (MovementComponent)components[typeof(MovementComponent)];
            Console.SetCursorPosition(movement.Position.X, movement.Position.Y);
            Console.Write(((CharComponent)components[typeof(CharComponent)]).Char);
        }
    }
}