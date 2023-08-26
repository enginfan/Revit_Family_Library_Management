using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleKill.Interfaces
{
    public interface ISelectable
    {
        bool IsNotSelected { get;}
        bool IsSelected { get; set; }

        event EventHandler SelectionChanged;
    }
}
