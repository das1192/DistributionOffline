<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmDOAReport.aspx.cs" Inherits="Pages_frmDOAReport" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .txtStyle
        {
            background: white;
            border: 1px solid #ffa853;
            border-radius: 5px;
            box-shadow: 0 0 5px 3px #ffa853;
            color: #666;
            padding: 5px 10px;
            width: 300px;
            outline: none;
            font-family: Times New Roman,Arial,Sans-Serif;
            font-size: 25px;
            font-weight: bold;
            text-align: center;
        }
        
        #loginform
        {
            min-width: 200px;
            height: 140px;
            background-color: #ffffff;
            border: 1px solid;
            border-color: #555555;
            padding: 16px 16px;
            border-radius: 4px;
            -webkit-box-shadow: 0px 1px 6px rgba(75, 31, 57, 0.8);
            -moz-box-shadow: 0px 1px 6px rgba(75, 31, 57, 0.8);
            box-shadow: 0px 1px 6px rgba(223, 88, 13, 0.8);
        }
        .modalBackground
        {
            background-color: #333333;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .txt
        {
            color: #505050;
        }
        .redstar
        {
            color: #FF0000;
        }
        .hidden
        {
            display: none;
        }
        
        
        
         .Background
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .Popup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 400px;
            height: 350px;
        }
        .lbl
        {
            font-size:16px;
            font-style:italic;
            font-weight:bold;
        }
        
        
      
      
        #grid-header
        {
            background-color: #f49521;
        }
    </style>
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
            <asp:TabContainer ID="ContainerProductMovement" runat="server" ActiveTabIndex="0" Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="DOA Database">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label22" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                        LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="From Date "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="To Date "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                                        Enabled="True" TargetControlID="txtToDate" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Brand"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchProductCategory" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD2" runat="server" Category="Category" TargetControlID="ddlSearchProductCategory"
                                        LoadingText="Loading Categories..." PromptText="--Please Select--" ServiceMethod="BindCategory"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label6" runat="server" Text="Model"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchSubCategory" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD3" runat="server" Category="Model" TargetControlID="ddlSearchSubCategory"
                                        ParentControlID="ddlSearchProductCategory" LoadingText="Loading Model..." PromptText="--Please Select--"
                                        ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td> 
                                  <td>
                                    <asp:Label ID="Label4" runat="server" Text="IMEI/Barcode"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBarcode" runat="server" Width="150px" CssClass="TextBox" />
                                    
                                </td>                             
                            </tr>
                             <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px;">
                                    <asp:Label ID="Label5" runat="server"  Text="Status:"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbtActive" runat="server" Text="Active" GroupName="Software" Checked="True" />
                                    &nbsp;
                                    <asp:RadioButton ID="rbtInActive" runat="server" Text="In Active" GroupName="Software"  />
                                     &nbsp;
                                    <asp:RadioButton ID="rbtReturn" runat="server" Text="Return" GroupName="Software"  />
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>                                        
                                </td>
                             
                                <td align="right">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button_VariableWidth"
                                        CausesValidation="False" OnClick="cmdSearch_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" GridLines="None" DataKeyNames="OID"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        EmptyDataText="No rows returned" Width="100%"  OnRowDeleting="gvT_PROD_RowDeleting" OnRowCommand="StockReturn_Details">
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

                                             <asp:TemplateField HeaderText="OID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOID" runat="server" Text='<%#Bind("OID") %>'></asp:Label></ItemTemplate>                                               
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="BranchOID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranchOID" runat="server" Text='<%#Bind("BranchOID") %>'></asp:Label></ItemTemplate>                                               
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="CategoryOID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryOID" runat="server" Text='<%#Bind("CategoryOID") %>'></asp:Label></ItemTemplate>                                               
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="OID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubcategoryOID" runat="server" Text='<%#Bind("SubcategoryOID") %>'></asp:Label></ItemTemplate>                                               
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="DescriptionOID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescriptionOID" runat="server" Text='<%#Bind("DescriptionOID") %>'></asp:Label></ItemTemplate>                                               
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
                                              
                                            <asp:TemplateField HeaderText="IMEI/Barcode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label></ItemTemplate>                                               
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                             
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>'></asp:Label></ItemTemplate>                                               
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>


                                               <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>                                                                   
                                             <asp:BoundField DataField="IDAT" HeaderText="Date">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                             <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        Text="Delete"><img alt="Delete" src="../Images/Delete.gif" /></asp:LinkButton></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Stock In">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkStockIn" Text="Stock In" runat="server" CommandArgument='<%# Eval("OID")%>'
                                                        CommandName="InvoiceNo"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>

                         <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnHidden" runat="server" Style="display: none" />
                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="btnHidden" PopupControlID="popUpPanel"
                                        CancelControlID="btnclose" BackgroundCssClass="modalBackground" DropShadow="True"
                                        runat="server" DynamicServicePath="" Enabled="True">
                                    </asp:ModalPopupExtender>
                                    <asp:Panel ID="popUpPanel" runat="server">
                                        <div id="loginform">
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblDeleteReasonMessage" runat="server" Style="font-weight: bold" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label25" runat="server" Text="Product ID:" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblProductID" runat="server" Style="font-weight: bold"></asp:Label>
                                                    </td>
                                                </tr>
                                               
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblReason" runat="server" Text="Delete Reason"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <span style="color: red;">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDeleteReason" runat="server" CssClass="txtStyle" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="Button_VariableWidth" OnClick="btnSubmitDiscount_Click"
                                                            CausesValidation="False" Width="100px" />&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnclose" runat="server" Text="Close" CausesValidation="False" CssClass="Button_VariableWidth" Width="100px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

