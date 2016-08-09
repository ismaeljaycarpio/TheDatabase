<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" 
    CodeFile="ResetValues.aspx.cs" Inherits="Pages_SystemData_ResetValues" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

    <div style="width: 40px; height: 40px;">
        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
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

    <br />


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 20px;">
                <span class="TopTitle">Reset Calculation values</span>
                <br />
                <asp:Label runat="server" ID="lblErrorMsg" ForeColor="Red"></asp:Label>
                <br />

                <table>
                    <tr>
                        <td align="left" valign="top">
                            <strong>Please select table(s)</strong>
                            <br />
                            <asp:CheckBoxList runat="server" ID="lstTable" AutoPostBack="false"
                                Style="display: block; overflow: auto; min-width: 300px; max-width: 600px; min-height: 500px; border: solid 1px #909090;">
                            </asp:CheckBoxList>
                            <br />
                            <br />
                            
                             <asp:LinkButton runat="server" ID="lnkShowAllTables" CssClass="btn" OnClick="lnkShowAllTables_Click"
                                ValidationGroup="MKE"> <strong>Show all tables again</strong></asp:LinkButton>

                        </td>
                        <td align="left" valign="top" style="padding-left: 10px;">
                            <asp:LinkButton runat="server" ID="lnkReset" CssClass="btn" OnClick="lnkReset_Click"
                                ValidationGroup="MKE"> <strong>Reset Calculation Values</strong></asp:LinkButton>

                            <br /><br />
                              <%--<asp:LinkButton runat="server" ID="lnkResetNew" CssClass="btn" OnClick="lnkResetNew_Click"
                                ValidationGroup="MKE"> <strong>Reset (new)</strong></asp:LinkButton>--%>

                        </td>
                        <td align="left" valign="top" style="padding-left: 10px;">
                            <asp:Label runat="server" ID="lblMessage"></asp:Label>

                        </td>

                        <td align="left" valign="top" style="padding-left: 10px;">
                            <asp:LinkButton runat="server" ID="lnkClearMessage" CssClass="btn" OnClick="lnkClearMessage_Click"
                                 Visible="false"> <strong>Clear Message</strong></asp:LinkButton>
                        </td>
                       
                    </tr>

                </table>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

