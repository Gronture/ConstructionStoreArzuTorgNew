using ConstructionStoreArzuTorg.ClassConnection;
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

namespace ConstructionStoreArzuTorg
{
    /// <summary>
    /// Логика взаимодействия для ReturnView.xaml
    /// </summary>
    public partial class ReturnView : Window
    {
        ЗаказанныеТовары products;
        public ReturnView()
        {
            InitializeComponent();
        }

        public ReturnView(ЗаказанныеТовары товары)
        {
            products = товары;
            InitializeComponent();
            LoadData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {

        }
        public void LoadData()
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var list = db.Товар
                 .Join(db.Единицы_измерения,
                     tovar => tovar.ID_Единицы_измерения,
                     pt => pt.ID_Измерений,
                     (tovar, pt) => new { Tovar = tovar, PT = pt })
                 .Join(db.РазмерыТовара,
                     joinResult => joinResult.Tovar.ID_Размеров,
                     param => param.ID_Размеров,
                     (joinResult, param) => new { Tovar = joinResult.Tovar, PT = joinResult.PT, Param = param })
                  .Join(db.Категория,
                     joinResult => joinResult.Tovar.ID_Категории,
                     param => param.ID_Категории,
                     (joinResult, param) => new { Tovar = joinResult.Tovar, PT = joinResult.PT, Param = param })
                  .Join(db.Сезонность,
                     joinResult => joinResult.Tovar.Сезонность,
                     param => param.ID,
                     (joinResult, param) => new { Tovar = joinResult.Tovar, PT = joinResult.PT, Param = param })
                  .Join(db.ЗаказанныеТовары,
                     joinResult => joinResult.Tovar.ID_Товара,
                     ord => ord.Товар,
                     (joinResult, ord) => new { Tovar = joinResult.Tovar, Param = joinResult.Param, Ord = ord, PT = joinResult.PT })
                  .Select(x => new ProductUpd
                 {
                     Название = x.Tovar.Название,
                     НазваниеКатегории = x.Tovar.Название,
                     Размеры = x.Tovar.РазмерыТовара.Размер,
                     ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                     Сезонность = x.Param.Название_сезона,
                     Ord = x.Ord.Заказ,
                     Count = x.Ord.Количество

                 }).ToList();
            }
        }
    }
}
