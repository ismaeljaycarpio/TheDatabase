<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" 
CodeFile="MapPopup.aspx.cs" Inherits="Pages_Content_MapPopup" ValidateRequest="false" %>
<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
  <script type="text/javascript" src="<%=ResolveUrl("~/Script/pasteimage.js")%>"></script>
<div style="padding:10px;">
    <table border="0" cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 30%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle" Text="Map Popup"></asp:Label></span>
                        </td>
                        <td>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:LinkButton runat="server" ID="hlBack" OnClientClick="parent.$.fancybox.close();return false;">
                                                    <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"
                                                        ToolTip="Back" />
                                                </asp:LinkButton>
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divSave">
                                                <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="true">
                                                    <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                        ToolTip="Save" />
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
            <td colspan="3" height="13">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
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
                                        <div>
                                            <asp:LinkButton runat="server" ID="lnlAddDataBaseField" CssClass="btn" OnClientClick="InsertMergeField(); return false;"
                                                CausesValidation="true"> <strong>Add</strong></asp:LinkButton>
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
            <td>
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <editor:WYSIWYGEditor runat="server" scriptPath="../../Editor/scripts/" ID="edtContent"
                    btnStyles="true" btnSave="false" EditorHeight="400" Height="400" EditorWidth="650"
                    Width="650" AssetManager="../../assetmanager/assetmanager.aspx" AssetManagerWidth="550"
                    AssetManagerHeight="400" Visible="true" ToolbarMode="0" btnFullScreen="False" btnPreview="False"
                    btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False" />
            </td>
            <td>
            </td>
        </tr>

        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
    </table>

</div>


 <script type="text/javascript">
     var oEditor = null;
     function InsertMergeField() {
         oUtil.obj.insertHTML("[" + document.getElementById("ctl00_HomeContentPlaceHolder_ddlDatabaseField").value + "]")
     }

     function paste(src) {
         oUtil.obj.insertHTML("<img src='" + src + "'>");
     }

     $(function () {
         $.pasteimage(paste);
     });


    </script>
</asp:Content>

