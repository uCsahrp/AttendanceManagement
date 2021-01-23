using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AttendanceManagement.Views;

namespace AttendanceManagement.Models
{
    class Admin : UserModel
    {
        private Ado ado = new Ado();

        public void GetUsers(DataGrid usertable)
        {
            ado.Adapter = new SqlDataAdapter("Select * From Users;", ado.Cnx);
            ado.Adapter.Fill(ado.DataSet, "Users");
            usertable.ItemsSource = ado.DataSet.Tables["users"].DefaultView;

        }

        #region Add New User

        public bool AddUser(string fullName, string email, string password, string confirmPass, int roleId, int classId)
        {
            //Regex for making sure Email is valid
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            string avatar;
            roleId++;
            classId++;
            switch (roleId)
            {
                case 1:
                    avatar = "Avatars/Admin.png";

                    break;
                case 2:
                    avatar = "Avatars/Secretary.png";
                    break;
                case 3:
                    avatar = "Avatars/Staff.png";
                    break;
                case 4:
                    avatar = "Avatars/Student.png";
                    break;
                default:
                    avatar = string.Empty;
                    break;
            }

            //Confirm pass must equal password.
            if (password != confirmPass)
            {
                MessageBox.Show("Passwords do not match");
                return false;
            }
            //Password must be at least 8 characters long
            else if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long");
                return false;
            }
            //If email is NOT valid
            else if (!match.Success)
            {
                MessageBox.Show("Invalid Email");
                return false;
            }
            //If there is no username
            else if (fullName == null)
            {
                MessageBox.Show("Must have Username");
                return false;
            }

            else
            {
                // define INSERT query with parameters
                string query = "INSERT INTO Users ([Full Name], [Email], [Password], [Avatar], [Role Id], [Class Id]) " +
                               "VALUES (@fullName, @email, @password, @avatar, @roleId, @classId) ";

                ado.Adapter = new SqlDataAdapter(query, ado.Cnx);
                try
                {
                    using (var cmd = ado.Cmd = new SqlCommand(query, ado.Cnx))
                    {
                        // define parameters and their values
                        cmd.Parameters.Add("@fullName", SqlDbType.VarChar, 200).Value = fullName;
                        cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar, 250).Value = password;
                        cmd.Parameters.Add("@avatar", SqlDbType.VarChar, 250).Value = avatar;
                        cmd.Parameters.Add("@roleId", SqlDbType.Int).Value = roleId;
                        if (roleId > 2)
                        {
                            cmd.Parameters.Add("@classId", SqlDbType.VarChar, 50).Value = classId;
                        }
                        else
                        {
                            cmd.Parameters.Add("@classId", SqlDbType.VarChar, 50).Value = null;
                        }

                        //Open Connection
                        ado.Connect();
                        cmd.ExecuteNonQuery();
                        ado.Disconnect();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex + string.Empty);
                    throw;
                }

            }
        }

        #endregion


        #region Edit USERS

        void EditUsers()
        {


        }




        #endregion
    }
}
