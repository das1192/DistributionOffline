<%@ Page Title="Search Invoice" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SearchInvoiceForReturn.aspx.cs" Inherits="Pages_SearchInvoiceForReturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function clearTextBox1() {
            document.getElementById('<%= txtMobileNo.ClientID %>').value = "";
            document.getElementById('<%= txtInvoiceNo.ClientID %>').value = "";
        }
        function clearTextBox2() {
            document.getElementById('<%= txtCustomerName.ClientID %>').value = "";
            document.getElementById('<%= txtInvoiceNo.ClientID %>').value = "";
        }
        function clearTextBox3() {
            document.getElementById('<%= txtMobileNo.ClientID %>').value = "";
            document.getElementById('<%= txtCustomerName.ClientID %>').value = "";
        }
        function clearTextBox() {
            document.getElementById('<%= txtMobileNo.ClientID %>').value = "";
            document.getElementById('<%= txtCustomerName.ClientID %>').value = "";
            document.getElementById('<%= txtInvoiceNo.ClientID %>').value = "";
        }
    </script>
    <style type="text/css">
        .style1
        {
            font-size: large;
        }
        /*AutoComplete flyout */
        .completionList
        {
            border: solid 1px #444444;
            margin: 0px;
            padding: 2px;
            height: 100px;
            overflow: auto;
            background-color: #FFFFFF;
        }
        
        .listItem
        {
            color: #1C1C1C;
        }
        
        .itemHighlighted
        {
            background-color: #ffc0c0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            </div>
            <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td align="center" colspan="3" style="font-weight: bolder">
                        <span class="style1">Sale Return By Invoice/IMEI</span>
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <div class="row" style="padding: 5px 0px; display: none">
                <div class="col-md-2">
                    <asp:TextBox ID="txtDateFrom" runat="server" Width="150px" CssClass="form-control"
                        placeholder="From Date" onkeypress="return clearTextBox()" />
                    <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                        TargetControlID="txtDateFrom" />
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtDateTo" runat="server" Width="150px" CssClass="form-control"
                        placeholder="To Date" onkeypress="return clearTextBox()" />
                    <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True"
                        TargetControlID="txtDateTo" />
                </div>
                <div class="col-md-2">
                </div>
                <div class="col-md-1">
                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-warning" OnClick="Button1_Click"
                        Text="Search" />
                </div>
            </div>
            <div class="row" style="padding: 5px 0px">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="row" style="padding: 5px 0px">
                <div class="col-md-11">
                    <div class="row">
                        <div class="col-md-3">
                            <asp:TextBox ID="txtCustomerName" placeholder="Customer Name" runat="server" CssClass="form-control"
                                OnTextChanged="txtCustomerName_TextChanged" AutoPostBack="true" onkeypress="return clearTextBox1()" />
                            <asp:AutoCompleteExtender ID="ace_txtCustomerName" runat="server" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionListElementID="listPlacement" EnableCaching="False" MinimumPrefixLength="0"
                                ServiceMethod="GetRetailerNames" ServicePath="~/DropdownWebService.asmx" TargetControlID="txtCustomerName"
                                ContextKey="True">
                            </asp:AutoCompleteExtender>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtMobileNo" runat="server" placeholder="Mobile No" CssClass="form-control"
                                onkeypress="return clearTextBox2()" />
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtInvoiceNo" runat="server" placeholder="Invoice No" CssClass="form-control"
                                onkeypress="return clearTextBox3()" />
                            <asp:AutoCompleteExtender ID="ace_txtInvoiceNo" runat="server" CompletionListCssClass="completionList"
                                CompletionListHighlightedItemCssClass="itemHighlighted" CompletionListItemCssClass="listItem"
                                CompletionListElementID="listPlacement" EnableCaching="False" MinimumPrefixLength="0"
                                ServiceMethod="GetInvoices" ServicePath="~/DropdownWebService.asmx" TargetControlID="txtInvoiceNo"
                                ContextKey="True">
                            </asp:AutoCompleteExtender>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtIMENoSearch" runat="server" placeholder="IME NO" CssClass="form-control"
                                OnTextChanged="txtIMENoSearch_TextChanged" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnIssue" runat="server" Text="Search" CssClass="btn btn-sm btn-success"
                        OnClick="btnIssue_Click" />&nbsp;&nbsp;
                </div>
                <div class="col-md-11">
                    <div class="row">
                        <div class="col-md-3">
                            From Date:
                            <asp:TextBox ID="dptFromDate" runat="server" Width="150px" CssClass="form-control"
                                onkeypress="return clearTextBox()" />
                            <asp:CalendarExtender ID="CalendarExtender1" Format="dd MMM yyyy" runat="server"
                                Enabled="True" TargetControlID="dptFromDate" />
                        </div>
                        <div class="col-md-3">
                            To Date:
                            <asp:TextBox ID="dptToDate" runat="server" Width="150px" CssClass="form-control"
                                onkeypress="return clearTextBox()" />
                            <asp:CalendarExtender ID="CalendarExtender4" Format="dd MMM yyyy" runat="server"
                                Enabled="True" TargetControlID="dptToDate" />
                        </div>
                        <div class="col-md-1">
                        <br />
                            <asp:Button ID="btnReport" runat="server" Text="View Report" CssClass="btn btn-sm btn-warning"
                                OnClick="btnReport_Click" />
                        </div>
                    </div>
                </div>
            </div>
            </div>
            <div class="table-responsive">
                <div class="row" style="padding: 5px 50px">
                    <asp:GridView ID="gvT_Issue_REQUISITION_DTL" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="SlNo,InvoiceNo,RetailerName,Address,MobileNo,InvoiceDate,Barcode,Description"
                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                        CellPadding="3" CssClass="table table-responsive table-bordered table-hover table-condensed"
                        EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" Width="100%">
                        <AlternatingRowStyle BackColor="AliceBlue" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    SI</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice">
                                <ItemTemplate>
                                    <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Bind("InvoiceNo") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Retailer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%#Bind("RetailerName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Address" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAddress" runat="server" Text='<%#Bind("Address") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mobile">
                                <ItemTemplate>
                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%#Bind("MobileNo") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDAT" runat="server" Text='<%#Bind("InvoiceDate")%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IMEI">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblApproved" Text='<%# Bind("Description")%>' runat="server" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Preview" Visible="True">
                                <HeaderTemplate>
                                    <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="btn btn-danger btn-sm"
                                        OnClick="btnReturn_Cliked" />
                                    <br />
                                    <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="chkAll_CheckedChanged"
                                        AutoPostBack="true" Text="Select All" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkReturn" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle Font-Bold="True" HorizontalAlign="Center" />
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#669900" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hdfCustomerId" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
