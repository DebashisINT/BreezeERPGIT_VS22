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

namespace ERP.OMS.Management.Activities
{
    public partial class ServiceTempleateAdd : System.Web.UI.Page
    {
        ServiceTemplate blLayer = new ServiceTemplate();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();
        DataTable TempTable = new DataTable(); 
         string adjustmentNumber="";
         int adjustmentId=0,ErrorCode=0;
        protected void Page_Load(object sender, EventArgs e)
        {
             
            if (!IsPostBack) 
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/ServiceTempleateList.aspx");
               
                if (Request.QueryString["Key"] != "Add")
                {
                    string AdjId = Request.QueryString["Key"];
                    EditModeExecute(AdjId);
                    hdAddEdit.Value = "Edit";
                    hdAdjustmentId.Value = AdjId;
                    lblHeading.Text = "Edit Service Template";

                   // btnSaveRecords.Visible = false;
                   // btnSaveRecords.ClientEnabled = false;
                }
                else
                {
                    hdAddEdit.Value = "Add";
                    AddmodeExecuted();
                    Session["ServiceDetailTable"] = null;
                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Service Template";
                    btn_SaveRecords.Visible = false;
                    btnSaveRecords.Visible = false;
                }  
            } 

        }

        private void AddmodeExecuted()
        {
            DataSet allDetails = blLayer.PopulateServiceTemplateDetails();
            
            ddlBranch.DataSource = allDetails.Tables[0];
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataTextField = "branch_description";
            ddlBranch.DataBind();

        }


        public void EditModeExecute(string id)
        {
            DataSet EditedDataDetails = blLayer.GetEditedData(id);


            ddlBranch.DataSource = EditedDataDetails.Tables[0];
            ddlBranch.DataValueField = "branch_id";
            ddlBranch.DataTextField = "branch_description";
            ddlBranch.DataBind();

            

            DataRow HeaderRow = EditedDataDetails.Tables[2].Rows[0];

            txtServiceDescription.Text = Convert.ToString(HeaderRow["Service_Description"]);
            hdnServiceProductId.Value = Convert.ToString(HeaderRow["Service_ProductID"]);
            txtServiceItemName.Value = Convert.ToString(HeaderRow["ProductName"]);
            ddlBranch.SelectedValue = Convert.ToString(HeaderRow["Service_Unit"]);
            txtQuantity.Text = Convert.ToString(HeaderRow["Service_Qty"]);
            txtNotes.Text = Convert.ToString(HeaderRow["Service_Remarks"]);
            //ddlBranch.Enabled = false;

            TempTable = EditedDataDetails.Tables[3];
            HiddenRowCount.Value = TempTable.Rows.Count.ToString();
            Session["ServiceDetailTable"] = EditedDataDetails.Tables[3];
            grid.DataBind();
        }


