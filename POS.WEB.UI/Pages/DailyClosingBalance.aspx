﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DailyClosingBalance.aspx.cs" Inherits="Pages_DailyClosingBalance" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerStockReportForAdmin" runat="server" ActiveTabIndex="0"
                Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Daily Closing Balance">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                 <td align="left">
                                    <asp:Label ID="Label22" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                        LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>

                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtDate" />
                                </td>  
                                <td colspan="2">&nbsp;</td>                           
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchProductCategory" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD2" runat="server" Category="Category" TargetControlID="ddlSearchProductCategory"
                                        LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label6" runat="server" Text="Model"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchSubCategory" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD3" runat="server" Category="Model" TargetControlID="ddlSearchSubCategory"
                                        ParentControlID="ddlSearchProductCategory" LoadingText="Loading Model..." PromptText="--Please Select--"
                                        ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label7" runat="server" Text="Color/Description"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchDescription" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD4" runat="server" Category="Description" TargetControlID="ddlSearchDescription"
                                        ParentControlID="ddlSearchSubCategory" LoadingText="Loading Color..." PromptText="--Please Select--"
                                        ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="6">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button_VariableWidth"
                                        CausesValidation="False" OnClick="cmdSearch_Click" />                                    
                                </td>
                            </tr>

                             <tr>
                               <td colspan="6" align="right">
                                    <asp:Label ID="lblTotal" runat="server" ForeColor="Green" Font-Bold=True></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        EmptyDataText="No rows returned" Width="100%">
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
                                            <asp:BoundField DataField="CCOM_NAME" HeaderText="Branch" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WGPG_NAME" HeaderText="Category">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubCategoryName" HeaderText="Model">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Description" HeaderText="Color/Description">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>                                              
                                            <asp:BoundField DataField="Quantity" HeaderText="Qty">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                              <asp:BoundField DataField="Closing" HeaderText="ClosingBalance">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>                                         
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

