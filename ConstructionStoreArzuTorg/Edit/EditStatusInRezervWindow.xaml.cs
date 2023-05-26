using ConstructionStoreArzuTorg.Employee;
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

namespace ConstructionStoreArzuTorg.Edit
{
    /// <summary>
    /// Логика взаимодействия для EditStatusInRezervWindow.xaml
    /// </summary>
    public partial class EditStatusInRezervWindow : Window
    {
        Резервация rezerv;
        public EditStatusInRezervWindow(Резервация резервация)
        {
            InitializeComponent();
            rezerv = резервация;
            Load();
        }
        public void Load()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                StatusComboBox.ItemsSource = db.Статус.Select(x => x.Название).ToList();
                StatusComboBox.SelectedItem = db.Статус.FirstOrDefault(x => x.ID == rezerv.Статус).Название;
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                try
                {
                    var item = db.Резервация.FirstOrDefault(x => x.ID == rezerv.ID);
                    var status = db.Статус.FirstOrDefault(x => x.Название == StatusComboBox.SelectedItem.ToString());

                    item.Статус = status.ID;
                    db.SaveChanges();

                    var newOtrg = new Заказ();
                    newOtrg.Дата_заказа = DateTime.Now;
                    newOtrg.ID_Клиента = item.Клиент;
                    newOtrg.ID_Сотрудника = db.Сотрудник.First().ID_Сотрудника;

                    db.Заказ.Add(newOtrg);
                    db.SaveChanges();
                    var listRezProducts = db.РезервацияТоваров.Where(x => x.Резервирование == item.ID).ToList();

                    for (int i = 0; i < listRezProducts.Count; i++)
                    {
                        var newProduct = new ЗаказанныеТовары();
                        //newProduct.Статус = db.Статус.FirstOrDefault(x => x.Название == "Продан").ID;
                        newProduct.Заказ = newOtrg.ID_Заказа;
                        newProduct.Количество = listRezProducts[i].Количество;
                        newProduct.Товар = listRezProducts[i].Товар;

                        db.ЗаказанныеТовары.Add(newProduct);
                        db.SaveChanges();
                    }


                    new RezervListWindow().Show();
                    Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка"); 
                }
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
