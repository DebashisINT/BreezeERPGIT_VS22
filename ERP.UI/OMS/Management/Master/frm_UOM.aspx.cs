using System.Data;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using DataAccessLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_UOM : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        BusinessLogicLayer.frm_UOM_BL bl = new BusinessLogicLayer.frm_UOM_BL();
        protected void Page_Load(object sender, EventArgs e)
        {
             rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frm_UOM.aspx");

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            FillGridView();

        }
        public void bindexport(int Filter)
        {
            grdDocuments.Columns[5].Visible = false; 
          
            string filename = "Units Of Measurement";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Units Of Measurement";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

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

        public void FillGridView()
        {

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlDataAdapter da = new SqlDataAdapter("Report_Uom", con))
            //    {
            //if (con.State == ConnectionState.Closed)
            //    con.Open();
            //ds.Reset();
            //da.Fill(ds);
            ds = bl.Report_Uom();

            if (ds==null ||ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Height4", "alert('No Record Found!..');", true);
            }
            else
            {
                dt = ds.Tables[0];
                grdDocuments.DataSource = dt.DefaultView;
                grdDocuments.DataBind();
            }
            //  }
            // }
        }

        protected void grdDocuments_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grdDocuments.ClearSort();
           
            if (e.Parameters == "s")
                grdDocuments.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                grdDocuments.FilterExpression = string.Empty;
            }
            string param = Convert.ToString(e.Parameters);
            if (param.Split('~')[0] == "Del")
            {
                string ID = Convert.ToString(param.Split('~')[1]);
                if (!IsUOMTransactionExist(ID))
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
                  
                    int rowsNo = oDBEngine.DeleteValue("Master_UOM", " UOM_ID=" + ID + "");
                    //FillGridView();

                    if (rowsNo > 0)
                    {
                        grdDocuments.JSProperties["cpReturnMesg"] = "Document Deleted Successfully";
                    }
                }
                else
                {
                    grdDocuments.JSProperties["cpReturnMesg"] = "This UOM is tagged in other modules. Cannot Delete.";
                }
            }
            FillGridView();
        }
        private bool IsUOMTransactionExist(string UOMid)
        {
            bool IsExist = false;
            if (UOMid != "" && Convert.ToString(UOMid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = CheckUOMTraanaction(UOMid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        public DataTable CheckUOMTraanaction(string UOMid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_UOM_Details");
            proc.AddVarcharPara("@Action", 100, "UOMTagOrNot");
            proc.AddIntegerPara("@UOMId", Convert.ToInt32(UOMid));
            dt = proc.GetTable();
            return dt;
        }
       
        protected void grdDocuments_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            string ID = Convert.ToString(e.Keys[0]);
            oDBEngine.DeleteValue("tbl_master_forms", " frm_id=" + ID + "");
            e.Cancel = true;
            FillGridView();
        }
        protected void grdDocuments_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        { 
            //comment below line for PDF export where giving error due to index was visible false 27122016

            //if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            //int rowindex = e.VisibleIndex;
            //string Rowid = Convert.ToString(e.GetValue("tmp_id"));
            //string correspond = Convert.ToString(e.GetValue("tmp_convuom"));
            //e.Row.Cells[4].Style.Add("cursor", "pointer");
            ////e.Row.Cells[4].ToolTip = "Click here to Change !";
            //e.Row.Cells[4].ToolTip = "Modify";
            //e.Row.Cells[4].Attributes.Add("onclick", "javascript:Changestatus('" + Rowid + "(" + correspond + "');");
            //e.Row.Cells[4].Style.Add("color", "Blue");
            //e.Row.Cells[4].Text = "abc";
        }
    }

}