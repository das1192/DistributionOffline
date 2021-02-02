<%@ Page Title="Balance Sheet" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReportAccBalanceSheet.aspx.cs" Inherits="Pages_ReportAccBalanceSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <div id="main-content">
        <div class="wrapper">
            <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                <ProgressTemplate>
                    <img alt="progress" src="../Images/progress.gif" />
                    Processing...
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="table-responsive bg-success" style="background-color: #debc94; padding: 10px 50px;
                        border: 3px solid #93718b;">
                        <div class="bg-dark" style="padding: 5px; font-style: ">
                            <h2 class="text-center text-white" style="background-color: #B8860B; color: #fff;
                                padding: 3px; font-weight: bold;">
                                Balance Sheet</h2>
                        </div>
                        <div>
                            <%--<section class="panel">--%>
                            <%-- <header class="panel-heading">
                              Search Criteria
                          </header>--%>
                            <%--<h4>Search Criteria</h4>
                          <hr />--%>
                            <table width="100%" cellspacing="5px" cellpadding="10px" style="padding-top: 15px;">
                                <tr>
                                    <td style="width:20%";>
                                        <asp:Label ID="Label6" Style="font-weight: bold" runat="server" Text="Date (YYYY-MM-DD)"></asp:Label>&nbsp;<br />
                                    </td>
                                    <td style="width:30%";>
                                        <asp:TextBox ID="dptDate" runat="server" Width="150px" CssClass="form-control mx-sm-3"
                                            onkeypress="return clearTextBox()" />
                                        <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                            TargetControlID="dptDate" />
                                    </td>
                                    <td style="width:20%";>
                                        &nbsp;<br />
                                    </td>
                                    <td style="width:30%";>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnShow" runat="server" CssClass="inline btn btn-info" OnClick="btnShow_Click"
                                            Text="Search" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row" style="border: 1px solid black;">
                        <h2 runat="server" id="h2HeadLine" style="text-align: center">
                        </h2>
                        <div class="col-md-6" style="padding: 25px;">

                        <div class="row bg-black-gradient">
                                <asp:Label ID="Label13" runat="server" Text="Assets" 
                                    CssClass="pull-left text-bold" Font-Bold="True" 
                                    Font-Size="Large"></asp:Label>
                            </div>

                           <%-- <h2 class="bg-black-gradient">
                                Asset</h2>--%>
                           <%-- <div class="row">
                                <asp:Label ID="Label10" runat="server" Text="Asset" 
                                    CssClass="pull-left text-bold bg-black-gradient" Font-Bold="True" 
                                    Font-Size="Large"></asp:Label>
                            </div>--%>
                            <div class="row">
                                <asp:Label ID="Label1" runat="server" Text="Closing Stock" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblClosingStock" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>
                            <div class="row">
                                <asp:Label ID="Label2" runat="server" Text="Bank" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblBank" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>
                            <div class="row">
                                <asp:Label ID="Label4" runat="server" Text="Cash" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblCash" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>
                            <div class="row">
                                <asp:Label ID="Label7" runat="server" Text="Accounts Receivable" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblAccountsReceivable" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>
                             <div class="row">
                                <asp:Label ID="Label10" runat="server" Text="Loss" CssClass="pull-left" 
                                     ForeColor="#CC0066"></asp:Label>
                                <asp:Label ID="lblLoss" runat="server" CssClass="pull-right" ForeColor="#CC0066"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6" style="padding: 25px; border-left: 3px solid black;">
                            <div class="row bg-light-blue-gradient">
                                <asp:Label ID="Label11" runat="server" Text="Liability" 
                                    CssClass="pull-left text-bold" Font-Bold="True" 
                                    Font-Size="Large"></asp:Label>
                            </div>
                            <div class="row">
                                <asp:Label ID="lblProfitLossLabel" runat="server" Text="Profit" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblProfitLoss" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>
                                <div class="row">
                                <asp:Label ID="Label15" runat="server" Text="Commission" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblTotalCommission" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>

                            <div class="row">
                                <asp:Label ID="Label3" runat="server" Text="Accounts Payable" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblAccountsPayable" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>
                                <div class="row bg-light-blue-gradient">
                                <asp:Label ID="Label12" runat="server" Text="Owner's Equity" 
                                    CssClass="pull-left text-bold" Font-Bold="True" 
                                    Font-Size="Large"></asp:Label>
                            </div>
                            <div class="row">
                                <asp:Label ID="Label5" runat="server" Text="Invest (Capital)" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblInvest" runat="server" Text="" CssClass="pull-right"></asp:Label>
                                 
                            </div>
                            <div class="row">
                             <asp:Label ID="Label14" runat="server" Text="Withdraw From Business (Capital)" CssClass="pull-left"></asp:Label>
                                <asp:Label ID="lblWithdrawFB" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="border: 1px solid black;">
                        <div class="col-md-6 bg-black-gradient" style="padding:0 25px;">
                            <div class="row">
                                <asp:Label ID="Label8" runat="server" Text="Total Asset" CssClass="pull-left text-bold"></asp:Label>
                                <asp:Label ID="lblTotalAsset" runat="server" CssClass="pull-right text-bold" 
                                    Font-Bold="True" Font-Size="Larger"></asp:Label>
                            </div>
                        </div>

                        <div class="col-md-6 bg-light-blue-gradient" style="padding:0 25px; border-left: 3px solid black;">
                            <div class="row">
                                <asp:Label ID="Label9" runat="server" Text="Total Liability and Owner's Equity" CssClass="pull-left text-bold"></asp:Label>
                                <asp:Label ID="lblTotalLOE" runat="server" CssClass="pull-right text-bold" 
                                    Font-Bold="True" Font-Size="Larger"></asp:Label>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
