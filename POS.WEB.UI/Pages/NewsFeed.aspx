<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPage.master" CodeFile="NewsFeed.aspx.cs" Inherits="Pages_NewsFeed" EnableEventValidation="false"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" Runat="Server">
 <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            <b>Working on request...</b></ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="ContinerBrand" runat="server" ActiveTabIndex="0" Width="100%">                
                <asp:TabPanel ID="Panel2" runat="server" HeaderText="Add Message">
                    <ContentTemplate>
                        <table cellpadding="2" border="0" width="100%">                            
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label23" runat="server" Style="font-weight: normal" Text="From Date"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="TextBox" Width="150px" style="margin:0px 5px 0px 5px;"></asp:TextBox><asp:CalendarExtender
                                                    ID="CalendarExtender2" runat="server" Enabled="True" Format="yyyy-MM-dd" TargetControlID="txtFromDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="Label24" runat="server" Style="font-weight: normal" Text="To Date"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="TextBox" Width="150px" style="margin:0px 5px 0px 5px;"></asp:TextBox><asp:CalendarExtender
                                                    ID="CalendarExtender3" runat="server" Enabled="True" Format="yyyy-MM-dd" TargetControlID="txtToDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="Label1" runat="server" Text="Branch"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList" style="margin-left:5px;">
                                                </asp:DropDownList>
                                                <asp:CascadingDropDown ID="Cascadingdropdown1" runat="server" Category="Branch" TargetControlID="ddlSearchBranch"
                                                    LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                                    ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                                </asp:CascadingDropDown>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNews" runat="server" Text="Message"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <textarea ID="txtNews" runat="server" rows="3" cols="50"></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;&nbsp;
                                            </td>
                                            <td colspan="2">
                                                <asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="Button" OnClientClick="this.disabled=true;"
                                                    UseSubmitBehavior="False" OnClick="cmdSave_Click" style="width:80px;height: 30px;padding:0px 2px 2px 2px;"/>&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="cmdCancel" CausesValidation="False" runat="server" Text="Cancel"
                                                    CssClass="Button" OnClick="cmdCancel_Click"  style="width:80px;height: 30px;padding:0px 2px 2px 2px;"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label><asp:HiddenField ID="lblOID"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

