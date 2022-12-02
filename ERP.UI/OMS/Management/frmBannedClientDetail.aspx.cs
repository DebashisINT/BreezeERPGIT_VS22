using System;
using System.Data;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frmBannedClientDetail : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine odbengine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = "";
            str = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "script1", "<Script Language='JavaScript'>height();</script> ");
                DataTable dt1 = new DataTable();

                dt1 = odbengine.GetDataTable("Master_BannedEntity", "BannedEntity_ID,Convert(varchar(11),BannedEntity_OrderDate,113) as BannedOrderDate,BannedEntity_Particulars as Particulars,BannedEntity_Pan as Pan,BannedEntity_BanPeriod as BanPeriod,BannedEntity_Description as Description,Convert(varchar(11),BannedEntity_CircularDate,113) as CircularDate,BannedEntity_CircularLink as CircularLink", "BannedEntity_ID=" + str);

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {

                        lblBannedOrderDate.Text = dr["BannedOrderDate"].ToString();
                        lblParticulars.Text = dr["Particulars"].ToString();
                        lblBanPeriod.Text = dr["BanPeriod"].ToString();
                        lblBannedPan.Text = dr["Pan"].ToString();
                        lblCirculardate.Text = dr["CircularDate"].ToString();
                        lblDescription.Text = dr["Description"].ToString();
                        //Session["BannedEntityCircularLink"] = dr["CircularLink"].ToString();
                        CID.InnerHtml = dr["CircularLink"].ToString();
                        hdnlink.Value = dr["CircularLink"].ToString();
                    }

                }
                else
                {
                    lblBannedOrderDate.Visible = false;
                    lblParticulars.Visible = false;
                    lblBanPeriod.Visible = false;
                    lblCircularLink.Visible = false;
                    lblBannedPan.Visible = false;
                    lblCirculardate.Visible = false;
                    lblDescription.Visible = false;
                    //Session["BannedEntityCircularLink"] = dr["CircularLink"].ToString();
                    //CID.InnerHtml = dr["CircularLink"].ToString();
                    hdnlink.Visible = false;
                    Label3.Text = "No Record Found....";
                    //Response.Write("No Record Found....");


                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "script2", "<Script Language='JavaScript'>height();</script> ");

            }

        }
    }
}