<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StockReturnReportForAdmin.aspx.cs" Inherits="Pages_StockReturnReportForAdmin"
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
            <asp:TabContainer ID="ContainerStockReturn" runat="server" ActiveTabIndex="2" 
                Width="100%">

                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Stock Return Not Received List (On Transit)">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
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
                                    <asp:Label ID="Label1" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtToDate" />
                                </td>
                                <td align="right">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button" CausesValidation="False"
                                        OnClick="cmdSearch_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:GridView ID="gvStockReturnList" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        DataKeyNames="StockReturnID" EmptyDataText="No rows returned" Width="100%" OnRowCommand="StockReturn_Details">
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
                                            <asp:TemplateField HeaderText="StockReturnID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStockReturnID" runat="server" Text='<%#Bind("StockReturnID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Return No">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkView" CommandArgument='<%#Eval("StockReturnID") %>'
                                                        Text='<%# Eval("StockReturnNo") %>' CommandName="ItemDetails"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IDAT" HeaderText="Transfer Date" DataFormatString="{0:yyyy-MM-dd}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FromStoreID" HeaderText="Transfer From">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ToStoreID" HeaderText="Transfer To">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Received">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Bind("ApprovedStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ReferenceBy" HeaderText="Reference By">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Preview">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPreview" Text="Preview" runat="server" CommandArgument='<%# Eval("StockReturnID")%>'
                                                        CommandName="Preview"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CommandArgument='<%# Eval("StockReturnID")%>'
                                                        CommandName="Delete"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="7" style="font-weight: bolder">
                                    Stock Return Details
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="None"
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
                                            <asp:BoundField DataField="StockReturnNo" HeaderText="Stock Return No">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WGPG_NAME" HeaderText="Brand">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubCategoryName" HeaderText="Model">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Description" HeaderText="Color/Description">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Barcode" HeaderText="IMEI/Barcode">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RQty" HeaderText="Qty">
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


                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Stock Return Not Received List (On Transit Details)">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDateDetails" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender5" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDateDetails" />
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDateDetails" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender6" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtToDateDetails" />
                                </td>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="IMEI/Barcode"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIMEIDetails" runat="server" Width="200px" CssClass="TextBox" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label14" runat="server" Text="Category"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlProductCategoryDetails" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown3" runat="server" Category="Category" TargetControlID="ddlProductCategoryDetails"
                                            LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label15" runat="server" Text="Model"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlSubCategoryDetails" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown4" runat="server" Category="Model" TargetControlID="ddlSubCategoryDetails"
                                            ParentControlID="ddlProductCategoryDetails" LoadingText="Loading Model..." PromptText="--Please Select--"
                                            ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label16" runat="server" Text="Description/Color"></asp:Label>
                                </td>
                                <td align="right">
                                    <div>
                                        <asp:DropDownList ID="ddlDescriptionDetails" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown5" runat="server" Category="Description" TargetControlID="ddlDescriptionDetails"
                                            ParentControlID="ddlSubCategoryDetails" LoadingText="Loading Color..." PromptText="--Please Select--"
                                            ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label17" runat="server" Text="Transfer From"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlTransferFromDetails" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown6" runat="server" Category="Branch1"
                                            TargetControlID="ddlTransferFromDetails" LoadingText="Loading Branch..." PromptText="--Please Select--"
                                            ServiceMethod="BindBranch" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label18" runat="server" Text="Transfer To"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlTransferToDetails" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown7" runat="server" Category="Branch1"
                                            TargetControlID="ddlTransferToDetails" LoadingText="Loading Branch..." PromptText="--Please Select--"
                                            ServiceMethod="BindBranch" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:Button ID="cmdSearchDetails" runat="server" Text="Search" 
                                        CssClass="Button" CausesValidation="False" onclick="cmdSearchDetails_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>

                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid"
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        Width="100%" >
                                        <Columns>   
                                         <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField> 

                                             <asp:BoundField DataField="FromStoreID" HeaderText="Transfer From">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="ToStoreID" HeaderText="Transfer To">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="WGPG_NAME" HeaderText="Brand">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                        
                                             <asp:BoundField DataField="SubCategoryName" HeaderText="Model">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            
                                             <asp:BoundField DataField="Description" HeaderText="Color/Description">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                        
                                             <asp:BoundField DataField="Barcode" HeaderText="IMEI/Barcode">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="RQty" HeaderText="Qty">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="ApprovedStatus" HeaderText="Received" 
                                                Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                         
                                              <asp:BoundField DataField="IDAT" HeaderText="Transfer Date" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            
                                                                                       
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </td>
                            </tr>                         
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>


                <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Stock Return Receive List">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceivedFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtReceivedFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceivedToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender4" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtReceivedToDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="IMEI/Barcode"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchBarcode" runat="server" Width="200px" CssClass="TextBox" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label7" runat="server" Text="Category"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlSearchProductCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD2" runat="server" Category="Category" TargetControlID="ddlSearchProductCategory"
                                            LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label8" runat="server" Text="Model"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlSearchSubCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD3" runat="server" Category="Model" TargetControlID="ddlSearchSubCategory"
                                            ParentControlID="ddlSearchProductCategory" LoadingText="Loading Model..." PromptText="--Please Select--"
                                            ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label9" runat="server" Text="Description/Color"></asp:Label>
                                </td>
                                <td align="right">
                                    <div>
                                        <asp:DropDownList ID="ddlSearchDescription" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD4" runat="server" Category="Description" TargetControlID="ddlSearchDescription"
                                            ParentControlID="ddlSearchSubCategory" LoadingText="Loading Color..." PromptText="--Please Select--"
                                            ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" Text="Transfer From"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Branch1"
                                            TargetControlID="DropDownList1" LoadingText="Loading Branch..." PromptText="--Please Select--"
                                            ServiceMethod="BindBranch" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label3" runat="server" Text="Transfer To"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="DropDownList2" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Branch1"
                                            TargetControlID="DropDownList2" LoadingText="Loading Branch..." PromptText="--Please Select--"
                                            ServiceMethod="BindBranch" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:Button ID="Button1" runat="server" Text="Search" CssClass="Button" CausesValidation="False"
                                        OnClick="Button1_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>

                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid"
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        Width="100%" >
                                        <Columns>   
                                         <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField> 

                                             <asp:BoundField DataField="FromStoreID" HeaderText="Transfer From">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="ToStoreID" HeaderText="Transfer To">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="WGPG_NAME" HeaderText="Brand">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                        
                                             <asp:BoundField DataField="SubCategoryName" HeaderText="Model">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            
                                             <asp:BoundField DataField="Description" HeaderText="Color/Description">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                        
                                             <asp:BoundField DataField="Barcode" HeaderText="IMEI/Barcode">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="RQty" HeaderText="Qty">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                             <asp:BoundField DataField="ApprovedStatus" HeaderText="Received" 
                                                Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                             

                                            
                                                                                         
                                              <asp:BoundField DataField="IDAT" HeaderText="Transfer Date" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="EDAT" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            
                                                                                       
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
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
