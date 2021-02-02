<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="T_PRODForm.aspx.cs" Inherits="TalukderPOS.Web.UI.T_PRODForm" Title="Purchase Entry"
    EnableEventValidation="false" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="Javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <script type="text/javascript">

        function openPopup() {

            window.open("T_WGPGForm.aspx", "_blank", "WIDTH=1080,HEIGHT=790,scrollbars=no, menubar=no,resizable=yes,directories=no,location=no");

        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        //<![CDATA[    
        //
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
            width: 300px;
            outline: none;
            font-family: Times New Roman,Arial,Sans-Serif;
            font-size: 25px;
            font-weight: bold;
            text-align: center;
        }
        .display-on
        {
            display:none;
            
            }
        #loginform
        {
            min-width: 200px;
            height: 140px;
            background-color: #ffffff;
            border: 1px solid;
            border-color: #555555;
            padding: 16px 16px;
            border-radius: 4px;
            -webkit-box-shadow: 0px 1px 6px rgba(75, 31, 57, 0.8);
            -moz-box-shadow: 0px 1px 6px rgba(75, 31, 57, 0.8);
            box-shadow: 0px 1px 6px rgba(223, 88, 13, 0.8);
        }
        .modalBackground
        {
            background-color: #333333;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .txt
        {
            color: #505050;
        }
        .redstar
        {
            color: #FF0000;
        }
        .hidden
        {
            display: none;
        }
        
        
        
        .Background
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .Popup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 400px;
            height: 350px;
        }
        .lbl
        {
            font-size: 16px;
            font-style: italic;
            font-weight: bold;
        }
        
        
        
        
        #grid-header
        {
            background-color: #f49521;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...</ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="tContT_PROD" runat="server" Width="100%" CssClass="fancy fancy-green"
                ActiveTabIndex="4">
                <asp:TabPanel ID="tPnlDisplay" runat="server" HeaderText="Purchase List">
                    <ContentTemplate>
                        <table width="100%" class="table-responsive">
                            <tbody>
                                <tr>
                                    <td align="center" colspan="6" style="font-weight: bolder">
                                        Purchase Search
                                        <hr style="color: Green" />
                                        <hr style="color: Green" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label23" runat="server" Style="font-weight: normal" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="TextBox" Width="150px"></asp:TextBox><asp:CalendarExtender
                                            ID="CalendarExtender2" runat="server" Enabled="True" Format="yyyy-MM-dd" TargetControlID="txtDateFrom">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label24" runat="server" Style="font-weight: normal" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="TextBox" Width="150px"></asp:TextBox><asp:CalendarExtender
                                            ID="CalendarExtender3" runat="server" Enabled="True" Format="yyyy-MM-dd" TargetControlID="txtDateTo">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label31" runat="server" Text="Supplier"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlVendorListnew" runat="server" CssClass="DropDownList">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CascadingDropDown14" runat="server" Category="Vendor"
                                                Enabled="True" LoadingText="Loading Supplier..." PromptText="--Please Select--"
                                                ServiceMethod="BindVendor" ServicePath="~/DropdownWebService.asmx" TargetControlID="ddlVendorListnew">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlSearchProductCategory" runat="server" CssClass="DropDownList">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CDD2" runat="server" Category="Category" Enabled="True"
                                                LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                                ServicePath="~/DropdownWebService.asmx" TargetControlID="ddlSearchProductCategory">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Brand"></asp:Label>
                                    </td>

                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlSearchSubCategory" runat="server" CssClass="DropDownList">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CDD3" runat="server" Category="Model" Enabled="True" LoadingText="Loading Model..."
                                                ParentControlID="ddlSearchProductCategory" PromptText="--Please Select--" ServiceMethod="BindModel"
                                                ServicePath="~/DropdownWebService.asmx" TargetControlID="ddlSearchSubCategory">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>


                                    <td align="right">
                                        <asp:Label ID="Label7" runat="server" Text="Description"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlSearchDescription" runat="server" CssClass="DropDownList">
                                            </asp:DropDownList>
                                            <asp:CascadingDropDown ID="CDD4" runat="server" Category="Description" Enabled="True"
                                                LoadingText="Loading Color..." ParentControlID="ddlSearchSubCategory" PromptText="--Please Select--"
                                                ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx" TargetControlID="ddlSearchDescription">
                                            </asp:CascadingDropDown>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="cmdSearch" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                            OnClick="cmdSearch_Click" Text="Search" />&nbsp;&nbsp;<asp:Button ID="cmdPreview"
                                                runat="server" CausesValidation="False" CssClass="btn btn-primary" OnClick="cmdPreview_Click"
                                                Text="Preview" />
                                    </td>
                                    <td align="right" colspan="4">
                                        &nbsp;&nbsp; &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <table width="100%">
                                            <tr>
                                                <td align="center" style="font-weight: bolder">
                                                    Purchase List
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%;" valign="bottom">
                                                    <asp:Label ID="Label26" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTolQuantity" runat="server" Font-Bold="True" Font-Size="Large"
                                                        ForeColor="Green"></asp:Label><asp:Label ID="lblTotgrandtotal" runat="server" Font-Bold="True"
                                                            Font-Size="Large" ForeColor="Green"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gvT_PROD" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="mGrid"
                                                        DataKeyNames="OID" EmptyDataText="No rows returned" GridLines="None" OnPageIndexChanging="gvT_PROD_PageIndexChanging"
                                                        OnRowDeleting="gvT_PROD_RowDeleting" OnRowEditing="gvT_PROD_RowEditing" PageSize="25"
                                                        Width="100%" CaptionAlign="Top">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    SI</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSRNO" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ProductID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOID" runat="server" Text='<%#Bind("OID") %>' Visible="false"></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vendor">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCCOM_NAME" runat="server" Text='<%#Bind("Vendor_Name") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
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
                                                          <%--  <asp:TemplateField HeaderText="Barcode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Cost Price">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCostPrice" runat="server" Text='<%#Bind("CostPrice") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgrand" runat="server" Text='<%#Bind("grandtotal") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIDAT" runat="server" Text='<%#Bind("IDAT") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ageing">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAgeing" runat="server" Text='<%#Bind("Ageing") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:CommandField ButtonType="Image" CausesValidation="False" EditImageUrl="~/Images/edit.png"
                                                                Visible="false" HeaderText="Edit" ShowCancelButton="False" ShowEditButton="True">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                                        Text="Delete"><img alt="Delete" 
                                                                    src="../Images/Delete.gif" /></asp:LinkButton></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pgr" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        </td></tr><tr>
                            <td colspan="6">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                                EmptyDataText="No rows returned" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            SI</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CCOM_NAME" HeaderText="Branch">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="WGPG_NAME" HeaderText="Category">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SubCategoryName" HeaderText="Brand">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <AlternatingRowStyle CssClass="alt" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        </tbody></table><table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnHidden" runat="server" Style="display: none" /><asp:ModalPopupExtender
                                        ID="ModalPopupExtender1" TargetControlID="btnHidden" PopupControlID="popUpPanel"
                                        CancelControlID="btnclose" BackgroundCssClass="modalBackground" DropShadow="True"
                                        runat="server" DynamicServicePath="" Enabled="True">
                                    </asp:ModalPopupExtender>
                                    <asp:Panel ID="popUpPanel" runat="server">
                                        <div id="loginform">
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblDeleteReasonMessage" runat="server" Style="font-weight: bold" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label25" runat="server" Text="Product ID:" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblProductID" runat="server" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label29" runat="server" Text="IMEI:" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBarcode" runat="server" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblReason" runat="server" Text="Delete Reason"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span style="color: red;">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDeleteReason" runat="server" CssClass="txtStyle" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="Button_VariableWidth"
                                                            OnClick="btnSubmitDiscount_Click" CausesValidation="False" Width="100px" />&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnclose" runat="server" Text="Close" CausesValidation="False" CssClass="Button_VariableWidth"
                                                            Width="100px" />
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
                <asp:TabPanel ID="tpEditor" runat="server" HeaderText="Purchase Add/Edit" >
                    <ContentTemplate>
                        <div class="col-lg-5">
                            <div class="row">
                                <div class="table-responsive">
                                    <table cellpadding="2" border="0" width="100%">
                                        <tr id="trCategory" runat="server">
                                            <td align="left" runat="server">
                                                &nbsp;<asp:Label ID="lblProductCategoryId" runat="server" Text="Product Category"></asp:Label>
                                            </td>
                                            <td runat="server">
                                                <div>
                                                    <asp:DropDownList ID="ddlProductCategoryId" runat="server" CssClass="form-control"
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="ddlProductCategoryId_SelectedIndexChanged1">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trModel" runat="server">
                                            <td align="left" runat="server">
                                                &nbsp;<asp:Label ID="Label4" runat="server" Text="Brand"></asp:Label>
                                            </td>
                                            <td runat="server">
                                                <div>
                                                    <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control" 
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="ddlSubCategory_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trDescription" runat="server">
                                            <td align="left" runat="server">
                                                &nbsp;<asp:Label ID="Label3" runat="server" Text="Description"></asp:Label>
                                            </td>
                                            <td runat="server">
                                                <div>
                                                    <asp:DropDownList ID="ddlDescription" runat="server" CssClass="form-control" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlDescription_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label2" runat="server" Text="SES Price"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label28" runat="server" Text="MRP"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtSalePrice" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trvendor" runat="server">
                                            <td id="Td5" align="left" runat="server">
                                                <asp:Label ID="Label17" runat="server" Text="Vendor"></asp:Label>
                                            </td>
                                            <td id="Td6" runat="server">
                                                <div>
                                                    <asp:DropDownList ID="ddlVendorList" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <asp:CascadingDropDown ID="CCD8" runat="server" Category="Vendor" TargetControlID="ddlVendorList"
                                                        LoadingText="Loading Vendor..." PromptText="--Please Select--" ServiceMethod="BindVendor"
                                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                                    </asp:CascadingDropDown>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label12" runat="server" Text="Entry Mode(Quantity)"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlentry" runat="server" CssClass="form-control" 
                                                    OnSelectedIndexChanged="ddlentry_SelectedIndexChanged"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Text="Barcode" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                       <tr>
                                            <td align="left">
                                                &nbsp;<asp:Label ID="Label5" runat="server" Text="Barcode/IMEI"></asp:Label>
                                            </td>
                                            <td>
                                                <div>
                                                    <textarea id="txtBarcode" cols="30" rows="3" runat="server" cssclass="form-control"></textarea>
                                                    &nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trQuantity" runat="server" visible="False">
                                            <td align="left" runat="server">
                                                &nbsp;<asp:Label ID="lblQuantity" runat="server" Text="Quantity"></asp:Label>
                                            </td>
                                            <td runat="server">
                                                <div>
                                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" /></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="btn btn-sm btn-info"
                                                    OnClick="cmdAdd_Click" CausesValidation="False" />&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="lblOID" runat="server" />
                                                <asp:HiddenField ID="hidBarcode" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="row">
                                <div class="row">
                                    <div class="col-md-5">
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Label ID="Label33" runat="server" CssClass="control-label">Purchase Date</asp:Label></div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="dtpPurchaseDate" runat="server" CssClass="form-control" /><asp:CalendarExtender
                                            ID="CE_dtpPurchaseDate" runat="server" Enabled="True" Format="yyyy-MM-dd HH:mm"
                                            TargetControlID="dtpPurchaseDate" />
                                    </div>
                                </div>
                                <asp:Label ID="lblPurTolQuantity" runat="server" Font-Bold="True" Font-Size="Large"
                                    ForeColor="Green"></asp:Label>
                                    <asp:Label ID="lblPurTotgrandtotal" runat="server"
                                        Font-Bold="True" Font-Size="Large" ForeColor="Green"></asp:Label>
                                        <div class="table-responsive" style="padding-top:15px">
                                            <asp:GridView ID="gvT_BarCode" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                CssClass="table table-bordered table-hover table-condensed table-responsive"
                                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                                Width="100%" OnRowDeleting="gvT_PROD_ADD_RowDeleting" ShowFooter="True">
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            SI</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PROD_WGPG" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPROD_WGPG" runat="server" Text='<%#Bind("PROD_WGPG") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Category">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("Category") %>'></asp:Label></ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-sm btn-warning"
                                                                OnClick="btnCancel_Click" CausesValidation="False" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PROD_SUBCATEGORY" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPROD_SUBCATEGORY" runat="server" Text='<%#Bind("PROD_SUBCATEGORY") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Model">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubCategory" runat="server" Text='<%#Bind("SubCategory") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PROD_DES" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPROD_DES" runat="server" Text='<%#Bind("PROD_DES") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VendorID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVendorID" runat="server" Text='<%#Bind("VendorID") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVendor" runat="server" Text='<%#Bind("Vendor") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                               <asp:TemplateField HeaderText="IMEI/ BarCode">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBarCode" runat="server" Text='<%#Bind("BarCode") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField> 
                                                    <asp:TemplateField HeaderText="Purchase Price">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCostPrice" runat="server" Text='<%#Bind("CostPrice") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="MRP">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSellPrice" runat="server" Text='<%#Bind("SellPrice") %>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>'></asp:Label></ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-sm btn-success"
                                                                OnClick="btnSave_Click" CausesValidation="False" OnClientClick="this.disabled=true;"
                                                                UseSubmitBehavior="False" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                                Text="Delete"><img alt="Delete" src="../Images/Delete.gif" /></asp:LinkButton></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="MRP Adjustment">
                    <ContentTemplate>
                        <div id="dvMRPAdjustment">
                            <div class="row" style="padding: 0px 50px">
                                <div class="row">
                                    <p class="text-bold text-center">
                                        Product Price Adjutment</p>
                                    <hr style="color: Green" />
                                </div>
                                <div class="row" style="padding: 3px 0px">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblProductCategoryIdProadj" runat="server" Text="Product Category"
                                            CssClass="control-label"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlProductCategoryIdProAdj" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="Cascadingdropdown1" runat="server" Category="Category"
                                            TargetControlID="ddlProductCategoryIdProAdj" LoadingText="Loading Categories..."
                                            PromptText="--Please Select--" ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx"
                                            Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="lblMessageUpdate" runat="server" CssClass="text-bold text-danger"></asp:Label>
                                    </div>
                                </div>
                                <div class="row" style="padding: 3px 0px">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label8" runat="server" Text="Brand" CssClass="control-label"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSubCategoryIdProAdj" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Model" TargetControlID="ddlSubCategoryIdProAdj"
                                            ParentControlID="ddlProductCategoryIdProAdj" LoadingText="Loading Model..." PromptText="--Please Select--"
                                            ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="row" style="padding: 3px 0px">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label9" runat="server" Text="Description" CssClass="control-label"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlDescriptionPrpAdj" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown3" runat="server" Category="Description"
                                            TargetControlID="ddlDescriptionPrpAdj" ParentControlID="ddlSubCategoryIdProAdj"
                                            LoadingText="Loading Color..." PromptText="--Please Select--" ServiceMethod="BindDescription"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="row" style="padding: 3px 0px">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label11" runat="server" Text="Update MRP" CssClass="control-label"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtUpdateSalePrice" runat="server" CssClass="form-control text-bold text-center"
                                            onkeypress="return isNumberKey(event)" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                ControlToValidate="txtUpdateSalePrice" ErrorMessage="Sale Price is required"
                                                Display="Dynamic" runat="server" />
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="row" style="padding: 10px 0px">
                                    <div class="col-md-6">
                                        <asp:Button ID="Button2" runat="server" Text="Update Price" CssClass="btn btn-sm btn-success"
                                            OnClick="btnUpdate_Click" CausesValidation="False" />&nbsp;&nbsp;
                                        <asp:Button ID="Button3" runat="server" Text="Cancel" CssClass="btn btn-sm btn-warning"
                                            OnClick="btnCancel_Click" CausesValidation="False" />
                                    </div>
                                </div>
                                <div class="row">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tPnlDisplay2" runat="server" HeaderText="Purchase Return" Visible="false">
                    <ContentTemplate>
                        <table width="100%">
                            <tr style="display: none">
                                <td colspan="4" align="center" style="font-weight: bolder">
                                    Purchase Return
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <td id="Td1" align="left" runat="server" style="width: 20%;">
                                    &nbsp;<asp:Label ID="Label10" runat="server" Text="Product Category"></asp:Label>
                                </td>
                                <td id="Td2" runat="server" style="width: 30%;">
                                    <div>
                                        <asp:DropDownList ID="ddlProductCategoryIdRET" runat="server" 
                                            CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlProductCategoryIdRET_SelectedIndexChanged"
                                            >
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td style="width: 20%;" runat="server">
                                </td>
                                <td style="width: 30%;" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <asp:Label ID="Label13" runat="server" Text="Brand"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:DropDownList ID="ddlSubCategoryIdProRET" runat="server" 
                                        CssClass="form-control" AutoPostBack="True"
                                        onselectedindexchanged="ddlSubCategoryIdProRET_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20%;">
                                </td>
                                <td style="width: 30%;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <asp:Label ID="Label14" runat="server" Text="Description"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:DropDownList ID="ddlDescriptionPrpRET" runat="server" CssClass="form-control" 
                                    AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlDescriptionPrpRET_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20%;">
                                </td>
                                <td style="width: 30%;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtReturnPrice"
                                        Display="Dynamic" ErrorMessage="Return Price is required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="tr2" runat="server">
                                <td id="Td3" align="left" runat="server" style="width: 20%;">
                                    <asp:Label ID="Label18" runat="server" Text="Vendor"></asp:Label>
                                </td>
                                <td id="Td4" runat="server" style="width: 30%;">
                                    <div>
                                        <asp:DropDownList ID="ddlVendorListPRF" runat="server" CssClass="form-control"   
                                       OnSelectedIndexChanged="ddlVendorListPRF_SelectedIndexChanged" AutoPostBack="true">
                                        
                                        </asp:DropDownList>
                                     <%--   <asp:CascadingDropDown ID="CascadingDropDown4" runat="server" Category="Description" 
                                        TargetControlID="ddlVendorListPRF" ParentControlID="ddlDescriptionPrpRET"
                                        LoadingText="Loading Vendor..." PromptText="--Please Select--" ServiceMethod="BindDescription"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>--%>
                                    </div>
                                </td>
                                <td style="width: 20%;" runat="server">
                                </td>
                                <td style="width: 30%;" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20%;">
                                    <asp:Label ID="Label32" runat="server" Text="Return Unit Cost" ToolTip="Return Unit Cost"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:DropDownList ID="ddlUnitCostPRF" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20%;">
                                    &nbsp;&nbsp;
                                </td>
                                <td style="width: 30%;">
                                    <asp:TextBox ID="txtReturnPrice" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)"
                                        Visible="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20%;">
                                    <asp:Label ID="Label15" runat="server" Text="Return Quantity"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:TextBox ID="txtUpdateReturnPRF" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </td>
                                <td style="width: 20%;">
                                    &nbsp;&nbsp;
                                </td>
                                <td style="width: 30%;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                </td>
                                <td style="width: 30%;">
                                    <asp:Button ID="Button1" runat="server" Text="Return" CssClass="btn btn-success"
                                        OnClick="btnReturn_Click" CausesValidation="False" />&nbsp;&nbsp;
                                </td>
                                <td style="width: 20%;">
                                </td>
                                <td style="width: 30%;">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label16" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tPnlDisplay3" runat="server" HeaderText="Purchase History">
                    <ContentTemplate>
                        <div id="tabPurchaseHistory">
                            <div class="row">
                            </div>
                            <div class="row">
                            </div>
                            <div class="row">
                            </div>
                        </div>
                        <table width="100%">
                            <thead>
                                <tr>
                                    <td colspan="6" align="center" style="font-weight: bolder">
                                        <%--Purchase History
                                        <hr style="color: Green" />--%>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label19" Style="font-weight: normal" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateFrom2" runat="server" Width="150px" CssClass="TextBox" placeholder="From Date" /><asp:CalendarExtender
                                            ID="CalendarExtender5" Format="yyyy-MM-dd" runat="server" Enabled="True" TargetControlID="txtDateFrom2" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label20" Style="font-weight: normal" runat="server" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateTo2" runat="server" Width="150px" CssClass="TextBox" placeholder="To Date" /><asp:CalendarExtender
                                            ID="CalendarExtender6" Format="yyyy-MM-dd" runat="server" Enabled="True" TargetControlID="txtDateTo2" />
                                    </td>
                                    <td>
                                        Search Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSearchOption" runat="server" CssClass="DropDownList">
                                            <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Purchase" Value="Purchase"></asp:ListItem>
                                            <asp:ListItem Text="Purchase Return" Value="Purchase Return"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label21" runat="server" Text="Category"></asp:Label>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="ddlSearchProductCategory3" runat="server" CssClass="DropDownList">
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
                                            <asp:DropDownList ID="ddlSearchSubCategory3" runat="server" CssClass="DropDownList">
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
                                            <asp:DropDownList ID="ddlSearchDescription3" runat="server" CssClass="DropDownList">
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
                                    <td colspan="6">
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="Button4" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                            OnClick="cmdSearch3_Click" Text="Search" />
                                    </td>
                                    <td>
                                    </td>
                                    <td align="right" colspan="3">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <table width="100%">
                                            <tr>
                                                <td align="center" style="font-weight: bolder">
                                                    Purchase History
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%;" valign="bottom">
                                                    <asp:Label ID="Label30" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gv_Purchase_History" runat="server" AutoGenerateColumns="False"
                                                        GridLines="None" CssClass="mGrid" PageSize="25" AllowPaging="True" DataKeyNames="OID"
                                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                                        OnPageIndexChanging="gv_Purchase_History_PageIndexChanging" Width="100%">
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
                                                            <asp:TemplateField HeaderText="Vendor">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCCOM_NAME" runat="server" Text='<%#Bind("Vendor_Name") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
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
                                                            <asp:TemplateField HeaderText="Cost Price">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCostPrice" runat="server" Text='<%#Bind("CostPrice") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText="Qty">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Particular">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblParticular" runat="server" Text='<%#Bind("Particular") %>'></asp:Label></ItemTemplate>
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
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
