using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripleKill.Interfaces;

namespace TripleKill.ViewModels
{
    public abstract class CategoryViewModel : SelectableObject, IModelCore<ICategory>
    {
        public ICategory Model { get; set; }

        public string CategoryId => Model.Id;

        protected CategoryViewModel(ICategory category, ICommand command = null)
        {
            Model = category;
            if (Model is INotifyPropertyChanged model)
                model.PropertyChanged += Model_PropertyChanged;
            Children = null;
            Command = command;

        }


        private List<CategoryViewModel> children;

        public List<CategoryViewModel> Children
        {
            get
            {
                List<CategoryViewModel> result;
                if ((result = children) == null)
                {
                    var list = CreateChildren();
                    result = children = list != null ? list.Select(delegate (CategoryViewModel c)
                        {
                            c.Parent = this;
                            c.Level = Level + 1;
                            return c;
                        }).ToList() : null;
                }
                return result;

                //if (children == null)
                //{
                //    children = CreateChildren();
                //    if (children != null)
                //        return children.Select(delegate (CategoryViewModel c)
                //        {
                //            c.Parent = this;
                //            c.Level = Level + 1;
                //            return c;
                //        }).ToList();
                //    return null;
                //}
                //return children;
            }
            set
            {
                children = value;
                OnPropertyChanged();
            }
        }

        public ICommand Command { get; }

        public override bool Equals(object obj)
        {
            CategoryViewModel categoryCore;
            return (categoryCore = obj as CategoryViewModel) != null && CategoryId.Equals(categoryCore.CategoryId);
        }
        public override int GetHashCode()
        {
            if (CategoryId != null) return CategoryId.GetHashCode();
            return int.MinValue;
        }

        private int level;

        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged();
            }
        }
        public string Name => Model.Name;


        public CategoryViewModel Parent { get; set; }


        protected abstract List<CategoryViewModel> CreateChildren();

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        protected override void OnSelectionChanged()
        {
            if (IsSelected)
            {
                if (Parent != null)
                {
                    var children = Parent.Children;
                    if (children != null)
                        children.ForEach(x => { if (!x.Equals(this)) x.IsSelected = false; });
                    Parent.OnSelectionChanged();
                    OnPropertyChanged("Subjects");
                }
            }
        }


    }
}