        #region Grid Event
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Quantity")
            {
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "UOM")
            {
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "Rate")
            {
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "Remarks")
            {
                e.Editor.ReadOnly = false;
            }
            else
            {
                e.Editor.Enabled = true;
            }
            
            
            
        }


        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable AdjustmentTable = new DataTable();            
            AdjustmentTable.Columns.Add("SrlNo", typeof(string));
            AdjustmentTable.Columns.Add("ProductName", typeof(string));
            AdjustmentTable.Columns.Add("Discription", typeof(string));
            AdjustmentTable.Columns.Add("Quantity", typeof(decimal));
            AdjustmentTable.Columns.Add("UOM", typeof(string));
            AdjustmentTable.Columns.Add("Rate", typeof(decimal));
            AdjustmentTable.Columns.Add("Value", typeof(decimal));
            AdjustmentTable.Columns.Add("Remarks", typeof(string));
            AdjustmentTable.Columns.Add("ProductID", typeof(string));
            AdjustmentTable.Columns.Add("ActualSL", typeof(string));
            
            DataRow NewRow;
            foreach (var args in e.InsertValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["ProductName"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                    NewRow["ProductName"] = Convert.ToString(args.NewValues["ProductName"]);
                    NewRow["Discription"] = Convert.ToString(args.NewValues["Discription"]);
                    NewRow["Quantity"] = Convert.ToDecimal(args.NewValues["Quantity"]);
                    NewRow["UOM"] = Convert.ToString(args.NewValues["UOM"]);
                    NewRow["Rate"] = Convert.ToDecimal(args.NewValues["Rate"]);
                    NewRow["Value"] = Convert.ToDecimal(args.NewValues["Value"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["ProductID"] = Convert.ToString(args.NewValues["ProductID"]);                  
                    NewRow["ActualSL"] = Convert.ToString(args.NewValues["ActualSL"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["ProductName"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                    NewRow["ProductName"] = Convert.ToString(args.NewValues["ProductName"]);
                    NewRow["Discription"] = Convert.ToString(args.NewValues["Discription"]);
                    NewRow["Quantity"] = Convert.ToDecimal(args.NewValues["Quantity"]);
                    NewRow["UOM"] = Convert.ToString(args.NewValues["UOM"]);
                    NewRow["Rate"] = Convert.ToDecimal(args.NewValues["Rate"]);
                    NewRow["Value"] = Convert.ToDecimal(args.NewValues["Value"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["ProductID"] = Convert.ToString(args.NewValues["ProductID"]);
                    NewRow["ActualSL"] = Convert.ToString(args.NewValues["ActualSL"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }


           
            foreach (var args in e.DeleteValues)
            {
                DataTable AdjTable = (DataTable)Session["ServiceDetailTable"];
                if (AdjTable != null)
                {
                    string delId = Convert.ToString(args.Keys[0]);
                    DataRow[] AdjTableRow = AdjustmentTable.Select("ActualSL='" + delId + "'");
                    //DataRow[] delRow = AdjustmentTable.Select("DocumentId='" + AdjTableRow[0]["DocumentId"] + "' and DocumentType='" + AdjTableRow[0]["DocumentType"] + "'");
                    foreach (DataRow dr in AdjTableRow)
                        dr.Delete();

                    AdjustmentTable.AcceptChanges();
                }
            }
            
            TempTable = AdjustmentTable.Copy();
            AdjustmentTable.Columns.Remove("ProductName");
            AdjustmentTable.Columns.Remove("Discription");
            AdjustmentTable.Columns.Remove("UOM");
            AdjustmentTable.Columns.Remove("ActualSL");
            AdjustmentTable.AcceptChanges();
          //  RefetchSrlNo();

            foreach (DataRow d in AdjustmentTable.Rows)
            {
                string ProductDetails = Convert.ToString(d["ProductID"]);
                string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                string ProductID = ProductDetailsList[0];
                d["ProductID"] = ProductID;
            }
            AdjustmentTable.AcceptChanges();
            string ServiceProductID = "0";

            if(hdnServiceProductId.Value!="")
            {
                string ServiceProductDetails = Convert.ToString(hdnServiceProductId.Value);
                string[] ServiceProductDetailsList = ServiceProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                 ServiceProductID = ServiceProductDetailsList[0];
            }

            if(hdAddEdit.Value=="Edit")
            {
                //AdjustmentTable.Columns.Remove("ServiceTempDetails_ID");
                //AdjustmentTable.AcceptChanges();
                RefetchSrlNo();
            }
           

            blLayer.AddEditServiceTemplate(hdAddEdit.Value, txtServiceDescription.Text, ServiceProductID, txtQuantity.Text
                ,ddlBranch.SelectedValue, txtNotes.Text, Convert.ToString(Session["userid"]), ref adjustmentId
               , AdjustmentTable, ref ErrorCode, Request.QueryString["Key"]);

           // grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
            grid.JSProperties["cpErrorCode"] =  Convert.ToString(ErrorCode);
            grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
            e.Handled = true;

            #region To Show By Default Cursor after SAVE AND NEW
            //if (hdAddEdit.Value == "Add")
            //{
            //    if (HiddenSaveButton.Value != "E")
            //    {
            //        string schemavalue = CmbScheme.Value.ToString();
            //        string NumberingScheme = CmbScheme.Text;
            //        string BranchID = ddlBranch.SelectedValue;
            //        string BranchName = ddlBranch.SelectedItem.Text;
            //        string strDate = dtTDate.Date.ToString("yyyy-MM-dd");
            //        List<string> AfterAdd = new List<string> { schemavalue, NumberingScheme, BranchID, BranchName, strDate };

            //        Session["schemavalJourCreditorAdj"] = AfterAdd;

            //        string schematype = txtVoucherNo.Text;
            //        if (schematype == "Auto")
            //        {
            //            Session["SaveModeJourCreditorsAdj"] = "A";
            //        }
            //        else
            //        {
            //            Session["SaveModeJourCreditorsAdj"] = "M";
            //        }
            //    }

            //}


            #endregion
        }

        private void RefetchSrlNo()
        {
            //TempTable.Columns.Add("SrlNo", typeof(string));
            int conut = 1;
            foreach (DataRow dr in TempTable.Rows)
            {
                dr["SrlNo"] = conut;
                conut++;
            }
        }

      

        protected void grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            grid.JSProperties["cpadjustmentNumber"] = adjustmentNumber;
            grid.JSProperties["cpErrorCode"] =  Convert.ToString(ErrorCode);
            grid.JSProperties["cpadjustmentId"] = Convert.ToString(adjustmentId);
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        { 
        
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            grid.DataSource = TempTable;
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