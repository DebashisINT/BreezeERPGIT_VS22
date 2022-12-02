using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntityLayer.CommonELS;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Web;

namespace ERP.OMS.Management.Master
{
    public partial class taskMaster : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TaskDataSrc.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSrcActivity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/taskMaster.aspx");
           
           
            //GetActivityTypeList
           /* BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            DataTable dtbl = new DataTable();
            dtbl = objbl.GetActivityTypeList();*/
        }

       /* [WebMethod]
        public static List<string> GetActivityTypeList()
        {
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            DataTable dtbl = new DataTable();
            dtbl = objbl.GetActivityTypeList();
            List<string> obj = new List<string>();
            foreach (DataRow dr in dtbl.Rows)
            {
                obj.Add(Convert.ToString(dr["aty_activityType"]) + "|" + Convert.ToString(dr["aty_id"]));
            }
            return obj;
        }*/
        void LoadListControlPostDataOnCallback(ListControl control)
        {
            if (!TaskGridView.IsEditing) return;
            foreach (ListItem item in control.Items)
                item.Selected = false;
            foreach (string key in Request.Params.AllKeys)
            {
                IPostBackDataHandler dataHandler = control as IPostBackDataHandler;
                if (key.StartsWith(control.UniqueID))
                    dataHandler.LoadPostData(key, Request.Params);
            }
        }
        protected void TaskGridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string txt_title = Convert.ToString(e.NewValues["task_title"]).Trim();
            if (txt_title=="")
            {
                e.Cancel = true;
                
                return;
            }
            ASPxCallbackPanel callPnl = (ASPxCallbackPanel)TaskGridView.FindEditFormTemplateControl("checkBoxList_callBack");
            ASPxCheckBoxList chkListObj = (ASPxCheckBoxList)callPnl.FindControl("checkBoxList");
            string chkListStr = string.Empty;

            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            int list_counter = chkListObj.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                if (chkListObj.Items[i].Selected)
                {
                    chkListStr = chkListObj.Items[i].Value + "~" + chkListStr;
                }
            }
            chkListStr = chkListStr.Length > 0 ? chkListStr.Remove(chkListStr.Length - 1, 1) : "";

            objbl.setTasksActivity(txt_title, chkListStr,"Add",0);  //--------29.12.2016------Subhra----last two parameters are optional for here,it is used in Edit
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            string filename = "Task Master";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Task Master";
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
        //--------29.12.2016------Subhra----
        protected void TaskGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string txt_title = Convert.ToString(e.NewValues["task_title"]).Trim();
            ASPxCallbackPanel callPnl=(ASPxCallbackPanel) TaskGridView.FindEditFormTemplateControl("checkBoxList_callBack");


            ASPxCheckBoxList chkListObj =(ASPxCheckBoxList) callPnl.FindControl("checkBoxList");
            string chkListStr = string.Empty;

            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            int list_counter = chkListObj.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                if (chkListObj.Items[i].Selected)
                {
                    chkListStr = chkListObj.Items[i].Value + "~" + chkListStr;
                }
            }
            chkListStr = chkListStr.Length > 0 ? chkListStr.Remove(chkListStr.Length - 1, 1) : "";

            objbl.setTasksActivity(txt_title, chkListStr, "Edit", Convert.ToInt32(e.NewValues["task_id"]));  
        }
        protected void checkBoxList_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');

            ASPxCallbackPanel cbPanl = source as ASPxCallbackPanel;
            ASPxCheckBoxList list = (ASPxCheckBoxList)cbPanl.FindControl("checkBoxList");

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtCmb = new DataTable();
            if (Session["Check_Activity"]!=null)
            {
                dtCmb = oGenericMethod.GetDataTable("select activity_type_id as activity_type_id from tbl_task_activity where ref_task_id=" + Convert.ToString(Session["Check_Activity"]) + "");
                Session["Check_Activity"] = null;

                int dlist_counter = dtCmb.Rows.Count;

                int list_counter = list.Items.Count;
                for (int j = 0; j < dlist_counter; j++)
                {
                    for (int i = 0; i < list_counter; i++)
                    {
                        if (Convert.ToString(list.Items[i].Value).Trim() == Convert.ToString(dtCmb.Rows[j][0].ToString()).Trim())
                        {
                            list.Items[i].Selected = true;
                        }
                    }
                }
            }
           
        }

        protected void TaskGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            Session["Check_Activity"] = Convert.ToString(e.EditingKeyValue);
        }

        protected void TaskGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            BusinessLogicLayer.MasterDataCheckingBL delobj = new BusinessLogicLayer.MasterDataCheckingBL();

            if (e.Keys[0] != null)
            {
                if (delobj.DeleteTask(Convert.ToInt32(e.Keys[0])) < 0)
                {
                    TaskGridView.JSProperties["cpErrorMsg"] = "Tax is in use. Cannot delete.";
                    e.Cancel = true;
                }
                else
                {
                    BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
                    objbl.setTasksActivity("", "", "Del", Convert.ToInt32(e.Keys[0]));
                }
            }
           
           
        }

    }
}