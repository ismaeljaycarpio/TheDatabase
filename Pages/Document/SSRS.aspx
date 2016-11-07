<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="SSRS.aspx.cs" Inherits="Pages_Document_SSRS" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
     <script type="text/javascript" src="<%=ResolveUrl("~/Zebra_datepicker/javascript/zebra_datepicker.js")%>"></script>
    <link type="text/css" rel="stylesheet"  href="<%=ResolveUrl("~/Zebra_datepicker/css/default.css")%>" />
    <script type="text/javascript">
        $(document).ready(function () {
            initDatePickers();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(applicationInitHandler);

            function applicationInitHandler() {
                initDatePickers();
            }
            function initDatePickers() {

                try {
                    if ($.browser.webkit) {
                        $($(":hidden[id*='DatePickers']").val().split(",")).each(function (i, item) {
                            var h = $("table[id*='ParametersGrid'] span").filter(function (i) {
                                var v = "[" + $(this).text() + "]";
                                return (v != null && item != "" && v.indexOf(item) >= 0);
                            }).parent("td").next("td").find(":input:not(:checkbox)").Zebra_DatePicker({
                                format: 'd/m/Y',
                                readonly_element: false,
                                onSelect: function (view, elements) {
                                    $('input[value="View Report"]').click();
                                }


                            });
                        });
                        $($(":hidden[id*='DatePickers']").val().split(",")).each(function (i, item) {
                            var h = $("table[id*='ParametersGrid'] span").filter(function (i) {
                                var v = "[" + $(this).text() + "]";
                                return (v != null && item != "" && v.indexOf(item) >= 0);
                            }).parent("td").next("td").find("input").parent().children("input").each(function () {
                                $(this).val($(this).val().substring(0, 10));

                            });
                        });

                    }
                }
                catch (err) {
                    // $('textarea').trigger('change');  $('input[value="View Report"]').click();
                }

            }


            //function initDatePickers() {

            //    try{
            //        if ($.browser.webkit) {
            //            $('[id*=ParameterTable] td span:contains("Date")')
            //                .each(function () {
            //                    var td = $(this).parent().next();
            //                    $(':input:not(:checkbox)', td).Zebra_DatePicker({
            //                        format: 'd/m/Y',
            //                        readonly_element: false,
            //                        onSelect: function (view, elements) {
            //                            $('input[value="View Report"]').click();
            //                        }


            //                    });
            //                });
            //            $('[id*=ParameterTable] td span:contains("Date")')
            //             .each(function () {
            //                 var td = $(this).parent().next();
            //                 $('input[type=text]', td).each(function () {
            //                     $(this).val($(this).val().substring(0, 10));

            //                 });
            //             });

            //        }
            //    }
            //    catch(err)
            //    {
            //        // $('textarea').trigger('change');  $('input[value="View Report"]').click();
            //    }

            //}

        });


        //$(document).ready(function () {
        //    initDatePickers();
        //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(applicationInitHandler);

        //    function applicationInitHandler() {
        //        initDatePickers();
        //    }

        //    function initDatePickers() {

        //        try {
        //            if ($.browser.webkit) {
        //                $('[id*=ParameterTable] td span:contains("Date")')
        //                    .each(function () {
        //                        var td = $(this).parent().next();
        //                        $(':input:not(:checkbox)', td).datepicker({
        //                            showOn: "button"
        //                   , buttonImage: '/Reserved.ReportViewerWebControl.axd?OpType=Resource&Name=Microsoft.Reporting.WebForms.calendar.gif'
        //                   , buttonImageOnly: true
        //                   , dateFormat: 'dd/mm/yyyy'
        //                   , changeMonth: true
        //                   , changeYear: true
        //                        });
        //                    });
        //                $('[id*=ParameterTable] td span:contains("Date")')
        //                 .each(function () {
        //                     var td = $(this).parent().next();
        //                     $('input[type=text]', td).each(function () {
        //                         $(this).val($(this).val().substring(0, 10));

        //                     });
        //                 });

        //            }
        //        }
        //        catch (err) {
        //            // $('textarea').trigger('change');
        //        }

        //    }

        //});



        //$(document).ready(function () {

        //    function fixParameters() {
        //        if ($.browser.webkit) {

        //            // add date picker
        //            $($(":hidden[id*='DatePickers']").val().split(",")).each(function (i, item) {
        //                var h = $("table[id*='ParametersGrid'] span").filter(function (i) {
        //                    var v = "[" + $(this).text() + "]";
        //                    return (v != null && item != "" && v.indexOf(item) >= 0);
        //                }).parent("td").next("td").find(":input:not(:checkbox)").datepicker({
        //                    showOn: "button"
        //                   , buttonImage: '/Reserved.ReportViewerWebControl.axd?OpType=Resource&Name=Microsoft.Reporting.WebForms.calendar.gif'
        //                   , buttonImageOnly: true
        //                   , dateFormat: 'dd/mm/yyyy'
        //                   , changeMonth: true
        //                   , changeYear: true
        //                });
        //            });

        //            // remove time from date  parent("td").next("td").find(":input:not(:checkbox)").datepicker
        //            $($(":hidden[id*='DatePickers']").val().split(",")).each(function (i, item) {
        //                var h = $("table[id*='ParametersGrid'] span").filter(function (i) {
        //                    var v = "[" + $(this).text() + "]";
        //                    return (v != null && item != "" && v.indexOf(item) >= 0);
        //                }).parent("td").next("td").find("input").parent().children("input").each(function () {
        //                    $(this).val($(this).val().substring(0, 10));
        //                });
        //            });
        //        }
        //    }

        //    fixParameters();
        //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(applicationInitHandler);

        //    function applicationInitHandler() {
        //        fixParameters();
        //    }




        //});

    </script>
    <div style="width: 100%; height: 100%; padding: 10PX 10PX 10PX 10PX">
        <div style="padding-left: 500px;">
            <asp:HyperLink runat="server" ID="hlBack" ToolTip="Back">
                <asp:Image ID="Image25" runat="server" ImageUrl="~/App_Themes/Default/images/back32.png" />
            </asp:HyperLink>
        </div>
        <br /><asp:HiddenField ID="DatePickers" runat="server" ClientIDMode="Static" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%" AsyncRendering="false"  SizeToReportContent="true">
            
        </rsweb:ReportViewer>
        <br /> 
        <asp:Label runat="server" ID="lblNoReport" Visible="false" Text="No Report!"></asp:Label>
    </div>
</asp:Content>
