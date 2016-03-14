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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Data;

namespace Vaper.WindowMediation.WindowCreation
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : UserControl
    {
        public SearchPage()
        {
            InitializeComponent();

            lvMain.Items.Filter = Filter;
        }




        private static IEnumerable<string> _PropertyBindings = new string[] { };

        private bool Filter(object obj)
        {
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if (property.GetValue(obj) == null) break;

                if (property.GetValue(obj).ToString().ToUpper().Contains(txtFilter.Text.ToUpper()))
                {
                    return true;
                }
            }

            return false;

            //foreach (string path in _PropertyBindings)
            //{
            //    PropertyInfo property = obj.GetType().GetProperty(path);
                
            //    if (property != null)
            //    {
            //        if (property.GetValue(obj).ToString().ToUpper().Contains(txtFilter.Text.ToUpper()))
            //        {
            //            return true;
            //        }
            //    }
            //}

            //return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_PropertyBindings.Count() == 0)
            {
                List<string> bindingPaths = new List<string>();

                foreach (GridViewColumn column in gvData.Columns)
                {
                    bindingPaths.Add(((Binding)column.DisplayMemberBinding).Path.Path);
                }

                _PropertyBindings = bindingPaths;
            }

            CollectionViewSource.GetDefaultView(lvMain.ItemsSource).Refresh();

            //lvMain.Items.IsLiveFiltering = true;
            //lvMain.Items.Filter = Filter;
            //lvMain.InvalidateProperty(ListView.ItemsSourceProperty);
        }
    }
}
