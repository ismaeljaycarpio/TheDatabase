<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" CodeFile="EditMany.aspx.cs" Inherits="Pages_Record_EditMany" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:Panel ID="pnlEditMany" runat="server">
                <div style="padding-top: 50px; padding: 20px;">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblTitle" Font-Bold="true" Text="Update Multiple" CssClass="TopTitle"></asp:Label>
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-left: 50px;">
                                <strong runat="server" id="stgFieldToUpdate">Field to Update</strong>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYAxisBulk" runat="server" AutoPostBack="true" CssClass="NormalTextBox"
                                    OnSelectedIndexChanged="ddlYAxisBulk_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvddlYAxisBulk" ControlToValidate="ddlYAxisBulk"
                                    ErrorMessage="Mandatory!" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <strong>New Value</strong>
                            </td>
                            <td>

                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtTextBulk" CssClass="NormalTextBox" Visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtNumberBulk" CssClass="NormalTextBox" Visible="false"></asp:TextBox>
                                            <asp:DropDownList runat="server" ID="ddlDropdownBulk" CssClass="NormalTextBox" Visible="false">
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txtDateBulk" Width="90px" CssClass="NormalTextBox"
                                                Visible="false"></asp:TextBox>

                                            <asp:CheckBox runat="server" ID="chkCheckboxBulk" Visible="false" />
                                        </td>
                                        <td style="padding-left: 2px; padding-right: 2px;">
                                            <asp:ImageButton runat="server" ID="ibBulkDate" ImageUrl="~/Images/Calendar.png"
                                                AlternateText="Click to show calendar" CausesValidation="false" Visible="false" />

                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBulkTime" Width="50px" CssClass="NormalTextBox"
                                                ToolTip="Time" Visible="false"></asp:TextBox>

                                        </td>
                                        <td>

                                            <ajaxtoolkit:maskededitextender runat="server" id="meeBulkTime" targetcontrolid="txtBulkTime"
                                                autocompletevalue="00:00" masktype="Time" mask="99:99">
                                            </ajaxtoolkit:maskededitextender>

                                            <ajaxtoolkit:calendarextender id="ceDateBulk" runat="server" targetcontrolid="txtDateBulk"
                                                format="dd/MM/yyyy" firstdayofweek="Monday" popupbuttonid="ibBulkDate">
                                            </ajaxtoolkit:calendarextender>
                                            <asp:RangeValidator ID="rvDateBulk" runat="server" ControlToValidate="txtDateBulk"
                                                ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>
                                            <asp:RegularExpressionValidator ID="revNumberBulk" ControlToValidate="txtNumberBulk"
                                                runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>



                            </td>
                        </tr>
                        <tr style="height: 15px;">
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkEditManyCancel" CssClass="btn" CausesValidation="false" OnClientClick="parent.$.fancybox.close();return false;"> <strong>Cancel</strong></asp:LinkButton>

                                           
                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkEditManyOK" CssClass="btn" CausesValidation="true"
                                                OnClick="lnkEditManyOK_Click"> <strong>OK</strong></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trUpdateEveryItem" style="display:none;">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 100px;"></td>
                                        <td>
                                            <asp:CheckBox Visible="true" runat="server" ID="chkUpdateEveryItem" Text="I would like to update EVERY item in this table."
                                                TextAlign="Right" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblMsgBullk" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:HiddenField runat="server" ID="hfEditManyValue" />
                                <asp:HiddenField runat="server" ID="hfChkAll"/>

                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>


        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

