<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="Alert.aspx.cs" Inherits="Pages_Record_Alert" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div style="padding: 10px;">
        <table>
            <tr>
                <td valign="top" style="padding-right:20px;">
                    <asp:Image runat="server" ImageUrl="~/App_Themes/Default/images/alert.png" />
                </td>
                <td valign="top">
                    <h1>
                        ALERT!</h1>
                    <p>
                        The following warnings have been raised recently:</p>
                    <dbg:dbgGridView ID="gvWarning" runat="server" GridLines="Both" CssClass="gridview"
                        HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Left" AllowPaging="false"
                        AllowSorting="True" DataKeyNames="TableID" HeaderStyle-ForeColor="Black"
                        Width="100%" AutoGenerateColumns="false" PageSize="5" ShowHeader="false">
                        <PagerSettings Position="Top" />
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemStyle Width="10px" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="LblID" runat="server" Text='<%# Eval("TableID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Last Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastWarningTime" runat="server" Text='<%# Eval("LastWarningTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Table" >
                                <ItemTemplate>
                                  <li>  <asp:Label ID="lblTableName" runat="server" Text='<%# Eval("TableName") %>'></asp:Label>
                                     &nbsp;&nbsp; <b>-</b>&nbsp;&nbsp;
                                    <asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# GetWarningViewURL() + Cryptography.Encrypt(Eval("TableID").ToString())  %>'
                                        Text='<%# Eval("WarningCount")%>' Target="_blank" />   </li>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            -- No Alerts --
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="gridview_header" />
                        <RowStyle CssClass="gridview_row" />
                    </dbg:dbgGridView>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:CheckBox  ID="chkDoNotShowMe" runat="server" TextAlign="Right" CssClass="NormalTextBox" 
                        Text="Do not show again" AutoPostBack="true" 
                        oncheckedchanged="chkDoNotShowMe_CheckedChanged"/>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                <br />
                    <asp:LinkButton runat="server" ID="CancelButton" CssClass="btn" CausesValidation="false"
                        OnClientClick="parent.$.fancybox.close(); return false; "> <strong> Dismiss</strong> </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
