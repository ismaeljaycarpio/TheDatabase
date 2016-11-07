<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test2.aspx.cs" Inherits="Test_Test2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Schedule prototype</title>
    <style type="text/css">
        .OtherMonthDayStyle
        {
            background-color: #A5BFE1;
            padding: 0px;
        }
        .maincalender
        {
            background-color: #FFFFFF;
        }
        .DayStyle
        {
            padding: 0px;
        }
        .TitleStyle
        {
            font-size: 14pt;
            font-weight: bold;
        }
    </style>
</head>
<body style="background-image: none;">
     <%--<script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-1.11.3.min.js")%>"></script>--%>
    <script type="text/javascript" src="<%=ResolveUrl("~/script/jquery.js")%>"></script>
    <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"  type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>
   
    <form id="form1" runat="server">

     <script type="text/javascript">
        
    $(document).ready(function () {
        $(function () {
            $(".popuplink").fancybox({
                scrolling: 'auto',
                type: 'iframe',              
                width: 700,
                height: 550,
                titleShow: false              
            });
        });
    });
   
    </script>

    <div style="padding: 20px;">
        <asp:Calendar runat="server" ID="cldDate" Width="1200px" Height="500px" OnDayRender="cldDate_DayRender"
            CssClass="maincalender" OnSelectionChanged="cldDate_SelectionChanged" DayNameFormat="Full"
            BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" SelectedDayStyle-BackColor="ActiveBorder"
            SelectedDayStyle-ForeColor="Black" ShowGridLines="true" OnVisibleMonthChanged="cldDate_VisibleMonthChanged">
            <DayHeaderStyle VerticalAlign="Top" HorizontalAlign="Center" Height="30px" />
            <TitleStyle CssClass="TitleStyle" BackColor="White" Height="40px" />
            <DayStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="DayStyle" />
            <OtherMonthDayStyle CssClass="OtherMonthDayStyle" />
        </asp:Calendar>
    </div>
    <br />
      <br />
   <%--       <div>
              <div style='background-color:#C8D8EE;padding:2px;border: 2px solid #000000 ; border-style:outset; font-weight:bold; text-decoration:none; '>
                 <a class='popuplink' href='Default.aspx'>09:00 Water Quality</a>
              </div>
          </div>

    <br />
    <asp:HyperLink runat="server" NavigateUrl="~/Test/Default.aspx" CssClass="popuplink">Water</asp:HyperLink>--%>

    <br /><br /><br /><br /><br /><br />

    </form>
</body>
</html>
