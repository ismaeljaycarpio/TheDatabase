<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TableDetail.aspx.cs" Inherits="Pages_Record_TableDetail" EnableEventValidation="false" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/ControlByColumnValue.ascx" TagName="cbcValue" TagPrefix="dbg" %>

<%@ Register Src="~/Pages/UserControl/ControlByColumn.ascx" TagName="ControlByColumn" TagPrefix="dbg" %>

<%@ Register Src="~/Pages/UserControl/ViewDetail.ascx" TagName="OneView" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/UserControl/ExportTemplate.ascx" TagName="ExpTem" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/UserControl/ImportTemplate.ascx" TagName="ImpTem" TagPrefix="dbg" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <link href="<%=ResolveUrl("~/Styles/jquery-ui-1.7.3.custom.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-ui-1.9.2.custom.min.js")%>"></script>
    <style type="text/css">
        .gridview_header_freeze
        {
            height: 46px;
            background-color: #EDF3F7;
            color: #000000;
            position: relative; /*top: expression(this.parentNode.parentNode.parentNode.scrollTop-1);*/
            z-index: 10;
        }
        
        .sortHandle
        {
            cursor: move;
        }
        .sortHandle2
        {
            cursor: move;
        }


        .sortHandle3
        {
            cursor: move;
        }

      

        .cssplaceholder
        {
            border-top: 2px solid #00FFFF;
            border-bottom: 2px solid #00FFFF;
        }
    </style>
    <script type="text/javascript">


        function SelectAllCheckboxes(spanChk) {
            checkAll(spanChk);
            var GridView = spanChk.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && spanChk != inputList[i]) {
                    if (spanChk.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }

            }
        }

        function MouseEvents(objRef, evt) {
            if (evt.type == "mouseover") {
                objRef.style.backgroundColor = "#76BAF2";
                objRef.style.cursor = 'pointer';
            }
            else {

                if (evt.type == "mouseout") {
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

//        function ShowHistory() {
//            $("#divHistory").fadeIn();
//            $("#lnkShowHistory").fadeOut();
//            $("#lnkHideHistory").fadeIn();

//        }

//        function HideHistory() {
//            $("#divHistory").fadeOut();
//            $("#lnkShowHistory").fadeIn();
//            $("#lnkHideHistory").fadeOut();

//        }

    </script>

    <script type="text/javascript">
        function toggleAndOr(t, hf) {
            // alert(hf);

            if (t.text == "and") {
                t.text = "or";
            } else {
                t.text = "and";
            }
            document.getElementById(hf).value = t.text;
            //alert(document.getElementById(hf).value);

            //            if (t.innerHTML == "and") {
            //                t.innerHTML = "or";
            //            } else {
            //                t.innerHTML = "and";
            //            }

        }
</script>


    <script language="javascript" type="text/javascript">

        function Submit() {

            document.forms["aspnetForm"].submit()
            //        alert('ok');

        }

        function ClientActiveTabChanged() {
            if ($find("ctl00_HomeContentPlaceHolder_TabContainer2").get_activeTabIndex() == 1) {

                ShowHide();
            }

            if ($find("ctl00_HomeContentPlaceHolder_TabContainer2").get_activeTabIndex() == 3) {

                ShowHide();
            }

        };

        var fixHelper = function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });

            return ui;
        };

        function ShowHideCategory(sCatergory,lnk) {

            $('#hfCategory').val(sCatergory);
            ShowHide();
            if ($(".TablLinkClass") != null && lnk != null) {
                $(".TablLinkClass").css('font-weight', 'normal');
            }
            if (lnk != null) {
                lnk.style.fontWeight = 'bold';
            }
        }

        function ShowHideTemplateCategory(sTemplateCategory, lnk) {

            $('#hfTemplateCategory').val(sTemplateCategory);
            ShowHide();
            if ($(".TemplateTablLinkClass") != null && lnk != null) {
                $(".TemplateTablLinkClass").css('font-weight', 'normal');
            }
            if (lnk != null) {
                lnk.style.fontWeight = 'bold';
            }
        }
        
        function OpenTableRenameConfirm() {
            $('#hlTableRename').trigger('click');
            //alert($('#hlTableRename').toString());
        }

   

        function ShowHide() {

            //var sCatergory = $('#ddlCategory').val();

            var sTemplateCategory = $('#hfTemplateCategory').val();

            if (sTemplateCategory == 'import') {
                $("#divTemplateImport").fadeIn();
                $("#divTemplateExport").fadeOut();
                $("#divTemplateWordMerge").fadeOut();               
            }

            if (sTemplateCategory == 'export') {
                $("#divTemplateImport").fadeOut();
                $("#divTemplateExport").fadeIn();
                $("#divTemplateWordMerge").fadeOut();
            }

            if (sTemplateCategory == 'word') {
                $("#divTemplateImport").fadeOut();
                $("#divTemplateExport").fadeOut();
                $("#divTemplateWordMerge").fadeIn();
            }

            var sCatergory = $('#hfCategory').val();
            if (sCatergory == 'display') {
                $("#divDisplay").fadeIn();
                $("#divImportData").fadeOut();
                $("#divAddRecords").fadeOut();
                $("#divColours").fadeOut();
                $("#divGraphs").fadeOut(); 
                $("#divEmails").fadeOut();
            }
            if (sCatergory == 'importdata') {
                $("#divDisplay").fadeOut();
                $("#divImportData").fadeIn();
                $("#divAddRecords").fadeOut();
                $("#divColours").fadeOut();
                $("#divGraphs").fadeOut();
                $("#divEmails").fadeOut();
            }
            if (sCatergory == 'addrecords') {
                $("#divDisplay").fadeOut();
                $("#divImportData").fadeOut();
                $("#divAddRecords").fadeIn();
                $("#divColours").fadeOut();
                $("#divGraphs").fadeOut();
                $("#divEmails").fadeOut();
            }
            if (sCatergory == 'colours') {
                $("#divDisplay").fadeOut();
                $("#divImportData").fadeOut();
                $("#divAddRecords").fadeOut();
                $("#divColours").fadeIn();
                $("#divGraphs").fadeOut();
                $("#divEmails").fadeOut();
            }
            if (sCatergory == 'graphs') {
                $("#divDisplay").fadeOut();
                $("#divImportData").fadeOut();
                $("#divAddRecords").fadeOut();
                $("#divColours").fadeOut();
                $("#divGraphs").fadeIn();
                $("#divEmails").fadeOut();
            }
            if (sCatergory == 'emails') {
                $("#divDisplay").fadeOut();
                $("#divImportData").fadeOut();
                $("#divAddRecords").fadeOut();
                $("#divColours").fadeOut();
                $("#divGraphs").fadeOut();
                $("#divEmails").fadeIn();
            }

            var chkAddUserRecord = document.getElementById("chkAddUserRecord");
            var divAutomaticallyAddUserRecord = document.getElementById("divAutomaticallyAddUserRecord");
            if (chkAddUserRecord != null) {
                if (chkAddUserRecord.checked == true) {
                    $("#divAutomaticallyAddUserRecord").fadeIn();
                }
                else {
                    $("#divAutomaticallyAddUserRecord").fadeOut();
                }
            }



            var chkUniqueRecordedate = document.getElementById("chkUniqueRecordedate");
           // var divUniqueRecord = document.getElementById("divUniqueRecord");
            if (chkUniqueRecordedate != null) {
                if (chkUniqueRecordedate.checked == true) {
                    $("#divUniqueRecord").fadeIn();
                    if ($("[id$='ddlUniqueColumnID'] option:selected").index() === 0)
                        $("#chkDataUpdateUniqueColumnID").prop("disabled", true);
                    else
                        $("#chkDataUpdateUniqueColumnID").prop("disabled", false);
                }
                else {
                    $("#divUniqueRecord").fadeOut();
                    $("#chkDataUpdateUniqueColumnID").prop("disabled", true)
                }
            }


            /*
            var chkDataUpdateUniqueColumnID = document.getElementById("chkDataUpdateUniqueColumnID");
            var divDataUpdateUniqueColumnID = document.getElementById("divDataUpdateUniqueColumnID");
            if (chkDataUpdateUniqueColumnID != null) {
                if (chkDataUpdateUniqueColumnID.checked == true) {
                    $("#divDataUpdateUniqueColumnID").fadeIn();
                }
                else {
                    $("#divDataUpdateUniqueColumnID").fadeOut();
                }
            }
            */


            var strColumnValue = $('#ddlHeaderText').val();
            if (strColumnValue != '') {
                $('#hlDDEdit').fadeOut();
            }
            else {
                $('#hlDDEdit').fadeIn();
            }

        };

        if (window.addEventListener)
            window.addEventListener("load", ShowHide(), false);
        else if (window.attachEvent)
            window.attachEvent("onload", ShowHide());
        else if (document.getElementById)
            window.onload = ShowHide();

        //        $(document).ready(function () {
        //            gridviewScroll();
        //        });

        //        function gridviewScroll() {
        //            $('#ctl00_HomeContentPlaceHolder_TabContainer2_tabField_gvTheGrid').gridviewScroll({
        //                width: 660,
        //                height: 200,
        //                freezesize: 2
        //            });
        //        }





        $(document).ready(function () {


            if (document.getElementById('hlDDEdit') != null)
            {
                document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?headername=yes&formula=' + encodeURIComponent(document.getElementById('hfDisplayColumnsFormula').value) + '&Tableid=' + document.getElementById('ctl00_HomeContentPlaceHolder_hfTableID').value;

            }


            $('#ddlHeaderText').change(function (e) {

                var strColumnValue = $('#ddlHeaderText').val();
                if (document.getElementById('hlDDEdit') != null)
                {
                    if (strColumnValue != '') {
                        $('#hlDDEdit').fadeOut();
                        document.getElementById('hfDisplayColumnsFormula').value = '[' + $('#ddlHeaderText option:selected').text() + ']';
                    }
                    else {
                        $('#hlDDEdit').fadeIn();
                    }

                    document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?headername=yes&formula=' + encodeURIComponent(document.getElementById('hfDisplayColumnsFormula').value) + '&Tableid=' + document.getElementById('ctl00_HomeContentPlaceHolder_hfTableID').value;

                }
                
            });


            $('#ddlHeaderText').change();


            $(function () {
                $("#ctl00_HomeContentPlaceHolder_TabContainer2_tabField_upGrid").sortable({
                    items: '.gridview_row',
                    cursor: 'crosshair',
                    helper: fixHelper,
                    cursorAt: { left: 10, top: 10 },
                    connectWith: '#ctl00_HomeContentPlaceHolder_TabContainer2_tabField_upGrid',
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
                        var SC = '';
                        $(".ColumnID").each(function () {
                            SC = SC + this.value.toString() + ',';
                        });
                        //                        alert( SC);
                        document.getElementById("hfOrderSC").value = SC;
                        $("#btnOrderSC").trigger("click");


                        //                        $.ajax({
                        //                            url: 'OrderSC.aspx?newSC=' + SC,
                        //                            dataType: 'json'

                        //                        });

                    }
                });
            });

            $(function () {
                $("#ctl00_HomeContentPlaceHolder_TabContainer2_tabChildTTables_upTableChild").sortable({
                    items: '.gridview_row',
                    cursor: 'crosshair',
                    helper: fixHelper,
                    cursorAt: { left: 10, top: 10 },
                    connectWith: '#ctl00_HomeContentPlaceHolder_TabContainer2_tabChildTTables_upTableChild',
                    handle: '.sortHandle2',
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
                        var TC = '';
                        $(".TableChildID").each(function () {
                            TC = TC + this.value.toString() + ',';
                        });

                        document.getElementById("hfOrderTC").value = TC;
                        $("#btnOrderTC").trigger("click");

                    }
                });
            });


            $(function () {
                $("#ctl00_HomeContentPlaceHolder_TabContainer2_tabForms_upForms").sortable({
                    items: '.gridview_row',
                    cursor: 'crosshair',
                    helper: fixHelper,
                    cursorAt: { left: 10, top: 10 },
                    connectWith: '#ctl00_HomeContentPlaceHolder_TabContainer2_tabForms_upForms',
                    handle: '.sortHandle3',
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
                        var TC = '';
                        $(".FormSetID").each(function () {
                            TC = TC + this.value.toString() + ',';
                        });

                        document.getElementById("hfOrderFS").value = TC;
                        $("#btnOrderFS").trigger("click");

                    }
                });
            });

            //ValidatorEnable(document.getElementById('rfvNewMenuName'), false);

            //$('#ctl00_HomeContentPlaceHolder_TabContainer2_tabProperties_ddlMenu').change(function (e) {
            //    if (document.getElementById("ctl00_HomeContentPlaceHolder_TabContainer2_tabProperties_ddlMenu").value == 'new') {
            //        $("#trNewMenuName").fadeIn();
            //        var txtNewMenuName = document.getElementById("ctl00_HomeContentPlaceHolder_TabContainer2_tabProperties_txtNewMenuName");
            //        txtNewMenuName.value = '';
            //        $('#ctl00_HomeContentPlaceHolder_lblMsg').text('');
            //        ValidatorEnable(document.getElementById('rfvNewMenuName'), true);
            //    }
            //    else {
            //        $("#trNewMenuName").fadeOut();
            //        var txtNewMenuName = document.getElementById("ctl00_HomeContentPlaceHolder_TabContainer2_tabProperties_txtNewMenuName");
            //        txtNewMenuName.value = '';
            //        $('#ctl00_HomeContentPlaceHolder_lblMsg').text('');
            //        ValidatorEnable(document.getElementById('rfvNewMenuName'), false);
            //    }
            //});


            $("#chkAddUserRecord").click(function () {
                ShowHide();
            });


            $("#chkUniqueRecordedate").click(function () {
                ShowHide();
            });

            $("#chkDataUpdateUniqueColumnID").click(function () {
                ShowHide();
            });



            $("#chkSummaryPageContent").click(function () {
                var chkSummaryPageContent = document.getElementById("chkSummaryPageContent");
                if (chkSummaryPageContent.checked == true) {
                    $("#hlSummaryPageContent").trigger("click");                 
                }
            });

