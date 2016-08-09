<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailValidation.aspx.cs" 
Inherits="Security_EmailValidation"  MasterPageFile="~/Home/Marketing2.master" %>

 <asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    
     <div id="Content" class="ContentMainTra" >
      
             <table border="0" align="left"  cellpadding="0" cellspacing="0" style="width:970px;">
                <tr>
                    <td height="30"></td>
                </tr>
                <tr>
                    <td align="center"></td>
                </tr>
                                                                                              
                <tr>
               
                                                   
                         <div style="text-align:center">
                                <asp:Label runat="server" ID="lblMessage" ForeColor="Red"></asp:Label>
                                 <br /><br />
                                <div runat="server" id="divLogIn">           
                                    To log in please click <asp:HyperLink runat="server" ID="hpLogIn" NavigateUrl="~/Login.aspx">here</asp:HyperLink>
                                </div>
                        </div>     
                   </td>
                </tr> 
                
             </table>   
             <br /><br /><br /><br /><br /><br /><br /><br /><br /><br />          
    </div>
    
   </asp:Content> 


