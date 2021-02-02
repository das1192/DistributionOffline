<%@ Page Title="Discount Reference" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DiscountReference.aspx.cs" Inherits="Pages_DiscountReference" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" Runat="Server">
 <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContainerDiscountReference" runat="server" 
                ActiveTabIndex="0" Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Discount Reference List">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Discount Type"></asp:Label>
                                    <asp:DropDownList ID="ddlSearchDiscountType" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CAS_ddlSearchDiscountType" runat="server" Category="DiscountType"
                                        TargetControlID="ddlSearchDiscountType" LoadingText="Loading Discount Type..."
                                        PromptText="--Please Select--" ServiceMethod="BindDiscountType" ServicePath="~/DropdownWebService.asmx"
                                        Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td align="right">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button_VariableWidth" OnClick="cmdSearch_Click"
                                        CausesValidation="False" />
                                </td>
                            </tr>
                           
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvModel" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" PageSize="25" AllowPaging="True" DataKeyNames="OID" BorderColor="#CCCCCC"
                                        BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        OnRowDeleting="gvModel_RowDeleting" OnRowEditing="gvModel_RowEditing"
                                        OnPageIndexChanging="gvModel_PageIndexChanging" Width="100%">
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
                                            <asp:BoundField DataField="DiscountType" HeaderText="Discount Type">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Reference" HeaderText="Reference">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Email" HeaderText="Email">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
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
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Add/Edit Discount Reference">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            <tr>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblProductCategoryId" runat="server" Text="Discount Type"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CAS_ddlDiscountType" runat="server" Category="DiscountType"
                                        TargetControlID="ddlDiscountType" LoadingText="Loading Discount Type..." PromptText="--Please Select--"
                                        ServiceMethod="BindDiscountType" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblModel" runat="server" Text="Discount Reference"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReference" runat="server" CssClass="TextBox" />
                                    <asp:RequiredFieldValidator ID="rfvtxtSubCategory" ControlToValidate="txtReference"
                                        ErrorMessage="Reference is required" Display="Dynamic" runat="server" />
                                </td>
                            </tr>


                             <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
                                </td>                               
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBox" />                               
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Button_VariableWidth" OnClick="btnSave_Click"
                                        OnClientClick="this.disabled=true;" UseSubmitBehavior="false" Enabled="true" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                        CssClass="Button_VariableWidth" OnClick="btnCancel_Click" />
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

