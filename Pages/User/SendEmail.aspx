<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="SendEmail.aspx.cs" Inherits="Pages_User_SendEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%--  <link href="<% =ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css") %>" rel="stylesheet"      type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <script type="text/javascript" src="<%=ResolveUrl("~/js/jquery.autogrow-textarea.js")%>"></script>
    <script type="text/javascript">

        $(function () {
            $(".popuplink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 700,
                height: 400,
                titleShow: false
            });
        });

       


    </script>
    <div style="padding-left: 50px; padding-top: 5px; padding-bottom: 50px; padding-right: 50px;">
        <table cellpadding="3">
            <tr>
                <td colspan="3" height="40">
                    <span class="TopTitle">
                        <asp:Label runat="server" ID="lblTitle" Text="Send Email"></asp:Label></span>
                </td>
            </tr>
            <tr>
                <td colspan="3" height="13">
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>To:</strong>
                </td>
                <td align="left" style="width: 500px;">
                    <asp:TextBox ID="txtTo" runat="server" Width="500px" Height="20px" CssClass="MultiLineTextBox"
                        Font-Size="11px" ClientIDMode="Static" TextMode="MultiLine" Text=""></asp:TextBox>
                </td>
                <td align="left">
                    <asp:HyperLink runat="server" ID="hlAddTo" Text="Add User…" CssClass="popuplink"
                        NavigateUrl="~/Pages/User/UserPopup.aspx?type=to"> </asp:HyperLink>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td align="right">
                    <strong>CC:</strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCC" runat="server" Width="500px" Height="20px" CssClass="MultiLineTextBox"
                        Font-Size="11px" TextMode="MultiLine" Text=""></asp:TextBox>
                </td>
                <td align="left">
                    <asp:HyperLink runat="server" ID="hlAddCC" Text="Add User…" CssClass="popuplink"
                        NavigateUrl="~/Pages/User/UserPopup.aspx?type=cc"> </asp:HyperLink>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td align="right">
                    <strong>BCC:</strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBCC" runat="server" Width="500px" Height="20px" CssClass="MultiLineTextBox"
                        Font-Size="11px" TextMode="MultiLine" Text=""></asp:TextBox>
                </td>
                <td align="left">
                    <asp:HyperLink runat="server" ID="hlAddBCC" Text="Add User…" CssClass="popuplink"
                        NavigateUrl="~/Pages/User/UserPopup.aspx?type=bcc"> </asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Subject:</strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtSubject" runat="server" Width="500px" CssClass="NormalTextBox"
                        Text=""></asp:TextBox>
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Message:</strong>
                </td>
                <td align="left" colspan="2">
                    <asp:TextBox ID="txtMessage" runat="server" Width="500px" Height="150px" CssClass="MultiLineTextBox"
                        Font-Size="11px" TextMode="MultiLine" Text="Please find attached Record data in CSV format."></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Attachment:</strong>
                </td>
                <td align="left">
                    <%--<asp:TextBox ID="txtAttachment" runat="server" Width="500px" CssClass="NormalTextBox" BackColor="InactiveBorder"
                        Text="" ReadOnly="true"></asp:TextBox>--%>
                    <asp:Label runat="server" ID="lblAttachmnet"></asp:Label>
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td align="left">
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>Cancel</strong>  </asp:HyperLink>
                                    </div>
                                </td>
                                <td>
                                    <div runat="server" id="divSave">
                                        <asp:LinkButton runat="server" ID="lnkSend" CssClass="btn" CausesValidation="true"
                                            OnClick="lnkSend_Click"> <strong>Send</strong> </asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label runat="server" ForeColor="Red" ID="lblMsg"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <br />
        <asp:HiddenField runat="server" ID="hfType" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfFileName" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hfSelectedEmails" ClientIDMode="Static" />
        <asp:Button runat="server" ID="btnUpdateEmail" ClientIDMode="Static" OnClick="btnUpdateEmail_Click"
            Style="display: none;" />
    </div>
    <script type='text/javascript'>
        $(function () {
            $('textarea').autogrow();
        });

    </script>
</asp:Content>
