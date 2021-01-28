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


        #region On Window Loads
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




        #region Selection Changed

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
                    IdSelectedUser = int.Parse(Id_user);
                    // MessageBox.Show(Id_user);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetUsers();
        }

        private void Userstable_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //var grid = (DataGrid)sender;
            //var selected = grid.SelectedCells;
            //var id = selected[0].Item.ToString();
            //string value = ((DataGrid)sender).Rows[e.RowIndex].Cells[0].Value;
            //MessageBox.Show(id);
        }


        private void ClassFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataView view = adonet.DataSet.Tables[0].DefaultView;
            view.RowFilter = $"[Class Name] = '{ClassFilter.SelectedItem.ToString()}'";
            userstable.ItemsSource = view.ToTable().DefaultView;
            view.RowFilter = String.Empty;


        }
    }
}
