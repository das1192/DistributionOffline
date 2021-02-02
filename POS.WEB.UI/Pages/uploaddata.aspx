<%@ Page Language="C#" AutoEventWireup="true" CodeFile="uploaddata.aspx.cs" Inherits="Pages_uploaddata" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center;">
    <h2 style="color:Green;text-align:center;">Keep your data safe. Upload to server from here. Only Zipped files are Accepted</h2>
     <asp:FileUpload ID="fileupload" runat="server"/>
     <asp:Button ID="btnSave" runat="server" Text="UPLOAD" OnClick="btnSave_Click"
                                        CssClass="btn btn-default" OnClientClick="this.disabled=true;" 
                                        UseSubmitBehavior="False" />&nbsp;&nbsp;
                                    
    </div>
     <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <p>
        &nbsp;</p>
    </form>
</body>
</html>
