using ConstructionStoreArzuTorg.Manager;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// Логика взаимодействия для AddDiscounts.xaml
    /// </summary>
    public partial class AddDiscounts : Window
    {
        public AddDiscounts()
        {
            InitializeComponent();
        }

        private void AddDiscountsButton_Click(object sender, RoutedEventArgs e)
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
                Сезонность скидки = new Сезонность();
                скидки.Название_сезона = SeasonTextBox.Text;
                скидки.Процент = int.Parse(DiscountsTextBox.Text);
                db.Сезонность.Add(скидки);
                db.SaveChanges();
            }
            new DiscountsMenu().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new DiscountsMenu().Show();
            Close();
        }
    }
}
