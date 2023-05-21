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

namespace ConstructionStoreArzuTorg.Manager
{
    /// <summary>
    /// Логика взаимодействия для WarehouseView.xaml
    /// </summary>
    public partial class WarehouseView : Window
    {
        public WarehouseView()
        {
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = GetWarehouse();
            }
        }

        public List<WarehouseUpd> GetWarehouse()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var result = db.Склад.ToList().GroupJoin(
                    db.Товар.ToList(),
                    cl => cl.Товар,
                    ci => ci.ID_Товара,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (house, product) => new WarehouseUpd
                    {
                        ID = house.cl.ID,
                        НазваниеТовара = product?.Название,
                        Количество = house.cl.Количество,
                    }).ToList();

                var result2 = db.Товар.ToList().GroupJoin(
                   db.Категория.ToList(),
                   cl => cl.ID_Категории,
                   ci => ci.ID_Категории,
                   (cl, ci) => new { cl, ci })
                   .SelectMany(x => x.ci.DefaultIfEmpty(),
                   (tovar, kat) => new WarehouseUpd
                   {
                       ID = tovar.cl.ID_Категории,
                       НазваниеКатегории = kat.Название,
                   }).ToList();

                for (int i = 0; i < result.Count; i++)
                {
                    result[i].НазваниеКатегории = result2[i].НазваниеКатегории;
                }


                return result;
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ManagerMenuView().Show();
            Close();
        }
    }
}
