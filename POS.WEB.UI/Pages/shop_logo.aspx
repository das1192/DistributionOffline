<%@ Page Title="Add Shop Logo" MasterPageFile="~/MasterPage.master" Language="C#" AutoEventWireup="true" CodeFile="shop_logo.aspx.cs" Inherits="Pages_shop_logo" %>

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
            <asp:TabContainer ID="ContainerBankInfo" runat="server" ActiveTabIndex="0"
                Width="100%" CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Shop List">
                    <HeaderTemplate>
                        Shop List
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">                           
                            <tr>
                                <td>
                                    <asp:GridView ID="gvShopLOGO" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        DataKeyNames="OID" EmptyDataText="No rows returned" OnRowDeleting="ShopLOGO_RowDeleting"
                                         Width="100%">
                                        <AlternatingRowStyle CssClass="alt" />
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
                                            

                                          <asp:TemplateField HeaderText="Image">  
                                            <ItemTemplate>  
                                                 <asp:Image ID="ImgBookPic" runat="server" Height="91px" Width="220px" />
                                            </ItemTemplate>  
                                        </asp:TemplateField>                                  

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
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Add Shop Logo">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Shop Logo"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    
                                  
                                    <asp:FileUpload ID="fileupload" runat="server"/>
                                    <asp:RegularExpressionValidator ID="RegExValFileUploadFileType" runat="server"
                        ControlToValidate="fileupload"
                        ErrorMessage="Only .jpg Files are allowed" Font-Bold="True"
                        Font-Size="Medium"
                        ValidationExpression="(.*?)\.(jpg|jpeg)$"></asp:RegularExpressionValidator>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                        CssClass="btn btn-default" OnClientClick="this.disabled=true;" 
                                        UseSubmitBehavior="False" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                        CssClass="btn btn-default" OnClick="btnCancel_Click" />
                                        
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
