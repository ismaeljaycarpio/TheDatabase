<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="EachRecordTable.aspx.cs"
    Inherits="Pages_DocGen_EachRecordTable" %>

<%@ Register Src="~/Pages/UserControl/RecordList.ascx" TagName="RecordList" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />
    <asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>
</head>
<body style="background-color: #FFFFFF; background-image: none;">
    
     <%--<script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-1.11.3.min.js")%>"></script>--%>
    <script type="text/javascript" src="<%=ResolveUrl("~/script/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/JS/Common.js")%>"></script>
     <script type="text/javascript" src="<%=ResolveUrl("~/JS/modernizr.custom.80028.js")%>"></script>  
     <link type="text/css" rel="stylesheet"  href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" />
        <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>
     <%--  <script type="text/javascript" src="<%=ResolveUrl("~/fancybox2/source/jquery.fancybox.js?v=2.1.5")%>"></script>
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/fancybox2/source/jquery.fancybox.css?v=2.1.5")%>" media="screen" />--%>
 

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="36000">
        </asp:ScriptManager>

<asp:UpdateProgress class="ajax-indicator-full" ID="upMaster" runat="server">
    <ProgressTemplate>
        <table style="width: 100%; height: 100%; text-align: center;">
            <tr valign="middle">
                <td>
                    <p style="font-size:12px;">
                        Please wait...
                    </p>
                    <asp:Image runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                </td>
            </tr>
        </table>
    </ProgressTemplate>
</asp:UpdateProgress>
        <asp:UpdatePanel ID="upNotificationMessage" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="divNotificationMessage" style="position: fixed; top: 0px;">
                    <asp:Label runat="server" ID="lblNotificationMessage"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="Content" style="padding-right: 10px;">
            <asp:RecordList runat="server" ID="rlOne" PageType="p" ShowAddButton="true" ShowEditButton="true" />
        </div>
    </form>


    <script type="text/javascript">

        $(document).ready(function () {
            var close = document.getElementById('aNotificationMessageClose');


            if (close != null) {
                close.addEventListener('click', function () {
                    var note = document.getElementById('divNotificationMessage');
                    note.style.display = 'none';
                }, false);
            }

            function HidedivNotificationMessage() {
                $('#divNotificationMessage').fadeOut();
                var note = document.getElementById('divNotificationMessage');
                if (note != null)
                    note.style.display = 'none';
            }
            // setTimeout(function () { HidedivNotificationMessage(); }, " + hfTopMessageDisplayNumberSeconds.Value + @");


        });
    </script>
</body>
</html>
