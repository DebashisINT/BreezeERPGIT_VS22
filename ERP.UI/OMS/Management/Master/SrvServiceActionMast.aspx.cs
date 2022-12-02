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
    public partial class SrvServiceActionMast : System.Web.UI.Page
    {
        SrvServiceActionMastBL SrvBL = new SrvServiceActionMastBL();
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/SrvServiceActionMast.aspx");

            GridServiceAction.JSProperties["cpEdit"] = null;
            GridServiceAction.JSProperties["cpinsert"] = null;
            GridServiceAction.JSProperties["cpUpdate"] = null;
            GridServiceAction.JSProperties["cpDelete"] = null;
            GridServiceAction.JSProperties["cpExists"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            BindServiceActionGrid();

            if (!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }

        protected void BindServiceActionGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            //dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM Master_ServiceAction order by SrvActionID");
            dtFillGrid = SrvBL.GetServiceActionList();
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                GridServiceAction.DataSource = dtFillGrid;
                GridServiceAction.DataBind();
            }
            else
            {
                GridServiceAction.DataSource = null;
                GridServiceAction.DataBind();
            }
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            GridServiceAction.Settings.ShowFilterRow = true;
        }

        protected void GridServiceAction_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridServiceAction.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridServiceAction.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }

        protected void GridServiceAction_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            GridServiceAction.JSProperties["cpEdit"] = null;
            GridServiceAction.JSProperties["cpinsert"] = null;
            GridServiceAction.JSProperties["cpUpdate"] = null;
            GridServiceAction.JSProperties["cpDelete"] = null;
            GridServiceAction.JSProperties["cpExists"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridServiceAction.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridServiceAction.FilterExpression = string.Empty;
            }

            if (WhichCall == "Edit")
            {
                //  DataTable dtEdit = oGenericMethod.GetDataTable("select SrvActionID,SrvActionDesc from Master_ServiceActionlem where SrvActionID=" + WhichType + "");
                DataTable dtEdit = SrvBL.GetServiceActionEdit(WhichType);
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string ServiceActionDesc = Convert.ToString(dtEdit.Rows[0]["SrvActionDesc"]);
                    int ServiceActionId = Convert.ToInt32(dtEdit.Rows[0]["SrvActionID"]);
                    GridServiceAction.JSProperties["cpEdit"] = ServiceActionDesc + "~" + ServiceActionId;
                }
            }

            if (WhichCall == "updateServiceAction")
            {
                // updtcnt = oGenericMethod.Update_Table("Master_ServiceActionlem", "SrvActionDesc='" + txtServiceActionDesc.Text + "',Modified_By='" + Session["userid"] + "',Modifed_On='" + oGenericMethod.GetDate(110) + "'", "SrvActionID=" + WhichType + "");
                updtcnt = SrvBL.UpdateServiceActionMaster(txtServiceActionDesc.Text, Session["userid"].ToString(), WhichType);
                if (updtcnt > 0)
                {
                    GridServiceAction.JSProperties["cpUpdate"] = "Success";
                    BindServiceActionGrid();
                }
                else
                    GridServiceAction.JSProperties["cpUpdate"] = "fail";

            }
            if (WhichCall == "saveServiceAction")
            {
                // string[,] countrecord = oGenericMethod.GetFieldValue("Master_ServiceActionlem", "SrvActionDesc", "SrvActionDesc='" + txtServiceActionDesc.Text + "'", 1);
                DataTable dtCheck = SrvBL.GetServiceActionChecking(txtServiceActionDesc.Text);
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    GridServiceAction.JSProperties["cpExists"] = "Exists";
                }
                else
                {
                    //insertcount = oGenericMethod.Insert_Table("Master_ServiceActionlem", "SrvActionDesc,Created_On,Created_By",
                    //   "'" + txtServiceActionDesc.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"]);
                    insertcount = SrvBL.InsertServiceActionMaster(txtServiceActionDesc.Text.Trim(), Session["userid"].ToString());
                    if (insertcount > 0)
                    {
                        GridServiceAction.JSProperties["cpinsert"] = "Success";
                        BindServiceActionGrid();
                    }
                    else
                        GridServiceAction.JSProperties["cpinsert"] = "fail";
                }
            }

            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
                //  int i = masterdata.Deletecountry(Convert.ToString(id));
                int i = SrvBL.DeletePServiceActionMaster(Convert.ToString(id));
                if (i == 1)
                {
                    GridServiceAction.JSProperties["cpDelete"] = "Succesfully Deleted";
                }
                else
                {
                    GridServiceAction.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";
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
            GridServiceAction.Columns[2].Visible = false;

            string filename = "Service Action";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Service Action";
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