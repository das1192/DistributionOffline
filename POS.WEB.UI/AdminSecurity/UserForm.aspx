<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserForm.aspx.cs" Inherits="SALESANDINVENTORY.Web.UI.UserForm" Title="User"
    EnableViewStateMac="false" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContainerSystemUser" runat="server" ActiveTabIndex="0" 
                Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="System User List">
                    <HeaderTemplate>
                        System User List
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="50px" width="100%">  
                              <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Branch"></asp:Label>
                                </td>                              
                                <td align="left">
                                    <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                        LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>                                    
                                </td>
                                <td align="right">
                                    <asp:Button ID="cmdSearch" runat="server" CssClass="Button_VariableWidth" CausesValidation="False"
                                        Text="Search" onclick="cmdSearch_Click"/> 
                                </td>
                            </tr>
                            <tr><td>&nbsp;</td></tr>
                                                
                            <tr>
                                <td colspan ="3">
                                    <asp:GridView ID="gvUser" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="Id"
                                        EmptyDataText="No Data Found" ShowHeaderWhenEmpty="True" OnRowDeleting="gvUser_RowDeleting"
                                        Width="100%" OnRowEditing="gvUser_RowEditing">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="ID" Visible="False" />
                                            <asp:BoundField DataField="UserId" HeaderText="User ID" Visible="False" />
                                            <asp:BoundField DataField="CCOM_NAME" HeaderText="Branch" />
                                            <asp:BoundField DataField="UserFullName" HeaderText="Full Name" />
                                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                            <asp:BoundField DataField="Password" HeaderText="Password" />

                                              <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                ShowEditButton="True" EditImageUrl="~/Images/edit.png" ButtonType="Image">
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
                                        <EmptyDataRowStyle Font-Bold="True" HorizontalAlign="Center" />
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#669900" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <AlternatingRowStyle BackColor="AliceBlue" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tpEditor" runat="server" HeaderText="Add/Edit System User">
                    <ContentTemplate>
                        <table  border="0" width="100%">
                            <tr>
                                <td align="center" style="font-weight: bold" colspan="3">
                                    System User Information Entry
                                    <hr style="color: Green;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="txtUserId" runat="server" CssClass="TextBox" Visible="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBranchname" runat="server" Text="Branch Name"></asp:Label>
                                </td>                              
                                <td colspan="2" align="left">
                                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch1" TargetControlID="ddlBranch"
                                        LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFullName" runat="server" Text="Full Name"></asp:Label>
                                </td>                               
                                <td align="left">
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="TextBox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFullName"
                                        ErrorMessage="User full name is required" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label>
                                </td>                                
                                <td align="left">
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="TextBox" OnTextChanged="txtUserName_TextChanged"
                                        AutoPostBack="True" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvUserName" ControlToValidate="txtUserName" ErrorMessage="User Name is required"
                                        Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                                </td>                               
                                <td align="left">
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="TextBox"  />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" ErrorMessage="Password is required"
                                        Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Label>
                                </td>                               
                                <td align="left">
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="TextBox" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" ControlToValidate="txtConfirmPassword"
                                        ErrorMessage="ConfirmPassword is required" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td >&nbsp;</td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" CssClass="Button_VariableWidth" OnClick="btnSave_Click"
                                        Text="Save"/> &nbsp;&nbsp;
                                   
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="Button_VariableWidth"
                                        OnClick="btnCancel_Click" Text="Cancel" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:HiddenField ID="lblOID" runat="server" />
                                </td>
                            </tr>
                        </table>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
