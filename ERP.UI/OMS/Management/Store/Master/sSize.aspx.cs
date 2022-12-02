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
using System.Web.Services;
using System.Xml.Linq;
using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
//////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;

namespace ERP.OMS.Management.Store.Master
{
    public partial class Management_Store_Master_sSize : System.Web.UI.Page
    {
        public string pageAccess = "";
        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        string[] lengthIndex;
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath =Convert.ToString( HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Store/Master/sSize.aspx");
            ///////////////////////////
            Session["requesttype"] = "Product";
            Session["ContactType"] = "Product";
            Session["KeyVal_InternalID"] = "";
            Session["Name"] = "";
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            ///////////////////////////
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;
            Session["ids"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                BindUOM();
            }
            BindGrid();


            // BindGridDet();
            //UOMBind();
            BindGrid2();
        }





        protected void BindGrid()
        {

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtFillGrid = new DataTable();
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            dtFillGrid = oStore_MasterBL.GetSizeList();
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                cityGrid.DataSource = dtFillGrid;
                cityGrid.DataBind();
            }

        }

        public void bindexport(int Filter)
        {
            cityGrid.Columns[3].Visible = false;
           
            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = "Size/Strength Schemes";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Size/Strength Schemes";
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

        protected void btnSearch(object sender, EventArgs e)
        {
            cityGrid.Settings.ShowFilterRow = true;
        }

        protected void cityGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < cityGrid.Columns.Count; i++)
                    if (cityGrid.Columns[i] is GridViewCommandColumn)
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

