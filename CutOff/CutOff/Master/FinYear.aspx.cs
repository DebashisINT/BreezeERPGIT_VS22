using DataAccessLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CutOff.CutOff.Master
{
    public partial class FinYear : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        ProcedureExecute objProcedureExecute = new ProcedureExecute();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["key"] != null)
                {

                    ShowForm(Convert.ToString(Request.QueryString["key"]));

                }
            }
        }
        private void ShowForm(string p)
        {
            try
            {
                DataTable DT = new DataTable();
                ViewState["financeid"] = p;
                if (!string.IsNullOrEmpty(p))
                {
                    using (objProcedureExecute = new ProcedureExecute("getFinanCialYear"))
                    {
                        objProcedureExecute.AddIntegerPara("@FinYear_ID", Convert.ToInt32(p));
                        DT = objProcedureExecute.GetTable();
                    }
                    if (DT.Rows.Count > 0)
                    {
                        if (Convert.ToString(DT.Rows[0]["FinYear_Code"]) != "")
                        {
                            txtFinYearUniqueId.Text = Convert.ToString(DT.Rows[0]["FinYear_Code"]);
                        }
                        else
                        {
                            txtFinYearUniqueId.Text = Convert.ToString(DT.Rows[0]["FinYear_Code"]);
                        }
                        
                        txtStart.Value = Convert.ToDateTime(DT.Rows[0]["FinYear_StartDate"]);
                        txtStart.ClientEnabled = false;
                        txtEnd.Value = Convert.ToDateTime(DT.Rows[0]["FinYear_EndDate"]);
                        txtRemarks.Text = Convert.ToString(DT.Rows[0]["FinYear_Remarks"]);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void SaveFinYear_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.ToString() == "SaveData")
            {
                string output="";
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("prc_AddEditFinYear");
                proc.AddVarcharPara("@Action", 500, "NoCutOff");
                proc.AddDateTimePara("@inputDate", txtEnd.Date);
                proc.AddVarcharPara("@UniqueId", 500, Convert.ToString(Request.QueryString["key"]));
                proc.AddVarcharPara("@remarks", 100, txtRemarks.Text);
                proc.AddVarcharPara("@UserId", 500, Convert.ToString(HttpContext.Current.Session["userid"]));
                proc.AddVarcharPara("@ReturnValue", 500, output,QueryParameterDirection.Output);
                ds = proc.GetDataSet();

                SaveFinYear.JSProperties["cpResult"] = Convert.ToString(ds.Tables[0].Rows[0][0]);

            }
        }
    }
}