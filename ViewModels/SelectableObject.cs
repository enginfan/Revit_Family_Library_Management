using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripleKill.Commands;
using TripleKill.Interfaces;

namespace TripleKill.ViewModels
{
    public abstract class SelectableObject : ObserverableObject, ISelectable
    {
        public bool IsNotSelected => !IsSelected;

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected == value)
                    return;
                isSelected = value;
                OnPropertyChanged();
                OnPropertyChanged("IsNotSelected");
                SelectionChanged?.Invoke(this, EventArgs.Empty);
                OnSelectionChanged();
            }
        }

        protected SelectableObject()
        {
            SelectCommand = new DelegateCommand((x) => IsSelected = !IsSelected);
        }

        public ICommand SelectCommand { get; protected set; }


        protected virtual void OnSelectionChanged()
        {
        }

        public event EventHandler SelectionChanged;
    }
}
