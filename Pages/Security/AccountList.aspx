<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="AccountList.aspx.cs" Inherits="Pages_Security_AccountList" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
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
                                    <span class="TopTitle">Accounts </span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                                            <strong>Account Name</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAccountNameSearch" CssClass="NormalTextBox"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td style="width: 10px">
                                        </td>
                                        <td align="right">
                                            <strong>Name</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNameSearch" CssClass="NormalTextBox"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td align="right">
                                            <strong>Email</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEmailSearch" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td>
                                           
                                           <%-- <div>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="bL">
                                                            &nbsp;
                                                        </td>
                                                        <td class="bC">
                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="ButtonLink" OnClick="lnkSearch_Click"> Go</asp:LinkButton>
                                                        </td>
                                                        <td class="bR">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>

                                             <div>
                                               
                                                <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"><strong>Go</strong> </asp:LinkButton>
                                                      
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <asp:CheckBox ID="chkIsActive" Checked="false" runat="server" AutoPostBack="true"
                                                Text="Show Deleted Records" OnCheckedChanged="chkIsActive_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <div>
                            <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                AllowPaging="True" AllowSorting="True" DataKeyNames="AccountID" HeaderStyle-ForeColor="Black"
                                Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                                OnPreRender="gvTheGrid_PreRender" OnRowCommand="gvTheGrid_RowCommand" OnRowDataBound="gvTheGrid_RowDataBound">
                                <PagerSettings Position="Top" />
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="LblID" runat="server" Text='<%# Eval("AccountID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="AccountID">
                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("AccountID").ToString())  %>'
                                                ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Name" SortExpression="AccountName">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("AccountID").ToString())  %>'
                                                Text='<%# Eval("AccountName")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                    <asp:BoundField DataField="AccEmail" SortExpression="AccEmail" HeaderText="Email" />
                                    <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" SortExpression="PhoneNumber" />
                                    <asp:TemplateField HeaderText="Account Type" SortExpression="AccountTypeID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountTypeID" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ExpiryDate" SortExpression="ExpiryDate" HeaderText="Expiry Date"
                                        DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />



                                     <asp:TemplateField HeaderText="Signed In" SortExpression="SignedInCount">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlSignedInCount" runat="server" NavigateUrl='<%# GetSignedURL() + Cryptography.Encrypt(Eval("AccountID").ToString())  %>'
                                                Text='<%# Eval("SignedInCount")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Uploaded" SortExpression="UploadedCount">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlUploadedCount" runat="server" NavigateUrl='<%# GetUploadedURL() + Cryptography.Encrypt(Eval("AccountID").ToString())  %>'
                                                Text='<%# Eval("UploadedCount")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Wizard" SortExpression="CreatedByWizard" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedByWizard" runat="server" Text='<%# Eval("CreatedByWizard") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="SMS Count" SortExpression="SMSCount" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSMSCount" runat="server" Text='<%# Eval("SMSCount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkUse" CommandName="Use" CommandArgument='<%# Eval("AccountID")%>'
                                                ForeColor='<%# GetUseColor(Eval("AccountID").ToString()) %>'><%# GetUseCaption(Eval("AccountID").ToString()) %></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                                <HeaderStyle CssClass="gridview_header" />
                                <%--<RowStyle CssClass="gridview_row" />--%>
                                <PagerTemplate>
                                    <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" OnExportForCSV="Pager_OnExportForCSV"
                                        OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                        OnBindTheGridAgain="Pager_BindTheGridAgain" />
                                </PagerTemplate>
                            </dbg:dbgGridView>
                        </div>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                            <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                    Add new record now.</strong>
                            </asp:HyperLink>
                        </div>
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                    Clear filter</strong>
                            </asp:LinkButton>
                                                       or
                            <asp:HyperLink runat="server" ID="hplNewDataFilter" Style="text-decoration: none;
                                color: Black;">                                
                                  <strong style="text-decoration: underline; color: Blue;">
                                     Add new record.</strong>
                            </asp:HyperLink>
                        </div>
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
