using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for InvoiceManager
/// </summary>
public class InvoiceManager
{
	public InvoiceManager()
	{
		//
		// TODO: Add constructor logic here
		//

    }



    public static int ets_Invoice_Insert(Invoice p_Invoice)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Invoice_Insert", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                SqlParameter pRV = new SqlParameter("@nNewID", SqlDbType.Int);
                pRV.Direction = ParameterDirection.Output;

                command.Parameters.Add(pRV);
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Invoice.AccountID));
                command.Parameters.Add(new SqlParameter("@sAccountName", p_Invoice.AccountName));
                command.Parameters.Add(new SqlParameter("@nAccountTypeID", p_Invoice.AccountTypeID));
                command.Parameters.Add(new SqlParameter("@nNetAmountAUD", p_Invoice.NetAmountAUD));
                command.Parameters.Add(new SqlParameter("@nGSTAmountAUD", p_Invoice.GSTAmountAUD));

                command.Parameters.Add(new SqlParameter("@nGrossAmountAUD", p_Invoice.GrossAmountAUD));
                command.Parameters.Add(new SqlParameter("@dInvoiceDate", p_Invoice.InvoiceDate));
                command.Parameters.Add(new SqlParameter("@dStartDate", p_Invoice.StartDate));
                command.Parameters.Add(new SqlParameter("@dEndDate", p_Invoice.EndDate));
                command.Parameters.Add(new SqlParameter("@sPaymentMethod", p_Invoice.PaymentMethod));
                command.Parameters.Add(new SqlParameter("@nPaypalID", p_Invoice.PaypalID));
                command.Parameters.Add(new SqlParameter("@dPaidDate", p_Invoice.PaidDate));
                command.Parameters.Add(new SqlParameter("@sNotes", p_Invoice.Notes));
                command.Parameters.Add(new SqlParameter("@sOrganisationName", p_Invoice.OrganisationName));
                command.Parameters.Add(new SqlParameter("@sBillingEmail", p_Invoice.BillingEmail));
                command.Parameters.Add(new SqlParameter("@sBillingAddress", p_Invoice.BillingAddress));
                command.Parameters.Add(new SqlParameter("@sCountry", p_Invoice.Country));
                command.Parameters.Add(new SqlParameter("@sClientRef", p_Invoice.ClientRef));

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


    public static int ets_Invoice_Update(Invoice p_Invoice)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Invoice_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nInvoiceID", p_Invoice.InvoiceID));
                command.Parameters.Add(new SqlParameter("@nAccountID", p_Invoice.AccountID));
                command.Parameters.Add(new SqlParameter("@sAccountName", p_Invoice.AccountName));
                command.Parameters.Add(new SqlParameter("@nAccountTypeID", p_Invoice.AccountTypeID));
                command.Parameters.Add(new SqlParameter("@nNetAmountAUD", p_Invoice.NetAmountAUD));
                command.Parameters.Add(new SqlParameter("@nGSTAmountAUD", p_Invoice.GSTAmountAUD));

                command.Parameters.Add(new SqlParameter("@nGrossAmountAUD", p_Invoice.GrossAmountAUD));
                command.Parameters.Add(new SqlParameter("@dInvoiceDate", p_Invoice.InvoiceDate));
                command.Parameters.Add(new SqlParameter("@dStartDate", p_Invoice.StartDate));
                command.Parameters.Add(new SqlParameter("@dEndDate", p_Invoice.EndDate));
                command.Parameters.Add(new SqlParameter("@sPaymentMethod", p_Invoice.PaymentMethod));
                command.Parameters.Add(new SqlParameter("@nPaypalID", p_Invoice.PaypalID));
                command.Parameters.Add(new SqlParameter("@dPaidDate", p_Invoice.PaidDate));
                command.Parameters.Add(new SqlParameter("@sNotes", p_Invoice.Notes));
                command.Parameters.Add(new SqlParameter("@sOrganisationName", p_Invoice.OrganisationName));
                command.Parameters.Add(new SqlParameter("@sBillingEmail", p_Invoice.BillingEmail));
                command.Parameters.Add(new SqlParameter("@sBillingAddress", p_Invoice.BillingAddress));
                command.Parameters.Add(new SqlParameter("@sCountry", p_Invoice.Country));
                command.Parameters.Add(new SqlParameter("@sClientRef", p_Invoice.ClientRef));

                command.Parameters.Add(new SqlParameter("@nPaidAmount", p_Invoice.PaidAmount));

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



    public static DataTable ets_Invoice_Select(int? nAccountID, string sAccountName,int? nAccountTypeID,
       DateTime? dInvoiceDateFrom, DateTime? dInvoiceDateTo,  string sPaymentMethod,
        bool? bIsPaid, string sOrganisationName, string sBillingEmail, string sBillingAddress,
        string sCountry,string sClientRef,DateTime? dDateAdded, DateTime? dDateUpdated, string sOrder,
      string sOrderDirection, int? nStartRow, int? nMaxRows, ref int iTotalRowsNum)
    {

        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Invoice_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@nAccountID", nAccountID));

                if (sAccountName != "")
                    command.Parameters.Add(new SqlParameter("@sAccountName", sAccountName));

                if (nAccountTypeID != null)
                    command.Parameters.Add(new SqlParameter("@nAccountTypeID", nAccountTypeID));

                if (dInvoiceDateFrom != null)
                    command.Parameters.Add(new SqlParameter("@dInvoiceDateFrom", dInvoiceDateFrom));
                if (dInvoiceDateTo != null)
                    command.Parameters.Add(new SqlParameter("@dInvoiceDateTo", dInvoiceDateTo));


               
                if (sPaymentMethod != "")
                    command.Parameters.Add(new SqlParameter("@sPaymentMethod", sPaymentMethod));
                if (bIsPaid != null)
                    command.Parameters.Add(new SqlParameter("@bIsPaid", bIsPaid));
                if (sOrganisationName != "")
                    command.Parameters.Add(new SqlParameter("@sOrganisationName", sOrganisationName));
                if (sBillingEmail != "")
                    command.Parameters.Add(new SqlParameter("@sBillingEmail", sBillingEmail));
                if (sBillingAddress != "")
                    command.Parameters.Add(new SqlParameter("@sBillingAddress", sBillingAddress));
                if (sCountry != "")
                    command.Parameters.Add(new SqlParameter("@sCountry", sCountry));
                if (sClientRef != "")
                    command.Parameters.Add(new SqlParameter("@sClientRef", sClientRef));
           
                if (dDateAdded != null)
                    command.Parameters.Add(new SqlParameter("@dDateAdded", dDateAdded));

                if (dDateUpdated != null)
                    command.Parameters.Add(new SqlParameter("@dDateUpdated", dDateUpdated));
                               

                if (string.IsNullOrEmpty(sOrder) || string.IsNullOrEmpty(sOrderDirection))
                { sOrder = "InvoiceID"; sOrderDirection = "DESC"; }

                command.Parameters.Add(new SqlParameter("@sOrder", sOrder + " " + sOrderDirection));

                if (nStartRow != null)
                    command.Parameters.Add(new SqlParameter("@nStartRow", nStartRow + 1));

                if (nMaxRows != null)
                    command.Parameters.Add(new SqlParameter("@nMaxRows", nMaxRows));


                

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

                iTotalRowsNum = 0;
                if (ds == null) return null;


                if (ds.Tables.Count > 1)
                {
                    iTotalRowsNum = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                }
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                {
                    return null;
                }


            }
        }
    }




    public static int ets_Invoice_Delete(int nInvoiceID)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Invoice_Delete", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nInvoiceID ", nInvoiceID));

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




    public static Invoice ets_Invoice_Detail(int nInvoiceID)
    {


        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("ets_Invoice_Details", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.Add(new SqlParameter("@nInvoiceID", nInvoiceID));

                connection.Open();

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Invoice temp = new Invoice(
                                (int)reader["InvoiceID"], reader["AccountID"] == DBNull.Value ? null : (int?)reader["AccountID"],
                               reader["AccountName"] == DBNull.Value ? "" : (string)reader["AccountName"],
                               reader["AccountTypeID"] == DBNull.Value ? null : (int?)reader["AccountTypeID"], (double?)double.Parse(reader["NetAmountAUD"].ToString()),
                               reader["GSTAmountAUD"] == DBNull.Value ? null : (double?)double.Parse(reader["GSTAmountAUD"].ToString()),
                                reader["GrossAmountAUD"] == DBNull.Value ? null : (double?)double.Parse(reader["GrossAmountAUD"].ToString()),
                                 reader["InvoiceDate"] == DBNull.Value ? null : (DateTime?)reader["InvoiceDate"],
                                  reader["StartDate"] == DBNull.Value ? null : (DateTime?)reader["StartDate"],
                                   reader["EndDate"] == DBNull.Value ? null : (DateTime?)reader["EndDate"],
                                    reader["PaymentMethod"] == DBNull.Value ? "" : (string)reader["PaymentMethod"],
                                    reader["PaypalID"] == DBNull.Value ? null : (int?)reader["PaypalID"],
                                    reader["PaidDate"] == DBNull.Value ? null : (DateTime?)reader["PaidDate"],
                                    reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"],
                                    reader["OrganisationName"] == DBNull.Value ? "" : (string)reader["OrganisationName"],
                                    reader["BillingEmail"] == DBNull.Value ? "" : (string)reader["BillingEmail"],
                                    reader["BillingAddress"] == DBNull.Value ? "" : (string)reader["BillingAddress"],
                                    reader["Country"] == DBNull.Value ? "" : (string)reader["Country"],
                                    reader["ClientRef"] == DBNull.Value ? "" : (string)reader["ClientRef"],
                                (DateTime)reader["DateAdded"],
                                (DateTime)reader["DateUpdated"]
                                );
                            temp.PaidAmount = reader["PaidAmount"] == DBNull.Value ? null : (double?)double.Parse(reader["PaidAmount"].ToString());


                            connection.Close();
                            connection.Dispose();

                            return temp;
                        }

                    }
                }
                catch
                {
                   
                }

                connection.Close();
                connection.Dispose();
                return null;

            }

        }



      

    }






	
}




