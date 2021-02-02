using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for Staff.xaml
    /// </summary>
    public partial class Staff : Window
    {
        public Staff()
        {
            InitializeComponent();
        }



        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-QUDI77S\MSSQLSERVER01;Initial Catalog=AttendanceManagement;Integrated Security=True");
        SqlCommand Cmd;
        SqlDataAdapter Sda;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        public string date = DateTime.Now.ToString("dd-MM-yyyy");



        public void remplir_combo()
        {

            Cmd = new SqlCommand("select [Class Name] from Classes", conn);
            SqlDataReader dr = Cmd.ExecuteReader();
            while (dr.Read())
            {
                comb_classes.Items.Add(dr["Class Name"]);
            }
            dr.Close();
        }


        DataTable Dt = new DataTable();

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            comb_classes.Text = "c#";
            string combo_text = comb_classes.Text;


            dg.Visibility = Visibility.Visible;
            dg1.Visibility = Visibility.Hidden;
            conn.Open();

            remplir_combo();

            Cmd = new SqlCommand("select u.[User Id],u.[Full Name],u.Email,c.[Class Name] from Users u inner join Classes c on u.[Class Id]=c.[Id Class] where u.[Role Id]=4 and c.[Class Name]='" + combo_text + "'", conn);
            SqlDataReader dr = Cmd.ExecuteReader();
            DataTable t = new DataTable();
            t.Load(dr);
            dg.ItemsSource = t.DefaultView;
            dr.Close();

            conn.Close();

            late_temp.Visibility = Visibility.Hidden;
            abs_temp.Visibility = Visibility.Hidden;

        }

        private void btn_ajouter_ab_Click(object sender, RoutedEventArgs e)
        {
            dg.Visibility = Visibility.Visible;
            dg1.Visibility = Visibility.Hidden;
            late_temp.Visibility = Visibility.Visible;
            abs_temp.Visibility = Visibility.Visible;

        }



        private void check_absent_unchecked(object sender, RoutedEventArgs e)
        {

            DataRowView row = dg.SelectedItem as DataRowView;
            int id_student = Convert.ToInt32(row.Row[0].ToString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("delete from Attendance where [Student id]='" + id_student + "' and Date = '" + date + "' and Absent = 'oui'", conn);
                cmd1.ExecuteNonQuery();
                conn.Close();
            }

        }



        private void check_retard_checked(object sender, RoutedEventArgs e)
        {
            check_absent_unchecked(sender, e);
            DataRowView row = dg.SelectedItem as DataRowView;
            int id_student = Convert.ToInt32(row.Row[0].ToString());
            conn.Open();
            SqlCommand cmd1 = new SqlCommand("INSERT INTO Attendance (Date, IsJustified, [Student id],Absent,Retard) VALUES ('" + date + "','false','" + id_student + "', 'non' , 'oui')", conn);
            cmd1.ExecuteNonQuery();
            conn.Close();
        }

        private void check_retard_unchecked(object sender, RoutedEventArgs e)
        {
            DataRowView row = dg.SelectedItem as DataRowView;
            int id_student = Convert.ToInt32(row.Row[0].ToString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("delete from Attendance where [Student id]='" + id_student + "' and Date = '" + date + "'and retard = 'oui'", conn);
                cmd1.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void afficher_les_absents_Click(object sender, RoutedEventArgs e)
        {
            string combo_text = comb_classes.Text;
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                dg.Visibility = Visibility.Hidden;
                dg1.Visibility = Visibility.Visible;


                Cmd = new SqlCommand("select a.[Date] , u.[Full Name] , a.[Description] ,c.[Class Name] From Users u inner join Attendance a on a.[Student Id] =u.[User Id] inner join Classes c on c.[Id Class]=u.[Class Id] where a.[Date]='" + date + "' and c.[Class Name]='" + combo_text + "' ", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                DataTable t = new DataTable();
                t.Load(dr);
                dr.Close();
                dg1.ItemsSource = t.DefaultView;
            }
            else
            {
                dg.Visibility = Visibility.Hidden;
                dg1.Visibility = Visibility.Visible;


                Cmd = new SqlCommand("select a.[Date], u.[Full Name], a.[Description], c.[Class Name] From Users u inner join Attendance a on a.[Student Id] = u.[User Id] inner join Classes c on c.[Id Class] = u.[Class Id] where a.[Date]='" + date + "' and c.[Class Name]='" + combo_text + "' ", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                DataTable t = new DataTable();
                t.Load(dr);
                dr.Close();
                dg1.ItemsSource = t.DefaultView;
            }




        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void check_absent_checked(object sender, RoutedEventArgs e)
        {
            check_retard_unchecked(sender, e);
            DataRowView row = dg.SelectedItem as DataRowView;
            int id_student = Convert.ToInt32(row.Row[0].ToString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("INSERT INTO Attendance (Date, IsJustified, [Student id],Absent,Retard) VALUES ('" + date + "','false','" + id_student + "', 'oui' , 'non')", conn);
                cmd1.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void text_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                dg.Visibility = Visibility.Visible;
                dg1.Visibility = Visibility.Hidden;
                late_temp.Visibility = Visibility.Visible;
                abs_temp.Visibility = Visibility.Visible;
                string text = text_search.Text;
                Cmd = new SqlCommand("select u.[User Id],u.[Full Name],u.Email,c.[Class Name] from Users u inner join Classes c on u.[Class Id]=c.[Id Class] where u.[Role Id]=4 and u.[Full Name] like '%" + text + "%' ", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                DataTable t = new DataTable();
                t.Load(dr);
                dg.ItemsSource = t.DefaultView;
                dr.Close();

                conn.Close();
            }
            else
            {
                dg.Visibility = Visibility.Visible;
                dg1.Visibility = Visibility.Hidden;
                late_temp.Visibility = Visibility.Visible;
                abs_temp.Visibility = Visibility.Visible;
                string text = text_search.Text;
                Cmd = new SqlCommand("select u.[User Id],u.[Full Name],u.Email,c.[Class Name] from Users u inner join Classes c on u.[Class Id]=c.[Id Class] where u.[Role Id]=4 and u.[Full Name] like '%" + text + "%' ", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                DataTable t = new DataTable();
                t.Load(dr);
                dg.ItemsSource = t.DefaultView;
                dr.Close();

                conn.Close();
            }



        }

        private void comb_classes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string combo_text = comb_classes.SelectedValue.ToString();
            MessageBox.Show(combo_text);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();

                dg.Visibility = Visibility.Hidden;
                dg1.Visibility = Visibility.Visible;


                Cmd = new SqlCommand("select u.[User Id],u.[Full Name],u.Email,c.[Class Name] from Users u inner join Classes c on u.[Class Id]=c.[Id Class] where u.[Role Id]=4 and c.[Class Name]='" + combo_text + "'", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                DataTable t = new DataTable();
                t.Load(dr);
                dr.Close();
                dg1.ItemsSource = t.DefaultView;
            }
            else
            {
                dg.Visibility = Visibility.Hidden;
                dg1.Visibility = Visibility.Visible;




                Cmd = new SqlCommand("select u.[User Id],u.[Full Name],u.Email,c.[Class Name] from Users u inner join Classes c on u.[Class Id]=c.[Id Class] where u.[Role Id]=4 and c.[Class Name]='" + combo_text + "'", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                DataTable t = new DataTable();
                t.Load(dr);
                dr.Close();
                dg1.ItemsSource = t.DefaultView;
            }
        }
    }
}
