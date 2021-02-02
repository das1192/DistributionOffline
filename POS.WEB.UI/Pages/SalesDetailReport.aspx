<%@ Page Title="Sales Report" Language="C#" AutoEventWireup="true" CodeFile="SalesDetailReport.aspx.cs"
    Inherits="TalukderPOS.Web.UI.SalesDetailReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script language="javascript" type="text/javascript">

        function printdiv(strid) {
            var prtContent = document.getElementById(strid);
            var WinPrint = window.open(); //  window.open('', '', 'letf=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            //prtContent.innerHTML = ""; // to set the printed part vacant
        } 
    </script>
    <title></title>
</head>
<body>
    <form runat="server" id="reportForm">
    <asp:ScriptManager ID="scmForm" runat="server" LoadScriptsBeforeUI="false">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <div id="div_print">
                <h1 style="text-align:center; font-size:18px; font-weight:normal">Sales Invoice</h1>
                                <hr style="color:Green; margin-top:10px" />
                                <h1 style="text-align:center; font-size:18px; font-weight:normal">Talukder Group of Industries</h1>
                                 <h1 style="text-align:center; font-size:18px; font-weight:normal">Aftab Heritage, 3rd Floor</h1>
                                 <h1 style="text-align:center; font-size:18px; font-weight:normal">Plot # 2, Road # 4, Sector-1, Uttara, Dhaka.</h1>
                                                                                               

                                <hr style="color:Green; margin-top:10px" />
                    <table width="100%">
                        <tr>
                            <td align="left" style="font-weight: bold" width="150px">
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td width="100px">
                                <asp:Label ID="lblSalesDate" runat="server" Text=" Date:"></asp:Label>
                            </td>
                            <td>
                                <div>
                                    <%--<asp:TextBox ID="txtSaleDate" runat="server" CssClass="TextBox" Enabled="False" />--%>
                                    <asp:Label ID="lblSaleDate" runat="Server"></asp:Label>
                                    &nbsp;<br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOrderNo" runat="server" Text="Invoice No:"></asp:Label>
                            </td>
                            <td>
                                <div>
                                    <%--<asp:TextBox ID="txtOrderNo" runat="server" CssClass="TextBox" Enabled="False" />--%>
                                    <asp:Label ID="lblOrderNumber" runat="Server"></asp:Label>
                                    &nbsp;<br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="lblMessage2" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <div>
                                    <asp:GridView ID="gvSalesDetail" runat="server" AutoGenerateColumns="False" ViewStateMode="Enabled"
                                        Width="600px">
                                        <Columns>
                                            <asp:BoundField DataField="PROD_NAME" HeaderText="Product">
                                                <ItemStyle Width="100px" HorizontalAlign="Center"  />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SalePrice" HeaderText="UnitPrice">
                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                          
                                            <asp:BoundField DataField="Qty" HeaderText="Quantity">
                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                           <%-- <asp:BoundField DataField="Amount" HeaderText="Price">
                                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            </asp:BoundField>--%>
                                           
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#8bc139" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <AlternatingRowStyle BackColor="AliceBlue" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table>
                    </table>
                    <div style="width:603px; height:auto; overflow:hidden;">
                    <table border=".5" cellpadding="0" cellspacing="0" style="float:right">
                        <tr>
                            <td align="left" width="100px">
                                Total Amount:
                            </td>
                            <td align="left">
                                <div>
                                    <%--<asp:TextBox ID="txtTotalAmount" runat="server" Width="100px" Enabled="False">0.00</asp:TextBox>--%>
                                    <asp:Label ID="lblTotalAmount" runat="Server">0.00</asp:Label>
                                    &nbsp;<br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Total VAT:
                            </td>
                            <td align="left">
                                <div>
                                    <%--<asp:TextBox ID="txtTotalVAT" runat="server" Width="100px" Enabled="False">0.00</asp:TextBox>--%>
                                    <asp:Label ID="lblTotalVal" runat="Server">0.00</asp:Label>
                                    &nbsp;<br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Total Bill:
                            </td>
                            <td align="left">
                                <div>
                                    <%--<asp:TextBox ID="txtTotalBill" runat="server" Width="100px" Enabled="False">0.00</asp:TextBox>--%>
                                    <asp:Label ID="lblTotalBill" runat="Server"></asp:Label>
                                    &nbsp;<br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Discount:
                            </td>
                            <td align="left">
                                <div>
                                    <%--<asp:TextBox ID="txtDiscountFinal" runat="server" Width="100px" Enabled="False">0.00</asp:TextBox>--%>
                                    <asp:Label ID="lblDiscountFinal" runat="Server">0.00</asp:Label>
                                    &nbsp;<br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Net Payable:
                            </td>
                            <td align="left">
                                <div>
                                    <%--<asp:TextBox ID="txtNetPayable" runat="server" Width="100px" Enabled="False">0.00</asp:TextBox>--%>
                                    <asp:Label ID="lblNetPayable" runat="Server">0.00</asp:Label>
                                    &nbsp;<br />
                                </div>
                            </td>
                        </tr>
                        </table>
                    </div>
                        <table>
                        <tr>
                            <td align="left">
                                In Words:
                            </td>
                            <td align="left" width="500px">
                                <div>
                                    <asp:Label ID="lblPayableInWord" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td align="center" width="180px">
                            <hr />
                            <b>Received By</b>
                            </td>
                            <td width="240px"></td>
                            <td align="center" width="180px">
                            <hr />
                            <b>Prepared By</b>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="printdiv('div_print');" />
                            <%--<input name="b_print" type="button" class="ipt" onclick="printdiv('div_print');"
                                value=" Print " />--%>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
