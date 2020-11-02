using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApiOauth2.Helper_Code.Classes
{
    public class Constants
    {

        

        #region Token Session Time Out Configuration Value

        public static int GetTokenSessionTimeOut()
        {
            int tokenSessionTimeOutValue = 0;
            try
            {
                tokenSessionTimeOutValue = int.Parse(ConfigurationManager.AppSettings["TokenSessionTime"].ToString());
            }
            catch(Exception ex)
            {
                tokenSessionTimeOutValue = 20;
            }
            return tokenSessionTimeOutValue;
        }

        #endregion

        #region Connection String

        private static string _connectionString = ConfigurationManager.AppSettings["SQLConnectionString"].ToString();
        public static string GetConnectionString()
        {
            return _connectionString;
        }

        #endregion

        public static string AppendTimeStamp(string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }

    }
}