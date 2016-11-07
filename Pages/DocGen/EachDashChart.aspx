<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EachDashChart.aspx.cs" Inherits="Pages_DocGen_EachDashChart" %>
 <%@ Register Src="~/Pages/UserControl/DBGGraphControl.ascx" TagName="GraphControl" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
                <link href="~/App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />

</head>
<body style="background-image:none; background-color:#ffffff;">
     <%--<script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-1.11.3.min.js")%>"></script>--%>
<script type="text/javascript" src="<%=ResolveUrl("~/script/jquery.js")%>"></script>
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
    </asp:ScriptManager>
    <div>
         <asp:GraphControl  runat="server" ID="gcGraph" 
            BackURL="~/Pages/DocGen/DashBoard.aspx"  ParentPage="home" Mode="edit" EnableViewState="true"  />  
    </div>
    </form>
</body>
</html>
