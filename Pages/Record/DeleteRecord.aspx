<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="DeleteRecord.aspx.cs" Inherits="Pages_Record_DeleteRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>
<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
    <asp:Panel runat="server" ID="pnlDeleteAll">
         <asp:Label runat="server" ID="lblTitle" CssClass="TopTitle"></asp:Label>
    <br />
                <div style="padding-top: 50px; padding: 20px;">
                    <table>
                        <tr runat="server" id="trDeleteRestoreMessage">
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblDeleteRestoreMessage" Font-Bold="true" Text="Are you sure you want to delete selected item(s)?"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr runat="server" id="trDeleteReason" visible="false">
                            <td colspan="2">
                                <strong>Please enter the reason for deleting the selected records*</strong><br />
                                <br />

                                <asp:TextBox runat="server" ID="txtDeleteReason" ValidationGroup="DR" CssClass="NormalTextBox"
                                    TextMode="MultiLine" Height="70px" Width="310px"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="rfvDeleteReason" runat="server" ControlToValidate="txtDeleteReason"
                                    ErrorMessage="Required" Display="Dynamic" ValidationGroup="DR"></asp:RequiredFieldValidator>--%>
                                <br />
                                <br />

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:LinkButton  runat="server" ID="lnkDeleteAllOK" CssClass="btn"> <strong>OK</strong></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton OnClientClick="parent.$.fancybox.close();return false;" runat="server" ID="lnkDeleteAllNo" CssClass="btn"  CausesValidation="false"> 
                                                <strong>Cancel</strong></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trDeleteAllEvery">
                            <td colspan="2">
                                <br />
                                <asp:CheckBox runat="server" ID="chkDelateAllEvery" TextAlign="Right" Text="I would like to delete EVERY item in this table" AutoPostBack="false" />
                            </td>
                        </tr>
                        <tr runat="server" id="trDeleteParmanent" style="display: none;">
                            <td colspan="2">
                                <asp:CheckBox runat="server" ID="chkDeleteParmanent" TextAlign="Right" Text="I wish to delete these records permanently." AutoPostBack="false" />
                            </td>
                        </tr>
                        <tr runat="server" id="trUndo" style="display: none;">
                            <td colspan="2">
                                <asp:CheckBox runat="server" ID="chkUndo" TextAlign="Right" Text="I will not be able to undo this action." AutoPostBack="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 20px;">
                                <asp:Label runat="server" ID="lblDeleteMessageNote" Width="350px"
                                    Text="Note: Deleted records are retained in the database and can be viewed or restored by your Admin user. Admin Users can also delete them permanently."></asp:Label>
                            </td>
                        </tr>
                     
                    </table>
                    <asp:HiddenField runat="server" ID="hfParmanentDelete" Value="no" />
                </div>
        
    </asp:Panel>
   
 </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

