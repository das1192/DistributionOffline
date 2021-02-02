<%@ Page Title="Receive Stock Return" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransferStockReturn.aspx.cs" Inherits="Pages_TransferStockReturn" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerStockReturnReceived" runat="server" ActiveTabIndex="0" Width="100%">

                 <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Receive Stock Return">
                    <ContentTemplate>
                        <table cellpadding="2" border="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" style="font-weight: bolder">
                                    Received Stock Return
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                       
                            <tr>
                                <td style="width: 20px">
                                    <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Stock Return No:"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddlStockReturnNo" runat="server" AutoPostBack="True" CssClass="DropDownList"
                                        OnSelectedIndexChanged="ddlStockReturnNo_SelectedIndexChanged">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100%;" valign="bottom">
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                </td>
                            </tr>                        
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvStockReturnDetails" runat="server" AutoGenerateColumns="False"
                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                        DataKeyNames="StockReturnDetailID" CellPadding="3" EmptyDataText="No rows returned"
                                        ShowHeaderWhenEmpty="True" Width="100%">
                                        <AlternatingRowStyle BackColor="AliceBlue" />
                                        <Columns>
                                             <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStockReturnDetailID" runat="server" Text='<%#Bind("StockReturnDetailID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="False" HeaderText="CCOM_CODE">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCCOM_CODE" runat="server" Text='<%#Bind("OID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="true" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="True" HeaderText="Transfer From">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCCOM_NAME" runat="server" Text='<%#Bind("FromStoreID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="ToStoreIDOID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblToStoreIDOID" runat="server" Text='<%#Bind("ToStoreIDOID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Transfer To">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblToStoreID" runat="server" Text='<%#Bind("ToStoreID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStockReturnID" runat="server" Text='<%#Bind("StockReturnID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%#Bind("PCategoryID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Brand">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%#Bind("PCategoryName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubCategoryID" runat="server" Text='<%#Bind("SubCategoryID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Model">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubCategory" runat="server" Text='<%#Bind("SubCategory") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescriptionID" runat="server" Text='<%#Bind("DescriptionID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Color/Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Barcode/IMEI">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Return Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQtyPcs" runat="server" Text='<%#Bind("QtyPcs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is Faulty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFaultyStat" runat="server" Text='<%#Bind("faulty_stat") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                              <asp:TemplateField Visible="false" HeaderText="Transfer Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferDate" runat="server" Text='<%#Bind("TransferDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="grid_header" />
                                                <ItemStyle Font-Bold="false" Font-Size="Small" HorizontalAlign="Center" />
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
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnIssue" runat="server" Text="Received" CssClass="Button_VariableWidth"
                                        OnClick="btnIssue_Click" CausesValidation="False" Height="40px" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>

                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Stock Return Not Receive List">
                    <ContentTemplate>
                        <table width="100%">                            
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server"
                                        Enabled="True" TargetControlID="txtToDate" />
                                </td>
                                <td align="right">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button" 
                                        CausesValidation="False" onclick="cmdSearch_Click"/>
                                </td>
                            </tr>
                            <tr><td colspan="5">&nbsp;</td></tr>                           
                            <tr><td colspan="5">&nbsp;</td></tr>


                            <tr>
                                <td colspan="5">
                                    <asp:GridView ID="gvStockReturnList" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid"
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="StockReturnID"
                                        EmptyDataText="No rows returned" Width="100%" OnRowCommand="StockReturn_Details" >
                                        <Columns>
                                         <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
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
                                            
                                            <asp:BoundField DataField="ReferenceBy" HeaderText="Reference By" />                                                                                       

                                            <asp:TemplateField HeaderText="Preview">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPreview" Text="Preview" runat="server" CommandArgument='<%# Eval("StockReturnID")%>'
                                                        CommandName="Preview"></asp:LinkButton>
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
                                    <asp:Label ID="lblMessage1" Style="font-weight: bold"  runat="server" ></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td align="left" colspan="5" style="font-weight: bolder">
                                    
                                </td>
                            </tr>

                              <tr>
                                <td align="center" colspan="5" style="font-weight: bolder">
                                    Stock Return Details
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid"
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        Width="100%" DataKeyNames="StockReturnDetailID" >
                                        <Columns>   
                                         <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>                                         
                                            <asp:BoundField DataField="StockReturnDetailID" HeaderText="StockReturnDetailID" Visible="False" />
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

                 <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Stock Return Receive List">
                    <ContentTemplate>
                        <table width="100%">                            
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceivedFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtReceivedFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceivedToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender4" Format="yyyy-MM-dd" runat="server"
                                        Enabled="True" TargetControlID="txtReceivedToDate" />
                                </td>
                                 <td>
                                    <asp:Label ID="Label10" runat="server" Text="IMEI/Barcode"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchBarcode" runat="server" Width="200px" CssClass="TextBox" />
                                    
                                </td>
                               
                            </tr>

                            <tr><td colspan="6">&nbsp;</td></tr>       

                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label7" runat="server" Text="Category"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlSearchProductCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD2" runat="server" Category="Category" TargetControlID="ddlSearchProductCategory"
                                            LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label8" runat="server" Text="Model"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlSearchSubCategory" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD3" runat="server" Category="Model" TargetControlID="ddlSearchSubCategory"
                                            ParentControlID="ddlSearchProductCategory" LoadingText="Loading Model..." PromptText="--Please Select--"
                                            ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label9" runat="server" Text="Description/Color"></asp:Label>
                                </td>
                                <td align="right">
                                    <div>
                                        <asp:DropDownList ID="ddlSearchDescription" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD4" runat="server" Category="Description" TargetControlID="ddlSearchDescription"
                                            ParentControlID="ddlSearchSubCategory" LoadingText="Loading Color..." PromptText="--Please Select--"
                                            ServiceMethod="BindDescription" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                            </tr>
                            <tr><td colspan="6">&nbsp;</td></tr>  

                            <tr>
                            <td align="left">
                                    <asp:Label ID="Label22" runat="server" Text="Transfer From"></asp:Label>
                                </td>
                                <td>
                                    <div>
                                        <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                            LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                            ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                             <td align="right" colspan="4">
                                    <asp:Button ID="Button1" runat="server" Text="Search" CssClass="Button" 
                                        CausesValidation="False" onclick="Button1_Click" />
                                </td>
                            </tr>

                            <tr><td colspan="6">&nbsp;</td></tr>                           
                           

                            
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid"
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                        Width="100%" >
                                        <Columns>   
                                         <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField> 

                                             <asp:BoundField DataField="FromStoreID" HeaderText="Transfer From">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
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

                                             <asp:BoundField DataField="ApprovedStatus" HeaderText="Received" Visible="false">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                             

                                             <asp:BoundField DataField="ToStoreID" HeaderText="Transfer To" Visible="false">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                                                                         
                                              <asp:BoundField DataField="IDAT" HeaderText="Transfer Date" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="EDAT" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            
                                                                                       
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

               

            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
