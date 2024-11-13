<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_register.aspx.cs" Inherits="資料庫作業.User_register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="background-color: #bdd8eb">
     <form id="form1" runat="server">
            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="14pt" Text="註冊使用者帳號"></asp:Label>
            <label>
            <br /> 
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 帳號：</label>
            <asp:TextBox ID="userAccount" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="userAccount" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
            <label>
            <asp:Label ID="Label1" runat="server" ForeColor="Red" Visible="False">帳號名已存在</asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 密碼：</label>
            <asp:TextBox ID="userPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="userPassword" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
            <label>
            <br />
            &nbsp;&nbsp;&nbsp; 確認密碼：</label>
            <asp:TextBox ID="userConfirmPwd" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="userConfirmPwd" ErrorMessage="*" Font-Size="12pt" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:Label ID="Label2" runat="server" ForeColor="Red" Visible="False">密碼與確認密碼不一致</asp:Label>
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 姓名：<asp:TextBox ID="userName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="userName" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            身分證字號：<asp:TextBox ID="userId" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="userId" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
            <label>
            <asp:Label ID="Label3" runat="server" ForeColor="Red" Visible="False">此身分證帳號已註冊過</asp:Label>
            </label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 生日：<asp:TextBox ID="userBirthday" runat="server" TextMode="Date"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="userBirthday" Display="Dynamic" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 電話：<asp:TextBox ID="userPhone" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="userPhone" Display="Dynamic" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="userPhone" ErrorMessage="電話規則錯誤" ForeColor="#FF3300" ValidationExpression="^09[0-9]{8}$" Display="Dynamic"></asp:RegularExpressionValidator>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 信箱：<asp:TextBox ID="userEmail" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="userEmail" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="userEmail" ErrorMessage="信箱規則錯誤" ForeColor="#FF3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnRegister" runat="server" Text="註冊" OnClick="Btn_Register_Click" Width="50px" style="height: 27px" />
            <br />
            <asp:Label ID="Inset_pass" runat="server" ForeColor="#6600FF" Text="*新增成功" Visible="False"></asp:Label>
            &nbsp;&nbsp;&nbsp;
            <br />
            <asp:Label ID="Inset_failed" runat="server" ForeColor="#FF3300" Text="*新增失敗" Visible="False"></asp:Label>
            <br />
            <br />
            &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/Start.aspx" CausesValidation="False" OnClick="LinkButton1_Click">返回主頁</asp:LinkButton>

    </form>
</body>
</html>
