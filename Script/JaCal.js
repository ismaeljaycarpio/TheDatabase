

var JaCalTitle = "";
var JaCal;
var JaCalTitleCell;
var JaCalControlCell;
var JaCalDateCell;
var JaCalCurDate = new Date();
var JaCalSelDate = JaCalCurDate.getDate();
var JaCalSelMonth = JaCalCurDate.getMonth();
var JaCalSelYear = JaCalCurDate.getFullYear();
var JCTarget = "";
var MonthName = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');
var ShortMonthName = new Array('jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec');
var CurrentCulture = 'en';

//Browser Test
function isIECompatible() {
    var browserAgent = navigator.userAgent.toLowerCase();
    if (browserAgent.indexOf("compatible") + 1)
        return true;
    else
        return false;
}

//Creates a Table and Adds to the Document
function MakePopup() {

    var JCPop = document.getElementById('JaCalPop');
    if (!JCPop) {
        JCPop = document.createElement("table");
        JCPop.id = 'JaCalPop';
        JCPop.style.position = 'absolute';
        JCPop.style.visibility = 'hidden';
        //JCPop.style.zIndex = '99!important';
        //JCPop.style.zIndex = '99999';
        JCPop.className = 'JaCalContainer';

        var JCRow = JCPop.insertRow(0);
        JaCalTitleCell = JCRow.insertCell(0);
        JaCalTitleCell.className = 'JaCalContainerCell';

        JCRow = JCPop.insertRow(1);
        JaCalControlCell = JCRow.insertCell(0);
        JaCalControlCell.className = 'JaCalContainerCell';

        JCRow = JCPop.insertRow(2);
        JaCalDateCell = JCRow.insertCell(0);
        JaCalDateCell.className = 'JaCalContainerCell';

        AddHeader();
        document.body.appendChild(JCPop);
        GenJaCal();
    }
    JaCal = JCPop;
}

//Adds Header and Controls to the Calendar Container
function AddHeader() {
    JaCalTitleCell.innerHTML = '<table class = "JaCalTitle" width=100% height=10><tr><td style="cursor: default;">' + JaCalTitle + '</td><td Id="Y1" onmouseover = JaCalHilight("Y1"); onmouseout = JaCalResetHighlight("Y1"); onclick=clearControl(); align="center" title="Clear" style="cursor: pointer;">Clear</td><td id="X1" class = "JaCalCloseButton" onmouseover = JaCalHilight("X1"); onmouseout = JaCalResetHighlight("X1"); onclick=HideJaCal(); align="center" title="Close" style="cursor: pointer;">X</td></tr></table>';

    var JCTable = '<table class = "JaCalControls" width=100%><tr>';
    JCTable += '<td id="B1" onmouseover = JaCalHilight("B1"); onmouseout = JaCalResetHighlight("B1"); title="Previous Month"><a href=javascript:JaCalChangeMonth("-"); class="JaCalLink"  onMouseOver="return true;" style="cursor: pointer;">&laquo;</a></td>';
    JCTable += '<td id="B2" onmouseover = JaCalHilight("B2"); onmouseout = JaCalResetHighlight("B2"); title="Next Month"><a href=javascript:JaCalChangeMonth("+"); class="JaCalLink" onMouseOver="return true;" style="cursor: pointer;">&raquo;</a></td>';
    JCTable += '<td width="100%" align="center"><input id="JaCalInpMonth" class = "InputBox" type="textbox" size=9 value="" onfocus=clearYear(); onblur=JaCalSetMonth(); onkeypress=JaCalSetMonthOnKey(event); onClick=drpMonth();></input><div id=divMonth class=divMonth ></div></td>';
    JCTable += '<td align="center"><input id="JaCalInpYear" class = "InputBox" type="textbox" size=4 value="" onfocus=clearMonth(); onblur=JaCalSetYear(); onkeypress=JaCalSetYearOnKey(event); onClick=drpYear();></input><div id=divYear class=divYear ></div></td>';
    JCTable += '<td id="B3" onmouseover = JaCalHilight("B3"); onmouseout = JaCalResetHighlight("B3"); title="Previous Year"><a href=javascript:JaCalChangeYear("-"); class="JaCalLink"  onMouseOver="return true;" style="cursor: pointer;">&laquo;</a></td>';
    JCTable += '<td id="B4" onmouseover = JaCalHilight("B4"); onmouseout = JaCalResetHighlight("B4"); title="Next Year"><a href=javascript:JaCalChangeYear("+"); class="JaCalLink" onMouseOver="return true;" style="cursor: pointer;">&raquo;</a></td>';
    JCTable += '</tr></table>';

    JaCalControlCell.innerHTML = JCTable;
}

