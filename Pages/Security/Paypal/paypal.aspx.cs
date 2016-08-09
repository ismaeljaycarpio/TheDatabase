using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Mail;


public partial class paypal : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        //Post back to either sandbox or live
        //string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        //string strLive = "https://www.paypal.com/cgi-bin/webscr";

        string strPaypalURL=SecurityManager.GetPaypalActionURL();
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strPaypalURL);

        //Set values for the request back
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded";
        byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
        string strRequest = Encoding.ASCII.GetString(param);
        strRequest += "&cmd=_notify-validate";
        req.ContentLength = strRequest.Length;

        //for proxy
        //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
        //req.Proxy = proxy;

        //Send the request to PayPal and get the response
        StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
        streamOut.Write(strRequest);
        streamOut.Close();
        StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
        string strResponse = streamIn.ReadToEnd();
        streamIn.Close();

        

      

        //if (Request.QueryString["orderID"] != null)
        //{
        //    txWriter.WriteLine("orderID:" + Request.QueryString["orderID"].ToString());
        //}

     

        if (strResponse == "VERIFIED")
        {

            //Put info into text file for testing

            string strInvoice_status = HttpContext.Current.Request["Invoice_status"];
            string strInvoiceID=HttpContext.Current.Request["custom"];
            string strreceiver_email=HttpContext.Current.Request["receiver_email"];
            string strmc_gross = HttpContext.Current.Request["mc_gross"];
            string strtxn_id=HttpContext.Current.Request["txn_id"];

            if (strInvoice_status == null)
                strInvoice_status = "";

            if (strInvoiceID == null)
                strInvoiceID = "";

            if (strreceiver_email == null)
                strreceiver_email = "";

            if (strmc_gross == null)
                strmc_gross = "";

            if (strtxn_id == null)
                strtxn_id = "";

            TextWriter txWriter = new StreamWriter(Server.MapPath("Upload/") + "verified_" + string.Format("{0:d7}", (DateTime.Now.Ticks / 10) % 10000000) + ".txt");
            txWriter.WriteLine(strResponse);
            txWriter.WriteLine("last_name:" + HttpContext.Current.Request["last_name"]);
            txWriter.WriteLine("first_name:" + HttpContext.Current.Request["first_name"]);
          
            txWriter.WriteLine("payer_status:" + HttpContext.Current.Request["payer_status"]);
            txWriter.WriteLine("Invoice_type:" + HttpContext.Current.Request["Invoice_type"]);
            
            txWriter.WriteLine("payer_email:" + HttpContext.Current.Request["payer_email"]);
           
            txWriter.WriteLine("receiver_id:" + HttpContext.Current.Request["receiver_id"]);

            txWriter.WriteLine("txn_type:" + HttpContext.Current.Request["txn_type"]);
            txWriter.WriteLine("Invoice_gross:" + HttpContext.Current.Request["Invoice_gross"]);
            
            txWriter.WriteLine("custom:" + strInvoiceID );
            
            txWriter.WriteLine("txn_id:" + HttpContext.Current.Request["txn_id"]);
            txWriter.WriteLine("receiver_email:" + strreceiver_email);
            txWriter.WriteLine("resend:" + HttpContext.Current.Request["resend"]);
            txWriter.WriteLine("residence_country:" + HttpContext.Current.Request["residence_country"]);

            txWriter.WriteLine("mc_currency:" + HttpContext.Current.Request["mc_currency"]);

            txWriter.WriteLine("mc_gross:" + strmc_gross);

            txWriter.WriteLine("Invoice_status:" + strInvoice_status);
            txWriter.WriteLine("pending_reason:" + HttpContext.Current.Request["pending_reason"]);          

            txWriter.Close();

            //End Test


            //check the Invoice_status is Completed
            //check that txn_id has not been previously processed
            //check that receiver_email is your Primary PayPal email
            //check that Invoice_amount/Invoice_currency are correct
            //strInvoice_status.Trim().ToLower()=="completed" &&

            if ( strmc_gross.Trim()!="" 
                && strreceiver_email.Trim().ToLower()==SecurityManager.GetPaypalReceiverEmail().ToLower()
                && strInvoiceID.Trim()!="")
            {
               
                    DataTable dtPaypal=Common.DataTableFromText("SELECT * FROM Paypal WHERE txn_id='" + strtxn_id.Trim() + "'");

                    if (dtPaypal.Rows.Count>0)
                    {
                        //Oops.. we have this txn_id, so do nothing 
                    }
                    else
                    {
                        Invoice theInvoice=InvoiceManager.ets_Invoice_Detail(int.Parse(strInvoiceID.Trim()));

                       
                        //if (double.Parse(strmc_gross.Trim())==theInvoice.GrossAmountAUD)
                        //{
                            //evrything is ok

                    
                            try
                            {

                                //so lets build the paypal

                                Paypal thePaypal=new Paypal(null,strtxn_id,strInvoice_status,
                                    HttpContext.Current.Request["pending_reason"]==null?"":HttpContext.Current.Request["pending_reason"],
                                    HttpContext.Current.Request["payer_email"] == null ? "" : HttpContext.Current.Request["payer_email"],
                                    strreceiver_email,double.Parse(strmc_gross.Trim()),
                                    HttpContext.Current.Request["txn_type"] == null ? "" : HttpContext.Current.Request["txn_type"], null);

                                int iPayPalID=SecurityManager.PayPal_Insert(thePaypal);

                                //now update Invoice

                                theInvoice.PaypalID=iPayPalID;
                                //theInvoice.IsPaid=true;
                                theInvoice.PaidDate = DateTime.Now;

                                //theInvoice.AccountID = theInvoice.UserID;
                                InvoiceManager.ets_Invoice_Update(theInvoice);

                                //now update expiry date

                                Account theAccount=SecurityManager.Account_Details((int)theInvoice.AccountID);                                                              


                                theAccount.ExpiryDate = theInvoice.EndDate;
                                //theAccount.IsClient = true;
                                theAccount.NextBilledAccountTypeID = theAccount.AccountTypeID;
                                theAccount.AccountTypeID = theInvoice.AccountTypeID;
                                theAccount.SMSCount = 0;
                                theAccount.EmailCount = 0;

                                SecurityManager.Account_Update(theAccount);



                            }
                            catch (Exception ex)
                            {
                               
                                ErrorLog theErrorLog = new ErrorLog(null, "Paypal Error", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
                                SystemData.ErrorLog_Insert(theErrorLog);
                               

                            }

                            //Send email
                          

                        //}
                    }
                
            }



            //process Invoice
        }
        else if (strResponse == "INVALID")
        {
            //log for manual investigation
            TextWriter txWriter = new StreamWriter(Server.MapPath("Upload1/") +"invalid_" + string.Format("{0:d7}", (DateTime.Now.Ticks / 10) % 10000000) + ".txt");
            txWriter.WriteLine(strResponse);
            txWriter.Close();
        }
        else
        {
            //log response/ipn data for manual investigation

            TextWriter txWriter = new StreamWriter(Server.MapPath("Upload1/") + "unknown_" + string.Format("{0:d7}", (DateTime.Now.Ticks / 10) % 10000000) + ".txt");
            txWriter.WriteLine(strResponse);
            txWriter.Close();
        }
    }




    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    //Post back to either sandbox or live
    //    string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
    //    // string strLive = "https://www.paypal.com/cgi-bin/webscr";
    //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strSandbox);

    //    //Set values for the request back
    //    req.Method = "POST";
    //    req.ContentType = "application/x-www-form-urlencoded";
    //    byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
    //    string strRequest = Encoding.ASCII.GetString(param);
    //    strRequest += "&cmd=_notify-validate";
    //    req.ContentLength = strRequest.Length;

    //    //for proxy
    //    //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
    //    //req.Proxy = proxy;

    //    //Send the request to PayPal and get the response
    //    StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
    //    streamOut.Write(strRequest);
    //    streamOut.Close();
    //    StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
    //    string strResponse = streamIn.ReadToEnd();
    //    streamIn.Close();

    //    Session["Reponse"] = strResponse;
    //    //if (strResponse == "VERIFIED")
    //    //{
    //    //    //UPDATE YOUR DATABASE

    //    //    TextWriter txWriter = new StreamWriter(Server.MapPath("Upload/") + Session["orderID"].ToString() + ".txt");
    //    //    txWriter.WriteLine(strResponse);
    //    //    txWriter.Close();

    //    //    //check the Invoice_status is Completed
    //    //    //check that txn_id has not been previously processed
    //    //    //check that receiver_email is your Primary PayPal email
    //    //    //check that Invoice_amount/Invoice_currency are correct
    //    //    //process Invoice
    //    //}
    //    //else if (strResponse == "INVALID")
    //    //{
    //    //    //UPDATE YOUR DATABASE

    //    //    TextWriter txWriter = new StreamWriter(Server.MapPath("Upload/") + Session["orderID"].ToString() + ".txt");
    //    //    txWriter.WriteLine(strResponse);
    //    //    //log for manual investigation
    //    //    txWriter.Close();
    //    //}
    //    //else
    //    //{  //UPDATE YOUR DATABASE

    //    //    TextWriter txWriter = new StreamWriter(Server.MapPath("Upload/") + Session["orderID"].ToString() + ".txt");
    //    //    txWriter.WriteLine("Invalid");
    //    //    //log response/ipn data for manual investigation
    //    //    txWriter.Close();
    //    //}
    //}
}
