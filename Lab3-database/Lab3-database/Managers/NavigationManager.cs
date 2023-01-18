using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Lab3_database.ViewModels;

namespace Lab3_database.Managers
{
    public class NavigationManager
    {
        private ObservableObject _currentViewModel;

            public ObservableObject CurrentViewModel
            {
                get { return _currentViewModel; }
                set
                {
                    _currentViewModel = value;
                    OnCurrentViewModelChanged();
                }
            }

            private void OnCurrentViewModelChanged()
            {
                CurrentViewModelChanged?.Invoke();
            }

            public event Action CurrentViewModelChanged;
        }
}
