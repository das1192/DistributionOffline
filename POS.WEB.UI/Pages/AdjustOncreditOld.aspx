<%@ Page Title="Old Adjust On Credit" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="AdjustOncreditOld.aspx.cs" Inherits="Pages_AdjustOncreditOld" %>

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
            <table width="100%" border="0">
                <tr>
                    <td>
                        <asp:Label ID="Label6" Style="font-weight: bold" runat="server" Text="From Date :"></asp:Label>&nbsp;<br />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" Width="150px" CssClass="TextBox" onkeypress="return clearTextBox()" />&nbsp;<br />
                        <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                            TargetControlID="txtDateFrom" />
                    </td>
                    <td>
                        <asp:Label ID="Label7" Style="font-weight: bold" runat="server" Text="To Date :"></asp:Label>&nbsp;<br />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateTo" runat="server" Width="150px" CssClass="TextBox" onkeypress="return clearTextBox()" />&nbsp;<br />
                        <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True"
                            TargetControlID="txtDateTo" />
                    </td>
                    
                </tr>
                <tr>
                <td colspan="4" align="left">
                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-success" 
                        OnClick="Button1_Click" Text="Search" />
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="3">
                        <p>
                            &nbsp;</p>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20px">
                        <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Customer Name:"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBox" OnTextChanged="txtCustomerName_TextChanged"
                            onkeypress="return clearTextBox1()" />
                    </td>
                    <td style="width: 20px">
                        <asp:Label ID="Label2" runat="server" Style="font-weight: bold" Text="Mobile No:"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="TextBox" onkeypress="return clearTextBox2()" />
                    </td>
                    <td style="width: 20px">
                        <asp:Label ID="Label4" runat="server" Style="font-weight: bold" Text="Invoice No:"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="TextBox" onkeypress="return clearTextBox3()" />
                    </td>
                </tr>
               
            </table>
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnIssue" runat="server" Text="Search" 
                            CssClass="btn btn-success" OnClick="btnIssue_Click" />&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td>
                        <asp:GridView ID="gvT_Issue_REQUISITION_DTL" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="InvoiceNo" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                            BorderWidth="1px" OnRowEditing="gvT_Issue_REQUISITION_DTL_RowEditing" CellPadding="3"
                            EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="adjust_oncredit">
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


                                


                                <asp:TemplateField HeaderText="Preview" Visible="True">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkButtonPreview" runat="server" CausesValidation="false" CommandName="Edit"
                                            Text="Preview">                                                                 
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Adjust" Visible="True">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkButtonAdjust" runat="server" CausesValidation="false" CommandName="Adjust"
                                            Text="Adjust">                                                                 
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
