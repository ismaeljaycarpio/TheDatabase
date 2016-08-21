<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Test_Default"
    EnableTheming="true" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%-- <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script> --%>
</head>
<body style="background-image: none; background-color: White;">


    <script type="text/javascript">

        function ColumnTypeIn(sCT,sSCT)
        {
            var s_a = sSCT.split(',');
            for(var i=0; i<s_a.length; i++)
            {
                if(s_a[i]==sCT)
                {
                    return true;
                }
            }
            return false;
        }

        //alert(ColumnTypeIn('aa', 'ask,dskjfh,aa,sdjhg'));
        //$(function () {
        //    $("#tbAuto").autocomplete({
        //        source: function (request, response) {
        //            $.ajax({
        //                url: "../CascadeDropdown.asmx/GetTestList",
        //                data: "{ 'AccountName': '" + request.term + "' }",
        //                dataType: "json",
        //                type: "POST",
        //                contentType: "application/json; charset=utf-8",
        //                dataFilter: function (data) { return data; },
        //                success: function (data) {
        //                    response($.map(data.d, function (item) {
        //                        return {
        //                            value: item.AccountName,
        //                            id: item.AccountID
        //                        }
        //                    }))
        //                },
        //                error: function (XMLHttpRequest, textStatus, errorThrown) {
        //                    alert(textStatus);
        //                }
        //            });
        //        },
        //        minLength: 1,
        //        change: function (event, ui) {
        //            //alert(ui.item.id); 
        //            if (ui.item.id == null) {
        //                document.getElementById('hfAccountID').value = '';
        //            }
        //            else {
        //                document.getElementById('hfAccountID').value = ui.item.id;
        //            }
        //        }
        //    });
        //});





        //     $(function () {
        //         $("#txtSTValue").autocomplete({
        //             source: function (request, response) {
        //                 $.ajax({
        //                     url: "../CascadeDropdown.asmx/GetDisplayColumns",
        //                     data: "{'samplecolumnid':'13609', 'search': '" + request.term + "' }",
        //                     dataType: "json",
        //                     type: "POST",
        //                     contentType: "application/json; charset=utf-8",
        //                     dataFilter: function (data) { return data; },
        //                     success: function (data) {
        //                         response($.map(data.d, function (item) {
        //                             return {
        //                                 value: item.Text,
        //                                 id: item.ID
        //                             }
        //                         }))
        //                     },
        //                     error: function (XMLHttpRequest, textStatus, errorThrown) {
        //                         alert(textStatus);
        //                     }
        //                 });
        //             },
        //             minLength: 1,
        //             select: function (event, ui) {
        //                 if (ui.item.id == null) {
        //                     document.getElementById('hfAccountID').value = '';
        //                 }
        //                 else {
        //                     document.getElementById('hfAccountID').value = ui.item.id;
        //                 }
        //             }
        //         });
        //     });






        //function showid() {

        //    alert(document.getElementById('hfAccountID').value);
        //}


    </script>

    <form id="form1" runat="server">
        <div style="padding: 20px;">
            Test page
        <br />
            <div class="demo">



                 <br />
                <br />
                 Old BatchID<asp:TextBox runat="server" ID="txtBatchID" Width="70px"></asp:TextBox>

                <br />
                <br />
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Import above Batch again" />

                <br />
                <br />
                <br />
                <asp:Label runat="server" ID="lblMsg"></asp:Label>

            </div>
        </div>
    </form>
</body>
</html>
