using ConstructionStoreArzuTorg.Employee;
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

namespace ConstructionStoreArzuTorg.Add
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        public AddOrder()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                ClientComboBox.ItemsSource = db.Клиент.Select(x => x.Фамилия + " " + x.Имя).ToList();
                EmpComboBox.ItemsSource = db.Сотрудник.Select(x => x.Фамилия + " " + x.Имя).ToList();   
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in grid.Children)
            {
                
                if (control is ComboBox)
                {
                    var comboBox = (ComboBox)control;
                    if (comboBox.SelectedValue == null || comboBox.SelectedValue.ToString() == string.Empty)
                    {
                        MessageBox.Show("Ошибка");
                        return;
                    }
                }
                if (control is DatePicker)
                {
                    var datePicker = (DatePicker)control;
                    if (datepicker.SelectedDate == null)
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
            }
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                try
                {
                    var zakaz = new Заказ();
                    zakaz.ID_Клиента = db.Клиент.Where(x => x.Фамилия + " " + x.Имя == ClientComboBox.Text).FirstOrDefault().ID_Клиента;
                    zakaz.ID_Сотрудника = db.Сотрудник.Where(x => x.Фамилия + " " + x.Имя == EmpComboBox.Text).FirstOrDefault().ID_Сотрудника;
                    zakaz.Дата_заказа = (DateTime)datepicker.SelectedDate;
                   

                    db.Заказ.Add(zakaz);
                    db.SaveChanges();

                    new AddProductToOrder(zakaz).Show();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка ошибка");
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new OrderListView().Show();
            Close();
        }
    }
}
