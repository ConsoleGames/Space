using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public class EntityManager
    {
        public Dictionary<string, Dictionary<Type, Component>> Entities { get; private set; }

        public EntityManager()
        {
            Entities = new Dictionary<string, Dictionary<Type, Component>>();
        }

        public void AddEntity(string name, params Component[] components)
        {
            if (Entities.ContainsKey(name))
                throw new ArgumentException("Entity already exists!", "name");

            Entities.Add(name, components.ToDictionary(component => component.GetType()));
        }

        public void AddComponentsToEntity(string name, params Component[] components)
        {
            if (!Entities.ContainsKey(name))
                throw new ArgumentException("Entity doesn't exist!", "name");

            var entity = Entities[name];
            foreach (var component in components)
            {
                var type = component.GetType();
                if (entity.ContainsKey(type))
                    throw new Exception("Entity already contains component [" + type.FullName + "]!");

                entity.Add(type, component);
            }
        }

        public TComponent GetComponentForEntity<TComponent>(string name) where TComponent : Component
        {
            if (!Entities.ContainsKey(name))
                throw new ArgumentException("Entity doesn't exist!", "name");

            var entity = Entities[name];

            if (!entity.ContainsKey(typeof(TComponent)))
                throw new Exception("Entity doesn't have Component!");

            return (TComponent)entity[typeof(TComponent)];
        }
    }
}