<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="operator.aspx.cs" Inherits="資料庫作業._operator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="營運商業面"></asp:Label>
            <br />


            <br />
            今日日期:
            <asp:Label ID="Label_date_time" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="訂單資料"></asp:Label>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Order_Id" DataSourceID="SqlDataSource1" EmptyDataText="沒有資料錄可顯示。">
                <Columns>
                    <asp:BoundField DataField="Order_Id" HeaderText="Order_Id" ReadOnly="True" SortExpression="Order_Id" />
                    <asp:BoundField DataField="user_id" HeaderText="user_id" SortExpression="user_id" />
                    <asp:BoundField DataField="order_pick_up_location" HeaderText="order_pick_up_location" SortExpression="order_pick_up_location" />
                    <asp:BoundField DataField="order_pick_up_time" HeaderText="order_pick_up_time" SortExpression="order_pick_up_time" />
                    <asp:BoundField DataField="order_accompany_person_number" HeaderText="order_accompany_person_number" SortExpression="order_accompany_person_number" />
                    <asp:BoundField DataField="order_get_of_location" HeaderText="order_get_of_location" SortExpression="order_get_of_location" />
                    <asp:BoundField DataField="order_locate_time" HeaderText="order_locate_time" SortExpression="order_locate_time" />
                    <asp:BoundField DataField="order_user_level" HeaderText="order_user_level" SortExpression="order_user_level" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" DeleteCommand="DELETE FROM [User_order] WHERE [Order_Id] = @Order_Id" InsertCommand="INSERT INTO [User_order] ([user_id], [order_pick_up_location], [order_pick_up_time], [order_accompany_person_number], [order_get_of_location], [order_locate_time], [order_user_level]) VALUES (@user_id, @order_pick_up_location, @order_pick_up_time, @order_accompany_person_number, @order_get_of_location, @order_locate_time, @order_user_level)" SelectCommand="SELECT [Order_Id], [user_id], [order_pick_up_location], [order_pick_up_time], [order_accompany_person_number], [order_get_of_location], [order_locate_time], [order_user_level] FROM [User_order] WHERE ([order_pick_up_time] = @order_pick_up_time)" UpdateCommand="UPDATE [User_order] SET [user_id] = @user_id, [order_pick_up_location] = @order_pick_up_location, [order_pick_up_time] = @order_pick_up_time, [order_accompany_person_number] = @order_accompany_person_number, [order_get_of_location] = @order_get_of_location, [order_locate_time] = @order_locate_time, [order_user_level] = @order_user_level WHERE [Order_Id] = @Order_Id">
                <DeleteParameters>
                    <asp:Parameter Name="Order_Id" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="user_id" Type="Int32" />
                    <asp:Parameter Name="order_pick_up_location" Type="String" />
                    <asp:Parameter Name="order_pick_up_time" Type="String" />
                    <asp:Parameter Name="order_accompany_person_number" Type="Int32" />
                    <asp:Parameter Name="order_get_of_location" Type="String" />
                    <asp:Parameter Name="order_locate_time" Type="String" />
                    <asp:Parameter Name="order_user_level" Type="Int32" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="Label_date_time" Name="order_pick_up_time" PropertyName="Text" Type="String" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="user_id" Type="Int32" />
                    <asp:Parameter Name="order_pick_up_location" Type="String" />
                    <asp:Parameter Name="order_pick_up_time" Type="String" />
                    <asp:Parameter Name="order_accompany_person_number" Type="Int32" />
                    <asp:Parameter Name="order_get_of_location" Type="String" />
                    <asp:Parameter Name="order_locate_time" Type="String" />
                    <asp:Parameter Name="order_user_level" Type="Int32" />
                    <asp:Parameter Name="Order_Id" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            司機名單<br />
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="Driver_Id" DataSourceID="SqlDataSource2" EmptyDataText="沒有資料錄可顯示。">
                <Columns>
                    <asp:BoundField DataField="Driver_Id" HeaderText="Driver_Id" ReadOnly="True" SortExpression="Driver_Id" />
                    <asp:BoundField DataField="driver_name" HeaderText="driver_name" SortExpression="driver_name" />
                    <asp:BoundField DataField="driver_phone" HeaderText="driver_phone" SortExpression="driver_phone" />
                    <asp:BoundField DataField="driver_email" HeaderText="driver_email" SortExpression="driver_email" />
                    <asp:BoundField DataField="driver_birthday" HeaderText="driver_birthday" SortExpression="driver_birthday" />
                    <asp:BoundField DataField="driver_account" HeaderText="driver_account" SortExpression="driver_account" />
                    <asp:BoundField DataField="driver_password" HeaderText="driver_password" SortExpression="driver_password" />
                    <asp:BoundField DataField="driver_car_number" HeaderText="driver_car_number" SortExpression="driver_car_number" />
                    <asp:BoundField DataField="driver_sit_number" HeaderText="driver_sit_number" SortExpression="driver_sit_number" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" DeleteCommand="DELETE FROM [Driver] WHERE [Driver_Id] = @Driver_Id" InsertCommand="INSERT INTO [Driver] ([driver_name], [driver_phone], [driver_email], [driver_birthday], [driver_account], [driver_password], [driver_car_number], [driver_sit_number]) VALUES (@driver_name, @driver_phone, @driver_email, @driver_birthday, @driver_account, @driver_password, @driver_car_number, @driver_sit_number)" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT [Driver_Id], [driver_name], [driver_phone], [driver_email], [driver_birthday], [driver_account], [driver_password], [driver_car_number], [driver_sit_number] FROM [Driver]" UpdateCommand="UPDATE [Driver] SET [driver_name] = @driver_name, [driver_phone] = @driver_phone, [driver_email] = @driver_email, [driver_birthday] = @driver_birthday, [driver_account] = @driver_account, [driver_password] = @driver_password, [driver_car_number] = @driver_car_number, [driver_sit_number] = @driver_sit_number WHERE [Driver_Id] = @Driver_Id">
                <DeleteParameters>
                    <asp:Parameter Name="Driver_Id" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="driver_name" Type="String" />
                    <asp:Parameter Name="driver_phone" Type="String" />
                    <asp:Parameter Name="driver_email" Type="String" />
                    <asp:Parameter Name="driver_birthday" Type="String" />
                    <asp:Parameter Name="driver_account" Type="String" />
                    <asp:Parameter Name="driver_password" Type="String" />
                    <asp:Parameter Name="driver_car_number" Type="String" />
                    <asp:Parameter Name="driver_sit_number" Type="Int32" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="driver_name" Type="String" />
                    <asp:Parameter Name="driver_phone" Type="String" />
                    <asp:Parameter Name="driver_email" Type="String" />
                    <asp:Parameter Name="driver_birthday" Type="String" />
                    <asp:Parameter Name="driver_account" Type="String" />
                    <asp:Parameter Name="driver_password" Type="String" />
                    <asp:Parameter Name="driver_car_number" Type="String" />
                    <asp:Parameter Name="driver_sit_number" Type="Int32" />
                    <asp:Parameter Name="Driver_Id" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <br />
            <asp:Label ID="Label3" runat="server" Text="輸入配對資料"></asp:Label>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label4" runat="server" Text="司機id:"></asp:Label>
            <asp:TextBox ID="TextBox_driver" runat="server" TextMode="Number">0</asp:TextBox>
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label5" runat="server" Text="訂單id:"></asp:Label>
            <asp:TextBox ID="TextBox_order" runat="server" TextMode="Number">0</asp:TextBox>
            <br />
            <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="輸入完成" />
            <br />
            <br />
            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataKeyNames="Match_Id" DataSourceID="SqlDataSource3" EmptyDataText="沒有資料錄可顯示。">
                <Columns>
                    <asp:BoundField DataField="Match_Id" HeaderText="Match_Id" ReadOnly="True" SortExpression="Match_Id" />
                    <asp:BoundField DataField="Order_id" HeaderText="Order_id" SortExpression="Order_id" />
                    <asp:BoundField DataField="Driver_id" HeaderText="Driver_id" SortExpression="Driver_id" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" DeleteCommand="DELETE FROM [Match_order] WHERE [Match_Id] = @Match_Id" InsertCommand="INSERT INTO [Match_order] ([Order_id], [Driver_id]) VALUES (@Order_id, @Driver_id)" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT [Match_Id], [Order_id], [Driver_id] FROM [Match_order]" UpdateCommand="UPDATE [Match_order] SET [Order_id] = @Order_id, [Driver_id] = @Driver_id WHERE [Match_Id] = @Match_Id">
                <DeleteParameters>
                    <asp:Parameter Name="Match_Id" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="Order_id" Type="Int32" />
                    <asp:Parameter Name="Driver_id" Type="Int32" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Order_id" Type="Int32" />
                    <asp:Parameter Name="Driver_id" Type="Int32" />
                    <asp:Parameter Name="Match_Id" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <br />
            <br />
            <br />


        </div>
    </form>
</body>
</html>
