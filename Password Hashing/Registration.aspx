<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Password_Hashing.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

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
            var pwd1 = document.getElementById('<%=tb_pwd.ClientID %>').value;
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
                <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
                <br />
                <br />
            </h2>
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label7" runat="server" Text="First Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_fname" runat="server" Height="36px" Width="280px">Jessica</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label8" runat="server" Text="Last Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_lname" runat="server" Height="36px" Width="280px">Tan</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Label ID="Label5" runat="server" Text="Credit Card Number"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:TextBox ID="tb_creditcard" runat="server" Height="32px" Width="281px">4242 4242 4242 4242</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="Email (UserID)"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_userid" runat="server" Height="36px" Width="280px">jayson@yahoo.com</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px" onkeyup="javascript:validate()">P@ssw0rd1234</asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label4" runat="server" Text="Confirm Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_cfpwd" runat="server" Height="32px" Width="281px" onkeyup="javascript:matchpassword()"></asp:TextBox>
                        <asp:Label ID="lbl_pwdmatch" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label6" runat="server" Text="Date of Birth"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_dob" runat="server" Height="32px" Width="281px">03-10-1997</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label9" runat="server" Text="Photo"></asp:Label>
                    </td>
                    <td class="style2">
                        <input id="oFile" type="file" runat="server" name="oFile" accept="image/*" />
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
