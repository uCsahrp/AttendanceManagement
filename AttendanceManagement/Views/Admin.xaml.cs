using System;
using System.Collections.Generic;
using System.Data;
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
            //admin.GetUsers(userstable);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //admin.usertable = userstable;
            admin.GetUsers(userstable);
            userName.Text = admin.UserName;
        }



        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            AdminPopup popup = new AdminPopup();

            popup.Show();

            if (admin.changed)
            {

            }
            //this.Close();

        }



        private void userstable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            items = userstable.SelectedItem as DataRowView;
            EditPopup editPop = new EditPopup();
            editPop.Show();


        }


        private void userstable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = userstable.SelectedItem as DataRowView;
            IdSelectedUser = int.Parse(row.Row.ItemArray[0].ToString());

            //MessageBox.Show(IdSelectedUser + String.Empty);


        }


        private void SearchInput_TextChanged(object sender, TextChangedEventArgs e)
        {

            var fullname = SearchInput.Text.ToString();
            UserModel.Search(fullname, userstable);
        }

        private void DelUser_Click_1(object sender, RoutedEventArgs e)
        {

            Dispatcher.Invoke(() =>
            {
                Task.Run(() =>
                    {
                        userstable.Items.Clear();
                        admin.GetUsers(userstable);
                        admin.DeleteUser(IdSelectedUser);
                    });

            Dispatcher.Invoke(() =>
            {
                Task.Run(() =>
            Task.Run(() =>
                {
                   
                
                    admin.GetUsers(userstable);
                    admin.DeleteUser(IdSelectedUser);
                });
            Message.Text = admin.error;

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            var fullname = SearchInput.Text.ToString();
            UserModel.Search(fullname, userstable);
            items = userstable.SelectedItem as DataRowView;
            EditPopup editPop = new EditPopup();
            editPop.Show();

            var fullname = SearchInput.Text.ToString();
            UserModel.Search(fullname, userstable);
        }
    }
}
