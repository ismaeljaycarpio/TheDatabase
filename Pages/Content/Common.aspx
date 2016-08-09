<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="Common.aspx.cs" Inherits="Pages_Content_Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
<table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">

 <tr>
                    <td width="28"  > </td>
                    <td  >
                        <h1> <asp:Label runat="server" ID="lblHeading"></asp:Label>  </h1>       
                        
                    </td>
                    <td width="28"  >  </td>
                </tr>
                
    <tr>
         <td width="28"  > </td>
          <td valign="top" align="center">
      
       <%--<DBGurus:DBGContent 
    id="dbgContentCommon" 
    runat="server" 
    connectionname="CnString" 
    contentkey="CopyTableHelp" 
    tablename="Content" 
    ExtenderPath="../../Extender/" 
    ShowInlineContentEditor="true" 
    UseAssetManager="true" />--%>
         <asp:Label runat="server" ID="lblContentCommon"></asp:Label>     
        </td>
         <td width="28"  > </td>
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

