<%@ Page Title="" Language="C#" MasterPageFile="~/Home/Home.master" AutoEventWireup="true"
    CodeFile="View.aspx.cs" Inherits="DocGen.Document.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <asp:Literal ID="ltTextStyles" runat="server"></asp:Literal>
    <asp:Literal ID="ltCommonStyles" runat="server">
        <style type="text/css">
            h1, h2, h3, h4, h5, h6, p, span, label, li, th, td
            {
                font-family: Verdana,Arial,Helvetica,sans-serif;
            }
            th
            {
                background: #f1f1f1;
            }
            .ReportContentContainer
            {
                width: 1000px;
                color: #000000;
            }
            table.TableSection
            {
                border-collapse: collapse;
            }
            table.TableSection th
            {
                background-color: #f8f8f8;
            }
            
            .MsoTocHeading
            {
                font-family: Verdana,Arial,Helvetica,sans-serif;
                font-weight: bold;
                font-size: 20px;
                text-align: center;
            }
            
            p.MsoToc1, li.MsoToc1, div.MsoToc1, p.MsoToc2, li.MsoToc2, div.MsoToc2, p.MsoToc3, li.MsoToc3, div.MsoToc3
            {
                mso-style-update: auto;
                mso-style-priority: 39;
                mso-style-next: Normal;
                margin-top: 0in;
                margin-right: 0in;
                margin-bottom: 5.0pt;
                mso-pagination: widow-orphan;
                font-size: 11.0pt;
                font-family: Verdana,Arial,Helvetica,sans-serif;
                mso-ascii-font-family: Calibri;
                mso-ascii-theme-font: minor-latin;
                mso-fareast-font-family: Calibri;
                mso-fareast-theme-font: minor-latin;
                mso-hansi-font-family: Calibri;
                mso-hansi-theme-font: minor-latin;
                mso-bidi-font-family: "Times New Roman";
                mso-bidi-theme-font: minor-bidi;
                text-decoration: none;
            }
            
            p.MsoToc1, li.MsoToc1, div.MsoToc1
            {
                margin-left: 0pt;
            }
            p.MsoToc2, li.MsoToc2, div.MsoToc2
            {
                margin-left: 11.0pt;
            }
            p.MsoToc3, li.MsoToc3, div.MsoToc3
            {
                margin-left: 22.0pt;
            }
            p.MsoToc4, li.MsoToc4, div.MsoToc4
            {
                margin-left: 33.0pt;
            }
            p.MsoToc5, li.MsoToc5, div.MsoToc5
            {
                margin-left: 44.0pt;
            }
            p.MsoToc6, li.MsoToc6, div.MsoToc6
            {
                margin-left: 55.0pt;
            }
            
            .MsoHyperlink a
            {
                text-decoration: none;
            }
            
            h1
            {
                font-size: 18px;
                color: #000;
                margin: 0px;
            }
            h2
            {
                font-size: 15px;
                color: #000;
                margin: 0px;
            }
            h3
            {
                font-size: 13px;
                margin: 0px;
            }
            h4
            {
                font-size: 12px;
                margin: 0px;
            }
            p
            {
                font-size: 11px;
                margin: 0px;
            }
        </style>
    </asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HomeContentPlaceHolder" runat="server">
    <asp:Label runat="server" ID="Label1" CssClass="TopTitle" Text="View Report:"></asp:Label>
    <asp:Label ID="ltTitle" CssClass="TopTitle" runat="server">---</asp:Label>
    <br />
    <br />
    <div>
        <table cellpadding="3">
            <tr>
                <td>
                    <div>
                        <asp:HyperLink runat="server" ID="hlBack" CssClass="btn" NavigateUrl="#"> <strong> Back</strong> </asp:HyperLink>
                    </div>
                </td>
                <td>
                    <div id="Div2" runat="server">
                        <asp:HyperLink runat="server" ID="hlEdit" CssClass="btn"><strong> Edit </strong></asp:HyperLink>
                    </div>
                </td>
                <td>
                    <div id="Div1" runat="server">
                        <asp:LinkButton ID="lbSave" runat="server" CssClass="btn" OnClick="lbSave_Click"> <strong>Save to Word </strong> </asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div class="ReportContentContainer" style="overflow: hidden;">
        <asp:Literal ID="ltReportContent" runat="server"></asp:Literal>
    </div>
    <div id="lnkTOC" style="display: none; position: fixed; bottom: 10px; right: 10px;
        background: #fff url(../images/List.png) 5px 5px no-repeat; padding: 10px 6px 0px 32px;
        min-height: 24px; vertical-align: middle; border: solid 1px #666; font-size: 10px;">
        <a href="#TOC"><b>Table Of Contents</b></a>
    </div>
    <script type="text/javascript">
        $(function () {
            if ($("#divTOC").length == 1) {
                $("#lnkTOC").fadeIn('slow');
            }
        });
    </script>
    <br />
    <br />
</asp:Content>
