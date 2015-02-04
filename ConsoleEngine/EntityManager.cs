using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public class EntityManager
    {
        public readonly Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();

        public void AddEntity(string name, Entity entity)
        {
            if (Entities.ContainsKey(name))
                throw new ArgumentException("Entity [" + name + "] already exists!", "name");

            Entities.Add(name, entity);
        }

        public Entity GetEntity(string name)
        {
            if (!Entities.ContainsKey(name))
                throw new KeyNotFoundException("Entity [" + name + "] doesn't exist!");

            return Entities[name];
        }

        public void UpdateEntities()
        {
            foreach (var entity in Entities.Values)
                entity.Update();
        }
    }
}