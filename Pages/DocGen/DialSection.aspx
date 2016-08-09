<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="DialSection.aspx.cs" Inherits="DocGen.Document.DialSection.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div>
        <table cellpadding="3">
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="Dial"></asp:Label>
                </td>
                <td align="right">
                    <table>
                        <tr>
                            <%--<td>
                                <div runat="server" id="div21">
                                    <asp:LinkButton runat="server" ID="CancelButton" CausesValidation="false" OnClientClick="parent.$.fancybox.close(); return false; ">
                                        <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                    </asp:LinkButton>
                                </div>
                            </td>--%>
                            <td>
                            </td>
                            <td>
                                <div runat="server" id="div2">
                                    <asp:LinkButton runat="server" ID="SaveButton" OnClick="SaveButton_Click">
                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                    </asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong runat="server" id="stgTableCap">Table</strong>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDialTable1" AutoPostBack="true" DataTextField="TableName"
                        DataValueField="TableID" CssClass="NormalTextBox" OnSelectedIndexChanged="ddlDialTable1_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvTable" runat="server" ControlToValidate="ddlDialTable1"
                        ErrorMessage="Required" CssClass="NormalTextBox" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong runat="server" id="stgField">Field</strong>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDailYaxis1" CssClass="NormalTextBox" 
                        onselectedindexchanged="ddlDailYaxis1_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="-None-" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="ddlDailYaxis1"
                        ErrorMessage="Required" CssClass="NormalTextBox" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong>Dial</strong>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDialType1" CssClass="NormalTextBox" 
                        >
                        <asp:ListItem Value="RoundWhite" Text="Round White"></asp:ListItem>
                        <asp:ListItem Value="RoundBlack" Text="Round Black"></asp:ListItem>
                        <asp:ListItem Value="HorizintalYellow" Text="Horizonal Yellow"></asp:ListItem>
                        <asp:ListItem Value="HorizinalWhite" Text="Horizonal White" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong  >Label</strong>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDialLable1" CssClass="NormalTextBox" 
                    ToolTip="The text that appears inside the dial"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong >Scale</strong>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtScale1" CssClass="NormalTextBox"
                     ToolTip="The number between points on the scale"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtScale1"
                        runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong  >Heading</strong>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtHeading" CssClass="NormalTextBox" ToolTip="Appears above the dial"></asp:TextBox>
                    
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong  >Height</strong>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtHeight" CssClass="NormalTextBox"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtHeight"
                        runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <strong  >Width</strong>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtWidth" CssClass="NormalTextBox" ></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtWidth"
                        runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
