<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    EnableTheming="true" CodeFile="LookUp.aspx.cs" Inherits="Pages_LookUp_LookUp"
    EnableEventValidation="false" ValidateRequest="false" %>

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
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle" Text="LookUp Data"></asp:Label>
                </span>
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                            <div id="search" style="padding-bottom: 10px" onkeypress="abc();">
                                <div class="epraybackground" style="color: #ffffff;">
                                    <table style="border-collapse: collapse" cellpadding="4">
                                        <tr>
                                            <td align="right">
                                                <strong>Search Text</strong>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDisplayText" CssClass="roundedtextbox" Width="250px"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <%--<asp:ImageButton runat="server" ID="lnkSearch" SkinID="go" 
                                                    onclick="lnkSearch_Click"/>--%>
                                                <div>
                                                    
                                                        <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn"
                                                         OnClick="lnkSearch_Click"> <strong>Go </strong> </asp:LinkButton>
                                                          
                                                </div>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="epraybackground">
                            <div class="onlyrounded" style="background-color: #ffffff;">
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
                                <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                    AllowPaging="True" AllowSorting="True" DataKeyNames="LookupDataID" HeaderStyle-ForeColor="Black"
                                    Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                                    OnPreRender="gvTheGrid_PreRender">
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
                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("LookupDataID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("LookupDataID").ToString()) %>'
                                                    ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField  SortExpression="DisplayText" HeaderText="Display Text">
                                   <ItemStyle  HorizontalAlign="left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDisplayText" runat="server" Text='<%# Eval("DisplayText") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                        <asp:BoundField DataField="DisplayText" SortExpression="DisplayText" HeaderText="Display Text" />
                                        <asp:BoundField DataField="Value" SortExpression="Value" HeaderText="Value" />
                                        <%--<asp:TemplateField  SortExpression="Value" HeaderText="Value">
                                    <ItemStyle  HorizontalAlign="left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField> --%>
                                        <asp:BoundField DataField="DateAdded" SortExpression="DateAdded" HeaderText="Date Added"
                                            DataFormatString="{0:d}" ReadOnly="true" />
                                        <asp:BoundField DataField="DateUpdated" SortExpression="DateUpdated" HeaderText="Date Updated"
                                            DataFormatString="{0:d}" ReadOnly="true" />
                                    </Columns>
                                    <HeaderStyle CssClass="gridview_header" />
                                    <RowStyle CssClass="gridview_row" />
                                    <PagerTemplate>
                                        <asp:GridViewPager runat="server" ID="Pager" HideDelete="false" OnApplyFilter="Pager_OnApplyFilter"
                                            OnDeleteAction="Pager_DeleteAction" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                            OnBindTheGridAgain="Pager_BindTheGridAgain" />
                                    </PagerTemplate>
                                </dbg:dbgGridView>
                                <br />
                                <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                                    <asp:HyperLink runat="server" ID="hplNewData">
                                        <asp:Image runat="server" ID="imgAddNewRecord" SkinID="BigAdd" />
                                        <strong>Add new Record</strong>
                                    </asp:HyperLink>
                                </div>
                                <br />
                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                        <asp:AsyncPostBackTrigger ControlID="lnkSearch" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
    </table>
</asp:Content>
