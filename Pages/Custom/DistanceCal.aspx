<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="DistanceCal.aspx.cs" Inherits="Pages_Custom_DistanceCal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">
   <%-- <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
--%>

    <div style="padding: 10px 10px 10px 10px;">
     
        <br />
          Enter Volunteer RecordID (e.g. 543401, 543402, 543403, 543404, 543405, 543406): <br /> <asp:TextBox runat="server" ID="txtVolunteerRecordID" Width="200PX"></asp:TextBox>
               <asp:LinkButton runat="server" ID="lnkGetClosestDistance" Text="Get Closest distance" 
        OnClick="lnkGetClosestDistance_OnClick"></asp:LinkButton>


        <br />   <br /> <br />
          Volunteer Lat,Long:   <asp:Label runat="server" ID="lblVolLatLong"></asp:Label>

        <br />  <br />
        Closest Polling Distance:<asp:Label runat="server" ID="lblDistance"></asp:Label>
        <br />
        Polling RecordID:     <asp:Label runat="server" ID="lblPollingRecordID"></asp:Label>
        <br />
        
        Polling Lat, Long:<asp:Label runat="server" ID="lblLatLong"></asp:Label>



           <br />   <br /><br /><br />
        Eneter any Address:<asp:TextBox runat="server" ID="txtAddress" Width="300px"></asp:TextBox>
        <br />
        <br />
        <asp:LinkButton runat="server" ID="lnkGetAddress" Text="Get Lat & Long" 
        OnClick="lnkGetAddress_OnClick"></asp:LinkButton>
        <br />
        <br />
        <asp:Label runat="server" ID="lblLat"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lblLong"></asp:Label>
        <br />

    </div>


</asp:Content>

