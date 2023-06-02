using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Employee;
using ConstructionStoreArzuTorg.Manager;
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

namespace ConstructionStoreArzuTorg.Add
{
    /// <summary>
    /// Логика взаимодействия для AddProvider.xaml
    /// </summary>
    public partial class AddProvider : Window
    {
        public AddProvider()
        {
            InitializeComponent();
        }
        
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
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
                Поставщик поставщик = new Поставщик();
                поставщик.Наименование = NameTextBox.Text;
                поставщик.Расчётный_счёт = RSTextBox.Text;
                поставщик.Учётный_номер_плательщика = int.Parse(NumPlatTextBox.Text);
                поставщик.Название_банка = NameBankTextBox.Text;
                поставщик.Код_банка = CodeBankTextBox.Text;
                поставщик.Адрес = AddressTextBox.Text;
                поставщик.ФИО = FIOTextBox.Text;
                поставщик.Должность = PosTextBox.Text;
                поставщик.Почта = EmailTextBox.Text;
                db.Поставщик.Add(поставщик);
                db.SaveChanges();



                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDocument = null;
                wordApplication.Visible = true;

                var templatePathObj = @"D:\Проекты\ConstructionStoreArzuTorg-master\ConstructionStoreArzuTorg\DogovorWithProvider.docx";

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





                Random random = new Random();
                var items = new Dictionary<string, string>
                {
                    { "{NameProvider}",  поставщик.Наименование },
                    { "{PositionProvider}",  поставщик.Должность },
                    { "{PositionProvider}",  поставщик.ID_Поставщика.ToString() },
                    { "{Address}",  поставщик.Адрес },
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







            new ProviderListView().Show();
            Close();

            
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ProviderListView().Show();
            Close();
        }
    }
}
