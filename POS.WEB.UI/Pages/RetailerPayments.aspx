<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RetailerPayments.aspx.cs" Inherits="TalukderPOS.Web.UI.Vendor_OutgoingForm"
    Title="Retailer Payment" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContainerProductVendor" runat="server" ActiveTabIndex="0" Width="100%"
                CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Retailer Due Adjustment">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6" style="padding: 0px 35px; border-right: 2px solid gray;">
                                <table cellpadding="2" border="0" width="100%">
                                    <tr id="tr2" runat="server">
                                        <td id="Td5" align="left" runat="server">
                                            <asp:Label ID="Label10" runat="server" Text="Date"></asp:Label>
                                        </td>
                                        <td id="Td6" runat="server">
                                        </td>
                                        <td id="Td7" runat="server">
                                            <asp:TextBox ID="dptSupplierPaymentDate" runat="server" 
                                                CssClass="form-control" />
                                            <asp:CalendarExtender ID="CE_dptSupplierPaymentDate" runat="server" Enabled="True" Format="yyyy-MM-dd"
                                                TargetControlID="dptSupplierPaymentDate" />
                                        </td>
                                    </tr>
                                    <tr id="trvendor" runat="server">
                                        <td id="Td1" align="left" runat="server">
                                            <asp:Label ID="Label17" runat="server" Text="Retailer"></asp:Label>
                                        </td>
                                        <td runat="server">
                                        </td>
                                        <td runat="server">
                                            <div>
                                                <asp:DropDownList ID="ddlRetailerList" runat="server" CssClass="form-control" 
                                                    AutoPostBack="True" onselectedindexchanged="ddlRetailerList_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                     <tr id="tr1" runat="server">
                                        <td id="Td2" align="left" runat="server">
                                            <asp:Label ID="lblPayablelbl" runat="server"></asp:Label>
                                        </td>
                                        <td id="Td3" runat="server">
                                        </td>
                                        <td id="Td4" runat="server">
                                            <asp:Label ID="lblPayableAmount" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Remarks:"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Payment Mode"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                             <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control" 
                                                 onselectedindexchanged="ddlPaymentMode_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trBank" runat="server" visible="False">
                                        <td runat="server">
                                            <asp:Label ID="Label12" runat="server" Text="Bank Name"></asp:Label>
                                        </td>
                                        <td runat="server">
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td runat="server">
                                             <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                    
                                    <tr id="trCashAmt" runat="server">
                                        <td>
                                            <asp:Label ID="lblAmount" runat="server" Text="Cash Amount:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control text-center" BackColor="#FFFF99" PlaceHolder="Enter Cash Amount"
                                                Font-Bold="True" />
                                                 <asp:RegularExpressionValidator ID="RevtxtAmount" ControlToValidate="txtAmount"
                                                runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                             
                                        </td>
                                    </tr>
                                    <tr id="trBankAmt" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label13" runat="server" Text="Bank Amount:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                          <asp:TextBox ID="txtBankAmount" runat="server" CssClass="form-control text-center" BackColor="#ff9999" PlaceHolder="Enter Bank Amount" 
                                                Font-Bold="True" />
                                               
                                       <asp:RegularExpressionValidator ID="RevtxtBankAmount" ControlToValidate="txtBankAmount"
                                                runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click"
                                                OnClientClick="this.disabled=true;" UseSubmitBehavior="False" />&nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                                CssClass="btn btn-warning" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                            <asp:HiddenField ID="lblOID" runat="server" />
                                            <asp:HiddenField ID="hdfRefNo" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-6" style="padding: 0px 35px;">
                                <asp:GridView ID="gvCashBalance" runat="server" AutoGenerateColumns="False" GridLines="None"
                                    CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Visible="false"
                                    EmptyDataText="No rows returned" Width="100%">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:BoundField DataField="BankName" HeaderText="Bank">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AccountNo" HeaderText="Account No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Debit" HeaderText="Debit">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Credit" HeaderText="Credit">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Balance" HeaderText="Balance">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Supplier Payment History">
                    <HeaderTemplate>
                        Retailer Payment History
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="5" align="center" style="font-weight: bolder">
                                    Retailer Payment History
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label2" Style="font-weight: normal" runat="server" Text="From Date"></asp:Label>
                                    <asp:TextBox ID="FromdateSup" runat="server" Width="150px" CssClass="TextBox" />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="FromdateSup" />
                                    <asp:Label ID="Label3" Style="font-weight: normal" runat="server" Text="To Date"></asp:Label>
                                    <asp:TextBox ID="TodateSup" runat="server" Width="150px" CssClass="TextBox" />
                                    <asp:CalendarExtender ID="CalendarExtender4" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="TodateSup" />
                                </td>
                                <td>
                                    <asp:Label ID="Label31" runat="server" Text="Retailer"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlRetailerListnew" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown78" runat="server" Category="Retailer"
                                            TargetControlID="ddlRetailerListnew" LoadingText="Loading Retailer..." PromptText="--Please Select--"
                                            ServiceMethod="BindRetailer" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td colspan="1">
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="False" CssClass="btn btn-info"
                                        OnClick="cmdSearchSup_Click" Text="Search" />
                                </td>
                                <td align="right" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 100%;" valign="bottom">
                                                <asp:Label ID="lblMsgSupplierPaymentList" runat="server" ForeColor="Red" 
                                                    Font-Bold="True" BackColor="#FFFF66" Font-Italic="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grdRetailerAmt" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                    CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                    DataKeyNames="ReferenceNo,OID,BankID" EmptyDataText="No rows returned" OnRowDeleting="grdRetailerAmt_RowDeleting"
                                                    OnRowEditing="grdRetailerAmt_RowEditing" Width="100%">
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
                                                        <asp:BoundField DataField="OID" HeaderText="OID" Visible="False">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RetailerName" HeaderText="Retailer">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="CashAmount" HeaderText="Cash Amount">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="CardAmount" HeaderText="Card Amount">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="BankName" HeaderText="Bank Name">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IDAT" HeaderText="Date">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                            Visible="False" ShowEditButton="True" EditImageUrl="~/Images/edit.png" 
                                                            ButtonType="Image">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:CommandField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                                    Text="Delete" OnClientClick="return confirm('Are you sure to delete?')">
                                                        <img alt="Delete" src="../Images/Delete.gif" />
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
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
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tabpanel3" runat="server" HeaderText="Supplier Payment Delete" Visible="false">
                    <HeaderTemplate>
                        Deleted Payment History
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="3" align="center" style="font-weight: bolder">
                                    Deleted Payment History
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label23" Style="font-weight: normal" runat="server" Text="From Date"></asp:Label>
                                    <asp:TextBox ID="txtDateFromdelete" runat="server" Width="150px" CssClass="TextBox" /><asp:CalendarExtender
                                        ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True" TargetControlID="txtDateFromdelete" />
                                    <asp:Label ID="Label24" Style="font-weight: normal" runat="server" Text="To Date"></asp:Label>
                                    <asp:TextBox ID="txtDateTodelete" runat="server" Width="150px" CssClass="TextBox" /><asp:CalendarExtender
                                        ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True" TargetControlID="txtDateTodelete" />
                                </td>
                                <td align="right" colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="btn btn-info" OnClick="cmdSearchDelete_Click"
                                        CausesValidation="False" />&nbsp;&nbsp;
                                </td>
                                <td>
                                </td>
                                <td align="right">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 100%;" valign="bottom">
                                                <asp:Label ID="Label26" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvT_Delete" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                    CssClass="mGrid" AllowPaging="True" DataKeyNames="OID" BorderColor="#CCCCCC"
                                                    BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                                    OnPageIndexChanging="gvT_Delete_PageIndexChanging" Width="100%">
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
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblParticular" runat="server" Text='<%#Bind("Vendor_Name") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPurpose" runat="server" Text='<%#Bind("AMOUNT") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("NEWDATE") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remark">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Remarks") %>'></asp:Label></ItemTemplate>
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
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Retailer Due Report">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label5" runat="server" Text="Retailer"></asp:Label>:
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlRetailerID" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="Cascadingdropdown1" runat="server" Category="Retailer" TargetControlID="ddlRetailerID"
                                            LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindRetailer"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Button ID="cmdSearchForprice" runat="server" CausesValidation="False" CssClass="btn btn-info"
                                        OnClick="cmdSearchForprice_Click" Text="Search" />
                                </td>
                                <td>
                                    &nbsp; <asp:Button ID="Button2" runat="server" CausesValidation="False" CssClass="btn btn-warning"
                                        OnClick="Print_Click" Text="Print" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="2">
                                        <asp:GridView ID="grdRetailerDue" runat="server" AutoGenerateColumns="False" GridLines="None" DataKeyNames="ID"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        EmptyDataText="No rows returned" Width="100%">
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
                                            <asp:TemplateField HeaderText="Retailer">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("Name") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Sale">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPurchase" runat="server" Text='<%#Bind("TotalSaleAmount") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Sale Return">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPurchase" runat="server" Text='<%#Bind("TotalSaleReturn") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Total Payment">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayment" runat="server" Text='<%#Bind("TotalPayment") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="Current Due Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPayment" runat="server" Text='<%#Bind("TotalDueAmount") %>'></asp:Label></ItemTemplate>
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
                <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="Supplier Transaction" Visible="false">
                    <HeaderTemplate>
                        Supplier Transaction
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="5" align="center" style="font-weight: bolder">
                                    Supplier Transaction
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label6" Style="font-weight: normal" runat="server" Text="From Date"></asp:Label>
                                    <asp:TextBox ID="dtFromDateST" runat="server" Width="150px" CssClass="TextBox" />
                                    <asp:CalendarExtender ID="CalendarExtender5" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="dtFromDateST" />
                                    <asp:Label ID="Label7" Style="font-weight: normal" runat="server" Text="To Date"></asp:Label>
                                    <asp:TextBox ID="dtToDateST" runat="server" Width="150px" CssClass="TextBox" />
                                    <asp:CalendarExtender ID="CalendarExtender6" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="dtToDateST" />
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Particular"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlParticularST" runat="server" CssClass="DropDownList">
                                            <asp:ListItem Selected="True">--Please Select--</asp:ListItem>
                                            <asp:ListItem>Purchase</asp:ListItem>
                                            <asp:ListItem Value="Purchase Amendment">Purchase Amendment</asp:ListItem>
                                            <asp:ListItem Value="Purchase Return">Purchase Return</asp:ListItem>
                                            <asp:ListItem>Payment</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td colspan="1">
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblVendorST" runat="server" Text="Supplier"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlVendorST" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingddlVendorST" runat="server" Category="Vendor"
                                        Enabled="True" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                        ServiceMethod="BindVendor" ServicePath="~/DropdownWebService.asmx" TargetControlID="ddlVendorST">
                                    </asp:CascadingDropDown>
                                </td>
                                <td colspan="1">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnSearchSupplierTransaction" runat="server" CausesValidation="False"
                                        CssClass="btn btn-info" OnClick="btnSearchSupplierTransaction_Click" Text="Search" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="1">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 100%;" valign="bottom">
                                                <asp:Label ID="lblTotal" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                                                <asp:Label ID="lblPurchase" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Green"></asp:Label>
                                                <asp:Label ID="lblPurchaseAmendment" runat="server" Font-Bold="True" Font-Size="Large"
                                                    ForeColor="Green"></asp:Label>
                                                <asp:Label ID="lblPurchaseReturn" runat="server" Font-Bold="True" Font-Size="Large"
                                                    ForeColor="Green"></asp:Label>
                                                <asp:Label ID="lblPayment" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblBalance" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblGrandTotalST" runat="server" Font-Bold="True" Font-Size="Large"
                                                    ForeColor="Green"></asp:Label>
                                                &nbsp;<asp:Label ID="Label9" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvSupplierTransaction" runat="server" AutoGenerateColumns="False"
                                                    GridLines="None" CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" EmptyDataText="No rows returned" OnRowDeleting="grdRetailerAmt_RowDeleting"
                                                    OnRowEditing="grdRetailerAmt_RowEditing" Width="100%">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Vendor_Name" HeaderText="Supplier Name">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Particular" HeaderText="Particular">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TotalQty" HeaderText="Total Quantity">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Total" HeaderText="Total Amount">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IDAT" HeaderText="Transaction Date" DataFormatString="{0:dd-MMM-yyyy}">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
