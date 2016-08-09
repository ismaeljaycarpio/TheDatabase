<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
 CodeFile="RoleGroupDetail.aspx.cs" Inherits="Pages_Security_RoleGroupDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
 <script type="text/javascript">     
     function GetBackAndReFresh() {
         window.parent.document.getElementById('hfRoleGroupID').value = document.getElementById('hfRoleID').value;

         window.parent.document.getElementById('btnRoleSaved').click();
         parent.$.fancybox.close();
     }
    </script>
 <table border="0" cellpadding="0" cellspacing="0"  align="center" runat="server" id="divDetail">
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                <asp:HiddenField runat="server" ClientIDMode="Static" ID="hfRoleID" />
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
      
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
               
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div >
                                <table>
                                    <tr>
                                        <td align="right" style="width:150px;">
                                            <strong>New Role Name*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRoleName" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                             <asp:RequiredFieldValidator ID="rfvRoleName" runat="server" ControlToValidate="txtRoleName"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height:10px;"> </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <div runat="server" id="divSave">

                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                    CausesValidation="true"> <strong>Save </strong> </asp:LinkButton>

                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            
                            
                           
                        </asp:Panel>
                
                
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
    </table>

</asp:Content>

