<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="SendEmail.aspx.cs" Inherits="Pages_Record_SendEmail" ValidateRequest="false" %>

<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script type="text/javascript">
        $(function () {
            $(".popuplink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 550,
                height: 250,
                titleShow: false
            });
        });
        var text_max = 160;
       
        $('#txtContent_fedback').html('Approx ' + text_max + ' characters remaining');

        function UpdateContent() {
           
            var text_length = $('#txtContent').val().length;
            var text_remaining = text_max - text_length;

            $('#txtContent_fedback').html('Approx ' + text_remaining + ' characters remaining');
        }


    </script>
    <div style="padding-left: 10px; padding-top: 10px;">
       
        
        
        <table>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                 <span class="TopTitle">
            <asp:Label runat="server" ID="lblTitle" Text="Send Email"></asp:Label></span>

                            </td>
                            
                            <td align="right">
                              
                                            <asp:HyperLink runat="server" ID="hlBack" CssClass="btn">  <strong>Back </strong> </asp:HyperLink>
                                       &nbsp;&nbsp;&nbsp;
                                             <asp:LinkButton runat="server" ID="lnkSend" CssClass="btn" CausesValidation="false"
                        OnClick="lnkSend_Click"> <strong>Send...</strong></asp:LinkButton>
                                        
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height:15px;">
                <td></td>
            </tr>
            <tr>
                <td>

                    <table width="100%">

                        <tr>

                            <td align="left">
                                <strong>Delivery Method</strong>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="optDeliveryMethod"
                                                RepeatDirection="Horizontal" AutoPostBack="true"
                                                OnSelectedIndexChanged="optDeliveryMethod_SelectedIndexChanged">
                                                <asp:ListItem Text="Email" Value="E" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="SMS" Value="S"></asp:ListItem>
                                            </asp:RadioButtonList>

                                        </td>
                                        <td>
                                            <asp:Image runat="server" ID="imgInfoNoSMS" ImageUrl="~/App_Themes/Default/Images/info_s.png"
                                                Visible="false" ToolTip="Table does not contain a mobile phone number field" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 100px;"></td>
                            <td align="left" style="width:200px;" >
                                <%--<div runat="server" id="tdAttachToRecord">
                                     <strong>Attach to Record</strong>
                                <br />
                                <asp:RadioButtonList runat="server" ID="optAttachToRecord"
                                    RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="None" Value="none" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Outgoing" Value="outgoing"></asp:ListItem>
                                    <asp:ListItem Text="Both" Value="both"></asp:ListItem>
                                </asp:RadioButtonList>
                                </div>--%>
                               

                            </td>
                            <td style="width: 175px;"></td>
                            <td align="right">
                                                               
                                <table cellspacing="8" >
                                    <tr>
                                        <td colspan="4" align="left">
                                            <strong>Template</strong>
                                        </td>
                                    </tr>
                                    <tr align="right">
                                        <td align="left">
                                            <asp:DropDownList runat="server" ID="ddlTemplate" CssClass="NormalTextBox" DataTextField="ContentKey" AutoPostBack="true"
                                                DataValueField="ContentID" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </td>
                                        <td>
                                            <asp:HyperLink runat="server" ID="hlNew"
                                                CssClass="popuplink" Text="New" NavigateUrl="~/Pages/SystemData/NewContent.aspx" />
                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkSaveTemplate" Visible="false"
                                                OnClick="lnkSaveTemplate_Click">Save</asp:LinkButton>

                                        </td>
                                        <td>

                                            <asp:LinkButton runat="server" ID="lnkDeleteTemplate" Visible="false"
                                                OnClick="lnkDeleteTemplate_Click" OnClientClick="return confirm('Are you sure you wish to delete the current template?');">Delete</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>

            <tr runat="server" id="trTo" visible="false">
                <td>
                    <strong>To</strong><br />
                    <asp:DropDownList runat="server" ID="ddlTo" CssClass="NormalTextBox">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="right"  style="width:55px;"><strong runat="server" id="stgSubject">Subject</strong></td>
                            <td align="left" style="width:545px;">
                                <asp:TextBox runat="server" ID="txtSuject" CssClasss="NormalTextBox" Width="500px"></asp:TextBox>
                            </td>
                            <td style="width: 50px;"></td>
                           
                            <td align="right">

                                <table>
                                    <tr>
                                        <td align="right">
                                            <strong>Value</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlDatabaseField" CssClass="NormalTextBox" DataTextField="Text"
                                                DataValueField="Value" ToolTip="Select database value and then click Add to add it to your content.">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <div runat="server" id="div1">
                                                <asp:LinkButton runat="server" ID="lnkAddDataBaseField"  OnClientClick="InsertMergeField(); return false;"
                                                    CausesValidation="true">Add</asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="lnkAddDataBaseFieldText" CssClass="btn" OnClientClick=" insertAtCaret(); return false;"
                                                    CausesValidation="true" Visible="false"> <strong>Add</strong></asp:LinkButton>
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
                <td>
                    <editor:WYSIWYGEditor runat="server" scriptPath="../../Editor/scripts/" ID="edtContent"
                        btnStyles="true" btnSave="false" EditorHeight="400" Height="400" EditorWidth="610"
                        Width="1000" AssetManager="../../assetmanager/assetmanager.aspx" AssetManagerWidth="550"
                        AssetManagerHeight="400" Visible="true" ToolbarMode="0" btnPreview="False"
                        btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False" />
                    <asp:TextBox runat="server" ID="txtContent" Width="1000" Height="200" TextMode="MultiLine"
                        CssClass="MultiLineTextBox" Visible="false" ClientIDMode="Static"></asp:TextBox> <br />
                   <div id="txtContent_fedback" style="font-weight:bold;font-style:italic;" runat="server" clientidmode="Static"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                   
                    &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                 
                   

                </td>
            </tr>
        </table>
    </div>



    <asp:Label runat="server" ID="lblSendEmail" />
    <ajaxToolkit:ModalPopupExtender ID="mpeSendEmail" runat="server" TargetControlID="lblSendEmail"
        PopupControlID="pnlSendEmail" BackgroundCssClass="modalBackground" OkControlID="lnkSendEmailCancel" />
    <asp:Panel ID="pnlSendEmail" runat="server" Style="display: none">
        <div style="border-width: 5px; background-color: #ffffff; border-color: #4F8FDD; height: 250px; border-style: outset;">
            <div style="padding-top: 50px; padding: 20px;">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblConfirmTitle" Font-Bold="true" Text="Send Email Confirmation" CssClass="TopTitle"></asp:Label>
                            <br />
                            <br />

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblMsgSendEmail"></asp:Label>
                            <asp:Label runat="server" ID="lblMsgSendSMS"></asp:Label>
                            <br />
                            <br />

                            <asp:HiddenField runat="server" ID="hfRecordIDs" Value="" />
                            <asp:HiddenField runat="server" ID="hfEmailCount" Value="" />
                            <asp:HiddenField runat="server" ID="hfEmailCountMonth" Value="0" />
                            <asp:HiddenField runat="server" ID="hfSMSCountMonth" Value="0" />
                            <asp:HiddenField runat="server" ID="HiddenField2" Value="" />
                            <asp:Button ID="btnContentSaved" runat="server" Visible="true" ClientIDMode="Static" Text=""
                                Height="1px" Width="1px" Style="display: none;" OnClick="btnContentSaved_Click" />
                            <asp:HiddenField runat="server" ID="hfContentKey" Value="" ClientIDMode="Static" />
                        </td>
                    </tr>

                    <%--<tr>
                        <td colspan="2" align="left">
                            <asp:CheckBox runat="server" ID="chkSaveOutgoing" Text="Save outgoing message in database"  TextAlign="Right" />
                        </td>

                    </tr>--%>
                    <tr runat="server" id="trSaveReplies">
                        <td colspan="2" align="left">
                            <asp:CheckBox runat="server" ID="chkSaveReplies" Text="Save replies in database"  TextAlign="Right" />
                            <br />
                            <br />
                        </td>

                    </tr>
                    <tr>
                        <td colspan="2">

                            <table>
                                <tr>
                                    <td style="width: 100px;"></td>

                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkSendEmailCancel" CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                    </td>

                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkSendEmailOK" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkSendEmailOK_Click"> <strong>OK</strong></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                        <br />
                                        <asp:Label runat="server" ID="lblEmailCountMonth" Font-Italic="true"></asp:Label>
                                        <asp:Label runat="server" ID="lblSMSCountMonth" Font-Italic="true"></asp:Label>

                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <%--<tr>
                            <td colspan="2">
                                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                    <ProgressTemplate>
                                        <table style="width: 50px; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>--%>
                </table>
            </div>
        </div>
    </asp:Panel>


    <script type="text/javascript">
        var oEditor = null;
        function InsertMergeField() {
            oUtil.obj.insertHTML("[" + document.getElementById("ctl00_HomeContentPlaceHolder_ddlDatabaseField").value + "]")
        }

        function insertAtCaret() {
            var txtarea = document.getElementById('txtContent');
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
</asp:Content>
