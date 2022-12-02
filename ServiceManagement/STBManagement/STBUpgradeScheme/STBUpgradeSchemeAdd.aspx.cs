using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.STBManagement.STBUpgradeScheme
{
    public partial class STBUpgradeSchemeAdd : System.Web.UI.Page
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
                        HeaderName.InnerHtml = "Modify STB - Upgrade Scheme";
                        hdAddEdit.Value = "Update";
                    }
                    else
                    {
                        HeaderName.InnerHtml = " View STB - Upgrade Scheme";
                        hdAddEdit.Value = "view";
                    }
                    string STBUpgradeSchemeID = Request.QueryString["id"];
                    hdnSTBUpgradeSchemeID.Value = Request.QueryString["id"];
                    EditModeExecute(STBUpgradeSchemeID);
                }
                else
                {
                    HeaderName.InnerHtml = "Add STB - Upgrade Scheme";
                    hdAddEdit.Value = "Insert";
                }
            }
        }

        private void EditModeExecute(string STBUpgradeSchemeID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBUpgradeSchemeDetails");
                    proc.AddVarcharPara("@ACTION", 300, "EDIT");
                    proc.AddVarcharPara("@STBUpgradeSchemeID", 100, STBUpgradeSchemeID.ToString().Trim());
                    ds = proc.GetDataSet();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlModel.DataSource = ds.Tables[1];
                        ddlModel.ValueField = "ModelID";
                        ddlModel.TextField = "ModelDesc";
                        ddlModel.DataBind();
                        ddlModel.SelectedIndex = 0;

                        txtSTBPrice.Value = ds.Tables[0].Rows[0]["STBPrice"].ToString();
                        txtSTBRemotePrice.Value = ds.Tables[0].Rows[0]["STBRemotePrice"].ToString();
                        txtSTBCordAdapterPrice.Value = ds.Tables[0].Rows[0]["STBCordAdapterPrice"].ToString();
                        chkIsActive.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"].ToString());
                        ddlModel.Value = ds.Tables[0].Rows[0]["Model"].ToString();
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

            ddlModel.DataSource = branchtable.Tables[0];
            ddlModel.ValueField = "ModelID";
            ddlModel.TextField = "ModelDesc";
            ddlModel.DataBind();
            ddlModel.SelectedIndex = 0;

        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_STBUpgradeSchemeDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCHALL");
            DataSet ds = proc.GetDataSet();
            return ds;
        }

        [WebMethod]
        public static string save(InputSTBUpgradeScheme apply)
        {
            string output = string.Empty;
            DataTable dtview = new DataTable();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBUpgradeSchemeInsertUpdate");
                    proc.AddPara("@Action", Convert.ToString(apply.Action));
                    proc.AddPara("@STBUpgradeSchemeID", apply.STBUpgradeSchemeID);
                    proc.AddPara("@model", Convert.ToString(apply.model));
                    proc.AddPara("@STBPrice", Convert.ToString(apply.STBPrice));
                    proc.AddPara("@STBRemotePrice", apply.STBRemotePrice);
                    proc.AddPara("@STBCordAdapterPrice", apply.STBCordAdapterPrice);
                    proc.AddPara("@IsActive", apply.IsActive);
                    proc.AddPara("@CreatedBy", Convert.ToString(user_id));
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

    public class InputSTBUpgradeScheme
    {
        public String Action { get; set; }
        public String STBUpgradeSchemeID { get; set; }
        public String model { get; set; }
        public String STBPrice { get; set; }
        public String STBRemotePrice { get; set; }
        public String STBCordAdapterPrice { get; set; }
        public String IsActive { get; set; }
    }
}