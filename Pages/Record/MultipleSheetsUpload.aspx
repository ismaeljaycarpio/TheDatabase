﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="MultipleSheetsUpload.aspx.cs" Inherits="Pages_Record_MultipleSheetsUpload"
    EnableTheming="true" %>

<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
   
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
                                    <strong>Template:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblTemlate"> </asp:Label>
                                    <asp:HiddenField runat="server" Value="" ID="hfImportTemlateID"/>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <strong>File Name:</strong>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblExcelFileName"> </asp:Label>
                                </td>
                            </tr>
                            
                            <tr>
                                <td align="right">
                                    <strong>Batch Description:</strong>
                                </td>
                                <td>
                                     <asp:Label runat="server" ID="lblBatchDesc" > </asp:Label>
                                </td>
                            </tr>
                          
                           <%--<tr runat="server" id="trLocation" visible="false">
                                <td align="right">
                                    <strong>Location:</strong>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="false" DataTextField="LocationName"
                                        DataValueField="LocationID" CssClass="NormalTextBox">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>


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
