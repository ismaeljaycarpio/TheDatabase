<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pager.ascx.cs" Inherits="Common_Pager" EnableTheming="true" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<script  type="text/javascript">



    function pagerabc() {
        var b = document.getElementById('<%= lnkGo.ClientID %>');
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

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server" DefaultButton="lnkGo">
            <div class="pagercorner">
                <div runat="server" id="divPagerGradient" class="pagergradient">      <%-- onkeypress="pagerabc();"--%>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pagertable">
                        <tr style="height: 36px;">
                            <td style="width: 20px;">
                            </td>
                            <td style="width:35%" runat="server" id="tdAddPart">
                                <asp:HyperLink ID="HyperAdd" runat="server" ImageUrl="Images/add.png" ToolTip="Add">                                   
                                </asp:HyperLink>


                                <asp:Image runat="server" ID="imgSpacer2" ImageUrl="~/Images/spacer.gif" Visible="false"  />

                                  <asp:HyperLink ID="HyperAdd2" runat="server" ImageUrl="Images/Icon_Upload.png" ToolTip="Add" Visible="false">                                   
                                </asp:HyperLink>

                                <asp:ImageButton ID="cmdAdd" runat="server" ImageUrl="Images/add.png" OnClick="cmdAdd_Click"
                                    CausesValidation="False" ToolTip="Add" />
                               <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/spacer.gif" AlternateText="" Width="3px" />
                                 <%--OnClientClick='return confirm("Are you sure you want to delete selected item(s)?");'--%>
                                <asp:LinkButton ID="DeleteLinkButton" runat="server" CausesValidation="false"                              
                                    ToolTip="Delete" OnClick="DeleteLinkButton_Click">
                                    <img alt="" id="ImgDelete" runat="server" src="Images/delete.png" style="border-width: 0" />
                                </asp:LinkButton>

                                  <asp:HyperLink ID="hlUploadFile" runat="server" ImageUrl="~/App_Themes/Default/images/Upload_S.png" ToolTip="Upload" Visible="false">                                   
                                </asp:HyperLink>

                               <%-- OnClientClick='return confirm("Are you sure you want to restore selected item(s)?");'--%>

                                <asp:LinkButton ID="lnkUnDelete" runat="server" CausesValidation="false" 
                                    ToolTip="Restore" OnClick="lnkUnDelete_Click">
                                    <img alt="" id="Img3" runat="server" src="Images/restore.png" style="border-width: 0" />
                                </asp:LinkButton>
                                  <asp:LinkButton ID="lnkParmanentDelete" runat="server" CausesValidation="false" 
                                    ToolTip="Permanent Delete" OnClick="lnkParmanentDelete_Click">
                                    <img alt="" id="Img1" runat="server" src="Images/ParmanentDelete.png" style="border-width: 0" />
                                </asp:LinkButton>

                                <%--<img src="../../Images/spacer.gif" width="3" alt="Spacer" />--%>                                
                                    <%--<img src="Images/spacer.gif" width="3" alt="Spacer" />--%>
                                      <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" AlternateText="" Width="3px" />

                                    <asp:ImageButton ID="cmdPDF" runat="server" ImageUrl="Images/pdf.png" OnClick="cmdPDF_Click"
                                    CausesValidation="False" ToolTip="Save to PDF" />
                                    
                                  
                                <asp:Image runat="server" ID="imgPDF" ImageUrl="~/Images/spacer.gif" width="3" />
                                <asp:ImageButton ID="cmdWord" runat="server" ImageUrl="Images/word.png" OnClick="cmdWord_Click"
                                    CausesValidation="False" ToolTip="Save to Word" />
                                 <asp:Image runat="server" ID="imgWord" ImageUrl="~/Images/spacer.gif"  width="3" />
                                <asp:ImageButton ID="cmdCSV" runat="server" ImageUrl="Images/csv.png" OnClick="cmdCSV_Click"
                                    CausesValidation="False" ToolTip="Export to CSV" />
                                 <asp:Image runat="server" ID="imgCSV" ImageUrl="~/Images/spacer.gif" width="3" />

                                 <asp:ImageButton ID="cmdExcel" runat="server" ImageUrl="Images/excel.png" OnClick="cmdExcel_Click"
                                    CausesValidation="False" ToolTip="Export to XLS" Visible="false" />
                                 <asp:Image runat="server" ID="imgExcel" ImageUrl="~/Images/spacer.gif" width="3" Visible="false" />

                                   <asp:ImageButton ID="cmdAllExport" runat="server" ImageUrl="Images/excel.png" OnClick="cmdAllExport_Click"
                                    CausesValidation="False" ToolTip="Export" Visible="false" />
                                 <asp:Image runat="server" ID="imgAllExportS" ImageUrl="~/Images/spacer.gif" width="3" Visible="false" />

                                <asp:ImageButton ID="cmdRefresh" runat="server" ImageUrl="Images/refresh.png" OnClick="cmdRefresh_Click"
                                    ToolTip="Refresh" />
                                 <asp:Image runat="server" ID="imgRefresh" ImageUrl="~/Images/spacer.gif" width="3" />
                                
                                <asp:ImageButton ID="cmdFilter" runat="server" ImageUrl="Images/filter.png" OnClick="cmdFilter_Click"
                                    ToolTip="Reset filters" />
                                 <asp:Image runat="server" ID="imgFilter" ImageUrl="~/Images/spacer.gif" width="3" />
                                  <asp:LinkButton ID="lnkEditMany" runat="server" CausesValidation="false" 
                                    ToolTip="Update Multiple" OnClick="lnkEditMany_Click">
                                    <img alt="" id="ImgEditMany" runat="server" src="Images/Edit_Many.png" style="border-width: 0" />
                                </asp:LinkButton>

                                 <asp:LinkButton ID="lnkCopyRecord" runat="server" CausesValidation="false" style="padding-left:3px;" 
                                    ToolTip="Copy Record" OnClick="lnkCopyRecord_Click" Visible="false">
                                   <asp:Image runat="server" ID="imgCopyRecord" ImageUrl="~/App_Themes/Default/images/CopyRecord.png" style="border-width: 0"/>
                                </asp:LinkButton>

                                 <asp:Image runat="server" ID="Image2" ImageUrl="~/Images/spacer.gif" width="3" />
                                  <asp:LinkButton ID="lnkSendEmail" runat="server" CausesValidation="false" Visible="false" 
                                    ToolTip="Send Message" OnClick="lnkSendEmail_Click">
                                    <img alt="" id="ImgSendEmail" runat="server" src="Images/SendEmail.png" style="border-width: 0" />
                                </asp:LinkButton>
                            </td>
                            <td width="20" align="center">
                                <asp:Image ID="imgcach1" ImageUrl="Images/cach.png" runat="server" />
                            </td>
                            <td  style="padding-left: 5px">
                                <table border="0" width="100%" id="tblNavigation" 
                                cellspacing="0" cellpadding="0" runat="server" clientidmode="Static" >
                                    <tr>
                                        <td rowspan="2" width="28" runat="server" id="tdFirst">
                                            <asp:ImageButton ID="cmdFirst" CausesValidation="false" ImageUrl="Images/first.png"
                                                runat="server" OnClick="cmdFirst_Click" ToolTip="First Page" />
                                        </td>
                                        <td rowspan="2" width="30">
                                            <asp:ImageButton ID="cmdPrev" CausesValidation="false" ImageUrl="Images/prev.png"
                                                runat="server" OnClick="cmdPrev_Click" ToolTip="Previous Page" />
                                        </td>
                                        <td rowspan="2" width="30">
                                            <asp:TextBox ID="txtPageIndex" runat="server" OnTextChanged="txtPageIndex_TextChanged"
                                                CssClass="pager_textbox" Text="1" BorderStyle="None"></asp:TextBox>
                                            <%--<ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" 
                                        TargetControlID="txtPageIndex" WatermarkText=" " runat="server"></ajaxToolkit:TextBoxWatermarkExtender>--%>
                                            <%--<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                    TargetControlID="txtPageIndex" FilterType="Custom" ValidChars="123456789"  FilterMode="ValidChars">
                                    </ajaxToolkit:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td >
                                            <asp:ImageButton ID="cmdUp" ImageUrl="Images/up.png" runat="server" CausesValidation="False"
                                                OnClick="cmdUp_Click" ToolTip="Next Page" />
                                        </td>
                                        <td rowspan="2" width="28"  runat="server" id="trPageCount">
                                            of
                                            <asp:Label ID="lblPageCount" runat="server" Text="0" CssClass="NormalTextBox"></asp:Label>
                                        </td>
                                        <td rowspan="2" width="33" runat="server" id="tdGoS" visible="false">
                                            <asp:ImageButton ID="cmdGoS" CausesValidation="false" ImageUrl="Images/go_s.png"
                                                runat="server" OnClick="cmdGoS_Click" ToolTip="Go to selected page" />
                                        </td>
                                        <td rowspan="2" width="28">
                                            <asp:ImageButton ID="cmdNext" CausesValidation="false" ImageUrl="Images/next.png"
                                                runat="server" OnClick="cmdNext_Click" ToolTip="Next Page" />
                                        </td>
                                        <td rowspan="2" width="28" runat="server" id="tdLast">
                                            <asp:ImageButton ID="cmdLast" CausesValidation="false" ImageUrl="Images/last.png"
                                                runat="server" OnClick="cmdLast_Click" ToolTip="Last Page" />
                                        </td>
                                        <td rowspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr >
                                        <td style="width:20px;" runat="server" id="tdDown">
                                            <asp:ImageButton ID="cmdDown" ImageUrl="Images/down.png" runat="server" CausesValidation="False"
                                                OnClick="cmdDown_Click" ToolTip="Previous Page" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="10" align="center" id="tdPageSizeRow1" runat="server">
                                <asp:Image ID="imgcach2" ImageUrl="Images/cach.png" runat="server" />
                            </td>
                            <td width="50" align="center" id="tdPageSizeRow" runat="server">
                                <asp:Label ID="lblTotalRows" runat="server" Text="0" CssClass="NormalTextBox"></asp:Label>
                                Items
                            </td>
                            <td width="10" align="center" id="tdPageSizeRow2" runat="server">
                                <asp:Image ID="imgcach3" ImageUrl="Images/cach.png" runat="server" />
                            </td>
                            <td width="90" style="padding-left: 5px" runat="server" id="tdPageSize">
                                <table border="0" width="100%" id="table2" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td rowspan="2">
                                            <asp:TextBox ID="txtPageSize" runat="server" Text="5" OnTextChanged="txtPageSize_TextChanged" 
                                                CssClass="pager_textbox" BorderStyle="None"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="cmdUpSize" ImageUrl="Images/up.png" runat="server" OnClick="cmdUpSize_Click" ToolTip="Increase Page Size" />
                                        </td>
                                        <td rowspan="2" >
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblPer" Text="/"></asp:Label>
                                                    </td>
                                                    <td>
                                                         <asp:Label runat="server" ID="lblPage" Text="Page"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="cmdDownSize" ImageUrl="Images/down.png" runat="server" OnClick="cmdDownSize_Click"  ToolTip="Decrease Page Size"
                                                CausesValidation="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="10" id="tdPageSizeRow3" runat="server">
                            </td>
                            <td width="20">
                                <asp:ImageButton ID="imgcach4" runat="server" ImageUrl="Images/cach.png" />
                            </td>
                            <td >
                                <div runat="server" id="divGo">
                                    <table id="Table3" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="bLp">
                                                &nbsp;
                                            </td>
                                            <td class="bCp">
                                                <asp:LinkButton runat="server" ID="lnkGo" CssClass="ButtonLink" OnClick="lnkGo_Click"
                                                    CausesValidation="false"> Go</asp:LinkButton>
                                            </td>
                                            <td class="bRp">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--<asp:ImageButton ID="cmdGo" ImageUrl="Images/Go.png" runat="server"
                            CausesValidation="False" OnClick="cmdGo_Click" />--%>
                            </td>
                            <td>
                                <%--<asp:HyperLink runat="server" ID="hlEdit" ImageUrl="Images/Edit.png" Visible="false" ></asp:HyperLink>--%>
                                <div runat="server" id="divEdit" visible="false">
                                    <table id="Table4" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="bL">
                                                &nbsp;
                                            </td>
                                            <td class="bC">
                                                <asp:HyperLink runat="server" ID="hlEdit" CssClass="ButtonLink">Edit</asp:HyperLink>
                                            </td>
                                            <td class="bR">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td >
                                       <%--<asp:ImageButton ID="cmdCog" runat="server" ImageUrl="Images/cog.png" OnClick="cmdCog_Click"
                                    CausesValidation="False" ToolTip="Page Size" Visible="false" />--%>
                                          <asp:HyperLink ID="hlEditView" runat="server" ImageUrl="Images/cog.png" ToolTip="Edit View" Visible="false">                                   
                                            </asp:HyperLink>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>                   
        <asp:PostBackTrigger ControlID="cmdWord" />
        <asp:PostBackTrigger ControlID="cmdExcel" />
        <asp:PostBackTrigger ControlID="cmdPDF" />
        <asp:PostBackTrigger ControlID="cmdCSV" />
        <asp:PostBackTrigger ControlID="cmdAllExport" />
    </Triggers>
</asp:UpdatePanel>
<div style="height: 5px">
</div>
