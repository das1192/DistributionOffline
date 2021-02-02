<%@ Page Title="Invoice List" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SearchInvoiceForAdmin.aspx.cs" Inherits="Pages_SearchInvoiceForAdmin"
    EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                    <td align="center" style="font-weight: bolder">
                        Search Invoice
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Style="font-weight: bold" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList">
                            </asp:DropDownList>
                            <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                ServicePath="~/DropdownWebService.asmx" Enabled="True">
                            </asp:CascadingDropDown>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Style="font-weight: bold" runat="server" Text="From Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />
                            <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                TargetControlID="txtFromDate" />
                        </td>
                        <td>
                            <asp:Label ID="lblReceiveDate" Style="font-weight: bold" runat="server" Text="To Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                            <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                                Enabled="True" TargetControlID="txtToDate" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left" style="width: 100%;" valign="bottom">
                            <asp:Label ID="Label26" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="height: 16px; width: 100%">
            </div>

            <div style="margin-bottom: 50px;">
                <div style="float: left;">
                    <table>
                        <tr>
                            <td style="padding: 5px;">
                                <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Drop Status:"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbtYes" runat="server" Text="Yes" GroupName="Software" Style="font-weight: bold" />
                                &nbsp;
                                <asp:RadioButton ID="rbtNo" runat="server" Text="No" GroupName="Software" Checked="True"
                                    Style="font-weight: bold" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: right; margin-right: 303px;">
                    <table>
                        <tr>
                            <td style="padding: 5px;">
                                <asp:Label ID="Label4" runat="server" Style="font-weight: bold" Text="Report Type:"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbtInvoiceList" runat="server" Text="Invoice List" GroupName="Software1"
                                    Checked="True" Style="font-weight: bold" />
                                &nbsp;
                                <asp:RadioButton ID="rbtInvoiceDetailDatabase" runat="server" Text="Invoice Detail Database"
                                    GroupName="Software1" Style="font-weight: bold" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
                        
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Button ID="cmdProcess" runat="server" Text="Search" CssClass="Button_VariableWidth"
                                CausesValidation="False" OnClick="cmdProcess_Click" />
                        </td>
                        <td>
                            <asp:Button ID="cmdExport" runat="server" Text="Export To Excel" CssClass="Button_VariableWidth"
                                OnClick="cmdExport_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvInvoiceList" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="InvoiceNo"
                                EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="StockReturn_Details">
                                <AlternatingRowStyle BackColor="AliceBlue" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            SI
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSRNO" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Bind("InvoiceNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkView" CommandArgument='<%#Eval("InvoiceNo") %>'
                                                Text='<%# Eval("InvoiceNo") %>' CommandName="Invoice"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%#Bind("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle Font-Size="Small" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Address" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" runat="server" Text='<%#Bind("Address") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle Font-Size="Small" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mobile">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobileNo" runat="server" Text='<%#Bind("MobileNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIDAT" runat="server" Text='<%#Bind("IDAT", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Drop Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApproved" runat="server" Text='<%# RenderPriority(DataBinder.Eval(Container.DataItem,"DropStatus")) %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sales Person">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Invoice Date">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkChange" Text="Change" runat="server" CommandArgument='<%# Eval("InvoiceNo")%>'
                                                CommandName="InvoiceNo"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
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
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvDetailsInvoice" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                ShowHeaderWhenEmpty="True" Width="100%">
                                <AlternatingRowStyle BackColor="AliceBlue" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            SI
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSRNO" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo1" runat="server" Text='<%#Bind("InvoiceNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIDAT1" runat="server" Text='<%#Bind("Purchase_Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName1" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DOB" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateOfBirth1" runat="server" Text='<%#Bind("DOB","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MOB">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobileNo1" runat="server" Text='<%#Bind("MOB") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ALT_MOB" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAlternativeMobileNo1" runat="server" Text='<%#Bind("ALT_MOB") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Address" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress1" runat="server" Text='<%#Bind("Address") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWGPG_NAME1" runat="server" Text='<%#Bind("Category") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCategoryName1" runat="server" Text='<%#Bind("Model") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description/Color">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription1" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Barcode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Method Of Payment" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaymentMode1" runat="server" Text='<%#Bind("PaymentMode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmailAddress" runat="server" Text='<%#Bind("EmailAddress") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Drop Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDropStatus" runat="server" Text='<%#Bind("DropStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="grid_header" />
                                        <ItemStyle  Font-Size="Small" HorizontalAlign="Center" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
