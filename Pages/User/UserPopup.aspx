<%@ Page Language="C#" MasterPageFile="~/Home/Popup.master" CodeFile="UserPopup.aspx.cs"
    Inherits="User_List" EnableEventValidation="false" %>

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

        function GetBack() {

            parent.$.fancybox.close();
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="650" align="center">
        <tr>
            <td colspan="3" height="40">
              

                <table width="100%" cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left"  style="width:50%;">
                                         <span class="TopTitle">Users </span>
                                    </td>
                                    <td align="left">
                                        <div style="width:40px; height:40px;">
                                            <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" >
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                            <div id="search" style="padding-bottom: 10px" onkeypress="abc();">
                               
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr id="tr1" runat="server" visible="true">
                                        <td align="right">
                                            <strong>Search</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtSearch" CssClass="NormalTextBox"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td style="width: 10px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <div>
                                                
                                                    <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong> </asp:LinkButton>
                                                        
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                      
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                             HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            AllowPaging="True" AllowSorting="True" DataKeyNames="UserID" HeaderStyle-ForeColor="Black"
                           AutoGenerateColumns="false" PageSize="5" OnSorting="gvTheGrid_Sorting"
                            OnPreRender="gvTheGrid_PreRender" Width="700px" >
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="true">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name" />
                                <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name" />
                                <asp:TemplateField Visible="true" HeaderText="Email" SortExpression="Email">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" 
                                    HideAdd="true" OnApplyFilter="Pager_OnApplyFilter"
                                    OnBindTheGridToExport="Pager_BindTheGridToExport" 
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" OnExportForCSV="Pager_OnExportForCSV" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false">
                               No user found.
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                    </Triggers>
                </asp:UpdatePanel>
                <span style="font-weight: bold" align="center"></span>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left">
                 <div>
                        <table>
                            <tr>
                               
                                <td>
                                    <div>
                                        
                                         <asp:LinkButton runat="server" ID="hlBack"  CssClass="btn" OnClientClick="GetBack();return false;" > <strong>Cancel</strong>   </asp:LinkButton>
                                              
                                    </div>
                                </td>
                                 <td>
                                    <div runat="server" id="divSave">
                                        
                                                    <asp:LinkButton runat="server" ID="lnkOk" CssClass="btn" CausesValidation="true"
                                                        OnClick="lnkOk_Click"> <strong>Add</strong></asp:LinkButton>
                                                
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
</asp:Content>
