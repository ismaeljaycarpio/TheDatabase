<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"   ValidateRequest="false"
    CodeFile="CalendarSection.aspx.cs" Inherits="DocGen.Document.Calendar.Edit" %>

<%@ Register TagPrefix="editor" Assembly="WYSIWYGEditor" Namespace="InnovaStudio" %>
<%@ Register Src="~/Pages/UserControl/ControlByColumn.ascx" TagName="ControlByColumn" TagPrefix="dbg" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
    <script type="text/javascript">
        function SavedAndRefresh() {
            window.parent.document.getElementById('btnRefresh').click();
            parent.$.fancybox.close();

        }

        function CloseAndRefresh() {
            if (document.getElementById('hfRemoveSection').value == '0') {
                parent.$.fancybox.close();
            }
            else {
                //                window.parent.document.getElementById('btnRefresh').click();
                window.parent.RemoveNoAddedSection();
                parent.$.fancybox.close();
            }

        }
    
    </script>
    <asp:HiddenField runat="server" ID="hfRemoveSection" ClientIDMode="Static" Value="0" />
    <br />
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification"
        ValidationGroup="MainValidationGroup" />
    <br />
    <div>
        <table cellpadding="3">
            <tr>
                <td align="left">
                    <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="Calendar"></asp:Label>
                </td>
                <td align="right">
                    <table>
                        <tr>
                            <%--<td>
                                <div runat="server" id="div1">
                                   
                                    <asp:LinkButton runat="server" ID="CancelButton" CausesValidation="false"
                                        OnClientClick="CloseAndRefresh(); return false; " > 
                                        <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                        </asp:LinkButton>
                                           
                                </div>
                            </td>--%>
                            <td>
                            </td>
                            <td>
                                <div runat="server" id="div2">
                                    <asp:LinkButton runat="server" ID="SaveButton" OnClick="SaveButton_Click" ValidationGroup="MainValidationGroup">
                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"
                                            ToolTip="Save" />
                                    </asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Calendar Title</strong>
                </td>
                <td align="left">
                    <asp:TextBox runat="server" ID="txtCalendarTitle" CssClass="NormalTextBox" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Table </strong>
                </td>
                <td align="left">
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="True" CssClass="NormalTextBox"
                                    DataTextField="TableName" DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTable"
                                    CssClass="failureNotification" ErrorMessage="Table is required." ToolTip="Table is required."
                                    ValidationGroup="MainValidationGroup">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <table runat="server" id="tblDateField" visible="false">
                                    <tr>
                                        <td>
                                            <strong>Date Field</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDateField" runat="server" AutoPostBack="false" CssClass="NormalTextBox"
                                                DataTextField="DisplayName" DataValueField="ColumnID">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlDateField"
                                                CssClass="failureNotification" ErrorMessage="Date Field is required." ToolTip="Date Field is required."
                                                ValidationGroup="MainValidationGroup">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" Checked="true" TextAlign="Right" Text="Show Add Record Icon" ID="chkShowAddRecordIcon"  />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="left">
                    <asp:RadioButtonList runat="server" id="radioCalendarDefaultView" RepeatDirection="Horizontal">
                        <asp:ListItem Value="month" Text="Month View" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="week" Text="Week View"></asp:ListItem>
                    </asp:RadioButtonList>

                </td>
            </tr>
            <tr runat="server" id="trDisplayText" visible="false">
                <td align="right" valign="middle">
                    <strong>Display Text</strong>
                </td>
                <td align="left">
                    <table>
                        <tr>
                          
                            <td align="left">
                                <table>
                                    <tr>
                                        <td>
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
                                                        <div>
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
                            
                            <td>
                                <editor:WYSIWYGEditor runat="server" scriptPath="../../Editor/scripts/" ID="edtContent"
                                    btnStyles="true" btnSave="false" EditorHeight="200" Height="200" EditorWidth="650"
                                    Width="650" AssetManager="../../assetmanager/assetmanager.aspx" AssetManagerWidth="550"
                                    AssetManagerHeight="400" Visible="true" ToolbarMode="0" btnPreview="False"
                    btnSearch="False" btnBookmark="False" btnAbsolute="False" btnForm="False" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="trFilter" visible="false">
                <td align="right" valign="top" style="padding-top:10px;">
                    <strong>Filter</strong>
                </td>
                <td align="left">
                    <asp:UpdatePanel runat="server" ID="upFilter" UpdateMode="Always">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <dbg:ControlByColumn runat="server" ID="cbcFilter1" OnddlYAxis_Changed="cbcFilter1_OnddlYAxis_Changed" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkAddFilter1" CausesValidation="false" OnClick="lnkAddFilter1_Click">
                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trFilter2" runat="server" visible="false">
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkMinusFilter2" CausesValidation="false" OnClick="lnkMinusFilter2_Click">
                                            <asp:Image ID="Image6" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <dbg:ControlByColumn runat="server" ID="cbcFilter2" OnddlYAxis_Changed="cbcFilter2_OnddlYAxis_Changed" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkAddFilter2" CausesValidation="false" OnClick="lnkAddFilter2_Click">
                                            <asp:Image ID="Image5" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trFilter3" runat="server" visible="false">
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkMinusFilter3" CausesValidation="false" OnClick="lnkMinusFilter3_Click">
                                            <asp:Image ID="Image7" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <dbg:ControlByColumn runat="server" ID="cbcFilter3" OnddlYAxis_Changed="cbcFilter3_OnddlYAxis_Changed" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>           
            <tr runat="server" id="trColourText" visible="false">
                <td align="right" valign="top" style="padding-top:10px;">
                    <strong>Text Colour</strong>
                </td>
                <td align="left">
                    <asp:UpdatePanel runat="server" ID="upColour" UpdateMode="Always">
                        <ContentTemplate>

                             <div >
                                        <asp:GridView ID="grdCalendarColor" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                            CssClass="gridview" OnRowCommand="grdCalendarColor_RowCommand" OnRowDataBound="grdCalendarColor_RowDataBound"
                                             ShowHeaderWhenEmpty="true"
                                            ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblID" Text='<%# Eval("ID") %>' ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnMinus" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png"
                                                            CommandName="minus"  CommandArgument='<%# Eval("ID") %>'   />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              
                                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                     <HeaderTemplate>
                                                         <strong>Field and Value</strong>
                                                     </HeaderTemplate>
                                                    
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                       
                                                            <dbg:ControlByColumn runat="server" ID="cbcColour" OnddlYAxis_Changed="CBCColorYAxisChanged" />
                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                     <HeaderTemplate>
                                                         <strong>Colour</strong>
                                                     </HeaderTemplate>
                                                    
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>                                                       
                                                            <asp:DropDownList runat="server" ID="ddlTextColour" CssClass="NormalTextBox">
                                                                <asp:ListItem Value="000000" Text="Black"></asp:ListItem>
                                                                <asp:ListItem Value="FFFFFF" Text="White"></asp:ListItem>
                                                                <asp:ListItem Value="C0C0C0" Text="Silver"></asp:ListItem>
                                                                <asp:ListItem Value="808080" Text="Gray"></asp:ListItem>
                                                                <asp:ListItem Value="FF0000" Text="Red"></asp:ListItem>
                                                                <asp:ListItem Value="800000" Text="Maroon"></asp:ListItem>
                                                                <asp:ListItem Value="FFFF00" Text="Yellow"></asp:ListItem>
                                                                <asp:ListItem Value="808000" Text="Olive"></asp:ListItem>
                                                                <asp:ListItem Value="00FF00" Text="Lime"></asp:ListItem>
                                                                <asp:ListItem Value="008000" Text="Green"></asp:ListItem>
                                                                <asp:ListItem Value="00FFFF" Text="Aqua"></asp:ListItem>
                                                                <asp:ListItem Value="008080" Text="Teal"></asp:ListItem>
                                                                <asp:ListItem Value="0000FF" Text="Blue"></asp:ListItem>
                                                                <asp:ListItem Value="000080" Text="Navy"></asp:ListItem>
                                                                <asp:ListItem Value="FF00FF" Text="Fuchsia"></asp:ListItem>
                                                                <asp:ListItem Value="800080" Text="Purple"></asp:ListItem>
                                                              </asp:DropDownList>                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnPlus" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png"
                                                            CommandName="plus" Visible="false"   CommandArgument='<%# Eval("ID") %>'  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                            </Columns>
                                            <HeaderStyle CssClass="gridview_header" Height="25px" />
                                            <RowStyle CssClass="gridview_row_NoPadding" />
                                        </asp:GridView>
                                       
                                        <div>
                                            <asp:Label ID="lblMsgTab" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>

                            <%--<table>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <strong>Field and Value</strong>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <strong>Colour</strong>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <dbg:ControlByColumn runat="server" ID="cbcColour1" OnddlYAxis_Changed="cbcColour1_OnddlYAxis_Changed" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="txtTextColour1" CssClass="NormalTextBox"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkAddColour1" CausesValidation="false" OnClick="lnkAddColour1_Click">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trColour2" runat="server" visible="false">
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkMinusColour2" CausesValidation="false" OnClick="lnkMinusColour2_Click">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <dbg:ControlByColumn runat="server" ID="cbcColour2" OnddlYAxis_Changed="cbcColour2_OnddlYAxis_Changed" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="txtTextColour2" CssClass="NormalTextBox"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkAddColour2" CausesValidation="false" OnClick="lnkAddColour2_Click">
                                            <asp:Image ID="Image3" runat="server" ImageUrl="~/App_Themes/Default/Images/PlusAdd.png" />
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trColour3" runat="server" visible="false">
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkMinusColour3" CausesValidation="false" OnClick="lnkMinusColour3_Click">
                                            <asp:Image ID="Image8" runat="server" ImageUrl="~/App_Themes/Default/Images/Minus.png" />
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <dbg:ControlByColumn runat="server" ID="cbcColour3" OnddlYAxis_Changed="cbcColour3_OnddlYAxis_Changed" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="txtTextColour3" CssClass="NormalTextBox"></asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>

              <tr>
                                <td align="right">
                                    <strong>Height</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHeight" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtHeight"
                                        runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
            <tr>
                <td align="right">
                     <strong>Width</strong>
                </td>
                <td align="left">
                     <asp:TextBox ID="txtWidth" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtWidth"
                                        runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong></strong>
                </td>
                <td align="left">
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        var oEditor = null;
        function InsertMergeField() {
            oUtil.obj.insertHTML("[" + document.getElementById("ctl00_HomeContentPlaceHolder_ddlDatabaseField").value + "]")
        }
    </script>
</asp:Content>
