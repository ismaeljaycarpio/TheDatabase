using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;



public partial class Pages_Custom_UpdateVol : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void lnkUpdateVolLat_OnClick(object sender, EventArgs e)
    {
       
//        DataTable dtAllVol = Common.DataTableFromText(@"SELECT RecordID,V007,V008,V009,V010,V023,V024 FROM [Record]
//                    WHERE IsActive=1 AND TableID=2101 AND (V013 IS NULL OR V014 IS  NULL)");



        DataTable dtAllVol = Common.DataTableFromText(@"SELECT RecordID,V007,V008,V009,V010,V023,V024 FROM [Record]
                    WHERE IsActive=1 AND TableID=2101 AND (V013 IS NULL OR V014 IS  NULL)  and RecordID > 754493");

        if (dtAllVol.Rows.Count > 0)
        {
            foreach (DataRow drV in dtAllVol.Rows)
            {
              

                string strVolunteerAdddress = "";
                if (drV["V007"] != DBNull.Value)
                    strVolunteerAdddress = drV["V007"].ToString();

                if (drV["V008"] != DBNull.Value)
                    strVolunteerAdddress = strVolunteerAdddress + " " + drV["V008"].ToString();

                if (drV["V009"] != DBNull.Value)
                    strVolunteerAdddress = strVolunteerAdddress + " " + drV["V009"].ToString();

                if (drV["V010"] != DBNull.Value)
                    strVolunteerAdddress = strVolunteerAdddress + " " + drV["V010"].ToString();

                if (drV["V023"] != DBNull.Value)
                    strVolunteerAdddress = strVolunteerAdddress + " " + drV["V023"].ToString();

                if (drV["V024"] != DBNull.Value)
                    strVolunteerAdddress = strVolunteerAdddress + " " + drV["V024"].ToString();

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

                            if (xmlLocation[0] != null)
                            {
                                strVolLat = xmlLocation[0].ChildNodes[0].InnerText;

                                strVolLong = xmlLocation[0].ChildNodes[1].InnerText;
                            }

                            if (strVolLat != "" && strVolLong != "")
                            {                                                             

                               Common.ExecuteText("UPDATE [Record] SET V013='" + strVolLat + "',V014='" + strVolLong +  "' WHERE RecordID=" + drV["RecordID"].ToString());
                                
                            }


                        }
                    }
                }



            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "SuccessInfo", "alert('Successfully updated all volunteer lat & long!');", true);

        }



    }


    protected void lnkAutoLevelling_OnClick(object sender, EventArgs e)
    {

//        DataTable dtPoll = Common.DataTableFromText(@"SELECT DISTINCT P.RecordID FROM Record P INNER JOIN Record V ON 
//(P.RecordID=V.V016 OR P.RecordID=V.V017 OR P.RecordID=V.V018)
//WHERE P.TableID=2100 AND V.TableID=2101 ");

//        foreach (DataRow drPoll in dtPoll.Rows)
//        {
//            DataTable dtVol = Common.DataTableFromText(@" SELECT CONVERT(decimal(20,10),V020)+ CONVERT(decimal(20,10),V021)
//                         + CONVERT(decimal(20,10),V022) AS TotalTime,RecordID,V013,V014
//                         FROM Record 
//                         WHERE TableID=2101 AND 
//                         ( Record.V016='" + drPoll["RecordID"].ToString() + @"'  OR  Record.V017='" + drPoll["RecordID"].ToString()
//                                          + @"'  OR  Record.V018='" + drPoll["RecordID"].ToString() + @"' )
//                         ORDER BY TotalTime");


//            if (dtVol.Rows.Count > 3)
//            {
//                for (int i = 3; i < dtVol.Rows.Count-1; i++)
//                {
//                    Common.ExecuteText("UPDATE [Record] SET V016=null,V017=null,V018=null,V020=null,V021=null,V022=null WHERE RecordID=" + dtVol.Rows[i]["RecordID"].ToString());
//                }


//            }

//        }



        //lets start from the beginning

        DataTable dtAllVol = Common.DataTableFromText(@" SELECT RecordID,V013,V014,V037 FROM [Record]
WHERE IsActive=1 AND TableID=2101 AND V013 IS NOT NULL 
AND V014 IS NOT NULL AND (V016 IS NULL OR V017 IS NULL OR V018 IS NULL)
ORDER BY V037 DESC");


        foreach (DataRow drV in dtAllVol.Rows)
        {
            string strVolLat = drV["V013"].ToString();
            string strVolLong = drV["V014"].ToString();

//            DataTable dtEmptyPoll = Common.DataTableFromText(@"SELECT RecordID,V014,V015 FROM Record WHERE TableID=2100 AND RecordID NOT IN
//(SELECT V016 FROM Record WHERE TableID=2101 AND V016 IS NOT NULL)
//AND RecordID NOT IN
//(SELECT V017 FROM Record WHERE TableID=2101 AND V017 IS NOT NULL)
//AND RecordID NOT IN
//(SELECT V018 FROM Record WHERE TableID=2101 AND V018 IS NOT NULL)");




            DataTable dtEmptyPoll = Common.DataTableFromText(@"SELECT * FROM (SELECT P.RecordID,P.V014,P.V015, (SELECT COUNT(*) FROM Record C WHERE C.TableID=2101 AND 
 ( C.V016=P.RecordID  OR  C.V017=P.RecordID  OR  C.V018=P.RecordID )) AS TotalVol
 FROM Record P WHERE P.TableID=2100) AS MainQ WHERE  TotalVol<3");

            if (dtEmptyPoll.Rows.Count > 0)
            {
                string strPollLat = "";
                string strPollLong = "";
                List<double> lstDistance = new List<double>();
                List<string> lstPollRecord = new List<string>();
                List<string> lstPollLatLong = new List<string>();

                            foreach (DataRow dr in dtEmptyPoll.Rows)
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
                            try
                            {
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


                                Common.ExecuteText("UPDATE [Record] SET V016='" + strPollRecordID1 + "',V017='" + strPollRecordID2 + "',V018='" + strPollRecordID3 + "' WHERE RecordID=" + drV["RecordID"].ToString());
                            }
                            catch
                            {
                                //
                            }

            }



        }


    }


    protected void lnkUpdateVolForTime_OnClick(object sender, EventArgs e)
    {
       

        //get time 2

        try
        {


           


            //return;



            //get time 1

            DataTable dtTime1 = Common.DataTableFromText(@"  SELECT  V.RecordID, V.V013, V.V014, P.V014, P.V015                
                FROM [Record] V
                JOIN [Record] P on P.TableID = 2100 and P.IsActive = 1 and ISNUMERIC(P.V014)=1 and ISNUMERIC(P.V015)=1
                WHERE V.TableID = 2101 AND v.IsActive = 1 and ISNUMERIC(V.V013)=1 and ISNUMERIC(V.V014)=1
                AND P.RecordID=V.V016");


            foreach (DataRow dr in dtTime1.Rows)
            {

                XmlDocument theDoc = new XmlDocument();
                theDoc.Load("https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + dr[1].ToString() + "," + dr[2].ToString() + "&destinations=" + dr[3].ToString() + "," + dr[4].ToString() + "");

                if (theDoc != null)
                {
                    XmlNodeList xmlduration = theDoc.GetElementsByTagName("duration");
                    if (xmlduration != null)
                    {
                        if (xmlduration[0] != null)
                        {
                            string strTime = xmlduration[0].ChildNodes[0].InnerText;
                            if (strTime != "")
                            {
                                Common.ExecuteText("UPDATE [Record] SET V020='" + strTime + "' WHERE RecordID=" + dr[0].ToString());
                            }
                        }
                    }

                }

            }

            //TIME 2

            DataTable dtTime2 = Common.DataTableFromText(@"  SELECT  V.RecordID, V.V013, V.V014, P.V014, P.V015                
                FROM [Record] V
                JOIN [Record] P on P.TableID = 2100 and P.IsActive = 1 and ISNUMERIC(P.V014)=1 and ISNUMERIC(P.V015)=1
                WHERE V.TableID = 2101 AND v.IsActive = 1 and ISNUMERIC(V.V013)=1 and ISNUMERIC(V.V014)=1
                AND P.RecordID=V.V017");


            foreach (DataRow dr in dtTime2.Rows)
            {

                XmlDocument theDoc = new XmlDocument();
                theDoc.Load("https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + dr[1].ToString() + "," + dr[2].ToString() + "&destinations=" + dr[3].ToString() + "," + dr[4].ToString() + "");

                if (theDoc != null)
                {
                    XmlNodeList xmlduration = theDoc.GetElementsByTagName("duration");
                    if (xmlduration != null)
                    {
                        if (xmlduration[0] != null)
                        {
                            string strTime = xmlduration[0].ChildNodes[0].InnerText;
                            if (strTime != "")
                            {
                                Common.ExecuteText("UPDATE [Record] SET V021='" + strTime + "' WHERE RecordID=" + dr[0].ToString());
                                lblMSG.Text = lblMSG.Text + "UPDATE [Record] SET V021='" + strTime + "' WHERE RecordID=" + dr[0].ToString() + "<br/>";
                            }
                            else
                            {
                                lblMSG.Text = lblMSG.Text + "strTime is  emty";
                            }
                        }
                        else
                        {
                            //lblMSG.Text = lblMSG.Text + "xmlduration[0] is  null";

                            lblMSG.Text = lblMSG.Text + "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + dr[1].ToString() + "," + dr[2].ToString() + "&destinations=" + dr[3].ToString() + "," + dr[4].ToString() + "" + "<br/>";


                        }
                    }
                    else
                    {
                        lblMSG.Text = lblMSG.Text + "xmlduration is  null";
                    }

                }
                else
                {
                    lblMSG.Text = lblMSG.Text + " " + dr[1].ToString() + "," + dr[2].ToString() + "&destinations=" + dr[3].ToString() + "," + dr[4].ToString() + "" + "<br/>";
                }

            }

            //return;


            //get time 3

            DataTable dtTime3 = Common.DataTableFromText(@"  SELECT  V.RecordID, V.V013, V.V014, P.V014, P.V015                
                FROM [Record] V
                JOIN [Record] P on P.TableID = 2100 and P.IsActive = 1 and ISNUMERIC(P.V014)=1 and ISNUMERIC(P.V015)=1
                WHERE V.TableID = 2101 AND v.IsActive = 1 and ISNUMERIC(V.V013)=1 and ISNUMERIC(V.V014)=1
                AND P.RecordID=V.V018");


            foreach (DataRow dr in dtTime3.Rows)
            {

                XmlDocument theDoc = new XmlDocument();
                theDoc.Load("https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + dr[1].ToString() + "," + dr[2].ToString() + "&destinations=" + dr[3].ToString() + "," + dr[4].ToString() + "");

                if (theDoc != null)
                {
                    XmlNodeList xmlduration = theDoc.GetElementsByTagName("duration");
                    if (xmlduration != null)
                    {
                        if (xmlduration[0] != null)
                        {
                            string strTime = xmlduration[0].ChildNodes[0].InnerText;
                            if (strTime != "")
                            {
                                Common.ExecuteText("UPDATE [Record] SET V022='" + strTime + "' WHERE RecordID=" + dr[0].ToString());
                            }
                        }
                    }

                }

            }
            
            


        }
        catch(Exception  ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "TheDB- Update vol", ex.Message, ex.StackTrace, DateTime.Now, HttpContext.Current.Request.Url.ToString());
            int iErrroLogID = SystemData.ErrorLog_Insert(theErrorLog);
                                  
        }


    }

      protected void lnkUpdateVol_OnClick(object sender, EventArgs e)
    {
       
        DataTable dtAllVol = Common.DataTableFromText(@"SELECT RecordID FROM [Record]
                    WHERE IsActive=1 AND TableID=2101  AND V013 IS NOT NULL AND V014 IS NOT NULL");

        if (dtAllVol.Rows.Count > 0)
        {
            foreach (DataRow drV in dtAllVol.Rows)
            {

                try
                {

                    DataTable dtPoll = Common.DataTableFromText(@"SELECT TOP 3  P.RecordID, V.V007, V.V008, P.V007, P.V008, P.V009, P.V010, P.V011,
                SQRT(SQUARE(ABS(CAST(V.V013 as real)- CAST(P.V014 as real))) + SQUARE(ABS(CAST(V.V014 as real) - CAST(P.V015 as real))))
                FROM [Record] V
                JOIN [Record] P on P.TableID = 2100 and P.IsActive = 1 and ISNUMERIC(P.V014)=1 and ISNUMERIC(P.V015)=1
                WHERE V.TableID = 2101 AND v.IsActive = 1 and ISNUMERIC(V.V013)=1 and ISNUMERIC(V.V014)=1
                AND V.RecordID = " + drV["RecordID"].ToString() + @" 
                ORDER BY SQRT(SQUARE(ABS(CAST(V.V013 as real)- CAST(P.V014 as real))) + SQUARE(ABS(CAST(V.V014 as real) - CAST(P.V015 as real)))) asc ");

                    if (dtPoll.Rows.Count == 3)
                    {

                        string strPollRecordID1 = dtPoll.Rows[0][0].ToString();
                        string strPollRecordID2 = dtPoll.Rows[1][0].ToString();
                        string strPollRecordID3 = dtPoll.Rows[2][0].ToString();


                        Common.ExecuteText("UPDATE [Record] SET V016='" + strPollRecordID1 + "',V017='" + strPollRecordID2 + "',V018='" + strPollRecordID3 + "' WHERE RecordID=" + drV["RecordID"].ToString());
                    }
                }
                catch
                {
                    //
                }

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "SuccessInfo", "alert('Successfully updated all volunteer info!');", true);

        }


        
    }


//    protected void lnkUpdateVol_OnClick(object sender, EventArgs e)
//    {
       
//        DataTable dtAllVol = Common.DataTableFromText(@"SELECT RecordID,V013,V014 FROM [Record]
//                    WHERE IsActive=1 AND TableID=2101");

//IS NULL OR CONVERT(decimal(20,10),V019)<3) AND TableID=2100");

//        if (dtAllVol.Rows.Count > 0)
//        {
//            foreach (DataRow drV in dtAllVol.Rows)
//            {
                
                    

//                    if (strVolunteerAdddress != "")
//                    {
                        
//                        if (theDoc != null)
//                        {
//                            XmlNodeList xmlLocation = theDoc.GetElementsByTagName("location");
//                            if (xmlLocation != null)
//                            {
//                                string strVolLat = "";
//                                string strVolLong = "";

//                                if (xmlLocation[0] != null)
//                                {
//                                    strVolLat = xmlLocation[0].ChildNodes[0].InnerText;

//                                    strVolLong = xmlLocation[0].ChildNodes[1].InnerText;
//                                }

//                                if (strVolLat != "" && strVolLong != "")
//                                {
//                                    //lblVolLatLong.Text = strVolLat + "," + strVolLong;


//                                    DataTable dtPoll = Common.DataTableFromText(@"SELECT RecordID,V014,V015,V019 FROM [Record] WHERE IsActive=1
//                    AND (V014 IS NOT NULL OR V015 IS NOT NULL)
//                    AND (V019 IS NULL OR CONVERT(decimal(20,10),V019)<3) AND TableID=2100");

                                   

//                                    if (dtPoll.Rows.Count > 0)
//                                    {
//                                        string strPollLat = "";
//                                        string strPollLong = "";
//                                        List<double> lstDistance = new List<double>();
//                                        List<string> lstPollRecord = new List<string>();
//                                        List<string> lstPollLatLong = new List<string>();

//                                        foreach (DataRow dr in dtPoll.Rows)
//                                        {
//                                            strPollLat = dr["V014"].ToString();
//                                            strPollLong = dr["V015"].ToString();
//                                            if (strPollLat != "" && strPollLong != "")
//                                            {
//                                                double dDistacne = Math.Sqrt(Math.Pow((double.Parse(strVolLat) - double.Parse(strPollLat)), 2)
//                                                    + Math.Pow((double.Parse(strVolLong) - double.Parse(strPollLong)), 2));

//                                                lstDistance.Add(dDistacne);
//                                                lstPollRecord.Add(dr["RecordID"].ToString());
//                                                lstPollLatLong.Add("[" + dr["V014"].ToString() + "," + dr["V015"].ToString() + "]");
//                                            }

//                                        }


//                                        //get 3 MIN 

//                                        double dMin1 = lstDistance.Min<double>();
//                                        int iIndex1 = lstDistance.FindIndex(x => x == dMin1);

//                                        lstDistance.RemoveAt(iIndex1);
//                                        string strPollRecordID1 = lstPollRecord[iIndex1];
//                                        lstPollRecord.RemoveAt(iIndex1);

//                                        string strPollLatLong1 = lstPollLatLong[iIndex1];
//                                        lstPollLatLong.RemoveAt(iIndex1);

//                                        //2nd

//                                        double dMin2 = lstDistance.Min<double>();
//                                        int iIndex2 = lstDistance.FindIndex(x => x == dMin2);

//                                        lstDistance.RemoveAt(iIndex2);
//                                        string strPollRecordID2 = lstPollRecord[iIndex2];
//                                        lstPollRecord.RemoveAt(iIndex2);

//                                        string strPollLatLong2 = lstPollLatLong[iIndex2];
//                                        lstPollLatLong.RemoveAt(iIndex2);


//                                        //3rd

//                                        double dMin3 = lstDistance.Min<double>();
//                                        int iIndex3 = lstDistance.FindIndex(x => x == dMin3);

//                                        lstDistance.RemoveAt(iIndex3);
//                                        string strPollRecordID3 = lstPollRecord[iIndex3];
//                                        lstPollRecord.RemoveAt(iIndex3);

//                                        string strPollLatLong3 = lstPollLatLong[iIndex3];
//                                        lstPollLatLong.RemoveAt(iIndex3);


//                                        //lblDistance.Text = dMin1.ToString() + "," + dMin2.ToString() + "," + dMin3.ToString();
//                                        //lblPollingRecordID.Text = strPollRecordID1 + "," + strPollRecordID2 + "," + strPollRecordID3;
//                                        //lblLatLong.Text = strPollLatLong3 + "," + strPollLatLong3 + "," + strPollLatLong3;

//                                        Common.ExecuteText("UPDATE [Record] SET V013='" + strVolLat + "',V014='" + strVolLong + "',V016='" + strPollRecordID1 + "',V017='" + strPollRecordID2 + "',V018='" + strPollRecordID3 + "' WHERE RecordID=" + drV["RecordID"].ToString());
//                                    }



//                                }


//                            }
//                        }
//                    }


                
//            }

//            ScriptManager.RegisterStartupScript(this, this.GetType(), "SuccessInfo", "alert('Successfully updated all volunteer information!');", true);

//        }


        
//    }
}