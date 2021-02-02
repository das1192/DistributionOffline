<%@ Page Title="Branch Stock Summery Product wise" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="BranchStockSummeryProductWise.aspx.cs" Inherits="Pages_BranchStockSummeryProductWise"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--    <link href="../StylesBootStrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../StylesBootStrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script language="javascript" type="text/javascript">


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

     
       
    </script>
    <style type="text/css">
        .style1
        {
            font-size: large;
        }
        .centerHeaderText th {
        text-align: center;background-color:Gray;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...</ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContainerBranchStock" runat="server" ActiveTabIndex="0" Width="100%"
                CssClass="fancy fancy-green">
                <asp:TabPanel ID="tPnlDisplay3" runat="server" HeaderText="Product wise Profit/Loss">
                    <ContentTemplate>
                    <div class ="table-responsive">
                        <table width="100%">
                            <thead>
                                <tr>
                                    <td colspan="6" align="center" style="font-weight: bolder">
                                        Product wise Profit
                                        <hr style="color: Green" />
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label19" Style="font-weight: normal" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateFrom2" runat="server" Width="150px" CssClass="form-control" /><asp:CalendarExtender
                                            ID="CalendarExtender5" Format="dd MMM yyyy" runat="server" Enabled="True" TargetControlID="txtDateFrom2" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label20" Style="font-weight: normal;" runat="server" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateTo2" runat="server" Width="150px" 
                                            CssClass="form-control" /><asp:CalendarExtender
                                            ID="CalendarExtender6" Format="dd MMM yyyy" runat="server" Enabled="True" TargetControlID="txtDateTo2" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label21" runat="server" Text="Category"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlSearchProductCategory3" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CascadingDropDown10" runat="server" Category="Category"
                                                TargetControlID="ddlSearchProductCategory3" LoadingText="Loading Categories..."
                                                PromptText="--Please Select--" ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx"
                                                Enabled="True">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label22" runat="server" Text="Brand"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlSearchSubCategory3" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CascadingDropDown11" runat="server" Category="Model" TargetControlID="ddlSearchSubCategory3"
                                                ParentControlID="ddlSearchProductCategory3" LoadingText="Loading Model..." PromptText="--Please Select--"
                                                ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label27" runat="server" Text="Description"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlSearchDescription3" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CascadingDropDown12" runat="server" Category="Description"
                                                TargetControlID="ddlSearchDescription3" ParentControlID="ddlSearchSubCategory3"
                                                LoadingText="Loading Color..." PromptText="--Please Select--" ServiceMethod="BindDescription"
                                                ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSearchStockSummery" runat="server" Text="Search" CssClass="inline btn btn-info"
                                            CausesValidation="False" OnClick="btnSearchStockSummery_Click" />
                                    </td>
                                    <td>
                                    </td>
                                    <td align="right" colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                    
                                        <table width="100%">
                                        
                                            <tr>
                                                <td align="left" style="" valign="bottom">
                                                    <asp:Label ID="Label30" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <div style="text-align:center ;">
                                                    <asp:GridView ID="gvStockSummery" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                        
                                                        CssClass="table table-bordered table-condensed table-hover table-responsive" 
                                                        PageSize="50" AllowPaging="false" BorderColor="#CCCCCC" BorderStyle="None"
                                                        BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned" Width="100%"
                                                        OnPageIndexChanging="gvStockSummery_PageIndexChanging" ShowFooter="True"
                                                        DataKeyNames="OpeningStockQty,ClosingStockQty,OpeningStockValue,ClosingStockValue,Profit">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    SL No</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

<asp:BoundField DataField="CategoryName" HeaderText="Category" Visible="False">
    <ItemStyle HorizontalAlign="Left" />
</asp:BoundField>
<asp:BoundField DataField="SubcategoryName" HeaderText="Subcategory" Visible="False">
    <ItemStyle HorizontalAlign="Left" />
</asp:BoundField>
<asp:BoundField DataField="Description" HeaderText="Description">
    <ItemStyle HorizontalAlign="Left" />
</asp:BoundField>


<asp:BoundField DataField="OpeningStockQty" HeaderText="Opening Stock Qty" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>

<asp:BoundField DataField="todayP_Qty" HeaderText="Purchase Qty" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todayPR_Qty" HeaderText="Purchase Return Qty" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todayS_Qty" HeaderText="Sale Qty">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todaySR_Qty" HeaderText="Sale Return Qty">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todaySR_Qty" HeaderText="Sale Return Qty" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todayPRMIS_Qty" HeaderText="Missing Qty" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="ClosingStockQty" HeaderText="Closing Stock Qty" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
    
<asp:BoundField DataField="OpeningStockValue" HeaderText="Opening Stock Value" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>

<asp:BoundField DataField="todayP_Cost" HeaderText="Purchase Value" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todayPR_Cost" HeaderText="Purchase Return Value" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todayS_Cost" HeaderText="Sale Value" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todaySR_Cost" HeaderText="Sale Return Value" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todayPRMIS_Value" HeaderText="Missing Value" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="todayPRPROT_Value" HeaderText="Price Protection Value" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>


<asp:BoundField DataField="ClosingStockValue" HeaderText="Closing Stock Value" Visible="False">
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>

<asp:BoundField DataField="Profit" HeaderText="Profit/Loss">
    <ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#296487" Font-Bold="True" Font-Italic="True" 
                                                            ForeColor="White" />
                                                        <HeaderStyle BackColor="#296487" ForeColor="White" />
                                                        <PagerStyle CssClass="pgr" />
                                                    </asp:GridView>
                                                </div>
                                                <div style="text-align:right;font-size:large;">
                                                    <asp:Label ID="lblTotalGiftAmount" runat="server"></asp:Label>
                                                </div>
                                                <div style="text-align:right;font-size:large;">
                                                    <asp:Label ID="lblTotalDiscountAmount" runat="server"></asp:Label>
                                                </div>
                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
