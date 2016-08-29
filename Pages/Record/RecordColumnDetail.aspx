<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="RecordColumnDetail.aspx.cs" EnableEventValidation="false"
    Inherits="Pages_Record_ColumnDetail" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>

<%@ Register Src="~/Pages/UserControl/ControlByColumn.ascx" TagName="ControlByColumn"
    TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script src="../Document/Uploadify/jquery.uploadify.v2.1.4.js" type="text/javascript"></script>
    <script src="../Document/Uploadify/swfobject.js" type="text/javascript"></script>
    <link href="../Document/Uploadify/uploadify.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">



        $(document).ready(function () {

            var hfTableID = document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID");
            var hfColumnID = document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID");
            var strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();
            var strTextType = $('#ddlTextType').val();
            var strCalType = $('#ctl00_HomeContentPlaceHolder_ddlCalculationType').val();
            var strDDType = $('#ddlDDType').val();
            var txtColumnName = document.getElementById("ctl00_HomeContentPlaceHolder_txtColumnName");
            var chkSummaryPage = document.getElementById("ctl00_HomeContentPlaceHolder_chkSummaryPage");
            var chkDetailPage = document.getElementById("ctl00_HomeContentPlaceHolder_chkDetailPage");
            var chkGraph = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
            var chkImport = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
            var chkExport = document.getElementById("ctl00_HomeContentPlaceHolder_chkExport");
            var chkMobile = document.getElementById("ctl00_HomeContentPlaceHolder_chkMobile");

            var hfColumnSystemname = document.getElementById("hfColumnSystemname");
            var hfDateTimeColumn = document.getElementById("ctl00_HomeContentPlaceHolder_hfDateTimeColumn");
            var hfIsSystemColumn = document.getElementById("hfIsSystemColumn");

            var txtDisplayTextSummary = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextSummary");
            var txtDisplayTextDetail = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextDetail");
            var txtGraph = document.getElementById("ctl00_HomeContentPlaceHolder_txtGraph");
            var txtNameOnImport = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport");
            var hfIsImportPositional = document.getElementById("ctl00_HomeContentPlaceHolder_hfIsImportPositional");
            var txtNameOnExport = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnExport");
            var txtMobile = document.getElementById("ctl00_HomeContentPlaceHolder_txtMobile");
            //Warning

            var hlWarningAdvanced = document.getElementById("ctl00_HomeContentPlaceHolder_hlWarningAdvanced");
            var txtValidationOnWarning = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnWarning");
            var txtMinWaring = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinWaring");
            var txtMaxWrning = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxWrning");
            var chkWarningFormula = document.getElementById("chkWarningFormula");
            var chkWarningConditions = document.getElementById("chkWarningConditions");
            //Exceedance
            var hfShowExceedance = document.getElementById("hfShowExceedance");

            var hlExceedanceAdvanced = document.getElementById("ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced");
            var txtValidationOnExceedance = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnExceedance");
            var txtMinExceedance = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinExceedance");
            var txtMaxExceedance = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxExceedance");
            var chkExceedanceFormula = document.getElementById("chkExceedanceFormula");
            var chkExceedanceConditions = document.getElementById("chkExceedanceConditions");
            //Validation
            var hlValidAdvanced = document.getElementById("ctl00_HomeContentPlaceHolder_hlValidAdvanced");
            var txtValidationEntry = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationEntry");
            var txtMinValid = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinValid");
            var txtMaxValid = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxValid");
            var chkValidFormula = document.getElementById("chkValidFormula");
            var chkValidConditions = document.getElementById("chkValidConditions");
            var ddlImportance = document.getElementById("ctl00_HomeContentPlaceHolder_ddlImportance");

            var txtCalculation = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");


            //function showUnrequiredControlsForContentType(bVal) {
            //    if (bVal) {
            //        $("#divchkCompareOperator").fadeIn();
            //        $("#divchkValidationCanIgnore").fadeIn();
            //    }
            //    else {
            //        $("#divchkCompareOperator").fadeOut();
            //        $("#divchkValidationCanIgnore").fadeOut();
            //    }
            //}

            function ManageFormula() {
                var bShowValidationRoot = false;
                strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();



                if (ColumnTypeIn(strTypeV, 'number')) {
                    var strNumberTypeV = $('#ctl00_HomeContentPlaceHolder_ddlNumberType').val();
                    if (ColumnTypeIn(strNumberTypeV, '1,4,5,6,7')) { bShowValidationRoot = true; }
                }

                if (ColumnTypeIn(strTypeV, 'calculation')) {
                    var strCalType = $('#ctl00_HomeContentPlaceHolder_ddlCalculationType').val();
                    if (ColumnTypeIn(strCalType, 'n,f,d')) { bShowValidationRoot = true; }
                }

                if (bShowValidationRoot) {
                    //$("#divchkValidationCanIgnore").fadeIn();
                    $("#divValidationRoot").fadeIn();
                    $("#divGraphOptions").fadeIn();
                    ShowWarningMinMax();

                    if (hfShowExceedance.value == 'yes') {
                        ShowExceedanceMinMax();
                    }
                    ShowValidMinMax();
                }
                else {
                    //$("#divchkValidationCanIgnore").fadeOut();
                    $("#divValidationRoot").fadeOut();
                    $("#divGraphOptions").fadeOut();

                    var chkWarning = document.getElementById("ctl00_HomeContentPlaceHolder_chkWarning");
                    chkWarning.checked = false;
                    var chkExceedence = document.getElementById("ctl00_HomeContentPlaceHolder_chkExceedence");
                    chkExceedence.checked = false;
                    var chkMaximumValueat = document.getElementById("ctl00_HomeContentPlaceHolder_chkMaximumValueat");
                    chkMaximumValueat.checked = false;

                    
                    //var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinValid");
                    txtMinValid.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxValid");
                    txtMaxValid.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationEntry");
                    txtValidationEntry.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinWaring");
                    txtMinWaring.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxWrning");
                    txtMaxWrning.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnWarning");
                    txtValidationOnWarning.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinExceedance");
                    txtMinExceedance.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxExceedance");
                    txtMaxExceedance.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnExceedance");
                    txtValidationOnExceedance.value = '';
                }
                chkWarningClick(false);
                chkExceedenceClick(false);
                chkMaximumValueatClick(false);
            }

            function ColumnTypeIn(sCT, sSCT) {
                var s_a = sSCT.split(',');
                for (var i = 0; i < s_a.length; i++) {
                    if (s_a[i] == sCT) {
                        return true;
                    }
                }
                return false;
            }


            //function clearMinMaxExceedanceValue() {
            //    document.getElementById("ctl00_HomeContentPlaceHolder_txtMinExceedance").value = "";
            //    document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxExceedance").value = "";
            //}

            //function enableCompareValidatorForDataExceedance(bVal) {
            //    var cvtxtMinExceed = document.getElementById("ctl00_HomeContentPlaceHolder_cvtxtMinExceedance");
            //    var cvtxtMaxExceed = document.getElementById("ctl00_HomeContentPlaceHolder_cvtxtMaxExceedance");
            //    var cvExceedRanged = document.getElementById("ctl00_HomeContentPlaceHolder_cvExceedanceRange");
            //    if (bVal) {
            //        if (hfShowExceedance != null) {
            //            if (hfShowExceedance.value == 'no') {
            //                ValidatorEnable(cvtxtMinExceed, false)
            //                ValidatorEnable(cvtxtMaxExceed, false)
            //                ValidatorEnable(cvExceedRanged, false)
            //                clearMinMaxExceedanceValue();
            //            }
            //            else {
            //                ValidatorEnable(cvtxtMinExceed, true)
            //                ValidatorEnable(cvtxtMaxExceed, true)
            //                ValidatorEnable(cvExceedRanged, true)
            //            }
            //        }
            //    }
            //    else {
            //        ValidatorEnable(cvtxtMinExceed, false)
            //        ValidatorEnable(cvtxtMaxExceed, false)
            //        ValidatorEnable(cvExceedRanged, false)
            //    }
            //}

            function chkButtonWarningMessageClick() {
                var chkButtonWarningMessage = document.getElementById("chkButtonWarningMessage");
                var txtButtonWarningMessage = document.getElementById("txtButtonWarningMessage");
                if (chkButtonWarningMessage != null && chkButtonWarningMessage.checked == true) {
                    $("#txtButtonWarningMessage").fadeIn();
                }
                else {
                    $("#txtButtonWarningMessage").fadeOut();
                    if (txtButtonWarningMessage != null) {
                        txtButtonWarningMessage.value = '';
                    }
                }
            }
            function chkButtonOpenLinkClick() {
                var chk = document.getElementById("chkButtonOpenLink");
                var txt = document.getElementById("txtButtonOpenLink");
                if (chk != null && chk.checked == true) {
                    $("#txtButtonOpenLink").fadeIn();
                }
                else {
                    $("#txtButtonOpenLink").fadeOut();
                    if (txt != null) {
                        txt.value = '';
                    }
                }
            }
            function chkSPToRunClick() {
                var chk = document.getElementById("chkSPToRun");
                var txt = document.getElementById("txtSPToRun");
                if (chk != null && chk.checked == true) {
                    $("#txtSPToRun").fadeIn();
                }
                else {
                    $("#txtSPToRun").fadeOut();
                    if (txt != null) {
                        txt.value = '';
                    }
                }
            }
            function chkImageOnSummaryClick() {
                var chk = document.getElementById("chkImageOnSummary");
                var txt = document.getElementById("txtImageOnSummaryMaxHeight");

                if (chk != null) {
                    if (chk.checked == true) {
                        $("#tblImageOnSummaryMaxHeight").fadeIn();
                    }
                    else {

                        if (txt != null) {
                            txt.value = '';
                        }

                        $("#tblImageOnSummaryMaxHeight").fadeOut();
                    }
                }
            }

            function chkCompareOperatorClick() {
                var chkCompareOperator = document.getElementById("chkCompareOperator");
                var divCompareOperator = document.getElementById("divCompareOperator");
                if (chkCompareOperator.checked == true) {

                    ValidatorEnable(document.getElementById('rfvCompareOperator'), true);
                    ValidatorEnable(document.getElementById('rfvCompareTable'), true);
                    ValidatorEnable(document.getElementById('rfvCompareColumnID'), true);
                    divCompareOperator.style.display = 'block';
                    document.getElementById('rfvCompareOperator').style.display = 'none';
                    document.getElementById('rfvCompareTable').style.display = 'none';
                    document.getElementById('rfvCompareColumnID').style.display = 'none';
                }
                else {
                    divCompareOperator.style.display = 'none';
                    ValidatorEnable(document.getElementById('rfvCompareOperator'), false);
                    ValidatorEnable(document.getElementById('rfvCompareTable'), false);
                    ValidatorEnable(document.getElementById('rfvCompareColumnID'), false);
                }
            }
            function ddlCalculationTypeChange(bEvent) {

                strCalType = $('#ctl00_HomeContentPlaceHolder_ddlCalculationType').val();
                document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = document.getElementById('hfCalculationType').value ;

                if (strCalType == 'n' || strCalType == 'f') {
                    $("#trGraphOption").fadeIn();
                    if (bEvent = true)
                        chkGraph.checked = true;
                    $("#tblDateCal").fadeOut();
                    $("#lblCheckForFlat").fadeIn();
                    $("#chkFlatLine").fadeIn();
                    $("#trFlatLine").fadeIn();
                    $("#trRound").fadeIn();

                    $("#trCheckUnlikelyValue").fadeIn();
                }
                else {
                    $("#trGraphOption").fadeOut();
                    chkGraph.checked = false;
                }
                if (strCalType == 'n') {
                    $("#tblFinancialSymbol").fadeOut();

                }
                if (strCalType == 'f') {
                    $("#tblFinancialSymbol").fadeIn();

                }
                if (strCalType == 'd' || strCalType == 't') {
                    $("#tblFinancialSymbol").fadeOut();

                    $("#lblCheckForFlat").fadeOut();
                    $("#chkFlatLine").fadeOut();
                    $("#trFlatLine").fadeOut();
                    $("#trRound").fadeOut();

                    $("#trCheckUnlikelyValue").fadeOut();

                    if (strCalType == 'd') {
                        $("#tblDateCal").fadeIn();
                        document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = document.getElementById('hfCalculationType').value + '&date=yes';
                    }
                    else {
                        $("#tblDateCal").fadeOut();
                    }

                }

                chkGraphClick();
                ManageFormula();
            }


            function ShowWarningMinMax() {
                var hfShowWarningMinMax = document.getElementById("hfShowWarningMinMax");
                if (hfShowWarningMinMax.value == 'yes') {

                    //var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning');
                    txtValidationOnWarning.style.display = 'none';

                    var x = document.getElementById('divWarningMinMax');
                    x.style.display = 'block';
                    hlWarningAdvancedURL();
                }
                else {
                    //var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning');
                    txtValidationOnWarning.style.display = '';
                    var x = document.getElementById('divWarningMinMax');
                    x.style.display = 'none';
                }

            }

            function ShowExceedanceMinMax() {
                var hfShowExceedanceMinMax = document.getElementById("hfShowExceedanceMinMax");
                if (hfShowExceedanceMinMax.value == 'yes') {

                    //var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance');
                    txtValidationOnExceedance.style.display = 'none';

                    var x = document.getElementById('divExceedanceMinMax');
                    x.style.display = 'block';

                    hlExceedanceAdvancedURL();

                }
                else {
                    //var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance');
                    txtValidationOnExceedance.style.display = '';
                    var x = document.getElementById('divExceedanceMinMax');
                    x.style.display = 'none';
                }

            }

            function ShowValidMinMax() {
                var hfShowValidMinMax = document.getElementById("hfShowValidMinMax");
                if (hfShowValidMinMax.value == 'yes') {

                    //var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry');
                    txtValidationEntry.style.display = 'none';

                    var x = document.getElementById('divValidMinMax');
                    x.style.display = 'block';

                    //x = document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced');
                    // x.style.display = 'block';
                    hlValidAdvancedURL();

                }
                else {

                    //var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry');
                    txtValidationEntry.style.display = '';

                    var x = document.getElementById('divValidMinMax');
                    x.style.display = 'none';
                    //x = document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced');
                    // x.style.display = 'none';
                }

            }

            function ResetColumnType(bEvent) {
                strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();


                if (ColumnTypeIn(strTypeV, 'text,number')) {
                    $("#trTextDimension").fadeIn();
                    if (ColumnTypeIn(strTypeV, 'text')) {
                        $('#tdHeightLabel').fadeIn();
                        $('#tdHeight').fadeIn();
                    }
                    else {
                        $('#tdHeightLabel').fadeOut();
                        $('#tdHeight').fadeOut();
                    }
                }
                else {
                    $("#trTextDimension").fadeOut();
                }


                $("#trDetailPage1").fadeIn();
                $("#trDetailPage2").fadeIn();
                if (bEvent)
                    chkDetailPage.checked = true;






                chkDetailPageClick();
                if (ColumnTypeIn(strTypeV, 'staticcontent,button,calculation') == false) {
                    $("#trMandatory").fadeIn();
                }
                else {
                    $(ddlImportance).val('');
                    $("#trMandatory").fadeOut();

                }

                if (ColumnTypeIn(strTypeV, 'number,calculation')) {
                    $("#trGraphOption").fadeIn();
                    if (bEvent)
                        chkGraph.checked = true;
                }
                else {
                    $("#trGraphOption").fadeOut();
                    chkGraph.checked = false;
                    $("#trFlatLine").fadeOut();
                }
                chkGraphClick();
                if (ColumnTypeIn(strTypeV, 'staticcontent,button')) {
                    $("#trImportOption").fadeOut();
                    $("#trExportOption").fadeOut();
                    $("#trMobileSiteSummary").fadeOut();
                    $("#trSummaryPage1").fadeOut();
                    chkSummaryPage.checked = false;
                    chkImport.checked = false;
                    chkExport.checked = false;
                    chkMobile.checked = false;
                    $("#divchkCompareOperator").fadeOut();
                    $("#divchkValidationCanIgnore").fadeOut();
                    var chkValidationCanIgnore = document.getElementById("ctl00_HomeContentPlaceHolder_chkValidationCanIgnore");
                    chkValidationCanIgnore.checked = false;

                }
                else {

                    if (ColumnTypeIn(strTypeV, 'calculation')) {
                        $("#trImportOption").fadeOut(); chkImport.checked = false;
                    }
                    else {
                        $("#trImportOption").fadeIn();
                    }

                    $("#trExportOption").fadeIn();
                    $("#trMobileSiteSummary").fadeIn();
                    $("#trSummaryPage1").fadeIn();
                    $("#divchkValidationCanIgnore").fadeIn();
                    //chkSummaryPage.checked = true;
                    if (bEvent) {
                        chkImport.checked = true;
                        chkExport.checked = true;
                        chkMobile.checked = false;
                    }


                    $("#divchkCompareOperator").fadeIn();

                    //chkCompareOperatorClick();
                }
                chkSummaryPageClick();
                chkImportClick();
                chkExportClick();
                chkMobileClick();
                if (ColumnTypeIn(strTypeV, 'calculation')) {
                    $("#trCalculationType").fadeIn();
                    $("#trCalculation").fadeIn();
                    ddlCalculationTypeChange(false);
                }
                else {
                    $("#trCalculationType").fadeOut();
                    $("#trCalculation").fadeOut();
                }

                if (ColumnTypeIn(strTypeV, 'trafficlight')) {
                    $("#trTrafficLight").fadeIn();
                }
                else {
                    $("#trTrafficLight").fadeOut();
                }

                if (ColumnTypeIn(strTypeV, 'staticcontent')) {
                    $("#trStaticContent").fadeIn();
                }
                else {
                    $("#trStaticContent").fadeOut();
                }


                if (ColumnTypeIn(strTypeV, 'button')) {
                    $("#trButton1").fadeIn();
                    $("#trButton2").fadeIn();
                    $("#trButton3").fadeIn();
                    $("#trButton4").fadeIn();

                    chkButtonWarningMessageClick();
                    chkButtonOpenLinkClick();
                    chkSPToRunClick();
                    chkImageOnSummaryClick();
                }
                else {
                    $("#trButton1").fadeOut();
                    $("#trButton2").fadeOut();
                    $("#trButton3").fadeOut();
                    $("#trButton4").fadeOut();
                }

                if (ColumnTypeIn(strTypeV, 'checkbox')) {
                    $("#trCheckbox1").fadeIn();
                    $("#trCheckbox2").fadeIn();
                    $("#trCheckbox3").fadeIn();

                }
                else {
                    $("#trCheckbox1").fadeOut();
                    $("#trCheckbox2").fadeOut();
                    $("#trCheckbox3").fadeOut();

                }


                if (ColumnTypeIn(strTypeV, 'location')) {
                    $("#trLocation").fadeIn();
                    chkShowMapClick(false);
                }
                else {
                    $("#trLocation").fadeOut();
                }

                if (ColumnTypeIn(strTypeV, 'date_time')) {
                    $("#trReminders").fadeIn();
                    $("#trDateTimeType").fadeIn();
                    ddlDateTimeTypeChange(false);
                }
                else {
                    $("#trDateTimeType").fadeOut();
                    $("#trReminders").fadeOut();
                }
                if (ColumnTypeIn(strTypeV, 'text')) {
                    $("#trTextType").fadeIn();

                    ddlTextTypeClick();
                }
                else {
                    $("#trTextType").fadeOut();
                }

               






                if (ColumnTypeIn(strTypeV, 'dropdown,listbox') == false) {
                    $("#trDDTable").fadeOut();
                    $("#trFilter").fadeOut();
                    $("#trDDDisplayColumn").fadeOut();
                    $("#trDDValues").fadeOut();
                    $("#trDDType").fadeOut();
                    $("#trListboxType").fadeOut();
                }
                else {
                    if (ColumnTypeIn(strTypeV, 'listbox')) {
                        $("#trListboxType").fadeIn();

                        ddlListBoxTypeChange(false);
                    }
                    else {
                        $("#trListboxType").fadeOut();
                    }
                    if (ColumnTypeIn(strTypeV, 'dropdown')) {
                        $("#trDDType").fadeIn();
                        ddlDDTypeChange(false);
                    }
                    else {
                        $("#trDDType").fadeOut();
                    }

                }



                //$("#trDDTableLookup").fadeOut();
                // $("#trTextDimension").fadeOut();



                //  $("#lblSPDefaultValue").fadeOut();





                //$("#tblColumnColour").fadeOut();
                //$("#trFlatLine").fadeOut();

                if (ColumnTypeIn(strTypeV, 'image')) {
                    $("#trImageHeightSummary").fadeIn();
                    $("#trImageHeightDetail").fadeIn();
                }
                else {
                    $("#trImageHeightSummary").fadeOut();
                    $("#trImageHeightDetail").fadeOut();
                }
                if (ColumnTypeIn(strTypeV, 'radiobutton')) {
                    $("#trRadioOptionType").fadeIn();
                    ddlOptionTypeChange(false);
                }
                else {
                    $("#trRadioOptionType").fadeOut();
                    $('#trOptionImageGrid').fadeOut();

                }



                if (ColumnTypeIn(strTypeV, 'number,calculation') == false) {
                    $("#trRecordCountTable").fadeOut();
                    $("#trRecordCountClick").fadeOut();
                    $("#trSlider").fadeOut();
                    $("#trColumnToAvg").fadeOut();
                    $("#trAvgNumValues").fadeOut();
                    $("#trRound").fadeOut();

                    $("#trIgnoreSymbols").fadeOut();
                    $("#trCheckUnlikelyValue").fadeOut();
                }

                if (ColumnTypeIn(strTypeV, 'calculation') == false) {
                    $("#tblDateCal").fadeOut();
                }

                if (ColumnTypeIn(strTypeV, 'number,calculation')) {
                    chkRoundClick(false);
                    chkFlatLineClick(false)
                }
                if (ColumnTypeIn(strTypeV, 'staticcontent,button,file,image,calculation')) {
                    $("#trDefaultValue").fadeOut();
                }
                else {
                    $("#trDefaultValue").fadeIn();

                }

                if (ColumnTypeIn(strTypeV, 'number')) {
                    $("#trNumber1").fadeIn();
                    ddlNumberTypeChange(false);
                }
                else {
                    $("#trNumber1").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblSymbol").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtSymbol").fadeOut();
                }

                if (ColumnTypeIn(strTypeV, 'staticcontent,button,file,image')) {
                    $("#tblColumnColour").fadeOut();
                }
                else {
                    $("#tblColumnColour").fadeIn();
                }
                var hlResetCalculations = document.getElementById("hlResetCalculations");
                if (ColumnTypeIn(strTypeV, 'calculation') && hfColumnID.value!=-1) {
                    hlResetCalculations.style.display = 'block';
                }
                else {
                    hlResetCalculations.style.display = 'none';
                }

                if (bEvent) {
                    $("#ddlDefaultValue").val($("#ddlDefaultValue option:first").val());
                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                    txtDefaultValue.value = '';
                    var chkCompareOperator = document.getElementById("chkCompareOperator");
                    chkCompareOperator.checked = false;

                    ShowHideDCEdit();
                    var chkRound = document.getElementById("ctl00_HomeContentPlaceHolder_chkRound");
                    chkRound.checked = false;
                    chkRoundClick(false);
                    var chkFlatLine = document.getElementById("chkFlatLine");
                    chkFlatLine.checked = false;
                    chkFlatLineClick(false);
                    var chkIgnoreSymbols = document.getElementById("ctl00_HomeContentPlaceHolder_chkIgnoreSymbols");
                    chkIgnoreSymbols.checked = false;
                    var chkCheckUnlikelyValue = document.getElementById("ctl00_HomeContentPlaceHolder_chkCheckUnlikelyValue");
                    chkCheckUnlikelyValue.checked = false;
                    $(ddlImportance).val('');

                }

                ddlDefaultValueChange(false);
                chkCompareOperatorClick();
                ManageFormula();
            }



            function txtColumnNameChange() {

                if (chkSummaryPage.checked == true) {
                    txtDisplayTextSummary.value = txtColumnName.value;
                }
                if (chkDetailPage.checked == true) {
                    txtDisplayTextDetail.value = txtColumnName.value;
                }

                if (chkGraph.checked == true) {
                    txtGraph.value = txtColumnName.value;
                }

                if (chkImport.checked == true) {
                    if (hfIsImportPositional.value == '0') {
                        txtNameOnImport.value = txtColumnName.value;
                    }
                }
                if (chkExport.checked == true) {
                    txtNameOnExport.value = txtColumnName.value;
                }

                if (chkMobile.checked == true) {
                    txtMobile.value = txtColumnName.value;
                }


            }

            function chkSummaryPageClick() {
                if (chkSummaryPage.checked == true) {
                    txtDisplayTextSummary.value = txtColumnName.value;
                    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeIn();
                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeOut();
                    txtDisplayTextSummary.value = '';
                }
            }

            function chkDetailPageClick() {
                if (chkDetailPage.checked == true) {
                    $("#trDetailPage2").fadeIn();
                    txtDisplayTextDetail.value = txtColumnName.value;

                    if (strTypeV == 'staticcontent') {
                        $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeOut();
                        $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeOut();
                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeIn();
                        $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeIn();
                    }

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeOut();

                    $("#trDetailPage2").fadeOut();

                    txtDisplayTextDetail.value = '';
                }
            }


            function chkGraphClick() {
                if (chkGraph.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeIn();

                    txtGraph.value = txtColumnName.value;

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();

                    txtGraph.value = '';
                }
            }

            function chkImportClick() {

                if (chkImport.checked == true) {

                    if (hfDateTimeColumn.value == 'yes') {
                        $("#tblDateOptions").fadeIn();
                        var opt = document.getElementById("ctl00_HomeContentPlaceHolder_optSingle");
                        opt.checked = true;
                        $("#trDateFormat").fadeIn();

                    }
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeIn();

                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();

                    var txtD = document.getElementById("ctl00_HomeContentPlaceHolder_txtConstant");

                    if (hfIsImportPositional.value == '0') {
                        txtNameOnImport.value = txtColumnName.value;
                    }
                    else {
                        var hfPosMax = document.getElementById("ctl00_HomeContentPlaceHolder_hfMaxPosition");
                        txtNameOnImport.value = hfPosMax.value;

                    }

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    txtNameOnImport.value = '';
                    if (hfDateTimeColumn.value == 'yes') {
                        $("#tblDateOptions").fadeOut();
                        $("#trDateFormat").fadeOut();
                        $("#trTimeSection").fadeOut();

                        txtNameOnImport.value = '';
                        txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImportTime");
                        txtNI.value = '';

                        var opt = document.getElementById("ctl00_HomeContentPlaceHolder_optSingle");
                        opt.checked = true;
                    }


                }
            }

            function chkExportClick() {
                if (chkExport.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_lblExport").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnExport").fadeIn();
                    txtNameOnExport.value = txtColumnName.value;

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblExport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnExport").fadeOut();
                    txtNameOnExport.value = '';
                }
            }

            function chkMobileClick() {
                if (chkMobile.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_lblMobile").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtMobile").fadeIn();
                    txtMobile.value = txtColumnName.value;

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblMobile").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtMobile").fadeOut();
                    txtMobile.value = '';
                }
            }



            function ShowHideDCEdit() {
                var hlDDEdit = document.getElementById('hlDDEdit');
                var ddDDDisplayColumn = document.getElementById('ddDDDisplayColumn');
                var strAD = ddDDDisplayColumn.value;
                // alert(strAD);
                //hlDDEdit.style.display = 'none';
                if (strAD == '') {
                    hlDDEdit.style.display = 'block';
                }
                else {
                    hlDDEdit.style.display = 'none';
                }
            }
            //setTimeout(function () { ShowHideDCEdit(); }, 2000);



            function ReadOnlyTextBox(event) {
                window.event.returnValue = false;
            }

            function hlWarningAdvancedURL() {
                hlWarningAdvanced.href = '../Help/FormulaTest.aspx?type=warning&formula='
               + encodeURIComponent(txtValidationOnWarning.value)
               + '&min=' + encodeURIComponent(txtMinWaring.value)
               + '&max=' + encodeURIComponent(txtMaxWrning.value)
               + "&Tableid=" + hfTableID.value + "&Columnid=" + hfColumnID.value;
            }

            function WarningChanged() {
                hlWarningAdvancedURL();
                if (chkWarningFormula != null)
                {
                    if (txtMinWaring.value == '' && txtMaxWrning.value == '' && txtValidationOnWarning.value == '')
                    {
                        chkWarningFormula.checked = false;
                    }
                    else
                    {
                        chkWarningFormula.checked = true;
                    }
                }
                    

                if (chkWarningConditions != null)
                    chkWarningConditions.checked = false;
            }
            $('#ctl00_HomeContentPlaceHolder_txtMinWaring').change(function () {

                WarningChanged();
            });


            $('#ctl00_HomeContentPlaceHolder_txtMaxWrning').change(function () {
                WarningChanged();
            });


            function hlExceedanceAdvancedURL() {
                hlExceedanceAdvanced.href = '../Help/FormulaTest.aspx?type=exceedance&formula='
               + encodeURIComponent(txtValidationOnExceedance.value)
               + '&min=' + encodeURIComponent(txtMinExceedance.value)
               + '&max=' + encodeURIComponent(txtMaxExceedance.value)
               + "&Tableid=" + hfTableID.value + "&Columnid=" + hfColumnID.value;
            }

            function ExceedanceChanged() {
                hlExceedanceAdvancedURL();
                if (chkExceedanceFormula != null)
                {
                    if (txtMinExceedance.value == '' && txtMaxExceedance.value == '' && txtValidationOnExceedance.value == '') {
                        chkExceedanceFormula.checked = false;
                    }
                    else {
                        chkExceedanceFormula.checked = true;
                    }
                }
                   

                if (chkExceedanceConditions != null)
                    chkExceedanceConditions.checked = false;
            }

            $('#ctl00_HomeContentPlaceHolder_txtMinExceedance').change(function () {
                ExceedanceChanged();
            });


            $('#ctl00_HomeContentPlaceHolder_txtMaxExceedance').change(function () {
                ExceedanceChanged();
            });

            function hlValidAdvancedURL() {
                hlValidAdvanced.href = '../Help/FormulaTest.aspx?type=valid&formula='
               + encodeURIComponent(txtValidationEntry.value)
               + '&min=' + encodeURIComponent(txtMinValid.value)
               + '&max=' + encodeURIComponent(txtMaxValid.value)
               + "&Tableid=" + hfTableID.value + "&Columnid=" + hfColumnID.value;
            }

            function ValidChanged() {
                hlValidAdvancedURL();
                if (chkValidFormula != null)
                {
                    if (txtMinValid.value == '' && txtMaxValid.value == '' && txtValidationEntry.value == '') {
                        chkValidFormula.checked = false;
                    }
                    else {
                        chkValidFormula.checked = true;
                    }
                }
                   

                if (chkValidConditions != null)
                    chkValidConditions.checked = false;
            }

            function ShowMandatory(bShow) {
                if (bShow) {
                    $("#trMandatory").fadeIn();
                }
                else {
                    $(ddlImportance).val('');
                    $("#trMandatory").fadeOut();

                }

            }

            $('#ctl00_HomeContentPlaceHolder_txtMinValid').change(function () {
                ValidChanged();
            });

            $('#ctl00_HomeContentPlaceHolder_txtMaxValid').change(function () {
                ValidChanged();

            });


            $('#ctl00_HomeContentPlaceHolder_txtValidationEntry').keypress(function (e) {

                $(hlValidAdvanced).focus();
                return false;

            });

            $('#ctl00_HomeContentPlaceHolder_txtValidationOnWarning').keypress(function (e) {

                $(hlWarningAdvanced).focus();
                return false;

            });

            $('#ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').keypress(function (e) {

                $(hlExceedanceAdvanced).focus();
                return false;

            });

            $('#ctl00_HomeContentPlaceHolder_txtCalculation').keypress(function (e) {

                $(hlCalculationEdit).focus();
                return false;

            });


            $('#ctl00_HomeContentPlaceHolder_txtColumnName').change(function () {
                txtColumnNameChange();
            });
            $('#ctl00_HomeContentPlaceHolder_txtColumnName').keyup(function () {
                txtColumnNameChange();
            });
            $('#ctl00_HomeContentPlaceHolder_ddlCalculationType').change(function () {
                ddlCalculationTypeChange(true);
            });

            $("#chkShowWhen").click(function () {
                var chkShowWhen = document.getElementById("chkShowWhen");
                if (chkShowWhen.checked == true) {
                    $("#hlShowWhen").trigger("click");
                }

            });
            function OpenResetValidationConfirm()
            {
                $("#hlResetValidation").trigger("click");
            }

            $("#chkValidFormula").click(function () {
                if (chkValidFormula.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_hlValidAdvanced").trigger("click");
                    document.getElementById('hlValidConditions').innerHTML = 'Conditions';//T
                    if (chkValidConditions != null)
                        chkValidConditions.checked = false;
                }

            });

            $("#chkValidConditions").click(function () {
                if (chkValidConditions.checked == true) {
                    $("#hlValidConditions").trigger("click");
                    if (chkValidFormula != null)
                        chkValidFormula.checked = false;
                }
            });

            $("#chkWarningFormula").click(function () {
                if (chkWarningFormula.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_hlWarningAdvanced").trigger("click");
                    document.getElementById('hlWarningConditions').innerHTML = 'Conditions';
                    if (chkWarningConditions != null)
                        chkWarningConditions.checked = false;
                }

            });

            $("#chkWarningConditions").click(function () {
                if (chkWarningConditions.checked == true) {
                    $("#hlWarningConditions").trigger("click");
                    if (chkWarningFormula != null)
                        chkValidFormula.checked = false;
                }
            });

            $("#chkExceedanceFormula").click(function () {
                if (chkExceedanceFormula.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced").trigger("click");
                    document.getElementById('hlExceedanceConditions').innerHTML = 'Conditions';
                    if (chkExceedanceConditions != null)
                        chkExceedanceConditions.checked = false;
                }

            });

            $("#chkExceedanceConditions").click(function () {
                if (chkExceedanceConditions.checked == true) {
                    $("#hlValidConditions").trigger("click");
                    if (chkExceedanceFormula != null)
                        chkExceedanceFormula.checked = false;
                }
            });

            $("#chkColumnColour").click(function () {
                var chkColumnColour = document.getElementById("chkColumnColour");
                if (chkColumnColour.checked == true) {
                    $("#hlColumnColour").trigger("click");
                }

            });
            $("#chkFiltered").click(function () {
                var chkFiltered = document.getElementById("chkFiltered");
                if (chkFiltered.checked == true) {
                    $("#hlFiltered").trigger("click");
                }

            });

            $("#chkImageOnSummary").click(function () {

                chkImageOnSummaryClick();
            });

            $("#chkButtonWarningMessage").click(function () {
                chkButtonWarningMessageClick();
            });
            $("#chkButtonOpenLink").click(function () {
                chkButtonOpenLinkClick();
            });
            $("#chkSPToRun").click(function () {
                chkSPToRunClick();
            });

            $("#ctl00_HomeContentPlaceHolder_chkSummaryPage").click(function () {
                chkSummaryPageClick();
            });
            $("#ctl00_HomeContentPlaceHolder_chkDetailPage").click(function () {
                chkDetailPageClick();
            });
            $("#ctl00_HomeContentPlaceHolder_chkGraph").click(function () {
                chkGraphClick();
            });

            $("#ctl00_HomeContentPlaceHolder_chkImport").click(function () {
                chkImportClick();
            });
            $("#ctl00_HomeContentPlaceHolder_chkExport").click(function () {
                chkExportClick();
            });
            $("#ctl00_HomeContentPlaceHolder_chkMobile").click(function () {
                chkMobileClick();
            });

            function chkExceedenceClick(bEvent) {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExceedence");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtExceedence");
                if (chk.checked == true) {
                    txt.disabled = false;
                }
                else {
                    txt.value = '';
                    txt.disabled = true;
                }
            }
            $("#ctl00_HomeContentPlaceHolder_chkExceedence").click(function () {
                chkExceedenceClick(true);
            });

            function chkWarningClick(bEvent) {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkWarning");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtWarning");
                if (chk.checked == true) {
                    txt.disabled = false;
                }
                else {
                    txt.value = '';
                    txt.disabled = true;
                }
            }

            $("#ctl00_HomeContentPlaceHolder_chkWarning").click(function () {
                chkWarningClick(true);
            });

            $("#chkCompareOperator").click(function () {
                chkCompareOperatorClick();
            });

            function chkMaximumValueatClick(bEvent) {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMaximumValueat");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaximumValueat");
                if (chk.checked == true) {
                    txt.disabled = false;
                }
                else {
                    txt.value = '';
                    txt.disabled = true;
                }
            }
            $("#ctl00_HomeContentPlaceHolder_chkMaximumValueat").click(function () {
                chkMaximumValueatClick(true);
            });

            function ddDDDisplayColumnChange(bEvent) {
                var ddDDDisplayColumn = document.getElementById('ddDDDisplayColumn');
                var hlDDEdit = document.getElementById('hlDDEdit');
                var strColumnValue = ddDDDisplayColumn.value;// $('#ddDDDisplayColumn').val();

                if (strColumnValue != '') {
                    hlDDEdit.style.display = 'none';
                    document.getElementById('hfDisplayColumnsFormula').value = '[' + $('#ddDDDisplayColumn option:selected').text() + ']';
                }
                else {
                    hlDDEdit.style.display = 'block';
                }

                document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?formula=' + encodeURIComponent(document.getElementById('hfDisplayColumnsFormula').value) + '&Tableid=' + $('#ddlDDTable').val();

            }
            $('#ddDDDisplayColumn').change(function (e) {
                ddDDDisplayColumnChange(true);

            });

            function ddlOptionTypeChange(bEvent) {
                var strOptionType = $('#ddlOptionType').val();
                $('#trOptionImageGrid').fadeOut();
                $('#trDDValues').fadeIn();
                if (strOptionType == 'values') {
                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesCap").text('Values');
                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesHelp").text('Enter the values – one on each line');
                }
                else if (strOptionType == 'value_text') {
                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesCap").text('Value & Text');
                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesHelp").text('Enter value, comma(,) and text – one on each line');
                }
                else {
                    $('#trOptionImageGrid').fadeIn();
                    $('#trDDValues').fadeOut();
                }
            }

            $('#ddlOptionType').change(function (e) {
                ddlOptionTypeChange(true);
            });

            function ddlListBoxTypeChange(bEvent) {
                var strOptionType = $('#ddlListBoxType').val();
                $("#divQuickAddLink").fadeOut();
                if (strOptionType == 'values') {
                    $("#trDDTable").fadeOut();
                    $("#trDDTableLookup").fadeOut();
                    $("#trDDDisplayColumn").fadeOut();
                    $("#trDDValues").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesCap").text('Values');
                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesHelp").text('Enter the values – one on each line');

                }
                else if (strOptionType == 'value_text') {
                    $("#trDDTable").fadeOut();
                    $("#trDDTableLookup").fadeOut();
                    $("#trDDDisplayColumn").fadeOut();
                    $("#trDDValues").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesCap").text('Value & Text');
                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesHelp").text('Enter value, comma(,) and text – one on each line');

                }
                else {
                    $("#trDDTable").fadeIn();
                    $("#trDDTableLookup").fadeIn();
                    $("#trDDDisplayColumn").fadeIn();
                    $("#trDDValues").fadeOut();

                }
            }

            $('#ddlListBoxType').change(function (e) {
                ddlListBoxTypeChange(true);
            });



            function ddlDDTypeChange(bEvent) {
                strDDType = $('#ddlDDType').val();

                if (strDDType == 'values') {
                    $('#ctl00_HomeContentPlaceHolder_lnkCreateTable').fadeIn();
                }
                else {
                    $('#ctl00_HomeContentPlaceHolder_lnkCreateTable').fadeOut();
                }

                if (strDDType == 'ct' || strDDType == 'lt') {
                    $('#trDDTable').fadeIn();
                    $("#trDDTableLookup").fadeIn();
                    $('#trDDDisplayColumn').fadeIn();
                    $('#trDDValues').fadeOut();
                    //$("#trDefaultValue").fadeOut();
                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                    txtDefaultValue.value = '';
                    $("#trTextDimension").fadeIn();
                    $('#tdHeightLabel').fadeOut();
                    $('#tdHeight').fadeOut();

                    $("#tdPredictiveAndFilter").fadeIn();
                    //var chkFilterValues = document.getElementById("chkFilterValues");
                    if (strDDType == 'lt') {
                        $("#trFilter").fadeIn();
                        $("#tdPredictiveAndFilter").fadeOut();
                        $("#lblDisplayColumn").text('2nd Dropdown');
                        $("#divQuickAddLink").fadeOut();
                    }
                    else {
                        if (bEvent = false) {
                            ddDDDisplayColumnChange(false);
                        }

                        $("#trFilter").fadeOut();
                        $("#tdPredictiveAndFilter").fadeIn();
                        $("#lblDisplayColumn").text('Display Field');
                        $("#divQuickAddLink").fadeIn();
                        document.getElementById('hlFiltered').href = 'Filtered.aspx?hfFilterOperator=' + encodeURIComponent(document.getElementById('hfFilterOperator').value) + '&hfFilterParentColumnID=' + encodeURIComponent(document.getElementById('hfFilterParentColumnID').value) + "&hfFilterOtherColumnID=" + encodeURIComponent(document.getElementById("hfFilterOtherColumnID").value) + "&hfFilterValue=" + encodeURIComponent(document.getElementById("hfFilterValue").value) + "&ParentTableID=" + encodeURIComponent($('#ddlDDTable').val()) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

                    }
                }
                if (strDDType == 'values' || strDDType == 'value_text') {
                    $('#trDDValues').fadeIn();
                    $('#trDDTable').fadeOut();
                    $("#trDDTableLookup").fadeOut();
                    $('#trDDDisplayColumn').fadeOut();
                    $("#trDefaultValue").fadeIn();

                    $("#trTextDimension").fadeIn();
                    $('#tdHeightLabel').fadeOut();
                    $('#tdHeight').fadeOut();

                    $('#trFilter').fadeOut();
                    $("#tdPredictiveAndFilter").fadeOut();

                    if (strDDType == 'values') {
                        $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesCap").text('Values');
                        $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesHelp").text('Enter the values – one on each line');
                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesCap").text('Value & Text');
                        $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesHelp").text('Enter value, comma(,) and text – one on each line');
                    }
                }


                //                if (strDDType == 'linked') {
                //                    $('#trDDTable').fadeIn();
                //                    $('#trDDDisplayColumn').fadeIn();
                //                    $('#trDDValues').fadeOut();
                //                    $("#trDefaultValue").fadeOut();
                //                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                //                    txtDefaultValue.value = '';
                //                    $("#trTextDimension").fadeIn();
                //                    $('#tdHeightLabel').fadeOut();
                //                    $('#tdHeight').fadeOut();
                //                    $('#trFilter').fadeIn();
                //                    $("#tdPredictiveAndFilter").fadeIn();
                //                }

            }
            $('#ddlDDType').change(function (e) {
                ddlDDTypeChange(true);


            });
            function ddlDDTable_Change(bEvent) {
                var strDDTableID = $('#ddlDDTable').val();
                var hlDDEdit = document.getElementById('hlDDEdit');
                if (strDDTableID == '-1') {
                    if (strTypeV == 'dropdown') {
                        $("#tdTableFilter").fadeOut();
                        $("#divQuickAddLink").fadeOut();
                    }

                    document.getElementById('hfDisplayColumnsFormula').value = 'email';
                    if (hlDDEdit != null) {
                        hlDDEdit.style.display = 'none';
                    }

                }
                else {
                    if (strTypeV == 'dropdown') {
                        $("#tdTableFilter").fadeIn();
                        $("#divQuickAddLink").fadeIn();
                    }
                    document.getElementById('hfDisplayColumnsFormula').value = '';
                    document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?formula=' + encodeURIComponent(document.getElementById('hfDisplayColumnsFormula').value) + '&Tableid=' + $('#ddlDDTable').val();
                    document.getElementById('hlFiltered').href = 'Filtered.aspx?hfFilterOperator=' + encodeURIComponent(document.getElementById('hfFilterOperator').value) + '&hfFilterParentColumnID=' + encodeURIComponent(document.getElementById('hfFilterParentColumnID').value) + "&hfFilterOtherColumnID=" + encodeURIComponent(document.getElementById("hfFilterOtherColumnID").value) + "&hfFilterValue=" + encodeURIComponent(document.getElementById("hfFilterValue").value) + "&ParentTableID=" + encodeURIComponent($('#ddlDDTable').val()) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

                }
            }

            $('#ddlDDTable').change(function (e) {

                ddlDDTable_Change(true);

            });

            function ddlTextTypeClick() {
                strTextType = $('#ddlTextType').val();
                if (strTextType == "own") {
                    $("#txtOwnRegEx").fadeIn();
                    $("#hlRegEx").fadeIn();
                }
                else {
                    $("#txtOwnRegEx").fadeOut();
                    $("#hlRegEx").fadeOut();
                }

                if (strTextType == "readonly") {

                    $(ddlImportance).val('');
                    $("#trMandatory").fadeOut();
                }
                else {
                    $("#trMandatory").fadeIn();

                }
            }


            $('#ddlTextType').change(function (e) {

                ddlTextTypeClick();

            });

            $('#ctl00_HomeContentPlaceHolder_ddlType').change(function (e) {
                ResetColumnType(true);

            });
            function ddlDateTimeTypeChange(bEvent) {
                var strDateTimeTypeV = $('#ddlDateTimeType').val();
                //$("#trDefaultValue").fadeIn();
                if (strDateTimeTypeV == 'datetime' || strDateTimeTypeV == 'date') {
                    $("#trReminders").fadeIn();
                }
                else {
                    $("#trReminders").fadeOut();
                }
            }

            $('#ddlDateTimeType').change(function (e) {

                ddlDateTimeTypeChange(true);
            });


            function ddlDefaultValueChange(bEvent) {
                var strDefaultValue = $('#ddlDefaultValue').val();
                if (strDefaultValue == null || strDefaultValue == '') {
                    strDefaultValue = $('#hf_ddlDefaultValue').val();
                    if (strDefaultValue == null || strDefaultValue == '') {
                        strDefaultValue = 'none';
                    }
                }


                if (strDefaultValue == 'none') {
                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                    $("#tdDefaultParent").fadeOut();
                    $("#tdDefaultSyncData").fadeOut();
                }
                if (strDefaultValue == 'value') {
                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeIn();
                    $("#tdDefaultParent").fadeOut();
                    $("#tdDefaultSyncData").fadeOut();
                }
                if (strDefaultValue == 'parent') {
                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                    $("#tdDefaultParent").fadeIn();
                    $("#tdDefaultSyncData").fadeIn();
                }

                if (strDefaultValue == 'login') {
                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                    $("#tdDefaultParent").fadeOut();
                    $("#tdDefaultSyncData").fadeOut();
                }

                if (strTypeV == 'date_time') {
                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default');
                    $('#ddlDefaultValue option[value="value"]').text('To Today');
                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default Value');
                    $('#ddlDefaultValue option[value="value"]').text('Value');
                }


            }

            $('#ddlDefaultValue').change(function (e) {
                ddlDefaultValueChange(true);
            });

            function ddlNumberTypeChange(bEvent) {
                var strNumberTypeV = $('#ctl00_HomeContentPlaceHolder_ddlNumberType').val();
                var chkIgnoreSymbols = document.getElementById("ctl00_HomeContentPlaceHolder_chkIgnoreSymbols");
                var chkRound = document.getElementById("ctl00_HomeContentPlaceHolder_chkRound");
                var chkCheckUnlikelyValue = document.getElementById("ctl00_HomeContentPlaceHolder_chkCheckUnlikelyValue");
                var chkFlatLine = document.getElementById("chkFlatLine");

                //normal=1,4=avg, Financial=6,   7=slider, 8=id,constant=2,5=rc,

                if (ColumnTypeIn(strNumberTypeV, '8')) {
                    if (hfColumnID.value != -1) {
                        $("#hlResetIDs").fadeIn();
                    }
                    else {
                        $("#hlResetIDs").fadeOut();
                    }
                }
                else {
                    $("#hlResetIDs").fadeOut();
                }

                if (ColumnTypeIn(strNumberTypeV, '7')) {
                    $("#trSlider").fadeIn();
                    if (bEvent) {
                        //default
                    }
                }
                else {
                    $("#trSlider").fadeOut();
                }

                if (ColumnTypeIn(strNumberTypeV, '5')) {
                    $("#trRecordCountTable").fadeIn();
                    $("#trRecordCountClick").fadeIn();
                }
                else {
                    $("#trRecordCountTable").fadeOut();
                    $("#trRecordCountClick").fadeOut();
                }

                if (ColumnTypeIn(strNumberTypeV, '2')) {
                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeIn();
                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();
                    var txtConstant = document.getElementById("ctl00_HomeContentPlaceHolder_txtConstant");
                    txtConstant.value = '';
                }

                if (ColumnTypeIn(strNumberTypeV, '1,6,4')) {
                    $("#trMandatory").fadeIn();
                }
                else {
                    $("#trMandatory").fadeOut();
                    $(ddlImportance).val('');
                }
                if (ColumnTypeIn(strNumberTypeV, '1,6,4')) {
                    $("#trRound").fadeIn(); chkRoundClick(false);
                    $("#trIgnoreSymbols").fadeIn();
                    $("#trCheckUnlikelyValue").fadeIn();
                    $("#trFlatLine").fadeIn(); chkFlatLineClick(false);
                }
                else {
                    $("#trRound").fadeOut(); chkRound.checked = false; chkRoundClick(false);
                    $("#trIgnoreSymbols").fadeOut(); chkIgnoreSymbols.checked = false;
                    $("#trCheckUnlikelyValue").fadeOut(); chkCheckUnlikelyValue.checked = false;
                    $("#trFlatLine").fadeOut(); chkFlatLine.checked = false; chkFlatLineClick(false);
                }
                if (ColumnTypeIn(strNumberTypeV, '1,6,7')) {
                    $("#trImportOption").fadeIn();
                    $("#trDefaultValue").fadeIn();

                }
                else {
                    $("#trImportOption").fadeOut();
                    chkImport.checked = false;
                    $("#trDefaultValue").fadeOut();
                }


                if (ColumnTypeIn(strNumberTypeV, '1,4,5,6,7')) {
                    $("#divchkCompareOperator").fadeIn();
                }
                else {
                    var chkCompareOperator = document.getElementById("chkCompareOperator");
                    $("#divchkCompareOperator").fadeIn(); chkCompareOperator.checked = false;
                }
                if (ColumnTypeIn(strNumberTypeV, '4')) {
                    $("#trColumnToAvg").fadeIn();
                    $("#trAvgNumValues").fadeIn();
                }
                else {
                    $("#trColumnToAvg").fadeOut();
                    $("#trAvgNumValues").fadeOut();
                }
                if (ColumnTypeIn(strNumberTypeV, '6')) {
                    $("#ctl00_HomeContentPlaceHolder_lblSymbol").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtSymbol").fadeIn();
                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblSymbol").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtSymbol").fadeOut();
                }
                chkCompareOperatorClick();
                chkImportClick();

                if (bEvent) { $("#ddlDefaultValue").val($("#ddlDefaultValue option:first").val()); }
                ddlDefaultValueChange(false);
                ManageFormula();

            }

            $('#ctl00_HomeContentPlaceHolder_ddlNumberType').change(function (e) {
                ddlNumberTypeChange(true);
            });
            function chkRoundClick(bEvent) {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkRound");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtRoundNumber");
                if (chk.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_txtRoundNumber").fadeIn();

                    if (txt.value == '') { txt.value = '2'; }


                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_txtRoundNumber").fadeOut();
                    txt.value = '';
                    //$("#ctl00_HomeContentPlaceHolder_lblRoundNumber").fadeOut();

                }
            }

            $("#ctl00_HomeContentPlaceHolder_chkRound").click(function () {
                chkRoundClick(true);
            });

            $("#ctl00_HomeContentPlaceHolder_optSingle").click(function () {
                var optSingle = document.getElementById("ctl00_HomeContentPlaceHolder_optSingle");
                if (optSingle.checked == true) {
                    $("#trTimeSection").fadeOut();
                    if (hfIsImportPositional.value == "0") {
                        $("#ctl00_HomeContentPlaceHolder_lblImport").text("Label");
                        txtNameOn.value = "Date Time Recorded";
                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_lblImport").text("Position");

                    }
                }

            });

            $("#ctl00_HomeContentPlaceHolder_optDouble").click(function () {
                var optDouble = document.getElementById("ctl00_HomeContentPlaceHolder_optDouble");
                if (optDouble.checked == true) {
                    $("#trTimeSection").fadeIn();
                    if (hfIsImportPositional.value == "0") {
                        $("#ctl00_HomeContentPlaceHolder_lblImport").text("Date Label");

                        txtNameOnImport.value = "Date Recorded";
                        document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImportTime").value = "Time Recorded";

                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_lblImport").text("Date Position");


                    }
                }
            });

            function chkFlatLineClick(bEvent) {
                var chk = document.getElementById("chkFlatLine");
                var txt = document.getElementById("txtFlatLine");
                if (chk.checked == true) {
                    $("#lblFlatlineNumber").fadeIn();
                    $("#txtFlatLine").fadeIn();
                    if (bEvent)
                        txt.value = '';
                }
                else {
                    $("#lblFlatlineNumber").fadeOut();
                    $("#txtFlatLine").fadeOut();
                    txt.value = '';
                }
            }

            $("#chkFlatLine").click(function () {
                chkFlatLineClick(true);
            });

            function chkShowMapClick(bEvent) {
                var chk = document.getElementById("chkShowMap");
                if (chk.checked == true) {
                    $("#tblMapDimension").fadeIn();
                }
                else {
                    $("#tblMapDimension").fadeOut();
                }
            }

            $("#chkShowMap").click(function () {
                chkShowMapClick(true);
            });



            if (hfShowExceedance != null) {
                if (hfShowExceedance.value == 'no') {
                    $("#trExceedance1").fadeOut();
                    $("#trExceedance2").fadeOut();
                }
            }

            var hfGOD = document.getElementById("hfGOD");
            if (hfGOD != null && hfColumnID.value != -1) {
                if (hfGOD.value == 'yes') {
                    $("#trResetValidation").fadeIn();
                }
            }

            var hfHideFormula = document.getElementById("hfHideFormula");
            if (hfHideFormula != null) {
                if (hfHideFormula.value == 'yes') {
                    $(".formula").fadeOut();
                }
            }
            var hfHideConditions = document.getElementById("hfHideConditions");
            if (hfHideConditions != null) {
                if (hfHideConditions.value == 'yes') {
                    $(".conditions").fadeOut();
                }
            }
            //if (hfShowWarningMinMax.value == 'no') {
            //    ShowWarningMinMax('no');
            //}
            function ShowHide() {
                ResetColumnType(false);
            }

            ShowHide();



        });







        //if (window.addEventListener)
        //    window.addEventListener("load", ShowHide, false);
        //else if (window.attachEvent)
        //    window.attachEvent("onload", ShowHide);
        //else if (document.getElementById)
        //    window.onload = ShowHide;

    </script>
    <div style="padding-left: 20px; padding-right: 20px;">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center" onload="ShowHide();">
            <tr>
                <td colspan="3">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left" style="width: 50%;">
                                <span class="TopTitle">
                                    <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                            </td>
                            <td align="left">
                                <div style="width: 40px; height: 40px;">
                                    <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <table style="width: 100%; text-align: center">
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
                            <td style="padding-left: 100px;">
                                <div runat="server" id="divSaveGroup">
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
                                                        <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/Edit_big.png"
                                                            ToolTip="Edit" />
                                                    </asp:HyperLink>
                                                </div>
                                            </td>
                                            <td>
                                                <div runat="server" id="divDelete">
                                                    <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="javascript:return confirm('Are you sure you want to delete this Field Permanently?')"
                                                        CausesValidation="false" OnClick="lnkDelete_Click">
                                                        <asp:Image runat="server" ID="Image3" ImageUrl="~/App_Themes/Default/images/delete_big.png"
                                                            ToolTip="Delete" />
                                                    </asp:LinkButton>
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
                                <%--<div runat="server" id="divValid" visible="false" >
                                    <asp:Label runat="server" ID="lblValidInfo" Text="" ForeColor="Red"></asp:Label>
                                   
                                    <table style="padding-top:5px;">
                                        <tr>
                                            <td>
                                                <div runat="server" id="divOk">
                                                    <asp:LinkButton runat="server" ID="lnkOk" CssClass="btn" OnClick="lnkOk_Click" CausesValidation="true"> <strong>OK</strong></asp:LinkButton>
                                                </div>
                                            </td>
                                            <td>
                                                <div runat="server" id="divNo">
                                                    <asp:LinkButton runat="server" ID="lnkNo" CssClass="btn" CausesValidation="false"
                                                        OnClick="lnkNo_Click"> <strong>Cancel</strong>  </asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>--%>
                            </td>
                            <td align="right" style="padding-left: 50px;">
                                <asp:HyperLink runat="server" ID="hlHelpCommon" ClientIDMode="Static" NavigateUrl="~/Pages/Help/Help.aspx?contentkey=ColumnDetailHelp">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/images/help.png" />
                                </asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top"></td>
                <td valign="top">
                    <div id="search" style="padding-bottom: 10px">
                    </div>
                    <div runat="server" id="divDetail">
                        <asp:HiddenField runat="server" ID="hfColumnID" Value="-1" />
                        <asp:HiddenField runat="server" ID="hfTableID" />
                        <asp:HiddenField runat="server" ID="hfIsImportPositional" />
                        <asp:HiddenField runat="server" ID="hfMaxPosition" />
                        <asp:HiddenField runat="server" ID="hfShowWarningMinMax" Value="yes" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfShowExceedanceMinMax" Value="yes" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfShowValidMinMax" Value="yes" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfDateTimeColumn" Value="no" />
                        <asp:HiddenField runat="server" ID="hfColumnSystemname" Value="" ClientIDMode="Static" />

                        <asp:HiddenField runat="server" ID="hfShowExceedance" Value="no" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfHideFormula" Value="no" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfHideConditions" Value="no" ClientIDMode="Static" />
                         <asp:HiddenField runat="server" ID="hfConditions_T" Value="Conditions" ClientIDMode="Static" />
                         <asp:HiddenField runat="server" ID="hfGOD" Value="no" ClientIDMode="Static" />
                        <%-- <asp:HiddenField runat="server" ID="hfHideColumnID" Value="" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfHideColumnValue" Value="" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfHideColumnOperator" Value="" ClientIDMode="Static" />

                        <asp:HiddenField runat="server" ID="hfJoinOperator" Value="" ClientIDMode="Static" />

                         <asp:HiddenField runat="server" ID="hfHideColumnID2" Value="" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfHideColumnValue2" Value="" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfHideColumnOperator2" Value="" ClientIDMode="Static" />--%>


                        <asp:HiddenField runat="server" ID="hfIsSystemColumn" Value="" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hfCalculationType" Value="" ClientIDMode="Static" />
                        <table>
                            <tr>
                                <td align="right" runat="server" id="trTable">
                                    <strong runat="server" id="stgTableNameCaption">Table</strong>
                                </td>
                                <td colspan="6">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 300px;">
                                                <asp:TextBox runat="server" ID="txtTable" Enabled="false" CssClass="NormalTextBox"
                                                    Width="250px"></asp:TextBox>
                                            </td>
                                            <td style="padding-left: 50px;">
                                                <asp:CheckBox runat="server" ID="chkSummarySearch" Text="Search Field" Font-Bold="true"
                                                    TextAlign="Right" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong runat="server" id="stgFieldNameCap">Field Name*</strong>
                                </td>
                                <td colspan="6">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 300px;">
                                                <asp:TextBox ID="txtColumnName" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>

                                            </td>
                                            <td style="padding-left: 50px;">&nbsp;<strong>Visible To</strong>&nbsp;<asp:DropDownList runat="server" ID="ddlOnlyForAdmin" CssClass="NormalTextBox">
                                                <asp:ListItem Value="0" Text="All Users" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Admin Only"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Own Data Only"></asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblSystemName" Visible="false" Style="color: #C0C0C0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="6"></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="right"></td>
                                <td colspan="6">
                                    <asp:Label runat="server" ID="lblColumnMessage" Text="Note this is the field used as the date on the graphs"
                                        Visible="false"></asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvColumnName" runat="server" ControlToValidate="txtColumnName"
                                        ErrorMessage="Field Name - Required"></asp:RequiredFieldValidator>
                                </td>
                                <td></td>
                            </tr>

                            <tr>
                                <td colspan="8" style="height: 10px;"></td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <div style="border-style: solid; border-width: 1px; width: 500px; min-height: 140px; padding: 5px;">
                                                    <table>
                                                        <tr id="trSummaryPage1">
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkSummaryPage" runat="server" ToolTip="Available for selection on views" />
                                                            </td>
                                                            <td align="left" colspan="2">

                                                                <strong>Views</strong>

                                                            </td>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblSummaryPage" Text="Heading" Font-Bold="true" ToolTip="Available for selection on views"></asp:Label>

                                                            </td>
                                                            <td>
                                                                <asp:TextBox ToolTip="Available for selection on views" runat="server" ID="txtDisplayTextSummary" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <%--<tr id="trSummaryPage2" style="display: none;">
                                                            <td align="right"></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblAlignment" Text="Alignment" Font-Bold="true" Visible="false"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlAlignment" CssClass="NormalTextBox" Visible="false">
                                                                                <asp:ListItem Value="center" Text="Centre" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Value="left" Text="Left"></asp:ListItem>
                                                                                <asp:ListItem Value="right" Text="Right"></asp:ListItem>
                                                                                <asp:ListItem Value="-1" Text="Default"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                         
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>--%>

                                                        <%--<tr id="trSummaryPage3" style="display: none;">
                                                            <td align="right"></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblViewName" Text="View Name" Font-Bold="true" Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtViewName" CssClass="NormalTextBox" Width="250px" Visible="false"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>--%>
                                                        <tr id="trDetailPage1">
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkDetailPage" runat="server" />
                                                            </td>
                                                            <td>
                                                                <strong>Detail</strong>

                                                            </td>
                                                            <td></td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblDetailPage" Text="Label" Font-Bold="true"></asp:Label>
                                                            </td>

                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtDisplayTextDetail" Height="50px"
                                                                    CssClass="NormalTextBox" Width="250px" TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <tr id="trDetailPage2">
                                                            <td align="right"></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td align="right" style="width: 100px;"></td>
                                                            <td colspan="3">
                                                                <asp:CheckBox runat="server" ID="chkDisplayOnRight" Text="" TextAlign="Right" Font-Bold="true"
                                                                    ClientIDMode="Static" />
                                                                <asp:Label runat="server" ID="lblDisplayOnRight" Text="Display on the right" Font-Bold="true"
                                                                    ClientIDMode="Static"></asp:Label>

                                                                <asp:CheckBox runat="server" ID="chkShowWhen" Text="" TextAlign="Right" Font-Bold="true"
                                                                    ClientIDMode="Static" />
                                                                <asp:HyperLink runat="server" NavigateUrl="~/Pages/Record/ShowHide.aspx" CssClass="showlink"
                                                                    ID="hlShowWhen" ClientIDMode="Static">Show When...</asp:HyperLink>
                                                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfShowHref" />
                                                                <br />
                                                                <div runat="server" id="divTableTab">
                                                                    <strong>Page:</strong>
                                                                    <asp:DropDownList runat="server" ID="ddlTableTab" DataTextField="TabName" DataValueField="TableTabID"
                                                                        CssClass="NormalTextBox">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="trGraphOption">
                                                            <td>
                                                                <asp:CheckBox ID="chkGraph" runat="server" />
                                                            </td>
                                                            <td>
                                                                <strong>Graph</strong>
                                                            </td>
                                                            <td></td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblGraph" Text="Label" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGraph" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <tr id="trImportOption">
                                                            <td align="right" valign="middle">
                                                                <asp:CheckBox ID="chkImport" runat="server" />

                                                            </td>
                                                            <td valign="middle" colspan="2">
                                                                <asp:Label runat="server" ID="lblImportMain" Text="Import" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td colspan="4" valign="top">
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 97px;"></td>
                                                                        <td>
                                                                            <table runat="server" clientidmode="Static" id="tblDateOptions" style="display: none;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:RadioButton runat="server" ID="optSingle" GroupName="Import" Checked="true"
                                                                                            Text="Single Field" TextAlign="Right" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton runat="server" ID="optDouble" GroupName="Import" Text="Two Fields"
                                                                                            TextAlign="Right" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" style="width: 97px;">
                                                                            <asp:Label runat="server" ID="lblImport" Text="Name/Position" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtNameOnImport" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trTimeSection" style="display: none;">
                                                                        <td align="right" style="width: 97px;">
                                                                            <asp:Label runat="server" ID="lblImportTime" Text="Time Position" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtNameOnImportTime" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <table id="trDateFormat" style="display: none;" runat="server" clientidmode="Static">
                                                                                <tr>
                                                                                    <td align="right" style="width: 94px;">
                                                                                        <asp:Label runat="server" ID="lblDateFormat" Text="Date Format" Font-Bold="true"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList runat="server" ID="ddlDateFormat" CssClass="NormalTextBox">
                                                                                            <asp:ListItem Text="DD/MM/YYYY" Value="DD/MM/YYYY" Selected="True"></asp:ListItem>
                                                                                            <asp:ListItem Text="MM/DD/YYYY" Value="MM/DD/YYYY"></asp:ListItem>
                                                                                            <asp:ListItem Text="YYYY-MM-DD" Value="YYYY-MM-DD"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trExportOption" clientidmode="Static">
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkExport" runat="server" />
                                                            </td>
                                                            <td>
                                                                <strong>Export</strong>
                                                            </td>
                                                            <td></td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblExport" Text="Heading" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtNameOnExport" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7">
                                                                <!-- spacer to make it consistent -->
                                                            </td>
                                                        </tr>
                                                        <tr id="trMobileSiteSummary">
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkMobile" runat="server" />
                                                            </td>
                                                            <td>
                                                                <strong>Mobile</strong>
                                                            </td>
                                                            <td></td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblMobile" Text="Heading" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtMobile" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div id="divchkCompareOperator" style="display: none;">
                                                    <asp:CheckBox runat="server" ID="chkCompareOperator" ClientIDMode="Static" Text="Compare Values" Font-Bold="true" TextAlign="Right" />
                                                </div>


                                                <br />
                                                <div style="border-style: solid; border-width: 1px; width: 500px; padding: 5px; display: none; margin-top: 5px;"
                                                    id="divCompareOperator">
                                                    <table>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Compare Operator</strong>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList runat="server" ID="ddlCompareOperator" CssClass="NormalTextBox" ClientIDMode="Static">
                                                                    <asp:ListItem Text="--Please Select--" Value="" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Equal" Value="Equal"></asp:ListItem>
                                                                    <asp:ListItem Text="Not Equal" Value="NotEqual"></asp:ListItem>
                                                                    <asp:ListItem Text="Greater Than" Value="GreaterThan"></asp:ListItem>
                                                                    <asp:ListItem Text="Greater Than Equal" Value="GreaterThanEqual"></asp:ListItem>
                                                                    <asp:ListItem Text="Less Than" Value="LessThan"></asp:ListItem>
                                                                    <asp:ListItem Text="Less Than Equal" Value="LessThanEqual"></asp:ListItem>
                                                                    <asp:ListItem Text="Data Type Check" Value="DataTypeCheck"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="rfvCompareOperator" runat="server" ControlToValidate="ddlCompareOperator" Display="Dynamic" SetFocusOnError="true"
                                                                    ErrorMessage="Required" ClientIDMode="Static"></asp:RequiredFieldValidator>

                                                            </td>


                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Compare Table</strong>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList runat="server" ID="ddlCompareTable" CssClass="NormalTextBox" ClientIDMode="Static">
                                                                </asp:DropDownList>
                                                                <ajaxToolkit:CascadingDropDown runat="server" ID="ddlCompareTableC" Category="Tableid"
                                                                    ClientIDMode="Static" TargetControlID="ddlCompareTable" ServicePath="~/CascadeDropdown.asmx"
                                                                    ServiceMethod="GetRelatedTablesCompare">
                                                                </ajaxToolkit:CascadingDropDown>
                                                                <%--<asp:HiddenField runat="server" ID="hf_ddlDefaultParentTable" ClientIDMode="Static" />--%>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="rfvCompareTable" runat="server" ControlToValidate="ddlCompareTable" Display="Dynamic" SetFocusOnError="true"
                                                                    ErrorMessage="Required" ClientIDMode="Static"></asp:RequiredFieldValidator>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <strong>Compare Column</strong>
                                                            </td>
                                                            <td align="left">
                                                                <%--<asp:DropDownList runat="server" ID="ddlCompareColumnID" CssClass="NormalTextBox"
                                                                    DataValueField="ColumnID" DataTextField="DisplayName">
                                                                </asp:DropDownList>--%>

                                                                <asp:DropDownList runat="server" ID="ddlCompareColumnID" CssClass="NormalTextBox" ClientIDMode="Static">
                                                                </asp:DropDownList>

                                                                <ajaxToolkit:CascadingDropDown runat="server" ID="ddlCompareColumnIDC" Category="Column"
                                                                    ClientIDMode="Static" TargetControlID="ddlCompareColumnID" ServicePath="~/CascadeDropdown.asmx"
                                                                    ServiceMethod="GetCompareColumns" ParentControlID="ddlCompareTable">
                                                                </ajaxToolkit:CascadingDropDown>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="rfvCompareColumnID" runat="server" ControlToValidate="ddlCompareColumnID" Display="Dynamic" SetFocusOnError="true"
                                                                    ErrorMessage="Required" ClientIDMode="Static"></asp:RequiredFieldValidator>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div id="divchkValidationCanIgnore" style="display: none;">
                                                    <asp:CheckBox runat="server" ID="chkValidationCanIgnore"
                                                        Text="Allow user to ignore invalid data" Font-Bold="true" />

                                                </div>
                                                <br />
                                                <div id="divValidationRoot" style="display: none;">
                                                    <div style="border-style: solid; border-width: 1px; width: 500px; min-height: 140px; padding: 5px;" id="divValidation">
                                                        <table>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label runat="server" ID="lblValidationEntry" Text="Data invalid if outside the range" Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" valign="top" style="width: 10px;"></td>
                                                                <td valign="top" style="padding-top: 5px;">
                                                                    <asp:TextBox runat="server" ID="txtValidationEntry" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                        Width="250px" Height="60px"></asp:TextBox>

                                                                    <div id="divValidMinMax">
                                                                        <table>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label runat="server" ID="lblMinValid" Text="Value less than:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtMinValid" CssClass="NormalTextBox" Width="87px"></asp:TextBox>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Label runat="server" ID="lblMaxValid" Text="Value greater than:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtMaxValid" CssClass="NormalTextBox" Width="87px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>


                                                                    </div>

                                                                </td>
                                                                <td valign="top" style="padding-top: 7px;">

                                                                    <table>
                                                                        <tr class="formula">
                                                                            <td>
                                                                                <asp:CheckBox runat="server" ID="chkValidFormula" ClientIDMode="Static" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlValidAdvanced">Formula</asp:HyperLink>
                                                                                <%--<asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlValidEdit">Edit</asp:HyperLink>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top" class="conditions">
                                                                            <td>
                                                                                <asp:CheckBox runat="server" ID="chkValidConditions" ClientIDMode="Static" />
                                                                            </td>
                                                                            <td>
                                                                                <div style="word-wrap: break-word;">
                                                                                    <asp:HyperLink runat="server" NavigateUrl="#" ClientIDMode="Static" ID="hlValidConditions">Conditions</asp:HyperLink>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label runat="server" ID="lblWarningValidation" Text="Data Warning if outside the range"
                                                                        Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" valign="top"></td>
                                                                <td valign="top" style="padding-top: 5px;">
                                                                    <asp:TextBox runat="server" ID="txtValidationOnWarning" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                        Width="250px" Height="60px" Style="display: none;"></asp:TextBox>
                                                                    <div id="divWarningMinMax">

                                                                        <table>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label runat="server" ID="lblMinWarning" Text="Value less than:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtMinWaring" CssClass="NormalTextBox" Width="87px"></asp:TextBox>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Label runat="server" ID="lblMaxWarning" Text="Value greater than:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtMaxWrning" CssClass="NormalTextBox" Width="87px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>


                                                                    </div>


                                                                </td>
                                                                <td valign="top" style="padding-top: 7px;">
                                                                    <table>
                                                                        <tr class="formula">
                                                                            <td>
                                                                                <asp:CheckBox runat="server" ID="chkWarningFormula" ClientIDMode="Static" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlWarningAdvanced">Formula</asp:HyperLink>
                                                                                <%-- <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlWarningEdit"
                                                                                                                Style="display: none; top: 2px;">Edit</asp:HyperLink>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top" class="conditions">
                                                                            <td>
                                                                                <asp:CheckBox ClientIDMode="Static" runat="server" ID="chkWarningConditions" />
                                                                            </td>
                                                                            <td>
                                                                                <div style="word-wrap: break-word;">
                                                                                    <asp:HyperLink ClientIDMode="Static" runat="server" NavigateUrl="#" CssClass="validationlink2" ID="hlWarningConditions">Conditions</asp:HyperLink>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td></td>
                                                            </tr>

                                                            <tr id="trExceedance1">
                                                                <td colspan="3">
                                                                    <asp:Label runat="server" ID="lblExceedanceValidation" Text="Data Exceedance if outside the range"
                                                                        Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr id="trExceedance2">
                                                                <td align="right" valign="top"></td>
                                                                <td valign="top" style="padding-top: 5px;">
                                                                    <asp:TextBox runat="server" ID="txtValidationOnExceedance" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                        Width="250px" Height="60px" Style="display: none;"></asp:TextBox>

                                                                    <div id="divExceedanceMinMax">
                                                                        <table>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label runat="server" ID="lblMinExceedance" Text="Value less than:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtMinExceedance" CssClass="NormalTextBox" Width="87px"></asp:TextBox>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Label runat="server" ID="lblMaxExceedance" Text="Value greater than:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtMaxExceedance" CssClass="NormalTextBox" Width="87px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <%-- <div style="float: left; margin-left: 40px;">--%>
                                                                        <%--<asp:RegularExpressionValidator ID="revtxtMinExceedance" ControlToValidate="txtMinExceedance"
                                                                                runat="server" ErrorMessage="Min must be a valid number." Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                                                            </asp:RegularExpressionValidator>--%>
                                                                        <%--<asp:RegularExpressionValidator ID="revtxtMaxExceedance" ControlToValidate="txtMaxExceedance"
                                                                                runat="server" ErrorMessage="Max must be a valid number." Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                                                            </asp:RegularExpressionValidator>--%>
                                                                        <%--<asp:CompareValidator ID="cvtxtMinExceedance" runat="server" Operator="DataTypeCheck" Type="Double"
                                                                                                            ForeColor="Red"
                                                                                                            Display="Dynamic" SetFocusOnError="true"
                                                                                                            ControlToValidate="txtMinExceedance"
                                                                                                            ErrorMessage="Min must be a valid number.">
                                                                                                        </asp:CompareValidator>
                                                                                                        <asp:CompareValidator ID="cvtxtMaxExceedance" runat="server" Operator="DataTypeCheck" Type="Double"
                                                                                                            ForeColor="Red"
                                                                                                            Display="Dynamic" SetFocusOnError="true"
                                                                                                            ControlToValidate="txtMaxExceedance"
                                                                                                            ErrorMessage="Max must be a valid number.">
                                                                                                        </asp:CompareValidator>--%>
                                                                        <%--<asp:CompareValidator runat="server" ID="cvExceedanceRange" ControlToValidate="txtMinExceedance"
                                                                                ForeColor="Red"
                                                                                Display="Dynamic" SetFocusOnError="true" 
                                                                                ControlToCompare="txtMaxExceedance" Operator="LessThanEqual" Type="Double"
                                                                                ErrorMessage="Min must be less than Max.">
                                                                            </asp:CompareValidator>--%>
                                                                        <%--</div>--%>
                                                                    </div>
                                                                </td>
                                                                <td valign="top" style="padding-top: 7px;">

                                                                    <table>

                                                                        <tr class="formula">
                                                                            <td>
                                                                                <asp:CheckBox ClientIDMode="Static" runat="server" ID="chkExceedanceFormula" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlExceedanceAdvanced">Formula</asp:HyperLink>
                                                                                <%--<asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlExceedanceEdit"
                                                                                                                Style="display: none; top: 2px;">Edit</asp:HyperLink>--%>

                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top" class="conditions">
                                                                            <td>
                                                                                <asp:CheckBox ClientIDMode="Static" runat="server" ID="chkExceedanceConditions" />
                                                                            </td>
                                                                            <td>
                                                                                <div style="word-wrap: break-word;">
                                                                                    <asp:HyperLink ClientIDMode="Static" runat="server" NavigateUrl="#" CssClass="validationlink2" ID="hlExceedanceConditions">Conditions</asp:HyperLink>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr id="trResetValidation" style="display:none;">
                                                                <td ></td>
                                                                <td colspan="3">
                                                                    <asp:HyperLink ID="hlResetValidation" ClientIDMode="Static" runat="server"
                                                                    CssClass="popupresetIDs" >Revalidate Records</asp:HyperLink>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td valign="top" style="padding-left: 10px;">
                                                <div style="border-style: solid; border-width: 1px; min-width: 450px; min-height: 140px; padding: 5px;"
                                                    id="divColumnType">
                                                    <table>
                                                        <tr id="trColumtType">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblFieldType" Text="Field Type" Font-Bold="true" Width="100px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlType" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="button" Text="Button"></asp:ListItem>
                                                                    <asp:ListItem Value="calculation" Text="Calculation"></asp:ListItem>
                                                                    <asp:ListItem Value="checkbox" Text="Checkbox"></asp:ListItem>
                                                                    <asp:ListItem Value="staticcontent" Text="Content"></asp:ListItem>
                                                                    <%--<asp:ListItem Value="content" Text="Content Editor"></asp:ListItem>--%>
                                                                    <%--<asp:ListItem Value="data_retriever" Text="Data Retriever"></asp:ListItem>--%>
                                                                    <asp:ListItem Value="date_time" Text="Date / Time"></asp:ListItem>
                                                                    <asp:ListItem Value="dropdown" Text="Dropdown"></asp:ListItem>
                                                                    <asp:ListItem Value="file" Text="File"></asp:ListItem>
                                                                    <asp:ListItem Value="image" Text="Image"></asp:ListItem>
                                                                    <asp:ListItem Value="listbox" Text="List Box (multi-select)"></asp:ListItem>
                                                                    <asp:ListItem Value="location" Text="Location"></asp:ListItem>
                                                                    <asp:ListItem Value="number" Text="Number"></asp:ListItem>
                                                                    <asp:ListItem Value="radiobutton" Text="Radio Button"></asp:ListItem>
                                                                    <asp:ListItem Value="text" Text="Text" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="trafficlight" Text="Traffic Light"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td align="right" colspan="2">
                                                                <asp:HyperLink ID="hlResetCalculations" ClientIDMode="Static" runat="server"
                                                                    CssClass="popupresetCals" Style="display: none;">Reset Calculation Values</asp:HyperLink>

                                                            </td>
                                                        </tr>
                                                        <tr id="trCalculationType" style="display: none;">
                                                            <td align="right" style="width: 120px;">
                                                                <strong>Calculation Type</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>

                                                                        <td align="left">
                                                                            <asp:DropDownList runat="server" ID="ddlCalculationType"
                                                                                CssClass="NormalTextBox">
                                                                                <asp:ListItem Value="n" Text="Number" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Value="f" Text="Financial"></asp:ListItem>
                                                                                <asp:ListItem Value="d" Text="Date/Time"></asp:ListItem>
                                                                                <asp:ListItem Value="t" Text="Text"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <table id="tblFinancialSymbol" style="display: none;">
                                                                                <tr>
                                                                                    <td align="right" style="padding-left: 10px;">
                                                                                        <strong>Symbol</strong>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtCalFinancialSymbol" Text="$" CssClass="NormalTextBox"
                                                                                            Width="50px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table id="tblDateCal" style="display: none;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <strong>Result Format</strong>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList runat="server" ID="ddlDateResultFormat" CssClass="NormalTextBox">
                                                                                            <asp:ListItem Value="datetime" Text="Date and time" Selected="True"></asp:ListItem>
                                                                                            <asp:ListItem Value="date" Text="Date only"></asp:ListItem>
                                                                                            <asp:ListItem Value="time" Text="Time only"></asp:ListItem>
                                                                                            <asp:ListItem Value="minute" Text="Number of minutes"></asp:ListItem>
                                                                                            <asp:ListItem Value="hour" Text="Number of hours"></asp:ListItem>
                                                                                            <asp:ListItem Value="day" Text="Number of days"></asp:ListItem>

                                                                                            <asp:ListItem Value="ymd" Text="Year, Month & Day"></asp:ListItem>
                                                                                            <asp:ListItem Value="ym" Text="Year & Month"></asp:ListItem>

                                                                                        </asp:DropDownList>

                                                                                    </td>
                                                                                </tr>
                                                                            </table>

                                                                        </td>

                                                                    </tr>

                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trCalculation" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <asp:Label runat="server" ID="lblCalculation" Text="Calculation" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td colspan="5">
                                                                <table  style="border-collapse: collapse; border-spacing: 0;">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:TextBox runat="server" ID="txtCalculation" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                                Width="250px" Height="60px"></asp:TextBox>

                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:HyperLink runat="server" NavigateUrl="#" CssClass="calculationlink" ID="hlCalculationEdit">Edit</asp:HyperLink>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </td>

                                                        </tr>
                                                        <tr id="trTrafficLight" style="display: none;">
                                                            <td colspan="5">
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                    <ContentTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td colspan="5">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <strong>Controlling Field</strong>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:DropDownList runat="server" ID="ddlTLControllingField" CssClass="NormalTextBox"
                                                                                                    AutoPostBack="true" DataTextField="DisplayName" DataValueField="ColumnID" OnSelectedIndexChanged="ddlTLControllingField_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trTLValueImage" runat="server" visible="false">
                                                                                <td colspan="5">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <strong>Value</strong>
                                                                                            </td>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <strong>Image</strong>
                                                                                            </td>
                                                                                            <td></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue1" ShowColumnDDL="false" />
                                                                                            </td>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlTLImage1" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkAddTL1" CausesValidation="false" OnClick="lnkAddTL1_Click">
                                                                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTLValueLight2" runat="server" visible="false">
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkMinusTL2" CausesValidation="false" OnClick="lnkMinusTL2_Click">
                                                                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue2" ShowColumnDDL="false" />
                                                                                            </td>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlTLImage2" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkAddTL2" CausesValidation="false" OnClick="lnkAddTL2_Click">
                                                                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTLValueLight3" runat="server" visible="false">
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkMinusTL3" CausesValidation="false" OnClick="lnkMinusTL3_Click">
                                                                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue3" ShowColumnDDL="false" />
                                                                                            </td>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlTLImage3" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkAddTL3" CausesValidation="false" OnClick="lnkAddTL3_Click">
                                                                                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTLValueLight4" runat="server" visible="false">
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkMinusTL4" CausesValidation="false" OnClick="lnkMinusTL4_Click">
                                                                                                    <asp:Image ID="Image9" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue4" ShowColumnDDL="false" />
                                                                                            </td>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlTLImage4" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkAddTL4" CausesValidation="false" OnClick="lnkAddTL4_Click">
                                                                                                    <asp:Image ID="Image10" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTLValueLight5" runat="server" visible="false">
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkMinusTL5" CausesValidation="false" OnClick="lnkMinusTL5_Click">
                                                                                                    <asp:Image ID="Image11" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue5" ShowColumnDDL="false" />
                                                                                            </td>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlTLImage5" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <%--<asp:LinkButton runat="server" ID="lnkAddTL5"  CausesValidation="false" OnClick="lnkAddTL5_Click">
                                                                                                    <asp:Image ID="Image12" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                                </asp:LinkButton>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr id="trStaticContent" style="display: none;">
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:CheckBox runat="server" ID="chkAllowContenEdit" Text="Allow user to edit content"
                                                                                Font-Bold="true" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <editor:WYSIWYGEditor runat="server" scriptPath="../../Editor/scripts/" ID="edtContent"
                                                                                btnSave="false" EditorHeight="250" Height="250" EditorWidth="500" Width="500"
                                                                                AssetManager="../../assetmanager/assetmanager.aspx" AssetManagerWidth="550" AssetManagerHeight="400"
                                                                                Visible="true" ToolbarMode="0" btnPreview="False"
                                                                                btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trButton1" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <strong>Run Procedure</strong>
                                                            </td>
                                                            <td valign="top" colspan="5">
                                                                <table style="border-collapse: collapse; border-spacing: 0;">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:CheckBox runat="server" ID="chkSPToRun" ClientIDMode="Static" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtSPToRun" CssClass="NormalTextBox" Width="200px" ClientIDMode="Static"
                                                                                ToolTip="@RecordID int,@UserID int, @Return varchar(max) output"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>


                                                            </td>
                                                        </tr>
                                                        <tr id="trButton2" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <strong>Image</strong>
                                                            </td>
                                                            <td valign="top" colspan="5">
                                                                <%--<asp:TextBox runat="server" ID="txtButtonImage" CssClass="NormalTextBox" Width="400px" ToolTip="Image full URL."></asp:TextBox>--%>
                                                                <table style="border-collapse: collapse; border-spacing: 0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblButtonValue" ClientIDMode="Static"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:FileUpload runat="server" ID="fuButtonValue" ClientIDMode="Static" />
                                                                            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfButtonValue" />
                                                                        </td>
                                                                    </tr>

                                                                </table>

                                                            </td>

                                                        </tr>
                                                        <tr id="trButton3" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <strong>Warning Message</strong>
                                                            </td>
                                                            <td valign="top" colspan="5">
                                                                <table style="border-collapse: collapse; border-spacing: 0;">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:CheckBox runat="server" ID="chkButtonWarningMessage" ClientIDMode="Static" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtButtonWarningMessage" CssClass="NormalTextBox" Width="400px" TextMode="MultiLine" ToolTip="" ClientIDMode="Static"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trButton4" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <strong>Open Link</strong>
                                                            </td>
                                                            <td valign="top" colspan="5">
                                                                <table style="border-collapse: collapse; border-spacing: 0;">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:CheckBox runat="server" ID="chkButtonOpenLink" ClientIDMode="Static" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ClientIDMode="Static" ID="txtButtonOpenLink" CssClass="NormalTextBox" Width="400px" ToolTip="Link full URL."></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trCheckbox1" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <strong>Ticked Value</strong>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox runat="server" ID="txtTickedValue" CssClass="NormalTextBox" Text="Yes"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="trCheckbox2" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <strong>Unticked Value</strong>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox runat="server" ID="txtUntickedValue" CssClass="NormalTextBox" Text="No"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="trCheckbox3" style="display: none;">
                                                            <td align="right" valign="top"></td>
                                                            <td valign="top">
                                                                <asp:CheckBox ID="chkTickedByDefault" runat="server" Text="Ticked by Default" TextAlign="Right"
                                                                    Font-Bold="true" />
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="trLocation" style="display: none;">
                                                            <td align="right" valign="top"></td>
                                                            <td valign="top">
                                                                <asp:CheckBox ID="chkLocationAddress" runat="server" Text="All user to enter Address"
                                                                    TextAlign="Right" Font-Bold="true" Checked="true" />
                                                                <br />
                                                                <asp:CheckBox ID="chkLatLong" runat="server" Text="Show Latitude/Longitude" TextAlign="Right"
                                                                    Font-Bold="true" Checked="true" />
                                                                <br />
                                                                <asp:CheckBox ID="chkShowMap" runat="server" Text="Show Map" TextAlign="Right"
                                                                    Font-Bold="true" Checked="true" ClientIDMode="Static" />
                                                                <br />
                                                                <table>

                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <table id="tblMapDimension">
                                                                                <tr>
                                                                                    <td style="padding-left: 20px;">
                                                                                        <strong>Map Height</strong>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtMapHeight" Width="50px" Text="200"></asp:TextBox>
                                                                                        <asp:RangeValidator runat="server" ID="rvMapHeight" ControlToValidate="txtMapHeight"
                                                                                            Type="Integer" MinimumValue="100" MaximumValue="1000" Display="Dynamic" ErrorMessage="100 to 1000 please!"> </asp:RangeValidator>
                                                                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator12" ControlToValidate="txtMapHeight"
                                                                                   runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                               </asp:RegularExpressionValidator>--%>
                                                                                    </td>
                                                                                    <td></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="padding-left: 20px;">
                                                                                        <strong>Map Width</strong>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtMapWidth" Width="50px" Text="400"></asp:TextBox>
                                                                                        <asp:RangeValidator runat="server" ID="rvtxtMapWidth" ControlToValidate="txtMapWidth"
                                                                                            Type="Integer" MinimumValue="200" MaximumValue="1000" Display="Dynamic" ErrorMessage="200 to 1000 please!"> </asp:RangeValidator>
                                                                                    </td>
                                                                                    <td></td>
                                                                                </tr>
                                                                            </table>

                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td style="padding-left: 20px;">
                                                                            <strong>Hover Text</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlMapPinHoverColumnID" CssClass="NormalTextBox"
                                                                                DataValueField="ColumnID" DataTextField="DisplayName">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:HyperLink runat="server" ID="hlEditMappopup" ClientIDMode="Static"
                                                                                NavigateUrl="~/Pages/Content/MapPopup.aspx">Edit Popup</asp:HyperLink>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="trDateTimeType" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <strong>Type</strong>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:DropDownList runat="server" ID="ddlDateTimeType" ClientIDMode="Static" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="date" Text="Date Only"></asp:ListItem>
                                                                    <asp:ListItem Value="datetime" Text="Date & Time"></asp:ListItem>
                                                                    <asp:ListItem Value="time" Text="Time"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="trTextType" style="display: none;">
                                                            <td align="right" valign="top">
                                                                <strong>Text Type</strong>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:DropDownList runat="server" ID="ddlTextType" ClientIDMode="Static" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="" Text="Free text (any value) " Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="text" Text="Text only – no numbers"></asp:ListItem>
                                                                    <asp:ListItem Value="email" Text="Email address"></asp:ListItem>
                                                                    <asp:ListItem Value="link" Text="Website/link"></asp:ListItem>
                                                                    <asp:ListItem Value="isbn" Text="ISBN"></asp:ListItem>
                                                                    <asp:ListItem Value="readonly" Text="Read Only (set elsewhere)"></asp:ListItem>
                                                                    <asp:ListItem Value="own" Text="Regular Expression"></asp:ListItem>
                                                                    <asp:ListItem Value="mobile" Text="Mobile Number"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <br />
                                                                <asp:HyperLink Target="_blank" runat="server" ID="hlRegEx" ClientIDMode="Static"
                                                                    NavigateUrl="http://regexlib.com/" Text="http://regexlib.com/"></asp:HyperLink>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtOwnRegEx" ClientIDMode="Static" TextMode="MultiLine"
                                                                    CssClass="MultiLineTextBox" Width="150px" Height="50"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="trNumber1" style="display: none;">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblNumberType" Text="Number Type" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlNumberType" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="4" Text="Average"></asp:ListItem>
                                                                    <%--<asp:ListItem Value="3" Text="Calculated"></asp:ListItem>--%>
                                                                    <asp:ListItem Value="2" Text="Constant"></asp:ListItem>
                                                                    <asp:ListItem Value="6" Text="Financial"></asp:ListItem>
                                                                    <asp:ListItem Value="8" Text="ID"></asp:ListItem>
                                                                    <asp:ListItem Value="1" Text="Normal" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="5" Text="Record Count"></asp:ListItem>
                                                                    <asp:ListItem Value="7" Text="Slider"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td align="right">
                                                                <asp:HyperLink ID="hlResetIDs" ClientIDMode="Static" runat="server"
                                                                    CssClass="popupresetIDs" Style="display: none;">Reset IDs</asp:HyperLink>
                                                                <asp:Label runat="server" ID="lblConstant" Text="Value" Font-Bold="true" Style="display: none;"></asp:Label>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox runat="server" ID="txtConstant" TextMode="SingleLine" CssClass="NormalTextBox"
                                                                    Width="150px" Style="display: none;"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="revtxtConstant" ControlToValidate="txtConstant"
                                                                    runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                </asp:RegularExpressionValidator>


                                                                <asp:Button runat="server" ID="btnResetIDsOK" ClientIDMode="Static" Style="display: none;" OnClick="btnResetIDsOK_Click" />
                                                                <asp:Button runat="server" ID="btnResetCalValues" ClientIDMode="Static" Style="display: none;" OnClick="btnResetCalValues_Click" />

                                                                <asp:Button runat="server" ID="btnValidateRecordsOK" ClientIDMode="Static" Style="display: none;" OnClick="btnValidateRecordsOK_Click" />
                                                                <asp:Button runat="server" ID="btnValidateRecordsNO" ClientIDMode="Static" Style="display: none;" OnClick="btnValidateRecordsNO_Click" />

                                                                <asp:Button runat="server" ID="btnConfirmInvalidOK" ClientIDMode="Static" Style="display: none;" OnClick="btnConfirmInvalidOK_Click" />
                                                                <asp:Button runat="server" ID="btnConfirmInvalidNO" ClientIDMode="Static" Style="display: none;" OnClick="btnConfirmInvalidNO_Click" />

                                                                <asp:Button runat="server" ID="btnRevalidateRecords" ClientIDMode="Static" Style="display: none;" OnClick="btnRevalidateRecords_Click" />

                                                            </td>
                                                        </tr>

                                                        <tr runat="server" clientidmode="Static" id="trDDType" style="display: none;">
                                                            <td align="right">
                                                                <strong>Type</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlDDType" ClientIDMode="Static" CssClass="NormalTextBox">
                                                                                <asp:ListItem Value="values" Text="Values" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Value="value_text" Text="Value & Text"></asp:ListItem>
                                                                                <%--<asp:ListItem Value="table" Text="Table Predictive"></asp:ListItem>--%>
                                                                                <%--<asp:ListItem Value="tabledd" Text="Table Dropdown"></asp:ListItem>--%>
                                                                                <asp:ListItem Value="ct" Text="Table"></asp:ListItem>
                                                                                <asp:ListItem Value="lt" Text="Linked"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td id="tdPredictiveAndFilter">
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="width: 100px;">
                                                                                        <%--<asp:CheckBox runat="server" ID="chkFilterValues" Text="Filter Values" ClientIDMode="Static"
                                                                                            TextAlign="Right" />--%>
                                                                                        <asp:CheckBox runat="server" ID="chkPredictive" Text="Predictive" ClientIDMode="Static"
                                                                                            TextAlign="Right" Font-Bold="true" />
                                                                                    </td>
                                                                                    <td style="width: 100px;" id="tdTableFilter">
                                                                                        <asp:CheckBox runat="server" ID="chkFiltered" Text="" TextAlign="Right" Font-Bold="true"
                                                                                            ClientIDMode="Static" />
                                                                                        <asp:HyperLink runat="server" NavigateUrl="~/Pages/Record/Filtered.aspx" CssClass="showfilteredlink"
                                                                                            ID="hlFiltered" ClientIDMode="Static">Filtered...</asp:HyperLink>
                                                                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfFiltered" />
                                                                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfFilterParentColumnID" />
                                                                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfFilterOtherColumnID" />
                                                                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfFilterValue" />
                                                                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfFilterOperator" Value="equals" />
                                                                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfParentTableID" />

                                                                                    </td>

                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <asp:LinkButton runat="server" ID="lnkCreateTable" Visible="false" CausesValidation="false"
                                                                                OnClick="lnkCreateTable_Click"> <strong>Create values from Table</strong> </asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trRadioOptionType" style="display: none;">
                                                            <td align="right">
                                                                <strong>Type</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList runat="server" ID="ddlOptionType" ClientIDMode="Static" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="values" Text="Values" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="value_text" Text="Value & Text"></asp:ListItem>
                                                                    <asp:ListItem Value="value_image" Text="Value & Image"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                &nbsp;
                                                                <asp:CheckBox runat="server" ID="chkDisplayVertical" Text="Vertical List" TextAlign="Right" />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trListboxType" style="display: none;">
                                                            <td align="right">
                                                                <strong>Type</strong>
                                                            </td>
                                                            <td colspan="4">
                                                                <asp:DropDownList runat="server" ID="ddlListBoxType" ClientIDMode="Static" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="values" Text="Values" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="value_text" Text="Value & Text"></asp:ListItem>
                                                                    <asp:ListItem Value="table" Text="Table"></asp:ListItem>
                                                                </asp:DropDownList>

                                                            </td>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkListCheckBox" Font-Bold="true" Checked="true" />
                                                                <asp:Label runat="server" ID="lblListCheckBox" Text="Use Checkboxes" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>

                                                        <tr clientidmode="Static" id="trDDValues" style="display: none;">
                                                            <td align="right" style="vertical-align: top;">
                                                                <asp:Label runat="server" Font-Bold="true" Text="Values" ID="lblDropdownValuesCap"></asp:Label>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:TextBox runat="server" ID="txtDropdownValues" CssClass="MultiLineTextBox" TextMode="MultiLine"
                                                                    Width="150px" Height="50px"></asp:TextBox>
                                                                <br />
                                                                <asp:Label runat="server" Text="Enter the values – one on each line" ID="lblDropdownValuesHelp"
                                                                    Font-Size="Small"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trOptionImageGrid" style="display: none;">
                                                            <td></td>
                                                            <td>
                                                                <div id="divOptionImageGrid" style="min-width: 400px;">
                                                                    <asp:UpdatePanel ID="upOptionImage" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>
                                                                            <dbg:dbgGridView ID="gvOptionImage" runat="server" GridLines="Both" CssClass="gridview"
                                                                                HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                                                                AllowPaging="True" AllowSorting="True" DataKeyNames="OptionImageID" HeaderStyle-ForeColor="Black"
                                                                                Width="100%" AutoGenerateColumns="false" PageSize="15" OnRowDataBound="gvOptionImage_RowDataBound">
                                                                                <PagerSettings Position="Top" />
                                                                                <Columns>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="LblID" runat="server" Text='<%# Eval("OptionImageID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                                        <%--<HeaderTemplate>
                                                                                <input id="chkAll" onclick="DoMasterSelect(this, 'divOptionImageGrid')" runat="server" type="checkbox" />
                                                                            </HeaderTemplate>--%>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkDelete" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField>
                                                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                                        <ItemTemplate>
                                                                                            <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetOptionImageEditURL() + Cryptography.Encrypt(Eval("OptionImageID").ToString()) %>'
                                                                                                ImageUrl="~/App_Themes/Default/Images/iconEdit.png" CssClass="optionimgelink" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Value">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Image">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:Image runat="server" ID="imgImage" AlternateText='<%# Eval("FileName") %>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                </Columns>
                                                                                <HeaderStyle CssClass="gridview_header" />
                                                                                <RowStyle CssClass="gridview_row" />
                                                                                <PagerTemplate>
                                                                                    <asp:GridViewPager runat="server" ID="Pager" HideDelete="false" HideAdd="false"
                                                                                        OnDeleteAction="Pager_DeleteAction" HideNavigation="true"
                                                                                        HideExport="true" HideFilter="true" HideRefresh="true" HideGo="true" HidePageSize="true" />
                                                                                </PagerTemplate>
                                                                                <EmptyDataTemplate>
                                                                                    <div style="padding-left: 10px;">
                                                                                        <asp:HyperLink runat="server" ID="hplNewData" Style="text-decoration: none; color: Black;" CssClass="optionimgelink">
                                                                                        <asp:Image runat="server" ID="imgAddNewRecord" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                                                                        <strong style="text-decoration: underline; color: Blue;">
                                                                                            Add new image with value.</strong>
                                                                                        </asp:HyperLink>
                                                                                    </div>
                                                                                </EmptyDataTemplate>
                                                                            </dbg:dbgGridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <br />

                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ClientIDMode="Static" runat="server" ID="chkImageOnSummary" Font-Bold="true" TextAlign="Right" Text="Image On Summary" />
                                                                        </td>
                                                                        <td style="padding-left: 20px;">
                                                                            <table id="tblImageOnSummaryMaxHeight">
                                                                                <tr>
                                                                                    <td align="right">

                                                                                        <strong>Max Height</strong>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox runat="server" ID="txtImageOnSummaryMaxHeight" CssClass="NormalTextBox" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                                                                        <asp:RegularExpressionValidator ID="revtxtImageOnSummaryMaxHeight" ControlToValidate="txtImageOnSummaryMaxHeight"
                                                                                            runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                                        </asp:RegularExpressionValidator>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>


                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trDDTableLookup" style="display: none;">
                                                            <td></td>
                                                            <td colspan="5">
                                                                <asp:LinkButton runat="server" ID="lnkCreateLookupTable" Visible="false" CausesValidation="false"
                                                                    OnClick="lnkCreateLookupTable_Click"> <strong>Create Lookup Table</strong> </asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trDDTable" style="display: none;">
                                                            <td align="right">
                                                                <strong>Table</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlDDTable" CssClass="NormalTextBox" ClientIDMode="Static">
                                                                            </asp:DropDownList>
                                                                            <ajaxToolkit:CascadingDropDown runat="server" ID="ddlDDTableC" Category="Tableid"
                                                                                ClientIDMode="Static" TargetControlID="ddlDDTable" ServicePath="~/CascadeDropdown.asmx"
                                                                                ServiceMethod="GetTables">
                                                                            </ajaxToolkit:CascadingDropDown>
                                                                            <asp:HiddenField runat="server" ID="hf_ddlDDTable" ClientIDMode="Static" />
                                                                        </td>
                                                                        <td align="right">
                                                                            <strong style="display: none;">Field</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddDDLinkedParentColumn" ClientIDMode="Static"
                                                                                CssClass="NormalTextBox" Style="display: none;">
                                                                            </asp:DropDownList>
                                                                            <ajaxToolkit:CascadingDropDown runat="server" ID="ddDDLinkedParentColumnC" Category="Column"
                                                                                ClientIDMode="Static" TargetControlID="ddDDLinkedParentColumn" ServicePath="~/CascadeDropdown.asmx"
                                                                                ServiceMethod="GetColumnsLink" ParentControlID="ddlDDTable">
                                                                            </ajaxToolkit:CascadingDropDown>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trFilter" style="display: none;">
                                                            <td align="right">
                                                                <strong>1st Dropdown</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList runat="server" ID="ddlFilter" ClientIDMode="Static" CssClass="NormalTextBox"
                                                                    DataTextField="DisplayName" DataValueField="ColumnID">
                                                                </asp:DropDownList>
                                                                <ajaxToolkit:CascadingDropDown runat="server" ID="ddlFilterC" Category="Column" ClientIDMode="Static"
                                                                    TargetControlID="ddlFilter" ServicePath="~/CascadeDropdown.asmx" ServiceMethod="GetFilteredColumns"
                                                                    ParentControlID="ddlDDTable">
                                                                </ajaxToolkit:CascadingDropDown>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trDDDisplayColumn" style="display: none;">
                                                            <td align="right">
                                                                <%--<strong>Display Field</strong>--%>
                                                                <asp:Label runat="server" ID="lblDisplayColumn" Text="Display Field" Font-Bold="true" ClientIDMode="Static"></asp:Label>
                                                            </td>
                                                            <td colspan="5">

                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddDDDisplayColumn" ClientIDMode="Static" CssClass="NormalTextBox">
                                                                            </asp:DropDownList>
                                                                            <ajaxToolkit:CascadingDropDown runat="server" ID="ddDDDisplayColumnC" Category="Column"
                                                                                ClientIDMode="Static" TargetControlID="ddDDDisplayColumn" ServicePath="~/CascadeDropdown.asmx"
                                                                                ServiceMethod="GetColumnsWithDefault" ParentControlID="ddlDDTable">
                                                                            </ajaxToolkit:CascadingDropDown>

                                                                            <asp:HiddenField runat="server" ID="hfDisplayColumnsFormula" ClientIDMode="Static" />

                                                                        </td>
                                                                        <td>
                                                                            <asp:HyperLink runat="server" ID="hlDDEdit" Text="Edit" ClientIDMode="Static" NavigateUrl="~/Pages/Help/TableColumn.aspx"></asp:HyperLink>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divQuickAddLink">
                                                                                <asp:CheckBox runat="server" ID="chkQuickAddLink" TextAlign="Right" Text="Quick Add Link" Font-Bold="true" />
                                                                                <br />
                                                                                <%--<asp:CheckBox runat="server" ID="chkShowViewLink" TextAlign="Right" Text="Show View Link" Font-Bold="true" />--%>

                                                                                <asp:DropDownList runat="server" ID="ddlShowViewLink" CssClass="NormalTextBox">
                                                                                    <asp:ListItem Text="No View Link" Value=""></asp:ListItem>
                                                                                    <asp:ListItem Text="View Link on Detail" Value="Detail"></asp:ListItem>
                                                                                    <asp:ListItem Text="View Link on Summary" Value="Summary"></asp:ListItem>
                                                                                    <asp:ListItem Text="View Link on Both" Value="Both"></asp:ListItem>
                                                                                </asp:DropDownList>

                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </td>
                                                        </tr>
                                                        <tr id="trMandatory">
                                                             <td align="right">
                                                                 <strong>Importance</strong>
                                                            </td>
                                                            <td align="left">
                                                                   <asp:DropDownList runat="server" ID="ddlImportance" ToolTip="Required means it is important but you can still save the data without it. 
                                                                       Mandatory will prevent the data being saved unless entered." CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="" Text="Optional" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="r" Text="Required"></asp:ListItem>
                                                                       <asp:ListItem Value="m" Text="Mandatory"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:CheckBox runat="server" ID="chkMandatory" Font-Bold="true" />--%>
                                                                <%--<asp:Label runat="server" ID="lblMandatory" Text="Mandatory" Font-Bold="true"></asp:Label>--%>
                                                            </td>

                                                            <td align="left"></td>
                                                            <td style="width: 10px;"></td>
                                                            <td align="right">
                                                                <%--<asp:CheckBox runat="server" Font-Bold="true" ID="chkShowTotal" />--%>
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblSPDefaultValue" ForeColor="Gray" Style="display: none;"></asp:Label>
                                                                <%--<asp:Label runat="server" ID="lblShowTotal" Text="Show Field Total" Font-Bold="true"  Style="display: none;"></asp:Label>--%>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trTextDimension">
                                                            <td></td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <strong>Width</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtTextWidth" Width="50px" Text="22"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revtxtTextWidth" ControlToValidate="txtTextWidth"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                        <td></td>
                                                                        <td id="tdHeightLabel">
                                                                            <strong>Height</strong>
                                                                        </td>
                                                                        <td id="tdHeight">
                                                                            <asp:TextBox runat="server" ID="txtTextHeight" Width="50px" Text="1"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revtxtTextHeight" ControlToValidate="txtTextHeight"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trRecordCountTable" style="display: none;">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="Label1" Text="Table" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList runat="server" ID="ddlRecordCountTable" CssClass="NormalTextBox"
                                                                    DataValueField="TableID" DataTextField="TableName">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trRecordCountClick" style="display: none;">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="Label3" Text="Click" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList runat="server" ID="ddlRecordCountClick" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="no" Text="No action" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="open" Text="Open Child Records"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trSlider" style="display: none;">
                                                            <td align="right"></td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <strong>Min</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtSliderMin" CssClass="NormalTextBox" Width="50px" Text="0"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revtxtSliderMin" ControlToValidate="txtSliderMin"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>

                                                                        <td align="right">
                                                                            <strong>Max</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtSliderMax" CssClass="NormalTextBox" Width="50px" Text="100"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revtxtSliderMax" ControlToValidate="txtSliderMax"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trColumnToAvg" style="display: none;">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblColumnToAvg" Text="Field to Avg" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList runat="server" ID="ddlAvgColumn" CssClass="NormalTextBox" DataValueField="ColumnID"
                                                                    DataTextField="DisplayName">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trAvgNumValues" style="display: none;">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblAvgNumValues" Text="Avg Num Values" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtAvgNumValues" CssClass="NormalTextBox" Width="50px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td align="right"></td>
                                                            <td>
                                                                <asp:RegularExpressionValidator ID="revtxtAvgNumValues" ControlToValidate="txtAvgNumValues"
                                                                    runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trRound" style="display: none;">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblSymbol" Text="Symbol" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSymbol" Text="$" CssClass="NormalTextBox" Width="50px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td style="width: 10px;"></td>
                                                            <td align="right">
                                                                <asp:CheckBox runat="server" Font-Bold="true" ID="chkRound" />
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblRound" Text="Decimal Places" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtRoundNumber" CssClass="NormalTextBox" Width="50px"></asp:TextBox>
                                                                            <%--<asp:Label runat="server" ID="lblRoundNumber" Text="Numbers" Font-Bold="true"></asp:Label>--%>
                                                                            <asp:RegularExpressionValidator ID="revtxtRoundNumber" ControlToValidate="txtRoundNumber"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>

                                                                        </td>
                                                                    </tr>
                                                                </table>


                                                            </td>
                                                        </tr>


                                                        <tr clientidmode="Static" id="trIgnoreSymbols" style="display: none;">
                                                            <td align="right"></td>
                                                            <td valign="top" colspan="2"></td>
                                                            <td style="width: 10px;"></td>
                                                            <td align="right">
                                                                <asp:CheckBox runat="server" Font-Bold="true" ID="chkIgnoreSymbols" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblIgnoreSymbols" Text="Ignore Symbols" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trCheckUnlikelyValue" style="display: none;">
                                                            <td colspan="4"></td>
                                                            <td align="right">
                                                                <asp:CheckBox runat="server" Font-Bold="true" ID="chkCheckUnlikelyValue" ToolTip="Outside 3 standard deviations" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblCheckUnlikelyValue" Text="Warn of Unlikely Readings"
                                                                    Font-Bold="true" ToolTip="Outside 3 standard deviations"></asp:Label>
                                                            </td>

                                                        </tr>

                                                        <tr clientidmode="Static" id="trFlatLine" style="display: none;">
                                                            <td align="right"></td>
                                                            <td valign="top" colspan="5">


                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chkFlatLine" ClientIDMode="Static" />

                                                                            <asp:Label runat="server" ID="lblCheckForFlat" Text="Check for flat line:" ClientIDMode="Static"
                                                                                Font-Bold="true"></asp:Label>
                                                                        </td>


                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Label runat="server" ID="lblFlatlineNumber" Text="Number of entries" ClientIDMode="Static"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:TextBox runat="server" ID="txtFlatLine" CssClass="NormalTextBox" Width="75px"
                                                                                ClientIDMode="Static"></asp:TextBox>
                                                                        </td>



                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <asp:RegularExpressionValidator ID="revtxtFlatLine" ControlToValidate="txtFlatLine"
                                                                                runat="server" ErrorMessage="Invalid Flat line number of entries!" Display="Dynamic"
                                                                                ValidationExpression="^[0-9]+$">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                        </tr>




                                                        <tr>
                                                            <td colspan="6"></td>
                                                        </tr>
                                                        <tr id="trImageHeightSummary" style="display: none;">
                                                            <td align="right">
                                                                <strong>Max Height on Summary</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:TextBox runat="server" ID="txtImageHeightSummary" TextMode="SingleLine" CssClass="NormalTextBox"
                                                                    Width="100px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="revtxtImageHeightSummary" ControlToValidate="txtImageHeightSummary"
                                                                    runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr id="trImageHeightDetail" style="display: none;">
                                                            <td align="right">
                                                                <strong>Max Height on Detail</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:TextBox runat="server" ID="txtImageHeightDetail" TextMode="SingleLine" CssClass="NormalTextBox"
                                                                    Width="100px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="revtxtImageHeightDetail" ControlToValidate="txtImageHeightDetail"
                                                                    runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trReminders" style="display: none;">
                                                            <td align="right"></td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <%--<asp:CheckBox runat="server" ID="chkReminders" CssClass="NormalTextBox" Font-Bold="true"
                                                                                ClientIDMode="Static" />--%>
                                                                        </td>
                                                                        <td style="width: 90px; padding-left: 25px;">
                                                                            <asp:HyperLink runat="server" ID="hlReminders" ClientIDMode="Static" NavigateUrl="~/Pages/Schedule/DataReminder.aspx">Reminders</asp:HyperLink>
                                                                            <%--<asp:Label runat="server" ID="lblReminders" ClientIDMode="Static" Text="Reminders"
                                                                                Font-Bold="true"></asp:Label>--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr clientidmode="Static" id="trDefaultValue" style="display: none;">
                                                            <td align="right"></td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td align="right" style="width: 100px;">
                                                                            <asp:Label runat="server" ID="lblDefauleValue" Text="Default Value" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                        <td align="left">


                                                                            <asp:DropDownList runat="server" ID="ddlDefaultValue" ClientIDMode="Static" CssClass="NormalTextBox">
                                                                            </asp:DropDownList>
                                                                            <ajaxToolkit:CascadingDropDown runat="server" ID="ddlDefaultValueC" Category="DefaultValue"
                                                                                ClientIDMode="Static" TargetControlID="ddlDefaultValue" ServicePath="~/CascadeDropdown.asmx"
                                                                                ServiceMethod="GetDefaultValueOption" ParentControlID="ddlDDTable">
                                                                            </ajaxToolkit:CascadingDropDown>
                                                                            <asp:HiddenField runat="server" ID="hf_ddlDefaultValue" ClientIDMode="Static" Value="none" />

                                                                        </td>
                                                                        <td id="tdDefaultSyncData" align="left">
                                                                            <asp:CheckBox runat="server" ID="chkDefaultSyncData" Text="Sync Data" TextAlign="Right" Font-Bold="true" />
                                                                        </td>
                                                                        <td align="left" style="padding-left: 2px;">
                                                                            <asp:TextBox runat="server" ID="txtDefaultValue" CssClass="NormalTextBox" Width="165px"></asp:TextBox>
                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdDefaultParent" colspan="4" style="display: none;">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <strong>Table</strong>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList runat="server" ID="ddlDefaultParentTable" CssClass="NormalTextBox"
                                                                                            ClientIDMode="Static">
                                                                                        </asp:DropDownList>
                                                                                        <ajaxToolkit:CascadingDropDown runat="server" ID="ddlDefaultParentTableC" Category="Tableid"
                                                                                            ClientIDMode="Static" TargetControlID="ddlDefaultParentTable" ServicePath="~/CascadeDropdown.asmx"
                                                                                            ServiceMethod="GetRelatedTables">
                                                                                        </ajaxToolkit:CascadingDropDown>
                                                                                        <%--<asp:HiddenField runat="server" ID="hf_ddlDefaultParentTable" ClientIDMode="Static" />--%>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <strong>Field</strong>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList runat="server" ID="ddlDefaultParentColumn" ClientIDMode="Static"
                                                                                            CssClass="NormalTextBox">
                                                                                        </asp:DropDownList>
                                                                                        <ajaxToolkit:CascadingDropDown runat="server" ID="ddlDefaultParentColumnC" Category="Column"
                                                                                            ClientIDMode="Static" TargetControlID="ddlDefaultParentColumn" ServicePath="~/CascadeDropdown.asmx"
                                                                                            ServiceMethod="GetDefaultParentColumns" ParentControlID="ddlDefaultParentTable">
                                                                                        </ajaxToolkit:CascadingDropDown>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="tblColumnColour">
                                                            <td></td>
                                                            <td colspan="4">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:CheckBox runat="server" ID="chkColumnColour" Text="" TextAlign="Right" Font-Bold="true"
                                                                                ClientIDMode="Static" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:HyperLink runat="server" NavigateUrl="~/Pages/Record/ColumnColourList.aspx" CssClass="colourlink"
                                                                                ID="hlColumnColour" ClientIDMode="Static">Set colour by value</asp:HyperLink>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div id="divGraphOptions" style="border-style: solid; border-width: 1px; min-width: 450px; padding: 5px; display: none;">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 100px;" valign="top" align="right">
                                                                <strong>Graph Options</strong>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chkWarning" Text="Show Warning at" TextAlign="Right"
                                                                                Font-Bold="true" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtWarning" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revtxtWarning" ControlToValidate="txtWarning"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chkExceedence" Text="Show Exceedance at" TextAlign="Right"
                                                                                Font-Bold="true" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtExceedence" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revtxtExceedence" ControlToValidate="txtExceedence"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chkMaximumValueat" Text="Maximum Value at" TextAlign="Right"
                                                                                Font-Bold="true" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtMaximumValueat" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="revtxtMaximumValueat" ControlToValidate="txtMaximumValueat"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px;" valign="top" align="right">
                                                                <strong>Default Type</strong>
                                                            </td>
                                                            <td style="padding-left: 0.7em;">
                                                                <asp:DropDownList runat="server" ID="ddlDefaultGraphDefinition"
                                                                    DataTextField="DefinitionName" DataValueField="GraphDefinitionID"
                                                                    AppendDataBoundItems="true"
                                                                    CssClass="NormalTextBox" Width="262px">
                                                                    <asp:ListItem Text="--Please Select--" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="width: 450px; height: 140px;">
                                                    <table>
                                                        <tr id="trHelpText">
                                                            <td>
                                                                <table>
                                                                    <tr align="left">
                                                                        <td valign="top">
                                                                            <strong>Help Text</strong>&nbsp; (appears when user hovers over the field)
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 110px;">
                                                                            <asp:TextBox ID="txtNotes" runat="server" CssClass="MultiLineTextBox" TextMode="MultiLine"
                                                                                Width="450px" Height="96px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height: 10px;"></td>
                                        </tr>
                                        <tr>
                                            <td valign="top"></td>
                                            <td valign="top" style="padding-left: 10px;"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="3" height="13"></td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>


                            <div runat="server" id="divHistoryRoot" visible="false">
                                <asp:LinkButton runat="server" ID="lnkShowHistory" ClientIDMode="Static" Text="Show Change History"
                                    OnClick="lnkShowHistory_Click" CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnkHideHistory" ClientIDMode="Static" Text="Hide Change History"
                                    CausesValidation="false" OnClick="lnkHideHistory_Click" Visible="false"></asp:LinkButton>
                                <br />
                                <div runat="server" id="divHistory" visible="false">
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
                                            <asp:TemplateField HeaderText="Updated Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="UpdateDate" runat="server" Text='<%# Eval("DateAdded") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUser" runat="server" Text='<%# Eval("User") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Changed Field List">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblColumnList" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="gridview_header" ForeColor="Black" HorizontalAlign="Center" />
                                        <RowStyle CssClass="gridview_row" HorizontalAlign="Center" />
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
            <tr>
                <td colspan="3" height="50">
                    <asp:Button ID="btnRefreshRminder" runat="server" ClientIDMode="Static" OnClick="btnRefreshRminder_Click"
                        Style="display: none;" />

                    <asp:Button ID="btnOptionImage" runat="server" ClientIDMode="Static" OnClick="btnOptionImage_Click"
                        Style="display: none;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
