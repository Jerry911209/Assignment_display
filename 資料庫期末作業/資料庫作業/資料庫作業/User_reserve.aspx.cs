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
    public partial class User_reserve : System.Web.UI.Page
    {
        static string user_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            Label_pass.Visible = false;
            Label_Nopass.Visible = false;

            if (!IsPostBack)
            {
                user_id = Request.QueryString["id"];
            }
        }

        protected void btn_check_order_Click(object sender, EventArgs e)
        {
            bool time_arro = false;
            string Date = DateTime.Now.ToString("yyyy-MM-dd");

            string pick_up_time = order_Pickup_Time.Text, pick_up_location = order_Pickup_Location.Text, get_of_loction = order_Getof_Location.Text;
            int order_accompany_person_number = Convert.ToInt32(order_PeopleNum.Text), order_user_level = Convert.ToInt32(orderLevel.Text);
            string contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";

            //SqlConnection conn = new SqlConnection(contion);
            //conn.Open();

            //SqlCommand cmd = new SqlCommand("Select * From [User_order] Where user_id=" + user_id, conn);
            //SqlDataReader dr = cmd.ExecuteReader();

            //while (dr.Read())
            //{
            //    if (pick_up_time == dr["order_pick_up_time"].ToString())
            //    {
            //        time_arro = true;
            //        break;
            //    }
            //}

            //cmd.Cancel();
            //dr.Close();
            //conn.Close();

            //if (time_arro == false)
            //{
                SqlConnection Conn = new SqlConnection(contion);
                Conn.Open();

                SqlCommand Cmd = new SqlCommand("Insert Into User_order(user_id,order_accompany_person_number,order_user_level,order_pick_up_location,order_pick_up_time,order_get_of_location,order_locate_time) Values ('" + user_id + "'," + order_accompany_person_number + "," + order_user_level + ",'" + pick_up_location + "','" + pick_up_time + "','" + get_of_loction + "','" + Date + "')", Conn);


                Cmd.ExecuteNonQuery();

                Cmd.Cancel();
                Conn.Close();
                Conn.Dispose();

                Label_pass.Visible = true;
            //}

        }

        protected void Btn_logout_Click(object sender, EventArgs e)
        {

        }
    }
}