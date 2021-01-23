using AttendanceManagement.Views;
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

namespace AttendanceManagement
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }


        #region Button Login Event ==>

        private void SubmitLogin_OnClick(object sender, RoutedEventArgs e)
        {
            UserModel user = new UserModel();

            Dispatcher.InvokeAsync(() =>
            {
                if (user.Login(InputEmail.Text, InputPassword.Password))
                {
                    switch (user.RoleId)
                    {
                        case 1:
                            //Admin

                            MessageBox.Show(user.RoleId + "Welcome to Admin Section", "Success", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                            Admin admin = new Admin();

                            //Set Avatar To User
                            user.UserAvatar = "Admin Icon.png";

                            admin.Show();
                            //Close Login Window
                            Close();

                            break;
                        case 2:
                            //Secretary

                            MessageBox.Show(user.RoleId + "Welcome to Secretary Section", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            //Set Avatar To User
                            user.UserAvatar = "Secretary Icon.png";

                            //Close Login Window
                            Close();

                            break;
                        case 3:
                            //Staff
                            MessageBox.Show(user.RoleId + "Welcome to Staff Section", "Success", MessageBoxButton.OK,
                                MessageBoxImage.Information);

                            //Set Avatar To User
                            user.UserAvatar = "Staff Icon.png";

                            //Close Login Window
                            Close();

                            break;
                        case 4:
                            //Student

                            MessageBox.Show(user.RoleId + "Welcome to Student Section", "Success", MessageBoxButton.OK,
                                MessageBoxImage.Information);

                            //Close Login Window

                            Student userStudent = new Student();

                            //Set Avatar To User
                            user.UserAvatar = "Student Icon.png";

                            //Open Student Window
                            userStudent.Show();
                            Close();

                            break;
                    }
                }
                else
                {
                    ErrorMsg.Text = user.error;
                    InputPassword.Password = "";
                    InputEmail.Focus();
                }
            });
        }

        #endregion


        #region EventMouse Down to Drag Window ==>

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
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
            MainWindow main = new MainWindow();
            main.Show();
            Close();

        }

        #endregion


        #region Event Input mail TextChange To Delete Error Message ==>

        private void InputEmail_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorMsg.Text = "";
        }

        #endregion


        #region Event For Event KeyDown 'Enter' ==>

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitLogin_OnClick(sender, e);
            }
        }

        #endregion

    }
}
