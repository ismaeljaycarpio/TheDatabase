<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" ValidateRequest="false"
    CodeFile="ConditionDetail.aspx.cs" Inherits="Pages_Schedule_ConditionDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/ColumnUI.ascx" TagName="cUI" TagPrefix="dbg" %>


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
                        <td align="left" style="width:500px;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
                        </td>
                        <td align="left">
                            <div style="width: 40px; height: 40px;">
                                <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress2" runat="server">
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
                        <td>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:HyperLink runat="server" ID="hlBack">
                                                    <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                                </asp:HyperLink>
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false">
                                                <asp:HyperLink runat="server" ID="hlEditLink">
                                                    <asp:Image runat="server" ID="ImageEdit"  ImageUrl="~/App_Themes/Default/images/Edit_big.png"  ToolTip="Edit" />
                                                </asp:HyperLink>
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="search" >
                               
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">
                                    <tr>
                                        <td align="right">
                                            <strong>When*</strong>
                                        </td>
                                        <td>
                                           <asp:DropDownList runat="server" ID="ddlCheckColumn" DataValueField="ColumnID" DataTextField="DisplayName"
                                               CssClass="NormalTextBox" OnSelectedIndexChanged="ddlCheckColumn_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCheckColumn" runat="server" ControlToValidate="ddlCheckColumn"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Is*</strong>
                                        </td>
                                        <td>

                                            <dbg:cUI runat="server" ID="cuiCheckValue" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td>

                                            <table>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:Label runat="server" ID="lblValidation" Text="Data Invalid if outside the range"
                                                            Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top" style="width: 15px;"></td>
                                                    <td align="right">
                                                        Value less than:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMin" runat="server" Width="150px" CssClass="NormalTextBox"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="revNumber" ControlToValidate="txtMin"
                                                            runat="server" ErrorMessage="Numeric!" Display="Dynamic"
                                                            ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                                        </asp:RegularExpressionValidator>
                                                    </td>
                                                    <td align="right">
                                                        Value greater than:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMax" runat="server" Width="150px" CssClass="NormalTextBox"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtMax"
                                                            runat="server" ErrorMessage="Numeric!" Display="Dynamic"
                                                            ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                                        </asp:RegularExpressionValidator>
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                   
                                   
                                    
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
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
