using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Production.DB;
using Production.ViewModels;

namespace Production.Pages.EditingPages
{
    public partial class MaterialSuppliersEditPage : Page
    {
        public ObservableCollection<SupplierViewModel> Suppliers { get; set; }
        //public ObservableCollection<Supplier> Suppliers { get; set; }
        public ObservableCollection<Supplier> SelectedSuppliers { get; private set; }

        public MaterialSuppliersEditPage(Material current_material)
        {
            //Suppliers = new ObservableCollection<Supplier>(DBContext.GetContext().Suppliers.ToList());
            var suppliersFromDb = DBContext.GetContext().Suppliers.ToList();
            Suppliers = new ObservableCollection<SupplierViewModel>(suppliersFromDb.Select(s => new SupplierViewModel(s)).ToList());

            SelectedSuppliers = new ObservableCollection<Supplier>(current_material.Suppliers);
            InitializeComponent();
            foreach (var supplierViewModel in Suppliers)
            {
                supplierViewModel.IsSelected = SelectedSuppliers.Contains(supplierViewModel.Supplier);
            }
            DataContext = this;
        }

        private void Supplier_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                Supplier supplier = checkBox.DataContext as Supplier;
                if (supplier != null)
                {
                    if (checkBox.IsChecked == true)
                    {
                        if (!SelectedSuppliers.Contains(supplier))
                        {
                            SelectedSuppliers.Add(supplier);
                        }
                    }
                    else
                    {
                        if (SelectedSuppliers.Contains(supplier))
                        {
                            SelectedSuppliers.Remove(supplier);
                        }
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
