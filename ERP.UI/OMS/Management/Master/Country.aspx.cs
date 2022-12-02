using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.IO;
using EntityLayer.CommonELS;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_Country : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        //GenericMethod oGenericMethod;
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
               
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Country.aspx");
            
            GridCountry.JSProperties["cpEdit"] = null;
            GridCountry.JSProperties["cpinsert"] = null;
            GridCountry.JSProperties["cpUpdate"] = null;
            GridCountry.JSProperties["cpDelete"] = null;
            GridCountry.JSProperties["cpExists"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            BindCountryGrid();

            if(!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }


        protected void BindCountryGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM tbl_master_country order by cou_country");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                GridCountry.DataSource = dtFillGrid;
                GridCountry.DataBind();
            }
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            GridCountry.Settings.ShowFilterRow = true;
        }
        //protected void GridCountry_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType == GridViewRowType.Data)
        //    {
        //        int commandColumnIndex = -1;
        //        for (int i = 0; i < GridCountry.Columns.Count; i++)
        //            if (GridCountry.Columns[i] is GridViewCommandColumn)
        //            {
        //                commandColumnIndex = i;
        //                break;
        //            }
        //        if (commandColumnIndex == -1)
        //            return;
        //        //____One colum has been hided so index of command column will be leass by 1 
        //        commandColumnIndex = commandColumnIndex - 1;
        //        DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
        //        for (int i = 0; i < cell.Controls.Count; i++)
        //        {
        //            DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
        //            if (button == null) return;
        //            DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

        //            if (hyperlink.Text == "Delete")
        //            {
        //                if (Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "Delete" || Session["PageAccess"].ToString().Trim() == "All")
        //                {
        //                    hyperlink.Enabled = true;
        //                    continue;
        //                }
        //                else
        //                {
        //                    hyperlink.Enabled = false;
        //                    continue;
        //                }
        //            }


        //        }

        //    }

        //}
        protected void GridCountry_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridCountry.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridCountry.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void GridCountry_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            GridCountry.JSProperties["cpEdit"] = null;
            GridCountry.JSProperties["cpinsert"] = null;
            GridCountry.JSProperties["cpUpdate"] = null;
            GridCountry.JSProperties["cpDelete"] = null;
            GridCountry.JSProperties["cpExists"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridCountry.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridCountry.FilterExpression = string.Empty;
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable("select cou_id,cou_country,Country_NSECode,Country_BSECode,Country_MCXCode,Country_MCXSXCode,Country_NCDEXCode,Country_CDSLID,Country_NSDLID,Country_NdmlID,Country_DotExID,Country_CvlID from tbl_master_country where cou_id=" + WhichType + "");
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string c_cdsl = string.Empty;
                    string c_nsdl = string.Empty;
                    string c_ndml = string.Empty;
                    string c_dotex = string.Empty;
                    string c_cvl = string.Empty;
                    string country = Convert.ToString(dtEdit.Rows[0]["cou_country"]);
                    int cntryId = Convert.ToInt32(dtEdit.Rows[0]["cou_id"]);
                    string c_nsecode = Convert.ToString(dtEdit.Rows[0]["Country_NSECode"]);
                    string c_bsecode = Convert.ToString(dtEdit.Rows[0]["Country_BSECode"]);
                    string c_mcxcode = Convert.ToString(dtEdit.Rows[0]["Country_MCXCode"]);
                    string c_mcxsxcode = Convert.ToString(dtEdit.Rows[0]["Country_MCXSXCode"]);
                    string c_ncdexcode = Convert.ToString(dtEdit.Rows[0]["Country_NCDEXCode"]);
                    if (dtEdit.Rows[0]["Country_CDSLID"] != DBNull.Value)
                    {
                        c_cdsl = Convert.ToString(dtEdit.Rows[0]["Country_CDSLID"]);
                    }
                    else
                    {
                        c_cdsl = "";
                    }

                    if (dtEdit.Rows[0]["Country_NSDLID"] != DBNull.Value)
                    {
                        c_nsdl = Convert.ToString(dtEdit.Rows[0]["Country_NSDLID"]);
                    }
                    else
                    {
                        c_nsdl = "";
                    }

                    if (dtEdit.Rows[0]["Country_NdmlID"] != DBNull.Value)
                    {
                        c_ndml = Convert.ToString(dtEdit.Rows[0]["Country_NdmlID"]);
                    }
                    else
                    {
                        c_ndml = "";
                    }

                    if (dtEdit.Rows[0]["Country_DotExID"] != DBNull.Value)
                    {
                        c_dotex = Convert.ToString(dtEdit.Rows[0]["Country_DotExID"]);
                    }
                    else
                    {
                        c_dotex = "";
                    }

                    if (dtEdit.Rows[0]["Country_CvlID"] != DBNull.Value)
                    {
                        c_cvl = Convert.ToString(dtEdit.Rows[0]["Country_CvlID"]);
                    }
                    else
                    {
                        c_cvl = "";
                    }
                    GridCountry.JSProperties["cpEdit"] = country + "~" + cntryId + "~" + c_nsecode + "~" + c_bsecode + "~" + c_mcxcode + "~" + c_mcxsxcode + "~" + c_ncdexcode + "~" + c_cdsl + "~" + c_nsdl + "~" + c_ndml + "~" + c_dotex + "~" + c_cvl + "~" + WhichType;
                }
            }

            if (WhichCall == "updatecountry")
            {
                //updtcnt = oGenericMethod.Update_Table("tbl_master_country", "cou_country='" + txtCountryName.Text + "',Country_NSECode='" + txtNseCode.Text + "',Country_BSECode='" + txtBseCode.Text + "',Country_MCXCode='" + txtMcxCode.Text + "',Country_MCXSXCode='" + txtMcsxCode.Text + "',Country_NCDEXCode='" + txtNcdexCode.Text + "',Country_CDSLID=case when '" + txtCdslCode.Text + "'='' then null else '" + txtCdslCode.Text + "' end,Country_NSDLID=case when '" + txtNsdlCode.Text + "'='' then null else '" + txtNsdlCode.Text + "' end,Country_NdmlID=case when '" + txtNdmlCode.Text + "'='' then null else '" + txtNdmlCode.Text + "' end,Country_DotExID=case when '" + txtDotexidCode.Text + "'='' then null else '" + txtDotexidCode.Text + "' end,Country_CvlID=case when '" + txtCvlidCode.Text + "'='' then null else '" + txtCvlidCode.Text + "' end", "cou_id=" + WhichType + "");
                updtcnt = oGenericMethod.Update_Table("tbl_master_country", "cou_country='" + txtCountryName.Text + "'", "cou_id=" + WhichType + "");
                if (updtcnt > 0)
                {
                    GridCountry.JSProperties["cpUpdate"] = "Success";
                    BindCountryGrid();
                }
                else
                    GridCountry.JSProperties["cpUpdate"] = "fail";

            }
            if (WhichCall == "savecountry")
            {
                string[,] countrecord = oGenericMethod.GetFieldValue("tbl_master_country", "cou_country", "cou_country='" + txtCountryName.Text + "'", 1);
                if (countrecord[0, 0] != "n")
                {
                    GridCountry.JSProperties["cpExists"] = "Exists";
                }
                else
                {
                    //insertcount = oGenericMethod.Insert_Table("tbl_master_country", "cou_country,CreateDate,CreateUser,Country_NSECode,Country_BSECode,Country_MCXCode,Country_MCXSXCode,Country_NCDEXCode,Country_CDSLID,Country_NSDLID,Country_NdmlID,Country_DotExID,Country_CvlID",
                    //   "'" + txtCountryName.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"] + ",'" + txtNseCode.Text + "','" + txtBseCode.Text + "','" + txtMcxCode.Text + "','" + txtMcsxCode.Text + "','" + txtNcdexCode.Text + "',case when '" + txtCdslCode.Text + "'='' then null else '" + txtCdslCode.Text + "' end,case when '" + txtNsdlCode.Text + "'='' then null else '" + txtNsdlCode.Text + "' end,case when '" + txtNdmlCode.Text + "'='' then null else '" + txtNdmlCode.Text + "' end,case when '" + txtDotexidCode.Text + "'='' then null else '" + txtDotexidCode.Text + "' end,case when '" + txtCvlidCode.Text + "'='' then null else '" + txtCvlidCode.Text + "' end");
                    insertcount = oGenericMethod.Insert_Table("tbl_master_country", "cou_country,CreateDate,CreateUser",
                       "'" + txtCountryName.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"]);
                    if (insertcount > 0)
                    {
                        GridCountry.JSProperties["cpinsert"] = "Success";
                        BindCountryGrid();
                    }
                    else
                        GridCountry.JSProperties["cpinsert"] = "fail";
                }
            }

            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
                int i = masterdata.Deletecountry(Convert.ToString(id));
                if (i == 1)
                {
                    GridCountry.JSProperties["cpDelete"] = "Succesfully Deleted";
                }
                else
                {
                    GridCountry.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";

                }
                //below line comment by sanjib due to check other module 23122016

                //oGenericMethod = new BusinessLogicLayer.GenericMethod();
                //deletecont = oGenericMethod.Delete_Table("tbl_master_country", "cou_id = " + WhichType + "");
                //if (deletecont > 0)
                //{
                //    GridCountry.JSProperties["cpDelete"] = "Success";
                //    BindCountryGrid();
                //}
                //else
                //    GridCountry.JSProperties["cpDelete"] = "Fail";


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
            GridCountry.Columns[2].Visible = false;

            string filename = "Countries";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Countries";
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
