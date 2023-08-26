using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleKill.Interfaces;

namespace TripleKill.Models
{
    public class FamilyContent : IContent
    {
        public string CategoryId { get; }

        public string ContentLocalPath { get; set; }

        public string Id { get; }

        public string Name { get; set; }
    }
}
