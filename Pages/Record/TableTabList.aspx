<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Popup.master" AutoEventWireup="true"
    CodeFile="TableTabList.aspx.cs" Inherits="Pages_Record_TableTabList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">

  <style type="text/css">
               .sortHandle
        {
            cursor: move;
        }

        .cssplaceholder
        {
            border-top: 2px solid #00FFFF;
            border-bottom: 2px solid #00FFFF;
        }
     </style>


  <link href="<%=ResolveUrl("~/Styles/jquery-ui-1.7.3.custom.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Script/jquery-ui-1.9.2.custom.min.js")%>"></script>    

<link href="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript" src="<%=ResolveUrl("~/fancybox/jquery.fancybox-1.3.4.pack.js")%>"></script>
       <script type="text/javascript">
           function MouseEvents(objRef, evt) {
               if (evt.type == "mouseover") {
                   objRef.style.backgroundColor = "#76BAF2";
                   objRef.style.cursor = 'pointer';
               }
               else {

                   if (evt.type == "mouseout") {
                       if (objRef.rowIndex % 2 == 0) {
                           //Alternating Row Color
                           objRef.style.backgroundColor = "white";
                       }
                       else {
                           objRef.style.backgroundColor = "#DCF2F0";
                       }
                   }
               }
           }



    </script>

    <script language="javascript" type="text/javascript">

        var fixHelper = function (e, ui) {
            ui.children().each(function () {
                $(this).width($(this).width());
            });

            return ui;
        };

         $(document).ready(function () {

         $(function () {
                $("#ctl00_HomeContentPlaceHolder_upPages").sortable({
                    items: '.gridview_row',
                    cursor: 'crosshair',
                    helper: fixHelper,
                    cursorAt: { left: 10, top: 10 },
                    connectWith: '#ctl00_HomeContentPlaceHolder_upPages',
                    handle: '.sortHandle',
                    axis: 'y',
                    distance: 15,
                    dropOnEmpty: true,
                    receive: function (e, ui) {
                        $(this).find("tbody").append(ui.item);

                    },
                    start: function (e, ui) {
                        ui.placeholder.css("border-top", "2px solid #00FFFF");
                        ui.placeholder.css("border-bottom", "2px solid #00FFFF");

                    },
                    update: function (event, ui) {
                        var TC = '';
                        $(".TableTabID").each(function () {
                            TC = TC + this.value.toString() + ',';
                        });

                        document.getElementById("hfOrderFF").value = TC;
                        $("#btnOrderFF").trigger("click");

                    }
                });
            });
        });



    </script>
    <table border="0" cellpadding="0" cellspacing="0"  align="center">
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="width: 50%;">
                            <span class="TopTitle">
                                <asp:Label runat="server" ID="lblTitle" Text="Pages"></asp:Label></span>
                        </td>
                        <td align="left">
                            <div style="width: 40px; height: 40px;">
                                <asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress2" runat="server">
                                    <ProgressTemplate>
                                        <table style="width: 100%; text-align: center">
                                            <tr>
                                                <td>
                                                    <img alt="Processing..." src="../../Images/ajax.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
                <asp:UpdatePanel ID="upPages" runat="server">
                    <ContentTemplate>
                        <div id="search" style="padding-bottom: 10px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                                ShowMessageBox="false" ShowSummary="false" HeaderText="Please correct the following errors:" />
                        </div>
                        <asp:Panel ID="Panel2" runat="server" >
                          <asp:HiddenField runat="server" ID="hfOrderFF" ClientIDMode="Static" />
                            <asp:Button runat="server" ID="btnOrderFF" ClientIDMode="Static" Style="display: none;"
        OnClick="btnOrderFF_Click" />
                            <div runat="server" id="divDetail" >
                                <table cellpadding="3">
                                   
                                    <tr >
                                        <td>
                                        </td>
                                        <td>
                                                 <div style="padding-left: 20px; padding-top: 10px;">
                                        <asp:GridView ID="grdTableTab" runat="server" AutoGenerateColumns="False" DataKeyNames="TableTabID"
                                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" CssClass="gridview"
                                            OnRowCommand="grdTableTabt_RowCommand" OnRowDataBound="grdTableTab_RowDataBound" 
                                            AlternatingRowStyle-BackColor="#DCF2F0">
                                            <RowStyle CssClass="gridview_row" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("TableTabID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="~/App_Themes/Default/Images/icon_delete.gif"
                                                            CommandName="deletetype" CommandArgument='<%# Eval("TableTabID") %>'  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/TableTabDetail.aspx"
                                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" ToolTip="Edit" ID="hlEditDetail"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:HyperLink runat="server" CssClass="popuplink" NavigateUrl="~/Pages/Record/TableTabDetail.aspx"
                                                            ImageUrl="~/Pages/Pager/Images/add.png" ToolTip="Add Child Table" ID="hlAddDetail"></asp:HyperLink>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="sortHandle">
                                                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/MoveIcon.png"
                                                            ToolTip="Drag and drop to change order" />
                                                        <input type="hidden" id='hfTableTabID' value='<%# Eval("TableTabID") %>' class='TableTabID' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              
                                                <asp:TemplateField HeaderText="Page">
                                                    <ItemTemplate>
                                                        <div style="padding-left: 10px;">
                                                            <asp:Label runat="server" ID="lblTabName" Text='<%# Eval("TabName")%>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                               
                                            </Columns>
                                            <HeaderStyle CssClass="gridview_header" />
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <div runat="server" id="divTableTab" visible="false" style="padding-left: 20px;">
                                        <asp:HyperLink runat="server" ID="hlAddTableTab" Style="text-decoration: none;
                                            color: Black;" CssClass="popuplink">
                                            <asp:Image runat="server" ID="Image6" ImageUrl="~/App_Themes/Default/images/BigAdd.png" />
                                            No Pages have been added yet. <strong style="text-decoration: underline; color: Blue;">
                                                Add new Page now.</strong>
                                        </asp:HyperLink>
                                    </div>
                                    <br />

                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                              <asp:Button runat="server" ID="btnRefreshPages" ClientIDMode="Static" Style="display: none;"
        OnClick="btnRefreshPages_Click" />
                            <br />
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                           
                                        </td>
                                        <td>
                                            <div>
                                                <asp:LinkButton runat="server" ID="lnkBack" OnClientClick="javascript:  parent.$.fancybox.close();"
                                                    CssClass="btn" CausesValidation="false"> <strong>Close</strong></asp:LinkButton>
                                                <%--<asp:HyperLink runat="server" ID="hlBack"  CssClass="btn"> <strong>Back</strong> </asp:HyperLink>--%>
                                               
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="3" height="13">
            </td>
        </tr>
    </table>

      <script type="text/javascript">

          function CloseAndRefresh() {
              window.parent.document.getElementById('btnRefreshForms').click();
              parent.$.fancybox.close();
              // alert('ok');
          }
    
    </script>
</asp:Content>
