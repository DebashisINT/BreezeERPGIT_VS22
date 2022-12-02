using System;
using System.Configuration;
using System.Data;
using System.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_frmCancelMeeting : System.Web.UI.Page
    {
        clsDropDownList clsdropdown = new clsDropDownList();
      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDown();
                FillData();
            }
        }
        public void FillData()
        {


            string str = Request.QueryString["id"].ToString();
            string productid = "";
            string leadid = "";
            if (str != "")
            {
                string[] st = str.Split(',');
                for (int i = 0; i < st.GetUpperBound(0); i++)
                {
                    string[] s = st[i].Split('@');
                    if (s.GetValue(0).ToString() != "")
                    {
                        if (i == 0)
                        {
                            string sub_str = s.GetValue(0).ToString();
                            leadid = sub_str.ToString().Trim().Substring((sub_str.ToString().Trim().Length - 10), 10);
                        }
                        else
                        {
                            string sub_str = s.GetValue(0).ToString();
                            leadid += "," + sub_str.ToString().Trim().Substring((sub_str.ToString().Trim().Length - 10), 10);
                        }
                    }
                    if (s.GetValue(4) != "")
                    {
                        if (i == 0)
                        {
                            productid = s.GetValue(4).ToString();
                        }
                        else
                        {
                            productid += "," + s.GetValue(4).ToString();
                        }
                    }
                }
            }
            if (leadid != "")
            {
                string[] ld = leadid.Split(',');
                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_master_lead ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_lead.cnt_internalId", "ISNULL(tbl_master_lead.cnt_firstName, '') + ' ' + ISNULL(tbl_master_lead.cnt_middleName, '') + ' ' + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, tbl_trans_phonecall.phc_nextCall, tbl_trans_phonecall.phc_id, tbl_trans_phonecall.phc_note", " phc_leadcotactId='" + ld.GetValue(0) + "'");
                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        lblName.Text = dt.Rows[0]["name"].ToString();
                        ViewState["selphonecallid"] = dt.Rows[0]["phc_id"].ToString();
                    }
                    else
                    {
                        string jscript = "<script language='javascript'>alert('Please select the lead to post pond visit');window.close();</script>";
                        ClientScript.RegisterStartupScript(GetType(), "JScript", jscript);
                    }
                }
            }
            else
            {
                string jscript = "<script language='javascript'>alert('Please select the lead to post pond visit');window.close();</script>";
                ClientScript.RegisterStartupScript(GetType(), "JScript", jscript);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ViewState["selphonecallid"].ToString() != "")
            {
                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_callDispose='" + drpCancelMeeting.SelectedValue.ToString() + "',phc_note='" + txtNote.Text + "'", " phc_id='" + ViewState["selphonecallid"].ToString() + "'");
                oDBEngine.InsurtFieldValue("tbl_trans_phonecalldetails", "phd_phoneCallId, phd_branchId, phd_callDate, phd_callStart, phd_callEnd, phd_callDispose, phd_callduration, phd_note, phd_nextCall, phd_CallType", "'" + ViewState["selphonecallid"].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + oDBEngine.GetDate().ToShortDateString() + " " + oDBEngine.GetDate().ToShortTimeString() + "','" + oDBEngine.GetDate().ToShortDateString() + " " + oDBEngine.GetDate().ToShortTimeString() + "','" + drpCancelMeeting.SelectedValue.ToString() + "',0,'" + txtNote.Text + "','','RC'");
                string JScript = "<script language='javascript'>close();</script>";
                ClientScript.RegisterStartupScript(GetType(), "JScript", JScript);
            }
        }
        public void FillDropDown()
        {
            string[,] data = oDBEngine.GetFieldValue("tbl_Master_DispositionCategory INNER JOIN tbl_master_calldispositions ON tbl_Master_DispositionCategory.Int_id = tbl_master_calldispositions.Call_Category", "tbl_master_calldispositions.call_id,tbl_master_calldispositions.call_dispositions", " tbl_Master_DispositionCategory.Int_id IN (3, 6)", 2);
            if (data[0, 0] != "n")
            {
                clsdropdown.AddDataToDropDownList(data, drpCancelMeeting);
            }
        }
    }
}