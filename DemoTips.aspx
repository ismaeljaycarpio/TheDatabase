<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="DemoTips.aspx.cs" Inherits="DemoTips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div style="padding: 5px;">
        <table>
           <tr>
                <td colspan="2">
                       <asp:Label runat="server" ID="lblDemoTips"></asp:Label>
                       <br />
                    
                </td>
            </tr>
           
        </table>
    </div>
</asp:Content>

