<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="Document.aspx.cs" Inherits="Pages_Document_Document" EnableEventValidation="false" %>

<%@ Register Src="~/Pages/Pager/Pager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Namespace="DBGServerControl" Assembly="DBGServerControl" TagPrefix="dbg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function abc() {
            var b = document.getElementById('<%= lnkSearch.ClientID %>');
            if (b && typeof (b.click) == 'undefined') {
                b.click = function () {
                    var result = true;
                    if (b.onclick) result = b.onclick();
                    if (typeof (result) == 'undefined' || result) {
                        eval(b.getAttribute('href'));
                    }
                }
            }

        }
    </script>

    
<script type="text/javascript">

    $(document).ready(function () {
      
        $(function () {
            $(".popuplink").fancybox({
                scrolling: 'auto',
                type: 'iframe',
                width: 500,
                height: 250,
                titleShow: false
            });
        });

    });

    function OpenAddFolder() {
        document.getElementById('hlAddFolder').click();
    }

    function SetFolder(iFolderID)
    {
           document.getElementById('hfParentFolderID').value=iFolderID;
            document.getElementById('btnFolderSaved').click();
    }


</script>

     <div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
                <tr>
                    <td colspan="3" height="40">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 50%;">
                                    <span class="TopTitle">Documents</span>
                                </td>
                                <td align="left">
                                    <div style="width: 40px; height: 40px;">
                                        <%--<asp:UpdateProgress class="ajax-indicator" ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                            <ProgressTemplate>
                                                <table style="width: 100%; text-align: center">
                                                    <tr>
                                                        <td>
                                                            <img alt="Processing..." src="../../Images/ajax.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>--%>
                                    </div>
                                </td>
                                <td style="width: 200px;">
                                    <table>
                                        <tr>
                                           
                                            <td>
                                                <%--<asp:HyperLink runat="server" ID="hlNewReport" ToolTip="New Report">
                                                    <asp:Image runat="server" ID="imgNewReport" ImageUrl="~/App_Themes/Default/images/CreateReport.png"  />
                                                </asp:HyperLink>--%>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%--<asp:HyperLink runat="server" ID="hplNewData" ToolTip="Upload Document">
                                                    <asp:Image runat="server" ID="imgAddNewRecord" SkinID="Upload" />
                                                </asp:HyperLink>--%>
                                            </td>
                                             <td style="padding-left:100px;">
                                                <asp:HyperLink runat="server" ID="hlBack" CssClass="ButtonLink" CausesValidation="false">
                                                    <asp:Image runat="server" ID="imgBack" ImageUrl="~/App_Themes/Default/images/Back.png"  ToolTip="Back" />
                                                </asp:HyperLink>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
               
                <tr>
                    <td valign="top">
                    </td>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSearch">
                            <div id="search"  class="searchcorner"  onkeypress="abc();">
                                <br />
                                <table style="border-collapse: collapse" cellpadding="4">
                                    <tr>
                                        <td align="right">
                                            <strong>Category </strong>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlDocumentType" AutoPostBack="true" DataValueField="DocumentTypeID"
                                                CssClass="NormalTextBox" DataTextField="DocumentTypeName" OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:HyperLink runat="server" ID="hlDocumentTypeEdit" Text="Edit" NavigateUrl="~/Pages/Document/DocumentType.aspx"></asp:HyperLink>
                                        </td>
                                         <td></td>
                                   
                                        <td align="right">
                                            <strong>Date</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDateFrom"  Width="100px" ValidationGroup="MKE"
                                                BorderStyle="Solid" BorderColor="#909090"
                                                BorderWidth="1" />
                                                <asp:ImageButton runat="server" ID="ibEndDate"  ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false"/>
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateFrom" runat="server" TargetControlID="txtDateFrom"
                                                Format="dd/MM/yyyy" PopupButtonID="ibEndDate"  FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                            
                                            <asp:RangeValidator ID="rngDateFrom" runat="server" ControlToValidate="txtDateFrom"
                                                ValidationGroup="MKE" ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date"
                                                MinimumValue="1/1/1753" MaximumValue="1/1/3000"></asp:RangeValidator>

                                              <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" TargetControlID="txtDateFrom" WatermarkText="dd/mm/yyyy"
                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>

                                            To
                                            <asp:TextBox runat="server" ID="txtDateTo"  Width="100px" CssClass="NormalTextBox"
                                                ValidationGroup="MKE"   BorderStyle="Solid"
                                                BorderColor="#909090" BorderWidth="1" />
                                                <asp:ImageButton runat="server" ID="ibDateTo"  ImageUrl="~/Images/Calendar.png" AlternateText="Click to show calendar" CausesValidation="false"/>
                                            <ajaxToolkit:CalendarExtender ID="ce_txtDateTo" runat="server" TargetControlID="txtDateTo"
                                                Format="dd/MM/yyyy" PopupButtonID="ibDateTo"  FirstDayOfWeek="Monday">
                                            </ajaxToolkit:CalendarExtender>
                                           
                                            <asp:RangeValidator ID="rngDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="MKE"
                                                ErrorMessage="*" Font-Bold="true" Display="Dynamic" Type="Date" MinimumValue="1/1/1753"
                                                MaximumValue="1/1/3000"></asp:RangeValidator>
                                            
                                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="txtDateTo" WatermarkText="dd/mm/yyyy"
                                runat="server" WatermarkCssClass="MaskText"></ajaxToolkit:TextBoxWatermarkExtender>

                                        </td>
                                        <td style="width: 10px;">
                                        </td>
                                        <td align="right">
                                            <strong>Search</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDocumentText" CssClass="NormalTextBox"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <div>
                                                
                                                    <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" ValidationGroup="MKE"  CausesValidation="true"
                                                    OnClick="lnkSearch_Click"> <strong>Go </strong> </asp:LinkButton>
                                                       
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkAllFolder" TextAlign="Right"  
                                                Text="All Folder" AutoPostBack="true" 
                                                oncheckedchanged="chkAllFolder_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                        </asp:Panel>
                        
                       
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblCurrentFolder" Text="<a href='javascript:SetFolder(-1)'>Home</a>/"></asp:Label>
                                </td>
                                <td align="right">
                                    <%--<asp:LinkButton runat="server" ID="lnkCreateFolder" CssClass="NormalTextBox" OnClientClick="OpenAddFolder();return false;">Create New Folder</asp:LinkButton>--%>
                                     <asp:HyperLink runat="server" ID="lnkCreateFolder" CssClass="popuplink" >Create New Folder</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                      
                        <dbg:dbgGridView ID="gvTheGrid" runat="server" GridLines="Both" CssClass="gridview"
                            HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AllowPaging="True"
                            AllowSorting="True" DataKeyNames="RecordID" HeaderStyle-ForeColor="Black" Width="100%"
                            AutoGenerateColumns="false" PageSize="15" OnSorting="gvTheGrid_Sorting" 
                            OnPreRender="gvTheGrid_PreRender" OnRowDataBound="gvTheGrid_RowDataBound">
                            <PagerSettings Position="Top" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("RecordID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFD" runat="server" Text='<%# Eval("FD") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>


                                        <%--<asp:HyperLink ID="hlView" runat="server" ToolTip="View" NavigateUrl='<%# GetFileURL(Eval("RecordID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconShow.png" Target="_blank" />--%>

                                             <asp:LinkButton ID="lnkFileView" runat="server"  OnClick="DownloadFile" >
                                                 <asp:Image runat="server" ID="Image1" Height="16px" Width="16px"
                                                 ImageUrl="~/App_Themes/Default/Images/iconShow.png" />
                                             </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlEdit" runat="server" ToolTip="Edit" NavigateUrl='<%# GetEditURL(Eval("RecordID").ToString()) %>'
                                            ImageUrl="~/App_Themes/Default/Images/iconEdit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Width="10px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlFolderIcon" runat="server" ToolTip="Folder" NavigateUrl='#' 
                                        Visible="false" > 
                                                <asp:Image runat="server" ID="imgFolderIcon" Height="16px" Width="16px"
                                                  ImageUrl="~/App_Themes/Default/Images/Folder.png" />
                                            </asp:HyperLink>
                                             <asp:LinkButton ID="lnkFileIcon" runat="server"  OnClick="DownloadFile" Visible="false" >
                                                 <asp:Image runat="server" ID="imgFileIcon" Height="16px" Width="16px"
                                                 ImageUrl="~/Pages/Document/Images/file_extension_txt.png" />
                                             </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField  HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                     <ItemStyle  HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        
                                            <asp:LinkButton ID="lnkFolderName" runat="server" OnClick="GoToFolder" Visible="false" Text='<%# Eval("Name") %>'></asp:LinkButton>
                                        <%--<asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' Visible="false"></asp:Label>--%>
                                                <%--<asp:HyperLink ID="hlFileName" runat="server"  Visible="false" Text='<%# Eval("Name") %>' ></asp:HyperLink>--%>
                                            <asp:LinkButton ID="lnkFileName" runat="server"  OnClick="DownloadFile" Visible="false" Text='<%# Eval("Name") %>'>
                                             </asp:LinkButton>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Category" HeaderStyle-HorizontalAlign="Left">
                                   <ItemStyle  HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategory" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="File Type" HeaderStyle-HorizontalAlign="Left">
                                   <ItemStyle  HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Size"  HeaderStyle-HorizontalAlign="Left">
                                  <ItemStyle  HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSize" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField SortExpression="DocumentDescription" HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentDescription" runat="server" Text='<%# Eval("DocumentDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField SortExpression="DocumentDate" HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentDate" runat="server" Text='<%# GetDate(Eval("DocumentID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:BoundField DataField="DocumentDate" SortExpression="DocumentDate" HeaderText="Date"
                                    DataFormatString="{0:d}" ReadOnly="true" />--%>
                                <%--<asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlFileURL" runat="server" NavigateUrl='<%# GetFileURL(Eval("DocumentID").ToString())  %>'
                                            Target="_blank" Text='View' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <HeaderStyle CssClass="gridview_header" />
                            <RowStyle CssClass="gridview_row" />
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" ID="Pager" OnExportForCSV="Pager_OnExportForCSV" ShowAdd2="true" 
                                    HideAdd="true" OnDeleteAction="Pager_DeleteAction" OnApplyFilter="Pager_OnApplyFilter" HideExport="true"
                                    OnBindTheGridToExport="Pager_BindTheGridToExport" OnBindTheGridAgain="Pager_BindTheGridAgain" />
                            </PagerTemplate>
                        </dbg:dbgGridView>
                        <br />
                        <div runat="server" id="divEmptyData" visible="false" style="padding-left: 100px;">                            
                                No documents have been added yet. <br /> <strong style="text-decoration: none; color: Blue;">
                                   Upload document now </strong>
                            <asp:HyperLink runat="server" ID="hpNew" Style="text-decoration: none; color: Black;" ToolTip="Upload" >                                
                            </asp:HyperLink>  
                             <%--<asp:HyperLink runat="server" ID="hpNewFolder" Style="text-decoration: none; color: Black;" ToolTip="Add Folder"  > 
                             <asp:Image runat="server" ID="imgAddNewFolder" Height="24px" Width="24px" ImageUrl="~/App_Themes/Default/Images/add32.png" />                               
                            </asp:HyperLink>--%> 
                            <%--<strong style="text-decoration: none; color: Blue;">
                                  or create report now</strong>--%>
                            <%--<asp:HyperLink runat="server" ID="hpNew2" Style="text-decoration: none; color: Black;" ToolTip="New Report">                               
                            </asp:HyperLink>--%>
                        </div>
                        <div runat="server" id="divNoFilter" visible="false" style="padding-left: 100px;">
                            <asp:LinkButton runat="server" ID="lnkNoFilter" Style="text-decoration: none; color: Black;"
                                OnClick="Pager_OnApplyFilter">
                                <asp:Image runat="server" ID="Image2" ImageUrl="~/App_Themes/Default/images/BigFilter.png"  />
                                No records match that filter. <strong style="text-decoration: none; color: Blue;">
                                    Clear filter</strong>
                            </asp:LinkButton> <br />
                            or <strong style="text-decoration: none; color: Blue;">
                                    Upload document now</strong>
                            <asp:HyperLink runat="server" ID="hplNewDataFilter" Style="text-decoration: none;
                                color: Black;" ToolTip="Upload"> 
                            </asp:HyperLink>
                             <%--<strong style="text-decoration: none; color: Blue;">
                                  or create report now</strong>--%>
                             <%--<asp:HyperLink runat="server" ID="hplNewDataFilter2" Style="text-decoration: none;
                                color: Black;" ToolTip="New Report"> 
                            </asp:HyperLink>--%>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" height="13">
                        <asp:HiddenField runat="server" ID="hfCRDocumentTypeID" />
                        <asp:Button ID="btnFolderSaved" runat="server" Visible="true" ClientIDMode="Static" Text=""
                                            Height="1px" Width="1px" style="display:none;" OnClick="btnFolderSaved_Click" />
                        <asp:HyperLink runat="server" ClientIDMode="Static" ID="hlAddFolder" CssClass="popuplink"
                         NavigateUrl="~/Pages/Document/FolderDetail.aspx" style="display:none;"></asp:HyperLink>
                         <asp:HiddenField runat="server" ID="hfParentFolderID" Value="-1" ClientIDMode="Static" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger  ControlID="gvTheGrid" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
</asp:Content>
