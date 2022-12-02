using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Data;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using DataAccessLayer;
using System.Collections;
using System.Threading.Tasks;

namespace ERP.OMS.Management.Activities
{
    public partial class TDSNilChallan : System.Web.UI.Page
    {
       
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataTable TempTable = new DataTable(); 
         string adjustmentNumber="";
         int adjustmentId=0,ErrorCode=0;
         CommonBL cbl = new CommonBL();
         TdsNillChallanBL blLayer = new TdsNillChallanBL();
         protected void Page_Init(object sender, EventArgs e)
         {            
             //dsTDS.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);             
         }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/CustomerReceiptAdjustmentList.aspx");
                tdsDate.Date = DateTime.Now;
                ////rev srijeeta
                //if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                //{
                //    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                //}
                ////end of rev srijeeta
                
                if (Request.QueryString["Key"] != "Add")
                {
                    string AdjId = Request.QueryString["Key"];
                    EditModeExecute(AdjId);
                    hdAddEdit.Value = "Edit";
                    hdAdjustmentId.Value = AdjId;
                }
                else
                {
                    hdAddEdit.Value = "Add";
                    AddmodeExecuted();
                    Session["AdjustmentDetailTable"] = null;
                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View TDS Nil Challan";
                    btn_SaveRecords.Visible = false;
                    btnSaveRecords.Visible = false;
                }
                 }
            //rev srijeeta
            Task PopulateStockTrialDataTask = new Task(() => GetAllDropDownDetailForBranch());
            PopulateStockTrialDataTask.RunSynchronously();
           //end of rev srijeeta
        }
        //rev srijeeta
        public void GetAllDropDownDetailForBranch()
        {
            DataSet dst = new DataSet();
            dst = AllDropDownDetailForBranch();

            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataSource = dst.Tables[1];
                cmbBranchfilter.DataBind();
                
            }

        }
        public DataSet AllDropDownDetailForBranch()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData");
            proc.AddVarcharPara("@userbranchlist", 5000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            ds = proc.GetDataSet();
            return ds;
        }
        
        //end of rev srijeeta
        
        private void AddmodeExecuted()
        {
            DataSet DataDetails = blLayer.BindLoadData();

            tdsSection.DataSource = DataDetails.Tables[0];
            tdsSection.ValueField = "ID";
            tdsSection.TextField = "Section_Code";
            tdsSection.DataBind();
            
        }


        public void EditModeExecute(string id)
         {
            DataSet EditedDataDetails = blLayer.GetEditedData(id);
           DataRow HeaderRow = EditedDataDetails.Tables[0].Rows[0];

           tdsSection.DataSource = EditedDataDetails.Tables[2];
           tdsSection.ValueField = "ID";
           tdsSection.TextField = "Section_Code";
           tdsSection.DataBind();
           tdsSection.Value=Convert.ToString(HeaderRow["SectionID"]).Trim();
            txtDeductionON.Text = Convert.ToString(HeaderRow["DeductionON"]);
            tdsDate.Date=Convert.ToDateTime(HeaderRow["Payment_Date"]);
            ddlFinYear.Value = Convert.ToString(HeaderRow["FinYear"]);
            ddlQuater.Value = Convert.ToString(HeaderRow["Quater"]);
            ddlEntityType.Value = Convert.ToString(HeaderRow["Type"]);
            txtSurcharge.Text = Convert.ToString(HeaderRow["Surcharge"]);
            txteduCess.Text = Convert.ToString(HeaderRow["eduCess"]);
            txtInterest.Text = Convert.ToString(HeaderRow["Interest"]);
            txtLateFees.Text = Convert.ToString(HeaderRow["LateFees"]);
            txtTotal.Text = Convert.ToString(HeaderRow["Total"]);
            txtTax.Text = Convert.ToString(HeaderRow["Tax"]);
            txtOthers.Text = Convert.ToString(HeaderRow["Others"]);
            txtBankName.Text = Convert.ToString(HeaderRow["BankName"]);
            //rev srijeeta
            cmbBranchfilter.ValueField = Convert.ToString(HeaderRow["branch_id"]);
            //end of rev srijeeta
            txtBankBrach.Text = Convert.ToString(HeaderRow["BankBrach"]);
            
            txtBRS.Text = Convert.ToString(HeaderRow["BRS"]);
            txtChallanNo.Text = Convert.ToString(HeaderRow["ChallanNo"]);
            
            //TDSIDS,
            TdsNillChallanBL tds = new TdsNillChallanBL();
            DataTable DT = new DataTable();
            DT = tds.GetTDSPaymentEdit(Convert.ToDateTime(tdsDate.Date.ToString("yyyy-MM-dd")), Convert.ToString(HeaderRow["Section_Code"]), ddlQuater.Value, ddlFinYear.Value, ddlEntityType.Value);
            if (DT != null && DT.Rows.Count > 0)
            {
                Session["TDSNIllChallanDetails"] = DT;
                DataView dvData = new DataView(DT);
                grid.DataSource = GetTDSGridDetails(dvData.ToTable());
                grid.DataBind();
            }

            DataTable Dttds = new DataTable();
            Dttds = EditedDataDetails.Tables[1];
            foreach (DataRow drr in Dttds.Rows)
            {
                string DET_ID = Convert.ToString(drr["DETID"]);
                //grid.Selection.SetSelectionByKey(int.Parse(DET_ID), true);
                grid.Selection.SetSelectionByKey(DET_ID,true);

                


            }

           
            
        }


        #region Grid Event
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //if (e.Column.FieldName != "AdjAmt")
            //{
            //    e.Editor.Enabled = true;
            //}
            //else
            //{
            //    e.Editor.ReadOnly = false;
            //}
            e.Editor.ReadOnly = true;
        }


        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            string TDSIDS = string.Empty;
            for (int i = 0; i < grid.GetSelectedFieldValues("DETID").Count; i++)
            {
                TDSIDS += "," + Convert.ToString(grid.GetSelectedFieldValues("DETID")[i]);             
            }
            TDSIDS = TDSIDS.TrimStart(',');


            //blLayer.AddEditTDSNillChallanEntry(hdAddEdit.Value, Convert.ToString(tdsSection.Value), txtDeductionON.Text, tdsDate.Date.ToString("yyyy-MM-dd"),
            //    ddlFinYear.Value, ddlQuater.Value, ddlEntityType.Value, txtSurcharge.Text, txteduCess.Text,
            //    txtInterest.Text, txtLateFees.Text, txtTotal.Text, txtTax.Text, txtOthers.Text, txtBankName.Text, txtBankBrach.Text, txtBRS.Text, txtChallanNo.Text, TDSIDS,
            //    Convert.ToString(Session["userid"]), ref adjustmentId,
            //    ref adjustmentNumber, ref ErrorCode, Request.QueryString["Key"]);
            //rev srijeeta
            blLayer.AddEditTDSNillChallanEntry(hdAddEdit.Value, Convert.ToString(tdsSection.Value), txtDeductionON.Text, tdsDate.Date.ToString("yyyy-MM-dd"),
               ddlFinYear.Value, ddlQuater.Value, ddlEntityType.Value, txtSurcharge.Text, txteduCess.Text,
               txtInterest.Text, txtLateFees.Text, txtTotal.Text, txtTax.Text, txtOthers.Text, txtBankName.Text, txtBankBrach.Text, txtBRS.Text, txtChallanNo.Text, TDSIDS,
               Convert.ToString(Session["userid"]), ref adjustmentId,
               ref adjustmentNumber, ref ErrorCode, Request.QueryString["Key"], Convert.ToString(cmbBranchfilter.Value));
            // end of rev srijeeta
           
            if (adjustmentId > 0)
            {
               // grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
                grid.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
                grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
                e.Handled = true;
            }
            
        }

        private void RefetchSrlNo()
        {
            TempTable.Columns.Add("SrlNo", typeof(string));
            int conut = 1;
            foreach (DataRow dr in TempTable.Rows)
            {
                dr["SrlNo"] = conut;
                conut++;
            }
        }

        public DataTable GetAddLockStatus()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustmentDrNote_details");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForAdvInv");

            dt = proc.GetTable();
            return dt;

        }

        protected void grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            //grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
            //grid.JSProperties["cpErrorCode"] =  Convert.ToString(ErrorCode);
            //grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string command = e.Parameters.Split('~')[0];
            

            if (command == "TDSPayment")
            {
                DateTime TDSPaymentDate = Convert.ToDateTime(tdsDate.Date.ToString("yyyy-MM-dd"));
                //DateTime TDSPaymentDate=Convert.ToDateTime(e.Parameters.Split('~')[1]);
                string TDSCode = e.Parameters.Split('~')[2];
                string TDSQuater = e.Parameters.Split('~')[3];
                string TDSYear = e.Parameters.Split('~')[4];
                string Type = e.Parameters.Split('~')[5];

                //TDSPaymentBL tds = new TDSPaymentBL();
                DataTable DT = new DataTable();
                DT = blLayer.GetTDSPayment(TDSPaymentDate, TDSCode, TDSQuater, TDSYear, Type);
                if (DT != null && DT.Rows.Count > 0)
                {
                    Session["TDSNIllChallanDetails"] = DT;
                    DataView dvData = new DataView(DT);
                    grid.DataSource = GetTDSGridDetails(dvData.ToTable());
                    grid.DataBind();
                }
                else
                {
                    Session["TDSNIllChallanDetails"] = null;
                    grid.DataSource = null;
                    grid.DataBind();
                    grid.JSProperties["cpBlankGrid"] = 1;
                }
                //grid.JSProperties["cpErrorCode"] = 1;
            }
            else if (command == "SaveTDS")
            {
                string TDSIDS = string.Empty;
                for (int i = 0; i < grid.GetSelectedFieldValues("DETID").Count; i++)
                {
                    TDSIDS += "," + Convert.ToString(grid.GetSelectedFieldValues("DETID")[i]);
                }
                TDSIDS = TDSIDS.TrimStart(',');


                //blLayer.AddEditTDSNillChallanEntry(hdAddEdit.Value, Convert.ToString(tdsSection.Value), txtDeductionON.Text, tdsDate.Date.ToString("yyyy-MM-dd"),
                //    ddlFinYear.Value, ddlQuater.Value, ddlEntityType.Value, txtSurcharge.Text, txteduCess.Text,
                //    txtInterest.Text, txtLateFees.Text, txtTotal.Text, txtTax.Text, txtOthers.Text, txtBankName.Text, txtBankBrach.Text, txtBRS.Text, txtChallanNo.Text, TDSIDS,
                //    Convert.ToString(Session["userid"]), ref adjustmentId,
                //    ref adjustmentNumber, ref ErrorCode, Request.QueryString["Key"]); 
                //rev srijeeta
                blLayer.AddEditTDSNillChallanEntry(hdAddEdit.Value, Convert.ToString(tdsSection.Value), txtDeductionON.Text, tdsDate.Date.ToString("yyyy-MM-dd"),
                    ddlFinYear.Value, ddlQuater.Value, ddlEntityType.Value, txtSurcharge.Text, txteduCess.Text,
                    txtInterest.Text, txtLateFees.Text, txtTotal.Text, txtTax.Text, txtOthers.Text, txtBankName.Text, txtBankBrach.Text, txtBRS.Text, txtChallanNo.Text, TDSIDS,
                    Convert.ToString(Session["userid"]), ref adjustmentId,
                    ref adjustmentNumber, ref ErrorCode, Request.QueryString["Key"], Convert.ToString(cmbBranchfilter.Value)); 
                //end of rev srijeeta
                

                if (adjustmentId > 0)
                {
                    grid.JSProperties["cpChallanNumber"] = adjustmentNumber;
                    grid.JSProperties["cpErrorCode"] = Convert.ToString(ErrorCode);
                    grid.JSProperties["cpTDSId"] = Convert.ToString(adjustmentId);
                    Session["TDSNIllChallanDetails"] = null;
                    //e.Handled = true;
                }
                else if (adjustmentId == -9)
                {
                    DataTable dts = new DataTable();
                    dts = GetAddLockStatus();
                    grid.JSProperties["cpErrorCode"] = "AddLock";
                    grid.JSProperties["cpAddLockStatus"] = (Convert.ToString(dts.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dts.Rows[0]["Lock_Todate"]));
                }
            }
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["TDSNIllChallanDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["TDSNIllChallanDetails"];
                DataView dvData = new DataView(Quotationdt);               
                grid.DataSource = GetTDSGridDetails(dvData.ToTable());
            }
            
           
        }

        public IEnumerable GetTDSGridDetails(DataTable TDSGridDetailsDt)
        {
            List<TDSGridDetails> TDSGridDetailsList = new List<TDSGridDetails>();

            if (TDSGridDetailsDt != null && TDSGridDetailsDt.Rows.Count > 0)
            {
                for (int i = 0; i < TDSGridDetailsDt.Rows.Count; i++)
                {
                    TDSGridDetails TDSGridDetails = new TDSGridDetails();

                    TDSGridDetails.DETID = Convert.ToString(TDSGridDetailsDt.Rows[i]["DETID"]);
                    TDSGridDetails.Document_No = Convert.ToString(TDSGridDetailsDt.Rows[i]["Document_No"]);
                    TDSGridDetails.PartyID = Convert.ToString(TDSGridDetailsDt.Rows[i]["PartyID"]);
                    TDSGridDetails.TDSTCS_Code = Convert.ToString(TDSGridDetailsDt.Rows[i]["TDSTCS_Code"]);
                    TDSGridDetails.PaymentDate = Convert.ToString(TDSGridDetailsDt.Rows[i]["PaymentDate"]);
                    TDSGridDetails.Total_Tax = Convert.ToString(TDSGridDetailsDt.Rows[i]["Total_Tax"]);
                    TDSGridDetails.Tax_Amount = Convert.ToString(TDSGridDetailsDt.Rows[i]["Tax_Amount"]);
                    TDSGridDetails.Surcharge = Convert.ToString(TDSGridDetailsDt.Rows[i]["Surcharge"]);
                    TDSGridDetails.EduCess = Convert.ToString(TDSGridDetailsDt.Rows[i]["EduCess"]);
                    TDSGridDetails.IsOpening = Convert.ToString(TDSGridDetailsDt.Rows[i]["IsOpening"]);

                    TDSGridDetailsList.Add(TDSGridDetails);
                }
            }

            return TDSGridDetailsList;
        }
        public class TDSGridDetails
        {
            public string DETID { get; set; }
            public string Document_No { get; set; }
            public string PartyID { get; set; }
            public string TDSTCS_Code { get; set; }
            public string PaymentDate { get; set; }
            public string Total_Tax { get; set; }
            public string Tax_Amount { get; set; }
            public string Surcharge { get; set; }
            public string EduCess { get; set; }
            public string IsOpening { get; set; } 
        }
        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

       
    
    }
}