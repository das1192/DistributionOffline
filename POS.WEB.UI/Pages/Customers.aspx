<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Customers.aspx.cs" Inherits="TalukderPOS.Web.UI.Vendor_OutgoingForm"
    Title="Retailers" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerProductVendor" runat="server" ActiveTabIndex="2" Width="100%"
                CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Commission Journal">
                    <HeaderTemplate>
                        Add Retailer
                    </HeaderTemplate>
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
                                            <asp:TextBox ID="dptSupplierPaymentDate" runat="server" CssClass="form-control" />
                                            <asp:CalendarExtender ID="CE_dptSupplierPaymentDate" runat="server" Enabled="True"
                                                Format="yyyy-MM-dd" TargetControlID="dptSupplierPaymentDate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CName" runat="server" Text="Retailer Name:" Font-Bold="True" ></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="CustomerName" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"/>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="CustomerName"
                                                ErrorMessage="Retailer Name is required" Display="Dynamic" runat="server" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAddress" runat="server" Text="Address:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                                Font-Bold="True" />
                                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatorAddress" ControlToValidate="txtAddress"
                                                runat="server" ErrorMessage="Address is required."></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNumber" runat="server" Text="Number:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="MNumber" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                                Font-Bold="True" />
                                           <%-- <asp:RequiredFieldValidator ID="rfvAmount" ControlToValidate="MNumber" ErrorMessage="Mobile Number is required"
                                                Display="Dynamic" runat="server" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="MNumber"
                                                runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPhone" runat="server" Text="Telephone:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                                Font-Bold="True" />
                                           <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtPhone"
                                                runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEmail" runat="server" Text="Email:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                            
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                                Font-Bold="True" />
                                          <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidatorForEmail" ControlToValidate="MNumber"
                                                ErrorMessage="Email is required" Display="Dynamic" runat="server" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorForEmail" ControlToValidate="txtEmail"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server"
                                                ErrorMessage="Invalid Email Address."></asp:RegularExpressionValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBalance" runat="server" Text="Opening Balance:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBalance" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                                Font-Bold="True" />
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorBalance" ControlToValidate="txtBalance" ErrorMessage="Opening Balance is required."
                                                Display="Dynamic" runat="server" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorBalance" ControlToValidate="txtBalance"
                                                runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRemarks" runat="server" Text="Remarks:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                                Font-Bold="True" />
                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStatus" runat="server" Text="Active Status:"  Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkStatus"  runat="server" Checked="true"/>
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
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"> </asp:Label>
                                            <asp:HiddenField ID="lblOID" runat="server" />
                                            <asp:HiddenField ID="hdfRefNo" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                    <HeaderTemplate>
                        Retailer List<br />
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="grdCustomerList" runat="server" AutoGenerateColumns="False" GridLines="None" DataKeyNames="ID"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="3" OnRowEditing="grdCustomerList_RowEditing"
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
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Name">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Number" HeaderText="Number">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Address" HeaderText="Address">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Telephone" HeaderText="Telephone">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Email" HeaderText="Email">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OpeningBalance" HeaderText="O. Balance">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CustomerStatus" HeaderText="Active Status">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:BoundField>
                                           <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                ShowEditButton="True" EditImageUrl="~/Images/edit.png" ButtonType="Image">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:CommandField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                    <HeaderTemplate>
                        Retailer Sale Report<br />
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtfDate"  placeholder="From Date" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                        <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                            TargetControlID="txtfDate" />
                     <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtfDate"
                            ErrorMessage="Date is required" Display="Dynamic" runat="server" />--%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceiveDate" placeholder="To Date" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                        <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                            Enabled="True" TargetControlID="txtReceiveDate" />
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtReceiveDate"
                            ErrorMessage="Date is required" Display="Dynamic" runat="server" />--%>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRetailerID" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="Cascadingdropdown1" runat="server" Category="Retailer" TargetControlID="ddlRetailerID"
                                            LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindRetailer"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblRetailerMsg" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                        <asp:Button ID="btnReport"
                                            runat="server" Text="View Report" onclick="btnReport_Click" CssClass="btn btn-success btn-sm"/>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
