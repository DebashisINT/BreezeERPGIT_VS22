using DataAccessLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class erpHolidayList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/erpHolidayList.aspx");
            if (!IsPostBack)
            {
                string output = string.Empty;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int NoOfRowEffected = 0;
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_HOLIDAYINSERT");
                    proc.AddVarcharPara("@Action", 500, "ALL");
                    proc.AddIntegerPara("@CREATE_BY", user_id);
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (NoOfRowEffected > 0)
                    {
                        output = "true";
                    }
                }
                else
                {

                }
            }
        }

        [WebMethod]
        public static string DeleteHoliday(int HolidayID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_HOLIDAYINSERT");
                    proc.AddIntegerPara("@HOLIDAYID", HolidayID);
                    proc.AddVarcharPara("@Action", 500, "DELETE");
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (NoOfRowEffected > 0)
                    {
                        output = "true";
                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "HOLIDAYID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.ERP_HOLIDAYVIEWs
                    //where d.ContactType == cust_type
                    orderby d.SEQ
                    select d;
            e.QueryableSource = q;
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

       
    }
}