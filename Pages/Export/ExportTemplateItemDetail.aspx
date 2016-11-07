<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master"
 AutoEventWireup="true" CodeFile="ExportTemplateItemDetail.aspx.cs" Inherits="Pages_Export_ExportTemplateItemDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
     
     
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
                                 Font-Bold="true" Text="Export Columns"> </asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <strong>Not Selected:</strong><br />
                                <asp:ListBox runat="server" ID="lstNotUsed" style="min-width:250px;" Height="280px" 
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
                                <asp:ListBox runat="server" ID="lstUsed"  style="min-width:250px;"  Height="280px" 
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
              
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
      <script type="text/javascript">

          function CloseAndRefresh() {
              window.parent.document.getElementById('btnRefreshExportTemplateItem').click();
              parent.$.fancybox.close();
              // alert('ok');
          }
    
    </script>

</asp:Content>

