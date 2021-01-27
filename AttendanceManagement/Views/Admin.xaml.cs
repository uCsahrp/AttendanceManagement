using AttendanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {

        Models.Admin admin = new Models.Admin();
        public static DataRowView items;
        int IdSelectedUser = 0;


        public Admin()
        {
            InitializeComponent();

        }


        #region On Window Loads
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //admin.usertable = userstable;
            GetUsers();
            userName.Text = admin.UserName;
            Helper.GetClasses(ClassFilter);
        }

        #endregion


        #region Add Users To and Refesh the DataGrid

        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            AdminPopup popup = new AdminPopup();

            popup.Show();
            popup.Closed += (s, e) =>
            {

                if (admin.changed)
                {
                    Helper.GetUsers(userstable);
                }
            };
            //this.Close();

        }

        #endregion



        #region Grid Double Click

        private void userstable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            items = userstable.SelectedItem as DataRowView;
            EditPopup editPop = new EditPopup();
            editPop.Show();
            editPop.Closed += (s, e) =>
            {

                if (admin.changed)
                {
                    GetUsers();
                }
            };

        }

        #endregion

        #region Selection Changed

        private void userstable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //TODO Fix This

            DataRow row = userstable.SelectedItems as DataRow;


            IdSelectedUser = int.Parse(row..ToString());

            MessageBox.Show(IdSelectedUser + String.Empty);

        }

        #endregion






        #region Delete User

        private void DelUser_Click(object sender, RoutedEventArgs e)
        {

            admin.DeleteUser(IdSelectedUser);


            //Helper.GetUsers(userstable);
            GetUsers();

            Message.Text = admin.error;

        }

        #endregion





        #region On Text Change

        private void SearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {

            //var fullname = SearchInput.Text.ToString();
            //UserModel.Search(fullname, userstable);
            //items = userstable.SelectedItem as DataRowView;
            //EditPopup editPop = new EditPopup();
            //editPop.Show();
        }

        #endregion





        #region Edit Button 

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            var fullname = SearchInput.Text.ToString();
            UserModel.Search(fullname, userstable);
            items = userstable.SelectedItem as DataRowView;
            EditPopup editPop = new EditPopup();
            editPop.Show();


            editPop.Closed += (s, ev) =>
            {
                //Helper.GetUsers(userstable);
                GetUsers();

            };
        }

        #endregion


        #region GetUsers

        void GetUsers()
        {
            Ado adonet = new Ado();
            adonet.Cmd.CommandText = "Select u.[User Id], u.[Full Name], u.Email, r.[Role Name],c.[Class Name] From Users u INNER JOIN Roles r ON u.[Role Id]= r.[Role Id] Left JOIN Classes c On u.[Class Id] = c.[Id Class]; ";
            adonet.Cmd.Connection = adonet.Cnx;
            adonet.Connect();
            adonet.DataReader = adonet.Cmd.ExecuteReader();
            adonet.Datatable.Load(adonet.DataReader);
            adonet.Disconnect();
            userstable.ItemsSource = adonet.Datatable.DefaultView;

        }

        #endregion


        #region Button Exit Event ==>

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetUsers();
        }
    }
}
