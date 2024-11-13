using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 資料庫作業
{
    public partial class start : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button__user_login_Click(object sender, EventArgs e)//使用者登入
        {
            Server.Transfer("User_login.aspx");
        }

        protected void Button_user_inset_Click(object sender, EventArgs e)//使用者新增
        {
            Server.Transfer("User_register.aspx");

        }

        protected void Button_driver_in_Click(object sender, EventArgs e)//進入司機葉面
        {
            Server.Transfer("driver_in.aspx");

        }
    }
}