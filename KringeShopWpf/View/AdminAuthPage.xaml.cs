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
using KringeShopWpf.ViewModel;

namespace KringeShopWpf.View
{
    /// <summary>
    /// Логика взаимодействия для AdminRegistrationPage.xaml
    /// </summary>
    public partial class AdminAuthPage : Window
    {
        public AdminAuthPage()
        {
            InitializeComponent();
            DataContext = new AdminAuthPageVM();
            pwdBox.PasswordChar= '*';
            ((AdminAuthPageVM)DataContext).SetPassBox(pwdBox);
        }
    }
}
