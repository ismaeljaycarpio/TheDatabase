<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordReminder.aspx.cs"
    MasterPageFile="~/Home/Marketing2.master" Inherits="Security_PasswordReminder" EnableTheming="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
   <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"   type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>

   


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="ContentMainTra" style="width: 970px;">
                <tr>
                    <td width="28">
                    </td>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <span class="TopTitle">Password Reminder</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <img alt="Processing..." src="../Images/ajax.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                </td>
                                <td align="right">
                                                <asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static"
                                                NavigateUrl="~/Pages/Help/Help.aspx?contentkey=PasswordReminderHelp" >
                                                <asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/Default/images/help.png"  />
                                                </asp:HyperLink>
                                    </td>
                            </tr>
                        </table>
                    </td>
                    <td width="28">
                    </td>
                </tr>
              
                <tr>
                    <td width="28">
                    </td>
                    <td align="center">
                        Please enter your email address.
                    </td>
                    <td width="28">
                    </td>
                </tr>
                <tr>
                    <td width="28">
                    </td>
                    <td align="center" height="20px">
                        <asp:Label ID="lblEmailStatus" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                    <td width="28">
                    </td>
                </tr>
                <tr>
                    <td width="28">
                    </td>
                    <td align="center" height="22px">
                        <asp:TextBox ID="txtEmail" CssClass="NormalTextBox" MaxLength="100" runat="server"  Width="300px"></asp:TextBox>
                        <br />
                        <br />
                    </td>
                    <td width="28">
                    </td>
                </tr>
                <tr>
                    <td width="28">
                    </td>
                    <td align="center" height="40px">
                        <div>
                            <table>
                                <tr>
                                   
                                    <td>
                                        <div>
                                            <asp:LinkButton runat="server" ID="lnkReturn" OnClick="lnkReturn_Click" CssClass="btn"> <strong>Return</strong> </asp:LinkButton>
                                        </div>
                                    </td>

                                     <td>
                                        <div runat="server" id="divSave">
                                            <asp:LinkButton runat="server" ID="lnkSendPassword" CssClass="btn" OnClick="lnkSendPassword_Click"> <strong>Send Password</strong> </asp:LinkButton>
                                        </div>
                                    </td>

                                </tr>
                            </table>
                        </div>
                    </td>
                    <td width="28">
                    </td>
                </tr>
                <tr>
                    <td width="28">
                    </td>
                    <td height="20">
                    </td>
                    <td width="28">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
