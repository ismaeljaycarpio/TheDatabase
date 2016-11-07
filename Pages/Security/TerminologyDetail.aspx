<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="TerminologyDetail.aspx.cs" Inherits="Pages_Security_TerminologyDetail" %>

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                <div runat="server" id="divDetail" onkeypress="abc();">
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
                                                <td>
                                                    <div style="width: 40px; height: 40px;">
                                                        <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server">
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
                                                                        <asp:LinkButton runat="server" ID="lnkSave"  OnClick="lnkSave_Click"
                                                                            CausesValidation="true"> 
                                                                                <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                                                             </asp:LinkButton>
                                                                    </div>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
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
                                <div id="search" style="padding-bottom: 10px">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                        ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                                </div>
                                <table cellpadding="3">
                                    <tr>
                                        <td align="right">
                                            <strong>Page Name</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlPageName" CssClass="NormalTextBox" DataTextField="PageName"
                                                DataValueField="PageName" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlPageName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Input Text</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInputText" CssClass="NormalTextBox" DataTextField="InputText"
                                                DataValueField="InputText" Width="300px">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtInpuText" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>--%>
                                            <asp:RequiredFieldValidator ID="rfvInputText" runat="server" ControlToValidate="ddlInputText"
                                                ErrorMessage="Input Text - Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Output Text</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOutputText" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvOutputText" runat="server" ControlToValidate="txtOutputText"
                                                ErrorMessage="Output Text - Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
