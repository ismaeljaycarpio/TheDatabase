<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="RecordUpload.aspx.cs" Inherits="Pages_Record_RecordUpload"
    EnableTheming="true" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="JavaScript">

        extArray = new Array(".csv");

        function LimitAttach(form, file) {
            allowSubmit = false;
            if (!file) return;
            while (file.indexOf("\\") != -1)
                file = file.slice(file.indexOf("\\") + 1);
            ext = file.slice(file.indexOf(".")).toLowerCase();
            for (var i = 0; i < extArray.length; i++) {
                if (extArray[i] == ext) { allowSubmit = true; break; }
            }
            if (allowSubmit) form.submit();
            else {
                alert("Please only upload files that end in types:  "
    + (extArray.join("  ")) + "\nPlease select a new "
    + "file to upload and submit again.");
                return false;
            }
        }
        //  End -->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle" Text="Upload Records" Visible="true"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13"></td>
        </tr>
        <tr>
            <td colspan="3"></td>
        </tr>
        <tr>
            <td valign="top"></td>
            <td valign="top">
                <div id="search" style="padding-bottom: 10px">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                        ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div runat="server" id="divDetail">
                        <table cellpadding="3">
                            <tr>
                                <td align="right" style="vertical-align: top;">
                                    <strong>Template</strong>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTemplate" CssClass="NormalTextBox" OnPreRender="ddlTemplate_PreRender"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:Label runat="server" ID="lblImportTemplateFile"></asp:Label>
                                    <%--<asp:HyperLink runat="server" ID="hlImportTemplate" NavigateUrl="~/Pages/Import/ImportTemplate.aspx">Edit</asp:HyperLink>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong></strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblImortType" Font-Bold="true"></asp:Label>
                                    <br />
                                    <div runat="server" id="divColumnName">
                                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Vertical" CssClass="gridviewborder"
                                            AllowPaging="false" AllowSorting="false"  HeaderStyle-ForeColor="Black"
                                            Width="100%" AutoGenerateColumns="true" OnRowDataBound="gvTheGrid_RowDataBound">
                                            <PagerSettings Position="Top" />
                                            <%--<Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("DBGSystemRecordID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>--%>
                                            <HeaderStyle CssClass="gridviewborder_header" />
                                            <RowStyle CssClass="gridviewborder_row" />
                                        </dbg:dbgGridView>
                                        <%--<asp:Label runat="server" ID="lblImportDateTimeNotes" Text="* Note that Date and Time Recorded are imported separately"
                                            ForeColor="Red" Font-Size="XX-Small"></asp:Label>--%>
                                        <br />
                                    </div>
                                    <div runat="server" id="divColumnPosition">
                                        <dbg:dbgGridView ID="gvPosition" runat="server" GridLines="Both" CssClass="gridviewborder"
                                            AllowPaging="false" AllowSorting="false" HeaderStyle-ForeColor="Black" AutoGenerateColumns="false">
                                            <PagerSettings Position="Top" />
                                            <Columns>
                                                <asp:BoundField DataField="DisplayName" HeaderText="Column Name" />
                                                <asp:BoundField DataField="PositionOnImport" HeaderText="Column Position" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                            <HeaderStyle CssClass="gridviewborder_header" />
                                            <RowStyle CssClass="gridviewborder_row" />
                                        </dbg:dbgGridView>
                                        <%--<asp:Label runat="server" ID="Label1" Text="* Note that Date and Time Recorded are imported separately"
                                            ForeColor="Red" Font-Size="XX-Small"></asp:Label>--%>
                                        <br />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td valign="top"></td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-bottom: 5px;">
                                    <strong>Upload File:</strong>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fuRecordFile" runat="server" Style="width: 500px;" size="70" />
                                    <br />
                                    <span style="font-style: italic;">Please select a CSV/XLS/XLSX file to upload.</span>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <strong>Download:</strong>
                                </td>
                                <td>
                                    <div runat="server" id="divUploadFiles">
                                        <asp:LinkButton runat="server" ID="lnkDownloadTemplate" Text="CSV " NavigateUrl="#"
                                            OnClick="lnkDownloadTemplate_Click" CssClass="NormalTextBox"></asp:LinkButton>
                                        or
                                    <asp:LinkButton runat="server" ID="lnkDownloadXLS" Text="XLS" NavigateUrl="#" OnClick="lnkDownloadXLS_Click"
                                        CssClass="NormalTextBox"></asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkDownloadXLSX" Text="XLSX" Visible="false" NavigateUrl="#"
                                            OnClick="lnkDownloadXLSX_Click" CssClass="NormalTextBox"></asp:LinkButton>
                                    </div>
                                    <div runat="server" id="divCustomUploadFiles" visible="false">
                                        <%--<asp:HyperLink runat="server" ID="hlCustomUploadSheet" Target="_blank"
                                                                         Text="Download"></asp:HyperLink>--%>
                                        <asp:LinkButton runat="server" ID="hlCustomUploadSheet" Text="View"
                                            OnClick="hlCustomUploadSheet_Click"></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Batch Description:</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBatchDescription" runat="server" Width="300px" CssClass="MultiLineTextBox"
                                        TextMode="MultiLine" Height="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <%--<tr runat="server" id="trLocation" visible="false">
                                <td align="right">
                                    <strong>Location:</strong>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="false" DataTextField="LocationName"
                                        DataValueField="LocationID" CssClass="NormalTextBox">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>

                            <%--  <tr runat="server" id="trDataUpdateUniqueColumnID" visible="false">
                                <td></td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkDataUpdateUniqueColumnID" ClientIDMode="Static" Checked="false"
                                        TextAlign="Right" Font-Bold="true" Text="Update existing data, matching on [ColumnName]" />

                                </td>
                            </tr>--%>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <asp:LinkButton runat="server" ID="lnkNext" CssClass="btn" OnClick="lnkNext_Click"
                                            CausesValidation="true"> <strong>Next</strong> </asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                        <br />
                    </div>
                </asp:Panel>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="3" height="13"></td>
        </tr>
    </table>
</asp:Content>
