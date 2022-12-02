using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmClientsInBannedList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine odbengine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "script1", "<Script Language='JavaScript'>height();</script> ");

                BindData();

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "script2", "<Script Language='JavaScript'>height();</script> ");

            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindData();

        }
        protected void BindData()
        {
            DataTable dt1 = new DataTable();

            dt1 = odbengine.GetDataTable("Master_BannedEntity", "BannedEntity_ID,Convert(varchar(11),BannedEntity_OrderDate,113) as BannedOrderDate,BannedEntity_Description as Description,BannedEntity_Pan as Pan,BannedEntity_NSECircularNumber as CircularNumber,BannedEntity_BanPeriod as BanPeriod,BannedEntity_CircularLink as CircularLink", null);

            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    //lblCustomerName.Text = dr["CNAME"].ToString();
                    //lblUCC.Text = dr["UCC"].ToString();
                    //lblBannedOrderDate.Text = Convert.ToDateTime(dr["BannedOrderDate"]).ToString("dd-MMM-yyyy");
                    //lblParticulars.Text = dr["Particulars"].ToString();
                    //lblBanPeriod.Text = dr["BanPeriod"].ToString();
                    //lblCircularLink.Text = dr["BannedEntity_CircularLink"].ToString();
                    //Session["BannedEntityCircularLink"] = dr["CircularLink"].ToString();

                }
                GridView1.DataSource = dt1;
                GridView1.DataBind();
            }
            else
            {
                Label3.Text = "No Record Found....";
                //Response.Write("No Record Found....");


            }
        }
        protected void btnPanNumber_Click(object sender, EventArgs e)
        {
            string pannumber = txtPanNumber.Text.ToString();
            DataTable dt2 = new DataTable();

            dt2 = odbengine.GetDataTable("Master_BannedEntity", "BannedEntity_ID,Convert(varchar(11),BannedEntity_OrderDate,113) as BannedOrderDate,BannedEntity_Description as Description,BannedEntity_Pan as Pan,BannedEntity_NSECircularNumber as CircularNumber,BannedEntity_BanPeriod as BanPeriod,BannedEntity_CircularLink as CircularLink", "BannedEntity_Pan='" + pannumber + "'");
            GridView1.DataSource = dt2;
            GridView1.DataBind();


        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}