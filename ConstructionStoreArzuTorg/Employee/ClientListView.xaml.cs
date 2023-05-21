using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Edit;
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

namespace ConstructionStoreArzuTorg.Employee
{
    /// <summary>
    /// Логика взаимодействия для ClientListView.xaml
    /// </summary>
    public partial class ClientListView : Window
    {
        public ClientListView()
        {
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = db.Клиент.ToList();
            }
        }

        public List<Клиент> GetClient()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var returnList = db.Клиент.ToList();
                return returnList;
            }
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            new AddClientView().Show();
            Close();
        }

        private void EditCleintButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as Клиент;
            new EditClientView(selectedItem).Show();
            Close();
        }

        private void SortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var list = GetClient();
            grid.ItemsSource = list.Where(x => x.Фамилия.ToLower().Contains(SortTextBox.Text)).ToList();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeMenu().Show();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedElement = grid.SelectedItem as Клиент;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var findElement = db.Клиент.Where(x => x.ID_Клиента == selectedElement.ID_Клиента).FirstOrDefault();
                db.Клиент.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }
    }
}
