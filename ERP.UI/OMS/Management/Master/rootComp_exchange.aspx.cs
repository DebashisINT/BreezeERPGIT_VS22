using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_rootComp_exchange : ERP.OMS.ViewState_class.VSPage
    {
        Int32 ID;
        /* For Tier Structure
        Converter OConvert = new Converter();
        DBEngine oDBEngine = new DBEngine(string.Empty);
         */

        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        string DataEx = "N";
        string ComOfc = "";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Session["KeyValN"] = "n";
            if (!IsPostBack)
            {

                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["requesttype"] = "Companies";
            Session["ContactType"] = "Companies";
            if (!IsPostBack)
            {
                Exchange.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                gridStatusDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                BindCombo();
            }

            //if (Request.QueryString["id"] != "ADD")
            //{
            //    if (Request.QueryString["id"] != null)
            //    {
            //        ID = Int32.Parse(Request.QueryString["id"]);
            //        HttpContext.Current.Session["KeyVal"] = ID;
            //    }
            //    string[,] InternalId;

            //    if (ID != 0)
            //    {
            //        InternalId = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalId", "cmp_id=" + ID, 1);
            //    }
            //    else
            //    {
            //        InternalId = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalId", "cmp_id=" + HttpContext.Current.Session["KeyVal"], 1);
            //    }
            //    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
            //}
            txtCompliance.Attributes.Add("onkeyup", "CallList(this,'SearchByEmployees',event)");


            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            dtSEBIEXP.EditFormatString = OConvert.GetDateFormat("Date");
            dtFMCEXP.EditFormatString = OConvert.GetDateFormat("Date");
            dtSEBIEXP.Attributes.Add("readonly", "true");
            dtFMCEXP.Attributes.Add("readonly", "true");
            LitCompName.Text = "Company Name :" + "  " + Session["CompanyName"].ToString();
            fillGrid();
            gridContract.JSProperties["cpDelmsg"] = null;
        }
        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters != null)
            {
                string[] data = e.Parameters.Split('~');
                if (data[0] == "Delete")
                {


                    SqlDataReader objReader = oDBEngine.GetReader("select top 1 ExchangeTrades_ID from Trans_ExchangeTrades where ExchangeTrades_segment='" + data[1] + "' union select top 1 ComExchangeTrades_ID from Trans_ComExchangeTrades where ComExchangeTrades_segment='" + data[1] + "' union select top 1 AccountsLedger_ID from Trans_AccountsLedger where AccountsLedger_ExchangesegmentId='" + data[1] + "'");


                    if (objReader.HasRows)
                    {
                        gridContract.JSProperties["cpDelmsg"] = "Cannot Delete. This ProfileTrade Code Is In Use";
                        oDBEngine.CloseConnection();
                        objReader.Close();
                    }
                    else
                    {
                        oDBEngine.CloseConnection();
                        objReader.Close();
                        int delcount = oDBEngine.DeleteValue(" [tbl_master_companyExchange]", " exch_internalId='" + data[1] + "'");
                        if (delcount > 0)
                            gridContract.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                        else
                            gridContract.JSProperties["cpDelmsg"] = "Not Deleted, There are Some Problem.";

                    }


                }
                else if (data[0].ToString() == "s")
                {
                    gridContract.Settings.ShowFilterRow = true;

                }
                else if (data[0].ToString() == "All")
                {
                    gridContract.FilterExpression = string.Empty;

                }

            }
            fillGrid();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "height4454", "<script>height();</script>");

        }

        public void fillGrid()
        {

            gridStatusDataSource.SelectCommand = "select exch_internalId,exch_compId,exch_exchId,(select top 1 [exh_name] from  [tbl_master_exchange] where  [exh_cntId]=exch_exchId) as ExchangeName,exch_segmentId,case when exch_segmentId='CM' then 'Capital Market(CM)' when exch_segmentId='WDM' then 'WDM'when exch_segmentId='FO' then 'Futures & Options(FO)'when exch_segmentId='CDX' then 'Currency Derivative(CDX)'when exch_segmentId='SPOT' then 'Commodity(SPOT)' when exch_segmentId='COMM' then 'Commodity Derivative(COMM)' end as SegmentName, exch_membershipType, case when exch_membershipType='TM' then 'Trading Member (TM)' when exch_membershipType='TCM' then 'Trading Cum Clearing Member (TCM)' when exch_membershipType='CTM' then 'Trading Member Of CSE (CTM)' when exch_membershipType='ITCM' then 'Institutional Trading Cum Clearing Member(ITCM)' when exch_membershipType='PCM' then 'Professional Clearing Member  (PCM)' end as MemberShipType ,  exch_TMCode, exch_CMCode, exch_sebiNo, exch_regnNo from [tbl_master_companyExchange]   where exch_compId='" + Session["KeyVal_InternalID"].ToString() + "'  and exch_exchId is not null";
            gridContract.DataBind();


        }
        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Session["KeyValN"].ToString();
            e.Properties["cpfast"] = DataEx;
        }
        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            DataTable dtCheck = new DataTable();

            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                // BindCombo();
                clearField();
                string[,] DT = oDBEngine.GetFieldValue("[tbl_master_companyExchange]", "exch_compId,exch_exchId,exch_segmentId,exch_membershipType,exch_TMCode,exch_CMCode,exch_sebiNo,exch_sebiexpDate,exch_regnNo,exch_regnexpdate,exch_ComplianceOfficer,(select   isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from  tbl_master_contact where cnt_internalid=exch_ComplianceOfficer) as ComplianceOfc,CreateDate,CreateUser,exch_cmbpid,exch_GrievanceID,exch_InvestorGrievanceID,exch_ComplianceOfficer", "exch_internalId='" + data[1] + "'", 18);
                cmbExchName.SelectedValue = DT[0, 1];
                cmbSegName.SelectedValue = DT[0, 2];
                cmbMemberType.SelectedValue = DT[0, 3];
                txtTMCODE.Text = DT[0, 4];
                txtCMCODE.Text = DT[0, 5];
                txtcmbpid.Text = DT[0, 14];
                txtSEBINO.Text = DT[0, 6];
                txtbroker.Text = DT[0, 15];
                txtexchange.Text = DT[0, 16];
                txtCompliance_hidden.Value = DT[0, 17];
                if (DT[0, 7].ToString() != "")
                {
                    dtSEBIEXP.Value = Convert.ToDateTime(DT[0, 7].ToString());
                }
                txtFMCNO.Text = DT[0, 8];
                if (DT[0, 9].ToString() != "")
                {
                    dtFMCEXP.Value = Convert.ToDateTime(DT[0, 9].ToString());
                }
                txtCompliance.Text = DT[0, 11];

                //    Page.ClientScript.RegisterStartupScript(GetType(), "ff", "<script>FilterOff('" + DT[0, 1].ToString() + "');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript1s0", "height();", true);

                if (DT[0, 1].ToString() == "EXA0000001")
                {
                    DataEx = "S";
                }
                //    trSegment.Visible = false;
                //    trMemType.Visible = false;
                //    TmCode.Visible = false;
                //    CmCode.Visible = false;
                //    trSEBI.Visible = false;
                //    trExp.Visible = false;
                //    TrFMC.Visible = false;
                //    TrFmcEX.Visible = false;
                //    TrCompOf.Visible = false;

                //}
                //else
                //{

                //    trSegment.Visible = true;
                //    trMemType.Visible = true;
                //    TmCode.Visible = true;
                //    CmCode.Visible = true;
                //    trSEBI.Visible = true;
                //    trExp.Visible = true;
                //    TrFMC.Visible = true;
                //    TrFmcEX.Visible = true;
                //    TrCompOf.Visible = true;
                //}



            }
            else if (data[0] == "AddNew")
            {
                clearField();
                Session["KeyValN"] = "n";
            }
            else if (data[0] == "SaveNew")
            {
                DataTable dtEXN = new DataTable();
                if (cmbExchName.SelectedItem.Value == "EXA0000001")
                {
                    dtEXN = oDBEngine.GetDataTable("[tbl_master_companyExchange]", "*", "exch_compId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString().Trim() + "' and exch_exchId='" + cmbExchName.SelectedItem.Value + "' ");
                }
                else
                {
                    dtEXN = oDBEngine.GetDataTable("[tbl_master_companyExchange]", "*", "exch_compId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString().Trim() + "' and exch_exchId='" + cmbExchName.SelectedItem.Value + "' and exch_segmentId='" + cmbSegName.SelectedItem.Value + "'");
                }
                if (dtEXN.Rows.Count > 0)
                {
                    DataEx = "Y";
                }
                else
                {
                    if (cmbExchName.SelectedItem.Value.ToString() == "EXA0000001")
                    {
                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("[tbl_master_companyExchange]", "exch_compId,exch_exchId,exch_membershipType,CreateDate,CreateUser", "'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','" + cmbExchName.SelectedItem.Value + "','Accounts','" + oDBEngine.GetDate() + "','" + Session["userid"].ToString() + "'");
                    }
                    else
                    {
                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("[tbl_master_companyExchange]", "exch_compId,exch_exchId,exch_segmentId,exch_membershipType,exch_TMCode,exch_CMCode,exch_sebiNo,exch_cmbpid,exch_sebiexpDate,exch_regnNo,exch_regnexpdate,exch_ComplianceOfficer,CreateDate,CreateUser,exch_GrievanceID,exch_InvestorGrievanceID", "'" + HttpContext.Current.Session["KeyVal_InternalID"] + "','" + cmbExchName.SelectedItem.Value + "','" + cmbSegName.SelectedItem.Value + "','" + cmbMemberType.SelectedItem.Value + "','" + txtTMCODE.Text.ToString().Trim() + "','" + txtCMCODE.Text.ToString().Trim() + "','" + txtcmbpid.Text.ToString().Trim() + "','" + txtSEBINO.Text.ToString().Trim() + "','" + dtSEBIEXP.Value + "','" + txtFMCNO.Text.ToString().Trim() + "','" + dtFMCEXP.Value + "','" + txtCompliance_hidden.Value + "','" + oDBEngine.GetDate() + "','" + Session["userid"].ToString() + "','" + txtbroker.Text.ToString().Trim() + "','" + txtexchange.Text.ToString().Trim() + "'");
                    }

                }


            }
            else if (data[0] == "SaveOld")
            {
                //string ComOfc = string.Empty;
                if (txtCompliance_hidden.Value == "")
                {
                    ComOfc = null;

                }

                else
                {
                    ComOfc = txtCompliance_hidden.Value.ToString();
                }
                if (txtexchange.Text == "")
                {
                    txtexchange.Text = null;
                }
                if (txtbroker.Text == "")
                {
                    txtbroker.Text = null;
                }


                if (txtCMCODE.Text == "")
                {
                    txtCMCODE.Text = null;
                }
                if (txtcmbpid.Text == "")
                {
                    txtcmbpid.Text = null;
                }
                if (dtFMCEXP.Value == null)
                    dtFMCEXP.Value = DBNull.Value;
                if (dtFMCEXP.Value == null)
                    dtFMCEXP.Value = DBNull.Value;
                DataTable dtEXO = new DataTable();
                if (cmbExchName.SelectedItem.Value == "EXA0000001")
                {
                    dtEXO = oDBEngine.GetDataTable("[tbl_master_companyExchange]", "*", "exch_compId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString().Trim() + "' and exch_exchId='" + cmbExchName.SelectedItem.Value + "'  and exch_internalId !='" + data[1] + "' ");
                }
                else
                {
                    dtEXO = oDBEngine.GetDataTable("[tbl_master_companyExchange]", "*", "exch_compId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString().Trim() + "' and exch_exchId='" + cmbExchName.SelectedItem.Value + "' and exch_segmentId='" + cmbSegName.SelectedItem.Value + "' and exch_internalId !='" + data[1] + "' ");
                }
                if (dtEXO.Rows.Count > 0)
                {
                    DataEx = "Y";
                }
                else
                {
                    if (cmbExchName.SelectedItem.Value.ToString() == "EXA0000001")
                    {
                        oDBEngine.SetFieldValue("tbl_master_companyExchange", "exch_compId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "',exch_exchId='" + cmbExchName.SelectedItem.Value + "',exch_membershipType='Accounts',exch_membershipType=null,exch_TMCode=null,exch_CMCode=null,exch_sebiNo=null,exch_sebiexpDate=null,exch_regnNo=null,exch_regnexpdate=null,exch_ComplianceOfficer=null", "exch_internalId='" + data[1] + "'");
                    }
                    else
                    {
                        oDBEngine.SetFieldValue("tbl_master_companyExchange", "exch_compId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "',exch_exchId='" + cmbExchName.SelectedItem.Value + "',exch_segmentId='" + cmbSegName.SelectedItem.Value + "',exch_membershipType='" + cmbMemberType.SelectedItem.Value + "',exch_TMCode='" + txtTMCODE.Text.ToString().Trim() + "',exch_CMCode='" + txtCMCODE.Text.ToString().Trim() + "',exch_cmbpid='" + txtcmbpid.Text.ToString().Trim() + "',exch_sebiNo='" + txtSEBINO.Text.ToString().Trim() + "',exch_sebiexpDate='" + dtSEBIEXP.Value + "',exch_regnNo='" + txtFMCNO.Text.ToString() + "',exch_regnexpdate='" + dtFMCEXP.Value + "',exch_ComplianceOfficer='" + txtCompliance_hidden.Value + "',exch_GrievanceID='" + txtbroker.Text.ToString().Trim() + "',exch_InvestorGrievanceID='" + txtexchange.Text.ToString().Trim() + "'", "exch_internalId='" + data[1] + "'");
                    }
                }

            }
        }

        private void clearField()
        {
            cmbExchName.SelectedValue = "0";
            cmbSegName.SelectedValue = "0";
            cmbMemberType.SelectedValue = "0";
            txtTMCODE.Text = "";
            txtCMCODE.Text = "";
            txtSEBINO.Text = "";
            txtCompliance.Text = "";
            txtFMCNO.Text = "";
            txtCompliance_hidden.Value = "";
            txtcmbpid.Text = "";
            txtexchange.Text = "";
            txtbroker.Text = "";


        }


        public void BindCombo()
        {
            DataTable dtCMB = oDBEngine.GetDataTable("[tbl_master_exchange]", "[exh_cntId], [exh_name]", null, "[exh_name]");
            for (int i = 0; i < dtCMB.Rows.Count; i++)
            {
                cmbExchName.Items.Add(new ListItem(dtCMB.Rows[i]["exh_name"].ToString(), dtCMB.Rows[i]["exh_cntId"].ToString()));

            }
            cmbExchName.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        public void HideTextBox()
        {


        }
    }
}