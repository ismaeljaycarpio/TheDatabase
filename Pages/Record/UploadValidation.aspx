<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="UploadValidation.aspx.cs" Inherits="Pages_Record_UploadValidation"
    EnableEventValidation="false" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <%--<link href="<% =ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css") %>" rel="stylesheet"    type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>--%>

    <script type="text/javascript" language="javascript">


        function RefreshImport() {
            $("#btnHiddenRefresh").trigger('click');
        }


        function RefreshImportHeader() {
            $("#btnHiddenRefreshHeader").trigger('click');
        }


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

    <div style="padding-left: 20px; padding-right: 20px;">
        <table border="0" cellpadding="0" cellspacing="0" align="center" width="100%">
            <tr>
                <td colspan="3" height="40">
                    <span class="TopTitle">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <asp:Label ID="lblTitle" runat="server" Text="Data Validation"></asp:Label>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server">
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
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="3" height="13"></td>
            </tr>
            <tr>
                <td valign="top"></td>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div>
                                <div id="search" style="padding-bottom: 10px" runat="server" visible="true">
                                    <table>
                                        <tr>
                                            <td>
                                                <table style="border-collapse: collapse" cellpadding="3">
                                                    <tr>
                                                        <td align="right">
                                                            <strong runat="server" id="stgTableCap">Table</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblTable"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trImportTemplate" visible="false">
                                                        <td align="right">
                                                            <strong runat="server" id="Strong1">Template</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblImportTemplate"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Batch Description</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblBatch"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>File</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblFile"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Positional</strong>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblPositional"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="padding-left: 20px;">
                                                                        <%--<strong>Add missing Locations</strong>--%>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:Label runat="server" ID="lblMissingSS"></asp:Label>--%>
                                                                    </td>
                                                                    <td align="right" style="padding-left: 20px;">
                                                                        <strong>Imported</strong>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblIsImported"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <%--<table runat="server" id="tblAllowDataUpdate" visible="false">
                                                                            <tr>
                                                                                <td></td>
                                                                                <td align="right" style="padding-left: 20px;">
                                                                                    <strong>Data Update</strong>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label runat="server" ID="lblDataUpdate" Text="No"></asp:Label>
                                                                                </td>

                                                                            </tr>

                                                                        </table>--%>

                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellspacing="3">
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label runat="server" ID="lblValidData" ForeColor="Green" Text="Valid Data: 0"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                   
                                                                    <td style="padding-left:10px;">
                                                                        <asp:Label runat="server" ID="lblWarning" ForeColor="Blue" Text="Valid with Warnings: 0" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                   <!-- 
                                                                    <td style="padding-left:10px;" runat="server" id="tdExceedance">
                                                                        <asp:Label runat="server" ID="lblExceedance" ForeColor="Orange" Text="Exceedances: 0" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                   -->
                                                                    <td style="padding-left:10px;">
                                                                        <asp:Label runat="server" ID="lblInvalidData" ForeColor="Red" Text="Invalid Data: 0"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 30px;"></td>
                                                                    <td>
                                                                        <%--<asp:LinkButton runat="server" ID="lnkImportNow2" Text="Import Now" OnClick="lnkImportNow2_Click"
                                                                        Font-Bold="true"></asp:LinkButton>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 100px;"></td>
                                            <td>
                                                <table>
                                                    <tr runat="server" id="trColumnHeader">
                                                        <td align="right">
                                                            <strong>Column Header Row</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtImportColumnHeaderRow" CssClass="NormalTextBox"
                                                                Width="75px" Text="1" onblur="RefreshImportHeader()"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Data Start Row</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtImportDataStartRow" CssClass="NormalTextBox"
                                                                Width="75px" Text="2" onblur="RefreshImport()"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr style="height: 60px;">
                                                        <td align="right" style="padding-left: 80px; width: 90px;">

                                                            <asp:HyperLink runat="server" ID="hlBack" ToolTip="Goes back to the previous screen and does not change the temp records"
                                                                CssClass="btn"> <strong>Back</strong>  </asp:HyperLink>

                                                        </td>
                                                        <td runat="server" id="trRejectSubmit" visible="false">
                                                            <div runat="server" id="divReject" visible="false">
                                                                <asp:LinkButton runat="server" ID="lnkReject" ToolTip="Deletes the temp records and the batch"
                                                                    CssClass="btn" CausesValidation="true"
                                                                    OnClick="lnkReject_Click"> <strong>Reject</strong> </asp:LinkButton>
                                                            </div>

                                                            <div runat="server" id="divSubmit" visible="false">
                                                                <asp:LinkButton runat="server" ID="lnkSubmit" CssClass="btn" CausesValidation="true"
                                                                    OnClick="lnkSubmit_Click"> <strong>Submit</strong> </asp:LinkButton>
                                                            </div>
                                                        </td>
                                                        <td style="width: 110px;">
                                                            <asp:LinkButton runat="server" ID="lnkRevalidate" CssClass="btn" CausesValidation="true" ToolTip="Attempts to validate the records again"
                                                                OnClick="lnkRevalidate_Click"> <strong>Revalidate</strong> </asp:LinkButton>

                                                        </td>
                                                        <td align="left" style="padding-left: 5px; width: 100px;">
                                                            <div runat="server" id="divImport">
                                                                <asp:LinkButton ToolTip="Imports the valid records" runat="server" ID="lnkImport" CssClass="btn" CausesValidation="true"
                                                                    OnClick="lnkImport_Click"> <strong>Import</strong> </asp:LinkButton>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                </table>

                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button Style="display: none;" runat="server" ID="btnHiddenRefresh" ClientIDMode="Static"
                                        OnClick="btnHiddenRefresh_Click" CausesValidation="true"></asp:Button>
                                    <asp:Button Style="display: none;" runat="server" ID="btnHiddenRefreshHeader" ClientIDMode="Static"
                                        OnClick="btnHiddenRefreshHeader_Click" CausesValidation="true"></asp:Button>
                                </div>
                            </div>
                            <div style="background-color: Green; font-size: 14px; width: 100%; color: White;">
                                <strong id="stValid" name="stValid">Valid with Warnings</strong>
                            </div>
                            <div>
                                <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                                    HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                    AllowSorting="True" DataKeyNames="DBGSystemRecordID" HeaderStyle-ForeColor="Black"
                                    Width="100%" AutoGenerateColumns="true" PageSize="15" OnSorting="gvTheGrid_Sorting"
                                    OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                                    <PagerSettings Position="Top" />
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("DBGSystemRecordID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridview_header" />
                                    <RowStyle CssClass="gridview_row" />
                                    <PagerTemplate>
                                        <asp:GridViewPager runat="server" ID="Pager" HideFilter="true" TableName="Recordlist"
                                            HideAdd="true" HideDelete="true" OnBindTheGridToExport="Pager_BindTheGridToExport"
                                            OnBindTheGridAgain="Pager_BindTheGridAgain" OnExportForCSV="Pager_OnExportForCSV" />
                                    </PagerTemplate>
                                </dbg:dbgGridView>
                            </div>
                            <div runat="server" id="divEmptyData" visible="false">
                                <strong>No data. </strong>
                            </div>
                            <br />
                            <div style="background-color: Blue; font-size: 14px; width: 100%; color: White;">
                                <strong id="stWarnings" name="stWarnings">Valid with Warnings</strong>
                                <asp:Image runat="server" ImageUrl="~/Images/warning.png" AlternateText="This data will be imported"
                                    ToolTip="This data will be imported" />
                            </div>
                            <div>
                                <dbg:dbgGridView ID="gvWarning" runat="server" GridLines="Both" CssClass="gridview"
                                    HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                    AllowSorting="True" DataKeyNames="DBGSystemRecordID" HeaderStyle-ForeColor="Black"
                                    Width="100%" AutoGenerateColumns="true" PageSize="15" OnSorting="gvWarning_Sorting"
                                    OnPreRender="gvWarning_PreRender" OnRowDataBound="gvWarning_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                                    <PagerSettings Position="Top" />
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("DBGSystemRecordID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridview_header" />
                                    <RowStyle CssClass="gridview_row" />
                                    <PagerTemplate>
                                        <asp:GridViewPager runat="server" ID="Pager" HideFilter="true" TableName="Recordlist"
                                            HideAdd="true" HideDelete="true" OnBindTheGridToExport="gvWarningPager_BindTheGridToExport"
                                            OnBindTheGridAgain="gvWarningPager_BindTheGridAgain" OnExportForCSV="gvWarningPager_OnExportForCSV" />
                                    </PagerTemplate>
                                </dbg:dbgGridView>
                            </div>
                            <div runat="server" id="divEmptyWarning" visible="false">
                                <strong>No data. </strong>
                            </div>
                            <br />
                            <div style="background-color: Red; font-size: 14px; width: 100%; color: White;">
                                <strong id="stInvalid" name="stInvalid">Invalid Data </strong>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/warning.png" AlternateText="This data will be rejected"
                                    ToolTip="This data will be rejected" />
                            </div>
                            <div>

                                <asp:DropDownList runat="server" ID="ddlInValidReason" CssClass="NormalTextBox" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlInValidReason_OnSelectedIndexChanged">
                                </asp:DropDownList>

                                <dbg:dbgGridView ID="gvInValid" runat="server" GridLines="Both" CssClass="gridview"
                                    HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                                    AllowSorting="True" DataKeyNames="DBGSystemRecordID" HeaderStyle-ForeColor="Black"
                                    Width="100%" AutoGenerateColumns="true" PageSize="15" OnSorting="gvInValid_Sorting"
                                    OnPreRender="gvInValid_PreRender" OnRowDataBound="gvInValid_RowDataBound" AlternatingRowStyle-BackColor="#DCF2F0">
                                    <PagerSettings Position="Top" />
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Width="10px" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("DBGSystemRecordID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridview_header" />
                                    <RowStyle CssClass="gridview_row" />
                                    <PagerTemplate>
                                        <asp:GridViewPager runat="server" ID="Pager" HideFilter="true" TableName="Recordlist"
                                            HideAdd="true" HideDelete="true" OnBindTheGridToExport="gvInValidPager_BindTheGridToExport"
                                            OnBindTheGridAgain="gvInValidPager_BindTheGridAgain" OnExportForCSV="gvInValidPager_OnExportForCSV" />
                                    </PagerTemplate>
                                </dbg:dbgGridView>
                            </div>
                            <div runat="server" id="divEmptyDataInValid" visible="false">
                                <strong>No data. </strong>
                            </div>
                            <br />
                            <strong>To revalidate please upload your modified file again. </strong>
                            <br />
                            <br />

                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                            <div>
                                <table>
                                    <tr>
                                        <td>

                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtImportDataStartRow"
                                                ErrorMessage="Invalid Import Data Start Row data!" MaximumValue="1000000" MinimumValue="2"
                                                Type="Integer" Display="None" />
                                            <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtImportColumnHeaderRow"
                                                ErrorMessage="Invalid Import Column Header Row!" MaximumValue="1000000" MinimumValue="1"
                                                Type="Integer" Display="None" />
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                                ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct following errors:" />

                                        </td>
                                        <td></td>
                                        <td>
                                            <div runat="server" visible="false">
                                                <asp:LinkButton runat="server" ID="lnkCancel" CssClass="btn" CausesValidation="false"
                                                    OnClick="lnkCancel_Click"> <strong>Cancel</strong> </asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvTheGrid" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <span style="font-weight: bold" align="center"></span>
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="3" height="13"></td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hfColumnRowChanges" Value="no" />
        <br />
        <br />
        <%--<asp:Button runat="server" ID="btnTest" Text="test"  OnClick="btnTrigger_Click"/>--%>
        <%--<asp:Label runat="server" ID="lblInputErrors" />--%>
        <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" ClientIDMode="Static" runat="server"
        BehaviorID="popup" TargetControlID="lblInputErrors" PopupControlID="pnlPopup"
        BackgroundCssClass="modalBackground" OkControlID="btnOk" />--%>
        <%--<asp:Panel ID="pnlPopup" runat="server" Style="display: none">
        <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD;
            height: 100px; border-style: outset;">
            <div style="padding-top: 50px; padding: 20px;">
                <asp:Label ID="Label1" runat="server" Text="Sorry you have reached the limit of your account. 
                    To change your account type please" />
                <asp:HyperLink runat="server" ID="hlChangeAccountType" NavigateUrl="#" CssClass="popuplink"
                    onclick="$find('popup').hide(); return false;"> click here.</asp:HyperLink>
            </div>
            <div style="text-align: center; padding-left: 250px;">
                <asp:LinkButton runat="server" ID="btnOk" CssClass="btn" CausesValidation="false"> <strong>Ok</strong></asp:LinkButton>
            </div>


        </div>
    </asp:Panel>--%>
        <br />

        <asp:Label runat="server" ID="lblColumnRowStart" />
        <ajaxToolkit:ModalPopupExtender ID="mpeColumnRowStart" ClientIDMode="Static" runat="server"
            BehaviorID="popup2" TargetControlID="lblColumnRowStart" PopupControlID="pnlColumnRowStart"
            BackgroundCssClass="modalBackground" OkControlID="btnColumnRowStartOk" />
        <asp:Panel ID="pnlColumnRowStart" runat="server" Style="display: none">
            <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD; height: 100px; border-style: outset;">
                <div style="padding-top: 50px; padding: 20px;">

                    <br />
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label3" runat="server" Text="You have changed the Column Header Row / Data Start Row. Do you want to save those values for next time?" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px;"></td>
                            <td>
                                <div style="padding-right: 10px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkChangeColumnStartNo" CssClass="btn" CausesValidation="false"
                                                    OnClick="lnkChangeColumnStartNo_Click"> <strong>No</strong></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton runat="server" ID="lnkChangeColumnStartYes" CssClass="btn" CausesValidation="false"
                                                    OnClick="lnkChangeColumnStartYes_Click"> <strong>Yes</strong></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:Button Style="display: none;" runat="server"
                    ID="btnColumnRowStartOk" ClientIDMode="Static"></asp:Button>

            </div>
        </asp:Panel>


    </div>

</asp:Content>
