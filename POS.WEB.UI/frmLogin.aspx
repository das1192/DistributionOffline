<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmLogin.aspx.cs" Inherits="frmLogin"
    EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <title>Welcome to MMSL POS Managment Solutions</title>
    <link href="Styles/login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css">
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" href="Styles/AdminLTE.css" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 179px;
            height: 106px;
        }
    </style>
    <link href="Styles/loginstyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class=" demo">
        <div id="large-header" class="large-header">
            <canvas id="demo-canvas"></canvas>
            <div class="form-box main-title " id="login-box">
                <div class="header myheader ">
                </div>
                <div class="body bg-das" style="">
                    <div class="form-group">
                        <h2>
                            Point of Sale
                        </h2>
                    </div>
                    <hr />
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                            <asp:TextBox ID="txtusername" runat="server" CssClass="form-control" value="user name ........."
                                onfocus="if(this.value=='user name .........')this.value=''" onblur="if(this.value=='')this.value='user name .........'"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></span>
                            <asp:TextBox ID="txtpassword" runat="server" Text="Password" TextMode="Password"
                                CssClass="form-control" value="password" onfocus="if(this.value=='password')this.value=''"
                                onblur="if(this.value=='')this.value='password'"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblMessageInvalid" runat="server" ForeColor="#CC0000" Font-Bold="True"
                            Font-Size="Large"></asp:Label>
                    </div>
                    <div class="form-group">
                        <div>
                        <asp:Button ID="btnLogin" runat="server" ValidationGroup="us" CssClass="btn bg-olive btn-block"
                            OnClick="btnLogin_Click" Text="Sign Me In" />
                        
                        </div>
                        <hr />
                        <div class="gamelink">
                            <a href="pacman.htm">Play Game</a></div>
                    </div>
                    
                </div>
                <%--<div class="footer">
                </div>
                <table style="display:none;">
                    <tr id="msgtr" runat="server" visible="false">
                        <td class="style190">
                            &nbsp;
                        </td>
                        <td class="style190" colspan="2">
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="False" BackColor="White"
                                ForeColor="#CC0000"></asp:Label>
                        </td>
                    </tr>
                </table>--%>
            </div>
        </div>
    </div>
    </form>
    <script src="Scripts/TweenLite.min.js" type="text/javascript"></script>
    <script src="Scripts/EasePack.min.js" type="text/javascript"></script>
    <script src="Scripts/demo.js" type="text/javascript"></script>
</body>
</html>
