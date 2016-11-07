<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master"
 AutoEventWireup="true" CodeFile="ViewItemDetail.aspx.cs" Inherits="Pages_Record_ViewItemDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
     <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>

     
     <%-- <asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upCommon">
        <ProgressTemplate>
            <table style="width:100%;  height:100%; text-align: center;" >
                <tr valign="middle">
                    <td>
                        <p style="font-weight:bold;"> Please wait...</p>
                        <asp:Image ID="ImageProcessing" runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                    </td>
                </tr>
            </table>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    
    <br /> 

    <div style="width:100%;">
        <asp:UpdatePanel ID="upCommon" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <table>
                        
                        <tr>
                            <td colspan="3">
                                <asp:Label runat="server" ID="lblDetailTitle" Font-Size="16px"
                                 Font-Bold="true" Text="View Columns"> </asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <strong>Not Selected:</strong><br />
                                <asp:ListBox runat="server" ID="lstNotUsed" style="min-width:250px;" Height="150px" 
                                SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td align="center" style="width: 150px;">
                                
                                <br />
                                <asp:LinkButton runat="server" ID="lnkAdd" OnClick="lnkAdd_Click" > 
                               
                               <asp:Image  runat="server" ID="imgAdd" ImageUrl="~/App_Themes/Default/Images/arrow_right.png"/>
                                </asp:LinkButton>
                                <br />
                                <br />   
                                <asp:LinkButton runat="server" ID="lnkRemove" OnClick="lnkRemove_Click" > 
                                            <asp:Image  runat="server" ID="imgRemove" ImageUrl="~/App_Themes/Default/Images/arrow_left.png"/>
                                </asp:LinkButton>
                            </td>
                            <td align="left" valign="top">
                                    <strong>Selected:</strong><br />
                                <asp:ListBox runat="server" ID="lstUsed"  style="min-width:250px;"  Height="150px" 
                                SelectionMode="Multiple"></asp:ListBox>

                            </td>
                        </tr>
                        <tr >
                            <td>
                            </td>
                            <td align="left" style="padding-top:50px; padding-left:50px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkSaveNew" CssClass="btn" CausesValidation="true"
                                                OnClick="lnkSaveNew_Click"> <strong>Save</strong></asp:LinkButton>
                                        </td>
                                        <td style="padding-left:10px;">
                                            <asp:LinkButton runat="server" ID="lnkCancel" CssClass="btn" CausesValidation="false"
                                                OnClientClick="parent.$.fancybox.close();return false;"> 
                                      <strong>Cancel</strong></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                                                                               
                                     
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <%--<div>
                    <table>
                        <tr>
                            <td align="right">
                                <strong>Field Name:</strong>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlColumn" runat="server" DataTextField="DisplayName" Width="255px"
                                    DataValueField="ColumnID" CssClass="NormalTextBox">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvColumn" runat="server" ControlToValidate="ddlColumn"
                                    ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <strong>Heading:</strong>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtHeading" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHeading"
                                    ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td align="left">
                                <asp:CheckBox runat="server" ID="chkSearchField" Text="Search Field" TextAlign="Right"
                                    Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkFilterField" Text="Filter Field" TextAlign="Right"
                                    Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <strong>Alignment</strong>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlAlignment" CssClass="NormalTextBox">
                                    <asp:ListItem Value="left" Text="Left" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="right" Text="Right"></asp:ListItem>
                                    <asp:ListItem Value="center" Text="Center"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <strong>Width:</strong>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtWidth" CssClass="NormalTextBox"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revWidth" ControlToValidate="txtWidth" ValidationGroup="MKE"
                                    runat="server" ErrorMessage="Numeric value please!" Display="Dynamic" ValidationExpression="(^-?\d{1,20}\.$)|(^-?\d{1,20}$)|(^-?\d{0,20}\.\d{1,10}$)">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkShowTotal" Text="Show Total" TextAlign="Right"
                                    Font-Bold="true" />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div runat="server" id="div1" style="padding-left: 10px;">
                                                <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn" CausesValidation="true"
                                                    OnClick="lnkSave_Click"> <strong>Save</strong></asp:LinkButton>
                                            </div>
                                        </td>
                                        <td>
                                            <div runat="server" id="div2" style="padding-left: 10px;">
                                                <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                                    CssClass="btn" CausesValidation="false"> <strong>Cancel</strong></asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
      <script type="text/javascript">

          function CloseAndRefresh() {
              window.parent.document.getElementById('btnRefreshViewItem').click();
              parent.$.fancybox.close();
              // alert('ok');
          }
    
    </script>

</asp:Content>

