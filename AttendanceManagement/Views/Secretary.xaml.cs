using AttendanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

namespace AttendanceManagement.Views
{
    /// <summary>
    /// Interaction logic for Secretary.xaml
    /// </summary>
    public partial class Secretary : Window
    {
        public Secretary()
        {
            InitializeComponent();
        }

        public bool justf;

        SqlConnection conn =
            new SqlConnection(
                @"Data Source=ADAM-DELL; initial catalog=AttendanceManagement; integrated security=true;");

        SqlCommand Cmd;
        SqlDataReader dr;
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        public void affi_filter(string f)
        {
            Cmd = new SqlCommand(
             "select a.[Student Id] ,u.[Full Name] , a.[Date] , a.[Description] ,c.[Class Name], a.IsJustified From Users u inner join Attendance a on a.[Student Id] =u.[User Id] inner join Classes c on c.[Id Class]=u.[Class Id] where [Class Name]= '" +
             f + "' ", conn);
            SqlDataReader dr = Cmd.ExecuteReader();
            DataTable t = new DataTable();
            t.Load(dr);
            dg.ItemsSource = t.DefaultView;
            dr.Close();
            conn.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            if (combo_class.Text == "All")
            {

                Cmd = new SqlCommand(
                    "select a.[Student Id] ,u.[Full Name] , a.[Date] , a.[Description] ,c.[Class Name], a.IsJustified From Users u inner join Attendance a on a.[Student Id] =u.[User Id] inner join Classes c on c.[Id Class]=u.[Class Id]",
                    conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                DataTable t = new DataTable();
                t.Load(dr);
                dg.ItemsSource = t.DefaultView;
                dr.Close();
                conn.Close();
            }
            else if (combo_class.Text == "c#")
            {

                affi_filter("c#");
            }
            else if (combo_class.Text == "JEE")
            {

                affi_filter("JEE");

            }
            else if (combo_class.Text == "FEBE")
            {

                affi_filter("FEBE");

            }
            else if (combo_class.Text == "classe1")
            {

                affi_filter("classe1");

            }
            else if (combo_class.Text == "classe2")
            {
                affi_filter("classe2");

            }
            else if (combo_class.Text == "classe3")
            {

                affi_filter("classe3");

            }
            else if (combo_class.Text == "classe4")
            {

                affi_filter("classe4");
            }
            conn.Close();

        }


        private void Button_Click_2(object sender, RoutedEventArgs e)

        {
            // update

            DataRowView row = dg.SelectedItem as DataRowView;
            int id_student = Convert.ToInt32(row.Row[0].ToString());
            string date = row.Row[2].ToString();
            string description = desc.Text;

            conn.Open();
            SqlCommand cmd =
                new SqlCommand(
                    "update Attendance set  [Description] = '" + description + "' , IsJustified  = '" + justf +
                    "'WHERE [Student Id]='" + id_student + "' and Date = '" + date + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Les données ont été bien Enregistrées");


        }

        private void IsVerified_Checked(object sender, RoutedEventArgs e)
        {
            justf = true;

        }

        private void IsVerified_Unchecked(object sender, RoutedEventArgs e)
        {
            justf = false;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            Ado ado = new Ado();
            var query =
                "select a.[Student Id] ,u.[Full Name] , a.[Date] , a.[Description] ,c.[Class Name], a.IsJustified From Users u inner join Attendance a on a.[Student Id] =u.[User Id] inner join Classes c on c.[Id Class]=u.[Class Id]";
            ado.Adapter = new SqlDataAdapter(query, ado.Cnx);
            ado.Adapter.Fill(ado.DataSet);
            dg.ItemsSource = ado.DataSet.Tables[0].DefaultView;

        }

    }
}