[Serializable]
public class Invoice
{
    private int? _iInvoiceID;
    private int? _iAccountID;
    private string _strAccountName;
    private int? _iAccountTypeID;
    private double? _dNetAmountAUD;
    private double? _dGSTAmountAUD;
    private double? _dGrossAmountAUD;
    private DateTime? _dateInvoiceDate;
    private DateTime? _dateStartDate;
    private DateTime? _dateEndDate;
    private string _strPaymentMethod;
    private int? _iPaypalID;
    private DateTime? _datePaidDate;
    private string _strNotes;
    private string _strOrganisationName;
    private string _strBillingEmail;
    private string _strBillingAddress;
    private string _strCountry;
    private string _strClientRef;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;

    public double? PaidAmount { get; set; }

    public Invoice(int? p_iInvoiceID, int? p_iAccountID, string p_strAccountName,
        int? p_iAccountTypeID, double? p_dNetAmountAUD, double? p_dGSTAmountAUD, double? p_dGrossAmountAUD,
        DateTime? p_dateInvoiceDate, DateTime? p_dateStartDate, DateTime? p_dateEndDate,
        string p_strPaymentMethod, int? p_iPaypalID, DateTime? p_datePaidDate,
        string p_strNotes, string p_strOrganisationName, string p_strBillingEmail, string p_strBillingAddress,
        string p_strCountry, string p_strClientRef,
        DateTime? p_dateAdded, DateTime? p_dateUpdated)
    {
        _iInvoiceID = p_iInvoiceID;
        _iAccountID = p_iAccountID;
        _strAccountName = p_strAccountName;
        _iAccountTypeID = p_iAccountTypeID;
        _dNetAmountAUD = p_dNetAmountAUD;
        _dGSTAmountAUD = p_dGSTAmountAUD;
        _dGrossAmountAUD = p_dGrossAmountAUD;
        _dateInvoiceDate = p_dateInvoiceDate;
        _dateStartDate = p_dateStartDate;
        _dateEndDate = p_dateEndDate;
        _strPaymentMethod = p_strPaymentMethod;
        _iPaypalID = p_iPaypalID;
        _datePaidDate = p_datePaidDate;
        _strNotes = p_strNotes;
        _strOrganisationName = p_strOrganisationName;
        _strBillingEmail = p_strBillingEmail;
        _strBillingAddress = p_strBillingAddress;
        _strCountry = p_strCountry;
        _strClientRef = p_strClientRef;

        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
    }

