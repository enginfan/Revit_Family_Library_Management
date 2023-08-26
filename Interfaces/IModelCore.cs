using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleKill.Interfaces
{
    public interface IModelCore<out T> where T : class
    {
        T Model { get; }
    }
}
