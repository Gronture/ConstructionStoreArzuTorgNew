using ConstructionStoreArzuTorg.Employee;
using ConstructionStoreArzuTorg.Manager;
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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }
        private void Clear()
        {
            LoginTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;    
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var needUser = db.Пользователь
                     .FirstOrDefault(u => u.Логин == LoginTextBox.Text && u.Пароль == PasswordBox.Password);
                if (needUser is null)
                {
                    Clear();
                    return;
                }

                var emp = db.СотрудникПользователь
                                .Where(x => x.Пользователь == needUser.ID)
                                .Join(db.Сотрудник, x => x.Сотрудник, y => y.ID_Сотрудника, (x, y) => y)
                                .Join(db.Должность, x => x.ID_Должности, y => y.ID_Должности, (x, y) => new { Employee = x, Role = y.Название })
                                .FirstOrDefault();


                if (emp != null)
                {

                    string role = emp.Role;


                    switch (role)
                    {
                        case "Менеджер":
                            {
                                new ManagerMenuView().Show();
                                Close();
                                break;
                            }
                        case "Сотрудник":
                            {
                                new EmployeeMenu().Show();
                                Close();
                                break;
                            }
                        default:
                            {
                                Clear();
                                break;
                            }
                    }
                }
                else
                {
                    Clear();
                }

            }
        }
    }
}
