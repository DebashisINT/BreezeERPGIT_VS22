using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class SrvProbMast : System.Web.UI.Page
    {
        SrvProbMastBL ProbBL = new SrvProbMastBL();
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/SrvProbMast.aspx");

            GridProb.JSProperties["cpEdit"] = null;
            GridProb.JSProperties["cpinsert"] = null;
            GridProb.JSProperties["cpUpdate"] = null;
            GridProb.JSProperties["cpDelete"] = null;
            GridProb.JSProperties["cpExists"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            BindProbGrid();

            if (!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }

        protected void BindProbGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            //dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM Master_Problem order by ProblemID");
            dtFillGrid = ProbBL.GetProbList();
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                GridProb.DataSource = dtFillGrid;
                GridProb.DataBind();
            }
            else
            {
                GridProb.DataSource = null;
                GridProb.DataBind();
            }
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            GridProb.Settings.ShowFilterRow = true;
        }

        protected void GridProb_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridProb.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridProb.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }

        protected void GridProb_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            GridProb.JSProperties["cpEdit"] = null;
            GridProb.JSProperties["cpinsert"] = null;
            GridProb.JSProperties["cpUpdate"] = null;
            GridProb.JSProperties["cpDelete"] = null;
            GridProb.JSProperties["cpExists"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridProb.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridProb.FilterExpression = string.Empty;
            }

            if (WhichCall == "Edit")
            {
                //  DataTable dtEdit = oGenericMethod.GetDataTable("select ProblemID,ProblemDesc from Master_Problem where ProblemID=" + WhichType + "");
                DataTable dtEdit = ProbBL.GetProbEdit(WhichType);
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string ProbDesc = Convert.ToString(dtEdit.Rows[0]["ProblemDesc"]);
                    int ProbId = Convert.ToInt32(dtEdit.Rows[0]["ProblemID"]);
                    GridProb.JSProperties["cpEdit"] = ProbDesc + "~" + ProbId;
                }
            }

            if (WhichCall == "updateProb")
            {
                // updtcnt = oGenericMethod.Update_Table("Master_Problem", "ProblemDesc='" + txtProbDesc.Text + "',Modified_By='" + Session["userid"] + "',Modifed_On='" + oGenericMethod.GetDate(110) + "'", "ProblemID=" + WhichType + "");
                updtcnt = ProbBL.UpdateProblemMaster(txtProbDesc.Text, Session["userid"].ToString(), WhichType);
                if (updtcnt > 0)
                {
                    GridProb.JSProperties["cpUpdate"] = "Success";
                    BindProbGrid();
                }
                else
                    GridProb.JSProperties["cpUpdate"] = "fail";

            }
            if (WhichCall == "saveProb")
            {
                // string[,] countrecord = oGenericMethod.GetFieldValue("Master_Problem", "ProblemDesc", "ProblemDesc='" + txtProbDesc.Text + "'", 1);
                DataTable dtCheck = ProbBL.GetProbChecking(txtProbDesc.Text);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    GridProb.JSProperties["cpExists"] = "Exists";
                }
                else
                {
                    //insertcount = oGenericMethod.Insert_Table("Master_Problem", "ProblemDesc,Created_On,Created_By",
                    //   "'" + txtProbDesc.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"]);
                    insertcount = ProbBL.InsertProblemMaster(txtProbDesc.Text.Trim(), Session["userid"].ToString());
                    if (insertcount > 0)
                    {
                        GridProb.JSProperties["cpinsert"] = "Success";
                        BindProbGrid();
                    }
                    else
                        GridProb.JSProperties["cpinsert"] = "fail";
                }
            }

            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
              //  int i = masterdata.Deletecountry(Convert.ToString(id));
                int i = ProbBL.DeleteProblemMaster(Convert.ToString(id));
                if (i == 1)
                {
                    GridProb.JSProperties["cpDelete"] = "Succesfully Deleted";
                }
                else
                {
                    GridProb.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";
                }
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

        public void bindexport(int Filter)
        {
            GridProb.Columns[2].Visible = false;

            string filename = "Problem";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Problem";
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
    }
}