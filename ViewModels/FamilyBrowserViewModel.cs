using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripleKill.Commands;
using TripleKill.Interfaces;
using TripleKill.Models;

namespace TripleKill.ViewModels
{
    public class FamilyBrowserViewModel : BaseViewModel, IViewModel
    {
        private readonly List<FamilyContentViewModel> entities;

        private List<CategoryViewModel> categoryTree;

        public List<CategoryViewModel> CategoryTree
        {
            get { return categoryTree; }
            set
            {
                categoryTree = value;
                OnPropertyChanged("CategoryTree");
            }
        }


        private ObservableCollection<FamilyContentViewModel> items;

        public ObservableCollection<FamilyContentViewModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        private string inputtingKeyword;

        public string InputtingKeyword
        {
            get => inputtingKeyword;
            set
            {
                inputtingKeyword = value;
                OnPropertyChanged("InputtingKeyword");
            }
        }


        private CategoryViewModel rootCategory;

        public CategoryViewModel RootCategory
        {
            get { return rootCategory; }
            set
            {
                rootCategory = value;
                OnPropertyChanged("RootCategory");
            }
        }

        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                ICommand result;
                if ((result = searchCommand) == null)
                {
                    result = new DelegateCommand(delegate
                    {
                        Items = new ObservableCollection<FamilyContentViewModel>();
                        if (!string.IsNullOrEmpty(inputtingKeyword))
                            foreach (var item in entities)
                            {
                                if (item.Model.Name.Contains(inputtingKeyword))
                                    Items.Add(item);
                            }
                        else entities.ForEach(x => Items.Add(x));
                    });
                }
                return result;
            }
        }

        private CategoryViewModel selectedCategory;

        public CategoryViewModel SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (!object.Equals(selectedCategory, value))
                {
                    selectedCategory = value;
                    if (selectedCategory != null && !selectedCategory.IsSelected)
                        selectedCategory.IsSelected = true;
                    OnPropertyChanged("SelectedCategory");
                }
                Items = new ObservableCollection<FamilyContentViewModel>();
                foreach (var item in entities)
                {
                    if (selectedCategory.Model.Subjects.Contains(item.Model.Name))
                        Items.Add(item);
                }

            }
        }

        public FamilyBrowserViewModel()
        {
            GetCategoryTree();
            entities = items.ToList();
        }


        private void GetCategoryTree()
        {
            var path = @"C:\ProgramData\Autodesk\RVT 2020\Libraries\China";
            //var path = @"C:\Users\whutk\Desktop\测试族库\";
            items = new ObservableCollection<FamilyContentViewModel>();
            categoryTree = new List<CategoryViewModel> { GetCategoryTreeFromLocal(path) };
        }

        private CategoryViewModel GetCategoryTreeFromLocal(string path)
        {
            var root = new CategoryModel("所有构件");
            var rootCategroy = new PublicCategoryCore(root);
            GetChildrenCategory(path, rootCategroy);
            return rootCategroy;
        }

        private void GetChildrenCategory(string path, PublicCategoryCore rootCategroy)
        {
            var directory = new DirectoryInfo(path);
            var subDirectories = directory.GetDirectories();
            var files = directory.GetFiles();
            if (files.Length != 0)
            {
                var familyItems = new List<FamilyContentViewModel>();

                foreach (var item in files)
                {
                    if (item.Extension == ".rfa")
                    {
                        var family = new FamilyContent();
                        family.Name = item.Name.Substring(0, item.Name.Length - 4);
                        family.ContentLocalPath = item.FullName;
                        var entity = new FamilyContentViewModel(family);
                        entity.PlaceFamily += PlaceFamilyReal;
                        entity.LoadFamily += LoadFamilyReal;
                        familyItems.Add(entity);
                        var name = entity.Name;
                    }
                }
                if (familyItems.Count != 0)
                {
                    familyItems.ForEach(x => items.Add(x));
                    rootCategroy.Model.Subjects.AddRange(familyItems.Select(x => x.DisplayName).ToList());
                }

            }
            if (subDirectories.Length != 0)
            {
                foreach (var item in subDirectories)
                {
                    var name = item.Name;
                    if (name == "机械设备")
                    {
                        var tt = name;
                    }
                    var entity = new CategoryModel(name);
                    var thisCategory = new PublicCategoryCore(entity);
                    thisCategory.Parent = rootCategroy;
                    GetChildrenCategory(item.FullName, thisCategory);
                    rootCategroy.Children.Add(thisCategory);
                    
                }
                rootCategroy.Children.ForEach(x => rootCategroy.Model.Subjects.AddRange(x.Model.Subjects));
            }

        }
        public OperateFamily PlaceFamily;
        public OperateFamily LoadFamily;
        public void PlaceFamilyReal(string name)
        {
            PlaceFamily(name);
        }

        public void LoadFamilyReal(string path)
        {
            LoadFamily(path);
        }
    }
}
