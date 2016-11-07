<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="UpdateLinkedTables.aspx.cs" Inherits="Pages_SystemData_UpdateLinkedTables" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle" Text="Update Linked Tables"></asp:Label></span>
                        </td>
                        <td align="left">
                            <div style="width: 40px; height: 40px;">
                                <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                    <ProgressTemplate>
                                        <table style="width: 100%; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>--%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13"></td>
        </tr>

        <tr>
            <td valign="top"></td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">

                                    <tr>
                                        <td align="right">
                                            <strong>Parent table</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlParentTable" CssClass="NormalTextBox" CausesValidation="false"
                                                DataValueField="TableID" DataTextField="TableName" OnSelectedIndexChanged="ddlParentTable_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlParentTable"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Child table</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlChildTable" CssClass="NormalTextBox" CausesValidation="false"
                                                DataValueField="TableID" DataTextField="TableName" OnSelectedIndexChanged="ddlChildTable_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlChildTable"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Child Field to set</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" AutoPostBack="false" ID="ddlChildFieldtoset" CssClass="NormalTextBox"
                                                DataValueField="SystemName" DataTextField="DisplayName">
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlChildFieldtoset"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Child join field</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" AutoPostBack="false" ID="ddlChildjoinfield" CssClass="NormalTextBox"
                                                DataValueField="SystemName" DataTextField="DisplayName">
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlChildjoinfield"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td align="right">
                                            <strong>Parent join field</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" AutoPostBack="false" ID="ddlParentjoinfield" CssClass="NormalTextBox"
                                                DataValueField="SystemName" DataTextField="DisplayName">
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlParentjoinfield"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" style="height: 20px;"></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Child join field 2</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" AutoPostBack="false" ID="ddlChildjoinfield2" CssClass="NormalTextBox"
                                                DataValueField="SystemName" DataTextField="DisplayName">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td align="right">
                                            <strong>Parent join field 2</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" AutoPostBack="false" ID="ddlParentjoinfield2" CssClass="NormalTextBox"
                                                DataValueField="SystemName" DataTextField="DisplayName">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>


                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />

                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div runat="server" id="divSave">

                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                    CausesValidation="true"> <strong>OK</strong> </asp:LinkButton>

                                            </div>
                                        </td>
                                        <td>
                                            <div>

                                                <asp:HyperLink runat="server" ID="hlBack" CssClass="btn" NavigateUrl="~/Default.aspx"> <strong>Cancel</strong> </asp:HyperLink>

                                            </div>
                                        </td>

                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="3" height="13"></td>
        </tr>
    </table>
</asp:Content>

