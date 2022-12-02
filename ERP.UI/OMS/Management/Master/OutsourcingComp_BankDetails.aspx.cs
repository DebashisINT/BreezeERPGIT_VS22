using System;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_OutsourcingComp_BankDetails : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //BankDetails.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    BankDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //BankDetails.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    BankDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!Page.IsPostBack)
            {
                string[,] EmployeeNameID = objEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }
            }
        }

        protected void BankDetailsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            TextBox bankname = (TextBox)BankDetailsGrid.FindEditFormTemplateControl("txtbankname");
            bankname.Attributes.Add("onkeyup", "CallList(this,'bankdetails',event)");
        }
        protected void BankDetailsGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string[] BankDetail = e.NewValues["BankName1"].ToString().Split('~');
            string condition = "";
            if (BankDetail[0].ToString() != "")
            {
                if (BankDetail[3].ToString() == "0")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_bankname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_branchname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                }
                if (BankDetail[3].ToString() == "1")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_micrno='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_branchname='" + BankDetail[2].ToString() + "'";
                }
                if (BankDetail[3].ToString() == "2")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_branchname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                }
                if (condition != "")
                {
                    string[,] DT = objEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
                    if (DT[0, 0] != "n")
                    {
                        e.NewValues["BankName1"] = DT[0, 0].ToString();
                    }
                    else
                    {
                        lblmessage.Text = "Bank Name is not available in the database!";
                        return;
                    }

                }
                else
                {
                    lblmessage.Text = "Bank Name is not available in the database!";
                    BankDetailsGrid.CancelEdit();
                }
            }
            else
            {
                lblmessage.Text = "Please enter a valid Bank Name!";
                BankDetailsGrid.CancelEdit();
            }
        }
        protected void BankDetailsGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["Category"] == null)
            {
                e.RowError = "Please select Category.";
                return;
            }
            if (e.NewValues["AccountType"] == null)
            {
                e.RowError = "Please select Account Type.";
                return;
            }
            if (e.NewValues["BankName1"].ToString() == "")
            {
                e.RowError = "Please select Bank Name.";
                return;
            }
            if (BankDetailsGrid.IsNewRowEditing)
            {
                string Category = e.NewValues["Category"].ToString();
                string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_accountCategory", " cbd_cntId='" + Session["KeyVal_InternalID"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);
                if (Category1[0, 0] != "n")
                {
                    if (Category1[0, 0] == "Default")
                    {
                        e.RowError = "Default Category Already Exists!";
                        return;
                    }
                }
            }
            else
            {
                string KeyVal = e.Keys["Id"].ToString();
                string Category = e.NewValues["Category"].ToString();
                string[,] Category1 = objEngine.GetFieldValue("tbl_trans_contactBankDetails", "cbd_id", " cbd_cntId='" + Session["KeyVal_InternalID"].ToString() + "' and cbd_accountCategory='" + Category + "'", 1);
                if (Category1[0, 0] != "n")
                {
                    if (KeyVal != Category1[0, 0])
                    {
                        e.RowError = "Default Category Already Exists!";
                        return;
                    }
                }
            }
        }
        protected void BankDetailsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] BankDetail = e.NewValues["BankName1"].ToString().Split('~');
            string condition = "";
            if (BankDetail[0].ToString() != "")
            {
                if (BankDetail[3].ToString() == "0")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_bankname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_branchname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                }
                if (BankDetail[3].ToString() == "1")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_micrno='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_branchname='" + BankDetail[2].ToString() + "'";
                }
                if (BankDetail[3].ToString() == "2")
                {
                    if (BankDetail[0] != "")
                        condition = " bnk_branchname='" + BankDetail[0].ToString() + "' and ";
                    if (BankDetail[1] != "")
                        condition += " bnk_bankname='" + BankDetail[1].ToString() + "' and ";
                    if (BankDetail[2] != "")
                        condition += " bnk_micrno='" + BankDetail[2].ToString() + "'";
                }
                if (condition != "")
                {
                    string[,] DT = objEngine.GetFieldValue(" tbl_master_bank", " bnk_id", condition, 1);
                    if (DT[0, 0] != "n")
                    {
                        e.NewValues["BankName1"] = DT[0, 0].ToString();
                    }
                    else
                    {

                        BankDetailsGrid.CancelEdit();
                    }

                }
                else
                {

                    BankDetailsGrid.CancelEdit();
                }
            }
            else
            {

                BankDetailsGrid.CancelEdit();
            }
        }

    }
}