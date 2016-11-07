<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true" CodeFile="JCU_Participants.aspx.cs" Inherits="Pages_Custom_JCU_Participants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .gridview_header th {
            padding: 0px;
            border: 1px solid #000000;
        }

        .gridview_row td {
            border: 1px solid #000000;
        }

        .gridview_row a {
            text-decoration: none;
            color: #000000;
        }

        .gridview_row img {
            border-width: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="Server">
<asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1"></asp:ScriptManagerProxy>

<%--<asp:UpdateProgress class="ajax-indicator-full" ID="UpdateProgress3" runat="server">
    <ProgressTemplate>
        <table style="width: 100%; height: 100%; text-align: center;">
            <tr valign="middle">
                <td>
                    <p style="font-weight: bold;">
                        Please wait...
                    </p>
                    <asp:Image runat="server" AlternateText="Processing..." ImageUrl="~/Images/ajax.gif" />
                </td>
            </tr>
        </table>
    </ProgressTemplate>
</asp:UpdateProgress>--%>

<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
    <div style="padding: 10px;">
        <asp:Label runat="server" ID="lblTitle" Text="Participants" CssClass="TopTitle"></asp:Label>
        <br />
        <br />

        <div style="border: 1px solid #000000; padding: 10px;">
            <table>
                <tr>
                    <td></td>
                    <td></td>
                    <td align="left">Site</td>
                    <td align="left">Participant ID</td>
                    <td align="left">Initials</td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right"><strong>Search</strong> </td>
                    <td align="left">
                        <asp:DropDownList runat="server" ID="ddlSite" CssClass="NormalTextBox"></asp:DropDownList>
                    </td>
                    <td align="left">
                        <asp:TextBox runat="server" ID="txtParticipantID" CssClass="NormalTextBox"></asp:TextBox></td>
                    <td align="left">
                        <asp:TextBox runat="server" ID="txtInitials" CssClass="NormalTextBox"></asp:TextBox>
                    </td>
                    <td></td>
                    <td>

                        <table width="120px">

                            <tr>
                                <td>
                                    <div>
                                        <asp:LinkButton runat="server" ID="lnkSearch" CssClass="btn" OnClick="lnkSearch_Click"> <strong>Go</strong></asp:LinkButton>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <asp:LinkButton runat="server" ID="lnkReset" CssClass="btn" OnClick="lnkReset_Click"
                                            CausesValidation="false"> <strong>Reset</strong></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>

            </table>
        </div>


        <br />
        <div>
            <asp:GridView ID="grdVisit" runat="server" AutoGenerateColumns="False" DataKeyNames="RecordID" ShowHeaderWhenEmpty="true"
                HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" CssClass="gridview" GridLines="Both"
                OnRowDataBound="grdTable_RowDataBound">
                <RowStyle CssClass="gridview_row" />
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblParticipantRecordID" runat="server" Text='<%# Eval("RecordID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>



                    <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="right"><strong>*Week</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="left"><strong>Evaluation<br /> Visit:</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:HyperLink runat="server" ID="hfIDnInitils"></asp:HyperLink>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>-6</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>1</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl6"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>-2</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>2</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl2"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>0</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>3</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl0"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>8</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>4</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl8"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>12f</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>5</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl12"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>24</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>6</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl24"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>36</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>7</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl36"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>42</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>8</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl42"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>44</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>9</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl44"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>46</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>10</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl46"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>66</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>11</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl66"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>94 EOT <br />Phase 2</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>12</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl94"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>Early<br /> termination</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>N/A</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lblET"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>96</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>Follow<br /> Up</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbl96"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>Unscheduled <br /> Visits</strong>  </td>
                                </tr>
                                <tr>
                                    <td style="height: 50px;" align="center"><strong>UV</strong> </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>

                            <asp:Label runat="server" ID="lbluv"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>



                </Columns>
                <HeaderStyle CssClass="gridview_header" />
            </asp:GridView>
        </div>

        <br />
        <div>
            <h3>key</h3>
            <table class="gridview_row" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <img src="jcu_images/Visit has not started yet.png" />
                    </td>
                    <td>Visit not started yet
                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="jcu_images/Visit has an open query.png" />
                        <strong>9</strong>
                    </td>
                    <td>Visit has an open query<br />
                        Number indicates the number of open queries

                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="jcu_images/Visit has missing data.png" />
                        <strong>9</strong>
                    </td>
                    <td>Visit has missing data

                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="jcu_images/Visit has missing data.png" />
                        <strong>3</strong>
                    </td>
                    <td>Visit has missing data<br />
                        Number indicates the number of open queries

                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="jcu_images/Visit data is complete.png" />

                    </td>
                    <td>Visit data is complete and ready for monitor review

                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="jcu_images/Visit data is complete.png" />
                        <strong>*</strong>
                    </td>
                    <td>Changes or new data added to previously monitored data, needs to be re-monitored

                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="jcu_images/verified by monitor.png" />

                    </td>
                    <td>Visit data complete and verified by monitor

                    </td>
                </tr>
                <tr>
                    <td>
                        <img src="jcu_images/Locked.png" />

                    </td>
                    <td>Locked – no further changes possible

                    </td>
                </tr>




            </table>

        </div>



    </div>

 </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

