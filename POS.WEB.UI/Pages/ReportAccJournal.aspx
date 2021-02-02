<%@ Page Title="Journal Report" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReportAccJournal.aspx.cs" Inherits="Pages_ReportAccJournal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">

    </script>
    <style type="text/css">
        .style1
        {
            
            background-color :#B8860B;
            color:#fff;
            padding:5px;
            font-weight:bold;
           
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <div id="main-content">
        <div class="wrapper">
            <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                <ProgressTemplate>
                    <img alt="progress" src="../Images/progress.gif" />
                    Processing...
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="table-responsive bg-success" style="background-color: #debc94; padding: 10px 25px;border: 3px solid #93718b;">
                        <table cellpadding="2" border="0" width="100%">
                            <tr>
                                <td align="center" colspan="3" style="font-weight: bolder">
                                    <h2 class="text-center text-white" style="background-color:#B8860B;color:#fff;padding:3px;font-weight:bold;">JOURNAL</h2>
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                        </table>
                        <div class="table-responsive">
                            <section class="panel">
                             <%--   <header class="panel-heading">
                              Search Criteria
                          </header>--%>
                          <h4>Search Criteria</h4>
                          <hr />
                        <table width="100%" cellspacing="5px" cellpadding="10px" style="padding-top:10px;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" Style="font-weight: bold" runat="server" Text="From Date :"></asp:Label>&nbsp;<br />
                                </td>
                                <td>
                                    <asp:TextBox ID="dptFromDate" runat="server" Width="150px" CssClass="form-control mx-sm-3"
                                         /> 
                                         <%--onkeypress="return clearTextBox()"--%>

                                    <asp:CalendarExtender ID="CalendarExtender2" Format="dd MMM yyyy" runat="server" Enabled="True"
                                        TargetControlID="dptFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label7" Style="font-weight: bold" runat="server" Text="To Date :"></asp:Label>&nbsp;<br />
                                </td>
                                <td>
                                    <asp:TextBox ID="dptToDate" runat="server" Width="150px" CssClass="form-control"
                                        /> <%--onkeypress="return clearTextBox()" --%>
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="dd MMM yyyy" runat="server" Enabled="True"
                                        TargetControlID="dptToDate" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    <asp:Button ID="btnSearchPurchaseInvoice" runat="server" CssClass="btn btn-info"
                                        OnClick="btnSearchPurchaseInvoice_Click" Text="Search" />
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                            <td colspan="4">
                            &nbsp;
                            </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        </div>
                        <hr />
                        <div class="table-responsive">
                            <table cellpadding="2" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvJournal" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                            ShowHeaderWhenEmpty="True" Width="100%" AllowPaging="True" PageSize="50" PagerSettings-Position="TopAndBottom"
                                            DataKeyNames="Journal_OID" OnPageIndexChanging="gvJournal_PageIndexChanging"
                                            CssClass="table table-responsive table-bordered table-condensed table-striped table-advance table-hover">
                                            <AlternatingRowStyle BackColor="AliceBlue" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        SI</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEntryDate" runat="server" Text='<%#Bind("IDATTIME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <%--        Journal_OID, AccountID, Remarks, Branch, Customer_Name
                                                        ,Particular
                                                        , RefferenceNumber, Debit, Credit
                                                        , Narration, IDAT, IDATTIME
                                               <asp:TemplateField HeaderText="Journal Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblJournalCode" runat="server" Text='<%#Bind("JournalCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Particular">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDetails" runat="server" Text='<%#Bind("Particular") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reference No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccountReference" runat="server" Text='<%#Bind("RefferenceNumber") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Debit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDrAmount" runat="server" Text='<%#Bind("Debit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Credit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCrAmount" runat="server" Text='<%#Bind("Credit") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Created by">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserFullName" runat="server" Text='<%#Bind("UserFullName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Narration" HeaderText="Narration"/> --%>
                                               
                                                <asp:TemplateField HeaderText="Narration">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNarration" runat="server" Text='<%#Bind("Narration") %>'></asp:Label>

                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Center" />
                                                    <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataRowStyle HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#00CC00" ForeColor="White" CssClass="centerHeaderText" />
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <PagerSettings Position="TopAndBottom" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <RowStyle ForeColor="#000066" />
                                            <SelectedRowStyle BackColor="#669999" ForeColor="White" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
