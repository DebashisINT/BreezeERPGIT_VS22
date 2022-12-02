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
    public partial class SrvReturnReason : System.Web.UI.Page
    {
        SrvReturnReasonBL ReasonBL = new SrvReturnReasonBL();
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/SrvReturnReason.aspx");

            GridReason.JSProperties["cpEdit"] = null;
            GridReason.JSProperties["cpinsert"] = null;
            GridReason.JSProperties["cpUpdate"] = null;
            GridReason.JSProperties["cpDelete"] = null;
            GridReason.JSProperties["cpExists"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            BindReasonGrid();

            if (!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }

        protected void BindReasonGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            //dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM Master_Reasonlem order by ReasonlemID");
            dtFillGrid = ReasonBL.GetReasonList();
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                GridReason.DataSource = dtFillGrid;
                GridReason.DataBind();
            }
            else
            {
                GridReason.DataSource = null;
                GridReason.DataBind();
            }
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            GridReason.Settings.ShowFilterRow = true;
        }

        protected void GridReason_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridReason.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridReason.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }

        protected void GridReason_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            GridReason.JSProperties["cpEdit"] = null;
            GridReason.JSProperties["cpinsert"] = null;
            GridReason.JSProperties["cpUpdate"] = null;
            GridReason.JSProperties["cpDelete"] = null;
            GridReason.JSProperties["cpExists"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridReason.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridReason.FilterExpression = string.Empty;
            }

            if (WhichCall == "Edit")
            {
                //  DataTable dtEdit = oGenericMethod.GetDataTable("select ReasonlemID,ReasonlemDesc from Master_Reasonlem where ReasonlemID=" + WhichType + "");
                DataTable dtEdit = ReasonBL.GetReasonEdit(WhichType);
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string ReasonDesc = Convert.ToString(dtEdit.Rows[0]["ReasonDesc"]);
                    int ReasonId = Convert.ToInt32(dtEdit.Rows[0]["ReasonID"]);
                    GridReason.JSProperties["cpEdit"] = ReasonDesc + "~" + ReasonId;
                }
            }

            if (WhichCall == "updateReason")
            {
                // updtcnt = oGenericMethod.Update_Table("Master_Reasonlem", "ReasonlemDesc='" + txtReasonDesc.Text + "',Modified_By='" + Session["userid"] + "',Modifed_On='" + oGenericMethod.GetDate(110) + "'", "ReasonlemID=" + WhichType + "");
                updtcnt = ReasonBL.UpdateReasonMaster(txtReasonDesc.Text, Session["userid"].ToString(), WhichType);
                if (updtcnt > 0)
                {
                    GridReason.JSProperties["cpUpdate"] = "Success";
                    BindReasonGrid();
                }
                else
                    GridReason.JSProperties["cpUpdate"] = "fail";

            }
            if (WhichCall == "saveReason")
            {
                // string[,] countrecord = oGenericMethod.GetFieldValue("Master_Reasonlem", "ReasonlemDesc", "ReasonlemDesc='" + txtReasonDesc.Text + "'", 1);
                DataTable dtCheck = ReasonBL.GetReasonChecking(txtReasonDesc.Text);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    GridReason.JSProperties["cpExists"] = "Exists";
                }
                else
                {
                    //insertcount = oGenericMethod.Insert_Table("Master_Reasonlem", "ReasonlemDesc,Created_On,Created_By",
                    //   "'" + txtReasonDesc.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"]);
                    insertcount = ReasonBL.InsertReasonMaster(txtReasonDesc.Text.Trim(), Session["userid"].ToString());
                    if (insertcount > 0)
                    {
                        GridReason.JSProperties["cpinsert"] = "Success";
                        BindReasonGrid();
                    }
                    else
                        GridReason.JSProperties["cpinsert"] = "fail";
                }
            }

            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
                //  int i = masterdata.Deletecountry(Convert.ToString(id));
                int i = ReasonBL.DeleteReasonMaster(Convert.ToString(id));
                if (i == 1)
                {
                    GridReason.JSProperties["cpDelete"] = "Succesfully Deleted";
                }
                else
                {
                    GridReason.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";
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
            GridReason.Columns[2].Visible = false;

            string filename = "Return Reason";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Return Reason";
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