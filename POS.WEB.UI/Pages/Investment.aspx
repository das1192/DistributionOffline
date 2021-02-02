<%@ Page Title="Investment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="Investment.aspx.cs" Inherits="Pages_Investment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">


    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

     
       
    </script>
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
            <asp:TabContainer ID="ContainerBankInfo" runat="server" ActiveTabIndex="1" CssClass="fancy fancy-green"
                Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Investment/Withdraw List">
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
                                    <asp:Button ID="btnSearchInvest" runat="server" CssClass="btn btn-success btn-sm"
                                        OnClick="btnSearchInvest_Click" CausesValidation="False" Text="Search" />
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
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
                                    <asp:GridView ID="gvInvestList" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        DataKeyNames="RefferenceNumber,Journal_OID,AccountID,Remarks,IDAT" EmptyDataText="No rows returned"
                                        OnRowDeleting="gvBank_RowDeleting" OnRowEditing="gvBank_RowEditing" Width="100%">
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
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RefferenceNumber" HeaderText="Reference No">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Debit" HeaderText="Debit">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Credit" HeaderText="Credit">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IDAT" HeaderText="IDAT">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:BoundField>
                                            <asp:CommandField CausesValidation="False" HeaderText="Edit" ShowCancelButton="False"
                                                Visible="False" ShowEditButton="True" EditImageUrl="~/Images/edit.png" ButtonType="Image">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:CommandField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                        Text="Delete" OnClientClick="return confirm('Are you sure to delete?')">
                                                        <img alt="Delete" src="../Images/Delete.gif" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Add Invest/Withdraw">
                    <ContentTemplate>
                        <table cellpadding="2" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblBankName4" runat="server" Text="Date"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="dptDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="dptDate_CalendarExtender" runat="server" Enabled="True"
                                        Format="yyyy-MM-dd" TargetControlID="dptDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBankName3" runat="server" Text="Action"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOwnerAction" runat="server" AutoPostBack="True" CssClass="form-control"
                                        OnSelectedIndexChanged="ddlOwnerAction_SelectedIndexChanged">
                                        <asp:ListItem Value="Invest">Invest</asp:ListItem>
                                        <asp:ListItem Value="Withdraw">Withdraw From Bank To Cash</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBankName2" runat="server" Text="Account No"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBankName1" runat="server" Text="Remarks"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBankName0" runat="server" Text="Amount" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                      onkeypress="return isNumberKey(event)"    Font-Bold="True"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvtxtAmount" runat="server" ControlToValidate="txtAmount"
                                        Display="Dynamic" ErrorMessage="Amount!"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-sm btn-success"
                                        OnClick="btnSave_Click" OnClientClick="this.disabled=true;" UseSubmitBehavior="False" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" CausesValidation="False" runat="server" Text="Cancel"
                                        CssClass="btn btn-warning btn-sm" OnClick="btnCancel_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#33CC33"></asp:Label>
                                    <asp:HiddenField ID="lblOID" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>


                <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Cash Transfer to Bank">
                    <ContentTemplate>
                    <div class="row">
                            <div class="col-md-5" style="padding: 0px 35px; border-right: 2px solid gray;">
                        <table cellpadding="2" border="0">
                            
                           
                            <tr id="Tr1" runat="server">
                                        <td id="Td1"  align="left" runat="server">
                                            <asp:Label ID="Label2" runat="server" Text="Trnsfer Bank"></asp:Label>
                                        </td>
                                        <td id="Td2"  runat="server">
                                        </td>
                                        <td id="Td3" runat="server">
                                            <div>
                                                <asp:DropDownList ID="ddlPamymentFromBank" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Amount" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                    <span style="color: red;">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAmountTransfer111" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                      onkeypress="return isNumberKey(event)"    Font-Bold="True"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                                <td>
                                    
                                    <asp:Button ID="TransferCash" runat="server" Text="Transfer Cash" 
                                       UseSubmitBehavior="False" CausesValidation="False" CssClass="btn btn-sm btn-success" 
                                        onclick="TransferCash_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblsuccessorfail" runat="server" Font-Bold="True"></asp:Label>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
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
                                        <asp:BoundField DataField="BankName" HeaderText="Cash">
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

                <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Withdraw From Business">
                    <ContentTemplate>
                    <div class="row">
                            <div class="col-md-5" style="padding: 0px 35px; border-right: 2px solid gray;">
                        <table cellpadding="2" border="0">
                            
                           
                                    <tr id="tr2" runat="server">
                                        <td id="Td4" align="left" runat="server">
                                            <asp:Label ID="Label5" runat="server" Text="Payment From"></asp:Label>
                                        </td>
                                        <td id="Td5" runat="server">
                                        </td>
                                        <td id="Td6" runat="server">
                                            <div>
                                                <asp:DropDownList ID="ddlPamymentFrom" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
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
                                            <asp:TextBox ID="txtAmount2" runat="server" CssClass="form-control text-center" BackColor="#FFFF99"
                                             onkeypress="return isNumberKey(event)"    Font-Bold="True"  />
                                            <%--<asp:RequiredFieldValidator ID="rfvAmount" ControlToValidate="txtAmount2" ErrorMessage="Amount is required"
                                                Display="Dynamic" runat="server" />--%>
                                        </td>
                                    </tr>
<tr>
                                        <td>
                                            <asp:Label ID="lblRemarksBusiness" runat="server" Text="Remarks"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRemarksBusiness" runat="server" CssClass="form-control" />
                                        </td>
                                    </tr>


                             <tr>
                                 <td>
                                     <asp:Button ID="btnSave2" runat="server" CssClass="btn btn-success" 
                                         OnClick="btnSave2_Click" Text="Withdraw" UseSubmitBehavior="False" CausesValidation="False" />
                                    
                                 </td>
                            </tr>

                            <tr>
                                        <td colspan="3">
                                            <asp:Label ID="Label3" runat="server"></asp:Label>
                                            <asp:HiddenField ID="HiddenField2" runat="server" />
                                        </td>
                                    </tr>
                             
                          
                        </table>
                        </div>
                        
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>

            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
