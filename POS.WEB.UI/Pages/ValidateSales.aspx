<%@ Page Title="Sales" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ValidateSales.aspx.cs" Inherits="Pages_ValidateSales" %>

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
            <div>
                <div class="row form-inline">
                    <%--<div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Sales Person ID"></asp:Label>
                    </div>--%>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtStuffID" runat="server" CssClass="form-control text-bold" 
                            placeholder="Sales Person ID"  Font-Size="Large" />
                    </div>
                    <div class="col-md-1" >
                    </div>
                    <div class="col-md-2" >
                        <asp:Button ID="btnIssue" runat="server" Text="GO" CssClass="btn btn-sm btn-success" OnClick="btnIssue_Click" />
                    </div>
                </div>
                <div class="row">
                </div>
                <div class="row">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
                <div class="row">
                </div>
            </div>
            <%-- <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td align="center" style="font-weight: bolder">
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 100%;" valign="bottom">
                        
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="left">
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
