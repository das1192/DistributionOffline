<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmProductDelete.aspx.cs" Inherits="Pages_frmProductDelete" %>

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
            <asp:TabContainer ID="ContainerStockReturn" runat="server" ActiveTabIndex="0" 
                Width="100%">               

                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Product Delete">
                    <ContentTemplate>
                        <fieldset style="border: 1px Solid Black;">
                            <legend>Product Delete</legend>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblBarcode" runat="server" Text="Barcode"></asp:Label>
                                        <asp:TextBox ID="txtBarCode" runat="server" CssClass="TextBox" AutoPostBack="True"
                                            OnTextChanged="txtBarcode_TextChanged" />
                                    </td>                                  
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvStockReturn" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="OID"
                                            EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" Width="850px" OnRowDeleting="gvStockReturn_RowDeleting">
                                            <AlternatingRowStyle BackColor="AliceBlue" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        SI</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="OID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOID" runat="server" Text='<%#Bind("OID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Branch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCCOM_NAME" runat="server" Text='<%#Bind("CCOM_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                               
                                                <asp:TemplateField HeaderText="Brand">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("WGPG_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                             
                                                <asp:TemplateField HeaderText="Model">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubCategory" runat="server" Text='<%#Bind("SubCategoryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                            
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Barcode">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                               
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                            Text="Delete">
                                                            <img alt="Delete" src="../Images/Delete.gif" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                     <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
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
                        </fieldset>
                        <table>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" CssClass="Button_VariableWidth" CausesValidation="False"
                                        OnClientClick="this.disabled=true;" UseSubmitBehavior="False" OnClick="btnSave_Click"
                                        Text="Save   " />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="Button_VariableWidth"
                                        OnClick="btnCancel_Click" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