//            $("#ddlCategory").change(function () {
//                ShowHide();
//            });

            $("[id$='ddlUniqueColumnID']").change(function () {
                if ($("[id$='ddlUniqueColumnID'] option:selected").index() === 0)
                    $("#chkDataUpdateUniqueColumnID").prop("disabled", true);
                else
                    $("#chkDataUpdateUniqueColumnID").prop("disabled", false);
            });

        });

       
     
    </script>
    <asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server">
        <ProgressTemplate>
            <table style="width:100%;  height:100%; text-align: center;" >
                <tr valign="middle">
                    <td>
                        <p style="font-weight:bold;"> Please wait...</p>
                        <asp:Image ID="Image5" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                    </td>
                </tr>
            </table>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <table border="0" cellpadding="0" cellspacing="0" align="center" onload="ShowHide();">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle"></asp:Label></span> <a name="topline" id="topline">
                                </a>
                        </td>
                        <td align="left">
                            <div >
                               <%-- <asp:UpdateProgress class="ajax-indicator-Grey" ID="UpdateProgress3" runat="server">
                                    <ProgressTemplate>
                                        <table style="width:600px; height:600px; text-align: center">
                                            <tr>
                                                <td>
                                                    
                                                    <asp:Image runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>--%>
                            </div>
                        </td>
                        <td align="right">
                            <table>
                                <tr style="width: 100%">
                                    <td align="right">
                                    </td>
                                    <td align="right">
                                        <div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div runat="server" id="divFinished" visible="false">
                                                            <asp:HyperLink runat="server" ID="hlFinished" Text="Back" CssClass="btn"><strong>Finished</strong></asp:HyperLink>
                                                        </div>
                                                        <div>
                                                            <table runat="server" id="tblButtons">
                                                                <tr>
                                                                    <td>
                                                                        <%--<div>
                                                                            <asp:HyperLink runat="server" ID="hlProperties" ClientIDMode="Static" Text="Properties"
                                                                                CssClass="btn"><strong>Properties</strong></asp:HyperLink>
                                                                        </div>--%>
                                                                    </td>
                                                                    <td>
                                                                        <div>
                                                                            <asp:HyperLink runat="server" ID="hlBack" Text="Back" CssClass="btn"><strong>Back</strong></asp:HyperLink>
                                                                        </div>
                                                                    </td>
                                                                    <%--<td>
                                                                                <div runat="server" id="divSave">
                                                                                    
                                                                                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                                                        CausesValidation="true" > <strong>Save</strong> </asp:LinkButton>
                                                                                            
                                                                                </div>
                                                                            </td>--%>
                                                                    <td>
                                                                        <div runat="server" id="divEdit" visible="false">
                                                                            <asp:HyperLink runat="server" ID="hlEdit" CssClass="btn"> <strong>Edit</strong> </asp:HyperLink>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div runat="server" id="divDelete">
                                                                            <asp:LinkButton runat="server" ID="lnkDelete" CssClass="btn" CausesValidation="false"
                                                                                OnClick="lnkDelete_Click"> <strong>Delete</strong> </asp:LinkButton>
                                                                        </div>
                                                                        <div runat="server" id="divUnDelete">
                                                                            <asp:LinkButton runat="server" ID="lnkUnDelete" OnClientClick="javascript:return confirm('Are you sure you want to restore this Table?')"
                                                                                CssClass="btn" CausesValidation="false" OnClick="lnkUnDelete_Click"> <strong>Restore</strong>  </asp:LinkButton>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div runat="server" id="divRecords">
                                                                            <asp:HyperLink runat="server" ID="hlRecords" CssClass="btn"> <strong>Records</strong></asp:HyperLink>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static" NavigateUrl="~/Pages/Help/Help.aspx?contentkey=TableDetailHelp">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/images/help.png" />
                                        </asp:HyperLink>
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
            <td valign="top" colspan="3">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" runat="server">
                            <div runat="server" id="div1">
                                <div runat="server" id="divDetail">
                                    <%--<asp:HiddenField runat="server" ID="hfMenuID" />--%>
                                    <asp:HiddenField runat="server" ID="hfTableID" />
                                    <asp:HiddenField runat="server" ID="hfOrderSC" ClientIDMode="Static" />
                                    <asp:HiddenField runat="server" ID="hfOrderTC" ClientIDMode="Static" />
                                    <asp:HiddenField runat="server" ID="hfOrderFS" ClientIDMode="Static" />
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <ajaxToolkit:TabContainer ID="TabContainer2" runat="server" OnClientActiveTabChanged="ClientActiveTabChanged"
                    Width="1000px" CssClass="DBGTab">
                    <ajaxToolkit:TabPanel ID="tabField" runat="server">
                        <HeaderTemplate>
                            <strong runat="server" id="stgFieldsCap">Fields</strong>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div style="padding-left: 20px; padding-top: 10px;">
                                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:CheckBox runat="server" ID="chkShowSystemFields" TextAlign="Right" Font-Bold="true"
                                            Text="Show System Fields" AutoPostBack="true" OnCheckedChanged="chkShowSystemFields_OnCheckedChanged" />
                                               &nbsp;
                                            <asp:DropDownList runat="server" ID="ddlTableTabFilter" AutoPostBack="true" CssClass="NormalTextBox"
                                          OnSelectedIndexChanged="ddlTableTabs_SelectedIndexChanged" DataTextField="TabName" DataValueField="TableTabID"> </asp:DropDownList>
                                        <p runat="server" id="pInstruction" visible="false">
                                            Click on the Add icon to add fields:</p>
                                        <asp:Label runat="server" ID="lblSaveFieldsLabel" Width="200px"></asp:Label>
                                        <asp:LinkButton runat="server" ID="lnkSaveFields" OnClick="lnkSaveFields_Click">
                                            <asp:Image runat="server" ID="Image3" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                ToolTip="Save" />
                                        </asp:LinkButton>
                                         

                                         

                                            &nbsp; &nbsp; &nbsp;
                                        
                                            <asp:HyperLink runat="server" ID="hlTableTabs" Style="text-decoration: none; color: Black; padding-bottom:10px;"
                                                CssClass="popuplinktt"> <strong style="text-decoration: underline; color: Blue;">
                                               Configure Pages</strong>
                                            </asp:HyperLink>
                                       
                                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                            DataKeyNames="ColumnID" HeaderStyle-ForeColor="Black" Width="100%" AutoGenerateColumns="false"
                                            PageSize="550" OnPreRender="gvTheGrid_PreRender" OnRowCommand="gvTheGrid_RowCommand"
                                            OnRowDataBound="gvTheGrid_RowDataBound" OnRowDeleting="gvTheGrid_RowDeleting"
                                            AlternatingRowStyle-BackColor="#DCF2F0">
                                            <PagerSettings Position="Top" />
                                            <HeaderStyle CssClass="gridview_header" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("ColumnID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/App_Themes/Default/Images/delete.png"
                                                            CommandName="delete" CommandArgument='<%# Eval("ColumnID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() +  Cryptography.Encrypt(Eval("ColumnID").ToString()) %>'
                                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="sortHandle">
                                                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/MoveIcon.png"
                                                            ToolTip="Drag and drop to change order" />
                                                        <input type="hidden" id='hfColumnID' value='<%# Eval("ColumnID") %>' class='ColumnID' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Field Name">
                                                    <ItemTemplate>
                                                        <%--<asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("ColumnID").ToString())  %>'
                                                            Text='<%# Eval("DisplayName")%>' />--%>
                                                        <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Type" ItemStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIsNumeric" runat="server" Text='<%# Eval("ColumnType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Show in the Add field box" ItemStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDisplayTextSummary" runat="server" Checked='<%#  !String.IsNullOrEmpty(Eval("DisplayTextSummary").ToString()) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Show on Detail" ItemStyle-Width="80px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDisplayTextDetail" runat="server" Checked='<%#  !String.IsNullOrEmpty(Eval("DisplayTextDetail").ToString()) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField Visible="false" HeaderText="Page" >
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                          <asp:DropDownList runat="server" ID="ddlTableTab"  DataTextField="TabName" DataValueField="TableTabID"
                                                               CssClass="NormalTextBox"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText=" Importance" ItemStyle-Width="80px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <%--<asp:CheckBox ID="chkIsMandatory" runat="server" Checked='<%#  Eval("IsMandatory") %>' />--%>
                                                         <asp:DropDownList runat="server" ID="ddlImportance" ToolTip="Required means it is important but you can still save the data without it. Mandatory will prevent the data being saved unless entered." CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="" Text="Optional" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="r" Text="Required"></asp:ListItem>
                                                                       <asp:ListItem Value="m" Text="Mandatory"></asp:ListItem>
                                                                </asp:DropDownList>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText=" Display on the Right" ItemStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDisplayRight" runat="server" Checked='<%#  Eval("DisplayRight") %>' />
                                                        <%--<asp:Label ID="lblDisplayRight" runat="server" Text=""></asp:Label>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Import" ItemStyle-Width="70px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkNameOnImport" runat="server" Checked='<%#  !String.IsNullOrEmpty(Eval("NameOnImport").ToString()) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Import Position" ItemStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPositionOnImport" runat="server" Text='<%# Eval("PositionOnImport") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Export" ItemStyle-Width="70px" >
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkNameOnExport" runat="server" Checked='<%#  !String.IsNullOrEmpty(Eval("NameOnExport").ToString()) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField  HeaderText="Copy Field" ItemStyle-Width="70px" Visible="false" ><%-- 14th columns--%>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAllowCopy" runat="server" Checked='<%#  Eval("AllowCopy")==DBNull.Value?false: Eval("AllowCopy") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="gridview_row" />
                                            <PagerTemplate>
                                                <asp:GridViewPager runat="server" ID="Pager" HideDelete="true" HideFilter="true"  HideGo="true" HideNavigation="true" HidePageSize="true"
                                                    OnExportForCSV="Pager_OnExportForCSV" OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                                    OnBindTheGridAgain="Pager_BindTheGridAgain" />
                                            </PagerTemplate>
                                        </dbg:dbgGridView>
                                        <br />
                                        <div runat="server" id="divEmptyFields" visible="false" style="padding-left: 20px;">
                                        <asp:HyperLink runat="server" ID="hlAddNewField" Style="text-decoration: none;
                                            color: Black;" >
                                            <asp:Image runat="server" ID="Image7" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                            No custom fields have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                                Add new field now.</strong>
                                        </asp:HyperLink>
                                    </div>


                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                                        <asp:AsyncPostBackTrigger ControlID="chkShowSystemFields" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabProperties" runat="server">
                        <HeaderTemplate>
                            <strong>Properties</strong>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div style="padding-left: 20px; padding-top: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <table>
                                                <%--<tr >
                                                    <td align="right">
                                                        
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlCategory" ClientIDMode="Static" CssClass="NormalTextBox">
                                                            <asp:ListItem Text="Display" Value="display" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Import/Data" Value="importdata"></asp:ListItem>
                                                            <asp:ListItem Text="Add Records" Value="addrecords"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:HiddenField runat="server" ID="hfCategory" Value="display" ClientIDMode="Static" />
                                                        <asp:LinkButton runat="server" ID="lnkPropertyDisplay" Text="Display" Style="font-weight: bold;"></asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="lnkPropertyImportData" Text="Import/Data"></asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="lnkPropertyAddRecords" Text="Add Records"></asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="lnkColours" Text="Colours"></asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="lnkGraphs" Text="Graphs"></asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="lnkEmails" Text="Emails"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                    <td colspan="2">

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div id="divDisplay">
                                                            <table>
                                                                <tr>
                                                                    <td align="right" style="width: 230px;">
                                                                        <strong>Name*:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtTable" runat="server" Width="256px" CssClass="NormalTextBox"></asp:TextBox>
                                                                         <asp:HyperLink ID="hlTableRename" ClientIDMode="Static" runat="server" CssClass="popuprenametable" style="display:none;"></asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trMenu" visible="false">
                                                                    <td align="right">
                                                                        <strong>Menu:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="false" DataTextField="MenuP"
                                                                            Width="155px" DataValueField="MenuID" CssClass="NormalTextBox">
                                                                        </asp:DropDownList>--%>
                                                                        <%--<asp:HyperLink runat="server" ID="hlMenuEdit" Text="Edit" NavigateUrl="~/Pages/Record/TableGroup.aspx"
                                                                            CssClass="NormalTextBox" Visible="false"></asp:HyperLink>--%>
                                                                    </td>
                                                                </tr>
                                                               <%-- <tr id="trNewMenuName" style="display: none;">--%>
                                                                 <%--<tr runat="server" id="trNewMenuHidden" visible="false">
                                                                    <td align="right">
                                                                        <strong>New Menu Name*:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNewMenuName" runat="server" Width="256px" CssClass="NormalTextBox"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator runat="server" ID="rfvNewMenuName" ControlToValidate="txtNewMenuName"
                                                                            ErrorMessage="New Menu Name - Required" CssClass="NormalTextBox" Display="None" Enabled="false"
                                                                            ClientIDMode="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>--%>
                                                                <tr id="Tr1" runat="server" >
                                                                    <td align="right">
                                                                        <strong>Pin</strong>
                                                                    </td>
                                                                    <td align="left">
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList runat="server" ID="ddlPinImages" AutoPostBack="false" ClientIDMode="Static"
                                                                                        CssClass="NormalTextBox">
                                                                                        <asp:ListItem Selected="True" Text="--Please Select--" Value="Pages/Record/PINImages/DefaultPin.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Blue" Value="Pages/Record/PINImages/PinBlue.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Black" Value="Pages/Record/PINImages/PinBlack.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Gray" Value="Pages/Record/PINImages/PinGray.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Green" Value="Pages/Record/PINImages/PinGreen.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Light Blue" Value="Pages/Record/PINImages/PinLBlue.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Orange" Value="Pages/Record/PINImages/PinOrange.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Purple" Value="Pages/Record/PINImages/PinPurple.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Red" Value="Pages/Record/PINImages/PinRed.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Pin Yellow" Value="Pages/Record/PINImages/PinYellow.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round Blue" Value="Pages/Record/PINImages/RoundBlue.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round Gray" Value="Pages/Record/PINImages/RoundGray.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round Green" Value="Pages/Record/PINImages/RoundGreen.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round Light Blue" Value="Pages/Record/PINImages/RoundLightBlue.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round Orange" Value="Pages/Record/PINImages/RoundOrange.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round Purple" Value="Pages/Record/PINImages/RoundPurple.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round Red" Value="Pages/Record/PINImages/RoundRed.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round White" Value="Pages/Record/PINImages/RoundWhite.png"></asp:ListItem>
                                                                                        <asp:ListItem Text="Round Yellow" Value="Pages/Record/PINImages/RoundYellow.png"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Panel runat="server" ID="pnlPINImages" Width="50px" ScrollBars="None">
                                                                                        &nbsp;
                                                                                        <asp:Image runat="server" ID="imgPIN" ImageUrl="~/Pages/Record/PINImages/DefaultPin.png"
                                                                                            ClientIDMode="Static" />
                                                                                    </asp:Panel>
                                                                                </td>
                                                                                <td style="width:10px;">

                                                                                </td>
                                                                                <td align="right">
                                                                                    <strong>Display Order</strong>
                                                                                </td>
                                                                                <td style="padding-left:2px;">
                                                                                     <asp:TextBox runat="server" ID="txtPinDisplayOrder" TextMode="SingleLine" CssClass="NormalTextBox"
                                                                                        Width="50px"></asp:TextBox>
                                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtPinDisplayOrder"
                                                                                        runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                                    </asp:RegularExpressionValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 15px;">
                                                                    <td colspan="2">
                                                                    </td>
                                                                </tr>

                                                                  <tr runat="server" visible="false">
                                                                    <td align="right">
                                                                        <strong>Search or Filter:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlFilterType"    CssClass="NormalTextBox">
                                                                            <asp:ListItem Text="Search Fields" Value="box"></asp:ListItem>
                                                                             <asp:ListItem Text="Filter Dropdowns" Value="ddl" Selected="True"></asp:ListItem>
                                                                            
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        
                                                                    </td>
                                                                    <td>
                                                                        <%--//oliver <begin> Instructed by Jon to add the word 'Search' during our skype conversation (8/29/2016)--%>
                                                                             <asp:CheckBox runat="server" ID="chkShowAdvancedOptions" Checked="true" TextAlign="Right"
                                                                                        Font-Bold="true" Text="Show Advanced Search Options" />
                                                                        <%--//oliver <end>--%>
                                                                    </td>
                                                                </tr>

                                                                <tr runat="server" id="trFilter" visible="false">
                                                                    <td align="right">
                                                                        <strong>Summary Page Filter</strong>
                                                                    </td>
                                                                    <td align="left">
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <%--<asp:DropDownList ID="ddlYAxis" runat="server" AutoPostBack="true" CssClass="NormalTextBox"
                                                                                        OnSelectedIndexChanged="ddlYAxis_SelectedIndexChanged">
                                                                                    </asp:DropDownList>--%>

                                                                                    <dbg:cbcValue runat="server" ID="cbcvSumFilter" OnddlYAxis_Changed="cbcvSumFilter_OnddlYAxis_Changed" />
                                                                                </td>
                                                                              
                                                                                <td align="left">
                                                                                   <asp:CheckBox runat="server" ID="chkHideFilter"  TextAlign="Right"
                                                                                        Font-Bold="true" Text="Hide Summary Page Filter" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trSummaryPageSort" visible="false">
                                                                    <td align="right">
                                                                        <strong>Summary Page Sort by</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlSortColumn" DataValueField="ColumnID" DataTextField="DisplayName"
                                                                            CssClass="NormalTextBox">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trReasonChange" visible="false">
                                                                    <td align="right">
                                                                        <strong>Reason for change</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlReasonChange" CssClass="NormalTextBox">
                                                                            <asp:ListItem Value="mandatory" Text="Mandatory"></asp:ListItem>
                                                                            <asp:ListItem Value="optional" Text="Optional"></asp:ListItem>
                                                                            <asp:ListItem Value="none" Text="None" Selected="True"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trChangeHistory" visible="false">
                                                                    <td align="right">
                                                                        <strong>Change History</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlChangeHistory" CssClass="NormalTextBox">
                                                                            <asp:ListItem Value="always" Text="Always Visible" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Value="admin" Text="Visible to Admin Users"></asp:ListItem>
                                                                            <asp:ListItem Value="none" Text="Not Visible"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong runat="server" id="stgDisplayHeader">Display Field In Header</strong>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:TextBox runat="server" ID="txtHeaderName" ClientIDMode="Static" CssClass="NormalTextBox" Width="200px"></asp:TextBox>--%>
                                                                        <asp:DropDownList runat="server" ID="ddlHeaderText" ClientIDMode="Static" CssClass="NormalTextBox"
                                                                            DataValueField="ColumnID" DataTextField="DisplayName">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField runat="server" ID="hfDisplayColumnsFormula" ClientIDMode="Static" />
                                                                        &nbsp;
                                                                        <asp:HyperLink runat="server" ID="hlDDEdit" Text="Edit" ClientIDMode="Static" NavigateUrl="~/Pages/Help/TableColumn.aspx"></asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                                 <tr>
                                                                     <td align="right">
                                                                       
                                                                    </td>
                                                                    <td>
                                                                        
                                                                        <table style="border-collapse:collapse;border-spacing:0;">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:CheckBox runat="server" ID="chkSummaryPageContent" ClientIDMode="Static" />
                                                                                </td>
                                                                                <td style="padding-left:2px;">
                                                                                    <asp:HyperLink runat="server" ID="hlSummaryPageContent" ClientIDMode="Static" Text="Display Content Below Summary Page"></asp:HyperLink>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                     <td align="right">
                                                                       
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkBoxAroundField" TextAlign="Right" Text="Box Around Fields" />
                                                                    </td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                     <td align="right">
                                                                       
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkShowTabVertically" TextAlign="Right" Text="Tabs as a Vertical list" />
                                                                    </td>
                                                                    
                                                                </tr>
                                                                <tr style="height: 15px;">
                                                                    <td colspan="2">
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trFlashAlerts">
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkFlashAlerts" AutoPostBack="false" Text="Flash on recent alerts"
                                                                            TextAlign="Right" />
                                                                    </td>
                                                                </tr>
                                                                <tr >
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkNavigationArrows" AutoPostBack="false" Text="Navigation Arrows"
                                                                            TextAlign="Right" />
                                                                    </td>
                                                                </tr>
                                                                  <tr >
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkSaveAndAdd" AutoPostBack="false" Text="Save and Add another button"
                                                                            TextAlign="Right" />
                                                                    </td>
                                                                </tr>

                                                            </table>
                                                        </div>
                                                        <div id="divImportData">
                                                            <table>
                                                                <tr runat="server" id="trMaxTime">
                                                                    <td align="right">
                                                                        <strong>Maximum Time Between Records:</strong>
                                                                    </td>
                                                                    <td align="left">
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtMaxTimeBetweenRecords" CssClass="NormalTextBox"
                                                                                        Width="75px"></asp:TextBox>&nbsp;&nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList runat="server" ID="ddlMaxTimeBetweenRecordsUnit" CssClass="NormalTextBox"
                                                                                        AutoPostBack="false">
                                                                                        <asp:ListItem Selected="True" Value="Minutes" Text="Minutes"></asp:ListItem>
                                                                                        <asp:ListItem Value="Hours" Text="Hours"></asp:ListItem>
                                                                                        <asp:ListItem Value="Days" Text="Days"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trLateDate">
                                                                    <td align="right">
                                                                        <strong>Late Data - Notify after:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtLateDataDays" CssClass="NormalTextBox" Width="75px"></asp:TextBox>&nbsp;Days
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="tr2">
                                                                    <td align="right">
                                                                        <strong runat="server" id="stgImportDataColumnHeader">Import Data Field Header Row</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtImportColumnHeaderRow" CssClass="NormalTextBox"
                                                                            Width="75px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trImportData">
                                                                    <td align="right">
                                                                        <strong>Import Data Start Row</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtImportDataStartRow" CssClass="NormalTextBox" Width="75px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" style="vertical-align: top;">
                                                                        <strong>Template</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlTemplate" CssClass="NormalTextBox" >
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="top">
                                                                        <strong>Custom Upload Sheet:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:FileUpload ID="fuRecordFile" runat="server" Style="width: 500px;" size="70"
                                                                            Font-Size="11px" />
                                                                        <br />
                                                                        Please select a CSV/XLS/XLSX file to upload.
                                                                        <br />
                                                                        <%--<asp:HyperLink runat="server" ID="hlCustomUploadSheet" Target="_blank"
                                                                         Text="View" Visible="false"></asp:HyperLink>--%>
                                                                         <asp:LinkButton runat="server" ID="hlCustomUploadSheet" Text="View" Visible="false"
                                                                         OnClick="hlCustomUploadSheet_Click" ></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 15px;">
                                                                    <td colspan="2">
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trImport">
                                                                    <td align="right">
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkIsPosition" AutoPostBack="true" Text="Use Positions on Import"
                                                                            TextAlign="Right" OnCheckedChanged="chkIsPosition_CheckedChanged" />
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trUniqueRecordedate">
                                                                    <td align="right">
                                                                        <strong></strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkUniqueRecordedate" runat="server" Text="Unique Records Only"
                                                                            TextAlign="Right" ClientIDMode="Static"/>
                                                                        <br />
                                                                        <div id="divUniqueRecord" clientidmode="Static" style="padding-left: 50px;">
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="right" style="width:150px;">
                                                                                        <strong>1st Column</strong>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList runat="server" ID="ddlUniqueColumnID" DataValueField="ColumnID"
                                                                                            DataTextField="DisplayName" CssClass="NormalTextBox">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="width:150px;">
                                                                                        <strong>2nd Column</strong>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList runat="server" ID="ddlUniqueColumnID2" DataValueField="ColumnID"
                                                                                            DataTextField="DisplayName" CssClass="NormalTextBox">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong></strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkDataUpdateUniqueColumnID" ClientIDMode="Static" Checked="false"  
                                                                            TextAlign="Right"  Text="Allow data to be updated (Unique Reords)"/>
                                                                        <%--<br />--%>
                                                                        <div id="divDataUpdateUniqueColumnID" style="padding-left:20px; display: none; visibility: hidden;" clientidmode="Static">
                                                                            <table>
                                                                                <tr align="right">
                                                                                    <td><strong>Unique identifier</strong></td>
                                                                                    <td>
                                                                                        <asp:DropDownList runat="server" ID="ddlDataUpdateUniqueColumnID" ClientIDMode="Static" CssClass="NormalTextBox"
                                                                                            DataValueField="ColumnID" DataTextField="DisplayName">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                       
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkCopyToChildTables"  TextAlign="Right"  Text="Copy data to child tables"/>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divAddRecords" style="padding-left: 100px;">
                                                            
                                                                    <table>
                                                                        <tr runat="server" id="trAnonymousUser" visible="false">
                                                                            <td align="right">
                                                                                <strong></strong>
                                                                            </td>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="UpdatePanel2"  runat="server" UpdateMode="Conditional" >
                                                                                    <ContentTemplate>
                                                                                        
                                                                                       
                                                                                        <div >
                                                                                           <span style="padding-left:5px;" >Parent Table</span> 
                                                                                            <asp:DropDownList runat="server" ID="ddlParentTable" CssClass="NormalTextBox" DataTextField="TableName"
                                                                                                DataValueField="ParentTableID" AutoPostBack="true" OnSelectedIndexChanged="ddlParentTable_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                            <br />
                                                                                            <asp:CheckBox ID="chkAnonymous" runat="server" Text="Allow users to Add records without login"
                                                                                            TextAlign="Right" AutoPostBack="true" OnCheckedChanged="chkAnonymous_CheckedChanged" />
                                                                                            <br />
                                                                                            <div runat="server" id="divValidateUser" visible="false">
                                                                                                Validate user be asking them to enter:
                                                                                                <br />
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlValidateColumnID1" runat="server" DataTextField="DisplayName"
                                                                                                                DataValueField="ColumnID" AutoPostBack="false" CssClass="NormalTextBox">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td style="padding-left: 5px;">
                                                                                                            and
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlValidateColumnID2" runat="server" DataTextField="DisplayName"
                                                                                                                DataValueField="ColumnID" AutoPostBack="false" CssClass="NormalTextBox">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <br />
                                                                                            </div>
                                                                                            <br />
                                                                                            Link:<asp:HyperLink runat="server" Target="_blank" NavigateUrl="#" ID="hlPublicFormURL"></asp:HyperLink>
                                                                                            <%--<asp:Label runat="server" ID="lblPublicFormURL"></asp:Label>--%>
                                                                                        </div>
                                                                                    </ContentTemplate>
                                                                                    
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 20px;">
                                                                            <td colspan="2">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" valign="top">
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox runat="server" ID="chkAddUserRecord" Text="Automatically Create a User Record"
                                                                                    TextAlign="Right" ClientIDMode="Static" Checked="false"></asp:CheckBox>
                                                                                <br />
                                                                                <div id="divAutomaticallyAddUserRecord" clientidmode="Static" style="padding-left: 50px;">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <strong>Username</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlAddUserUserColumnID" DataValueField="ColumnID"
                                                                                                    DataTextField="DisplayName" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <strong>Password</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlAddUserPasswordColumnID" DataValueField="ColumnID"
                                                                                                    DataTextField="DisplayName" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:CheckBox runat="server" ID="chkAddUserNotification" Text="Notify User" TextAlign="Right"
                                                                                                    Font-Bold="true"></asp:CheckBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td align="left">
                                                                                        <asp:CheckBox runat="server" ID="chkShowEditAfterAdd" Text="Show Edit After Add"
                                                                                    TextAlign="Right" ClientIDMode="Static" Checked="false"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td align="left">
                                                                                        <asp:CheckBox runat="server" ID="chkAllowCopyRecords" Text="Allow users to copy records"
                                                                                    TextAlign="Right" ClientIDMode="Static" Checked="false"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <%--<tr>
                                                                            <td></td>
                                                                            <td align="left">
                                                                                        <asp:CheckBox runat="server" ID="chkSaveAndAdd" Text="Save and Add"
                                                                                    TextAlign="Right" ClientIDMode="Static" Checked="false"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>--%>
                                                                    </table>
                                                               
                                                        </div>
                                                        <div id="divColours">
                                                            <table>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong>Header Colour:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtHeaderColor" MaxLength="6" CssClass="NormalTextBox"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong>Tab Colour:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtTabColour" MaxLength="6" CssClass="NormalTextBox"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong>Tab Text Colour:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtTabTextColour" MaxLength="6" CssClass="NormalTextBox"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong>Filter Top Colour:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtFilterTopColour" MaxLength="6" CssClass="NormalTextBox"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong>Filter Bottom Colour:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtFilterBottomColour" MaxLength="6" CssClass="NormalTextBox"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divGraphs">
                                                            <table>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong>X-Axis Column:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlGraphXAxisColumnID" DataValueField="ColumnID" DataTextField="DisplayName"
                                                                            CssClass="NormalTextBox">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <strong>Series Column:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlGraphSeriesColumnID" DataValueField="ColumnID" DataTextField="DisplayName"
                                                                            CssClass="NormalTextBox">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                 <tr>
                                                                    <td align="right">
                                                                        <strong>Default Y-Axis Column:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlGraphDefaultYAxisColumnID" DataValueField="ColumnID" DataTextField="DisplayName"
                                                                            CssClass="NormalTextBox">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                 <tr>
                                                                    <td align="right">
                                                                        <strong>On Start:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlGraphOnStart" CssClass="NormalTextBox">
                                                                                <asp:ListItem Value="Averages" Text="Show Averages"></asp:ListItem>
                                                                                <asp:ListItem Value="EmptyGraph" Text="Show Empty Graph"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td align="right">
                                                                        <strong>Default Graph Period:</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlDefaultGraphPeriod" 
                                                                            CssClass="NormalTextBox">
                                                                            <asp:ListItem Value="">-- Please Select --</asp:ListItem>
                                                                            <asp:ListItem Value="0">All</asp:ListItem>
                                                                            <asp:ListItem Value="1">1 year</asp:ListItem>
                                                                            <asp:ListItem Value="2">6 months</asp:ListItem>
                                                                            <asp:ListItem Value="3">3 months</asp:ListItem>
                                                                            <asp:ListItem Value="4">1 month</asp:ListItem>
                                                                            <asp:ListItem Value="5">1 week</asp:ListItem>
                                                                            <asp:ListItem Value="6">1 day</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divEmails" style="padding-left:200px;">
                                                            <br />
                                                            <br />
                                                            <asp:CheckBox runat="server" ID="chkShowSentEmails" TextAlign="Right" Font-Bold="true" Text="Show Sent Emails" />
                                                            <br />
                                                            <br />
                                                            <asp:CheckBox runat="server" ID="chkShowReceivedEmails" TextAlign="Right" Font-Bold="true" Text="Show Received Emails " />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                    </td>
                                                    <td align="left">
                                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtLateDataDays"
                                                            ErrorMessage="Invalid Late data!" MaximumValue="1000000" MinimumValue="0" Type="Integer"
                                                            Display="None" />
                                                        <asp:RequiredFieldValidator ID="rfvTable" runat="server" ControlToValidate="txtTable"
                                                            ErrorMessage="Name - Required" CssClass="NormalTextBox" Display="None"></asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtImportDataStartRow"
                                                            ErrorMessage="Invalid Import Data Start Row data!" MaximumValue="1000000" MinimumValue="2"
                                                            Type="Integer" Display="None" />
                                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtImportColumnHeaderRow"
                                                            ErrorMessage="Invalid Import Field Header Row!" MaximumValue="1000000" MinimumValue="1"
                                                            Type="Integer" Display="None" />
                                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtMaxTimeBetweenRecords"
                                                            ErrorMessage="Invalid Maximum Time Between Records!" MaximumValue="1000000" MinimumValue="0"
                                                            Type="Double" Display="None" />
                                                        <asp:Label runat="server" ID="Label1" ForeColor="Red"></asp:Label>
                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                                            ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct following errors:" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top" style="vertical-align: top">
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
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabChildTTables" runat="server">
                        <HeaderTemplate>
                            <strong>Child Tables</strong>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upTableChild" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div style="padding-left: 20px; padding-top: 10px;">
                                        <asp:GridView ID="grdTable" runat="server" AutoGenerateColumns="False" DataKeyNames="TableChildID" EmptyDataText="No Child Tables"
                                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" CssClass="gridview"
                                            OnRowCommand="grdTable_RowCommand" OnRowDataBound="grdTable_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                                            <RowStyle CssClass="gridview_row" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("TableChildID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/App_Themes/Default/Images/icon_delete.gif"
                                                            CommandName="deletetype" CommandArgument='<%# Eval("TableChildID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/ChildTableDetail.aspx"
                                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" ToolTip="Edit" ID="hlEditDetail"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/ChildTableDetail.aspx"
                                                            ImageUrl="~/Pages/Pager/Images/add.png" ToolTip="Add Child Table" ID="hlAddDetail" Visible="false"></asp:HyperLink>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="sortHandle2">
                                                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/MoveIcon.png"
                                                            ToolTip="Drag and drop to change order" />
                                                        <input type="hidden" id='hfTableChildID' value='<%# Eval("TableChildID") %>' class='TableChildID' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Table">
                                                    <ItemTemplate>
                                                        <div style="padding-right: 10px;">
                                                            <a target="_blank" href='<%# GetTableViewURL() + Cryptography.Encrypt(Eval("ChildTableID").ToString())  %>'>
                                                                <%# Eval("ChildTableName")%></a>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <div style="padding-left: 10px;">
                                                            <asp:Label runat="server" ID="lblDescription" Text='<%# Eval("Description")%>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Show On Detail Page">
                                                    <ItemTemplate>
                                                        <div style="padding-left: 10px;">
                                                            <asp:Label runat="server" ID="lblDetailPageType" Text=""></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:CheckBoxField HeaderText="Add Button" DataField="ShowAddButton" />--%>
                                                <%--<asp:CheckBoxField HeaderText="Edit Button" DataField="ShowEditButton" />--%>
                                                <asp:TemplateField Visible="true" HeaderText="Add Button" ItemStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkShowAddButton" runat="server" AutoPostBack="true" OnCheckedChanged="UpdateTableChild"
                                                            Checked='<%#  Eval("ShowAddButton")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Edit Button" ItemStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkShowEditButton" runat="server" AutoPostBack="true" OnCheckedChanged="UpdateTableChild"
                                                            Checked='<%#  Eval("ShowEditButton") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Show When" ItemStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkShowWhen" runat="server" AutoPostBack="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="gridview_header" />
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <div runat="server" id="divEmptyData" visible="false" style="padding-left: 20px;">
                                        <asp:HyperLink runat="server" ID="hplAddChildTable" Style="text-decoration: none;
                                            color: Black;" CssClass="popuplink">
                                            <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                            No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                                Add new record now.</strong>
                                        </asp:HyperLink>
                                    </div>
                                    <br />
                                    <br />
                                    <asp:HiddenField runat="server" ID="hfTableChildDeleteID" Value="" />
                                    <asp:Label runat="server" ID="lblModalDeleteTableChild" />
                                    <ajaxToolkit:ModalPopupExtender ID="mpeModalDeleteTableChild" ClientIDMode="Static"
                                        runat="server" BehaviorID="popup" TargetControlID="lblModalDeleteTableChild"
                                        PopupControlID="pnlModalDeleteTableChild" BackgroundCssClass="modalBackground"
                                        OkControlID="lnkDeleteTableChildCancel" />
                                    <asp:Panel ID="pnlModalDeleteTableChild" runat="server" Style="display: none">
                                        <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD;
                                            height: 150px; border-style: outset;">
                                            <div style="padding-top: 50px; padding: 20px;">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label Width="400px" runat="server" ID="lblTableChildDelateMessage" Text="Are you sure you wish to delete this relationship?  
                                                                The link field will be converted to a text field and the 
                                                                values retained so you can relink it later if you want."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px;">
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="lnkDeleteTableChildOK" CssClass="btn" CausesValidation="false"
                                                                            OnClick="lnkDeleteTableChildOK_Click"> <strong>Ok</strong></asp:LinkButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton runat="server" ID="lnkDeleteTableChildCancel" CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <br />
                                    <br />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdTable" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabTemplates" runat="server">
                        <HeaderTemplate>
                            <strong>Templates</strong>
                        </HeaderTemplate>
                        <ContentTemplate>

                            <div style="padding-left: 20px; padding-top: 10px;">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:HiddenField runat="server" ID="hfTemplateCategory" Value="import" ClientIDMode="Static" />
                                            <asp:LinkButton runat="server" ID="lnkTemplateImport" Text="Import" ></asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="lnkTemplateExport" Text="Export"></asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="lnkTemplateWordMerge" Text="Word Merge"></asp:LinkButton>

                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div id="divTemplateImport">
                                                <dbg:ImpTem runat="server" ID="itOne" />
                                            </div>
                                            <div id="divTemplateExport">
                                                <dbg:ExpTem runat="server" ID="etOne" />

                                            </div>
                                            <div id="divTemplateWordMerge">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div style="padding-left: 20px; padding-top: 10px;">
                                                            <asp:GridView ID="gvTemplates" runat="server" AutoGenerateColumns="False" DataKeyNames="DocTemplateID"
                                                                HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" CssClass="gridview"
                                                                OnRowCommand="gvTemplates_RowCommand" OnRowDataBound="gvTemplates_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LblID" runat="server" Text='<%# Eval("DocTemplateID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/App_Themes/Default/Images/delete_s.png"
                                                                                CommandName="deletetype" CommandArgument='<%# Eval("DocTemplateID") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>

                                                                            <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/DocTemplateDetail.aspx"
                                                                                ImageUrl="~/App_Themes/Default/Images/iconEdit.png" ToolTip="Edit" ID="hlEditDetail"></asp:HyperLink>

                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/DocTemplateDetail.aspx"
                                                                                ImageUrl="~/Pages/Pager/Images/add.png" ToolTip="Add Child Table" ID="hlAddDetail"></asp:HyperLink>
                                                                        </HeaderTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Word Document">
                                                                        <ItemTemplate>
                                                                            <div style="padding-right: 10px;">
                                                                                <asp:Label runat="server" ID="lblFileName" Text='<%# Eval("FileName")%>'></asp:Label>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--//oliver <begin> Jon asked me to hide this based on email 8/18/2016--%>
                                                                    <asp:TemplateField HeaderText="Data Retriever" Visible="false">
                                                                        <ItemTemplate>
                                                                            <div style="padding-left: 10px;">
                                                                                <asp:Label runat="server" ID="lblSPName" Text='<%# Eval("SPName")%>'></asp:Label>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--//oliver <end>--%>
                                                                </Columns>
                                                                <HeaderStyle CssClass="gridview_header" />
                                                            </asp:GridView>
                                                        </div>
                                                        <br />
                                                        <div runat="server" id="divEmptyDataTemplates" visible="false" style="padding-left: 20px;">
                                                            <asp:HyperLink runat="server" ID="hlAddTemplates" Style="text-decoration: none; color: Black;"
                                                                CssClass="popuplink">
                                            <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                            No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                                Add new record now.</strong>
                                                            </asp:HyperLink>
                                                        </div>
                                                        <br />
                                                        <br />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvTemplates" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>

                                        </td>
                                    </tr>
                                </table>
                            </div>



                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabTable" runat="server">
                        <HeaderTemplate>
                            <strong>Notifications</strong>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upWarningRecipients" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div style="padding-left: 50px; padding-top: 10px;">
                                        <asp:GridView ID="grdTableUser" runat="server" AutoGenerateColumns="False" DataKeyNames="TableUserID"
                                            CssClass="gridviewborder" OnRowCommand="grdTableUser_RowCommand" OnRowDataBound="grdTableUser_RowDataBound"
                                            BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" ShowHeaderWhenEmpty="true"
                                            ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblID" Text='<%# Eval("TableUserID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/App_Themes/Default/Images/delete_s.png"
                                                            CommandName="deletetype" CommandArgument='<%# Eval("TableUserID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="User" DataField="UserName" />


                                                 <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <strong>Add Data Notifications</strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <FooterTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlAddDataEmail" runat="server" Target="_blank">View Email</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlAddDataSMS" runat="server" Target="_blank">View SMS</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlAddDataOption" AutoPostBack="true" CssClass="NormalTextBox"
                                                            OnSelectedIndexChanged="UpdateSupplyLed">
                                                            <asp:ListItem Text="None" Value="none" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Email" Value="email" ></asp:ListItem>
                                                            <asp:ListItem Text="SMS" Value="sms"></asp:ListItem>
                                                            <asp:ListItem Text="Both" Value="both"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <strong>Upload Notifications</strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <FooterTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlUploadEmail" runat="server" Target="_blank">View Email</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlUploadSMS" runat="server" Target="_blank">View SMS</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlUploadOption" AutoPostBack="true" CssClass="NormalTextBox"
                                                            OnSelectedIndexChanged="UpdateSupplyLed">
                                                            <asp:ListItem Text="None" Value="none"></asp:ListItem>
                                                            <asp:ListItem Text="Email" Value="email" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="SMS" Value="sms"></asp:ListItem>
                                                            <asp:ListItem Text="Both" Value="both"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <strong>Warnings</strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <FooterTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlUploadWarningEmail" runat="server" Target="_blank">View Email</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlUploadWarningSMS" runat="server" Target="_blank">View SMS</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlUploadWarningOption" AutoPostBack="true"
                                                            CssClass="NormalTextBox" OnSelectedIndexChanged="UpdateSupplyLed">
                                                            <asp:ListItem Text="None" Value="none"></asp:ListItem>
                                                            <asp:ListItem Text="Email" Value="email" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="SMS" Value="sms"></asp:ListItem>
                                                            <asp:ListItem Text="Both" Value="both"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <strong>Exceedances</strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <FooterTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlUploadExceedanceEmail" runat="server" Target="_blank">View Email</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlUploadExceedanceSMS" runat="server" Target="_blank">View SMS</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlUploadExceedanceOption" AutoPostBack="true"
                                                            CssClass="NormalTextBox" OnSelectedIndexChanged="UpdateSupplyLed">
                                                            <asp:ListItem Text="None" Value="none"></asp:ListItem>
                                                            <asp:ListItem Text="Email" Value="email" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="SMS" Value="sms"></asp:ListItem>
                                                            <asp:ListItem Text="Both" Value="both"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                
                                                 <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <strong>Late data/Flat data</strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <FooterTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlLateWarningEmail" runat="server" Target="_blank">View Email</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlLateWarningSMS" runat="server" Target="_blank">View SMS</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlLateWarningOption" AutoPostBack="true" CssClass="NormalTextBox"
                                                            OnSelectedIndexChanged="UpdateSupplyLed">
                                                            <asp:ListItem Text="None" Value="none"></asp:ListItem>
                                                            <asp:ListItem Text="Email" Value="email" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="SMS" Value="sms"></asp:ListItem>
                                                            <asp:ListItem Text="Both" Value="both"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="gridviewborder_header" />
                                            <RowStyle CssClass="gridviewborder_row" />
                                        </asp:GridView>
                                        <div runat="server" id="divUserAdd" visible="true" style="width: 100%;padding-bottom:20px;">
                                            <br />
                                            <table>
                                                <tr>
                                                    <td>
                                                        <strong>Add Recipient</strong>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddlUser" runat="server" DataTextField="UserName" DataValueField="UserID"
                                                            CssClass="NormalTextBox">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 5px">
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <div runat="server" id="div2">
                                                            <asp:LinkButton runat="server" ID="lnkSmallSave" CssClass="btn" CausesValidation="false"
                                                                OnClick="lnkSmallSave_Click"> <strong>Add</strong>   </asp:LinkButton>
                                                        </div>
                                                    </td>
                                                    <td style="width: 20px;">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div>
                                            <asp:Label ID="lblMsgTab" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdTableUser" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                 

                     <%--  <ajaxToolkit:TabPanel ID="tabAttachMents" runat="server">
                        <HeaderTemplate>
                            <strong>Attachments</strong>
                        </HeaderTemplate>
                        <ContentTemplate>

                             <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                            <div style="padding:10px;">
                            <table>
                                <tr>
                                    <td valign="top">
                                        <table>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:CheckBox runat="server" ID="chkAttachOutgoingEmails" Text="Attach Outgoing Emails"
                                                        TextAlign="Right" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    Save to Table
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlOutSaveToTable" CssClass="NormalTextBox"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlOutSaveToTable_SelectedIndexChanged">
                                                            <asp:ListItem Text="--Select Table--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    Save Recipient
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlOutSaveRecipient" CssClass="NormalTextBox">
                                                        <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    Save Subject to
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlOutSaveSubjectto" CssClass="NormalTextBox">
                                                         <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    Save Body to
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlOutSaveBodyTo" CssClass="NormalTextBox">
                                                         <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                    </td>
                                    <td valign="top">
                                        <table>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:CheckBox runat="server" ID="chkAttachIncomingEmails" Text="Attach Incoming Emails"
                                                        TextAlign="Right" />
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td align="right">
                                                    Save to Table
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlInSaveToTable" CssClass="NormalTextBox"
                                                     AutoPostBack="true" OnSelectedIndexChanged="ddlInSaveToTable_SelectedIndexChanged">
                                                         <asp:ListItem Text="--Select Table--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>


                                             <tr>
                                                <td align="right">
                                                    Save Sender
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlInSaveToSender" CssClass="NormalTextBox">
                                                        <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="right">
                                                    Save Subject to
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlInSaveSubJectTo" CssClass="NormalTextBox">
                                                         <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    Save Body to
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlInSaveBodyTo" CssClass="NormalTextBox">
                                                         <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    Save Attachment to
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlInSaveAttachmentTo" CssClass="NormalTextBox">
                                                         <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    Identifier
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlInIdentifier" CssClass="NormalTextBox">
                                                         <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    Note: Identifier must be between two hash <br />
                                                     characters like this #12345#
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                    <td valign="top">

                                     <asp:LinkButton runat="server" ID="lnkAttachementSave" OnClick="lnkAttachementSave_Click" CausesValidation="true">
                                                    <asp:Image runat="server" ID="Image4" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                        ToolTip="Save" />
                                                </asp:LinkButton>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div style="border: 1px solid; padding:10px 10px 10px 10px;">
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        Email
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtAttacmentEmail" Width="200px" CssClass="NormalTextBox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        Port
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtAttacmentPort" CssClass="NormalTextBox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        User Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtAttacmentUserName" Width="200px" CssClass="NormalTextBox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        Server
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtAttacmentServer" CssClass="NormalTextBox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        Password
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtAttacmentPassword" Width="200px"  
                                                           CssClass="NormalTextBox"  MaxLength="30"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        SSL
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList runat="server" ID="optAttachmentSSL" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            </div>
                                 </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>--%>


                    <ajaxToolkit:TabPanel ID="tabForms" runat="server" Visible="false">
                        <HeaderTemplate>
                            <strong>Forms</strong>
                        </HeaderTemplate>
                        <ContentTemplate>

                         <asp:UpdatePanel ID="upForms" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                        <div style="padding-left: 20px; padding-top: 10px;">
                                        <asp:GridView ID="grdFormSet" runat="server" AutoGenerateColumns="False" DataKeyNames="FormSetID"
                                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" CssClass="gridview"
                                            OnRowCommand="grdFormSet_RowCommand" OnRowDataBound="grdFormSet_RowDataBound" 
                                            AlternatingRowStyle-BackColor="#DCF2F0">
                                            <RowStyle CssClass="gridview_row" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("FormSetID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/App_Themes/Default/Images/icon_delete.gif"
                                                            CommandName="deletetype" CommandArgument='<%# Eval("FormSetID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/ChildTableDetail.aspx"
                                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" ToolTip="Edit" ID="hlEditDetail"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/ChildTableDetail.aspx"
                                                            ImageUrl="~/Pages/Pager/Images/add.png" ToolTip="Add Child Table" ID="hlAddDetail"></asp:HyperLink>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="sortHandle3">
                                                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/MoveIcon.png"
                                                            ToolTip="Drag and drop to change order" />
                                                        <input type="hidden" id='hfFormSetID' value='<%# Eval("FormSetID") %>' class='FormSetID' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              
                                                <asp:TemplateField HeaderText="Form">
                                                    <ItemTemplate>
                                                        <div style="padding-left: 10px;">
                                                            <asp:Label runat="server" ID="lblForm" Text='<%# Eval("FormSetName")%>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                               
                                            </Columns>
                                            <HeaderStyle CssClass="gridview_header" />
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <div runat="server" id="divEmptyDataFormSet" visible="false" style="padding-left: 20px;">
                                        <asp:HyperLink runat="server" ID="hlAddFormSet" Style="text-decoration: none;
                                            color: Black;" CssClass="popuplink">
                                            <asp:Image runat="server" ID="Image6" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                            No forms have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                                Add new form now.</strong>
                                        </asp:HyperLink>
                                    </div>
                                    <br />
                                     </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdFormSet" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>


                     <%--<ajaxToolkit:TabPanel ID="tabView" runat="server">
                        <HeaderTemplate>
                            <strong>Views</strong>
                        </HeaderTemplate>
                        <ContentTemplate>

                                     <dbg:OneView runat="server" ID="vdOne" />


                          </ContentTemplate>
                    </ajaxToolkit:TabPanel>--%>


                </ajaxToolkit:TabContainer>
            </td>
        </tr>
        <tr>
            <td colspan="3" >
                    
            </td>
        </tr>
        <tr>
            <td colspan="3" height="20px">
            </td>
        </tr>
        <tr>
            <td colspan="3">
               
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton runat="server" ID="lnkShowHistory" ClientIDMode="Static" Text="Show Change History"
                                        OnClick="lnkShowHistory_Click" CausesValidation="false"></asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkHideHistory" ClientIDMode="Static" Text="Hide Change History"
                                        CausesValidation="false" OnClick="lnkHideHistory_Click" Visible="false"></asp:LinkButton>
                                </td>
                                <td align="right">
                                    <asp:Label runat="server" ID="lblTableID" style="color:#C0C0C0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divHistory" runat="server" visible="false">
                            <div runat="server" id="div3">
                                <strong>Change History</strong>
                                 <br />
                                <dbg:dbgGridView ID="gvChangedLog" runat="server" GridLines="Both" CssClass="gridview"
                                    HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                    AllowSorting="false" DataKeyNames="DateAdded" HeaderStyle-ForeColor="Black" Width="100%"
                                    AutoGenerateColumns="false" PageSize="15" OnPreRender="gvChangedLog_PreRender"
                                    OnRowDataBound="gvChangedLog_RowDataBound">
                                    <PagerSettings Position="Top" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" ID="hlView" CssClass="popuplink">
                                                    <asp:Image runat="server" ID="imgView" ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                                </asp:HyperLink>
                                                <%--<asp:ImageButton ID="btnView" runat="server" ToolTip="View" 
                                                                        ImageUrl="~/App_Themes/Default/Images/iconShow.png"  />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="true" HeaderText="Updated Date" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="UpdateDate" runat="server" Text='<%# Eval("DateAdded") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="true" HeaderText="User">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUser" runat="server" Text='<%# Eval("User") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="true" HeaderText="Changed Field List">
                                            <ItemTemplate>
                                                <asp:Label ID="lblColumnList" runat="server" Text='<%# Eval("ColumnList") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridview_header" />
                                    <RowStyle CssClass="gridview_row" />
                                    <PagerTemplate>
                                        <asp:GridViewPager runat="server" ID="CL_Pager" HideFilter="true" HideAdd="true"
                                            HideDelete="true" OnBindTheGridToExport="CL_Pager_BindTheGridToExport" OnApplyFilter="CL_Pager_OnApplyFilter"
                                            OnBindTheGridAgain="CL_Pager_BindTheGridAgain" OnExportForCSV="CL_Pager_OnExportForCSV" />
                                    </PagerTemplate>
                                    <EmptyDataTemplate>
                                        <div style="padding-left: 100px;">
                                            No changes have been made yet.
                                        </div>
                                    </EmptyDataTemplate>
                                </dbg:dbgGridView>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvChangedLog" />
                        <asp:AsyncPostBackTrigger ControlID="lnkShowHistory" />
                        <asp:AsyncPostBackTrigger ControlID="lnkHideHistory" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <asp:Button runat="server" ID="btnRefreshGrid" ClientIDMode="Static" Style="display: none;"
        OnClick="btnRefreshGrid_Click" />
        <asp:Button runat="server" ID="btnRefreshColumns" ClientIDMode="Static" Style="display: none;"
        OnClick="btnRefreshColumns_Click" />
         
         <asp:Button runat="server" ID="btnRefreshForms" ClientIDMode="Static" Style="display: none;"
        OnClick="btnRefreshForms_Click" />
    <asp:Button runat="server" ID="btnRefreshTemplates" ClientIDMode="Static" Style="display: none;"
        OnClick="btnRefreshTemplates_Click" />
    <asp:Button runat="server" ID="btnOrderSC" ClientIDMode="Static" Style="display: none;"
        OnClick="btnOrderSC_Click" />
    <asp:Button runat="server" ID="btnOrderTC" ClientIDMode="Static" Style="display: none;"
        OnClick="btnOrderTC_Click" />

          <asp:Button runat="server" ID="btnOrderFS" ClientIDMode="Static" Style="display: none;"
        OnClick="btnOrderFS_Click" />
    

     <asp:Button runat="server" ID="btnTableRenameOK" ClientIDMode="Static" Style="display: none;" OnClick="btnTableRenameOK_Click" />
    <asp:Button runat="server" ID="btnTableRenameNo" ClientIDMode="Static" Style="display: none;" OnClick="btnTableRenameNo_Click" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfWebroot" />
    <br />
    <asp:Label runat="server" ID="lblDeleteAll" />
    <ajaxToolkit:ModalPopupExtender ID="mpeDeleteAll" runat="server" TargetControlID="lblDeleteAll"
        PopupControlID="pnlDeleteAll" BackgroundCssClass="modalBackground" OkControlID="lnkDeleteAllNo" />
    <asp:Panel ID="pnlDeleteAll" runat="server" Style="display: none">
        <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD;
            height: 220px; border-style: outset;">
            <div style="padding-top: 50px; padding: 20px;">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblDeleteRestoreMessage" Font-Bold="true" Text="Are you sure you want to delete this table?"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 100px;">
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkDeleteAllOK" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkDeleteAllOK_Click"> <strong>OK</strong></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkDeleteAllNo" CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server" id="trDeleteParmanent">
                        <td colspan="2">
                            <asp:CheckBox runat="server" ID="chkDeleteParmanent" TextAlign="Right" Text="I wish to delete this table permanently." />
                        </td>
                    </tr>
                    <tr runat="server" id="trUndo">
                        <td colspan="2">
                            <asp:CheckBox runat="server" ID="chkUndo" TextAlign="Right" Text="I will not be able to undo this action." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <table style="width: 50px; text-align: center">
                                        <tr>
                                            <td>
                                                <img alt="Processing..." src="../../Images/ajax.gif" />
                                            </td>
                                        </tr>
                                    </table>
                                </ProgressTemplate>
                            </asp:UpdateProgress>--%>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>
    <br />
    
</asp:Content>
