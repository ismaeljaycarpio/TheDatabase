<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ReportView.aspx.cs" Inherits="Pages_RRP_ReportView" %>

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
        <strong>Report: </strong>
        <asp:DropDownList runat="server" ID="ddlReport" CssClass="NormalTextBox" AutoPostBack="true"
        OnSelectedIndexChanged="ddlReport_SelectedIndexChanged">
            <%--<asp:ListItem Text="Risk Matrix" Value="risk"></asp:ListItem>--%>
        </asp:DropDownList>
        <br />     <br />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server">
        </rsweb:ReportViewer>
        <iframe runat="server" id="ifReport" visible="false" height="700px" width="1000px" ></iframe>
        <br /> 
        <asp:Label runat="server" ID="lblNoReport" Visible="false" Text="No Report!" ></asp:Label>
    </div>
</asp:Content>
