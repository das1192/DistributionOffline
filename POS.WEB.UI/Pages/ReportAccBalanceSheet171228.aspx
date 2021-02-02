<%@ Page Title="Balance Sheet" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReportAccBalanceSheet171228.aspx.cs" Inherits="Pages_ReportAccBalanceSheet171228" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
       
    </script>
    <style type="text/css">
        .style1
        {
            font-size: large;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <div id="main-content">
    <div>
    <h2>Developing...</h2>
    </div>
        <div class="wrapper" style="display:none">
            <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                <ProgressTemplate>
                    <img alt="progress" src="../Images/progress.gif" />
                    Processing...
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="table-responsive bg-success" style="background-color: #ecddc6">
                        <div class="bg-dark" style="padding: 5px; font-style: ">
                            <h2 class="text-center text-white">
                                Profit or Loss</h2>
                        </div>
                        <div class="table-responsive">
                            <section class="panel">
                                <header class="panel-heading">
                              Search Criteria
                          </header>
                        <table width="100%" cellspacing="5px" cellpadding="10px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" Style="font-weight: bold" runat="server" Text="From Date :"></asp:Label>&nbsp;<br />
                                </td>
                                <td>
                                    <asp:TextBox ID="dptFromDate" runat="server" Width="150px" CssClass="form-control mx-sm-3"
                                        onkeypress="return clearTextBox()" /> 


                                    <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="dptFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label7" Style="font-weight: bold" runat="server" Text="To Date :"></asp:Label>&nbsp;<br />
                                </td>
                                <td>
                                    <asp:TextBox ID="dptToDate" runat="server" Width="150px" CssClass="form-control"
                                        onkeypress="return clearTextBox()" />
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="dptToDate" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnShow" runat="server" CssClass="inline btn btn-info"
                                        OnClick="btnShow_Click" Text="Search" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                   
                                </td>
                            </tr>
                           
                        </table>
                        </div>
                        <hr />
                        <div class="row" style="padding:0 50px;">
                            <div class="col-md-6">
                                <asp:Label ID="Label1" runat="server" Text="Opening Stock Value" CssClass="pull-left" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblOpeningStockValue" runat="server" Text="" CssClass="pull-right" Font-Bold="True"></asp:Label>
                                <br />
                                <asp:Label ID="Label3" runat="server" Text="Purchase Value" CssClass="pull-left" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblPurchaseValue" runat="server" Text="" CssClass="pull-right" Font-Bold="True"></asp:Label>
                                <br />
                                <asp:Label ID="Label5" runat="server" Text="DirectExpense" CssClass="pull-left" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblDirectExpense" runat="server" Text="" CssClass="pull-right" Font-Bold="True"></asp:Label>
                                
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="Label9" runat="server" Text="Net Sale" CssClass="pull-left" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblNetSale" runat="server" Text="" CssClass="pull-right" Font-Bold="True"></asp:Label>
                                <br />
                                <asp:Label ID="Label11" runat="server" Text="Closing Stock" CssClass="pull-left" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblClosingStock" runat="server" CssClass="pull-right" 
                                    Font-Bold="True"></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="border-top:1px solid green;padding:0 50px;">
                            <div class="col-md-6">
                                <asp:Label ID="lblGrossProfitLabel" runat="server" Text="" CssClass="pull-left lead" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblGrossProfit" runat="server" Text="" CssClass="pull-right lead"></asp:Label>
                                
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblGrossLossLabel" runat="server" Text="" CssClass="pull-left lead" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblGrossLoss" runat="server" Text="" CssClass="pull-right lead" Font-Bold="True"></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="padding:0 50px;">
                            <div class="col-md-6">
                                <asp:Label ID="Label2" runat="server" Text="Indirect Expense" CssClass="pull-left" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblIndirectExpense" runat="server" Text="" CssClass="pull-right" Font-Bold="True"></asp:Label>
                                
                            </div>
                            <div class="col-md-6">
                               
                            </div>
                        </div>
                        <div class="row" style="border-top:1px solid green;padding:0 50px;">
                            <div class="col-md-6">
                                <asp:Label ID="lblNetProfitLabel" runat="server" Text="" CssClass="pull-left lead" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblNetProfit" runat="server" Text="" CssClass="pull-right lead" Font-Bold="True"></asp:Label>
                                
                            </div>
                            <div class="col-md-6">
                                <asp:Label ID="lblNetLossLabel" runat="server" Text="" CssClass="pull-left lead" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblNetLoss" runat="server" Text="" CssClass="pull-right lead" Font-Bold="True"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-md-2">
                            </div>--%>
                            <div class="col-md-12">
                                <asp:Label ID="lblDetailsHead" runat="server" CssClass="text-center text-info lead"
                                    BackColor="#33CC33" Font-Bold="True" ForeColor="White" Width="100%"></asp:Label>
                                <asp:GridView ID="gvLedger" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                    ShowHeaderWhenEmpty="True" Width="100%" OnRowEditing="gvLedger_RowEditing" AllowPaging="True"
                                    PageSize="25" DataKeyNames="Details,DetailsID " OnPageIndexChanging="gvLedger_PageIndexChanging"
                                    CssClass="table table-responsive table-bordered table-striped table-advance"
                                    ShowFooter="True" ForeColor="White">
                                    <AlternatingRowStyle BackColor="AliceBlue" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                SL NO</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntryDate" runat="server" Text='<%#Bind("EntryDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDetails" runat="server" Text='<%#Bind("Details") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Left" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Journal Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDrAmount" runat="server" Text='<%#Bind("JournalCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dr Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDrAmount" runat="server" Text='<%#Bind("DrAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalDrAmount" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Right" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cr Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCrAmount" runat="server" Text='<%#Bind("CrAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCrAmount" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Right" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%#Bind("Balance") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalBalance" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Right" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Narration">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNarration" runat="server" Text='<%#Bind("Narration") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Left" Width="25%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle Font-Bold="True" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#00CC00" ForeColor="White" />
                                    <HeaderStyle BackColor="#00CC00" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                </asp:GridView>
                            </div>
                            <%-- <div class="col-md-2">
                            </div>--%>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
