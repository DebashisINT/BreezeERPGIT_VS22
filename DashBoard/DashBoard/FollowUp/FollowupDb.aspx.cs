using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.FollowUp
{
    public partial class FollowupDb : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                FormDate.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);
                toDate.MinDate = new DateTime(2000, 1, 1, 1, 1, 1, 1);

                DataTable dt = oDBEngine.GetDataTable(@"select PendFollow,totFollow,ClsFollow,convRat,followHis,followUsing
                                            from tbl_master_dashboard_setting_details 
                                            where user_id=" + Convert.ToString(Session["userid"]));

                if (dt.Rows.Count > 0)
                {
                    PendingDivbtn.Visible = Convert.ToBoolean(dt.Rows[0]["PendFollow"]);
                    totFollowup.Visible = Convert.ToBoolean(dt.Rows[0]["totFollow"]);
                    closedFollowupBtn.Visible = Convert.ToBoolean(dt.Rows[0]["ClsFollow"]);
                    Conversionbtn.Visible = Convert.ToBoolean(dt.Rows[0]["convRat"]);
                    followhisBtn.Visible = Convert.ToBoolean(dt.Rows[0]["followHis"]);
                    followUnisngbtn.Visible = Convert.ToBoolean(dt.Rows[0]["followUsing"]);
                }



            }
           
        }

        protected void gridEF_DataBinding(object sender, EventArgs e)
        {
          
        }

       
    }
}