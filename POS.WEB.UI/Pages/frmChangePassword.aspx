<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmChangePassword.aspx.cs" Inherits="Pages_frmChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style>
        td
        {
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" Runat="Server">
<div style="border-style: solid; border-width: 1px;">
        <div>
            <table border="0px" width="100%" style="font-size: 16px">
                <tr>
                    <td align="center" style="font-size: large; background-color: Green; color: White;
                        font-weight: bold;">
                        Password Change
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table border="0px" style="font-size: 16px">
                <tr>
                    <td>
                        <asp:Label ID="Label25" runat="server" Text="User Name"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label29" runat="server" Text="Current Password"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblCurrentPassword" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNewInvoiceDate" runat="server" Text="New Password"></asp:Label>
                    </td>
                    <td>
                        <span style="color: red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewPassword" runat="server" Width="150px" CssClass="TextBox" />                        
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

