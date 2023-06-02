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

namespace ConstructionStoreArzuTorg.Other
{
    /// <summary>
    /// Логика взаимодействия для ReturnView.xaml
    /// </summary>
    public partial class ReturnView : Window
    {
        ЗаказанныеТовары product;
        public ReturnView()
        {
            InitializeComponent();
        }

        public ReturnView(ЗаказанныеТовары заказанныеТовары)
        {
            product = заказанныеТовары;
            InitializeComponent();
            LoadData();
        }


        public void LoadData()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
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
                      ID_Товара = x.Tovar.ID_Товара,
                      Название = x.Tovar.Название,
                      НазваниеКатегории = x.Tovar.Категория.Название,
                      Размеры = x.Tovar.РазмерыТовара.Размер,
                      ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                      Сезонность = x.Param.Название_сезона,
                      Ord = x.Ord.Заказ,
                      Count = x.Ord.Количество
                  }).ToList();

             


                tovarTextBox.Text = list.FirstOrDefault(x => x.ID_Товара == product.Товар && x.Ord == product.Заказ).Название;
              

            }
        }

        private void BackBotton_Click(object sender, RoutedEventArgs e)
        {
            new OrderListView().Show();
            Close();
        }

        private void EditStatusButton_Click(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                if (countTextBox.Text != string.Empty)
                {
                    try
                    {
                        var inTextBoxCount = int.Parse(countTextBox.Text);
                        if (inTextBoxCount <= 0)
                        {
                            countTextBox.Clear();
                        }



                        var itemToEdit = db.ЗаказанныеТовары.FirstOrDefault(x => x.ID == product.ID);
                        var productPrice = db.Товар.FirstOrDefault(x => x.ID_Товара == itemToEdit.Товар).Стоимость;


                        var supply = db.Заказ.FirstOrDefault(x => x.ID_Заказа == itemToEdit.Заказ);

                        if (itemToEdit != null)
                        {
                            itemToEdit.Количество -= inTextBoxCount;
                            itemToEdit.Сумма = itemToEdit.Количество * productPrice;

                            var item = new ВозвратТовара();

                            item.Количество = inTextBoxCount;
                            item.Заказ = itemToEdit.Заказ;
                            item.Заказанные_товары = itemToEdit.ID;
                            db.ВозвратТовара.Add(item);

                            var itemOnWarehouse = db.Склад.FirstOrDefault(x => x.Товар == itemToEdit.Товар);
                            itemOnWarehouse.Количество += inTextBoxCount;


                            supply.Сумма = (decimal)db.ЗаказанныеТовары.Where(x => x.Заказ == supply.ID_Заказа).Sum(x => x.Сумма);

                            if (itemToEdit.Количество <= 0)
                            {
                                var sameProduct = db.ЗаказанныеТовары
                                    .FirstOrDefault(x => x.Заказ == itemToEdit.Заказ && x.Товар == itemToEdit.Товар && x.ID != itemToEdit.ID && x.Количество > 0);
                                if (sameProduct != null)
                                {
                                    sameProduct.Количество -= Math.Abs(itemToEdit.Количество);
                                    db.ЗаказанныеТовары.Remove(itemToEdit);

                                }
                                else
                                {
                                    db.ЗаказанныеТовары.Remove(itemToEdit);
                                }
                                db.SaveChanges();
                            }
                            else
                            {
                                db.SaveChanges();
                            }
                        }

                        //else
                        //    itemToEdit.Статус = db.Статус.FirstOrDefault(x => x.Название == "Возврат").ID;

                        db.SaveChanges();
                        new OrderListView().Show();
                        Close();
                    }
                    catch
                    {
                        MessageBox.Show("Возникла ошибка");
                    }
                }

            }
        }
    }
}
