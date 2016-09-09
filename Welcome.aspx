<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" Inherits="DemoTips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div style="padding: 5px;">
        <table>
           <tr>
                <td colspan="2">
                       <asp:Label runat="server" ID="lblWelcomeTips"></asp:Label>                       
                </td>
            </tr>
            <tr runat="server" id="trContinue" visible="false">
                <td>
                </td>
                <td>
                     <asp:HyperLink runat="server" ID="hlContinue" CssClass="btn" Target="_parent"><strong>Continue</strong></asp:HyperLink>
                </td>
            </tr>
           
        </table>
    </div>
</asp:Content>

