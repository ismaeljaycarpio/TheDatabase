<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sendpayment_recurring.aspx.cs" Inherits="sendpayment_recurring" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ETS Monthly Service</title>
</head>
<body>
  
    <form action="<%=SecurityManager.GetPaypalActionURL()%>" method="post"
     id="form1" name="form1">
    <input type="hidden" name="cmd" value="_xclick-subscriptions"/> 
    <input type="hidden" name="business" value="<%=SecurityManager.GetPaypalReceiverEmail()%>"/> 
    <input type="hidden" name="item_name" value="ETS Monthly Service"/> 
    <%--<input type="submit" value="Monthly Subscribe!" />--%> 
    <input type="hidden" name="notify_url" value="<%=Getnotify_urlURL()%>"/>

    <%--<input type="hidden" name="a1" value="0"/>   --%> 
    <%--<input type="hidden" name="p1" value="1"/>--%> 
    <%--<input type="hidden" name="t1" value="D"/>--%> 
    <input type="hidden" name="a3" value="<%=Session["PaymentAmount"]%>"/> 
    <input type="hidden" name="p3" value="1"/> 
    <input type="hidden" name="t3" value="M"/> 
    <input type="hidden" name="src" value="1"/> 
    <%--<input type="hidden" name="srt" value="10"/>--%> 
    <input type="hidden" name="sra" value="1"/> 
    <input type="hidden" name="custom" value="<%=Session["InvoiceID"]%>"/> 

    <input type="hidden" name="return" value="<%=GetReturnURL()%>"/><!--this page will be your redirection page -->
    <input type="hidden" name="cancel_return" value="<%=Getcancel_returnURL()%>"/>

     <input type="hidden" name="currency_code" value="AUD"/>
      <input type="hidden" name="no_shipping" value="1">


</form>
      
    <script language="javascript" type="text/javascript">
   document.form1.submit();   
    </script>

</body>
</html>
