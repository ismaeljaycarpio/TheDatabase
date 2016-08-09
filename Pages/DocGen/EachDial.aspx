<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EachDial.aspx.cs" Inherits="Pages_DocGen_EachDial" %>
 <%@ Register Assembly="netchartdir" Namespace="ChartDirector" TagPrefix="chart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
                <link href="~/App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />

</head>
<body style="background-image:none; background-color:#ffffff;">
    <form id="form1" runat="server">
    <table>
        <tr runat="server" id="trHeading">
            <td>
                <asp:Label runat="server" ID="lblHeading" Text="" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <div style="padding:5px;">
          <chart:WebChartViewer ID="wcFirst" runat="server" Visible="true" />
    </div>
            </td>
        </tr>
    </table>
    
    </form>
</body>
</html>
