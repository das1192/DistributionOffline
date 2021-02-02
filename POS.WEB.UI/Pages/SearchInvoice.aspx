<%@ Page Title="Search Invoice" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SearchInvoice.aspx.cs" Inherits="Pages_SearchInvoice" %>

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
                            <span class="style1">Search Invoice</span>
                            <hr style="color: Green" />
                        </td>
                    </tr>
                </table>
                <div class="row" style="padding:5px 0px">
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
                <div class="row" style="padding:5px 0px">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
                <div class="row" style="padding:5px 0px">
                    <div class="col-md-11">
                        <div class="row">
                            <div class="col-md-3">
                                <asp:TextBox ID="txtCustomerName" placeholder="Customer Name" runat="server" CssClass="form-control"
                                    OnTextChanged="txtCustomerName_TextChanged" onkeypress="return clearTextBox1()" />
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtMobileNo" runat="server" placeholder="Mobile No" CssClass="form-control"
                                    onkeypress="return clearTextBox2()" />
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtInvoiceNo" runat="server" placeholder="Invoice No" CssClass="form-control"
                                    onkeypress="return clearTextBox3()" />
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtIMENoSearch" runat="server" placeholder="IME NO" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnIssue" runat="server" Text="Search" CssClass="btn btn-sm btn-success"
                            OnClick="btnIssue_Click" />
                    </div>
                </div>
            </div>
            <div class="table-responsive">
                <div class="row"  style="padding:5px 50px">
                    <asp:GridView ID="gvT_Issue_REQUISITION_DTL" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="InvoiceNo" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                        BorderWidth="1px" OnRowEditing="gvT_Issue_REQUISITION_DTL_RowEditing" CellPadding="3"
                        CssClass="table table-responsive table-bordered table-hover table-condensed"
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
                            <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%#Bind("CustomerName") %>'></asp:Label>
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
                                    <asp:Label ID="lblIDAT" runat="server" Text='<%#Bind("IDAT", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Drop Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblApproved" Text='<%# RenderPriority(DataBinder.Eval(Container.DataItem,"DropStatus")) %>'
                                        runat="server" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Preview" Visible="True">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkButtonPreview" runat="server" CausesValidation="false" CommandName="Edit"
                                        Text="Preview">                                                                 
                                    </asp:LinkButton>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
