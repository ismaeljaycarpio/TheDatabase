<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="FormSetWizard.aspx.cs" Inherits="Pages_Record_FormSetWizard" %>

<%@ Register Src="~/Pages/UserControl/DetailEdit.ascx" TagName="DetailEdit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="search" style="padding-bottom: 10px">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                    ShowMessageBox="true" ShowSummary="false" HeaderText="Please correct the following errors:" />
            </div>
            <div>
                <table>
                    <tr>
                        <td align="left" style="width: 500px;">
                            <asp:Label runat="server" ID="lblFormSetName" CssClass="TopTitle"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="lblHeaderName"></asp:Label>
                        </td>
                        <td align="left">
                            <table>
                                <tr>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hlBack" CssClass="btn" Visible="false"> 
                                        <strong>Back</strong> </asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkSaveForLater" CausesValidation="false"
                                         OnClick="lnkSaveForLater_Click"> Save for later </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkCancel" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkCancel_Click"> <strong>Cancel</strong> </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkPrev" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkPrev_Click"> <strong>&lt;&lt;&nbspPrev</strong> </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkNext" CssClass="btn" CausesValidation="true"
                                            OnClick="lnkNext_Click"> <strong>Next&nbsp&gt;&gt;</strong> </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkSubmit" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkSubmit_Click"> <strong>Submit</strong> </asp:LinkButton>
                                    </td>
                                     <td  style="padding-left:50px;">
                                        <asp:HyperLink runat="server" ID="hlPrint" CssClass="btn" Target="_blank" > 
                                        <strong>Print</strong> </asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="right">
                                        <asp:Label runat="server" ID="lblStep" Text="Step 1 of 6"></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="Panel2" runat="server">
                <div runat="server" id="divDynamic" class="NormalTextBox">
                    <ajaxToolkit:TabContainer ID="tabDetail" runat="server" ActiveTabIndex="0" CssClass="DBGTab"
                        Visible="true">
                    </ajaxToolkit:TabContainer>
                </div>
            </asp:Panel>
            <div>
                <table>
                    <tr>
                        <td align="left" style="width: 500px;">
                        </td>
                        <td align="left">
                            <table>
                                <tr>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hlBack2" CssClass="btn" Visible="false"> 
                                        <strong>Back</strong> </asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkSaveForLater2" CausesValidation="false" 
                                        OnClick="lnkSaveForLater_Click"> Save for later </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkCancel2" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkCancel_Click"> <strong>Cancel</strong> </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkPrev2" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkPrev_Click"> <strong>&lt;&lt;&nbspPrev</strong> </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkNext2" CssClass="btn" CausesValidation="true"
                                            OnClick="lnkNext_Click"> <strong>Next&nbsp &gt;&gt;</strong> </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lnkSubmit2" CssClass="btn" CausesValidation="false"
                                            OnClick="lnkSubmit_Click"> <strong>Submit</strong> </asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="right">
                                        <asp:Label runat="server" ID="lblStep2" Text="Step 1 of 6"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<asp:DetailEdit runat="server" ID="deOne"  />--%>
    <div style="width: 40px; height: 40px;">
        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <table style="width: 100%; text-align: center">
                    <tr>
                        <td>
                            <asp:Image ID="Image2" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                        </td>
                    </tr>
                </table>
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
    </div>
</asp:Content>
