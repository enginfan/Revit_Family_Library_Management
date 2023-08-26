using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleKill.Interfaces;

namespace TripleKill.ViewModels
{
    public class PublicCategoryCore : CategoryViewModel
    {
        public PublicCategoryCore(ICategory category) : base(category)
        {
            Model = category;
        }

        protected override List<CategoryViewModel> CreateChildren()
        {
            var children = Model.Children;
            if (children == null) return null;
            return children.Select(x => new PublicCategoryCore(x)).ToList<CategoryViewModel>();
        }
    }
}
