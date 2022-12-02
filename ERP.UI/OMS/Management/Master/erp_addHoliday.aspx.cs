using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class erp_addHoliday : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["HolidayDetails"] != null)
                {
                    Session["HolidayDetails"] = null;
                }

                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    PopulateBranchByHierchy(userbranchHierachy);
                    cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
                }
                if (Request.QueryString["Key"] != "ADD")
                {
                    PopulateHolidayDetailsById(Request.QueryString["id"]);

                    Hidden_add_edit.Value = Request.QueryString["Key"];
                    Hidn_team_id.Value = Request.QueryString["id"];
                    if (Request.QueryString["Key"] == "edit")
                    {
                        HeaderName.Text = " Edit Holiday";
                    }
                    else
                    {
                        HeaderName.Text = " View Holiday";
                    }
                }
                else
                {
                    Hidden_add_edit.Value = "add";
                    HeaderName.Text = " Add Holiday";
                }
            }
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;
        }

        [WebMethod]
        public static String AddData(string Name, string FromDate, string todate, String Guids)
        {
            //DataTable dt = new DataTable();
            //Session
            //dt.Columns.Add("HIddenID", typeof(int));
            //dt.Columns.Add("Name", typeof(String));
            //dt.Columns.Add("FrmDate", typeof(String));
            //dt.Columns.Add("ToDate", typeof(String));  


            DataTable dt = (DataTable)HttpContext.Current.Session["HolidayDetails"];

            if (dt == null)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("HIddenID", typeof(System.Guid));
                dtable.Columns.Add("Name", typeof(System.String));
                dtable.Columns.Add("FrmDate", typeof(System.String));
                dtable.Columns.Add("ToDate", typeof(System.String));
                object[] trow = { Guid.NewGuid(), Name, FromDate, todate };// Add new parameter Here
                dtable.Rows.Add(trow);
                HttpContext.Current.Session["HolidayDetails"] = dtable;
            }
            else
            {
                if (string.IsNullOrEmpty(Guids))
                {
                    object[] trow = { Guid.NewGuid(), Name, FromDate, todate };// Add new parameter Here
                    dt.Rows.Add(trow);
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (Guids.ToString() == item["HIddenID"].ToString())
                            {
                                item["Name"] = Name;
                                item["FrmDate"] = FromDate;
                                item["ToDate"] = todate;
                            }
                        }
                    }
                }
                HttpContext.Current.Session["HolidayDetails"] = dt;
            }

            return "Holiday is Added Successfully.";
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["HolidayDetails"] != null)
            {
                GrdHoliday.DataSource = (DataTable)Session["HolidayDetails"];
            }
        }

        protected void GrdHoliday_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (Session["HolidayDetails"] != null)
            {
                GrdHoliday.DataBind();
            }
        }

        [WebMethod]
        public static string save(HolidayMain apply)
        {
            string output = string.Empty;

            int NoOfRowEffected = 0;
            DataTable dt = (DataTable)HttpContext.Current.Session["HolidayDetails"];

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        ProcedureExecute proc = new ProcedureExecute("PRC_HOLIDAYINSERT");
                        proc.AddVarcharPara("@BRANCH", 25, Convert.ToString(apply.branch));
                        proc.AddVarcharPara("@HOLIDAY_CODE", 100, Convert.ToString(apply.holidayCode).Trim());
                        proc.AddVarcharPara("@HOLIDAY_DESC", 500, Convert.ToString(apply.holidayName).Trim());
                        proc.AddVarcharPara("@FROMDATE", 10, Convert.ToString(apply.fromdate));
                        proc.AddVarcharPara("@TODATE", 10, Convert.ToString(apply.todate));
                        proc.AddIntegerPara("@CREATE_BY", user_id);
                        proc.AddPara("@PARAMTABLE", dt);
                        proc.AddVarcharPara("@Action", 30, Convert.ToString(apply.Action).Trim());
                        proc.AddVarcharPara("@HOLIDAYID", 25, Convert.ToString(apply.holiday_ID));
                        proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                        NoOfRowEffected = proc.RunActionQuery();
                        output = Convert.ToString(proc.GetParaValue("@is_success"));
                        //if (NoOfRowEffected > 0)
                        //{
                            output = "true";
                        //}
                    }
                }
                else
                {
                    output = "Please Add Holiday Details.";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        [WebMethod]
        public static String DeleteData(string HiddenID)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["HolidayDetails"];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        dt.Rows.Remove(item);
                        break;
                    }
                }
            }
            return "Holiday is Remove Successfully.";
        }

        [WebMethod]
        public static HolidayDetails EditData(String HiddenID)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["HolidayDetails"];
            HolidayDetails ret = new HolidayDetails();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        ret.holidayName = item["Name"].ToString();
                        ret.fromdate = Convert.ToDateTime(item["FrmDate"].ToString());
                        ret.todate = Convert.ToDateTime(item["ToDate"].ToString());
                        ret.Guid = item["HIddenID"].ToString();
                       // dt.Rows.Remove(item);
                        break;
                    }
                }
            }
            return ret;// "Holiday Remove Sucessfylly";
        }

        private void PopulateHolidayDetailsById(string HolidayID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_HOLIDAYINSERT");
                    proc.AddVarcharPara("@Action", 30, "SELECT");
                    proc.AddIntegerPara("@HOLIDAYID", Convert.ToInt32(HolidayID.ToString().Trim()));
                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    ds = proc.GetDataSet();
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    if (output == "true")
                    {
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            txt_HolidayCode_nm.Text = ds.Tables[0].Rows[0]["HOLIDAY_CODE"].ToString().Trim();
                            txt_HolidayCode_nm.ClientEnabled = false;

                            txt_HolidayDes_nm.Text = ds.Tables[0].Rows[0]["HOLIDAY_DESC"].ToString().Trim();
                            txt_HolidayDes_nm.ClientEnabled = false;

                            FormDate.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["FROMDATE"].ToString().Trim());
                            FormDate.ClientEnabled = false;

                            toDate.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["TODATE"].ToString().Trim());
                            toDate.ClientEnabled = false;

                            cmbBranchfilter.Value = Convert.ToString(ds.Tables[0].Rows[0]["BRANCH"].ToString().Trim());

                            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                            {
                                Session["HolidayDetails"] = ds.Tables[1];
                                GrdHoliday.DataBind();
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
        }
    }
}