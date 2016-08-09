<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="UpdateVol.aspx.cs" Inherits="Pages_Custom_UpdateVol" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" Runat="Server">

<br />Click the following button that sets the latitude and longitude for ALL of the volunteers <br /><br />

   <asp:LinkButton ID="lnkUpdateVolLat" runat="server" OnClick="lnkUpdateVolLat_OnClick" CssClass="btn"> <strong>Update ALL Volunteer Lat & Long</strong> </asp:LinkButton>


 <br /><br />   <br />
and finds the closest 3 polling places and sets those fields in the volunteer table
<br /><br />

<asp:LinkButton ID="lnkUpdateVol" runat="server" OnClick="lnkUpdateVol_OnClick" CssClass="btn"> <strong>Update ALL Volunteer polling places</strong> </asp:LinkButton>
 

 <br /><br />   <br />
and finds the closest 3 polling places Time
<br /><br />

<asp:LinkButton ID="lnkUpdateVolForTime" runat="server" OnClick="lnkUpdateVolForTime_OnClick" CssClass="btn"> <strong>Update ALL Volunteer time</strong> </asp:LinkButton>




<br /><br />

<asp:LinkButton ID="lnkAutoLevelling" runat="server" OnClick="lnkAutoLevelling_OnClick" CssClass="btn"> <strong>Auto Levelling</strong> </asp:LinkButton>

<br /><br /><br />

<asp:Label runat="server" ID="lblMSG"></asp:Label>
 

</asp:Content>

