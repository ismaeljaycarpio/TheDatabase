<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="UserName.aspx.cs" Inherits="Pages_User_UserName" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
       

        function GetBackValue() 
        {
            window.parent.document.getElementById('txtEmail').value = document.getElementById('txtEmail').value;
            window.parent.document.getElementById('hfUserName').value = document.getElementById('txtUsername').value;
            window.parent.document.getElementById('hlAdvanced').href = 'UserName.aspx?UserID=' + encodeURIComponent(document.getElementById('hfUserID').value) + '&Email=' + encodeURIComponent(document.getElementById('txtEmail').value) + '&UserName=' + encodeURIComponent(document.getElementById('txtUsername').value) + '&mode=' + encodeURIComponent(document.getElementById('hfMode').value);
             parent.$.fancybox.close();
        }

    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="820" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle" Text="User Email and Username"></asp:Label></span>
                                <asp:HiddenField runat="server" ID="hfMode" ClientIDMode="Static" />
                                 <asp:HiddenField runat="server" ID="hfUserID" ClientIDMode="Static" Value="-1" />
                        </td>
                        <td align="left">
                            <div style="width: 40px; height: 40px;">
                                <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                    <ProgressTemplate>
                                        <table style="width: 100%; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>--%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                             <div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <div style="text-align: right; width: 700px;">
                                    <table style="text-align: right; width: 100%;">
                                        <tr>
                                            <td align="left">
                                                <asp:Label runat="server" ID="dbgHelpContent" CssClass="NormalTextBox"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:LinkButton runat="server" ID="lbHide" Text="Hide" Visible="true" OnClick="lbHide_Click"
                                                    CausesValidation="false"></asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="lbHelp" Text="Help..." Visible="false" OnClick="lbHelp_Click"
                                                    CausesValidation="false"></asp:LinkButton>
                                                <asp:CheckBox runat="server" Text="Show help by default" TextAlign="Right" ID="chkDoNotShowHelp"
                                                    AutoPostBack="true" OnCheckedChanged="chkDoNotShowHelp_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
               <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" >
                            <div runat="server" id="divDetail" >
                                <table cellpadding="3">
                                    <tr>
                                        <td align="right">
                                            <strong>Email*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" Width="250px" CssClass="NormalTextBox" ClientIDMode="Static"></asp:TextBox>
                                              <asp:RegularExpressionValidator Display="Dynamic" ID="REVEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                    </asp:RegularExpressionValidator>

                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Username*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUsername" runat="server" Width="250px" CssClass="NormalTextBox" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                 </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                          
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div runat="server" id="divSave">
                                                
                                                            <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" 
                                                                CausesValidation="true" onclick="lnkSave_Click"> <strong>Save</strong> </asp:LinkButton>
                                                      
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                
                                                    <asp:LinkButton runat="server" ID="hlBack"  CssClass="btn"
                                                     OnClientClick=" parent.$.fancybox.close(); return false;"> <strong>Back</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false">
                                               
                                                        <asp:HyperLink runat="server" ID="hlEditLink"  CssClass="btn"> <strong>Edit</strong> </asp:HyperLink>
                                                      
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
                
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
