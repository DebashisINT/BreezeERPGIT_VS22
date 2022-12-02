using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.STBManagement.STB
{
    public partial class setTopBoxAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AddmodeExecuted();
                if (Request.QueryString["Key"] != "Add")
                {
                    if (Request.QueryString["Key"] == "edit")
                    {
                        HeaderName.InnerHtml = "Modify Set Top Box";
                        hdAddEdit.Value = "Update";
                    }
                    else
                    {
                        HeaderName.InnerHtml = " View Set Top Box";
                        hdAddEdit.Value = "view";
                    }
                    string SetTopBoxID = Request.QueryString["id"];
                    hdnSetTopBoxID.Value = Request.QueryString["id"];
                    EditModeExecute(SetTopBoxID);
                }
                else
                {
                    HeaderName.InnerHtml = "Add Set Top Box";
                    hdAddEdit.Value = "Insert";
                }
            }
        }

        private void EditModeExecute(string SetTopBoxID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBSetTopBoxDetails");
                    proc.AddVarcharPara("@ACTION", 300, "EDIT");
                    proc.AddVarcharPara("@SetTopBoxID", 100, SetTopBoxID.ToString().Trim());
                    ds = proc.GetDataSet();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlModel.DataSource = ds.Tables[1];
                        ddlModel.ValueField = "ModelID";
                        ddlModel.TextField = "ModelDesc";
                        ddlModel.DataBind();
                        ddlModel.SelectedIndex = 0;

                        ddlSTBModel.DataSource = ds.Tables[2];
                        ddlSTBModel.ValueField = "ModelID";
                        ddlSTBModel.TextField = "ModelDesc";
                        ddlSTBModel.DataBind();
                        ddlSTBModel.SelectedIndex = 0;

                        
                        ddlSTBType.Value = Convert.ToString(ds.Tables[0].Rows[0]["STBType"].ToString());
                        ddlManufacturer.Value = ds.Tables[0].Rows[0]["Manufacturer"].ToString();
                        txtPriceDAS1.Value = ds.Tables[0].Rows[0]["PriceDAS1"].ToString();
                        txtPriceDAS2.Value = ds.Tables[0].Rows[0]["PriceDAS2"].ToString();
                        txtPriceDAS3.Value = ds.Tables[0].Rows[0]["PriceDAS3"].ToString();
                        txtPriceDAS4.Value = ds.Tables[0].Rows[0]["PriceDAS4"].ToString();

                        chkIsActive.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"].ToString());

                        ddlModel.Value = ds.Tables[0].Rows[0]["Model"].ToString();
                        ddlSTBModel.Value = ds.Tables[0].Rows[0]["STBmodel"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
        }

        private void AddmodeExecuted()
        {
            DataSet branchtable = dsFetchAll();

            ddlModel.DataSource = branchtable.Tables[1];
            ddlModel.ValueField = "ModelID";
            ddlModel.TextField = "ModelDesc";
            ddlModel.DataBind();
            ddlModel.SelectedIndex = 0;

            ddlSTBType.DataSource = branchtable.Tables[2];
            ddlSTBType.ValueField = "ID";
            ddlSTBType.TextField = "STBType";
            ddlSTBType.DataBind();
            ddlSTBType.SelectedIndex = 0;

            ddlManufacturer.DataSource = branchtable.Tables[0];
            ddlManufacturer.ValueField = "Manufacturer_Id";
            ddlManufacturer.TextField = "Manufacturer_Name";
            ddlManufacturer.DataBind();
            ddlManufacturer.SelectedIndex = 0;

            ddlSTBModel.DataSource = branchtable.Tables[3];
            ddlSTBModel.ValueField = "ModelID";
            ddlSTBModel.TextField = "ModelDesc";
            ddlSTBModel.DataBind();
            ddlSTBModel.SelectedIndex = 0;

        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSetTopBoxDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCHALL");
            DataSet ds = proc.GetDataSet();
            return ds;
        }

        [WebMethod]
        public static string save(InputSetTopBox apply)
        {
            string output = string.Empty;
            DataTable dtview = new DataTable();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBSetTopBoxInsertUpdate");
                    proc.AddPara("@Action", Convert.ToString(apply.Action));
                    proc.AddPara("@SetTopBoxID", apply.SetTopBoxID);
                    proc.AddPara("@model", Convert.ToString(apply.model));
                    proc.AddPara("@STBType", Convert.ToString(apply.STBType));
                    proc.AddPara("@Manufacturer", Convert.ToString(apply.Manufacturer));
                    proc.AddPara("@PriceDAS1", Convert.ToString(apply.PriceDAS1));
                    proc.AddPara("@PriceDAS2", apply.PriceDAS2);
                    proc.AddPara("@PriceDAS3", apply.PriceDAS3);
                    proc.AddPara("@PriceDAS4", Convert.ToString(apply.PriceDAS4));
                    proc.AddPara("@IsActive", apply.IsActive);
                    proc.AddPara("@CreatedBy", Convert.ToString(user_id));
                    proc.AddPara("@STBmodel", Convert.ToString(apply.STBmodel));
                    dtview = proc.GetTable();
                    if (dtview != null && dtview.Rows.Count > 0)
                    {
                        output = "true~" + apply.Action;
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
    }

    public class InputSetTopBox
    {
        public String Action { get; set; }
        public String SetTopBoxID { get; set; }
        public String model { get; set; }
        public String STBType { get; set; }
        public String Manufacturer { get; set; }
        public String PriceDAS1 { get; set; }
        public String PriceDAS2 { get; set; }
        public String PriceDAS3 { get; set; }
        public String PriceDAS4 { get; set; }
        public String IsActive { get; set; }
        public String STBmodel { get; set; }
    }
}