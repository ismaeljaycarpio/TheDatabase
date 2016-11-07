<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="FormSetFormDetail.aspx.cs" Inherits="Pages_Record_FormSetFormDetail" %>

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
    <table border="0" cellpadding="0" cellspacing="0" align="center">
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
                                <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress2" runat="server">
                                    <ProgressTemplate>
                                        <table style="width: 100%; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>--%>
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
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                            <div runat="server" id="divDetail" onkeypress="abc();">
                                <table cellpadding="3">
                                  <tr>
                                        <td align="right">
                                            <strong>Form Set</strong>
                                        </td>
                                        <td>
                                           
                                           <asp:DropDownList runat="server" ID="ddlFormSet"  CssClass="NormalTextBox"
                                            DataValueField="FormSetID" DataTextField="FormSetName" Enabled="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td align="right">
                                            <strong>Table*</strong>
                                        </td>
                                        <td>
                                          <asp:DropDownList runat="server" ID="ddlTable"  CssClass="NormalTextBox" AutoPostBack="true"
                                            DataValueField="ChildTableID" DataTextField="ChildTableName" OnSelectedIndexChanged="ddlTable_OnSelectedIndexChanged"></asp:DropDownList>

                                             <asp:RequiredFieldValidator ID="rfvTable" runat="server" ControlToValidate="ddlTable"
                                                ErrorMessage="Table - Required"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                      <tr>
                                        <td align="right">
                                            <strong>On Completing the Page Update the Column</strong>
                                        </td>
                                        <td>
                                              <asp:DropDownList runat="server" ID="ddlUpdateColumn"  CssClass="NormalTextBox"
                                            DataValueField="ColumnID" DataTextField="DisplayName"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trUpdateColumnValue">
                                        <td align="right">
                                            <strong>Update Column Value</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUpdateColumnValue" runat="server" Width="150px" CssClass="NormalTextBox"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                    <tr runat="server" visible="false">
                                        <td align="right">
                                            <strong>Incomplete Image</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIncompleteImage" runat="server" Width="350px"
                                             CssClass="NormalTextBox"  Text="/App_Themes/Default/Images/incomplete.png"></asp:TextBox>
                                           
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
                                            <div runat="server" id="divSave">
                                                
                                                            <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" OnClick="lnkSave_Click"
                                                                CausesValidation="true"> <strong>Save</strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                               
                                                            <%--<asp:HyperLink runat="server" ID="hlBack"  CssClass="btn"> <strong>Back</strong> </asp:HyperLink>--%>
                                                              <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                                    CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="divEdit" visible="false">
                                                
                                                            <asp:HyperLink runat="server" ID="hlEditLink" CssClass="btn"> <strong>Edit</strong> </asp:HyperLink>
                                                       
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

     <script type="text/javascript">

         function CloseAndRefresh() {
             window.parent.document.getElementById('btnRefreshPages').click();
             parent.$.fancybox.close();
             // alert('ok');
         }
    
    </script>
</asp:Content>
