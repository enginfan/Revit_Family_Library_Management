using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleKill.Interfaces;

namespace TripleKill.Models
{
    public class CategoryModel : BaseModel, ICategory, IModel
    {
        private List<ICategory> children;
        public List<ICategory> Children
        {
            get
            {
                List<ICategory> results;
                if ((results = children) == null)
                    results = children = new List<ICategory>();
                return results;
            }
            set { children = value; }
        }
        public string ParentId { get; set; }
        public List<string> Subjects { get; set; }

        public CategoryModel(string id):base(id)
        {
            Name = id;
            Subjects = new List<string>();

        }
    }
}
