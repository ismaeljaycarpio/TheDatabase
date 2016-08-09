<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TableList.aspx.cs" Inherits="Pages_Template_TableList" EnableEventValidation="false" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%--  <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"   type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <span class="TopTitle" runat="server" id="spanCaption">Add Table From Template</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <img alt="Processing..." src="../../Images/ajax.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </div>
                                </td>
                                <td>
                                    <%--<asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static" NavigateUrl="~/Pages/Help/Help.aspx?contentkey=CopyTableHelp">
                                        <asp:Image ID="Image1" runat="server"  ImageUrl="~/App_Themes/Default/images/help.png"  />
                                    </asp:HyperLink>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                    </td>
                    <td valign="top">
                        <div id="divAccounts" runat="server" visible="false" style="padding-bottom: 10px;
                            text-align: left;">
                            <table style="border-collapse: collapse" cellpadding="4">
                                <tr>
                                    <td align="right">
                                        <strong>From Account</strong>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFromAccount" DataValueField="AccountID" DataTextField="AccountName"
                                            CssClass="NormalTextBox" AutoPostBack="true" OnSelectedIndexChanged="ddlFromAccount_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="right">
                                        <strong>To Account</strong>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlToAccount" DataValueField="AccountID" DataTextField="AccountName"
                                            CssClass="NormalTextBox">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                      <span runat="server" id="spanAvailableTablesCap"> Available Tables:</span>
                        <br />
                        <table>
                            <tr>
                                <td style="min-width:400px;">
                                    <dbg:dbgGridView ID="gvTheGrid" runat="server" HeaderStyle-HorizontalAlign="Center"
                                        RowStyle-HorizontalAlign="Center" GridLines="Both" CssClass="gridview" AllowPaging="false"
                                        AllowSorting="True" DataKeyNames="TableID" HeaderStyle-ForeColor="Black" Width="100%"
                                        AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting" OnPreRender="gvTheGrid_PreRender"
                                        OnRowDataBound="gvTheGrid_RowDataBound">
                                        <PagerSettings Position="Top" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="LblID" runat="server" Text='<%# Eval("TableID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkDelete" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="TableName" HeaderText="Table Name">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlView" runat="server" NavigateUrl="#" Text='<%# Eval("TableName")%>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Menu" HeaderText="Menu" SortExpression="Menu" Visible="false" />
                                            <%--<asp:TemplateField HeaderText="Include example data" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkWithData" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                        </Columns>
                                        <HeaderStyle CssClass="gridview_header" />
                                        <RowStyle CssClass="gridview_row" />
                                        <PagerTemplate>
                                            <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" HideAdd="true" OnExportForCSV="Pager_OnExportForCSV"
                                                OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                                OnBindTheGridAgain="Pager_BindTheGridAgain" />
                                        </PagerTemplate>
                                    </dbg:dbgGridView>
                                </td>
                                <td style="padding-left: 100px; vertical-align: top;">
                                    <div style="background-color: #FFE8BC; padding: 10px; width: 230px;">
                                        <asp:Label runat="server" ID="lblHelpContent"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                        <br />
                        <div>
                            <table>
                                <tr>
                                    <td>
                                         <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>&lt;&nbsp;Back</strong> </asp:HyperLink>
                                    </td>

                                     <td>
                                        <div>
                                            <asp:HyperLink runat="server" ID="hlCancel" CssClass="btn"> <strong>Cancel</strong> </asp:HyperLink>
                                        </div>
                                    </td>

                                    <td>
                                        <div>
                                            <asp:LinkButton runat="server" ID="lnkCopyTemplateOnly" CssClass="btn" OnClick="lnkCopyTemplateOnly_Click"> <strong>Copy Template Only</strong> </asp:LinkButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:LinkButton runat="server" ID="lnkCopyExampleData" CssClass="btn" OnClick="lnkCopyExampleData_Click"> <strong>Copy with Example Data</strong> </asp:LinkButton>
                                        </div>
                                    </td>
                                   
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