//Positions the calendar to the specified control
function SetPosition(ActivatedObjectId, Ox, Oy) {
    var obj = document.getElementById(ActivatedObjectId);
    var curX = 0;
    var curY = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            curX += obj.offsetLeft
            curY += obj.offsetTop
            obj = obj.offsetParent;
        }
    }
    else if (obj.x) {
        curX += obj.x;
        curY += obj.y;
    }

    if (JaCal) {
        curX = curX + parseInt(Ox);
        curY = curY + parseInt(Oy);
        //var ss= curX + "px 0 0 "+ curY  +"px";		
        //JaCal.style.margin= ss;
        JaCal.style.top = curY + "px";
        JaCal.style.left = curX + "px";
    }
}

//Generates the calendar
function GenJaCal(SelDate) {
    var tempMonth;
    var tempYear;
    document.getElementById('divMonth').innerHTML = "";
    document.getElementById('divMonth').className = "divMonth";

    document.getElementById('divYear').innerHTML = "";
    document.getElementById('divYear').className = "divYear";

    if (SelDate == null || SelDate == 'NAN') {
        SelDate = new Date();
    }
    else {
        tempMonth = SelDate.getMonth();
        tempYear = SelDate.getFullYear();
        JaCalSelDate = SelDate.getDate();
        JaCalSelMonth = SelDate.getMonth();
        JaCalSelYear = SelDate.getFullYear();
    }

    document.getElementById("JaCalInpYear").value = JaCalSelYear;
    document.getElementById("JaCalInpMonth").value = MonthName[JaCalSelMonth];

    var FirstDateOfSelMonth = new Date(JaCalSelYear, JaCalSelMonth, 1);
    var FirstDayOfSelMonth = FirstDateOfSelMonth.getDay();
    var CurDateObj = new Date();
    var CurDate = CurDateObj.getDate();
    var CurMonth = CurDateObj.getMonth();
    var CurYear = CurDateObj.getFullYear();

    var DaysInSelMonth = 32 - new Date(JaCalSelYear, JaCalSelMonth, 32).getDate();

    var JCTable = '<table class="JaCalDateTable"><tr align="center" class = "JaCalDateHeader" style="cursor: default;"> <td width="24">Su</td> <td width="24">Mo</td> <td width="24">Tu</td> <td width="24">We</td> <td width="24">Th</td> <td width="24">Fr</td> <td width="24">Sa</td></tr>';
    JCTable += '<tr align="center">';

    for (week_day = 0; week_day < FirstDayOfSelMonth; week_day++) {
        if (week_day == 0 || week_day == 6)
            JCTable += '<td class = "JaCalHoliday" style="cursor: default;">&nbsp</td>';
        else
            JCTable += '<td class = "JaCalNormalDay" style="cursor: default;">&nbsp;</td>';
    }

    week_day = FirstDayOfSelMonth;

    for (day = 1; day <= DaysInSelMonth; day++) {
        week_day %= 7;

        if (week_day == 0)
            JCTable += '</tr><tr align="center">';

        if (week_day == 0 || week_day == 6) {
            if (JaCalSelDate == day && JaCalSelYear == tempYear && JaCalSelMonth == tempMonth)
                JCTable += '<td id="C' + day + '" class = "JaCalHighlight"; onclick=JaCalSelect(' + day + '); style="cursor: pointer;">' + day + '</td>';
            else
                JCTable += '<td id="H' + day + '" class = "JaCalHoliday"; onmouseover = JaCalHilight("H' + day + '"); onmouseout = JaCalResetHighlight("H' + day + '");  onclick=JaCalSelect(' + day + '); style="cursor: pointer;">' + day + '</td>';

        }
        else if (JaCalSelYear == CurYear && JaCalSelMonth == CurMonth && CurDate == day)
            JCTable += '<td id="C' + day + '" class = "JaCalCurrentDay"; onmouseover = JaCalHilight("C' + day + '"); onmouseout = JaCalResetHighlight("C' + day + '"); onclick=JaCalSelect(' + day + '); style="cursor: pointer;">' + day + '</td>';
        else if (JaCalSelDate == day && JaCalSelYear == tempYear && JaCalSelMonth == tempMonth)
            JCTable += '<td id="C' + day + '" class = "JaCalHighlight"; onclick=JaCalSelect(' + day + '); style="cursor: pointer;">' + day + '</td>';
        else
            JCTable += '<td id="N' + day + '" class = "JaCalNormalDay"; onmouseover = JaCalHilight("N' + day + '"); onmouseout = JaCalResetHighlight("N' + day + '"); onclick=JaCalSelect(' + day + '); style="cursor: pointer;">' + day + '</td>';

        week_day++;
    }

    for (week_day; week_day < 7; week_day++) {
        if (week_day == 0 || week_day == 6)
            JCTable += '<td class = "JaCalHoliday" style="cursor: default;">&nbsp;</td>';
        else
            JCTable += '<td class = "JaCalNormalDay" style="cursor: default;">&nbsp;</td>';
    }

    JCTable += '</tr></table>';
    JCTable += '<!--[if lte IE 6.5]><iframe></iframe><![endif]-->';
    JaCalDateCell.innerHTML = JCTable;
}

