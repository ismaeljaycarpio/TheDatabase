<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="MessageList.aspx.cs" Inherits="Pages_Record_MessageList" %>
<%@ Register Src="~/Pages/UserControl/MessageList.ascx" TagName="MLList" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
    <div style="padding:10px;">
        <asp:Label runat="server" CssClass="TopTitle" Text="Messages"></asp:Label>
        <br />
        <asp:MLList runat="server" ID="mlAll" />
    </div>
</asp:Content>

