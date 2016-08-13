<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ExportTemplateItem.aspx.cs" Inherits="Pages_Export_ExportTemplateItem" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <style type="text/css">
        .sortHandleVT
        {
            cursor: move;
        }
        
        .cssplaceholder
        {
            border-top: 2px solid #00FFFF;
            border-bottom: 2px solid #00FFFF;
        }
    </style>
    <link href="<%=ResolveUrl("~/Styles/jquery-ui-1.7.3.custom.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-ui-1.9.2.custom.min.js")%>"></script>
    <%--<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" />--%>
    <%--<script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>
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
    <script language="javascript" type="text/javascript">

        var fixHelper = function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });

            return ui;
        };

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes                       
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes                        
                        inputList[i].checked = false;
                    }
                }
            }
        }

        function SelectAllCheckboxes(spanChk) {
            checkAll(spanChk);
            // Added as ASPX uses SPAN for checkbox

            var oItem = spanChk.children;

            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];

            xState = theBox.checked;

            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)

                if (elm[i].type == "checkbox" && elm[i].id != theBox.id && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkIsActive' && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkShowOnlyWarning' && elm[i].id != 'ctl00_HomeContentPlaceHolder_rlOne_chkShowAdvancedOptions') {

                    //elm[i].click();

                    if (elm[i].checked != xState)

                        elm[i].click();

                    //elm[i].checked=xState;

                }

        }


    </script>
    <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <table style="width: 100%; text-align: center">
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                    </td>
                </tr>
            </table>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <span class="TopTitle">
                                        <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
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
                                                            <%-- <td>
                                                                <div runat="server" id="divRefresh" visible="false">
                                                                    <asp:LinkButton runat="server" ID="lnkRefesh" OnClick="lnkRefesh_Click" CausesValidation="false">
                                                                        
                                                                       <asp:Image runat="server" ID="Image3" ImageUrl="~/App_Themes/Default/images/Refresh2.png"
                                                                            ToolTip="Refresh all fields with current export names" />
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
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
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">
                                    <tr>
                                        <td align="right" style="width: 200px;">
                                            <strong runat="server" id="stgTableCap">Table*</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                CssClass="NormalTextBox" Width="250px" >
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTable"
                                                ErrorMessage="Table - Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 200px;">
                                            <strong>Template Name*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExportTemplateName" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvExportTemplateName" runat="server" ControlToValidate="txtExportTemplateName"
                                                ErrorMessage="Export Template Name - Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                            <div style="padding-left: 20px; padding-top: 10px;" id="divExportTemplateItemSingleInstance">
                                                    <table>
                                                        <tr>
                                                            <td align="right" style="padding-right:20px;">
                                                                 <asp:LinkButton runat="server" ID="lnkRefesh" OnClick="lnkRefesh_Click" CausesValidation="false" Visible="false">
                                                                        Reset Order                                                                      
                                                                    </asp:LinkButton>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:HiddenField runat="server" ID="hfExportTemplateItemIDForColumnIndex" ClientIDMode="Static" />
                                                                <asp:Button runat="server" ID="btnRefreshExportTemplateItem" ClientIDMode="Static"
                                                                    Style="display: none;" OnClick="btnRefreshExportTemplateItem_Click" />
                                                                <asp:Button runat="server" ID="btnExportTemplateItemIDForColumnIndex" ClientIDMode="Static"
                                                                    Style="display: none;" OnClick="btnExportTemplateItemIDForColumnIndex_Click" />
                                                                <dbg:dbgGridView ID="grdExportTemplateItem" AllowPaging="True" runat="server" AutoGenerateColumns="false"
                                                                    DataKeyNames="ExportTemplateItemID" HeaderStyle-HorizontalAlign="Center" PageSize="500"
                                                                    RowStyle-HorizontalAlign="Center" CssClass="gridview" OnRowCommand="grdExportTemplateItem_RowCommand"
                                                                    OnRowDataBound="grdExportTemplateItem_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0"
                                                                    Width="750px">
                                                                    <PagerSettings Position="Top" />
                                                                    <RowStyle CssClass="gridview_row" />
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                            <HeaderTemplate>
                                                                                <input id="chkAll" onclick="javascript: SelectAllCheckboxes(this);" runat="server"
                                                                                    type="checkbox" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="false">
                                                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("ExportTemplateItemID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-CssClass="sortHandleVT">
                                                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/MoveIcon.png"
                                                                                    ToolTip="Drag and drop to change order" />
                                                                                <input type="hidden" id='hfExportTemplateItemID' value='<%# Eval("ExportTemplateItemID") %>'
                                                                                    class='ExportTemplateItemID' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Field Name">
                                                                            <ItemTemplate>
                                                                                <div style="padding-left: 10px;">
                                                                                    <asp:Label runat="server" ID="lblFieldName"></asp:Label>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Export Header">
                                                                            <ItemTemplate>
                                                                                <div style="padding-left: 10px;">
                                                                                    <asp:TextBox runat="server" ID="txtHeading" CssClass="NormalTextBox" Width="400px"></asp:TextBox>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="gridview_header" />
                                                                    <PagerTemplate>
                                                                        <asp:GridViewPager runat="server" ID="ExportTemplateItemPager" OnDeleteAction="ExportTemplateItemPager_DeleteAction"
                                                                            HideFilter="true" HidePageSizeButton="true"  DelConfirmation="Are you sure you want to remove selected field(s)?" 
                                                                            HideGo="true" HideNavigation="true" HideRefresh="true" HideExport="true" OnBindTheGridAgain="ExportTemplateItemPager_BindTheGridAgain" />
                                                                    </PagerTemplate>
                                                                </dbg:dbgGridView>

                                                            </td>
                                                        </tr>
                                                    </table>
                                              
                                            </div>
                                            <br />
                                            <div runat="server" id="divEmptyAddExportTemplateItem" visible="false" style="padding-left: 20px;">
                                                <asp:HyperLink runat="server" ID="hlAddExportTemplateItem" Style="text-decoration: none;
                                                    color: Black;" CssClass="popuplinkVT">
                                                    <asp:Image runat="server" ID="Image8" ImageUrl="~/App_Themes/Default/images/add-remove.png" />
                                                    No items have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                                        Add new item now.</strong>
                                                </asp:HyperLink>
                                            </div>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>


                               <asp:HyperLink ID="hlAddExportTemplateItem2" ClientIDMode="Static" runat="server" NavigateUrl="~/DemoTips.aspx"
                                    Style="display: none;"  CssClass="popuplinkVT"></asp:HyperLink>
                            <br />
                        </asp:Panel>
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
            <asp:AsyncPostBackTrigger ControlID="grdExportTemplateItem" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function OpenExportTemplateItem() {
            $("#ctl00_HomeContentPlaceHolder_hlAddExportTemplateItem").trigger('click');
        }
    </script>
</asp:Content>
