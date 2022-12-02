using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Collections.Generic;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class Management_PrintTag : System.Web.UI.Page
    {
        DBEngine oDBEngine = new DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            #region read data from xls or xl
            DataSet ds = new DataSet();
            DataTable dtm = new DataTable();
            string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {
                string fileLocation = Server.MapPath("~/BarCode/") + Request.Files["file"].FileName;
                if (System.IO.File.Exists(fileLocation))
                {

                    System.IO.File.Delete(fileLocation);
                }
                Request.Files["file"].SaveAs(fileLocation);
                string excelConnectionString = string.Empty;
                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                //connection String for xls file format.
                if (fileExtension == ".xls")
                {
                    excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                //connection String for xlsx file format.
                else if (fileExtension == ".xlsx")
                {

                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }
                //Create Connection to Excel work book and add oledb namespace
                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                excelConnection.Open();
                DataTable dt = new DataTable();

                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    //  return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int t = 0;
                //excel data saves in temp file here.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[t] = row["TABLE_NAME"].ToString();
                    t++;
                }
                OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                string query = string.Format("Select * from [{0}]", excelSheets[0]);
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                {
                    dataAdapter.Fill(ds);
                    //grdActive.DataSource=ds.Tables[0];
                    //grdActive.DataBind();
                    dtm = ds.Tables[0];
                }
            }
            #endregion
            dtm.Columns.Add("Id", typeof(System.Int32));
            int i = 1;
            foreach (DataRow row in dtm.Rows)
            {
                //need to set value to NewColumn column
                row["Id"] = i;   // or set it to some other value
                i = i + 1;
            }
            grdActive.DataSource = dtm;
            grdActive.DataBind();
            Session["chkData"] = dtm;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < grdActive.Columns.Count; i++)
            {
                dt.Columns.Add("column" + i.ToString());
            }
            foreach (GridViewRow row in grdActive.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < grdActive.Columns.Count; j++)
                {
                    dr["column" + j.ToString()] = row.Cells[j].Text;
                }

                dt.Rows.Add(dr);
            }
        }
        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            string strname = string.Empty;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["chkData"];
            List<string> lstids = new List<string>();
            foreach (GridViewRow gvrow in grdActive.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkActive");
                if (chk != null & chk.Checked)
                {
                    HiddenField hidId = (HiddenField)gvrow.FindControl("hidId");
                    lstids.Add(hidId.Value);
                }
            }
            int count = dt.Rows.Count;
            for (int i = 0; i < count - 1; i++)
            {
                try
                {
                    if (Convert.ToString(dt.Rows[i]["Id"]) != string.Empty)
                    {
                        if (!lstids.Contains(Convert.ToString(dt.Rows[i]["Id"])))
                        {
                            dt.Rows.Remove(dt.Rows[i]);
                            i = i - 1;
                        }
                    }
                }
                catch { }
            }

            string redirect = "<script>window.open('PrintTagPopup.aspx');</script>";
            Response.Write(redirect);
            //  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "key", "window.open('vehicle_trackView.aspx');", True)

        }
    }
}