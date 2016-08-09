<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="GraphOptionDetail.aspx.cs" Inherits="Pages_UserControl_GraphOptionDetail" %>
 <%@ Register Src="~/Pages/UserControl/ColorPicker.ascx" TagName="CP" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <div runat="server" id="divDetail">
        <div runat="server" id="div3">
            <table>
                <tr>
                    <td colspan="2">
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="Value in High cannot be less than value in Low."
                            ClientValidationFunction="ValidateLowHigh1" ValidationGroup="goadd"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblDetailTitle" Font-Size="16px" Font-Bold="true" Text="Series Options"> </asp:Label>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td align="right">                                   
                                    <asp:Label runat="server" ID="lblTable" Font-Bold="true" Text="Table"></asp:Label>
                                </td>
                                <td>
                                     <asp:DropDownList runat="server" ID="ddlTable" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged"
                                        CssClass="NormalTextBox" DataTextField="TableName" DataValueField="TableID"
                                        AutoPostBack="true"  Width="260px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTable" runat="server" ControlToValidate="ddlTable"
                                        ErrorMessage="Required" Display="Dynamic" ValidationGroup="goadd"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">                                  
                                    <asp:Label runat="server" ID="lblAnalyte" Font-Bold="true" Text="Field"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlAnalyte" CssClass="NormalTextBox" AutoPostBack="true"
                                      Width="260px" OnSelectedIndexChanged="ddlAnalyte_SelectedIndexChanged">
                                        <asp:ListItem Text="--None--" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="rfvAnalyte" runat="server" ControlToValidate="ddlAnalyte"
                                        ErrorMessage="Required" Display="Dynamic" ValidationGroup="goadd"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="height:10px;"><td colspan="2"></td></tr>
                            <%--<tr >
                                <td align="right">                                   
                                     <asp:Label runat="server" ID="lblLocation" Font-Bold="true" Text="Location"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlLocation" DataTextField="LocationName"
                                        DataValueField="LocationID" CssClass="NormalTextBox" Width="260px">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="right">
                                    <strong>Graph Type</strong>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlGraphType" CssClass="NormalTextBox" Width="260px">
                                        <asp:ListItem Text="Line" Value="line" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Bar" Value="bar"></asp:ListItem>
                                        <asp:ListItem Text="Stacked Bar" Value="stackedbar"></asp:ListItem>
                                        <asp:ListItem Text="Area" Value="area"></asp:ListItem>
                                        <asp:ListItem Text="Point" Value="point"></asp:ListItem>
                                        <asp:ListItem Text="Mean, Min and Max" Value="MMM"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <tr style="height:10px;"><td colspan="2"></td></tr>
                            <tr>
                                <td align="right">
                                    <strong>Axis</strong>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlAxes" CssClass="NormalTextBox" AutoPostBack="true" 
                                        Width="100px" onselectedindexchanged="ddlAxes_SelectedIndexChanged">
                                        <asp:ListItem Text="Left" Value="Left" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                        <asp:ListItem Text="Percentage" Value="Percentage"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtAxisLabel" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Colour</strong>
                                </td>
                                <td>
                                    <asp:CP runat="server" ID="cpColour" />
                                    <%--<select style="width: 100px" id="ddlColour" class="NormalTextBox" runat="server">
                                        <option value="">--Auto--</option>
                                        <option style="background-color: Aqua;" value="Aqua">Aqua</option>
                                        <option style="background-color: Black; color: White;" value="Black">Black</option>
                                        <option style="background-color: Blue;" value="Blue">Blue</option>
                                        <option style="background-color: Fuchsia;" value="Fuchsia">Fuchsia</option>
                                        <option style="background-color: Gray;" value="Gray">Gray</option>
                                        <option style="background-color: Green;" value="Green">Green</option>
                                        <option style="background-color: Lime;" value="Lime">Lime</option>
                                        <option style="background-color: Maroon;" value="Maroon">Maroon</option>
                                        <option style="background-color: Navy; color: White;" value="Navy">Navy</option>
                                        <option style="background-color: Olive;" value="Olive">Olive</option>
                                        <option style="background-color: Orange;" value="Orange">Orange</option>
                                        <option style="background-color: Purple;" value="Purple">Purple</option>
                                        <option style="background-color: Red;" value="Red">Red</option>
                                        <option style="background-color: Silver;" value="Silver">Silver</option>
                                        <option style="background-color: Teal;" value="Teal">Teal</option>
                                        <option style="background-color: Yellow;" value="Yellow">Yellow</option>
                                    </select>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Low</strong>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtLowestValue" autocomplete="on" Width="70px" CssClass="NormalTextBox"
                                        ClientIDMode="Static" />
                                    <asp:RegularExpressionValidator ID="revLowestValue" ControlToValidate="txtLowestValue"
                                        runat="server" ErrorMessage="Numeric value please." ValidationGroup="goadd" Display="Dynamic"
                                        ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp; <strong>High</strong>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtHighestValue" autocomplete="on" Width="70px" CssClass="NormalTextBox"
                                        ClientIDMode="Static" />
                                    <asp:RegularExpressionValidator ID="revHighestValue" ControlToValidate="txtHighestValue"
                                        runat="server" ErrorMessage="Numeric value please." ValidationGroup="goadd" Display="Dynamic"
                                        ValidationExpression="(^-?\d{1,18}\.$)|(^-?\d{1,18}$)|(^-?\d{0,18}\.\d{1,4}$)">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                           
                                            <td>
                                                <div runat="server" id="div2" style="padding-left: 10px;">
                                                    <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                                        CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                                </div>
                                            </td>
                                             <td>
                                                <div runat="server" id="div1" style="padding-left: 10px;">
                                                    <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" CausesValidation="true"
                                                        OnClick="lnkSave_Click" > <strong>Save</strong></asp:LinkButton>
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
    </div>
    <script type="text/javascript">

        function CloseAndRefresh(iS) {
            window.parent.document.getElementById('hfDetailSearchID').value = iS;
            window.parent.document.getElementById('btnRefreshChartPop').click();
            parent.$.fancybox.close();
            // alert('ok');
        }
    
    </script>
</asp:Content>
