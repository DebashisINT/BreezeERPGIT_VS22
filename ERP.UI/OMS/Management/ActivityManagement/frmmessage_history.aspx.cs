using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_frmmessage_history : System.Web.UI.Page
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataSource = (DataTable)Session["GridDateReport"];
                GridView1.DataBind();
                if (Session["FileName"] != null)
                {
                    lbl.Text = Session["FileName"].ToString();
                }
            }
        }
        protected void Button4_Click(object sender, EventArgs e)
        {
            string filename;
            if (Session["FileName"] == null)
            {
                filename = "Message_History_" + Session["userid"].ToString() + "_" + oDBEngine.GetDate().ToShortDateString();
            }
            else
            {
                filename = Session["FileName"].ToString();
            }
            if (Convert.ToInt32(DropDownList2.SelectedValue) > 0)
            {
                if (Convert.ToInt32(DropDownList2.SelectedValue) == 1)
                {
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                    HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                    GridView1.RenderControl(htmlWriter);
                    Response.Write(stringWriter.ToString());
                    Response.End();
                }
                else if (Convert.ToInt32(DropDownList2.SelectedValue) == 2)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".doc");
                    Response.ContentType = "application/vnd.word";
                    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                    HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                    GridView1.RenderControl(htmlWriter);
                    Response.Write(stringWriter.ToString());
                    Response.End();
                }
                else if (Convert.ToInt32(DropDownList2.SelectedValue) == 3)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".txt");
                    Response.ContentType = "application/vnd.txt";
                    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                    HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                    GridView1.RenderControl(htmlWriter);
                    Response.Write(stringWriter.ToString());
                    Response.End();
                }
            }
        }
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "center");
                    e.Row.Cells[i].Text = Server.HtmlDecode(e.Row.Cells[i].Text.ToString());
                }
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            System.Web.UI.Control ctrl = (System.Web.UI.Control)pnl;
            Converter.PrintWebControl(ctrl);
        }
    }
}