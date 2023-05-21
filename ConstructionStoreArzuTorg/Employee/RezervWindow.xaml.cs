using ConstructionStoreArzuTorg.Add;
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
    /// Логика взаимодействия для RezervWindow.xaml
    /// </summary>
    public partial class RezervWindow : Window
    {
        public RezervWindow()
        {
            InitializeComponent();
            Load();
        }
        public void Load()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                clientComboBox.ItemsSource = db.Клиент.Select(x => x.Фамилия).ToList();
            }
        }
        private void AddRezerv_Click(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var newRezerv = new Резервация();
                newRezerv.Клиент = db.Клиент.FirstOrDefault(x => x.Фамилия == clientComboBox.SelectedItem.ToString()).ID_Клиента;
                newRezerv.Дата = (DateTime)calendar.SelectedDate;
                newRezerv.Статус = 1;
                newRezerv.Цена = 0;

                db.Резервация.Add(newRezerv);
                db.SaveChanges();

                new AddRezervWindow(newRezerv).Show();
                Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new RezervListWindow().Show();
            Close();
        }
    }
}
