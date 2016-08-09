<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Marketing2.master" AutoEventWireup="true"
    CodeFile="Public.aspx.cs" Inherits="Public" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <br />
    <div runat="server" id="divParentTable" visible="false" style="font-size:12px;">
        <table>
            <tr>
                <td align="right">
                </td>
                <td>
                    <b>Please enter following information and click on Continue button.</b>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td>
                    <asp:Label runat="server" ID="lblNotVaid" Text="Following information are valid, please try again."
                        Visible="false" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblParentRecord" Font-Bold="true" Text=""></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="NormalTextBox" ID="txtParentRecord"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblParentRecord2" Font-Bold="true" Text=""></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="NormalTextBox" ID="txtParentRecord2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:LinkButton runat="server" ID="lnkContinue" CssClass="btn" OnClick="lnkContinue_Click"> <strong>Continue</strong></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:Label runat="server" ID="lblBadRequest" Visible="false" 
    Text="Sorry! The request is not valid. Please contact with the administrator."></asp:Label>
    <br />
</asp:Content>
