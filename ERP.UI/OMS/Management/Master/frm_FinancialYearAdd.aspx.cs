using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using DataAccessLayer;
using System.Globalization;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_FinancialYearAdd : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        ProcedureExecute objProcedureExecute = new ProcedureExecute();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add("Onclick", "Javascript:return ValidatePage();");
            txtStart.EditFormatString = OConvert.GetDateFormat("Date");
            txtEnd.EditFormatString = OConvert.GetDateFormat("Date");
            txtStart.Attributes.Add("readonly", "true");
            txtEnd.Attributes.Add("readonly", "true");
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            if (!IsPostBack)
            {
                if (Request.QueryString["FinYearID"] != null)
                {

                    ShowForm(Convert.ToString(Request.QueryString["FinYearID"]));

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
                    // objProcedureExecute.ads
                    using (objProcedureExecute = new ProcedureExecute("getFinanCialYear"))
                    {
                        objProcedureExecute.AddIntegerPara("@FinYear_ID", Convert.ToInt32(p));
                        DT = objProcedureExecute.GetTable();
                    }
                    if (DT.Rows.Count > 0)
                    {
                        if (Convert.ToString(DT.Rows[0]["FinYear_Code"]) != "")
                        {
                            string[] FinYear = Convert.ToString(DT.Rows[0]["FinYear_Code"]).Split('-');
                            txtFinYear.Text = FinYear[0];
                            txtFinYear1.Text = FinYear[1];
                        }
                        else
                        {
                            txtFinYear.Text = Convert.ToString(DT.Rows[0]["FinYear_Code"]);
                        }

                       // txtFinYear.Text = Convert.ToString(DT.Rows[0]["FinYear_Code"]) == "" ? "" : Convert.ToString(DT.Rows[0]["FinYear_Code"]);
                        hdFinYearOld.Value = Convert.ToString(DT.Rows[0]["FinYear_Code"]) == "" ? "" : Convert.ToString(DT.Rows[0]["FinYear_Code"]);
                        txtStart.Value = Convert.ToDateTime(DT.Rows[0]["FinYear_StartDate"]);
                        txtEnd.Value = Convert.ToDateTime(DT.Rows[0]["FinYear_EndDate"]);
                        txtRemarks.Text = Convert.ToString(DT.Rows[0]["FinYear_Remarks"]);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string financeid = string.Empty;
                try
                {

                    financeid = Convert.ToString(ViewState["financeid"]);
                }
                catch (Exception ex)
                {
                    financeid = string.Empty;

                }
                if (financeid.Trim() == "undefined")
                {
                    financeid = string.Empty;
                }
                string FinYear = txtFinYear.Text.Trim() + "-" + txtFinYear1.Text.Trim();

                if (string.IsNullOrEmpty(financeid))
                {
                    string startdate = txtStart.Date.Year.ToString() + "-" + txtStart.Date.Month.ToString() + "-" + txtStart.Date.Day.ToString();
                    string Enddate = txtEnd.Date.Year.ToString() + "-" + txtEnd.Date.Month.ToString() + "-" + txtEnd.Date.Day.ToString();
                    DataTable dtFinS = oDBEngine.GetDataTable("master_finyear", "*", " '" + startdate + "' between FinYear_StartDate and FinYear_EndDate");
                    DataTable dtFinE = oDBEngine.GetDataTable("master_finyear", "*", " '" + Enddate + "' between FinYear_StartDate and FinYear_EndDate");

                    if (dtFinS.Rows.Count > 0 || dtFinE.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>jAlert('Already Exists.');</script>");
                    }
                    else
                    {
                       
                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("master_finyear", " FinYear_Code, FinYear_StartDate,FinYear_EndDate,FinYear_Remarks ", " '" + FinYear + "','" + txtStart.Value + "','" + txtEnd.Value + "','" + txtRemarks.Text.ToString().Trim() + "'");
                        if (NoofRowsAffect > 0)
                        {
                            // this.Page.ClientScript.RegisterStartupScript(GetType(), "Script4", "<script>parent.editwin.close()</script>");
                            //Response.Redirect("frm_FinancialYear.aspx");
                            ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "jAlert('Record saved Successfully');window.location ='frm_FinancialYear.aspx';", true);
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Script5", "<script>jAlert('Please Try Again Later.')</script>");
                        }
                    }
                }

                else
                {
                    try
                    {
                        int retValue = masterChecking.checkFinancialYear(hdFinYearOld.Value.Trim());
                        if (retValue == -10)
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "ScriptExsits", "<script>jAlert('Record(s) Exists against this Financial year, Can not Modify.')</script>");
                        }
                        else
                        {
                            using (objProcedureExecute = new ProcedureExecute("UpdateFinanCialYear"))
                            {
                                objProcedureExecute.AddIntegerPara("@FinYear_ID", Convert.ToInt32(financeid));
                                objProcedureExecute.AddVarcharPara("@FinYear_Code", 10, Convert.ToString(FinYear));

                                //commented due to exception throwing while update 28122016
                                objProcedureExecute.AddDateTimePara("@FinYear_StartDate", Convert.ToDateTime(txtStart.Value));
                                objProcedureExecute.AddDateTimePara("@FinYear_EndDate", Convert.ToDateTime(txtEnd.Value));
                                //DateTime startdate=DateTime.ParseExact(txtStart.Value, "yyyy-mm-dd", CultureInfo.InvariantCulture);
                                //DateTime endate= DateTime.ParseExact(txtEnd.Text, "yyyy-mm-dd", CultureInfo.InvariantCulture);
                                //objProcedureExecute.AddDateTimePara("@FinYear_StartDate",startdate );
                                //objProcedureExecute.AddDateTimePara("@FinYear_EndDate", endate);
                                objProcedureExecute.AddVarcharPara("@FinYear_Remarks", 100, Convert.ToString(txtRemarks.Text.Trim()));
                                // DT = objProcedureExecute.GetTable();

                                int NoOfRowEffected = objProcedureExecute.RunActionQuery();
                                if (NoOfRowEffected > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "jAlert('Record updated Successfully');window.location ='frm_FinancialYear.aspx';", true);
                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Script5", "<script>jAlert('Error please Try Again Later.')</script>");
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "Script5", "<script>jAlert('Error please Try Again Later.')</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Script5", "<script>jAlert('Error please Try Again Later.')</script>");
            }
        }

    }
}
