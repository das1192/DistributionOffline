<%@ Page Title="Add Shop Logo" MasterPageFile="~/MasterPage.master" Language="C#" AutoEventWireup="true" CodeFile="sync.aspx.cs" Inherits="Pages_sync" %>

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
            <asp:TabContainer ID="ContainerBankInfo" runat="server" ActiveTabIndex="0"
                Width="100%" CssClass="fancy fancy-green">
                
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Add Shop Logo">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Back UP"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="Backup Database" CssClass="btn btn-success" OnClick="btnbackup_Click"
                                        OnClientClick="this.disabled=true;" UseSubmitBehavior="false" Enabled="true" />&nbsp;&nbsp;
                                        <%--Button_VariableWidth--%>
                                    
                                </td>
                                <td>
                                    
                                  
                                  
                                    
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                                <td>
                                  
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    <asp:HiddenField ID="lblOID" runat="server" />
                                </td>
                            </tr>
                        </table>
                         <a href="http://210.4.67.249:8084/Pages/uploaddata.aspx" target="_blank" style="float:right;font-weight:bold;color:#CD5C5C;">Click here to Upload Latest Data</a>   
                    </ContentTemplate>
                    
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
        
    </asp:UpdatePanel>
</asp:Content>
