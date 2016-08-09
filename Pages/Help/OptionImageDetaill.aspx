<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" ValidateRequest="false"
    CodeFile="OptionImageDetaill.aspx.cs" Inherits="Pages_Help_OptionImageDetaill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function abc() {
            var b = document.getElementById('<%= lnkSave.ClientID %>');
            if (b && typeof (b.click) == 'undefined') {
                b.click = function () {
                    var result = true;
                    if (b.onclick) result = b.onclick();
                    if (typeof (result) == 'undefined' || result) {
                        eval(b.getAttribute('href'));
                    }
                }
            }

        }

       

    </script>
    <table border="0" cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                        </td>
                        <td align="left">
                            <div style="width: 40px; height: 40px;">
                                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress2" runat="server">
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
                            </div>
                        </td>
                        <td>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:LinkButton runat="server" ID="lnkBack" OnClientClick=" parent.$.fancybox.close();return false;">
                                                    <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                                </asp:LinkButton>
                                            </div>
                                        </td>
                                      
                                        <td>
                                            <div runat="server" id="divSave">
                                                <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="true">
                                                    <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
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
            <td valign="top">
            </td>
            <td valign="top">
           
                        <div id="search" >
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">
                                    <tr>
                                        <td align="right" valign="top">
                                            <strong>Value*</strong>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtValue" runat="server" Width="150px" CssClass="NormalTextBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="txtValue"
                                                ErrorMessage="Value - Required"></asp:RequiredFieldValidator>
                                         
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="top">
                                            <strong>Image*</strong>
                                        </td>
                                        <td valign="top">
                                            <asp:FileUpload ID="fuImage" runat="server" Width="400px"
                                                Font-Size="11px" onchange="this.form.submit();" />
                                            <asp:CustomValidator ID="ImageValidator" ControlToValidate="fuImage" runat="server"
                                                CssClass="failureNotification" ErrorMessage="Image file is invalid." ToolTip="Image file is invalid."
                                                ValidationGroup="MainValidationGroup" OnServerValidate="ImageValidator_ServerValidate">*</asp:CustomValidator>
                                            <p class="comment">
                                                <asp:Label ID="lblAllowedExt" runat="server"><b>Allowed file types</b>: {0}</asp:Label>
                                               
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong></strong>
                                        </td>
                                        <td align="left">
                                            <asp:Image ID="imgImage" onerror="this.src = '../../images/no_img.gif';" AlternateText="Image"
                                                Style="max-width: 450px" runat="server" ImageUrl="~/Images/no_img.gif" />
                                        </td>
                                    </tr>
                                    
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
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
   

     <script type="text/javascript">

         function CloseAndRefresh() {
             window.parent.document.getElementById('btnOptionImage').click();
             parent.$.fancybox.close();
             // alert('ok');
         }

    </script>
</asp:Content>
