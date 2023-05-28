using ConstructionStoreArzuTorg.Employee;
using ConstructionStoreArzuTorg.Report;
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

namespace ConstructionStoreArzuTorg.Manager
{
    /// <summary>
    /// Логика взаимодействия для ManagerMenuView.xaml
    /// </summary>
    public partial class ManagerMenuView : Window
    {
        public ManagerMenuView()
        {
            InitializeComponent();
        }

        private void EmployeeListButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeListView().Show();
            Close();
        }

        private void DiscountsButton_Click(object sender, RoutedEventArgs e)
        {
            new DiscountsMenu().Show();
            Close();
        }

        private void WarehouseButton_Click(object sender, RoutedEventArgs e)
        {
            new WarehouseView().Show();
            Close();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            new ReportMenu().Show();
            Close();
        }

        private void SuppliesButton_Click(object sender, RoutedEventArgs e)
        {
            new DeliveriesListView().Show();
            Close();
        }

        private void ProviderButton_Click(object sender, RoutedEventArgs e)
        {
            new ProviderListView().Show();
            Close();
        }
    }
}
