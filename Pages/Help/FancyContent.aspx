<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" ValidateRequest="false"
    AutoEventWireup="true" CodeFile="FancyContent.aspx.cs" Inherits="Pages_Help_FancyContent" %>
<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">

    <div>
        <asp:Label runat="server" ID="lblTopTitle" CssClass="TopTitle">Content</asp:Label>
        <br />
        <br />
        <br />
        <div style="text-align:center;">
            <table>
                <tr>
                    <td colspan="2">
                          <editor:WYSIWYGEditor runat="server" scriptPath="../../Editor/scripts/" ID="edtContent"
                                            btnStyles="true" btnSave="false" EditorHeight="400" Height="400" EditorWidth="610"
                                            Width="700" AssetManager="../../assetmanager/assetmanager.aspx" AssetManagerWidth="550"
                                            AssetManagerHeight="400" Visible="true" ToolbarMode="0" btnPreview="False"
                    btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False" />

                    </td>
                </tr>
                <tr><td colspan="2" style="height:30px;"></td></tr>
                <tr >
                    <td style="width:40%;">


                    </td>
                    <td>
                        <table>

                            <tr>
                                <td>
                                    <asp:LinkButton runat="server" ID="lnkOK" CssClass="btn" OnClientClick="CloseAndRefresh();" OnClick="lnkSave_Click" >
                                           <strong>Save</strong>
                                    </asp:LinkButton>
                                </td>
                                <td style="padding-left:5px;">
                                    <asp:LinkButton runat="server" ID="lnkNo" CssClass="btn">
                                           <strong>Close</strong>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
           
        </div>


    </div>

</asp:Content>

