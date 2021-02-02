<%@ Page Title="Sales Report" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalesReport.aspx.cs" EnableEventValidation="false" Inherits="Reports_SalesReport" %>

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
                        Sales Report
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" Style="font-weight: bold" runat="server" Text="From Date :"></asp:Label>&nbsp;<br />
                        <asp:TextBox ID="txtfDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                        <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                            TargetControlID="txtfDate" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtfDate"
                            ErrorMessage="Date is required" Display="Dynamic" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblReceiveDate" Style="font-weight: bold" runat="server" Text="To Date :"></asp:Label>&nbsp;<br />
                        <asp:TextBox ID="txtReceiveDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                        <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                            Enabled="True" TargetControlID="txtReceiveDate" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtReceiveDate"
                            ErrorMessage="Date is required" Display="Dynamic" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label2" Style="font-weight: bold" runat="server" Text="Category :"></asp:Label>&nbsp;<br />
                      
                                        <asp:DropDownList ID="ddlSearchProductCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD2" runat="server" Category="Category" TargetControlID="ddlSearchProductCategory"
                                            LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                  
                    </td>
                    <td>
                        <asp:Label ID="Label3" Style="font-weight: bold" runat="server" Text="Brand :"></asp:Label>&nbsp;<br />
                      
                                        <asp:DropDownList ID="ddlSearchSubCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD3" runat="server" Category="Model" TargetControlID="ddlSearchSubCategory"
                                            ParentControlID="ddlSearchProductCategory" LoadingText="Loading Model..." PromptText="--Please Select--"
                                            ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                  
                    </td>
                    <td>
                        <asp:Label ID="Label4" Style="font-weight: bold" runat="server" Text="Model :"></asp:Label>&nbsp;<br />
                      
                                        <asp:DropDownList ID="ddlSearchDescription" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD4" runat="server" Category="Description" TargetControlID="ddlSearchDescription"
                                            ParentControlID="ddlSearchSubCategory" LoadingText="Loading Color..." PromptText="--Please Select--"
                                            ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                  
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Report Type
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtPDF" runat="server" Text="PDF" GroupName="Software" Checked="True" />
                        <asp:RadioButton ID="rbtExcel" runat="server" Text="Excel" GroupName="Software" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAdd" runat="server" Text="Sales Items Report" CssClass="btn btn-success"
                            OnClick="btnAdd_Click" CausesValidation="true" />
                    </td>
                    <td>
                        <asp:Button ID="cmdSummary" runat="server" CssClass="btn btn-warning" Text="Sales Summary"
                            OnClick="cmdSummary_Click" CausesValidation="true" />
                    </td>
                    <td>
                        <asp:Button ID="cmdOK" runat="server" CssClass="btn btn-info" Text="Sales Details"
                            Visible="true" CausesValidation="true" OnClick="cmdOK_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
