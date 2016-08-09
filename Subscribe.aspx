<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="Subscribe.aspx.cs" Inherits="Subscribe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div style="padding: 0 10px 10px 10px;">
        <h2>Thank you for trying the demo</h2>
        <p>
            Please complete the form below to receive occassional emails: </p>
            <ul>
            <li>Special offers and promotions</li>
            <li>News of ETS major releases</li>
            </ul>
        <table>
            <tr runat="server" id="trSubscribeInfo" style="height:30px">
                <td align="right">
                    <strong>Name *</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="335px" CssClass="NormalTextBox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtName"
                        ErrorMessage="Required." Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height:30px">
                <td align="right">
                    <strong>Email *</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="335px" CssClass="NormalTextBox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Required." Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator1"
                        runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid." ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr style="height:30px">
                <td align="right">
                    <strong>Phone</strong>
                </td>
                <td>
                    <asp:TextBox ID="txtPhone" runat="server" Width="335px" CssClass="NormalTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td></td>
            <td><div runat="server" id="divSave">
                                                    <asp:LinkButton runat="server" ID="lnkGetQuote" CssClass="btn" OnClick="lnkSubmit_Click"
                                                        CausesValidation="true"> <strong>Submit</strong> </asp:LinkButton>
                                                </div></td>
            </tr>

        </table>
        <p>Note: You can unsubcribe at any time.</p>

        <span style="font-size:large; text-align:center; margin:0 0 0 50px"><a href="SignUp.aspx" target="_blank">Start Free Trial</a></span>
    </div>
</asp:Content>
