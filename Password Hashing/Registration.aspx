<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Password_Hashing.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                    <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px">mypassword</asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="style3">
        <asp:Label ID="Label4" runat="server" Text="Confirm Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_cfpwd" runat="server" Height="32px" Width="281px"></asp:TextBox>
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
                <td class="style4">
       
                </td>
                <td class="style5">
    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
        onclick="btn_Submit_Click" Text="Submit" Width="288px" />
                </td>
            </tr>
    </table>
&nbsp;<br />
        <asp:Label ID="lb_error1" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lb_error2" runat="server"></asp:Label>
    <br />
        <br />
    
    </div>
    </form>
</body>
</html>
