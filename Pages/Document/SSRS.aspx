<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="SSRS.aspx.cs" Inherits="Pages_Document_SSRS" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div style="width: 100%; height: 100%; padding: 10PX 10PX 10PX 10PX">
        <div style="padding-left: 500px;">
            <asp:HyperLink runat="server" ID="hlBack" ToolTip="Back">
                <asp:Image ID="Image25" runat="server" ImageUrl="~/App_Themes/Default/images/back32.png" />
            </asp:HyperLink>
        </div>
        <br />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%" AsyncRendering="false"  SizeToReportContent="true">
        </rsweb:ReportViewer>
        <br /> 
        <asp:Label runat="server" ID="lblNoReport" Visible="false" Text="No Report!"></asp:Label>
    </div>
</asp:Content>
