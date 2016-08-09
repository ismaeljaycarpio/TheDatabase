<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="AddAccount.aspx.cs" Inherits="Pages_User_AddAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%--<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />--%>
    <%--<script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>

    <script language="javascript" type="text/javascript">


        function GetBackValue() {
            window.parent.document.getElementById('btnRefreshLinkedUser').click();
            parent.$.fancybox.close();
        }

    </script>
    <table border="0" cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle" Text="Link Account"></asp:Label></span>
                        </td>
                        <td align="right">
                            <%--<asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static" NavigateUrl="~/Pages/Help/Help.aspx?contentkey=AddAccountHelp">
                                <asp:Image ID="Image2" runat="server"  ImageUrl="~/App_Themes/Default/images/help.png"  />
                            </asp:HyperLink>--%>
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
                <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                <div id="search" style="padding-bottom: 10px">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                        ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div runat="server" id="divDetail">
                        <table cellpadding="3" cellspacing="0">
                            <tr>
                                <td align="right">
                                    <strong>Email</strong>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtEmail" runat="server" Width="250px" CssClass="NormalTextBox"
                                        Text="" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Email/UserName - Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr runat="server" id="trPassword">
                                <td align="right">
                                    <strong>Password</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="NormalTextBox" TextMode="Password"
                                        Text="" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                        ErrorMessage="Password - Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--<tr runat="server" id="trSelectAccount" visible="false">
                                <td align="right">
                                    <strong>Select the other account:</strong>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSelectAccount" runat="server" CssClass="NormalTextBox"
                                     DataValueField="AccountID" DataTextField="AccountName">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>

                            <tr runat="server" id="trRoles" visible="false">
                                <td align="right">
                                    <strong>Role:</strong>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBasicRoles" runat="server" CssClass="NormalTextBox"
                                        DataTextField="Role" DataValueField="RoleID">
                                        <%-- <asp:ListItem Value="5" Text="Read Only" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Add Record Data Only "></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Add and Edit Record Data"></asp:ListItem>                                     
                                        <asp:ListItem Value="2" Text="Administrator"></asp:ListItem> --%>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:HiddenField runat="server" ID="hfSelectedAccount" Value="" />
                    <div style="padding-left: 150px;">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <asp:LinkButton runat="server" ID="hlBack" CssClass="btn" OnClientClick=" parent.$.fancybox.close(); return false;"> <strong>Cancel</strong> </asp:LinkButton>
                                        <asp:HyperLink runat="server" ID="hlCancel" CssClass="btn" Text="Cancel" Visible="false"></asp:HyperLink>
                                    </div>
                                </td>

                                <td>
                                    <div runat="server" id="divSave" visible="false">
                                        <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" CausesValidation="true"
                                            OnClick="lnkSave_Click"> <strong>Save</strong> </asp:LinkButton>
                                    </div>
                                    <div runat="server" id="divVerify">
                                        <asp:LinkButton runat="server" ID="lnkVerify" CssClass="btn" CausesValidation="true"
                                            OnClick="lnkVerify_Click"> <strong>Verify</strong> </asp:LinkButton>
                                    </div>
                                </td>

                            </tr>

                        </table>
                    </div>
                </asp:Panel>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="3">
                <p style="padding-left: 20px; color: #6077FF;">
                    In order to link to another account you must enter the Email
                    <br />
                    and Password of an admin user in that account and choose
                    <br />
                    the role you will have in that account
                </p>
            </td>
        </tr>
    </table>
</asp:Content>
