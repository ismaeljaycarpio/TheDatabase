<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="RoleDetail.aspx.cs" 
Inherits="Pages_Security_RoleDetail" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
<table border="0" cellpadding="0" cellspacing="0" width="928" align="center">
        <tr>
            <td colspan="3" height="40">
                <span  class="TopTitle">
                    <asp:Label runat="server" ID="lblTitle"></asp:Label></span>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
                
            </td>
        </tr>
         <tr>
         <td colspan="3">
    
    <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <table style="width:100%; text-align:center">
                <tr>
                    <td> <img alt="Processing..." src="../../Images/ajax.gif" /> </td>
                </tr>
                </table>
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
    </td>
    </tr>
        <tr>
            <td valign="top" >
                
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                           
                        </div>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSave">
                                                      
                            <div runat="server" id="divDetail"  >
                            
                            
                            <table>
                            <tr>
                                <td align="right"><strong>Role Name*</strong></td>
                                <td>
                                  <asp:TextBox ID="txtRole" runat="server" Width="250px" CssClass="NormalTextBox"></asp:TextBox> 
                                  <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="txtRole" 
                                    ErrorMessage="Role Name - Required"  ></asp:RequiredFieldValidator>
                                  
                                   </td>
                              </tr>                                             
                               
                               <tr>
                                <td align="right"><strong>Role Type*</strong></td>
                                <td>
                                  <asp:TextBox ID="txtRoleTye" runat="server" Width="50px" CssClass="NormalTextBox"></asp:TextBox> 
                                  <asp:RequiredFieldValidator ID="rfvRoleTye" runat="server" ControlToValidate="txtRoleTye" 
                                    ErrorMessage="Role Type - Required"  ></asp:RequiredFieldValidator>
                                  
                                   </td>
                              </tr>     
                              
                              <tr>
                                <td align="right"><strong>Notes</strong></td>
                                <td>
                                  <asp:TextBox ID="txtRoleNotes" runat="server" TextMode="MultiLine" Height="50px" Width="250px" CssClass="MultiLineTextBox"></asp:TextBox> 
                                  
                                   </td>
                              </tr>                              
                             

                            
                            </table>
                            </div>                    
                            
                            
                            
                                                   
                            
                            <br />
                             <asp:Label runat="server" ID="lblMsg" ForeColor="Red" ></asp:Label>
                              <br />
                            <%--<asp:ImageButton ID="cmdSave" runat="server" ImageUrl="~/App_Themes/Default/Images/btnSave.png"
                                CausesValidation="true" onclick="cmdSave_Click"  />--%>
                            &nbsp;
                            <%--<asp:ImageButton ID="cmdBack" runat="server" ImageUrl="~/App_Themes/Default/Images/btnBack.png"
                                CausesValidation="false" onclick="cmdBack_Click"  />--%>
                                 <div>
                                    <table>
                                     <tr>
                                        <td>
                                             <div runat="server" id="divSave">
                                                 <table id="Table1" cellpadding="0" cellspacing="0"  >
                                                    <tr>
                                                        <td class="bL">&nbsp;</td>
                                                        <td class="bC">
                                                            <asp:LinkButton runat="server" ID="lnkSave" CssClass="ButtonLink" 
                                                                onclick="lnkSave_Click" CausesValidation="true"> Save</asp:LinkButton>
                                                            
                                                            </td>
                                                        <td class="bR">&nbsp;</td>
                                                    </tr>
                                                </table>   
                                             </div>
                                     
                                         </td>
                                 
                                         <td>
                                 
                                            <div>
                                                <table cellpadding="0" cellspacing="0"  >
                                                    <tr>
                                                        <td class="bL">&nbsp;</td>
                                                        <td class="bC">
                                                            <asp:LinkButton runat="server" ID="lnkBack" onclick="lnkBack_Click" 
                                                            CssClass="ButtonLink" CausesValidation="false"> Cancel</asp:LinkButton>
                                                            
                                                            </td>
                                                        <td class="bR">&nbsp;</td>
                                                    </tr>
                                                </table>    
                                             </div>
                                        </td>
                                        <td>
                                        
                                        <div runat="server" id="divEdit" visible="false">
                                                 <table id="Table2" cellpadding="0" cellspacing="0"  >
                                                    <tr>
                                                        <td class="bL">&nbsp;</td>
                                                        <td class="bC">
                                                           <asp:HyperLink runat="server" ID="hlEditLink" Text="Edit" CssClass="ButtonLink" ></asp:HyperLink>
                                                            
                                                            </td>
                                                        <td class="bR">&nbsp;</td>
                                                    </tr>
                                                </table>   
                                             </div>
                                        
                                        </td>
                                    </tr>
                                 </table>
                                
                                </div>
                        </asp:Panel>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>
                <span style="font-weight: bold" align="center"></span>
            </td>
            <td >
               
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
               
            </td>
        </tr>
    </table>


</asp:Content>