//Shows calendar
function ShowJaCal(TargetObjectId, ActivatedObjectId, Ox, Oy) {
    JCTarget = TargetObjectId;
    MakePopup();
    if (JaCal) {
        if (JaCal.style.visibility == 'hidden') {
            SetPosition(ActivatedObjectId, Ox, Oy);
            if (document.getElementById(JCTarget).value == "") {
                JaCalCurDate = new Date();
                JaCalSelDate = JaCalCurDate.getDate();
                JaCalSelMonth = JaCalCurDate.getMonth();
                JaCalSelYear = JaCalCurDate.getFullYear();
                GenJaCal();
            }
            else {
                GenJaCal(checkdate(document.getElementById(JCTarget).value));
            }


            JaCal.style.visibility = 'visible';
            var CBtn = document.getElementById("X2");
            if (CBtn != null) {
                setTimeout(function() { CBtn.focus() }, 10);
            }
        }
        else {
            JaCal.style.visibility = 'hidden';
        }
    }
}

//Hides calendar
function HideJaCal() {
    if (JaCal) {
        JaCal.style.visibility = 'hidden';
        var CBtn = document.getElementById(JCTarget);
        if (CBtn != null) {
            setTimeout(function() { CBtn.focus() }, 10);
        }
    }
}

//Selected a date
function JaCalSelect(SelDate) {
    if (SelDate != "") {
        HideJaCal();
        var DispDate = SelDate.toString();
        var DispMonth = (JaCalSelMonth + 1).toString();
        var DispYear = JaCalSelYear.toString();
        if (DispDate.length == 1)
            DispDate = "0" + DispDate;
        if (DispMonth.length == 1)
            DispMonth = "0" + DispMonth;

        if (CurrentCulture == 'ar') {
            document.getElementById(JCTarget).value = DispYear + "/" + DispMonth + "/" + DispDate;
        }
        else {
            document.getElementById(JCTarget).value = DispDate + "/" + DispMonth + "/" + DispYear;
        }
    }
}

//Change year
function JaCalChangeYear(direction) {
    if (!isNaN(JaCalSelYear)) {
        JaCalSelYear = parseInt(JaCalSelYear);
    }

    if (direction == "+") {
        JaCalSelYear += 1;
    }
    else {
        JaCalSelYear -= 1;
    }
    GenJaCal();
}

//Set custom year on key press
function JaCalSetYearOnKey(e) {
    if (isIECompatible()) {
        if (e.keyCode == 13) {
            JaCalSetYear();
        }
    }
    else {
        if (e.which == 13) {
            JaCalSetYear();
        }
    }
}

