using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace DocGen.DAL
{
    public class DBUtil
    {
        public static List<SPInputParam> GetSPInputParams(string SPName)
        {
            List<SPInputParam> lstParams = new List<SPInputParam>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CnString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetInputParams", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SPName", SPName);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lstParams.Add(new SPInputParam() {
                        Name = reader["PARAMETER_NAME"].ToString(),
                        DataType = reader["DATA_TYPE"].ToString(),
                        MaxCharLength = Convert.ToInt32(reader["CHARACTER_MAXIMUM_LENGTH"])
                    });
                }
                reader.Close();
                reader.Dispose();
                conn.Close();
            }
            return lstParams;
        }

        public static DataTable ExecuteSP(string ConnectionString, string SPName, List<SPInputParam> SpParams, out string Message)
        {
            DataTable result = new DataTable();
            Message = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(SPName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SPInputParam p in SpParams)
                    {
                        cmd.Parameters.AddWithValue(p.Name, p.Value);
                    }
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(result);
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                Message = ex.Message;
            }
            return result;
        }

        public static DataTable ExecuteSP(string SPName, List<SPInputParam> SpParams, out string Message)
        {
            return ExecuteSP(ConfigurationManager.ConnectionStrings["CnString"].ConnectionString, SPName, SpParams, out Message);            
        }

        public static DataTable ExecuteSQL(string ConnectionString, string SQLStatement, out string Message)
        {
            DataTable result = new DataTable();
            Message = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(SQLStatement, conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(result);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return result;
        }

        public static DataTable ExecuteSQL(string SQLStatement, out string Message)
        {
            return ExecuteSQL(ConfigurationManager.ConnectionStrings["CnString"].ConnectionString, SQLStatement, out Message);            
        }
    }

    public class SPInputParam
    {
        public string Name{get; set;}
        public string DataType{get; set;}
        public int MaxCharLength{get; set;}
        public object Value { get; set; }
    }
}