<%@ Page Title="Branch Stock" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="BranchStock.aspx.cs" Inherits="Pages_BranchStock"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

     
       
    </script>
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
            <asp:TabContainer ID="ContainerBranchStock" runat="server" ActiveTabIndex="3" Width="100%"
                CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Stock Report">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Category"></asp:Label>:
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlProductCategoryForPrice" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="Cascadingdropdown1" runat="server" Category="Category"
                                            TargetControlID="ddlProductCategoryForPrice" LoadingText="Loading Categories..."
                                            PromptText="--Please Select--" ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx"
                                            Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right">
                                    &nbsp;<asp:Label ID="Label3" runat="server" Text="Model"></asp:Label>:
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlModelForPrice" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Model" TargetControlID="ddlModelForPrice"
                                            ParentControlID="ddlProductCategoryForPrice" LoadingText="Loading Model..." PromptText="--Please Select--"
                                            ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right">
                                    &nbsp;<asp:Label ID="Label8" runat="server" Text="Description"></asp:Label>:
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlColorForPrice" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown3" runat="server" Category="Description"
                                            TargetControlID="ddlColorForPrice" ParentControlID="ddlModelForPrice" LoadingText="Loading Color..."
                                            PromptText="--Please Select--" ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx"
                                            Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="cmdSearchForprice" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                        OnClick="cmdSearchForprice_Click" Text="Search" />
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Label ID="lblQty" runat="server" Font-Size="Large"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="gvProductPrice" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" DataKeyNames="ACCOID" BorderColor="#CCCCCC" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned" Width="100%">
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
                                            <asp:TemplateField HeaderText="ProductID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOID1" runat="server" Text='<%#Bind("ACCOID") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Category">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategory1" runat="server" Text='<%#Bind("PROD_WGPG") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Model">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModel1" runat="server" Text='<%#Bind("PROD_SUBCATEGORY") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription1" runat="server" Text='<%#Bind("Description") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantity1" runat="server" Text='<%#Bind("Quantity") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Stock Adjustment (Qty)">
                    <HeaderTemplate>
                        Stock/Price Adjustment
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <tbody>
                                <tr>
                                    <td align="left">
                                        &nbsp;<asp:Label ID="Label6" runat="server" Text="Category"></asp:Label>
                                        :
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlProductCategoryForPrice2" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="Cascadingdropdown4" runat="server" Category="Category"
                                                Enabled="True" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                                ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx" TargetControlID="ddlProductCategoryForPrice2">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                    <td align="left">
                                        &nbsp;<asp:Label ID="Label7" runat="server" Text="Model"></asp:Label>
                                        :
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlModelForPrice2" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="Cascadingdropdown5" runat="server" Category="Model" Enabled="True"
                                                LoadingText="Loading Model..." ParentControlID="ddlProductCategoryForPrice2"
                                                PromptText="--Please Select--" ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx"
                                                TargetControlID="ddlModelForPrice2">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                    <td align="left">
                                        &nbsp;<asp:Label ID="Label9" runat="server" Text="Description/Color"></asp:Label>
                                        :
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlColorForPrice2" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="Cascadingdropdown6" runat="server" Category="Description"
                                                Enabled="True" LoadingText="Loading Color..." ParentControlID="ddlModelForPrice2"
                                                PromptText="--Please Select--" ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx"
                                                TargetControlID="ddlColorForPrice2">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Stock Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlstocktype" runat="server" AutoPostBack="True" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Text="Quantity" Value="Quantity"></asp:ListItem>
                                            <asp:ListItem Text="Barcode" Value="Barcode" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right" colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                            OnClick="cmdSearchForprice2_Click" Text="Search" />
                                    </td>
                                    <td align="right" colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:GridView ID="gvStockAdjust" runat="server" AutoGenerateColumns="False" BorderColor="#CCCCCC"
                                            BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="mGrid" DataKeyNames="ACCOID,CostPrice,Quantity,CategoryID,SubCategoryID,DescriptionID,Branch,AVERAGE,Barcode,TPRODID"
                                            EmptyDataText="No rows returned" GridLines="None" OnRowCommand="StockAdjust_Details"
                                            Width="100%">
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
                                                <asp:TemplateField HeaderText="ProductIDSMS" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOIDSMS" runat="server" Text='<%#Bind("SMSOID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ProductIDACC" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOIDACC" runat="server" Text='<%#Bind("ACCOID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Category">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("PROD_WGPG") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Model">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModel" runat="server" Text='<%#Bind("PROD_SUBCATEGORY") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cost Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCostPrice" runat="server" Text='<%#Bind("CostPrice") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock Adjust">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkAdjust" runat="server" CausesValidation="false" CommandName="Adjust"
                                                            Text="Adjust">Stock Adjust</asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price Adjust">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkPriceAdjust" runat="server" CausesValidation="false" CommandName="cmdPriceAdjust"
                                                            Text="Price Adjust"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnHidden" runat="server" Style="display: none" />
                                    <asp:ModalPopupExtender ID="ModalPopupExtender2" TargetControlID="btnHidden" PopupControlID="popUpPanel"
                                        CancelControlID="btnclose" BackgroundCssClass="modalBackground" DropShadow="True"
                                        runat="server" DynamicServicePath="" Enabled="True">
                                    </asp:ModalPopupExtender>
                                    <asp:Panel ID="popUpPanel" runat="server">
                                        <div id="loginform" style="background-color: #92e9ff; border: 30px solid #004f7b;">
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label25" runat="server" Text="Product ID:" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblOIDSMSGR" runat="server" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label29" runat="server" Text="Category:" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCategorynew" runat="server" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label1" runat="server" Text="Model:" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblModelnew" runat="server" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label5" runat="server" Text="Description:" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDescriptionnew" runat="server" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label4" runat="server" Text="Old Quantity:" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblOLDQUANTITY" runat="server" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblReason" runat="server" Text="Missing Quantity"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span style="color: red;">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="txtStyle" BackColor="Yellow" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-success"
                                                            OnClick="btnSubmitDiscount_Click" CausesValidation="False" Width="100px" />&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnclose" runat="server" Text="Close" CausesValidation="False" CssClass="btn btn-warning"
                                                            Width="100px" />
                                                        <br />
                                                        <asp:Label ID="lblCategoryID" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblTPRODID" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblOIDACCGR" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblCostPrice" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblSubCategoryID" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblDescriptionID" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblBranch" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblAVERAGE" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblBarcode" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnHidden2" runat="server" Style="display: none" />
                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="btnHidden2" PopupControlID="popUpPanel2"
                                        CancelControlID="btnCancelNewPrice" BackgroundCssClass="modalBackground" DropShadow="True"
                                        runat="server" DynamicServicePath="" Enabled="True">
                                    </asp:ModalPopupExtender>
                                    <asp:Panel ID="popUpPanel2" runat="server">
                                        <div id="loginform" style="background-color: #92e9ff; border: 30px solid #004f7b;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNewPrice" runat="server" Text="New Price"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span style="color: red;">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNewPrice" runat="server" onkeypress="return isNumberKey(event)"
                                                            CssClass="txtStyle text-center" BackColor="Yellow" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNewQty" runat="server" Text="Quantity"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span style="color: red;">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNewQty" runat="server" onkeypress="return isNumberKey(event)"
                                                            CssClass="txtStyle text-center" Enabled="False" />
                                                        <asp:Label ID="lblIsBarcode" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblBarcodeSign" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label10" runat="server" Text="Vendor"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span style="color: red;">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlVendorForPriceAdjust" runat="server" Enabled="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Button ID="btnSaveNewPrice" runat="server" Text="Save" CssClass="btn btn-success"
                                                            OnClick="btnSaveNewPrice_Click" CausesValidation="False" Width="100px" />&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnCancelNewPrice" runat="server" Text="Close" CausesValidation="False"
                                                            CssClass="btn btn-warning" Width="100px" />
                                                        <asp:Label ID="lblRowIndexNewPrice" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tPnlDisplay3" runat="server" HeaderText="Stock Adjustment Report">
                    <ContentTemplate>
                        <table width="100%">
                            <thead>
                                <tr>
                                    <td colspan="6" align="center" style="font-weight: bolder">
                                        Stock Adjustment Report
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
                                        <asp:TextBox ID="txtDateFrom2" runat="server" Width="150px" CssClass="TextBox" /><asp:CalendarExtender
                                            ID="CalendarExtender5" Format="yyyy-MM-dd" runat="server" Enabled="True" TargetControlID="txtDateFrom2" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label20" Style="font-weight: normal" runat="server" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateTo2" runat="server" Width="150px" CssClass="TextBox" /><asp:CalendarExtender
                                            ID="CalendarExtender6" Format="yyyy-MM-dd" runat="server" Enabled="True" TargetControlID="txtDateTo2" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <br />
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
                                    <td align="right">
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
                                    </td>
                                    <td>
                                        <asp:Button ID="Button4" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                            OnClick="cmdSearch3_Click" Text="Search" />
                                    </td>
                                    <td align="right" colspan="4">
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <table width="100%">
                                            <tr>
                                                <td align="center" style="font-weight: bolder">
                                                    Stock Adjustment Report
                                                    <hr style="color: Green" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%;" valign="bottom">
                                                    <asp:Label ID="Label30" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gv_stockadjustment" runat="server" AutoGenerateColumns="False"
                                                        GridLines="None" CssClass="mGrid" PageSize="5" AllowPaging="True" DataKeyNames="OID"
                                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                                        OnPageIndexChanging="gv_stockadjustment_PageIndexChanging" Width="100%">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <PagerStyle CssClass="pgr" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    SI</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ProductID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOID" runat="server" Text='<%#Bind("OID") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPROD_WGPG" runat="server" Text='<%#Bind("PROD_WGPG") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Brand">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPROD_SUBCATEGORY" runat="server" Text='<%#Bind("PROD_SUBCATEGORY") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="OLD Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("OldQuantity") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Missing Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblnewQuantity" runat="server" Text='<%#Bind("NewQuantity") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIDAT" runat="server" Text='<%#Bind("IDAT") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Products with Barcode">
                    <HeaderTemplate>
                        Products with Barcode
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <thead>
                                <tr>
                                    <td colspan="6" align="center" style="font-weight: bolder">
                                        Products purchased with barcode
                                        <hr style="color: Green" />
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblFromDateBP" Style="font-weight: normal" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="dptFromDateBP" runat="server" CssClass="form-control" />
                                        <asp:CalendarExtender ID="dptFromDateBP_CE" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                            TargetControlID="dptFromDateBP" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblToDateBP" Style="font-weight: normal" runat="server" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="dptToDateBP" runat="server" CssClass="form-control" />
                                        <asp:CalendarExtender ID="dptToDateBP_CE" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                            TargetControlID="dptToDateBP" />
                                    </td>
                                    <td>
                                        Status&nbsp;
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatusBP" runat="server" CssClass="form-control">
                                            <asp:ListItem Selected="True" Value="0">Available</asp:ListItem>
                                            <asp:ListItem Value="1">Sold</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblCategoryBP" runat="server" Text="Category" CssClass="control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlCategoryBP" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CascadingDropDown7" runat="server" Category="Category"
                                                TargetControlID="ddlCategoryBP" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                                ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBrandBP" runat="server" Text="Brand" CssClass="control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlBrandBP" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CascadingDropDown8" runat="server" Category="Model" TargetControlID="ddlBrandBP"
                                                ParentControlID="ddlCategoryBP" LoadingText="Loading Model..." PromptText="--Please Select--"
                                                ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDescriptionBP" runat="server" Text="Description" CssClass="control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlDescriptionBP" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CascadingDropDown9" runat="server" Category="Description"
                                                TargetControlID="ddlDescriptionBP" ParentControlID="ddlBrandBP" LoadingText="Loading Color..."
                                                PromptText="--Please Select--" ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx"
                                                Enabled="True">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblBarcodeBP" runat="server" Text="Barcode"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBarcodeBP" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVendorBP" runat="server" Text="Vendor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlVendorBP" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown_ddlVendorBP" runat="server" Category="Vendor"
                                            TargetControlID="ddlVendorBP" LoadingText="Loading Vendor..." PromptText="--Please Select--"
                                            ServiceMethod="BindVendor" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </td>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                        <asp:Button ID="btnSearchBP" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                            OnClick="btnSearchBP_Click" Text="Search" />
                                    </td>
                                    <td align="right" colspan="4">
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <table width="100%">
                                            <tr>
                                                <td align="center" style="font-weight: bolder">
                                                    Product List
                                                    <hr style="color: Green" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%;" valign="bottom">
                                                    <asp:Label ID="Label15" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gvProductListBP" runat="server" AutoGenerateColumns="False" BorderColor="#CCCCCC"
                                                        BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="mGrid" EmptyDataText="No rows returned"
                                                        GridLines="None" PageSize="25" Width="100%" OnPageIndexChanging="gvProductListBP_PageIndexChanging"
                                                        AllowPaging="True" DataKeyNames="StoreMasterStockOID,T_PRODOID,T_STOCKOID,PROD_DES,Branch,Barcode,SaleStatus,PROD_WGPG,PROD_SUBCATEGORY,CostPrice,Vendor_ID">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    SL
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSRNO0" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Category">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOID2" runat="server" Text='<%#Bind("WGPG_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Brand">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPROD_WGPG0" runat="server" Text='<%#Bind("SubCategoryName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription2" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Barcode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPROD_SUBCATEGORY0" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cost (tk)">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCostPrice" runat="server" Text='<%#Bind("CostPrice") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vendor">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVendor_Name" runat="server" Text='<%#Bind("Vendor_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity2" runat="server" Text='<%#Bind("ProductStatus") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SalesDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIDAT" runat="server" Text='<%#Bind("SalesDate") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <%-- <asp:Button ID="btnPurchaseReturn" runat="server" Text="Return" Width="100%"
                                                                     OnClick="btnPurchaseReturn_Click" OnClientClick="this.disabled=true;"
                                                                        ForeColor="Red" />--%>
                                                                    <asp:Button ID="btnPurchaseReturn" runat="server" Text="Return" CausesValidation="True"
                                                                        CssClass="btn btn-sm btn-warning" UseSubmitBehavior="false"
                                                                        OnClick="btnPurchaseReturn_Click" Enabled="true" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkPurchaseReturn" runat="server" Enabled="true" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pgr" />
                                                    </asp:GridView>
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
