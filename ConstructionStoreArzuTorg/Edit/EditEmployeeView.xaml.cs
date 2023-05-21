using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для EditEmployeeView.xaml
    /// </summary>
    public partial class EditEmployeeView : Window
    {
        EmployeeUpd _emp = new EmployeeUpd();
        public EditEmployeeView(EmployeeUpd emp)
        {
            InitializeComponent();
            _emp = emp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var list = db.Должность.ToList();
                foreach (var item in list)
                    PositionComboBox.Items.Add(item.Название);
            }

            FirstNameTextBox.Text = _emp.Фамилия;
            SecondNameTextBox.Text = _emp.Имя;
            LastNameTextBox.Text = _emp.Отчество;
            PolTextBox.Text = _emp.Пол;
            StajTextBox.Text = _emp.Стаж.ToString();
            PhoneTextBox.Text = _emp.Телефон;
            PositionComboBox.Text = _emp.НазваниеДолжности;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
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
                if (control is ComboBox)
                {
                    var comboBox = (ComboBox)control;
                    if (comboBox.SelectedValue == null || comboBox.SelectedValue.ToString() == string.Empty)
                    {
                        MessageBox.Show("Ошибка");
                        return;
                    }
                }
            }
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var needObject = db.Сотрудник.Where(x => x.ID_Сотрудника == _emp.ID_Сотрудника).FirstOrDefault();
                if(needObject != null)
                {
                    needObject.Фамилия = FirstNameTextBox.Text;
                    needObject.Имя = SecondNameTextBox.Text;
                    needObject.Отчество = LastNameTextBox.Text;
                    needObject.ID_Должности = db.Должность.Where(x => x.Название == PositionComboBox.Text).FirstOrDefault().ID_Должности;
                    needObject.Телефон = PhoneTextBox.Text;
                    needObject.Пол = PolTextBox.Text;
                    needObject.Стаж = int.Parse(StajTextBox.Text);
                    db.SaveChanges();
                }
            }
            new EmployeeListView().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeListView().Show();
            Close();
        }
    }
}
