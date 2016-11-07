<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ErrorLog.aspx.cs" Inherits="Pages_SystemData_ErrorLog" EnableEventValidation="false" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function abc() {
            var b = document.getElementById('<%= lnkSearch.ClientID %>');
            if (b && typeof (b.click) == 'undefined') {
                b.click = function () {
                    var result = true;
                    if (b.onclick) result = b.onclick();
                    if (typeof (result) == 'undefined' || result) {
                        eval(b.getAttribute('href'));
                    }
                }
            }

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <span class="TopTitle">Error Logs</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                    <td colspan="3" height="13">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                    </td>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                            <div id="search" style="padding-bottom: 10px" onkeypress="abc();">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                    HeaderText="List of validation errors" />

                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td>
                                                         <asp:TextBox runat="server" ID="txtErrorURL" CssClass="NormalTextBox" Width="1000px" ToolTip="Error URL with query string."></asp:TextBox>
                                                        <br /><br />
                                                   
                                                         <asp:LinkButton runat="server" ID="lnkErrorURL" CssClass="btn" OnClick="lnkErrorURL_Click"> <strong>Open this page as Account Holder</strong></asp:LinkButton>
                                                    
                                                            <br /><br /><br /><br />
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="border-collapse: collapse" cellpadding="4">

                                                <tr>
                                                    <td align="right">
                                                        <strong>Error Message</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtErrorMessageSearch" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
                                                        <br />
                                                    </td>
                                                    <td></td>
                                                    <td>

                                                        <div>

                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong></asp:LinkButton>

                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                        <td></td>
                                    </tr>
                                </table>

                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            AllowPaging="True" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            AllowSorting="True" DataKeyNames="ErrorLogID" HeaderStyle-ForeColor="Black" Width="100%"
                            AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("ErrorLogID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="viewHyperLink" runat="server" ToolTip="View" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("ErrorLogID").ToString())  %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Module" HeaderText="Module">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModule" runat="server" Text='<%# Eval("Module") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ErrorMessage" HeaderText="Error Message">
                                    <ItemTemplate>
                                        <asp:Label ID="lblErrorMessage" runat="server" Text='<%# GetErrorMessage( Eval("ErrorMessage").ToString() )%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ErrorTime" SortExpression="ErrorTime" HeaderText="Date/Time"
                                    ReadOnly="true" />
                                <asp:TemplateField SortExpression="Path" HeaderText="Path">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPath" runat="server" Text='<%# Eval("Path") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" IsInLine="true" HideAdd="true" OnDeleteAction="Pager_DeleteAction"
                                    OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                    Clear filter.</strong>
                            </asp:LinkButton>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                        <span style="font-weight: bold" align="center"></span>
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
