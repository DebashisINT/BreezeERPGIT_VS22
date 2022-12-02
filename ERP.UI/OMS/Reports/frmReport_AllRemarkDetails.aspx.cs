using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_AllRemarkDetails : System.Web.UI.Page
    {
        DataTable dtbl = new DataTable();
        DataTable dtDev = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);


        public string pageAccess = "";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            this.Page.Title = "Influx All Remrks Details Report";
            if (!IsPostBack)
            {
                //td_export.Visible = false;
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                //______________________________End Script____________________________//
                populateRemarkTypeCombo();
                Session["mode"] = "N";
            }
            BindRemarkDetails();

        }

        public void populateRemarkTypeCombo()
        {
            if (dtbl != null)
            {
                dtbl = null;
            }
            if (Session["userlastsegment"].ToString() == "4")
            {
                dtbl = oDBEngine.GetDataTable("tbl_master_remarksCategory", " cat_description,id ", "cat_applicablefor='Em'");
            }
            else
            {
                dtbl = oDBEngine.GetDataTable("tbl_master_remarksCategory", " cat_description,id ", "cat_applicablefor!='Em'");

            }
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                cmbRemarkType.Items.Add(dtbl.Rows[i]["cat_description"].ToString(), dtbl.Rows[i]["id"].ToString());
            }
            cmbRemarkType.SelectedIndex = 0;

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
        protected void BindRemarkDetails()
        {
            string catId = "0";
            string catdesc = "N";
            string CatType;
            if (Session["userlastsegment"].ToString() == "4")
            {
                CatType = "Y";

            }
            else
            {
                CatType = "N";

            }
            if (rbCategory.SelectedItem.Value.ToString() == "S")
            {
                catId = cmbRemarkType.SelectedItem.Value.ToString(); ;
            }
            dtDev.Rows.Clear();
            dtDev.Columns.Clear();
            dtDev.Columns.Add("SlNo", typeof(Int32));
            dtDev.Columns.Add("Remark Category");
            dtDev.Columns.Add("Remarks");
            dtDev.Columns.Add("Name");
            dtDev.Columns.Add("Short Name/Code");
            dtDev.Columns.Add("Branch");
            dtDev.Columns.Add("Phone");
            dtDev.Columns.Add("RM");
            dtDev.Columns.Add("Referred By");
            dtDev.Columns.Add("Pan");

            //string[,] Data ={ { "@CategoryId", SqlDbType.Int.ToString(), catId }, { "@name", SqlDbType.VarChar.ToString(), chkName.Value.ToString() }, { "@Sname", SqlDbType.VarChar.ToString(), chkShortNm.Value.ToString() }, { "@Branch", SqlDbType.VarChar.ToString(), chkBranch.Value.ToString() }, { "@Phone", SqlDbType.VarChar.ToString(), chkPhones.Value.ToString() }, { "@RM", SqlDbType.VarChar.ToString(), chkRm.Value.ToString() }, { "@RefBy", SqlDbType.VarChar.ToString(), chkReff.Value.ToString() }, { "@RemValue", SqlDbType.VarChar.ToString(), txtRemValue.Text.Trim() } };
            string[,] Data = { { "@CategoryId", SqlDbType.Int.ToString(), catId }, { "@name", SqlDbType.VarChar.ToString(), "Y" }, { "@Sname", SqlDbType.VarChar.ToString(), "Y" }, { "@Branch", SqlDbType.VarChar.ToString(), "Y" }, { "@Phone", SqlDbType.VarChar.ToString(), "Y" }, { "@RM", SqlDbType.VarChar.ToString(), "Y" }, { "@RefBy", SqlDbType.VarChar.ToString(), "Y" }, { "@pan", SqlDbType.VarChar.ToString(), "Y" }, { "@RemValue", SqlDbType.VarChar.ToString(), txtRemValue.Text.Trim() }, { "@ContactType", SqlDbType.VarChar.ToString(), CatType } };
            string sProcedure = "RemarkDetailsSelect";
            dtbl = oDBEngine.GetDatatable_StoredProcedure(sProcedure, Data);
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                if (CatType == "Y" && (Convert.ToString(dtbl.Rows[i]["AppFor"]) == "Em" || Convert.ToString(dtbl.Rows[i]["rea_internalId"]).Substring(0, 2) == "EM"))
                {
                    DataRow dr = dtDev.NewRow();
                    dr["SlNo"] = dtbl.Rows[i]["ROWID"].ToString();
                    dr["Remark Category"] = dtbl.Rows[i]["cat_description"].ToString();
                    dr["Remarks"] = dtbl.Rows[i]["Remarks"].ToString();
                    dr["Name"] = dtbl.Rows[i]["Name"].ToString();
                    dr["Short Name/Code"] = dtbl.Rows[i]["cnt_ShortName"].ToString();
                    dr["Branch"] = dtbl.Rows[i]["Branch"].ToString();
                    dr["Phone"] = dtbl.Rows[i]["phf_phoneNumber"].ToString();
                    dr["RM"] = dtbl.Rows[i]["cnt_RelationshipManager"].ToString();
                    dr["Referred By"] = dtbl.Rows[i]["cnt_referedBy"].ToString();
                    dr["Pan"] = dtbl.Rows[i]["Pan"].ToString();
                    dtDev.Rows.Add(dr);
                }
                else if (CatType == "N" && (!string.IsNullOrEmpty(Convert.ToString(dtbl.Rows[i]["rea_internalId"])) && Convert.ToString(dtbl.Rows[i]["rea_internalId"]).Substring(0, 2) != "EM"))
                {
                    DataRow dr = dtDev.NewRow();
                    dr["SlNo"] = dtbl.Rows[i]["ROWID"].ToString();
                    dr["Remark Category"] = dtbl.Rows[i]["cat_description"].ToString();
                    dr["Remarks"] = dtbl.Rows[i]["Remarks"].ToString();
                    dr["Name"] = dtbl.Rows[i]["Name"].ToString();
                    dr["Short Name/Code"] = dtbl.Rows[i]["cnt_ShortName"].ToString();
                    dr["Branch"] = dtbl.Rows[i]["Branch"].ToString();
                    dr["Phone"] = dtbl.Rows[i]["phf_phoneNumber"].ToString();
                    dr["RM"] = dtbl.Rows[i]["cnt_RelationshipManager"].ToString();
                    dr["Referred By"] = dtbl.Rows[i]["cnt_referedBy"].ToString();
                    dr["Pan"] = dtbl.Rows[i]["pan"].ToString();
                    dtDev.Rows.Add(dr);
                }

            }
            dtDev.AcceptChanges();
            ASPxRemarkGrid.DataSource = dtDev;
            ASPxRemarkGrid.DataBind();
            ShowColumns();
        }
        public void ShowColumns()
        {
            if (!chkName.Checked)
            {
                ASPxRemarkGrid.Columns["Name"].Visible = false;
            }
            else
            {
                ASPxRemarkGrid.Columns["Name"].Visible = true;
            }
            if (!chkShortNm.Checked)
            {
                ASPxRemarkGrid.Columns["Short Name/Code"].Visible = false;
            }
            else
            {
                ASPxRemarkGrid.Columns["Short Name/Code"].Visible = true;
            }
            if (!chkBranch.Checked)
            {
                ASPxRemarkGrid.Columns["Branch"].Visible = false;
            }
            else
            {
                ASPxRemarkGrid.Columns["Branch"].Visible = true;
            }
            if (!chkPhones.Checked)
            {
                ASPxRemarkGrid.Columns["Phone"].Visible = false;
            }
            else
            {
                ASPxRemarkGrid.Columns["Phone"].Visible = true;
            }
            if (!chkRm.Checked)
            {
                ASPxRemarkGrid.Columns["RM"].Visible = false;
            }
            else
            {
                ASPxRemarkGrid.Columns["RM"].Visible = true;
            }
            if (!chkReff.Checked)
            {
                ASPxRemarkGrid.Columns["Referred By"].Visible = false;
            }
            else
            {
                ASPxRemarkGrid.Columns["Referred By"].Visible = true;
            }

            if (!chkpan.Checked)
            {
                ASPxRemarkGrid.Columns["Pan"].Visible = false;
            }
            else
            {
                ASPxRemarkGrid.Columns["Pan"].Visible = true;
            }

        }
        protected void ASPxRemarkGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //td_export.Visible = true;
            Session["mode"] = "Y";
            BindRemarkDetails();

        }

    }
}