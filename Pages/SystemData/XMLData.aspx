<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="XMLData.aspx.cs" Inherits="Pages_SystemData_XMLData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">


    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
        ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
    <table cellpadding="3">
        <tr>
            <td align="right">
                <strong>SearchCriteriaID(encrypted)</strong>
            </td>
            <td>
                <asp:TextBox ID="txtSearchCriteriaID" runat="server" Width="150px" CssClass="NormalTextBox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSearchCriteriaID" runat="server" ControlToValidate="txtSearchCriteriaID"
                    ErrorMessage="SearchCriteriaID - Required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                    CausesValidation="true"> <strong>Copy into XMLData table</strong> </asp:LinkButton>
            </td>
        </tr>

    </table>
</asp:Content>