//Set custom year
function JaCalSetYear() {

    var TxtNewYear = document.getElementById("JaCalInpYear");
    var NewYear = Trim(TxtNewYear.value);
    if (NewYear == JaCalSelYear) {
        return;
    }
    var NewYearLen = 0;
    if (NewYear == "" || isNaN(NewYear)) {
        TxtNewYear.value = JaCalSelYear;
        return;
    }
    else {
        NewYear = String(+NewYear);
        NewYearLen = NewYear.length;
    }
    if (NewYearLen == 1 || NewYearLen == 2 || NewYearLen == 3) {
        var NewYearVal = +NewYear;
        if (NewYearVal < 47 || NewYearVal > 99) {
            NewYear = 2000 + NewYearVal;
        }
        else {
            NewYear = 1900 + NewYearVal;
        }
    }
    else if (NewYearLen != 4) {
        TxtNewYear.value = JaCalSelYear;
        return;
    }
    JaCalSelYear = NewYear;
    GenJaCal();
}

//Change month
function JaCalChangeMonth(direction) {
    if (direction == "+") {
        if (JaCalSelMonth == 11) {
            JaCalSelMonth = 0;
            JaCalSelYear += 1;
        }
        else {
            JaCalSelMonth += 1;
        }
    }
    else {
        if (JaCalSelMonth == 0) {
            JaCalSelMonth = 11;
            JaCalSelYear -= 1;
        }
        else {
            JaCalSelMonth -= 1;
        }
    }
    GenJaCal();
}

//Set custom year on key press
function JaCalSetMonthOnKey(e) {
    if (isIECompatible()) {
        if (e.keyCode == 13) {
            JaCalSetMonth();
        }
    }
    else {
        if (e.which == 13) {
            JaCalSetMonth();
        }
    }
}

//Set custom month
function JaCalSetMonth() {
    var TxtNewMonth = document.getElementById("JaCalInpMonth");
    var NewMonth = Trim(TxtNewMonth.value);
    if (NewMonth == MonthName[JaCalSelMonth]) {
        return;
    }
    var NewMonthLen = 0;
    if (isNaN(NewMonth)) {
        NewMonthLen = NewMonth.length;
        if (NewMonthLen > 2) {
            NewMonth = NewMonth.substr(0, 3).toLowerCase();
            for (var i = 0; i < 12; i++) {
                if (ShortMonthName[i] == NewMonth) {
                    JaCalSelMonth = i;
                    break;
                }
            }
            if (i > 11) {
                TxtNewMonth.value = MonthName[JaCalSelMonth];
                return;
            }
        }
        else {
            TxtNewMonth.value = MonthName[JaCalSelMonth];
            return;
        }
        GenJaCal();
    }
    else {
        var NewMonthVal = +NewMonth;
        if (NewMonthVal > 0 && NewMonthVal < 13) {
            JaCalSelMonth = --NewMonthVal;
            GenJaCal();
        }
        else {
            TxtNewMonth.value = MonthName[JaCalSelMonth];
        }
    }
}

//Highlighting hover cell
function JaCalHilight(tdID) {
    var tdType = (tdID.substring(0, 1));
    if (tdType == 'X')
        document.getElementById(tdID).className = 'JaCalHighlight';
    else
        document.getElementById(tdID).className = 'JaCalHighlight';
}

//Resetting highlighted cell
function JaCalResetHighlight(tdID) {
    var tdType = (tdID.substring(0, 1));
    if (tdType == 'H')
        document.getElementById(tdID).className = "JaCalHoliday";
    else if (tdType == 'C')
        document.getElementById(tdID).className = "JaCalCurrentDay";
    else if (tdType == 'N')
        document.getElementById(tdID).className = "JaCalNormal";
    else if (tdType == 'X')
        document.getElementById(tdID).className = "JaCalCloseButton";
    else if (tdType == 'Y')
        document.getElementById(tdID).className = "JaCalCloseButton";
    else
        document.getElementById(tdID).className = "";
}

//Trim white space
function Trim(str) {
    return str.replace(/^\s+/g, '').replace(/\s+$/g, '');
}

