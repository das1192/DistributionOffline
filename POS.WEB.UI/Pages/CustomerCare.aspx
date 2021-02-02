<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomerCare.aspx.cs" Inherits="Pages_CustomerCare" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript" language="Javascript">
     var specialKeys = new Array();
     specialKeys.push(8); //Backspace
     function IsNumeric(e) {
         var keyCode = e.which ? e.which : e.keyCode
         var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
         document.getElementById("error").style.display = ret ? "none" : "inline";
         return ret;
     }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...</ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="From Date "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                        <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                            TargetControlID="txtFromDate" />
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label3" runat="server" Text="To Date "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                        <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                            Enabled="True" TargetControlID="txtToDate" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        &nbsp;
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
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="MRP From"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProductPriceFrom" runat="server" Width="150px" CssClass="TextBox"  onkeypress="return IsNumeric(event);" ondrop="return false;" onpaste="return false;" />                                                                  
                        <span id="Span2" style="color: Red; display: none">* Input digits (0 - 9)</span>   
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label5" runat="server" Text="MRP To"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProductPriceTo" runat="server" Width="150px" CssClass="TextBox" onkeypress="return IsNumeric(event);" ondrop="return false;" onpaste="return false;"/>                                                               
                        <span id="Span1" style="color: Red; display: none">* Input digits (0 - 9)</span>   
                    </td>
                  
                </tr>
                <tr>
                    <td colspan="6" align="right">
                        <asp:Button ID="cmdProcess" runat="server" Text="Process" CssClass="Button_VariableWidth"
                            CausesValidation="False" OnClick="cmdProcess_Click" />&nbsp;
                        <asp:Button ID="cmdExport" runat="server" Text="Export To Excel" CssClass="Button_VariableWidth"
                            OnClick="cmdExport_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="left" style="width: 100%;" valign="bottom">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="gvDetailsInvoice" runat="server" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                            ShowHeaderWhenEmpty="True" Width="100%" CssClass="mGrid">
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

                                <asp:TemplateField HeaderText="Invoice">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNo1" runat="server" Text='<%#Bind("InvoiceNo") %>'></asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIDAT1" runat="server" Text='<%#Bind("Purchase_Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                      <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName1" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                      <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="DOB" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateOfBirth1" runat="server" Text='<%#Bind("DOB","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                      <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="MOB">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobileNo1" runat="server" Text='<%#Bind("MOB") %>'></asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="ALT_MOB" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAlternativeMobileNo1" runat="server" Text='<%#Bind("ALT_MOB") %>'></asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Address" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress1" runat="server" Text='<%#Bind("Address") %>'></asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Email" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmailAddress" runat="server" Text='<%#Bind("EmailAddress") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>   

                                <asp:TemplateField HeaderText="Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWGPG_NAME1" runat="server" Text='<%#Bind("Category") %>'></asp:Label>
                                    </ItemTemplate>
                                   <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Model">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubCategoryName1" runat="server" Text='<%#Bind("Model") %>'></asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description/Color">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription1" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="IMEI/Barcode">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Payment Mood" Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentMode1" runat="server" Text='<%#Bind("PaymentMode") %>'></asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="MRP" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentMode1" runat="server" Text='<%#Bind("SalePrice") %>'></asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                                            
                            </Columns>                           
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
