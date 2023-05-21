using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.ClassConnection;
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
    /// Логика взаимодействия для EditProductView.xaml
    /// </summary>
    public partial class EditProductView : Window
    {
        ProductUpd _product = new ProductUpd();
        public EditProductView(ProductUpd product)
        {
            InitializeComponent();
            _product = product;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            //{
            //    var firstlist = db.Категория.ToList();
            //    foreach (var item in firstlist)
            //        CategoryComboBox.Items.Add(item.Название);

            //    var secontlist = db.РазмерыТовара.ToList();
            //    foreach (var item in secontlist)
            //        DimensionsComboBox.Items.Add(item.Размер);

            //    var thristlist = db.Единицы_измерения.ToList();
            //    foreach (var item in thristlist)
            //        UnitComboBox.Items.Add(item.Название);

            //    var lastlist = db.Скидки.ToList();
            //    foreach (var item in lastlist)
            //        DiscountComboBox.Items.Add(item.Процент);
            //}
            //NameTextBox.Text = _product.Название;
            //ManufacturerTextBox.Text = _product.Производитель;
            //CountryTextBox.Text = _product.Страна_происхождения;
            //CategoryComboBox.Text = _product.НазваниеКатегории;
            //DimensionsComboBox.Text = _product.Размеры;
            //UnitComboBox.Text = _product.ЕдиницаИзмерения;
            //PriceTextBox.Text = _product.Стоимость.ToString();
            //DiscountComboBox.Text = _product.Скидка.ToString();
        }

        private void AcceptProductButton_Click(object sender, RoutedEventArgs e)
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
                //var needObject = db.Товар.Where(x => x.ID_Товара == _product.ID_Товара).FirstOrDefault();
                //if (needObject != null)
                //{
                //    int disco = int.Parse(DiscountComboBox.Text);
                //    needObject.ID_Категории = db.Категория.Where(x => x.Название == CategoryComboBox.Text).FirstOrDefault().ID_Категории;
                //    needObject.ID_Размеров = db.РазмерыТовара.Where(x => x.Размер == DimensionsComboBox.Text).FirstOrDefault().ID_Размеров;
                //    needObject.ID_Единицы_измерения = db.Единицы_измерения.Where(x => x.Название == UnitComboBox.Text).FirstOrDefault().ID_Измерений;
                //    needObject.Скидка = db.Скидки.Where(x => x.Процент == disco).FirstOrDefault().ID;
                //    needObject.Название = NameTextBox.Text;
                //    needObject.Производитель = ManufacturerTextBox.Text;
                //    needObject.Страна_происхождения = CountryTextBox.Text;
                //    needObject.Стоимость = decimal.Parse(PriceTextBox.Text);
                //    db.SaveChanges();
                //}
            }
            new ProductListView().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductListView().Show();
            Close();
        }
    }
}
