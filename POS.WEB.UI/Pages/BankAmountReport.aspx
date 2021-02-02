<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BankAmountReport.aspx.cs" Inherits="Pages_BankAccountReportAdmin" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="Server">
    <asp:UpdateProgress ID="updProgress" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...</ProgressTemplate>
    </asp:UpdateProgress>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">--%>
    <contenttemplate>
            <asp:TabContainer ID="ContainerSalesItems" runat="server" ActiveTabIndex="0" Width="100%">
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Bank Amount Report">
                    <ContentTemplate>
                        <table>
                            <tr  >
                                <td align="left"  >
                                    <asp:Label ID="Label22" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td >
                                    <asp:DropDownList ID="ddlSearchBranch" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CDD1" runat="server" Category="Branch1" TargetControlID="ddlSearchBranch"
                                        LoadingText="Loading Branch..." PromptText="--Please Select--" ServiceMethod="BindBranch"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td  >
                                    <asp:Label ID="lblBank" runat="server" Text="Bank "></asp:Label>
                                </td>
                                <td >
                                    <asp:DropDownList ID="ddlSearchBank" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Bank" TargetControlID="ddlSearchBank"
                                        LoadingText="Loading Bank..." PromptText="--Please Select--" ServiceMethod="Bindbank"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                                <td align="left" >
                                    <asp:Label ID="Label4" runat="server" Text="Payment Mode"></asp:Label>
                                </td>
                                <td  >
                                    <asp:DropDownList ID="ddlSearchPayment" runat="server" CssClass="DropDownList">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Category" TargetControlID="ddlSearchPayment"
                                        LoadingText="Loading Payment Mode..." PromptText="--Please Select--" ServiceMethod="BindPaymentMode"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr >
                            <td colspan="6">&nbsp;</td>
                            </tr>
                            <tr >
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" runat="server" Enabled="True"
                                        TargetControlID="txtFromDate" />
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="To Date "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" CssClass="TextBox" />&nbsp;<br />
                                    <asp:CalendarExtender ID="txtReceiveDate_CalendarExtender" Format="yyyy-MM-dd" runat="server"
                                        Enabled="True" TargetControlID="txtToDate" />
                                </td>
                                <td align="right" colspan="2">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="Button_VariableWidth" Width="100px"
                                        CausesValidation="False" OnClick="cmdSearch_Click" />&nbsp;
                                        <asp:Button ID="cmdExportToExcel" runat="server" Text="Export To Excel" 
                                        CssClass="Button_VariableWidth" Width="120px"
                                        CausesValidation="False" onclick="cmdExportToExcel_Click" />
                                </td>
                            </tr>
                            <tr>
                               <td colspan="6" align="right">
                                    <asp:Label ID="lblTotal" runat="server" ForeColor="Green" Font-Bold="True" ></asp:Label>
                                </td>
                            </tr>
                            <tr >
                                <td colspan="6">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        CssClass="mGrid" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                        EmptyDataText="No rows returned" Width="100%" OnRowCommand="grdCustomPagging_RowCommand">
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>SI</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSRNO1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                             <asp:BoundField DataField="CCOM_NAME" HeaderText="Branch">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                                                                    
                                         <asp:TemplateField HeaderText="InvoiceNo">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkView" CommandArgument='<%#Eval("InvoiceNo") %>' Text='<%# Eval("InvoiceNo") %>' CommandName="VIEW"></asp:LinkButton>
                                             </ItemTemplate>
                                              <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                           </asp:TemplateField>

                                         
                                           
    

                                            <asp:BoundField DataField="PaymentMode" HeaderText="Payment Mode">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BankName" HeaderText="Bank Name">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:###,###}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IDAT" HeaderText="Date">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </contenttemplate>
    <%--</asp:UpdatePanel>--%>
</asp:Content>
