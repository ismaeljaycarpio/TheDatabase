<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="MultipleSheetsNewTable.aspx.cs" Inherits="Pages_Record_MultipleSheetsNewTable"
    EnableTheming="true" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

 <script language="javascript" type="text/javascript">
     $(document).ready(function () {

         
         ValidatorEnable(document.getElementById('rfvNewMenuName'), true);
         $('#ctl00_HomeContentPlaceHolder_ddlMenu').change(function (e) {
             if (document.getElementById("ctl00_HomeContentPlaceHolder_ddlMenu").value == 'new') {
                 $("#trNewMenuName").fadeIn();
                 
                 ValidatorEnable(document.getElementById('rfvNewMenuName'), true);
             }
             else {
                 $("#trNewMenuName").fadeOut();
                 
                 ValidatorEnable(document.getElementById('rfvNewMenuName'), false);
             }
         });

         if (document.getElementById("ctl00_HomeContentPlaceHolder_ddlMenu").value == 'new') {
         }
         else {
             ValidatorEnable(document.getElementById('rfvNewMenuName'), false);
         }

     });


     function ShowHide() {
         
             if (document.getElementById("ctl00_HomeContentPlaceHolder_ddlMenu").value == 'new') {
                 $("#trNewMenuName").fadeIn();
                 ValidatorEnable(document.getElementById('rfvNewMenuName'), true);
             }
             else {
                 $("#trNewMenuName").fadeOut();
                 ValidatorEnable(document.getElementById('rfvNewMenuName'), false);
             }
         
     }

     if (window.addEventListener)
         window.addEventListener("load", ShowHide, false);
     else if (window.attachEvent)
         window.attachEvent("onload", ShowHide);
     else if (document.getElementById)
         window.onload = ShowHide;


    </script>


   
    <table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3" height="40">
                <span class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle" Text="Multiple Sheets file" Visible="true"></asp:Label></span>
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
                <div id="search" style="padding-bottom: 10px">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                        ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div runat="server" id="divDetail">
                        <table cellpadding="3">
                         
                              <tr>
                                <td align="right">
                                    <strong>File Name:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblExcelFileName" > </asp:Label>
                                </td>
                            </tr>

                            <tr style="height:10px;"><td colspan="2"></td></tr>
                            
                            <tr>
                                <td align="right">
                                    <strong>Table Name:</strong>
                                </td>
                                <td>
                                     <asp:Label runat="server" ID="lblTable" > </asp:Label>
                                </td>
                            </tr>
                          
                           <tr>
                                <td align="right">
                                    <strong>Updload Data Now:</strong>
                                </td>
                                <td>
                                     <asp:Label runat="server" ID="lblRecordsData" > </asp:Label>
                                     <asp:HiddenField  runat="server" ID="hfRecordsData" Value=""/>
                                </td>
                            </tr>
                            <tr style="height:10px;"><td colspan="2"></td></tr>
                             <tr runat="server" visible="false">
                                <td align="right">
                                    <strong>Column Header Row:</strong>
                                </td>
                                <td>
                                     <asp:Label runat="server" ID="lblImportColumnHeaderRow" > </asp:Label>
                                </td>
                            </tr>
                             <tr runat="server" visible="false">
                                <td align="right">
                                    <strong>Data Start Row:</strong>
                                </td>
                                <td>
                                     <asp:Label runat="server" ID="lblImportDataStartRow" > </asp:Label>
                                </td>
                            </tr>
                            <tr style="height:10px;"><td colspan="2"></td></tr>
                             <tr>
                                <td align="right">
                                    <strong>Show under Menu:</strong>
                                </td>
                                <td>
                                    <%-- <asp:Label runat="server" ID="lblShowunderMenu" > </asp:Label>
                                     <asp:HiddenField  runat="server" ID="hfShowMenu" value=""/>--%>

                                      <asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="false" DataTextField="MenuP"
                                                        Width="155px" DataValueField="MenuID" CssClass="NormalTextBox">
                                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <tr  id="trNewMenuName">
                                <td align="right">
                                    <strong>New Menu Name:</strong>
                                </td>
                                <td>
                                     <%--<asp:Label runat="server" ID="lblNewMenuName" > </asp:Label>--%>
                                      <asp:TextBox ID="txtNewMenuName" runat="server" Width="256px" CssClass="NormalTextBox"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvNewMenuName" ControlToValidate="txtNewMenuName"
                                                        ClientIDMode="Static" ErrorMessage="New Menu Name - Required" CssClass="NormalTextBox"
                                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                          
                              <tr style="height:15px;"><td colspan="2"></td></tr>

                              <tr id="trSheetNames" runat="server">
                                <td align="right">
                                       <strong>Select a sheet: </strong>
                                </td>
                                <td>
                                    
                                    <asp:DropDownList ID="ddlSheetNames" runat="server" CssClass="NormalTextBox">

                                     </asp:DropDownList>
                                      
                                </td>
                            </tr>
                          
                        </table>
                    </div>
                    
                   
                    <div>
                        
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                               
                                                 <asp:HyperLink runat="server" ID="hlBack" 
                                                 CssClass="btn"> <strong>Back</strong> </asp:HyperLink>
                                                     
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                               
                                            <asp:LinkButton runat="server" ID="lnkNext" CssClass="btn" OnClick="lnkNext_Click"
                                                CausesValidation="true"> <strong>Next</strong></asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                <br />
                           
                        
                    </div>
                </asp:Panel>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            <asp:HiddenField runat="server" ID="hfguidNew" /> 
            <asp:HiddenField runat="server" ID="hfFileExtension" />            
            </td>
        </tr>
    </table>
</asp:Content>
