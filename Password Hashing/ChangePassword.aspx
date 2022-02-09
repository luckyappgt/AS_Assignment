<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Password_Hashing.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_newpwd.ClientID %>').value;

            if (str.length < 12) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password Length Must be at Least 12 Characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTMl = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 lowercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 uppercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_special_character")
            }
            document.getElementById("lbl_pwdchecker").innerHTML = "";

        }

        function matchpassword() {
            var pwd1 = document.getElementById('<%=tb_newpwd.ClientID %>').value;
            var pwd2 = document.getElementById('<%=tb_cfpwd.ClientID %>').value;
            if (pwd1 != pwd2) {
                document.getElementById("lbl_pwdmatch").innerHTML = "Password does not match";
                document.getElementById("lbl_pwdmatch").style.color = "Red";
                return ("no_match")
            }
            document.getElementById("lbl_pwdmatch").innerHTML = "";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <h2>
                <br />
                <asp:Label ID="Label1" runat="server" Text="Change of Password"></asp:Label>
                <br />
                <br />
            </h2>
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="Old Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_oldpwd" runat="server" Height="32px" Width="281px" TextMode="Password"></asp:TextBox>
                        <asp:Label ID="lbl_oldmatch" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="New Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_newpwd" runat="server" Height="32px" Width="281px" onkeyup="javascript:validate()" TextMode="Password"></asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label4" runat="server" Text="Confirm Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_cfpwd" runat="server" Height="32px" Width="281px" onkeyup="javascript:matchpassword()" TextMode="Password"></asp:TextBox>
                        <asp:Label ID="lbl_pwdmatch" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style4"></td>
                    <td class="style5">
                        <asp:Button ID="btn_Submit" runat="server" Height="48px"
                            OnClick="btn_Submit_Click" Text="Submit" Width="288px" />
                    </td>
                </tr>
            </table>
            &nbsp;<br />
            <asp:Label ID="lb_error1" runat="server"></asp:Label>
            <br />
            <br />
            <br />

        </div>
    </form>
</body>
</html>
