<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
 EnableTheming="true"  ValidateRequest="false" 
CodeFile="Default.aspx.cs" Inherits="DocGen.Document.DashBoard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>
    <asp:Literal ID="ltCommonStyles" runat="server">
        <style type="text/css">

             .TopRightClose
            {
                position: absolute;
                top: -15px;
                right: -15px;
                width: 30px;
                height: 30px;
                background: transparent url('fancybox/fancybox.png') -40px 0px;
                cursor: pointer;
                z-index: 1103;
            }


            h1, h2, h3, h4, h5, h6, p, span, label, li, th, td
            {
                font-family: Verdana,Arial,Helvetica,sans-serif;
            }

            th
            {
                background: #f1f1f1;
            }

            .ReportContentContainer *
            {
                color: #000000;
            }

            table.TableSection
            {
                border-collapse: collapse;
                border-color:Black;
            }

            table.TableSection th
            {
                background-color: #f8f8f8;
            }

             table.RecordTableSection
            {
                border-collapse: collapse;
                border-color:Black;
            }
            table.RecordTableSection th
            {
                background-color: #f8f8f8;
            }


            
             table.CalendarSection
            {
                border-collapse: collapse;
                border-color:Black;
            }

            table.CalendarSection th
            {
                background-color: #f8f8f8;
            }

            .MsoTocHeading
            {
                font-family: Verdana,Arial,Helvetica,sans-serif;
                font-weight: bold;
                font-size: 20px;
                text-align: center;
            }
            
            p.MsoToc1, li.MsoToc1, div.MsoToc1, p.MsoToc2, li.MsoToc2, div.MsoToc2, p.MsoToc3, li.MsoToc3, div.MsoToc3
            {
                mso-style-update: auto;
                mso-style-priority: 39;
                mso-style-next: Normal;
                margin-top: 0in;
                margin-right: 0in;
                margin-bottom: 5.0pt;
                mso-pagination: widow-orphan;
                font-size: 11.0pt;
                font-family: Verdana,Arial,Helvetica,sans-serif;
                mso-ascii-font-family: Calibri;
                mso-ascii-theme-font: minor-latin;
                mso-fareast-font-family: Calibri;
                mso-fareast-theme-font: minor-latin;
                mso-hansi-font-family: Calibri;
                mso-hansi-theme-font: minor-latin;
                mso-bidi-font-family: "Times New Roman";
                mso-bidi-theme-font: minor-bidi;
                text-decoration: none;
            }
            
            p.MsoToc1, li.MsoToc1, div.MsoToc1
            {
                margin-left: 0pt;
            }

            p.MsoToc2, li.MsoToc2, div.MsoToc2
            {
                margin-left: 11.0pt;
            }

            p.MsoToc3, li.MsoToc3, div.MsoToc3
            {
                margin-left: 22.0pt;
            }

            p.MsoToc4, li.MsoToc4, div.MsoToc4
            {
                margin-left: 33.0pt;
            }

            p.MsoToc5, li.MsoToc5, div.MsoToc5
            {
                margin-left: 44.0pt;
            }

            p.MsoToc6, li.MsoToc6, div.MsoToc6
            {
                margin-left: 55.0pt;
            }
            
            .MsoHyperlink a
            {
                text-decoration: none;
            }
            
            h1
            {
                font-size: 18px;
                color: #000;
                margin: 0px;
            }
            h2
            {
                font-size: 15px;
                color: #000;
                margin: 0px;
            }


            h3
            {
                font-size: 13px;
                margin: 0px;
            }
            h4
            {
                font-size: 12px;
                margin: 0px;
            }


            p
            {
                font-size: 11px;
                margin: 0px;
            }
            
            
            #EditAreaContainer
            {
                width: 100%;
                clear: both;
            }
            #EditArea
            {
                z-index: 9999;

            }

            #EditArea.Empty
            {
                height: 100px;
                border: solid 1px #f8f8f8;
                margin: 10px 0px;
                border-left-width: 25px;
                border-radius: 5px 0px 0px 5px;
                background-color: #FFFFFF;
            }

            #EditArea div.Section
            {
                border: solid 1px #f8f8f8;
                margin: 0px 0px;
                padding: 20px 0px 0px 0px;
                position: relative;
                min-height: 50px;
                border-radius: 5px 0px 0px 5px;
                background-color: #FFFFFF;
                cursor: move;
                overflow:hidden;
            }

            #EditArea div.Section:hover
            {
                border-color: #e6e6e6;
                background-color: #FCFBED;
            }

            #EditArea div.Section > .Toolbar
            {
                display: none;
                width:100%;
            }

            #EditArea div.Section > .Toolbar > .section-type
            {
                float:right;
                padding-right:20px;
            }


            #EditArea div.Section:hover > .Toolbar
            {
                display: block;
            }


            #EditArea .Toolbar a
            {
                cursor: pointer;
            }


            #EditArea .Toolbar span #EditArea > div.Section
            {
            }


            #EditArea div.Section > .Toolbar
            {
                position: absolute;
                left: 0px;
                top: 0px;
                background:#e6e6e6;
                border-radius: 5px 0px 0px 0px;
                padding:3px 6px;
            }


            #EditArea div.Section > .Toolbar > a
            {
                display:inline-block !important;
            }



            #Toolbox
            {
                width: 100px;
                position: fixed;
                bottom: 30px;
                right: 100px;
                background-color: #ffffff;
                z-index: 100;
                border-style: solid;
                border-color: #BDBFBC;
            }

            #Toolbox > div
            {
                margin: 3px 5px;
                padding: 5px;
            }
            #Toolbox .ui-draggable
            {
                cursor: move;
            }
            #Toolbox .Toolbar
            {
                display: none;
            }

            #EditArea .Toolbar a.ui-icon-carat-2-n-s
            {
                cursor: move;
            }

            .LayoutTable
            {
            }
            td.Zone
            {
               
                vertical-align: top;
            }

            td.Zone .ZoneInner
            {
                padding: 0px;
                min-height: 50px;
                border: 1px dotted #CACACA;
            }

            .position-indicator
            {
                position: relative;
                background: red;
            }

            .position-indicator > hr
            {
                position: absolute;
                top: -2px;
                height: 0px;
                width: 100%;
                margin: 0px;
                height: 2px;
                border: none;
                background: orange;
            }
        </style>
    </asp:Literal>

    <%--<script type="text/javascript">
        function readyFancy() {
            $("a#sampleVT").trigger("click");
        }
        function callMe(site) {
            $("#sampleVT").fancybox({
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '' + site + '',
                'autoScale': true,
                'onStart': function () { $("body").css({ 'overflow': 'hidden' }); },
                'onClosed': function () { $("body").css({ "overflow": "visible" }); }
            });
            readyFancy();
        }
    </script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
    <link href="<%=ResolveUrl("~/Styles/jquery-ui-1.7.3.custom.css")%>" rel="stylesheet" 
    type="text/css" />     
    <script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-ui-1.9.2.custom.min.js")%>"></script>



   
 <script type="text/javascript" language="JavaScript">

     //function autoResize(id) {

     //    try
     //    {
     //        setTimeout(function () { autoResize1(id);}, 1000);
     //    }
     //    catch(err)
     //    {
     //        //
     //    }         
        
     //}

     function ResizeDashboardControl(id) {
         try
         {
             var newwidth=$(window).width();
             var theControl=document.getElementById(id);
             theControl.style.width = ((newwidth*5)/6) + "px";
         }
         catch(err)
         {
             //
         }

     }

    
     function autoResize(id) {

         try
         {
             var newheight;
             var newwidth;
             var theIframe= document.getElementById(id);
             if (document.getElementById) {
                 newheight =theIframe.contentWindow.document.body.scrollHeight;
                 newwidth =  $(window).width(); //theIframe.contentWindow.document.body.scrollWidth;
                 //newwidth = $(window).width() -100;
                 theIframe.style.height = (newheight) + "px";
                 theIframe.style.width = ((newwidth*5)/6) + "px";
             }
         }
         catch(err)
         {
             //
             //alert(err.message);
         }         
        
     }

     
     
     function autoResizeRecord(id,w) {

         try
         {
             var newheight;
             var newwidth;
             var theIframe= document.getElementById(id);
             if (document.getElementById) {
                 newheight = theIframe.contentWindow.document.body.scrollHeight;
                 newwidth = theIframe.contentWindow.document.body.scrollWidth;
                 //newwidth = $(window).width() -100;

                 theIframe.style.height = (newheight) + "px";
                 theIframe.style.width =(newwidth) + "px";  
             
             }
         }
         catch(err)
         {
             //
         }     
       
     }




     $(document).ready(function () {
         ResizeDashboardControl('tblEditDashboard');
     });



