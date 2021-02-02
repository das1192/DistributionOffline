<%@ Page Title="Sales Report" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalesReportForAdmin.aspx.cs" Inherits="Pages_SalesReportForAdmin"
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
            <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td align="center" colspan="2" style="font-weight: bolder">
                        Sales Report
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Style="font-weight: bold" Text="Branch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTransferTo" runat="server" CssClass="DropDownList">
                        </asp:DropDownList>
                        <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch2" TargetControlID="ddlTransferTo"
                            LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                        </asp:CascadingDropDown>
                    </td>
                    <td>
                        <asp:Label ID="Label1" Style="font-weight: bold" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                        <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                            TargetControlID="txtfDate" />
                    </td>
                    <td>
                        <asp:Label ID="lblReceiveDate" Style="font-weight: bold" runat="server" Text="To Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReceiveDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                        <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                            Enabled="True" TargetControlID="txtReceiveDate" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="center">
                       <asp:Label ID="lblMessage" runat="server" Style="font-weight: bold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Report Type"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtSalesItemsReport" runat="server" Text="Sales Items Report"
                            GroupName="Software" Checked="True" Style="font-weight: bold" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtSalesSummaryReport" runat="server" Text="Sales Summary Report" GroupName="Software"
                            Style="font-weight: bold" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtSalesDetailsReport" runat="server" Text="Sales Details Report" GroupName="Software"
                            Style="font-weight: bold" />
                    </td>
                </tr>    
                  <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtSalesComboReport" runat="server" Text="Sales Combo Report" GroupName="Software"
                            Style="font-weight: bold" />
                    </td>
                </tr>      
                  <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtBarphoneComboReport" runat="server" Text="Barphone Combo Report" GroupName="Software"
                            Style="font-weight: bold"  />
                    </td>
                </tr>      
                
                  <tr>
                    <td>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtConsolidedSalesSummary" runat="server" Text="Daily Consolided Sales Summary" GroupName="Software"
                            Style="font-weight: bold" />
                    </td>
                </tr>      
                          
                <tr>
                    
                    <td>
                        <asp:Button ID="cmdPreview" runat="server" Text="Preview" CssClass="Button_VariableWidth"
                            CausesValidation="False" onclick="cmdPreview_Click" />
                    </td>
                    <td>
                            <asp:Button ID="cmdExport" runat="server" Text="Combo Excel Report" CssClass="Button_VariableWidth"
                                OnClick="cmdExport_Click" />
                        </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
