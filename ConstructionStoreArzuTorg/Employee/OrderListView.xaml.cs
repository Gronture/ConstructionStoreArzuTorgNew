using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Other;
using Syncfusion.Linq;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
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

namespace ConstructionStoreArzuTorg.Employee
{
    /// <summary>
    /// Логика взаимодействия для OrderListView.xaml
    /// </summary>
    public partial class OrderListView : Window
    {
        public OrderListView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateView();
        }
        public OrderListView(List<ProductUpd> list)
        {
            InitializeComponent();
            tovarsGrid.ItemsSource = list;
        }

        public void UpdateView()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var first = db.Заказ.ToList()
                  .GroupJoin(
                    db.Сотрудник.ToList(),
                    cl => cl.ID_Сотрудника,
                    ci => ci.ID_Сотрудника,
                    (cl, ci) => new { cl, ci }).SelectMany(
                    x => x.ci.DefaultIfEmpty(),
                    (one, two) => new OrderUpd()
                    {
                        Дата_заказа = one.cl.Дата_заказа,
                        ФамилияСотрудника = two.Имя + " " + two.Фамилия,
                        ID_Заказа = one.cl.ID_Заказа,
                        Сумма = Math.Round(one.cl.Сумма, 2)

                    }).ToList();

                var second = db.Заказ.ToList()
                   .GroupJoin(
                   db.Клиент.ToList(),
                   cl => cl.ID_Клиента,
                   ci => ci.ID_Клиента,
                   (cl, ci) => new { cl, ci }).SelectMany(
                   x => x.ci.DefaultIfEmpty(),
                   (one, two) => new OrderUpd()
                   {
                       ФамилияКлиента = two.Имя + " " + two.Фамилия
                   }).ToList();

