<%@ Page Title="Shop Payment" MasterPageFile="~/MasterPage.master" Language="C#"
    AutoEventWireup="true" CodeFile="add_shopPayment.aspx.cs" Inherits="Pages_add_shopPayment" %>

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
            <div class="row" style="padding: 1px;">
                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" CssClass="pull-right"
                    BackColor="#009933" ForeColor="White"></asp:Label>
            </div>
            <asp:TabContainer ID="tabcon" runat="server" ActiveTabIndex="1" Width="100%" CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Shop List">
                    <HeaderTemplate>
                        Shop Payment List
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div>
                            <div class="row" style="padding: 15px;">
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
                                        
                                        <%--
StatusPayment	EntryDate	MonthFrom	MonthTo	MonthNo	MonthlyFee	TotalFee	Remarks	OId--%>
                                       <%-- <asp:BoundField DataField="MonthNo" HeaderText="Month No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="MonthlyFee" HeaderText="Monthly Fee">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalFee" HeaderText="Total">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Shop Payment Form">
                    <ContentTemplate>
                        <div style="min-height: 500px;">
                            <div class="row">
                                <div class="col-md-10">
                                    <div class="row" style="padding: 3px;">
                                        <div class="col-md-2" style="text-align: right">
                                            From
                                        </div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="dtpFrom" runat="server" CssClass="form-control" AutoPostBack="True"
                                                OnTextChanged="dtpFrom_TextChanged" />
                                            <asp:CalendarExtender ID="CE_dtpFrom" runat="server" Enabled="True" Format="dd-MMM-yyyy"
                                                TargetControlID="dtpFrom" />
                                        </div>
                                        <div class="col-md-2" style="text-align: right">
                                            Month Number
                                        </div>
                                        <div class="col-md-1">
                                            <asp:TextBox ID="txtMonthNo" runat="server" CssClass="form-control" AutoPostBack="True"
                                                OnTextChanged="txtMonthNo_TextChanged" BackColor="#FFFF66"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2" style="text-align: right">
                                            To
                                        </div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="dtpTo" runat="server" CssClass="form-control" Enabled="False" />
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px;">
                                        <div class="col-md-2" style="text-align: right">
                                            Transaction ID
                                        </div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtTransactionID" runat="server" CssClass="form-control" BackColor="#FFFF66" />
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px;">
                                        <div class="col-md-2" style="text-align: right">
                                            Remarks
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="row">
                                        <div class="col-md-6" style="text-align: right">
                                            Monthly Fee
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblMonthlyFee" runat="server"></asp:Label>
                                        </div>
                                        
                                    </div>

                                    <div class="row">
                                    <div class="col-md-6" style="text-align: right">
                                            Total Fee
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblTotalFee" runat="server" BackColor="Yellow" Font-Bold="True" ForeColor="Black"></asp:Label>
                                        </div>
                                        </div>

                                </div>
                            </div>
                            <div class="row" style="padding: 3px;">
                                <div class="col-md-2">
                                    <asp:Button ID="btnSavePayment" runat="server" Text="Send For Approval" CssClass="btn btn-sm btn-success"
                                        OnClientClick="return confirm('Are you sure?')" Font-Bold="True" OnClick="btnSavePayment_Click" />
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblFromDate" runat="server"></asp:Label>
                                    <asp:Label ID="lblToDate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row" style="padding: 3px;">
                            </div>
                            <div class="row" style="padding: 3px;">
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-4">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
