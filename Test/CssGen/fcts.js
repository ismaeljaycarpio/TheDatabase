image1 = new Image();
image1.src = "http://csstxt.com/img/information.gif";
image2 = new Image();
image2.src = "http://csstxt.com/img/start-result.gif";
image3 = new Image();
image3.src = "http://csstxt.com/img/end-result.gif";
image4 = new Image();
image4.src = "http://csstxt.com/img/ic-code.jpg";

function displaytext(a) {
    thiselement = document.getElementById(a);
    if (thiselement.style.display == "none" || thiselement.style.display == "") {
        thiselement.style.display = "block";
        thiselement.focus()
    } else {
        thiselement.style.display = "none"
    }
}

function fct_0(b, c, a, d) {
    if (document.getElementById(b).className == c) {
        document.getElementById(b).className = a;
        document.getElementById(d).value = "1"
    } else {
        document.getElementById(b).className = c;
        document.getElementById(d).value = "2"
    }
    new_fct()
}

function load_new() {
    if (document.getElementById("step_load").value == "on") {
        new_fct()
    } else {
        document.getElementById("step_load").value = "off"
    }
}

function load_new_last() {
    if (document.getElementById("step_load").value == "on") {
        new_fct()
    } else {
        document.getElementById("step_load").value = "on"
    }
}

function i3_b() {
    if (document.getElementById("bordercontent").value == "none") {
        document.getElementById("slider5").value = 0;
        document.getElementById("myInput3").value = ""
    } else {
        var a = 0
    }
}

