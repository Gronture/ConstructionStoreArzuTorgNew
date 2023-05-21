using ConstructionStoreArzuTorg.Add;
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
    /// Логика взаимодействия для DiscountsMenu.xaml
    /// </summary>
    public partial class DiscountsMenu : Window
    {
        public DiscountsMenu()
        {
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = db.Сезонность.ToList();
            }
        }
        private void AddDiscountsButton_Click(object sender, RoutedEventArgs e)
        {
            new AddDiscounts().Show();
            Close();
        }

        private void DeleteDiscountsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedElement = grid.SelectedItem as Сезонность;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var findElement = db.Сезонность.Where(x => x.ID == selectedElement.ID).FirstOrDefault();
                db.Сезонность.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ManagerMenuView().Show();
            Close();
        }
    }
}
