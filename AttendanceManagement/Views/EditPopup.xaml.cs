using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace AttendanceManagement.Views
{
    /// <summary>
    /// Logique d'interaction pour EditPopup.xaml
    /// </summary>
    public partial class EditPopup : Window
    {
        Models.Admin admin = new Models.Admin();
        public EditPopup()
        {
            InitializeComponent();
        }

        private void BtnAddSubmit_Loaded(object sender, RoutedEventArgs e)
        {
            var item = Admin.items;
            if (item != null)
            {
                int id = int.Parse(item.Row["User Id"].ToString());
                admin.getusersedit(id, FullName, UserMail, UserPassword, UserPassword2) ;
               
            }
        }
    }
}
