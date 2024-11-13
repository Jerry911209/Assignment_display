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
    public partial class driver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button_log_in_Click(object sender, EventArgs e)//登入
        {
            string account = TextBox_account.Text;
            string password = TextBox_password.Text;

            int driver_id = 0;
            bool find_driver = false;
            string contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";
            SqlConnection Conn = new SqlConnection(contion);

            Conn.Open();
            SqlCommand cmd = new SqlCommand("Select * From Driver", Conn);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string data_account = dr["driver_account"].ToString(), data_password = dr["driver_password"].ToString();

                if (account == data_account && password == data_password)
                {
                    driver_id = Convert.ToInt32(dr["Driver_Id"]);
                    find_driver = true;
                    break;
                }
            }

            cmd.Cancel();
            dr.Close();
            Conn.Close();

            if(find_driver == true)
            {
               // Server.Transfer("driver_use_form.aspx");
                Response.Redirect("~/driver_use_form.aspx?id=" + driver_id);
            }
        }

        protected void Button_create_Click(object sender, EventArgs e)//新增帳號
        {
            Response.Redirect("~/draver_inset_new.aspx");
            //Server.Transfer("draver_inset_new.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Server.Transfer("start.aspx");

        }
    }
}