function new_fct() {
   
    error_load = false;
    var E = 8000;
    if (document.getElementById("zonetext0").value.length > E) {
        document.getElementById("zone01").innerHTML = '<font color="#FF0000">Too many characters</font>';
        document.getElementById("zonetext0").focus();
        error_load = true
    } else {
        document.getElementById("zone01").innerHTML = ""
    }
    
    var x = document.getElementById("zonetext0").value;
    var w = x.split("&").join("&amp;").split("<").join("&lt;").split(">").join("&gt;");
    x = w;
    if (!error_load) {
       
        var d = window.location.href;
        var I = /http:\/\/csstxt.com/gi;
//        if (!d.match(I)) {
//            return false
//        }
        if (document.getElementById("seecode").value == 1) {
            document.getElementById("a1").style.display = "none"
        }
        var g = "";
        var R = document.getElementById("idb0").value;
        var q = document.getElementById("b0val").value;
        var Q = document.getElementById("idb1").value;
        var p = document.getElementById("b1val").value;
        var P = document.getElementById("idb2").value;
        var o = document.getElementById("b2val").value;
        var O = document.getElementById("idb3").value;
        var n = document.getElementById("b3val").value;
        var N = document.getElementById("idb4").value;
        var m = document.getElementById("b4val").value;
        var M = document.getElementById("idb5").value;
        var l = document.getElementById("b5val").value;
        var L = document.getElementById("idb6").value;
        var k = document.getElementById("b6val").value;
        var K = document.getElementById("idb7").value;
        var i = document.getElementById("b7val").value;
        if (q == 1) {
            g += R + ";"
        } else {
            g += "font-weight:normal;"
        }
        if (p == 1) {
            if (o == 1) {
                if (l == 1) {
                    g += "text-decoration:" + Q + " " + P + " " + M + ";"
                } else {
                    g += "text-decoration:" + Q + " " + P + ";"
                }
            } else {
                if (l == 1) {
                    g += "text-decoration:" + Q + " " + M + ";"
                } else {
                    g += "text-decoration:" + Q + ";"
                }
            }
        } else {
            if (o == 1) {
                if (l == 1) {
                    g += "text-decoration:" + P + " " + M + ";"
                } else {
                    g += "text-decoration:" + P + ";"
                }
            } else {
                if (l == 1) {
                    g += "text-decoration:" + M + ";"
                } else {
                    g += ""
                }
            }
        }
        if (n == 1) {
            if (m == 1) {
                g += N + ";"
            } else {
                g += O + ";"
            }
        } else {
            if (m == 1) {
                g += N + ";"
            } else {
                g += ""
            }
        }
        if (k == 1) {
            if (i == 1) {
                g += K + ";"
            } else {
                g += L + ";"
            }
        } else {
            if (i == 1) {
                g += K + ";"
            } else {
                g += ""
            }
        }
        var v = document.getElementById("myInput1").value;
        g += "color:#" + v + ";";
        var B = document.getElementById("myInput2").value;
        if (B != "") {
            g += "background-color:#" + B + ";"
        }
        var t = '#' + document.getElementById("myInput3").value;
        var C = document.getElementById("slider5").value;
        var F = document.getElementById("bordercontent").value;
        if (C > 0 && t != "" && F != "none") {
            g += "border: " + C + "px " + F + " " + t + ";"
        }
        var U = document.getElementById("slider1").value;
        g += "letter-spacing:" + U + "pt;";
        var u = document.getElementById("slider2").value;
        g += "word-spacing:" + u + "pt;";
        var A = document.getElementById("slider3").value;
        var Y = document.getElementById("pixeltext").value;
        var b = document.getElementById("emtext").value;
        var y = document.getElementById("pcttext").value;
        if (Y == 1) {
            g += "font-size:" + A + "px;"
        } else {
            if (y == 1) {
                g += "font-size:" + A + "%;"
            } else {
                if (b == 1) {
                    g += "font-size:" + A + "em;"
                } else {
                    g += "font-size:" + A + "px;"
                }
            }
        }
        var s = document.getElementById("alignfont").value;
        g += "text-align:" + s + ";";
        var ab = document.getElementById("familyfont").value;
        g += "font-family:" + ab + ";";
        var a = document.getElementById("slider4").value;
        g += "line-height:" + a + ";";
        var aa = document.getElementById("slider6").value;
        if (aa > 0 && aa != "") {
            g += "margin:" + aa + "px;"
        }
        var r = document.getElementById("slider7").value;
        if (r > 0 && r != "") {
            g += "padding:" + r + "px;"
        }
        var W = document.getElementById("slider8").value;
        if (W > 0 && W != "") {
            g += "width:" + W + "px;"
        }
        var X = document.getElementById("slider9").value;
        if (X > 0 && X != "") {
            g += "height:" + X + "px;"
        }
        var S = "</p>";
        var T = document.getElementById("ptagtext").value;
        if (T == 1) {
            start_style = '<p style="';
            S = "</p>"
        } else {
            start_style = '<div style="';
            S = "</div>"
        }
        var D = '">';
        var J = '<img border="0" valign="absmiddle" src="http://csstxt.com/img/start-result.gif" alt="" />';
        var V = '<img border="0" valign="absmiddle" src="http://csstxt.com/img/end-result.gif" alt="" />';
        var z = '<br /><br /><center><a href="javascript:void(0);" onclick="displaytext(\'a1\');document.getElementById(\'seecode\').value=\'2\';new_fct()"><img style="border:none;" onmouseover="this.src=\'img/ic-code-1.jpg\'" onmouseout="this.src=\'img/ic-code.jpg\'" src="http://csstxt.com/img/ic-code.jpg" align="absmiddle" alt="" /></center>';
        var c = window.location.href;
        var G = /http:\/\/csstxt.com/gi;
//        if (!c.match(G)) {
//            return false
//        }
//        alert('Y');

        document.getElementById("zonetextnew").innerHTML = J + start_style + g + D + x + S + V + z;
        if (document.getElementById("seecode").value == 2) {
            var j = '<textarea style="color:#0F4A73;width:680px;height:85px;" onclick="this.focus();this.select();" id="text_a" cols="81" rows="5" readonly="readonly">';
            var h = "</textarea>";
            document.getElementById("csscode1").innerHTML = j + start_style + g + D + x + S + h;
            var f = '<textarea style="color:#0F4A73;width:680px;height:260px;" onclick="this.focus();this.select();" id="text_b" rows="15" readonly="readonly">';
            var e = "</textarea>";
            if (T == 1) {
                var Z = '<html><head>\n<style type="text/css">\n.mycss\n{\n' + g;
                var H = '\n}\n</style>\n</head>\n<body>\n<p class="mycss">' + x + "</p>\n</body>\n</html>";
                document.getElementById("i1").innerHTML = 'It creates a css style into the Html Tag &lt;p&gt; like this : &lt;p style="..."&gt;text example&lt;/p&gt;';
                document.getElementById("i2").innerHTML = 'It creates a stylesheet with a class called .mycss : to use it : &lt;p class="mycss"&gt;text example&lt;/p&gt;';
                document.getElementById("csscode2").innerHTML = f + Z + H + e
            } else {
                var Z = '<html><head>\n<style type="text/css">\n.mycss\n{\n' + g;
                var H = '\n}\n</style>\n</head>\n<body>\n<div class="mycss">' + x + "</div>\n</body>\n</html>";
                document.getElementById("i1").innerHTML = 'It creates a css style into the Html Tag &lt;div&gt; like this : &lt;div style="..."&gt;text example&lt;/div&gt;';
                document.getElementById("i2").innerHTML = 'It creates a stylesheet with a class called .mycss : to use it : &lt;div class="mycss"&gt;text example&lt;/div&gt;';
                document.getElementById("csscode2").innerHTML = f + Z + H + e
            }
            document.getElementById("seecode").value = "1"
        }
    }
}
Slider.Range = Slider.extend({
    options: {
        start: 0,
        end: 150
    },
    initialize: function (c, a, b) {
        this.parent(c, a, b);
        this.options.steps = this.options.end - this.options.start;
        return this
    },
    set: function (a) {
        this.step = a.limit(this.options.start, this.options.end);
        this.checkStep();
        this.end();
        this.fireEvent("onTick", this.toPosition(this.step));
        return this
    },
    toStep: function (a) {
        return Math.round((a + this.options.offset) / this.max * this.options.steps) + this.options.start
    },
    toPosition: function (a) {
        return (this.max * a / this.options.steps) - (this.max * this.options.start / this.options.steps)
    }
});
window.addEvent("domready", function () {
    var e = new Slider.Range("area1", "knob1", {
        start: -10,
        end: 10,
        onChange: function (f) {
            $("spanslider1").setHTML('<input id="slider1" type="text" class="sliding" size="2" value="' + f + '">')
        },
        onComplete: function () {
            load_new()
        }
    }).set(1);
    var d = new Slider.Range("area2", "knob2", {
        start: -20,
        end: 30,
        onChange: function (f) {
            $("spanslider2").setHTML('<input id="slider2" type="text" class="sliding" size="2" value="' + f + '">')
        },
        onComplete: function () {
            load_new()
        }
    }).set(2);
    var c = new Slider.Range("area3", "knob3", {
        start: 0,
        end: 150,
        onChange: function (f) {
            $("spanslider3").setHTML('<input id="slider3" type="text" class="sliding" size="2" value="' + f + '">')
        },
        onComplete: function () {
            load_new()
        }
    }).set(12);
    var b = new Slider.Range("area4", "knob4", {
        start: 0,
        end: 5,
        onChange: function (f) {
            $("spanslider4").setHTML('<input id="slider4" type="text" class="sliding" size="2" value="' + f + '">')
        },
        onComplete: function () {
            load_new()
        }
    }).set(1);
    var a = new Slider.Range("area5", "knob5", {
        start: 0,
        end: 50,
        onChange: function (g) {
            $("spanslider5").setHTML('<input id="slider5" type="text" class="sliding" size="2" value="' + g + '">');
            if (g > 0) {
                if (document.getElementById("bordercontent").value == "none") {
                    document.getElementById("bordercontent").value = "solid"
                }
                if (document.getElementById("myInput3").value == "") {
                    document.getElementById("myInput3").value = "b5a759"
                }
                var f = 0
            } else {
                document.getElementById("bordercontent").value = "none";
                document.getElementById("myInput3").value = ""
            }
        },
        onComplete: function () {
            load_new_last()
        }
    }).set(0);
    new_fct()
});
var offsetx = 12;
var offsety = 8;

