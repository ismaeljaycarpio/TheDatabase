<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="CalculationTest.aspx.cs"
    Inherits="Page_Help_CalculationTest" MasterPageFile="~/Home/Popup.master" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">

//        $(document).ready(function () {
//            document.getElementById('hfCalculationType').value = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_ddlCalculationType').value;
//            alert(document.getElementById('hfCalculationType').value);

//        });

        function GetBackValue() {
            window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_txtCalculation').value = document.getElementById('txtFormula').value;
           
            window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_hlCalculationEdit').href = '../Help/CalculationTest.aspx?type=calculation&formula=' + encodeURIComponent(document.getElementById('txtFormula').value) + "&Tableid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfTableID").value + "&Columnid=" + document.getElementById("ctl00_HomeContentPlaceHolder_hfColumnID").value;
           
            var lblMsg = window.parent.document.getElementById('ctl00_HomeContentPlaceHolder_lblMsg');
            if (lblMsg != null) {
                lblMsg.innerHTML = '';
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
        <asp:HiddenField  runat="server" ID="hfCalculationType" ClientIDMode="Static"/>
        <table cellpadding="3">
            <tr>
                <td colspan="5">
                    <table>
                        <tr>
                            <td align="left">
                                <h1>
                                    <asp:Label ID="lblValidationType" runat="server" Text="Calculation"> </asp:Label>
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
                <td style="text-align: right; padding-top:100px;" valign="top" >
                    <asp:Label ID="Label1" runat="server" Text="Formula:" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <strong runat="server" id="stgField">Field:</strong>
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
                                <asp:TextBox ID="txtFormula" runat="server" Height="180px" TextMode="MultiLine" Width="400px"
                                    CssClass="MultiLineTextBox" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                    Help:<a href="http://www.tsql.info" target="_blank">http://www.tsql.info</a>
                            </td>
                        </tr>
                        <tr align="left" style="text-align:left;">
                            <td align="left" style="text-align:left;">
<pre>
<strong>For Date & Time calculation: </strong> 
Please add number inside [], 
number mean following in the calculation
    • Number of minutes then 1 is the number of minutes
    • Number of hours then 1 is the number of hours
    • Number of days then 1 is the number of days
    • Date only then 1 is the number of days
    • Time only is the number of minutes
    • Date and time is the number of minutes
    • Year, Month & day then 1 is the number of days
    • Year & Month then 1 is the number of days
</pre>
                              

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
                     <asp:HiddenField runat="server" ID="hfColumnID" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>