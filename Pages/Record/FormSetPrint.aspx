<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormSetPrint.aspx.cs" Inherits="Pages_Record_FormSetPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 <%@ Register Src="~/Pages/UserControl/DetailEdit.ascx" TagName="DetailEdit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="shortcut icon" href="../../Images/favicon.ico" />
 <link href="~/App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />
  <style  type="text/css">
      #divDynamic
      {
          font-size: 12px;
      }
      
      @media print
      {
          .hideImageForPrint
          {
              display: none;
          }
      }
  </style>

    <title>Print</title>
</head>
<body>
     <%--<script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-1.11.3.min.js")%>"></script>--%>
      <script type="text/javascript" src="<%=ResolveUrl("~/script/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/JS/Common.js")%>"></script>
    <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>
        <script type="text/javascript" src="<%=Request.Url.Scheme+@"://maps.google.com/maps/api/js?sensor=false" %>"></script>

    <script src="<%=Request.Url.Scheme+@"://ajax.googleapis.com/ajax/libs/jqueryui/1.8.23/jquery-ui.min.js" %>"></script>
    <link href="<%=Request.Url.Scheme+@"://ajax.googleapis.com/ajax/libs/jqueryui/1.8.23/themes/base/jquery-ui.css" %>" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server" style="background-color:White; background-image:none;">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="36000">
    </asp:ScriptManager>
    <div>
        <table align="center" border="0" cellpadding="0" cellspacing="0" class="printbanner">
            <tr>
                <td align="center" valign="middle" class="hideImageForPrint" bgcolor="#ffff99">
                  Choose the <b>Print</b> button in your browser to print
                    this document. Or <asp:LinkButton runat="server" ID="lnkPrint" OnClientClick="window.print();return false;" Text="Click Here"></asp:LinkButton>
                        
                </td>
            </tr>
        </table>
         <div runat="server" id="divDynamic" clientidmode="Static" style="padding:20px;">
         
         
         </div>
    </div>
    </form>
</body>
</html>
