<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="frmAccessoriesEdit.aspx.cs"
    Inherits="Pages_frmAccessoriesEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset>
        <legend>Accessories Quantity Update</legend>
        <div>
            <table cellpadding="0" cellspacing="3px" border="0">              
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" CssClass="lbl" Text="Model" Font-Bold="true"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblModel" runat="server" CssClass="lbl" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" CssClass="lbl" Text="Color/Description" Font-Bold="true"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblDescription" runat="server" CssClass="lbl" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" CssClass="lbl" Text="SES Price" Font-Bold="true"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtCostPrice" runat="server" Font-Size="14px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" CssClass="lbl" Text="MRP" Font-Bold="true"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtSalePrice" runat="server" Font-Size="14px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" CssClass="lbl" Text="Quantity" Font-Bold="true"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server" Font-Size="14px"></asp:TextBox>
                    </td>
                </tr>

                  <tr>
                    <td colspan="3">
                        <asp:Label ID="lblMessage" runat="server" CssClass="lbl" Font-Bold="true" ForeColor="Red"></asp:Label>
                    </td>                   
                    <td>
                        <asp:HiddenField ID="lblOID" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>


    <div>
        <table>
            <tr>
                <td align="center" colspan="3" style="padding-left:114px">
                    <asp:Button ID="Button2" runat="server" Text="Save" OnClick="Button2_Click" Width="100px"  />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
