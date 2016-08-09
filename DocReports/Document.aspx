<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Marketing2.master" AutoEventWireup="true"
    CodeFile="Document.aspx.cs" Inherits="Pages_Reports_Document" EnableEventValidation="false" %>

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
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                <tr>
                    <td colspan="2" height="40">
                       
                             <table width="100%" cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left"  style="width:50%;">
                                         <span class="TopTitle">Documents and Reports</span>
                                    </td>
                                    <td align="left">
                                        <div style="width:40px; height:40px;">
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                                </tr>
                            </table>


                    </td>
                    <td>
                      
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
                            <div id="search" style="padding-bottom: 10px" onkeypress="abc();">
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr>
                                        <td>
                                            <strong>Table</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <strong>Document Type</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlDocumentType" AutoPostBack="true" DataValueField="DocumentTypeID"
                                                CssClass="NormalTextBox" DataTextField="DocumentTypeName" OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="padding-left: 100px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Date</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDateFrom" autocomplete="off" Width="100px" ValidationGroup="MKE" Height="18px"
                                                MaxLength="1" Font-Size="11px" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                Format="dd/MM/yyyy"  FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meeDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="NormalTextBox"
                                                OnInvalidCssClass="NormalTextBox" MaskType="Date" AcceptNegative="None" ErrorTooltipEnabled="True"
                                                CultureName="en-GB" />
                                            <ajaxToolkit:MaskedEditValidator ID="mevDateFrom" runat="server" ControlExtender="meeDateFrom"
                                                ControlToValidate="txtDateFrom" InvalidValueMessage="Date is invalid" Display="Dynamic"
                                                InvalidValueBlurredMessage="*" IsValidEmpty="true" ValidationGroup="MKE" />
                                            <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1911" MaximumValue="1/1/3000"></asp:RangeValidator>
                                            To
                                            <asp:TextBox runat="server" ID="txtDateTo" autocomplete="off" Width="100px" CssClass="NormalTextBox" Height="18px"
                                                ValidationGroup="MKE" MaxLength="1" Font-Size="11px" BorderStyle="Solid" BorderColor="#909090" BorderWidth="1" />
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                                Format="dd/MM/yyyy"  FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meeDateTo" runat="server" TargetControlID="txtDateTo"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="NormalTextBox"
                                                OnInvalidCssClass="NormalTextBox" MaskType="Date" AcceptNegative="None" ErrorTooltipEnabled="True"
                                                CultureName="en-GB" />
                                            <ajaxToolkit:MaskedEditValidator ID="Maskededitvalidator1" runat="server" ControlExtender="meeDateTo"
                                                ControlToValidate="txtDateTo" InvalidValueMessage="Date is invalid" Display="Dynamic"
                                                InvalidValueBlurredMessage="*" IsValidEmpty="true" ValidationGroup="MKE" />
                                            <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                                                ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1911"
                                                MaximumValue="1/1/3000"></asp:RangeValidator>
                                        </td>
                                        <td style="width: 10px;">
                                        </td>
                                        <td align="right">
                                            <strong>Search</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDocumentText" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <div>
                                               
                                                    <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" 
                                                    OnClick="lnkSearch_Click"> <strong>Go </strong> </asp:LinkButton>
                                                  
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                            AllowSorting="True" DataKeyNames="DocumentID" HeaderStyle-ForeColor="Black" Width="100%"
                            AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting" OnPreRender="gvTheGrid_PreRender">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <%--<asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("DocumentID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <%--<ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL(Eval("DocumentID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DocumentText" HeaderText="Document Name">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# GetFileURL(Eval("DocumentID").ToString())  %>'
                                            Target="_blank" Text='<%# Eval("DocumentText")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DocumentDate" SortExpression="DocumentDate" HeaderText="Date"
                                    DataFormatString="{0:d}" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Document Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentType" runat="server" Text='<%# Eval("DocumentTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlFileURL" runat="server" NavigateUrl='<%# GetFileURL() + Eval("UniqueName").ToString()  %>'
                                            Target="_blank" Text='View' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" OnExportForCSV="Pager_OnExportForCSV" HideDelete="true"
                                    HideAdd="true" OnDeleteAction="Pager_DeleteAction" OnApplyFilter="Pager_OnApplyFilter"
                                    OnBindTheGridToExport="Pager_BindTheGridToExport" OnBindTheGridAgain="Pager_BindTheGridAgain" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />                        
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                No records match that filter. <strong style="text-decoration: underline; color: Blue;">
                                    Clear filter</strong>
                            </asp:LinkButton>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
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
</asp:Content>
