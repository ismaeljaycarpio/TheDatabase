<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="TableColumn.aspx.cs"
    Inherits="Page_Help_CalculationTest" MasterPageFile="~/Home/Popup.master" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">

        function GetBackValue() {
            if (window.parent.document.getElementById('ddlHeaderText') == null) {
                window.parent.document.getElementById('hfDisplayColumnsFormula').value = document.getElementById('txtFormula').value;
                window.parent.document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?formula=' + encodeURIComponent(document.getElementById('txtFormula').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value;
            }
            if (window.parent.document.getElementById('ddlHeaderText') != null) {
                window.parent.document.getElementById('hfDisplayColumnsFormula').value = document.getElementById('txtFormula').value;
                window.parent.document.getElementById('hlDDEdit').href = '../Help/TableColumn.aspx?headername=yes&formula=' + encodeURIComponent(document.getElementById('txtFormula').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value;
            }
           
            parent.$.fancybox.close();

        }


        function insertAtCaret() {
            var txtarea = document.getElementById('txtFormula');
            var text = "[" + document.getElementById('ctl00_HomeContentPlaceHolder_ddlDatabaseField').value + "]";
            var scrollPos = txtarea.scrollTop;
            var strPos = 0;
            var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ? "ff" : (document.selection ? "ie" : false));
            if (br == "ie") {
                txtarea.focus();
                var range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length); strPos = range.text.length;
            }
            else if (br == "ff") strPos = txtarea.selectionStart;
            var front = (txtarea.value).substring(0, strPos);
            var back = (txtarea.value).substring(strPos, txtarea.value.length);
            txtarea.value = front + text + back; strPos = strPos + text.length;
            if (br == "ie") {
                txtarea.focus();
                var range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length);
                range.moveStart('character', strPos);
                range.moveEnd('character', 0);
                range.select();
            }
            else if (br == "ff") {
                txtarea.selectionStart = strPos;
                txtarea.selectionEnd = strPos; txtarea.focus();
            }
            txtarea.scrollTop = scrollPos;
        }

    </script>
    <div style="padding-top: 10px;">
        <table cellpadding="3">
            <tr>
                <td colspan="5">
                    <table>
                        <tr>
                            <td align="left">
                                <h1>
                                    <asp:Label ID="lblValidationType" runat="server" Text="Display Fields"> </asp:Label>
                                </h1>
                            </td>
                            <td align="right" style="padding-left:100px;">
                                <div runat="server" id="divSave">
                                    <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="false">
                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                    </asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Formula:" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <strong>Database Value</strong>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlDatabaseField" CssClass="NormalTextBox" DataTextField="Text"
                                                            DataValueField="Value" ToolTip="Select database value and then click Add to add it to your content.">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="div1">
                                                            <asp:LinkButton runat="server" ID="lnlAddDataBaseFieldText" CssClass="btn" OnClientClick=" insertAtCaret(); return false;"
                                                                Visible="true"> <strong>Add</strong></asp:LinkButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:TextBox ID="txtFormula" runat="server" Height="80px" TextMode="MultiLine" Width="400px"
                                    CssClass="MultiLineTextBox" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                </td>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="text-align: right;" valign="top">
                    <%--<asp:Label ID="Label4" runat="server" Text="Help" Font-Bold="true"></asp:Label>--%>
                </td>
                <td colspan="3" style="text-align: left;">
                    <%--<asp:Label runat="server" ID="lblContentCommon"></asp:Label>--%>
                    <asp:HiddenField runat="server" ID="hfTableID" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
