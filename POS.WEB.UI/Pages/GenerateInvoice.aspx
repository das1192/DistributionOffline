<%@ Page Title="Generate Invoice" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="GenerateInvoice.aspx.cs" Inherits="Pages_GenerateInvoice" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            
           
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
