using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Site_GoogleMap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "ETS - Choose Location";

        try
        {
            if (!IsPostBack)
            {

                Content theContent = SystemData.Content_Details_ByKey("ChooseLocationHelp", null);
                if (theContent != null)
                {
                    lblContentCommon.Text = theContent.ContentP;
                }



                bool bHasValue = false;

                hfFlag.Value = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Images/Flag.png";


                Account theAccount = SecurityManager.Account_Details(int.Parse(Session["AccountID"].ToString()));

                if (theAccount != null)
                {
                    if (theAccount.MapCentreLat != null && theAccount.MapCentreLong != null)
                    {
                        hfCentreLat.Value = theAccount.MapCentreLat.ToString();
                        hfCentreLong.Value = theAccount.MapCentreLong.ToString();

                        hfLat.Value = theAccount.MapCentreLat.ToString();
                        hfLng.Value = theAccount.MapCentreLong.ToString();
                        bHasValue = true;
                    }
                    if (theAccount.OtherMapZoomLevel != null)
                    {
                        hfOtherZoomLevel.Value = theAccount.OtherMapZoomLevel.ToString();
                    }

                    if (Request.QueryString["zoom"] != null)
                    {
                        hfOtherZoomLevel.Value = Request.QueryString["zoom"].ToString();
                    }

                }


                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"].ToString() == "account")
                    {
                        hfType.Value = "account";
                        hfPath.Value = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Site/";
                    }
                    if (Request.QueryString["type"].ToString() == "mapsection")
                    {
                        hfType.Value = "mapsection";
                        hfPath.Value = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/Site/";
                    }

                }

                if (Request.QueryString["lat"] != null)
                {
                    if (Request.QueryString["lat"].ToString().Trim() != "")
                    {
                        hfLat.Value = Request.QueryString["lat"].ToString();
                        bHasValue = true;
                    }
                }

               // bHasValue = false;

                if (Request.QueryString["lng"] != null)
                {
                    if (Request.QueryString["lng"].ToString().Trim() != "")
                    {
                        hfLng.Value = Request.QueryString["lng"].ToString();
                        bHasValue = true;
                    }
                }

                if (bHasValue == false)
                {
                    //lets call the SP
                    //DataTable dtLatLng = SiteManager.ets_GetLatestLatLong(int.Parse(Session["AccountID"].ToString()));
                    //if (dtLatLng != null)
                    //{
                    //    if (dtLatLng.Rows.Count > 0)
                    //    {

                    //        hfLat.Value = dtLatLng.Rows[0][0].ToString();
                    //        hfLng.Value = dtLatLng.Rows[0][1].ToString();
                    //    }


                    //}

                }

            }
        }

        catch (Exception ex)
        {
            //
        }
    }

    
  
}