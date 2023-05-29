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

namespace ConstructionStoreArzuTorg.Add
{
    /// <summary>
    /// Логика взаимодействия для AddProductToOrder.xaml
    /// </summary>
    public partial class AddProductToOrder : Window
    {
        Заказ _заказ;
        public AddProductToOrder(Заказ заказ)
        {
            InitializeComponent();
            _заказ = заказ;
            LoadData();
        }

        public void ClearComboBox()
        {
            ProductComboBox.Text = string.Empty;
            RazmerComboBox.Text = string.Empty;
            CategorComboBox.Text = string.Empty;
            EdIzmComboBox.Text = string.Empty;
            ColvoTextBox.Text = string.Empty;
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
                      НазваниеКатегории = x.Tovar.Категория.Название,
                      Размеры = x.Tovar.РазмерыТовара.Размер,
                      ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                      Сезонность = x.Tovar.Сезонность1.Название_сезона,
                      Ord = x.Ord.Заказ,
                      Count = x.Ord.Количество
                  }).Where(x => x.Ord == _заказ.ID_Заказа).ToList();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var tovars = db.Товар.ToList();
                var names = new List<string>();

                foreach (var item in tovars) { names.Add(item.Название); }
                ProductComboBox.ItemsSource = names.Distinct();
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
                var products = db.Товар.ToList();
                var nameProd = ProductComboBox.SelectedItem.ToString();

                var categoria = db.Категория.Where(x => x.Название == CategorComboBox.SelectedItem.ToString()).FirstOrDefault();
                var size = db.РазмерыТовара.Where(x => x.Размер == RazmerComboBox.SelectedItem.ToString()).FirstOrDefault();
                var unitOfWork = db.Единицы_измерения.Where(x => x.Название == EdIzmComboBox.SelectedItem.ToString()).FirstOrDefault();



                var needitems = db.Товар.Where(x => x.Название == nameProd).ToList();

                var needProduct = needitems.Where(x => x.ID_Категории == categoria.ID_Категории &&
                x.ID_Размеров == size.ID_Размеров &&
                x.ID_Единицы_измерения == unitOfWork.ID_Измерений).FirstOrDefault();

