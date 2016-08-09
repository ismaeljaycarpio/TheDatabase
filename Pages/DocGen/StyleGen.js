



function new_fct() {

    error_load = false;
    var E = 8000;
//    if (document.getElementById("zonetext0").value.length > E) {
////        document.getElementById("zone01").innerHTML = '<font color="#FF0000">Too many characters</font>';
//        document.getElementById("zonetext0").focus();
//        error_load = true
//    } else {
////        document.getElementById("zone01").innerHTML = ""
//    }

    //    var x = document.getElementById("zonetext0").value;
    //    var x = document.getElementById("Record").innerHTML;
    var x = 'The quick brown fox jumps over the lazy dog';
    var w = x.split("&").join("&amp;").split("<").join("&lt;").split(">").join("&gt;");
    x = w;
    if (!error_load) {

        var d = window.location.href;
     
        var g = "";
        var chkBold = document.getElementById("chkBold");
        var chkUnderline = document.getElementById("chkUnderline");
        var chkItalic = document.getElementById("chkItalic");
        var chkStrikethrough = document.getElementById("chkStrikethrough");
       
        if (chkBold.checked == true) {
            g += "font-weight:bold;"
        }
        else {
            g += "font-weight:normal;"
        }

        if (chkUnderline.checked == true && chkStrikethrough.checked == true) {
            g += "text-decoration:" + "underline line-through;";
        }
        else {
            if (chkStrikethrough.checked == true) {
                g += "text-decoration:" + "line-through;";
            }
            if (chkUnderline.checked == true) {
                g += "text-decoration:" + "underline;";
            }

        }

        if (chkItalic.checked == true) {
            g += "font-style:italic;";
        }


        var ddlTextColour = document.getElementById("ddlTextColour");
        if (ddlTextColour.value!='')
        {
            g += "color:" + ddlTextColour.value + ";";
        }
       
       var ddlBackground = document.getElementById("ddlBackground");
        if (ddlBackground.value!='')
        {
            g += "background-color:" + ddlBackground.value + ";";
        }
        
        var ddlBorder = document.getElementById("ddlBorder");

        if (ddlBorder.value != '') {

            if (ddlBorder.value > 0) {
                var ddlBorderColour = document.getElementById("ddlBorderColour");
                if (ddlBorderColour.value != '') {
                    g += "border: " + ddlBorder.value + "px solid " + ddlBorderColour.value + ";";
                }
                else {
                    g += "border: " + ddlBorder.value + "px solid;";
                }
                
            }
          
        }

        var ddlFontSize = document.getElementById("ddlFontSize");

        if (ddlFontSize.value != '') {
            g += "font-size:" + ddlFontSize.value + "px;"
        }

        var ddlFont = document.getElementById("ddlFont");
        if (ddlFont.value != '') {
            g += "font-family:" + ddlFont.value + ";";
        }

        var ddlLineHeight = document.getElementById("ddlLineHeight");
        if (ddlLineHeight.value != '') {
            g += "line-height:" + ddlLineHeight.value + ";";
        }

        var txtMargin = document.getElementById("txtMargin");
        if (txtMargin.value != '') {           
                g += "margin:" + txtMargin.value + "px;";           
        }

            document.getElementById("zonetextnew").innerHTML = 'The quick brown fox jumps over the lazy dog';
            $("#zonetextnew").attr('style', g);
            
//        document.getElementById("zonetextnew").innerHTML = "<div style='width:680px;" + g + "'>" + x + "</div>";

//        document.getElementById("csscode1").innerHTML = '<textarea style="color:#0F4A73;width:680px;height:100px;" onclick="this.focus();this.select();" id="text_b" rows="10" readonly="readonly">' + g + '</textarea>';
        var txtGenStyle = document.getElementById("txtGenStyle");
        txtGenStyle.value = g;
        document.getElementById('hlAdvanced').href = 'StyleAdvanced.aspx?txtGenStyle=' + encodeURIComponent(document.getElementById('txtGenStyle').value) + '&txtStyle=' + encodeURIComponent(document.getElementById('txtStyle').value);


//        alert(x);
    }
}




function validatenum(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]/;

    if (key == 0 || key == 1 || key == 2 || key == 3 || key == 4
    || key == 5 || key == 6 || key == 7 || key == 8 || key == 9) {

        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }
}


