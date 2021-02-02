<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ChangeInvoiceDate.aspx.cs" Inherits="Pages_ChangeInvoiceDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        td
        {
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <div style="border-style: solid; border-width: 1px;">
        <div>
            <table border="0px" width="100%" style="font-size: 16px">
                <tr>
                    <td align="center" style="font-size: large; background-color: Green; color: White;
                        font-weight: bold;">
                        Change Invoice Date
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table border="0px" style="font-size: 16px">
                <tr>
                    <td>
                        <asp:Label ID="Label25" runat="server" Text="Invoice No"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label29" runat="server" Text="Current Invoice Date"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblCurrentInvoiceDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNewInvoiceDate" runat="server" Text="New Invoice Date"></asp:Label>
                    </td>
                    <td>
                        <span style="color: red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewInvoiceDate" runat="server" Width="150px" CssClass="TextBox" />
                        <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                            TargetControlID="txtNewInvoiceDate" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="font-size: 16px; font-weight: bold; color: Red;">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                    <td align="left">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Button_VariableWidth"
                            CausesValidation="False" Width="100px" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
