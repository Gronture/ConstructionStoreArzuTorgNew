using ConstructionStoreArzuTorg.Add;
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

namespace ConstructionStoreArzuTorg.Edit
{
    /// <summary>
    /// Логика взаимодействия для EditClientView.xaml
    /// </summary>
    public partial class EditClientView : Window
    {
        Клиент _client;
        public EditClientView(Клиент client)
        {
            InitializeComponent();
            _client = client;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FirstNameTextBox.Text = _client.Фамилия;
            SecondNameTextBox.Text = _client.Имя;
            LastNameTextBox.Text = _client.Отчество;
            PolTextBox.Text = _client.Пол;
            AgeTextBox.Text = _client.Возраст.ToString();
            PhoneTextBox.Text = _client.Телефон;
            AddressTextBox.Text = _client.Адрес;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ClientListView().Show();
            Close();
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
                var needObject = db.Клиент.Where(x => x.ID_Клиента == _client.ID_Клиента).FirstOrDefault();
                if (needObject != null)
                {
                    needObject.Фамилия = FirstNameTextBox.Text;
                    needObject.Имя = SecondNameTextBox.Text;
                    needObject.Отчество = LastNameTextBox.Text;
                    needObject.Пол = PolTextBox.Text;
                    needObject.Возраст = int.Parse(AgeTextBox.Text);
                    needObject.Телефон = PhoneTextBox.Text;
                    needObject.Адрес = AddressTextBox.Text;
                    db.SaveChanges();
                }                
            }
            new ClientListView().Show();
            Close();
        }
    }
}
