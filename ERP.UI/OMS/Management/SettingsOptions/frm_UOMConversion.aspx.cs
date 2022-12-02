using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.IO;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_frm_UOMConversion : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            FillGridView();

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Commented By:Subhabrata

            //Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            //switch (Filter)
            //{
            //    case 1:
            //        exporter.WritePdfToResponse();
            //        break;
            //    case 2:
            //        exporter.WriteXlsToResponse();
            //        break;
            //    case 3:
            //        exporter.WriteRtfToResponse();
            //        break;
            //    case 4:
            //        exporter.WriteCsvToResponse();
            //        break;
            //}

            //END
            exporter.PageHeader.Left ="UOM Conversion rates";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    //exporter.WritePdfToResponse();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        exporter.WritePdf(stream);
                        WriteToResponse("UOM Conversion rates", true, "pdf", stream);
                    }
                    //Page.Response.End();
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

        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
            //Page.Response.End();
        }

        public void FillGridView()
        {

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Report_Uom", con))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    if (ds.Tables[1].Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Height4", "alert('No Record Found!..');", true);
                    }
                    else
                    {
                        dt = ds.Tables[1];
                        grdDocuments.DataSource = dt.DefaultView;
                        grdDocuments.DataBind();
                    }
                }
            }
        }

        protected void grdDocuments_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grdDocuments.ClearSort();
            FillGridView();
            if (e.Parameters == "s")
                grdDocuments.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                grdDocuments.FilterExpression = string.Empty;
            }
        }
        protected void grdDocuments_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            // DBEngine oDBEngine = new DBEngine(string.Empty);
            string ID = e.Keys[0].ToString();
            oDBEngine.DeleteValue("Config_Conversion", " Conversion_ID=" + ID + "");
            e.Cancel = true;
            FillGridView();
        }
        protected void grdDocuments_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string Rowid = e.GetValue("tmpconversion_id").ToString();
            string correspond = e.GetValue("tmpconversion_fromuom").ToString();
            string correspond1 = e.GetValue("tmpconversion_touom").ToString();
            e.Row.Cells[3].Style.Add("cursor", "hand");
            e.Row.Cells[3].ToolTip = "Click here to Change !";
            e.Row.Cells[3].Attributes.Add("onclick", "javascript:ChangestatusNew('" + Rowid + "(" + correspond + "(" + correspond1 + "');");
            e.Row.Cells[3].Style.Add("color", "Blue");
          
            


            FillGridView();
        }
    }
}