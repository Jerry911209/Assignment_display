<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_reserve.aspx.cs" Inherits="資料庫作業.User_reserve" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="background-color: #bdd8eb">
       <form id="form1" runat="server">
           <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Text="新增預約"></asp:Label>
           <label><br />
            <br />
            <br />
            &nbsp;&nbsp;&nbsp; 接送地點：</label>
        <asp:TextBox ID="order_Pickup_Location" runat="server"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="order_Pickup_Location" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>

        <label>
            <br />
            &nbsp;&nbsp;&nbsp; 接送時間：</label>
        <asp:TextBox ID="order_Pickup_Time" runat="server" TextMode="Date"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="order_Pickup_Time" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;目的地：<asp:TextBox ID="order_Getof_Location" runat="server"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="order_Getof_Location" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        &nbsp;&nbsp;&nbsp; 陪同人數：<asp:TextBox ID="order_PeopleNum" runat="server" TextMode="Number"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="order_PeopleNum" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
           <br />
           &nbsp;&nbsp;&nbsp;&nbsp;失能等級：<asp:TextBox ID="orderLevel" runat="server" TextMode="Number"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="orderLevel" Display="Dynamic" ErrorMessage="*" Font-Size="12pt" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        &nbsp;<br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label_pass" runat="server" ForeColor="#3399FF" Visible="False">新增成功</asp:Label>
           <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label_Nopass" runat="server" ForeColor="Red" Visible="False">新增失敗</asp:Label>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
 
            <asp:Button ID="btn_check_order" runat="server" Text="確定" Width="50px" OnClick="btn_check_order_Click" />

        <br />
        <br />
        <asp:LinkButton ID="Btn_Revise" runat="server" PostBackUrl="~/User_revise.aspx" Visible="False">修改個人資料</asp:LinkButton>

        <br />
            <asp:LinkButton ID="Btn_logout" runat="server" PostBackUrl="~/Start.aspx" CausesValidation="False" OnClick="Btn_logout_Click">登出</asp:LinkButton>

    </form>
</body>
</html>
