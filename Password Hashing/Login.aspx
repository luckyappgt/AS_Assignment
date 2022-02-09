<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Password_Hashing.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LezjmEeAAAAAIJv31TlWdtqt81Act8wOQP4CR6r"></script>
</head>
<body>
       <form id="form1" runat="server">
    <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="style3">
        <asp:Label ID="Label2" runat="server" Text="User ID/Email"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_userid" runat="server" Height="16px" Width="280px" placeholder="example@email.com" TextMode="Email"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="16px" Width="281px" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="style3">
       
                </td>
                <td class="style2">
    <asp:Button ID="btn_login" runat="server" Height="48px" 
        onclick="btn_Login_Click" Text="Login" Width="288px" />
                </td>
            </tr>
    </table>
           <h2>
        <br />
        <asp:Label ID="Label4" runat="server">No Account? Click on the button below to register</asp:Label>
        <br />
   </h2>
           <asp:Button ID="btn_register" runat="server" Height="48px" 
        onclick="btn_register_Click" Text="Register" Width="288px" />

&nbsp;&nbsp;&nbsp;
    <br />
           <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" /><br />
           <br />
        <asp:Label ID="lb_error" runat="server"></asp:Label> <br />
           <asp:Label ID="lb_locked" runat="server"></asp:Label>
           <br />
           <br />
           <br />
        <br />
        <br />
   
    <div>
    
    </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LezjmEeAAAAAIJv31TlWdtqt81Act8wOQP4CR6r', { action: "Login" }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
