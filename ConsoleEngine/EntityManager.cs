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

        private readonly Dictionary<Mode, List<Entity>> update = new Dictionary<Mode, List<Entity>>
            {
                { Mode.Add, new List<Entity>() },
                { Mode.Remove, new List<Entity>() }
            };

        private bool updating;

        public void AddEntities(params Entity[] entities)
        {
            if (updating)
            {
                foreach (var entity in entities)
                    update[Mode.Add].Add(entity);
            }
            else
            {
                foreach (var entity in entities)
                {
                    if (Entities.Contains(entity))
                        continue;

                    Entities.Add(entity);
                }
            }
        }

        public void RemoveEntities(params Entity[] entities)
        {
            if (updating)
            {
                foreach (var entity in entities)
                    update[Mode.Remove].Add(entity);
            }
            else
            {
                foreach (var entity in entities)
                    Entities.Remove(entity);
            }
        }

        public void UpdateEntities()
        {
            updating = true;

            foreach (var entity in Entities)
                entity.Update();

            updating = false;

            AddEntities(update[Mode.Add].ToArray());
            update[Mode.Add].Clear();

            RemoveEntities(update[Mode.Remove].ToArray());
            update[Mode.Remove].Clear();
        }

        private enum Mode
        {
            Add,
            Remove
        }
    }
}