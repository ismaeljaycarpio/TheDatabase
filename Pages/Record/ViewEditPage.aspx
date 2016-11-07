<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="ViewEditPage.aspx.cs" Inherits="Pages_Record_ViewEditPage" %>
  <%@ Register Src="~/Pages/UserControl/ViewDetail.ascx" TagName="OneView" TagPrefix="dbg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
    <%--<link href="<%=ResolveUrl("~/Styles/jquery-ui-1.11.3.min.css")%>" rel="stylesheet"
        type="text/css" />
      <script type="text/javascript" src="<%=ResolveUrl("~/Styles/jquery-ui-1.11.3.min.js")%>"></script>    --%>


    <link href="<%=ResolveUrl("~/Styles/jquery-ui-1.7.3.custom.css")%>" rel="stylesheet"
        type="text/css" />
      <script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-ui-1.9.2.custom.min.js")%>"></script>  
  <%-- 
    

<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>
 --%>
 <script language="javascript" type="text/javascript">

      var fixHelper = function (e, ui) {
          ui.children().each(function () {
              $(this).width($(this).width());
          });

          return ui;
      };


     //function CloseAndRefresh() { jquery-ui-1.11.3.min.css
      //    window.parent.document.getElementById('btnReloadMe').click();
      //    parent.$.fancybox.close();
         
      //}

</script>



<div>
        <dbg:OneView runat="server" ID="vdOne" />
</div>
</asp:Content>

