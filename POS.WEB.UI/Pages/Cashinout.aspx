<%@ Page Title="Cash Flow" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Cashinout.aspx.cs" Inherits="Pages_Cashinout" EnableEventValidation="false" %>

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
                        Cash Flow 
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
                        <asp:GridView ID="gvCashINOUT" runat="server" AutoGenerateColumns="False" 
                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                            CellPadding="3" EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" 
                            Width="100%">
                            <AlternatingRowStyle BackColor="AliceBlue" />
                            <Columns>
                                <asp:TemplateField HeaderText="Total Cash In">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCostPrice" runat="server" Text='<%#Bind("CashIN") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Cash Out">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("CashOUT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIDAT" runat="server" Text='<%#Bind("IDAT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td align="left" colspan="4" style="width: 100%;" valign="bottom">
                        &nbsp;
                    </td>
                </tr>
                
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

