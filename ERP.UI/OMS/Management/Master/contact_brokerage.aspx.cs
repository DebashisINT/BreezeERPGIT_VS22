using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using DevExpress.Web;

using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_contact_brokerage : ERP.OMS.ViewState_class.VSPage
    {
        string Error = "";
        DataTable dtbrokerage = new DataTable();
        DataTable dttradeprof = new DataTable();
        DataTable dtothercharge = new DataTable();
        //DBEngine odbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine odbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine odbEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Contact OContactBrokerageBL = new Contact();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlComp.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlSegment.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlSegment1.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlCode.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlPPCode.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlPPCode1.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlCharges.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                fillgrid();
            }
            Session["fromdate"] = "";
            Error = "";

            if (HttpContext.Current.Session["userid"] == null)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (Session["Name"] != null)
            {
                lblName.Text = Session["Name"].ToString();
            }
            if (!IsPostBack)
            {
                Session["CompanyID"] = null;
            }
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "1")
            {
                gridBrokerage.Columns[9].Visible = false;
                gridTrading.Columns[6].Visible = false;
                gridOtherCharges.Columns[7].Visible = false;
            }



        }
        void fillgrid()
        {

            dtbrokerage = odbEngine.GetDataTable("SELECT ChargeGroupMembers_ID,ChargeGroupMembers_CustomerID,ChargeGroupMembers_CompanyID,ChargeGroupMembers_SegmentID,ChargeGroupMembers_GroupType,ltrim(rtrim(ChargeGroupMembers_GroupCode)) as ChargeGroupMembers_GroupCode,ChargeGroupMembers_FromDate,ChargeGroupMembers_CreateUser,ChargeGroupMembers_CreateDateTime,ChargeGroupMembers_ModifyUser,ChargeGroupMembers_ModifyDateTime,case ChargeGroupMembers_FromDate when '1/1/1900 12:00:00 AM' then null else convert(varchar(11),ChargeGroupMembers_FromDate,113) end as ChargeGroupMembers_FromDate1,convert(varchar(11),ChargeGroupMembers_UntilDate,113) as ChargeGroupMembers_UntilDate1,''ChargeGroupMembers_UntilDate FROM [Trans_ChargeGroupMembers] where ChargeGroupMembers_CustomerID='" + Session["KeyVal_InternalID"].ToString() + "' order by ChargeGroupMembers_SegmentID,ChargeGroupMembers_UntilDate1");
            gridBrokerage.DataSource = dtbrokerage.DefaultView;
            gridBrokerage.DataBind();

            dttradeprof = odbEngine.GetDataTable("SELECT ProfileMember_ID,ProfileMember_CustomerID,ProfileMember_Type,ltrim(rtrim(ProfileMember_Code)) as ProfileMember_Code,ProfileMember_DateFrom,ProfileMember_DateTo,ProfileMember_CreateUser,ProfileMember_CreateDateTime,ProfileMember_ModifyUser,ProfileMember_ModifyDateTime,convert(varchar(11),ProfileMember_DateFrom,113) as ProfileMember_DateFrom1,convert(varchar(11),ProfileMember_DateTo,113) as ProfileMember_DateTo1 FROM Trans_ProfileMember where ProfileMember_CustomerID='" + Session["KeyVal_InternalID"].ToString() + "' order by ProfileMember_Type");
            gridTrading.DataSource = dttradeprof.DefaultView;
            gridTrading.DataBind();

            dtothercharge = odbEngine.GetDataTable("SELECT OtherChargeMember_ID,OtherChargeMember_CustomerID,OtherChargeMember_OtherChargeCode,OtherChargeMember_DateFrom ,convert(varchar(11),OtherChargeMember_DateFrom,113) as OtherChargeMember_DateFrom1,convert(varchar(11),OtherChargeMember_DateTo,113) as OtherChargeMember_DateTo1,OtherChargeMember_CompanyID,OtherChargeMember_SegmentID FROM [Trans_OtherChargeMember] where OtherChargeMember_CustomerID='" + Session["KeyVal_InternalID"].ToString() + "'");
            gridOtherCharges.DataSource = dtothercharge.DefaultView;
            gridOtherCharges.DataBind();

        }


        #region Code For Brokerage
        protected void gridBrokerage_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string ID = e.Keys[0].ToString();

            DataTable dtdelete = odbEngine.GetDataTable("select ChargeGroupMembers_CustomerID,ChargeGroupMembers_CompanyID,ChargeGroupMembers_SegmentID from Trans_ChargeGroupMembers where ChargeGroupMembers_ID='" + e.Keys[0] + "'");
            string segmentid = dtdelete.Rows[0]["ChargeGroupMembers_SegmentID"].ToString();
            string cmp = dtdelete.Rows[0]["ChargeGroupMembers_CompanyID"].ToString();
            string CustomerID = dtdelete.Rows[0]["ChargeGroupMembers_CustomerID"].ToString();

            /* For Tier Structure

            String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
      
            SqlCommand lcmdBrkgInsert = new SqlCommand("Chargegroupmember_delete", lcon);
            lcmdBrkgInsert.CommandType = CommandType.StoredProcedure;
            lcmdBrkgInsert.Parameters.AddWithValue("@Chargegroupmember_ID", ID);
            lcmdBrkgInsert.Parameters.AddWithValue("@Company", cmp.ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@Client", CustomerID.ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@Segment", segmentid);
            lcmdBrkgInsert.Parameters.AddWithValue("@param", "Brokerage");
            lcmdBrkgInsert.Parameters.AddWithValue("@Tradprof_type", "");


            lcmdBrkgInsert.ExecuteNonQuery();
             */


            // Tier Structure Start ------------

            OContactBrokerageBL.Delete_Chargegroupmember(ID, cmp.ToString().Trim(), CustomerID.ToString().Trim(), segmentid, "Brokerage", "");


            // Tier Structure End ----------------

            e.Cancel = true;
            fillgrid();
            gridBrokerage.CancelEdit();

        }
        protected void gridBrokerage_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxComboBox company = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compCompany");
            ASPxComboBox segment = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compSegment");
            ASPxComboBox Mode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compMode");
            ASPxComboBox GCode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("comboCode");
            ASPxDateEdit date = (ASPxDateEdit)gridBrokerage.FindEditFormTemplateControl("dtDate");
            SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + company.SelectedItem.Value + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
            segment.DataBind();
            e.NewValues["ChargeGroupMembers_SegmentID"] = segment.SelectedItem.Value;
            e.NewValues["ChargeGroupMembers_GroupType"] = Mode.SelectedItem.Value;
            string BID = "";
            if (Mode.SelectedItem.Value == "1")
            {

                e.NewValues["ChargeGroupMembers_GroupCode"] = Session["KeyVal_InternalID"].ToString();
                string[,] ID = odbEngine.GetFieldValue("config_brokerageMain", "top 1 brokerageMain_ID", " brokerageMain_customerid='" + Session["KeyVal_InternalID"].ToString() + "' and brokerageMain_CompanyID='" + company.SelectedItem.Value + "' and brokerageMain_SegmentID='" + segment.SelectedItem.Value + "'", 1);
                if (ID[0, 0] != "n")
                    BID = ID[0, 0].ToString();
                else
                    BID = "0";
                Error = "3" + "~" + "ADD" + "~" + Session["KeyVal_InternalID"].ToString() + "~" + company.SelectedItem.Value + "~" + segment.SelectedItem.Value + "~" + lblName.Text + "~" + company.SelectedItem.Text + "~" + segment.SelectedItem.Text + "~" + "C" + "~" + BID;
                Session["CompanyID"] = company.SelectedItem.Text + " [" + segment.SelectedItem.Text + "]";
                Session["Date"] = date.Value.ToString();
            }

            else
            {
                Error = "";
                if (GCode.SelectedItem == null)
                {
                    e.NewValues["ChargeGroupMembers_GroupCode"] = Session["KeyVal_InternalID"].ToString();

                }
                else
                {
                    e.NewValues["ChargeGroupMembers_GroupCode"] = GCode.SelectedItem.Value;
                }
            }

            DateTime From = Convert.ToDateTime(date.Value);
            if (From == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
            {
                From = Convert.ToDateTime("1/1/1900 12:00:00 AM");
            }
            e.NewValues["ChargeGroupMembers_FromDate"] = From;

            /* For Tier Structure

            String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
            SqlCommand lcmdBrkgInsert = new SqlCommand("insertBrokerage", lcon);
            lcmdBrkgInsert.CommandType = CommandType.StoredProcedure;
            lcmdBrkgInsert.Parameters.AddWithValue("@ChargeGroupMembers_CompanyID", company.SelectedItem.Value);
            lcmdBrkgInsert.Parameters.AddWithValue("@ChargeGroupMembers_CustomerID", Session["KeyVal_InternalID"].ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@ChargeGroupMembers_SegmentID", segment.SelectedItem.Value);
            lcmdBrkgInsert.Parameters.AddWithValue("@ChargeGroupMembers_CreateUser", HttpContext.Current.Session["userid"]);
            lcmdBrkgInsert.Parameters.AddWithValue("@ChargeGroupMembers_FromDate", From);
            lcmdBrkgInsert.Parameters.AddWithValue("@ChargeGroupMembers_GroupType", Mode.SelectedItem.Value);
       
        
            if (Mode.SelectedItem.Value == "1")
            {
                lcmdBrkgInsert.Parameters.AddWithValue("@ChargeGroupMembers_GroupCode", Session["KeyVal_InternalID"].ToString());
            }
            else
            {
                lcmdBrkgInsert.Parameters.AddWithValue("@ChargeGroupMembers_GroupCode", GCode.SelectedItem.Value.ToString());
            }

            lcmdBrkgInsert.ExecuteNonQuery();

            */

            //-------------- For Tier Start-----------------------------------------------------------

            string vGroupCard = "";

            if (Mode.SelectedItem.Value == "1")
            {
                vGroupCard = Session["KeyVal_InternalID"].ToString();
            }
            else
            {
                vGroupCard = GCode.SelectedItem.Value.ToString();
            }

            OContactBrokerageBL.Insert_Brokerage(Session["KeyVal_InternalID"].ToString().Trim(), company.SelectedItem.Value.ToString(), segment.SelectedItem.Value.ToString(),
            Mode.SelectedItem.Value.ToString(), vGroupCard, From, HttpContext.Current.Session["userid"].ToString());



            //string Session["KeyVal_InternalID"].ToString().Trim(),company.SelectedItem.Value,segment.SelectedItem.Value,
            //Mode.SelectedItem.Value,Session["KeyVal_InternalID"].ToString(),From,HttpContext.Current.Session["userid"].ToString()


            //-------------- For Tier End---------------------------------------------------------------

            fillgrid();
            if (Mode.SelectedItem.Value == "1")
            {

                e.NewValues["ChargeGroupMembers_GroupCode"] = Session["KeyVal_InternalID"].ToString();
                string[,] ID = odbEngine.GetFieldValue("config_brokerageMain", "top 1 brokerageMain_ID", " brokerageMain_customerid='" + Session["KeyVal_InternalID"].ToString() + "' and brokerageMain_CompanyID='" + company.SelectedItem.Value + "' and brokerageMain_SegmentID='" + segment.SelectedItem.Value + "'", 1);
                if (ID[0, 0] != "n")
                {
                    BID = ID[0, 0].ToString();
                    Error = "3" + "~" + "ADD" + "~" + Session["KeyVal_InternalID"].ToString() + "~" + company.SelectedItem.Value + "~" + segment.SelectedItem.Value + "~" + lblName.Text + "~" + company.SelectedItem.Text + "~" + segment.SelectedItem.Text + "~" + "C" + "~" + BID;
                }
                else
                {
                    Session["fromdate"] = date.Date;
                    BID = "0";
                    Error = "3" + "~" + "AADD" + "~" + Session["KeyVal_InternalID"].ToString() + "~" + company.SelectedItem.Value + "~" + segment.SelectedItem.Value + "~" + lblName.Text + "~" + company.SelectedItem.Text + "~" + segment.SelectedItem.Text + "~" + "C" + "~" + BID;
                }
                // Error = "3" + "~" + "ADD" + "~" + Session["KeyVal_InternalID"].ToString() + "~" + company.SelectedItem.Value + "~" + segment.SelectedItem.Value + "~" + lblName.Text + "~" + company.SelectedItem.Text + "~" + segment.SelectedItem.Text + "~" + "C" + "~" + BID;
                Session["CompanyID"] = company.SelectedItem.Text + " [" + segment.SelectedItem.Text + "]";
                Session["Date"] = date.Value.ToString();
            }
            else
            {
                Error = "";
            }
            e.Cancel = true;
            gridBrokerage.CancelEdit();

        }
        protected void gridBrokerage_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {

            string id = e.EditingKeyValue.ToString();
            DataTable dtEdit = odbEngine.GetDataTable("Trans_ChargeGroupMembers", "*", "ChargeGroupMembers_ID='" + id + "'");
            if (dtEdit.Rows.Count > 0)
            {
                if (Session["userlastsegment"].ToString() != "1")
                {
                    ASPxComboBox segment = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compSegment");
                    ASPxComboBox Mode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compMode");
                    ASPxComboBox GCode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("comboCode");
                    ASPxDateEdit date = (ASPxDateEdit)gridBrokerage.FindEditFormTemplateControl("dtDate");
                    ASPxButton btnupdate = (ASPxButton)gridBrokerage.FindEditFormTemplateControl("btnUpdate");
                    ASPxButton btnclose = (ASPxButton)gridBrokerage.FindEditFormTemplateControl("btnCancel");
                    SqlSegment.SelectCommand = "select exch_internalId,isnull(((select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+ exch_segmentId),exch_membershipType) as Exchange from tbl_master_companyExchange where exch_compId='" + Session["LastCompany"].ToString() + "'";
                    segment.DataBind();
                    segment.Value = dtEdit.Rows[0][3].ToString();
                    Mode.Value = dtEdit.Rows[0][4].ToString();
                    GCode.Value = dtEdit.Rows[0][5].ToString().Trim();
                    Mode.Enabled = false;
                    GCode.Enabled = false;
                    date.ClientEnabled = false;
                    btnupdate.ClientVisible = false;
                    btnclose.Text = "Close";

                    if (dtEdit.Rows[0][4].ToString() == "1")
                    {
                        Error = "0~1";
                    }
                    else
                    {
                        Error = "1~1";
                    }
                    //date.Enabled = false;
                }
                else
                {
                    ASPxComboBox company = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compCompany");
                    ASPxComboBox segment = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compSegment");
                    ASPxComboBox Mode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compMode");
                    ASPxComboBox GCode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("comboCode");
                    ASPxDateEdit date = (ASPxDateEdit)gridBrokerage.FindEditFormTemplateControl("dtDate");
                    ASPxButton btnupdate = (ASPxButton)gridBrokerage.FindEditFormTemplateControl("btnUpdate");
                    ASPxButton btnclose = (ASPxButton)gridBrokerage.FindEditFormTemplateControl("btnCancel");
                    company.Value = dtEdit.Rows[0][2].ToString();
                    SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + dtEdit.Rows[0][2].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
                    segment.DataBind();
                    segment.Value = dtEdit.Rows[0][3].ToString();
                    Mode.Value = dtEdit.Rows[0][4].ToString();
                    GCode.Value = dtEdit.Rows[0][5].ToString().Trim();
                    Mode.Enabled = false;
                    GCode.Enabled = false;
                    date.ClientEnabled = false;
                    btnupdate.ClientVisible = false;
                    btnclose.Text = "Close";
                    if (dtEdit.Rows[0][4].ToString() == "1")
                    {
                        Error = "0~1";
                    }
                    else
                    {
                        Error = "1~1";
                    }


                }

            }
            gridBrokerage.JSProperties["cpInsertError"] = "";
        }
        protected void gridBrokerage_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxComboBox company = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compCompany");
            ASPxComboBox segment = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compSegment");
            ASPxComboBox Mode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compMode");
            ASPxComboBox GCode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("comboCode");
            ASPxDateEdit date = (ASPxDateEdit)gridBrokerage.FindEditFormTemplateControl("dtDate");
            SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + company.SelectedItem.Value + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
            segment.DataBind();
            e.NewValues["ChargeGroupMembers_SegmentID"] = segment.SelectedItem.Value;
            e.NewValues["ChargeGroupMembers_GroupType"] = Mode.SelectedItem.Value;
            if (Mode.SelectedItem.Value == "1")
            {
                string BID = "";
                e.NewValues["ChargeGroupMembers_GroupCode"] = Session["KeyVal_InternalID"].ToString();
                string[,] ID = odbEngine.GetFieldValue("config_brokerageMain", "top 1 brokerageMain_ID", " brokerageMain_customerid='" + Session["KeyVal_InternalID"].ToString() + "' and brokerageMain_CompanyID='" + company.SelectedItem.Value + "' and brokerageMain_SegmentID='" + segment.SelectedItem.Value + "'", 1);
                if (ID[0, 0] != "n")
                    BID = ID[0, 0].ToString();
                else
                    BID = "0";
                Error = "3" + "~" + "ADD" + "~" + Session["KeyVal_InternalID"].ToString() + "~" + company.SelectedItem.Value + "~" + segment.SelectedItem.Value + "~" + lblName.Text + "~" + company.SelectedItem.Text + "~" + segment.SelectedItem.Text + "~" + "C" + "~" + BID;
                Session["CompanyID"] = company.SelectedItem.Text + " [" + segment.SelectedItem.Text + "]";
                Session["Date"] = date.Value.ToString();
            }
            else
            {
                Error = "";
                if (GCode.SelectedItem == null)
                {
                    e.NewValues["ChargeGroupMembers_GroupCode"] = Session["KeyVal_InternalID"].ToString();
                }
                else
                {
                    e.NewValues["ChargeGroupMembers_GroupCode"] = GCode.SelectedItem.Value;
                }
            }
            DateTime From = Convert.ToDateTime(date.Value);
            if (From == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
            {
                From = Convert.ToDateTime("1/1/1900 12:00:00 AM");
            }
            e.NewValues["ChargeGroupMembers_FromDate"] = From;
        }
        protected void gridBrokerage_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpInsertError"] = Error;

        }
        protected void gridBrokerage_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (!gridBrokerage.IsNewRowEditing)
            {
                string keyval = e.Keys[0].ToString();
                ASPxDateEdit date = (ASPxDateEdit)gridBrokerage.FindEditFormTemplateControl("dtDate");
                ASPxComboBox company = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compCompany");
                ASPxComboBox segment = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compSegment");
                //Session["fromdate"] = date.Date;
                SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + company.SelectedItem.Value + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
                segment.DataBind();
                string[,] FromDate = odbEngine.GetFieldValue("Trans_ChargeGroupMembers", "ChargeGroupMembers_ID", " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,ChargeGroupMembers_FromDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + date.Value + "')) as datetime) and ChargeGroupMembers_CustomerID='" + Session["KeyVal_InternalID"].ToString() + "' and ChargeGroupMembers_ID<>'" + keyval + "' and ChargeGroupMembers_SegmentID='" + segment.SelectedItem.Value + "'", 1);
                if (FromDate[0, 0] != "n")
                {
                    e.RowError = "This Date Already Exists !";
                    return;
                }
            }
            else
            {
                ASPxComboBox Mode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compMode");
                ASPxComboBox GCode = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("comboCode");
                ASPxDateEdit date = (ASPxDateEdit)gridBrokerage.FindEditFormTemplateControl("dtDate");
                ASPxComboBox company = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compCompany");
                ASPxComboBox segment = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compSegment");
                if (Mode.SelectedItem == null)
                {
                    e.RowError = "Please Select Mode !";
                    return;
                }
                if (Mode.SelectedItem.Value != "1")
                {
                    if (GCode.SelectedItem == null)
                    {
                        e.RowError = "Please Select Group Code !";
                        return;

                    }
                }



                //Session["fromdate"] = date.Date;
                SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + company.SelectedItem.Value + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
                segment.DataBind();
                string[,] FromDate = odbEngine.GetFieldValue("Trans_ChargeGroupMembers", "ChargeGroupMembers_ID", " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,ChargeGroupMembers_FromDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + date.Value + "')) as datetime) and ChargeGroupMembers_CustomerID='" + Session["KeyVal_InternalID"].ToString() + "' and ChargeGroupMembers_SegmentID='" + segment.SelectedItem.Value + "'", 1);
                if (FromDate[0, 0] != "n")
                {
                    e.RowError = "This Date Already Exists !";
                    return;
                }
            }
        }
        protected void gridBrokerage_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {

            if (Session["userlastsegment"].ToString() != "1")
            {
                ASPxComboBox company = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compCompany");
                ASPxComboBox segment = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compSegment");
                SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
                segment.DataBind();
                company.Value = Session["LastCompany"].ToString();
                string[,] seg = odbEngine.GetFieldValue("tbl_master_segment", "seg_name", " seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 1);
                if (seg[0, 0] != "n")
                {
                    segment.Text = seg[0, 0];
                }
                segment.ClientEnabled = false;
                company.ClientEnabled = false;
                Error = "ADD";
            }
        }
        protected void gridbrokerage_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            int rowindex = e.VisibleIndex;
            //if (Session["LastCompany"].ToString() == null)
            //    e.Enabled = false;
            string segment = gridBrokerage.GetRowValues(rowindex, "ChargeGroupMembers_SegmentID").ToString();
            string date = gridBrokerage.GetRowValues(rowindex, "ChargeGroupMembers_UntilDate1").ToString();
            if (date != "")
            {
                GridViewCommandColumn ccol = (GridViewCommandColumn)(gridBrokerage.Columns[10]);

                e.Enabled = false;



            }
            if (Session["userlastsegment"].ToString() != "1")
            {
                if (segment != HttpContext.Current.Session["usersegid"].ToString())
                {
                    GridViewCommandColumn ccol = (GridViewCommandColumn)(gridBrokerage.Columns[10]);

                    e.Visible = false;

                }
            }



        }
        #endregion

        #region Code For Gridtrading
        protected void gridTrading_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string ID = e.Keys[0].ToString();

            DataTable dtdeleteprof = odbEngine.GetDataTable("select ProfileMember_CustomerID,ProfileMember_Type from Trans_ProfileMember where ProfileMember_ID='" + e.Keys[0] + "'");

            string cmp = dtdeleteprof.Rows[0]["ProfileMember_Type"].ToString();
            string CustomerID = dtdeleteprof.Rows[0]["ProfileMember_CustomerID"].ToString();

            /* For Tier Structure
            String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
            SqlCommand lcmdBrkgInsert = new SqlCommand("Chargegroupmember_delete", lcon);
            lcmdBrkgInsert.CommandType = CommandType.StoredProcedure;
            lcmdBrkgInsert.Parameters.AddWithValue("@Chargegroupmember_ID", ID); 
            lcmdBrkgInsert.Parameters.AddWithValue("@Tradprof_type", cmp.ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@Client", CustomerID.ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@Company", "");
            lcmdBrkgInsert.Parameters.AddWithValue("@Segment", "");
            lcmdBrkgInsert.Parameters.AddWithValue("@param", "Trading");
            lcmdBrkgInsert.ExecuteNonQuery();
             */

            // Tier Structure Start ------------

            OContactBrokerageBL.Delete_Chargegroupmember(ID, "", CustomerID.ToString().Trim(), "", "Trading", cmp.ToString().Trim());



            // Tier Structure End ----------------


            e.Cancel = true;
            fillgrid();
            gridTrading.CancelEdit();
        }
        protected void gridTrading_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxComboBox compType = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compType");
            ASPxComboBox compCode = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compCode");
            ASPxDateEdit date2 = (ASPxDateEdit)gridTrading.FindEditFormTemplateControl("dDate");
            SqlPPCode.SelectCommand = "select TradingProfile_Code,TradingProfile_Name from Master_TradingProfile where TradingProfile_Type='" + compType.SelectedItem.Value + "'";
            compCode.DataBind();
            e.NewValues["ProfileMember_Type"] = compType.SelectedItem.Value;
            e.NewValues["ProfileMember_Code"] = compCode.SelectedItem.Value;
            DateTime From = Convert.ToDateTime(date2.Value);
            if (From == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
            {
                From = Convert.ToDateTime("1/1/1900 12:00:00 AM");
            }
            e.NewValues["ProfileMember_DateFrom"] = From;

            /*
            String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
            SqlCommand lcmdBrkgInsert = new SqlCommand("insertTradingProfile", lcon);
            lcmdBrkgInsert.CommandType = CommandType.StoredProcedure;
            lcmdBrkgInsert.Parameters.AddWithValue("@ProfileMember_Type", compType.SelectedItem.Value);
            lcmdBrkgInsert.Parameters.AddWithValue("@ProfileMember_CustomerID", Session["KeyVal_InternalID"].ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@ProfileMember_Code", compCode.SelectedItem.Value);
            lcmdBrkgInsert.Parameters.AddWithValue("@ProfileMember_CreateUser", HttpContext.Current.Session["userid"]);
            lcmdBrkgInsert.Parameters.AddWithValue("@ProfileMember_DateFrom", From);
            lcmdBrkgInsert.ExecuteNonQuery();
            */

            // For Tier Structure Start --------------

            OContactBrokerageBL.Insert_Trading(compType.SelectedItem.Value.ToString(), Session["KeyVal_InternalID"].ToString().Trim(),
               compCode.SelectedItem.Value.ToString(), HttpContext.Current.Session["userid"].ToString(), From);

            //-------For Tier Structure End-----------

            fillgrid();
            e.Cancel = true;
            gridTrading.CancelEdit();
        }
        protected void gridTrading_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxComboBox compType = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compType");
            ASPxComboBox compCode = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compCode");
            ASPxDateEdit date2 = (ASPxDateEdit)gridTrading.FindEditFormTemplateControl("dDate");
            SqlPPCode.SelectCommand = "select TradingProfile_Code,TradingProfile_Name from Master_TradingProfile where TradingProfile_Type='" + compType.SelectedItem.Value + "'";
            compCode.DataBind();
            e.NewValues["ProfileMember_Type"] = compType.SelectedItem.Value;
            e.NewValues["ProfileMember_Code"] = compCode.SelectedItem.Value;
            DateTime From = Convert.ToDateTime(date2.Value);
            if (From == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
            {
                From = Convert.ToDateTime("1/1/1900 12:00:00 AM");
            }
            e.NewValues["ProfileMember_DateFrom"] = From;
        }
        protected void gridTrading_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {

            string id = e.EditingKeyValue.ToString();
            DataTable dtEdit = odbEngine.GetDataTable("Trans_ProfileMember", "ProfileMember_Type,ProfileMember_Code,(select TradingProfile_Name from Master_TradingProfile where TradingProfile_Code=ProfileMember_Code ) as new", "ProfileMember_ID='" + id + "'");
            if (dtEdit.Rows.Count > 0)
            {
                ASPxComboBox Type = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compType");
                ASPxComboBox Code = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compCode");
                ASPxButton btnupdate1 = (ASPxButton)gridTrading.FindEditFormTemplateControl("btnUpdate1");
                ASPxButton btnclose1 = (ASPxButton)gridTrading.FindEditFormTemplateControl("btnCancel1");
                ASPxDateEdit date1 = (ASPxDateEdit)gridTrading.FindEditFormTemplateControl("dDate");
                SqlPPCode.SelectCommand = "select TradingProfile_Code,TradingProfile_Name from Master_TradingProfile where TradingProfile_Type='" + dtEdit.Rows[0][0].ToString() + "'";
                Code.DataBind();
                Code.Value = dtEdit.Rows[0][2].ToString().Trim();
                Type.Value = dtEdit.Rows[0][0].ToString().Trim();
                Type.Enabled = false;
                Code.ClientEnabled = false;
                btnupdate1.ClientVisible = false;
                date1.ClientEnabled = false;
                btnclose1.Text = "Close";
            }
        }
        protected void gridTrading_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (gridTrading.IsNewRowEditing)
            {
                ASPxComboBox Mode = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compType");
                ASPxComboBox GCode = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compCode");
                ASPxDateEdit date = (ASPxDateEdit)gridTrading.FindEditFormTemplateControl("dDate");
                if (Mode.SelectedItem == null)
                {
                    e.RowError = "Please Select Type !";
                    return;
                }
                if ((GCode.Text == null) || (GCode.Text == ""))
                {
                    e.RowError = "Please Select Code !";
                    return;
                }
                if (date.Date.ToString() == "1/1/0001 12:00:00 AM")
                {
                    e.RowError = "Please Select date !";
                    return;
                }

            }

        }
        protected void gridTrading_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            int rowindex = e.VisibleIndex;
            //if (Session["LastCompany"].ToString() == null)
            //    e.Enabled = false;
            string datetrade = gridTrading.GetRowValues(rowindex, "ProfileMember_DateTo1").ToString();
            if (datetrade != "")
            {
                //GridViewCommandColumn ccol = (GridViewCommandColumn)(gridBrokerage.Columns[10]);

                e.Enabled = false;



            }
        }
        #endregion

        #region Code for GridOthercharges
        protected void gridOtherCharges_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            int rowindex = e.VisibleIndex;
            //if (Session["LastCompany"].ToString() == null)
            //    e.Enabled = false;
            string segment = gridOtherCharges.GetRowValues(rowindex, "OtherChargeMember_SegmentID").ToString();
            string date = gridOtherCharges.GetRowValues(rowindex, "OtherChargeMember_DateTo1").ToString();
            if (date != "")
            {
                GridViewCommandColumn ccol = (GridViewCommandColumn)(gridOtherCharges.Columns[7]);

                e.Enabled = false;



            }
            if (Session["userlastsegment"].ToString() != "1")
            {
                if (segment != HttpContext.Current.Session["usersegid"].ToString())
                {
                    GridViewCommandColumn ccol = (GridViewCommandColumn)(gridOtherCharges.Columns[7]);

                    e.Visible = false;

                }
            }



        }
        protected void gridOtherCharges_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string IDother = e.Keys[0].ToString();

            DataTable dtdeleteprof = odbEngine.GetDataTable("select OtherChargeMember_CustomerID,OtherChargeMember_CompanyID,OtherChargeMember_SegmentID from Trans_OtherChargeMember where OtherChargeMember_ID='" + e.Keys[0] + "'");

            string segmentid = dtdeleteprof.Rows[0]["OtherChargeMember_SegmentID"].ToString();
            string cmp = dtdeleteprof.Rows[0]["OtherChargeMember_CompanyID"].ToString();
            string CustomerID = dtdeleteprof.Rows[0]["OtherChargeMember_CustomerID"].ToString();

            /* For Tier Structure
            String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
            SqlCommand lcmdBrkgInsert = new SqlCommand("Chargegroupmember_delete", lcon);
            lcmdBrkgInsert.CommandType = CommandType.StoredProcedure;
            lcmdBrkgInsert.Parameters.AddWithValue("@Chargegroupmember_ID", IDother);
            lcmdBrkgInsert.Parameters.AddWithValue("@Company", cmp.ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@Client", CustomerID.ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@Segment", segmentid.ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@param", "Other");
            lcmdBrkgInsert.Parameters.AddWithValue("@Tradprof_type", "");
            lcmdBrkgInsert.ExecuteNonQuery();
             */

            // Tier Structure Start ------------

            OContactBrokerageBL.Delete_Chargegroupmember(IDother, cmp.ToString().Trim(), CustomerID.ToString().Trim(), segmentid.ToString().Trim(), "Other", "");


            // Tier Structure End ----------------

            e.Cancel = true;
            fillgrid();
            gridOtherCharges.CancelEdit();
        }
        protected void gridOtherCharges_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxComboBox compOtherCCode = (ASPxComboBox)gridOtherCharges.FindEditFormTemplateControl("compOtherCCode");
            ASPxDateEdit dtOtherCDate = (ASPxDateEdit)gridOtherCharges.FindEditFormTemplateControl("dtOtherCDate");
            ASPxComboBox company = (ASPxComboBox)gridOtherCharges.FindEditFormTemplateControl("cmbCompany");
            ASPxComboBox segment = (ASPxComboBox)gridOtherCharges.FindEditFormTemplateControl("cmbSegment");
            e.NewValues["OtherChargeMember_OtherChargeCode"] = compOtherCCode.SelectedItem.Value;
            // e.NewValues["OtherChargeMember_SegmentID"] = HttpContext.Current.Session["userlastsegment"].ToString();
            e.NewValues["OtherChargeMember_SegmentID"] = HttpContext.Current.Session["usersegid"].ToString();
            DateTime From = Convert.ToDateTime(dtOtherCDate.Value);
            if (From == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
            {
                From = Convert.ToDateTime("1/1/1900 12:00:00 AM");
            }
            e.NewValues["OtherChargeMember_DateFrom"] = From;

            /*

            String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
            SqlCommand lcmdBrkgInsert = new SqlCommand("insertOtherCharges", lcon);
            lcmdBrkgInsert.CommandType = CommandType.StoredProcedure;
            lcmdBrkgInsert.Parameters.AddWithValue("@OtherChargeMember_CompanyID", company.SelectedItem.Value);
            lcmdBrkgInsert.Parameters.AddWithValue("@OtherChargeMember_CustomerID", Session["KeyVal_InternalID"].ToString().Trim());
            lcmdBrkgInsert.Parameters.AddWithValue("@OtherChargeMember_SegmentID", segment.SelectedItem.Value);
            lcmdBrkgInsert.Parameters.AddWithValue("@OtherChargeMember_CreateUser", HttpContext.Current.Session["userid"]);
            lcmdBrkgInsert.Parameters.AddWithValue("@OtherChargeMember_DateFrom", From);
            lcmdBrkgInsert.Parameters.AddWithValue("@OtherChargeMember_OtherChargeCode", compOtherCCode.SelectedItem.Value);

            lcmdBrkgInsert.ExecuteNonQuery();

             */
            //-------------- For Tier Start-----------------------------------------------------------


            OContactBrokerageBL.Insert_OtherChanges(company.SelectedItem.Value.ToString(), Session["KeyVal_InternalID"].ToString().Trim(),
               segment.SelectedItem.Value.ToString(), HttpContext.Current.Session["userid"].ToString(), From, compOtherCCode.SelectedItem.Value.ToString());

            //-------------- For Tier End---------------------------------------------------------------

            fillgrid();
            e.Cancel = true;
            gridOtherCharges.CancelEdit();
        }
        protected void gridOtherCharges_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string keyVal = e.Keys[0].ToString();
            ASPxComboBox compOtherCCode = (ASPxComboBox)gridOtherCharges.FindEditFormTemplateControl("compOtherCCode");
            ASPxDateEdit dtOtherCDate = (ASPxDateEdit)gridOtherCharges.FindEditFormTemplateControl("dtOtherCDate");
            e.NewValues["OtherChargeMember_OtherChargeCode"] = compOtherCCode.SelectedItem.Value;
            e.NewValues["OtherChargeMember_SegmentID"] = HttpContext.Current.Session["usersegid"].ToString();
            // e.NewValues["OtherChargeMember_SegmentID"] = HttpContext.Current.Session["userlastsegment"].ToString();
            DateTime From = Convert.ToDateTime(dtOtherCDate.Value);
            if (From == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
            {
                From = Convert.ToDateTime("1/1/1900 12:00:00 AM");
            }
            e.NewValues["OtherChargeMember_DateFrom"] = From;
            //int NoofAffected = odbEngine.SetFieldValue("Trans_OtherChargeMember", "[OtherChargeMember_OtherChargeCode] ='" + compOtherCCode.SelectedItem.Value + "', [OtherChargeMember_DateFrom] = '" + From + "', [OtherChargeMember_ModifyUser] = '" + Session["userid"].ToString() + "', [OtherChargeMember_ModifyDateTime] = '" + oDBEngine.GetDate().ToString() + "'", " OtherChargeMember_ID='" + keyVal + "'");
        }
        protected void gridOtherCharges_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (gridOtherCharges.IsNewRowEditing)
            {
                ASPxComboBox Mode = (ASPxComboBox)gridOtherCharges.FindEditFormTemplateControl("compOtherCCode");
                ASPxDateEdit date = (ASPxDateEdit)gridOtherCharges.FindEditFormTemplateControl("dtOtherCDate");
                if (Mode.SelectedItem == null)
                {
                    e.RowError = "Please Select Type !";
                    return;
                }

                if (date.Date.ToString() == "1/1/0001 12:00:00 AM")
                {
                    e.RowError = "Please Select date !";
                    return;
                }

            }

        }
        protected void gridOtherCharges_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxButton btnupdate1 = (ASPxButton)gridOtherCharges.FindEditFormTemplateControl("btnUpdate2");
            ASPxButton btnclose1 = (ASPxButton)gridOtherCharges.FindEditFormTemplateControl("btnCancel2");
            ASPxDateEdit date1 = (ASPxDateEdit)gridOtherCharges.FindEditFormTemplateControl("dtOtherCDate");
            ASPxComboBox Type = (ASPxComboBox)gridOtherCharges.FindEditFormTemplateControl("compOtherCCode");
            btnupdate1.ClientVisible = false;
            btnclose1.Text = "Close";
            Type.Enabled = false;
            date1.ClientEnabled = false;
        }
        //protected void gridOtherCharges_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        //{
        //    int rowindex = e.VisibleIndex;
        //    //if (Session["LastCompany"].ToString() == null)
        //    //    e.Enabled = false;
        //    string datetrade = gridOtherCharges.GetRowValues(rowindex, "OtherChargeMember_DateTo1").ToString();
        //    if (datetrade != "")
        //    {
        //        //GridViewCommandColumn ccol = (GridViewCommandColumn)(gridBrokerage.Columns[10]);

        //        e.Enabled = false;


        //    }



        //}
        protected void gridOtherCharges_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (Session["userlastsegment"].ToString() != "1")
            {
                ASPxComboBox company = (ASPxComboBox)gridOtherCharges.FindEditFormTemplateControl("cmbCompany");
                ASPxComboBox segment = (ASPxComboBox)gridOtherCharges.FindEditFormTemplateControl("cmbSegment");
                SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
                segment.DataBind();
                company.Value = Session["LastCompany"].ToString();
                string[,] seg = odbEngine.GetFieldValue("tbl_master_segment", "seg_name", " seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 1);
                if (seg[0, 0] != "n")
                {
                    segment.Text = seg[0, 0];
                }
                segment.ClientEnabled = false;
                company.ClientEnabled = false;
            }
        }
        #endregion
        protected void compSegment_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox segment = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compSegment");
            ASPxComboBox company = (ASPxComboBox)gridBrokerage.FindEditFormTemplateControl("compCompany");
            SqlSegment.SelectCommand = "select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS Exchange from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + company.SelectedItem.Value + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
            segment.DataBind();
        }
        protected void compCode_Callback(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox type = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compType");
            ASPxComboBox code = (ASPxComboBox)gridTrading.FindEditFormTemplateControl("compCode");
            SqlPPCode.SelectCommand = "select TradingProfile_Code,TradingProfile_Name from Master_TradingProfile where TradingProfile_Type='" + type.SelectedItem.Value + "'";
            code.DataBind();
        }


    }
}