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
    public partial class driver_use_form : System.Web.UI.Page
    {
        static int id_counter = 0, totali_d_count = 0;
        static int driver_id = 0;
        static int[] date_order_id = new int[100];
        static int[] data_total_orderid = new int[99999];



        protected void Page_Load(object sender, EventArgs e)
        {
            Label_report.Visible = false;
            Label_report_arro.Visible = false;

            if (!IsPostBack)
            {
                Label_order_Id.Visible = true;
                Label_order_pick_up_time.Visible = true;
                Label_order_pick_up_location.Visible = true;
                Label_order_accompany_person_number.Visible = true;
                Label_order_user_level.Visible = true;

                for (int i1 = 0; i1 < 99999; i1++)
                {
                    data_total_orderid[i1] = 0;
                }
                for (int i1 = 0; i1 < 100; i1++)
                {
                    date_order_id[i1] = 0;
                }

                driver_id = Convert.ToInt32(Request.QueryString["id"]);//讀取前一業資料
                Label_driver_id.Text = driver_id.ToString();

                string contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";
                SqlConnection Conn = new SqlConnection(contion);

                Conn.Open();
                SqlCommand Cmd = new SqlCommand("Select * From Match_order Where Driver_id=" + driver_id, Conn);//找司機的訂單

                SqlDataReader Dr = Cmd.ExecuteReader();

                int i = 0;
                while (Dr.Read())
                {
                    int order_id = Convert.ToInt32(Dr["Order_id"]);



                    data_total_orderid[i] = order_id;

                    i++;

                }

                Cmd.Cancel();
                Dr.Close();
                Conn.Close();

                for (int j = 0; j < 9999; j++)
                {
                    if (data_total_orderid[j] != 0)
                    {
                        SqlConnection conn = new SqlConnection(contion);
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("Select * From User_order Where Order_Id=" + data_total_orderid[j], conn);//找到司機的訂單之後再找今日訂單存在陣列中
                        int l = 0;
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {

                            string time = dr["order_pick_up_time"].ToString();

                            string Date = DateTime.Now.ToString("yyyy-MM-dd");

                            if (time == Date)
                            {
                                date_order_id[l] = Convert.ToInt32(dr["Order_Id"]);
                                l++;
                            }

                        }
                        cmd.Cancel();
                        dr.Close();
                        conn.Close();
                    }

                }


                SqlConnection driver_conn = new SqlConnection(contion);//第一個訂單資料
                driver_conn.Open();

                SqlCommand driver_cmd = new SqlCommand("Select * From User_order Where Order_Id=" + date_order_id[id_counter], driver_conn);//找今日第一個訂單

                SqlDataReader driver_dr = driver_cmd.ExecuteReader();
                while (driver_dr.Read())
                {
                    Label_order_Id.Text = "訂單編號為:" + driver_dr["Order_Id"].ToString();
                    Label_order_pick_up_time.Text = "上車時間為:" + driver_dr["order_pick_up_time"].ToString();
                    Label_order_pick_up_location.Text = "上車地點為:" + driver_dr["order_pick_up_location"].ToString();
                    Label_order_accompany_person_number.Text = "下車地點為:" + driver_dr["order_get_of_location"].ToString();
                    Label_order_user_level.Text = "搭乘者失能等極為:" + driver_dr["order_user_level"].ToString();

                }
                driver_cmd.Cancel();
                driver_dr.Close();
                driver_conn.Close();

            }
        }

        protected void Button1_Click(object sender, EventArgs e)//訂單完成顯示panel
        {
            Panel_report.Visible = true;
            Label_report_order_Id.Text = Label_order_Id.Text;

        }

        protected void Button2_Click(object sender, EventArgs e)//回報輸入完成
        {
            try
            {
                string contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";//建立

                int account_person = Convert.ToInt32(TextBox_report_accompany_person_number.Text);
                double receivable_money = Convert.ToDouble(TextBox_report_receivable_money.Text), report_total_money = Convert.ToDouble(TextBox_report_total_money.Text);
                double report_subaidy_money = Convert.ToDouble(TextBox_report_subaidy_money.Text), repot_driving_km = Convert.ToDouble(TextBox_repot_driving_km.Text);

                SqlConnection conn = new SqlConnection(contion);

                conn.Open();
                SqlCommand Cmd = new SqlCommand("Insert Into  Driver_report(Driver_id,Order_id,report_receivable_money,report_total_money,report_subaidy_money,report_accompany_person_number,repot_driving_km) Values (" + Convert.ToInt32(driver_id) + "," + Convert.ToInt32(date_order_id[totali_d_count]) + "," + receivable_money + "," + report_total_money + "," + report_subaidy_money + "," + account_person + "," + repot_driving_km + ")", conn);



                Cmd.ExecuteNonQuery();

                Cmd.Cancel();
                conn.Close();
                conn.Dispose();


                totali_d_count++;
                Label_report.Visible = true;
                id_counter++;

                if (date_order_id[id_counter] == 0)//下一個訂單ID為0
                {
                    Label_order_over.Visible = true;

                    Label_order_Id.Visible = false;
                    Label_order_pick_up_time.Visible = false;
                    Label_order_pick_up_location.Visible = false;
                    Label_order_accompany_person_number.Visible = false;
                    Label_order_user_level.Visible = false;
                }
                else
                {
                    SqlConnection driver_conn = new SqlConnection(contion);//下一個訂單ID不為0，顯示接下來的訂單資料
                    driver_conn.Open();

                    SqlCommand driver_cmd = new SqlCommand("Select * From User_order Where Order_Id=" + date_order_id[id_counter], driver_conn);//找今日第一個訂單

                    SqlDataReader driver_dr = driver_cmd.ExecuteReader();
                    while (driver_dr.Read())
                    {

                        Label_order_Id.Text = "訂單編號為:" + driver_dr["Order_Id"].ToString();
                        Label_order_pick_up_time.Text = "上車時間為:" + driver_dr["order_pick_up_time"].ToString();
                        Label_order_pick_up_location.Text = "上車地點為:" + driver_dr["order_pick_up_location"].ToString();
                        Label_order_accompany_person_number.Text = "下車地點為:" + driver_dr["order_get_of_location"].ToString();
                        Label_order_user_level.Text = "搭乘者失能等級為:" + driver_dr["order_user_level"].ToString();

                    }
                    driver_cmd.Cancel();
                    driver_dr.Close();
                    driver_conn.Close();
                    Panel_report.Visible = false;
                }

            }
            catch
            {
                Label_report_arro.Visible = true;
            }



        }
    }
}