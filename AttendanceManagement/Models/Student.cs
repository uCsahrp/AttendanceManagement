using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AttendanceManagement.Models;
using AttendanceManagement.Views;

namespace AttendanceManagement.Models
{
    class Student : UserModel
    {

        public Student()
        {

        }

        #region getstudentdata

        public void GetStudent(int userid, DataGrid usertable, TextBlock fullname, TextBlock staffName, TextBlock ClassName)
        {
            Ado adonet = new Ado();
            adonet.Connect();
            adonet.Cmd = new SqlCommand("Select * From Users INNER JOIN Roles ON Users.[Role Id]= Roles.[Role Id] INNER JOIN Classes On Users.[Class Id] = Classes.[Id Class] Left JOIN Attendance ON Attendance.[Student Id] = Users.[User Id] WHERE[User Id]=@userid;", adonet.Cnx);
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

        public void getcountabsent(int userid, TextBlock absentdays)
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

        public void GetCountAbsentJust(int userid, TextBlock absentdaysjust)
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

        public void GetCountAbsentNotJust(int userid, TextBlock absentdaysNojust)
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

