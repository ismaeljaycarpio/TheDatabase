using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;

namespace DocGen.Utility
{    
    public class ConvertUtil
    {
        public static DateTime GetDate(string value, string format)
        {
            DateTime dtValue;
            DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue);
            return dtValue;
        }

        public static DateTime GetDate(string value)
        {
            DateTime dtValue;
            DateTime.TryParseExact(value, ConfigurationManager.AppSettings["DefaultDateFormat"], CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue);
            return dtValue;
        }

        public static DateTime GetDate(TextBox inputControl, string format)
        {
            DateTime dtValue;
            DateTime.TryParseExact(inputControl.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue);
            return dtValue;
        }

        public static DateTime GetDate(TextBox inputControl)
        {
            DateTime dtValue;
            DateTime.TryParseExact(inputControl.Text, ConfigurationManager.AppSettings["DefaultDateFormat"], CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue);
            return dtValue;
        }

        public static string GetDateString(DateTime value)
        {
            return value.CompareTo(DateTime.MinValue) == 0 ? "" : value.ToString(ConfigurationManager.AppSettings["DefaultDateFormat"]);
        }

        public static string GetDateString(DateTime value, string format)
        {
            return value.CompareTo(DateTime.MinValue) == 0 ? "" : value.ToString(format);            
        }
    }
}