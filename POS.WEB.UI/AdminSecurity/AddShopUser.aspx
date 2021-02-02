<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddShopUser.aspx.cs" Inherits="AdminSecurity_AddShopUser" 
Title="Add Shop User" EnableViewStateMac="false" EnableEventValidation="false"
%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">

    
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

     
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContainerSystemUser" runat="server" ActiveTabIndex="0" 
                Width="100%" CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="System User List">
                   <HeaderTemplate>
                        User List
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="50px" width="100%">  
                              
                                                
                            <tr>
                                <td>
                                    <asp:GridView ID="gvUser" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="Id"
                                        EmptyDataText="No Data Found" ShowHeaderWhenEmpty="True" 
                                        Width="100%" OnRowEditing="gvUser_RowEditing" OnRowDeleting="gvUser_RowDeleting">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="ID" Visible="False" />
                                            <asp:BoundField DataField="UserId" HeaderText="User ID" Visible="False" />
                                            <asp:BoundField DataField="ShopName" HeaderText="Shop Name" />
                                            <asp:BoundField DataField="UserFullName" HeaderText="Full Name" />
                                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                            <asp:BoundField DataField="Password" HeaderText="Password" />
                                            <asp:BoundField DataField="StuffID" HeaderText="Stuff ID" />
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
                <asp:TabPanel ID="tpEditor" runat="server" HeaderText="Add/Edit Shop User">
                  
                    <ContentTemplate>
                        <table border="0" width="70%">
                            
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="txtUserId" runat="server" CssClass="TextBox" Visible="False" 
                                        ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblShopname" runat="server" Text="Shop Name"></asp:Label>
                                </td> 
                                <td colspan="2" align="left">
                                    <asp:DropDownList ID="DropDownListShopName" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                     <asp:CascadingDropDown ID="NEW" runat="server" category="New" 
                                        TargetControlID="DropDownListShopName" LoadingText="Loading Shop..." PromptText="--Please Select--"
                                      ServiceMethod ="BindShop" ServicePath ="~/DropdownWebService.asmx" 
                                        >
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
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="TextBox"
                                        AutoPostBack="True" ontextchanged="txtUserName_TextChanged1" />
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
                                <td align="left">
                                    &nbsp;<asp:Label ID="lblMobileNumber" runat="server" Text="Mobile Number"></asp:Label>
                                </td>
                                
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="TextBox" MaxLength="11"
                                            onkeypress="return isNumberKey(event)" />&nbsp;<br />
                                                                            
                                    </div>
                                </td>
                                <td>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtMobileNumber"
                                            ErrorMessage="Mobile No is required" Display="Dynamic" runat="server" />   
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Alternative Mobile Number"></asp:Label>
                                </td>
                                
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtAlternativeMobileNo" runat="server" CssClass="TextBox" MaxLength="11"
                                            onkeypress="return isNumberKey(event)" />
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label3" runat="server" Text="E-mail Address"></asp:Label>
                                </td>
                               
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="TextBox" />
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label4" runat="server" Text="National ID NO" ></asp:Label>
                                </td>
                               
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtnationalid" runat="server" CssClass="TextBox" />
                                    </div>
                                </td>
                                <td>
                                </td>
                            </tr><tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label5" runat="server" Text="Address" ></asp:Label>
                                </td>
                               
                                <td>
                                    <div>
                                        <asp:TextBox ID="txtaddress" runat="server" CssClass="TextBox" />
                                    </div>
                                </td>
                                <td><asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtaddress"
                                            ErrorMessage="Address is required" Display="Dynamic" runat="server" />  
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click"
                                        Text="Save"/> &nbsp;&nbsp;
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                           <tr>
                                <td colspan="3" align="left">
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