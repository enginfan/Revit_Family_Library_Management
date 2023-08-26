using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleKill.Models
{
    public abstract class BaseModel
    {
        public string Id { get; }

        public string Name { get; set; }

        protected BaseModel(string id)
        {
            Id = id;
        }
    }
}
