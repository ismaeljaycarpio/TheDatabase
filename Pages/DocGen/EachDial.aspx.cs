using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocGen.DAL;
using ChartDirector;
public partial class Pages_DocGen_EachDial : System.Web.UI.Page
{
    int _DocumentSectionID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out _DocumentSectionID);
            PopulateCommonDialGraph();
        }
        catch
        {
            //
        }


    }
    protected void PopulateCommonDialGraph()
    {
        //First Chart
        using (DocGen.DAL.DocGenDataContext ctx = new DocGen.DAL.DocGenDataContext())
        {

            DocGen.DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DocGen.DAL.DocumentSection>(s => s.DocumentSectionID == _DocumentSectionID);
            if (section != null)
            {
                 DialSectionDetail dialSecDetail = JSONField.GetTypedObject<DialSectionDetail>(section.Details);
                 if (dialSecDetail != null)
                 {
                     if (dialSecDetail.TableID != null && dialSecDetail.ColumnID != null)
                     {

                         Column theColumn = RecordManager.ets_Column_Details((int)dialSecDetail.ColumnID);

                         string strValue = Common.GetValueFromSQL(@"SELECT " + theColumn.SystemName + @" FROM Record WHERE IsActive=1 AND TableID=" + dialSecDetail.TableID.ToString() + @" AND DateTimeRecorded=
            ( SELECT MAX(DateTimeRecorded) FROM Record WHERE IsActive=1 AND TableID=" + dialSecDetail.TableID.ToString() + ")");

                         if (strValue == "")
                             strValue = "0";

                         double value = double.Parse(strValue);

                         int iHeight = 180;//115
                         int iWidth = 200;//200


                         if (dialSecDetail.Height != null)
                             iHeight = (int)dialSecDetail.Height;
                         if (dialSecDetail.Width != null)
                             iWidth = (int)dialSecDetail.Width;

                         if (dialSecDetail.Heading == "")
                         {
                             trHeading.Visible = false;
                         }
                         else
                         {
                             lblHeading.Text = dialSecDetail.Heading;
                         }

                         int iBGColor = 0xFFFFFF;
                         double dAngle = 45; //135

                         int iRadius = iWidth*3/5;//120
                         int iCY = iHeight *5/6;//150
                         int iPointerEdgeColor = 0x000000;
                         int iLegendBackBround = 0x000000;
                         int iFontColor = 0x000000;
                         int iMeterColor = 0x000000;

                         bool bRound = false;

                         if (dialSecDetail.Dial != "")
                         {
                             switch (dialSecDetail.Dial)
                             {
                                 case "RoundWhite":
                                     bRound = true;
                                     //iHeight = 180;
                                     iRadius = iWidth*85/200;
                                     dAngle = 135;
                                     iCY = iHeight*5/9;//100
                                     break;
                                 case "RoundBlack":
                                     bRound = true;
                                     //iHeight = 180;
                                     iBGColor = 0x000000;
                                     iRadius = iWidth * 85 / 200;
                                     dAngle = 135;
                                     iCY = iHeight*5/9;//100
                                     iPointerEdgeColor = 0xffffff;
                                     iLegendBackBround = 0xFF0000;
                                     iFontColor = 0xFFFFFF;
                                     iMeterColor = 0xFFFFFF;
                                     break;
                                 case "HorizintalYellow":
                                     iBGColor = 0xFFFBCE;

                                     break;
                                 case "HorizinalWhite":

                                     //do nothing
                                     break;
                             }


                         }

                         AngularMeter m = new AngularMeter(iWidth, iHeight, iBGColor, 0x000000);
                         m.setRoundedFrame();

                         m.setMeter(iWidth/2, iCY, iRadius, -dAngle, dAngle);
                         m.setMeterColors(iMeterColor, iMeterColor);

                         double dMax = 10;
                         double mTic = 1;
                         double dMulti = 1;




                         if (theColumn.MaxValueAt != null)
                         {
                             dMax = (double)theColumn.MaxValueAt;
                             if (dialSecDetail.Scale == null || (double)dialSecDetail.Scale == 0)
                             {
                                 mTic = (double)theColumn.MaxValueAt / 10;
                             }
                             else
                             {
                                 mTic = (double)dialSecDetail.Scale;
                             }


                             //value = value / dMulti;
                         }
                         else
                         {
                             string strMaxValueAt = Common.GetValueFromSQL(@"SELECT MAX(CONVERT(decimal(20,10)," + theColumn.SystemName + @")) FROM Record WHERE IsActive=1 AND TableID=" + dialSecDetail.TableID.ToString());

                             if (strMaxValueAt != "")
                             {
                                 dMax = double.Parse(strMaxValueAt);
                                 if (dialSecDetail.Scale == null || (double)dialSecDetail.Scale == 0)
                                 {
                                     mTic = dMax / 10;
                                 }
                                 else
                                 {
                                     mTic = (double)dialSecDetail.Scale;
                                 }

                             }
                         }

                         m.setScale(0, dMax, Math.Round(mTic, 0), mTic / 2, mTic / 10);




                         double dWaring = 6;
                         if (theColumn.ShowGraphWarning != null)
                             dWaring = (double)theColumn.ShowGraphWarning / dMulti;

                         double dExceed = 8;

                         if (theColumn.ShowGraphExceedance != null)
                             dExceed = (double)theColumn.ShowGraphExceedance / dMulti;


                         m.addZone(0, dWaring, 0x99ff99, 0x808080);

                         m.addZone(dWaring, dExceed, 0xffff00, 0x808080);

                         m.addZone(dExceed, dMax, 0xff3333, 0x808080);


                         string strColumnName = "";

                         if (theColumn.DisplayTextSummary != "")
                         {
                             if (theColumn.DisplayTextSummary.Length > 6)
                             {
                                 strColumnName = theColumn.DisplayTextSummary.Substring(0, 5);
                             }
                             else
                             {
                                 strColumnName = theColumn.DisplayTextSummary;
                             }

                         }

                         int iX = iWidth*4/10;//85

                         if (dialSecDetail.Label != "")
                             strColumnName = dialSecDetail.Label;

                         //if (dMulti != 1)
                         //{
                         //    strColumnName = strColumnName + "(x" + Math.Round(dMulti, 2).ToString() + ")";
                         //    iX = 55;
                         //}

                         m.addText(iX, iWidth*7/20, strColumnName, "Verdana bold", 10, iFontColor);

                         m.addText(iWidth*3/4, 8, m.formatValue(value * dMulti, "2"), "Verdana", 8, 0xffffff).setBackground(
                     iLegendBackBround, 0, -1);

                         m.addPointer(value, 0x40666699, iPointerEdgeColor);

                         m.setDropShadow();

                        
                        wcFirst.Image = m.makeWebImage(Chart.PNG);
                        wcFirst.ImageMap = m.getHTMLImageMap("", "",
                        "title='[{dataSetName}]:  {value}'");
                        





                     }

                 }


            }

        }


               

    }


}