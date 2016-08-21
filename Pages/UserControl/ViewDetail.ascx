<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ViewDetail.ascx.cs" Inherits="Pages_UserControl_ViewDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%--<%@ Register Src="~/Pages/UserControl/ControlByColumnValue.ascx" TagName="cbcValue"
    TagPrefix="dbg" %>--%>
<%@ Register Src="~/Pages/UserControl/ControlByColumn.ascx" TagName="ControlByColumn"
    TagPrefix="dbg" %>

<style type="text/css">
    .sortHandleVT {
        cursor: move;
    }

    .cssplaceholder {
        border-top: 2px solid #00FFFF;
        border-bottom: 2px solid #00FFFF;
    }
</style>

<script type="text/javascript">
    function toggleAndOr(t, hf) {
        // alert(hf);

        if (t.text == "and") {
            t.text = "or";
        } else {
            t.text = "and";
        }
        document.getElementById(hf).value = t.text;


    }


</script>

<div>
    <asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upViewItem">
        <ProgressTemplate>
            <table style="width: 100%; height: 100%; text-align: center;">
                <tr valign="middle">
                    <td>
                        <p style="font-weight: bold;">Please wait...</p>
                        <asp:Image ID="ImageProcessing" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                    </td>
                </tr>
            </table>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="upViewItem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="padding-left: 20px; padding-top: 10px;">
                <table>
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Label runat="server" ID="lblViewTitle" Text="Edit View" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <asp:HiddenField runat="server" ID="hfCurrentViewID" Value="" />
                            <asp:HiddenField runat="server" ID="hfCurrentViewRowIndex" Value="" />
                            <asp:HiddenField runat="server" ID="hfTextSearch" />
                            <asp:HiddenField runat="server" ID="hfIsDanamic" Value="no" />
                            <br />
                            <asp:Label runat="server" ID="lblMsg"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2" align="left">
                            <table >
                                <tr>
                                    <td>

                                        <asp:LinkButton runat="server" ID="lnkBack"
                                            OnClientClick="parent.$.fancybox.close();return false;" OnClick="lnkShowView_Click">
                                                            <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"
                                                                ToolTip="Back" />
                                        </asp:LinkButton>


                                    </td>
                                    <td  style="padding-left:10px;">
                                        <%--<asp:LinkButton runat="server" ID="lnkResetDefault" OnClick="lnkResetDefault_Click" CausesValidation="true">
                                                        Reset
                                        </asp:LinkButton>--%>
                                          <asp:HyperLink ID="hlSaveDefault" ClientIDMode="Static" runat="server"  CssClass="popupsavedefault">Reset</asp:HyperLink>
                                    </td>
                                    <td style="padding-left:10px;">
                                        <asp:LinkButton runat="server" ID="lnkAddView" OnClick="lnkAddView_Click" CausesValidation="true"
                                            Text="Add"> </asp:LinkButton>
                                    </td>
                                    <td style="padding-left:10px;">
                                        <asp:LinkButton runat="server" ID="lnlDeleteView" OnClick="lnlDeleteView_Click" CausesValidation="true"
                                            Text="Delete"> </asp:LinkButton>
                                    </td>
                                    
                                     <td style="padding-left:10px;">
                                        <asp:LinkButton runat="server" ID="lnkSaveView" OnClick="lnkSaveView_Click" CausesValidation="true">
                                            <asp:Image runat="server" ID="imgSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                ToolTip="Save View" />
                                        </asp:LinkButton>
                                    </td>

                                  
                                    <td style="padding-left:10px;">
                                        <asp:LinkButton runat="server" ID="lnkNavigatePrev" OnClick="lnkNavigatePrev_Click">
                                            <asp:Image ID="Image14" runat="server" ImageUrl="~/App_Themes/Default/Images/bullet_arrow_left.png"
                                                ToolTip="Previous Record" />
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkNavigateNext" OnClick="lnkNavigateNext_Click">
                                            <asp:Image ID="Image15" runat="server" ImageUrl="~/App_Themes/Default/Images/bullet_arrow_right.png"
                                                ToolTip="Next Record" />
                                        </asp:LinkButton>
                                    </td>
                                </tr>

                                <%--<tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                     <td>
                                    </td>
                                    <td>
                                         <asp:LinkButton runat="server" ID="lnkShowView" OnClick="lnkShowView_Click" CausesValidation="true"
                                            Text="Show" Font-Bold="true"> </asp:LinkButton>
                                        
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>--%>
                            </table>
                        </td>
                    </tr>
                    <%--<tr runat="server" id="trResetViews" visible="false">
                        <td colspan="3" style="padding-left: 200px;">
                            <strong style="color: Red;">Do you want to replace existing views for all user with this one?</strong>
                            <br />
                            <br />
                            <div style="padding-left: 200px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkOK" OnClick="lnkOK_Click" CssClass="btn">
                                           <strong>Yes</strong>
                                            </asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkNo" OnClick="lnkNo_Click" CssClass="btn">
                                           <strong>No</strong>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </td>
                    </tr>--%>
                    <tr>
                        <td></td>
                        <td style="width: 50px;"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td align="right">
                                        <strong>View Title</strong>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox runat="server" ID="txtViewName" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr runat="server" visible="false">
                                    <td align="right">
                                        <strong>User</strong>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList runat="server" ID="ddlViewUser" DataTextField="UserName" DataValueField="UserID"
                                            CssClass="NormalTextBox">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr >
                                    <td align="right">
                                        <strong>Rows per page</strong>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox runat="server" ID="txtRowsPerPage" CssClass="NormalTextBox" Width="50px" MaxLength="3"></asp:TextBox>
                                         <asp:RegularExpressionValidator ID="revRowsPerPage" ControlToValidate="txtRowsPerPage"
                                            runat="server" ErrorMessage="Numeric." Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                        </asp:RegularExpressionValidator>
                                        <asp:RangeValidator ID="rngRowsPerPage" runat="server" ControlToValidate="txtRowsPerPage"
                                             ErrorMessage="Must be between 5 and 100" Font-Bold="true" Display="Dynamic" Type="Integer"
                                            MinimumValue="5" MaximumValue="100"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <strong>Sort Order</strong>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList runat="server" CssClass="NormalTextBox" ID="ddlViewSortOrder" DataValueField="ColumnID"
                                            DataTextField="DisplayName">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <strong>Sort Order Direction</strong>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList runat="server" ID="rbSortOrderDirection" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="ASC" Text="Ascending"></asp:ListItem>
                                            <asp:ListItem Value="DESC" Text="Desending"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr runat="server" visible="false">
                                    <td align="right" valign="top">
                                        <strong>Page Type</strong>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList runat="server" CssClass="NormalTextBox" ID="ddlViewPageType" Enabled="false">
                                            <asp:ListItem Text="--Please Select--" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Main Summary" Value="list"></asp:ListItem>
                                            <asp:ListItem Text="Dashboard" Value="dash"></asp:ListItem>
                                            <asp:ListItem Text="Child Summary" Value="child"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"></td>
                                    <td align="left"></td>
                                </tr>
                            </table>
                        </td>
                        <td></td>
                        <td>

                            <table>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:CheckBox runat="server" ID="chkShowSearchFields" TextAlign="Left" Font-Bold="true"
                                            Text="Show Search Fields" Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:CheckBox runat="server" ID="chkViewAddIcon" TextAlign="Left" Font-Bold="true"
                                            Text="Add Icon" Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:CheckBox runat="server" ID="chkViewEditIcon" TextAlign="Left" Font-Bold="true"
                                            Text="Edit Icon" Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:CheckBox runat="server" ID="chkViewDeleteIcon" TextAlign="Left" Font-Bold="true"
                                            Text="Delete Icon" Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:CheckBox runat="server" ID="chkViewIcon" TextAlign="Left" Font-Bold="true" Text="View Icon"
                                            Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:CheckBox runat="server" ID="chkViewBulkEditIcon" TextAlign="Left" Font-Bold="true"
                                            Text="Bulk Edit Icon" Checked="true" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:CheckBox runat="server" ID="chkShowFixedHeader" TextAlign="Left" Font-Bold="true"
                                            Text="Show Fixed Header"  />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>

                        <td colspan="3">
                            <table>
                                <tr>
                                    <td style="width: 130px;"></td>
                                    <td align="right">
                                        <strong>Filter</strong>
                                    </td>
                                    <td>
                                        <table>
                                            <td>
                                                <dbg:ControlByColumn runat="server" ID="cbcSearchMain" OnddlYAxis_Changed="cbcSearchMain_OnddlYAxis_Changed" />
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkAddSearch1">
                                                                <asp:Image ID="Image9" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                </asp:LinkButton>
                                            </td>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="trSearch1" style="display: none;">
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkMinusSearch1">
                                                        <asp:Image ID="Image10" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                        </asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <asp:HiddenField runat="server" ID="hfAndOr1" Value="" />
                                        <asp:LinkButton runat="server" ID="lnkAndOr1" Text="and"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <table>
                                            <td>
                                                <dbg:ControlByColumn runat="server" ID="cbcSearch1" OnddlYAxis_Changed="cbcSearch1_OnddlYAxis_Changed" />
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkAddSearch2">
                                                                <asp:Image ID="Image11" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                </asp:LinkButton>
                                            </td>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="trSearch2" style="display: none;">
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkMinusSearch2">
                                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                        </asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <asp:HiddenField runat="server" ID="hfAndOr2" Value="" />
                                        <asp:LinkButton runat="server" ID="lnkAndOr2">and</asp:LinkButton>
                                    </td>
                                    <td>
                                        <table>
                                            <td>
                                                <dbg:ControlByColumn runat="server" ID="cbcSearch2" OnddlYAxis_Changed="cbcSearch2_OnddlYAxis_Changed" />
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkAddSearch3" Style="display: none;">
                                                                <asp:Image ID="Image12" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                </asp:LinkButton>
                                            </td>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="trSearch3" style="display: none;">
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkMinusSearch3">
                                                        <asp:Image ID="Image13" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                        </asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <asp:HiddenField runat="server" ID="hfAndOr3" Value="" />
                                        <asp:LinkButton runat="server" ID="lnkAndOr3">and</asp:LinkButton>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <dbg:ControlByColumn runat="server" ID="cbcSearch3" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div style="padding-left: 20px; padding-top: 10px;" id="divViewItemSingleInstance">
                <asp:HiddenField runat="server" ID="hfViewItemIDForColumnIndex" ClientIDMode="Static" />
                <asp:Button runat="server" ID="btnRefreshViewItem" ClientIDMode="Static" Style="display: none;"
                    OnClick="btnRefreshViewItem_Click" />

                <asp:Button runat="server" ID="btnViewItemIDForColumnIndex" ClientIDMode="Static" Style="display: none;"
                    OnClick="btnViewItemIDForColumnIndex_Click" />
                <dbg:dbgGridView ID="grdViewItem" AllowPaging="True" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="ViewItemID" HeaderStyle-HorizontalAlign="Center" PageSize="500"
                    RowStyle-HorizontalAlign="Center" CssClass="gridview" OnRowCommand="grdViewItem_RowCommand"
                    OnRowDataBound="grdViewItem_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0"
                    Width="750px">
                    <PagerSettings Position="Top" />
                    <RowStyle CssClass="gridview_row" />
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
                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("ViewItemID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="sortHandleVT">
                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                            <ItemTemplate>
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/MoveIcon.png"
                                    ToolTip="Drag and drop to change order" />
                                <input type="hidden" id='hfViewItemID' value='<%# Eval("ViewItemID") %>' class='ViewItemID' />
                            </ItemTemplate>
                            <%--<HeaderTemplate>
                                    <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/ChildTableDetail.aspx"
                                        ImageUrl="~/Pages/Pager/Images/add-remove.png" ToolTip="Items" ID="hlAddDetail"></asp:HyperLink>
                             </HeaderTemplate>--%>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Field Name">
                            <ItemTemplate>
                                <div style="padding-left: 10px;">
                                    <asp:Label runat="server" ID="lblFieldName"></asp:Label>

                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Heading">
                            <ItemTemplate>
                                <div style="padding-left: 10px;">

                                    <asp:TextBox runat="server" ID="txtHeading" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Search Field">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <div style="padding-left: 10px;">
                                    <asp:CheckBox runat="server" ID="chkSearchField" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filter Field" Visible="false">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <div style="padding-left: 10px;">
                                    <asp:CheckBox runat="server" ID="chkFilterField" Checked="true" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alignment">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <div style="padding-left: 10px;">
                                    <asp:DropDownList runat="server" ID="ddlAlignment" CssClass="NormalTextBox">
                                        <asp:ListItem Value="left" Text="Left" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="right" Text="Right"></asp:ListItem>
                                        <asp:ListItem Value="center" Text="Center"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Width">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <div style="padding-left: 10px;">
                                    <asp:TextBox runat="server" ID="txtWidth" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtWidth"
                                        runat="server" ErrorMessage="View Width - Numeric only" Display="None" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Show Total">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <div style="padding-left: 10px;">
                                    <asp:CheckBox runat="server" ID="chkShowTotal" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gridview_header" />
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" ID="ViewItemPager" HideDelete="false" HideFilter="true"
                            HidePageSizeButton="true" HideGo="true" HideNavigation="true" HideRefresh="true"  DelConfirmation="Are you sure you want to remove selected field(s) from this view?" 
                            HideExport="true" OnBindTheGridAgain="ViewItemPager_BindTheGridAgain" OnDeleteAction="ViewItemPager_DeleteAction" />
                    </PagerTemplate>
                </dbg:dbgGridView>
            </div>
            <br />
            <div runat="server" id="divEmptyAddViewItem" visible="false" style="padding-left: 20px;">
                <asp:HyperLink runat="server" ID="hlAddViewItem" Style="text-decoration: none; color: Black;"
                    CssClass="popuplinkVT">
                    <asp:Image runat="server" ID="Image8" ImageUrl="~/App_Themes/Default/images/add-remove.png" />
                    No view items have been added yet. <strong style="text-decoration: underline; color: Blue;">
                        Add new view item now.</strong>
                </asp:HyperLink>
            </div>
            <br />


            <div style="text-align: center">
                <asp:HyperLink runat="server" ID="hlShowThisView" Target="_parent" Visible="false"></asp:HyperLink>
               

                <asp:Button runat="server" ID="btnSaveDefaultOK" ClientIDMode="Static" Style="display: none;" OnClick="lnkOK_Click" />

                <asp:Button runat="server" ID="btnSaveDefaultNo" ClientIDMode="Static" Style="display: none;" OnClick="lnkNo_Click" />

            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grdViewItem" />
          
        </Triggers>


    </asp:UpdatePanel>
    <script type="text/javascript">
        function OpenDefaultConfirm() {
            $('#hlSaveDefault').trigger('click');
        }

    </script>
</div>
