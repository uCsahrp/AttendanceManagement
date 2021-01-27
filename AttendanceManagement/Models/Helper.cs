using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AttendanceManagement.Models
{
    public static class Helper
    {

        #region Methode Get Roles To Fill Drop Down

        public static void GetRoles(object comboBox)
        {
            Ado adonet = new Ado();


            adonet.Connect();
            adonet.Adapter = new SqlDataAdapter("Select * From Roles", adonet.Cnx);
            adonet.Adapter.Fill(adonet.DataSet, "Roles");
            adonet.Disconnect();
            var combo = ((ComboBox)comboBox);
            combo.SelectedValuePath = "Role Id";
            combo.DisplayMemberPath = "Role Name";
            combo.ItemsSource = adonet.DataSet.Tables["Roles"].DefaultView;

        }

        #endregion


        public static void GetClasses(object comboBox)
        {
            Ado adonet = new Ado();

            adonet.Connect();
            adonet.Adapter = new SqlDataAdapter("Select * From Classes", adonet.Cnx);
            adonet.Adapter.Fill(adonet.DataSet, "Classes");
            adonet.Disconnect();
            var combo = ((ComboBox)comboBox);
            combo.SelectedValuePath = "Class Id";
            combo.DisplayMemberPath = "Class Name";
            combo.ItemsSource = adonet.DataSet.Tables["Classes"].DefaultView;
        }




        public static void GetUsers(DataGrid usertable)
        {
            Ado adonet = new Ado();
            adonet.Connect();
            adonet.Adapter = new SqlDataAdapter("Select * From Users INNER JOIN Roles ON Users.[Role Id]= Roles.[Role Id] INNER JOIN Classes On Users.[Class Id] = Classes.[Id Class];", adonet.Cnx);
            adonet.Adapter.Fill(adonet.DataSet, "Users");
            adonet.Disconnect();

            try
            {
                usertable.Items.Clear();
                usertable.ItemsSource = adonet.DataSet.Tables["Users"].DefaultView;
            }
            catch (Exception)
            {


            }
        }



        #region getstudentdata

        public static void GetStudent(int userid, DataGrid usertable, TextBlock fullname, TextBlock staffName, TextBlock ClassName)
        {
            Ado adonet = new Ado();
            adonet.Connect();
            adonet.Cmd = new SqlCommand("Select * From Users INNER JOIN Roles ON Users.[Role Id]= Roles.[Role Id] INNER JOIN Classes On Users.[Class Id] = Classes.[Id Class] INNER JOIN Attendance ON Attendance.[Student Id] = Users.[User Id] WHERE [User Id ]=@userid;", adonet.Cnx);
            adonet.Cmd.Parameters.Add("@userid", SqlDbType.VarChar, 200).Value = userid;
            adonet.Adapter.SelectCommand = adonet.Cmd;
            adonet.Adapter.Fill(adonet.DataSet, "student");
            usertable.Items.Clear();
            usertable.ItemsSource = adonet.DataSet.Tables["student"].DefaultView;
            fullname.Text = adonet.DataSet.Tables["student"].Rows[0][1].ToString();
            staffName.Text = adonet.DataSet.Tables["student"].Rows[0][12].ToString();
            ClassName.Text = adonet.DataSet.Tables["student"].Rows[0][10].ToString();
            adonet.Disconnect();




        }


        #endregion


        #region getcountabsent

        public static void getcountabsent(int userid, TextBlock absentdays)
        {
            Ado adonet = new Ado();
            adonet.Connect();
            adonet.Cmd = new SqlCommand("SELECT COUNT(Date) FROM Attendance WHERE [Student Id ]=@userid;", adonet.Cnx);
            adonet.Cmd.Parameters.Add("@userid", SqlDbType.VarChar, 200).Value = userid;
            adonet.Adapter.SelectCommand = adonet.Cmd;
            adonet.Adapter.Fill(adonet.DataSet, "Count");
            //Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
            string count = adonet.Cmd.ExecuteScalar().ToString() + " DAYS";
            absentdays.Text = count;
            adonet.Disconnect();

        }
        #endregion


        #region GetAbsentJustified

        public static void GetCountAbsentJust(int userid, TextBlock absentdaysjust)
        {
            Ado adonet = new Ado();
            adonet.Connect();
            adonet.Cmd = new SqlCommand("SELECT COUNT(Date) FROM Attendance WHERE IsJustified=1 AND [Student Id ]=@userid;", adonet.Cnx);
            adonet.Cmd.Parameters.Add("@userid", SqlDbType.VarChar, 200).Value = userid;
            adonet.Adapter.SelectCommand = adonet.Cmd;
            adonet.Adapter.Fill(adonet.DataSet, "Count");
            //Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
            string count = adonet.Cmd.ExecuteScalar().ToString() + " Absences ";
            absentdaysjust.Text = count;
            adonet.Disconnect();

        }




        #endregion


        #region GetAbsentnotJustified

        public static void GetCountAbsentNotJust(int userid, TextBlock absentdaysNojust)
        {
            Ado adonet = new Ado();
            adonet.Connect();
            adonet.Cmd = new SqlCommand("SELECT COUNT(Date) FROM Attendance WHERE IsJustified=0 AND [Student Id ]=@userid;", adonet.Cnx);
            adonet.Cmd.Parameters.Add("@userid", SqlDbType.VarChar, 200).Value = userid;
            adonet.Adapter.SelectCommand = adonet.Cmd;
            adonet.Adapter.Fill(adonet.DataSet, "Count");
            //Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
            string count = adonet.Cmd.ExecuteScalar().ToString() + " Absences ";
            absentdaysNojust.Text = count;
            adonet.Disconnect();

        }




        #endregion





    }
}
