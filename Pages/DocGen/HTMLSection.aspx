<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="HTMLSection.aspx.cs" Inherits="DocGen.Document.HTMLSection.Edit" %>

<%--<%@ Register assembly="FredCK.FCKeditorV2" namespace="FredCK.FCKeditorV2" tagprefix="FCKeditorV2" %>--%>
<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
      <script type="text/javascript" src="<%=ResolveUrl("~/Script/pasteimage.js")%>"></script>
    <br />
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification"
        ValidationGroup="MainValidationGroup" />
    <div>
        <table cellpadding="3">
            <tr>
                <td align="left">
                    <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text=" HTML Section"></asp:Label>
                </td>
                <td align="right">
                    <table>
                        <tr>
                            <td>
                                <%--<div runat="server" id="div21">
                                    <asp:LinkButton runat="server" ID="CancelButton" CausesValidation="false" OnClientClick="CloseAndRefresh(); return false; ">
                                        <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                    </asp:LinkButton>
                                </div>--%>
                            </td>
                            <td>
                            </td>
                            <td>
                                <div runat="server" id="div11">
                                    <asp:LinkButton runat="server" ID="SaveButton" OnClick="SaveButton_Click" ValidationGroup="MainValidationGroup">
                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                    </asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <editor:wysiwygeditor runat="server" scriptpath="../../Editor/scripts/" id="fckeTemplate"
                        btnstyles="true" btnsave="false" editorheight="500" height="550" editorwidth="810" 
                        width="1050" assetmanager="../../assetmanager/assetmanager.aspx" assetmanagerwidth="550"
                        assetmanagerheight="400" visible="true" ToolbarMode="0" btnPreview="False"
                    btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False"  />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong></strong>
                </td>
                <td align="left">
                    <asp:HiddenField runat="server" ID="hfRemoveSection" ClientIDMode="Static" Value="0" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
                var oEditor = null;
        //        function InsertMergeField() {
        //            oUtil.obj.insertHTML("[" + document.getElementById("ctl00_HomeContentPlaceHolder_ddlReturnFields").value + "]")
        //        }

        function SavedAndRefresh() {
            window.parent.document.getElementById('btnRefresh').click();
            parent.$.fancybox.close();

        }

        function CloseAndRefresh() {
            if (document.getElementById('hfRemoveSection').value == '0') {
                parent.$.fancybox.close();
            }
            else {
                //                window.parent.document.getElementById('btnRefresh').click();
                window.parent.RemoveNoAddedSection();
                parent.$.fancybox.close();
            }

        }

        function paste(src) {
             oUtil.obj.insertHTML("<img src='" + src + "'>");
            //alert('paste test');
        }

        $(function () {
            $.pasteimage(paste);
            //oUtil.obj.pasteimage(paste);
            //ctl00_HomeContentPlaceHolder_fckeTemplate.$.pasteimage(paste);
            //oUtil.$.pasteimage(paste);
        });

    </script>
</asp:Content>
