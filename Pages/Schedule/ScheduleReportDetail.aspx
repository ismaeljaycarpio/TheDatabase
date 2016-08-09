<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="ScheduleReportDetail.aspx.cs" Inherits="Pages_Schedule_ScheduleReportDetail" %>

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
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                            <asp:HiddenField runat="server" ID="hfDocumentTypeID" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">
                                    <tr>
                                        <td align="right">
                                            <strong>Report</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlReport" DataValueField="DocumentID" DataTextField="DocumentText"
                                                CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvReport" runat="server" ControlToValidate="ddlReport"
                                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Frequency</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlFrequency" CssClass="NormalTextBox" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged">
                                                <asp:ListItem Text="Monthly" Value="M"></asp:ListItem>
                                                <asp:ListItem Text="Weekly" Value="W"></asp:ListItem>
                                                <asp:ListItem Text="Daily" Value="D"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>When</strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlWhen" CssClass="NormalTextBox">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <strong>Reporting Period</strong>
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="border: 0px;">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtReportPeriod" CssClass="NormalTextBox" Width="30px"></asp:TextBox>
                                                    </td>
                                                    <td style="padding-left: 3px;">
                                                        <asp:DropDownList runat="server" ID="ddlTimePeriod" AutoPostBack="false" CssClass="NormalTextBox">
                                                            <asp:ListItem Text="Hour" Value="H"></asp:ListItem>
                                                            <asp:ListItem Text="Day" Value="D"></asp:ListItem>
                                                            <asp:ListItem Text="Week" Value="W"></asp:ListItem>
                                                            <asp:ListItem Text="Month" Value="M"></asp:ListItem>
                                                            <asp:ListItem Text="Year" Value="Y" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtReportPeriod"
                                                            runat="server" ErrorMessage="Numeric!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                                        </asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="top">
                                            <strong>Email Reports</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEmails" TextMode="MultiLine" Width="300px" Height="60px"></asp:TextBox>
                                            <br />
                                            Enter email addresses separated with semi-colons.
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                            <br />
                            <div>
                                <table>
                                    <tr>
                                       
                                        <td>
                                            <div>
                                                <asp:HyperLink runat="server" ID="hlBack" CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false">
                                                <asp:HyperLink runat="server" ID="hlEditLink" CssClass="btn"> <strong>Edit</strong> </asp:HyperLink>
                                            </div>
                                        </td>
                                         <td>
                                            <div runat="server" id="divSave">
                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                    CausesValidation="true"> <strong>Save</strong> </asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
