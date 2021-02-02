<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StockReportForAdmin.aspx.cs" Inherits="Pages_StockReportForAdmin" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerStockReportForAdmin" runat="server" ActiveTabIndex="0"
                Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Current Stock Report">
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
                                    <asp:Label ID="Label10" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="lblReceiveDate" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                                        Enabled="True" TargetControlID="txtToDate" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Brand"></asp:Label>
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
                                <td>
                                    Search Type
                                </td>
                                <td>
                                    
                                        <asp:RadioButton ID="rbtDetails" runat="server" Text="Details" GroupName="Software"
                                            Checked="True" />
                                        <asp:RadioButton ID="rbtSummary" runat="server" Text="Summary" GroupName="Software" />                                                                           
                                </td>
                             
                                <td align="right" colspan="4">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button_VariableWidth"
                                        CausesValidation="False" OnClick="cmdSearch_Click" />&nbsp;&nbsp;
                                    <asp:Button ID="cmdExport" runat="server" Text="Export To Excel" CssClass="Button_VariableWidth"
                                        OnClick="cmdExport_Click" />
                                </td>
                            </tr>
                            <tr>
                               <td colspan="6" align="right">
                                    <asp:Label ID="lblTotal" runat="server" Text="" ForeColor="Green" Font-Bold=true></asp:Label>
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
                                            <asp:BoundField DataField="Branch" HeaderText="Branch">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PROD_WGPG" HeaderText="Brand">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PROD_SUBCATEGORY" HeaderText="Model">
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
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                             <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView2" runat="server" Width="100%">
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>



                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Total Stock Report">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label2" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBranchTotal" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Branch1" TargetControlID="ddlBranchTotal"
                                        LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDateTotal" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDateTotal" />
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDateTotal" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server"
                                        Enabled="True" TargetControlID="txtToDateTotal" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label5" runat="server" Text="Brand"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategoryTotal" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Category" TargetControlID="ddlCategoryTotal"
                                        LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label8" runat="server" Text="Model"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubCategoryTotal" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown3" runat="server" Category="Model" TargetControlID="ddlSubCategoryTotal"
                                        ParentControlID="ddlCategoryTotal" LoadingText="Loading Model..." PromptText="--Please Select--"
                                        ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label9" runat="server" Text="Color/Description"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDescriptionTotal" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown4" runat="server" Category="Description" TargetControlID="ddlDescriptionTotal"
                                        ParentControlID="ddlSubCategoryTotal" LoadingText="Loading Color..." PromptText="--Please Select--"
                                        ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>

                             <tr>    
                             <td>&nbsp;</td>                           
                                <td align="left" colspan="5">
                                    <asp:Button ID="cmdSearchTotal" runat="server" Text="Search" CssClass="Button_VariableWidth"
                                        CausesValidation="False" onclick="cmdSearchTotal_Click" />                                   
                                </td>
                            </tr>
                           
                            <tr>
                               <td colspan="6" align="right">
                                     <asp:Label ID="lblTotalStock1" runat="server" Text="" ForeColor="Green" Font-Bold=true></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" GridLines="None"
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
                                            <asp:BoundField DataField="Branch" HeaderText="Branch">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PROD_WGPG" HeaderText="Brand">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PROD_SUBCATEGORY" HeaderText="Model">
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