                for (int i = 0; i < first.Count; i++) { first[i].ФамилияКлиента = second[i].ФамилияКлиента; }
                orderGrid.ItemsSource = first;

            }
        }


        public List<OrderUpd> GetOrders()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var first = db.Заказ.ToList()
                  .GroupJoin(
                    db.Сотрудник.ToList(),
                    cl => cl.ID_Сотрудника,
                    ci => ci.ID_Сотрудника,
                    (cl, ci) => new { cl, ci }).SelectMany(
                    x => x.ci.DefaultIfEmpty(),
                    (one, two) => new OrderUpd()
                    {
                        Дата_заказа = one.cl.Дата_заказа,
                        ФамилияСотрудника = two.Имя + " " + two.Фамилия,
                        ID_Заказа = one.cl.ID_Заказа,
                        Сумма = Math.Round(one.cl.Сумма, 2)

                    }).ToList();

                var second = db.Заказ.ToList()
                   .GroupJoin(
                   db.Клиент.ToList(),
                   cl => cl.ID_Клиента,
                   ci => ci.ID_Клиента,
                   (cl, ci) => new { cl, ci }).SelectMany(
                   x => x.ci.DefaultIfEmpty(),
                   (one, two) => new OrderUpd()
                   {
                       ФамилияКлиента = two.Имя + " " + two.Фамилия
                   }).ToList();

                for (int i = 0; i < first.Count; i++) { first[i].ФамилияКлиента = second[i].ФамилияКлиента; }
                return first;

            }
        }

        public List<ProductUpd> GetProduct()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var firstJoin = db.Товар.ToList().GroupJoin(
                    db.Категория.ToList(),
                    cl => cl.ID_Категории,
                    ci => ci.ID_Категории,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (product, category) => new ProductUpd
                    {
                        ID_Товара = product.cl.ID_Товара,
                        Название = product.cl.Название,
                        НазваниеКатегории = category.Название,
                        Производитель = product.cl.Производитель,
                        Страна_происхождения = product.cl.Страна_происхождения
                    }).ToList();


                var secondJoin = db.Товар.ToList().GroupJoin(
                    db.РазмерыТовара.ToList(),
                    cl => cl.ID_Размеров,
                    ci => ci.ID_Размеров,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (product, dimensions) => new ProductUpd
                    {
                        Размеры = db.РазмерыТовара.Where(x => x.ID_Размеров == dimensions.ID_Размеров).FirstOrDefault().Размер,
                    }).ToList();

                var thirdJoin = db.Товар.ToList().GroupJoin(
                     db.Единицы_измерения.ToList(),
                     cl => cl.ID_Единицы_измерения,
                     ci => ci.ID_Измерений,
                     (cl, ci) => new { cl, ci })
                     .SelectMany(x => x.ci.DefaultIfEmpty(),
                     (product, units) => new ProductUpd
                     {
                         ЕдиницаИзмерения = db.Единицы_измерения.Where(x => x.ID_Измерений == units.ID_Измерений).FirstOrDefault().Название,
                     }).ToList();

                var lastJoin = db.Товар.ToList().GroupJoin(
                    db.Сезонность.ToList(),
                    cl => cl.Сезонность,
                    ci => ci.ID,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (product, units) => new ProductUpd
                    {
                        Стоимость = product.cl.Стоимость,
                        Сезонность = db.Сезонность.Where(x => x.ID == units.ID).FirstOrDefault().Название_сезона,
                        СерийныйНомер = product.cl.Серийный_номер,
                        Гарантия = product.cl.Гарантия,
                        Стоимость_со_скидкой = product.cl.Стоимость_со_скидкой,
                    }).ToList();



                for (int i = 0; i < firstJoin.Count; i++)
                {
                    firstJoin[i].Размеры = secondJoin[i].Размеры;
                    firstJoin[i].ЕдиницаИзмерения = thirdJoin[i].ЕдиницаИзмерения;
                    firstJoin[i].Стоимость = lastJoin[i].Стоимость;
                    firstJoin[i].Сезонность = lastJoin[i].Сезонность;
                    firstJoin[i].СерийныйНомер = lastJoin[i].СерийныйНомер;
                    firstJoin[i].Гарантия = lastJoin[i].Гарантия;
                    firstJoin[i].Стоимость_со_скидкой = lastJoin[i].Стоимость_со_скидкой;
                }


                return firstJoin;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddOrder().Show();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = orderGrid.SelectedItem as OrderUpd;

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var item = db.Заказ.Where(x => x.ID_Заказа == selectedItem.ID_Заказа).FirstOrDefault();
                db.Заказ.Remove(item);
                db.SaveChanges();

                UpdateView();

            }
        }
        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {

            var selectedItem = orderGrid.SelectedItem as OrderUpd;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {


                var joinedDataProduct = GetProductUpds().Where(x => x.Ord == selectedItem.ID_Заказа).ToList();
                var tovars = db.Товар.ToList();
                var result = joinedDataProduct.GroupBy(t => t).GroupBy(t => t.Count()).ToArray();
                var uniqueElements = result[0].Count();


                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDocument = null;
                wordApplication.Visible = true;

                var templatePathObj = @"D:\Проекты\ConstructionStoreArzuTorg-master\ConstructionStoreArzuTorg\OrderRerort.docx";

                try
                {
                    wordDocument = wordApplication.Documents.Add(templatePathObj);
                }
                catch (Exception exception)
                {
                    if (wordDocument != null)
                    {
                        wordDocument.Close(false);
                        wordDocument = null;
                    }
                    wordApplication.Quit();
                    wordApplication = null;
                    throw;
                }



                //var secondObject = db.ЗаказанныеТовары.Where(x => x.ID == )
                var needObject = db.Заказ.Where(x => x.ID_Заказа == selectedItem.ID_Заказа).FirstOrDefault();
                var client = db.Клиент.Where(x => x.ID_Клиента == needObject.ID_Клиента).FirstOrDefault();
                var worker = db.Сотрудник.Where(x => x.ID_Сотрудника == needObject.ID_Сотрудника).FirstOrDefault();
               // var tovar = db.Товар.Where(x => x.ID_Товара == needObject.) 


                var needCount = uniqueElements + 1;

                wordApplication.Selection.Find.Execute("{Table}");
                Microsoft.Office.Interop.Word.Range wordRange = wordApplication.Selection.Range;



                var wordTable = wordDocument.Tables.Add(wordRange,
                    needCount, 3);


                wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;
                wordTable.Range.Font.Name = "Times New Roman";
                wordTable.Range.Font.Size = 12;


                wordTable.Cell(1, 1).Range.Text = "Наименование товара";
                wordTable.Cell(1, 2).Range.Text = "Категория";
                wordTable.Cell(1, 3).Range.Text = "Количество";
                //wordTable.Cell(1, 4).Range.Text = "Стоимость";
                //wordTable.Cell(1, 5).Range.Text = "Стоимость без НДС";
                //wordTable.Cell(1, 6).Range.Text = "НДС";
                //wordTable.Cell(1, 7).Range.Text = "Стоимость с НДС";




                decimal nds = needObject.Сумма * 20 / 120;
                decimal sumNoNDS = needObject.Сумма - nds;
                //decimal cena = sumNoNDS / joinedDataProduct.Count;


                for (int i = 0; i < joinedDataProduct.Count; i++)
                {
                    wordTable.Cell(i + 2, 1).Range.Text = joinedDataProduct[i].Название;
                    wordTable.Cell(i + 2, 2).Range.Text = joinedDataProduct[i].НазваниеКатегории;
                    wordTable.Cell(i + 2, 3).Range.Text = joinedDataProduct[i].Count.ToString();
                    //wordTable.Cell(i + 2, 4).Range.Text = joinedDataProduct[i].Стоимость.ToString();
                    //wordTable.Cell(i + 2, 5).Range.Text = (joinedDataProduct[i].Стоимость * needObject.Сумма).ToString();
                    //wordTable.Cell(i + 2, 6).Range.Text = joinedDataProduct[i].Стоимость.ToString();
                    //wordTable.Cell(i + 2, 7).Range.Text = joinedDataProduct[i].Стоимость.ToString();
                }

                Random random = new Random();

               


                var items = new Dictionary<string, string>
                {
                    { "{Date}", needObject.Дата_заказа.ToString("dd.MM.yyyy")  },
                    { "{Client}", client.Фамилия + " " + client.Имя },
                    { "{Worker}", worker.Фамилия + " " + worker.Имя },
                    { "{Addres}", client.Адрес },
                    { "{Sum}", Math.Round(needObject.Сумма,2).ToString() },
                    { "{SumNDS}", Math.Round(nds,2).ToString() },
                    { "{SumNoNDS}", Math.Round(sumNoNDS, 2).ToString() },

                    { "{ID}",  random.Next(1000000, 9999999) + needObject.ID_Заказа.ToString() }
                };


                foreach (var item in items)
                {
                    Microsoft.Office.Interop.Word.Find find = wordApplication.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    object wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                    object replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

                    find.Execute(
                        FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: Type.Missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: Type.Missing, Replace: replace);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeMenu().Show();
            Close();
        }

        

        private void orderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = orderGrid.SelectedItem as OrderUpd;
            if (selectedItem != null)
            {
                var needList = GetProductUpds().Where(x => x.Ord == selectedItem.ID_Заказа).ToList();
                tovarsGrid.ItemsSource = needList;
            }
            
        }
        public List<ProductUpd> GetProductUpds()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                return db.Товар
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
                      Count = x.Ord.Количество,
                      ID = x.Ord.ID     
                  }).ToList();

            }
        }

        private void tovarsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ProductUpd)tovarsGrid.SelectedItem;
            

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var needProduct = db.Товар.FirstOrDefault(x => x.ID_Товара == selectedItem.ID_Товара);
               

                var needItem = db.ЗаказанныеТовары.FirstOrDefault(x => x.ID == selectedItem.ID);
               
                new ReturnView(needItem).Show();
                Close();
            }
        }
    }
}
