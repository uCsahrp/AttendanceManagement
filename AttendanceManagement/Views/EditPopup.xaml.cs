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

        private Models.Admin admin = new Models.Admin();


        public EditPopup()
        {
            InitializeComponent();
            Helper.GetRoles(UserRole);
            //Helper.GetClasses(ClassesBox);
            GetClasses();
        }


        #region On Load

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var item = Admin.items;
            if (item != null)
            {

                //int Roleid = int.Parse(item.Row["Role Id"].ToString());
                try
                {
                    int id = int.Parse(item["User Id"].ToString());
                    FullName.Text = item["Full Name"].ToString();
                    UserMail.Text = item["Email"].ToString();
                    UserPassword.Password = item["Password"].ToString();
                    UserPassword2.Password = item["Password"].ToString();
                    var roleId = int.Parse(item["Role Id"].ToString());
                    UserRole.SelectedValue = roleId;

                    if (roleId > 2)
                    {
                        ClassesBox.SelectedIndex = int.Parse(item["Class Id"].ToString());

                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                //admin.getusersedit(id, FullName, UserMail, UserPassword, UserPassword2);
            }

        }

        #endregion



        #region Role Changes Event

        private void UserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 1)
            {
                ClassCombo.Visibility = Visibility.Visible;
            }
        }


        #endregion



        #region GetClasses

        void GetClasses()
        {
            Ado adonet = new Ado();

            var query = $"Select * From Classes";
            adonet.Adapter = new SqlDataAdapter(query, adonet.Cnx);
            adonet.Adapter.Fill(adonet.DataSet);
            ClassesBox.SelectedValuePath = "Class Id";
            ClassesBox.DisplayMemberPath = "Class Name";
            ClassesBox.ItemsSource = adonet.DataSet.Tables[0].DefaultView;

        }

        #endregion



        #region Btn Edit Event

        private void BtnEditSubmit_Click(object sender, RoutedEventArgs e)
        {
            var item = Admin.items;
            int id = int.Parse(item["User Id"].ToString());


            if (admin.EditUsers(id, FullName.Text, UserMail.Text, UserPassword.Password, UserPassword2.Password, ClassesBox.SelectedIndex, UserRole.SelectedIndex))
            {

                Message.Text = admin.error;
                FullName.Text = "";
                UserMail.Text = "";
                UserRole.SelectedIndex = -1;
                ClassesBox.SelectedIndex = -1;
                UserPassword.Password = "";
                UserPassword2.Password = "";

                this.Close();
            }
            else
            {
                Message.Text = admin.error;
                hello.Text = UserRole.SelectedIndex.ToString();

            }
        }


        #endregion



        #region EventMouse Down to Drag Window ==>

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {
            }
            //
        }

        #endregion



        #region Button Exit Event ==>

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

            Close();
            admin.changed = true;
        }

        #endregion

    }
}
