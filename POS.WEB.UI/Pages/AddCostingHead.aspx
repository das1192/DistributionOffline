<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCostingHead.aspx.cs" Inherits="Pages_AddCostingHead" Title="Add Costing Head" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContainerCostingHead" runat="server" ActiveTabIndex="1"
                Width="100%" CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Costing Head">
                    <ContentTemplate>
                        <table width="100%">
                           
                            <tr>
                                <td>
                                    <asp:GridView ID="gvCostingHead" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        DataKeyNames="OID" EmptyDataText="No rows returned" OnRowDeleting="gvCostingHead_RowDeleting"
                                        OnRowEditing="gvCostingHead_RowEditing" Width="100%">
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
                                            <asp:BoundField DataField="ExpenseType" HeaderText="Expense Type">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CostingHead" HeaderText="Costing Head">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

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
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Add/Edit Costing Head">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCostingHead" runat="server" Text="Costing Head:"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                <asp:DropDownList ID="ddlExType" runat="server" CssClass="form-control" 
                                                    AutoPostBack="True">
                                                    <%--<asp:ListItem Text="Select Expense Type"></asp:ListItem>--%>
                                                    <asp:ListItem Text="Indirect" Value="Indirect"></asp:ListItem>
                                                    <asp:ListItem Text="Direct" Value="Direct"></asp:ListItem>
                                                </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCostingHead" runat="server" CssClass="TextBox" />
                                    <asp:RequiredFieldValidator ID="rfvCostingHead" ControlToValidate="txtCostingHead"
                                        ErrorMessage="Costing Head is required" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click"
                                        OnClientClick="this.disabled=true;" UseSubmitBehavior="False" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                        CssClass="btn btn-warning" OnClick="btnCancel_Click" />
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
