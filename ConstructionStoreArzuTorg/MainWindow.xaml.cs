using ConstructionStoreArzuTorg.Employee;
using ConstructionStoreArzuTorg.Manager;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConstructionStoreArzuTorg
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void EmpButton_Click(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                MessageBox.Show(db.Должность.FirstOrDefault().Название);
            }
            //new EmployeeMenu().Show();
            //Close();
        }

        private void ManagerButton_Click(object sender, RoutedEventArgs e)
        {
            new ManagerMenuView().Show();
            Close();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            new ReportMenu().Show();
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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
