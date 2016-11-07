<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="GraphPopup.aspx.cs" Inherits="Pages_Graph_GraphPopup" EnableEventValidation="false" %>

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

         function GetBack() {

             parent.$.fancybox.close();
         }

         function CheckOnlyOne(masterCheckbox, containerID) {
             var checked = masterCheckbox.checked;
             $('#' + containerID + ' input[type="checkbox"]').attr('checked', false);
             masterCheckbox.checked = true;
         }

//          $(document).ready(function () {
//             $("input[type='checkbox']").change(function () {
//                 $(this).closest(".checkboxContainer").find("input[type='checkbox']").not(this).prop("checked", false);
//                 $(this).prop("checked", true);
//             });
//     });
       
    </script>
     <div style="padding-left:20px;padding-right:20px;">
           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" width="100%" cellpadding="0" cellspacing="0"  align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" >
                                    <span class="TopTitle" style="min-width:600px;">Open Saved Graphs</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr runat="server" visible="false">
                                        <td align="right">
                                           
                                        </td>
                                        <td>
                                             <asp:CheckBox ID="chkIsActive" Checked="false" runat="server" AutoPostBack="true"
                                                Text="Show Deleted Records" OnCheckedChanged="chkIsActive_CheckedChanged" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <div>
                                              
                                                 <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click" Visible="false"> <strong></strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <div id="divGridMain" >
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                             HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            AllowPaging="True" AllowSorting="True" DataKeyNames="GraphOptionID" HeaderStyle-ForeColor="Black"
                            Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                            OnPreRender="gvTheGrid_PreRender">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("GraphOptionID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <%--<asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                   
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server"  onclick="CheckOnlyOne(this, 'divGridMain')" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%--<asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("GraphOptionID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                               
                                <asp:TemplateField HeaderText="Graph Title">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkHeading" runat="server" Text='<%# Eval("Heading") %>' 
                                        CommandArgument='<%# Eval("GraphOptionID") %>' OnClick="lnkOk_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                              
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" HideAdd="true"
                                HideExport="true" 
                                    OnApplyFilter="Pager_OnApplyFilter"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" HideFilter="true" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                    </div>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                             No records have been added yet.
                        </div>
                        <%--<div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
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
                        </div>--%>
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
                <tr runat="server" id="trAddGraphs">
                    <td>
                    </td>
                    <td align="left">
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <%--<div runat="server" id="divSave">
                                            <asp:LinkButton runat="server" ID="lnkOk" CssClass="btn" CausesValidation="true"
                                                OnClick="lnkOk_Click"> <strong>Add</strong></asp:LinkButton>
                                        </div>--%>
                                        <asp:Button runat="server" ID="btnOk" OnClick="lnkOk_Click" style="display:none;" />
                                    </td>
                                    <td>
                                        <div>
                                            <asp:LinkButton runat="server" ID="hlBack" CssClass="btn" OnClientClick="GetBack();return false;"> <strong>Cancel</strong>   </asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
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
