<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="driver_use_form.aspx.cs" Inherits="資料庫作業.driver_use_form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-bottom: 0px;
        }
    </style>
</head>
<body style="background-color: #bdd8eb">
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="司機頁面" Font-Bold="True" Font-Size="14pt"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label_driver_id" runat="server" Visible="False"></asp:Label>
            <br />
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label2" runat="server" Text="訂單資訊"></asp:Label>
            <br />
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label_order_over" runat="server" ForeColor="#0066CC" Text="今日訂單已完成" Visible="False"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label_order_Id" runat="server"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label_order_pick_up_time" runat="server"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label_order_pick_up_location" runat="server"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label_order_accompany_person_number" runat="server"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label_order_get_of_location" runat="server"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label_order_user_level" runat="server"></asp:Label>
            <br />
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" CssClass="auto-style1" OnClick="Button1_Click" Text="訂單完成" />
            <br />
            <asp:Panel ID="Panel_report" runat="server" Visible="False">
                <br />
                <asp:Label ID="Label_report_order_Id" runat="server"></asp:Label>
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label3" runat="server" Text="輸入應收金額"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox_report_receivable_money" runat="server" TextMode="Number"></asp:TextBox>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label4" runat="server" Text="輸入全部金額"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox_report_total_money" runat="server" TextMode="Number"></asp:TextBox>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label5" runat="server" Text="輸入補助金額"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox_report_subaidy_money" runat="server" TextMode="Number"></asp:TextBox>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label6" runat="server" Text="輸入陪同人數"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox_report_accompany_person_number" runat="server" TextMode="Number"></asp:TextBox>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label7" runat="server" Text="輸入里程"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBox_repot_driving_km" runat="server" TextMode="Number"></asp:TextBox>
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="回報資料輸入完成" />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </asp:Panel>
            <asp:Label ID="Label_report" runat="server" ForeColor="#3366FF" Text="回報成功，將配置下一個訂單" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="Label_report_arro" runat="server" ForeColor="#FF3300" Text="回報失敗" Visible="False"></asp:Label>
            <br />
            <br />
            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/start.aspx">回主頁</asp:LinkButton>
            <br />
        </div>
    </form>
</body>
</html>
