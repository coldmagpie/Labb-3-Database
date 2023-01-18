using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lab3_database.DataModels;
using Lab3_database.Managers;

namespace Lab3_database.ViewModels
{
    public class CreateCategoryViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        private CategoryManager _categoryManager;
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateConfirmCommand { get; }
        public IRelayCommand NavigateUpdateCommand { get; }
        public IRelayCommand NavigateDeleteCommand { get; }

        private string _newCategory;
        public string NewCategory
        {
            get { return _newCategory; }
            set
            {
                SetProperty(ref _newCategory, value); 
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }
        private string _newName;
        public string NewName
        {
            get { return _newName; }
            set
            {
                SetProperty(ref _newName, value);
                NavigateUpdateCommand.NotifyCanExecuteChanged();
                NavigateUpdateCommand.NotifyCanExecuteChanged();
            }
        }
        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                SetProperty(ref _selectedCategory, value);
                NavigateDeleteCommand.NotifyCanExecuteChanged();
                NavigateUpdateCommand.NotifyCanExecuteChanged();

                if (_selectedCategory != null)
                {
                    NewName = _selectedCategory.Name;
                }
            }
        }

        public CreateCategoryViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _categoryManager = new CategoryManager();
            _navigationManager = navigationManager;
            _categories = new ObservableCollection<Category>(_categoryManager.GetAllCategories());
            NavigateGoBackCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new HomeViewModel(_navigationManager));
            NavigateConfirmCommand = new RelayCommand(CreateCategory, ()=> !string.IsNullOrEmpty(NewCategory));
            NavigateUpdateCommand = new RelayCommand(UpdateCategory, () => _selectedCategory != null);
            NavigateDeleteCommand = new RelayCommand(DeleteCategory, () => _selectedCategory != null);
        }

        private void CreateCategory()
        {
            _categoryManager.CreateCategory(NewCategory);
            UpdateCategories();
            NewCategory = string.Empty;
        }

        private void UpdateCategory()
        {
            _categoryManager.UpdateCategory(_selectedCategory.Id, NewName);
            UpdateCategories();
            NewName = string.Empty;
        }
        private void DeleteCategory()
        {
            _categoryManager.DeleteCategory(_selectedCategory.Id);
            UpdateCategories();
            NewName = string.Empty;
        }
        private void UpdateCategories()
        {
            Categories.Clear();
            foreach (var category in _categoryManager.GetAllCategories())
            {
                Categories.Add(category);
            }
        }
    }
}
