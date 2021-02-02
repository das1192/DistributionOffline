<%@ Page Title="Shop Status" MasterPageFile="~/MasterPage.master" Language="C#" AutoEventWireup="true"
    CodeFile="add_shopStatus.aspx.cs" Inherits="Pages_add_shopStatus" %>

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
            <div class="row" style="padding-bottom: 10px;">
                <div class="col-md-9">
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" CssClass="pull-right"
                        BackColor="#009933" ForeColor="White"></asp:Label>
                </div>
            </div>
            <asp:TabContainer ID="ContainerBankInfo" runat="server" ActiveTabIndex="1" Width="100%"
                CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Shop List">
                    <HeaderTemplate>
                        Shop List with Status
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="row" style="padding: 15px;">
                            <asp:GridView ID="gvShop" runat="server" AutoGenerateColumns="False" GridLines="None"
                                CssClass="table table-bordered table-condensed table-hover table-responsive"
                                DataKeyNames="OID,ActiveStatus" EmptyDataText="No rows returned" Width="100%">
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            SL NO</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="OID" HeaderText="OID" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopCode" HeaderText="Code">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopName" HeaderText="Name">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopMobile" HeaderText="Mobile No">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopAddress" HeaderText="Address">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ActiveStatus" HeaderText="Active Status">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Modify">
                                        <ItemTemplate>
                                            <asp:Button ID="btnActiveInactive" runat="server" Text="Active" CssClass="btn btn-sm"
                                                BackColor="White" ForeColor="#006600" OnClientClick="return confirm('Are you sure?')"
                                                OnClick="btnActiveInactive_Click" BorderColor="#000066" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Shop List">
                    <HeaderTemplate>
                        Payment Status
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div>
                        <div class="row" style="padding-bottom:10px;">
                                <div class="col-md-2">
                                    Shop
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlShopName" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-4">
                                    
                                </div>
                            </div>
                            <div class="row" style="padding-bottom:10px;">
                                <div class="col-md-2">
                                   <asp:Button ID="btnShopPayment" runat="server" Text="Show" CssClass="btn btn-sm btn-info"
                                        OnClick="btnShopPayment_Click" />
                                </div>
                                <div class="col-md-4">
                                </div>
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-4">
                                   
                                </div>
                            </div>
                            <div class="row" style="padding:10px;">
                                <asp:GridView ID="gvShopPayment" runat="server" AutoGenerateColumns="False" GridLines="None"
                                    CssClass="table table-bordered table-condensed table-hover table-responsive"
                                    DataKeyNames="OID,StatusPayment" EmptyDataText="No rows returned" Width="100%">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                SL NO</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ShopName" HeaderText="To">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StatusPayment" HeaderText="StatusPayment">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EntryDate" HeaderText="Date">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MonthYear" HeaderText="Month">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>




                                        <asp:BoundField DataField="MonthlyFee" HeaderText="Monthly Fee">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalFee" HeaderText="Total">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TransactionID" HeaderText="Month No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Approved">
                                        <ItemTemplate>
                                            <asp:Button ID="btnPaymentApproved" runat="server" Text="Approved" CssClass="btn btn-success"
                                                BackColor="White" ForeColor="#006600" OnClientClick="return confirm('Are you sure?')"
                                                 BorderColor="#000066" onclick="btnPaymentApproved_Click" 
                                                Visible="False" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
