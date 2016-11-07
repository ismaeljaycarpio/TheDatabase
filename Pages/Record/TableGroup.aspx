<%@ Page Language="C#" MasterPageFile="~/Home/Home.master" CodeFile="TableGroup.aspx.cs"
    Inherits="Record_Menu" EnableEventValidation="false" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <link href="<%=ResolveUrl("~/Styles/jquery-ui-1.7.3.custom.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-ui-1.9.2.custom.min.js")%>"></script>

    <%-- <link href="<%=ResolveUrl("~/Styles/jquery-ui-1.11.3.min.css")%>" rel="stylesheet"
        type="text/css" />
      <script type="text/javascript" src="<%=ResolveUrl("~/Styles/jquery-ui-1.11.3.min.js")%>"></script> --%>


    <style type="text/css">
        .sortHandle {
            cursor: move;
        }

        .cssplaceholder {
            border-top: 2px solid #00FFFF;
            border-bottom: 2px solid #00FFFF;
        }
    </style>

    <script type="text/javascript">
        function MouseEvents(objRef, evt) {


            if (evt.type == "mouseover") {
                objRef.style.backgroundColor = "#76BAF2";
                objRef.style.cursor = 'pointer';
            }
            else {

                if (evt.type == "mouseout") {
                    if (objRef.rowIndex % 2 == 0) {
                        //Alternating Row Color
                        objRef.style.backgroundColor = "#DCF2F0";
                    }
                    else {

                        objRef.style.backgroundColor = "white";
                    }
                }
            }
        }



    </script>

    <script type="text/javascript">
       <%-- function abc() {
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

        }--%>

        var fixHelper = function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });

            return ui;
        };


        $(document).ready(function () {

            $(function () {
                $("#ctl00_HomeContentPlaceHolder_upTheGrid").sortable({
                    items: '.gridview_row',
                    cursor: 'crosshair',
                    helper: fixHelper,
                    cursorAt: { left: 10, top: 10 },
                    connectWith: '#ctl00_HomeContentPlaceHolder_upTheGrid',
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
                        $(".MenuID").each(function () {
                            MT = MT + this.value.toString() + ',';
                        });

                        document.getElementById("hfOrderMT").value = MT;
                        $("#btnOrderMT").trigger("click");

                    }
                });
            });

        });


        //function SetMenu(iMenuID) {
        //    document.getElementById('hfParentMenuID').value = iMenuID;
        //    document.getElementById('ctl00_HomeContentPlaceHolder_lnkSearch').click();
        //}
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">Menus</span>
                        </td>
                        <td align="left">
                            <div style="width: 40px; height: 40px;">
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
        <%--<tr>
            <td colspan="3" height="13"></td>
        </tr>--%>
        <tr>
            <td valign="top"></td>
            <td valign="top">
                <asp:Panel ID="Panel2" runat="server">
                    <div  >
                        <br />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                            HeaderText="List of validation errors" ShowSummary="false" />
                          <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                                </ContentTemplate>
                              </asp:UpdatePanel>
                        <table style="border-collapse: collapse;min-width:900px" cellpadding="4">
                            <tr runat="server" id="trMenuSearch" visible="false">
                                <td align="right">
                                    <strong>Menu</strong>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMenuSearch" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <div>
                                        <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                            <tr runat="server" id="trEditPart">
                                <td colspan="4">
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table style="min-width: 900px;">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" style="width: 70%;">
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
                                                                                                <%--<td>
                                                                                            <div runat="server" id="divEdit" visible="false">
                                                                                                <asp:HyperLink runat="server" ID="hlEditLink">
                                                                        <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/Edit_big.png"
                                                                            ToolTip="Edit" />
                                                                                                </asp:HyperLink>
                                                                                            </div>
                                                                                        </td>--%>
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
                                                        <td>

                                                            <div runat="server" id="divDetail" class="searchcorner" style="padding: 15px;padding-left:50px;">
                                                                <table>

                                                                    <tr>
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:RadioButtonList runat="server" ID="optMenuType"
                                                                                RepeatDirection="Horizontal" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="optMenuType_SelectedIndexChanged">
                                                                                <asp:ListItem Text="Menu" Value="m" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="Table" Value="t"></asp:ListItem>
                                                                                 <asp:ListItem Text="Documents" Value="doc"></asp:ListItem>
                                                                                <asp:ListItem Text="Report" Value="r"></asp:ListItem>
                                                                                <asp:ListItem Text="Divider" Value="d"></asp:ListItem>
                                                                                <asp:ListItem Text="Link" Value="l"></asp:ListItem>
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
                                                                   
                                                                    <tr runat="server" visible="false" id="trExternalPageLink">
                                                                        <td align="right">
                                                                            <strong>Link*:</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtExternalPageLink" runat="server" Width="600px" CssClass="NormalTextBox"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvExternalPageLink" runat="server" ControlToValidate="txtExternalPageLink"
                                                                                ErrorMessage="Hyperlink - Required"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <strong runat="server" id="stgShowUnder">Show Under:</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlShowUnder"
                                                                                CssClass="NormalTextBox">
                                                                            </asp:DropDownList>
                                                                             <asp:RequiredFieldValidator ID="rfvShowUnder" runat="server" ControlToValidate="ddlShowUnder" Enabled="false"
                                                                                CssClass="failureNotification" ErrorMessage="Show Under - Required." ToolTip="Show Under - Required."></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                     <tr runat="server" id="trDocumentType" visible="false">
                                                                        <td align="right">
                                                                            <strong>Category:</strong>
                                                                        </td>
                                                                        <td style="padding-left: 3PX;">
                                                                            <asp:DropDownList runat="server" ID="ddlDocumentType" CssClass="NormalTextBox" 
                                                                                ClientIDMode="Static" DataTextField="DocumentTypeName" DataValueField="DocumentTypeID">
                                                                            </asp:DropDownList>

                                                                            <%--<asp:RequiredFieldValidator ID="rfvDocumentType" runat="server" ControlToValidate="ddlDocumentType"
                                                                                CssClass="failureNotification" ErrorMessage="Category - Required." ToolTip="Category - Required."></asp:RequiredFieldValidator>--%>

                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trTable" visible="false">
                                                                        <td align="right" style="width: 120px;">
                                                                            <strong runat="server" id="stgTableCap">Table*:</strong>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="ddlTable" runat="server" CssClass="NormalTextBox"
                                                                                DataTextField="TableName" DataValueField="TableID" >
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvTable" runat="server" ControlToValidate="ddlTable"
                                                                                CssClass="failureNotification" ErrorMessage="Table - Required." ToolTip="Table - Required."></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        
                                                                    </tr>
                                                                    <tr runat="server" id="trReport" visible="false">
                                                                        <td align="right">
                                                                            <strong>Report*:</strong>
                                                                        </td>
                                                                        <td style="padding-left: 3PX;">
                                                                            <asp:DropDownList runat="server" ID="ddlReports" CssClass="NormalTextBox" 
                                                                                ClientIDMode="Static" DataTextField="DocumentText" DataValueField="DocumentID">
                                                                            </asp:DropDownList>

                                                                            <asp:RequiredFieldValidator ID="rfvReports" runat="server" ControlToValidate="ddlReports"
                                                                                CssClass="failureNotification" ErrorMessage="Report - Required." ToolTip="Report - Required."></asp:RequiredFieldValidator>

                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="trOpenInNewWindow" visible="false">
                                                                        <td align="right"></td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkOpenInNewWindow" runat="server"  TextAlign="Right"
                                                                                Text="Open in new window" Font-Bold="true" />
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

                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label runat="server" ID="lblCurrentMenu" Text="<a href='TableGroup.aspx'>Top Level</a>/"></asp:Label>
                                    <asp:HiddenField runat="server" ID="hfParentMenuID" Value="-1" ClientIDMode="Static" />
                                </td>
                                <td align="right">
                                    <asp:CheckBox ID="chkIsActive" Checked="false" runat="server" AutoPostBack="true"
                                        Text="Show Deleted Records" OnCheckedChanged="chkIsActive_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel ID="upTheGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div>
                                                <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                                    HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="false"
                                                    AllowSorting="True" DataKeyNames="MenuID" HeaderStyle-ForeColor="Black" Width="100%"
                                                    AutoGenerateColumns="false" PageSize="1000" OnSorting="gvTheGrid_Sorting" OnPreRender="gvTheGrid_PreRender"
                                                    OnRowDataBound="gvTheGrid_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                                                    <PagerSettings Position="Top" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("MenuID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="sortHandle">
                                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/MoveIcon.png"
                                                                    ToolTip="Drag and drop to change order" />
                                                                <input type="hidden" id='hfMenuID' value='<%# Eval("MenuID") %>' class='MenuID' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                <asp:HyperLink ID="hlAdd" runat="server" ToolTip="Add" NavigateUrl='<%# GetAddURL() %>'
                                                                    ImageUrl="~/Pages/Pager/Images/add.png" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() +  Cryptography.Encrypt(Eval("MenuID").ToString()) %>'
                                                                    ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Menu">
                                                            <ItemTemplate>
                                                                
                                                                <asp:Label ID="lblMenuP" runat="server" Text='<%# Eval("Menu")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type">

                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Table">
                                                            <ItemTemplate>                                                                
                                                                <asp:Label ID="lblTable" runat="server" Text='<%# Eval("TableName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridview_header" />
                                                    <RowStyle CssClass="gridview_row" />
                                                    <PagerTemplate>
                                                        <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" OnExportForCSV="Pager_OnExportForCSV"
                                                            OnDeleteAction="Pager_DeleteAction" OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                                            OnBindTheGridAgain="Pager_BindTheGridAgain" />
                                                    </PagerTemplate>
                                                </dbg:dbgGridView>
                                            </div>
                                            <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                                                <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                    Add new record now.</strong>
                                                </asp:HyperLink>
                                            </div>
                                            <%--<div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                                                <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                                    OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png" />
                                No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                    Clear filter</strong>
                                                </asp:LinkButton>
                                                or
                            <asp:HyperLink runat="server" ID="hplNewDataFilter" Style="text-decoration: none; color: Black;">                                
                                  <strong style="text-decoration: underline; color: Blue;">
                                     Add new record.</strong>
                            </asp:HyperLink>
                                            </div>--%>
                                            <br />
                                            <%--<asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>--%>
                                            <asp:HiddenField runat="server" ID="hfOrderMT" ClientIDMode="Static" />
                                            <asp:Button runat="server" ID="btnOrderMT" ClientIDMode="Static" Style="display: none;"
                                                OnClick="btnOrderMT_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </td>

                            </tr>
                        </table>

                    </div>
                </asp:Panel>

            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="3" height="13"></td>
        </tr>
    </table>
</asp:Content>
