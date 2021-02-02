<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductPrice.aspx.cs" Inherits="Pages_ProductPrice" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerProductPrice" runat="server" ActiveTabIndex="0" Width="100%">
                <asp:TabPanel ID="Panel1" runat="server" HeaderText="Product Price">
                    <ContentTemplate>
                        <table width="100%">                           
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>
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
                            <td>&nbsp;</td>
                             <td colspan="5">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button_VariableWidth"
                                        OnClick="cmdSearch_Click" CausesValidation="False" />&nbsp;
                                    <asp:Button ID="cmdPreview" runat="server" Text="Preview" CssClass="Button_VariableWidth"
                                        CausesValidation="False" OnClick="cmdPreview_Click" />
                                </td>
                            </tr>


                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="gvProductPrice" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" PageSize="25" AllowPaging="True" DataKeyNames="OID" BorderColor="#CCCCCC"
                                        BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        OnRowDataBound="gvProductPrice_RowDataBound" OnRowDeleting="gvProductPrice_RowDeleting"
                                        OnRowEditing="gvProductPrice_RowEditing" OnPageIndexChanging="gvProductPrice_PageIndexChanging"
                                        Width="100%">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <PagerStyle CssClass="pgr" />
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
                                            <asp:BoundField DataField="WGPG_NAME" HeaderText="Category">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubCategoryName" HeaderText="Model">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Description" HeaderText="Color/Description">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PurchasePrice" HeaderText="SES Price">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SalePrice" HeaderText="MRP">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                ShowEditButton="True" EditImageUrl="~/Images/edit.png" ButtonType="Image">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:CommandField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        Text="Delete" OnClientClick="return confirm('Are you sure to delete?')"><img alt="Delete" src="../Images/Delete.gif" /></asp:LinkButton>
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
                </asp:TabPanel>
                <asp:TabPanel ID="Panel2" runat="server" HeaderText="Add/Edit Product Price">
                    <ContentTemplate>
                        <table cellpadding="2" border="0" width="100%">
                            <tr>
                                <td align="center" style="font-weight: bolder">
                                    Product Price
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="2" border="0" width="100%">
                            <tr id="trCategory" runat="server">
                                <td id="Td1" align="left" runat="server">
                                    &nbsp;<asp:Label ID="lblProductCategoryId" runat="server" Text="Category:"></asp:Label>
                                </td>
                                <td id="Td2" colspan="2" runat="server">
                                    <div>
                                        <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown4" runat="server" Category="Category"
                                            TargetControlID="ddlProductCategory" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                            ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trModel" runat="server">
                                <td id="Td3" align="left" runat="server">
                                    &nbsp;<asp:Label ID="Label4" runat="server" Text="Model:"></asp:Label>
                                </td>
                                <td id="Td4" colspan="2" runat="server">
                                    <div>
                                        <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown5" runat="server" Category="Model" TargetControlID="ddlSubCategory"
                                            ParentControlID="ddlProductCategory" LoadingText="Loading Model..." PromptText="--Please Select--"
                                            ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trDescription" runat="server">
                                <td id="Td5" align="left" runat="server">
                                    &nbsp;<asp:Label ID="Label3" runat="server" Text="Color/Description:"></asp:Label>
                                </td>
                                <td id="Td6" colspan="2" runat="server">
                                    <div>
                                        <asp:DropDownList ID="ddlDescription" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown6" runat="server" Category="Description"
                                            TargetControlID="ddlDescription" ParentControlID="ddlSubCategory" LoadingText="Loading Color..."
                                            PromptText="--Please Select--" ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx"
                                            Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="lblCOST_PRICE" runat="server" Text="SES Price:"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtCOST_PRICE" runat="server" CssClass="TextBox" onkeypress="return isNumberKey(event)" /></div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtCOST_PRICE"
                                        ErrorMessage="Cost Price is required" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label2" runat="server" Text="MRP:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSalePrice" runat="server" CssClass="TextBox" onkeypress="return isNumberKey(event)" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtSalePrice"
                                        ErrorMessage="Sell Price is required" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" Text="Save     " CssClass="Button_VariableWidth"
                                        OnClick="btnSave_Click" CausesValidation="False" OnClientClick="this.disabled=true;"
                                        UseSubmitBehavior="False" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel  " CssClass="Button_VariableWidth"
                                        OnClick="btnCancel_Click" CausesValidation="False" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    <asp:HiddenField ID="lblOID" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