</script>




      <%--<a href="#" id="sampleVT"></a>--%> 
    <br />
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification" ValidationGroup="MainValidationGroup" />
   
    
    <div class="demo" runat="server" id="divEditDashboard" style="width:100%;">
        <div id="Toolbox">

             <asp:LinkButton ID="lnkNoEditDashboard" ToolTip="Exit Editing" CssClass="TopRightClose" runat="server" ClientIDMode="Static" OnClick="lnkNoEditDashboard_Click" >
                         
            </asp:LinkButton>

            <h3>
                &nbsp; Drag to add</h3>
            <div class="Section NewSection MapSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a>
                    <a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())"
                        title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())"
                            title="Delete section"></a>
                    <div class="section-type">Map</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image5" runat="server"  ImageUrl="~/App_Themes/Default/images/Map.png"  ToolTip="Map" />
                </div>
            </div>

             <div class="Section NewSection DashChartSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                    <div class="section-type">Chart</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Default/images/chart_big.png"  ToolTip="Chart" />
                </div>
            </div>

           <div class="Section NewSection DialSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                    <div class="section-type">Dial</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image7" runat="server"  ImageUrl="~/App_Themes/Default/images/clock_select_remain.png"  ToolTip="Dial" />
                </div>
            </div>

            <%--<div class="Section NewSection TextSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a>
                    <a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())"
                     title="Edit section"></a><a class="ui-icon ui-icon-trash" 
                     onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/images/text_big.png" ToolTip="Text" />
                </div>
            </div>--%>
           <%-- <div class="Section NewSection TableSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                    <div class="section-type">Table</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/images/table_big.png" ToolTip="Record Listing" />
                </div>
            </div>--%>

             <div class="Section NewSection CalendarSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                    <div class="section-type">Calendar</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/images/Calendar.png" ToolTip="Calendar" />
                </div>
            </div>
           
            <div class="Section NewSection ImageSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                    <div class="section-type">Image</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="ImageButton3" runat="server" ImageUrl="~/App_Themes/Default/images/picture_big.png"  ToolTip="Image" />
                </div>
            </div>
            <div class="Section NewSection HTMLSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                    <div class="section-type">HTML</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/App_Themes/Default/images/html_big.png" ToolTip="HTML" />
                </div>
            </div>
            <div class="Section NewSection ColumnsSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                    <div class="section-type">Column</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image8" runat="server" ImageUrl="~/App_Themes/Default/images/columns_big.png"  ToolTip="Columns" />
                </div>
            </div>

            <div class="Section NewSection RecordTableSection" ondblclick="EditSection($(this));">
                <div class="Toolbar">
                    <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                    <div class="section-type">Record Table</div>
                </div>
                <div class="Content" style="padding-left: 23px;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/images/Record_table.png" ToolTip="Record Table" />
                </div>
            </div>
          
            <div style="text-align: center; padding-bottom:5px; margin:0px;" runat="server" visible="false" id="divMakeDefaultDashBoard">
                <%--<asp:LinkButton ID="lnkMakeDefaultDashBoard" runat="server" ClientIDMode="Static" OnClick="lnkMakeDefaultDashBoard_Click">
                           Make Default
                </asp:LinkButton>--%>
                <asp:HyperLink runat="server" ID="hlMakeDefaultDashBoard" CssClass="popupdefaultdash" Text="Reset"></asp:HyperLink>
            </div>

        </div>
        <div id="EditAreaContainer">
            <div id="EditArea">
                <asp:Repeater ID="rptSection" runat="server">
                    <ItemTemplate>
                        <div class='Section <%# ((DocGen.DAL.DocumentSectionType)Eval("DocumentSectionType")).SectionType %>Section' id='SECTION_<%# Eval("DocumentSectionID")%>' ondblclick="EditSection($(this))">
                            <div class="Toolbar">
                                <a class="ui-icon ui-icon-carat-2-n-s" title="Drag and drop to change order"></a><a class="ui-icon ui-icon-pencil" onclick="EditSection($(this).parent().parent())" title="Edit section"></a><a class="ui-icon ui-icon-trash" onclick="RemoveSection($(this).parent().parent())" title="Delete section"></a>
                                <div style="float:right"><%# SectionName(((DocGen.DAL.DocumentSectionType)Eval("DocumentSectionType")).SectionType)%></div>
                            </div>
                            <div class="Content">
                                Loading content..
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="ui-widget">
            <div style="margin-top: 20px; padding: 0 .7em;">
                <p>
                    <span style="float: left; margin-right: .3em;" class="ui-icon ui-icon-info"></span><strong>Note:</strong> Double click on section to edit it, drag and drop to re-order.</p>
            </div>
        </div>
        <div  style="width:100%;padding-left:10px;">
           
            <br />
              <table  cellpadding="3" style="min-width: 700px; width: 100%;max-width:1000px;">
                  <tr>
                    <td align="right">
                        <asp:LinkButton ID="lnkViewDashboard"  runat="server" ClientIDMode="Static" OnClick="lnkNoEditDashboard_Click" Visible="false" >
                            View dashboard
                         </asp:LinkButton>
                    </td>
                  </tr>
              </table>
          </div>
    </div>
   
          <div runat="server" id="divNotEditDashboard" style="width:100%;padding-left:10px;">
            <asp:Literal ID="ltReportContent" runat="server"></asp:Literal>
            <br />
              <table id="tblEditDashboard" cellpadding="3" style="min-width: 700px; width: 100%;max-width:1000px;">
                  <tr>
                    <td align="right">
                         <asp:LinkButton ID="lnkEditDashboard"  runat="server" ClientIDMode="Static" OnClick="lnkEditDashboard_Click" Visible="false" >
                            Edit dashboard
                         </asp:LinkButton>
                         
                        
                    </td>
                  </tr>
              </table>
          </div>
          <br />
          <%--<table cellpadding="3" style="min-width: 700px; width: 100%;">
                  <tr>
                    <td align="right">
                     
                         
                    </td>
                  </tr>
              </table>--%>
  
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfDocumentID" Value="0" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfTableCount" Value="" />
    
    <div>
        <div>
            <asp:Button runat="server" ID="btnRefresh" ClientIDMode="Static" Style="display: none;" OnClick="btnRefresh_Click" />
             <asp:HyperLink ID="hlShowDemoTips" ClientIDMode="Static" runat="server" NavigateUrl="~/DemoTips.aspx"
            Style="display: none;"></asp:HyperLink>
            <asp:HyperLink ID="hlShowWelcomeTips" ClientIDMode="Static" runat="server" NavigateUrl="~/Welcome.aspx"
            Style="display: none;"></asp:HyperLink>

            <asp:Button runat="server" ID="btnMakeDefaultDashBoard" ClientIDMode="Static" Style="display: none;" OnClick="btnMakeDefaultDashBoard_Click" />

        </div>
    </div>


    <script type="text/javascript">
        var isBusy = false;
        var editingSection = null;
        var documentID = <%= DocumentID %>;
        var LoaderCounter = 0;
        $(function () {                        
            $("#EditArea > .Section").each(function () {
                LoadSectionContent(this.id.toString().replace('SECTION_', ''));
            });            
            if($("#EditArea > .Section:first").length == 0)
                $("#EditArea").addClass("Empty");
            else
                $("#EditArea").removeClass("Empty");
            InitDragAndDrop();            
            $(".demo div").disableSelection();
        });

        function InitDragAndDrop()
        {
            $("#EditArea, .ZoneInner").sortable({                
                connectWith: "#EditArea, .ZoneInner",
                revert: true,
                dropOnEmpty: true,
                placeholder: "position-indicator", 
                cursorAt: {left:10, top: 10},        
                tolerance: "pointer",       
                //handle: '.ui-icon-carat-2-n-s',
                start: function(e, ui){
                    ui.placeholder.html('<hr/>');
                },
                stop: function (event, ui) {                                        
                    var addedItem = $("#EditArea .NewSection:first");                                                     
                    if (addedItem.length == 1) {                        
                        addedItem.removeClass('NewSection');
                        var prevItem = addedItem.prev();
                        var prevID = 0;
                        if (prevItem.length == 1) {
                            prevID = prevItem.attr('id').replace('SECTION_', '');
                        }                                                
                        EditSection(addedItem, prevID);
                    }
                },
                update: function (event, ui) {                    
                    SaveSectionsOrder();
                }
            });            

            $("#Toolbox .NewSection").draggable({
                connectToSortable: "#EditArea",
                helper: "clone",
                revert: "invalid"
            });
        }

        function EditSection(section, prevID) {
            if (section.hasClass('NewSection') || editingSection != null) {
                return;
            }
           
            editingSection = section;
            if (typeof prevID == 'undefined') {
                prevID = -1;
            }
            var sectionID = '';
            if (typeof section.attr('id') != 'undefined')
                sectionID = section.attr('id').replace('SECTION_', '');
            var iH = 495;
            var iW = 680;            
            var sType = "";
            if (section.hasClass("TextSection")) {
                sType = "Text";
                iW = 950;
                iH = 470;
            }
            else if (section.hasClass("ImageSection")) {
                sType = "Image";
                iW = 1000;
                iH = 600;
            }
            else if (section.hasClass("TableSection")) {
                sType = "Table";
                iW = 1000;
                iH = 700;
            }
              else if (section.hasClass("CalendarSection")) {
                sType = "Calendar";
                iW = 1000;
                iH = 700;
            }
              else if (section.hasClass("RecordTableSection")) {
                  if(sectionID!='')
                  {
                      editingSection=null;
                     alert('Please edit view.')
                      return;
                  }
                   
                sType = "RecordTable";
                iW = 1200;
                iH = 700;
            }
            else if (section.hasClass("HTMLSection")) {
                sType = "HTML";
                iW = 1200;
                iH = 700;
            }
            else if (section.hasClass("DashChartSection")) {
                sType = "DashChart";
                iW = 1100;
                iH = 550;
            }
            else if (section.hasClass("ColumnsSection")) {
                sType = "Columns";
                iW = 350;
                iH = 250;
            }
            else if (section.hasClass("MapSection")) {
                sType = "Map";
                iW = 900;
                iH = 500;
            }
            else if (section.hasClass("DialSection")) {
                sType = "Dial";
                iW = 400;
                iH = 320;
            }

            $.fancybox({
                'padding': 0,
                'autoScale': false,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'title': sType + ' Section Properties',
                'width': iW,
                'height': iH,
                'href':'Pages/DocGen/' + sType + 'Section.aspx?DocumentID=' + document.getElementById('hfDocumentID').value + '&DocumentSectionID=' + sectionID + '&PrevID=' + prevID,
                //'href': 'TextSection.aspx?DocumentSectionID=' + sectionID + '&PrevID=' + prevID,
                //  DocumentID=' + document.getElementById('hfDocumentID').value + 
                'type': 'iframe',
                'titleShow': false ,
                'onClosed': function (currentArray, currentIndex, currentOpts) {
                    setTimeout(function () { RemoveNoAddedSection(); }, currentOpts.speedOut);
                }
            });
        }

        function RemoveSection(section) {
            if (confirm('Are you sure you want to remove this section?')) {
                
                if(section.attr('id')==null)
                {
                    section.remove();
                }
                //alert('not null');
                var id = section.attr('id').toString().replace('SECTION_', '');
                //alert(id);
//                $.ajax({               
//                    url: 'Pages/DocGen/REST/SectionREST.svc/section/delete/' + id,
//                    type: 'POST',
//                    success: function (result) {
//                        if (result) {
//                            section.remove();
//                        }
//                        else {
//                            alert('Error!');
//                        }
//                    },
//                    error: function (a, b, c) {
//                        //                         alert(a.responseText);
//                        section.remove();
//                    }
//                });

                 $.ajax({
                    url: 'Pages/DocGen/REST/SectionREST.ashx?type=section_delete&sectionId=' + id,
                    cache: false,
                    success: function (content) {                    
                       if (content=='False') {
                        alert('Error!');
                       }  
                       else
                       {
                           section.remove();
                           if($("#EditArea > .Section:first").length == 0)
                               $("#EditArea").addClass("Empty");
                           else
                               $("#EditArea").removeClass("Empty");
                       }                          
                                   
                    },
                    error: function (a, b, c) {
                        section.remove();
                        if($("#EditArea > .Section:first").length == 0)
                            $("#EditArea").addClass("Empty");
                        else
                            $("#EditArea").removeClass("Empty");
                    }
                });



            }
        }

