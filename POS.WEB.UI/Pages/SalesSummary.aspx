<%@ Page Title="Sales Summary" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesSummary.aspx.cs" Inherits="Pages_SalesSummary" %>

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
			<asp:TabContainer ID="tContT_STOCK" runat="server" ActiveTabIndex="0" >


            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Sales Summary">
					<ContentTemplate>
                    <table width="100%">                       
                        <tr>
                             <td align="right">&nbsp;<asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>:</td>                             
                             <td>
                                 <div>
                                        <asp:DropDownList ID="ddlSearchProductCategory" runat="server" 
                                            CssClass="DropDownList" AutoPostBack="True" 
                                            onselectedindexchanged="ddlSearchProductCategory_SelectedIndexChanged">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                            </td>

                             <td align="right">&nbsp;<asp:Label ID="Label6" runat="server" Text="Model"></asp:Label>:</td>                             
                             <td>
                                 <div>
                                        <asp:DropDownList ID="ddlSearchSubCategory" runat="server" CssClass="DropDownList"  
                                            AutoPostBack="True" 
                                            onselectedindexchanged="ddlSearchSubCategory_SelectedIndexChanged">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                            </td>


                             <td align="right">&nbsp;<asp:Label ID="Label7" runat="server" Text="Description/Color"></asp:Label>:</td>                             
                             <td>
                                 <div>
                                        <asp:DropDownList ID="ddlSearchDescription" runat="server" CssClass="DropDownList"  AutoPostBack="True">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                </div>
                            </td>
                        </tr>


                        <tr>
                        <td>
                        <p></p>
                        </td>
                        </tr>


                        <tr>
                            <td align="left" colspan="2">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button" onclick="cmdSearch_Click" CausesValidation="False"/>&nbsp;                                    
                            </td>
                            
                        </tr>



                        <tr>
                            <td colspan="7">
                            <br />                             
                            </td>
                        </tr>
                                            <tr><td colspan="7">
                                            <asp:GridView ID="gvStockBalance" runat="server" AutoGenerateColumns="False" BackColor="White"                                            
                                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                            EmptyDataText="No rows returned" Width="100%">
                                            <Columns>
                                             <%--<asp:BoundField DataField="OID" HeaderText="OID" Visible="false" /> 
                                             <asp:BoundField DataField="Category" HeaderText="Category" /> 
                                             <asp:BoundField DataField="SubCategoryName" HeaderText="Model" /> 
                                             <asp:BoundField DataField="Description" HeaderText="Description" /> 
                                             <asp:BoundField DataField="Barcode" HeaderText="Barcode" /> 
                                             <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" />                                                                                      
                                             <asp:BoundField DataField="SalePrice" HeaderText="Sale Price" />--%>
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

