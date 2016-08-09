<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TableGroupDetail.aspx.cs" EnableEventValidation="false" Inherits="Pages_Record_MenuDetail" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <link href="<%=ResolveUrl("~/Styles/jquery-ui-1.7.3.custom.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-ui-1.9.2.custom.min.js")%>"></script>
    <style type="text/css">
        .sortHandle
        {
            cursor: move;
        }
        
        .cssplaceholder
        {
            border-top: 2px solid #00FFFF;
            border-bottom: 2px solid #00FFFF;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function abc() {
            var b = document.getElementById('<%= lnkSave.ClientID %>');
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
    <%--<script type="text/javascript" language="javascript">
        function MouseEvents(objRef, evt) {
            var checkbox = objRef.getElementsByTagName("input")[0];



            if (evt.type == "mouseover") {
                objRef.style.backgroundColor = "#76BAF2";
                objRef.style.cursor = 'pointer';
            }
            else {

                if (checkbox != null && checkbox.checked) {
                    objRef.style.backgroundColor = "#96FFFF";
                }
                else if (evt.type == "mouseout") {
                    if (objRef.rowIndex % 2 == 0) {
                        //Alternating Row Color
                        objRef.style.backgroundColor = "white";
                    }
                    else {
                        objRef.style.backgroundColor = "#DCF2F0";
                    }
                }
            }
        }



    </script>--%>
    <%--<script type="text/javascript">
        var fixHelper = function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });

            return ui;
        };


        $(document).ready(function () {

            $(function () {
                $("#ctl00_HomeContentPlaceHolder_UpdatePanel1").sortable({
                    items: '.gridview_row',
                    cursor: 'crosshair',
                    helper: fixHelper,
                    cursorAt: { left: 10, top: 10 },
                    connectWith: '#ctl00_HomeContentPlaceHolder_UpdatePanel1',
                    handle: '.sortHandle',
                    axis: 'y',
                    distance: 15,
                    dropOnEmpty: true,
                    receive: function (e, ui) {
                        $(this).find("tbody").append(ui.item);

                    },
                    start: function (e, ui) {
                        ui.placeholder.css("border-top", "2px solid #00FFFF");
                        ui.placeholder.css("border-bottom", "2px solid #00FFFF");

                    },
                    update: function (event, ui) {
                        var MT = '';
                        $(".TableID").each(function () {
                            MT = MT + this.value.toString() + ',';
                        });

                        document.getElementById("hfOrderMT").value = MT;
                        $("#btnOrderMT").trigger("click");

                    }
                });
            });

        });
    </script>--%>




    <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
        <div runat="server" id="div1" onkeypress="abc();">
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <asp:Label runat="server" ID="lblTitle" CssClass="TopTitle"></asp:Label>
                                </td>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <asp:HyperLink runat="server" ID="hlBack">
                                                                        <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"
                                                                            ToolTip="Back" />
                                                                    </asp:HyperLink>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divDelete">
                                                                    <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="javascript:return confirm('Are you sure you want to delete this Record?')"
                                                                        CausesValidation="false" OnClick="lnkDelete_Click">
                                                                        <asp:Image runat="server" ID="Image3" ImageUrl="~/App_Themes/Default/images/delete_big.png"
                                                                            ToolTip="Delete" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                                <div runat="server" id="divUnDelete">
                                                                    <asp:LinkButton runat="server" ID="lnkUnDelete" OnClientClick="javascript:return confirm('Are you sure you want to restore this Record?')"
                                                                        CausesValidation="false" OnClick="lnkUnDelete_Click">
                                                                        <asp:Image runat="server" ID="Image4" ImageUrl="~/App_Themes/Default/images/Restore_Big.png"
                                                                            ToolTip="Restore" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divEdit" visible="false">
                                                                    <asp:HyperLink runat="server" ID="hlEditLink">
                                                                        <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/Edit_big.png"
                                                                            ToolTip="Edit" />
                                                                    </asp:HyperLink>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div runat="server" id="divSave">
                                                                    <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="true">
                                                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                                            ToolTip="Save" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red" Font-Size="12px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div style="width: 40px; height: 40px;">
                            <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <table style="width: 100%; text-align: center">
                                        <tr>
                                            <td>
                                                <asp:Image ID="Image1" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                                            </td>
                                        </tr>
                                    </table>
                                </ProgressTemplate>
                            </asp:UpdateProgress>--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                    </td>
                    <td valign="top" width="902">
                        <div id="search" style="padding-bottom: 10px">
                        </div>
                        <div runat="server" id="divDetail">
                            <table>

                                <tr>
                                    <td></td>
                                    <td>
                                           <asp:RadioButtonList runat="server" ID="optMenuType" 
                                                                RepeatDirection="Horizontal" AutoPostBack="true" 
                                                                onselectedindexchanged="optMenuType_SelectedIndexChanged">
                                                                <asp:ListItem Text="Menu" Value="m" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Divider" Value="d"></asp:ListItem>
                                                                <asp:ListItem Text="Custom Page" Value="c"></asp:ListItem>
                                                                 
                                                            </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr runat="server" id="trMenu">
                                    <td align="right">
                                        <strong>Menu*:</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMenu" runat="server" Width="200px" CssClass="NormalTextBox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvMenu" runat="server" ControlToValidate="txtMenu"
                                            ErrorMessage="Menu - Required"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr runat="server" visible="false" id="trCustomPageLink">
                                    <td align="right">
                                        <strong>Custom Page Link*:</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustomPageLink" runat="server" Width="400px" CssClass="NormalTextBox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCustomPageLink" runat="server" ControlToValidate="txtCustomPageLink"
                                            ErrorMessage="Hyperlink - Required"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <strong>Show Under:</strong>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlShowUnder" 
                                         CssClass="NormalTextBox" ></asp:DropDownList>
                                    </td>
                                </tr>

                                <tr runat="server" visible="false">
                                    <td align="right">
                                        <strong>Show on Menu</strong>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkShowOnMenu" runat="server" Checked="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div runat="server" id="divTable">
                                    <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                        HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="true"
                                        AllowSorting="True" DataKeyNames="TableID" HeaderStyle-ForeColor="Black" Width="100%"
                                        AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting" OnPreRender="gvTheGrid_PreRender"
                                        OnRowDataBound="gvTheGrid_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                                        <PagerSettings Position="Top" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="LblID" runat="server" Text='<%# Eval("TableID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="sortHandle">
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                <ItemTemplate>
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/MoveIcon.png"
                                                        ToolTip="Drag and drop to change order" />
                                                    <input type="hidden" id='hfMenuID' value='<%# Eval("TableID") %>' class='TableID' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Table">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("TableID").ToString())  %>'
                                                        Text='<%# Eval("TableName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="gridview_header" />
                                        <RowStyle CssClass="gridview_row" />
                                        <PagerTemplate>
                                            <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" TableName="Table"
                                                HideAdd="true" HideExport="true" HideEdit="true" HideFilter="true" OnApplyFilter="Pager_OnApplyFilter"
                                                OnBindTheGridToExport="Pager_BindTheGridToExport" OnBindTheGridAgain="Pager_BindTheGridAgain"
                                                OnExportForCSV="Pager_OnExportForCSV" />
                                        </PagerTemplate>
                                    </dbg:dbgGridView>
                                    <asp:HiddenField runat="server" ID="hfOrderMT" ClientIDMode="Static" />
                                    <asp:Button runat="server" ID="btnOrderMT" ClientIDMode="Static" Style="display: none;"
                                        OnClick="btnOrderMT_Click" />
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
