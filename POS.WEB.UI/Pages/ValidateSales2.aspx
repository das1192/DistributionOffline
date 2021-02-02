<%@ Page Title="Sales" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ValidateSales2.aspx.cs" Inherits="Pages_ValidateSales" %>

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



            <table cellpadding="2" border="0" width="100%">
                <tr>
                    <td align="center" style="font-weight: bolder">
                        Sales Person ID
                        <hr style="color: Green" />
                    </td>
                </tr>
            </table>






            <table width="100%">
                <tr>
                    <td style="width:100%;" valign="bottom"   > 
                        <asp:Label ID="lblMessage" runat="server"  ></asp:Label> 
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="Label3" runat="server" Style="font-weight: bold" Text="Enter Sales Person ID:"></asp:Label>
                        <br />
                       <asp:TextBox ID="txtStuffID" runat="server" CssClass="TextBox" />
                    </td>
                    
                    
                </tr>
            </table>



            <table width="100%">
                <tr>
                    
                    <td align="left" >
                        <asp:Button ID="btnIssue" runat="server" Text="OK" CssClass="btn btn-success"  
                            onclick="btnIssue_Click" Width="100px"/>&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

