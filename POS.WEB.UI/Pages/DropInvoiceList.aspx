<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DropInvoiceList.aspx.cs" Inherits="Pages_DropInvoiceList" EnableEventValidation="false" %>

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
            <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td align="center" style="font-weight: bolder">
                        Drop Invoice List
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
                        <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                            CssClass="btn btn-success" onclick="cmdProcess_Click" Text="Process" />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
              
               
                <tr>
                    <td colspan="4" align="left" style="width: 100%;" valign="bottom">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvInvoiceList" runat="server" AutoGenerateColumns="False" 
                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                            CellPadding="3" DataKeyNames="InvoiceNo" EmptyDataText="No rows returned" 
                            OnRowCommand="ItemDetails" ShowHeaderWhenEmpty="True" Width="100%">
                            <AlternatingRowStyle BackColor="AliceBlue" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        SI
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSRNO" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InvoiceNo" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Bind("InvoiceNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice No">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkView" runat="server" 
                                            CommandArgument='<%#Eval("InvoiceNo") %>' CommandName="ItemDetails" 
                                            Text='<%# Eval("InvoiceNo") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCCOM_NAME" runat="server" Text='<%#Bind("CCOM_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Drop Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDropStatus" runat="server" Text='<%#Bind("DropStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Drop Reason">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReason" runat="server" Text='<%#Bind("Reason") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" 
                                            Text='<%#Bind("InvoiceDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Request Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestDate" runat="server" 
                                            Text='<%#Bind("RequestDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approved Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproveddate" runat="server" 
                                            Text='<%#Bind("Approveddate", "{0:dd/MM/yyyy}") %>'></asp:Label>
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
                    <td align="left" colspan="5" style="width: 100%;" valign="bottom">
                        &nbsp;
                    </td>
                </tr>
                
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

