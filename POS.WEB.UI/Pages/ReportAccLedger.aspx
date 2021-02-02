<%@ Page Title="Ledger Report" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReportAccLedger.aspx.cs" Inherits="Pages_ReportAccLedger" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
       
    </script>
    <style type="text/css">
        .style1
        {
            font-size: large;
        }
        .centerHeaderText th
        {
            text-align: center;
        }
    </style>
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
                    <div class="table-responsive bg-success" style="background-color: #debc94;padding:10px 25px;border: 3px solid #93718b;">
                       
                        <div class="bg-dark" style="padding:5px;font-style:">
                         <h2 class="text-center text-white" style="background-color:#B8860B;color:#fff;padding:3px;font-weight:bold;">LEDGER</h2>
                        </div>
                        <div class="table-responsive">
                            <section class="panel">
                               <%-- <header class="panel-heading">
                              Search Criteria
                          </header>--%>
                          <h4>Search Criteria</h4>
                          <hr />
                        <table width="100%" cellspacing="5px" cellpadding="10px" style="padding-top:15px;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" Style="font-weight: bold" runat="server" Text="From Date :"></asp:Label>&nbsp;<br />
                                </td>
                                <td>
                                    <asp:TextBox ID="dptFromDate" runat="server" Width="150px" CssClass="form-control"
                                        onkeypress="return clearTextBox()" /> 


                                    <asp:CalendarExtender ID="CalendarExtender2" Format="dd MMM yyyy" runat="server" Enabled="True"
                                        TargetControlID="dptFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label7" Style="font-weight: bold" runat="server" Text="To Date :"></asp:Label>&nbsp;<br />
                                </td>
                                <td>
                                    <asp:TextBox ID="dptToDate" runat="server" Width="150px" CssClass="form-control"
                                        onkeypress="return clearTextBox()" />
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="dd MMM yyyy" runat="server" Enabled="True"
                                        TargetControlID="dptToDate" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    <asp:Label ID="lblDetailsType" runat="server" Style="font-weight: bold" CssClass="form-label" Text="Accounts Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDetailsType" runat="server" CssClass="form-control" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlDetailsType_SelectedIndexChanged" ></asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblDetails" runat="server" Style="font-weight: bold" CssClass="form-label" Text="Accounts Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDetails" runat="server" CssClass="form-control" ></asp:DropDownList>
                                </td>
                            </tr>
                             <tr style="display:none">
                            <td>
                            
                            </td>
                            <td>
                            <asp:TextBox ID="txtDetails" runat="server" Text='<% #Eval("Details") %>'
                                                            CssClass="form-control"  AutoPostBack="True" OnTextChanged="txtDetails_TextChanged"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ServiceMethod="GetExpenseHeadEID" MinimumPrefixLength="0"
                                                            CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" TargetControlID="txtDetails"
                                                            ID="ACE_txtDetails" runat="server" FirstRowSelected="false">
                                                        </cc1:AutoCompleteExtender>
                                                        <asp:Label ID="lbltxtDetailsID" runat="server"
                                                            Visible="false"></asp:Label>
                            </td>
                            </tr>
                            <tr>
                                <td colspan="">
                                    <asp:Button ID="btnSearchPurchaseInvoice" runat="server" CssClass="inline btn btn-info"
                                        OnClick="btnSearchPurchaseInvoice_Click" Text="Search" />
                                </td>
                                <td>
                                    <asp:Button ID="btnLedgerReport" runat="server" Text="Download PDF" 
                                        CssClass="inline btn btn-warning" onclick="btnLedgerReport_Click"></asp:Button>
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
                        <div class="row">
                            <%--<div class="col-md-2">
                            </div>--%>
                            <div class="col-md-12">
                                <asp:Label ID="lblDetailsHead" runat="server" CssClass="text-center text-info lead"
                                    BackColor="#33CC33" Font-Bold="True" ForeColor="White" Width="100%"></asp:Label>
                                <asp:GridView ID="gvLedger" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                    ShowHeaderWhenEmpty="True" Width="100%" OnRowEditing="gvLedger_RowEditing" AllowPaging="True"
                                    PageSize="25" DataKeyNames="" OnPageIndexChanging="gvLedger_PageIndexChanging"
                                    CssClass="table table-responsive table-bordered table-striped table-advance"
                                    ShowFooter="True"  >
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
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Particular">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDetails" runat="server" Text='<%#Bind("Particular") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="Total" CssClass="text-right"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Left" Width="20%" />
                                        </asp:TemplateField>
                                                <%--j.Particular,CONVERT(nvarchar(12),j.IDATTIME,106)EntryDate,j.AccountID,j.Narration
                                                , j.Debit,j.Credit,Balance  RefferenceNumber--%>
                                       <asp:TemplateField HeaderText="Reference No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRefferenceNumber" runat="server" Text='<%#Bind("RefferenceNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDrAmount" runat="server" Text='<%#Bind("Debit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalDrAmount" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Font-Size="Small" HorizontalAlign="Right" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCrAmount" runat="server" Text='<%#Bind("Credit") %>'></asp:Label>
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
                                    <HeaderStyle BackColor="#00CC00" ForeColor="White"  CssClass="centerHeaderText"/>
                                    <PagerSettings Position="TopAndBottom" />
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
