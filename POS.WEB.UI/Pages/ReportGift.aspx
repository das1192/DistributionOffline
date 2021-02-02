<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ReportGift.aspx.cs" Inherits="Pages_ReportGift" EnableEventValidation="false" %>

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
                        Report of Gift
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
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
                    <td>
                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CssClass="btn btn-success"
                            OnClick="cmdProcess_Click" Text="Search" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvGift" runat="server" 
                          Width="100%"  
                          CssClass="table table-bordered table-condensed table-hover table-responsive table-light" 
                            AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                            <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                            <asp:BoundField DataField="Description" HeaderText="Item Name" />
                            <asp:BoundField DataField="SESPrice" HeaderText="Price" />
                            </Columns>
                            
                        </asp:GridView>
                    </td>

                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
