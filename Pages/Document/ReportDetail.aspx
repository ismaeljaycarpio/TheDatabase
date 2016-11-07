<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="ReportDetail.aspx.cs" Inherits="DocGen.DocumentNS.ReportDetail.Edit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>
    <asp:Literal ID="ltCommonStyles" runat="server">
      <%--  <style type="text/css">
            h1, h2, h3, h4, h5, h6, p, span, label, li, th, td
            {
                font-family: "Helvetica Neue" , "Lucida Grande" , "Segoe UI" ,Arial,Helvetica,Verdana,sans-serif;
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
            }
            table.TableSection th
            {
                background-color: #f8f8f8;
            }
            
            .MsoTocHeading
            {
                font-family: "Helvetica Neue" , "Lucida Grande" , "Segoe UI" ,Arial,Helvetica,Verdana,sans-serif;
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
                font-family: "Calibri" , "sans-serif";
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
        </style>--%>
    </asp:Literal>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" >
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <table style="width: 100%; text-align: center">
                            <tr>
                                <td>
                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                </td>
                            </tr>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
        </tr>
        <tr>
            <td valign="top" style="width:50px;">
            </td>
            <td valign="top" align="left">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" ValidationGroup="MKE"
                                ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct the following errors:" />
                            <asp:HiddenField runat="server" ID="hfDocumentTypeID" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail">
                                <table >
                                    <tr>
                                        <td valign="top" style="padding-top:7px;" align="left" >
                                            
                                            <%--<fieldset style="border-color: #8F9C9E;">--%>
                                           
                                                <table cellpadding="3">

                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList runat="server" ID="optReportType" 
                                                                RepeatDirection="Horizontal" AutoPostBack="true" 
                                                                onselectedindexchanged="optReportType_SelectedIndexChanged">
                                                                <asp:ListItem Text="Standard Report" Value="standard" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="SQL Server Report" Value="ssrs"></asp:ListItem>
                                                                 <asp:ListItem Text="Web Page" Value="webpage"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" id="trCopyReports">
                                                        <td align="right" runat="server" id="tdCopyReports" >
                                                            <strong>Copy from existing </strong>
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox runat="server" ID="chkCopyFromExisting" CssClass="NormalTextBox" ClientIDMode="Static" />
                                                                    </td>
                                                                    <td style="padding-left: 3PX;">
                                                                        <asp:DropDownList runat="server" ID="ddlReports" CssClass="NormalTextBox" AutoPostBack="true"
                                                                            ClientIDMode="Static" DataTextField="DocumentText" DataValueField="DocumentID"
                                                                            OnSelectedIndexChanged="ddlReports_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trReportName">
                                                        <td align="right">
                                                            <strong>Name</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDocumentText" CssClass="NormalTextBox" ClientIDMode="Static"
                                                                Width="450px"></asp:TextBox>

                                                            <asp:RequiredFieldValidator ID="rfvDocumentText" runat="server" ControlToValidate="txtDocumentText"
                                                                CssClass="failureNotification" ErrorMessage="Document name is required." ToolTip="Document name is required."
                                                                ValidationGroup="MKE">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trMenu">
                                                        <td align="right">
                                                            <strong>Menu:</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="false" DataTextField="MenuP"
                                                                Width="155px" DataValueField="MenuID" CssClass="NormalTextBox">
                                                            </asp:DropDownList>
                                                                                 <asp:HiddenField runat="server" ID="hfOldMenuID" />
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trTable" visible="false">
                                                        <td align="right">
                                                            <strong runat="server" id="stgTableCap">Table</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                                CssClass="NormalTextBox">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" id="tr1">
                                                        <td align="right" valign="top">
                                                            <strong runat="server" id="stgDescription">Description</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDocumentDescription" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                                                ClientIDMode="Static" Width="450px" Height="60px"></asp:TextBox>


                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trReportType" visible="false">
                                                        <td align="right">
                                                            <strong>Report Type</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlDocumentType" AutoPostBack="false" ClientIDMode="Static"
                                                                DataValueField="DocumentTypeID" DataTextField="DocumentTypeName" CssClass="NormalTextBox">
                                                            </asp:DropDownList>
                                                            <asp:HyperLink runat="server" ID="hlDocumentTypeEdit" Text="Edit" NavigateUrl="~/Pages/Document/DocumentType.aspx"></asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trDateStart">
                                                        <td align="right" style="width:130px;">
                                                            <strong>Report Start Date </strong>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="NormalTextBox" 
                                                                Width="100px"  BorderStyle="Solid"
                                                                BorderColor="#909090" BorderWidth="1"></asp:TextBox>
                                                              <asp:ImageButton runat="server" ID="ibStartDate"  ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false"/>  

                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                                Format="dd/MM/yyyy" PopupButtonID="ibStartDate" FirstDayOfWeek="Monday">
                                                            </ajaxToolkit:CalendarExtender>
                                                           
                                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtStartDate"
                                                                ValidationGroup="MKE" ErrorMessage="Invalid Start date" Font-Bold="true" Display="Dynamic" Type="Date"
                                                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator> 
                                                              <ajaxToolkit:TextBoxWatermarkExtender ID="tbwStartDate" TargetControlID="txtStartDate" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trDateEnd">
                                                        <td align="right">
                                                            <strong>Report End Date</strong>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="NormalTextBox" 
                                                                Width="100px"  BorderStyle="Solid"
                                                                BorderColor="#909090" BorderWidth="1"></asp:TextBox>
                                                             <asp:ImageButton runat="server" ID="ibEndDate"  ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false"/>  

                                                            <asp:CompareValidator ID="EndDateCompare" runat="server" Type="Date" ControlToValidate="txtEndDate"
                                                                ControlToCompare="txtStartDate" Operator="GreaterThanEqual"  ToolTip="End date must be after the start date"
                                                                ErrorMessage="End date must be after the start date" CssClass="failureNotification"
                                                                ValidationGroup="MKE">*</asp:CompareValidator>
                                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtEndDate"
                                                                Format="dd/MM/yyyy" PopupButtonID="ibEndDate" FirstDayOfWeek="Monday">
                                                            </ajaxToolkit:CalendarExtender>
                                                           
                                                            <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtEndDate"
                                                                ValidationGroup="MKE" ErrorMessage="Invalid End date" Font-Bold="true" Display="Dynamic" Type="Date"
                                                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtEndDate" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>
                                                           
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trPublished">
                                                        <td></td>
                                                        <td align="left">
                                                            <asp:CheckBox  runat="server" ID="chkPublished" TextAlign="Right" CssClass="NormalTextBox" Text="Publish" />
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trFileName" visible="false">
                                                        <td align="right" valign="top">
                                                            <asp:Label runat="server" ID="lblFileNameCaption" Text="Published address" Visible="true"
                                                                Font-Bold="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblPublishedAddress" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" id="trEditor" visible="false">
                                                          <td align="right" valign="top">
                                                          <strong>Editor</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblEditor" ></asp:Label>
                                                        </td>
                                                    </tr>

                                                </table>
                                            <%--</fieldset>--%>
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <%--<td valign="top" runat="server" id="trStyle">
                                            <fieldset style="border-color: #8F9C9E;">
                                                <legend><strong>Style</strong></legend>
                                                <table cellpadding="3">
                                                    <tr>
                                                        <td align="left">

                                                            <table cellpadding="3">
                                                                <tr>
                                                                    <td>
                                                                        <strong>Style</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlStyle" runat="server" CssClass="NormalTextBox" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlStyle_SelectedIndexChanged" >
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:HyperLink runat="server" ID="hlEdit" NavigateUrl="~/Pages/DocGen/DocumentStyleList.aspx"
                                                                            Target="_blank">Edit</asp:HyperLink>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            
                                                        
                                                           
                                                             
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                                  <strong>Definition</strong>   <br />
                                                             <asp:TextBox ID="txtStyle" Width="150px" runat="server" CssClass="MultiLineTextBox" TextMode="MultiLine" Rows="10"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>


                                            </fieldset>
                                        </td>--%>
                                    </tr>
                                </table>
                               
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                            <div>
                                <table>
                                    <tr>
                                       
                                        <td style="width:5px;"></td>
                                        <td>
                                            <div runat="server" id="divBack">
                                               
                                                    <asp:HyperLink runat="server" ID="hlBack"  
                                                    CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divClose" visible="false">
                                                
                                                            <asp:LinkButton runat="server" ID="HyperLink1"  CssClass="btn"
                                                                OnClientClick="parent.$.fancybox.close();return false;"> <strong>Close</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>

                                         <td style="width:5px;"></td>
                                        <td>
                                            <div runat="server" id="div1">
                                               
                                                    <asp:HyperLink runat="server" ID="hlSchedule"  NavigateUrl="~/Pages/Schedule/ScheduleReport.aspx" 
                                                    CssClass="btn"> <strong>Schedule</strong> </asp:HyperLink>
                                                       
                                            </div>
                                        </td>

                                         <td style="width:5px;"></td>

                                          <td>
                                            <div runat="server" id="divSave">
                                               
                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                    CausesValidation="true" ValidationGroup="MKE"> <strong>Save</strong></asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                         <%--<td>
                                            <div runat="server" id="divStyle" >
                                                
                                                           <asp:HyperLink runat="server" ID="hlStyles" CssClass="btn"
                                                            NavigateUrl="~/Pages/DocGen/DocumentStyleList.aspx" Target="_blank"> <strong>Styles</strong> </asp:HyperLink>
                                                       
                                            </div>
                                        </td>--%>

                                    </tr>
                                </table>
                            </div>
                            <br />
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlReports" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
    </table>
    <script type="text/javascript">


        function SavedAndRefresh() {

            parent.$.fancybox.close();

        }

        function ShowHideReportDDL() {

            var chk = document.getElementById('chkCopyFromExisting');
            if (chk.checked) {
                $('#ddlReports').fadeIn();
            }
            else {
                $('#ddlReports').fadeOut();
            }

        }



    </script>
</asp:Content>
