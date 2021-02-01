using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Logique d'interaction pour AdminPopup.xaml
    /// </summary>
    public partial class AdminPopup : Window
    {
        public AdminPopup()
        {
            InitializeComponent();
        }
        Models.Admin admin = new Models.Admin();

        private void AdminPopup_OnLoaded(object sender, RoutedEventArgs e)
        {


            FullName.Focus();
            GetClasses();
            Helper.GetRoles(UserRole);

        }








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


        void UploadPic(string pathFile, string ftp, string username, string password)
        {

        }


        #region Add User

        private void BtnAddSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {

                if (admin.AddUser(FullName.Text, UserMail.Text, UserPassword.Password, UserPassword2.Password, UserRole.SelectedIndex
                    , ClassesBox.SelectedIndex))
                {
                    Message.Text = admin.error;
                    FullName.Text = "";
                    UserMail.Text = "";
                    UserRole.SelectedIndex = -1;
                    ClassesBox.SelectedIndex = -1;
                    UserPassword.Password = "";
                    UserPassword2.Password = "";
                    admin.changed = true;
                    this.Close();

                }
                else
                {
                    Message.Text = admin.error;
                }
                Thread.Sleep(1000);


            });
        }


        #endregion


        #region Role Selection Changed

        private void UserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClassCombo.Visibility = ((ComboBox)sender).SelectedIndex > 1 ? Visibility.Visible : Visibility.Hidden;
            //if (((ComboBox)sender).SelectedIndex > 1)
            //{
            //    ClassCombo.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    ClassCombo.Visibility = Visibility.Hidden;
            //}
        }

        #endregion


        #region BtnExit

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

            this.Close();


        }

        #endregion


        #region EventMouse Down to Drag Window ==>

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch
            {
            }
            //
        }

        #endregion

    }
}
