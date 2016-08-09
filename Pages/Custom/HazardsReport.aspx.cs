using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChartDirector;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;

public partial class Pages_Custom_HazardsReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = "Open / Closed Hazards";
        if (!IsPostBack)
        {
            txtDateFrom.Text = DateTime.Today.AddMonths(-1).ToShortDateString();
            txtDateTo.Text = DateTime.Today.ToShortDateString();
            lnkSearch_Click(null, null);
        }

    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        DateTime dtDateFrom = DateTime.Today.AddMonths(-1);
        DateTime dtDateTo = DateTime.Today;
        if (txtDateFrom.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtDateFrom.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                dtDateFrom = dtTemp;
            }
        }
        if (txtDateTo.Text != "")
        {
            DateTime dtTemp;
            if (DateTime.TryParseExact(txtDateTo.Text.Trim(), Common.Dateformats, new CultureInfo("en-GB"), DateTimeStyles.None, out dtTemp))
            {
                dtDateTo = dtTemp;
                dtDateTo = dtDateTo.AddHours(23).AddMinutes(59);
            }
        }



         XYChart c;
         c = new XYChart(600, 400, 0xffffff, 0x000000, 1);
         BarLayer layerBar = c.addBarLayer2(); //BAR 
         BarLayer layerBar2 = c.addBarLayer2(); //BAR 
         BarLayer layerBar3 = c.addBarLayer2(); //BAR 

         double[] dTempData = new double[5];
         double[] dTempData2 = new double[5];
         double[] dTempData3 = new double[5];

         double[] dTempDataX = new double[5];
         dTempDataX[0] = 0;
         dTempDataX[1] = 1;
         dTempDataX[2] = 2;
         dTempDataX[3] = 3;
         dTempDataX[4] = 4;

         dTempData[0] = Chart.NoValue;
        dTempData[1] = 8;
        dTempData[2] = Chart.NoValue;
        dTempData[3] = Chart.NoValue;
        dTempData[4] = Chart.NoValue;


        dTempData2[0] = Chart.NoValue;
        dTempData2[1] = Chart.NoValue;
        dTempData2[2] = 1;
        dTempData2[3] = Chart.NoValue;
        dTempData2[4] = Chart.NoValue;


        dTempData3[0] = Chart.NoValue;
        dTempData3[1] = Chart.NoValue;
        dTempData3[2] = Chart.NoValue;
        dTempData3[3] = 9;
        dTempData3[4] = Chart.NoValue;


        DataTable dtSPData= rrp_OpenClosedHazards(dtDateFrom, dtDateTo);

        if (dtSPData != null)
        {
            if (dtSPData.Rows.Count > 2)
            {
                if(dtSPData.Rows[0][1]!=DBNull.Value)
                {
                    dTempData[1] = int.Parse(dtSPData.Rows[0][1].ToString());
                }
                if (dtSPData.Rows[1][1] != DBNull.Value)
                {
                    dTempData2[2] = int.Parse(dtSPData.Rows[1][1].ToString());
                }
                if (dtSPData.Rows[2][1] != DBNull.Value)
                {
                    dTempData3[3] = int.Parse(dtSPData.Rows[2][1].ToString());
                }
            }
        }


        int iColor = -1;
        layerBar.addDataSet(dTempData, GetIntColorFromName("Purple"), "Added").setDataSymbol(Chart.CircleSymbol, 9);//BAR
        layerBar.setXData(dTempDataX);


        layerBar2.addDataSet(dTempData2, GetIntColorFromName("Gray"), "Closed").setDataSymbol(Chart.CircleSymbol, 9);//BAR
        layerBar2.setXData(dTempDataX);

        layerBar3.addDataSet(dTempData3, GetIntColorFromName("Blue"), "Total").setDataSymbol(Chart.CircleSymbol, 9);//BAR
        layerBar3.setXData(dTempDataX);

        string[] strLabel = new string[5];
        c.xAxis().setLabels(strLabel);


        c.setPlotArea(50, 80, 480, 270, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);


        c.addLegend2(540, 160, 1, "Verdana Bold", 7).setBackground(Chart.Transparent);

        c.addTitle(Chart.TopCenter, "Open and Closed Hazards", "Verdana Bold", 17
                 );

        string strSecondTitle = "";

        
            strSecondTitle = dtDateFrom.ToLongDateString() + " to "
                + dtDateTo.ToLongDateString();

            if (txtDateFrom.Text != "" && txtDateTo.Text != "")
            {
                try
                {

                }
                catch
                {
                    //
                }
            }

            c.addText(160, 30, strSecondTitle, "Verdana Bold", 8);

        c.setDropShadow();
        c.setRoundedFrame();

        WebChartViewer1.Image = c.makeWebImage(Chart.PNG);
    }


    public static DataTable rrp_OpenClosedHazards(DateTime dFrom, DateTime dTo)
    {
        using (SqlConnection connection = new SqlConnection(DBGurus.strGlobalConnectionString))
        {
            using (SqlCommand command = new SqlCommand("rrp_OpenClosedHazards", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (dFrom != null)
                    command.Parameters.Add(new SqlParameter("@dFrom", dFrom));
                if (dTo != null)
                    command.Parameters.Add(new SqlParameter("@dTo", dTo));

                
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

                
                if (ds == null) return null;


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
    public static int GetIntColorFromName(string strColorName)
    {
        try
        {
            switch (strColorName)
            {
                case "Aqua":
                    return 0x00FFFF;
                case "Black":
                    return 0x000000;
                case "Blue":
                    return 0x0000FF;
                case "Fuchsia":
                    return 0xFF00FF;
                case "Gray":
                    return 0x808080;
                case "Green":
                    return 0x008000;
                case "Lime":
                    return 0x00FF00;
                case "Maroon":
                    return 0x800000;
                case "Navy":
                    return 0x000080;
                case "Olive":
                    return 0x808000;
                case "Orange":
                    return 0xFFA500;
                case "Purple":
                    return 0x800080;
                case "Red":
                    return 0xFF0000;
                case "Silver":
                    return 0xC0C0C0;
                case "Teal":
                    return 0x008080;
                case "Yellow":
                    return 0xFFFF00;
                default:
                    return int.Parse(strColorName.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);

            }
        }
        catch (Exception ex)
        {
            return -1;
        }


    }

}