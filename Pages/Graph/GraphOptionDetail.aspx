<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" 
AutoEventWireup="true" CodeFile="GraphOptionDetail.aspx.cs" Inherits="Pages_Graph_GraphOptionDetail" %>


<%@ Register Src="~/Pages/UserControl/DBGGraphControl.ascx" TagName="GraphControl" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">

    <asp:GraphControl  runat="server" ID="gcTest"  ShowUseReportDates="false" ShowDates="false"  />

    <br />
 

</asp:Content>

