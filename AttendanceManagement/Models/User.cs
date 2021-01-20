using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Models
{
    public abstract class User
    {
        #region UserPropeties

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public string UserAvatar { get; set; }

        public int RoleId { get; set; }

        public int ClassId { get; set; }

        public string error;

        #endregion


        #region User Constructor
        public User() { }

        #endregion


        #region UserLogin

        public abstract bool Login(string email, string password);



        #endregion


        #region LogOut

        public abstract void Logout();

        #endregion


        #region CheckAttendance

        public abstract void CheckAttendance();

        #endregion 
    }
}
