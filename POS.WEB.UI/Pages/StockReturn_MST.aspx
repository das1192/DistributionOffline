<%@ Page Title="Stock Return" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="StockReturn_MST.aspx.cs" Inherits="Pages_StockReturn_MST"
    EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerStockReturn" runat="server" ActiveTabIndex="1" 
                Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Stock Return List">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtToDate" />
                                </td>
                                <td align="right">
                                    <asp:Button ID="Button2" runat="server" Text="Search" CssClass="Button" CausesValidation="False"
                                        OnClick="Button1_Click" />&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Search Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbtNotReceived" runat="server" Text="Not Received" GroupName="Software"
                                        Checked="True" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbtReceived" runat="server" Text="Received" GroupName="Software" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                </td>
                            </tr>
                          
                            <tr>
                                <td align="left" colspan="5" style="font-weight: bolder">
                                    <asp:Label ID="lblMessage1" Style="font-weight: bold" runat="server"></asp:Label>
                                </td>
                            </tr>
                               <tr>
                                <td colspan="5">
                                    <asp:GridView ID="gvStockReturnList" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        DataKeyNames="StockReturnID" EmptyDataText="No rows returned" Width="100%" OnRowCommand="StockReturn_Details">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="StockReturnID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStockReturnID" runat="server" Text='<%#Bind("StockReturnID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="Stock Return No">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkView" CommandArgument='<%#Eval("StockReturnID") %>'
                                                        Text='<%# Eval("StockReturnNo") %>' CommandName="ItemDetails"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="IDAT" HeaderText="Transfer Date" DataFormatString="{0:yyyy-MM-dd}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>                                                                                      

                                            <asp:BoundField DataField="FromStoreID" HeaderText="Transfer From">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ToStoreID" HeaderText="Transfer To">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                                                                       
                                              <asp:TemplateField HeaderText="Received">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Bind("ApprovedStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>


                                            <asp:BoundField DataField="ReferenceBy" HeaderText="Reference By">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Preview">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPreview" Text="Preview" runat="server" CommandArgument='<%# Eval("StockReturnID")%>'
                                                        CommandName="Preview"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CommandArgument='<%# Eval("StockReturnID")%>'
                                                        CommandName="Delete"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="5" style="font-weight: bolder">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="5" style="font-weight: bolder">
                                    Stock Return Product Details
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        EmptyDataText="No rows returned" Width="100%" OnRowDeleting="GridView2_RowDeleting"
                                        DataKeyNames="StockReturnDetailID">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="StockReturnDetailID" HeaderText="StockReturnDetailID"
                                                Visible="False" />
                                            
                                            <asp:BoundField DataField="StockReturnNo" HeaderText="Stock Return No">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                        
                                            <asp:BoundField DataField="WGPG_NAME" HeaderText="Brand">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                        
                                             <asp:BoundField DataField="SubCategoryName" HeaderText="Model">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            
                                             <asp:BoundField DataField="Description" HeaderText="Color/Description">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                        
                                             <asp:BoundField DataField="Barcode" HeaderText="IMEI/Barcode">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                        
                                            <asp:BoundField DataField="RQty" HeaderText="Qty">
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
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <RowStyle ForeColor="#000066" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Add Product">
                    <ContentTemplate>
                        <fieldset style="border: 1px Solid Black;">
                            <legend>Add Product</legend>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblBarcode" runat="server" Text="Barcode"></asp:Label>
                                        <asp:TextBox ID="txtBarCode" runat="server" CssClass="TextBox" AutoPostBack="True"
                                            OnTextChanged="txtBarcode_TextChanged" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label22" runat="server" Text="Transfer To Branch"></asp:Label>
                                        <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                            LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </td>
                                    <td align="left">
                                    <asp:CheckBox 
                                        ID="CheckBox1" 
                                        runat="server" 
                                        Text="IS Faulty"                                         
                                        AutoPostBack="True"
                                        Font-Names="Serif"
                                        Font-Size="X-Large"
                                        />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold=True></asp:Label>
                                        <asp:HiddenField ID="lblOID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                         <asp:GridView ID="gvStockReturn" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="RequisitionDetailId"
                                            EmptyDataText="No rows returned" ShowHeaderWhenEmpty="True" Width="850px" OnRowDeleting="gvStockReturn_RowDeleting">
                                            <AlternatingRowStyle BackColor="AliceBlue" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>SI</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>                                               

                                                <asp:TemplateField HeaderText="PCategoryID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategoryId" runat="server" Text='<%#Bind("PCategoryID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Brand">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("PCategoryName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="SubCategoryID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubCategoryID" runat="server" Text='<%#Bind("SubCategoryID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Model">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubCategory" runat="server" Text='<%#Bind("SubCategory") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DescriptionID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescriptionID" runat="server" Text='<%#Bind("DescriptionID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Barcode">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="StockInHand">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockInHand" runat="server" Text='<%#Bind("StockInHand") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Qty Pcs">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblQtyPcs" runat="server" Width="40px" Text='<%#Bind("QtyPcs") %>'
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" />
                                                    <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                            Text="Delete">
                                                            <img alt="Delete" src="../Images/Delete.gif" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataRowStyle Font-Bold="True" HorizontalAlign="Center" />
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <HeaderStyle BackColor="#669900" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <RowStyle ForeColor="#000066" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        Reference :
                                        <textarea id="txtRemarks" cols="30" rows="3" runat="server" class="Text_Area" maxlength="200"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <table>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" CssClass="Button_VariableWidth" CausesValidation="False"
                                        OnClientClick="this.disabled=true;" UseSubmitBehavior="False" OnClick="btnSave_Click"
                                        Text="Save   " />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="Button_VariableWidth"
                                        OnClick="btnCancel_Click" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
