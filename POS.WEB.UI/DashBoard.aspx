<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableViewStateMac="false" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ViewContentPlace">
    <div class="backgr">
        <div class="container" style="padding-top: 20px;">
            <div class="row">
                <div class="col-sm-4">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <b>Current Stock</b></div>
                        <div class="panel-body">
                            <asp:Literal ID="tempHtmlTable2" runat="server"></asp:Literal></div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <b>Sales Status</b></div>
                        <div class="panel-body">
                            <asp:Literal ID="tempHtmlTable3" runat="server"></asp:Literal></div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <b>Top Moving Product</b></div>
                        <div class="panel-body">
                            <asp:Literal ID="tempHtmlTable4" runat="server"></asp:Literal></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container" style="margin-top: 20px;">
            <%--<div class="row" style="padding-left:120px;border-bottom:5px solid green;">
                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-6">
                        <div class="panel panel-primary">
                        <div class="panel-heading">
                            Admin</div>
                        <div class="panel-body">
                            <a href="Pages/Description.aspx?menuhead=101">
                                <img src="Images/admin.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                        </div>
                        <div class="col-md-6">
                        <div class="panel panel-primary">
                        <div class="panel-heading">
                            Payment</div>
                        <div class="panel-body">
                            <a href="Pages/Vendor_OutgoingForm.aspx?menuhead=107">
                                <img src="Images/paydaily.png" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-6">
                        <div class="panel panel-primary">
                        <div class="panel-heading">
                            Stock</div>
                        <div class="panel-body">
                            <a href="Pages/BranchStock.aspx?menuhead=109">
                                <img src="Images/stock.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                        </div>
                        <div class="col-md-6">
                        <div class="panel panel-primary">
                        <div class="panel-heading">
                            Sales</div>
                        <div class="panel-body">
                            <a href="Pages/ValidateSales.aspx?menuhead=104">
                                <img src="Images/sale.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-6">
                        <div class="panel panel-primary">
                        <div class="panel-heading">
                            Purchase</div>
                        <div class="panel-body">
                            <a href="Pages/T_PRODForm.aspx?menuhead=102">
                                <img src="Images/add_pr.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                        </div>
                        <div class="col-md-6">
                        <div class="panel panel-primary">
                        <div class="panel-heading">
                            Accounts</div>
                        <div class="panel-body">
                            <a href="Pages/ReportAccJournal.aspx?menuhead=110">
                                <img src="Images/pnl.png" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-6">
                        <div class="panel panel-primary">
                        <div class="panel-heading">
                            Report</div>
                        <div class="panel-body">
                            <a href="Pages/SalesReport.aspx?menuhead=105">
                                <img src="Images/report.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                        <div>
                        </div>
                    </div>
                        </div>
                        <div class="col-md-6">
                        
                        </div>
                    </div>
                </div>
            </div>--%>
           <%-- <div class="row" style="padding-top: 20px;">
                <a href="Pages/SearchInvoiceForReturn.aspx?menuhead=104">Pages/SearchInvoiceForReturn.aspx</a>
                <br />
                <a href="Pages/RetailerPayments.aspx?menuhead=104">Pages/RetailerPayments.aspx</a>
            </div>--%>
            <div class="row">
                 <div class="col-sm-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <b class="showHidePaySupplier" id="showHidePaySupplier">Cash & Bank Status +</b>
                        </div>
                        <div class="panel-body PaySupplierPanel" id="PaySupplierPanel" style="display:none">
                            <asp:GridView ID="gvCashBalance" runat="server" AutoGenerateColumns="False" GridLines="None"
                                    CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                    EmptyDataText="No rows returned" Width="100%">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:BoundField DataField="BankName" HeaderText="Bank">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AccountNo" HeaderText="Account No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Debit" HeaderText="Debit">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Credit" HeaderText="Credit">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Balance" HeaderText="Balance">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-top: 20px;">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Admin</div>
                                <div class="panel-body">
                                    <a href="Pages/Description.aspx?menuhead=101">
                                        <img src="Images/admin.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Purchase</div>
                                <div class="panel-body">
                                    <a href="Pages/T_PRODForm.aspx?menuhead=102">
                                        <img src="Images/add_pr.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Sales</div>
                                <div class="panel-body">
                                    <a href="Pages/ValidateSales2.aspx?menuhead=104">
                                        <img src="Images/sale.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Stock</div>
                                <div class="panel-body">
                                    <a href="Pages/BranchStock.aspx?menuhead=109">
                                        <img src="Images/stock.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-3">
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Accounts</div>
                                <div class="panel-body">
                                    <a href="Pages/ReportAccJournal.aspx?menuhead=110">
                                        <img src="Images/pnl.png" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Payment</div>
                                <div class="panel-body">
                                    <a href="Pages/Vendor_OutgoingForm.aspx?menuhead=107">
                                        <img src="Images/paydaily.png" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Report</div>
                                <div class="panel-body">
                                    <a href="Pages/SalesReport.aspx?menuhead=105">
                                        <img src="Images/report.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                                <div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--<div class="row" style="padding-top: 20px;">
                <div class="col-sm-1">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Admin</div>
                        <div class="panel-body">
                            <a href="Pages/Description.aspx?menuhead=101">
                                <img src="Images/admin.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Payment</div>
                        <div class="panel-body">
                            <a href="Pages/Vendor_OutgoingForm.aspx?menuhead=107">
                                <img src="Images/paydaily.png" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Stock</div>
                        <div class="panel-body">
                            <a href="Pages/BranchStock.aspx?menuhead=109">
                                <img src="Images/stock.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Sales</div>
                        <div class="panel-body">
                            <a href="Pages/ValidateSales.aspx?menuhead=104">
                                <img src="Images/sale.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Accounts</div>
                        <div class="panel-body">
                            <a href="Pages/ReportAccJournal.aspx?menuhead=110">
                                <img src="Images/pnl.png" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Purchase</div>
                        <div class="panel-body">
                            <a href="Pages/T_PRODForm.aspx?menuhead=102">
                                <img src="Images/add_pr.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Report</div>
                        <div class="panel-body">
                            <a href="Pages/SalesReport.aspx?menuhead=105">
                                <img src="Images/report.jpg" class="img-responsive" style="width: 100%" alt="Image" /></a></div>
                        <div>
                        </div>
                    </div>
                </div>
            </div>--%>
        </div>
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title" id="cathead">
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div id="test">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-warning" data-dismiss="modal">
                            Close
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="myModal2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title">
                            Monthly Sale (Datewise)</h4>
                    </div>
                    <div class="modal-body">
                        <div id="test2">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-warning" data-dismiss="modal">
                            Close
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="myModal5" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title">
                            Daily Sale</h4>
                    </div>
                    <div class="modal-body">
                        <div id="test5">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">
                            close
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--End body_inner_content-->
    <%--<div class="body_inner_content">
                <!--Begin body_inner_content-->
                <a href="frmLogin.aspx"><div class="hr">
                    <img src="Catagory/HR.png" title="HR" alt="Human Resource" />
                    <span class="humans">Human Resources</span>
                </div></a>
                <a href="frmLogin.aspx"><div class="pointofsale">
                    <img src="Catagory/admin.png" title="Admin" alt="Admin" />
                    <span class="point">SECURITY &amp; ADMIN</span>
                </div></a>
                <a href="frmLogin.aspx"><div class="account">
                    <asp:Image ID="imageAccount" runat="server" ImageUrl="~/Catagory/account.png" />
                    <span class="accounting">ACCOUNTING</span>
                </div></a>
            </div>
            <!--End body_inner_content-->
        </div>--%>
    <!--End body_inner-->
</asp:Content>
