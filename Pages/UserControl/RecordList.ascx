<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordList.ascx.cs" Inherits="Pages_UserControl_RecordList" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/ControlByColumn.ascx" TagName="ControlByColumn"
    TagPrefix="dbg" %>
<%--<%@ Register Src="~/Pages/UserControl/ControlByColumnValue.ascx" TagName="cbcValue" TagPrefix="dbg" %>--%>
<%@ Register Src="~/Pages/UserControl/ViewDetail.ascx" TagName="OneView" TagPrefix="dbg" %>
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




<asp:Literal ID="ltScriptHere" runat="server"></asp:Literal>
<asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>

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

    function AddClick() {

        $('#ctl00_HomeContentPlaceHolder_rlOne_btnAdd').trigger('click');
    }
</script>
<script language="javascript" type="text/javascript">
    function SelectAllCheckboxes(spanChk) {

        // alert($(spanChk).attr('id'));
        checkAll(spanChk);
        var GridView = spanChk.parentNode.parentNode.parentNode;

        //alert($(GridView).attr('id'));
        // alert(GridView.id);

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



    function SelectAllCheckboxesHR(spanChk, GridView) {

        checkAllHR(spanChk, GridView);
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


    function CreateFile() {

        document.getElementById('btnEmail').click();

    }



</script>


<script type="text/javascript">

    $.fn.visibleHeight = function () {
        var elBottom, elTop, scrollBot, scrollTop, visibleBottom, visibleTop;
        scrollTop = $(window).scrollTop();
        scrollBot = scrollTop + $(window).height();
        elTop = this.offset().top;
        elBottom = elTop + this.outerHeight();
        visibleTop = elTop < scrollTop ? scrollTop : elTop;
        visibleBottom = elBottom > scrollBot ? scrollBot : elBottom;
        return visibleBottom - visibleTop
    }

    function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {



        var tbl = document.getElementById(gridId);

        //alert($("#DivMainContent").visibleHeight());
        height = $("#DivMainContent").visibleHeight();
        height = height - 65;
        if (tbl) {
            var DivPR = document.getElementById('DivPagerRow');
            var DivHR = document.getElementById('DivHeaderRow');
            var DivMC = document.getElementById('DivMainContent');
            var DivFR = document.getElementById('DivFooterRow');
            var DivR = document.getElementById('DivRoot');
            //var scrollbarWidth = DivMC.offsetWidth - DivMC.clientWidth;

            //*** Set divheaderRow Properties ****
            var oWidth = $(tbl).outerWidth();
            width = $(tbl).width();
            var iWidth = $(tbl).innerWidth();
            headerHeight = DivHR.style.height;
            var paregHeight = 45;
            $(DivR).width(width - 20);

            //pager
            DivPR.style.height = paregHeight + 'px'; // headerHeight / 2
            //DivPR.style.width = (parseInt(width)) + 'px';
            //$(DivPR).outerWidth(oWidth - 20);
            $(DivPR).width(width - 20);
            //$(DivPR).innerWidth(iWidth - 20);

            DivPR.style.position = 'relative';
            DivPR.style.top = '0px';
            //DivPR.style.verticalAlign = 'top';

            DivHR.style.height = headerHeight + 'px';// headerHeight/2
            //$(DivHR).outerWidth(oWidth - 20 );
            $(DivHR).width(width - 20);
            //$(DivHR).innerWidth(iWidth - 20 );


            DivHR.style.position = 'relative';
            DivHR.style.top = '0px';
            // DivHR.style.verticalAlign = 'top';
            //DivHR.rules = "none";


            //*** Set divMainContent Properties ****
            //$(DivMC).outerWidth(oWidth);
            $(DivMC).width(width);
            //$(DivMC).innerWidth(iWidth);

            DivMC.style.height = height + 'px';
            DivMC.style.position = 'relative';
            DivMC.style.top = "0px"; //(headerHeight) + 'px';// //
            DivMC.style.zIndex = '0';




            if (isFooter) {
                //*** Set divFooterRow Properties ****
                DivFR.style.width = (parseInt(width)) + 'px';
                DivFR.style.position = 'relative';
                DivFR.style.top = -(headerHeight) + 'px';
                DivFR.style.verticalAlign = 'top';
                DivFR.style.paddingtop = '2px';


                var tblfr = tbl.cloneNode(true);
                tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
                var tblBody = document.createElement('tbody');
                tblfr.style.width = '100%';
                tblfr.cellSpacing = "0";
                tblfr.border = "0px";
                tblfr.rules = "none";
                //*****In the case of Footer Row *******
                tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
                tblfr.appendChild(tblBody);
                DivFR.appendChild(tblfr);
            }
            //****Copy Header in divHeaderRow****

            var tblPR = tbl.cloneNode(true);
            tblPR.removeChild(tblPR.getElementsByTagName('tbody')[0]);
            $(tblPR).attr('id', gridId + 'PR');
            $(tblPR).css({ minWidth: "980px" });

            var tblHR = tbl.cloneNode(true);
            tblHR.removeChild(tblHR.getElementsByTagName('tbody')[0]);
            $(tblHR).attr('id', gridId + 'HR');



            var tblBodyPR = document.createElement('tbody');
            var tblBodyHR = document.createElement('tbody');


            tblBodyPR.appendChild(tbl.rows[0]);
            tblBodyHR.appendChild(tbl.rows[0]);

            tblPR.appendChild(tblBodyPR);
            tblHR.appendChild(tblBodyHR);

            DivPR.appendChild(tblPR);
            DivHR.appendChild(tblHR);

            $('#ctl00_HomeContentPlaceHolder_rlOne_UpdateProgress1').fadeOut();
        }
    }

    function SetStyleEvent() {

        $('#ctl00_HomeContentPlaceHolder_rlOne_UpdateProgress1').fadeIn();

        var DivPR = document.getElementById('DivPagerRow');
        var DivHR = document.getElementById('DivHeaderRow');
        var DivMC = document.getElementById('DivMainContent');
        var DivFR = document.getElementById('DivFooterRow');
        var DivR = document.getElementById('DivRoot');

        DivMC.style.overflowY = 'auto';
        DivMC.style.overflowX = 'hidden';
        DivMC.style.minWidth = "1000px";

        DivPR.style.minWidth = "980px";
        DivHR.style.minWidth = "1000px";
        DivFR.style.minWidth = "1000px";
        DivR.style.minWidth = "980px";

        DivMC.style.paddingRight = '17px';
        DivHR.style.paddingRight = '17px';
        DivR.style.overflow = 'hidden';


        var tbl = document.getElementById('ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid');


        var chkAll = $(tbl.rows[1]).find('#ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid_ctl02_chkAll');
        $(chkAll).attr('onclick', 'javascript: SelectAllCheckboxesHR(this,ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid);');

        var iTD0 = 0;
        $(tbl.rows[1]).find('th').each(function () {

            var aTH = $(tbl.rows[1]).find('th').eq(iTD0);
            var aTD = $(tbl.rows[2]).find('td').eq(iTD0);
            //$(aTH).outerWidth($(tbl.rows[2]).find('td').eq(iTD0).outerWidth());

            // var iMaxWidth = 0;
            // var iRow=0;
            // $("#ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid tr").each(function () {

            //     if (iRow > 1)
            //     {
            //         //var cell = $('table#ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid tr:nth-child(' + iRow + ') td:nth-child(' + iTD0 + ')');
            //         //if( $('#ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid tr:eq(' + iRow + ')') !=
            //         //    $('#ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid tr').last())
            //         //{
            //             var cell = $('#ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid tr:eq(' + iRow + ') td:eq(' + iTD0 + ')');
            //             iMaxWidth = Math.max(iMaxWidth, $(cell).width());
            //        // }


            //     }
            //     else if (iRow==1)
            //     {
            //         var cell = $('#ctl00_HomeContentPlaceHolder_rlOne_gvTheGrid tr:eq(' + iRow + ') th:eq(' + iTD0 + ')');
            //         iMaxWidth = Math.max(iMaxWidth, $(cell).width());
            //     }


            //     iRow = iRow + 1;
            // });
            //// alert(iRow);
            // $(aTH).width(iMaxWidth);
            // $(aTD).width(iMaxWidth);

            //$(aTH).css({width:$(tbl.rows[2]).find('td').eq(iTD0).width()});
            //$(aTD).css({ width: $(tbl.rows[2]).find('td').eq(iTD0).width() });

            $(aTH).width($(tbl.rows[2]).find('td').eq(iTD0).width());
            $(aTD).width($(tbl.rows[2]).find('td').eq(iTD0).width());

            iTD0 = iTD0 + 1;
        });

        //var iTD0 = 0;
        //$(tbl.rows[2]).find('td').each(function () {

        //    var aTH = $(tbl.rows[2]).find('td').eq(iTD0);

        //    //$(aTH).outerWidth($(tbl.rows[2]).find('td').eq(iTD0).outerWidth());
        //    $(aTH).width($(tbl.rows[2]).find('td').eq(iTD0).width());
        //    //$(aTH).outerWidth($(tbl.rows[2]).find('td').eq(iTD0).outerWidth());
        //    iTD0 = iTD0 + 1;
        //});

        //var iTH = 0;
        //$(tbl.rows[1]).find('th').each(function () {
        //    var iTD = 0;
        //    $(tbl.rows[2]).find('td').each(function () {
        //        if (iTD == iTH) {
        //            var aTH = $(tbl.rows[1]).find('th').eq(iTD);
        //            //$(aTH).outerWidth($(tbl.rows[2]).find('td').eq(iTD).outerWidth());
        //            $(aTH).width($(tbl.rows[2]).find('td').eq(iTD).width());
        //            //$(aTH).innerWidth($(tbl.rows[2]).find('td').eq(iTD).innerWidth());
        //        }
        //        iTD = iTD + 1;
        //    });           
        //    iTH = iTH + 1;
        //});


        //var scrollbarWidth = DivMC.offsetWidth - DivMC.clientWidth;
        //var aTHL = $(tbl.rows[1]).find('th').eq(iTH - 1);

        //$(aTHL).outerWidth($(aTHL).outerWidth() + scrollbarWidth);
        //$(aTHL).width($(aTHL).width() + scrollbarWidth);
        //$(aTHL).innerWidth($(aTHL).innerWidth() + scrollbarWidth);

    }

    function OnScrollDiv(Scrollablediv) {
        //DivPagerRow
        document.getElementById('DivPagerRow').scrollLeft = Scrollablediv.scrollLeft;
        document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
        document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
    }

    function autoResizeMe(id, w) {
        //var newheight;
        var newwidth;

        if (document.getElementById) {
            //newheight = document.getElementById(id).contentWindow.document.body.scrollHeight;
            // var newwidthS = document.getElementById(id).contentWindow.document.body.scrollWidth;
            newwidth = $(window).width() - 100;
            // document.getElementById(id).height = (newheight) + "px";
            //alert(newwidth);
            if (parseInt(w) == 2) {
                document.getElementById(id).width = (newwidth / parseInt(w)) + "px";
            }
            else {

                document.getElementById(id).width = (newwidth) + "px";

            }

        }

    }


</script>



<style type="text/css">
    .headerlink a {
        text-decoration: none;
    }
</style>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div runat="server" id="divRecordListTop">
            <table cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td colspan="3" align="left">
                        <table runat="server" id="tblTopCaption" width="100%">
                            <%-- 1000px--%>
                            <tr runat="server" id="trRecordListTitle">
                                <td align="left" style="width: 50%;" runat="server" id="tdTopTitile">
                                    <span class="TopTitle">
                                        <asp:Label ID="lblTitle" runat="server" Text="Records" Visible="false"> </asp:Label>
                                        <%--<asp:Label ID="lblRecords" runat="server" Text="Records:" Visible="true"> </asp:Label>--%>
                                        <asp:HiddenField runat="server" ID="hfViewID" />
                                        <asp:HiddenField runat="server" ID="hfHidePagerGoButton" />

                                    </span>
                                    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlTableMenu" CssClass="TopTitle"
                                        DataValueField="TableID" DataTextField="TableName" OnSelectedIndexChanged="ddlTableMenu_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 50px;">
                                    <div style="width: 50px;">
                                        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 50px; text-align: center">
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
                                <td runat="server" id="tdTopButtons">
                                    <table style="width: 100%; text-align: right; padding-right: 42px;" class="ListConfigControl">
                                        <tr>

                                            <td></td>
                                            <td style="width: 50px;">
                                                <div runat="server" id="divShowGraph" visible="true">
                                                    <asp:HyperLink runat="server" ID="hlShowGraph" CssClass="ButtonLink">
                                            <asp:Image runat="server" ID="imgShowGraph" ImageUrl="~/App_Themes/Default/images/Graph.png"
                                                ToolTip="Graph" />
                                                    </asp:HyperLink>
                                                </div>
                                            </td>
                                            <td style="width: 50px;">
                                                <div runat="server" id="divUpload" visible="true">
                                                    <asp:HyperLink runat="server" ID="hlUpload" CssClass="ButtonLink">
                                            <asp:Image runat="server" ID="imgUpload" ImageUrl="~/App_Themes/Default/images/Upload.png"
                                                ToolTip="Upload" />
                                                    </asp:HyperLink>
                                                </div>
                                            </td>
                                            <td style="width: 50px;" runat="server" id="divEmail">
                                                <div>
                                                    <asp:ImageButton ID="ibEmail" runat="server" ImageUrl="~/App_Themes/Default/images/email.png"
                                                        ToolTip="Email" OnClick="ibEmail_Click" />
                                                </div>
                                            </td>
                                            <td style="width: 50px;">
                                                <div runat="server" id="divConfig" visible="false">
                                                    <asp:HyperLink runat="server" ID="hlConfig" CssClass="ButtonLink">
                                            <asp:Image runat="server" ID="imgConfig" ImageUrl="~/App_Themes/Default/images/Config.png"
                                                ToolTip="Configure" />
                                                    </asp:HyperLink>
                                                </div>
                                            </td>
                                            <td runat="server" id="divBatches" visible="false">
                                                <div>
                                                    <asp:HyperLink runat="server" ID="hlBatches" CssClass="btn"> <strong>My Batches</strong> </asp:HyperLink>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px;">
            <table border="0" cellpadding="0" cellspacing="0" align="left" style="width: 100%;">
                <%-- 1000px--%>
                <tr>
                    <td colspan="3" height="13"></td>
                </tr>
                <tr>
                    <td valign="top"></td>
                    <td valign="top">
                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="lnkSearch" Style="width: 100%">
                            <div runat="server" id="divSearch" onclick="abc();" class="searchcorner" style="width: 100%;">
                                <div>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr runat="server" id="trFiletrTop">
                                            <td>
                                                <table runat="server" id="tblFilterByColumn" visible="false">
                                                    <tr runat="server" id="trSummaryFilter">
                                                        <td align="right">
                                                            <asp:Label runat="server" ID="lblFilterColumnName" Font-Bold="true"></asp:Label>
                                                            <asp:HiddenField runat="server" ID="hfFilterColumnSystemName" />
                                                        </td>
                                                        <td>
                                                            <%--<asp:DropDownList runat="server" ID="ddlFilterValue" AutoPostBack="true" CssClass="NormalTextBox"
                                                                OnSelectedIndexChanged="ddlFilterValue_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>
                                                            <%--<dbg:cbcValue runat="server" ID="cbcvSumFilter" OnddlYAxis_Changed="cbcvSumFilter_OnddlYAxis_Changed" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <%--<td align="right" runat="server" id="tdFilterCaption">
                                                <strong>Filter:</strong>
                                            </td>--%>
                                            <td align="left">
                                                <%--<asp:DropDownList ID="ddlYAxis" runat="server" AutoPostBack="true"
                                                 CssClass="NormalTextBox" OnSelectedIndexChanged="ddlYAxis_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                <div runat="server" id="tdFilterYAxis">

                                                    <table>
                                                        <tr>
                                                            <td></td>
                                                            <td align="right">
                                                                <strong runat="server" id="stgFilter">Filter:</strong>
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <dbg:ControlByColumn ID="cbcSearchMain" runat="server" OnddlCompareOperator_Changed="cbcSearchMain_OnddlCompareOperator_Changed" OnddlYAxis_Changed="cbcSearchMain_OnddlYAxis_Changed" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkAddSearch1" runat="server" OnClick="lnkSearch_Click" Visible="false">
                                                                        <asp:Image  runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png"/>
                                                                        </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trSearch1" style="display: none;">
                                                            <td>
                                                                <asp:LinkButton runat="server" ID="lnkMinusSearch1">
                                                                <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                </asp:LinkButton>
                                                            </td>
                                                            <td align="right">
                                                                <asp:HiddenField runat="server" ID="hfAndOr1" Value="" />
                                                                <asp:LinkButton runat="server" ID="lnkAndOr1" Text="and"></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <dbg:ControlByColumn ID="cbcSearch1" runat="server" OnddlCompareOperator_Changed="cbcSearch1_OnddlCompareOperator_Changed" OnddlYAxis_Changed="cbcSearch1_OnddlYAxis_Changed" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkAddSearch2" runat="server" OnClick="lnkSearch_Click" Visible="false">
                                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                        </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
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
                                                                    <tr>
                                                                        <td>
                                                                            <dbg:ControlByColumn ID="cbcSearch2" runat="server" OnddlCompareOperator_Changed="cbcSearch2_OnddlCompareOperator_Changed" OnddlYAxis_Changed="cbcSearch2_OnddlYAxis_Changed" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkAddSearch3" runat="server" OnClick="lnkSearch_Click" Visible="false">
                                                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                        </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trSearch3" style="display: none;">
                                                            <td>
                                                                <asp:LinkButton runat="server" ID="lnkMinusSearch3">
                                                                <asp:Image ID="Image6" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
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
                                                                            <dbg:ControlByColumn runat="server" ID="cbcSearch3" OnddlCompareOperator_Changed="cbcSearch3_OnddlCompareOperator_Changed" />
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField runat="server" ID="hfTextSearch" Value="" />
                                                </div>
                                            </td>
                                            <td>
                                                <div runat="server" id="tdFilterDynamic">
                                                    <table runat="server" id="tblSearchControls" visible="true" cellpadding="3">
                                                    </table>
                                                </div>
                                            </td>
                                            <td style="float:right;"> 
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <table width="120px">
                                                                <tr>
                                                                    <td colspan="2"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div>
                                                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"
                                                                                ValidationGroup="MKE"> <strong>Go</strong></asp:LinkButton>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div>
                                                                            <asp:LinkButton runat="server" ID="lnkReset" CssClass="btn" OnClick="lnkReset_Click"
                                                                                CausesValidation="false"> <strong>Reset</strong></asp:LinkButton>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"></td>
                                                                </tr>
                                                            </table>
                                                            <%--<asp:HyperLink class="popuplink" runat="server" 
                                                        ID="hlAdvancedSearch" Text="Advanced..." NavigateUrl="~/Pages/Record/AdvancedFilter.aspx"></asp:HyperLink>--%>
                                                        </td>
                                                        <td>
                                                            <table id="tblAdvancedOptionChk" runat="server">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table>
                                                                            <%--Advanced Search Checkbox--%>
                                                                            <tr>
                                                                                <td style="text-align: right;">
                                                                                    <asp:CheckBox runat="server" ID="chkShowAdvancedOptions" AutoPostBack="true" OnCheckedChanged="chkShowAdvancedOptions_OnCheckedChanged" />

                                                                                </td>
                                                                                <td style="text-align: left;">
                                                                                    <asp:Label runat="server" ID="lblAdvancedCaption" Text="Advanced Search"></asp:Label>
                                                                                    <asp:HiddenField runat="server" ID="hfHideAdvancedOption" />

                                                                                </td>
                                                                            </tr>
                                                                            <%--Show Deleted Records Checkbox--%>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <asp:CheckBox ID="chkIsActive" Checked="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkIsActive_CheckedChanged" />
                                                                                </td>
                                                                                <td style="text-align: left">Show Deleted Records
                                                                                </td>
                                                                            </tr>
                                                                            <%--Show Only Warning Checkbox--%>
                                                                            <tr id="trChkShowOnlyWarning">
                                                                                <td style="text-align: right;">
                                                                                    <asp:CheckBox ID="chkShowOnlyWarning" Checked="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowOnlyWarning_CheckedChanged" />
                                                                                </td>
                                                                                <td style="text-align: left;">Show Only Warning</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 1%"></td>
                                        </tr>
                                        <tr runat="server" id="trFilterBottom">
                                            <td colspan="4">
                                                <table id="tblAdvancedOptionChkC" runat="server" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <div id="search" style="padding-bottom: 10px;" runat="server" visible="true">
                                                                <table runat="server" id="tblAdvancedOption" style="border-collapse: collapse; display: none;"
                                                                    cellpadding="4">
                                                                    <tr id="trRecordGroup" runat="server" visible="false">
                                                                        <td colspan="3"></td>
                                                                        <td align="right">
                                                                            <strong>Record Group</strong>
                                                                        </td>
                                                                        <td colspan="2"></td>
                                                                    </tr>
                                                                    <tr id="Tr1" runat="server" visible="false">
                                                                        <td colspan="3"></td>
                                                                        <td align="right">
                                                                            <strong>Table</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                                                CssClass="NormalTextBox">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3"></td>
                                                                        <td align="right"></td>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table>
                                                                                            <tr style="vertical-align: bottom;">

                                                                                                <td>
                                                                                                    <strong>Date Added:</strong>
                                                                                                    <br />
                                                                                                    <asp:TextBox runat="server" ID="txtDateFrom" Width="100px" CssClass="NormalTextBox"
                                                                                                        ValidationGroup="MKE" BorderWidth="1" BorderStyle="Solid" BorderColor="#909090" />

                                                                                                </td>
                                                                                                <td style="padding-bottom: 7px;">
                                                                                                    <asp:ImageButton runat="server" ID="imgDateForm" ImageUrl="~/Images/Calendar.png"
                                                                                                        AlternateText="Click to show calendar" CausesValidation="false" />
                                                                                                    <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgDateForm" FirstDayOfWeek="Monday">
                                                                                                    </ajaxToolkit:CalendarExtender>
                                                                                                    <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                                                                        ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                                                                        MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtDateFrom"
                                                                                                        WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                                                                                    </ajaxToolkit:TextBoxWatermarkExtender>

                                                                                                </td>
                                                                                                <td style="padding-bottom: 5px; padding-left: 2px; padding-right: 2px;">To

                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox runat="server" ID="txtDateTo" Width="100px" CssClass="NormalTextBox"
                                                                                                        ValidationGroup="MKE" BorderWidth="1" BorderStyle="Solid" BorderColor="#909090" />

                                                                                                </td>
                                                                                                <td style="padding-bottom: 7px;">
                                                                                                    <asp:ImageButton runat="server" ID="imgDateTo" ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar"
                                                                                                        CausesValidation="false" />
                                                                                                    <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgDateTo" FirstDayOfWeek="Monday">
                                                                                                    </ajaxToolkit:CalendarExtender>
                                                                                                    <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                                                                                                        ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1753"
                                                                                                        MaximumValue="1/1/3000"></asp:RangeValidator>
                                                                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" TargetControlID="txtDateTo"
                                                                                                        WatermarkText="dd/mm/yyyy" runat="server" WatermarkCssClass="MaskText">
                                                                                                    </ajaxToolkit:TextBoxWatermarkExtender>

                                                                                                </td>
                                                                                            </tr>

                                                                                        </table>





                                                                                    </td>
                                                                                    <td style="width: 10px;"></td>
                                                                                    <td>
                                                                                        <strong>Entered By:</strong>
                                                                                        <br />
                                                                                        <asp:Panel runat="server" ID="divEnteredBy">
                                                                                            <asp:DropDownList ID="ddlEnteredBy" runat="server" AutoPostBack="true" DataTextField="FullName" Style="max-width: 200px;"
                                                                                                DataValueField="UserID" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlEnteredBy_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                        </asp:Panel>
                                                                                    </td>

                                                                                    <td style="width: 10px;"></td>
                                                                                    <td>
                                                                                        <strong>Upload Batch:</strong>
                                                                                        <br />
                                                                                        <asp:Panel runat="server" ID="divUploadedBatch">
                                                                                            <asp:DropDownList ID="ddlUploadedBatch" runat="server" AutoPostBack="true" DataTextField="BatchDescription" Style="max-width: 200px;"
                                                                                                DataValueField="BatchID" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlUploadedBatch_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                        </asp:Panel>
                                                                                    </td>


                                                                                    <%--<td style="width: 10px;"></td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkShowOnlyWarning" Checked="false" runat="server" AutoPostBack="true"
                                                                                            Text="Show Only Warning" OnCheckedChanged="chkShowOnlyWarning_CheckedChanged" />
                                                                                    </td>--%>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>



                                                                </table>
                                                            </div>
                                                        </td>
                                                        <td style="padding-left: 10px">
                                                            <table width="120px">
                                                                <tr>
                                                                    <td>
                                                                        <%--<asp:LinkButton runat="server" ID="lnkSearch2" CssClass="btn" OnClick="lnkSearch_Click"
                                                                            Style="display: none;" ValidationGroup="MKE"> <strong>Go</strong></asp:LinkButton>--%>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:LinkButton runat="server" ID="lnkReset2" CssClass="btn" OnClick="lnkReset_Click"
                                                                            Style="display: none;" CausesValidation="false"> <strong>Reset</strong></asp:LinkButton>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 1%"></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>

                        <%-- <asp:Panel ID="gridPanel" runat="server" Style="max-height: 600px" Width="100%" ScrollBars="Auto">--%>
                        <%--<div style="overflow-y: auto; overflow-x: hidden;max-height:600px;">--%>
                        <%--   <div style="overflow: scroll;" onscroll="OnScrollDiv(this)" id="DivMainContent">--%>



                        <div id="DivRoot">
                            <div id="DivPagerRow">
                            </div>
                            <div style="overflow: hidden;" id="DivHeaderRow">
                            </div>
                            <div id="DivMainContent">
                                <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                    HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                    AllowSorting="True" DataKeyNames="<%# GetDataKeyNames() %>"
                                    Width="100%" AutoGenerateColumns="true" PageSize="15" OnSorting="gvTheGrid_Sorting"
                                    OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound" ShowFooter="true"
                                    OnDataBound="gvTheGrid_DataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                                    <PagerSettings Position="Top" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <%--<input id="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                type="checkbox" />--%>
                                                <asp:CheckBox ID="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkDelete" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("DBGSystemRecordID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                                <%--NavigateUrl='<%# GetEditURL() + Cryptography.Encrypt(Eval("DBGSystemRecordID").ToString()) %>'--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="DBGSystemRecordID" Visible="false">
                                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:HyperLink ID="viewHyperLink" runat="server" ToolTip="View" ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                                <%--NavigateUrl='<%# GetViewURL() + Cryptography.Encrypt(Eval("DBGSystemRecordID").ToString())  %>'--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridview_header" />
                                    <%--gridview_header/HeaderFreez/FixedHeader--%>
                                    <RowStyle CssClass="gridview_row" />
                                    <AlternatingRowStyle CssClass="gridview_row" />
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="true" CssClass="gridview_footer" />

                                    <PagerTemplate>

                                        <asp:GridViewPager runat="server" ID="Pager" TableName="Recordlist" OnExportForCSV="Pager_OnExportForCSV"
                                            OnExportForExcel="Pager_OnExportForExcel" OnDeleteAction="Pager_DeleteAction" HidePagerGoButton="false"
                                            HideSendEmail="true" HideFilter="true" OnParmanenetDelAction="Pager_OnParmanenetDelAction"
                                            OnUnDeleteAction="Pager_UnDeleteAction" OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                            OnSendEmailAction="Pager_OnSendEmailAction" OnBindTheGridAgain="Pager_BindTheGridAgain" OnCopyRecordAction="Pager_CopyRecordAction"
                                            HideExcelExport="true" HideExport="true" HideAllExport="false" HideRefresh="true" HideGo="true"
                                            OnEditManyAction="Pager_EditManyAction" OnAllExport="Pager_AllExport" HideEditView="false" TableID="<%#TableID%>" />
                                    </PagerTemplate>
                                </dbg:dbgGridView>
                            </div>
                            <div id="DivFooterRow" style="overflow: hidden">
                            </div>
                        </div>

                        <%--</asp:Panel>--%>

                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">
                            <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;">
                                <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                No records have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                    Add new record now.</strong> 
                            </asp:HyperLink>
                            &nbsp or  &nbsp   
                            <asp:HyperLink runat="server" ID="hlEditView2" Text="Edit View" Font-Bold="true" CssClass="popuplink2"> </asp:HyperLink>

                        </div>
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <%--<asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png" />
                                No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                    Clear filter</strong>
                            </asp:LinkButton>--%>
                            No records matched your search. &nbsp<asp:LinkButton runat="server" ID="lnkNoFilter"
                                OnClick="Pager_OnApplyFilter" Font-Bold="true" Text="Clear Search"> </asp:LinkButton>
                            , &nbsp   
                            <asp:HyperLink runat="server" ID="hlEditView" Text="Edit View" Font-Bold="true" CssClass="popuplink2"> </asp:HyperLink>
                            &nbsp or &nbsp
                            <asp:HyperLink runat="server" ID="hplNewDataFilter" Text="Add Record" Font-Bold="true"> </asp:HyperLink>
                            <%--<asp:Label runat="server" ID="lblTableCaptionForFilter"></asp:Label>--%>
                            <asp:HyperLink runat="server" ID="hplNewDataFilter2">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/Pages/Pager/Images/add.png" />
                            </asp:HyperLink>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                        <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Width="1" Style="display: none;" CausesValidation="false" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <asp:Label runat="server" ID="lblDeleteAll" />
        <ajaxToolkit:ModalPopupExtender ID="mpeDeleteAll" runat="server" TargetControlID="lblDeleteAll"
            PopupControlID="pnlDeleteAll" BackgroundCssClass="modalBackground" />
        <asp:Panel ID="pnlDeleteAll" runat="server" Style="display: none">
            <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD; height: 320px; border-style: outset;">
                <div style="padding-top: 50px; padding: 20px;">
                    <table>
                        <tr runat="server" id="trDeleteRestoreMessage">
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblDeleteRestoreMessage" Font-Bold="true" Text="Are you sure you want to delete selected item(s)?"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr runat="server" id="trDeleteReason" visible="false">
                            <td colspan="2">
                                <strong>Please enter the reason for deleting the selected records*</strong><br />
                                <br />

                                <asp:TextBox runat="server" ID="txtDeleteReason" ValidationGroup="DR" CssClass="NormalTextBox"
                                    TextMode="MultiLine" Height="70px" Width="310px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDeleteReason" runat="server" ControlToValidate="txtDeleteReason"
                                    ErrorMessage="Required" Display="Dynamic" ValidationGroup="DR"></asp:RequiredFieldValidator>
                                <br />
                                <br />

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkDeleteAllOK" CssClass="btn" CausesValidation="true" ValidationGroup="DR"
                                                OnClick="lnkDeleteAllOK_Click"> <strong>OK</strong></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkDeleteAllNo" CssClass="btn" CausesValidation="false"
                                                OnClick="lnkDeleteAllNo_Click"> <strong>Cancel</strong></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trDeleteAllEvery">
                            <td colspan="2">
                                <br />
                                <asp:CheckBox runat="server" ID="chkDelateAllEvery" TextAlign="Right" Text="I would like to delete EVERY item in this table" />
                            </td>
                        </tr>
                        <tr runat="server" id="trDeleteParmanent" style="display: none;">
                            <td colspan="2">
                                <asp:CheckBox runat="server" ID="chkDeleteParmanent" TextAlign="Right" Text="I wish to delete these records permanently." />
                            </td>
                        </tr>
                        <tr runat="server" id="trUndo" style="display: none;">
                            <td colspan="2">
                                <asp:CheckBox runat="server" ID="chkUndo" TextAlign="Right" Text="I will not be able to undo this action." />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 20px;">
                                <asp:Label runat="server" ID="lblDeleteMessageNote" Width="350px"
                                    Text="Note: Deleted records are retained in the database and can be viewed or restored by your Admin user. Admin Users can also delete them permanently."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        <table style="width: 50px; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField runat="server" ID="hfParmanentDelete" Value="no" />
                </div>
            </div>
        </asp:Panel>
        <br />
        <br />
        <asp:Label runat="server" ID="lblEditMany" />
        <ajaxToolkit:ModalPopupExtender ID="mpeEditMany" runat="server" TargetControlID="lblEditMany"
            PopupControlID="pnlEditMany" BackgroundCssClass="modalBackground" OkControlID="lnkEditManyCancel" />
        <asp:Panel ID="pnlEditMany" runat="server" Style="display: none">
            <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD; height: 235px; border-style: outset;">
                <div style="padding-top: 50px; padding: 20px;">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="Label2" Font-Bold="true" Text="Update Multiple" CssClass="TopTitle"></asp:Label>
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-left: 50px;">
                                <strong runat="server" id="stgFieldToUpdate">Field to Update</strong>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYAxisBulk" runat="server" AutoPostBack="true" CssClass="NormalTextBox"
                                    OnSelectedIndexChanged="ddlYAxisBulk_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <strong>New Value</strong>
                            </td>
                            <td>

                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtTextBulk" CssClass="NormalTextBox" Visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtNumberBulk" CssClass="NormalTextBox" Visible="false"></asp:TextBox>
                                            <asp:DropDownList runat="server" ID="ddlDropdownBulk" CssClass="NormalTextBox" Visible="false">
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txtDateBulk" Width="90px" CssClass="NormalTextBox"
                                                Visible="false"></asp:TextBox>

                                            <asp:CheckBox runat="server" ID="chkCheckboxBulk" Visible="false" />
                                        </td>
                                        <td style="padding-left: 2px; padding-right: 2px;">
                                            <asp:ImageButton runat="server" ID="ibBulkDate" ImageUrl="~/Images/Calendar.png"
                                                AlternateText="Click to show calendar" CausesValidation="false" Visible="false" />

                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBulkTime" Width="50px" CssClass="NormalTextBox"
                                                ToolTip="Time" Visible="false"></asp:TextBox>

                                        </td>
                                        <td>

                                            <ajaxToolkit:MaskedEditExtender runat="server" ID="meeBulkTime" TargetControlID="txtBulkTime"
                                                AutoCompleteValue="00:00" MaskType="Time" Mask="99:99">
                                            </ajaxToolkit:MaskedEditExtender>

                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateBulk"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday" PopupButtonID="ibBulkDate">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtDateBulk"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtNumberBulk"
                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>



                            </td>
                        </tr>
                        <tr style="height: 15px;">
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkEditManyCancel" CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="lnkEditManyCancel2" CssClass="btn" CausesValidation="false" Visible="false"
                                                OnClick="lnkEditManyCancel2_Click"> <strong>Cancel</strong></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkEditManyOK" CssClass="btn" CausesValidation="false"
                                                OnClick="lnkEditManyOK_Click"> <strong>OK</strong></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trUpdateEveryItem">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:CheckBox Visible="true" runat="server" ID="chkUpdateEveryItem" Text="I would like to update EVERY item in this table."
                                                TextAlign="Right" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblMsgBullk" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        <table style="width: 50px; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>
        <br />
        <br />
        <asp:Label runat="server" ID="lblAddRecord" />
        <ajaxToolkit:ModalPopupExtender ID="mpeAddRecord" runat="server" TargetControlID="lblAddRecord"
            PopupControlID="pnlAddRecord" BackgroundCssClass="modalBackground" OkControlID="lnkAddRecordCancel" />
        <asp:Panel ID="pnlAddRecord" runat="server" Style="display: none">
            <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD; height: 220px; border-style: outset;">
                <div style="padding-top: 50px; padding: 20px;">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblAddRecordTitle" Font-Bold="true" Text="Add Record" CssClass="TopTitle"></asp:Label>
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-left: 50px;">
                                <strong runat="server" id="Strong1">Form</strong>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFormSet" runat="server" AutoPostBack="false" CssClass="NormalTextBox"
                                    DataValueField="FormSetID" DataTextField="FormSetName">
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr style="height: 15px;">
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkAddRecordCancel" CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lnkAddRecordCancel2" CssClass="btn" CausesValidation="false" Visible="false"
                                                OnClick="lnkAddRecordCancel2_Click"> <strong>Cancel</strong></asp:LinkButton>

                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkAddRecordOK" CssClass="btn" CausesValidation="false"
                                                OnClick="lnkAddRecordOK_Click"> <strong>OK</strong></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblMsgAddRecord" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress4"
                                    runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        <table style="width: 50px; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>
        <br />


        <br />
        <asp:Label runat="server" ID="lblExportRecords" />
        <ajaxToolkit:ModalPopupExtender ID="mpeExportRecords" runat="server" TargetControlID="lblExportRecords"
            PopupControlID="pnlExportRecords" BackgroundCssClass="modalBackground" OkControlID="lnkExportRecordsCancel" />
        <asp:Panel ID="pnlExportRecords" runat="server" Style="display: none">
            <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD; border-style: outset;">
                <div style="padding-top: 50px; padding: 20px;">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="Label3" Font-Bold="true" Text="Export Records" CssClass="TopTitle"></asp:Label>
                                <br />
                                <br />

                                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress6"
                                    runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        <table style="width: 50px; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="left">
                                <strong>Records (rows) to Export</strong>
                                <br />
                                <asp:DropDownList runat="server" ID="rdbRecords" CssClass="NormalTextBox" Width="320px"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdbRecords_SelectedIndexChanged">
                                    <asp:ListItem Value="a" Text="Export All records in this table" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="f" Text="Export Records that match current filter"></asp:ListItem>
                                    <asp:ListItem Value="t" Text="Export Only records that have been ticked"></asp:ListItem>
                                    <asp:ListItem Value="d" Text="Export All records and child records"></asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-left: 50px;"></td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <strong>File Format</strong>
                                            <br />
                                            <asp:DropDownList ID="ddlExportFiletype" runat="server" AutoPostBack="false" CssClass="NormalTextBox" Width="125px">
                                                <asp:ListItem Value="e" Text="Excel"></asp:ListItem>
                                                <asp:ListItem Value="c" Text="CSV"></asp:ListItem>
                                                <asp:ListItem Value="w" Text="Word"></asp:ListItem>
                                                <asp:ListItem Value="p" Text="PDF"></asp:ListItem>
                                            </asp:DropDownList>

                                        </td>
                                        <td style="width: 10px;"></td>
                                        <td>
                                            <strong>Export Template</strong>
                                            <br />
                                            <asp:DropDownList runat="server" ID="ddlTemplate" CssClass="NormalTextBox" DataValueField="ExportTemplateID" Width="175px"
                                                AutoPostBack="true" DataTextField="ExportTemplateName" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </td>
                                        <td>
                                            <asp:HyperLink runat="server" ID="hlExportTemplate" NavigateUrl="~/Pages/Export/ExportTemplate.aspx">Edit</asp:HyperLink>
                                        </td>
                                        <td style="padding-left: 5px;">
                                            <asp:HyperLink runat="server" ID="hlExportTemplateNew" NavigateUrl="~/Pages/Export/ExportTemplate.aspx">New</asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td align="left">
                                <br />
                                <br />
                                <strong>Fields to export</strong><br />
                                <asp:CheckBoxList Style="display: block; overflow: auto; min-width: 350px; max-width: 500px; min-height: 150px; max-height: 300px; border: solid 1px black;"
                                    runat="server" ID="chklstFields" SelectionMode="Multiple">
                                </asp:CheckBoxList>

                                <%--DataValueField="FieldID" DataTextField="Heading"--%>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right">
                                <asp:LinkButton runat="server" ID="lnkUntickAllExport" OnClick="lnkUntickAllExport_Click">Untick All</asp:LinkButton>
                            </td>

                        </tr>

                        <tr style="height: 15px;">
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkExportRecordsCancel" CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lnkExportRecordsCancel2" CssClass="btn" CausesValidation="false" Visible="false"
                                                OnClick="lnkExportRecordsCancel2_Click"> <strong>Cancel</strong></asp:LinkButton>

                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkExportRecords" CssClass="btn" CausesValidation="false"
                                                OnClick="lnkExportRecords_Click"> <strong>Export</strong></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>


                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblMagExportRecords" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
        </asp:Panel>
        <br />

        <br />

        <asp:HiddenField runat="server" ID="hfUsingScrol" />

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
        <asp:PostBackTrigger ControlID="lnkExportRecords" />
    </Triggers>
</asp:UpdatePanel>
<script language="javascript" type="text/javascript">
    //        ShowHide();
</script>