                if (needProduct != null)
                {
                    ЗаказанныеТовары zak = new ЗаказанныеТовары();
                    zak.Заказ = _заказ.ID_Заказа;
                    zak.Количество = int.Parse(ColvoTextBox.Text);
                    zak.Товар = needProduct.ID_Товара;

                    db.ЗаказанныеТовары.Add(zak);
                    db.SaveChanges();

                    UpdateView();

                    ClearComboBox();
                }
                else
                {
                    MessageBox.Show("Ошибка");
                }
            }
            
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var selectedItem = tovarsGrid.SelectedItem as ProductUpd;
                var productName = selectedItem.Название;

                var categoria = db.Категория.Where(x => x.Название == selectedItem.НазваниеКатегории).FirstOrDefault();
                var size = db.РазмерыТовара.Where(x => x.Размер == selectedItem.Размеры).FirstOrDefault();
                var unitOfWork = db.Единицы_измерения.Where(x => x.Название == selectedItem.ЕдиницаИзмерения).FirstOrDefault();

                var needProduct = db.Товар.Where(x =>
                x.ID_Категории == categoria.ID_Категории &&
                x.ID_Размеров == size.ID_Размеров &&
                x.ID_Единицы_измерения == unitOfWork.ID_Измерений &&
                x.Название == productName).FirstOrDefault();

                var itemToRemove = db.ЗаказанныеТовары.Where(x =>
                x.Заказ == _заказ.ID_Заказа &&
                x.Количество == selectedItem.Count &&
                x.Товар == needProduct.ID_Товара).FirstOrDefault();

                db.ЗаказанныеТовары.Remove(itemToRemove);
                db.SaveChanges();

                UpdateView();
            }
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            new OrderListView().Show();
            Close();
            /*
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



                var needObject = db.Заказ.Where(x => x.ID_Заказа == selectedItem.ID_Заказа).FirstOrDefault();
                var client = db.Клиент.Where(x => x.ID_Клиента == needObject.ID_Клиента).FirstOrDefault();
                var worker = db.Сотрудник.Where(x => x.ID_Сотрудника == needObject.ID_Сотрудника).FirstOrDefault();


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
                




                decimal nds = needObject.Сумма * 20 / 120;
                decimal sumNoNDS = needObject.Сумма - nds;


                for (int i = 0; i < joinedDataProduct.Count; i++)
                {
                    wordTable.Cell(i + 2, 1).Range.Text = joinedDataProduct[i].Название;
                    wordTable.Cell(i + 2, 2).Range.Text = joinedDataProduct[i].НазваниеКатегории;
                    wordTable.Cell(i + 2, 3).Range.Text = joinedDataProduct[i].Count.ToString();
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


             
             */
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var needItem = db.Заказ.Where(x => x.ID_Заказа == _заказ.ID_Заказа).FirstOrDefault();
                db.Заказ.Remove(needItem);
                db.SaveChanges();

                new OrderListView().Show();
                Close();
            }
        }

        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductComboBox.SelectedItem == null)
                return;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var selectedItem = ProductComboBox.SelectedItem.ToString();

                var needCategories = db.Категория.ToList();
                var needUnitOfWork = db.Единицы_измерения.ToList();
                var needSizes = db.РазмерыТовара.ToList();


                var categories = new List<string>();
                var unitOfWork = new List<string>();
                var size = new List<string>();

                var needProducts = db.Товар.Where(x => x.Название == selectedItem).ToList().Distinct().ToList();

                foreach (var i in needProducts)
                {
                    var needCategor = needCategories.Where(x => x.ID_Категории == i.ID_Категории).FirstOrDefault();
                    categories.Add(needCategor.Название);

                    var needUnit = needUnitOfWork.Where(x => x.ID_Измерений == i.ID_Единицы_измерения).FirstOrDefault();
                    unitOfWork.Add(needUnit.Название);

                    var needSize = needSizes.Where(x => x.ID_Размеров == i.ID_Размеров).FirstOrDefault();
                    size.Add(needSize.Размер);
                }


                CategorComboBox.ItemsSource = categories.Distinct();
                EdIzmComboBox.ItemsSource = unitOfWork.Distinct();
                RazmerComboBox.ItemsSource = size.Distinct();


            }
        }

        private void CategorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorComboBox.SelectedItem == null)
                return;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var selectedItem = CategorComboBox.SelectedItem.ToString();
                var categor = db.Категория.Where(x => x.Название == selectedItem).FirstOrDefault();

                var needProducts = db.Товар.Where(x => x.ID_Категории == categor.ID_Категории).ToList();

                var sizes = db.РазмерыТовара.ToList();

                var newSizes = new List<string>();

                for (int i = 0; i < needProducts.Count; i++)
                {
                    // var unitOfWork = db.Единицы_измерения.Where(x => x.ID_Измерений == needProducts[i].ID_Категории).FirstOrDefault();
                    var size = sizes.Where(x => x.ID_Размеров == needProducts[i].ID_Размеров).FirstOrDefault();
                    newSizes.Add(size.Размер);

                }

                RazmerComboBox.ItemsSource = newSizes.Distinct();

            }
        }

        private void RazmerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RazmerComboBox.SelectedItem == null)
                return;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var selectedSize = RazmerComboBox.SelectedItem.ToString();
                var selectedCategor = CategorComboBox.SelectedItem.ToString();

                var categor = db.Категория.Where(x => x.Название == selectedCategor).FirstOrDefault();
                var size = db.РазмерыТовара.Where(x => x.Размер == selectedSize).FirstOrDefault();

                var needProducts = db.Товар.Where(x => x.ID_Размеров == size.ID_Размеров && x.ID_Категории == categor.ID_Категории).ToList();

                var unitofworks = db.Единицы_измерения.ToList();

                var newUnitOfWorks = new List<string>();
                for (int i = 0; i < needProducts.Count; i++)
                {
                    var unitOfWork = unitofworks.Where(x => x.ID_Измерений == needProducts[i].ID_Единицы_измерения).FirstOrDefault();
                    newUnitOfWorks.Add(unitOfWork.Название);


                }
                EdIzmComboBox.ItemsSource = newUnitOfWorks.Distinct();


            }
        }

        private void EdIzmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
