<%@ Page Title="Adjust On Credit" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="AdjustOncredit.aspx.cs" Inherits="Pages_AdjustOncredit" %>

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


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }





    </script>
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
            <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td align="center" colspan="3" style="font-weight: bolder">
                        Search Invoice
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <div class="row" style="padding: 5px 0px">
                <%--<div class="col-md-2">
                    <asp:Label ID="Label6" Style="font-weight: bold" runat="server" Text="From Date :"></asp:Label>
                </div>--%>
                <div class="col-md-3">
                    <asp:TextBox ID="txtDateFrom" runat="server"  CssClass="form-control"
                        onkeypress="return clearTextBox()" placeholder="From Date" />&nbsp;<br />
                    <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                        TargetControlID="txtDateFrom" />
                </div>
                <%--<div class="col-md-2">
                    <asp:Label ID="Label7" Style="font-weight: bold" runat="server" Text="To Date :"></asp:Label>
                </div>--%>
                <div class="col-md-3">
                    <asp:TextBox ID="txtDateTo" runat="server"  CssClass="form-control"
                        onkeypress="return clearTextBox()" placeholder="To Date" />&nbsp;<br />
                    <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True"
                        TargetControlID="txtDateTo" />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-success" OnClick="Button1_Click"
                        Text="Search" />
                </div>
            </div>
            <div class="row" style="padding: 5px 0px">
                <div class="col-md-3">
                    <asp:DropDownList ID="txtCustomerName" AutoPostBack="False" runat="server" CssClass="form-control"
                        OnSelectedIndexChanged="txtCustomerName_TextChanged">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" onkeypress="return clearTextBox2()"
                        placeholder="Mobile No" />
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" onkeypress="return clearTextBox3()"
                        placeholder="Invoice No" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnIssue" runat="server" Text="Search" CssClass="btn btn-sm btn-success"
                        OnClick="btnIssue_Click" />
                </div>
            </div>
            <div class="row">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <table width="100%" style="display: none">
                <tr>
                    <td colspan="3">
                        <p>
                            &nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20px" runat="server">
                        <br />
                        <%--<asp:CascadingDropDown  runat="server" Category="Customer" 
                                                    LoadingText="Loading Supplier..." 
                            PromptText="--Please Select--" ServiceMethod="BindCustomer"
                                                    ServicePath="~/DropdownWebService.asmx" 
                            Enabled="True" ID="CCD98" TargetControlID="lblCustomerList">
                                                </asp:CascadingDropDown>--%>
                        <%--<asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBox" OnTextChanged="txtCustomerName_TextChanged"
                           />--%>
                    </td>
                    <td style="width: 20px">
                        <asp:Label ID="Label2" runat="server" Style="font-weight: bold" Text="Mobile No:"></asp:Label>
                        <br />
                    </td>
                    <td style="width: 20px">
                        <asp:Label ID="Label4" runat="server" Style="font-weight: bold" Text="Invoice No:"></asp:Label>
                        <br />
                    </td>
                </tr>
            </table>
            <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td>
                        <asp:GridView ID="gvT_Issue_REQUISITION_DTL" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="InvoiceNo,Due" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True"
                            Width="100%">
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
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
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
                                <asp:TemplateField HeaderText="Total Ammount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTAmount" runat="server" Text='<%#Bind("NetAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paid Ammount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPAmount" runat="server" Text='<%#Bind("ReceiveAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Due">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDAmount" runat="server" Text='<%#Bind("Due") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pay">
                                    <ItemTemplate>
                                        <asp:TextBox ID="DuePay" runat="server" CssClass="form-control" Width="100px" Font-Bold="True"
                                            Style="text-align: right;" Enabled="true" Font-Size="Small" Text="0">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="DuePay"
                                            runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+">
                                        </asp:RegularExpressionValidator>
                                        <%--
                                     <asp:LinkButton ID="DPay" runat="server" CausesValidation="false" CommandName="PayDue"
                                            Text="Pay Due">                                                                 
                                      </asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%--OnTextChanged="txtrceamount_TextChanged"--%>
                                <asp:TemplateField HeaderText="Adjust" Visible="True">
                                    <ItemTemplate>
                                        <asp:Button ID="btnAdjust" runat="server" Text="Adjust" OnClientClick="return confirm('Are you sure?')"
                                            OnClick="btnAdjust_Click" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Preview" Visible="True">
                                    <ItemTemplate>
                                        <asp:Button ID="btnPreview" runat="server" Text="Preview" OnClick="btnPreview_Click" />
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
