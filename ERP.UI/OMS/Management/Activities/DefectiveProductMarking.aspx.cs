using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EntityLayer.CommonELS;
using System.IO;
using DevExpress.Web;
using BusinessLogicLayer;
using DataAccessLayer;

namespace ERP.OMS.Management.Activities
{
    public partial class DefectiveProductMarking : ERP.OMS.ViewState_class.VSPage
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Init(object sender, EventArgs e)
        {
            ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    

                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/DefectiveProductMarking.aspx");
                    
                    Session["exportval"] = null;
                    GetBranchDetails();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }

        #region Datatbase Section

        public void GetBranchDetails()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string userbranchID = Convert.ToString(Session["userbranchID"]);

            DataTable dt = GetBranchDetails(userbranchHierarchy);
            if (dt != null && dt.Rows.Count > 0)
            {
                cmbbranch.DataSource = dt;
                cmbbranch.DataBind();

                if (Session["userbranchID"] != null)
                {
                    cmbbranch.Value = userbranchID;
                }
            }
        }
        public DataTable GetBranchDetails(string userbranchHierarchy)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
                proc.AddVarcharPara("@Action", 3000, "GetBranch");
                proc.AddVarcharPara("@BranchList", 3000, userbranchHierarchy);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetStockDetails()
        {
            string strBranchID = Convert.ToString(cmbbranch.Value);
            string strProductID = Convert.ToString(productLookUp.Value);
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strFinYear = Convert.ToString(Session["LastFinYear"]);

            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
                proc.AddVarcharPara("@Action", 3000, "GetStockSerialDetails");
                proc.AddVarcharPara("@BranchID", 100, strBranchID);
                proc.AddVarcharPara("@ProductID", 100, strProductID);
                proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
                proc.AddVarcharPara("@FinYear", 100, strFinYear);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetOpeningStockDetailsForExport()
        {
            string strBranchID = Convert.ToString(cmbbranch.Value);
            string strProductID = Convert.ToString(productLookUp.Value);
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strFinYear = Convert.ToString(Session["LastFinYear"]);

            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
                proc.AddVarcharPara("@Action", 3000, "GetStockSerialDetails_Export");
                proc.AddVarcharPara("@BranchID", 100, strBranchID);
                proc.AddVarcharPara("@ProductID", 100, strProductID);
                proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
                proc.AddVarcharPara("@FinYear", 100, strFinYear);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void ModifyProductMarking(string strProductID,string strDentProduct,string strDisplayProduct,string strStolenProduct, ref int IsComplete)
        {
            try
            {

                DataSet dsInst = new DataSet();


                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                SqlCommand cmd = new SqlCommand("proc_ProductMarking_Modify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", strProductID);
                cmd.Parameters.AddWithValue("@DentProduct", strDentProduct);
                cmd.Parameters.AddWithValue("@DisplayProduct", strDisplayProduct);
                cmd.Parameters.AddWithValue("@StolenProduct", strStolenProduct);

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                IsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Grid Section

        protected void OpeningGrid_DataBinding(object sender, EventArgs e)
        {
            OpeningGrid.DataSource = GetStockDetails();
        }
        protected void OpeningGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];

            if (strSplitCommand == "BindGrid")
            {
                OpeningGrid.DataSource = GetStockDetails();
                OpeningGrid.DataBind();
            }
            else if (strSplitCommand == "SaveBindGrid")
            {
                string strProductID= Convert.ToString(productLookUp.Value);
                string strDentProduct = Convert.ToString(hdnDentProduct.Value);
                string strDisplayProduct = Convert.ToString(hdnDisplayProduct.Value);
                string strStolenProduct = Convert.ToString(hdnStolenProduct.Value);

                int IsComplete=0;
                ModifyProductMarking(strProductID, strDentProduct, strDisplayProduct, strStolenProduct, ref IsComplete);

                if(IsComplete==1)
                {
                    OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "successInsert";
                }
                else
                {
                    OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                }

                OpeningGrid.DataSource = GetStockDetails();
                OpeningGrid.DataBind();
            }
        }
        public bool GetChecked(string Access)
        {
            switch (Access)
            {
                case "Y":
                    return true;

                default:
                    return false;
            }
        }
        protected void openingGridExport_DataBinding(object sender, EventArgs e)
        {
            openingGridExport.DataSource = GetOpeningStockDetailsForExport();
        }   

        #endregion

        #region Checkbox Section

        protected void chkDent_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ OnDentSelectedChanged(s, e, {0}) }}", itemindex);
        }
        protected void chkDisplay_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ OnDisplaySelectedChanged(s, e, {0}) }}", itemindex);
        }
        protected void chkStolen_Init(object sender, EventArgs e)
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ OnStolenSelectedChanged(s, e, {0}) }}", itemindex);
        }

        #endregion

        #region Export Event

        public void bindexport(int Filter)
        {
            string filename = "Product Marking";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Product Marking";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            exporter.GridViewID = "openingGridExport";
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }

        #endregion
    }
}