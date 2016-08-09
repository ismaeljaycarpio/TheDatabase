<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowWhenCondition.ascx.cs"
    Inherits="Pages_UserControl_ShowWhenCondition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/ColumnUI.ascx" TagName="cUI" TagPrefix="dbg" %>

<div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfShowWhenID" Value="" />
            <table>
                <tr runat="server" id="trJoinOperator">
                    <td style="width: 50px;">
                        <asp:DropDownList runat="server" ID="ddlJoinOperator" CssClass="NormalTextBox" ClientIDMode="Static">
                            <asp:ListItem Text="" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="AND" Value="and"></asp:ListItem>
                            <asp:ListItem Text="OR" Value="or"></asp:ListItem>

                        </asp:DropDownList>
                    </td>
                    <td colspan="5"></td>
                </tr>
                <tr style="text-align: left;">
                    <td style="width: 50px;"></td>
                    <td valign="top">
                        <asp:DropDownList runat="server" ID="ddlTable" CssClass="NormalTextBox" ClientIDMode="Static"
                            DataValueField="TableID" DataTextField="TableName" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTable_SelectedIndexChanged" Visible="false">
                        </asp:DropDownList>

                    </td>
                    <td align="right" valign="top" style="width: 60px;">
                        <strong runat="server" id="stgField">Field: </strong>
                    </td>
                    <td valign="top">
                        <asp:DropDownList runat="server" ID="ddlHideColumn" CssClass="NormalTextBox" ClientIDMode="Static"
                            DataValueField="ColumnID" DataTextField="DisplayName" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlHideColumn_SelectedIndexChanged">
                        </asp:DropDownList>

                    </td>
                    <td valign="top">
                        <asp:DropDownList runat="server" ID="ddlOperator" AutoPostBack="true" OnSelectedIndexChanged="ddlOperator_SelectedIndexChanged" 
                            CssClass="NormalTextBox" ClientIDMode="Static">
                            <asp:ListItem Value="equals" Text="Equals" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="notequal" Text="Not Equals"></asp:ListItem>
                            <asp:ListItem Value="greaterthan" Text="Greater Than"></asp:ListItem>
                            <asp:ListItem Value="greaterthanequal" Text="Greater Than Equal"></asp:ListItem>
                            <asp:ListItem Value="lessthan" Text="Less Than"></asp:ListItem>
                            <asp:ListItem Value="lessthanequal" Text="Less Than Equal"></asp:ListItem>

                            <asp:ListItem Value="contains" Text="Contains"></asp:ListItem>
                            <asp:ListItem Value="notcontains" Text="Does Not Contain"></asp:ListItem>
                             <asp:ListItem  Value="empty" Text="Is Empty"></asp:ListItem>  
                            <asp:ListItem Value="notempty" Text="Is Not Empty" ></asp:ListItem> 

                        </asp:DropDownList>

                        <%--<asp:ListItem Value="isblank" Text="Is Blank"></asp:ListItem>
                          <asp:ListItem Value="isnotblank" Text="Is Not Blank"></asp:ListItem>--%>


                    </td>
                    <td align="left" valign="top">
                        <%--<asp:TextBox runat="server" ID="txtHideColumnValue" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                        <asp:ListBox runat="server" ID="lstHideColumnValue" Visible="false" SelectionMode="Multiple"
                            Style="min-width: 200px; min-height: 100px;"></asp:ListBox>
                        <asp:DropDownList runat="server" ID="ddlHideColumnValue" Visible="false" CssClass="NormalTextBox">
                        </asp:DropDownList>
                        <asp:CheckBox runat="server" ID="chkHideColumnValue" Visible="false" TextAlign="Right" />
                        <asp:RadioButtonList runat="server" ID="radioHideColumnValue" Visible="false"
                            CssClass="NormalTextBox" RepeatDirection="Horizontal">
                        </asp:RadioButtonList>--%>
                        <dbg:cUI runat="server" ID="cuiHideColumnValue" />
                        <asp:HiddenField runat="server" ID="hfHideColumnValue" Value="" ClientIDMode="Static" />
                    </td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
