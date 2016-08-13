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

        function showUnrequiredControlsForContentType(bVal) {
            if (bVal) {
                $("#divchkCompareOperator").fadeIn();
                $("#divchkValidationCanIgnore").fadeIn();
            }
            else {
                $("#divchkCompareOperator").fadeOut();
                $("#divchkValidationCanIgnore").fadeOut();
            }
        }

        function clearMinMaxExceedanceValue() {
            document.getElementById("ctl00_HomeContentPlaceHolder_txtMinExceedance").value = "";
            document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxExceedance").value = "";
        }

        function enableCompareValidatorForDataExceedance(bVal) {
            var cvtxtMinExceed = document.getElementById("ctl00_HomeContentPlaceHolder_cvtxtMinExceedance");
            var cvtxtMaxExceed = document.getElementById("ctl00_HomeContentPlaceHolder_cvtxtMaxExceedance");
            var cvExceedRanged = document.getElementById("ctl00_HomeContentPlaceHolder_cvExceedanceRange");
            var hfShowExceedance = document.getElementById("hfShowExceedance");

            if (bVal) {
                if (hfShowExceedance != null) {
                    if (hfShowExceedance.value == 'no') {
                        ValidatorEnable(cvtxtMinExceed, false)
                        ValidatorEnable(cvtxtMaxExceed, false)
                        ValidatorEnable(cvExceedRanged, false)
                        clearMinMaxExceedanceValue();
                    }
                    else {
                        ValidatorEnable(cvtxtMinExceed, true)
                        ValidatorEnable(cvtxtMaxExceed, true)
                        ValidatorEnable(cvExceedRanged, true)
                    }
                }
            }
            else {
                ValidatorEnable(cvtxtMinExceed, false)
                ValidatorEnable(cvtxtMaxExceed, false)
                ValidatorEnable(cvExceedRanged, false)
            }
        }

        function ShowHideButtonWarningMessage() {
            var chk = document.getElementById("chkButtonWarningMessage");
            var txt = document.getElementById("txtButtonWarningMessage");
            if (chk != null && chk.checked == true) {
                $("#txtButtonWarningMessage").fadeIn();
            }
            else {
                $("#txtButtonWarningMessage").fadeOut();
                if (txt != null) {
                    txt.value = '';
                }
            }
        }
        function ShowHideButtonOpenLink() {
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
        function ShowHideSPToRun() {
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
        function ShowHideImageOnSummary() {
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

        function ShowHideCompareControls() {
            var chk = document.getElementById("chkCompareOperator");
            var divCompareOperator = document.getElementById("divCompareOperator");
            if (chk.checked == true) {
                divCompareOperator.style.display = 'block';
                ValidatorEnable(document.getElementById('rfvCompareOperator'), true);
                ValidatorEnable(document.getElementById('rfvCompareTable'), true);
                ValidatorEnable(document.getElementById('rfvCompareColumnID'), true);
            }
            else {
                divCompareOperator.style.display = 'none';
                ValidatorEnable(document.getElementById('rfvCompareOperator'), false);
                ValidatorEnable(document.getElementById('rfvCompareTable'), false);
                ValidatorEnable(document.getElementById('rfvCompareColumnID'), false);
            }
            ShowHideImageOnSummary();
            var strNumberTypeV = $('#ctl00_HomeContentPlaceHolder_ddlNumberType').val();
            if (strNumberTypeV == "2") {
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
            }
            ShowHideButtonWarningMessage();
            ShowHideButtonOpenLink();
            ShowHideSPToRun();
        }
        function ShowHideCalControl() {
            document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = document.getElementById('hfCalculationType').value;
            $("#trReminders").fadeOut();

           
            var strCalType = $('#ctl00_HomeContentPlaceHolder_ddlCalculationType').val();
            if (strCalType == 'n' || strCalType == 'f')
            {
                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', false);
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtGraph");
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                chk.checked = true;
                $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeIn();
                if (txt.value == '') {
                    var txtColumnName = document.getElementById("ctl00_HomeContentPlaceHolder_txtColumnName");
                    txt.value = txtColumnName.value;
                }

            }
            else
            {
                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtGraph");
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                chk.checked = false;
                $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
                txt.value = '';
            }
            if (strCalType == 'n') {
                $("#tblFinancialSymbol").fadeOut();
                $("#tblDateCal").fadeOut();
                $("#lblCheckForFlat").fadeIn();
                $("#chkFlatLine").fadeIn();
                $("#trFlatLine").fadeIn();
                $("#trRound").fadeIn();
               
                $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeIn();

                $("#trCheckUnlikelyValue").fadeIn();
            }
            if (strCalType == 'f') {
                $("#tblFinancialSymbol").fadeIn();
                $("#tblDateCal").fadeOut();

                $("#lblCheckForFlat").fadeIn();
                $("#chkFlatLine").fadeIn();
                $("#trFlatLine").fadeIn();
                $("#trRound").fadeIn();


                $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeIn();

                $("#trCheckUnlikelyValue").fadeIn();
            }
            if (strCalType == 'd' || strCalType == 't') {
                $("#tblFinancialSymbol").fadeOut();



                $("#lblCheckForFlat").fadeOut();
                $("#chkFlatLine").fadeOut();
                $("#trFlatLine").fadeOut();
                $("#trRound").fadeOut();
                //                    $("#ctl00_HomeContentPlaceHolder_chkRound").fadeIn();

                $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeOut();

                $("#trCheckUnlikelyValue").fadeOut();


                if (strCalType == 'd') {
                    $("#tblDateCal").fadeIn();
                    //$("#divGraphOptions").fadeIn();
                    document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = document.getElementById('hfCalculationType').value + '&date=yes';
                }
                else {
                    $("#tblDateCal").fadeOut();
                    //$("#divGraphOptions").fadeOut();
                }

            }

        }

        function ShowHideDefaultControl() {
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

            var strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();
            if (strTypeV == 'date_time') {
                $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
            }
        }
        $(function () {
            $(".validationlink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 700,
                height: 650,
                titleShow: false
            });
        });

        $(function () {
            $("#hlReminders").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 900,
                height: 800,
                titleShow: false
            });
        });


        $(function () {
            $("#hlDDEdit").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 600,
                height: 300,
                titleShow: false
            });
        });

        $(function () {
            $(".calculationlink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 600,
                height: 550,
                titleShow: false
            });
        });

        $(function () {
            $(".popuplink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 600,
                height: 650,
                titleShow: false
            });
        });

        $(function () {
            $(".showlink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 900,
                height: 500,
                titleShow: false
            });
        });

        $(function () {
            $(".colourlink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 900,
                height: 500,
                titleShow: false
            });
        });

        $(function () {
            $(".showfilteredlink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 650,
                height: 350,
                titleShow: false
            });
        });

        $(document).ready(function () {


            document.getElementById('hlFiltered').href = 'Filtered.aspx?hfFilterOperator=' + encodeURIComponent(document.getElementById('hfFilterOperator').value) + '&hfFilterParentColumnID=' + encodeURIComponent(document.getElementById('hfFilterParentColumnID').value) + '&hfFilterOtherColumnID=' + encodeURIComponent(document.getElementById("hfFilterOtherColumnID").value) + "&hfFilterValue=" + encodeURIComponent(document.getElementById("hfFilterValue").value) + "&ParentTableID=" + encodeURIComponent(document.getElementById("hfParentTableID").value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

            document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?formula=' + encodeURIComponent(document.getElementById('hfDisplayColumnsFormula').value) + '&Tableid=' + document.getElementById('hf_ddlDDTable').value; // $('#ddlDDTable').val();
            document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningEdit').href = '../Help/FormulaTest.aspx?type=warning&min=&max=&formula=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
            document.getElementById('ctl00_HomeContentPlaceHolder_hlValidEdit').href = '../Help/FormulaTest.aspx?type=valid&min=&max=&formula=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
            document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceEdit').href = '../Help/FormulaTest.aspx?type=Exceedance&min=&max=&formula=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;


            document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = '../Help/CalculationTest.aspx?type=calculation&formula=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtCalculation').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
            document.getElementById('hfCalculationType').value = document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href;

            document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningAdvanced').href = '../Help/FormulaTest.aspx?type=warning&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinWaring').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxWrning').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

            document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced').href = '../Help/FormulaTest.aspx?type=exceedance&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinExceedance').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxExceedance').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

            document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced').href = '../Help/FormulaTest.aspx?type=valid&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinValid').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxValid').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;


        });

        $(function () {
            $("#hlHelpCommon").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                'transitionIn': 'elastic',
                'transitionOut': 'none',
                width: 600,
                height: 350,
                titleShow: false
            });
        });

    </script>
    <script type="text/javascript">


        function ShowWarningMinMax(val) {

            if (val == 'yes') {

                var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning');
                x.style.display = 'none';

                x = document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningEdit');
                x.style.display = 'none';

                x = document.getElementById('divWrningAdvanced');
                x.style.display = 'block';

                document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningAdvanced').href = '../Help/FormulaTest.aspx?type=warning&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinWaring').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxWrning').value)
                 + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;


            }
            if (val == 'no') {


                var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning');
                x.style.display = '';

                x = document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningEdit');
                x.style.display = '';

                x = document.getElementById('divWrningAdvanced');
                x.style.display = 'none';
            }

        }

        function ShowExceedanceMinMax(val) {

            if (val == 'yes') {

                var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance');
                x.style.display = 'none';

                x = document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceEdit');
                x.style.display = 'none';

                x = document.getElementById('divExceedanceAdvanced');
                x.style.display = 'block';

                document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced').href = '../Help/FormulaTest.aspx?type=Exceedance&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinExceedance').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxExceedance').value)
                 + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;


            }
            if (val == 'no') {


                var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance');
                x.style.display = '';

                x = document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceEdit');
                x.style.display = '';

                x = document.getElementById('divExceedanceAdvanced');
                x.style.display = 'none';
            }

        }

        function ShowValidMinMax(val) {

            if (val == 'yes') {

                var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry');
                x.style.display = 'none';

                x = document.getElementById('ctl00_HomeContentPlaceHolder_hlValidEdit');
                x.style.display = 'none';

                x = document.getElementById('divValidAdvanced');
                x.style.display = 'block';

                x = document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced');
                x.style.display = 'block';

                document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced').href = '../Help/FormulaTest.aspx?type=valid&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinValid').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxValid').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;


            }
            if (val == 'no') {


                var x = document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry');
                x.style.display = '';

                x = document.getElementById('ctl00_HomeContentPlaceHolder_hlValidEdit');
                x.style.display = '';

                x = document.getElementById('divValidAdvanced');
                x.style.display = 'none';
                x = document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced');
                x.style.display = 'none';
            }

        }



        $(document).ready(function () {

            $('#ctl00_HomeContentPlaceHolder_txtMinWaring').change(function () {
                document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningAdvanced').href = '../Help/FormulaTest.aspx?type=warning&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinWaring').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxWrning').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
                var chkWarningFormula = document.getElementById("chkWarningFormula");
                if (chkWarningFormula != null)
                    chkWarningFormula.checked = true;

                var chkWarningConditions = document.getElementById("chkWarningConditions");
                if (chkWarningConditions != null)
                    chkWarningConditions.checked = false;
            });


            $('#ctl00_HomeContentPlaceHolder_txtMaxWrning').change(function () {
                document.getElementById('ctl00_HomeContentPlaceHolder_hlWarningAdvanced').href = '../Help/FormulaTest.aspx?type=warning&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnWarning').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinWaring').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxWrning').value)
                 + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
                var chkWarningFormula = document.getElementById("chkWarningFormula");
                if (chkWarningFormula != null)
                    chkWarningFormula.checked = true;

                var chkWarningConditions = document.getElementById("chkWarningConditions");
                if (chkWarningConditions != null)
                    chkWarningConditions.checked = false;
            });

            $('#ctl00_HomeContentPlaceHolder_txtMinExceedance').change(function () {
                document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced').href = '../Help/FormulaTest.aspx?type=exceedance&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinExceedance').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxExceedance').value)
                 + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

                var chkExceedanceFormula = document.getElementById("chkExceedanceFormula");
                if (chkExceedanceFormula != null)
                    chkExceedanceFormula.checked = true;

                var chkExceedanceConditions = document.getElementById("chkExceedanceConditions");
                if (chkExceedanceConditions != null)
                    chkExceedanceConditions.checked = false;
            });


            $('#ctl00_HomeContentPlaceHolder_txtMaxExceedance').change(function () {
                document.getElementById('ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced').href = '../Help/FormulaTest.aspx?type=exceedance&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinExceedance').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxExceedance').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

                var chkExceedanceFormula = document.getElementById("chkExceedanceFormula");
                if (chkExceedanceFormula != null)
                    chkExceedanceFormula.checked = true;

                var chkExceedanceConditions = document.getElementById("chkExceedanceConditions");
                if (chkExceedanceConditions != null)
                    chkExceedanceConditions.checked = false;
            });


            $('#ctl00_HomeContentPlaceHolder_txtMinValid').change(function () {
                document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced').href = '../Help/FormulaTest.aspx?type=valid&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinValid').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxValid').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;

                var chkValidFormula = document.getElementById("chkValidFormula");
                if (chkValidFormula != null)
                    chkValidFormula.checked = true;

                var chkValidConditions = document.getElementById("chkValidConditions");
                if (chkValidConditions != null)
                    chkValidConditions.checked = false;
            });

            $('#ctl00_HomeContentPlaceHolder_txtMaxValid').change(function () {
                document.getElementById('ctl00_HomeContentPlaceHolder_hlValidAdvanced').href = '../Help/FormulaTest.aspx?type=valid&formula='
                + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtValidationEntry').value)
                + '&min=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMinValid').value)
                + '&max=' + encodeURIComponent(document.getElementById('ctl00_HomeContentPlaceHolder_txtMaxValid').value)
                + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
                var chkValidFormula = document.getElementById("chkValidFormula");
                if (chkValidFormula != null)
                    chkValidFormula.checked = true;

                var chkValidConditions = document.getElementById("chkValidConditions");
                if (chkValidConditions != null)
                    chkValidConditions.checked = false;

            });





            $('#ctl00_HomeContentPlaceHolder_txtValidationEntry').keypress(function (e) {

                $('#ctl00_HomeContentPlaceHolder_hlValidEdit').focus();
                return false;

            });

            $('#ctl00_HomeContentPlaceHolder_txtValidationOnWarning').keypress(function (e) {

                $('#ctl00_HomeContentPlaceHolder_hlWarningEdit').focus();
                return false;

            });

            $('#ctl00_HomeContentPlaceHolder_txtValidationOnExceedance').keypress(function (e) {

                $('#ctl00_HomeContentPlaceHolder_hlExceedanceEdit').focus();
                return false;

            });

            $('#ctl00_HomeContentPlaceHolder_txtCalculation').keypress(function (e) {

                $('#ctl00_HomeContentPlaceHolder_hlCalculationEdit').focus();
                return false;

            });
        });

        function ShowHideByColumnType() {
            var strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();
            $("#trReminders").fadeOut();
            $("#trOptionType").fadeOut();
            $("#trListboxType").fadeOut();
            $("#trStaticContent").fadeOut();
            $("#trTrafficLight").fadeOut();
            $("#trDDType").fadeOut();
            $("#trDDValues").fadeOut();
            $("#trOptionImageGrid").fadeOut();
            $("#trDDTable").fadeOut();
            $("#trDDTableLookup").fadeOut();
            $("#trDDDisplayColumn").fadeOut();
            $("#trFilter").fadeOut();
            $("#tdPredictive").fadeOut();

            $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
            $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
            $("#trImageHeightSummary").fadeOut();
            $("#trImageHeightDetail").fadeOut();
            $("#trTextDimension").fadeOut();
            $("#trTextType").fadeOut();
            $("#trDateTimeType").fadeOut();
            $("#trCheckbox1").fadeOut();
            $("#trCheckbox2").fadeOut();
            $("#trCheckbox3").fadeOut();
            $("#trLocation").fadeOut();

            $("#ctl00_HomeContentPlaceHolder_ddlNumberType").fadeOut();
            $('#ctl00_HomeContentPlaceHolder_lblNumberType').fadeOut();
            $("#trFlatLine").fadeOut();
            $("#trRecordCountTable").fadeOut();
            $("#trRecordCountClick").fadeOut();


            $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeOut();
            $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeOut();
            $("#trIgnoreSymbols").fadeOut();
            $("#trCheckUnlikelyValue").fadeOut();
            $("#trCalculation").fadeOut();
            $("#trCalculationType").fadeOut();
            $("#trRound").fadeOut();

            $("#divValidation").fadeOut();
            enableCompareValidatorForDataExceedance(false);

            var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
            //chk.checked = false;
            $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);
            $("#trGraphOption").fadeOut();
            $("#divGraphOptions").fadeOut();

            if (strTypeV == 'radiobutton') {
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
                $('#trDDValues').fadeIn();
                $('#trOptionType').fadeIn();
                $('#trOptionImageGrid').fadeOut();
                var strOptionType = $('#ddlOptionType').val();
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
            var hlResetCalculations = document.getElementById("hlResetCalculations");
            if (strTypeV == 'calculation') {
                $("#trCalculationType").fadeIn();
                ShowHideCalControl();
                if (hlResetCalculations != null && document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value != -1) {
                    hlResetCalculations.style.display = 'block';
                }

                $("#trDefaultValue").fadeOut();

                $("#divValidation").fadeIn();
                enableCompareValidatorForDataExceedance(true);

                $("#trCalculation").fadeIn();

                var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkMandatory");
                tempCheck.checked = false;
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();

                $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeIn();

                // $("#ctl00_HomeContentPlaceHolder_lblIgnoreSymbols").fadeOut();
                $("#trIgnoreSymbols").fadeOut();

                $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkImport").fadeOut();

                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', false);
               


                $("#trGraphOption").fadeIn();
                $("#divGraphOptions").fadeIn();

                //$("#divValidationEntry").fadeOut();
                //$("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeOut();
                //var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinValid");
                //txt.value = '';

                // txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxValid");
                // txt.value = '';

                //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationEntry");
                // txt.value = '';


                $("#divValidationWarning").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeIn();

                var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                tempCheck.checked = false;

                $("#trColumnToAvg").fadeOut();


            }
            else {


                if (hlResetCalculations != null) {
                    hlResetCalculations.style.display = 'none';
                }

                $("#trCalculation").fadeOut();
                $("#trCalculationType").fadeOut();
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                txt.value = '';
                $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                txt.value = '';
            }

            if (strTypeV == 'listbox') {
                $('#trDDValues').fadeIn();
                $("#trDefaultValue").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
                $('#tdListCheckBox').fadeIn();
                $('#trListboxType').fadeIn();
                $("#trTextDimension").fadeIn();

                var txtTextHeight = document.getElementById("ctl00_HomeContentPlaceHolder_txtTextHeight");
                if (txtTextHeight.value != '') {
                    if (txtTextHeight.value < 7) {
                        txtTextHeight.value = '7';
                    }
                }

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
            else {
                $('#tdListCheckBox').fadeOut();



            }

            if (strTypeV == 'number') {
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();

                $('#ctl00_HomeContentPlaceHolder_ddlNumberType').change();
                $("#trReminders").fadeOut();

                $("#ctl00_HomeContentPlaceHolder_ddlNumberType").fadeIn();
                $('#ctl00_HomeContentPlaceHolder_lblNumberType').fadeIn();
                $("#trFlatLine").fadeIn();


                $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeIn();
                $("#trIgnoreSymbols").fadeIn();
                $("#trCheckUnlikelyValue").fadeIn();
                $("#trCalculation").fadeIn();
                $("#trRound").fadeIn();

                $("#divValidation").fadeIn();
                enableCompareValidatorForDataExceedance(true);

                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                //chk.checked = false;
                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', false);
                $("#trGraphOption").fadeIn();
                $("#divGraphOptions").fadeIn();
                //chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDefaultValue");
                // if (chk.checked == true) {
                //    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeIn();
                //}

                $("#trTextDimension").fadeIn();
                $('#tdHeightLabel').fadeOut();
                $('#tdHeight').fadeOut();

                ShowHideDefaultControl();
            }


            if (strTypeV == 'location') {

                $("#trLocation").fadeIn();

                $("#trDefaultValue").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                $("#trReminders").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);
                $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);
                $("#ctl00_HomeContentPlaceHolder_chkExport").attr('disabled', true);
                $("#ctl00_HomeContentPlaceHolder_chkMobile").attr('disabled', true);
                $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);
            }


            if (strTypeV == 'checkbox') {
                $("#trCheckbox1").fadeIn();
                $("#trCheckbox2").fadeIn();
                $("#trCheckbox3").fadeIn();
                $("#trDefaultValue").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                $("#trReminders").fadeOut();
            }


            var strDateTimeTypeV = $('#ddlDateTimeType').val();

            if (strTypeV == 'date_time') {
                $("#trFlatLine").fadeOut();
                $("#trDateTimeType").fadeIn();
                //$("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default to Today');
                $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default');
                $('#ddlDefaultValue option[value="value"]').text('To Today');

                if (strDateTimeTypeV == 'datetime' || strDateTimeTypeV == 'date') {
                    $("#trReminders").fadeIn();
                }
                else {
                    $("#trReminders").fadeOut();
                }
                $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
            }

            var x = document.getElementById("ctl00_HomeContentPlaceHolder_hlCalculationEdit");
            if (strTypeV == 'text') {
                if (x != null) {
                    x.style.display = 'none';
                }
                $("#trTextDimension").fadeIn();
                $("#trTextType").fadeIn();
                $("#trReminders").fadeOut();

                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
            }

            if (strTypeV == 'image') {
                $("#trImageHeightSummary").fadeIn();
                $("#trImageHeightDetail").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
            }




            if (strTypeV == 'file' || strTypeV == 'image') {

                $("#trTextDimension").fadeOut();
                $("#trDefaultValue").fadeOut();
                $("#trFlatLine").fadeOut();
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                chk.checked = false;
                $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);
                $("#divGraphOptions").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
                $("#trReminders").fadeOut();

                $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();

                var txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport");
                txtNI.value = '';
                txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImportTime");
                txtNI.value = '';

                var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                tempCheck.checked = false;

                $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);

            }
            //            else {
            //                $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeIn();
            //                $("#ctl00_HomeContentPlaceHolder_chkImport").fadeIn();
            //                $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', false);
            //            }



            if (strTypeV == 'dropdown') {

                $('#trDDType').fadeIn();
                $('#trDDValues').fadeIn();
                $("#trReminders").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
                var strDDType = $('#ddlDDType').val();

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
                    // $("#trDefaultValue").fadeOut();
                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                    txtDefaultValue.value = '';
                    $("#trTextDimension").fadeIn();
                    $('#tdHeightLabel').fadeOut();
                    $('#tdHeight').fadeOut();
                    $("#trFilter").fadeOut();
                    $("#tdPredictive").fadeIn();

                    //var chkFilterValues = document.getElementById("chkFilterValues");
                    // if (chkFilterValues.checked) {
                    if (strDDType == 'lt') {
                        $("#trFilter").fadeIn();
                        $("#tdPredictive").fadeOut();
                        $("#lblDisplayColumn").text('2nd Dropdown');
                        $("#divQuickAddLink").fadeOut();
                    }
                    else {
                        $("#trFilter").fadeOut();
                        $("#tdPredictive").fadeIn();
                        $("#lblDisplayColumn").text('Display Field');
                        $("#divQuickAddLink").fadeIn();
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
                    $("#trFilter").fadeOut();

                    $("#tdPredictive").fadeOut();
                    $("#trFilter").fadeOut();
                    if (strDDType == 'values') {
                        $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesCap").text('Values');
                        $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesHelp").text('Enter the values – one on each line');
                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesCap").text('Value & Text');
                        $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesHelp").text('Enter value, comma(,) and text – one on each line');
                    }
                }
            }
            //            if (strTypeV == 'date_time') {
            //                $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
            //            }

            if (strTypeV == 'staticcontent' || strTypeV == 'button') {
                $("#trDefaultValue").fadeOut();
                $("#trSummaryPage1").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                $("#trReminders").fadeOut();
                $("#trHelpText").fadeOut();
                //$("#ctl00_HomeContentPlaceHolder_chkSummaryPage").attr('disabled', true);
                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);
                $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);
                $("#ctl00_HomeContentPlaceHolder_chkExport").attr('disabled', true);
                $("#ctl00_HomeContentPlaceHolder_chkMobile").attr('disabled', true);
                if (strTypeV == 'staticcontent') {
                    $("#trStaticContent").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeOut();
                    $("#trButton1").fadeOut();
                    $("#trButton2").fadeOut();
                    $("#trButton3").fadeOut();
                    $("#trButton4").fadeOut();

                }
                else {
                    $("#trStaticContent").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeIn();
                    $("#trButton1").fadeIn();
                    $("#trButton2").fadeIn();
                    $("#trButton3").fadeIn();
                    $("#trButton4").fadeIn();
                    //$("#trTextDimension").fadeIn();
                }

                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                $("#tblColumnColour").fadeOut();
                $("#divGraphOptions").fadeOut();
                $("#divchkCompareOperator").fadeOut();
                $("#divchkValidationCanIgnore").fadeOut();
            }
            else {
                $("#trSummaryPage1").fadeIn();
                $("#trButton1").fadeOut();
                $("#trButton2").fadeOut();
                $("#trButton3").fadeOut();
                $("#trButton4").fadeOut();

                $("#tblColumnColour").fadeIn();
                $("#divchkCompareOperator").fadeIn();
                $("#divchkValidationCanIgnore").fadeIn();
            }





            if (strTypeV == 'trafficlight') {
                $("#trDefaultValue").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                $("#trReminders").fadeOut();
                $("#trHelpText").fadeOut();
                $("#trTrafficLight").fadeIn();
            }



            if (strTypeV == 'calculation') {



                $("#divValidation").fadeIn();
                enableCompareValidatorForDataExceedance(true);

                $("#trCalculationType").fadeIn();
                ShowHideCalControl();
            }

            if (strTypeV == 'location') {
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
            }


        }

        function ShowHideByNumberType() {
            var strNumberTypeV = $('#ctl00_HomeContentPlaceHolder_ddlNumberType').val();
            var x = document.getElementById("ctl00_HomeContentPlaceHolder_hlCalculationEdit");

            if (strNumberTypeV == "2") {
                $("#trFlatLine").fadeOut();
                $("#trCheckUnlikelyValue").fadeOut();

                $("#divGraphOptions").fadeOut();

                $("#divchkCompareOperator").fadeOut();
                $("#divchkValidationCanIgnore").fadeOut();
                $("#divValidationRoot").fadeOut();

                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();

                $("#trRound").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeOut();

                $("#trIgnoreSymbols").fadeOut();

            }
            else {
                $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();

                $("#divGraphOptions").fadeIn();

                $("#divchkCompareOperator").fadeIn();
                $("#divchkValidationCanIgnore").fadeIn();
                $("#divValidationRoot").fadeIn();

            }



            if (strNumberTypeV == "4") {

            }
            else {
                $("#trColumnToAvg").fadeOut();
                $("#trAvgNumValues").fadeOut();
            }

            if (strNumberTypeV == "5") {
                $("#trRecordCountTable").fadeIn();
                $("#trRecordCountClick").fadeIn();
                $("#trFlatLine").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();

            }

            if (strNumberTypeV == "6") {

                $("#ctl00_HomeContentPlaceHolder_lblSymbol").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_txtSymbol").fadeIn();

            }
            else {
                $("#ctl00_HomeContentPlaceHolder_lblSymbol").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtSymbol").fadeOut();
            }


            if (strNumberTypeV == "7") {
                $("#trSlider").fadeIn();
                $("#trFlatLine").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_chkImport").fadeIn();


                $("#divValidationEntry").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeIn();

                $("#divValidationWarning").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeIn();
                $("#trIgnoreSymbols").fadeOut();
                $("#trRound").fadeOut();
                $("#trCheckUnlikelyValue").fadeOut();
                $("#trColumnToAvg").fadeOut();
                $("#trAvgNumValues").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                $("#trDefaultValue").fadeIn();

            }
            else {
                $("#trSlider").fadeOut();
            }

            var hlResetIDs = document.getElementById("hlResetIDs");
            if (strNumberTypeV == "8") {


                if (hlResetIDs != null && document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value != -1) {
                    hlResetIDs.style.display = 'block';
                }


                $("#trGraphOption").fadeOut();
                $("#divGraphOptions").fadeOut();
                $("#trSlider").fadeOut();
                $("#trFlatLine").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_chkImport").fadeIn();


                //$("#divValidationEntry").fadeOut();
                //$("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeOut();

                //$("#divValidationWarning").fadeOut();
                // $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeOut();

                $("#divValidation").fadeOut();
                enableCompareValidatorForDataExceedance(false);
                clearMinMaxExceedanceValue();

                $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeOut();
                $("#trIgnoreSymbols").fadeOut();
                $("#trRound").fadeOut();
                $("#trCheckUnlikelyValue").fadeOut();
                $("#trColumnToAvg").fadeOut();
                $("#trAvgNumValues").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                $("#trDefaultValue").fadeOut();

            }
            else {
                if (hlResetIDs != null) {
                    hlResetIDs.style.display = 'none';
                }
            }

            //FieldType = Content 
            if (strNumberTypeV == "1") {
                showUnrequiredControlsForContentType(false);
            }
        }

        function ShowHide() {

            var hfShowExceedance = document.getElementById("hfShowExceedance");
            if (hfShowExceedance != null) {
                if (hfShowExceedance.value == 'no') {
                    $("#trExceedance1").fadeOut();
                    $("#trExceedance2").fadeOut();
                }
            }

            var strTextType = $('#ddlTextType').val();
            if (strTextType == "own") {
                $("#txtOwnRegEx").fadeIn();
                $("#hlRegEx").fadeIn();
            }
            else {
                $("#txtOwnRegEx").fadeOut();
                $("#hlRegEx").fadeOut();
            }


            var xx = document.getElementById("hfShowWarningMinMax");
            if (xx.value == 'yes') {
                ShowWarningMinMax('yes');
            }

            if (xx.value == 'no') {
                ShowWarningMinMax('no');
            }


            var ee = document.getElementById("hfShowExceedanceMinMax");
            if (ee.value == 'yes') {
                ShowExceedanceMinMax('yes');
            }

            if (ee.value == 'no') {
                ShowExceedanceMinMax('no');
            }
            var yy = document.getElementById("hfShowValidMinMax");
            if (yy.value == 'yes') {
                ShowValidMinMax('yes');
            }

            if (yy.value == 'no') {
                ShowValidMinMax('no');
            }



            var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkSummaryPage");
            var x = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextSummary");
            if (chk.checked == false) { x.style.display = 'none'; }
            x = document.getElementById("ctl00_HomeContentPlaceHolder_lblSummaryPage");
            if (chk.checked == false) { x.style.display = 'none'; }
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_lblAlignment");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_ddlAlignment");
            //            if (chk.checked == false) { x.style.display = 'none'; }

            //            x = document.getElementById("tblCellBackColor");
            //            if (chk.checked == false) { x.style.display = 'none'; }


            //if (chk.checked == false) {
            //    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeOut();
            //    $("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeOut();
            //    $("#trSummaryPage2").fadeOut();
            //    $("#trSummaryPage3").fadeOut();
            //}

            chk = document.getElementById("chkFlatLine");
            //            x = document.getElementById("lblFlatlineNumber");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            //            x = document.getElementById("txtFlatLine");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            if (chk.checked == false) {
                $("#lblFlatlineNumber").fadeOut();
                $("#txtFlatLine").fadeOut();
            }

            chk = document.getElementById("chkShowMap");
            if (chk.checked == false) {
                $("#tblMapDimension").fadeOut();
            }


            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDetailPage");
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_lblDetailPage");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextDetail");
            //            if (chk.checked == false) { x.style.display = 'none'; }

            //            x = document.getElementById("chkDisplayOnRight");
            //            if (chk.checked == false) { x.style.display = 'none'; }

            //            x = document.getElementById("lblDisplayOnRight");
            //            if (chk.checked == false) { x.style.display = 'none'; }

            //            x = document.getElementById("chkShowWhen");
            //            if (chk.checked == false) { x.style.display = 'none'; }

            //            x = document.getElementById("hlShowWhen");
            //            if (chk.checked == false) { x.style.display = 'none'; }


            if (chk.checked == false) {
                $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeOut();
                $("#trDetailPage2").fadeOut();
            }


            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExceedence");
            var x = document.getElementById("ctl00_HomeContentPlaceHolder_txtExceedence");
            if (chk.checked == false) { x.disabled = true; }

            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkWarning");
            x = document.getElementById("ctl00_HomeContentPlaceHolder_txtWarning");
            if (chk.checked == false) { x.disabled = true; }

            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMaximumValueat");
            x = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaximumValueat");
            if (chk.checked == false) { x.disabled = true; }


            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_lblGraph");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_txtGraph");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            if (chk.checked == false) {
                $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
            }


            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_lblImport");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            if (chk.checked == false) {
                $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
            }


            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExport");
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_lblExport");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnExport");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            if (chk.checked == false) {
                $("#ctl00_HomeContentPlaceHolder_lblExport").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtNameOnExport").fadeOut();
            }

            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMobile");
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_lblMobile");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            //            x = document.getElementById("ctl00_HomeContentPlaceHolder_txtMobile");
            //            if (chk.checked == false) { x.style.display = 'none'; }
            if (chk.checked == false) {
                $("#ctl00_HomeContentPlaceHolder_lblMobile").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtMobile").fadeOut();
            }




            //reminders

            //            chk = document.getElementById("chkReminders");
            //            if (chk.checked == false) {
            //                $("#hlReminders").fadeOut();
            //            }
            //            else {
            //                $("#hlReminders").fadeIn();
            //            }




            $("#ctl00_HomeContentPlaceHolder_lblSymbol").fadeOut();
            $("#ctl00_HomeContentPlaceHolder_txtSymbol").fadeOut();


            ShowHideByColumnType();





            //Round
            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkRound");

            if (chk.checked == false) {
                $("#ctl00_HomeContentPlaceHolder_txtRoundNumber").fadeOut();
                //$("#ctl00_HomeContentPlaceHolder_lblRoundNumber").fadeOut();
            }


            //Default value

            //            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDefaultValue");
            //            if (chk.checked == false) {
            //                $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
            //            }

            ShowHideDefaultControl();

            //chkCalculated


            var hfColumnSystemname = document.getElementById("hfColumnSystemname");
            if (hfColumnSystemname.value == 'recordid') {
                $("#divColumnType").fadeOut();
            }

            if (hfColumnSystemname.value == 'enteredby') {
                $("#divColumnType").fadeOut();
            }

            if (hfColumnSystemname.value == 'notes') {
                $("#trFlatLine").fadeOut();
            }

            var hfDateTimeColumn = document.getElementById("ctl00_HomeContentPlaceHolder_hfDateTimeColumn");

            if (hfDateTimeColumn.value == 'yes') {

                $("#trTextDimension").fadeOut();
                $("#trDefaultValue").fadeIn();
                //                $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default to current date/time');
                $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default');
                $('#ddlDefaultValue option[value="value"]').text('To Today');

                $("#trFlatLine").fadeOut();
                var opt = document.getElementById("ctl00_HomeContentPlaceHolder_optSingle");
                if (opt.checked == true) {
                    $("#trTimeSection").fadeOut();
                }
                else {
                    $("#trTimeSection").fadeIn();
                }

            }




            if (document.getElementById("hfIsSystemColumn").value == 'yes') {
                $("#divValidation").fadeOut();
                enableCompareValidatorForDataExceedance(false);
                clearMinMaxExceedanceValue();

                $("#divValidationRoot").css("min-height", "200px");
            }
            else {
                $("#divValidationRoot").removeAttr("style");
            }

            ShowHideByNumberType();

            if (strTextType == "readonly") {
                var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkMandatory");
                tempCheck.checked = false;
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
            }
            else {
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
            }

            var strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();

            if (strTypeV == 'location') {
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
            }
            if (strTypeV == 'button') {
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                $("#tblColumnColour").fadeOut();
                $("#divGraphOptions").fadeOut();
                $("#divchkCompareOperator").fadeOut();
                $("#divchkValidationCanIgnore").fadeOut();
            }

            //ShowHideCompareControls();

            var strDDTableID = document.getElementById('hf_ddlDDTable').value;
            if (strDDTableID == '-1') {
                $("#tdTableFilter").fadeOut();
                $("#divQuickAddLink").fadeOut();
                var hlDDEdit = document.getElementById('hlDDEdit');
                if (hlDDEdit != null) {
                    hlDDEdit.style.display = 'none';
                }
            }


            setTimeout(function () { ShowHideCompareControls(); }, 1000);
            //setTimeout(function () { ddlDDTable_Change(); }, 1000);


        }

        function PutColumnName() {
            // alert('ss');
            var txtColumnName = document.getElementById("ctl00_HomeContentPlaceHolder_txtColumnName");




            var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkSummaryPage");
            if (chk.checked == true) {
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextSummary");
                txt.value = txtColumnName.value;
            }

            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDetailPage");
            if (chk.checked == true) {
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextDetail");
                txt.value = txtColumnName.value;
            }

            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
            if (chk.checked == true) {
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtGraph");
                txt.value = txtColumnName.value;
            }

            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
            if (chk.checked == true) {
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport");
                var hfIsIP = document.getElementById("ctl00_HomeContentPlaceHolder_hfIsImportPositional");

                if (hfIsIP.value == '0') {
                    txt.value = txtColumnName.value;
                }

            }

            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExport");
            if (chk.checked == true) {
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnExport");
                txt.value = txtColumnName.value;
            }

            chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMobile");
            if (chk.checked == true) {
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMobile");
                txt.value = txtColumnName.value;
            }


        }



        if (window.addEventListener)
            window.addEventListener("load", ShowHide, false);
        else if (window.attachEvent)
            window.attachEvent("onload", ShowHide);
        else if (document.getElementById)
            window.onload = ShowHide;

        $(document).ready(function () {

            $('#ctl00_HomeContentPlaceHolder_txtColumnName').change(function () {
                PutColumnName();
            });
            $('#ctl00_HomeContentPlaceHolder_txtColumnName').keyup(function () {
                PutColumnName();
            });
            $('#ctl00_HomeContentPlaceHolder_ddlCalculationType').change(function () {
                ShowHideCalControl();
            });

            //            $("#chkFilterValues").click(function () {
            //                var chkFilterValues = document.getElementById("chkFilterValues");
            //                if (chkFilterValues.checked) {
            //                    $("#trFilter").fadeIn();
            //                }
            //                else {
            //                    $("#trFilter").fadeOut();
            //                }
            //            });

            var txtCN = document.getElementById("ctl00_HomeContentPlaceHolder_txtColumnName");

            //$("#ctl00_HomeContentPlaceHolder_chkSummaryPage").click(function () {
            //    var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkSummaryPage");
            //    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextSummary");
            //    if (chk.checked == true) {
            //        $("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeIn();
            //        $("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeIn();
            //        //                    $("#ctl00_HomeContentPlaceHolder_lblAlignment").fadeIn();
            //        //                    $("#ctl00_HomeContentPlaceHolder_ddlAlignment").fadeIn();
            //        //                    $("#tblCellBackColor").fadeIn();
            //        $("#trSummaryPage2").fadeIn();
            //        $("#trSummaryPage3").fadeIn();
            //        txt.value = txtCN.value;

            //    }
            //    else {
            //        $("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeOut();
            //        $("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeOut();
            //        //                    $("#ctl00_HomeContentPlaceHolder_lblAlignment").fadeOut();
            //        $("#ctl00_HomeContentPlaceHolder_ddlAlignment").val('-1');
            //        //                    document.getElementById('ctl00_HomeContentPlaceHolder_ddlAlignment').value = '-1';
            //        //                    $("#ctl00_HomeContentPlaceHolder_ddlAlignment").fadeOut();
            //        //                    $("#tblCellBackColor").fadeOut();
            //        $("#trSummaryPage2").fadeOut();
            //        $("#trSummaryPage3").fadeOut();

            //        txt.value = '';
            //    }
            //});

            $("#chkShowWhen").click(function () {
                var chkShowWhen = document.getElementById("chkShowWhen");
                if (chkShowWhen.checked == true) {
                    $("#hlShowWhen").trigger("click");
                }

            });
            $("#chkValidFormula").click(function () {
                var chkValidFormula = document.getElementById("chkValidFormula");
                if (chkValidFormula.checked == true) {
                    if ($("#ctl00_HomeContentPlaceHolder_hlValidAdvanced").is(":visible")) {
                        $("#ctl00_HomeContentPlaceHolder_hlValidAdvanced").trigger("click");
                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_hlValidEdit").trigger("click");
                    }

                    document.getElementById('hlValidConditions').innerHTML = 'Conditions';
                    var chkValidConditions = document.getElementById("chkValidConditions");
                    if (chkValidConditions != null)
                        chkValidConditions.checked = false;
                }

            });

            $("#chkValidConditions").click(function () {
                var chkValidConditions = document.getElementById("chkValidConditions");
                if (chkValidConditions.checked == true) {
                    $("#hlValidConditions").trigger("click");
                    var chkValidFormula = document.getElementById("chkValidFormula");
                    if (chkValidFormula != null)
                        chkValidFormula.checked = false;
                }
            });

            $("#chkWarningFormula").click(function () {
                var chkWarningFormula = document.getElementById("chkWarningFormula");
                if (chkWarningFormula.checked == true) {
                    if ($("#ctl00_HomeContentPlaceHolder_hlWarningAdvanced").is(":visible")) {
                        $("#ctl00_HomeContentPlaceHolder_hlWarningAdvanced").trigger("click");
                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_hlWarningEdit").trigger("click");
                    }

                    document.getElementById('hlWarningConditions').innerHTML = 'Conditions';
                    var chkWarningConditions = document.getElementById("chkWarningConditions");
                    if (chkWarningConditions != null)
                        chkWarningConditions.checked = false;
                }

            });

            $("#chkWarningConditions").click(function () {
                var chkWarningConditions = document.getElementById("chkWarningConditions");
                if (chkWarningConditions.checked == true) {
                    $("#hlWarningConditions").trigger("click");
                    var chkWarningFormula = document.getElementById("chkWarningFormula");
                    if (chkWarningFormula != null)
                        chkValidFormula.checked = false;
                }
            });




            $("#chkExceedanceFormula").click(function () {
                var chkExceedanceFormula = document.getElementById("chkExceedanceFormula");
                if (chkExceedanceFormula.checked == true) {
                    if ($("#ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced").is(":visible")) {
                        $("#ctl00_HomeContentPlaceHolder_hlExceedanceAdvanced").trigger("click");
                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_hlExceedanceEdit").trigger("click");
                    }

                    document.getElementById('hlExceedanceConditions').innerHTML = 'Conditions';
                    var chkExceedanceConditions = document.getElementById("chkExceedanceConditions");
                    if (chkExceedanceConditions != null)
                        chkExceedanceConditions.checked = false;
                }

            });

            $("#chkExceedanceConditions").click(function () {
                var chkExceedanceConditions = document.getElementById("chkExceedanceConditions");
                if (chkExceedanceConditions.checked == true) {
                    $("#hlValidConditions").trigger("click");
                    var chkExceedanceFormula = document.getElementById("chkExceedanceFormula");
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

                ShowHideImageOnSummary();
            });

            $("#chkButtonWarningMessage").click(function () {
                ShowHideButtonWarningMessage();
            });
            $("#chkButtonOpenLink").click(function () {
                ShowHideButtonOpenLink();
            });
            $("#chkSPToRun").click(function () {
                ShowHideSPToRun();
            });


            $("#ctl00_HomeContentPlaceHolder_chkSummaryPage").click(function () {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkSummaryPage");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextSummary");
                if (chk.checked == true) {
                    txt.value = txtCN.value;
                    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeIn();
                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeOut();
                    txt.value = '';
                }
            });

            $("#ctl00_HomeContentPlaceHolder_chkDetailPage").click(function () {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDetailPage");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDisplayTextDetail");
                if (chk.checked == true) {

                    //                    $("#chkDisplayOnRight").fadeIn();
                    //                    $("#lblDisplayOnRight").fadeIn();
                    //                    $("#chkShowWhen").fadeIn();
                    //                    $("#hlShowWhen").fadeIn();
                    $("#trDetailPage2").fadeIn();
                    txt.value = txtCN.value;
                    var strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();
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
                    //                    $("#chkDisplayOnRight").fadeOut();
                    //                    $("#lblDisplayOnRight").fadeOut();

                    //                    $("#chkShowWhen").fadeOut();
                    //                    $("#hlShowWhen").fadeOut();
                    $("#trDetailPage2").fadeOut();

                    txt.value = '';
                }
            });



            $("#ctl00_HomeContentPlaceHolder_chkGraph").click(function () {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtGraph");
                if (chk.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeIn();

                    txt.value = txtCN.value;

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();

                    txt.value = '';
                }
            });


            function ImportSH() {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport");
                var hfDateTimeColumn = document.getElementById("ctl00_HomeContentPlaceHolder_hfDateTimeColumn");




                if (chk.checked == true) {


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
                    //txtD.value = '';

                    var hfIsIP = document.getElementById("ctl00_HomeContentPlaceHolder_hfIsImportPositional");

                    if (hfIsIP.value == '0') {
                        txt.value = txtCN.value;
                    }
                    else {
                        var hfPosMax = document.getElementById("ctl00_HomeContentPlaceHolder_hfMaxPosition");
                        txt.value = hfPosMax.value;

                        //                        if (txtCN.value == 'Date Time Recorded') {
                        //                            // txt.value = hfPosMax.value + ',' + (parseInt(hfPosMax.value) + 1);
                        //                        }
                        //                        else {
                        //                            txt.value = hfPosMax.value;
                        //                        }
                    }

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    txt.value = '';
                    if (hfDateTimeColumn.value == 'yes') {
                        $("#tblDateOptions").fadeOut();
                        $("#trDateFormat").fadeOut();
                        $("#trTimeSection").fadeOut();
                        var txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport");
                        txtNI.value = '';
                        txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImportTime");
                        txtNI.value = '';

                        var opt = document.getElementById("ctl00_HomeContentPlaceHolder_optSingle");
                        opt.checked = true;
                    }


                }
            }
            $("#ctl00_HomeContentPlaceHolder_chkImport").click(function () {
                ImportSH();
            });

            function ExportSH() {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExport");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnExport");
                if (chk.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_lblExport").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnExport").fadeIn();
                    txt.value = txtCN.value;

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblExport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnExport").fadeOut();
                    txt.value = '';
                }
            }
            $("#ctl00_HomeContentPlaceHolder_chkExport").click(function () {
                ExportSH();
            });


            $("#ctl00_HomeContentPlaceHolder_chkMobile").click(function () {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMobile");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMobile");
                if (chk.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_lblMobile").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtMobile").fadeIn();
                    txt.value = txtCN.value;

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblMobile").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtMobile").fadeOut();
                    txt.value = '';
                }
            });

            $("#ctl00_HomeContentPlaceHolder_chkExceedence").click(function () {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExceedence");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtExceedence");
                if (chk.checked == true) {
                    txt.disabled = false;
                }
                else {
                    txt.value = '';
                    txt.disabled = true;
                }
            });



            $("#ctl00_HomeContentPlaceHolder_chkWarning").click(function () {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkWarning");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtWarning");
                if (chk.checked == true) {
                    txt.disabled = false;
                }
                else {
                    txt.value = '';
                    txt.disabled = true;
                }
            });


            $("#chkCompareOperator").click(function () {
                ShowHideCompareControls();
            });


            $("#ctl00_HomeContentPlaceHolder_chkMaximumValueat").click(function () {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMaximumValueat");
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaximumValueat");
                if (chk.checked == true) {
                    txt.disabled = false;
                }
                else {
                    txt.value = '';
                    txt.disabled = true;
                }
            });

            $('#ddDDDisplayColumn').change(function (e) {
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


            });

            $('#ddlOptionType').change(function (e) {
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
            });

            $('#ddlListBoxType').change(function (e) {
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
            });

            $('#ddlDDType').change(function (e) {
                var strDDType = $('#ddlDDType').val();

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

                    $("#tdPredictive").fadeIn();
                    //var chkFilterValues = document.getElementById("chkFilterValues");
                    if (strDDType == 'lt') {
                        $("#trFilter").fadeIn();
                        $("#tdPredictive").fadeOut();
                        $("#lblDisplayColumn").text('2nd Dropdown');
                        $("#divQuickAddLink").fadeOut();
                    }
                    else {
                        $("#trFilter").fadeOut();
                        $("#tdPredictive").fadeIn();
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
                    $("#tdPredictive").fadeOut();

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
                //                    $("#tdPredictive").fadeIn();
                //                }


            });

            //$('#ddDDDisplayColumn').change(function (e) {
            //    var strDDDC = $('#ddDDDisplayColumn').val();
            //    if (strDDDC == '') {
            //        $('#hlDDEdit').fadeIn();
            //    }
            //    else {
            //        $('#hlDDEdit').fadeOut();
            //    }
            //});
            function ddlDDTable_Change() {
                var strDDTableID = $('#ddlDDTable').val();
                //alert('a');
                var strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();
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

                ddlDDTable_Change();

                //$('#ddDDDisplayColumn').change();
            });

            $('#ddlTextType').change(function (e) {
                var strTextType = $('#ddlTextType').val();
                if (strTextType == "own") {
                    $("#txtOwnRegEx").fadeIn();
                    $("#hlRegEx").fadeIn();
                }
                else {
                    $("#txtOwnRegEx").fadeOut();
                    $("#hlRegEx").fadeOut();
                }

                if (strTextType == "readonly") {
                    var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkMandatory");
                    tempCheck.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
                }

            });


            //            $('#ctl00_HomeContentPlaceHolder_ddlTLControllingField').change(function (e) {
            //                var strTLColumn = $('#ctl00_HomeContentPlaceHolder_ddlTLControllingField').val();
            //                if (strTLColumn == '') {
            //                    $("#trTLValueImage").fadeOut();
            //                }
            //                else {
            //                    $("#trTLValueImage").fadeIn();
            //                }
            //            });


            $('#ctl00_HomeContentPlaceHolder_ddlType').change(function (e) {
                $('#trOptionImageGrid').fadeOut();
                $("#tblFinancialSymbol").fadeOut();
                $("#tblDateCal").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_chkImport").fadeIn();
                var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                tempCheck.checked = true;

                ImportSH();


                var tempCheckE = document.getElementById("ctl00_HomeContentPlaceHolder_chkExport");
                tempCheckE.checked = true;
                ExportSH();
                $("#trDefaultValue").fadeIn();
                //                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDefaultValue");
                //                chk.checked = false;
                //                $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                //                var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                //                txtDefaultValue.value = '';
                ShowHideDefaultControl();
                var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                txtDefaultValue.value = '';
                $("#trSlider").fadeOut();
                $("#trStaticContent").fadeOut();
                $("#trTrafficLight").fadeOut();
                $("#trImageHeightSummary").fadeOut();
                $("#trImageHeightDetail").fadeOut();


                $('#trOptionType').fadeOut();
                $('#trListboxType').fadeOut();

                $("#trRecordCountTable").fadeOut();
                $("#trRecordCountClick").fadeOut();

                $("#divValidation").fadeOut();
                enableCompareValidatorForDataExceedance(false);
                clearMinMaxExceedanceValue();

                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                chk.checked = false;
                $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);
                $("#divGraphOptions").fadeOut();

                var chkFlatLine = document.getElementById("chkFlatLine");
                chkFlatLine.style.display = 'none';
                chkFlatLine.checked = false;
                $("#lblCheckForFlat").fadeOut();
                $("#lblFlatlineNumber").fadeOut();
                var txtFlatLine = document.getElementById("txtFlatLine");
                txtFlatLine.style.display = 'none';

                $("#trGraphOption").fadeOut();

                chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDetailPage");
                if (chk.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeIn();
                }

                var hfDateTimeColumn = document.getElementById("ctl00_HomeContentPlaceHolder_hfDateTimeColumn");
                if (hfDateTimeColumn.value == 'yes') {
                    return;
                }

                var strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();
                var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();

                $("#trLocation").fadeOut();

                $("#ctl00_HomeContentPlaceHolder_chkSummaryPage").attr('disabled', false);
                $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', false);
                $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', false);
                $("#ctl00_HomeContentPlaceHolder_chkExport").attr('disabled', false);
                $("#ctl00_HomeContentPlaceHolder_chkMobile").attr('disabled', false);
                //                $("#trHelpText").fadeIn();
                $("#trStaticContent").fadeOut();
                $("#trTrafficLight").fadeOut();
                $('#trDDType').fadeOut();
                $('#trDDValues').fadeOut();
                $('#trDDTable').fadeOut();
                $("#trDDTableLookup").fadeOut();
                $('#trDDDisplayColumn').fadeOut();
                $('#ddlDDType').val("values");
                $('#trFilter').fadeOut();
                $("#tdPredictive").fadeOut();
                $('#trTextType').fadeOut();
                $("#trTextDimension").fadeOut();


                $('#ctl00_HomeContentPlaceHolder_ddlNumberType').fadeOut();
                $('#ctl00_HomeContentPlaceHolder_lblNumberType').fadeOut();
                $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();

                txt.value = '';
                $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeOut();
                var chkST = document.getElementById("ctl00_HomeContentPlaceHolder_chkShowTotal");
                chkST.checked = false;
                $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeOut();

                var chkIS = document.getElementById("ctl00_HomeContentPlaceHolder_chkIgnoreSymbols");
                chkIS.checked = false;
                $("#trIgnoreSymbols").fadeOut();

                var chkR = document.getElementById("ctl00_HomeContentPlaceHolder_chkRound");
                chkR.checked = false;
                $("#trRound").fadeOut();

                $("#trCheckUnlikelyValue").fadeOut();


                $("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeIn();
                $("#divValidationEntry").fadeIn();



                $("#trColumnToAvg").fadeOut();
                $("#trAvgNumValues").fadeOut();

                $('#ctl00_HomeContentPlaceHolder_ddlNumberType').val("1");

                $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();

                $("#trCheckbox1").fadeOut();
                $("#trCheckbox2").fadeOut();
                $("#trCheckbox3").fadeOut();

                if (strTypeV == 'location') {
                    $("#trLocation").fadeIn();
                    $("#trDefaultValue").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    $("#trReminders").fadeOut();

                    var chk = document.getElementById("chkShowMap");
                    chk.checked = true;
                    $("#tblMapDimension").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();

                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);




                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);


                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExport");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblExport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnExport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkExport").attr('disabled', true);

                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMobile");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblMobile").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtMobile").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMobile").attr('disabled', true);
                }



                if (strTypeV == 'staticcontent' || strTypeV == 'button') {
                    $("#trDefaultValue").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    $("#trReminders").fadeOut();
                    $("#trHelpText").fadeOut();
                    $("#trSummaryPage1").fadeOut();
                    //var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkSummaryPage");
                    //chk.checked = false;
                    //$("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeOut();
                    //$("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeOut();
                    ////                    $("#ctl00_HomeContentPlaceHolder_lblAlignment").fadeOut();
                    ////                    $("#ctl00_HomeContentPlaceHolder_ddlAlignment").fadeOut();
                    ////                    $("#tblCellBackColor").fadeOut();
                    //$("#trSummaryPage2").fadeOut();
                    //$("#trSummaryPage3").fadeOut();
                    //$("#ctl00_HomeContentPlaceHolder_chkSummaryPage").attr('disabled', true);

                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);

                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);


                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExport");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblExport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnExport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkExport").attr('disabled', true);

                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMobile");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblMobile").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtMobile").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMobile").attr('disabled', true);

                    if (strTypeV == 'staticcontent') {
                        $("#trStaticContent").fadeIn();
                        $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeOut();
                        $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeOut();
                        $("#trButton1").fadeOut();
                        $("#trButton2").fadeOut();
                        $("#trButton3").fadeOut();
                        $("#trButton4").fadeOut();
                    }
                    else {
                        $("#trStaticContent").fadeOut();
                        $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeIn();
                        $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeIn();
                        $("#trButton1").fadeIn();
                        $("#trButton2").fadeIn();
                        $("#trButton3").fadeIn();
                        $("#trButton4").fadeIn();
                    }
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    $("#tblColumnColour").fadeOut();
                    $("#divGraphOptions").fadeOut();
                    $("#divchkCompareOperator").fadeOut();
                    $("#divchkValidationCanIgnore").fadeOut();
                }
                else {
                    $("#trSummaryPage1").fadeIn();
                    $("#trButton1").fadeOut();
                    $("#trButton2").fadeOut();
                    $("#trButton3").fadeOut();
                    $("#trButton4").fadeOut();

                    $("#tblColumnColour").fadeIn();
                    $("#divchkCompareOperator").fadeIn();
                    $("#divchkValidationCanIgnore").fadeIn();
                }




                if (strTypeV == 'trafficlight') {
                    $("#trTrafficLight").fadeIn();
                    $("#trStaticContent").fadeOut();
                    $("#trDefaultValue").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    $("#trReminders").fadeOut();
                    $("#trHelpText").fadeOut();

                }


                if (strTypeV == 'image') {
                    $("#trImageHeightSummary").fadeIn();
                    $("#trImageHeightDetail").fadeIn();

                }

                //                if (strTypeV == 'data_retriever') {
                //                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                //                   
                //                    $("#trTextDimension").fadeOut();
                //                    $("#trDefaultValue").fadeOut();
                //                    $("#trFlatLine").fadeOut();
                //                    var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                //                    chk.checked = false;
                //                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                //                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
                //                    $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);
                //                    $("#divGraphOptions").fadeOut();
                //                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                //                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
                //                    $("#trReminders").fadeOut();

                //                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                //                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();

                //                    var txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport");
                //                    txtNI.value = '';
                //                    txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImportTime");
                //                    txtNI.value = '';

                //                    var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                //                    tempCheck.checked = false;

                //                    $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);
                //                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                //                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();

                //                }


                if (strTypeV == 'file' || strTypeV == 'image') {

                    $("#trTextDimension").fadeOut();
                    $("#trDefaultValue").fadeOut();
                    $("#trFlatLine").fadeOut();

                    $("#divValidation").fadeOut();
                    enableCompareValidatorForDataExceedance(false);
                    clearMinMaxExceedanceValue();

                    var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);
                    $("#divGraphOptions").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();
                    $("#trReminders").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();

                    var txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport");
                    txtNI.value = '';
                    txtNI = document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImportTime");
                    txtNI.value = '';

                    var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                    tempCheck.checked = false;

                    $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);

                }

                if (strTypeV == 'number') {
                    $("#divValidation").fadeIn();
                    enableCompareValidatorForDataExceedance(true);

                    $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', false);
                    $("#divGraphOptions").fadeIn();

                    // $("#chkFlatLine").fadeIn();

                    $("#trReminders").fadeOut();

                    //                     $("#trRecordCountTable").fadeIn();
                    //                    $("#trRecordCountClick").fadeIn();
                    $("#divValidation").fadeIn();
                    enableCompareValidatorForDataExceedance(true);

                    $("#trGraphOption").fadeIn();
                    var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                    chk.checked = true;
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', false);

                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtGraph");

                    if (txt.value == '') {
                        var txtColumnName = document.getElementById("ctl00_HomeContentPlaceHolder_txtColumnName");
                        txt.value = txtColumnName.value;
                    }


                    $("#divGraphOptions").fadeIn();

                    var chkFlatLine = document.getElementById("chkFlatLine");
                    $("#chkFlatLine").fadeIn();
                    chkFlatLine.checked = false;
                    $("#lblCheckForFlat").fadeIn();
                    // $("#lblCheckForFlat").fadeIn();
                    // $("#lblFlatlineNumber").fadeIn();                    
                    //$("#txtFlatLine").fadeIn();

                    $("#trFlatLine").fadeIn();
                    $('#ctl00_HomeContentPlaceHolder_ddlNumberType').fadeIn();
                    $('#ctl00_HomeContentPlaceHolder_lblNumberType').fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeIn();
                    $("#trIgnoreSymbols").fadeIn();

                    $("#trRound").fadeIn();

                    $("#trCheckUnlikelyValue").fadeIn();

                    $("#trTextDimension").fadeIn();
                    $('#tdHeightLabel').fadeOut();
                    $('#tdHeight').fadeOut();

                }

                if (strTypeV == 'dropdown') {
                    document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?formula=' + encodeURIComponent(document.getElementById('hfDisplayColumnsFormula').value) + '&Tableid=' + $('#ddlDDTable').val();

                    $('#trDDType').fadeIn();
                    $('#trDDValues').fadeIn();
                    $("#trReminders").fadeOut();

                    $("#trTextDimension").fadeIn();
                    $('#tdHeightLabel').fadeOut();
                    $('#tdHeight').fadeOut();
                }


                if (strTypeV == 'radiobutton') {
                    $('#trDDValues').fadeIn();
                    $('#trOptionType').fadeIn();
                }


                if (strTypeV == 'listbox') {
                    $('#trDDValues').fadeIn();
                    $("#trDefaultValue").fadeOut();
                    $('#trListboxType').fadeIn();
                    $('#tdListCheckBox').fadeIn();
                    $("#trTextDimension").fadeIn();

                    var txtTextHeight = document.getElementById("ctl00_HomeContentPlaceHolder_txtTextHeight");
                    if (txtTextHeight.value != '') {
                        if (txtTextHeight.value < 7) {
                            txtTextHeight.value = '7';
                        }
                    }

                }
                else {
                    $('#tdListCheckBox').fadeOut();

                    var txtTextHeight = document.getElementById("ctl00_HomeContentPlaceHolder_txtTextHeight");

                    txtTextHeight.value = '1';


                }

                var hlResetCalculations = document.getElementById("hlResetCalculations");
                if (strTypeV == 'calculation') {

                    if (hlResetCalculations != null && document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value != -1) {
                        hlResetCalculations.style.display = 'block';
                    }

                    $("#trCalculationType").fadeIn();

                    ShowHideCalControl();
                    //calculated

                    $("#trDefaultValue").fadeOut();
                    //$("#trDateCalulationType").fadeIn();
                    $("#divValidation").fadeIn();
                    enableCompareValidatorForDataExceedance(true);

                    $("#trCalculation").fadeIn();

                    var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkMandatory");
                    tempCheck.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();

                    $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeIn();

                    // $("#ctl00_HomeContentPlaceHolder_lblIgnoreSymbols").fadeOut();
                    $("#trIgnoreSymbols").fadeOut();

                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").fadeOut();

                   
                   

                    $("#trGraphOption").fadeIn();
                    $("#divGraphOptions").fadeIn();

                    //$("#divValidationEntry").fadeOut();
                    //$("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeOut();
                    //var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinValid");
                    //txt.value = '';

                    //txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxValid");
                    //txt.value = '';

                    // txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationEntry");
                    // txt.value = '';


                    $("#divValidationWarning").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeIn();

                    var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                    tempCheck.checked = false;

                    $("#trColumnToAvg").fadeOut();
                    $("#trAvgNumValues").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_txtAvgNumValues").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_ddlAvgColumn").fadeOut();


                    //                    $("#ctl00_HomeContentPlaceHolder_lblCheckUnlikelyValue").fadeIn();

                }
                else {

                    if (hlResetCalculations != null) {
                        hlResetCalculations.style.display = 'none';
                    }

                    $("#trCalculation").fadeOut();
                    $("#trCalculationType").fadeOut();
                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                    txt.value = '';
                    $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                    txt.value = '';
                }


                if (strTypeV == 'text') {
                    $("#trTextDimension").fadeIn();
                    $('#tdHeightLabel').fadeIn();
                    $('#tdHeight').fadeIn();
                    $('#trTextType').fadeIn();
                    $("#trReminders").fadeOut();
                }

                var strDateTimeTypeV = $('#ddlDateTimeType').val();
                if (strTypeV == 'date_time') {
                    $("#trFlatLine").fadeOut();
                    $("#trDateTimeType").fadeIn();
                    // $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default to Today');
                    $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default');
                    $('#ddlDefaultValue option[value="value"]').text('To Today');
                    if (strDateTimeTypeV == 'datetime' || strDateTimeTypeV == 'date') {
                        $("#trReminders").fadeIn();
                    }
                    else {
                        $("#trReminders").fadeOut();
                    }
                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                }
                else {
                    $("#trDateTimeType").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").text('Default Value');
                    $('#ddlDefaultValue option[value="value"]').text('Value');
                }



                if (strTypeV == 'checkbox') {
                    $("#trCheckbox1").fadeIn();
                    $("#trCheckbox2").fadeIn();
                    $("#trCheckbox3").fadeIn();
                    $("#trDefaultValue").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    $("#trReminders").fadeOut();
                }

                if (strTypeV == 'staticcontent' || strTypeV == 'button') {
                    $("#trDefaultValue").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    $("#trReminders").fadeOut();
                    $("#trHelpText").fadeOut();
                    $("#trSummaryPage1").fadeOut();
                    //var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkSummaryPage");
                    //chk.checked = false;
                    //$("#ctl00_HomeContentPlaceHolder_lblSummaryPage").fadeOut();
                    //$("#ctl00_HomeContentPlaceHolder_txtDisplayTextSummary").fadeOut();
                    ////                    $("#ctl00_HomeContentPlaceHolder_lblAlignment").fadeOut();
                    ////                    $("#ctl00_HomeContentPlaceHolder_ddlAlignment").fadeOut();
                    ////                    $("#tblCellBackColor").fadeOut();
                    //$("#trSummaryPage2").fadeOut();
                    //$("#trSummaryPage3").fadeOut();
                    //$("#ctl00_HomeContentPlaceHolder_chkSummaryPage").attr('disabled', true);

                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkGraph");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtGraph").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkGraph").attr('disabled', true);

                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").attr('disabled', true);


                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkExport");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblExport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnExport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkExport").attr('disabled', true);

                    chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkMobile");
                    chk.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblMobile").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtMobile").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMobile").attr('disabled', true);


                    if (strTypeV == 'staticcontent') {
                        $("#trStaticContent").fadeIn();
                        $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeOut();
                        $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeOut();
                        $("#trButton1").fadeOut();
                        $("#trButton2").fadeOut();
                        $("#trButton3").fadeOut();
                        $("#trButton4").fadeOut();
                    }
                    else {
                        $("#trStaticContent").fadeOut();
                        $("#ctl00_HomeContentPlaceHolder_lblDetailPage").fadeIn();
                        $("#ctl00_HomeContentPlaceHolder_txtDisplayTextDetail").fadeIn();
                        $("#trButton1").fadeIn();
                        $("#trButton2").fadeIn();
                        $("#trButton3").fadeIn();
                        $("#trButton4").fadeIn();
                        // $("#trTextDimension").fadeIn();
                    }

                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    $("#tblColumnColour").fadeOut();
                    $("#divGraphOptions").fadeOut();
                    $("#divchkCompareOperator").fadeOut();
                    $("#divchkValidationCanIgnore").fadeOut();
                }
                else {
                    $("#trSummaryPage1").fadeIn();
                    $("#trButton1").fadeOut();
                    $("#trButton2").fadeOut();
                    $("#trButton3").fadeOut();
                    $("#trButton4").fadeOut();

                    $("#tblColumnColour").fadeIn();
                    $("#divchkCompareOperator").fadeIn();
                    $("#divchkValidationCanIgnore").fadeIn();
                }

            });

            $('#ddlDateTimeType').change(function (e) {
                var strDateTimeTypeV = $('#ddlDateTimeType').val();
                if (strDateTimeTypeV == 'datetime' || strDateTimeTypeV == 'date') {
                    $("#trReminders").fadeIn();
                }
                else {
                    $("#trReminders").fadeOut();
                }

            });

            $('#ddlDefaultValue').change(function (e) {
                ShowHideDefaultControl();
            });


            $('#ctl00_HomeContentPlaceHolder_ddlNumberType').change(function (e) {
                var strNumberTypeV = $('#ctl00_HomeContentPlaceHolder_ddlNumberType').val();

                $("#trSlider").fadeOut();


                if (strNumberTypeV == "1" || strNumberTypeV == "6") {
                    //normal
                    $("#trFlatLine").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").fadeIn();

                    $("#divValidationEntry").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeIn();

                    $("#divValidationWarning").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeIn();

                    $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeIn();
                    $("#trIgnoreSymbols").fadeIn();
                    //                    $("#ctl00_HomeContentPlaceHolder_lblIgnoreSymbols").fadeIn();

                    $("#trRound").fadeIn();
                    //                    $("#ctl00_HomeContentPlaceHolder_chkRound").fadeIn();
                    $("#trCheckUnlikelyValue").fadeIn();
                    //                    $("#ctl00_HomeContentPlaceHolder_lblCheckUnlikelyValue").fadeIn();

                    $("#trColumnToAvg").fadeOut();
                    $("#trAvgNumValues").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_txtAvgNumValues").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_ddlAvgColumn").fadeOut();

                    $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                    txt.value = '';

                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();

                    $("#trDefaultValue").fadeIn();
                    //                    $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").fadeIn();
                    //                    var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDefaultValue");
                    //                    chk.checked = false;
                    ShowHideDefaultControl();
                }
                else {

                    $("#trDefaultValue").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_lblDefauleValue").fadeOut();
                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                    txtDefaultValue.value = '';
                    //                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                }

                if (strNumberTypeV == "2") {
                    //constant
                    $("#trFlatLine").fadeOut();
                    $("#trCheckUnlikelyValue").fadeOut();

                    $("#divGraphOptions").fadeOut();

                    $("#divchkCompareOperator").fadeOut();
                    $("#divchkValidationCanIgnore").fadeOut();
                    $("#divValidationRoot").fadeOut();


                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").fadeOut();
                    var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                    tempCheck.checked = false;


                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkMandatory");
                    tempCheck.checked = false;

                    $("#trColumnToAvg").fadeOut();
                    $("#trAvgNumValues").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_lblColumnToAvg").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_ddlAvgColumn").fadeOut();

                    $("#divValidationEntry").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeOut();

                    $("#divValidationWarning").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeOut();

                    $("#divValidation").fadeOut();
                    enableCompareValidatorForDataExceedance(false);
                    clearMinMaxExceedanceValue();

                    $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                    txt.value = '';
                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinValid");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxValid");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationEntry");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinWaring");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxWrning");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnWarning");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinExceedance");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxExceedance");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnExceedance");
                    txt.value = '';


                    $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeOut();

                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkShowTotal");
                    tempCheck.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeOut();

                    //                    $("#ctl00_HomeContentPlaceHolder_chkIgnoreSymbols").fadeOut();
                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkIgnoreSymbols");
                    tempCheck.checked = false;
                    $("#trIgnoreSymbols").fadeOut();

                    //                    $("#ctl00_HomeContentPlaceHolder_lblRound").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_chkRound").fadeOut();
                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkRound");
                    tempCheck.checked = false;
                    //                    $("#ctl00_HomeContentPlaceHolder_txtRoundNumber").fadeOut();
                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtRoundNumber");
                    txt.value = '';
                    $("#trRound").fadeOut();


                    //                    $("#ctl00_HomeContentPlaceHolder_chkCheckUnlikelyValue").fadeOut();
                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkCheckUnlikelyValue");
                    tempCheck.checked = false;
                    $("#trCheckUnlikelyValue").fadeOut();


                }
                else {

                    $("#divGraphOptions").fadeIn();

                    $("#divchkCompareOperator").fadeIn();
                    $("#divchkValidationCanIgnore").fadeIn();
                    $("#divValidationRoot").fadeIn();

                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();

                    $("#divValidation").fadeIn();
                    enableCompareValidatorForDataExceedance(true);

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtConstant");
                    //txt.value = '';
                }




                if (strNumberTypeV == "4") {
                    //Avg
                    $("#trFlatLine").fadeIn();
                    $("#trColumnToAvg").fadeIn();
                    $("#trAvgNumValues").fadeIn();
                    //                    $("#ctl00_HomeContentPlaceHolder_lblColumnToAvg").fadeIn();
                    //                    $("#ctl00_HomeContentPlaceHolder_ddlAvgColumn").fadeIn();

                    //                    $("#ctl00_HomeContentPlaceHolder_lblIgnoreSymbols").fadeOut();
                    $("#trIgnoreSymbols").fadeOut();

                    $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeIn();

                    $("#trRound").fadeIn();
                    //                    $("#ctl00_HomeContentPlaceHolder_chkRound").fadeIn();
                    $("#trCheckUnlikelyValue").fadeIn();
                    //                    $("#ctl00_HomeContentPlaceHolder_lblCheckUnlikelyValue").fadeIn();

                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").fadeOut();
                    var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                    tempCheck.checked = false;

                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                    txt.value = '';
                    $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();

                    $("#divValidationEntry").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeOut();

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinValid");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxValid");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationEntry");
                    txt.value = '';

                    $("#divValidationWarning").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeIn();

                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeIn();

                }

                //Finance
                if (strNumberTypeV == "6") {

                    $("#ctl00_HomeContentPlaceHolder_lblSymbol").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_txtSymbol").fadeIn();
                    $("#trCheckUnlikelyValue").fadeOut();
                    $("#trFlatLine").fadeOut();

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_lblSymbol").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtSymbol").fadeOut();
                    $("#trCheckUnlikelyValue").fadeIn();
                    $("#trFlatLine").fadeIn();
                }


                if (strNumberTypeV == "5") {
                    //record count
                    $("#trRecordCountTable").fadeIn();
                    $("#trRecordCountClick").fadeIn();
                    $("#trFlatLine").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtNameOnImport").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").fadeOut();
                    var tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkImport");
                    tempCheck.checked = false;


                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();
                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkMandatory");
                    tempCheck.checked = false;

                    $("#trColumnToAvg").fadeOut();
                    $("#trAvgNumValues").fadeOut();

                    $("#divValidationEntry").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeOut();

                    $("#divValidationWarning").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeOut();

                    $("#divValidation").fadeOut();
                    enableCompareValidatorForDataExceedance(false);
                    clearMinMaxExceedanceValue();

                    $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                    txt.value = '';
                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinValid");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxValid");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationEntry");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinWaring");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxWrning");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnWarning");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMinExceedance");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtMaxExceedance");
                    txt.value = '';

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnExceedance");
                    txt.value = '';


                    $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeOut();

                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkShowTotal");
                    tempCheck.checked = false;
                    $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeOut();

                    //                    $("#ctl00_HomeContentPlaceHolder_chkIgnoreSymbols").fadeOut();
                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkIgnoreSymbols");
                    tempCheck.checked = false;
                    $("#trIgnoreSymbols").fadeOut();

                    //                    $("#ctl00_HomeContentPlaceHolder_lblRound").fadeOut();
                    //                    $("#ctl00_HomeContentPlaceHolder_chkRound").fadeOut();
                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkRound");
                    tempCheck.checked = false;
                    //                    $("#ctl00_HomeContentPlaceHolder_txtRoundNumber").fadeOut();
                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtRoundNumber");
                    txt.value = '';
                    $("#trRound").fadeOut();


                    //                    $("#ctl00_HomeContentPlaceHolder_chkCheckUnlikelyValue").fadeOut();
                    tempCheck = document.getElementById("ctl00_HomeContentPlaceHolder_chkCheckUnlikelyValue");
                    tempCheck.checked = false;
                    $("#trCheckUnlikelyValue").fadeOut();


                }
                else {
                    $("#trRecordCountTable").fadeOut();
                    $("#trRecordCountClick").fadeOut();

                    $("#divValidation").fadeIn();
                    enableCompareValidatorForDataExceedance(true);

                    txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtConstant");
                    //txt.value = '';
                }

                if (strNumberTypeV == "7") {
                    $("#trSlider").fadeIn();
                    $("#trFlatLine").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").fadeIn();

                    $("#divValidationEntry").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeIn();

                    $("#divValidationWarning").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeIn();
                    $("#trIgnoreSymbols").fadeOut();
                    $("#trRound").fadeOut();
                    $("#trCheckUnlikelyValue").fadeOut();
                    $("#trColumnToAvg").fadeOut();
                    $("#trAvgNumValues").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                    txt.value = '';
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();

                    $("#trDefaultValue").fadeIn();
                    //                    var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDefaultValue");
                    //                    chk.checked = true;
                    //                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeIn();
                    //                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                    //                    txtDefaultValue.value = '50';

                    $('#ddlDefaultValue').val = 'value';
                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOIn();
                    $("#tdDefaultParent").fadeOut();
                    $("#tdDefaultSyncData").fadeOut();
                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                    txtDefaultValue.value = '50';
                }
                else {
                    $("#trSlider").fadeOut();
                }
                var hlResetIDs = document.getElementById("hlResetIDs");

                if (strNumberTypeV == "8") {
                    if (hlResetIDs != null && document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value != -1) {
                        hlResetIDs.style.display = 'block';
                    }
                    $("#trGraphOption").fadeOut();
                    $("#divGraphOptions").fadeOut();
                    $("#trSlider").fadeOut();
                    $("#trFlatLine").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtConstant").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblImportMain").fadeIn();
                    $("#ctl00_HomeContentPlaceHolder_chkImport").fadeIn();

                    $("#divValidation").fadeOut();
                    enableCompareValidatorForDataExceedance(false);
                    clearMinMaxExceedanceValue();

                    //$("#divValidationEntry").fadeOut();
                    //$("#ctl00_HomeContentPlaceHolder_lblValidationEntry").fadeOut();

                    // $("#divValidationWarning").fadeOut();
                    // $("#ctl00_HomeContentPlaceHolder_lblWarningValidation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkShowTotal").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblShowTotal").fadeOut();
                    $("#trIgnoreSymbols").fadeOut();
                    $("#trRound").fadeOut();
                    $("#trCheckUnlikelyValue").fadeOut();
                    $("#trColumnToAvg").fadeOut();
                    $("#trAvgNumValues").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_txtCalculation").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_hlCalculationEdit").fadeOut();
                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
                    txt.value = '';
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();

                    $("#trDefaultValue").fadeOut();
                    //                    var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDefaultValue");
                    //                    chk.checked = false;
                    //                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
                    //                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                    //                    txtDefaultValue.value = '';
                    ShowHideDefaultControl();
                    var txtDefaultValue = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
                    txtDefaultValue.value = '';
                }
                else {
                    if (hlResetIDs != null) {
                        hlResetIDs.style.display = 'none';
                    }
                }
                if (strNumberTypeV == "2") {
                    $("#trFlatLine").fadeOut();
                    $("#trCheckUnlikelyValue").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_chkMandatory").fadeOut();
                    $("#ctl00_HomeContentPlaceHolder_lblMandatory").fadeOut();

                    $("#trRound").fadeOut();
                    $("#trIgnoreSymbols").fadeOut();


                }



            });



            $("#ctl00_HomeContentPlaceHolder_chkRound").click(function () {
                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkRound");
                if (chk.checked == true) {
                    $("#ctl00_HomeContentPlaceHolder_txtRoundNumber").fadeIn();
                    //$("#ctl00_HomeContentPlaceHolder_lblRoundNumber").fadeIn();
                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtRoundNumber");
                    txt.value = '2';

                }
                else {
                    $("#ctl00_HomeContentPlaceHolder_txtRoundNumber").fadeOut();
                    //$("#ctl00_HomeContentPlaceHolder_lblRoundNumber").fadeOut();

                }
            });


            //            $("#ctl00_HomeContentPlaceHolder_chkDropdownValues").click(function () {
            //                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDropdownValues");
            //                if (chk.checked == true) {
            //                    $("#ctl00_HomeContentPlaceHolder_txtDropdownValues").fadeIn();
            //                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesText").fadeIn();
            //                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDropdownValues");
            //                    txt.value = '';

            //                }
            //                else {
            //                    $("#ctl00_HomeContentPlaceHolder_txtDropdownValues").fadeOut();
            //                    $("#ctl00_HomeContentPlaceHolder_lblDropdownValuesText").fadeOut();
            //                    var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDropdownValues");
            //                    txt.value = '';
            //                }
            //            });


            //            $("#chkReminders").click(function () {
            //                var chk = document.getElementById("chkReminders");
            //                if (chk.checked == true) {
            ////                    $("#lblReminders").fadeOut();
            //                    $("#hlReminders").fadeIn();
            //                    $("#hlReminders").trigger("click");

            //                }
            //                else {
            //                    $("#hlReminders").fadeOut();
            ////                    $("#lblReminders").fadeIn();
            //                }
            //            });





            //            $("#ctl00_HomeContentPlaceHolder_chkDefaultValue").click(function () {
            //                var chk = document.getElementById("ctl00_HomeContentPlaceHolder_chkDefaultValue");
            //                var strTypeV = $('#ctl00_HomeContentPlaceHolder_ddlType').val();
            //                if (strTypeV == 'date_time') {
            //                    $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
            //                }
            //                else {
            //                    if (chk.checked == true) {
            //                        $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeIn();
            //                        var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
            //                        txt.value = '';

            //                    }
            //                    else {
            //                        $("#ctl00_HomeContentPlaceHolder_txtDefaultValue").fadeOut();
            //                        var txt = document.getElementById("ctl00_HomeContentPlaceHolder_txtDefaultValue");
            //                        txt.value = '';
            //                    }
            //                }
            //            });

            $("#ctl00_HomeContentPlaceHolder_optSingle").click(function () {
                var optSingle = document.getElementById("ctl00_HomeContentPlaceHolder_optSingle");
                var hfIsIP = document.getElementById("ctl00_HomeContentPlaceHolder_hfIsImportPositional");
                if (optSingle.checked == true) {
                    $("#trTimeSection").fadeOut();
                    if (hfIsIP.value == "0") {
                        $("#ctl00_HomeContentPlaceHolder_lblImport").text("Label");
                        document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport").value = "Date Time Recorded";

                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_lblImport").text("Position");

                    }
                }

            });

            $("#ctl00_HomeContentPlaceHolder_optDouble").click(function () {
                var optDouble = document.getElementById("ctl00_HomeContentPlaceHolder_optDouble");
                var hfIsIP = document.getElementById("ctl00_HomeContentPlaceHolder_hfIsImportPositional");
                if (optDouble.checked == true) {
                    $("#trTimeSection").fadeIn();
                    if (hfIsIP.value == "0") {
                        $("#ctl00_HomeContentPlaceHolder_lblImport").text("Date Label");

                        document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImport").value = "Date Recorded";
                        document.getElementById("ctl00_HomeContentPlaceHolder_txtNameOnImportTime").value = "Time Recorded";

                    }
                    else {
                        $("#ctl00_HomeContentPlaceHolder_lblImport").text("Date Position");


                    }
                }
            });


            $("#chkFlatLine").click(function () {
                var chk = document.getElementById("chkFlatLine");
                var txt = document.getElementById("txtFlatLine");
                if (chk.checked == true) {
                    $("#lblFlatlineNumber").fadeIn();
                    $("#txtFlatLine").fadeIn();
                    txt.value = '';
                }
                else {
                    $("#lblFlatlineNumber").fadeOut();
                    $("#txtFlatLine").fadeOut();
                    txt.value = '';
                }
            });

            $("#chkShowMap").click(function () {
                var chk = document.getElementById("chkShowMap");
                if (chk.checked == true) {
                    $("#tblMapDimension").fadeIn();
                }
                else {
                    $("#tblMapDimension").fadeOut();
                }
            });
        });

        $(document).ready(function () {

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
            setTimeout(function () { ShowHideDCEdit(); }, 2000);

        });


    </script>
    <script type="text/javascript">

        function OpenValidPopup() {
            var txtValid = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationEntry");
            var left = (screen.width / 2) - (650 / 2);
            var top = (screen.height / 2) - (600 / 2);


            window.open("../Help/FormulaTest.aspx?type=valid&formula=" + encodeURIComponent(txtValid.value), "List", "scrollbars=yes,resizable=yes,width=650,height=600,top=" + top + ",left=" + left);
            return false;
        }

        function OpenWarningPopup() {
            var txtValid = document.getElementById("ctl00_HomeContentPlaceHolder_txtValidationOnWarning");
            var left = (screen.width / 2) - (650 / 2);
            var top = (screen.height / 2) - (600 / 2);
            window.open("../Help/FormulaTest.aspx?type=warning&formula=" + encodeURIComponent(txtValid.value), "List", "scrollbars=yes,resizable=yes,width=650,height=600,top=" + top + ",left=" + left);
            return false;
        }

        function OpenCalculationPopup() {
            var txtValid = document.getElementById("ctl00_HomeContentPlaceHolder_txtCalculation");
            //var hfColumnID = document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID");
            var hfTableID = document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID");

            var left = (screen.width / 2) - (800 / 2);
            var top = (screen.height / 2) - (650 / 2);

            window.open("../Help/CalculationTest.aspx?type=calculation&formula=" + encodeURIComponent(txtValid.value) + "&Tableid=" + hfTableID.value + "&Columnid=" + hfColumnID.value, "List", "scrollbars=yes,resizable=yes,width=800,height=650,top=" + top + ",left=" + left);
            return false;
        }

        function ReadOnlyTextBox(event) {
            window.event.returnValue = false;
        }

        //        function ShowHide() {
        //            var strDDDC = $('#ddDDDisplayColumn').val();
        //            if (strDDDC == '') {
        //                $('#hlDDEdit').fadeIn();
        //            }
        //            else {
        //                $('#hlDDEdit').fadeOut();
        //            }
        //        }
    </script>
    <asp:Panel ID="Panel2" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="928" align="center" onload="ShowHide();">
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
                                <div runat="server" id="divValid" visible="false">
                                    <asp:Label runat="server" ID="lblValidInfo" Text="" ForeColor="Red"></asp:Label>
                                    <table>
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
                                </div>
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
                <td valign="top">
                </td>
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
                                            <td style="width:300px;">
                                                <asp:TextBox runat="server" ID="txtTable" Enabled="false" CssClass="NormalTextBox"
                                                    Width="250px"></asp:TextBox>
                                            </td>
                                            <td style="padding-left:50px;">
                                                <asp:CheckBox runat="server"  ID="chkSummarySearch" Text="Search Field" Font-Bold="true"
                                                    TextAlign="Right" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong runat="server" id="stgFieldNameCap">Field Name*</strong>
                                </td>
                                <td colspan="6">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:300px;">
                                                <asp:TextBox ID="txtColumnName" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                              
                                            </td>
                                            <td style="padding-left: 50px;">
                                                &nbsp;<strong>Visible To</strong>&nbsp;<asp:DropDownList runat="server" ID="ddlOnlyForAdmin" CssClass="NormalTextBox">
                                                    <asp:ListItem Value="0" Text="All Users" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Admin Only"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Own Data Only"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                        <asp:Label runat="server" ID="lblSystemName" Visible="false" style="color:#C0C0C0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="6">
                                  
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                                <td colspan="6">
                                    <asp:Label runat="server" ID="lblColumnMessage" Text="Note this is the field used as the date on the graphs"
                                        Visible="false"></asp:Label>
                                          <asp:RequiredFieldValidator ID="rfvColumnName" runat="server" ControlToValidate="txtColumnName"
                                                    ErrorMessage="Field Name - Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                            </tr>
                         
                            <tr>
                                <td colspan="8" style="height: 10px;">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <div style="border-style: solid; border-width: 1px; width: 500px; min-height: 140px;
                                                    padding: 5px;">
                                                    <table>
                                                        <tr id="trSummaryPage1">
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkSummaryPage" runat="server" ToolTip="Available for selection on views" />
                                                            </td>
                                                            <td align="left" colspan="2">
                                                                <%--<strong>Show on All Views</strong>--%>
                                                                <strong>Views</strong>
                                                                
                                                            </td>
                                                            <td align="right">
                                                               <asp:Label runat="server" ID="lblSummaryPage" Text="Heading" Font-Bold="true" ToolTip="Available for selection on views"></asp:Label>

                                                            </td>
                                                            <td>
                                                                <asp:TextBox ToolTip="Available for selection on views" runat="server" ID="txtDisplayTextSummary" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>
                                                        <tr id="trSummaryPage2" style="display:none;">
                                                            <td align="right">
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
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
                                                                            <table id="tblCellBackColor">
                                                                                <tr>
                                                                                    <td>
                                                                                      
                                                                                    </td>
                                                                                    <td>
                                                                                        <%--<select runat="server" clientidmode="Static" style="width: 80px" 
                                                                                            id="ddlSumXCellBackColour"
                                                                                            class="NormalTextBox">
                                                                                            <option value=""></option>
                                                                                            <option style="color: Aqua;" value="aqua">Aqua</option>
                                                                                            <option style="color: Black;" value="black">Black</option>
                                                                                            <option style="color: Blue;" value="blue">Blue</option>
                                                                                            <option style="color: Fuchsia;" value="fuchsia">Fuchsia</option>
                                                                                            <option style="color: Gray;" value="gray">Gray</option>
                                                                                            <option style="color: Green;" value="green">Green</option>
                                                                                            <option style="color: Lime;" value="lime">Lime</option>
                                                                                            <option style="color: Maroon;" value="maroon">Maroon</option>
                                                                                            <option style="color: Navy;" value="navy">Navy</option>
                                                                                            <option style="color: Olive;" value="olive">Olive</option>
                                                                                            <option style="color: Orange;" value="orange">Orange</option>
                                                                                            <option style="color: Purple;" value="purple">Purple</option>
                                                                                            <option style="color: Red;" value="red">Red</option>
                                                                                            <option style="color: Silver;" value="silver">Silver</option>
                                                                                            <option style="color: Teal;" value="teal">Teal</option>
                                                                                            <option style="color: Black;" value="white">White</option>
                                                                                            <option style="color: Yellow;" value="yellow">Teal</option>
                                                                                        </select>--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>

                                                         <tr id="trSummaryPage3" style="display:none;">
                                                            <td align="right">
                                                                
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblViewName" Text="View Name" Font-Bold="true" Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtViewName" CssClass="NormalTextBox" Width="250px" Visible="false"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>
                                                        <tr id="trDetailPage1">
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkDetailPage" runat="server" />
                                                            </td>
                                                            <td colspan="3">
                                                                <strong>Detail</strong>
                                                                <asp:Label runat="server" ID="lblDetailPage" Text="Label" Font-Bold="true" Visible="false"></asp:Label>
                                                            </td>
                             
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtDisplayTextDetail" Height="50px" 
                                                                CssClass="NormalTextBox" Width="250px" TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>
                                                        <tr id="trDetailPage2">
                                                            <td align="right">
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td align="right" style="width: 100px;">
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:CheckBox runat="server" ID="chkDisplayOnRight" Text="" TextAlign="Right" Font-Bold="true"
                                                                    ClientIDMode="Static" />
                                                                <asp:Label runat="server" ID="lblDisplayOnRight" Text="Display on the right" Font-Bold="true"
                                                                    ClientIDMode="Static"></asp:Label>
                                                                
                                                                <asp:CheckBox runat="server" ID="chkShowWhen" Text="" TextAlign="Right" Font-Bold="true"
                                                                    ClientIDMode="Static" />
                                                                <asp:HyperLink runat="server" NavigateUrl="~/Pages/Record/ShowHide.aspx" CssClass="showlink"
                                                                    ID="hlShowWhen" ClientIDMode="Static">Show When...</asp:HyperLink>
                                                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfShowHref" />  <br />
                                                                <div runat="server" id="divTableTab">
                                                                 <strong> Page:</strong>
                                                               <asp:DropDownList runat="server" ID="ddlTableTab"  DataTextField="TabName" DataValueField="TableTabID"
                                                               CssClass="NormalTextBox"></asp:DropDownList>
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
                                                            <td>
                                                            </td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblGraph" Text="Label" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGraph" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
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
                                                                        <td style="width: 97px;">
                                                                        </td>
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
                                                        <tr id="trExportOption" runat="server" clientidmode="Static" >
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkExport" runat="server" />
                                                            </td>
                                                            <td>
                                                                <strong>Export</strong>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblExport" Text="Heading" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtNameOnExport" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="7"><!-- spacer to make it consistent --></td>
                                                        </tr>
                                                        <tr id="trMobileSiteSummary">
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkMobile" runat="server" />
                                                            </td>
                                                            <td>
                                                                <strong>Mobile</strong>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td align="right" style="width: 100px;">
                                                                <asp:Label runat="server" ID="lblMobile" Text="Heading" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtMobile" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div id="divchkCompareOperator">
                                                      <asp:CheckBox runat="server" ID="chkCompareOperator" ClientIDMode="Static" Text="Compare Values" Font-Bold="true" TextAlign="Right" />
                                                </div>
                                              
                                                
                                                <br />
                                                <div style="border-style: solid; border-width: 1px; width: 500px; padding: 5px; display:none; margin-top:5px;"
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
                                                                 <asp:RequiredFieldValidator ID="rfvCompareOperator" runat="server" ControlToValidate="ddlCompareOperator"
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
                                                                 <asp:RequiredFieldValidator ID="rfvCompareTable" runat="server" ControlToValidate="ddlCompareTable"
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

                                                                <asp:DropDownList runat="server" ID="ddlCompareColumnID" CssClass="NormalTextBox"  ClientIDMode="Static">
                                                                </asp:DropDownList>

                                                                 <ajaxToolkit:CascadingDropDown runat="server" ID="ddlCompareColumnIDC" Category="Column"
                                                                    ClientIDMode="Static" TargetControlID="ddlCompareColumnID" ServicePath="~/CascadeDropdown.asmx"
                                                                    ServiceMethod="GetDefaultParentColumns" ParentControlID="ddlCompareTable">
                                                                </ajaxToolkit:CascadingDropDown>
                                                            </td>
                                                             <td>
                                                                 <asp:RequiredFieldValidator ID="rfvCompareColumnID" runat="server" ControlToValidate="ddlCompareColumnID"
                                                    ErrorMessage="Required" ClientIDMode="Static"></asp:RequiredFieldValidator>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div id="divchkValidationCanIgnore">
                                                     <asp:CheckBox runat="server" ID="chkValidationCanIgnore" 
                                                                                        Text="Allow user to ignore invalid data" Font-Bold="true" />

                                                </div>
                                                <br />
                                                <div id="divValidationRoot">
                                                    <div style="border-style: solid; border-width: 1px; width: 500px; min-height: 140px; padding: 5px;"
                                                        id="divValidation">
                                                        <table>
                                                            <tr>
                                                                <td colspan="7">
                                                                    <asp:Label runat="server" ID="lblValidationEntry" Text="Data invalid if outside the range" Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" valign="top" style="width: 40px;"></td>
                                                                <td colspan="6">
                                                                    <div id="divValidationEntry">
                                                                        <table>
                                                                            <tr>
                                                                                <td valign="middle">

                                                                                    <div>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <asp:TextBox runat="server" ID="txtValidationEntry" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                                                        Width="250px" Height="60px"></asp:TextBox>
                                                                                                   
                                                                                                     <div id="divValidAdvanced">
                                                                                                         &nbsp;
                                                                                        <asp:Label runat="server" ID="lblMinValid" Text="Min:" Font-Bold="true"></asp:Label>
                                                                                                         <asp:TextBox runat="server" ID="txtMinValid" CssClass="NormalTextBox" Width="87px"></asp:TextBox>
                                                                                                         &nbsp;
                                                                                        <asp:Label runat="server" ID="lblMaxValid" Text="Max:" Font-Bold="true"></asp:Label>
                                                                                                         <asp:TextBox runat="server" ID="txtMaxValid" CssClass="NormalTextBox" Width="87px"></asp:TextBox>

                                                                                                     </div>

                                                                                                </td>
                                                                                                <td valign="top">

                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:CheckBox runat="server" ID="chkValidFormula" ClientIDMode="Static" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlValidAdvanced">Formula</asp:HyperLink>
                                                                                                                <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlValidEdit">Edit</asp:HyperLink>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr valign="top">
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
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>

                                                                        </table>
                                                                    </div>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="7">
                                                                    <asp:Label runat="server" ID="lblWarningValidation" Text="Data Warning if outside the range"
                                                                        Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" valign="top"></td>
                                                                <td colspan="6">
                                                                    <div id="divValidationWarning">
                                                                        <table>
                                                                            <tr>
                                                                                <td valign="middle">
                                                                                    
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td valign="top" style="padding-top: 5px;">
                                                                                        <asp:TextBox runat="server" ID="txtValidationOnWarning" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                                            Width="250px" Height="60px" Style="display: none;"></asp:TextBox>
                                                                                                    <div id="divWrningAdvanced">
                                                                                                        <asp:Label runat="server" ID="lblMinWarning" Text="Min:" Font-Bold="true"></asp:Label>
                                                                                                        <asp:TextBox runat="server" ID="txtMinWaring" CssClass="NormalTextBox" Width="87px"></asp:TextBox>
                                                                                                        &nbsp;
                                                                                        <asp:Label runat="server" ID="lblMaxWarning" Text="Max:" Font-Bold="true"></asp:Label>
                                                                                                        <asp:TextBox runat="server" ID="txtMaxWrning" CssClass="NormalTextBox" Width="87px"></asp:TextBox>

                                                                                                    </div>


                                                                                                </td>
                                                                                                <td valign="top">
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:CheckBox runat="server" ID="chkWarningFormula" ClientIDMode="Static" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlWarningAdvanced">Formula</asp:HyperLink>
                                                                                                                  <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlWarningEdit"
                                                                                                        Style="display: none; top: 2px;">Edit</asp:HyperLink>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr valign="top">
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
                                                                                            </tr>
                                                                                        </table>


                                                                                    
                                                                                  
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                                <td></td>
                                                            </tr>

                                                            <tr id="trExceedance1">
                                                                <td colspan="7">
                                                                    <asp:Label runat="server" ID="lblExceedanceValidation" Text="Data Exceedance if outside the range"
                                                                        Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr id="trExceedance2">
                                                                <td align="right" valign="top"></td>
                                                                <td colspan="6">
                                                                    <div id="divValidationExceedance">
                                                                        <table>
                                                                            <tr>
                                                                                <td valign="middle">
                                                                                    

                                                                                                    <table>
                                                                                            <tr>
                                                                                                <td valign="top" style="padding-top: 5px;">
                                                                                                         <asp:TextBox runat="server" ID="txtValidationOnExceedance" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                                                        Width="250px" Height="60px" Style="display: none;"></asp:TextBox>
                                                                                                    
                                                                                                    <div id="divExceedanceAdvanced">
                                                                                                        &nbsp;
                                                                                                        <asp:Label runat="server" ID="lblMinExceedance" Text="Min:" Font-Bold="true"></asp:Label>
                                                                                                        <asp:TextBox runat="server" ID="txtMinExceedance" CssClass="NormalTextBox" Width="87px" Text="" ></asp:TextBox>
                                                                                                        &nbsp;
                                                                                                        <asp:Label runat="server" ID="lblMaxExceedance" Text="Max:" Font-Bold="true"></asp:Label>
                                                                                                        <asp:TextBox runat="server" ID="txtMaxExceedance" CssClass="NormalTextBox" Width="87px" Text="" ></asp:TextBox>
                                                                                                            <div style="float:left; margin-left:40px;">
                                                                                                                <asp:CompareValidator ID="cvtxtMinExceedance" runat="server" Operator="DataTypeCheck" Type="Double"
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
                                                                                                                </asp:CompareValidator>
                                                                                                                <asp:CompareValidator runat="server" id="cvExceedanceRange" controltovalidate="txtMinExceedance" 
                                                                                                                    ForeColor="Red"
                                                                                                                    Display="Dynamic" SetFocusOnError="true"
                                                                                                                    controltocompare="txtMaxExceedance" operator="LessThanEqual" type="Double" 
                                                                                                                    errormessage="Min must be less than Max.">
                                                                                                                </asp:CompareValidator>
                                                                                                            </div>
                                                                                                    </div>
                                                                                                </td>
                                                                                                <td valign="top">

                                                                                                    <table>

                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:CheckBox ClientIDMode="Static" runat="server" ID="chkExceedanceFormula" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlExceedanceAdvanced">Formula</asp:HyperLink>
                                                                                                                 <asp:HyperLink runat="server" NavigateUrl="#" CssClass="validationlink" ID="hlExceedanceEdit"
                                                                                                        Style="display: none; top: 2px;">Edit</asp:HyperLink>

                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr valign="top">
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
                                                                                            </tr>
                                                                                        </table>                                                                           
                                                                                    
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                                <td></td>
                                                            </tr>

                                                            <tr id="trCalculation">
                                                                <td align="right" valign="top">&nbsp;
                                                                    <asp:Label runat="server" ID="lblCalculation" Text="Calculation" Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td colspan="6">
                                                                    <table>
                                                                        <tr>
                                                                            <td rowspan="2">
                                                                                <asp:TextBox runat="server" ID="txtCalculation" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                                    Width="250px" Height="60px"></asp:TextBox>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <br />
                                                                                <asp:HyperLink runat="server" NavigateUrl="#" CssClass="calculationlink" ID="hlCalculationEdit">Edit</asp:HyperLink>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                            <td valign="top" style="padding-left: 10px;">
                                                <div style="border-style: solid; border-width: 1px; min-width: 450px; min-height: 140px;
                                                    padding: 5px;" id="divColumnType">
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
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td align="right" colspan="2">
                                                                <asp:HyperLink ID="hlResetCalculations" ClientIDMode="Static" runat="server"
                                                                     CssClass="popupresetCals" Style="display: none;">Reset Calculation Values</asp:HyperLink>
                                                           
                                                            </td>
                                                        </tr>
                                                        <tr id="trCalculationType" >
                                                            <td align="right" style="width:120px;">
                                                                            <strong>Calculation Type</strong>
                                                                        </td>
                                                            <td colspan="5">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                       
                                                                        <td align="left">
                                                                            <asp:DropDownList runat="server" ID="ddlCalculationType"
                                                                             CssClass="NormalTextBox">
                                                                                <asp:ListItem Value="n" Text="Number" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Value="f" Text="Financial" ></asp:ListItem>
                                                                                <asp:ListItem Value="d" Text="Date/Time" ></asp:ListItem>
                                                                                 <asp:ListItem Value="t" Text="Text" ></asp:ListItem>
                                                                             </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <table id="tblFinancialSymbol">
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
                                                                            <table id="tblDateCal">
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
                                                                            <tr id="trTLValueImage"  runat="server" visible="false">
                                                                                <td colspan="5">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <strong>Value</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <strong>Image</strong>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue1" ShowColumnDDL="false"  />
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
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
                                                                                        <tr id="trTLValueLight2"   runat="server" visible="false">
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkMinusTL2"  CausesValidation="false" OnClick="lnkMinusTL2_Click">
                                                                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue2" ShowColumnDDL="false"  />
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlTLImage2" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkAddTL2"  CausesValidation="false" OnClick="lnkAddTL2_Click">
                                                                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTLValueLight3"   runat="server" visible="false">
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkMinusTL3"  CausesValidation="false" OnClick="lnkMinusTL3_Click">
                                                                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue3" ShowColumnDDL="false" />
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlTLImage3" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkAddTL3"  CausesValidation="false" OnClick="lnkAddTL3_Click">
                                                                                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTLValueLight4"   runat="server" visible="false">
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkMinusTL4"  CausesValidation="false" OnClick="lnkMinusTL4_Click">
                                                                                                    <asp:Image ID="Image9" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue4" ShowColumnDDL="false" />
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList runat="server" ID="ddlTLImage4" CssClass="NormalTextBox">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkAddTL4"  CausesValidation="false" OnClick="lnkAddTL4_Click">
                                                                                                    <asp:Image ID="Image10" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trTLValueLight5"   runat="server" visible="false">
                                                                                            <td>
                                                                                                <asp:LinkButton runat="server" ID="lnkMinusTL5"  CausesValidation="false" OnClick="lnkMinusTL5_Click">
                                                                                                    <asp:Image ID="Image11" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                                                                                </asp:LinkButton>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dbg:ControlByColumn runat="server" ID="cbcValue5" ShowColumnDDL="false"  />
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
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
                                                                                Visible="true" ToolbarMode="0"  btnPreview="False" 
                                                                                 btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False"/>
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
                                                                <table style="border-collapse:collapse;border-spacing:0">
                                                                     <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblButtonValue" ClientIDMode="Static" ></asp:Label>
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
                                                        <tr id="trCheckbox1">
                                                            <td align="right" valign="top">
                                                                <strong>Ticked Value</strong>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox runat="server" ID="txtTickedValue" CssClass="NormalTextBox" Text="Yes"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr id="trCheckbox2">
                                                            <td align="right" valign="top">
                                                                <strong>Unticked Value</strong>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox runat="server" ID="txtUntickedValue" CssClass="NormalTextBox" Text="No"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr id="trCheckbox3">
                                                            <td align="right" valign="top">
                                                            </td>
                                                            <td valign="top">
                                                                <asp:CheckBox ID="chkTickedByDefault" runat="server" Text="Ticked by Default" TextAlign="Right"
                                                                    Font-Bold="true" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr id="trLocation">
                                                            <td align="right" valign="top">
                                                            </td>
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
                                                                                        <asp:RangeValidator runat="server" ID="RangeValidator1" ControlToValidate="txtMapWidth"
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
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr id="trDateTimeType">
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
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr id="trTextType">
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
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtOwnRegEx" ClientIDMode="Static" TextMode="MultiLine"
                                                                    CssClass="MultiLineTextBox" Width="150px" Height="50"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
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
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td align="right">
                                                                <asp:HyperLink ID="hlResetIDs" ClientIDMode="Static" runat="server"
                                                                     CssClass="popupresetIDs" Style="display: none;">Reset IDs</asp:HyperLink>
                                                                <asp:Label runat="server" ID="lblConstant" Text="Value" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>

                                                                <asp:TextBox runat="server" ID="txtConstant" TextMode="SingleLine" CssClass="NormalTextBox"
                                                                    Width="150px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtConstant"
                                                                    runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                </asp:RegularExpressionValidator>

                                                                
                                                                <asp:Button runat="server" ID="btnResetIDsOK" ClientIDMode="Static" Style="display: none;" OnClick="btnResetIDsOK_Click" />
                                                                <asp:Button runat="server" ID="btnResetCalValues" ClientIDMode="Static" Style="display: none;" OnClick="btnResetCalValues_Click" />
                                                            </td>
                                                        </tr>
                                                        <%--<tr id="trDateCalculation">
                                                            <td align="right">
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkDateCalculation" Text="Date Calculation" Font-Bold="true" TextAlign="right"
                                                                    runat="server" />
                                                            </td>
                                                            <td colspan="4">
                                                            </td>
                                                        </tr>--%>
                                                        <%--<tr id="trDateCalulationType">
                                                            <td align="right">
                                                                <strong>Results In</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlDateCalculationType" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="year" Text="Years" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="month" Text="Months"></asp:ListItem>
                                                                    <asp:ListItem Value="day" Text="Days"></asp:ListItem>
                                                                    <asp:ListItem Value="hour" Text="Hours"></asp:ListItem>
                                                                    <asp:ListItem Value="minute" Text="Minutes"></asp:ListItem>
                                                                    <asp:ListItem Value="second" Text="Seconds"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td colspan="4">
                                                            </td>
                                                        </tr>--%>
                                                        <tr runat="server" clientidmode="Static" id="trDDType">
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
                                                                                <asp:ListItem Value="ct" Text="Table" ></asp:ListItem>
                                                                                <asp:ListItem Value="lt" Text="Linked" ></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>                                                                                    
                                                                        <td id="tdPredictive">
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="width:100px;">
                                                                                        <%--<asp:CheckBox runat="server" ID="chkFilterValues" Text="Filter Values" ClientIDMode="Static"
                                                                                            TextAlign="Right" />--%>
                                                                                            <asp:CheckBox runat="server" ID="chkPredictive" Text="Predictive" ClientIDMode="Static"
                                                                                            TextAlign="Right" Font-Bold="true" />
                                                                                    </td>
                                                                                    <td style="width:100px;" id="tdTableFilter">
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
                                                        <tr runat="server" clientidmode="Static" id="trOptionType">
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
                                                        <tr runat="server" clientidmode="Static" id="trListboxType">
                                                            <td align="right">
                                                                <strong>Type</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList runat="server" ID="ddlListBoxType" ClientIDMode="Static" CssClass="NormalTextBox">
                                                                    <asp:ListItem Value="values" Text="Values" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="value_text" Text="Value & Text"></asp:ListItem>
                                                                    <asp:ListItem Value="table" Text="Table"></asp:ListItem>
                                                                </asp:DropDownList>
                                                               
                                                            </td>
                                                        </tr>

                                                        <tr runat="server" clientidmode="Static" id="trDDValues">
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
                                                        <tr runat="server" clientidmode="Static" id="trOptionImageGrid">
                                                           <td></td>
                                                            <td>
                                                                <div id="divOptionImageGrid" style="min-width:400px;">
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
                                                                        <asp:TemplateField HeaderText="Image" >
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="" >
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
                                                                            <asp:CheckBox ClientIDMode="Static" runat="server" ID="chkImageOnSummary" Font-Bold="true" TextAlign="Right" Text="Image On Summary"/>
                                                                        </td>
                                                                        <td style="padding-left:20px;">
                                                                            <table id="tblImageOnSummaryMaxHeight" >
                                                                                <tr>
                                                                                    <td align="right">

                                                                                        <strong>Max Height</strong>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox runat="server" ID="txtImageOnSummaryMaxHeight" CssClass="NormalTextBox" Width="50px"   ClientIDMode="Static"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" ControlToValidate="txtImageOnSummaryMaxHeight"
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
                                                         <tr runat="server" clientidmode="Static" id="trDDTableLookup">
                                                             <td></td>
                                                             <td colspan="5">
                                                                 <asp:LinkButton runat="server" ID="lnkCreateLookupTable" Visible="false" CausesValidation="false"
                                                                                OnClick="lnkCreateLookupTable_Click" > <strong>Create Lookup Table</strong> </asp:LinkButton>
                                                             </td>
                                                         </tr>
                                                        <tr runat="server" clientidmode="Static" id="trDDTable">
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
                                                                            <strong  style="display:none;">Field</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddDDLinkedParentColumn" ClientIDMode="Static"
                                                                                CssClass="NormalTextBox" style="display:none;">
                                                                            </asp:DropDownList>
                                                                            <ajaxToolkit:CascadingDropDown runat="server" ID="ddDDLinkedParentColumnC" Category="Column"
                                                                                ClientIDMode="Static" TargetControlID="ddDDLinkedParentColumn" ServicePath="~/CascadeDropdown.asmx"
                                                                                ServiceMethod="GetColumnsLink" ParentControlID="ddlDDTable" >
                                                                            </ajaxToolkit:CascadingDropDown>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trFilter">
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
                                                        <tr runat="server" clientidmode="Static" id="trDDDisplayColumn">
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

                                                                                 <asp:DropDownList runat="server" ID="ddlShowViewLink"  CssClass="NormalTextBox">
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
                                                     
                                                        <tr runat="server" clientidmode="Static" id="trRecordCountTable">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="Label1" Text="Table" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList runat="server" ID="ddlRecordCountTable" CssClass="NormalTextBox"
                                                                    DataValueField="TableID" DataTextField="TableName">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trRecordCountClick">
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
                                                        <tr runat="server" clientidmode="Static" id="trSlider">
                                                            <td align="right">
                                                            </td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <strong>Min</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtSliderMin" CssClass="NormalTextBox" Width="50px" Text="0"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" ControlToValidate="txtSliderMin"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                           
                                                                        <td align="right">
                                                                            <strong>Max</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtSliderMax" CssClass="NormalTextBox" Width="50px" Text="100"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" ControlToValidate="txtSliderMax"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trColumnToAvg">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblColumnToAvg" Text="Field to Avg" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList runat="server" ID="ddlAvgColumn" CssClass="NormalTextBox" DataValueField="ColumnID"
                                                                    DataTextField="DisplayName">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trAvgNumValues">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblAvgNumValues" Text="Avg Num Values" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtAvgNumValues" CssClass="NormalTextBox" Width="50px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                            <td>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtAvgNumValues"
                                                                    runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trRound">
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblSymbol" Text="Symbol" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSymbol" Text="$" CssClass="NormalTextBox" Width="50px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
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
                                                                            <asp:RegularExpressionValidator ID="revHighestValue" ControlToValidate="txtRoundNumber"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>

                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                               
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trTextDimension">
                                                            <td>
                                                            </td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <strong>Width</strong>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtTextWidth" Width="50px" Text="22"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" ControlToValidate="txtTextWidth"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td id="tdHeightLabel">
                                                                            <strong>Height</strong>
                                                                        </td>
                                                                        <td id="tdHeight">
                                                                            <asp:TextBox runat="server" ID="txtTextHeight" Width="50px" Text="1"></asp:TextBox>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" ControlToValidate="txtTextHeight"
                                                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="chkMandatory" CssClass="NormalTextBox" Font-Bold="true" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMandatory" Text="Mandatory" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                        <td></td>
                                                                        <td id="tdListCheckBox">
                                                                              <asp:CheckBox runat="server" ID="chkListCheckBox" CssClass="NormalTextBox" Font-Bold="true" Checked="true" />
                                                                             <asp:Label runat="server" ID="lblListCheckBox" Text="Use Checkboxes" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblSPDefaultValue" ForeColor="Gray" ></asp:Label>
                                                                          
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td align="right">
                                                                <asp:CheckBox runat="server" Font-Bold="true" ID="chkShowTotal" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblShowTotal" Text="Show Field Total" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trIgnoreSymbols">
                                                            <td align="right">
                                                            </td>
                                                            <td valign="top" colspan="2">
                                                            </td>
                                                            <td style="width: 10px;">
                                                            </td>
                                                            <td align="right">
                                                                <asp:CheckBox runat="server" Font-Bold="true" ID="chkIgnoreSymbols" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblIgnoreSymbols" Text="Ignore Symbols" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" clientidmode="Static" id="trCheckUnlikelyValue">
                                                            <td colspan="4">
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox runat="server" Font-Bold="true" ID="chkCheckUnlikelyValue" ToolTip="Outside 3 standard deviations" />
                                                            </td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblCheckUnlikelyValue" Text="Warn of Unlikely Readings"
                                                                    Font-Bold="true" ToolTip="Outside 3 standard deviations"></asp:Label>
                                                            </td>
                                                            
                                                        </tr>
                                                        <%--<tr>
                                                            <td>
                                                            </td>
                                                            <td valign="top">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" Font-Bold="true" ID="chkDropdownValues" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblDropDownValues" Text="Dropdown" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td valign="top" style="padding-top: 5px;">
                                                                <asp:Label runat="server" ID="lblDropdownValuesText" Text="Values" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td style="padding-top: 5px;">
                                                                <asp:TextBox runat="server" ID="txtDropdownValues" CssClass="MultiLineTextBox" TextMode="MultiLine"
                                                                    Width="150px" Height="50px"></asp:TextBox>
                                                            </td>
                                                        </tr>--%>
                                                        <tr runat="server" clientidmode="Static" id="trDefaultValue">
                                                            <td align="right">
                                                            </td>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td style="width:100px;">
                                                                            <asp:Label runat="server" ID="lblDefauleValue" Text="Default Value" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                             <%--<asp:ListItem Text="None" Value="none"></asp:ListItem>
                                                                                <asp:ListItem Text="Value" Value="value"></asp:ListItem>
                                                                                <asp:ListItem Text="From Related Table" Value="parent"></asp:ListItem>--%>

                                                                            <asp:DropDownList runat="server" ID="ddlDefaultValue" ClientIDMode="Static" CssClass="NormalTextBox">                                                                               
                                                                            </asp:DropDownList>
                                                                             <ajaxToolkit:CascadingDropDown runat="server" ID="ddlDefaultValueC" Category="DefaultValue"
                                                                                ClientIDMode="Static" TargetControlID="ddlDefaultValue" ServicePath="~/CascadeDropdown.asmx"
                                                                                ServiceMethod="GetDefaultValueOption" ParentControlID="ddlDDTable">
                                                                            </ajaxToolkit:CascadingDropDown>
                                                                            <asp:HiddenField  runat="server" ID="hf_ddlDefaultValue" ClientIDMode="Static" Value="none"/>

                                                                        </td>
                                                                         <td id="tdDefaultSyncData" align="left">
                                                                            <asp:CheckBox runat="server" ID="chkDefaultSyncData" Text="Sync Data" TextAlign="Right" Font-Bold="true" />
                                                                        </td>
                                                                        <td align="left" style="padding-left: 2px;">
                                                                            <asp:TextBox runat="server" ID="txtDefaultValue" CssClass="NormalTextBox" Width="165px"></asp:TextBox>
                                                                        </td>
                                                                       
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdDefaultParent" colspan="4">
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
                                                        <tr runat="server" clientidmode="Static" id="trReminders">
                                                            <td align="right">
                                                            </td>
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
                                                        <tr >
                                                            <td colspan="4">
                                                                <table id="tblColumnColour">
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
                                                            <td colspan="2">
                                                                <table cellpadding="0" cellspacing="0"  clientidmode="Static" id="trFlatLine" >
                                                                    <tr>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:CheckBox runat="server" ID="chkFlatLine" ClientIDMode="Static" />
                                                                                   
                                                                                        <asp:Label runat="server" ID="lblCheckForFlat" Text="Check for flat line:" ClientIDMode="Static"
                                                                                            Font-Bold="true"></asp:Label>
                                                                                    </td>
                                                                                   
                                                                                </tr>
                                                                                <tr>
                                                                                      <td style="padding-left: 3px;">
                                                                                        <asp:Label runat="server" ID="lblFlatlineNumber" Text="Number of entries" ClientIDMode="Static"></asp:Label>
                                                                                    </td>
                                                                                    <td style="padding-left: 3px;">
                                                                                        <asp:TextBox runat="server" ID="txtFlatLine" CssClass="NormalTextBox" Width="75px"
                                                                                            ClientIDMode="Static"></asp:TextBox>
                                                                                    </td>
                                                                                
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" ControlToValidate="txtFlatLine"
                                                                                runat="server" ErrorMessage="Invalid Flat line number of entries!" Display="Dynamic"
                                                                                ValidationExpression="^[0-9]+$">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>

                                                        
                                                        <tr>
                                                            <td colspan="6">
                                                            </td>
                                                        </tr>
                                                        <tr id="trImageHeightSummary">
                                                            <td align="right">
                                                                <strong>Max Height on Summary</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:TextBox runat="server" ID="txtImageHeightSummary" TextMode="SingleLine" CssClass="NormalTextBox"
                                                                    Width="100px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" ControlToValidate="txtImageHeightSummary"
                                                                    runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr id="trImageHeightDetail">
                                                            <td align="right">
                                                                <strong>Max Height on Detail</strong>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:TextBox runat="server" ID="txtImageHeightDetail" TextMode="SingleLine" CssClass="NormalTextBox"
                                                                    Width="100px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" ControlToValidate="txtImageHeightDetail"
                                                                    runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div id="divGraphOptions"
                                                    style="border-style: solid; border-width: 1px; min-width: 450px; padding: 5px;"">
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
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtWarning"
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
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtExceedence"
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
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" ControlToValidate="txtMaximumValueat"
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
                                                            <td style="padding-left:0.7em;">
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
                                            <td colspan="2" style="height: 10px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                            </td>
                                            <td valign="top" style="padding-left: 10px;">
                                            </td>
                                        </tr>
                                    </table>
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
    </asp:Panel>
</asp:Content>
