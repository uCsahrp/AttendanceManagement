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

        public static int userid;
        private void SubmitLogin_OnClick(object sender, RoutedEventArgs e)
        {
            UserModel user = new UserModel();

            Dispatcher.Invoke(() =>
             {
                 try
                 {
                     if (user.Login(InputEmail.Text, InputPassword.Password))
                     {
                         switch (user.RoleId)
                         {
                             case 1:
                                 //Admin
                                 Admin admin = new Admin();

                                 admin.Show();
                                 //Close Login Window
                                 Close();

                                 break;
                             case 2:
                                 //Secretary
                                 Secretary secretary = new Secretary();

                                 secretary.Show();
                                 //Close Login Window
                                 Close();

                                 break;
                             case 3:
                                 //Staff
                                 Staff staff = new Staff();

                                 staff.Show();
                                 //Close Login Window
                                 Close();

                                 break;
                             case 4:
                                 //Student
                                 Student userStudent = new Student();

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
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex + String.Empty);

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
