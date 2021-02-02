<%@ Page Title="Barcode" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Barcode.aspx.cs" Inherits="Pages_Barcode" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerBarcode" runat="server" ActiveTabIndex="0" 
                Width="100%">
                <asp:TabPanel ID="Panel1" runat="server" HeaderText="Accessories Barcode List">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Brand:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchBrand" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="Cascadingdropdown1" runat="server" Category="Category"
                                        TargetControlID="ddlSearchBrand" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                        ServiceMethod="BindCategory1" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Model:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchModel" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Model" TargetControlID="ddlSearchModel"
                                        ParentControlID="ddlSearchBrand" LoadingText="Loading Model..." PromptText="--Please Select--"
                                        ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Color/Description:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchColor" runat="server" CssClass="DropDownList" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlDescription_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown3" runat="server" Category="Description"
                                        TargetControlID="ddlSearchColor" ParentControlID="ddlSearchModel" LoadingText="Loading Color..."
                                        PromptText="--Please Select--" ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx"
                                        Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td align="right">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button" OnClick="cmdSearch_Click"
                                        CausesValidation="False" />&nbsp;
                                    <asp:Button ID="cmdPreview" runat="server" Text="Preview" CssClass="Button" CausesValidation="False"
                                        OnClick="cmdPreview_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:GridView ID="gvBarcode" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" PageSize="25" AllowPaging="True" DataKeyNames="OID" BorderColor="#CCCCCC"
                                        BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        OnRowDeleting="gvBarcode_RowDeleting" OnPageIndexChanging="gvBarcode_PageIndexChanging"
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
                                            <asp:BoundField DataField="WGPG_NAME" HeaderText="Brand">
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
                                            <asp:BoundField DataField="BARCODE" HeaderText="Barcode">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                ShowEditButton="True" EditImageUrl="~/Images/edit.png" ButtonType="Image">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:CommandField>
                                            <asp:TemplateField HeaderText="Delete" Visible="False">
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
                <asp:TabPanel ID="Panel2" runat="server" HeaderText="Generate Barcode">
                    <ContentTemplate>
                        <table cellpadding="2" border="0" width="100%">
                            <tr>
                                <td align="center" style="font-weight: bolder">
                                    Create Accessories Barcode
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblmessagedtail" runat="server" Style="color: red; font-size: 40;
                                        font-weight: bold;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="width: 20px">
                                    <asp:Label ID="Label1" runat="server" Style="font-weight: bold" Text="Brand:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProductCategoryId" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="Cascadingdropdown8" runat="server" Category="Category"
                                        TargetControlID="ddlProductCategoryId" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                        ServiceMethod="BindCategory1" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20px">
                                    <asp:Label ID="Label2" runat="server" Style="font-weight: bold" Text="Model:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlModel" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown9" runat="server" Category="Model" TargetControlID="ddlModel"
                                        ParentControlID="ddlProductCategoryId" LoadingText="Loading Model..." PromptText="--Please Select--"
                                        ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20px">
                                    <asp:Label ID="Label4" runat="server" Style="font-weight: bold" Text="Color/Description:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDescription" runat="server" CssClass="DropDownList" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlDescription_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown10" runat="server" Category="Description"
                                        TargetControlID="ddlDescription" ParentControlID="ddlModel" LoadingText="Loading Color..."
                                        PromptText="--Please Select--" ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx"
                                        Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td valign="bottom">
                                    <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Barcode:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBarcode" runat="server" CssClass="TextBox"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtBarcode"
                                        ErrorMessage="Barcode is required" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td valign="bottom" align="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="Button_VariableWidth" OnClick="btnSave_Click"
                                        Text="Barcode Generate" OnClientClick="this.disabled=true;" UseSubmitBehavior="False" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <p>
                                        &nbsp;</p>
                                </td>
                            </tr>
                            <tr>
                                <td valign="bottom" colspan="2">
                                    <asp:GridView ID="gvT_WGPG" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        DataKeyNames="OID" OnRowDeleting="gvT_WGPG_RowDeleting" EmptyDataText="No rows returned"
                                        Width="100%">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:BoundField DataField="OID" HeaderText="OID" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CategotyID" HeaderText="CategotyID" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WGPG_NAME" HeaderText="Brand">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubcategoryID" HeaderText="SubcategoryID" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubCategoryName" HeaderText="Model">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DescriptionID" HeaderText="DescriptionID" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Description" HeaderText="Color/Description">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BARCODE" HeaderText="Barcode">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
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
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
