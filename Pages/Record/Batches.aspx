<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="Batches.aspx.cs" Inherits="Pages_Record_Batches" EnableEventValidation="false" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

  <script language="javascript" type="text/javascript">
      function abc() {
          var b = document.getElementById('<%= lnkSearch.ClientID %>');
          if (b && typeof (b.click) == 'undefined') {
              b.click = function () {
                  var result = true;
                  if (b.onclick) result = b.onclick();
                  if (typeof (result) == 'undefined' || result) {
                      eval(b.getAttribute('href'));
                  }
              }
          }

      }


//      function setYearRange() {
//          $find("calBehaviorTo").set_selectedDate(new Date(1950, 1, 1));
//      }

    </script>


     <script type="text/javascript">
         function MouseEvents(objRef, evt) {
             if (evt.type == "mouseover") {
                 objRef.style.backgroundColor = "#76BAF2";
                 objRef.style.cursor = 'pointer';
             }
             else {

                 if (evt.type == "mouseout") {
                     if (objRef.rowIndex % 2 == 0) {
                         //Alternating Row Color
                         objRef.style.backgroundColor = "white";
                     }
                     else {
                         objRef.style.backgroundColor = "#DCF2F0";
                     }
                 }
             }
         }



    </script>

    <div style="padding-left:20px;padding-right:20px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
                <tr>
                    <td colspan="3">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <asp:Label runat="server" ID="lblAdminArea" CssClass="TopTitle" Text="Admin Area:"></asp:Label>
                                    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlAdminArea" CssClass="TopTitle"
                                        OnSelectedIndexChanged="ddlAdminArea_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
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
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                    </td>
                    <td valign="top">
                         <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                        <div id="search"  class="searchcorner" onkeypress="abc();">
                            <br />
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                HeaderText="List of validation errors" />
                            <table style="border-collapse: collapse" cellpadding="4">
                                <tr>
                                    <td align="right">
                                        <strong runat="server" id="stgTable">Table</strong>
                                    </td>
                                    <td>
                                       
                                        <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                            DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                            CssClass="NormalTextBox">
                                        </asp:DropDownList>
                                        <br />
                                    </td>
                                    <td align="right">
                                        <strong>Date Uploaded</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtDateFrom"  Width="100px" CssClass="NormalTextBox"  
                                            ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                            <asp:ImageButton runat="server" ID="imgDateForm"  ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar"/>
                                        <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                            Format="dd/MM/yyyy" PopupButtonID="imgDateForm" FirstDayOfWeek="Monday">
                                        </ajaxToolkit:CalendarExtender>
                                       
                                        <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                            ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                            MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                          <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtDateFrom" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>

                                        To
                                        <asp:TextBox runat="server" ID="txtDateTo"  Width="100px" CssClass="NormalTextBox"  
                                            ValidationGroup="MKE" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                             <asp:ImageButton runat="server" ID="imgDateTo"  ImageUrl="~/Images/Calendar.png"  AlternateText="Click to show calendar"/>
                                        <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                            Format="dd/MM/yyyy" PopupButtonID="imgDateTo"   FirstDayOfWeek="Monday" >
                                        </ajaxToolkit:CalendarExtender>
                                       
                                        <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                                            ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1753"
                                            MaximumValue="1/1/3000"></asp:RangeValidator>
                                          <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="txtDateTo" WatermarkText="dd/mm/yyyy"
                                                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <strong>Search</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSearch" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <div>
                                            
                                            <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"
                                                ValidationGroup="MKE"> <strong>Go</strong></asp:LinkButton>
                                                  
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                        </asp:Panel>
                        
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                             HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                            AllowPaging="True" AllowSorting="True" DataKeyNames="BatchID" HeaderStyle-ForeColor="Black"
                            Width="100%" AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting"
                            OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound"
                             AlternatingRowStyle-BackColor="#DCF2F0">
                            <PagerSettings Position="Top" />
                            <Columns>
                               
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("BatchID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL() +  Cryptography.Encrypt(Eval("BatchID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Table" SortExpression="TableName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTableName"  runat="server" Text='<%# Eval("TableName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Full Name" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserName" Width="130px" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date Uploaded" SortExpression="Batch.DateAdded" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateAdded" Width="130px" runat="server" Text='<%# Eval("DateAdded") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch" SortExpression="BatchDescription">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchDescription" runat="server" Text='<%# Eval("BatchDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File" SortExpression="UploadedFileName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUploadedFileName" runat="server" Text='<%# Eval("UploadedFileName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valid" HeaderStyle-ForeColor="Green" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblValidCount" runat="server" Text='<%# Eval("ValidCount") %>' ForeColor="Green"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warning" HeaderStyle-ForeColor="Blue" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblWarningCount" runat="server" Text='<%# Eval("WarningCount") %>'
                                            ForeColor="Blue"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invalid" HeaderStyle-ForeColor="Red" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblNotValidCount" runat="server" Text='<%# Eval("NotValidCount") %>'
                                            ForeColor="Red"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Imported" SortExpression="IsImported" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsImported" runat="server" Text='<%# Eval("IsImported") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Positional"  ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsImportPositional" runat="server" Text='<%# Eval("IsImportPositional") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Template"  ItemStyle-HorizontalAlign="Center"   >
                                    <ItemTemplate>
                                        <asp:Label ID="lblImportTemplateName" runat="server" Text='<%# Eval("ImportTemplateName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" HideAdd="true" HideDelete="true" OnExportForCSV="Pager_OnExportForCSV"
                                    OnApplyFilter="Pager_OnApplyFilter" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                    OnBindTheGridAgain="Pager_BindTheGridAgain" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                    Clear filter.</strong>
                            </asp:LinkButton>
                        </div>
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                        <br />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
        </Triggers>
    </asp:UpdatePanel>
    </div>

    
</asp:Content>
