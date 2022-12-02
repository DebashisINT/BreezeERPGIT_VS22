using DataAccessLayer;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.UserForm
{
    public partial class userformList : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/UserForm/userformList.aspx?ModName=" + Request.QueryString["ModName"].Replace(" ","%20"));

            if (!IsPostBack)
            {
                hdModuleName.Value = Request.QueryString["ModName"];
                ProcedureExecute proc = new ProcedureExecute("prc_UserDefineForm");
                proc.AddVarcharPara("@Action", 100, "GetColumnList");
                proc.AddVarcharPara("@ModName", 500, Convert.ToString(hdModuleName.Value));
                proc.AddPara("@Userid", Convert.ToString(Session["userid"]));
                DataSet ds = proc.GetDataSet();

                ModuleName.Text = Request.QueryString["ModName"];
                string SqlQuery =Convert.ToString(ds.Tables[0].Rows[0][0]);
                 

                //Condition
                string condition = " ";
                bool showAllrecord = true;
                if (ds.Tables[3].Rows.Count == 0)
                {
                    showAllrecord = false;
                }


                DataRow conditionRow = ds.Tables[2].Rows[0];

                if (Convert.ToBoolean(conditionRow["Userwise"]) && !showAllrecord)
                    condition = " usr.user_branchId in (" + Convert.ToString(Session["userbranchHierarchy"]) + ") ";

                if (Convert.ToString(conditionRow["DateFilterBy"]).Trim() != "")
                {
                    if (condition.Trim() != "")
                        condition = condition + " and ";

                    condition = condition + " [" + Convert.ToString(conditionRow["DateFilterBy"]) + "] >={0} and ";
                    condition = condition + " [" + Convert.ToString(conditionRow["DateFilterBy"]) + "] <={1} ";

                    ViewState["hasCondition"] = 1;
                }
                else {

                    dateTable.Style.Add("display", "none");

                }
                if (condition.Trim() != "")
                {
                    condition = " where " + condition;
                    SqlQuery = string.Format(SqlQuery, condition);
                }
                else {
                    SqlQuery = string.Format(SqlQuery, "");
                }


                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;


               
               // string SqlQuery = string.Format(Convert.ToString(ds.Tables[0].Rows[0][0]), Convert.ToString(Session["userid"]));

                ViewState["SqlQuery"] = SqlQuery;
                //hdSqlQuery.Value = 
                Grid.DataBind();


                int vissibleindex = 0;
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    Grid.Columns[Convert.ToString(dr["FieldName"])].VisibleIndex = vissibleindex;
                    vissibleindex++;
                }





                for (int i = 0; i < Grid.Columns.Count; i++)
                {
                    if (Grid.Columns[i].GetType().ToString() == "DevExpress.Web.GridViewDataDateColumn") 
                    {
                        ((GridViewDataDateColumn)Grid.Columns[i]).PropertiesDateEdit.DisplayFormatString = "dd/MM/yyyy";
                    }
                }

              



                Grid.Columns["id"].Visible = false;
                Grid.Columns["id"].ShowInCustomizationForm = false;
                GridViewDataHyperLinkColumn col;
                if (rights.CanEdit)
                {
                    col = new GridViewDataHyperLinkColumn();
                    col.Caption = "Edit";
                    col.PropertiesHyperLinkEdit.Text = "Edit";
                    col.PropertiesHyperLinkEdit.CssPostfix = " editClass";
                    Grid.Columns.Add(col);
                }

                if (rights.CanDelete)
                {
                    col = new GridViewDataHyperLinkColumn();
                    col.Caption = "Delete";
                    col.PropertiesHyperLinkEdit.Text = "Delete";
                    col.PropertiesHyperLinkEdit.CssPostfix = " DeleteClass";
                    Grid.Columns.Add(col);

                }

                if (rights.CanPrint)
                {
                    col = new GridViewDataHyperLinkColumn();
                    col.Caption = "Print";
                    col.PropertiesHyperLinkEdit.Text = "Print";
                    col.PropertiesHyperLinkEdit.CssPostfix = " PrintClass";
                    Grid.Columns.Add(col);

                }


            }

        }

        protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
        {
            string SqlQuery = Convert.ToString(ViewState["SqlQuery"]);

            if (Convert.ToString(ViewState["hasCondition"]) == "1")
            {
                SqlQuery = string.Format(SqlQuery, "'" + FormDate.Date.ToString("yyyy-MM-dd") + "'", "'" + toDate.Date.ToString("yyyy-MM-dd") + "'");
            }


            DataTable dt = oDBEngine.GetDataTable(SqlQuery);
            Grid.DataSource = dt;
             
        }


        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }

        public void bindexport(int Filter)
        {
            string filename = hdModuleName.Value;
            exporter.FileName = filename;
            exporter.Landscape = true;

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        [WebMethod]
        public static string DeleteDetails(string id,string modName)
        {
            string RetMsg = "-1~Error";
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_UserDefineForm");
                proc.AddVarcharPara("@Action", 100, "DeleteDetails");
                proc.AddPara("@ModName", modName);
                proc.AddPara("@id", id); 
                proc.AddVarcharPara("@outputMsg", 200, "", QueryParameterDirection.Output);
                proc.AddIntegerPara("@status", null, QueryParameterDirection.Output);
                proc.RunActionQuery();

                RetMsg = proc.GetParaValue("@status").ToString() + "~" + proc.GetParaValue("@outputMsg").ToString();
            }
            catch (Exception ex)
            {
                RetMsg = "-1~" + ex.Message;
            }

            return RetMsg;
        }

        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Grid.DataBind();
        }
         
    }

  

}