﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="Report.aspx.cs" Inherits="Pages_Document_Report" EnableEventValidation="false" %>

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




        function MouseEvents(objRef, evt) {
            var checkbox = objRef.getElementsByTagName("input")[0];



            if (evt.type == "mouseover") {
                objRef.style.backgroundColor = "#76BAF2";
                objRef.style.cursor = 'pointer';
            }
            else {
                //            if (checkbox == null) {
                //                return;
                //            }

                if (checkbox != null && checkbox.checked) {
                    objRef.style.backgroundColor = "#96FFFF";
                }
                else if (evt.type == "mouseout") {
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
                <tr>
                    <td colspan="3" height="40">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <span class="TopTitle">Reports</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                                <td style="width: 200px;">
                                    <table>
                                        <tr>
                                           
                                            <td>
                                                <%--<asp:HyperLink runat="server" ID="hlNewReport" ToolTip="New Report">
                                                    <asp:Image runat="server" ID="imgNewReport" ImageUrl="~/App_Themes/Default/images/CreateReport.png"  />
                                                </asp:HyperLink>--%>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%--<asp:HyperLink runat="server" ID="hplNewData" ToolTip="Upload Document">
                                                    <asp:Image runat="server" ID="imgAddNewRecord" SkinID="Upload" />
                                                </asp:HyperLink>--%>
                                            </td>
                                             <td style="padding-left:100px;">
                                                <asp:HyperLink runat="server" ID="hlBack" CssClass="ButtonLink" CausesValidation="false">
                                                    <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                                </asp:HyperLink>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
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
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr runat="server" visible="false">
                                        <td align="right">
                                               <strong>Table</strong>
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                CssClass="NormalTextBox">
                                            </asp:DropDownList>

                                            <asp:DropDownList Visible="false" runat="server" ID="ddlDocumentType" AutoPostBack="true" DataValueField="DocumentTypeID"
                                                CssClass="NormalTextBox" DataTextField="DocumentTypeName" OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:HyperLink Visible="false" runat="server" ID="hlDocumentTypeEdit" Text="Edit" NavigateUrl="~/Pages/Document/DocumentType.aspx"></asp:HyperLink>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                           
                                        </td>
                                        <td>
                                            
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            
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
                                            <asp:TextBox runat="server" ID="txtDateFrom"  Width="100px" ValidationGroup="MKE"
                                                BorderStyle="Solid" BorderColor="#909090"
                                                BorderWidth="1" />
                                                <asp:ImageButton runat="server" ID="ibDateFrom"  ImageUrl="~/Images/Calendar.png"  AlternateText="Click to show calendar" CausesValidation="false"/>
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                Format="dd/MM/yyyy" PopupButtonID="ibDateFrom" FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            
                                            <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>

                                              <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtDateFrom" WatermarkText="dd/mm/yyyy"
                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>

                                            To
                                            <asp:TextBox runat="server" ID="txtDateTo"  Width="100px" CssClass="NormalTextBox"
                                                ValidationGroup="MKE"   BorderStyle="Solid"
                                                BorderColor="#909090" BorderWidth="1" />
                                                <asp:ImageButton runat="server" ID="ibDateTo"  ImageUrl="~/Images/Calendar.png"  AlternateText="Click to show calendar" CausesValidation="false"/>
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                                Format="dd/MM/yyyy" PopupButtonID="ibDateTo" FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                           
                                            <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                                                ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1753"
                                                MaximumValue="1/1/3000"></asp:RangeValidator>
                                            
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtDateTo" WatermarkText="dd/mm/yyyy"
                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>

                                        </td>
                                        <td style="width: 10px;">
                                        </td>
                                        <td align="right">
                                            <strong>Search</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDocumentText" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>

                                         <td style="width: 10px;">
                                        </td>
                                        <td align="right">
                                            <strong>Menu</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="true" DataTextField="MenuP"
                                                 OnSelectedIndexChanged="ddlMenu_OnSelectedIndexChanged" Width="155px" DataValueField="MenuID" CssClass="NormalTextBox">
                                                            </asp:DropDownList>
                                        </td>

                                        <td>
                                        </td>
                                        <td align="left">
                                            <div>
                                                
                                                    <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" ValidationGroup="MKE"  CausesValidation="true"
                                                    OnClick="lnkSearch_Click"> <strong>Go </strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </asp:Panel>
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                            AllowSorting="True" DataKeyNames="DocumentID" HeaderStyle-ForeColor="Black" Width="100%"
                            AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting" OnRowDataBound="gvTheGrid_RowDataBound"
                             OnPreRender="gvTheGrid_PreRender"  AlternatingRowStyle-BackColor="#DCF2F0">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("DocumentID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlView" runat="server" ToolTip="View" NavigateUrl='<%# GetFileURL(Eval("DocumentID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconShow.png" Target="_blank" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditHyperLink" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL(Eval("DocumentID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DocumentText" HeaderText="Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlView2" runat="server" NavigateUrl='<%# GetFileURL(Eval("DocumentID").ToString())  %>'
                                            Text='<%# Eval("DocumentText")%>' />
                                        <%--<asp:Label ID="lblDocumentText" runat="server" Text='<%# Eval("DocumentText") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type" Visible="false" >
                                     <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentType" runat="server" Text='<%# Eval("DocumentTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DocumentDescription" HeaderText="Description">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentDescription" runat="server" Text='<%# Eval("DocumentDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderText="Menu">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentDate" runat="server" Text='<%# GetMenu(Eval("DocumentID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="DocumentDate" SortExpression="DocumentDate" HeaderText="Date"
                                    DataFormatString="{0:d}" ReadOnly="true" />--%>
                                <%--<asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlFileURL" runat="server" NavigateUrl='<%# GetFileURL(Eval("DocumentID").ToString())  %>'
                                            Target="_blank" Text='View' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" OnExportForCSV="Pager_OnExportForCSV"
                                    HideAdd="false" OnDeleteAction="Pager_DeleteAction" OnApplyFilter="Pager_OnApplyFilter"
                                    OnBindTheGridToExport="Pager_BindTheGridToExport" OnBindTheGridAgain="Pager_BindTheGridAgain" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">                            
                                No records have been added yet. <br /> <%--<strong style="text-decoration: none; color: Blue;">
                                   Upload document now</strong>--%>
                            <%--<asp:HyperLink runat="server" ID="hpNew" Style="text-decoration: none; color: Black;" ToolTip="Upload" >                                
                            </asp:HyperLink>--%>   
                            <strong style="text-decoration: none; color: Blue;">
                                  or create report now</strong>
                            <asp:HyperLink runat="server" ID="hpNew2" Style="text-decoration: none; color: Black;" ToolTip="New Report">                               
                            </asp:HyperLink>
                        </div>
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                No records match that filter. <strong style="text-decoration: none; color: Blue;">
                                    Clear filter</strong>
                            </asp:LinkButton> <br />
                            or <%--<strong style="text-decoration: none; color: Blue;">
                                    Upload document now</strong>--%>
                            <%--<asp:HyperLink runat="server" ID="hplNewDataFilter" Style="text-decoration: none;
                                color: Black;" ToolTip="Upload"> 
                            </asp:HyperLink>--%>
                             <strong style="text-decoration: none; color: Blue;">
                                  or create report now</strong>
                             <asp:HyperLink runat="server" ID="hplNewDataFilter2" Style="text-decoration: none;
                                color: Black;" ToolTip="New Report"> 
                            </asp:HyperLink>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                        <asp:HiddenField runat="server" ID="hfCRDocumentTypeID" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
