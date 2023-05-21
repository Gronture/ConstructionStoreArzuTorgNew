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
    /// Логика взаимодействия для AddPosition.xaml
    /// </summary>
    public partial class AddPosition : Window
    {
        public AddPosition()
        {
            InitializeComponent();
        }

        private void AddPositionButton_Click(object sender, RoutedEventArgs e)
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
                Должность должность = new Должность();
                должность.Название = NameTextBox.Text;
                должность.Оклад = int.Parse(SalaryTextBox.Text);
                db.Должность.Add(должность);
                db.SaveChanges();
            }
            new PositionMenu().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new PositionMenu().Show();
            Close();
        }
    }
}
