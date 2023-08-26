using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleKill.Interfaces
{
    public interface ICategory : IModel
    {
        List<ICategory> Children { get; set; }

        string ParentId { get; set; }

        List<string> Subjects { get; set; }


    }
}
