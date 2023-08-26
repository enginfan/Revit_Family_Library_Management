using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleKill.Interfaces;
using TripleKill.Models;

namespace TripleKill.ViewModels
{
    public abstract class BaseViewModel : ObserverableObject, IViewModel
    {
        public string Id { get; }

        public string Name => GetType().Name;

        public void TryInvoke(Action action)
        {

        }
    }
}
