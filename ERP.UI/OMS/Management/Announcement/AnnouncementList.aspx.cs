using DataAccessLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Announcement
{
    public partial class AnnouncementList : System.Web.UI.Page
    {

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Announcement/AnnouncementList.aspx");
            ASPxGridView1.JSProperties["cpRetMsg"] = null;
            if (!IsPostBack)
            {
               
            }

        }

        protected void LinqServerModeDataSource1Count_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
           
            e.KeyExpression = "AncId";
            
            
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.tbl_Trans_announcements  
                    select d;
            e.QueryableSource = q;
             
        }

        protected void ASPxGridView1_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_AnnouncementAddEdit");
            proc.AddPara("@Action", "Delete"); 
            proc.AddPara("@id", e.Parameters); 
            proc.RunActionQuery();
            ASPxGridView1.JSProperties["cpRetMsg"] = "Announcement Deleted Successfully.";
            ASPxGridView1.DataBind();
        }
    }
}