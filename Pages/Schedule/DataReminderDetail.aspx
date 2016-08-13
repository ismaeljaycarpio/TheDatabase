<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" ValidateRequest="false"
    CodeFile="DataReminderDetail.aspx.cs" Inherits="Pages_Schedule_DataReminderDetail" %>

<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
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
                        <td align="left" style="width: 30%;">
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
                                            <strong>Days Before Date*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDays" runat="server" Width="150px" CssClass="NormalTextBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDays" runat="server" ControlToValidate="txtDays"
                                                ErrorMessage="Days - Required"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtDays"
                                                ErrorMessage="Numeric only!" MaximumValue="1000000" MinimumValue="0" Type="Double"
                                                Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Reminder Header*</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReminderHeader" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvReminderHeader" runat="server" ControlToValidate="txtReminderHeader"
                                                ErrorMessage="Reminder Header - Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td></td>
                                         <td align="left">
                                        <table>
                                            <tr >
                                                <td >
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
                                                                <div >
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
                                     </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Reminder Text</strong>
                                        </td>
                                        <td >
                                            <editor:WYSIWYGEditor runat="server" scriptPath="../../Editor/scripts/" ID="edtContent"
                                                btnStyles="true" btnSave="false" EditorHeight="200" Height="200" EditorWidth="650"
                                                Width="650" AssetManager="../../assetmanager/assetmanager.aspx" AssetManagerWidth="550"
                                                AssetManagerHeight="400" Visible="true" ToolbarMode="0" btnPreview="False"
                    btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right" valign="top" style="vertical-align:text-top;">
                                            <strong>Recipients</strong>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="grdUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="DataReminderUserID"
                                                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" 
                                                            CssClass="gridview" OnRowDataBound="grdUsers_RowDataBound"
                                                            OnRowCommand="grdUsers_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/App_Themes/Default/Images/delete_s.png"
                                                                            CommandName="deletetype"  CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete selected recipient?');" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Recipient">
                                                                    <ItemTemplate>
                                                                        <div style="padding-left: 10px;">
                                                                            <asp:Label runat="server" ID="lblUsers"></asp:Label>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderText="Userid" Visible="false">
                                                                    <ItemTemplate>
                                                                        <div style="padding-left: 10px;">
                                                                            <asp:Label runat="server" ID="lblUserID"></asp:Label>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>

                                                                <asp:TemplateField HeaderText="Send To" >
                                                                    <ItemTemplate>
                                                                        <div style="padding-left: 10px;">
                                                                            <asp:Label runat="server" ID="lblReminderColumn"></asp:Label>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                -- No User --
                                                            </EmptyDataTemplate>
                                                            <HeaderStyle CssClass="gridview_header" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr >
                                                                <td>
                                                                    <asp:DropDownList ID="ddlUser" runat="server" DataTextField="UserName" DataValueField="UserID"
                                                                        CssClass="NormalTextBox" ClientIDMode="Static">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td runat="server" id="tdReminderColumn" clientidmode="Static" >
                                                                    <asp:DropDownList ID="ddlReminderColumn" runat="server" DataTextField="DisplayName" DataValueField="ColumnID"
                                                                        CssClass="NormalTextBox">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <div runat="server" id="div2">
                                                                        <asp:LinkButton runat="server" ID="lnkAddUser" CssClass="btn" CausesValidation="false"
                                                                            OnClick="lnkAddUser_Click"> <strong>Add</strong>   </asp:LinkButton>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
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
    <script type="text/javascript">
        var oEditor = null;
        function InsertMergeField() {
            oUtil.obj.insertHTML("[" + document.getElementById("ctl00_HomeContentPlaceHolder_ddlDatabaseField").value + "]")
        }
    </script>
</asp:Content>
