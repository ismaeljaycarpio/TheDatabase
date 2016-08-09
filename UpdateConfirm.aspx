<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Open.master" AutoEventWireup="true" CodeFile="UpdateConfirm.aspx.cs" Inherits="UpdateConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row contact-wrap" style="padding-top: 80px;">
            <div class="status alert alert-success" style="display: none"></div>

            <div class="col-sm-4 col-sm-offset-1" runat="server" id="divDefaultMessage">
                <div class="form-group">
                    <span style="font-size:xx-large">Success! </span>
                </div>
               
                 <div class="form-group">
                     <br />
                    <span style="font-size:large;">Thanks the database has been updated.   </span>  
                </div>
               
                <div class="form-group">
                   
                    <span style="font-size:large;">Click <a href="Default.aspx">here</a> to go to your dashboard or otherwise you can just close this page. </span>
                </div>
            </div>

             <div class="col-sm-4 col-sm-offset-1" runat="server" id="divSPUpdateConfirmMsg" visible="false">
                        
                 </div>

        </div>

    </div>
    <asp:HiddenField runat="server" ID="hfRecordID" />
</asp:Content>

