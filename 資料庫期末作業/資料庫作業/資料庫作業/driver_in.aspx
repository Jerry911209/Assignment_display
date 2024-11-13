<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="driver_in.aspx.cs" Inherits="資料庫作業.driver" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="background-color: #bdd8eb">
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="司機登入介面" Font-Bold="True" Font-Size="14pt"></asp:Label>
            <br />
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label2" runat="server" Text="帳號"></asp:Label>
            <asp:TextBox ID="TextBox_account" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_account" ErrorMessage="*帳號輸入為空白" ForeColor="#FF3300"></asp:RequiredFieldValidator>
            <br />
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label3" runat="server" Text="密碼"></asp:Label>
            <asp:TextBox ID="TextBox_password" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox_password" ErrorMessage="*密碼輸入為空白" ForeColor="#FF3300"></asp:RequiredFieldValidator>
            <br />
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
            <asp:Button ID="Button_log_in" runat="server" OnClick="Button_log_in_Click" Text="登入" />
            <br />
            <br />
            <asp:Button ID="Button_create" runat="server" OnClick="Button_create_Click" style="height: 27px" Text="新增帳號" CausesValidation="False" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            <br />
            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/Start.aspx" CausesValidation="False" OnClick="LinkButton1_Click">返回主頁</asp:LinkButton>

            <br />
        </div>
    </form>
</body>
</html>
