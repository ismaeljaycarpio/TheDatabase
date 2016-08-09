<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="ChartSection.aspx.cs" Inherits="DocGen.Document.ChartSection.Edit" %>

    <%@ Register Src="~/Pages/UserControl/DBGGraphControl.ascx" TagName="GraphControl" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
   
    <div >
      <asp:GraphControl  runat="server" ID="gcTest"  ShowEmail="false"
       ShowExportToPDF="false"  ShowNextPrevious="false" ShowDates="false" />
    </div>
   
    
</asp:Content>
