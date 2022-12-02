using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Working_Schedule_General : ERP.OMS.ViewState_class.VSPage
    {
        String keyVal;
        DataTable dtValue = new DataTable();
        String ival = "";
        // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            //______________________________End Script____________________________//
            if (!IsPostBack)
            {
                InitialValue();
                if (Request.QueryString["id"] != null || Request.QueryString["id"].ToString() != "")
                {
                    keyVal = Request.QueryString["id"];
                    if (keyVal == "ADD")
                    {
                        HttpContext.Current.Session["KeyVal"] = null;
                    }
                    else
                    {
                        HttpContext.Current.Session["KeyVal"] = Convert.ToInt32(keyVal);
                        ShowForm();
                    }
                }

            }
        }
        #region ShowForm()
        private void ShowForm()
        {
            if (keyVal != "")
            {
                dtValue = oDBEngine.GetDataTable("tbl_Master_workingHours", "wor_id,wor_scheduleName,wor_mondayBeginTime,wor_mondayEndTime,wor_tuesdayBeginTime,wor_tuesdayEndTime,wor_wednesdayBeginTime,wor_wednesdayEndTime,wor_thursdayBeginTime,wor_thursdayEndTime,wor_fridayBeginTime,wor_fridayEndTime,wor_saturdayBeginTime,wor_saturdayEndTime,wor_sundayBeginTime,wor_sundayEndTime,wor_monBreak,wor_TueBreak,wor_wedBreak,wor_ThurBreak,wor_friBreak,wor_satBreak,wor_sunBreak", " wor_id = " + keyVal);
                txtSheduleName.Text = dtValue.Rows[0]["wor_scheduleName"].ToString();
                if (dtValue.Rows[0]["wor_mondayBeginTime"].ToString() != "")
                {
                    chkMon.Checked = true;
                    if (dtValue.Rows[0]["wor_monBreak"].ToString() != "")
                        cmbl2.SelectedValue = dtValue.Rows[0]["wor_monBreak"].ToString();
                    string getSPBTime = dtValue.Rows[0]["wor_mondayBeginTime"].ToString();

                    if (getSPBTime != "")
                        txtINtime0.Text = getSPBTime;
                    else
                        txtINtime0.Text = "10:00AM";
                }
                if (dtValue.Rows[0]["wor_mondayEndTime"].ToString() != "")
                {
                    string getSPETime = dtValue.Rows[0]["wor_mondayEndTime"].ToString();

                    if (getSPETime != "")
                        txtOUTtime0.Text = getSPETime;
                    else
                        txtOUTtime0.Text = "06:00PM";
                }
                if (dtValue.Rows[0]["wor_tuesdayBeginTime"].ToString() != "")
                {
                    chktues.Checked = true;
                    if (dtValue.Rows[0]["wor_TueBreak"].ToString() != "")
                        cmbl3.SelectedValue = dtValue.Rows[0]["wor_TueBreak"].ToString();

                    string getSPBTime = dtValue.Rows[0]["wor_tuesdayBeginTime"].ToString();

                    if (getSPBTime != "")
                        txtINtime1.Text = getSPBTime;
                    else
                        txtINtime1.Text = "10:00AM";

                }
                if (dtValue.Rows[0]["wor_tuesdayEndTime"].ToString() != "")
                {
                    string getSPETime = dtValue.Rows[0]["wor_tuesdayEndTime"].ToString();

                    if (getSPETime != "")
                        txtOUTtime1.Text = getSPETime;
                    else
                        txtOUTtime1.Text = "06:00:PM";
                }
                if (dtValue.Rows[0]["wor_wednesdayBeginTime"].ToString() != "")
                {
                    chkwed.Checked = true;
                    if (dtValue.Rows[0]["wor_wedBreak"].ToString() != "")
                        cmbl4.SelectedValue = dtValue.Rows[0]["wor_wedBreak"].ToString();

                    string getSPBTime = dtValue.Rows[0]["wor_wednesdayBeginTime"].ToString();

                    if (getSPBTime != "")
                        txtINtime2.Text = getSPBTime;
                    else
                        txtINtime2.Text = "10:00AM";
                }
                if (dtValue.Rows[0]["wor_wednesdayEndTime"].ToString() != "")
                {
                    string getSPETime = dtValue.Rows[0]["wor_wednesdayEndTime"].ToString();

                    if (getSPETime != "")
                        txtOUTtime2.Text = getSPETime;
                    else
                        txtOUTtime2.Text = "06:00PM";
                }
                if (dtValue.Rows[0]["wor_thursdayBeginTime"].ToString() != "")
                {
                    chkthur.Checked = true;
                    if (dtValue.Rows[0]["wor_ThurBreak"].ToString() != "")
                        cmbl5.SelectedValue = dtValue.Rows[0]["wor_ThurBreak"].ToString();

                    string getSPBTime = dtValue.Rows[0]["wor_thursdayBeginTime"].ToString();

                    if (getSPBTime != "")
                        txtINtime3.Text = getSPBTime;
                    else
                        txtINtime3.Text = "10:00AM";
                }
                if (dtValue.Rows[0]["wor_thursdayEndTime"].ToString() != "")
                {
                    string getSPETime = dtValue.Rows[0]["wor_thursdayEndTime"].ToString();

                    if (getSPETime != "")
                        txtOUTtime3.Text = getSPETime;
                    else
                        txtOUTtime3.Text = "06:00PM";
                }
                if (dtValue.Rows[0]["wor_fridayBeginTime"].ToString() != "")
                {
                    chkfri.Checked = true;
                    if (dtValue.Rows[0]["wor_friBreak"].ToString() != "")
                        cmbl6.SelectedValue = dtValue.Rows[0]["wor_friBreak"].ToString();

                    string getSPBTime = dtValue.Rows[0]["wor_fridayBeginTime"].ToString();

                    if (getSPBTime != "")
                        txtINtime4.Text = getSPBTime;
                    else
                        txtINtime4.Text = "10:00AM";
                }
                if (dtValue.Rows[0]["wor_fridayEndTime"].ToString() != "")
                {
                    string getSPETime = dtValue.Rows[0]["wor_fridayEndTime"].ToString();

                    if (getSPETime != "")
                        txtOUTtime4.Text = getSPETime;
                    else
                        txtOUTtime4.Text = "06:00PM";
                }
                if (dtValue.Rows[0]["wor_saturdayBeginTime"].ToString() != "")
                {
                    chksat.Checked = true;
                    if (dtValue.Rows[0]["wor_satBreak"].ToString() != "")
                        cmbl7.SelectedValue = dtValue.Rows[0]["wor_satBreak"].ToString();

                    string getSPBTime = dtValue.Rows[0]["wor_saturdayBeginTime"].ToString();

                    if (getSPBTime != "")
                        txtINtime5.Text = getSPBTime;
                    else
                        txtINtime5.Text = "10:00AM";
                }
                if (dtValue.Rows[0]["wor_saturdayEndTime"].ToString() != "")
                {
                    string getSPETime = dtValue.Rows[0]["wor_saturdayEndTime"].ToString();

                    if (getSPETime != "")
                        txtOUTtime5.Text = getSPETime;
                    else
                        txtOUTtime5.Text = "06:00PM";
                }
                if (dtValue.Rows[0]["wor_sundayBeginTime"].ToString() != "")
                {
                    chkSun.Checked = true;
                    if (dtValue.Rows[0]["wor_sunBreak"].ToString() != "")
                        cmbl1.SelectedValue = dtValue.Rows[0]["wor_sunBreak"].ToString();

                    string getSPBTime = dtValue.Rows[0]["wor_sundayBeginTime"].ToString();

                    if (getSPBTime != "")
                        txtINtime6.Text = getSPBTime;
                    else
                        txtINtime6.Text = "10:00:AM";
                }
                if (dtValue.Rows[0]["wor_sundayEndTime"].ToString() != "")
                {
                    string getSPETime = dtValue.Rows[0]["wor_sundayEndTime"].ToString();

                    if (getSPETime != "")
                        txtOUTtime6.Text = getSPETime;
                    else
                        txtOUTtime6.Text = "06:00:PM";
                }
            }
        }
        #endregion

        #region InitialValue
        private void InitialValue()
        {
            txtINtime0.Text = "10:00AM";
            txtINtime1.Text = "10:00AM";
            txtINtime2.Text = "10:00AM";
            txtINtime3.Text = "10:00AM";
            txtINtime4.Text = "10:00AM";
            txtINtime5.Text = "10:00AM";
            txtINtime6.Text = "10:00AM";
            txtOUTtime0.Text = "06:00PM";
            txtOUTtime1.Text = "06:00PM";
            txtOUTtime2.Text = "06:00PM";
            txtOUTtime3.Text = "06:00PM";
            txtOUTtime4.Text = "06:00PM";
            txtOUTtime5.Text = "06:00PM";
            txtOUTtime6.Text = "06:00PM";

        }
        #endregion

        #region To Save the Schedule Details
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string field = "";
            string values = "";
            if (Session["KeyVal"] != null)
            {
                field = "wor_scheduleName='" + txtSheduleName.Text.Trim().Replace("'", "`") + "'";
                if (chkMon.Checked)
                {
                    field += ",wor_mondayBeginTime='" + txtINtime0.Text + "',wor_mondayEndTime='" + txtOUTtime0.Text + "',wor_monBreak=" + cmbl2.SelectedValue;

                }
                if (chktues.Checked)
                {
                    field += ",wor_tuesdayBeginTime='" + txtINtime1.Text + "',wor_tuesdayEndTime='" + txtOUTtime1.Text + "',wor_TueBreak=" + cmbl3.SelectedValue;

                }
                if (chkwed.Checked)
                {
                    field += ",wor_wednesdayBeginTime='" + txtINtime2.Text + "',wor_wednesdayEndTime='" + txtOUTtime2.Text + "',wor_wedBreak=" + cmbl4.SelectedValue;


                }
                if (chkthur.Checked)
                {
                    field += ",wor_thursdayBeginTime='" + txtINtime3.Text + "',wor_thursdayEndTime='" + txtOUTtime3.Text + "',wor_ThurBreak=" + cmbl5.SelectedValue;


                }
                if (chkfri.Checked)
                {
                    field += ",wor_fridayBeginTime='" + txtINtime4.Text + "',wor_fridayEndTime='" + txtOUTtime4.Text + "',wor_friBreak=" + cmbl6.SelectedValue;


                }
                if (chksat.Checked)
                {
                    field += ",wor_saturdayBeginTime='" + txtINtime5.Text + "',wor_saturdayEndTime='" + txtOUTtime5.Text + "',wor_satBreak=" + cmbl7.SelectedValue;


                }
                if (chkSun.Checked)
                {
                    field += ",wor_sundayBeginTime='" + txtINtime6.Text + "',wor_sundayEndTime='" + txtOUTtime6.Text + "',wor_sunBreak=" + cmbl1.SelectedValue;


                }
                field += ",LastModifyDate=getdate(),LastModifyUser='" + HttpContext.Current.Session["userid"].ToString() + "'";

                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                int IsEffect = oDBEngine.SetFieldValue(" tbl_master_workingHours ", field, " wor_id=" + Session["KeyVal"].ToString());
                if (IsEffect > 0)
                {
                    //RequiredFieldValidator1.Text = "The shedule time updated succesfully.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "jAlert('Saved succesfully');window.location ='frm_workingShedule.aspx';", true);
                }

            }
            else
            {

                field = "wor_scheduleName";
                values = "'" + txtSheduleName.Text + "'";
                if (chkMon.Checked)
                {
                    field += ",wor_mondayBeginTime,wor_mondayEndTime,wor_monBreak ";
                    values += ",'" + txtINtime0.Text + "','" + txtOUTtime0.Text + "','" + cmbl2.SelectedValue + "'";
                }
                if (chktues.Checked)
                {
                    field += ",wor_tuesdayBeginTime,wor_tuesdayEndTime,wor_TueBreak  ";
                    values += ",'" + txtINtime1.Text + "','" + txtOUTtime1.Text + "','" + cmbl3.SelectedValue + "'";
                }
                if (chkwed.Checked)
                {
                    field += ",wor_wednesdayBeginTime,wor_wednesdayEndTime,wor_wedBreak  ";
                    values += ",'" + txtINtime2.Text + "','" + txtOUTtime2.Text + "','" + cmbl4.SelectedValue + "'";
                }
                if (chkthur.Checked)
                {
                    field += ",wor_thursdayBeginTime,wor_thursdayEndTime,wor_ThurBreak ";
                    values += ",'" + txtINtime3.Text + "','" + txtOUTtime3.Text + "','" + cmbl5.SelectedValue + "'";
                }
                if (chkfri.Checked)
                {
                    field += ",wor_fridayBeginTime,wor_fridayEndTime,wor_friBreak ";
                    values += ",'" + txtINtime4.Text + "','" + txtOUTtime4.Text + "','" + cmbl6.SelectedValue + "'";
                }
                if (chksat.Checked)
                {
                    field += ",wor_saturdayBeginTime,wor_saturdayEndTime,wor_satBreak ";
                    values += ",'" + txtINtime5.Text + "','" + txtOUTtime5.Text + "','" + cmbl7.SelectedValue + "'";
                }
                if (chkSun.Checked)
                {
                    field += ",wor_sundayBeginTime,wor_sundayEndTime,wor_sunBreak ";
                    values += ",'" + txtINtime6.Text + "','" + txtOUTtime6.Text + "','" + cmbl1.SelectedValue + "'";
                }
                field += ",CreateDate,CreateUser";
                values += ",getdate()," + HttpContext.Current.Session["userid"].ToString();
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                int IsEffect=oDBEngine.InsurtFieldValue(" tbl_master_workingHours ", field, values);
                if (IsEffect > 0)
                {
                    //RequiredFieldValidator1.Text = "The shedule time updated succesfully.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "jAlert('Saved Succesfully');window.location ='frm_workingShedule.aspx';", true);
                }
            }
        }
        #endregion


    }
}