    public int? InvoiceID
    {
        get { return _iInvoiceID; }
        set { _iInvoiceID = value; }
    }
    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public string AccountName
    {
        get { return _strAccountName; }
        set { _strAccountName = value; }
    }
    public int? AccountTypeID
    {
        get { return _iAccountTypeID; }
        set { _iAccountTypeID = value; }
    }

   
  
  

    public Double? NetAmountAUD
    {
        get { return _dNetAmountAUD; }
        set { _dNetAmountAUD = value; }
    }
  
    public Double? GSTAmountAUD
    {
        get { return _dGSTAmountAUD; }
        set { _dGSTAmountAUD = value; }
    }
    public Double? GrossAmountAUD
    {
        get { return _dGrossAmountAUD; }
        set { _dGrossAmountAUD = value; }
    }


    public DateTime? InvoiceDate
    {
        get { return _dateInvoiceDate; }
        set { _dateInvoiceDate = value; }
    }
    public DateTime? StartDate
    {
        get { return _dateStartDate; }
        set { _dateStartDate = value; }
    }
    public DateTime? EndDate
    {
        get { return _dateEndDate; }
        set { _dateEndDate = value; }
    }


    public string PaymentMethod
    {
        get { return _strPaymentMethod; }
        set { _strPaymentMethod = value; }
    }
    
    public int? PaypalID
    {
        get { return _iPaypalID; }
        set { _iPaypalID = value; }
    }

    public DateTime? PaidDate
    {
        get { return _datePaidDate; }
        set { _datePaidDate = value; }
    }


    public string Notes
    {
        get { return _strNotes; }
        set { _strNotes = value; }
    }

    public string OrganisationName
    {
        get { return _strOrganisationName; }
        set { _strOrganisationName = value; }
    }

    public string BillingEmail
    {
        get { return _strBillingEmail; }
        set { _strBillingEmail = value; }
    }

    public string BillingAddress
    {
        get { return _strBillingAddress; }
        set { _strBillingAddress = value; }
    }

    public string Country
    {
        get { return _strCountry; }
        set { _strCountry = value; }
    }

    public string ClientRef
    {
        get { return _strClientRef; }
        set { _strClientRef = value; }
    }

    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}