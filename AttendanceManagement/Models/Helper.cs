using System;
using System.Collections.Generic;
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

    }
}
