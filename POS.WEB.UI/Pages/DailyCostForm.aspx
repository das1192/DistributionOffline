<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DailyCostForm.aspx.cs" Inherits="TalukderPOS.Web.UI.DailyCostForm"
    Title="Daily Payment" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerDailyCost" runat="server" ActiveTabIndex="1" Width="100%"
                CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Expense History">
                    <ContentTemplate>
                     <table width="100%" cellspacing="5px" cellpadding="10px">
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label ID="Label6" Style="font-weight: bold" runat="server" Text="From Date (YYYY-MM-DD):"></asp:Label>&nbsp;<br />
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="dptFromDateDCL" runat="server" Width="150px" CssClass="form-control mx-sm-3" />
                                    <asp:CalendarExtender ID="CE_dptFromDateDCL" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="dptFromDateDCL" />
                                </td>
                                <td style="width: 15%">
                                    <asp:Label ID="Label7" Style="font-weight: bold" runat="server" Text="To Date  (YYYY-MM-DD):"></asp:Label>&nbsp;<br />
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="dptToDateDCL" runat="server" Width="150px" CssClass="form-control" />
                                    <asp:CalendarExtender ID="CE_dptToDateDCL" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="dptToDateDCL" />
                                </td>
                                <td style="width: 30%">
                                    <asp:Button ID="btnSearchDailyCost" runat="server" CssClass="btn btn-success btn-sm"
                                         CausesValidation="False" Text="Search" 
                                        onclick="btnSearchDailyCost_Click" />
                                    <asp:Label ID="Label3" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblMessageForList" runat="server" BackColor="#FFFF66" Font-Bold="True"
                                        Font-Italic="True" ForeColor="Red" Style="font-weight: bold"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvDailyCost" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" PageSize="25" AllowPaging="True" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" DataKeyNames="OID" EmptyDataText="No rows returned"
                                        OnPageIndexChanging="gvT_PROD_PageIndexChanging" OnRowDeleting="gvDailyCost_RowDeleting"
                                        OnRowEditing="gvDailyCost_RowEditing" Width="100%">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <PagerStyle CssClass="pgr" />
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
                                            <asp:BoundField DataField="OID" HeaderText="OID" Visible ="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CostingHead" HeaderText="Purpose">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AMOUNT" HeaderText="Amount">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NEWDATE" HeaderText="Date">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                Visible="False" ShowEditButton="True" EditImageUrl="~/Images/edit.png" ButtonType="Image">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:CommandField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        Text="Delete" OnClientClick="return confirm('Are you sure to delete?')">
                                                        <img alt="Delete" src="../Images/Delete.gif" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Daily Expense">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-5" style="padding: 0px 35px; border-right: 2px solid gray;">
                                <table cellpadding="2" border="0" width="100%">
                                    <tr id="tr2" runat="server">
                                        <td id="Td5" align="left" runat="server">
                                            <asp:Label ID="Label1" runat="server" Text="Date"></asp:Label>
                                        </td>
                                        <td id="Td6" runat="server">
                                        </td>
                                        <td id="Td7" runat="server">
                                            <asp:TextBox ID="dptFromDate" runat="server" CssClass="form-control" />
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" Format="yyyy-MM-dd"
                                                TargetControlID="dptFromDate" />
                                        </td>
                                    </tr>
                                    <tr id="trvendor" runat="server">
                                        <td id="Td1" align="left" runat="server">
                                            <asp:Label ID="Label17" runat="server" Text="Expense Type"></asp:Label>
                                        </td>
                                        <td runat="server">
                                        </td>
                                        <td runat="server">
                                            <div>
                                                <asp:DropDownList ID="ddlCostingHead" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                                <asp:CascadingDropDown ID="CCD8" runat="server" Category="Vendor" TargetControlID="ddlCostingHead"
                                                    LoadingText="Loading Costing Head..." PromptText="--Please Select--" ServiceMethod="BindCostingHead"
                                                    ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                                </asp:CascadingDropDown>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server">
                                        <td id="Td2" align="left" runat="server">
                                            <asp:Label ID="Label2" runat="server" Text="Payment From"></asp:Label>
                                        </td>
                                        <td id="Td3" runat="server">
                                        </td>
                                        <td id="Td4" runat="server">
                                            <div>
                                                <asp:DropDownList ID="ddlPamymentFrom" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRemarksCheckNo" runat="server" Text="Remarks / Check No:"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAmount" runat="server" Text="Amount:" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="color: red;">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                                Font-Bold="True" />
                                            <asp:RequiredFieldValidator ID="rfvAmount" ControlToValidate="txtAmount" ErrorMessage="Amount is required"
                                                Display="Dynamic" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click"
                                                OnClientClick="this.disabled=true;" Text="Save" UseSubmitBehavior="False" />
                                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-warning"
                                                OnClick="btnCancel_Click" Text="Cancel" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                            <asp:HiddenField ID="lblOID" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-7" style="padding: 0px 35px;">
                                <asp:GridView ID="gvCashBalance" runat="server" AutoGenerateColumns="False" GridLines="None"
                                    CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                    EmptyDataText="No rows returned" Width="100%">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <Columns>
                                        <asp:BoundField DataField="BankName" HeaderText="Bank">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AccountNo" HeaderText="Account No">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Debit" HeaderText="Debit">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Credit" HeaderText="Credit">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Balance" HeaderText="Balance">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Current Cash Balance" Visible="false">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <%-- Branch,t.BankName,t.AccountNo,t.Debit,t.Credit,t.Balance--%>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tPnlDisplay" runat="server" HeaderText="Cash Flow" Visible="false">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="6" align="center" style="font-weight: bolder">
                                    Cash Flow Search
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label23" Style="font-weight: normal" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDateFrom" runat="server" Width="150px" CssClass="TextBox" /><asp:CalendarExtender
                                        ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True" TargetControlID="txtDateFrom" />
                                </td>
                                <td>
                                    <asp:Label ID="Label24" Style="font-weight: normal" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDateTo" runat="server" Width="150px" CssClass="TextBox" /><asp:CalendarExtender
                                        ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True" TargetControlID="txtDateTo" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Button ID="cmdSearch" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                        OnClick="cmdSearchCashflow_Click" Text="Search" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    Search Type
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbtDetails" runat="server" Checked="True" GroupName="Software"
                                        Text="Details" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <div>
                                    </div>
                                </td>
                                <td align="right" colspan="4">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" style="font-weight: bolder">
                                                Cash Flow
                                                <hr style="color: Green" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 100%;" valign="bottom">
                                                <asp:Label ID="Label26" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvT_CashFlow" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                    CssClass="mGrid" AllowPaging="True" DataKeyNames="OID" BorderColor="#CCCCCC"
                                                    BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="No rows returned"
                                                    OnPageIndexChanging="gvT_CashFlow_PageIndexChanging" Width="100%">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <PagerStyle CssClass="pgr" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                SI</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OID" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOID" runat="server" Text='<%#Bind("OID") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Particular">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblParticular" runat="server" Text='<%#Bind("Particular") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Purpose">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPurpose" runat="server" Text='<%#Bind("CostingHead") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Previous Balance">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPrevBalance" runat="server" Text='<%#Bind("PrevBalance") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="New Balance">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNewBalance" runat="server" Text='<%#Bind("NewBalance") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="User">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("UserFullName") %>'></asp:Label></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
