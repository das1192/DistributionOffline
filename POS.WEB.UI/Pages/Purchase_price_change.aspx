<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Purchase_price_change.aspx.cs" Inherits="TalukderPOS.Web.UI.Purchase_price_change" Title="Purchase Price Amendment"
    EnableEventValidation="false" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="Javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <script type="text/javascript">

        function openPopup() {

            window.open("T_WGPGForm.aspx", "_blank", "WIDTH=1080,HEIGHT=790,scrollbars=no, menubar=no,resizable=yes,directories=no,location=no");

        }
        //<![CDATA[    
        //
</script>
    <style type="text/css">
        .txtStyle
        {
            background: white;
            border: 1px solid #ffa853;
            border-radius: 5px;
            box-shadow: 0 0 5px 3px #ffa853;
            color: #666;
            padding: 5px 10px;
            width: 300px;
            outline: none;
            font-family: Times New Roman,Arial,Sans-Serif;
            font-size: 25px;
            font-weight: bold;
            text-align: center;
        }
        
        #loginform
        {
            min-width: 200px;
            height: 140px;
            background-color: #ffffff;
            border: 1px solid;
            border-color: #555555;
            padding: 16px 16px;
            border-radius: 4px;
            -webkit-box-shadow: 0px 1px 6px rgba(75, 31, 57, 0.8);
            -moz-box-shadow: 0px 1px 6px rgba(75, 31, 57, 0.8);
            box-shadow: 0px 1px 6px rgba(223, 88, 13, 0.8);
        }
        .modalBackground
        {
            background-color: #333333;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .txt
        {
            color: #505050;
        }
        .redstar
        {
            color: #FF0000;
        }
        .hidden
        {
            display: none;
        }
        
        
        
         .Background
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .Popup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 400px;
            height: 350px;
        }
        .lbl
        {
            font-size:16px;
            font-style:italic;
            font-weight:bold;
        }
        
        
      
      
        #grid-header
        {
            background-color: #f49521;
        }
    </style>

    <script language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewContentPlace" runat="server">
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../Images/progress.gif" />
            Processing...</ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="tContT_PROD" runat="server" ActiveTabIndex="1" 
                Width="100%" CssClass="fancy fancy-green">
                
                <asp:TabPanel ID="tPnlDisplay2" runat="server" HeaderText="Purchase Price Amendment">
                    <ContentTemplate>
                        <table width="100%">
                            
                            <tr>
                                <td colspan="2" align="center" style="font-weight: bolder">
                                    Purchase Price Amendment
                                    <hr style="color: Green" />
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <td id="Td1" align="left" runat="server">
                                    &nbsp;<asp:Label ID="Label10" runat="server" Text="Product Category"></asp:Label>
                                </td>
                                <td id="Td2" runat="server">
                                    <div>
                                        <asp:DropDownList ID="ddlProductCategoryIdRET" runat="server" CssClass="form-control"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown7" runat="server" Category="Category"
                                            TargetControlID="ddlProductCategoryIdRET" LoadingText="Loading Categories..." PromptText="--Please Select--"
                                            ServiceMethod="BindCategory" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                        
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="Brand"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubCategoryIdProRET" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown8" runat="server" Category="Model" TargetControlID="ddlSubCategoryIdProRET"
                                        ParentControlID="ddlProductCategoryIdRET" LoadingText="Loading Model..." PromptText="--Please Select--"
                                        ServiceMethod="BindModel" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text="Description"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDescriptionPrpRET" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:CascadingDropDown ID="CascadingDropDown9" runat="server" Category="Description"
                                        TargetControlID="ddlDescriptionPrpRET" ParentControlID="ddlSubCategoryIdProRET"
                                        LoadingText="Loading Color..." PromptText="--Please Select--" ServiceMethod="BindDescription"
                                        ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                    </asp:CascadingDropDown>
                                </td>
                            </tr>
                            <tr id="tr2" runat="server">
                                <td id="Td3" align="left" runat="server">
                                    <asp:Label ID="Label18" runat="server" Text="Vendor"></asp:Label>
                                </td>
                                <td id="Td4" runat="server">
                                    <div>
                                        <asp:DropDownList ID="ddlVendorList2" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:CascadingDropDown ID="CascadingDropDown13" runat="server" Category="Vendor" TargetControlID="ddlVendorList2"
                                             LoadingText="Loading Vendor..." PromptText="--Please Select--"
                                            ServiceMethod="BindVendor" ServicePath="~/DropdownWebService.asmx" Enabled="True">
                                        </asp:CascadingDropDown>
                                    </div>
                                </td>
                                </tr>
                           
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label32" runat="server" Text="Return Amount"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReturnPrice" runat="server" CssClass="TextBox" onkeypress="return isNumberKey(event)" /><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator3" ControlToValidate="txtReturnPrice" ErrorMessage="Return Price is required"
                                        Display="Dynamic" runat="server" />
                                        
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="Amount Amendment" CssClass="btn btn-success"
                                        OnClick="btnChange_Click" CausesValidation="False" />&nbsp;&nbsp;
                                 
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label16" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        
                    </ContentTemplate>
                </asp:TabPanel>
               <%-- <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                </asp:TabPanel>--%>
            </asp:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
