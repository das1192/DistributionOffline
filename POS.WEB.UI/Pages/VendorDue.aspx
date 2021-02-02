<%@ Page Title="Vendor Due" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="VendorDue.aspx.cs" Inherits="Pages_VendorDue"
    EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerVendorDue" runat="server" ActiveTabIndex="0" Width="100%" CssClass="fancy fancy-green">
                
                <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Supplier Due Report">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Supplier"></asp:Label>:
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlVendorID" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="Cascadingdropdown1" runat="server" Category="Vendor"
                                            TargetControlID="ddlVendorID" LoadingText="Loading Categories..."
                                            PromptText="--Please Select--" ServiceMethod="BindVendor" ServicePath="~/DropdownWebService.asmx"
                                            Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                
                                
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <p>
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="6">
                                    <asp:Button ID="cmdSearchForprice" runat="server" Text="Search" CssClass="btn btn-default"
                                        CausesValidation="False" OnClick="cmdSearchForprice_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="gvVendorDue" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid"  BorderColor="#CCCCCC" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned" Width="100%">
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
                                            
                                                        <asp:TemplateField HeaderText="Supplier">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("Vendor_Name") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Purchase">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPurchase" runat="server" Text='<%#Bind("incomingsum1") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Purchase Return">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPurchase" runat="server" Text='<%#Bind("returnsum") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Payment">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPayment" runat="server" Text='<%#Bind("Outgoingsum") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Due">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBalance" runat="server" Text='<%#Bind("Balance") %>'></asp:Label></ItemTemplate>
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
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