        protected void cityGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!cityGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = cityGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }

        protected void cityGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;

            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            hiddenedit.Value = WhichType;

            if (e.Parameters == "s")
                cityGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
                cityGrid.FilterExpression = string.Empty;

            if (WhichCall == "savecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();


                //int cityID = 0;
                //if (CmbCity.Items.Count > 0)
                //    if (CmbCity.SelectedItem != null)
                //        cityID = Convert.ToInt32(CmbCity.SelectedItem.Value.ToString());
                //if (cityID != 0)
                //{

                string[,] countrecord = oGenericMethod.GetFieldValue("Master_Size", "Size_Name", "Size_Name='" + txtSize_Name.Text.Trim() + "'", 1);

                if (countrecord[0, 0] != "n")
                    cityGrid.JSProperties["cpExists"] = "Exists";
                else
                {
                    insertcount = oStore_MasterBL.InsertSize(txtSize_Name.Text.Trim(), txtSize_Description.Text.Trim(), Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    DataTable odt = oGenericMethod.GetDataTable("SELECT TOP 1 Size_ID FROM Master_Size ORDER BY Size_ID DESC");
                    if (odt.Rows.Count > 0)
                    {
                        Session["ids"] = Convert.ToString(odt.Rows[0]["Size_ID"]);
                    }
                    if (insertcount > 0)
                    {
                        //   hiddenedit.Value = Convert.ToString(odt.Rows[0]["Size_ID"]);
                        //cityGrid.JSProperties["cpinsert"] = "Success";
                        cityGrid.JSProperties["cpinsert"] = "Success" + "~" + Convert.ToString(odt.Rows[0]["Size_ID"]);
                        BindGrid();
                    }
                    else
                        cityGrid.JSProperties["cpinsert"] = "fail";
                }
                //}
                //else
                //    cityGrid.JSProperties["cpUpdateValid"] = "StateInvalid";
            }
            if (WhichCall == "updatecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                updtcnt = oStore_MasterBL.UpdateSize(Convert.ToInt32(WhichType), txtSize_Name.Text.Trim(), txtSize_Description.Text.Trim(), Convert.ToInt32(HttpContext.Current.Session["userid"]));
                if (updtcnt > 0)
                {
                    cityGrid.JSProperties["cpUpdate"] = "Success";
                    BindGrid();
                }
                else
                    cityGrid.JSProperties["cpUpdate"] = "fail";



            }
            if (WhichCall == "Delete")
            {
                try
                {
                    string[] CallVal = Convert.ToString(e.Parameters).Split('~');
                    lengthIndex = e.Parameters.Split('~');
                    if (Convert.ToString(lengthIndex[0]) == "Delete")
                    {
                        string SizeId = Convert.ToString(Convert.ToString(CallVal[1]));
                        int retValue = masterChecking.DeleteMastersize(Convert.ToInt32(SizeId));
                        if (retValue > 0)
                        {
                            Session["KeyVal"] = "Succesfully Deleted";
                            cityGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                            BindGrid();
                        }
                        else
                        {
                            Session["KeyVal"] = "Used in other modules. Cannot Delete.";
                            cityGrid.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";

                        }

                    }
                    //deletecnt = oGenericMethod.Delete_Table("Master_Size", "Size_ID=" + WhichType + "");

                    //if (deletecnt > 0)
                    //{
                    //    deletecnt = oGenericMethod.Delete_Table("Master_SizeDetail", "SizeDetail_MainID=" + WhichType + "");
                    //    cityGrid.JSProperties["cpDelete"] = "Success";

                    //    BindGrid();
                    //}
                    //else
                    //    cityGrid.JSProperties["cpDelete"] = "Fail";
                }
                catch (Exception ex)
                {

                    cityGrid.JSProperties["cpDelete"] = "FK";
                    cityGrid.JSProperties["cpMsg"] = Convert.ToString(ConfigurationManager.AppSettings["DeleteErrorMessage"]);
                }
            }
            if (WhichCall == "Redirect")
            {
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(Request.RawUrl);
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oStore_MasterBL.GetSizeDetails(Convert.ToInt32(WhichType));

                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string Size_Name = Convert.ToString(dtEdit.Rows[0]["Size_Name"]);
                    string Size_Description =Convert.ToString( dtEdit.Rows[0]["Size_Description"]);
                    //string Taxes_Description = dtEdit.Rows[0]["Taxes_Description"].ToString();

                    //string Taxes_ApplicableFor = dtEdit.Rows[0]["Taxes_ApplicableFor"].ToString();
                    //string Taxes_ApplicableOn = dtEdit.Rows[0]["Taxes_ApplicableOn"].ToString();
                    //string Taxes_OtherTax = dtEdit.Rows[0]["Taxes_OtherTax"].ToString();
                    //string OtherTax = dtEdit.Rows[0]["OtherTax"].ToString();

                    cityGrid.JSProperties["cpEdit"] = Size_Name + "~" + Size_Description + "~" + WhichType;

                    // BindGridDet();

                }
                BindGrid2();
                //markets.SelectParameters.Add("SizeDetail_MainID", WhichType);
            }


        }


        #region Inner grid

        protected void BindUOM()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("SELECT [UOM_ID],[UOM_Name] FROM Master_UOM");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbSizeDetail_UOM, dtCmb, "UOM_Name", "UOM_ID", 0);

            CmbSizeDetail_UOM.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));

        }


        protected void BindGrid2()
        {
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            string id = Convert.ToString(hiddenedit.Value).Trim() != "" ? Convert.ToString(hiddenedit.Value).Trim() : "0";
            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            if (id != "0")
            {
                DataTable dtFillGrid = new DataTable();
                dtFillGrid = oStore_MasterBL.GetSizeUOMMapDetails(Convert.ToInt32(id));
                AspxHelper oAspxHelper = new AspxHelper();
                if (dtFillGrid.Rows.Count > 0)
                {
                    cityGrid2.DataSource = dtFillGrid;
                    cityGrid2.DataBind();
                }

                else
                {
                    cityGrid2.DataSource = null;
                    cityGrid2.DataBind();
                }
            }
        }

        protected void cityGrid_HtmlRowCreated2(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < cityGrid2.Columns.Count; i++)
                    if (cityGrid2.Columns[i] is GridViewCommandColumn)
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

        protected void cityGrid_HtmlEditFormCreated2(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!cityGrid2.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = cityGrid2.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }

        protected void cityGrid_CustomCallback2(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            cityGrid2.JSProperties["cpinsert"] = null;
            cityGrid2.JSProperties["cpEdit"] = null;
            cityGrid2.JSProperties["cpUpdate"] = null;
            cityGrid2.JSProperties["cpDelete"] = null;
            cityGrid2.JSProperties["cpExists"] = null;
            cityGrid2.JSProperties["cpUpdateValid"] = null;
            cityGrid2.JSProperties["cpinnergrid"] = null;

            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

            if (e.Parameters == "s")
                cityGrid2.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
                cityGrid2.FilterExpression = string.Empty;

            if (WhichCall == "savecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                insertcount = oStore_MasterBL.InsertSizeDetails(Convert.ToInt32(hiddenedit.Value.Trim()), txtSizeDetail_AttribName.Text.Trim(), txtSizeDetail_Value.Text.Trim(), Convert.ToInt32(CmbSizeDetail_UOM.SelectedItem.Value), Convert.ToInt32(HttpContext.Current.Session["userid"]));
                if (insertcount > 0)
                {
                    cityGrid2.JSProperties["cpinsert"] = "Success";
                }
                else
                    cityGrid2.JSProperties["cpinsert"] = "fail";
            }
            if (WhichCall == "updatecity")
            {
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                updtcnt = oStore_MasterBL.UpdateSizeDetails(Convert.ToInt32(WhichType), txtSizeDetail_AttribName.Text.Trim(), txtSizeDetail_Value.Text.Trim(), Convert.ToInt32(CmbSizeDetail_UOM.SelectedItem.Value), Convert.ToInt32(HttpContext.Current.Session["userid"]));
                if (updtcnt > 0)
                {
                    cityGrid2.JSProperties["cpUpdate"] = "Success";

                }
                else
                    cityGrid2.JSProperties["cpUpdate"] = "fail";
            }
            if (WhichCall == "Delete")
            {
                deletecnt = oGenericMethod.Delete_Table("Master_SizeDetail", "SizeDetail_ID=" + WhichType + "");
                if (deletecnt > 0)
                {
                    cityGrid2.JSProperties["cpDelete"] = "Success";
                }
                else
                    cityGrid2.JSProperties["cpDelete"] = "Fail";
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oStore_MasterBL.GetSizeDetailsList(Convert.ToInt32(WhichType));
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string SizeDetail_AttribName = Convert.ToString(dtEdit.Rows[0]["SizeDetail_AttribName"]);
                    string SizeDetail_Value = Convert.ToString(dtEdit.Rows[0]["SizeDetail_Value"]);
                    string SizeDetail_UOM = Convert.ToString(dtEdit.Rows[0]["SizeDetail_UOM"]);
                    cityGrid2.JSProperties["cpEdit"] = SizeDetail_AttribName + "~" + SizeDetail_Value + "~" + SizeDetail_UOM + "~" + WhichType;
                }
            }

        }


        #endregion


        ////////////////////////////////////////////////////////

        //protected void marketsGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        //{
        //    string vai = e.KeyValue.ToString();
        //    if (e.RowType == GridViewRowType.Data)
        //    {            
        //        int commandColumnIndex = -1;
        //        for (int i = 0; i < marketsGrid.Columns.Count; i++)
        //            if (marketsGrid.Columns[i] is GridViewCommandColumn)
        //            {
        //                commandColumnIndex = i;
        //                break;
        //            }
        //        if (commandColumnIndex == -1)
        //            return;
        //        //____One colum has been hided so index of command column will be leass by 1 
        //        commandColumnIndex = commandColumnIndex - 5;
        //        DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
        //        for (int i = 0; i < cell.Controls.Count; i++)
        //        {
        //            DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
        //            if (button == null) return;
        //            DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

        //            if (hyperlink.Text == "Delete")
        //            {
        //                if (Session["PageAccess"].ToString() == "DelAdd" || Session["PageAccess"].ToString() == "Delete" || Session["PageAccess"].ToString() == "All")
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
        //protected void marketsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        //{



        //    if (!marketsGrid.IsNewRowEditing)
        //    {
        //        ASPxGridViewTemplateReplacement RT = marketsGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
        //        if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
        //            RT.Visible = true;
        //        else
        //            RT.Visible = false;
        //    }

        //}
        //protected void marketsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    if (e.Parameters == "s")
        //        marketsGrid.Settings.ShowFilterRow = true;

        //    if (e.Parameters == "All")
        //    {
        //        marketsGrid.FilterExpression = string.Empty;
        //    }
        //}

        //protected void marketsGrid_OnRowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e) 
        //{

        //    GridViewDataComboBoxColumn column = (GridViewDataComboBoxColumn)marketsGrid.Columns["SizeDetail_UOM"]; 
        //    //BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        //    //DataTable dtCmb = new DataTable();
        //    //dtCmb = oGenericMethod.GetDataTable("select cou_id as id,cou_country as name from tbl_master_country order By cou_country");
        //    //AspxHelper oAspxHelper = new AspxHelper();
        //    ////if (dtCmb.Rows.Count > 0)
        //    ////oAspxHelper.Bind_Combo(CmbCountryName, dtCmb, "name", "id", 0);

        //    ////CmbCountryName.Items.Insert(0, new DevExpress.Web.ListEditItem("Any", "0"));
        //}

        //private void UOMBind() 
        //{
        //    GridViewDataComboBoxColumn column = (GridViewDataComboBoxColumn)marketsGrid.Columns["SizeDetail_UOM"];
        //    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        //    DataTable dtCmb = new DataTable();
        //    dtCmb = oGenericMethod.GetDataTable("SELECT [UOM_ID],[UOM_Name] FROM Master_UOM");
        //    ////    foreach (DataRow item in dtCmb.Rows)
        //    ////{
        //    ////    column.PropertiesComboBox.Items.Add(item["UOM_Name"],item["UOM_ID"]);


        //    ////}
        //    column.PropertiesComboBox.DataSource = dtCmb;
        //    column.PropertiesComboBox.ValueField = dtCmb.Rows[0]["UOM_ID"].ToString();// "UOM_ID";       
        //    column.PropertiesComboBox.ValueType = typeof(int);
        //    column.PropertiesComboBox.TextField = dtCmb.Rows[0]["UOM_Name"].ToString();
        //}


        //////////////////////////////////////

        //protected void marketsGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        //{
        //    int visibleIndex = marketsGrid.FindVisibleIndexByKeyValue(e.Keys[marketsGrid.KeyFieldName]); 
        //    GridViewDataTextColumn column = (GridViewDataTextColumn)marketsGrid.Columns["SizeDetail_ID"];
        //    string id = Convert.ToString(e.Keys[visibleIndex].ToString());

        //    int deletecnt = 0;

        //    deletecnt = oGenericMethod.Delete_Table("Master_SizeDetail", "SizeDetail_ID=" + id + "");

        //    if (deletecnt > 0)
        //    {
        //        cityGrid.JSProperties["cpDelete"] = "Success";
        //         BindGridDet();
        //    }
        //    else
        //        cityGrid.JSProperties["cpDelete"] = "Fail";

        //}
        protected void ASPxPopupControl1_Init(object sender, EventArgs e)
        {
            hiddenedit.Value = (string)Session["ids"];
        }
        protected void ASPxPopupControl1_Load(object sender, EventArgs e)
        {
            hiddenedit.Value = (string)Session["ids"];
        }
        protected void Popup_Empcitys_Onload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["ids"])))
            {
                hiddenedit.Value = (string)Session["ids"];
            }
        }
        [WebMethod]
        public static string GetSizeId()
        {
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            DataTable odt = oGeneric.GetDataTable("SELECT TOP 1 Size_ID FROM Master_Size ORDER BY Size_ID DESC");
            string id = string.Empty;
            if (odt.Rows.Count > 0)
            {
                id = Convert.ToString(odt.Rows[0]["Size_ID"]);
            }
            return id;
        }
        [WebMethod]
        public static bool CheckUniqueName(string SizeName, int sizeid)
        {
            bool IsPresent = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                DataTable dt = new DataTable();
                if (sizeid==0)
                {
                    dt = oGenericMethod.GetDataTable(" SELECT COUNT(Size_Name) AS Size_Name FROM Master_Size WHERE Size_Name = '" + SizeName + "'");
                }
                else
                {
                    dt = oGenericMethod.GetDataTable(" SELECT COUNT(Size_Name) AS Size_Name FROM Master_Size WHERE Size_Name = '" + SizeName + "' and Size_ID!=" + sizeid + "");
                }
                //dt = oGenericMethod.GetDataTable(" SELECT COUNT(Size_Name) AS Size_Name FROM Master_Size WHERE Size_Name = '" + SizeName + "'");
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["Size_Name"]) > 0)
                    {
                        IsPresent = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return IsPresent;
        }
    }
}