using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleKill.Interfaces
{
    public interface IContent : IModel
    {
        string CategoryId { get; }

        string ContentLocalPath { get; set; }


    }
}
