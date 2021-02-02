<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportsViewer.aspx.cs" Inherits="Reports_ReportsViewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                 <%-- <tr>
                        <td valign="top">
                            <asp:ImageButton ID="imgbtnsale" runat="server" ImageUrl="../Images/PrintButton.jpg"
                                OnClick="imgbtnsale_Click" Height="35px" Width="80px" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <CR:CrystalReportViewer ID="CrystalReportViewer1" BorderColor="#3366CC" BorderStyle="Solid"
                                 
                                 Font-Size="Larger" BorderWidth="1px"
                                runat="server" AutoDataBind="true"  
                                  />
                            <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" BorderColor="#3366CC" BorderStyle="Solid"
                                HasPrintButton="False" HasSearchButton="False" HasGotoPageButton="False" HasZoomFactorList="False"
                                HasDrillUpButton="False" HasViewList="False" Font-Size="Larger" BorderWidth="1px"
                                runat="server" AutoDataBind="true" HasRefreshButton="True" HasCrystalLogo="False"
                                DisplayGroupTree="False" />--%>
                        </td>
                        <td>
                        </td></tr>
                      
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="CrystalReportViewer1" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    </form>
</body>
</html>
