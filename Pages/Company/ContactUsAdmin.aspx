<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ContactUsAdmin.aspx.cs" Inherits="Pages_Company_ContactUsAdmin" EnableEventValidation="false" %>

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
    <div id="Content" class="ContentMain">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                    <tr>
                        <td colspan="3">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" style="width: 50%;">
                                        <span class="TopTitle">Quotes & Contacts</span>
                                    </td>
                                    <td align="left">
                                        <div style="width: 40px; height: 40px;">
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                                    <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                HeaderText="List of validation errors" />--%>
                                    <table style="border-collapse: collapse" cellpadding="4">
                                        <tr>
                                            <td align="right">
                                                <strong>Email</strong>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtEmail" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <%--<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="../../Images/iconGo.png" 
                                            onclick="btnSearch_Click" />--%>
                                                <div>
                                                    
                                                                <asp:LinkButton runat="server" ID="lnkSearch" 
                                                                CssClass="btn" OnClick="lnkSearch_Click"> <strong> Go </strong> </asp:LinkButton>
                                                      
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                AllowSorting="True" DataKeyNames="ContactID" HeaderStyle-ForeColor="Black" Width="100%"
                                AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting" OnPreRender="gvTheGrid_PreRender"
                                OnRowDataBound="gvTheGrid_RowDataBound">
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
                                            <asp:Label ID="LblID" runat="server" Text='<%# Eval("ContactID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contact Type" SortExpression="ContactTypeID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContactType" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SubscriptionDate" SortExpression="SubscriptionDate" HeaderText="Subscription Date"
                                        DataFormatString="{0:d}" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Phone">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhone" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Message">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMessage" runat="server" Text='<%# Eval("Message") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="DateUpdated" SortExpression="DateUpdated" HeaderText="Date Updated" DataFormatString="{0:d}" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />--%>
                                </Columns>
                                <HeaderStyle CssClass="gridview_header" />
                                <RowStyle CssClass="gridview_row" />
                                <PagerTemplate>
                                    <asp:GridViewPager runat="server" ID="Pager" HideDelete="false" HideAdd="true" OnExportForCSV="Pager_OnExportForCSV"
                                        OnDeleteAction="Pager_DeleteAction" OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
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
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
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
    </div>
</asp:Content>
