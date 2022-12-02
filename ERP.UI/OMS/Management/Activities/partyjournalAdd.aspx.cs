using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class partyjournalAdd : System.Web.UI.Page
    {
        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["VendorGrid"] = null;
                Session["CustomerGrid"] = null;
                SetNumberingScheme();

                if (Request.QueryString["IsView"] == "Y")
                {
                    hdnKey.Value = Request.QueryString["key"];
                    SetEditData(Request.QueryString["key"]); 
                }
                
            }

        }

        void SetEditData(string key)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@BRANCH_HIERARCHY", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Action", 500, "GetEditData");
            proc.AddVarcharPara("@key", 500, key);
            ds = proc.GetDataSet();

            DataTable dt = ds.Tables[0];

            txtVoucherNo.Text = Convert.ToString(dt.Rows[0]["DOCUMENT_NUMBER"]);
            txtVoucherNo.ClientEnabled = false;

            dtTDate.Date = Convert.ToDateTime(dt.Rows[0]["DOCUMENT_DATE"]);
            dtTDate.ClientEnabled = false;


            DataTable Customerdt = ds.Tables[1];
            DataTable Vendordt = ds.Tables[2];
            Session["CustomerGrid"] = Customerdt;
            Session["VendorGrid"] = Vendordt;


            divNumbering.Style.Add("display", "none");
            divSave.Style.Add("display", "none");


            grid.DataBind();
            vendorgrid.DataBind();


        }

        void SetNumberingScheme()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@BRANCH_HIERARCHY", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Action", 500, "GETHeaderNumbering");
            dt = proc.GetTable();
            cmbBranchfilter.ValueField = "Id";
            cmbBranchfilter.TextField = "SchemaName";
            cmbBranchfilter.DataSource = dt;
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

        }

        #endregion

        #region Grid Event
        protected void grid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Unit" || e.Column.FieldName == "DocumentNumbering" || e.Column.FieldName == "Customer" || e.Column.FieldName == "Project" || e.Column.FieldName == "RefDoc")
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "AdjAmount" || e.Column.FieldName == "Remarks")
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "SrlNo" || e.Column.FieldName == "Amount" || e.Column.FieldName == "BalAmount")
            {

                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = false;
            }
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpSuccess"] = "1";
            grid.JSProperties["cpMsg"] = null;

            DataTable AdjustmentTable = new DataTable();


            if (Session["CustomerGrid"] != null)
            {
                AdjustmentTable = (DataTable)Session["CustomerGrid"];
            }
            else
            {
                AdjustmentTable.Columns.Add("SrlNo", typeof(string));
                AdjustmentTable.Columns.Add("Unit", typeof(string));
                AdjustmentTable.Columns.Add("DocumentNumbering", typeof(string));
                AdjustmentTable.Columns.Add("Customer", typeof(string));
                AdjustmentTable.Columns.Add("Project", typeof(string));
                AdjustmentTable.Columns.Add("RefDoc", typeof(string));
                AdjustmentTable.Columns.Add("Amount", typeof(string));
                AdjustmentTable.Columns.Add("BalAmount", typeof(string));
                AdjustmentTable.Columns.Add("AdjAmount", typeof(string));
                AdjustmentTable.Columns.Add("Remarks", typeof(string));
                AdjustmentTable.Columns.Add("UnitID", typeof(string));
                AdjustmentTable.Columns.Add("ProjectId", typeof(string));
                AdjustmentTable.Columns.Add("SchemaID", typeof(string));
                AdjustmentTable.Columns.Add("CustomerID", typeof(string));
                AdjustmentTable.Columns.Add("DocId", typeof(string));
                AdjustmentTable.Columns.Add("UpdateEdit", typeof(string));
            }



            DataRow NewRow;
            foreach (var args in e.InsertValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["Unit"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                    NewRow["Unit"] = Convert.ToString(args.NewValues["Unit"]);
                    NewRow["DocumentNumbering"] = Convert.ToString(args.NewValues["DocumentNumbering"]);
                    NewRow["Customer"] = Convert.ToString(args.NewValues["Customer"]);
                    NewRow["Project"] = Convert.ToString(args.NewValues["Project"]);
                    NewRow["RefDoc"] = Convert.ToString(args.NewValues["RefDoc"]);
                    NewRow["Amount"] = Convert.ToString(args.NewValues["Amount"]);
                    NewRow["BalAmount"] = Convert.ToString(args.NewValues["BalAmount"]);
                    NewRow["AdjAmount"] = Convert.ToString(args.NewValues["AdjAmount"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["UnitID"] = Convert.ToInt64(args.NewValues["UnitID"]);
                    NewRow["ProjectId"] = Convert.ToString(args.NewValues["ProjectId"]);
                    NewRow["SchemaID"] = Convert.ToString(args.NewValues["SchemaID"]);
                    NewRow["CustomerID"] = Convert.ToString(args.NewValues["CustomerID"]);
                    NewRow["DocId"] = Convert.ToString(args.NewValues["DocId"]);
                    NewRow["UpdateEdit"] = Convert.ToString(args.NewValues["UpdateEdit"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["Unit"])))
                {
                    // NewRow = AdjustmentTable.NewRow();
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string Unit = Convert.ToString(args.NewValues["Unit"]);
                    string DocumentNumbering = Convert.ToString(args.NewValues["DocumentNumbering"]);
                    string Customer = Convert.ToString(args.NewValues["Customer"]);
                    string Project = Convert.ToString(args.NewValues["Project"]);
                    string RefDoc = Convert.ToString(args.NewValues["RefDoc"]);
                    string Amount = Convert.ToString(args.NewValues["Amount"]);
                    string BalAmount = Convert.ToString(args.NewValues["BalAmount"]);
                    string AdjAmount = Convert.ToString(args.NewValues["AdjAmount"]);
                    string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                    string UnitID = Convert.ToString(args.NewValues["UnitID"]);
                    string ProjectId = Convert.ToString(args.NewValues["ProjectId"]);
                    string SchemaID = Convert.ToString(args.NewValues["SchemaID"]);
                    string CustomerID = Convert.ToString(args.NewValues["CustomerID"]);
                    string DocId = Convert.ToString(args.NewValues["DocId"]);
                    string UpdateEdit = Convert.ToString(args.NewValues["UpdateEdit"]);
                    // AdjustmentTable.Rows.Add(NewRow);

                    //string SrlNo = Convert.ToString(args.Keys["SrlNo"]);

                    bool isDeleted = false;
                    foreach (var arg in e.DeleteValues)
                    {
                        string DeleteID = Convert.ToString(arg.Keys["SrlNo"]);
                        if (DeleteID == SrlNo)
                        {
                            isDeleted = true;
                            break;
                        }
                    }

                    if (isDeleted == false)
                    {
                        bool Isexists = false;
                        foreach (DataRow drr in AdjustmentTable.Rows)
                        {
                            string OldSrlNo = Convert.ToString(drr["SrlNo"]);

                            if (OldSrlNo == SrlNo)
                            {
                                Isexists = true;

                                drr["SrlNo"] = SrlNo;
                                drr["Unit"] = Unit;
                                drr["DocumentNumbering"] = DocumentNumbering;
                                drr["Customer"] = Customer;
                                drr["Project"] = Project;
                                drr["RefDoc"] = RefDoc;
                                drr["Amount"] = Amount;
                                drr["BalAmount"] = BalAmount;
                                drr["AdjAmount"] = AdjAmount;
                                drr["Remarks"] = Remarks;
                                drr["UnitID"] = UnitID;
                                drr["ProjectId"] = ProjectId;
                                drr["SchemaID"] = SchemaID;
                                drr["CustomerID"] = CustomerID;
                                drr["DocId"] = DocId;
                                drr["UpdateEdit"] = UpdateEdit;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            AdjustmentTable.Rows.Add(SrlNo, Unit, DocumentNumbering, Customer, Project, RefDoc, Amount, BalAmount, AdjAmount, Remarks, UnitID, ProjectId, SchemaID, CustomerID, DocId, UpdateEdit);

                        }
                    }
                }
            }


            foreach (var args in e.DeleteValues)
            {
                string SrlNoID = Convert.ToString(args.Keys["SrlNo"]);
                string SrlNo = "";

                for (int i = AdjustmentTable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = AdjustmentTable.Rows[i];
                    string delSrlNo = Convert.ToString(dr["SrlNo"]);

                    if (delSrlNo == SrlNo)
                    {
                        SrlNo = Convert.ToString(dr["SrlNo"]);
                        dr.Delete();
                    }
                }
                AdjustmentTable.AcceptChanges();

            }




            DataTable BatchGridData = AdjustmentTable.Copy();


            var query = from row in BatchGridData.AsEnumerable()
                        group row by row.Field<string>("RefDoc") into sales
                        select new
                        {
                            Doc_No = sales.Key,
                            Count = sales.Count()
                        };

            Session["CustomerGrid"] = BatchGridData;
            foreach (var obj in query)
            {
                if (obj.Count > 1 && !string.IsNullOrEmpty(obj.Doc_No))
                {
                    grid.JSProperties["cpSuccess"] = "0";
                    grid.JSProperties["cpMsg"] = "Can not select duplecate document number(Customer) . Document Number : " + obj.Doc_No;
                    grid.DataBind();
                }
            }


            foreach (DataRow item in BatchGridData.Rows)
            {
                if (Convert.ToString(item["UnitID"]) == "")
                {
                    grid.JSProperties["cpSuccess"] = "0";
                    grid.JSProperties["cpMsg"] = "Unit can not be blank (Customer) . Line Number : " + Convert.ToString(item["SrlNo"]);
                    grid.DataBind();
                }
                else if (Convert.ToString(item["CustomerID"]) == "")
                {
                    grid.JSProperties["cpSuccess"] = "0";
                    grid.JSProperties["cpMsg"] = "Customer can not be blank (Customer) . Line Number : " + Convert.ToString(item["SrlNo"]);
                    grid.DataBind();
                }
                else if (Convert.ToString(item["AdjAmount"]) == "0.00")
                {
                    grid.JSProperties["cpSuccess"] = "0";
                    grid.JSProperties["cpMsg"] = "Adj. Amount can not be zero (Customer) . Line Number : " + Convert.ToString(item["SrlNo"]);
                    //  grid.DataBind();
                }
                else if (Convert.ToString(item["SchemaID"]) == "")
                {
                    grid.JSProperties["cpSuccess"] = "0";
                    grid.JSProperties["cpMsg"] = "Journal Numbering Schema can not be blank (Customer) . Line Number : " + Convert.ToString(item["SrlNo"]);
                    //  grid.DataBind();
                }
            }





        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            grid.DataSource = (DataTable)Session["CustomerGrid"];
        }

        protected void vendorgrid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Unit" || e.Column.FieldName == "DocumentNumbering" || e.Column.FieldName == "Vendor" || e.Column.FieldName == "Project" || e.Column.FieldName == "RefDoc")
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "AdjAmount" || e.Column.FieldName == "Remarks")
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = false;
            }
            else if (e.Column.FieldName == "SrlNo" || e.Column.FieldName == "Amount" || e.Column.FieldName == "BalAmount")
            {

                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ClientEnabled = true;
                e.Editor.ReadOnly = false;
            }
        }

        protected void vendorgrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void vendorgrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void vendorgrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void vendorgrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void vendorgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            bool valid = true;
            DataTable AdjustmentTable = new DataTable();
            if (Session["VendorGrid"] != null)
            {
                AdjustmentTable = (DataTable)Session["VendorGrid"];
            }
            else
            {
                AdjustmentTable.Columns.Add("SrlNo", typeof(string));
                AdjustmentTable.Columns.Add("Unit", typeof(string));
                AdjustmentTable.Columns.Add("DocumentNumbering", typeof(string));
                AdjustmentTable.Columns.Add("Vendor", typeof(string));
                AdjustmentTable.Columns.Add("Project", typeof(string));
                AdjustmentTable.Columns.Add("RefDoc", typeof(string));
                AdjustmentTable.Columns.Add("Amount", typeof(string));
                AdjustmentTable.Columns.Add("BalAmount", typeof(string));
                AdjustmentTable.Columns.Add("AdjAmount", typeof(string));
                AdjustmentTable.Columns.Add("Remarks", typeof(string));
                AdjustmentTable.Columns.Add("UnitID", typeof(string));
                AdjustmentTable.Columns.Add("ProjectId", typeof(string));
                AdjustmentTable.Columns.Add("SchemaID", typeof(string));
                AdjustmentTable.Columns.Add("VendorID", typeof(string));
                AdjustmentTable.Columns.Add("DocId", typeof(string));
                AdjustmentTable.Columns.Add("UpdateEdit", typeof(string));
            }

            DataRow NewRow;
            foreach (var args in e.InsertValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["Unit"])))
                {
                    NewRow = AdjustmentTable.NewRow();
                    NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
                    NewRow["Unit"] = Convert.ToString(args.NewValues["Unit"]);
                    NewRow["DocumentNumbering"] = Convert.ToString(args.NewValues["DocumentNumbering"]);
                    NewRow["Vendor"] = Convert.ToString(args.NewValues["Vendor"]);
                    NewRow["Project"] = Convert.ToString(args.NewValues["Project"]);
                    NewRow["RefDoc"] = Convert.ToString(args.NewValues["RefDoc"]);
                    NewRow["Amount"] = Convert.ToString(args.NewValues["Amount"]);
                    NewRow["BalAmount"] = Convert.ToString(args.NewValues["BalAmount"]);
                    NewRow["AdjAmount"] = Convert.ToString(args.NewValues["AdjAmount"]);
                    NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
                    NewRow["UnitID"] = Convert.ToInt64(args.NewValues["UnitID"]);
                    NewRow["ProjectId"] = Convert.ToString(args.NewValues["ProjectId"]);
                    NewRow["SchemaID"] = Convert.ToString(args.NewValues["SchemaID"]);
                    NewRow["VendorID"] = Convert.ToString(args.NewValues["VendorID"]);
                    NewRow["DocId"] = Convert.ToString(args.NewValues["DocId"]);
                    NewRow["UpdateEdit"] = Convert.ToString(args.NewValues["UpdateEdit"]);
                    AdjustmentTable.Rows.Add(NewRow);
                }
            }

            //foreach (var args in e.UpdateValues)
            //{
            //    if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["Unit"])))
            //    {
            //        NewRow = AdjustmentTable.NewRow();
            //        NewRow["SrlNo"] = Convert.ToString(args.NewValues["SrlNo"]);
            //        NewRow["Unit"] = Convert.ToString(args.NewValues["Unit"]);
            //        NewRow["DocumentNumbering"] = Convert.ToString(args.NewValues["DocumentNumbering"]);
            //        NewRow["Vendor"] = Convert.ToString(args.NewValues["Vendor"]);
            //        NewRow["Project"] = Convert.ToString(args.NewValues["Project"]);
            //        NewRow["RefDoc"] = Convert.ToString(args.NewValues["RefDoc"]);
            //        NewRow["Amount"] = Convert.ToString(args.NewValues["Amount"]);
            //        NewRow["BalAmount"] = Convert.ToString(args.NewValues["BalAmount"]);
            //        NewRow["AdjAmount"] = Convert.ToString(args.NewValues["AdjAmount"]);
            //        NewRow["Remarks"] = Convert.ToString(args.NewValues["Remarks"]);
            //        NewRow["UnitID"] = Convert.ToInt64(args.NewValues["UnitID"]);
            //        NewRow["ProjectId"] = Convert.ToString(args.NewValues["ProjectId"]);
            //        NewRow["SchemaID"] = Convert.ToString(args.NewValues["SchemaID"]);
            //        NewRow["VendorID"] = Convert.ToString(args.NewValues["VendorID"]);
            //        NewRow["DocId"] = Convert.ToString(args.NewValues["DocId"]);
            //        NewRow["UpdateEdit"] = Convert.ToString(args.NewValues["UpdateEdit"]);
            //        AdjustmentTable.Rows.Add(NewRow);
            //    }
            //}

            foreach (var args in e.UpdateValues)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(args.NewValues["Unit"])))
                {
                    // NewRow = AdjustmentTable.NewRow();
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string Unit = Convert.ToString(args.NewValues["Unit"]);
                    string DocumentNumbering = Convert.ToString(args.NewValues["DocumentNumbering"]);
                    string Vendor = Convert.ToString(args.NewValues["Vendor"]);
                    string Project = Convert.ToString(args.NewValues["Project"]);
                    string RefDoc = Convert.ToString(args.NewValues["RefDoc"]);
                    string Amount = Convert.ToString(args.NewValues["Amount"]);
                    string BalAmount = Convert.ToString(args.NewValues["BalAmount"]);
                    string AdjAmount = Convert.ToString(args.NewValues["AdjAmount"]);
                    string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                    string UnitID = Convert.ToString(args.NewValues["UnitID"]);
                    string ProjectId = Convert.ToString(args.NewValues["ProjectId"]);
                    string SchemaID = Convert.ToString(args.NewValues["SchemaID"]);
                    string VendorID = Convert.ToString(args.NewValues["VendorID"]);
                    string DocId = Convert.ToString(args.NewValues["DocId"]);
                    string UpdateEdit = Convert.ToString(args.NewValues["UpdateEdit"]);
                    // AdjustmentTable.Rows.Add(NewRow);

                    //string SrlNo = Convert.ToString(args.Keys["SrlNo"]);

                    bool isDeleted = false;
                    foreach (var arg in e.DeleteValues)
                    {
                        string DeleteID = Convert.ToString(arg.Keys["SrlNo"]);
                        if (DeleteID == SrlNo)
                        {
                            isDeleted = true;
                            break;
                        }
                    }

                    if (isDeleted == false)
                    {
                        bool Isexists = false;
                        foreach (DataRow drr in AdjustmentTable.Rows)
                        {
                            string OldSrlNo = Convert.ToString(drr["SrlNo"]);

                            if (OldSrlNo == SrlNo)
                            {
                                Isexists = true;

                                drr["SrlNo"] = SrlNo;
                                drr["Unit"] = Unit;
                                drr["DocumentNumbering"] = DocumentNumbering;
                                drr["Vendor"] = Vendor;
                                drr["Project"] = Project;
                                drr["RefDoc"] = RefDoc;
                                drr["Amount"] = Amount;
                                drr["BalAmount"] = BalAmount;
                                drr["AdjAmount"] = AdjAmount;
                                drr["Remarks"] = Remarks;
                                drr["UnitID"] = UnitID;
                                drr["ProjectId"] = ProjectId;
                                drr["SchemaID"] = SchemaID;
                                drr["VendorID"] = VendorID;
                                drr["DocId"] = DocId;
                                drr["UpdateEdit"] = UpdateEdit;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            AdjustmentTable.Rows.Add(SrlNo, Unit, DocumentNumbering, Vendor, Project, RefDoc, Amount, BalAmount, AdjAmount, Remarks, UnitID, ProjectId, SchemaID, VendorID, DocId, UpdateEdit);

                        }
                    }
                }
            }


            foreach (var args in e.DeleteValues)
            {
                string SrlNoID = Convert.ToString(args.Keys["SrlNo"]);
                string SrlNo = "";

                for (int i = AdjustmentTable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = AdjustmentTable.Rows[i];
                    string delSrlNo = Convert.ToString(dr["SrlNo"]);

                    if (delSrlNo == SrlNo)
                    {
                        SrlNo = Convert.ToString(dr["SrlNo"]);
                        dr.Delete();
                    }
                }
                AdjustmentTable.AcceptChanges();

            }



            DataTable BatchGridData = AdjustmentTable.Copy();

            Session["VendorGrid"] = BatchGridData;

            var query = from row in BatchGridData.AsEnumerable()
                        group row by row.Field<string>("RefDoc") into sales
                        select new
                        {
                            Doc_No = sales.Key,
                            Count = sales.Count()
                        };

            foreach (var obj in query)
            {
                if (obj.Count > 1 && obj.Doc_No != "")
                {
                    valid = false;
                    vendorgrid.JSProperties["cpSuccess"] = "0";
                    vendorgrid.JSProperties["cpMsg"] = "Can not select duplecate document number(Vendor) . Document Number : " + obj.Doc_No;
                    vendorgrid.DataBind();
                }
            }

            foreach (DataRow item in BatchGridData.Rows)
            {
                if (Convert.ToString(item["UnitID"]) == "")
                {
                    valid = false;
                    vendorgrid.JSProperties["cpSuccess"] = "0";
                    vendorgrid.JSProperties["cpMsg"] = "Unit can not be blank (Vendor) . Line Number : " + Convert.ToString(item["SrlNo"]);
                    //vendorgrid.DataBind();
                }
                else if (Convert.ToString(item["VendorID"]) == "")
                {
                    valid = false;
                    vendorgrid.JSProperties["cpSuccess"] = "0";
                    vendorgrid.JSProperties["cpMsg"] = "Vendor can not be blank (Vendor) . Line Number : " + Convert.ToString(item["SrlNo"]);
                    //vendorgrid.DataBind();
                }
                else if (Convert.ToString(item["AdjAmount"]) == "0.00")
                {
                    valid = false;
                    vendorgrid.JSProperties["cpSuccess"] = "0";
                    vendorgrid.JSProperties["cpMsg"] = "Adj. Amount can not be zero (Vendor) . Line Number : " + Convert.ToString(item["SrlNo"]);
                    //vendorgrid.DataBind();
                }
                else if (Convert.ToString(item["SchemaID"]) == "")
                {
                    valid = false;
                    vendorgrid.JSProperties["cpSuccess"] = "0";
                    vendorgrid.JSProperties["cpMsg"] = "Journal Numbering Schema can not be blank (Vendor) . Line Number : " + Convert.ToString(item["SrlNo"]);
                    //vendorgrid.DataBind();
                }
            }

            String Action = "Add";
            string output_text = "";
            if (valid)
            {
                string output_id = AddEditJournal(Action, (DataTable)Session["CustomerGrid"], (DataTable)Session["VendorGrid"], cmbBranchfilter.Value.ToString(), txtVoucherNo.Text, dtTDate.Date, (string)Session["userid"], ref output_text);

                if (Convert.ToInt32(output_id) > 0)
                {
                    vendorgrid.JSProperties["cpSuccess"] = "1";
                    vendorgrid.JSProperties["cpMsg"] = output_text;
                }
                else
                {
                    vendorgrid.JSProperties["cpSuccess"] = "0";
                    vendorgrid.JSProperties["cpMsg"] = output_text;
                }
            }




        }


        public string AddEditJournal(string action, DataTable Customer, DataTable Vendor, string schema, string doc_no, DateTime date, string user_id, ref string output_text)
        {

            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_PARTYJOURNAL_ADDEDIT", con);
            DataTable dtReceipt = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@Customer_Table", Customer);
            cmd.Parameters.AddWithValue("@Vendor_Table", Vendor);
            cmd.Parameters.AddWithValue("@Numbering", schema);
            cmd.Parameters.AddWithValue("@Doc_No", doc_no);
            cmd.Parameters.AddWithValue("@Date", date);
            cmd.Parameters.AddWithValue("@userid", user_id);
            SqlParameter output = new SqlParameter("@Return_Id", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            SqlParameter outputText = new SqlParameter("@Return_Text", SqlDbType.VarChar, 200);
            outputText.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(output);
            cmd.Parameters.Add(outputText);
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            string output_id = Convert.ToString(cmd.Parameters["@Return_Id"].Value.ToString());
            output_text = Convert.ToString(cmd.Parameters["@Return_Text"].Value.ToString());

            return output_id;
        }

        protected void vendorgrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["VendorGrid"] != null)
            {
                vendorgrid.DataSource = (DataTable)Session["VendorGrid"];
            }
        }
        #endregion

        #region Web Event

        [WebMethod(EnableSession = true)]
        public static object GetBranch()
        {
            List<branchs> brs = new List<branchs>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@BRANCH_HIERARCHY", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Action", 500, "GETBRANCH");
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                brs = (from DataRow dr in dt.Rows
                       select new branchs()
                       {
                           branch_description = Convert.ToString(dr["branch_description"]),
                           branch_id = Convert.ToString(dr["branch_id"])
                       }).ToList();
            }


            return brs;
        }

        [WebMethod(EnableSession = true)]
        public static object GetNumbering(string branch_id)
        {
            List<schema> obj = new List<schema>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@Action", 500, "GETNumbering");
            proc.AddVarcharPara("@Branch_id", 500, branch_id);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                obj = (from DataRow dr in dt.Rows
                       select new schema()
                       {
                           branch_description = Convert.ToString(dr["branch_description"]),
                           ID = Convert.ToString(dr["ID"]),
                           S_NAME = Convert.ToString(dr["S_NAME"])

                       }).ToList();
            }


            return obj;
        }

        [WebMethod(EnableSession = true)]
        public static object GetCustomer()
        {
            List<customer> obj = new List<customer>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@Action", 500, "GetCustomer");
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                obj = (from DataRow dr in dt.Rows
                       select new customer()
                       {
                           Billing = Convert.ToString(dr["Billing"]),
                           cnt_internalid = Convert.ToString(dr["cnt_internalid"]),
                           Name = Convert.ToString(dr["Name"]),
                           Shipping = Convert.ToString(dr["Shipping"]),
                           shortname = Convert.ToString(dr["shortname"]),
                           Type = Convert.ToString(dr["Type"])

                       }).ToList();
            }


            return obj;
        }

        [WebMethod(EnableSession = true)]
        public static object GetVendor()
        {
            List<customer> obj = new List<customer>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@Action", 500, "GetVendor");
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                obj = (from DataRow dr in dt.Rows
                       select new customer()
                       {
                           Billing = Convert.ToString(dr["Billing"]),
                           cnt_internalid = Convert.ToString(dr["cnt_internalid"]),
                           Name = Convert.ToString(dr["Name"]),
                           Shipping = Convert.ToString(dr["Shipping"]),
                           shortname = Convert.ToString(dr["shortname"]),                  
                           Type = Convert.ToString(dr["Type"])

                       }).ToList();
            }


            return obj;
        }

        [WebMethod(EnableSession = true)]
        public static object GetProject(string customer_id, string branch_id)
        {
            List<project> obj = new List<project>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@Action", 500, "GetProject");
            proc.AddVarcharPara("@customer_id", 500, customer_id);
            proc.AddVarcharPara("@branch_id", 500, branch_id);

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                obj = (from DataRow dr in dt.Rows
                       select new project()
                       {
                           Customer = Convert.ToString(dr["Customer"]),
                           Proj_Code = Convert.ToString(dr["Proj_Code"]),
                           Proj_Id = Convert.ToString(dr["Proj_Id"]),
                           Proj_Name = Convert.ToString(dr["Proj_Name"])
                       }).ToList();
            }


            return obj;
        }

        [WebMethod(EnableSession = true)]
        public static object GetProjectVendor(string customer_id, string branch_id)
        {
            List<project> obj = new List<project>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@Action", 500, "GetProjectVendor");
            proc.AddVarcharPara("@customer_id", 500, customer_id);
            proc.AddVarcharPara("@branch_id", 500, branch_id);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                obj = (from DataRow dr in dt.Rows
                       select new project()
                       {
                           Customer = Convert.ToString(dr["Customer"]),
                           Proj_Code = Convert.ToString(dr["Proj_Code"]),
                           Proj_Id = Convert.ToString(dr["Proj_Id"]),
                           Proj_Name = Convert.ToString(dr["Proj_Name"])
                       }).ToList();
            }


            return obj;
        }


        [WebMethod(EnableSession = true)]
        public static object GetRefDoc(string customer_id, string branch_id, string project_id, string date)
        {
            List<refDoc> obj = new List<refDoc>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@Action", 500, "GetRefDoc");
            proc.AddVarcharPara("@customer_id", 500, customer_id);
            proc.AddVarcharPara("@branch_id", 500, branch_id);
            proc.AddVarcharPara("@project_id", 500, project_id);
            proc.AddVarcharPara("@date", 500, date);

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                obj = (from DataRow dr in dt.Rows
                       select new refDoc()
                       {
                           branch_description = Convert.ToString(dr["branch_description"]),
                           Cust_Name = Convert.ToString(dr["Cust_Name"]),
                           invoice_date = Convert.ToString(dr["invoice_date"]),
                           Invoice_Id = Convert.ToString(dr["Invoice_Id"]),
                           Invoice_Number = Convert.ToString(dr["Invoice_Number"]),
                           Invoice_TotalAmount = Convert.ToString(dr["Invoice_TotalAmount"]),
                           Proj_Name = Convert.ToString(dr["Proj_Name"]),
                           UnPaidAmount = Convert.ToString(dr["UnPaidAmount"])

                       }).ToList();
            }


            return obj;
        }

        [WebMethod(EnableSession = true)]
        public static object GetRefDocVendor(string vendor_id, string branch_id, string project_id, string date)
        {
            List<refDoc> obj = new List<refDoc>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYJOURNAL");
            proc.AddVarcharPara("@Action", 500, "GetRefDocVendor");
            proc.AddVarcharPara("@customer_id", 500, vendor_id);
            proc.AddVarcharPara("@branch_id", 500, branch_id);
            proc.AddVarcharPara("@project_id", 500, project_id);
            proc.AddVarcharPara("@date", 500, date);

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                obj = (from DataRow dr in dt.Rows
                       select new refDoc()
                       {
                           branch_description = Convert.ToString(dr["branch_description"]),
                           Cust_Name = Convert.ToString(dr["Cust_Name"]),
                           invoice_date = Convert.ToString(dr["invoice_date"]),
                           Invoice_Id = Convert.ToString(dr["Invoice_Id"]),
                           Invoice_Number = Convert.ToString(dr["Invoice_Number"]),
                           Invoice_TotalAmount = Convert.ToString(dr["Invoice_TotalAmount"]),
                           Proj_Name = Convert.ToString(dr["Proj_Name"]),
                           UnPaidAmount = Convert.ToString(dr["UnPaidAmount"])

                       }).ToList();
            }


            return obj;
        }
        #endregion
    }

    #region LINQ class
    public class branchs
    {
        public string branch_id { get; set; }
        public string branch_description { get; set; }

    }

    public class schema
    {
        public string ID { get; set; }
        public string S_NAME { get; set; }

        public string branch_description { get; set; }

    }
    public class customer
    {
        public string cnt_internalid { get; set; }
        public string shortname { get; set; }
        public string Name { get; set; }
        public string Billing { get; set; }
        public string Shipping { get; set; }
        public string Type { get; set; }
    }

    public class project
    {
        public string Proj_Id { get; set; }
        public string Proj_Name { get; set; }
        public string Proj_Code { get; set; }
        public string Customer { get; set; }

    }

    public class refDoc
    {
        public string Invoice_Id { get; set; }
        public string Invoice_Number { get; set; }
        public string invoice_date { get; set; }
        public string Cust_Name { get; set; }
        public string branch_description { get; set; }
        public string Invoice_TotalAmount { get; set; }
        public string UnPaidAmount { get; set; }
        public string Proj_Name { get; set; }



    }
    #endregion

}