function clearControl() {
    document.getElementById(JCTarget).value = "";
    HideJaCal();
}

function drpMonth() {
    var JCTable = '<table width=90px >';


    for (i = 0; i < MonthName.length; i++) {
        JCTable += '<tr align="left">';
        //JCTable += '<td ><a class=MonthLink href=# onClick=SetMonth("'+ MonthName[i] +'");>' + MonthName[i]  + '</a></td>';
        JCTable += '<td ><a class=MonthLink href=JavaScript:SetMonth("' + MonthName[i] + '");>' + MonthName[i] + '</a></td>';
        JCTable += '</tr>';
    }

    JCTable += '</table>';

    document.getElementById('divMonth').innerHTML = JCTable;
    document.getElementById('divMonth').className = "divMonthActive";
}

function drpYear() {
    var JCTable = '<table >';


    for (i = 45; i > 0; i--) {
        JCTable += '<tr align="left">';
        JCTable += '<td ><a class=MonthLink href=JavaScript:SetYear("' + parseInt(JaCalCurDate.getFullYear() - i) + '");>' + parseInt(JaCalCurDate.getFullYear() - i) + '</a></td>';
        JCTable += '</tr>';
    }
    for (i = 0; i < 15; i++) {
        JCTable += '<tr align="left">';
        JCTable += '<td ><a class=MonthLink href=JavaScript:SetYear("' + parseInt(JaCalCurDate.getFullYear() + i) + '");>' + parseInt(JaCalCurDate.getFullYear() + i) + '</a></td>';
        JCTable += '</tr>';
    }
    JCTable += '</table>';

    document.getElementById('divYear').innerHTML = JCTable;
    document.getElementById('divYear').className = "divYearActive";
}

function SetMonth(mnth) {
    document.getElementById('divMonth').innerHTML = "";
    document.getElementById('divMonth').className = "divMonth";
    document.getElementById("JaCalInpMonth").value = mnth;
    JaCalSetMonth();
}

function SetYear(yre) {
    document.getElementById('divYear').innerHTML = "";
    document.getElementById('divYear').className = "divYear";
    document.getElementById("JaCalInpYear").value = yre;

    JaCalSetYear();
}
function clearMonth() {
    document.getElementById('divMonth').innerHTML = "";
    document.getElementById('divMonth').className = "divMonth";
}

function clearYear() {
    document.getElementById('divYear').innerHTML = "";
    document.getElementById('divYear').className = "divYear";
}

function checkdate(selDate) {
    var rgx;
    var Datefield;
    var Monthfield;
    var Yearfield;
    var dayobj;
    if (CurrentCulture == 'ar') {
        rgx = /^([0-9]{2,4})(\/|-)(0?[1-9]|1[012])(\/|-)(0?[1-9]|[12][0-9]|3[01])/;
        if (!selDate.match(rgx)) {
            dayobj = new Date();
        }
        else { //Detailed check for valid date ranges
            Monthfield = selDate.split(new RegExp("[/ -]{1}"))[2];
            Datefield = selDate.split(new RegExp("[/ -]{1}"))[1];
            Yearfield = selDate.split(new RegExp("[/ -]{1}"))[0];
            if (Yearfield.length == 2) { Yearfield = '20' + Yearfield; }
            dayobj = new Date(Yearfield, Monthfield - 1, Datefield)
        }
    }
    else {
        rgx = /^(0?[1-9]|[12][0-9]|3[01])(\/|-)(0?[1-9]|1[012])\2(\d{2,4})$/;
        if (!selDate.match(rgx)) {
            dayobj = new Date();
        }
        else { //Detailed check for valid date ranges
            Datefield = selDate.split(new RegExp("[/ -]{1}"))[0];
            Monthfield = selDate.split(new RegExp("[/ -]{1}"))[1];
            Yearfield = selDate.split(new RegExp("[/ -]{1}"))[2];
            if (Yearfield.length == 2) { Yearfield = '20' + Yearfield; }
            dayobj = new Date(Yearfield, Monthfield - 1, Datefield)
        }
    }
    //alert(dayobj);
    return dayobj;
}
