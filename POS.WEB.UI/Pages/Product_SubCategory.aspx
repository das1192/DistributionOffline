<%@ Page Title="Brand" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Product_SubCategory.aspx.cs" Inherits="Pages_Product_SubCategory" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...</ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContainerModel" runat="server" ActiveTabIndex="1" 
                Width="100%" CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Brand List">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Product Category  "></asp:Label><asp:DropDownList
                                        ID="ddlSearchProductCategory" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CAS_ddlSearchProductCategory" runat="server" Category="Category"
                                        TargetControlID="ddlSearchProductCategory" LoadingText="Loading Categories..."
                                        PromptText="--Please Select--" ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx"
                                        Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>

                                 
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>

                            <tr>
                                  <td align="left">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="btn btn-success"
                                        OnClick="cmdSearch_Click" CausesValidation="False" Width="100px" />&nbsp;&nbsp;
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="gvModel" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" PageSize="25" AllowPaging="True" DataKeyNames="OID" BorderColor="#CCCCCC"
                                        BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        OnRowDeleting="gvModel_RowDeleting" OnRowEditing="gvModel_RowEditing" OnPageIndexChanging="gvModel_PageIndexChanging"
                                        Width="100%">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
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
                                            <asp:BoundField DataField="SubCategoryName" HeaderText="Brand">
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
                                                        Text="Delete" OnClientClick="return confirm('Are you sure to delete?')"><img alt="Delete" src="../Images/Delete.gif" /></asp:LinkButton></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pgr" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>

                        
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Add/Edit Brand">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            <tr>
                                <td colspan="3">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblProductCategoryId" runat="server" Text="Product Category" ></asp:Label>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlProductCategoryId" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CAS_ddlProductCategoryId" runat="server" Category="Category"
                                        TargetControlID="ddlProductCategoryId" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                        ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblModel" runat="server" Text="Brand"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSubCategory" runat="server" CssClass="textbox" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                </td>
                            </tr>
                            


                              

                            <tr>
                                <td colspan="2">
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success"
                                        OnClick="btnSave_Click" OnClientClick="this.disabled=true;" 
                                        UseSubmitBehavior="False" Width="100px" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                        CssClass="btn btn-warning" OnClick="btnCancel_Click" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblMessage" runat="server" Font-Size="Large" ForeColor="Red" ></asp:Label><asp:HiddenField ID="lblOID"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
