using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Employee;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    /// Логика взаимодействия для AddRezervWindow.xaml
    /// </summary>
    public partial class AddRezervWindow : Window
    {
        Резервация _sell;
        public AddRezervWindow(Резервация rez)
        {
            InitializeComponent();
            _sell = rez;
            LoadData();
        }

        public void ClearComboBox()
        {
            ProductComboBox.Text = string.Empty;
            RazmerComboBox.Text = string.Empty;
            CategorComboBox.Text = string.Empty;
            EdIzmComboBox.Text = string.Empty;
        }
        public void UpdateView()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                tovarsGrid.ItemsSource = db.Товар
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
                     .Join(db.ПоставленныеТовары,
                      joinResult => joinResult.Tovar.ID_Товара,
                      post => post.Товар,
                      (joinResult, post) => new { Tovar = joinResult.Tovar, Param = joinResult.Param, Post = post, PT = joinResult.PT })
                  .Select(x => new ProductUpd
                  {
                      Название = x.Tovar.Название,
                      НазваниеКатегории = x.Param.Название,
                      Размеры = x.Tovar.РазмерыТовара.Размер,
                      ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                      Post = x.Post.Поставка,
                      Count = x.Post.Количество
                  }).Where(x => x.Post == _sell.ID).ToList();
            }
        }

        public void LoadData()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var namesProducts = new List<string>();
                var categoriesNames = new List<string>();
                var unitOfWorkNames = new List<string>();
                var sizesNames = new List<string>();

                var tovars = db.Товар.ToList();


                foreach (var i in tovars)
                    namesProducts.Add(i.Название);

                ProductComboBox.ItemsSource = namesProducts.Distinct();


            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
         /*   try
            {
                using (ConstructionStoreEntities db = new ConstructionStoreEntities())
                {
                    if (paramsComboBox.SelectedItem != null)
                    {
                        var needParam = db.Параметры.Where(x => x.Название == paramsComboBox.SelectedItem.ToString()).FirstOrDefault();
                        var needTovar = db.Товары.Where(x => x.Название == namesComboBox.Text).FirstOrDefault().ID;
                        var needProductWithParam = db.ПараметрыТоваров.Where(x => x.Параметр == needParam.ID && x.Товар == needTovar).FirstOrDefault().ID;

                        int count = int.Parse(CountTextBox.Text);
                        var warehouse = db.Склад.Where(x => x.Товар == needProductWithParam).FirstOrDefault();

                        if (count > warehouse.Количество)
                            MessageBox.Show("Недостаточно на складе");
                        else
                        {
                            ЗарезервированныеТовары product = new ЗарезервированныеТовары();
                            product.Количество = count;
                            product.Товар = needProductWithParam;
                            product.Резервация = _sell.ID;


                            var ixistItem = db.ЗарезервированныеТовары.Where(x => x.Товар == needProductWithParam && x.Резервация == _sell.ID).FirstOrDefault();
                            if (ixistItem == null)
                            {
                                db.ЗарезервированныеТовары.Add(product);
                                db.SaveChanges();

                            }
                            else
                            {
                                ixistItem.Количество += count;
                                db.SaveChanges();
                            }

                            namesComboBox.Text = string.Empty;
                            paramsComboBox.Text = string.Empty;
                            CountTextBox.Text = string.Empty;
                            UpdateView();
                        }
                    }
                    else
                    {
                        var needTovar = db.Товары.Where(x => x.Название == namesComboBox.Text).FirstOrDefault().ID;

                        var needProductWithParam = db.ПараметрыТоваров.Where(x => x.Параметр == null && x.Товар == needTovar).FirstOrDefault();
                        if (needProductWithParam != null)
                        {

                            int count = int.Parse(CountTextBox.Text);
                            var warehouse = db.Склад.Where(x => x.Товар == needProductWithParam.ID).FirstOrDefault();
                            if (count > warehouse.Количество)
                                MessageBox.Show("Недостаточно на складе");
                            else
                            {
                                ЗарезервированныеТовары product = new ЗарезервированныеТовары();
                                product.Количество = count;
                                product.Товар = needProductWithParam.ID;
                                product.Резервация = _sell.ID;

                                var ixistItem = db.ЗарезервированныеТовары.Where(x => x.Товар == needProductWithParam.ID && x.Резервация == _sell.ID).FirstOrDefault();
                                if (ixistItem == null)
                                {
                                    db.ЗарезервированныеТовары.Add(product);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    ixistItem.Количество += count;
                                    db.SaveChanges();
                                }
                                namesComboBox.Text = string.Empty;
                                paramsComboBox.Text = string.Empty;
                                CountTextBox.Text = string.Empty;
                                var list = GetJoinedDataWithPost().Where(x => x.Post == _sell.ID).ToList();
                                list.Add(UpdateWithNull());
                                tovarsGrid.ItemsSource = list;

                            }
                        }

                        else if (needProductWithParam == null)
                        {
                            ПараметрыТоваров prod = new ПараметрыТоваров();
                            prod.Параметр = null;
                            prod.Цена = 25;
                            prod.Товар = needTovar;


                            db.ПараметрыТоваров.Add(prod);
                            db.SaveChanges();

                            int count = int.Parse(CountTextBox.Text);
                            var warehouse = db.Склад.Where(x => x.Товар == needProductWithParam.ID).FirstOrDefault();
                            if (count > warehouse.Количество)
                                MessageBox.Show("Недостаточно на складе");
                            else
                            {
                                ЗарезервированныеТовары product = new ЗарезервированныеТовары();
                                product.Количество = count;
                                product.Товар = prod.ID;
                                product.Резервация = _sell.ID;

                                var ixistItem = db.ЗарезервированныеТовары.Where(x => x.Товар == needProductWithParam.ID && x.Резервация == _sell.ID).FirstOrDefault();
                                if (ixistItem == null)
                                {
                                    db.ЗарезервированныеТовары.Add(product);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    ixistItem.Количество += count;
                                    db.SaveChanges();
                                }


                                namesComboBox.Text = string.Empty;
                                paramsComboBox.Text = string.Empty;
                                CountTextBox.Text = string.Empty;

                                var list = GetJoinedDataWithPost().Where(x => x.Post == _sell.ID).ToList();
                                list.Add(UpdateWithNull());
                                tovarsGrid.ItemsSource = list;


                            }

                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var needItem = db.Резервация.Where(x => x.ID == _sell.ID).FirstOrDefault();
                db.Резервация.Remove(needItem);
                db.SaveChanges();

                new RezervListWindow().Show();
                Close();
            }
        }

        private void AddRezervButton_Click(object sender, RoutedEventArgs e)
        {
            new RezervListWindow().Show();
            Close();
        }
    }
}
