<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ErrorLogDetail.aspx.cs" Inherits="Pages_Security_AccountDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle"></asp:Label>
                </span>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
                <asp:Panel ID="Panel2" runat="server">
                    <div runat="server" id="divDetail">
                        <table cellpadding="3">
                            <tr>
                                <td align="right" style="width: 100px">
                                    <strong>Module:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblModule"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Date/Time:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblErrorTime"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Error Message:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblErrorMessage" ForeColor="Red" Width="700px"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 10px">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Error Track:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblErrorTrack" Width="700px"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 10px">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>Error Path:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblErrorPath"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <%--<asp:Label runat="server" ID="lblMsg" ForeColor="Red" ></asp:Label>--%>
                    <br />
                    <%--<asp:ImageButton ID="cmdBack" runat="server" ImageUrl="~/App_Themes/Default/Images/btnBack.png"
                                CausesValidation="false" onclick="cmdBack_Click"  />--%>
                    <div>
                        
                                  <asp:HyperLink runat="server" ID="hlBack"  CssClass="btn" > <strong>Back</strong> </asp:HyperLink>
                              
                    </div>
                </asp:Panel>
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
