<%@ Page Title="Sales Return Approved" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalesReturnApproved.aspx.cs" Inherits="Pages_SalesReturnApproved" %>

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
            
            <table style="width:100%">
                <tr>
                    <td align="center" colspan="3" style="font-weight: bolder">
                        Sales Return Approved
                        <hr style="color: Green" />
                    </td>
                </tr>
                
                <tr>
                    <td width="20%"> <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Invoice No:"></asp:Label>
                    </td>
                    <td style="width: 60%">
                        <asp:DropDownList ID="ddlInvoiceNo" runat="server" AutoPostBack="True" CssClass="form-control"
                            OnSelectedIndexChanged="ddlInvoiceNo_SelectedIndexChanged">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="20%"> <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td width="20%"> <asp:Label ID="Label4" runat="server" Style="font-weight: bold" Text="Customer Name:"></asp:Label>
                    </td>
                    <td style="width: 60%">
                        <asp:Label ID="lblCustomerName" runat="server" Style="font-weight: bold"></asp:Label>
                        <asp:Label ID="lblCustomerID" runat="server" Style="font-weight: bold" Visible="False"></asp:Label>
                    </td>
                    <td width="20%">
                    </td>
                </tr>
                <tr>
                    <td width="20%"> <asp:Label ID="Label5" runat="server" Style="font-weight: bold" Text="Payment Mode:"></asp:Label>
                    </td>
                    <td style="width: 60%">
                        <asp:Label ID="lblPaymentMode" runat="server" Style="font-weight: bold"></asp:Label>
                        <asp:Label ID="lblPaymentModeID" runat="server" Style="font-weight: bold" Visible="False"></asp:Label>
                    </td>
                    <td width="20%">
                    </td>
                </tr>
                 <tr>
                    <td width="20%"> <asp:Label ID="Label2" runat="server" Style="font-weight: bold" Text="Reason:"></asp:Label>
                    </td>
                    <td style="width: 60%"> <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Width="800px" />
                    </td>
                    <td width="20%">
                    </td>
                </tr>
                 <tr>
                    <td width="20%">
                    </td>
                    <td colspan="2" style="width:80%">
                    <asp:GridView ID="gvT_Issue_REQUISITION_DTL" runat="server" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                        CellPadding="3" EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" Width="850px"
                        DataKeyNames="StoreID,ShopName,GiftAmount,Discount,PCategoryID,PCategoryName,SubCategoryID,SubCategory,DescriptionID,Description,QtyPcs,SalePrice,TotalSalePrice,Barcode,SalesReturnDate,InvoiceNo,CostPrice,CustomerName,IDAT,IUSER">
                        <AlternatingRowStyle BackColor="AliceBlue" />
                        <Columns>
                            <asp:TemplateField Visible="false" HeaderText="CCOM_CODE">
                                <ItemTemplate>
                                    <asp:Label ID="lblStoreID" runat="server" Text='<%#Bind("StoreID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%#Bind("PCategoryID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("PCategoryName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubCategoryID" runat="server" Text='<%#Bind("SubCategoryID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Brand">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubCategory" runat="server" Text='<%#Bind("SubCategory") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescriptionID" runat="server" Text='<%#Bind("DescriptionID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost Price">
                                <ItemTemplate>
                                    <asp:Label ID="lblCostPrice" runat="server" Text='<%#Bind("CostPrice") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Return Qty Pcs">
                                <ItemTemplate>
                                    <asp:Label ID="lblQtyPcs" runat="server" Text='<%#Bind("QtyPcs") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Barcode">
                                <ItemTemplate>
                                    <asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SalesReturnDate" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalesReturnDate" runat="server" Text='<%#Bind("SalesReturnDate") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" Visible="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkButtonEdit" runat="server" CausesValidation="false" CommandName="Edit"
                                        OnClientClick="return confirm('Are you sure to Edit ?')" Text="Edit">
                                                                 <img alt="" src="../Images/edit.png" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle CssClass="grid_header" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" Visible="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                        OnClientClick="return confirm('Are you sure to delete?')" Text="Delete">
                                                              <img alt="" src="../Images/Delete.gif" />
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
                    <asp:Button ID="btnIssue" runat="server" Text="Approved" CssClass="btn btn-success"
                        OnClick="btnIssue_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" CausesValidation="False" runat="server" CssClass="btn btn-info"
                        Text="Cancel" OnClick="btnCancel_Click" />&nbsp;&nbsp;
                    <asp:Button ID="cmdDelete" CausesValidation="False" runat="server" Text="Delete"
                        CssClass="btn btn-warning" OnClick="cmdDelete_Click" />
                    </td>
                </tr>
            </table>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
