using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public abstract class Component
    {
        public virtual void Update()
        { }

        public virtual void Setup(Entity entity)
        { }
    }
}