<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="DisplayContent.aspx.cs" 
Inherits="Page_DisplayContent"  MasterPageFile="~/Home/Marketing2.master" %>
<%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>

  <asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    
    
     
             <table class="ContentMain" style="width:970px;">
             
              <tr>
                    <td width="28"  > </td>
                    <td  >
                        <h1> <asp:Label runat="server" ID="lblHeading"></asp:Label>  </h1>       
                        
                    </td>
                    <td width="28"  >  </td>
                </tr>
                
                
                  <tr>
                    <td width="28"  >
                        </td>
                    <td >
                               
                         <div>
                                    <div style="text-align:right;width:100%;">                                            
                                                       
                                                            <table style="text-align:right;width:100%;">
                                                                <tr>
                                                                    <td align="left">
                                                                      <%--<DBGurus:DBGContent 
                                                                            id="dbgContentCommon" 
                                                                            runat="server" 
                                                                            connectionname="CnString" 
                                                                            contentkey="LoginScreenContent" 
                                                                            tablename="Content" 
                                                                            ExtenderPath="Extender/" 
                                                                            ShowInlineContentEditor="false" 
                                                                            UseAssetManager="true" />--%>
                                                                            <asp:Label runat="server" ID="lblContentCommon"></asp:Label>                                                                
                                                                      </td>
                                                                 </tr>
                                                                 
                                                            </table>                              
                                                     
                                                     </div>                                 
                            </div>
                           
                    </td>
                     <td width="28"  >  </td>
                </tr>
                
                 <tr>
                <td width="28"  > </td>
                    <td align="center" height="10px">
                                                 
                             
                   </td>
                    <td width="28"  > </td>
                </tr> 
                
                <tr>
                 <td width="28"  > </td>
                    <td align="left">
                    
                          <div>
                           
                                        <asp:LinkButton runat="server" ID="lnkBack" onclick="lnkBack_Click" 
                                        CssClass="btn" CausesValidation="false"> <strong>Back </strong> </asp:LinkButton>
                                        
                            
                         </div>
                        
                    </td>
                     <td width="28"  > </td>
                </tr>
                                                                             
                <tr>
                <td width="28"  > </td>
                    <td align="center" height="40px">
                                                 
                             
                   </td>
                    <td width="28"  > </td>
                </tr> 
                
             </table>   
             
                
   
    
    </asp:Content>

