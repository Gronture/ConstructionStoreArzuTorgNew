using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Manager;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
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

namespace ConstructionStoreArzuTorg.Report
{
    /// <summary>
    /// Логика взаимодействия для ReportMenu.xaml
    /// </summary>
    public partial class ReportMenu : Window
    {
        public ReportMenu()
        {
            InitializeComponent();
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in grid.Children)
            {
                if (control is DatePicker)
                {
                    var datePicker = (DatePicker)control;
                    if (fromDate.SelectedDate == null && toDate.SelectedDate == null)
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
            }
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var tovars = db.ЗаказанныеТовары.ToList()
                     .GroupJoin(
                     db.Товар.ToList(),
                     cl => cl.Товар,
                     ci => ci.ID_Товара,
                     (cl, ci) => new { cl, ci }).SelectMany(
                     x => x.ci.DefaultIfEmpty(),
                     (one, two) => new ProductUpd()
                     {
                         Название = two.Название,
                         Count = one.cl.Количество,
                         Ord = one.cl.Заказ
                     }).ToList();


                var supply = db.Заказ.Where(x => x.Дата_заказа >= fromDate.SelectedDate && x.Дата_заказа <= toDate.SelectedDate).ToList();
                decimal price = 0;
                foreach (var syp in supply)
                    price += syp.Сумма;


                var popularProduct = tovars.Max(x => x.Count);
                var item = tovars.FirstOrDefault(a => a.Count == popularProduct);

                HotProductTextBox.Text = item.Название;
                SumTextBox.Text = decimal.Parse(price.ToString("N", CultureInfo.GetCultureInfo("ru-RU"))).ToString() + " BYN";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ManagerMenuView().Show();
            Close();
        }
    }
}
