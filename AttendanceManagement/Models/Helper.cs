using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static ObservableCollection<DataRow> observableCollection = new ObservableCollection<DataRow>();


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


        public static string SetAvatar(int RoleId)
        {
            //Set The Avatar Based on RoleId
            switch (RoleId)
            {
                case 0:
                    return String.Empty;
                case 1:
                    return "Assets/Avatars/admin.png";
                case 2:
                    return "Assets/Avatars/secretary.png";
                case 3:
                    return "Assets/Avatars/staff.png";
                default:
                    return "Assets/Avatars/student.png";
            }
        }

        public static void GetUsers(DataGrid usertable)
        {
            Ado adonet = new Ado();

            adonet.Connect();
            Task.Run(() =>
            {
                usertable.Items.Clear();
                // Views.Admin admin = new Views.Admin();

                adonet.Cmd.CommandText = "Select u.[User Id], u.[Full Name], u.Email, r.[Role Name],c.[Class Name] From Users u INNER JOIN Roles r ON u.[Role Id]= r.[Role Id] Left JOIN Classes c On u.[Class Id] = c.[Id Class]; ";
                adonet.Cmd.Connection = adonet.Cnx;
                adonet.DataReader = adonet.Cmd.ExecuteReader();
                adonet.Datatable.Load(adonet.DataReader);
                adonet.Disconnect();
            });

            usertable.ItemsSource = adonet.Datatable.DefaultView;

        }







    }
}
