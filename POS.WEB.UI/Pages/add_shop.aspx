<%@ Page Title="Add Shop" MasterPageFile="~/MasterPage.master" Language="C#" AutoEventWireup="true" CodeFile="add_shop.aspx.cs" Inherits="Pages_add_shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" Runat="Server">
<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContainerBankInfo" runat="server" ActiveTabIndex="0"
                Width="100%" CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Shop List">
                    <HeaderTemplate>
                        Shop List
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">                           
                            <tr>
                                <td>
                                    <asp:GridView ID="gvShop" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        DataKeyNames="OID" EmptyDataText="No rows returned" OnRowDeleting="gvShop_RowDeleting"
                                        OnRowEditing="gvShop_RowEditing" Width="100%">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OID" HeaderText="OID" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ShopName" HeaderText="Bank Name">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="Active" HeaderText="Active">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                ShowEditButton="True" EditImageUrl="~/Images/edit.png" ButtonType="Image">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:CommandField>

                                            <asp:TemplateField HeaderText="Delete" Visible="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        Text="Delete" OnClientClick="return confirm('Are you sure to delete?')">
                                                        <img alt="Delete" src="../Images/Delete.gif" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Add/Edit Shop">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblShopName" runat="server" Text="Shop Name"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShopName" runat="server" CssClass="TextBox" />
                                    <asp:RequiredFieldValidator ID="rfvWGPG_NAME" ControlToValidate="txtShopName"
                                        ErrorMessage="Shop Name is required" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                        CssClass="btn btn-default" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" Enabled="true" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                        CssClass="btn btn-default" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    <asp:HiddenField ID="lblOID" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
