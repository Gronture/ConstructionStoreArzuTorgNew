using ConstructionStoreArzuTorg.Add;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
    /// Логика взаимодействия для PositionMenu.xaml
    /// </summary>
    public partial class PositionMenu : Window
    {
        public PositionMenu()
        {
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = db.Должность.ToList();
            }
        }
        private void AddPositionButton_Click(object sender, RoutedEventArgs e)
        {
            new AddPosition().Show();
            Close();
        }

        private void DeletePositionButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedElement = grid.SelectedItem as Должность;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var findElement = db.Должность.Where(x => x.ID_Должности == selectedElement.ID_Должности).FirstOrDefault();
                db.Должность.Remove(findElement);
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
