using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.Models
{
    public class Account
    {
        public string Username { get; set; }
        public string DisplayName { get; set; } 

        public string Password { get; set; }

        public int Type { get; set; }

        public Account(string username, string displayName, int type, string password = null)
        {
            Username = username;
            DisplayName = displayName;
            Password = password;
            Type = type;
        }

        public Account(DataRow row)
        {
            Username = row["username"].ToString();
            DisplayName = row["displayName"].ToString();
            Password = row["password"].ToString();
            Type = (int)row["type"];
        }
    }
}
