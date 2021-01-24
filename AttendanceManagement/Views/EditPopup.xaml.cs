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
using AttendanceManagement.Models;

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
            Helper.GetRoles(UserRole);
            Helper.GetClasses(ClassesBox);
        }





        private void UserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 1)
            {
                ClassCombo.Visibility = Visibility.Visible;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var item = Admin.items;
            if (item != null)
            {

                //int Roleid = int.Parse(item.Row["Role Id"].ToString());
                int id = int.Parse(item.Row["User Id"].ToString());
                FullName.Text = item.Row["Full Name"].ToString();
                UserMail.Text = item.Row["Email"].ToString();
                UserPassword.Password = item.Row["Password"].ToString();
                UserPassword2.Password = item.Row["Password"].ToString();
                var roleId = int.Parse(item.Row["Role Id"].ToString());
                UserRole.SelectedValue = roleId;

                if (roleId > 2)
                {
                    ClassesBox.SelectedIndex = int.Parse(item.Row["Class Id"].ToString());

                }

                //admin.getusersedit(id, FullName, UserMail, UserPassword, UserPassword2);
            }

        }


        private void BtnEditSubmit_Click(object sender, RoutedEventArgs e)
        {
            var item = Admin.items;
            int id = int.Parse(item.Row["User Id"].ToString());


            if (admin.EditUsers(id, FullName.Text, UserMail.Text, UserPassword.Password, UserPassword2.Password, ClassesBox.SelectedIndex, UserRole.SelectedIndex))
            {

                Message.Text = admin.error;
                FullName.Text = "";
                UserMail.Text = "";
                UserRole.SelectedIndex = -1;
                ClassesBox.SelectedIndex = -1;
                UserPassword.Password = "";
                UserPassword2.Password = "";
            }
            else
            {
                Message.Text = admin.error;
                hello.Text = UserRole.SelectedIndex.ToString();

            }




        }
    }
}
