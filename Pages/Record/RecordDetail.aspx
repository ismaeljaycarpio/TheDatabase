<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="RecordDetail.aspx.cs" Inherits="Record_Record_Detail"
    EnableEventValidation="false" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/RecordList.ascx" TagName="ChildTable" TagPrefix="asp" %>
<%--<%@ Register Src="~/Pages/UserControl/DetailView.ascx" TagName="CTDetail" TagPrefix="asp" %>--%>
<%@ Register Src="~/Pages/UserControl/DetailEdit.ascx" TagName="CTDetail" TagPrefix="asp" %>
<%@ Register Src="~/Pages/UserControl/MessageList.ascx" TagName="MLList" TagPrefix="asp" %>

<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script src="../Document/Uploadify/jquery.uploadify.v2.1.4.js" type="text/javascript"></script>
    <script src="../Document/Uploadify/swfobject.js" type="text/javascript"></script>
    <link href="../Document/Uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%=Request.Url.Scheme+@"://maps.google.com/maps/api/js?sensor=false" %>"></script>

    <script src="<%=Request.Url.Scheme+@"://ajax.googleapis.com/ajax/libs/jqueryui/1.8.23/jquery-ui.min.js" %>"></script>
    <link href="<%=Request.Url.Scheme+@"://ajax.googleapis.com/ajax/libs/jqueryui/1.8.23/themes/base/jquery-ui.css" %>" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .topview {
            top: 50px;
            border: 1px solid #4F8FDD;
            padding: 10px;
        }

        table.multiple_cbl tbody {
            display: block; /*height: 160px;   */
            overflow: auto;
        }

        .paddingClass {
            padding-left: 10px;
            padding-right: 10px;
        }
    </style>
    <script type="text/javascript">
        function ValidatorUpdateDisplay(val) {
            //            if (typeof (val.display) == "string") {
            //                if (val.display == "None") {
            //                    return;
            //                }
            //                if (val.display == "Dynamic") {
            //                    val.style.display = val.isvalid ? "none" : "inline";
            //                    return;
            //                }

            //            } //radioV
            try {
                val.style.visibility = val.isvalid ? "hidden" : "visible";
                if (val.isvalid) {
                    if (val.controltovalidate.indexOf('radioV') > 0) {
                        document.getElementById(val.controltovalidate).style.border = '0px none';
                    }
                    else { document.getElementById(val.controltovalidate).style.border = '1px solid #909090'; }
                }
                else {
                    document.getElementById(val.controltovalidate).style.border = '2px solid red';
                }
            }
            catch (err) {
                //
            }
        }
        function setSelectedValue(selectObj, valueToSet) {
            //setTimeout(setSelectedValue, 1000);
            if (selectObj != null) {
                alert(selectObj.options.length);
                for (var i = 0; i < selectObj.options.length; i++) {
                    if (selectObj.options[i].value == valueToSet) {
                        selectObj.options[i].selected = true;
                        return;
                    }
                }
            }
        }


    </script>
    <script type="text/javascript">

        function ShowHideMainDivs(divSelected, lnk) {
            var hfCurrentSelectedTabLink = document.getElementsByName('hfCurrentSelectedTabLink');
            $(".showhidedivs_hide").hide();
            if (divSelected == null) {
                return;
            }
            if (lnk != null) {
                hfCurrentSelectedTabLink.value = lnk.id;
               //alert(hfCurrentSelectedTabLink.value);
            }
            $(".showhidedivs").hide();
            divSelected.style.display = 'block';
            if ($(".TablLinkClass") != null && lnk != null) {
                $(".TablLinkClass").css('font-weight', 'normal');
            }
            if (lnk != null) {
                lnk.style.fontWeight = 'bold';
            }

        }

        function ShowHideMainDivsName(divSelectedN, lnkN) {
            var hfCurrentSelectedTabLink = document.getElementsByName('hfCurrentSelectedTabLink');
            var divSelected = document.getElementsByName(divSelectedN);
            var lnk = document.getElementsByName(lnkN);

            if (divSelected == null) {
                return;
            }

            if (divSelected.style == null) {
                return;
            }



            if (lnk != null) {
                hfCurrentSelectedTabLink.value = lnk.id;
                //alert(hfCurrentSelectedTabLink.value);
            }




            $(".showhidedivs_hide").hide();
            $(".showhidedivs").hide();
            if (divSelected.style != null) {
                divSelected.style.display = 'block';
            }


            if ($(".TablLinkClass") != null && lnk != null) {
                $(".TablLinkClass").css('font-weight', 'normal');
            }
            if (lnk != null && lnk.style != null) {
                lnk.style.fontWeight = 'bold';
            }

        }


        function ChangeTab(index) {
            try {
                $find("ctl00_HomeContentPlaceHolder_tabDetail").set_activeTabIndex(index);
            }
            catch (err) {
                alert('There are no Tab for this now, please try at the time of edit this record.')
            }

        }

        function GetOptValue(optName) {
            var rates = document.getElementsByName(optName);
            var rate_value;
            for (var i = 0; i < rates.length; i++) {
                if (rates[i].checked) {
                    rate_value = rates[i].value;
                }
            }
            return rate_value;

        }

        //        function HideHistory() {
        //            $("#divHistory").fadeOut();
        //            $("#lnkShowHistory").fadeIn();
        //            $("#lnkHideHistory").fadeOut();

        //        }
        function ClientActiveTabChanged() {
            if ($find("ctl00_HomeContentPlaceHolder_tabDetail").get_activeTabIndex() == 0) {

                $("#divMainSaveEditAddetc").fadeIn();
                $("#divChangeHistory").fadeIn();
            }
            else {

                $("#lnkHiddenSave").trigger('click');
                $find("ctl00_HomeContentPlaceHolder_tabDetail").set_activeTabIndex(0);
                //                $("#divMainSaveEditAddetc").fadeOut();
            }

        }

        function ClientActiveTabChangedEdit() {
            if ($find("ctl00_HomeContentPlaceHolder_tabDetail").get_activeTabIndex() == 0) {

                $("#divMainSaveEditAddetc").fadeIn();
                $("#divChangeHistory").fadeIn();

                //                if (document.getElementById("lnkShowHistory") != null) {
                //                    if (document.getElementById("lnkShowHistory").style.display != 'none') {
                //                        $("#divHistory").fadeOut();
                //                    }
                //                }


            }
            else {

                $("#divMainSaveEditAddetc").fadeOut();
                $("#divChangeHistory").fadeOut();
            }

        }


        //        function ShowHistory() {
        //            $("#divHistory").fadeIn();
        //            $("#lnkShowHistory").fadeOut();
        //            $("#lnkHideHistory").fadeIn();

        //        }

        function CheckMyText(sender, args) {
            var compare = RegExp("^([0-1]?[0-9]|2[0-3]):([0-5][0-9])(:[0-5][0-9])?$");
            args.IsValid = compare.test(args.Value);
            return;
        }

        function abc() {
            var b = document.getElementById('<%= lnkSaveClose.ClientID %>');
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

        function OpenAuditDetail(val, id) {


            var left = (screen.width / 2) - (500 / 2);
            var top = (screen.height / 2) - (500 / 2);

            window.open("AuditDetail.aspx?UpdatedDate=" + encodeURIComponent(val) + "&RecordID=" + id, "List", "scrollbars=yes,resizable=yes,width=500,height=500,top=" + top + ",left=" + left);


        }


    </script>

    <asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server">
        <ProgressTemplate>
            <table style="width: 100%; height: 100%; text-align: center;">
                <tr valign="middle">
                    <td>
                        <p style="font-weight: bold;">
                            Please wait...
                        </p>
                        <asp:Image ID="Image5" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                    </td>
                </tr>
            </table>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="upDetailDynamic" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div runat="server" id="divHeaderColorDetail" style="">
                 <asp:HiddenField runat="server" ID="hfCurrentSelectedTabLink" ClientIDMode="Static" />
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 500px;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                            <br />
                            <br />
                            <asp:Label runat="server" ID="lblHeaderName" Style="padding-left: 5px;"> </asp:Label>
                            <asp:HiddenField runat="server" ID="hfRecordID" Value="-1" ClientIDMode="Static" />
                        </td>
                        <td align="left">
                            <table class="DetailPageControls">
                                <tr>
                                    <td></td>
                                    <td>
                                        <div>


                                            <div runat="server" id="trMainSave">
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
                                                            <div id="divMainSaveEditAddetc">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <div runat="server" id="divEdit" visible="false">
                                                                                <asp:HyperLink runat="server" ID="hlEditLink">
                                                                            <asp:Image runat="server" ID="ImageEdit" ImageUrl="~/App_Themes/Default/images/Edit_big.png"
                                                                                ToolTip="Edit" />
                                                                                </asp:HyperLink>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div runat="server" id="divSaveClose">
                                                                                <%--  <asp:UpdatePanel ID="upkSaveClose" runat="server" UpdateMode="Always">
                                                                            <ContentTemplate>--%>
                                                                                <table cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:LinkButton runat="server" ID="lnkSaveClose" OnClick="lnkSaveClose_Click" CausesValidation="true">
                                                                                        <asp:Image runat="server" ID="imgSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                                                            ToolTip="Save" />
                                                                                            </asp:LinkButton>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton runat="server" ID="lnkSaveAndAdd" OnClick="lnkSaveAndAdd_Click" CausesValidation="true"
                                                                                                Visible="false">
                                                                                        <asp:Image runat="server" ID="Image11" ImageUrl="~/App_Themes/Default/images/SaveAndAdd2.png"
                                                                                            ToolTip="Save and Add" />
                                                                                            </asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <%--  </ContentTemplate>
                                                                           
                                                                        </asp:UpdatePanel>--%>
                                                                            </div>
                                                                            <asp:Button Style="display: none;" runat="server" ID="lnkHiddenSave" ClientIDMode="Static"
                                                                                OnClick="tabDetail_ActiveTabChanged" CausesValidation="true"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <div runat="server" id="divSaveAndStay" visible="false">
                                                                                <%--<asp:UpdatePanel ID="upSaveAndStay" runat="server" UpdateMode="Always">
                                                                            <ContentTemplate>--%>
                                                                                <asp:LinkButton runat="server" ID="lnkSaveAndStay" OnClick="lnkSaveAndStay_Click"
                                                                                    CausesValidation="true">
                                                                            <asp:Image runat="server" ID="Image6" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                                                ToolTip="Save and Stay" />
                                                                                </asp:LinkButton>
                                                                                <%--  </ContentTemplate>
                                                                        </asp:UpdatePanel>--%>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <%--<div runat="server" id="divSaveAndAdd" visible="false">
                                                                        <asp:HyperLink runat="server" ID="hlSaveAndAdd">
                                                                            <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/BigAdd.png"
                                                                                ToolTip="Add" />
                                                                        </asp:HyperLink>
                                                                    </div>--%>
                                                                        </td>
                                                                        <td>
                                                                            <div runat="server" id="divWordExport" visible="false">
                                                                                <asp:LinkButton runat="server" ID="lnkWordWxport" OnClick="lnkWordWxport_Click">
                                                                            <asp:Image runat="server" ID="Image3" ImageUrl="~/App_Themes/Default/images/WordExport.png"
                                                                                ToolTip="Document Generation" />
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div runat="server" id="divPrint" visible="false">
                                                                                <asp:HyperLink runat="server" ID="hlPrint" Target="_blank">
                                                                            <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/print-icon.png"
                                                                                ToolTip="Print" />
                                                                                </asp:HyperLink>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                    <td valign="middle" align="left">
                                        <table runat="server" id="tblNavigateRecords" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:HyperLink runat="server" ID="hlNavigatePrev" Visible="false">
                                                <asp:Image ID="Image7" runat="server" ImageUrl="~/App_Themes/Default/Images/bullet_arrow_left.png"
                                                    ToolTip="Previous Record" />
                                                    </asp:HyperLink>
                                                    <asp:LinkButton runat="server" ID="lnkNavigatePrev" OnClick="lnkNavigatePrev_Click">
                                                <asp:Image ID="Image9" runat="server" ImageUrl="~/App_Themes/Default/Images/bullet_arrow_left.png"
                                                    ToolTip="Previous Record" />
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:HyperLink runat="server" ID="hlNavigateNext" Visible="false">
                                                <asp:Image ID="Image8" runat="server" ImageUrl="~/App_Themes/Default/Images/bullet_arrow_right.png"
                                                    ToolTip="Next Record" />
                                                    </asp:HyperLink>
                                                    <asp:LinkButton runat="server" ID="lnkNavigateNext" OnClick="lnkNavigateNext_Click">
                                                <asp:Image ID="Image10" runat="server" ImageUrl="~/App_Themes/Default/Images/bullet_arrow_right.png"
                                                    ToolTip="Next Record" />
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 20px;">
                            <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                        <ContentTemplate>--%>
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <%--/ContentTemplate>
                    </asp:UpdatePanel>--%>

                            <div runat="server" id="divValidWarningGrid" visible="false" style="border: solid 1px #909090;">


                                <table>
                                    <tr>
                                        <td>
                                            <strong style="font-size: 14px;">Validation Results</strong><br />

                                            <div style="padding-left: 10px;">

                                                <asp:GridView ID="gvValidWarningGrid" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                                    HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" CssClass="gridview"
                                                    OnRowDataBound="gvValidWarningGrid_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblColumnID" runat="server" Text='<%# Eval("ColumnID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValidationType" runat="server" Text='<%# Eval("ValidationType") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkIgnore" Checked='<%#  Eval("Ignore")=="yes"?true:false %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFullMsg" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValidWarningMsg" runat="server" Text='<%# Eval("ValidWarningMsg") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValidWarningFormula" runat="server" Text='<%# Eval("ValidWarningFormula") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField Visible="false">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>

                                                </asp:GridView>

                                                <span runat="server" id="spnIgnoreTick" style="font-size: 10px; padding-top: 5px;">To ignore tick the checkbox and press Save again</span>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <%--<asp:LinkButton runat="server" ID="lnkValidWarningRefresh" CssClass="btn" CausesValidation="false"
                                        OnClick="lnkValidWarningRefresh_Click"> <strong>Refresh</strong> </asp:LinkButton>--%>

                                        </td>
                                    </tr>

                                </table>



                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel runat="server" ID="pnlDetailWhole">
                <div runat="server" id="div1" onkeypress="abc();">

                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 10px;"></td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 10px;"></td>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">

                                                <tr>
                                                    <td valign="top"></td>
                                                    <td valign="top">
                                                        <%-- <asp:UpdatePanel ID="upDetailDynamic" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>--%>
                                                        <div id="search" style="padding-bottom: 10px">
                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                                                ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct the following errors:" />
                                                           
                                                        </div>
                                                        <asp:Panel ID="Panel2" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <div runat="server" id="divDynamic" class="NormalTextBox">
                                                                            <ajaxToolkit:TabContainer ID="tabDetail" runat="server" ActiveTabIndex="0" CssClass="DBGTab"
                                                                                Visible="true">
                                                                                <ajaxToolkit:TabPanel ID="tpDetail" runat="server">
                                                                                    <HeaderTemplate>
                                                                                        <%--<strong>Detail</strong>--%>
                                                                                        <asp:Label runat="server" Font-Bold="true" ID="lblFristTabTableName" Text="Detail"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ContentTemplate>
                                                                                        <asp:Panel runat="server" ID="pnlDetail">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td valign="top">
                                                                                                        <asp:Panel runat="server" ID="pnlMain">
                                                                                                            <asp:Panel runat="server" ID="pnlTabHeading">
                                                                                                            </asp:Panel>
                                                                                                            <asp:Panel runat="server" ID="pnlDetailTab" CssClass="showhidedivs">
                                                                                                                <table runat="server" id="tblMain">
                                                                                                                    <tr>
                                                                                                                        <td valign="top">
                                                                                                                            <table id="tblLeft" runat="server" visible="true" cellpadding="3">
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                        <td valign="top">
                                                                                                                            <table id="tblRight" runat="server" visible="true" cellpadding="3" style="margin-left: 10px;">
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td colspan="2" align="right">
                                                                                                                            <div runat="server" id="divSaveBottom" visible="false">
                                                                                                                                <table cellpadding="0" cellspacing="0">
                                                                                                                                    <tr>
                                                                                                                                        <td></td>
                                                                                                                                        <td valign="middle">
                                                                                                                                            <div runat="server" id="divSaveBottonSave" style="padding: 5px 15px 5px 15px;">
                                                                                                                                                <asp:LinkButton runat="server" ID="lnkSaveRRP" OnClick="lnkSaveClose_Click" CausesValidation="true">
                                                                                                                                            <table>
                                                                                                                                                <tr>
                                                                                                                                                    <td>
                                                                                                                                                        <asp:Image runat="server" ID="imgSaveRRP" ImageUrl="~/Pages/Pager/Images/rrp/save.png"
                                                                                                                                                            ToolTip="Save" />
                                                                                                                                                        <%--<img title="Save" src="../../Pages/Pager/Images/rrp/save.png" alt="Save" />--%>
                                                                                                                                                    </td>
                                                                                                                                                    <td valign="middle">
                                                                                                                                                        <strong style="color: #ffffff;">SAVE</strong>
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>
                                                                                                                                                </asp:LinkButton>
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </asp:Panel>
                                                                                                        </asp:Panel>
                                                                                                    </td>
                                                                                                    <td style="width: 10px;"></td>
                                                                                                    <td>
                                                                                                        <div runat="server" id="divProgressHisotry" visible="false" class="topview">
                                                                                                            <strong style="color: #0299C6; font-size: 15px;">Progress History</strong><br />
                                                                                                            <br />
                                                                                                            <asp:GridView ID="grdProgressHisotry" runat="server" AutoGenerateColumns="false"
                                                                                                                HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" CssClass="gridview"
                                                                                                                OnRowDataBound="grdProgressHisotry_RowDataBound">
                                                                                                                <RowStyle CssClass="gridview_row" />
                                                                                                                <Columns>
                                                                                                                    <%--<asp:TemplateField Visible="true">   DataKeyNames="ProgressColumnID"
                                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkProgressStatus" runat="server" AutoPostBack="false" Enabled="false" />
                                                                                                                <asp:Label runat="server" ID="lblFormSetName"></asp:Label>
                                                                                                                <asp:HyperLink runat="server" ID="hlProgressColumnID"></asp:HyperLink>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>--%>
                                                                                                                </Columns>
                                                                                                                <HeaderStyle CssClass="gridview_header" />
                                                                                                            </asp:GridView>
                                                                                                            <br />
                                                                                                            <br />
                                                                                                            <strong>Key:</strong>
                                                                                                            <br />
                                                                                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/empty.png" />
                                                                                                            &nbsp Not done - click to start
                                                                                                    <br />
                                                                                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Default/Images/incomplete.png" />
                                                                                                            &nbsp Part done - click to Resume
                                                                                                    <br />
                                                                                                            <asp:Image ID="Image5" runat="server" ImageUrl="~/App_Themes/Default/Images/complete.png" />
                                                                                                            &nbsp Complete - click to view
                                                                                                    <br />
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                            <div>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td style="width: 50px;"></td>
                                                                                                        <td colspan="2"></td>
                                                                                                        <td style="width: 30px;"></td>
                                                                                                        <td>
                                                                                                            <table runat="server" id="trReasonForChange" visible="false">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <strong runat="server" id="stgReasonForChange">Reason for change</strong>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox runat="server" ID="txtReasonForChange" Width="300px" CssClass="NormalTextBox"
                                                                                                                            TextMode="MultiLine" Height="50px"></asp:TextBox>
                                                                                                                        <asp:RequiredFieldValidator runat="server" ID="rfvReasonForChange" ControlToValidate="txtReasonForChange"
                                                                                                                            ErrorMessage="Reason for change is Required" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                            <br />
                                                                                        </asp:Panel>
                                                                                    </ContentTemplate>
                                                                                </ajaxToolkit:TabPanel>
                                                                            </ajaxToolkit:TabContainer>
                                                                        </div>
                                                                        <%--//oliver <begin> Ticket 1476--%>
                                                                        <div style="margin: 10px;">
                                                                            <div style="float: left; margin-left: -30px;">
                                                                                <asp:LinkButton runat="server" ID="lnkShowHistory" ClientIDMode="Static" Text="Show Change History"
                                                                                    OnClick="lnkShowHistory_Click" CausesValidation="false"></asp:LinkButton>
                                                                                <asp:LinkButton runat="server" ID="lnkHideHistory" ClientIDMode="Static" Text="Hide Change History"
                                                                                    Visible="false" CausesValidation="false" OnClick="lnkHideHistory_Click"></asp:LinkButton>
                                                                            </div>
                                                                            <div style="float: right; color: #C0C0C0;">
                                                                                Record ID:
                                                                                <asp:Label ID="lblRecordID" runat="server" Text=""></asp:Label>
                                                                            </div>
                                                                            <div style="float: left;"></div>
                                                                        </div>
                                                                        <%--//oliver <end>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <%--  </ContentTemplate>
                                                 
                                                    
                                                </asp:UpdatePanel>--%>
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
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkWordWxport" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <div>
        <asp:UpdatePanel ID="upChangeHistory" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divChangeHistory">
                    <table runat="server" id="tblChangeHistory" visible="false">
                        <%--<tr>
                            <td colspan="3">
                                <asp:LinkButton runat="server" ID="lnkShowHistory" ClientIDMode="Static" Text="Show Change History"
                                    OnClick="lnkShowHistory_Click" CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnkHideHistory" ClientIDMode="Static" Text="Hide Change History"
                                    Visible="false" CausesValidation="false" OnClick="lnkHideHistory_Click"></asp:LinkButton>
                            </td>
                        </tr>--%>
                        <tr runat="server" id="trTab" visible="false">
                            <td colspan="3">
                                <div id="divHistory">
                                    <div style="float: left; width: 55%;">
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
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Updated Date" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="UpdateDate" runat="server" Text='<%# Eval("DateAdded") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="User">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUser" runat="server" Text='<%# Eval("User") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Changed Column List">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblColumnList" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="true" HeaderText="Reason for change">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResonForChange" runat="server" Text='<%# Eval("ResonForChange") %>'></asp:Label>
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
                                    <div runat="server" id="divLastUpdatedInfo" visible="true" style="float: right; width: 45%; margin-top: 50px; position: relative;">
                                        <!--oliver: additional fields here...-->
                                        <div style="width: 100%; margin-left: 60px;">
                                            <div style="float: left; width: 100px; padding: 10px 0 0;"><b>Created By:</b></div>
                                            <div style="float: left; width: 180px; padding: 10px 0 0;">
                                                <asp:Label ID="lblCreatedBy" runat="server" Text=""></asp:Label></div>
                                            <div style="clear: both;"></div>
                                        </div>
                                        <div style="width: 100%; margin-left: 60px;">
                                            <div style="float: left; width: 100px; padding: 10px 0 0;"><b>Date Created:</b></div>
                                            <div style="float: left; width: 180px; padding: 10px 0 0;">
                                                <asp:Label ID="lblDateCreated" runat="server" Text=""></asp:Label></div>
                                            <div style="clear: both;"></div>
                                        </div>
                                        <div style="width: 100%; margin-left: 60px;">
                                            <div style="float: left; width: 100px; padding: 10px 0 0;">
                                                <asp:Label ID="lblUpdatedByText" runat="server" Text="<b>Updated By:</b>"></asp:Label></div>
                                            <div style="float: left; width: 180px; padding: 10px 0 0;">
                                                <asp:Label ID="lblUpdatedBy" runat="server" Text=""></asp:Label></div>
                                            <div style="clear: both;"></div>
                                        </div>
                                        <div style="width: 100%; margin-left: 60px;">
                                            <div style="float: left; width: 100px; padding: 10px 0 0;">
                                                <asp:Label ID="lblDateUpdatedText" runat="server" Text="<b>Date Updated:</b>"></asp:Label></div>
                                            <div style="float: left; width: 180px; padding: 10px 0 0;">
                                                <asp:Label ID="lblDateUpdated" runat="server" Text=""></asp:Label></div>
                                            <div style="clear: both;"></div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvChangedLog" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <br />
    <asp:Label runat="server" ID="lblModalWordExport" />
    <ajaxToolkit:ModalPopupExtender ID="mpeModalWordExport" ClientIDMode="Static" runat="server"
        BehaviorID="popup" TargetControlID="lblModalWordExport" PopupControlID="pnlModalWordExport"
        BackgroundCssClass="modalBackground" OkControlID="lnkWordExportCancel" />
    <asp:Panel ID="pnlModalWordExport" runat="server" Style="display: none">
        <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD; height: 200px; border-style: outset;">
            <div style="padding-top: 50px; padding: 20px;">
                <table>
                    <tr>
                        <td colspan="2">
                            <h3>Document Generation</h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <strong>Template:</strong>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlDataRetriever" DataTextField="FileName" DataValueField="DocTemplateID"
                                AutoPostBack="false" CssClass="NormalTextBox" Width="350px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <table style="padding: 50px 0 0 85px;">
                                <tr>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkWordExportOK" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkWordExportOK_Click"> <strong>OK</strong></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkWordExportCancel" CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <%--<asp:HyperLink runat="server" ID="hlChangeAccountType" NavigateUrl="#" CssClass="popuplink"
                    onclick="$find('popup').hide(); return false;"> click here.</asp:HyperLink>--%>
            </div>
            <%--<div style="text-align: center; padding-left: 250px;">
                
            </div>--%>
        </div>
    </asp:Panel>
    <br />
    <%-- OnClick="btnReloadMe_Click"--%>
    <asp:Button ID="btnReloadMe" runat="server" ClientIDMode="Static" OnClientClick="ReloadMe();return false;"
        Style="display: none;" />
</asp:Content>
