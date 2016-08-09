<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="TableGraphConfig.aspx.cs" Inherits="Pages_Graph_TableGraphConfig" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">

        function OnClose() {
            parent.$.fancybox.close();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="450px" cellpadding="3" cellspacing="2" border="0">
                <tr>
                    <td align="left" colspan="2">
                        <span class="TopTitle">Table Graph Settings</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td align="right">
                        <strong>X-Axis Column:</strong>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlGraphXAxisColumnID"
                            DataValueField="ColumnID" DataTextField="DisplayName"
                            CssClass="NormalTextBox">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong>Series Column:</strong>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlGraphSeriesColumnID"
                            DataValueField="ColumnID" DataTextField="DisplayName"
                            CssClass="NormalTextBox">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong>Default Graph Period:</strong>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlDefaultGraphPeriod" 
                            CssClass="NormalTextBox">
                            <asp:ListItem Value="">-- Please Select --</asp:ListItem>
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">1 year</asp:ListItem>
                            <asp:ListItem Value="2">6 months</asp:ListItem>
                            <asp:ListItem Value="3">3 months</asp:ListItem>
                            <asp:ListItem Value="4">1 month</asp:ListItem>
                            <asp:ListItem Value="5">1 week</asp:ListItem>
                            <asp:ListItem Value="6">1 day</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
            </table>

            <div style="position: absolute; bottom: 10px; right: 25px;">
                <asp:LinkButton runat="server" ID="LinkButtonOk" CssClass="btn"
                    OnClick="LinkButtonOk_Click">
                    <strong>OK</strong>
                </asp:LinkButton>
                <asp:LinkButton runat="server" ID="LinkButtonCancel" CssClass="btn"
                    style="margin-left:0.5em;"
                    OnClientClick="parent.$.fancybox.close(); return false;">
                    <strong>Cancel</strong>
                </asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

<%--    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
            window.parent.document.getElementById('HiddenButtonRefresh').click();
        });
    </script>--%>

</asp:Content>
