using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AttendanceManagement.Models;

namespace AttendanceManagement.Controllers
{
    class UserModel : User
    {


        //New Ado For Connection
        Ado Adonet = new Ado();


        #region Methode Login

        public override bool Login(string email, string password)
        {
            //Check if Email Entered
            if (email.Length == 0)
            {
                error = "Please enter your email.";
                return false;
            }
            //Check if it's a valid email
            else if (IsValidEmail(email))
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
                Adonet.Adapter.Fill(Adonet.DataSet, "User");

                //Close Cnx
                Adonet.Disconnect();

                //If their is a result
                if (Adonet.DataSet.Tables["User"].Rows.Count > 0)
                {

                    //Collect user information
                    RoleId = Convert.ToInt32(Adonet.DataSet.Tables["User"].Rows[0][5]);
                    UserId = Convert.ToInt32(Adonet.DataSet.Tables["User"].Rows[0][0]);
                    UserName = Adonet.DataSet.Tables["User"].Rows[0][1].ToString().Trim();
                    UserEmail = Adonet.DataSet.Tables["User"].Rows[0][2].ToString().Trim();

                    //Check if its a Staff Or Student Then they can have a Class Id
                    if (RoleId > 2)
                    {
                        ClassId = Convert.ToInt32(Adonet.DataSet.Tables["User"].Rows[0][6].ToString().Trim());
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

        #endregion


        #region Helper To Check if it's a valid email

        public bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                    + "@"
                                    + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            if (!regex.IsMatch(email))
                return true;
            else
                return false;
        }

        #endregion


        #region Method User LogOut

        public override void Logout()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Methode Check Attendance
        public override void CheckAttendance()
        {

        }

        #endregion


    }
}
