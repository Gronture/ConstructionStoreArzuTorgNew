using ConstructionStoreArzuTorg.Employee;
using ConstructionStoreArzuTorg.Manager;
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
    /// Логика взаимодействия для AddProvider.xaml
    /// </summary>
    public partial class AddProvider : Window
    {
        public AddProvider()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in grid.Children)
            {
                if (control is TextBox)
                {
                    var textbox = (TextBox)control;
                    if (textbox.Text == string.Empty)
                    {
                        MessageBox.Show("Ошибка");
                        return;
                    }

                }
            }
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                Поставщик поставщик = new Поставщик();
                поставщик.Наименование = NameTextBox.Text;
                поставщик.Расчётный_счёт = int.Parse(RSTextBox.Text);
                поставщик.Учётный_номер_плательщика = int.Parse(NumPlatTextBox.Text);
                поставщик.Название_банка = NameBankTextBox.Text;
                поставщик.Код_банка = CodeBankTextBox.Text;
                поставщик.Адрес = AddressTextBox.Text;
                поставщик.ФИО = FIOTextBox.Text;
                поставщик.Должность = PosTextBox.Text;
                поставщик.Почта = EmailTextBox.Text;
                db.Поставщик.Add(поставщик);
                db.SaveChanges();
            }
            new ProviderListView().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ProviderListView().Show();
            Close();
        }
    }
}
