<%@ Page Title="Description/Color" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Description.aspx.cs" Inherits="Pages_Description"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function Closepopup() {
            $('#myModal').modal('close');

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
            <asp:TabContainer ID="ContainerColor" runat="server" ActiveTabIndex="0" Width="100%"
                CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Description List">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                
                                <asp:Label ID="Label2" runat="server" Text="Product Category"></asp:Label>
                                    </td>
                                <td>
                                
                                    <asp:DropDownList ID="ddlSearchProductCategory" runat="server" 
                                        CssClass="form-control" 
                                        >
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="ddlSearchProductCategoryCascading" runat="server" 
                                        Category="Category" Enabled="True" LoadingText="Loading Categories..." 
                                        PromptText="--Please Select--" ServiceMethod="BindCategory" 
                                        ServicePath="~/DropdownWebService.asmx" 
                                        TargetControlID="ddlSearchProductCategory">
                                    </asp:CascadingDropDown>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Brand"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchProductSubCategory" runat="server" 
                                        CssClass="form-control" 
                                        >
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="ddlSearchProductSubCategoryCascading" runat="server" 
                                        Category="Model" Enabled="True" LoadingText="Loading Brand..." 
                                        ParentControlID="ddlSearchProductCategory" PromptText="--Please Select--" 
                                        ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" 
                                        TargetControlID="ddlSearchProductSubCategory">
                                    </asp:CascadingDropDown>
                                </td>
                                
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                            <td>
                                    <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                            
                            </td>
                            <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="--Please Select--" Selected="True">--Please Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                            
                            
                            </td>
                            
                                <td>
                                    <asp:Button ID="cmdSearch" runat="server" CausesValidation="False" 
                                        CssClass="btn btn-success" OnClick="cmdSearch_Click" Text="Search" />
                                    <asp:Button ID="cmdExport" runat="server" CausesValidation="False" 
                                        CssClass="btn btn-warning" OnClick="cmdExport_Click" 
                                        Text="Export To Excel" />
                                    <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Button" 
                                        Visible="False" /></td>
                            </tr>
                            
                            
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="gvColor" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" PageSize="25" AllowPaging="True" DataKeyNames="OID" BorderColor="#CCCCCC"
                                        BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        OnRowDeleting="gvColor_RowDeleting" OnRowEditing="gvColor_RowEditing" OnPageIndexChanging="gvColor_PageIndexChanging"
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
                                            <asp:BoundField DataField="SubCategoryName" HeaderText="Brand">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Description" HeaderText="Description">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SESPrice" HeaderText="SES Price">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MRP" HeaderText="MRP">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                ShowEditButton="True" EditImageUrl="~/Images/edit.png" ButtonType="Image">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:CommandField>
                                            <asp:TemplateField HeaderText="Modify">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        Text="Inactive" OnClientClick="return confirm('Are you sure to Inactive?')"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkbtnActive" runat="server" CausesValidation="false" 
                                                        Text="Active" OnClientClick="return confirm('Are you sure to Active?')" 
                                                        onclick="lnkbtnActive_Click"></asp:LinkButton>
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
                <asp:TabPanel ID="tpEditor" runat="server" HeaderText="Add/Edit/Description">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            <tr>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblProductCategoryId" runat="server" Text="Category"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProductCategoryId" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CAS_ddlProductCategoryId" runat="server" Category="Category"
                                        TargetControlID="ddlProductCategoryId" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                        ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                    <asp:Button ID="btnSubmit" runat="server" Text="Add New" CssClass="btn btn-primary"
                                        OnClick="addnew_Click" CausesValidation="False" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Brand"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CAS_ddlSubCategory" runat="server" Category="Model" TargetControlID="ddlSubCategory"
                                        ParentControlID="ddlProductCategoryId" LoadingText="Loading Model..." PromptText="--Please Select--"
                                        ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                    <asp:Button ID="Button2" runat="server" Text="Add New" CssClass="btn btn-primary"
                                        OnClick="addnewsub_Click" CausesValidation="False" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblColor" runat="server" Text="Description"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="TextBox" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="SES Price"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSESPrice" runat="server" CssClass="TextBox" onkeypress="return isNumberKey(event)" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="MRP"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMRP" runat="server" CssClass="TextBox" onkeypress="return isNumberKey(event)" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
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
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    <asp:HiddenField ID="lblOID" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                            &times;
                                        </button>
                                        <h4 class="modal-title" id="cafeId">
                                            Add Category
                                        </h4>
                                    </div>
                                    <div class="modal-body">
                                        <table cellpadding="2" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCategoryName" runat="server" Text="Category Name:"></asp:Label>
                                                </td>
                                                <td>
                                                    <span style="color: red;">*</span>
                                                </td>
                                                <td>
                                                
                                                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="TextBox"/>
                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="Save" CssClass="btn btn-default" OnClick="btncatSave_Click"
                                                        OnClientClick="this.disabled=true;" UseSubmitBehavior="False" />&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal fade" id="myModal2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"  aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                            &times;
                                        </button>
                                        <h4 class="modal-title" id="H1">
                                            Add Sub Category
                                        </h4>
                                    </div>
                                    <div class="modal-body">
                                        <table cellpadding="2" border="0">
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label6" runat="server" Text="Product Category"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="CascadingDropDownsub2" runat="server" CssClass="DropDownList">
                                                    </asp:DropDownList>
                                                    <asp:CascadingDropDown ID="CascadingDropDownsub244" runat="server" Category="Category"
                                                        TargetControlID="CascadingDropDownsub2" LoadingText="Loading Categories..." PromptText="--Please Select--"
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
                                                    <asp:TextBox ID="txtSubCategory22" runat="server" CssClass="textbox" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button3" runat="server" Text="Save" CssClass="Button_VariableWidth"
                                                        OnClick="btnSavesub_Click" OnClientClick="this.disabled=true;" 
                                                        UseSubmitBehavior="False" Width="100px" />&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
