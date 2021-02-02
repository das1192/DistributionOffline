<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="SalesStockSummaryReport.aspx.cs" Inherits="Pages_SalesStockSummaryReport"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 162px;
        }
        .style2
        {
            width: 56px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <asp:UpdateProgress ID="updProgress" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">--%>
    <contenttemplate>
            <asp:TabContainer ID="ContainerSalesItems" runat="server" ActiveTabIndex="0" Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Sales & Stock Summary Report">
                    <ContentTemplate>
                        <div>
                            <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label22" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList" 
                                        Width="191px">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                        LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Date From"></asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="txttoDate" runat="server" Width="167px" CssClass="TextBox" 
                                        Height="27px"/>&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txttoDate" />
                                </td>
                                <td class="style2">
                                    <asp:Label ID="Label3" runat="server" Text="No. of Previous Days "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPrevDtCount" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="cdd2" runat="server" Category="date1" TargetControlID="ddlPrevDtCount"
                                        LoadingText="Loading Days..." PromptText="--Please Select--" ServiceMethod="BindDays"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchProductCategory" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD3" runat="server" Category="Category" TargetControlID="ddlSearchProductCategory"
                                        LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>  
                                   <td style="padding: 5px;">
                                        <asp:Label ID="Label4" runat="server" Style="font-weight: bold" Text="Report Type:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtReport1" runat="server" Text="Report1" GroupName="Software1"
                                            Style="font-weight: bold" />
                                        &nbsp;
                                        <asp:RadioButton ID="rbtReport2" runat="server" Text="Report2" Checked="True"
                                            GroupName="Software1" Style="font-weight: bold" />
                                    </td>
                                     <td align="right">
                                            <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button_VariableWidth" Width="100px"
                                            CausesValidation="False" OnClick="cmdSearch_Click" />
                                    </td>                                   
                            </tr>
                             <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>                                                      
                            <tr >
                                <td colspan="6" ><asp:Label ID="lblMessage" runat="server" Font-Size="Larger" ForeColor="Red"></asp:Label></td>
                            </tr>
                        </table>
                        </div>

                     
                        
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </contenttemplate>
    <%--</asp:UpdatePanel>--%>
</asp:Content>
