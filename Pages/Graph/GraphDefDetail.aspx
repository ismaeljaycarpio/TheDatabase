<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="GraphDefDetail.aspx.cs" Inherits="Pages_Graph_GraphDefDetail" %>

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

        function validate() {
            var isValid = Page_ClientValidate(""); //parameter is the validation group
            if (isValid) {
                escapeHTML();
            }
            return isValid;
        }

        function escapeHTML() {
            $('[id*=txtDefinitionScript]').val(
                $('[id*=txtDefinitionScript]').val().
                replace(new RegExp('<', 'g'), '&lt;').
                replace(new RegExp('>', 'g'), '&gt;'));
        }

        $(function () {
            $('#chkWordWrap').change(function () {
                if ($(this).is(':checked')) {
                    $('textarea').css('white-space', '').css('word-wrap', '').css('overflow-x', '');
                }
                else {
                    $('textarea').css('white-space', 'pre').css('word-wrap', 'normal').css('overflow-x', 'scroll');
                }
            });
        });

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
                            <div style="width: 40px; height: 40px;">
                                <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                    <ProgressTemplate>
                                        <table style="width: 100%; text-align: center">
                                            <tr>
                                                <td>
                                                   <asp:Image ID="Image1" runat="server" AlternateText="Processing..."  ImageUrl="~/Images/ajax.gif"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>--%>
                            </div>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <asp:HyperLink runat="server" ID="hlBack">
                                                <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"
                                                    ToolTip="Back" />
                                            </asp:HyperLink>
                                        </div>
                                    </td>
                                    <td>
                                        <div runat="server" id="divEdit" visible="false">
                                            <asp:HyperLink runat="server" ID="hlEditLink">
                                                <asp:Image runat="server" ID="Image2"  ImageUrl="~/App_Themes/Default/images/Edit_big.png"
                                                    ToolTip="Edit" />
                                            </asp:HyperLink>
                                        </div>
                                    </td>
                                    <td>
                                        <div runat="server" id="divSave">
                                            <asp:LinkButton runat="server" ID="lnkSave" OnClick="lnkSave_Click" CausesValidation="false"
                                                OnClientClick="return validate();" >
                                                <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                                    ToolTip="Save" />
                                            </asp:LinkButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div runat="server" id="divDelete" visible="false">
                                            <asp:LinkButton runat="server" ID="lnkDelete" OnClick="lnkDelete_Click" CausesValidation="false"
                                                OnClientClick="javascript:return confirm('Are you sure you want to delete this Graph Definition?')">
                                                <asp:Image runat="server" ID="ImageDelete" ImageUrl="~/App_Themes/Default/images/delete_big.png"
                                                    ToolTip="Delete" />
                                            </asp:LinkButton>
                                        </div>
                                        <div runat="server" id="divUnDelete" visible="false">
                                            <asp:LinkButton runat="server" ID="lnkUnDelete" OnClick="lnkUnDelete_Click" CausesValidation="true"
                                                OnClientClick="javascript:return confirm('Are you sure you want to restore this Graph Definition?')">
                                                <asp:Image runat="server" ID="ImageUndelete" ImageUrl="~/App_Themes/Default/images/Restore_Big.png"
                                                    ToolTip="Restore" />
                                            </asp:LinkButton>
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
                                ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table border="0">
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="width: 2em;">&nbsp;</td>
                                        <td>
                                            <div style="position:relative">
                                                <strong>Definition</strong>
                                                <div style="position:absolute; top: -0.2em; right:1em;">
                                                    <label for="chkWordWrap">Word Wrap</label>
                                                    <input id="chkWordWrap" type="checkbox" checked="checked" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="3" style="border: 1px solid silver;">
                                                <tr>
                                                    <td align="right">
                                                        <strong>Definition Name</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDefinitionName" runat="server" Width="160px" CssClass="NormalTextBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDefinitionName" runat="server" ControlToValidate="txtDefinitionName"
                                                            ErrorMessage="Definition Name is required">*</asp:RequiredFieldValidator>
<%--                                                        <asp:RegularExpressionValidator ForeColor="Red" Display="Dynamic" ID="revDefinitionName" runat="server"
                                                        ControlToValidate="txtDefinitionName" ErrorMessage="Invalid character(s) in Definition Name"
                                                        ValidationExpression="^[A-Za-z0-9_ ]+$">*</asp:RegularExpressionValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>Is System</strong>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Enabled="false" ID="cbIsSytem" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>Is Hidden</strong>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="cbIsHidden" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>Stored Procedure</strong>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDefinitionKey" runat="server" Width="160px" CssClass="NormalTextBox"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ForeColor="Red" Display="Dynamic" ID="revDefinitionKey" runat="server"
                                                        ControlToValidate="txtDefinitionKey" ErrorMessage="Invalid character(s) in Definition Key"
                                                        ValidationExpression="^[A-Za-z0-9_ ]+$">*</asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr><td colspan="2"><strong>Valid for:</strong></td></tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>Table</strong>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlEachTable"
                                                            DataTextField="TableName" DataValueField="TableID" 
                                                            OnSelectedIndexChanged="ddlEachTable_SelectedIndexChanged" AutoPostBack="true"
                                                            AppendDataBoundItems="true"
                                                            CssClass="NormalTextBox" Width="160px">
                                                            <asp:ListItem Text="-- All --" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>Column</strong>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlEachAnalyte"
                                                            OnSelectedIndexChanged="ddlEachAnalyte_SelectedIndexChanged" AutoPostBack="false"
                                                            CssClass="NormalTextBox" Width="160px">
                                                            <asp:ListItem Text="-- All --" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr><td colspan="2">&nbsp;</td></tr>
                                                <tr><td colspan="2"><strong>SP Data Columns:</strong></td></tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>1st Column</strong>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlColumn1"
                                                            OnSelectedIndexChanged="ddlColumn1_SelectedIndexChanged" AutoPostBack="false"
                                                            CssClass="NormalTextBox" Width="160px">
                                                            <asp:ListItem Text="-- None --" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>2nd Column</strong>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlColumn2"
                                                            OnSelectedIndexChanged="ddlColumn2_SelectedIndexChanged" AutoPostBack="false"
                                                            CssClass="NormalTextBox" Width="160px">
                                                            <asp:ListItem Text="-- None --" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>3rd Column</strong>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlColumn3"
                                                            OnSelectedIndexChanged="ddlColumn3_SelectedIndexChanged" AutoPostBack="false"
                                                            CssClass="NormalTextBox" Width="160px">
                                                            <asp:ListItem Text="-- None --" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <strong>4th Column</strong>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlColumn4"
                                                            OnSelectedIndexChanged="ddlColumn4_SelectedIndexChanged" AutoPostBack="false"
                                                            CssClass="NormalTextBox" Width="160px">
                                                            <asp:ListItem Text="-- None --" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 2em;">&nbsp;</td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtDefinitionScript" runat="server" Width="450px" Height="600px"
                                                CssClass="NormalTextBox" TextMode="MultiLine"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDefinition" runat="server" ControlToValidate="txtDefinitionScript"
                                                ErrorMessage="Definition is required">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="width: 2em;">&nbsp;</td>
                                        <td>
                                            <div style="text-align: right; margin-right: 1em; margin-top: 0.5em;">
                                        <asp:LinkButton runat="server" ID="lnkConvert" CssClass="btn"
                                            OnClientClick="escapeHTML();"
                                            OnClick="lnkConvert_Click"
                                            CausesValidation="false"> <strong>Convert</strong> </asp:LinkButton>
                                            </div>
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

