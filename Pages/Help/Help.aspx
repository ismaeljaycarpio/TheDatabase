<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="Help.aspx.cs" Inherits="Pages_Help_Help" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div>
        <table>
            <tr>
                <td>
                    <h4>
                        <asp:Label runat="server" ID="lblHeading"></asp:Label></h4>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblHelpContent"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr >
                <td style="text-align: center;" align="center">
                    <div style="width: 100%; text-align: center; height:50px;">
                        <asp:LinkButton runat="server" ID="lnkClose" CssClass="btn" CausesValidation="false"
                            OnClientClick="parent.$.fancybox.close(); return false; "> <strong> Close</strong> </asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
