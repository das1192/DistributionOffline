<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmInterPOSTransfer.aspx.cs" Inherits="Pages_frmInterPOSTransfer" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="tContT_PROD" runat="server" ActiveTabIndex="0" 
                Width="100%">
                
                <asp:TabPanel ID="tpEditor" runat="server" HeaderText="Add/Edit Product">
                    <ContentTemplate>
                        <table cellpadding="2" border="0" width="100%">
                            <tr>
                                <td align="center" style="font-weight: bolder">
                                    Product Transfer To SIS
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="2" border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblProductCategoryId" runat="server" Text="Brand"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProductCategoryId" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown4" runat="server" Category="Category"
                                        TargetControlID="ddlProductCategoryId" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                        ServiceMethod="BindCategorySIS" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Model"></asp:Label>
                                </td>
                                <td id="Td1" runat="server">
                                    <div>
                                        <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown5" runat="server" Category="Model" TargetControlID="ddlSubCategory"
                                            ParentControlID="ddlProductCategoryId" LoadingText="Loading Model..." PromptText="--Please Select--"
                                            ServiceMethod="BindModelSIS" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Color/Description"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDescription" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown6" runat="server" Category="Description"
                                        TargetControlID="ddlDescription" ParentControlID="ddlSubCategory" LoadingText="Loading Color..."
                                        PromptText="--Please Select--" ServiceMethod="BindDescriptionSIS" ServicePath="~/DropdownWebService.asmx"
                                        Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown7" runat="server" Category="Branch1"
                                        TargetControlID="ddlBranch" ParentControlID="ddlProductCategoryId" LoadingText="Loading Branch..."
                                        PromptText="--Please Select--" ServiceMethod="BindBranchSIS" ServicePath="~/DropdownWebService.asmx"
                                        Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                          
                            <tr>
                                <td align="left">
                                    &nbsp;<asp:Label ID="Label5" runat="server" Text="Bar Code/IME:"></asp:Label>
                                </td>
                                <td>
                                    <textarea id="txtBarcode" cols="30" rows="10" runat="server" class="Text_Area1"></textarea>
                                </td>
                            </tr>
                         
                            <tr>
                                <td>
                                </td>
                                <td align="left">
                                    <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="Button_VariableWidth"
                                        OnClick="cmdAdd_Click" CausesValidation="False" Width="100px"/>&nbsp;&nbsp;
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Button_VariableWidth"
                                        OnClick="btnSave_Click" CausesValidation="False" OnClientClick="this.disabled=true;"
                                        UseSubmitBehavior="False" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan ="2">
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvT_BarCode" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        EmptyDataText="No rows returned" Width="100%" >
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PROD_WGPG" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPROD_WGPG" runat="server" Text='<%#Bind("PROD_WGPG") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Brand">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("Category") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PROD_SUBCATEGORY" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPROD_SUBCATEGORY" runat="server" Text='<%#Bind("PROD_SUBCATEGORY") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Model">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubCategory" runat="server" Text='<%#Bind("SubCategory") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PROD_DES" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPROD_DES" runat="server" Text='<%#Bind("PROD_DES") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Color/Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Barcode/IMEI">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBarCode" runat="server" Text='<%#Bind("BarCode") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BranchID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranchID" runat="server" Text='<%#Bind("BranchID") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Branch">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranch" runat="server" Text='<%#Bind("Branch") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SES Price">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCostPrice" runat="server" Text='<%#Bind("CostPrice") %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MRP">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSellPrice" runat="server" Text='<%#Bind("SellPrice") %>'></asp:Label></ItemTemplate>
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
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="SIS Transfer List">
                    <ContentTemplate>
                    <table cellpadding="2" border="0" width="100%">
                            <tr>
                                <td align="center" style="font-weight: bolder">
                                     SIS Transfer List
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                   <asp:Label ID="Label2" runat="server" Text="From Date "></asp:Label>
                                </td>
                                <td>
                                   <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate" />
                                </td>
                                <td>
                                      <asp:Label ID="Label1" runat="server" Text="To Date "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                                        Enabled="True" TargetControlID="txtToDate" />
                                </td>
                                <td>
                                  
                                </td>
                                <td>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                   
                                </td>
                                <td>
                                   
                                </td>
                                <td align="left">
                                    
                                </td>
                                <td>
                                   
                                </td>
                                <td align="left">
                                    
                                </td>
                                <td>
                                                                    
                                </td>
                            </tr>
                             <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                               <td>
                                    
                                </td>
                                <td>
                                    
                                </td>
                                <td>
                                    
                                </td>
                                
                                <td align="right" colspan="4">
                                    <asp:Button ID="cmdSISSearch" runat="server" Text="Search" CssClass="Button_VariableWidth"
                                        CausesValidation="False" OnClick="cmdSISSearch_Click" />
                                </td>
                                <td>
                                    
                                   <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="Button_VariableWidth"
                                    OnClick="btnPreview_Click" CausesValidation="true" /> 
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridViewsislist" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                         EmptyDataText="No rows returned" Width="100%">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CCOM_NAME" HeaderText="Branch">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
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
                                            <asp:BoundField DataField="Barcode" HeaderText="IMEI/Barcode">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                
                                              <asp:BoundField DataField="Remarks" HeaderText="Transferred Branch">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>    
                                              <asp:BoundField DataField="Particulars" HeaderText="Particulars">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>                                          
                                             <asp:BoundField DataField="IDAT" HeaderText="Date">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            
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

