using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleKill.Interfaces
{
    public interface IViewModel
    {
        string Id { get; }
        string Name { get;}
        void TryInvoke(Action action);
    }
}
