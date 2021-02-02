<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CommissionJournal.aspx.cs" Inherits="TalukderPOS.Web.UI.CommissionJournal"
    Title="Commission Journal" EnableEventValidation="false" %>

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
            <asp:TabContainer ID="ContainerProductVendor" runat="server" ActiveTabIndex="0" Width="100%"
                CssClass="fancy fancy-green">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Commission Journal">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-6" style="padding: 0px 35px; border-right: 2px solid gray;">
                                <table cellpadding="2" border="0" width="100%">
                                    <tr id="tr2" runat="server">
                                        <td id="Td5" align="left" runat="server">
                                            <asp:Label ID="Label10" runat="server" Text="Date"></asp:Label>
                                        </td>
                                        <td id="Td6" runat="server">
                                        </td>
                                        <td id="Td7" runat="server">
                                            <asp:TextBox ID="dptSupplierPaymentDate" runat="server" 
                                                CssClass="form-control" />
                                            <asp:CalendarExtender ID="CE_dptSupplierPaymentDate" runat="server" Enabled="True" Format="yyyy-MM-dd"
                                                TargetControlID="dptSupplierPaymentDate" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Cash:"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:CheckBox ID="CheckBox1" runat="server" 
                                                oncheckedchanged="CheckBox1_CheckedChanged" AutoPostBack="True"/>
                                        </td>
                                    </tr>
                                    <tr id="trvendor" runat="server">
                                        <td id="Td1" align="left" runat="server">
                                            <asp:Label ID="Label17" runat="server" Text="Supplier"></asp:Label>
                                        </td>
                                        <td runat="server">
                                        </td>
                                        <td runat="server">
                                            <div>
                                                <asp:DropDownList ID="ddlVendorList" runat="server" CssClass="form-control" 
                                                    AutoPostBack="True" onselectedindexchanged="ddlVendorList_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:CascadingDropDown ID="CCD8" runat="server" Category="Vendor" TargetControlID="ddlVendorList"
                                                    LoadingText="Loading Supplier..." PromptText="--Please Select--" ServiceMethod="BindVendor"
                                                    ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                                </asp:CascadingDropDown>
                                            </div>
                                        </td>
                                    </tr>
                                      <tr id="tr1" runat="server">
                                        <td id="Td2" align="left" runat="server">
                                            <asp:Label ID="lblPayablelbl" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td id="Td3" runat="server">
                                        </td>
                                        <td id="Td4" runat="server">
                                            <asp:Label ID="lblPayableAmount" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Remarks:"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" />
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
                                           
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-info" OnClick="btnSave_Click"
                                                OnClientClick="this.disabled=true;" UseSubmitBehavior="False" />&nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                                CssClass="btn btn-warning" OnClick="btnCancel_Click" />
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
                          
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                    <HeaderTemplate>
                        Commission Journal Report<br />
                    </HeaderTemplate>


                                        <ContentTemplate>
                        <table width="100%" cellspacing="5px" cellpadding="10px">
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label ID="Label6" Style="font-weight: bold" runat="server" Text="From Date (YYYY-MM-DD):"></asp:Label>&nbsp;<br />
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="dptFromDate" runat="server" Width="150px" CssClass="form-control mx-sm-3" />
                                    <asp:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="dptFromDate" />
                                </td>
                                <td style="width: 15%">
                                    <asp:Label ID="Label7" Style="font-weight: bold" runat="server" Text="To Date  (YYYY-MM-DD):"></asp:Label>&nbsp;<br />
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="dptToDate" runat="server" Width="150px" CssClass="form-control" />
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="dptToDate" />
                                </td>
                                <td style="width: 30%">
                                    <asp:Button ID="btnSearchCommission" runat="server" CssClass="btn btn-success btn-sm"
                                        OnClick="btnSearchInvest_Click" CausesValidation="False" Text="Search" />
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
                                    <asp:GridView ID="gvCommissionList" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None"
                                       OnRowDeleting="gvCommissionList_RowDeleting" 
                                       DataKeyNames="RefferenceNumber,Journal_OID,AccountID,IDAT" 
                                        BorderWidth="1px" CellPadding="3"
                                         EmptyDataText="No rows returned"
                                      
                                        Width="100%" >
                                        
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Journal_OID" HeaderText="Journal ID" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AccountID" HeaderText="Account ID" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Particular" HeaderText="Particular">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RefferenceNumber" HeaderText="Reference No">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Debit" HeaderText="Debit" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Credit" HeaderText="Amount">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IDAT" HeaderText="IDAT">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Narration" HeaderText="Narration">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="35%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        Text="Delete" OnClientClick="return confirm('Are you sure to delete?')">
                                                        <img alt="Delete" src="../Images/Delete.gif" />
                                                        </asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>



                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
