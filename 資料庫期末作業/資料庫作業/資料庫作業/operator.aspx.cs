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
    public partial class _operator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            Label_date_time.Text = Date;



        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            string contion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database_work.mdf;Integrated Security=True";//建立

            int driver_id = Convert.ToInt32(TextBox_driver.Text);
            int order_id = Convert.ToInt32(TextBox_order.Text);

            SqlConnection conn = new SqlConnection(contion);

            conn.Open();
            SqlCommand Cmd = new SqlCommand("Insert Into  Match_order(Order_id,Driver_id) Values (" + order_id + "," + driver_id + ")", conn);

            Cmd.ExecuteNonQuery();

            Cmd.Cancel();
            conn.Close();
            conn.Dispose();

            Response.Redirect(Request.FilePath);
        }
    }
}