using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.IO;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_empState : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public string pageAccess = "";
        //GenericMethod oGenericMethod;
        BusinessLogicLayer.GenericMethod oGenericMethod;
        // DBEngine oDBEngine = new DBEngine(string.Empty);

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/empState.aspx");
            
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
            if (!IsPostBack)
            {
                Session["exportval"] = null;

                BindCombobox();

            }
            BindGrid();
            //BindCombobox();
            //BindGrid();
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

            //...................................Code Added By Sam on 28092016.................................
            //dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM tbl_master_state order by state");

            dtFillGrid = oGenericMethod.GetDataTable(" SELECT tbl_master_state.*,dbo.tbl_master_country.cou_country as country FROM tbl_master_state join tbl_master_country on tbl_master_state.countryId=tbl_master_country.cou_id order by state");

            //...................................Code Above Added By Sam on 28092016.................................
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                StateGrid.DataSource = dtFillGrid;
                StateGrid.DataBind();
            }
        }




        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
        //    switch (Filter)
        //    {
        //        case 1:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;
        //    }
        //}
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
            StateGrid.Columns[3].Visible = false;

            string filename = "State";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "State";
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
                        if (Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd" || Convert.ToString(Session["PageAccess"]).Trim() == "Delete" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
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
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
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

            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                StateGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                StateGrid.FilterExpression = string.Empty;
            }

            if (WhichCall == "savestate")
            {
                string statecode = string.Empty;
                string stateuniquecode = string.Empty;
                //statecode = "999";
                //if (CmbCountryName.Value.ToString() != "1")
                //{
                   // statecode = txtstatecode.Text.Trim();
                //}
                if (txtstatecode.Text != "" && txtstatecode.Text != null)
                {
                    statecode = txtstatecode.Text.Trim();
                }
                if (txtstateuniquecode.Text != "" && txtstateuniquecode.Text != null)
                {
                    stateuniquecode = txtstateuniquecode.Text.Trim();
                }
                    
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string cdsl;
                string[,] countrecord = oGenericMethod.GetFieldValue("tbl_master_state", "state", "state='" + txtStateName.Text.Trim() + "'", 1);
                string[,] statechk = oGenericMethod.GetFieldValue("tbl_master_state", "StateCode", "StateCode='" + statecode + "'", 1);
                if (countrecord[0, 0] != "n" && statecode != "999")
                {
                    StateGrid.JSProperties["cpExists"] = "Exists";
                }
                else if (statecode.ToString() == "999" && CmbCountryName.Value.ToString() == "1")
                {
                   // StateGrid.JSProperties["cpExists"] = "Exists";
                    StateGrid.JSProperties["cpExistsinindia"] = "Exists";
                }
                //else if (statechk[0, 0] != "n" && statecode!="999" )
                //{
                //    StateGrid.JSProperties["cpExists"] = "Exists";
                //}
                else
                {
                    insertcount = oGenericMethod.Insert_Table("tbl_master_state", "state,StateCode,StateUniqueCode,countryId,CreateDate,CreateUser",
                       "'" + txtStateName.Text.Trim() + "','" + statecode + "','" + stateuniquecode + "','" + CmbCountryName.SelectedItem.Value + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + "");
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
                string statecode = string.Empty;
                string stateuniquecode = string.Empty;
                //statecode = "999";
                //if (CmbCountryName.Value.ToString() != "1")
                //{
                   // statecode = txtstatecode.Text.Trim();
                //}
                if (txtstatecode.Text != "" && txtstatecode.Text != null)
                {
                    statecode = txtstatecode.Text.Trim();
                }
                if (txtstateuniquecode.Text != "" && txtstateuniquecode.Text != null)
                {
                    stateuniquecode = txtstateuniquecode.Text.Trim();
                }
                string[,] statechk = oGenericMethod.GetFieldValue("tbl_master_state", "StateCode", "StateCode='" + statecode + "'", 1);
                if (statechk[0, 0] != "n" && statecode != "999")
                {
                    StateGrid.JSProperties["cpExists"] = "Exists";
                }
                else if (statecode.ToString() == "999" && CmbCountryName.Value.ToString() == "1")
                {
                    //StateGrid.JSProperties["cpExists"] = "Exists";
                    StateGrid.JSProperties["cpExistsinindia"] = "Exists";
                }
                else
                {
                    updtcnt = oGenericMethod.Update_Table("tbl_master_state", "State='" + txtStateName.Text.Trim() + "',StateCode='" + statecode + "',StateUniqueCode='" + stateuniquecode + "',countryId='" + CmbCountryName.SelectedItem.Value + "'", "id=" + WhichType + "");
                if (updtcnt > 0)
                {
                    StateGrid.JSProperties["cpUpdate"] = "Success";
                    BindGrid();
                }
                else
                { 
                    StateGrid.JSProperties["cpUpdate"] = "fail";
                }
                }

            }
            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
                int i = masterdata.DeleteState(Convert.ToString(id));
                if (i == 1)
                {
                    StateGrid.JSProperties["cpDelete"] = "Succesfully Deleted";
                }
                else
                {
                    StateGrid.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";
                    
                }
                //below line comment by sanjib due to check other module 23122016

                //deletecnt = oGenericMethod.Delete_Table("tbl_master_state", "id=" + WhichType + "");
                //if (deletecnt > 0)
                //{
                //    StateGrid.JSProperties["cpDelete"] = "Success";
                //    BindGrid();
                //}
                //else
                //    StateGrid.JSProperties["cpDelete"] = "Fail";
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable("select state,countryId,State_NSECode,State_BSECode,State_MCXCode,State_MCXSXCode,State_NCDEXCode,State_CdslID,State_NsdlID,State_NDMLId,State_DotExID,State_CVLID,StateCode,StateUniqueCode from tbl_master_state where id=" + WhichType + "");
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string cdsl = string.Empty;
                    string nsdl = string.Empty;
                    string ndml = string.Empty;
                    string dotex = string.Empty;
                    string cvl = string.Empty;

                    string state = Convert.ToString(dtEdit.Rows[0]["state"]);
                    int cntryId = Convert.ToInt32(dtEdit.Rows[0]["countryId"]);
                    string nsecode = Convert.ToString(dtEdit.Rows[0]["State_NSECode"]);
                    string bsecode = Convert.ToString(dtEdit.Rows[0]["State_BSECode"]);
                    string mcxcode = Convert.ToString(dtEdit.Rows[0]["State_MCXCode"]);
                    string mcxsxcode = Convert.ToString(dtEdit.Rows[0]["State_MCXSXCode"]);
                    string ncdexcode = Convert.ToString(dtEdit.Rows[0]["State_NCDEXCode"]);
                    if (dtEdit.Rows[0]["State_CdslID"] != DBNull.Value)
                    {
                        cdsl = Convert.ToString(dtEdit.Rows[0]["State_CdslID"]);
                    }
                    else
                    {
                        cdsl = "";
                    }

                    if (dtEdit.Rows[0]["State_NsdlID"] != DBNull.Value)
                    {
                        nsdl = Convert.ToString(dtEdit.Rows[0]["State_NsdlID"]);
                    }
                    else
                    {
                        nsdl = "";
                    }

                    if (dtEdit.Rows[0]["State_NDMLId"] != DBNull.Value)
                    {
                        ndml = Convert.ToString(dtEdit.Rows[0]["State_NDMLId"]);
                    }
                    else
                    {
                        ndml = "";
                    }

                    if (dtEdit.Rows[0]["State_DotExID"] != DBNull.Value)
                    {
                        dotex = Convert.ToString(dtEdit.Rows[0]["State_DotExID"]);
                    }
                    else
                    {
                        dotex = "";
                    }

                    if (dtEdit.Rows[0]["State_CVLID"] != DBNull.Value)
                    {
                        cvl = Convert.ToString(dtEdit.Rows[0]["State_CVLID"]);
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
                    string statecode = string.Empty;
                    string stateuniquecode = string.Empty;
                    if (dtEdit.Rows[0]["StateCode"] != DBNull.Value)
                    {
                        statecode = Convert.ToString(dtEdit.Rows[0]["StateCode"]);
                    }
                    if (dtEdit.Rows[0]["StateUniqueCode"] != DBNull.Value)
                    {
                        stateuniquecode = Convert.ToString(dtEdit.Rows[0]["StateUniqueCode"]);
                    }
                    StateGrid.JSProperties["cpEdit"] = state + "~" + cntryId + "~" + nsecode + "~" + bsecode + "~" + mcxcode + "~" + mcxsxcode + "~" + ncdexcode + "~" + cdsl + "~" + nsdl + "~" + ndml + "~" + dotex + "~" + cvl + "~" + WhichType + "~" + statecode + "~" + stateuniquecode;

                }
            }
        }
    }
}
