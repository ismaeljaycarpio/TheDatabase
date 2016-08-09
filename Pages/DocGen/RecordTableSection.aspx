<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="RecordTableSection.aspx.cs" Inherits="DocGen.Document.STRecordTableSection.Edit" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Pages/UserControl/RecordList.ascx" TagName="RecordList" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">

 <link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>

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
    <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="Record Table Section"></asp:Label>
    <asp:HiddenField runat="server" ID="hfRemoveSection" ClientIDMode="Static" Value="0" />
    <br />
    <span class="failureNotification">
        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="MainValidationSummary" runat="server" CssClass="failureNotification"
        ValidationGroup="MainValidationGroup" ShowSummary="false" ShowMessageBox="true"
        HeaderText="Please correct the following errors:" />
    <br />
    <div style="text-align: center;">
        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
            <ProgressTemplate>
                <div id="AJAXIndicator">
                    <asp:Image ID="Image2" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
    </div>
    <br />
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <table>
                <tr>
                    <td align="right" style="width: 120px;">
                      
                    </td>
                    <td align="left">
                       
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td align="right" style="width: 100px;">
                                    <strong></strong>
                                </td>
                                <td align="left" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <%--<div runat="server" id="div1">
                                                    <asp:LinkButton runat="server" ID="lbTest" CausesValidation="false" OnClick="lbTest_Click">
                                                        <asp:Image runat="server" ID="Image1" ImageUrl="~/App_Themes/Default/images/Refresh2.png"  ToolTip="Refresh Preview" />
                                                    </asp:LinkButton>
                                                </div>--%>
                                            </td>
                                            <td>
                                                <div runat="server" id="div2">
                                                    <asp:LinkButton runat="server" ID="SaveButton" OnClick="SaveButton_Click" ValidationGroup="MainValidationGroup">
                                                        <asp:Image runat="server" ID="ImageSave" ImageUrl="~/App_Themes/Default/images/Save.png"  ToolTip="Save" />
                                                    </asp:LinkButton>
                                                </div>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
         
                <tr runat="server" id="trTableTalbe">
                    <td align="right" style="width: 120px;">
                        <strong runat="server" id="stgTableCap">Table</strong>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="True" CssClass="NormalTextBox"
                            DataTextField="TableName" DataValueField="TableID" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTable"
                            CssClass="failureNotification" ErrorMessage="Table is required." ToolTip="Table is required."
                            ValidationGroup="MainValidationGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
              
                
                <tr>
                    <td align="right" colspan="3">
                                <asp:RecordList runat="server" ID="rlOne" PageType="p" ShowAddButton="true"
                                 ShowEditButton="true" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <strong></strong>
                    </td>
                    <td align="left" colspan="2">
                    </td>
                </tr>
            </table>
<%--        </ContentTemplate>
        <Triggers>
           
            <asp:AsyncPostBackTrigger ControlID="ddlTable" />
          
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
