using AttendanceManagement.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : Window
    {
        
        public Student()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var userid = Login.userid;
            Helper.GetStudent(userid, studentattendence, studentName, TeacherName, ClassName);
            Helper.getcountabsent(userid, absentdays);
            Helper.GetCountAbsentJust(userid, absentdaysjust);
            Helper.GetCountAbsentNotJust(userid, absentdaysNojust);


        }
    }
}
