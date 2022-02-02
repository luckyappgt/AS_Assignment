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
                <td class="style6">
        <asp:Label ID="Label5" runat="server" Text="NRIC"></asp:Label>
                </td>
                <td class="style7">
                    <asp:TextBox ID="tb_nric" runat="server" Height="32px" Width="281px">S8511018B</asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="style3">
        <asp:Label ID="Label6" runat="server" Text="Mobile"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(+65)
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_mobile" runat="server" Height="32px" Width="281px">81888188</asp:TextBox>
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
