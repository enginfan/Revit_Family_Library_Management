using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TripleKill.Models;
using TripleKill.ViewModels;

namespace TripleKill.Views
{
    /// <summary>
    /// FamilyBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class FamilyBrowser : Window
    {
        public FamilyBrowser()
        {
            var viewModel = new FamilyBrowserViewModel();
            viewModel.PlaceFamily += PlaceFamilyReal;
            viewModel.LoadFamily += LoadFamilyReal;
            DataContext = viewModel;
            InitializeComponent();
            //var items = DataContext as FamilyBrowserViewModel;
            //FamilyItems = new List<ContentEntityFacade>(items.Items);
        }
        public OperateFamily LoadFamily;
        public OperateFamily PlaceFamily;

        public void PlaceFamilyReal(string name)
        {
            this.Hide();
            PlaceFamily(name);
            this.ShowDialog();
        }

        public void LoadFamilyReal(string path)
        {
            LoadFamily(path);
        }
    }


}

