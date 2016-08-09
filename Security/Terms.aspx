<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Terms.aspx.cs" Inherits="Security_Terms" %>
<%@ Register Assembly="DBGWebControl" Namespace="DBGWebControl" TagPrefix="DBGurus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ETS - Terms and Conditions</title><meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
   
    <link rel="shortcut icon" href="../Images/favicon.ico" /
</head>
<body>
    <form id="form2" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    
     <div id="SkinOutline">
     
     <div id="SkinContent">
    
      <div id="Header">
     <table class="Container" border="0" cellpadding="0" cellspacing="0">
         <tr>
              <td class="Logo" rowspan="2">
                     <br /> 
                <a href="/">
                <asp:Image ID="imgLogo" runat="server"  SkinID="logo" />
                
                </a>
               </td>
                <td >
                       
                </td>
         </tr>
         <tr>
         <td></td><td></td>
         </tr>
    </table>
    
    </div>
    
     <div id="Content" class="ContentMain" style="width:1000px;">
      
             <table border="0" align="left"  cellpadding="0" cellspacing="0" width="100%" >
                <tr>
                    <td style="width:25px"></td>
                     <td height="30"></td>
                </tr>
                <tr>
                <td style="width:25px"></td>
                    <td align="center"></td>
                </tr>
                                                                                              
                <tr>
                <td style="width:25px"></td>
               
                    <td align="left" height="40px">
                                                   
                         <div style="text-align:left">
                                <DBGurus:DBGContent 
                                    id="DBGWelcomeContent" 
                                    runat="server" 
                                    connectionname="CnString" 
                                    contentkey="DBGTermsConditions" 
                                    tablename="Content" 
                                    ExtenderPath="Extender/" 
                                    ShowInlineContentEditor="false" 
                                    UseAssetManager="true" />
                        </div>     
                   </td>
                </tr> 
                
             </table>   
             <br /><br /><br /><br /><br /><br /><br /><br /><br /><br />          
    </div>
    
     </div>
      <div id="Footer">
            <table class="Container" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="Copyright">
			            Copyright © 2009 ETS. All rights reserved.
                    </td>
                    <td >

                    </td>

                </tr>
            </table>   
    </div>
    
    
    
    </div>
    
    </form>
</body>
</html>


