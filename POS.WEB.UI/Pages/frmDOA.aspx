<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmDOA.aspx.cs" Inherits="Pages_frmDOA" %>

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

                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="DOA">
                    <ContentTemplate>
                        <fieldset style="border: 1px Solid Black;">
                            <legend>Product Transfer To DOA</legend>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblBarcode" runat="server" Text="Barcode"></asp:Label>
                                        <asp:TextBox ID="txtBarCode" runat="server" CssClass="TextBox" 
                                            AutoPostBack="True" ontextchanged="txtBarcode_TextChanged"
                                            />
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
                                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                            EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" Width="850px" OnRowDeleting="gvStockReturn_RowDeleting">
                                            <AlternatingRowStyle BackColor="AliceBlue" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>SI</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                
                                                                                        

                                                <asp:TemplateField HeaderText="PCategoryID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategoryId" runat="server" Text='<%#Bind("PCategoryID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Brand">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("PCategoryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SubCategoryID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubCategoryID" runat="server" Text='<%#Bind("SubCategoryID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Model">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubCategory" runat="server" Text='<%#Bind("SubCategory") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DescriptionID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescriptionID" runat="server" Text='<%#Bind("DescriptionID") %>'></asp:Label>
                                                    </ItemTemplate>
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

                                                 <asp:TemplateField HeaderText="StockInHand">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockInHand" runat="server" Text='<%#Bind("StockInHand") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Qty Pcs">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblQtyPcs" runat="server" Width="40px" Text='<%#Bind("QtyPcs") %>'
                                                            AutoPostBack="true"></asp:TextBox>
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
                                    <asp:Button ID="btnSave" runat="server" CssClass="Button_VariableWidth" CausesValidation="False"
                                        OnClientClick="this.disabled=true;" UseSubmitBehavior="False" 
                                        Text="Save" onclick="btnSave_Click" Width="100px" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="Button_VariableWidth"
                                       Text="Cancel" Width="100px" onclick="btnCancel_Click"/>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

