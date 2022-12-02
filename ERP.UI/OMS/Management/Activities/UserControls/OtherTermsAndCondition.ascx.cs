using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities.UserControls
{
    public partial class OtherTermsAndCondition : System.Web.UI.UserControl
    {
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string Variable_Name = string.Empty;
                if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) != "")
                {
                    string Type = Convert.ToString(Request.QueryString["type"]);
                    Variable_Name = "Show_OTC_" + Type;
                }
                else
                {
                    try
                    {
                        HiddenField ctl = (HiddenField)this.Parent.FindControl("hfOtherTermsConditionDocType");
                        string DocType = ctl.Value;
                        Variable_Name = "Show_OTC_" + DocType;
                    }
                    catch (Exception ex) { Variable_Name = "Show_OTC_SO"; }
                }
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='" + Variable_Name + "' AND IsActive=1");

                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsVisible = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();

                    if (IsVisible == "Yes")
                    {
                        this.Visible = true;
                    }
                    else
                    {
                        this.Visible = false;
                    }
                }
                if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
                {
                    BindDataByDocID(Request.QueryString["key"], Request.QueryString["type"]);                    
                }
            }
        }
        public void BindDataByDocID(string docID, string docType)
        {
            DataSet dsControlDetails = GetTransporterControlDetails(docID, docType);         
            string DelDetails = string.Empty;

            if (dsControlDetails.Tables[0] != null && dsControlDetails.Tables[0].Rows.Count > 0)
            {
                cmbTypPO.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["TypPO"]);
                txtPORCNo.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["PORCNo"]);
                cmbPaymentTerms.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["PaymentTerms"]);
                txtPerformanceSecurityAmount.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["PerformanceSecurityAmount"]);
                cmbTypePerformanceSecurity.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["TypePerformanceSecurity"]);
                if (Convert.ToString(dsControlDetails.Tables[0].Rows[0]["SecurityPeriodFrom"])!="")
                dtSecurityPeriodFrom.Date = Convert.ToDateTime(dsControlDetails.Tables[0].Rows[0]["SecurityPeriodFrom"]);
                if (Convert.ToString(dsControlDetails.Tables[0].Rows[0]["SecurityPeriodTo"]) != "")
                dtSecurityPeriodTo.Date = Convert.ToDateTime(dsControlDetails.Tables[0].Rows[0]["SecurityPeriodTo"]);
                cmbPriceBasis.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["PriceBasis"]);
                txtBaseDate.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["BaseDate"]);
                cmbLDTerms.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["LDTerms"]);
                txtRateLD.Text = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["RateLD"]);
                cmbLDApplicable.Value = Convert.ToString(dsControlDetails.Tables[0].Rows[0]["LDApplicable"]);
            }          
           
            
        }
        public DataSet GetTransporterControlDetails(string docID, string docType)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("prc_OtherTermsAndCondition_CRUD");
                proc.AddVarcharPara("@Action", 20, "GetData");
                proc.AddVarcharPara("@DocID", 20, docID);
                proc.AddVarcharPara("@DocType", 20, docType);
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex) { return null; }

        }
        public void SaveTC(string TermsConditionData, string DocId, string DocType,string Action)
        {
            try
            {
                string[] controlDataList = TermsConditionData.Split('|');              

                ProcedureExecute proc = new ProcedureExecute("prc_OtherTermsAndCondition_CRUD");
                proc.AddVarcharPara("@Action", 500, Action);
                proc.AddVarcharPara("@DocId", 500, DocId);
                proc.AddVarcharPara("@DocType", 500, DocType);
                proc.AddVarcharPara("@TypPO", 20, controlDataList[1]);
                proc.AddVarcharPara("@PORCNo", 20, controlDataList[2]);
                proc.AddVarcharPara("@PaymentTerms", 20, controlDataList[3]);
               // proc.AddVarcharPara("@PerformanceSecurityAmount", 20, controlDataList[4]);
                proc.AddNVarcharPara("@PerformanceSecurityAmount", 50, (controlDataList[4].Trim() == "") ? "0" : Convert.ToString(controlDataList[4].Trim()));
                proc.AddVarcharPara("@TypePerformanceSecurity", 20, controlDataList[5]);
                proc.AddNVarcharPara("@SecurityPeriodFrom", 50, (controlDataList[6].Trim() == "01/01/0100 00:00:00") ? null : Convert.ToString(controlDataList[6].Trim()));
                proc.AddNVarcharPara("@SecurityPeriodTo", 50, (controlDataList[7].Trim() == "01/01/0100 00:00:00") ? null : Convert.ToString(controlDataList[7].Trim()));
                
               
                proc.AddVarcharPara("@PriceBasis", 20, controlDataList[8]);
                proc.AddVarcharPara("@BaseDate", 20, controlDataList[9]);
                proc.AddVarcharPara("@LDTerms", 20, controlDataList[10]);
                proc.AddNVarcharPara("@RateLD", 50, (controlDataList[11].Trim() == "") ? "0" : Convert.ToString(controlDataList[11].Trim()));
                //proc.AddVarcharPara("@RateLD", 20, controlDataList[11]);
                proc.AddVarcharPara("@LDApplicable", 20, controlDataList[12]);
                
                int _Ret = proc.RunActionQuery();
            }
            catch (Exception ex) { }
        }

        protected void callBackuserControlPanelMainOTC_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] Data = e.Parameter.Split('~');
            if (Data != null && Data.Length == 3 && Data[0] == "TCtagging") // Tagging Property
            {
                string DocId = Data[1];
                string DocType = Data[2];
                BindDataByDocID(DocId, DocType);
            }
        }
    }
}