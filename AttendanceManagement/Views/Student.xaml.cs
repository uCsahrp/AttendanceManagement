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
            Models.Student student = new Models.Student();
            student.
            var userid = Login.userid;
            student.GetStudent(userid, studentattendence, studentName, TeacherName, ClassName);
            student.getcountabsent(userid, absentdays);
            student.GetCountAbsentJust(userid, absentdaysjust);
            student.GetCountAbsentNotJust(userid, absentdaysNojust);
      

        }
    }

}
