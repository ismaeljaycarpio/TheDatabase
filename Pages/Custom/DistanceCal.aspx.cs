using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;


public partial class Pages_Custom_DistanceCal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
//            string strSearchAddressJS = @"                                                   
//                        function showAddress() {                                                      
//
//                        var address = document.getElementById('" + txtAddress.ClientID.ToString() + @"').value;   
//                        // var lblLat = document.getElementById('" + lblLat.ClientID.ToString() + @"');  
//                        //  var lblLong = document.getElementById('" + lblLong.ClientID.ToString() + @"');  
//                        var geocoder = new google.maps.Geocoder();
//                        geocoder.geocode({ 'address': address }, function (results, status) {
//                            if (status == google.maps.GeocoderStatus.OK) {
//                                results[0].geometry.location;
//                                //var b = new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng());
//                                $('#" + lblLat.ClientID.ToString() + @"').text(results[0].geometry.location.lat().toString());
//                                $('#" + lblLong.ClientID.ToString() + @"').text(results[0].geometry.location.lng().toString());
//                                //alert(results[0].geometry.location.lat().toString());
//
//                            } else {
//                                alert('Google Maps had some trouble finding ' + address + '.');
//                            }
//                        });
//                    };  
//
//                    ";

//            ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchAddressJS", strSearchAddressJS, true);

        }

    }

    protected void lnkGetClosestDistance_OnClick(object sender, EventArgs e)
    {
        lblDistance.Text = "";
        lblPollingRecordID.Text = "";
        lblLatLong.Text = "";
        lblVolLatLong.Text = "";

        try
        {
            if (txtVolunteerRecordID.Text != "")
            {
                DataTable dtTemp = Common.DataTableFromText(@"SELECT RecordID,V007,V008,V009,V010 FROM [Record] 
                WHERE IsActive=1 AND TableID=2101
                AND RecordID=" + txtVolunteerRecordID.Text);


                if (dtTemp.Rows.Count > 0)
                {

                    string strVolunteerAdddress = "";
                    if (dtTemp.Rows[0]["V007"] != DBNull.Value)
                        strVolunteerAdddress = dtTemp.Rows[0]["V007"].ToString();

                    if (dtTemp.Rows[0]["V008"] != DBNull.Value)
                        strVolunteerAdddress = strVolunteerAdddress + " " + dtTemp.Rows[0]["V008"].ToString();

                    if (dtTemp.Rows[0]["V009"] != DBNull.Value)
                        strVolunteerAdddress = strVolunteerAdddress + " " + dtTemp.Rows[0]["V009"].ToString();

                    if (dtTemp.Rows[0]["V010"] != DBNull.Value)
                        strVolunteerAdddress = strVolunteerAdddress + " " + dtTemp.Rows[0]["V010"].ToString();

                    if (strVolunteerAdddress != "")
                    {
                        XmlDocument theDoc = new XmlDocument();
                        theDoc.Load("http://maps.googleapis.com/maps/api/geocode/xml?address=" + strVolunteerAdddress + "&sensor=true");

                        if (theDoc != null)
                        {
                            XmlNodeList xmlLocation = theDoc.GetElementsByTagName("location");
                            if (xmlLocation != null)
                            {
                                string strVolLat = "";
                                string strVolLong = "";

                                strVolLat = xmlLocation[0].ChildNodes[0].InnerText;
                                strVolLong = xmlLocation[0].ChildNodes[1].InnerText;

                                if (strVolLat != "" && strVolLong != "")
                                {
                                    lblVolLatLong.Text = strVolLat + "," + strVolLong;

                                    DataTable dtPoll = Common.DataTableFromText(@"SELECT RecordID,V014,V015 FROM [Record] WHERE IsActive=1
                    AND (V014 IS NOT NULL OR V015 IS NOT NULL) AND TableID=2100");

                                    if (dtPoll.Rows.Count > 0)
                                    {
                                        string strPollLat = "";
                                        string strPollLong = "";
                                        List<double> lstDistance = new List<double>();
                                        List<string> lstPollRecord = new List<string>();
                                        List<string> lstPollLatLong = new List<string>();

                                        foreach (DataRow dr in dtPoll.Rows)
                                        {
                                            strPollLat = dr["V014"].ToString();
                                            strPollLong = dr["V015"].ToString();
                                            if (strPollLat != "" && strPollLong != "")
                                            {
                                                double dDistacne = Math.Sqrt(Math.Pow((double.Parse(strVolLat) - double.Parse(strPollLat)), 2)
                                                    + Math.Pow((double.Parse(strVolLong) - double.Parse(strPollLong)), 2));

                                                lstDistance.Add(dDistacne);
                                                lstPollRecord.Add(dr["RecordID"].ToString());
                                                lstPollLatLong.Add("[" + dr["V014"].ToString() + "," + dr["V015"].ToString() + "]");
                                            }

                                        }


                                        //get 3 MIN 

                                        double dMin1 = lstDistance.Min<double>();
                                        int iIndex1 = lstDistance.FindIndex(x => x == dMin1);

                                        lstDistance.RemoveAt(iIndex1);
                                        string strPollRecordID1 = lstPollRecord[iIndex1];
                                        lstPollRecord.RemoveAt(iIndex1);

                                        string strPollLatLong1 = lstPollLatLong[iIndex1];
                                        lstPollLatLong.RemoveAt(iIndex1);

                                        //2nd

                                        double dMin2 = lstDistance.Min<double>();
                                        int iIndex2 = lstDistance.FindIndex(x => x == dMin2);

                                        lstDistance.RemoveAt(iIndex2);
                                        string strPollRecordID2 = lstPollRecord[iIndex2];
                                        lstPollRecord.RemoveAt(iIndex2);

                                        string strPollLatLong2 = lstPollLatLong[iIndex2];
                                        lstPollLatLong.RemoveAt(iIndex2);


                                        //3rd

                                        double dMin3 = lstDistance.Min<double>();
                                        int iIndex3 = lstDistance.FindIndex(x => x == dMin3);

                                        lstDistance.RemoveAt(iIndex3);
                                        string strPollRecordID3 = lstPollRecord[iIndex3];
                                        lstPollRecord.RemoveAt(iIndex3);

                                        string strPollLatLong3 = lstPollLatLong[iIndex3];
                                        lstPollLatLong.RemoveAt(iIndex3);


                                        lblDistance.Text = dMin1.ToString() + "," + dMin2.ToString() + "," + dMin3.ToString();
                                        lblPollingRecordID.Text = strPollRecordID1 + "," + strPollRecordID2 + "," + strPollRecordID3;
                                        lblLatLong.Text = strPollLatLong3 + "," + strPollLatLong3 + "," + strPollLatLong3;


                                    }



                                }


                            }
                        }
                    }

                }

            }
        }
        catch
        {
            //
        }

    }

    protected void lnkGetAddress_OnClick(object sender, EventArgs e)
    {
        //testing

        lblLat.Text = "";
        lblLong.Text = "";

        try
        {
            XmlDocument theDoc = new XmlDocument();
            theDoc.Load("http://maps.googleapis.com/maps/api/geocode/xml?address=" + txtAddress.Text + "&sensor=true");

            if (theDoc != null)
            {
                XmlNodeList xmlLocation = theDoc.GetElementsByTagName("location");
                if (xmlLocation != null)
                {
                    lblLat.Text = xmlLocation[0].ChildNodes[0].InnerText;
                    lblLong.Text = xmlLocation[0].ChildNodes[1].InnerText;

                }
            }
        }
        catch
        {
            //
        }
        
    }
}