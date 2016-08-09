using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test_Test2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
   


    }

    protected void cldDate_DayRender(object sender, DayRenderEventArgs e)
    {
       
        e.Cell.Controls.Clear();
        if (e.Day.Date == DateTime.Today)
        {
            e.Cell.Controls.Add(new LiteralControl("<div style='width:100%;height:30px;text-align:left;background-color:#FFE271;'><strong>" + e.Day.Date.Day.ToString() + "</strong></div>"));

        }
        else
        {
            e.Cell.Controls.Add(new LiteralControl("<div style='width:100%;height:30px;text-align:left;background-color:#CDDEF2;'><strong>" + e.Day.Date.Day.ToString() + "</strong></div>"));

        }


        if (e.Day.Date.Day < 15 && e.Day.Date.Day >3 )
        {
            e.Cell.Controls.Add(new LiteralControl("<div style='padding-left:5px;padding-right:5px;'><div style='background-color:#C8D8EE;border: 3px outset #ffffff ;padding:1px;font-weight:bold;'><a class='popuplink' href='Default.aspx' style='font-size:9pt;text-decoration:none;'>09:00  <strong style='color:#000000;'> Water Quality </strong> </a> </div></div>"));

            e.Cell.Controls.Add(new LiteralControl("<div style='padding-left:5px;padding-right:5px;'><div style='background-color:#C8D8EE;border: 3px outset #ffffff ;padding:1px;font-weight:bold;'><a class='popuplink' href='Default.aspx' style='font-size:9pt;text-decoration:none;'>09:00  <strong style='color:#000000;'> Water Quality </strong> </a> </div></div>"));

        }

        if (e.Day.Date.Day == 17)
        {
            e.Cell.Controls.Add(new LiteralControl("<div style='padding-left:5px;padding-right:5px;'><div style='background-color:#C8D8EE;border: 3px outset #ffffff ;padding:1px;font-weight:bold;'><a class='popuplink' href='Default.aspx' style='font-size:9pt;text-decoration:none;'>09:00  <strong style='color:#000000;'> Water Quality </strong> </a> </div></div>"));

            e.Cell.Controls.Add(new LiteralControl("<div style='padding-left:5px;padding-right:5px;'><div style='background-color:#C8D8EE;border: 3px outset #ffffff ;padding:1px;font-weight:bold;'><a class='popuplink' href='Default.aspx' style='font-size:9pt;text-decoration:none;'>09:00  <strong style='color:#000000;'> Water Quality </strong> </a> </div></div>"));
            e.Cell.Controls.Add(new LiteralControl("<div style='padding-left:5px;padding-right:5px;'><div style='background-color:#C8D8EE;border: 3px outset #ffffff ;padding:1px;font-weight:bold;'><a class='popuplink' href='Default.aspx' style='font-size:9pt;text-decoration:none;'>09:00  <strong style='color:#000000;'> Water Quality </strong> </a> </div></div>"));

        }

    }

    protected void cldDate_SelectionChanged(object sender, EventArgs e)
    {


    }

    protected void cldDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {

    }

}