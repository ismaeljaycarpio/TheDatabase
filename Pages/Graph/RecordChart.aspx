<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" 
    CodeFile="RecordChart.aspx.cs" Inherits="Pages_Graph_RecordChart" EnableEventValidation="false" %>

    <%@ Register Src="~/Pages/UserControl/DBGGraphControl.ascx" TagName="GraphControl" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

    <div style="padding:10px;">
      <asp:GraphControl  runat="server" ID="gcTest"  ShowDates="false"  />
    </div>

</asp:Content>
