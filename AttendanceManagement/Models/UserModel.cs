using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using AttendanceManagement.Models;
namespace AttendanceManagement.Views
{
    class UserModel : User
    {

        //New Ado For Connection
        static Ado Adonet = new Ado();

        #region Methode Login

        public override bool Login(string email, string password)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);

                //Check if Email Entered
                if (email.Length == 0)
                {
                    error = "Please enter your email.";
                    return false;
                }
                //Check if it's a valid email

                else if (!match.Success)
                {
                    error = "Please Enter a valid email address.";
                    return false;
                }
                else
                {
                    //Open Connection
                    Adonet.Connect();

                    //Check for user from DB
                    Adonet.Adapter = new SqlDataAdapter($"Select * from Users Where Email ='{email}' and Password ='{password}'", Adonet.Cnx);

                    //Fill DataSet with the result
                    Adonet.Adapter.Fill(Adonet.DataSet1, "User");

                    //Close Cnx
                    Adonet.Disconnect();

                    //If their is a result
                    if (Adonet.DataSet1.Tables["User"].Rows.Count > 0)
                    {
                        //Collect user information
                        RoleId = Convert.ToInt32(Adonet.DataSet1.Tables["User"].Rows[0][5]);
                        UserId = Convert.ToInt32(Adonet.DataSet1.Tables["User"].Rows[0][0]);
                        UserName = Adonet.DataSet1.Tables["User"].Rows[0][1].ToString().Trim();
                        UserEmail = Adonet.DataSet1.Tables["User"].Rows[0][2].ToString().Trim();

                        //Check if its a Staff Or Student Then they can have a Class Id
                        if (RoleId > 2)
                        {
                            ClassId = Convert.ToInt32(Adonet.DataSet1.Tables["User"].Rows[0][6].ToString().Trim());
                        }

                        return true;
                    }

                    else
                    {
                        error = "Sorry, user not found !";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + String.Empty);
                return false;
            }
        }

        #endregion


        #region Method User LogOut

        public override void Logout()
        {

        }

        #endregion


        #region Methode Check Attendance
        public override void CheckAttendance()
        {

        }

        #endregion


        #region Search
        //public void Search(int UserId)
        //{

        //}

        public static void Search(string FullName, DataGrid userstable)
        {

        }
        #endregion
    }
}
