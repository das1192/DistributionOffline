<%@ Page Title="Staff Entry" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="StuffInformation.aspx.cs" Inherits="StuffInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
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
            <asp:TabContainer ID="ContainerStaffInformation" runat="server" ActiveTabIndex="0"
                Width="100%" CssClass="fancy fancy-green">
                <asp:TabPanel ID="Panel1" runat="server" HeaderText="Sales Executive(SE) List">
                    <ContentTemplate>
                        <table width="100%">

                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label5" runat="server" Text="Branch"></asp:Label>
                                </td>                               
                                <td>                                    
                                    <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                        LoadingText="Loading Shop..." PromptText="--Please Select--" ServiceMethod="BindShop"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>                                    
                                </td>
                                <td align="right">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" 
                                        CssClass="btn btn-default" onclick="cmdSearch_Click" 
                                        CausesValidation="False" />
                                   
                                </td>
                            </tr>
                            
                           
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="gvStaffInformation" runat="server" AutoGenerateColumns="False"
                                        GridLines="None" CssClass="mGrid" DataKeyNames="OID" BorderColor="#CCCCCC" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned" Width="100%"
                                        OnRowDeleting="gvStaffInformation_RowDeleting" OnRowEditing="gvStaffInformation_RowEditing">
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
                                            <asp:BoundField DataField="StuffID" HeaderText="SE ID">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Name">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AlternativeMobileNo" HeaderText="Mobile Number(Alt.)">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EMailAddress" HeaderText="Email">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AlternativeEMailAddress" HeaderText="Email(Alt.)">
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
                <asp:TabPanel ID="Panel2" runat="server" HeaderText="Add/Edit SE Information">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblBranch" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch2" TargetControlID="ddlBranch"
                                            LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindShop"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStuffID" runat="server" Text="SE ID:"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtStuffID" runat="server" CssClass="TextBox" />
                                        &nbsp;<br />
                                        <asp:RequiredFieldValidator ID="rfvtxtStuffID" ControlToValidate="txtStuffID" ErrorMessage="SE ID is required"
                                            Display="Dynamic" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label1" runat="server" Text="SE Name:"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtStuffName" runat="server" CssClass="TextBox" />
                                        &nbsp;<br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtStuffName"
                                            ErrorMessage="Stuff Name is required" Display="Dynamic" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="lblMobileNumber" runat="server" Text="Mobile Number"></asp:Label>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="TextBox" MaxLength="11"
                                            onkeypress="return isNumberKey(event)" />&nbsp;<br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtMobileNumber"
                                            ErrorMessage="Mobile No is required" Display="Dynamic" runat="server" />                                        
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Alternative Mobile Number"></asp:Label>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtAlternativeMobileNo" runat="server" CssClass="TextBox" MaxLength="11"
                                            onkeypress="return isNumberKey(event)" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label3" runat="server" Text="E-mail Address"></asp:Label>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="TextBox" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label4" runat="server" Text="Alternative E-mail Address"></asp:Label>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtAlternativeEmailAddress" runat="server" CssClass="TextBox" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default"
                                        OnClick="btnSave_Click" CausesValidation="true" />&nbsp;&nbsp;
                                    <asp:Button ID="cmdCancel" runat="server" Text="Cancel  " CssClass="btn btn-default"
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
