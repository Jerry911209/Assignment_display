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
    public partial class User_register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            Inset_pass.Visible = false;
            Inset_failed.Visible = false;
        }

        protected void Btn_Register_Click(object sender, EventArgs e)
        {
            string contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";

            string name = userName.Text, id = userId.Text, birthday = userBirthday.Text, phone = userPhone.Text;
            string email = userEmail.Text, account = userAccount.Text, password = userPassword.Text, confirmPwd = userConfirmPwd.Text;
            bool repet = false;

            if (password != confirmPwd) //密碼與確認密碼不一致
            {
                Label2.Visible = true;
            }
            else
            {
                //SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlConnection conn = new SqlConnection(contion);

                conn.Open();

                SqlCommand cmd = new SqlCommand("Select * From [User]", conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string data_id = dr["User_Id"].ToString(), data_account = dr["user_account"].ToString();

                    if (data_id == id) //比對身分證字號
                    {
                        Label3.Visible = true;
                        repet = true;
                    }
                    if (data_account == account) //比對帳號名
                    {
                        Label1.Visible = true;
                        repet = true;
                    }
                }
                cmd.Cancel();
                dr.Close();
                conn.Close();
            }

            if (repet == false)
            {
                //try
                //{
                contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";
                SqlConnection Conn = new SqlConnection(contion);
                Conn.Open();
                SqlCommand Cmd = new SqlCommand("Insert Into [User] (User_Id, user_name, user_phone, user_email, user_birthday, user_account, user_password) Values ('" + id + "','" + name + "','" + phone + "','" + email + "','" + birthday + "','" + account + "','" + password + "')", Conn);

                Cmd.ExecuteNonQuery();

                Cmd.Cancel();
                Conn.Close();
                Conn.Dispose();

                Inset_pass.Visible = true;
                //}
                //catch
                //{
                //    Inset_failed.Visible = true;
                //}
            }


        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }
    }
}