function newelement(newid) {
    if (document.createElement) {
        var el = document.createElement("div");
        el.id = newid;
        with (el.style) {
            display = "none";
            position = "absolute"
        }
        el.innerHTML = "&nbsp;";
        document.body.appendChild(el)
    }
}
var ie5 = (document.getElementById && document.all);
var ns6 = (document.getElementById && !document.all);
var ua = navigator.userAgent.toLowerCase();
var isapple = (ua.indexOf("applewebkit") != -1 ? 1 : 0);

function getmouseposition(c) {
    if (document.getElementById) {
        var b = (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body;
        pagex = (isapple == 1 ? 0 : (ie5) ? b.scrollLeft : window.pageXOffset);
        pagey = (isapple == 1 ? 0 : (ie5) ? b.scrollTop : window.pageYOffset);
        mousex = (ie5) ? event.x : (ns6) ? clientX = c.clientX : false;
        mousey = (ie5) ? event.y : (ns6) ? clientY = c.clientY : false;
        var a = document.getElementById("tooltip");
        a.style.left = (mousex + pagex + offsetx) + "px";
        a.style.top = (mousey + pagey + offsety) + "px"
    }
}
function tooltip(b) {
    if (!document.getElementById("tooltip")) {
        newelement("tooltip")
    }
    var a = document.getElementById("tooltip");
    a.innerHTML = b;
    a.style.display = "block";
    document.onmousemove = getmouseposition
}
function exit() {
    document.getElementById("tooltip").style.display = "none"
}
function ajaxget(a, b) {
    if (window.XMLHttpRequest) {
        obj1 = new XMLHttpRequest()
    } else {
        if (window.ActiveXObject) {
            obj1 = new ActiveXObject("Microsoft.XMLHTTP")
        } else {
            return (false)
        }
    }
    obj1.open("GET", a, true);
    obj1.onreadystatechange = function () {
        if (obj1.readyState == 1) {
            var c = "b"
        } else {
            if (obj1.readyState == 4) {
                if (obj1.status == 200) {
                    document.getElementById(b).innerHTML = obj1.responseText
                } else {
                    if (obj1.status == 404) {
                        document.getElementById(b).innerHTML = "Url BUG"
                    } else {
                        document.getElementById(b).innerHTML = "Error : ".obj1.status
                    }
                }
            }
        }
    };
    obj1.send(null);
    return
};