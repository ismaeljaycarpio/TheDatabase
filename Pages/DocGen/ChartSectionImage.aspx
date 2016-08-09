<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChartSectionImage.aspx.cs"
    Inherits="DocGen.Document.ChartSection.ChartSectionImage" %>
<%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>
    <%@ Register Src="~/Pages/UserControl/DBGGraphControl.ascx" TagName="GraphControl" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div>       

        <div>
            <chart:WebChartViewer ID="Chart1" runat="server" Visible="true"  />
             <asp:GraphControl  runat="server" ID="gcTest"  Visible="false"  />
        </div>


    </div>
    </form>
</body>
</html>
