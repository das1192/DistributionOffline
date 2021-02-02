<%@ Page Title="User Permission" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserPageAuthorizationForm.aspx.cs" Inherits="Pages_UserPageAuthorizationForm"
    EnableViewStateMac="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td align="center" style="font-weight: bold">
                        User Wise Authorization
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <div>
            </div>
            <div align="center" style="width: 100%">
                <table style="padding-left: 50px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            
                            <asp:CompareValidator ID="CVUserauthetication" runat="server" ErrorMessage="Sorry! Select one of the User Name"
                                ControlToValidate="ddlUser" Text="*" Display="Dynamic" ValidationGroup="UserPageAuthorizationForm"
                                ValueToCompare="0" Operator="GreaterThan">
                            </asp:CompareValidator>
                        </td>
                    </tr>
                </table>
            </div>
            <div align="center" style="width: 100%">                
                <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false"></asp:Label>
            </div>
            <div align="center" style="width: 100%;">
                <asp:Panel ID="panelGV" runat="server" ScrollBars="Auto" Width="60%" Height="520px">
                    <asp:GridView ID="gvAllPages" runat="server" AutoGenerateColumns="false" BackColor="White"
                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="MenuHeadID"
                        EmptyDataText="No Data Found" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-Font-Bold="true"
                        EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound="gvAllPages_RowDataBound"
                        OnPageIndexChanging="gvAllPages_PageIndexChanging" Width="100%" ShowHeader="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkBoxHeader" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblHeader" runat="server" Text='<%#Bind("MenuHeadName") %>' Font-Bold="true"
                                        CssClass="alphabetic"></asp:Label>
                                    <asp:GridView ID="gvPages" runat="server" AutoGenerateColumns="false" BackColor="#8DB6CD"
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="PageId"
                                        EmptyDataText="No Data Found" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-Font-Bold="true"
                                        EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound="gvPages_RowDataBound"
                                        Width="100%" ShowHeader="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkBoxPages" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle CssClass="alphabetic" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPages" runat="server" Text='<%#Bind("PageName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" Width="100%" ForeColor="Black" />
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div>
            </div>
            <div align="center" style="width: 100%">
                <asp:Button ID="btnPermission" runat="server" Text="Permission" ValidationGroup="UserPageAuthorizationForm"
                    CssClass="btn btn-success" OnClick="btnPermission_Click" />
            </div>
            <asp:ValidationSummary ID="vs" runat="server" CssClass="error" ValidationGroup="UserPageAuthorizationForm"
                ShowMessageBox="true" ForeColor="Red" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
