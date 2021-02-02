<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OnCreditSales.aspx.cs" Inherits="TalukderPOS.Web.UI.T_SALES_DTLForm"
    Title="SALES" EnableEventValidation="false" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">

        function VattoReceiveamount(e) {
            var key;
            key = (e.which) ? e.which : e.keyCode;
            if (key == 13) {
                document.getElementById('<%=txtReceiveAmount.ClientID %>').focus();
                return false;
            }
        }
        function RceamounttoSave(e) {
            var key;
            key = (e.which) ? e.which : e.keyCode;
            if (key == 13) {
                document.getElementById('<%=btnSaveSale.ClientID %>').focus();
                return false;
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

     
       
    </script>
    <style type="text/css">
        .txtStyle
        {
            background: white;
            border: 1px solid #ffa853;
            border-radius: 5px;
            box-shadow: 0 0 5px 3px #ffa853;
            color: #666;
            padding: 5px 10px;
            width: 200px;
            height: 20px;
            outline: none;
            font-family: Times New Roman,Arial,Sans-Serif;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
        }
        .poscharge
        {
            font-family: Times New Roman,Arial,Sans-Serif;
            font-size: 12px;
            color: Blue;
        }
        /*AutoComplete flyout */
        
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: inherit;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            height: 200px;
            text-align: left;
            list-style-type: none;
        }
        
        /* AutoComplete highlighted item */
        
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }
        
        /* AutoComplete item */
        
        .autocomplete_listItem
        {
            background-color: window;
            color: windowtext;
            padding: 1px;
        }
        
        .display-off
        {
            display:none;
            }
            
            
          .sale-body
          {
              margin-top :15px;
              }  
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="row">
        <div class="col-lg-12 sale-body">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="col-lg-7">
                        <div class="row">
                            <div class="panel2 panel-primary">
                                <table width="100%" class="table-responsive" style="margin-bottom: 25px;">
                                    <tr>
                                        <td align="center" style="font-weight: bold" colspan="3">
                                            Retailer Information
                                            <hr style="color: Green;" />
                                        </td>

                                        <td>
                                        <asp:TextBox ID="dtpSalesDate" runat="server" CssClass="form-control"  />
                                                    <asp:CalendarExtender ID="CE_dtpSalesDate" runat="server" Enabled="True" Format="yyyy-MM-dd HH:mm"
                                                        TargetControlID="dtpSalesDate" /> 
                                        
                                        </td>

                                    </tr>
                                    <tr>
                                        <td align="left">
                                            &nbsp;<asp:Label ID="lblCustomerName" runat="server" Text="Retailer Name" CssClass=""></asp:Label>
                                        </td>
                                        <td align="left" runat="server" >
                                            <%--<asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" />--%>
                                          <%--  onselectedindexchanged="ddlVendorList_SelectedIndexChanged"--%>
                                                 <div>
                                                <asp:DropDownList ID="ddlCustomerList" runat="server" CssClass="form-control" 
                                                    AutoPostBack="True" 
                                                         onselectedindexchanged="ddlCustomerList_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                              <%--  <asp:CascadingDropDown ID="CCD20" runat="server" Category="Customer" TargetControlID="ddlCustomerList"
                                                    LoadingText="Loading Customer..." PromptText="--Please Select--" ServiceMethod="BindCustomer"
                                                    ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                                </asp:CascadingDropDown>--%>
                                            </div>



                                        </td>
                                        <td align="left">
                                            <%--<asp:RequiredFieldValidator ID="rfvWGPG_NAME" ControlToValidate="txtCustomerName"
                                                ErrorMessage="Customer Name is required" Display="Dynamic" runat="server" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            &nbsp;<asp:Label ID="lblMobileNumber" runat="server" Text="Mobile Number" CssClass=""></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-control" MaxLength="11"
                                                onkeypress="return isNumberKey(event)" />
                                        </td>
                                        <td align="left">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtMobileNumber"
                                                ErrorMessage="Retailer Mobile No is required" Display="Dynamic" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            &nbsp;<asp:Label ID="lblAddress" runat="server" Text="Address" CssClass=""></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                                        </td>
                                        <td align="left">
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidatorForAddress" ControlToValidate="txtAddress"
                                                ErrorMessage="Retailer Address is required" Display="Dynamic" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label3" runat="server" Text="Email Address" CssClass=""></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label1" runat="server" Text="Payment Mode" CssClass=""></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:DropDownList ID="ddlPaymentMode"   runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trBank" runat="server">
                                        <td align="left">
                                            <asp:Label ID="Label5" runat="server" Text="Select Bank" CssClass=""></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control" 
                                                AutoPostBack="true" onselectedindexchanged="ddlBank_SelectedIndexChanged" >
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <%--sadiq 103 barcode--%>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblBarcode" runat="server" Text="EMI/Barcode" Visible="false"></asp:Label>
                                            <asp:TextBox Visible="true" ID="txtBarcode" runat="server" CssClass="form-control" placeholder="--Please Select--"
                                                OnTextChanged="txtBarcode_TextChanged" AutoPostBack="True"></asp:TextBox>
                                            <%--<asp:AutoCompleteExtender ID="ace_txtBarcode" runat="server" CompletionInterval="50"
                                                CompletionSetCount="40" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListElementID="listPlacement" EnableCaching="False" MinimumPrefixLength="0"
                                                ServiceMethod="GetBarcode" ServicePath="~/DropdownWebService.asmx" TargetControlID="txtBarcode">
                                            </asp:AutoCompleteExtender>--%>
                                        </td>
                                    </tr>
                                </table>
                                <div class="table-responsive" runat="server" id="dvCategory" visible="false">
                                    <div style="float: left; width: 30%">
                                        Category
                                        <asp:DropDownList ID="ddlSearchProductCategory" runat="server" 
                                            CssClass="form-control" AutoPostBack="True" 
                                            onselectedindexchanged="ddlSearchProductCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                       <%-- <asp:CascadingDropDown ID="CDD2" runat="server" Category="Category" TargetControlID="ddlSearchProductCategory"
                                            LoadingText="Loading Categories..." PromptText="--Please Select Category--" ServiceMethod="BindCategory"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>--%>
                                    </div>
                                    <div style="float: left; padding-left: 10px; width: 30%;">
                                        Brand
                                        <%--sadiq autopostback true to set contextkey value--%>
                                        <asp:DropDownList ID="ddlSearchSubCategory" runat="server" CssClass="form-control"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlSearchSubCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                       <%-- <asp:CascadingDropDown ID="CDD3" runat="server" Category="Model" TargetControlID="ddlSearchSubCategory"
                                            ParentControlID="ddlSearchProductCategory" LoadingText="Loading Model..." PromptText="--Please Select Model--"
                                            ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>--%>
                                    </div>
                                    <div style="float: right; width: 30%;">
                                        Color/Description
                                        <asp:TextBox ID="txtDescription" runat="server" AutoPostBack="True" CssClass="form-control"
                                            placeholder="--Please Select--" OnTextChanged="txtDescription_TextChanged"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACE_Description" runat="server" CompletionInterval="50"
                                            CompletionSetCount="40" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListElementID="listPlacement" EnableCaching="False" MinimumPrefixLength="0"
                                            ServiceMethod="GetDescription" ServicePath="~/DropdownWebService.asmx" TargetControlID="txtDescription">
                                        </asp:AutoCompleteExtender>

                                    </div>
                                    <asp:HiddenField ID="lblOID" runat="server" />
                                    <asp:Label ID="lblRowIndex" runat="server"></asp:Label>
                                </div>
                                <div style="clear: both;">
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvT_SALES_DTL" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        CssClass="table table-striped table-bordered" BorderColor="#CCCCCC" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned" Width="98%"
                                        
                                        
                                        
                                        DataKeyNames="OID,PROD_WGPG,PROD_SUBCATEGORY,DescriptionID,StoreID,Barcode,SalePrice,Stock,Qty,TotalPrice,RefAmount,WGPG_NAME,SubCategoryName,Description,Flag" 
                                        >
                                        <Columns>
                                            <asp:TemplateField HeaderText="Category">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%#Bind("WGPG_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Model">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubCategoryName" runat="server" Text='<%#Bind("SubCategoryName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description/Color">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                    <asp:CheckBox ID="chkGift" runat="server" Text="Gift" AutoPostBack="True" 
                                                        oncheckedchanged="chkGift_CheckedChanged" Visible="false" BackColor="Yellow" 
                                                        Font-Italic="True" ForeColor="#FF0066" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MRP">
                                                <ItemTemplate>
                                                    <%--sadiq  101 mrp open   label change to txt--%>
                                                    <asp:TextBox ID="txtSalePrice" runat="server" Width="40px" Text='<%#Bind("SalePrice") %>'
                                                        AutoPostBack="true" onkeypress="return isNumberKey(event)" 
                                                        OnTextChanged="txtSalePrice_TextChanged"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStock" runat="server" Text='<%#Bind("Stock") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQty" runat="server" Width="60px" Text='<%#Bind("Qty") %>' AutoPostBack="true"
                                                        onkeypress="return isNumberKey(event)" OnTextChanged="txtSalePrice_TextChanged"> </asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Barcode">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblbarcode" runat="server" Width="120px" Text='<%#Bind("Barcode") %>'
                                                        AutoPostBack="true" Enabled="False"></asp:TextBox></ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRefAmount" runat="server" Width="60px" Text='<%#Bind("RefAmount") %>'
                                                        AutoPostBack="true" onkeypress="return isNumberKey(event)" OnTextChanged="RefAmount_TextChanged"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Net Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPrice" runat="server" Text='<%#Bind("TotalPrice") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                   <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandName="Delete"
                                                        Text="Delete"> <img alt="Delete" src="../Images/Delete.gif" /></asp:LinkButton>--%>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false"
                                                        Text="Delete" onclick="lnkDelete_Click"> <img alt="Delete" src="../Images/Delete.gif" /></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Flag" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlag" runat="server" Text='<%#Bind("Flag") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle CssClass="alphabetic" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                    
                                </div>
                                <div class="table-responsive">
                                </div>
                                
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4" style="padding-left: 10px; float: right;">
                        <div class="row">
                            <div class="panel3 panel-primary">
                                <table width="100%" class="table-responsive">
                                    <tr>
                                        <td style="font-size: large; font-weight: normal; color: #000080">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" style="width: 40%">
                                                        Total Quantity:
                                                    </td>
                                                    <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="txttotalquantity" runat="server" Width="100px" Style="text-align: right;"
                                                            CssClass="form-control"  Font-Bold="True" Enabled="false" ForeColor="Black" BackColor="#D9DCE5"
                                                            class="input_text" Text="0" BorderColor="#D9DCE5" BorderStyle="None" Font-Size="Small"></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Pcs</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 40%">
                                                        Sub Total :
                                                    </td>
                                                    <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="txtSubTotal" runat="server" Width="100px" Style="text-align: right;"
                                                            CssClass="form-control"  Font-Bold="True" Enabled="false" ForeColor="Black" BackColor="#D9DCE5"
                                                            class="input_text" Text="0" BorderColor="#D9DCE5" BorderStyle="None" Font-Size="Small"></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 40%">
                                                        (-)Discount:
                                                    </td>
                                                    <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="txtdiscount" runat="server" CssClass="form-control" Width="100px"
                                                            Font-Bold="True" Style="text-align: right;" AutoPostBack="true" Text="0" Font-Size="Small"
                                                            OnTextChanged="txtdiscount_TextChanged" Enabled="false"></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>

                                                <tr class="display-off">
                                                    <td align="right" style="width: 40%">
                                                        (-)Gift:
                                                    </td>
                                                    <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="txtGift" runat="server" CssClass="form-control" Width="100px"
                                                            Font-Bold="True" Style="text-align: right;" Text="0" Font-Size="Small"
                                                             Enabled="false"></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>
                                                 <tr>
                                                    <td align="right" style="width: 40%">
                                                      Previous Due :
                                                    </td>
                                                    <td align="center"  style="width: 30%">
                                                        <asp:TextBox ID="PreviousDue" runat="server" Width="100px" Style="text-align: right;"
                                                            CssClass="form-control" Font-Bold="True" Enabled="false" ForeColor="Black" BackColor="#D9DCE5"
                                                            class="input_text" Text="" BorderColor="#D9DCE5" BorderStyle="None" Font-Size="Small"></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>

                                                <tr>
                                                    <td align="right" style="width: 40%">
                                                        Net Amount :
                                                    </td>
                                                    <td align="center"  style="width: 30%">
                                                        <asp:TextBox ID="txtNetAmount" runat="server" Width="100px" Style="text-align: right;"
                                                            CssClass="form-control" Font-Bold="True" Enabled="false" ForeColor="Black" BackColor="#D9DCE5"
                                                            class="input_text" Text="" BorderColor="#D9DCE5" BorderStyle="None" Font-Size="Small"></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>
                                            </table>
                                            <table id="ReceiveAmount" runat="server" style="width: 100%;">
                                                <tr>
                                                    <td align="right" style="width: 40%">
                                                        Receive Amount :
                                                    </td>
                                                    <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="txtReceiveAmount" runat="server" CssClass="form-control" Width="100px"
                                                            Font-Bold="True"  Style="text-align: right;" OnTextChanged="txtrceamount_TextChanged"
                                                           Enabled="false" AutoPostBack="true" Font-Size="Small" Text="0"></asp:TextBox>
                                                        
                                                    </td>
                                                     <td align="right" style="width: 40%">
                                                        Due Amount :
                                                    </td>
                                                       <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="TextDueAmount" runat="server" CssClass="form-control" Width="100px"
                                                            Font-Bold="True"  Style="text-align: right;" OnTextChanged="txtrceamount_TextChanged"
                                                           Enabled="false" AutoPostBack="true" Font-Size="Small" Text="0"></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>
                                                <tr id="trCashPaid" runat="server">
                                                    <td align="right" style="width: 40%">
                                                        Cash Paid :
                                                    </td>
                                                    <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="lblcashpaid" runat="server" CssClass="form-control" Width="100px"
                                                            Style="text-align: right;" Font-Bold="True" Enabled="True" ForeColor="Black"
                                                            BackColor="#D9DCE5" class="input_text" Text="0" BorderColor="#D9DCE5" BorderStyle="None"
                                                            Font-Size="Small" AutoPostBack="True"  onkeypress="return isNumberKey(event)"
                                                            ontextchanged="lblcashpaid_TextChanged"></asp:TextBox>


                                                          <%--    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                ControlToValidate="lblcashpaid" runat="server"
                                                ErrorMessage="Only Numbers allowed"
                                                ValidationExpression="\d+">
                                            </asp:RegularExpressionValidator>--%>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>
                                                <tr id="trCardPaid" runat="server" >
                                                    <td align="right" style="width: 40%">
                                                        <asp:Label
                                                            ID="Label2" runat="server" Text="Card Paid :"></asp:Label>
                                                        
                                                    </td>
                                                    <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="txtCardPaid" runat="server" Width="100px" CssClass="form-control"
                                                           Style="text-align: right;" Font-Bold="True" ForeColor="Black"
                                                            BackColor="#D9DCE5" class="input_text" Text="0" BorderColor="#D9DCE5" BorderStyle="None"
                                                            Font-Size="Small" AutoPostBack="True" 
                                                            ontextchanged="txtCardPaid_TextChanged" ></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>
                                                <tr id="trChange" runat="server" style="display:none">
                                                    <td align="right" style="width: 40%">
                                                        Change :
                                                    </td>
                                                    <td align="center" style="width: 30%">
                                                        <asp:TextBox ID="lblchange" runat="server" CssClass="form-control" Width="100px"
                                                            Style="text-align: right;" Font-Bold="True" Enabled="false" ForeColor="Black"
                                                            BackColor="#D9DCE5" class="input_text" Text="0" BorderColor="#D9DCE5" BorderStyle="None"
                                                            Font-Size="Small"></asp:TextBox>
                                                        
                                                    </td>
                                                    <td align="left" style="width: 30%">Tk.</td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        Remarks :
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <textarea id="txtRemarks" cols="30" rows="3" runat="server" cssclass="form-control"
                                                            maxlength="100"></textarea>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSaveSale" runat="server" Text="Save" CausesValidation="True" CssClass="btn btn-success"
                                                            OnClick="btnSaveSale_Click" UseSubmitBehavior="false" OnClientClick="this.disabled=true;"
                                                            Enabled="true" Style="width: 80px; height: 30px; padding: 0px 2px 2px 2px;" />
                                                        &nbsp;&nbsp; <%----%>
                                                        <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                                            CssClass="btn btn-warning" OnClick="btnCancel_Click" Style="width: 80px; height: 30px;
                                                            padding: 0px 2px 2px 2px;" />
                                                        <asp:Button ID="btnCheckChanges" runat="server" Text="Button" 
                                                            onclick="btnCheckChanges_Click" Visible="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
