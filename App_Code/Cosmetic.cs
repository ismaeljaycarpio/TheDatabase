using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


/// <summary>
/// Summary description for Cosmetic
/// </summary>
public class Cosmetic
{
	public Cosmetic()
	{
		//
		// TODO: Add constructor logic here
		//
	}





    public static int dbg_ColumnColour_Insert(ColumnColour p_ColumnColour)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ColumnColour_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;


                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);

                command.Parameters.Add(new SqlParameter("@sContext", p_ColumnColour.Context));
                command.Parameters.Add(new SqlParameter("@nID", p_ColumnColour.ID));

                command.Parameters.Add(new SqlParameter("@nControllingColumnID", p_ColumnColour.ControllingColumnID));

                command.Parameters.Add(new SqlParameter("@sOperator", p_ColumnColour.Operator));
                command.Parameters.Add(new SqlParameter("@sValue", p_ColumnColour.Value));
                command.Parameters.Add(new SqlParameter("@sColour", p_ColumnColour.Colour));

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return int.Parse(pRV.Value.ToString());
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return -1;
            }


        }

        
        
    }


    public static int dbg_ColumnColour_Update(ColumnColour p_ColumnColour)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ColumnColour_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nColumnColourID", p_ColumnColour.ColumnColourID));
                command.Parameters.Add(new SqlParameter("@nID", p_ColumnColour.ID));

                command.Parameters.Add(new SqlParameter("@nControllingColumnID", p_ColumnColour.ControllingColumnID));

                command.Parameters.Add(new SqlParameter("@sOperator", p_ColumnColour.Operator));
                command.Parameters.Add(new SqlParameter("@sValue", p_ColumnColour.Value));
                command.Parameters.Add(new SqlParameter("@sColour", p_ColumnColour.Colour));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = 0;
                }

                connection.Close();
                connection.Dispose();

                return i;
            }

        }


      

    }

    public static string fnGetColumnColour(int nRecordID, int nID, string Context)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbo.fnGetColumnColour", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                SqlParameter pRV = new SqlParameter("@Result", SqlDbType.VarChar);
                pRV.Direction = ParameterDirection.ReturnValue;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nRecordID", nRecordID));

                command.Parameters.Add(new SqlParameter("@nID", nID));
                command.Parameters.Add(new SqlParameter("@Context", Context));


                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return pRV.Value.ToString();
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();

                }
                return "";
            }
        }
    }

    public static DataTable dbg_ColumnColour_Select(string sContext, int nID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ColumnColour_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nID", nID));
                command.Parameters.Add(new SqlParameter("@sContext", sContext));



                //connection.Open();

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataTable dt = new DataTable();
                System.Data.DataSet ds = new System.Data.DataSet();


                connection.Open();
                try
                {
                    da.Fill(ds);
                }
                catch
                {
                    //
                }

                connection.Close();
                connection.Dispose();


                if (ds != null &&  ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }


            }
        }
    }



   


    public static int dbg_ColumnColour_Delete(int nColumnColourID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ColumnColour_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nColumnColourID ", nColumnColourID));

                int i = 1;
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    i = -1;
                }

                connection.Close();
                connection.Dispose();

                return i;
            }
        }
    }




    public static ColumnColour dbg_ColumnColour_Detail(int nColumnColourID)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("dbg_ColumnColour_Detail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nColumnColourID", nColumnColourID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ColumnColour temp = new ColumnColour(
                                (int)reader["ColumnColourID"],(string)reader["Context"],
                                 (int)reader["ID"],
                               (int)reader["ControllingColumnID"],
                                (string)reader["Operator"],
                               reader["Value"] == DBNull.Value ? "" : (string)reader["Value"],
                                (string)reader["Colour"]
                                );


                           
                            connection.Close();
                            connection.Dispose();
                            

                            return temp;
                        }
                    }
                }
                catch
                {
                  //
                }


                connection.Close();
                connection.Dispose();
                

                return null;

            }

        }        
    }


}