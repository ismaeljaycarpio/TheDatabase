<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdvancedFilter.aspx.cs"
 Inherits="Pages_Record_AdvancedFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reference</title>
   <style>
   
ul.biblica_sections,
ul.biblica_chapters
{
    line-height:24px;
    color:#333;
    list-style-image:url(smoothness/images/icon_external.gif);
}

ul.biblica_sections a,
ul.biblica_chapters a
{
    cursor:pointer;
}

ul.biblica_sections a:hover,
ul.biblica_chapters a:hover
{
    text-decoration:underline;
}

ul.biblica_verses
{
    color:#333;
    list-style-image:url(smoothness/images/speech.png);
    padding-left:26px;
}

ul.biblica_verses li
{
    margin-bottom:8px;
}

div.biblica_verses
{
    line-height:1.6;
    margin-top:10px;
    padding-right:10px;
}
.biblica_verses label.nValue
{
    display:inline-block;
    margin-right:3px;    
    font-weight:bold;
    font-size:12px;
    color:#000;
}
.biblica_verses span
{
    margin-right:8px;
    color:#333;
}
.biblica_verses .section-head
{
    display:block;
    font-weight:bold;
    font-size:14px;
    margin
}
   
   </style>
</head>
<body style="background:white;">
 <script type="text/javascript" src="<%=ResolveUrl("~/script/jquery.js")%>"></script>


    <form id="form1" runat="server">
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
                                                                onclick="lnkSave_Click" CausesValidation="true"> Ok</asp:LinkButton>
                                                            
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
                                                            <asp:LinkButton runat="server" ID="lbCancel" 
                                                            CssClass="ButtonLink" onclick="lbCancel_Click" >Cancel</asp:LinkButton>
                                                                                                                       
                                                            </td>
                                                        <td class="bR">&nbsp;</td>
                                                    </tr>
                                                </table>    
                                             </div>
                                        </td>
                                          
                                    </tr>
                                 </table>
                                
                                </div>

    </form>
</body>
</html>
