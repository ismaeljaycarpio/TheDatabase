<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="TextSection.aspx.cs" Inherits="DocGen.Document.TextSection.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:HiddenField runat="server" ID="hfRemoveSection" ClientIDMode="Static" Value="0" />
                <br />
                <span class="failureNotification">
                    <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                </span>
                <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification"
                    ValidationGroup="MainValidationGroup" />
                <table cellpadding="3">
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="left">
                                        <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="Text"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <table>
                                            <tr>
                                                
                                               
                                                <%--<td>
                                                    <div runat="server" id="div1">
                                                        <asp:LinkButton runat="server" ID="CancelButton" CausesValidation="false"
                                                            OnClientClick="CloseAndRefresh(); return false; ">
                                                           <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                                            </asp:LinkButton>
                                                    </div>
                                                </td>--%>
                                                 <td>
                                                </td>
                                                <td>
                                                    <div runat="server" id="div2">
                                                        <asp:LinkButton runat="server" ID="SaveButton"  OnClick="SaveButton_Click"
                                                            ValidationGroup="MainValidationGroup"> 
                                                            <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                                            </asp:LinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td align="right">
                                        <strong>Style</strong>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlStyle" runat="server" CssClass="NormalTextBox" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlStyle_SelectedIndexChanged" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                    <td align="right">
                                        <strong>Add merge field</strong>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlMergeField" runat="server" CssClass="NormalTextBox" AutoPostBack="false"
                                            ClientIDMode="Static" Width="200px">
                                            <asp:ListItem Selected="True" Value="" Text="-Select to add-"></asp:ListItem>
                                            <asp:ListItem Value="[ReportStartDate]" Text="Report Start Date"></asp:ListItem>
                                            <asp:ListItem Value="[ReportEndDate]" Text="Report End Date"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" style="width: 800px;">
                            <asp:TextBox ID="txtContent" Width="775px" Height="310px" runat="server" ClientIDMode="Static"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <strong></strong>
                        </td>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <%--<script type="text/javascript" src="../../WebService/DocGenWS.asmx/js"></script>--%>
    <%--<script type="text/javascript" src="../../js/jquery.textarea-expander.js"></script>--%>
    <script type="text/javascript">


        function SavedAndRefresh() {
            window.parent.document.getElementById('btnRefresh').click();
            parent.$.fancybox.close();

        }

        function CloseAndRefresh() {
            if (document.getElementById('hfRemoveSection').value == '0') {
                parent.$.fancybox.close();
            }
            else {
                //                                window.parent.document.getElementById('btnRefresh').click();
                window.parent.RemoveNoAddedSection();
                parent.$.fancybox.close();
            }

        }


        function insertAtCaret() {
            var txtarea = document.getElementById('txtContent');
            var text = document.getElementById('ddlMergeField').value;
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
