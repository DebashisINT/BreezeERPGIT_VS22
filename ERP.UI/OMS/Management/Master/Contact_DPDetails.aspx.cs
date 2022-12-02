using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

using System.Configuration;
using DevExpress.Web;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_Contact_DPDetails : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine dbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine dbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine dbEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        String bankid = "B";
        string DPID = "Y";
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        String Stat = "N";
        // string TESTID = "N";
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


            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            SelectDp.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //DpDetailsdata.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    DpDetailsdata.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //DpDetailsdata.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    DpDetailsdata.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                StDate.UseMaskBehavior = true;
                StDate.EditFormatString = Oconverter.GetDateFormat("Date");

                if (Session["Name"] != null)
                {
                    lblName.Text = Session["Name"].ToString();
                }
            }
            if (Request.QueryString["formtype"] != null)
            {
                string ID = Session["InternalId"].ToString();
                Session["KeyVal_InternalID_New"] = ID.ToString();
                DisabledTabPage();
            }
            else
            {
                if (Session["KeyVal_InternalID"] != null)
                {
                    Session["KeyVal_InternalID_New"] = Session["KeyVal_InternalID"].ToString();
                }
            }

        }
        public void DisabledTabPage()
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("General");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Bank");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Documents");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Family");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Registration");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Group");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Deposit");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Remarks");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Education");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
            page.Enabled = false;
            page = ASPxPageControl1.TabPages.FindByName("Other");
            page.Enabled = false;
        }


        protected void DpDetailsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            TextBox DPname = (TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtDPName");
            DPname.Attributes.Add("onkeyup", "CallList(this,'DPName',event)");


            string IsPOA = ((ASPxComboBox)DpDetailsGrid.FindEditFormTemplateControl("comboPOA")).Text;

            ASPxDateEdit TxtFPOADate = ((ASPxDateEdit)DpDetailsGrid.FindEditFormTemplateControl("TxtPOADate"));
            TextBox TxtFPOAName = ((TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtPoaName"));

            Label lblFPOADate = ((Label)DpDetailsGrid.FindEditFormTemplateControl("lblPOADate"));
            Label lblFPOAName = ((Label)DpDetailsGrid.FindEditFormTemplateControl("lblPOAName"));

            if (IsPOA == "Yes")
            //if (IsPOA == "1")
            {

                TxtFPOADate.Visible = true;
                TxtFPOAName.Visible = true;
                lblFPOADate.Visible = true;
                lblFPOAName.Visible = true;

            }
            else
            {

                TxtFPOADate.Visible = false;
                TxtFPOAName.Visible = false;
                lblFPOADate.Visible = false;
                lblFPOAName.Visible = false;

            }


        }
        protected void DpDetailsGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            TextBox DPname = (TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtDPName_hidden");
            e.NewValues["DPName"] = DPname.Text;
        }
        protected void DpDetailsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            TextBox DPname = (TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtDPName_hidden");
            e.NewValues["DPName"] = DPname.Text;
        }
        protected void DpDetailsGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["Category"] == null)
            {
                e.RowError = "Please Select Category";
                return;
            }
            //if (e.NewValues["DPName"] == null)
            //{
            //    e.RowError = "Please Select Dpname";
            //    return;
            //}
            if (e.NewValues["DP"] == "")
            {
                e.RowError = "Please Enter DP Name";
                return;
            }
            if (e.NewValues["ClientId"] == "")
            {
                e.RowError = "Please Enter Client ID";
                return;


            }
            if (e.NewValues["ClientId"] != "")
            {
                //e.RowError = "Please Enter Client ID";
                //return;
                string len = Convert.ToString(e.NewValues["ClientId"]);
                string var = Convert.ToString(len.Length);
                if (len.Length < 8)
                {
                    e.RowError = "ClientID Must be 8 character long.";
                    return;
                }

            }

            if (e.NewValues["POA"] == "")
            {
                e.RowError = "Please Select POA";
                return;
            }
            if (DpDetailsGrid.IsNewRowEditing)
            {
                string Category = e.NewValues["Category"].ToString();
                string ClientID = e.NewValues["ClientId"].ToString();



                string[,] Category1 = dbEngine.GetFieldValue("tbl_master_contactDPDetails", "dpd_accountType", " dpd_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "' and dpd_accountType='" + Category + "'", 1);

                if (Category1[0, 0] != "n")
                {
                    if (Category1[0, 0] == "Default")
                    {
                        e.RowError = "Default Category Already Exists!";
                        return;
                    }
                    if (Category1[0, 0] == "CommodityDP")
                    {
                        e.RowError = "CommodityDP Category Already Exists!";
                        return;
                    }
                }


            }
            else
            {
                string KeyVal = e.Keys["Id"].ToString();
                string Category = e.NewValues["Category"].ToString();
                if (Category != "Secondary" && Category != "CommodityDP Sec ")
                {
                    string[,] Category1 = dbEngine.GetFieldValue("tbl_master_contactDPDetails", "dpd_id", " dpd_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "' and dpd_accountType='" + Category + "'", 1);
                    if (Category1[0, 0] != "n")
                    {
                        if (KeyVal != Category1[0, 0])
                        {
                            e.RowError = "Category Already Exists!";
                            return;
                        }
                    }
                }
            }

            TextBox DPname = (TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtDPName_hidden");
            //e.NewValues["DPName"] = DPname.Text;
            string DpCode = DPname.Text;
            string CntID = e.NewValues["ClientId"].ToString();
            DataTable dtCnt = dbEngine.GetDataTable("tbl_master_contactDPDetails", "(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+ isnull(ltrim(rtrim(cnt_middlename)),'') +' '+isnull(ltrim(rtrim(cnt_lastname)),'') ++'['+ rtrim(ltrim(cnt_ucc)) +']'   from tbl_master_contact where cnt_internalid=dpd_cntid) as [ClientName]  ", "  dpd_ClientID='" + CntID + "' AND  dpd_dpcode= '" + DpCode + "' and dpd_cntId !='" + Session["KeyVal_InternalID_New"].ToString() + "' ");
            if (dtCnt.Rows.Count > 0)
            {
                DPID = "";
                for (int i = 0; i < dtCnt.Rows.Count; i++)
                {
                    DPID += "[" + dtCnt.Rows[i]["ClientName"].ToString() + "]";
                }

            }
        }

        protected void DpDetailsGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string tranid = e.Parameters.ToString();
            if (tranid.Length != 0)
            {

                string[] mainid = tranid.Split('~');
                if (mainid[0].ToString() == "Delete")
                {
                    DataTable dtDPS = oDBEngine.GetDataTable("Trans_DematTransactions", " * ", " DematTransactions_CustomerAccountS='" + mainid[1].ToString() + "'");
                    DataTable dtDPT = oDBEngine.GetDataTable("Trans_DematTransactions", " * ", " DematTransactions_CustomerAccountT='" + mainid[1].ToString() + "'");
                    if (dtDPS.Rows.Count > 0 || dtDPT.Rows.Count > 0)
                    {
                        bankid = "N";
                        DpDetailsGrid.DataBind();
                    }
                    else
                    {
                        oDBEngine.DeleteValue("tbl_master_contactDPDetails", "dpd_id ='" + mainid[1].ToString() + "'");
                        DpDetailsGrid.DataBind();
                    }

                }
                else if (mainid[0].ToString() == "GridBind")
                {

                    DpDetailsGrid.DataBind();
                }




            }
            if (e.Parameters == "DeleteCurrentID")
            {

                TextBox DPname = (TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtDPName_hidden");
                oDBEngine.DeleteValue("tbl_master_contactDPDetails", "dpd_id in (select top 1 dpd_id from tbl_master_contactDPDetails where dpd_cntID='" + Session["KeyVal_InternalID"].ToString() + "' order by CreateDate desc)");
                DpDetailsGrid.DataBind();

            }


        }

        protected void DpDetailsGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = bankid;
            e.Properties["cpWidth"] = DPID;

        }



        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_contactdpdetails", " dpd_Status,dpd_StatusChangeDate,dpd_StatusChangeReason ", " dpd_id ='" + data[1].ToString() + "'");
                if (dtAdd.Rows[0]["dpd_Status"].ToString() != "")
                {
                    cmbStatus.SelectedValue = dtAdd.Rows[0]["dpd_Status"].ToString();
                }
                else
                {
                    cmbStatus.SelectedValue = "N";
                }
                if (dtAdd.Rows[0]["dpd_StatusChangeDate"].ToString() != "")
                {
                    StDate.Value = Convert.ToDateTime(dtAdd.Rows[0]["dpd_StatusChangeDate"].ToString());
                }
                else
                {
                    StDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
                }
                txtReason.Text = dtAdd.Rows[0]["dpd_StatusChangeReason"].ToString();

            }
            else if (data[0] == "SaveOld")
            {
                int i = oDBEngine.SetFieldValue("tbl_master_contactdpdetails", " dpd_Status='" + cmbStatus.SelectedItem.Value + "'  ,dpd_StatusChangeDate='" + StDate.Value + "',  dpd_StatusChangeReason='" + txtReason.Text + "'  ", " dpd_id ='" + data[1].ToString() + "'");
                if (i == 1)
                {
                    Stat = "Y";
                }
            }


        }
        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Stat;
        }

        protected void comboPOA_SelectedIndexChanged(object sender, EventArgs e)
        {




            string IsPOA = ((ASPxComboBox)DpDetailsGrid.FindEditFormTemplateControl("comboPOA")).Text;

            ASPxDateEdit TxtFPOADate = ((ASPxDateEdit)DpDetailsGrid.FindEditFormTemplateControl("TxtPOADate"));
            TextBox TxtFPOAName = ((TextBox)DpDetailsGrid.FindEditFormTemplateControl("txtPoaName"));

            Label lblFPOADate = ((Label)DpDetailsGrid.FindEditFormTemplateControl("lblPOADate"));
            Label lblFPOAName = ((Label)DpDetailsGrid.FindEditFormTemplateControl("lblPOAName"));

            if (IsPOA == "Yes")
            //if (IsPOA == "1")
            {

                TxtFPOADate.Visible = true;
                TxtFPOAName.Visible = true;
                lblFPOADate.Visible = true;
                lblFPOAName.Visible = true;

            }
            else
            {

                TxtFPOADate.Visible = false;
                TxtFPOAName.Visible = false;
                lblFPOADate.Visible = false;
                lblFPOAName.Visible = false;

            }

        }

        public string makeShortDate(object oDate)
        {
            if (oDate is DBNull)
            {
                return "";
            }
            else
            {
                DateTime dDate = Convert.ToDateTime(oDate);
                string sDate = dDate.ToShortDateString();
                return sDate;
            }
        }



    }
}