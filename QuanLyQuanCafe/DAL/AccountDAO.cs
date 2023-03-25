using Microsoft.SqlServer.Management.Smo;
using QuanLyQuanCafe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAL
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string username, string password)
        {
            string query = "USP_Login @userName , @password";

            DataTable result =  DBContext.Instance.ExcuteQuery(query, new object[] {username, password});


            return result.Rows.Count > 0;
        }

        public Account GetAccountByUsername(string username)
        {
            DataTable data = DBContext.Instance.ExcuteQuery("Select * FROM account where Username = '" + username + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public bool UpdateAccount(string username, string displayName, string pass, string newPass) 
        {
            int result = DBContext.Instance.ExcuteNonQuery("USP_UpdateAccount @username , @displayName , @password , @newPass", new object[] {username, displayName, pass, newPass});

            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DBContext.Instance.ExcuteQuery("SELECT username, displayName, type FROM Account");
        }

        public bool InsertAccount(string username, string displayname, int type)
        {
            int result = DBContext.Instance.ExcuteNonQuery("INSERT dbo.Account ([userName], displayname, type) VALUES (N'" + username + "', N'" + displayname + "', " + type + ")");

            return result > 0;
        }

        public bool UpdateAccount(string username, string displayname, int type)
        {
            int result = DBContext.Instance.ExcuteNonQuery("UPDATE dbo.Account SET displayname = N'" + displayname + "', type = " + type + "WHERE username = " + username);

            return result > 0;
        }

        public bool DeleteAccount(string username)
        {
            int result = DBContext.Instance.ExcuteNonQuery("DELETE Account WHERE username = " + username);

            return result > 0;
        }

        public bool ResetPass(string username)
        {
            int result = DBContext.Instance.ExcuteNonQuery("UPDATE Account SET password = N'0' WHERE username = N'" + username+"'");

            return result > 0;
        }
    }
}
