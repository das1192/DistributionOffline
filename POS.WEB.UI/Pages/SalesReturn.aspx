<%@ Page Title="Sales Return" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalesReturn.aspx.cs" Inherits="Pages_SalesReturn" %>

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
                    <td align="center" colspan="3" style="font-weight: bolder">
                        Sales Return
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 20%">
                        <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Invoice No:">
                        </asp:Label>
                    </td>
                    <td style="width: 60%">
                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" AutoPostBack="true"
                            OnTextChanged="txtInvoiceNo_TextChanged" />
                    </td>
                    <td style="width: 20%;" valign="bottom">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                        <asp:Label ID="Label2" runat="server" Style="font-weight: bold" Text="Reason:"></asp:Label>
                    </td>
                    <td style="width: 60%">
                        <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Width="800px" />
                    </td>
                    <td style="width: 20%;" valign="bottom">
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                    </td>
                    <td colspan="2" style="width: 80%">
                        <asp:GridView ID="gvT_Issue_REQUISITION_DTL" runat="server" AutoGenerateColumns="False"
                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                            CellPadding="3" EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" Width="850px">
                            <AlternatingRowStyle BackColor="AliceBlue" />
                            <Columns>
                                <asp:TemplateField Visible="false" HeaderText="CCOM_CODE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStoreID" runat="server" Text='<%#Bind("StoreID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="True" HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCCOM_NAME" runat="server" Text='<%#Bind("ShopName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Left" />
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
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubCategoryID" runat="server" Text='<%#Bind("SubCategoryID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Model">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubCategory" runat="server" Text='<%#Bind("SubCategory") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescriptionID" runat="server" Text='<%#Bind("DescriptionID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description/Color">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return Qty Pcs">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyPcs" runat="server" Text='<%#Bind("QtyPcs") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid_header" />
                                    <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
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
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                    </td>
                    <td colspan="2" style="width: 80%">
                       <asp:Button ID="btnIssue" runat="server" Text="Drop" CssClass="btn btn-success" OnClick="btnIssue_Click"
                            OnClientClick="this.disabled=true;" UseSubmitBehavior="false" Enabled="true" />&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                            CssClass="btn btn-warning" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
