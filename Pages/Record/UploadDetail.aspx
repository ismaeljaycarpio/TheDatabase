<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="UploadDetail.aspx.cs" Inherits="Pages_Record_UploadDetail" %>

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
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                        </td>
                        <td align="left">
                             <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div>
                                                                <asp:HyperLink runat="server" ID="hlBack" >
                                                                <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" /> 
                                                                </asp:HyperLink>
                                                            </div>
                                                        </td>
                                                       
                                                        <td>
                                                            <div runat="server" id="divEdit" visible="false">
                                                                <asp:HyperLink runat="server" ID="hlEditLink" >  
                                                                       <asp:Image runat="server" ID="Image2"  ImageUrl="~/App_Themes/Default/images/Edit_big.png"  ToolTip="Edit" />
                                                                </asp:HyperLink>
                                                            </div>
                                                        </td>

                                                        <td>
                                                            <div runat="server" id="divSave">
                                                                <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click"
                                                                    CausesValidation="true"> 
                                                                   <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                                                    </asp:LinkButton>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width: 40px; height: 40px;">
                                                <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                    <ProgressTemplate>
                                                        <table style="width: 100%; text-align: center">
                                                            <tr>
                                                                <td>
                                                                    <asp:Image ID="Image1" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>--%>
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
            <td colspan="3" height="13">
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">
                                    <tr>
                                        <td align="right" style="width:100px;">
                                            <strong>Upload Name*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUploadName" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUploadName" runat="server" ControlToValidate="txtUploadName"
                                                ErrorMessage="Upload Name - Required"></asp:RequiredFieldValidator> 
                                        </td>
                                    </tr>
                                    <tr style="height:10px;"><td colspan="2"></td></tr>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset title="Upload Details">
                                                <legend>Upload Details</legend>
                                                <table>
                                                    <tr>
                                                       <td align="right" style="width:100px;">
                                                            <strong>Email From*</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmailFrom" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvEmailFrom" runat="server" ControlToValidate="txtEmailFrom"
                                                                ErrorMessage="Email From - Required"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator Display="Dynamic" ID="REVEmail" runat="server" ControlToValidate="txtEmailFrom"
                                                                ErrorMessage=" Invalid Email" ValidationExpression="^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$">
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            The email address sending in the data
                                                            <br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>File name*</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFilename" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFilename"
                                                                ErrorMessage="File name - Required"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                           Use any part of the file name e.g. xls
                                                            <br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <strong>Send Email To</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPopEmailFrom" runat="server" ForeColor="Red" ></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                      <tr style="height:10px;"><td colspan="2"></td></tr>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset>
                                                <legend>Save Data To</legend>
                                                <table>
                                                    <tr>
                                                        <td align="right" style="width:100px;">
                                                            <strong runat="server" id="stgTableCap">Table*</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="true" DataTextField="TableName"
                                                                DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                                                CssClass="NormalTextBox" Width="250px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTable"
                                                                ErrorMessage="Table - Required"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <asp:CheckBox ID="chkUseMapping" runat="server"
                                                                AutoPostBack="false" Text="Use mapping"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td align="right">
                                                            <strong>Location</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="false" DataTextField="LocationName"
                                                                DataValueField="LocationID" CssClass="NormalTextBox" Width="250px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                            <%--<div>
                                <table>
                                    <tr>
                                        <td>
                                            <div runat="server" id="divSave">
                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                    CausesValidation="true"> <strong>Save</strong> </asp:LinkButton>
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false">
                                                <asp:HyperLink runat="server" ID="hlEditLink" CssClass="btn"> <strong>Edit</strong> </asp:HyperLink>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
