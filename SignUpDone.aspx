<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="SignUpDone.aspx.cs" Inherits="SignUpDone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">



<!-- Google Code for Landing on Do It Yourself Page Conversion Page -->
<script type="text/javascript">
/* <![CDATA[ */
var google_conversion_id = 955007101;
var google_conversion_language = "en";
var google_conversion_format = "3";
var google_conversion_color = "ffffff";
var google_conversion_label = "fFIZCNzg31cQ_YCxxwM";
var google_remarketing_only = false;
/* ]]> */
</script>
<script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js">
</script>
<noscript>
<div style="display:inline;">
<img height="1" width="1" style="border-style:none;" alt="" src="//www.googleadservices.com/pagead/conversion/955007101/?label=fFIZCNzg31cQ_YCxxwM&amp;guid=ON&amp;script=0"/>
</div>
</noscript>

<script   type="text/javascript" language="javascript">
    
    window.setTimeout(function () {
        window.location.href = document.getElementById('hfRoot').value + '/Default.aspx?FromSignUp=yes';
    }, 1000);
</script>



<asp:HiddenField runat="server" ID="hfRoot" ClientIDMode="Static" />

<div>
     <br />
     <br />

     Loading website ....... 
     
     <br />
     <br />

     <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx?FromSignUp=yes">Take me to the website now</asp:HyperLink>

</div>

</asp:Content>