//        function LoadSectionContent(id) {
//            var $sectionDiv = $('#SECTION_' + id);
//            $(".Content:first", $sectionDiv).html('Loading content..');
//            $.ajax({
//                url: 'SectionContent.ashx?DocumentSectionID=' + id,
//                cache: false,
//                success: function (content) {
//                    $(".Content:first", $sectionDiv).html(content);
//                }
//            });
//        }

        function SaveSectionsOrder()
        {
            if(editingSection != null) //Section is being edit
            {
                return;
            }
            if(isBusy)
            {
                return;
            }
            else
            {
                isBusy = true;
            }
            var sections = $("#EditArea .Section");
            if(sections.length == 0)
            {
                $("#EditArea").addClass("Empty");
            }
            else
            {
                $("#EditArea").removeClass("Empty");
            }                    
            var OrderedIDs = '';
            var ParentID = '';
            var parentSection;
            var colIndex = '';
            for (var i = 0; i < sections.length; i++) {                        
                var id = sections[i].id.toString().replace('SECTION_', '');                                        
                if (id == ''){   
                    isBusy = false;                         
                    return;
                }
                if($(sections[i]).parent().hasClass('ZoneInner'))
                {
                    colIndex = $(sections[i]).parent().attr('colIndex');                            
                    parentSection = $(sections[i]).parent();
                    while(!parentSection.hasClass('Section'))
                    {
                        parentSection = parentSection.parent();
                    }
                    ParentID = parentSection.attr('id').replace('SECTION_', '');                            
                }
                else
                {
                    colIndex = '';
                    ParentID = '';
                }                
                OrderedIDs += ',' + id + '-' + ParentID + '-' + colIndex; 
            }                    
            
            if (OrderedIDs != '') {
                OrderedIDs = OrderedIDs.substr(1);
                $.fancybox(
		            'Updating sections order',
		            {
		                'autoDimensions': false,
		                'width': 300,
		                'height': 'auto',
		                'transitionIn': 'none',
		                'transitionOut': 'none',
		                'modal': true
		            }
	            );                        
//                $.ajax({                
//                    url: 'Pages/DocGen/REST/SectionREST.svc/sections/displayorder/' + OrderedIDs,
//                    type: 'POST',
//                    success: function (result) {
//                        if (!result) {
//                            alert('Error!');
//                        }
//                        isBusy = false;
//                        setTimeout(function () { $.fancybox.close(); }, 300);
//                    },
//                    error: function (a, b, c) {
//                        isBusy = false;
//                        alert(a.responseText);                                
//                        $.fancybox.close();
//                    }
//                });

                

                $.ajax({
                    url: 'Pages/DocGen/REST/SectionREST.ashx?type=sections_displayorder&sectionIds=' + OrderedIDs,
                    cache: false,
                    success: function (content) {                    
                       if (content=='False') {
                        alert('Error!');
                       }  
                       //alert(content);
                        //setTimeout(function () { $.fancybox.close(); }, 300);  
                        isBusy = false;         
                        $.fancybox.close();
                    },
                    error: function (a, b, c) {
                        alert('Error!');
                        isBusy = false;
                         $.fancybox.close();
                    }
                });



            }                    
        }

        function LoadSectionContent(id) {            
            var $sectionDiv = $('#SECTION_' + id);
            $(".Content:first", $sectionDiv).html('Loading content..');
            var _LoadUrl = 'Pages/DocGen/SectionContent.ashx?DocumentSectionID=' + id + '&Mode=Edit';                    
            
            $.ajax({
                url: _LoadUrl,
                cache: false,
                success: function (content) {                    
                    $(".Content:first", $sectionDiv).html(content);    
                    InitDragAndDrop();              
                },
                error: function (a, b, c) {
                    $(".Content:first", $sectionDiv).html('');
                }
            });
        }

        function SectionUpdated(id) {
            $.fancybox.close();            
            if ((typeof editingSection.attr('id') == 'undefined') || (editingSection.attr('id') == '')) {
                editingSection.attr('id', 'SECTION_' + id);
                editingSection = null;
                SaveSectionsOrder();
            }
            else
            {
                editingSection = null;
            }
            LoadSectionContent(id);            
        }

        function SectionCancelEditting() {
            $.fancybox.close();
            if ((typeof editingSection.attr('id') == 'undefined') || (editingSection.attr('id') == '')) {
                editingSection.remove();
                if($("#EditArea > .Section:first").length == 0)
                    $("#EditArea").addClass("Empty");
                else
                    $("#EditArea").removeClass("Empty");
            }
            editingSection = null;
        }

        function RemoveNoAddedSection() {
            //alert('RemoveNoAddedSection');
            if (editingSection != null && (editingSection.attr('id') == '' || editingSection.attr('id')==null) )
                editingSection.remove();
            editingSection = null;
        }

        function  Readonlymode()
        {
             // $("#EditArea .Toolbar").fadeOut();
              $("#lnkNoEditDashboard").css({ "display": 'none'});
              $("#lnkEditDashboard").css({ "display": 'block'});
              $("#Toolbox").css({ "display": 'none'});
              $("#EditArea .Toolbar").css({ "display": 'none'});
               $("#EditArea div.Section").css({ "border-left-width": '0px',"border":'solid 0px #f8f8f8',"cursor":'default'});
               $("#EditArea, .ZoneInner").sortable('disable');
        }

          function  EditMode()
        {
              //$("#EditArea .Toolbar").fadeIn();
              $("#lnkNoEditDashboard").css({ "display": 'block'});
              $("#lnkEditDashboard").css({ "display": 'none'});
               $("#Toolbox").css({ "display": 'block'});
               $("#EditArea .Toolbar").css({ "display": 'block'});
                $("#EditArea div.Section").css({ "border":'solid 1px #f8f8f8',"margin":'10px 0px', "padding":'0px',"position":'relative',"min-height":'50px',"border-radius":'5px 0px 0px 5px',"background-color":'#FFFFFF',"cursor":'move'});
                 $("#EditArea, .ZoneInner").sortable('enable');
                  $("#EditArea div.Section:hover").css({ "border-color": '#e6e6e6',"background-color":'#FCFBED'});
                  // $("#EditArea div.Section > .Toolbar").css({ "display": 'none'});
                  // $("#EditArea div.Section:hover > .Toolbar").css({ "display": 'block'});
        }


    </script>


    <script type="text/javascript">
        // Return a helper with preserved width of cells

        

        var fixHelper = function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });
            return ui;
        };

        function DynamicSortingInit() {
            $("#divSections tbody").sortable({
                helper: fixHelper
            }).disableSelection();
            $("#DynamicSortingToolbar").show();
        }

        function HasSelectedItem() {
            var selected = ($("#divSections td input:checked:first").length == 1);
            if (!selected)
                alert('No section is selected!');
            return selected;
        }

        function OpenFirstSection() {
            setTimeout(function () { OpenFirstText(); }, 100);

        }

        function OpenFirstText() {
            $("#hlText").trigger('click');
        }


        function OpenDemoTips() {
            $("#hlShowDemoTips").trigger('click');
        }

        function OpenWelcomeTips() {
            $("#hlShowWelcomeTips").trigger('click');
        }

        $(function () {
            $("#hlShowDemoTips").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                'transitionIn': 'elastic',
                'transitionOut': 'none',
                width: 700,
                height: 550,
                titleShow: false,
//                onClosed: function (currentArray, currentIndex, currentOpts) {
//                    setTimeout(function () { OpenAlertBox(); }, currentOpts.speedOut);
//                }
            });
        });

        $(function () {
            $("#hlShowWelcomeTips").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                'transitionIn': 'elastic',
                'transitionOut': 'none',
                width: 500,
                height: 250,
                titleShow: false,
                onClosed: function (currentArray, currentIndex, currentOpts) {
//                    setTimeout(function () { OpenAlertBox(); }, currentOpts.speedOut);
                if(document.getElementById('hfTableCount').value=='')
                {
                    window.location.href='/Pages/Record/TableOption.aspx?FirstTime=yes&MenuID=kdUxjBEM5oo=&SearchCriteria=kdUxjBEM5oo=';
                }
                }
            });
        });
        

    </script>


</asp:Content>
