using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace 資料庫作業
{
    public partial class User_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Btn_Check_Click(object sender, EventArgs e)
        {
            string contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(contion);
            conn.Open();

            bool pass = true;
            string User_id = "";
            string txtName = txtAccount.Text;
            string txtPsd = txtPassword.Text;

            SqlCommand cmd = new SqlCommand("Select * From [User]", conn);
            //cmd.Parameters.Add(new SqlParameter("@account", txtName));
            //cmd.Parameters.Add(new SqlParameter("@psd", txtPsd));

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["user_account"].ToString() == txtName && dr["user_password"].ToString() == txtPsd)
                {
                    User_id = dr["User_id"].ToString();
                    txtAccount.Text = "";
                    txtPassword.Text = "";
                    Label1.Visible = false;

                    Response.Redirect("~/User_reserve.aspx?id=" + User_id); //到預約頁面
                }
                else
                {
                    pass = false;
                }
            }
            if (pass == false)
            {
                Label1.Text = "輸入有誤！";
            }
            conn.Close();
            cmd.Dispose();
        }

        protected void Btn_Clear_Click(object sender, EventArgs e)
        {
            txtAccount.Text = "";
            txtPassword.Text = "";
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Server.Transfer("start.aspx");
        }
    }
}