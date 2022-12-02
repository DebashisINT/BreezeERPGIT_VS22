using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_targetSetUpFranchese : System.Web.UI.Page, ICallbackEventHandler
    {

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList clsdropdown = new clsDropDownList();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        string data = "";

        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
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
            if (!IsPostBack)
            {
                string[,] cmbData = oDBEngine.GetFieldValue("tbl_targetschemes", "tgt_id,tgt_descripition", "  convert(datetime, tgt_effectivedate,103) < convert(datetime, GETDATE(),103) AND  convert(datetime, tgt_enddate,103) > convert(datetime, GETDATE(),103) and tgt_enddate is not null", 2, "tgt_descripition");
                if (cmbData[0, 0] != "n")
                {
                    clsdropdown.AddDataToDropDownList(cmbData, cmbTargetname);
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>BtnCancel_Click();</script>");
                TxtBranchName.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByFranchese',event)");
                //ImgStartDate.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_TxtApplicableDate,'dd/mm/yyyy',ctl00_ContentPlaceHolder3_TxtApplicableDate,true,null,'0',0)");
                //TxtApplicableDate.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_TxtApplicableDate ,'dd/mm/yyyy',this,true,null,'0',0)");
                //TxtApplicableDate.Attributes.Add("readonly", "true");
                TxtApplicableDate.EditFormatString = OConvert.GetDateFormat("Date");
            }
            FillGrid();
            TxtApplicableDate.Attributes.Add("readonly", "true");
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
        }
        public void FillGrid()
        {
            // DataTable DtTable = oDBEngine.GetDataTable("tbl_master_branch b,tbl_targetschemes st,tbl_targetsetup t", "b.branch_description,st.tgt_descripition,convert(varchar(10),t.tgtset_applicabledate,113) as ApplicableDate,convert(varchar(10),isnull(t.tgtset_tilldate,''),113) as Tilldate ", " st.tgt_id=t.tgtset_scheme and t.tgtset_Entity=b.branch_internalId and b.branch_type='Franchisee'");
            DataTable DtTable = oDBEngine.GetDataTable("tbl_master_branch b,tbl_targetschemes st,tbl_targetsetup t", "b.branch_description,st.tgt_descripition,convert(varchar(20),convert(datetime,t.tgtset_applicabledate,103),113) as ApplicableDate,convert(varchar(20),convert(datetime,isnull(t.tgtset_tilldate,''),103),113) as Tilldate ", " st.tgt_id=t.tgtset_scheme and t.tgtset_Entity=b.branch_internalId and b.branch_type='Franchisee'");
            gridBranch.DataSource = DtTable.DefaultView;
            gridBranch.DataBind();
        }
        #region ICallbackEventHandler Members

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idList = id.Split('~');
            int NoOfRecord = 0;
            if (idList[0] == "Save")
            {
                //string ApplicableDate = objConverter.DateConverter(idList[3], "mm/dd/yyyy");
                string ApplicableDate = idList[3];
                string Tilldate = Convert.ToDateTime(ApplicableDate).AddDays(-1).ToString();
                string[,] Entity = oDBEngine.GetFieldValue("tbl_targetsetup", "tgtset_Entity", "tgtset_Entity='" + idList[1] + "' and tgtset_tilldate is null", 1);
                if (Entity[0, 0] != "n")
                {
                    DateTime tilldate = Convert.ToDateTime(Tilldate);
                    DataTable applicable = oDBEngine.GetDataTable("tbl_targetsetup", "tgtset_applicabledate", "tgtset_Entity='" + idList[1] + "' and tgtset_tilldate is null");
                    //DateTime Applicabledate = Convert.ToDateTime(objConverter.DateConverter(applicable.Rows[0][0].ToString(), "mm/dd/yyyy"));
                    DateTime Applicabledate = Convert.ToDateTime(applicable.Rows[0][0].ToString());
                    if (DateTime.Compare(tilldate, Applicabledate) > 0)
                    {
                        NoOfRecord = oDBEngine.SetFieldValue("tbl_targetsetup", "tgtset_tilldate='" + Tilldate + "'", " ( tgtset_tilldate is null or tgtset_tilldate = '1/1/1900 12:00:00 AM' or tgtset_tilldate = '1/1/1900')  and tgtset_Entity = '" + idList[1] + "'");
                        NoOfRecord = oDBEngine.InsurtFieldValue("tbl_targetsetup", "tgtset_Entity,tgtset_scheme,tgtset_applicabledate,createdate,createuser", "'" + idList[1] + "','" + idList[2] + "','" + idList[3] + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                    }
                    else
                    {
                        NoOfRecord = 0;
                    }
                }
                else
                {
                    NoOfRecord = oDBEngine.InsurtFieldValue("tbl_targetsetup", "tgtset_Entity,tgtset_scheme,tgtset_applicabledate,createdate,createuser", "'" + idList[1] + "','" + idList[2] + "','" + idList[3] + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                }
                if (NoOfRecord > 0)
                {
                    data = "Save~Y";
                }
                else
                {
                    data = "Save~N";
                }
            }

        }

        #endregion
        protected void gridBranch_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            FillGrid();
        }
    }
}