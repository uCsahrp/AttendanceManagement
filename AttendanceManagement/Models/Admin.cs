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
        public DataTable usersTable;
        public bool changed = false;
        public void GetUsers(DataGrid usertable)
        {

            ado.Connect();
            ado.Adapter = new SqlDataAdapter("Select * From Users;", ado.Cnx);
            ado.Adapter.Fill(ado.DataSet, "Users");
            ado.Disconnect();
            //usertable.Items.Clear();
            try
            {
                usertable.ItemsSource = ado.DataSet.Tables["Users"].DefaultView;
                usersTable =  ado.DataSet.Tables["Users"];
            }
            catch (Exception)
            {


            }
        }


        #region Add New User

        public bool AddUser(string fullName, string email, string password, string confirmPass, int roleId, int classId)
        {
            //Regex for making sure Email is valid
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            string avatar;
            var rId = roleId + 1;
            var cId = classId + 1;
            switch (rId)
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
                error = "Passwords do not match";
                //MessageBox.Show("Passwords do not match");
                return false;
            }
            //Password must be at least 8 characters long
            else if (password.Length < 6)
            {
                error = "Password must be at least 6 characters long";
                //MessageBox.Show("Password must be at least 6 characters long");
                return false;
            }
            //If email is NOT valid
            else if (!match.Success)
            {
                error = "Invalid Email";
                //MessageBox.Show("Invalid Email");
                return false;
            }
            //If there is no username
            else if (fullName == null)
            {
                error = "User Must have a Name";
                //MessageBox.Show("User Must have a Name");
                return false;
            }
            else if (rId == 0)
            {

                error = "User must have a Role";
                return false;
            }

            else
            {
                string query;
                if (rId > 2)
                {
                    // define INSERT query with parameters
                    query = $"INSERT INTO Users ([Full Name], [Email], [Password], [Avatar], [Role Id], [Class Id]) " +
                                                   "VALUES (@fullName, @email, @password, @avatar, @roleId, @classId) ";
                }
                else
                {
                    // define INSERT query with parameters
                    query = $"INSERT INTO Users ([Full Name], [Email], [Password], [Avatar], [Role Id]) " +
                                                   "VALUES (@fullName, @email, @password, @avatar, @roleId) ";
                }


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
                        cmd.Parameters.Add("@roleId", SqlDbType.Int).Value = rId;
                        if (rId > 2) cmd.Parameters.Add("@classId", SqlDbType.Int).Value = cId;


                        //Open Connection
                        ado.Connect();
                        cmd.ExecuteNonQuery();
                        ado.Disconnect();
                        error = "User Added Successfully.";
                        //GetUsers(userTable);
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

        public bool EditUsers(int id, string fullName, string email, string password, string confirmPass, int classId, int roleId)
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            var cId = classId + 1;
            var rId = roleId + 1;


            //Confirm pass must equal password.
            if (password != confirmPass)
            {
                error = "Passwords do not match";
                //MessageBox.Show("Passwords do not match");
                return false;
            }
            //Password must be at least 8 characters long
            else if (password.Length < 6)
            {
                error = "Password must be at least 6 characters long";
                //MessageBox.Show("Password must be at least 6 characters long");
                return false;
            }
            //If email is NOT valid
            else if (!match.Success)
            {
                error = "Invalid Email";
                //MessageBox.Show("Invalid Email");
                return false;
            }
            //If there is no username
            else if (fullName == null)
            {
                error = "User Must have a Name";
                //MessageBox.Show("User Must have a Name");
                return false;
            }
            else if (rId == 0)
            {
                error = "User must have a Role";
                return false;
            }

            else
            {
                string query;
                if (rId > 2)
                {
                    // define Update query with parameters
                    query = $"UPDATE Users set [Full Name] = @fullName,[Email] = @email, [Password]=@password,[Role Id]=@roleId, [Class Id]=@classId WHERE [User Id]=@id";
                }
                else
                {
                    // define INSERT query with parameters
                    query = $"UPDATE Users set [Full Name] = @fullName,[Email] = @email, [Password]=@password,[Role Id]=@roleId WHERE [User Id]=@id";

                }


                ado.Adapter = new SqlDataAdapter(query, ado.Cnx);
                try
                {
                    using (var cmd = ado.Cmd = new SqlCommand(query, ado.Cnx))
                    {
                        // define parameters and their values
                        cmd.Parameters.Add("@id", SqlDbType.Int, 0).Value = id;
                        cmd.Parameters.Add("@fullName", SqlDbType.VarChar, 200).Value = fullName;
                        cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar, 250).Value = password;
                        cmd.Parameters.Add("@roleId", SqlDbType.Int).Value = rId;
                        if (rId > 2) cmd.Parameters.Add("@classId", SqlDbType.Int).Value = cId;


                        //Open Connection
                        ado.Connect();
                        cmd.ExecuteNonQuery();
                        ado.Disconnect();
                        error = "User updated Successfully.";
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


        #region Delete User

        public void DeleteUser(int IdUser)
        {
            if (IdUser == 0)
            {
                error = "Please select the user you Want to Delete";
            }
            else
            {
                try
                {
                    ado.Connect();
                    ado.Cmd = new SqlCommand($"DELETE FROM [Users] WHERE [User Id]={IdUser} ", ado.Cnx);
                    ado.Cmd.ExecuteNonQuery();
                    ado.Disconnect();
                    error = "User was Deleted Successfully";

                }
                catch (Exception e)
                {
                    MessageBox.Show(e + String.Empty);
                    throw;
                }
            }

        }
        #endregion
    }
}
