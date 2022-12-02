using System;
using System.Data;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class empState : System.Web.UI.Page
    {
        public string pageAccess = "";
        BusinessLogicLayer.GenericMethod oGenericMethod;
        // DBEngine oDBEngine = new DBEngine(string.Empty);

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            StateGrid.JSProperties["cpinsert"] = null;
            StateGrid.JSProperties["cpEdit"] = null;
            StateGrid.JSProperties["cpUpdate"] = null;
            StateGrid.JSProperties["cpDelete"] = null;
            StateGrid.JSProperties["cpExists"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            BindCombobox();
            BindGrid();
        }
        protected void BindCombobox()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select cou_id as id,cou_country as name from tbl_master_country");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbCountryName, dtCmb, "name", "id", 0);

        }
        protected void BindGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM tbl_master_state order by state");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                StateGrid.DataSource = dtFillGrid;
                StateGrid.DataBind();
            }
        }




        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
        protected void btnSearch(object sender, EventArgs e)
        {
            StateGrid.Settings.ShowFilterRow = true;
        }
        protected void StateGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < StateGrid.Columns.Count; i++)
                    if (StateGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 2;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "Delete" || Session["PageAccess"].ToString().Trim() == "All")
                        {
                            hyperlink.Enabled = true;
                            continue;
                        }
                        else
                        {
                            hyperlink.Enabled = false;
                            continue;
                        }
                    }


                }

            }

        }
        protected void StateGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!StateGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = StateGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void StateGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;
            StateGrid.JSProperties["cpinsert"] = null;
            StateGrid.JSProperties["cpEdit"] = null;
            StateGrid.JSProperties["cpUpdate"] = null;
            StateGrid.JSProperties["cpDelete"] = null;
            StateGrid.JSProperties["cpExists"] = null;
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            string WhichCall = e.Parameters.ToString().Split('~')[0];
            string WhichType = null;
            if (e.Parameters.ToString().Contains("~"))
                WhichType = e.Parameters.ToString().Split('~')[1];
            if (e.Parameters == "s")
                StateGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                StateGrid.FilterExpression = string.Empty;
            }

            if (WhichCall == "savestate")
            {
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string cdsl;
                if (txtCdslCode.Text == "")
                {
                    cdsl = "";
                }
                else
                {
                    cdsl = txtCdslCode.Text;
                }
                string CdslCode = null;
                if (txtCdslCode.Text != string.Empty)
                    CdslCode = txtCdslCode.Text;
                string[,] countrecord = oGenericMethod.GetFieldValue("tbl_master_state", "state", "state='" + txtStateName.Text + "'", 1);
                if (countrecord[0, 0] != "n")
                {
                    StateGrid.JSProperties["cpExists"] = "Exists";
                }
                else
                {
                    insertcount = oGenericMethod.Insert_Table("tbl_master_state", "state,countryId,CreateDate,CreateUser,State_NSECode,State_BSECode,State_MCXCode,State_MCXSXCode,State_NCDEXCode,State_CdslID,State_NsdlID,State_NDMLId,State_DotExID,State_CVLID",
                       "'" + txtStateName.Text + "','" + CmbCountryName.SelectedItem.Value + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + ",'" + txtNseCode.Text + "','" + txtBseCode.Text + "','" + txtMcxCode.Text + "','" + txtMcsxCode.Text + "','" + txtNcdexCode.Text + "',case when '" + CdslCode + "'='' then null else '" + CdslCode + "' end,case when '" + txtNsdlCode.Text + "'='' then null else '" + txtNsdlCode.Text + "' end,case when '" + txtNdmlCode.Text + "'='' then null else '" + txtNdmlCode.Text + "' end,case when '" + txtDotexidCode.Text + "'='' then null else '" + txtDotexidCode.Text + "' end,case when '" + txtCvlidCode.Text + "'='' then null else '" + txtCvlidCode.Text + "' end");
                    if (insertcount > 0)
                    {
                        StateGrid.JSProperties["cpinsert"] = "Success";
                        BindGrid();
                    }
                    else
                        StateGrid.JSProperties["cpinsert"] = "fail";

                }
            }

            if (WhichCall == "updatestate")
            {
                updtcnt = oGenericMethod.Update_Table("tbl_master_state", "State='" + txtStateName.Text + "',countryId='" + CmbCountryName.SelectedItem.Value + "',State_NSECode='" + txtNseCode.Text + "',State_BSECode='" + txtBseCode.Text + "',State_MCXCode='" + txtMcxCode.Text + "',State_MCXSXCode='" + txtMcsxCode.Text + "',State_NCDEXCode='" + txtNcdexCode.Text + "',State_CdslID=case when '" + txtCdslCode.Text + "'='' then null else '" + txtCdslCode.Text + "' end,State_NsdlID=case when '" + txtNsdlCode.Text + "'='' then null else '" + txtNsdlCode.Text + "' end,State_NDMLId=case when '" + txtNdmlCode.Text + "'='' then null else '" + txtNdmlCode.Text + "' end,State_DotExID=case when '" + txtDotexidCode.Text + "'='' then null else '" + txtDotexidCode.Text + "' end,State_CVLID=case when '" + txtCvlidCode.Text + "'='' then null else '" + txtCvlidCode.Text + "' end", "id=" + WhichType + "");
                if (updtcnt > 0)
                {
                    StateGrid.JSProperties["cpUpdate"] = "Success";
                    BindGrid();
                }
                else
                    StateGrid.JSProperties["cpUpdate"] = "fail";

            }
            if (WhichCall == "Delete")
            {
                deletecnt = oGenericMethod.Delete_Table("tbl_master_state", "id=" + WhichType + "");
                if (deletecnt > 0)
                {
                    StateGrid.JSProperties["cpDelete"] = "Success";
                    BindGrid();
                }
                else
                    StateGrid.JSProperties["cpDelete"] = "Fail";
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable("select state,countryId,State_NSECode,State_BSECode,State_MCXCode,State_MCXSXCode,State_NCDEXCode,State_CdslID,State_NsdlID,State_NDMLId,State_DotExID,State_CVLID from tbl_master_state where id=" + WhichType + "");
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string cdsl = string.Empty;
                    string nsdl = string.Empty;
                    string ndml = string.Empty;
                    string dotex = string.Empty;
                    string cvl = string.Empty;

                    string state = dtEdit.Rows[0]["state"].ToString();
                    int cntryId = Convert.ToInt32(dtEdit.Rows[0]["countryId"]);
                    string nsecode = dtEdit.Rows[0]["State_NSECode"].ToString();
                    string bsecode = dtEdit.Rows[0]["State_BSECode"].ToString();
                    string mcxcode = dtEdit.Rows[0]["State_MCXCode"].ToString();
                    string mcxsxcode = dtEdit.Rows[0]["State_MCXSXCode"].ToString();
                    string ncdexcode = dtEdit.Rows[0]["State_NCDEXCode"].ToString();
                    if (dtEdit.Rows[0]["State_CdslID"] != DBNull.Value)
                    {
                        cdsl = dtEdit.Rows[0]["State_CdslID"].ToString();
                    }
                    else
                    {
                        cdsl = "";
                    }

                    if (dtEdit.Rows[0]["State_NsdlID"] != DBNull.Value)
                    {
                        nsdl = dtEdit.Rows[0]["State_NsdlID"].ToString();
                    }
                    else
                    {
                        nsdl = "";
                    }

                    if (dtEdit.Rows[0]["State_NDMLId"] != DBNull.Value)
                    {
                        ndml = dtEdit.Rows[0]["State_NDMLId"].ToString();
                    }
                    else
                    {
                        ndml = "";
                    }

                    if (dtEdit.Rows[0]["State_DotExID"] != DBNull.Value)
                    {
                        dotex = dtEdit.Rows[0]["State_DotExID"].ToString();
                    }
                    else
                    {
                        dotex = "";
                    }

                    if (dtEdit.Rows[0]["State_CVLID"] != DBNull.Value)
                    {
                        cvl = dtEdit.Rows[0]["State_CVLID"].ToString();
                    }
                    else
                    {
                        cvl = "";
                    }
                    if (cvl == "0")
                        cvl = "";
                    if (dotex == "0")
                        dotex = "";
                    if (ndml == "0")
                        ndml = "";
                    if (nsdl == "0")
                        nsdl = "";
                    if (cdsl == "0")
                        cdsl = "";
                    StateGrid.JSProperties["cpEdit"] = state + "~" + cntryId + "~" + nsecode + "~" + bsecode + "~" + mcxcode + "~" + mcxsxcode + "~" + ncdexcode + "~" + cdsl + "~" + nsdl + "~" + ndml + "~" + dotex + "~" + cvl + "~" + WhichType;

                }
            }
        }
    }
}