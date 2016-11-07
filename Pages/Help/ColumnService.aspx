<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="ColumnService.aspx.cs" Inherits="Pages_Help_ColumnService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

    <script type="text/javascript">
        function GetBackValue(sParam) {
            var chkControlValueChangeService = window.parent.document.getElementById('chkControlValueChangeService');
            if (chkControlValueChangeService != null) {
                if (sParam == 'y') {
                    chkControlValueChangeService.checked = true
                }
                else {
                    chkControlValueChangeService.checked = false
                }
            }

            parent.$.fancybox.close();
        }



    </script>

    <table border="0" cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 500px;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle" Text="Column Action"></asp:Label></span>
                        </td>
                        <td align="left"></td>
                        <td>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:LinkButton runat="server" ID="hlBack" OnClientClick="parent.$.fancybox.close();return false;">
                                                    <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png" ToolTip="Back" />
                                                </asp:LinkButton>
                                            </div>
                                        </td>

                                        <td>
                                            <div runat="server" id="divSave">
                                                <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="true">
                                                    <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/Save.png" ToolTip="Save" />
                                                </asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13"></td>
        </tr>
        <tr>
            <td valign="top"></td>
            <td valign="top">

                <div id="search">

                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                        ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div runat="server" id="divDetail">
                        <table cellpadding="3">


                            <tr>
                                <td align="right"></td>
                                <td align="left">
                                    <strong>Server side</strong>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <strong>SP Name</strong>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSPName" CssClass="NormalTextBox" Width="400"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>.Net Method</strong>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDotNetMethod" CssClass="NormalTextBox" Width="400"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <strong>Common Action</strong>
                                </td>
                                <td>

                                    <asp:DropDownList runat="server" ID="ddlAfterValueChange"
                                        CssClass="NormalTextBox" AutoPostBack="false">
                                        <asp:ListItem Value="" Text="--None--"></asp:ListItem>
                                        <asp:ListItem Value="save" Text="Save the record"></asp:ListItem>
                                        <asp:ListItem Value="show_hide_child_tabs" Text="Show hide child tabs"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Message Display Type</strong>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMessageType"
                                        CssClass="NormalTextBox" AutoPostBack="false">
                                        <asp:ListItem Value="" Text="--No Message--"></asp:ListItem>
                                        <asp:ListItem Value="tdbmsg" Text="Top Notification"></asp:ListItem>
                                        <asp:ListItem Value="js" Text="Box message"></asp:ListItem>
                                    </asp:DropDownList>


                                </td>
                            </tr>

                            <tr>
                                <td style="height:13px;"></td>
                                <td align="left"></td>
                            </tr>

                            <tr>
                                <td align="right"></td>
                                <td align="left">
                                    <strong>Browser side</strong>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Control Event</strong>
                                </td>
                                <td>

                                    <asp:DropDownList runat="server" ID="ddlControlEvent"
                                        CssClass="NormalTextBox" AutoPostBack="false">
                                        <asp:ListItem Value="" Text="--Default(value changed)--"></asp:ListItem>
                                        <asp:ListItem Value="blur" Text="blur/lost focus"></asp:ListItem>
                                        <asp:ListItem Value="change" Text="change"></asp:ListItem>
                                        <asp:ListItem Value="click" Text="click"></asp:ListItem>
                                        <asp:ListItem Value="dblclick" Text="dblclick"></asp:ListItem>
                                        <asp:ListItem Value="focus" Text="focus"></asp:ListItem>
                                        <asp:ListItem Value="focusin" Text="focusin"></asp:ListItem>
                                        <asp:ListItem Value="focusout" Text="focusout"></asp:ListItem>
                                        <asp:ListItem Value="hover" Text="hover"></asp:ListItem>
                                        <asp:ListItem Value="keydown" Text="keydown"></asp:ListItem>
                                        <asp:ListItem Value="keypress" Text="keypress"></asp:ListItem>
                                        <asp:ListItem Value="keyup" Text="keyup"></asp:ListItem>
                                        <asp:ListItem Value="load" Text="load"></asp:ListItem>
                                        <asp:ListItem Value="mousedown" Text="mousedown"></asp:ListItem>
                                        <asp:ListItem Value="mouseleave" Text="mouseleave"></asp:ListItem>
                                        <asp:ListItem Value="mousemove" Text="mousemove"></asp:ListItem>
                                        <asp:ListItem Value="mouseout" Text="mouseout"></asp:ListItem>
                                        <asp:ListItem Value="mouseover" Text="mouseover"></asp:ListItem>
                                        <asp:ListItem Value="mouseup" Text="mouseup"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Javascript Code</strong>
                                </td>
                                <td>

                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlConrolIDnText" Width="400" ClientIDMode="Static"
                                                    ToolTip="Select a field and then click Add control ID." AutoPostBack="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left">
                                                <asp:LinkButton runat="server" ID="lnlAddDataBaseFieldText" OnClientClick=" insertAtCaret(); return false;"
                                                    CausesValidation="true" Visible="true"> <strong>Add Control ID</strong></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:TextBox runat="server" ID="txtJavaScriptFunction" TextMode="MultiLine" ClientIDMode="Static"
                                        CssClass="NormalTextBox" Width="500" Height="260"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2"></td>
                            </tr>





                        </table>
                    </div>
                    <br />
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    <br />
                </asp:Panel>

            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="3" height="13"></td>
        </tr>
    </table>


    <script type="text/javascript">
        function insertAtCaret() {
            var txtarea = document.getElementById('txtJavaScriptFunction');
            var text = document.getElementById('ddlConrolIDnText').value;
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


</asp:Content>

