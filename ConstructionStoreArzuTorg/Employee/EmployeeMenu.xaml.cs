using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ConstructionStoreArzuTorg.Employee
{
    /// <summary>
    /// Логика взаимодействия для EmployeeMenu.xaml
    /// </summary>
    public partial class EmployeeMenu : Window
    {
        public EmployeeMenu()
        {
            InitializeComponent();
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            new ClientListView().Show();
            Close();
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductListView().Show();
            Close();
        }
        
        private void OdrerButton_Click(object sender, RoutedEventArgs e)
        {
            new OrderListView().Show();
            Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            string commandText = "D:\\Проекты\\ConstructionStoreArzuTorg\\ConstructionStoreArzuTorg\\HelpConstructionStore.chm";
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = commandText;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void RezervButton_Click(object sender, RoutedEventArgs e)
        {
            new RezervListWindow().Show();
            Close();
        }
    }
}
