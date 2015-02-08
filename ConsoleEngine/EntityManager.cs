using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public class EntityManager
    {
        public readonly List<Entity> Entities = new List<Entity>();

        public void AddEntity(params Entity[] entities)
        {
            foreach (var entity in entities)
            {
                if (Entities.Contains(entity))
                    continue;

                Entities.Add(entity);
            }
        }

        public void UpdateEntities()
        {
            foreach (var entity in Entities)
                entity.Update();
        }
    }
}