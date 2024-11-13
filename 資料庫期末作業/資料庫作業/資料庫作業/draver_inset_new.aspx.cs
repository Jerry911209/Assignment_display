using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace 資料庫作業
{
    public partial class draver_inset_new : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label__again_arrow.Visible = false;
            Label__inset_failed.Visible = false;
            Label__account_repet.Visible = false;
            Label__inset_pass.Visible = false;
        }

        protected void Button2_Click(object sender, EventArgs e)//回登入頁面
        {
            Server.Transfer("driver_in.aspx");
        }

        protected void Button_Inset_Click(object sender, EventArgs e)//新增
        {
            string driver_birthday = TextBox_birthday.Text, driver_name = TextBox_name.Text, driver_phone = TextBox_phone.Text, driver_email = TextBox_email.Text;
            string driver_account = TextBox_account.Text, driver_password = TextBox_password.Text, driver_password_again = TextBox_password_again.Text, driver_car_number = TextBox_car_number.Text;
            int driver_car_sit = Convert.ToInt32(TextBox_car_sit_number.Text);

            bool repet = false;
            if (driver_password != driver_password_again)//防密碼不同
            {
                Label__again_arrow.Visible = true;
            }
            else
            {
                string contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";
                SqlConnection Conn = new SqlConnection(contion);

                Conn.Open();
                SqlCommand cmd = new SqlCommand("Select * From Driver", Conn);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string data_name = dr["driver_name"].ToString(), data_phone = dr["driver_phone"].ToString();

                    if (data_name == driver_name && data_phone == driver_phone)//比對帳號的姓名和電話
                    {
                        repet = true;
                        break;
                    }
                }

                cmd.Cancel();
                dr.Close();
                Conn.Close();

                if (repet == true)//帳號重複
                {
                    Label__account_repet.Visible = true;
                }
                else//帳號無重複
                {
                    try//新增
                    {
                        SqlConnection conn = new SqlConnection(contion);

                        conn.Open();
                        SqlCommand Cmd = new SqlCommand("Insert Into  Driver(driver_name,driver_phone,driver_email,driver_birthday,driver_account,driver_password,driver_car_number,driver_sit_number) Values ('" + driver_name + "','" + driver_phone + "','" + driver_email + "','" + driver_birthday + "','" + driver_account + "','" + driver_password + "','" + driver_car_number + "'," + driver_car_sit + ")", conn);

                        Cmd.ExecuteNonQuery();

                        Cmd.Cancel();
                        conn.Close();
                        conn.Dispose();

                        Label__inset_pass.Visible = true;
                    }
                    catch//新增失敗
                    {
                        Label__inset_failed.Visible = true;
                    }
                }
            }


        }
    }
}