using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Edit;
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
    /// Логика взаимодействия для EmployeeListView.xaml
    /// </summary>
    public partial class EmployeeListView : Window
    {
        public EmployeeListView()
        {
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = GetEmployee();
            }
        }
        public List<EmployeeUpd> GetEmployee()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var result = db.Сотрудник.ToList().GroupJoin(
                    db.Должность.ToList(),
                    cl => cl.ID_Должности,
                    ci => ci.ID_Должности,
                    (cl,ci) => new {cl,ci})
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (emp, position) => new EmployeeUpd
                    {
                        ID_Сотрудника = emp.cl.ID_Сотрудника,
                        Фамилия = emp.cl.Фамилия,
                        Имя = emp.cl.Имя,
                        Отчество = emp.cl.Отчество,
                        НазваниеДолжности = position?.Название,
                        Стаж = emp.cl.Стаж,
                        Пол = emp.cl.Пол,
                        Телефон = emp.cl.Телефон,
                    }).ToList();
                return result;
            }
        }
        private void AddEmpButton_Click(object sender, RoutedEventArgs e)
        {
            new AddEmployeeView().Show();
            Close();
        }

        private void EditEmpButton_Click(object sender, RoutedEventArgs e)
        {
            var selectItem = grid.SelectedItem as EmployeeUpd;
            new EditEmployeeView(selectItem).Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ManagerMenuView().Show();
            Close();
        }

        private void SortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var list = GetEmployee();
            grid.ItemsSource = list.Where(x => x.Фамилия.ToLower().Contains(SortTextBox.Text)).ToList();
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
