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
        private Models.Admin admin = new Models.Admin();
        public static DataRowView items;
        int IdSelectedUser = 0;


        public Admin()
        {
            InitializeComponent();

        }



        #region On Admin Window Loads
        private void Admin_Loaded(object sender, RoutedEventArgs e)
        {
            //admin.usertable = userstable;
            GetUsers();
            userName.Text = admin.UserName;

            Dispatcher.Invoke(() =>
            {
                GetClasses();
            });
        }

        #endregion



        #region Add Users and Refesh the DataGrid

        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            AdminPopup popup = new AdminPopup();
            popup.Show();
            popup.Closed += (s, e) =>
            {

                GetUsers();

            };
        }

        #endregion



        #region DataGrid Double Click Event

        private void userstable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (userstable.SelectedItem is DataRowView)
            {
                items = userstable.SelectedItem as DataRowView;
            }
            else
            {
                return;
            }

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



        #region DataGrid Selection Changed Event

        private void userstable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //TODO Fix This
            try
            {
                int i = userstable.SelectedIndex;
                if (userstable.Items[i] is DataRowView)
                {
                    DataRowView rowView = (DataRowView)userstable.Items[i];
                    string Id_user = rowView[0].ToString().Trim();

                    //Save The Id of the Selected User
                    IdSelectedUser = int.Parse(Id_user);
                    //Save the Selected Row(User Infos)
                    items = rowView;
                }
                else
                {
                    return;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }

        #endregion



        #region Delete User

        private void DelUser_Click(object sender, RoutedEventArgs e)
        {
            //Call Delere Methode of Admin
            admin.DeleteUser(IdSelectedUser);
            //Refresh
            GetUsers();
            //Return the Error Message from Method to UI
            Message.Text = admin.error;

        }

        #endregion



        #region Edit Button 

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            var fullname = SearchInput.Text.ToString();
            UserModel.Search(fullname, userstable);

            if (userstable.SelectedItem is DataRowView)
            {
                items = userstable.SelectedItem as DataRowView;
            }
            else
            {
                //TODO FIX IN CASE TYPE BUTTON OR COLUMN NAME !!!!
                return;
            }

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
        Ado adonet = new Ado();

        void GetUsers()
        {

            Task.Run(() =>
         {

             adonet.DataSet.Tables.Clear();
             userstable.Items.Clear();

         });

            adonet.Connect();

            var query = $"Select u.[User Id], u.[Full Name], u.Email, r.[Role Name],c.[Class Name],c.[Id Class] From Users u INNER JOIN Roles r ON u.[Role Id]= r.[Role Id] Left JOIN Classes c On u.[Class Id] = c.[Id Class]; ";
            adonet.Adapter = new SqlDataAdapter(query, adonet.Cnx);

            adonet.Adapter.Fill(adonet.DataSet);

            adonet.Disconnect();

            userstable.ItemsSource = adonet.DataSet.Tables[0].DefaultView;

        }

        #endregion



        #region GetClasses


        void GetClasses()
        {
            Ado ado = new Ado();

            ado.Connect();
            var query = $"Select * From Classes";
            ado.Adapter = new SqlDataAdapter(query, ado.Cnx);
            ado.Adapter.Fill(ado.DataSet);
            ado.Disconnect();
            ClassFilter.SelectedValuePath = "Class Id";
            ClassFilter.DisplayMemberPath = "Class Name";
            ClassFilter.ItemsSource = ado.DataSet.Tables[0].DefaultView;


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



        #region Btn Refresh Click Event


        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetUsers();
        }


        #endregion



        #region DropDown Filter By Classes Selection Change Event


        private void ClassFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            try
            {
                Task.Run(() =>
                   {
                       userstable.Items.Clear();

                   });
                var item = (DataRowView)ClassFilter.SelectedItem;
                // var view = userstable.ItemsSource as DataView;
                DataView view = adonet.DataSet.Tables[0].DefaultView;
                view.RowFilter = $"[Class Name] = '{item[1].ToString()}'";
                userstable.ItemsSource = view.ToTable().DefaultView;
            }
            catch (Exception)
            {

            }

        }


        #endregion



        #region On Text Change

        private void SearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                Task.Run(() =>
                {
                    userstable.Items.Clear();

                });
                var text = SearchInput.Text.Trim();
                DataView view = adonet.DataSet.Tables[0].DefaultView;
                var row = adonet.DataSet.Tables[0].AsEnumerable()
                     .Where(row =>

                     string.IsNullOrEmpty(text)
                     ? true
                     : row["Full Name"].ToString().Contains(text) ?
                     true
                     : row["Email"].ToString().Contains(text) ?
                     true
                     : row["Class Name"].ToString().Contains(text) ?
                     true
                     : row["Role Name"].ToString().Contains(text)
                     ).CopyToDataTable();

                userstable.ItemsSource = row.DefaultView;

            }
            catch (Exception)
            {

            }

        }

        #endregion



    }
}
