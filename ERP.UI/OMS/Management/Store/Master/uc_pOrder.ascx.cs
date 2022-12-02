using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
////using DevExpress.Web.ASPxClasses;
using DevExpress.Web;
using System.Web.Services;
using System.Text;
using System.IO;
using System.Data.SqlClient;
//using DevExpress.Web;
using System.Resources;
using System.Collections;
using System.IO;
using BusinessLogicLayer;


public partial class uc_pOrder : System.Web.UI.UserControl
{
    DataTable dtXML = new DataTable();
    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
    bool isNotEditables = false;
    string EditIds = string.Empty;
    public string PXMLPATH
    {
        get { return (string)Session["CashBankVoucherFile_XMLPATH"]; }
        set { Session["CashBankVoucherFile_XMLPATH"] = value; }
    }
    public int PCounter
    {
        get { return (int)ViewState["Counter"]; }
        set { ViewState["Counter"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //BusinessLogicLayer.DBEngine objDB = new BusinessLogicLayer.DBEngine();  
        #region check Edit path from directory if file exist with is user ID
        PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_E");
        DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/OMS/Management/Documents/"));//Assuming Documents is your Folder
        FileInfo[] Files = d.GetFiles();

        bool isNotEditable = false;
        string session = Convert.ToString(Session["userid"]).Trim();
        foreach (var item in Files)
        {
            try
            {
                string[] Fnames = item.ToString().Split('_');
                string Uid = Fnames[1];
                string Pid = Fnames[2];
                string Mode = Fnames[3];
                //check with condition
                if (Uid.Equals(session) && Mode.Equals("E") && !isNotEditable)
                {
                    EditIds = Pid;
                    isNotEditables = true;
                    PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");

                }
            }
            catch { }
        }
        #endregion

        //accident
        if (!File.Exists(PXMLPATH))
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");

        }
        CurrencyBind();
        if (!IsPostBack)
        {
            BindGrid();
            BindSize();
            BindColor();
            BindYear(); 
            // load create file at page load
            if (IsFileExistance(PXMLPATH) == true)
            {
                DataSet loadDataset = new DataSet();
                PageloadGridBind();
                loadDataset.ReadXml(PXMLPATH);
                _pOderTableBind(loadDataset);

                if (!isNotEditable && EditIds.Equals(""))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", "<script> alert('You have existing create file');$(document).ready(function() {cPopup_IsEdit.Hide();});</script>", false);
                }
                // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", "<script>alert('You have an existing Create form.'); $(document).ready(function() {cPopup_IsEdit.Hide();});</script>", false);
            }

            if (isNotEditables)
            {
                if (Request.QueryString["ID"] == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", "<script>alert('You have an existing edit form.'); $(document).ready(function() {cPopup_IsEdit.Hide();});</script>", false);
                }
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");


            }
        }



        //new code block for showing key from resource page start

        //if (File.Exists(Server.MapPath("~/OMS/Management/DailyTask/ResourceFiles/CreateOrdersUC.resx")))
        //{
        //    ResourceReader resReader = new ResourceReader(Server.MapPath("~/OMS/Management/DailyTask/ResourceFiles/CreateOrdersUC.resx"));

        //    foreach (DictionaryEntry da in resReader)
        //    {
        //        Label currLBL = new Label();
        //        currLBL = (Label)Popup_Empcitys.FindControl(da.Key.ToString());

        //        //if (currLBL == null) { currLBL = (Label)FindControl(da.Key.ToString()); }

        //        currLBL.Text = da.Value.ToString();
        //    }

        //    resReader.Close();
        //}

        //new code block for showing key from resource page end


    }

    protected void BindGrid()
    {


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

    protected void cityGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
    {
        if (!cityGrid.IsNewRowEditing)
        {
            ASPxGridViewTemplateReplacement RT = cityGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
            if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                RT.Visible = true;
            else
                RT.Visible = false;
        }

    }

    protected void cityGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        cityGrid.JSProperties["cpinsert"] = null;
        cityGrid.JSProperties["cpEdit"] = null;
        cityGrid.JSProperties["cpUpdate"] = null;
        cityGrid.JSProperties["cpDelete"] = null;
        cityGrid.JSProperties["cpExists"] = null;
        cityGrid.JSProperties["cpUpdateValid"] = null;

        int insertcount = 0;
        int updtcnt = 0;
        int deletecnt = 0;

        // oGenericMethod = new GenericMethod();
        oGenericMethod = new BusinessLogicLayer.GenericMethod();

        string WhichCall = e.Parameters.ToString().Split('~')[0];
        string WhichType = null;
        if (e.Parameters.ToString().Contains("~"))
            if (e.Parameters.ToString().Split('~')[1] != "")
                WhichType = e.Parameters.ToString().Split('~')[1];

        if (e.Parameters == "s")
            cityGrid.Settings.ShowFilterRow = true;
        if (e.Parameters == "All")
            cityGrid.FilterExpression = string.Empty;

        if (WhichCall == "savecity")
        {
            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();


            int cityID = 0;
            //if (CmbCity.Items.Count > 0)
            //    if (CmbCity.SelectedItem != null)
            //        cityID = Convert.ToInt32(CmbCity.SelectedItem.Value.ToString());
            if (cityID != 0)
            {

                //string[,] countrecord = oGenericMethod.GetFieldValue("Master_Markets", "Markets_Code", "Markets_Code='" + txtMarkets_Code.Text + "'", 1);

                //if (countrecord[0, 0] != "n")
                //    cityGrid.JSProperties["cpExists"] = "Exists";
                //else
                //{
                //    //insertcount = oGenericMethod.Insert_Table("Master_Markets", "Markets_Code,Markets_Country,Markets_State,Markets_City,Markets_Name,Markets_Description,Markets_Address,Markets_Email,Markets_Phones,Markets_WebSite,Markets_ContactPerson,Markets_CreateUser,Markets_CreateTime",
                //    //   "'" + txtMarkets_Code.Text + "','" + CmbCountryName.SelectedItem.Value + "','" + CmbState.SelectedItem.Value + "','" + CmbCity.SelectedItem.Value + "','"
                //    //   + txtMarkets_Name.Text + "','" + txtMarkets_Description.Text + "','" + txtMarkets_Address.Text + "','" + txtMarkets_Email.Text + "','" + txtMarkets_Phones.Text + "','" + txtMarkets_WebSite.Text + "','" + txtMarkets_ContactPerson.Text + "','" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "',getdate()");

                //    //if (insertcount > 0)
                //    //{
                //    //    cityGrid.JSProperties["cpinsert"] = "Success";
                //    //    BindGrid();
                //    //}
                //    //else
                //    //    cityGrid.JSProperties["cpinsert"] = "fail";
                //}
            }
            else
                cityGrid.JSProperties["cpUpdateValid"] = "StateInvalid";
        }
        if (WhichCall == "updatecity")
        {
            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            int stateID = 0;
            //if (CmbState.Items.Count > 0)
            //    if (CmbState.SelectedItem != null)
            //        stateID = Convert.ToInt32(CmbState.SelectedItem.Value.ToString());

            //if (stateID != 0)
            //{
            //    updtcnt = oGenericMethod.Update_Table("Master_Markets", "Markets_Code='" + txtMarkets_Code.Text + "',Markets_Country='" + CmbCountryName.SelectedItem.Value.ToString() + "',Markets_State='" + CmbState.SelectedItem.Value.ToString() + "',Markets_City='" + CmbCity.SelectedItem.Value.ToString() + "',Markets_Name='" + txtMarkets_Name.Text + "',Markets_Description='" + txtMarkets_Description.Text + "',Markets_Address='" + txtMarkets_Address.Text + "',Markets_Email='" + txtMarkets_Email.Text + "',Markets_Phones='" + txtMarkets_Phones.Text + "',Markets_WebSite='" + txtMarkets_WebSite.Text + "',Markets_ContactPerson='" + txtMarkets_ContactPerson.Text + "',Markets_ModifyUser='" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "',Markets_ModifyTime=getdate()", "Markets_ID=" + WhichType + "");
            //    if (updtcnt > 0)
            //    {
            //        cityGrid.JSProperties["cpUpdate"] = "Success";
            //        BindGrid();
            //    }
            //    else
            //        cityGrid.JSProperties["cpUpdate"] = "fail";
            //}
            //else
            //    cityGrid.JSProperties["cpUpdateValid"] = "StateInvalid";


        }
        if (WhichCall == "Delete")
        {

            int count = 0;
            DataSet ds = new DataSet();
            string EditVal = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                EditVal = Convert.ToString(Request.QueryString["ID"]);
            }
            if (EditVal.Equals("") && !isNotEditables)
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
            }
            else if (isNotEditables)
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");

            }
            else
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");

            }
            if (File.Exists(PXMLPATH))
            {
                ds.ReadXml(PXMLPATH);
                if (ds.Tables.Count > 1)
                {
                    DataTable dt = ds.Tables[1];
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (Convert.ToString(dt.Rows[i]["RecordID"]) == WhichType)
                        {
                            dt.Rows[i].Delete();
                            count = count + 1;
                        }
                    }
                    ds.WriteXml(PXMLPATH);
                    ds.Dispose();
                }
                if (count > 0)
                {
                    cityGrid.JSProperties["cpDelete"] = "Success";
                    PageloadGridBind();
                }
                else
                {
                    cityGrid.JSProperties["cpDelete"] = "Fail";
                }
            }

            else
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
                if (File.Exists(PXMLPATH))
                {
                    ds.ReadXml(PXMLPATH);
                    if (ds.Tables.Count > 1)
                    {
                        DataTable dt = ds.Tables[1];
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (Convert.ToString(dt.Rows[i]["RecordID"]) == WhichType)
                            {
                                dt.Rows[i].Delete();
                                count = count + 1;
                            }
                        }
                        ds.WriteXml(PXMLPATH);
                        ds.Dispose();
                    }
                    if (count > 0)
                    {
                        cityGrid.JSProperties["cpDelete"] = "Success";
                        PageloadGridBind();
                    }
                    else
                    {
                        cityGrid.JSProperties["cpDelete"] = "Fail";
                        PageloadGridBind();
                    }
                }
            }

        }
        if (WhichCall == "Edit")
        {

            DataSet XMLDataSet = new DataSet();
            DataTable dtEditXML = null;
            DataRow drEditXML = null;
            string EditVal = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                EditVal = Convert.ToString(Request.QueryString["ID"]);
            }
            if (EditVal.Equals("") && !isNotEditables)
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
            }
            else if (isNotEditables)
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
            }
            else
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
            }
            XMLDataSet.ReadXml(PXMLPATH);
            if (XMLDataSet.Tables.Count > 1)
            {

                dtEditXML = XMLDataSet.Tables[1];
                for (int i = 0; i < dtEditXML.Rows.Count; i++)
                {
                    if (Convert.ToString(dtEditXML.Rows[i]["RecordID"]) == WhichType)
                    {
                        drEditXML = dtEditXML.Rows[i];

                    }
                }

            }
            var ItemArray = drEditXML.ItemArray;
            string ProductReferenceNo = Convert.ToString(Session["_RefNumber"]); 
            string pOrderDetail_OrderID = Convert.ToString(ItemArray[0]);
            string pOrderDetail_Number = Convert.ToString(ItemArray[1]);
            string pOrderDetail_ProductID = Convert.ToString(ItemArray[2]);
            string pOrderDetail_Brand = Convert.ToString(ItemArray[3]) != "NULL" ? Convert.ToString(ItemArray[3]) : string.Empty;
            string pOrderDetail_Size = Convert.ToString(ItemArray[6]) != "NULL" ? Convert.ToString(ItemArray[6]) : string.Empty;
            string pOrderDetail_Color = Convert.ToString(ItemArray[8]) != "NULL" ? Convert.ToString(ItemArray[8]) : string.Empty;
            string pOrderDetail_BestBeforeMonth = Convert.ToString(ItemArray[10]);
            string pOrderDetail_BestBeforeYear = Convert.ToString(ItemArray[12]);
            string pOrderDetail_QuoteCurrencyName = Convert.ToString(ItemArray[13]);
            string pOrderDetail_QuoteCurrency = Convert.ToString(ItemArray[14]);
            string pOrderDetail_UnitPrice = Convert.ToString(ItemArray[15]);
            string pOrderDetail_PriceLot = Convert.ToString(ItemArray[16]);
            string pOrderDetail_QuantityName = Convert.ToString(ItemArray[17]);
            string pOrderDetail_Quantity = Convert.ToString(ItemArray[18]);
            string pOrderDetail_QuantityUnit = Convert.ToString(ItemArray[19]);
            string QuantityUnitName = Convert.ToString(ItemArray[17]);
            string QuantityUnitId = Convert.ToString(ItemArray[18]);
            string pOrderDetail_PriceLotUnitName = Convert.ToString(ItemArray[20]);
            string pOrderDetail_PriceLotId = Convert.ToString(ItemArray[21]);
            string pOrderDetail_Remarks = Convert.ToString(ItemArray[24]) != "NULL" ? Convert.ToString(ItemArray[24]) : string.Empty;
            string pOrderDetail_ProductDescription = Convert.ToString(ItemArray[26]) != "NULL" ? Convert.ToString(ItemArray[26]) : string.Empty;
            string pOrderDetail_ApprovPrice = Convert.ToString(ItemArray[19]);
            string pOrderDetail_ApprovQuantity = Convert.ToString(ItemArray[19]);
            Session["pOrderDetailsEditId"] = pOrderDetail_OrderID;

            cityGrid.JSProperties["cpEdit"] = pOrderDetail_OrderID + "|"
                + pOrderDetail_Number + "|" + pOrderDetail_ProductID + "|"
                + pOrderDetail_Brand + "|" + pOrderDetail_Size + "|"
                + pOrderDetail_Color + "|" + pOrderDetail_BestBeforeMonth + "|"
                + pOrderDetail_BestBeforeYear + "|" + pOrderDetail_QuoteCurrency + "|"
                + pOrderDetail_UnitPrice + "|" + pOrderDetail_PriceLot + "|"
                + pOrderDetail_Quantity + "|" + pOrderDetail_QuantityUnit + "|"
                + pOrderDetail_Remarks + "|" + pOrderDetail_ApprovPrice + "|"
                + pOrderDetail_ApprovQuantity + "|"
                + pOrderDetail_QuoteCurrencyName + "|"
                + QuantityUnitName + "|"
                + QuantityUnitId + "|"
                + pOrderDetail_PriceLotUnitName + "|"
                + pOrderDetail_PriceLotId + "|"
                + WhichType + "|"
                + pOrderDetail_ProductDescription+"|"
                + ProductReferenceNo;
        }
    }

    protected void BindSize()
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select * from Master_Size order By Size_Name");
        AspxHelper oAspxHelper = new AspxHelper();
        if (dtCmb.Rows.Count > 0)
            oAspxHelper.Bind_Combo(ddlSize, dtCmb, "Size_Name", "Size_ID", 0);
        ddlSize.Items.Insert(0, new DevExpress.Web.ListEditItem("None", "0"));
        ddlSize.SelectedIndex = 0;

    }

    protected void BindColor()
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select * from Master_Color order By Color_Name");
        AspxHelper oAspxHelper = new AspxHelper();
        if (dtCmb.Rows.Count > 0)
            oAspxHelper.Bind_Combo(ddlColor, dtCmb, "Color_Name", "Color_ID", 0);
        ddlColor.Items.Insert(0, new DevExpress.Web.ListEditItem("None", "0"));
        ddlColor.SelectedIndex = 0;

    }
    protected void BindYear()
    {

        DataTable dtCmb = new DataTable();

        dtCmb.Columns.Add("id");
        dtCmb.Columns.Add("name");

        DataRow dr = dtCmb.NewRow();
        dr["id"] = "0";
        dr["name"] = "";
        dtCmb.Rows.Add(dr);

        for (int i = 2000; i <= 2050; i++)
        {
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = Convert.ToString(i);
            drsession["name"] = Convert.ToString(i);
            dtCmb.Rows.Add(drsession);
        }


        AspxHelper oAspxHelper = new AspxHelper();
        if (dtCmb.Rows.Count > 0)
            oAspxHelper.Bind_Combo(ASPxYear, dtCmb, "name", "id", 0);



    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string EditVal = string.Empty;
        if (Request.QueryString["ID"] != null)
        {
            EditVal = Convert.ToString(Request.QueryString["ID"]);
        }
        if (EditVal.Equals("") && !isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
        }
        else if (isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
        }
        else
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
        }
        string pOrderDetailsEditId = null;
        pOrderDetailsEditId = (string)Session["pOrderDetailsEditId"];
        if (pOrderDetailsEditId != null)
        {
            ds.ReadXml(PXMLPATH);
            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["RecordID"]) == pOrderDetailsEditId)
                    {
                        dt.Rows.Remove(dt.Rows[i]);
                        ds.WriteXml(PXMLPATH);
                    }
                }
            }
        }
        cityGrid.JSProperties["cpinsert"] = null;
        XMLToGridBind();
        CleartextBoxes(this);
        cityGrid.JSProperties["cpinsert"] = "Success";
    }
    private void XMLToGridBind()
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DataSet dsToAddXML = new DataSet();
        HiddenField pOrderHiddenField = (HiddenField)Parent.FindControl("pOderId_hidden");
        string EditVal = string.Empty;
        if (Request.QueryString["ID"] != null)
        {
            EditVal = Convert.ToString(Request.QueryString["ID"]);
        }

        if (EditVal.Equals("") && !isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
        }
        else if (isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
        }
        else
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
        }
        //PXMLPATH = "../../Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C";
        if (File.Exists(PXMLPATH))
        {

            dsToAddXML.ReadXml(PXMLPATH);
            if (dsToAddXML.Tables.Count > 1)
            {
                // FOR SERIAL NUMBER COUNTER
                if (dsToAddXML.Tables[1].Rows.Count > 0)
                {
                    PCounter = Convert.ToInt32(dsToAddXML.Tables[1].Rows[dsToAddXML.Tables[1].Rows.Count - 1]["RecordID"].ToString()) + 1;
                }

                DataRow drXML = dsToAddXML.Tables[1].NewRow();
                if (Session["pOrderDetailsEditId"] != null)
                {
                    drXML[0] = (string)Session["pOrderDetailsEditId"];
                }
                else
                {
                    drXML[0] = PCounter;
                }
                drXML[1] = txtProductDetails.Text.Trim();
                drXML[2] = txtProductDetails_hidden.Text.Trim();
                drXML[3] = txtProduct_Order_Detail_Brand.Text.Trim();
                drXML[4] = txtProduct_Order_Detail_Brand_hidden.Text.Trim();
                if (ddlSize.SelectedItem != null)
                {
                    drXML[5] = ddlSize.SelectedItem.Text.Trim();
                }
                if (ddlSize.SelectedItem != null)
                {
                    drXML[6] = ddlSize.SelectedItem.Value;
                }
                if (ddlColor.SelectedItem != null)
                {
                    drXML[7] = ddlColor.SelectedItem.Text.Trim();
                }
                if (ddlColor.SelectedItem != null)
                {
                    drXML[8] = ddlColor.SelectedItem.Value;
                }
                if (ASPxMonth.SelectedItem != null)
                {
                    drXML[9] = ASPxMonth.SelectedItem.Text.Trim();
                }
                if (ASPxMonth.SelectedItem != null)
                {
                    drXML[10] = ASPxMonth.SelectedItem.Value;
                }
                if (ASPxYear.SelectedItem != null)
                {
                    drXML[11] = ASPxYear.SelectedItem.Text.Trim();
                }
                if (ASPxYear.SelectedItem != null)
                {
                    drXML[12] = ASPxYear.SelectedItem.Value;
                }
                drXML[13] = txtQuoteCurrency.Text.Trim();
                drXML[14] = txtQuoteCurrency_hidden.Text.Trim();
                drXML[15] = txtpUnit.Text.Trim();
                drXML[16] = txtLotUnit.Text.Trim();
                drXML[17] = txtUnit.Text.Trim();
                drXML[18] = txtUnit_hidden.Text.Trim();
                drXML[19] = txtQuantity.Text.Trim();
                drXML[20] = txtQntityunit.Text.Trim();
                drXML[21] = txtQntityunit_hidden.Text.Trim();
                drXML[22] = txtUnit.Text.Trim();
                drXML[23] = txtUnit_hidden.Text.Trim();
                drXML[24] = txtAreaRemarks.Text.Trim();
                if (Convert.ToString(pOrderHiddenField.Value) != "")
                {
                    drXML[25] = Convert.ToString(pOrderHiddenField.Value);
                }
                drXML[26] = txtProductDescription.Text.Trim();
                int rownumb = dsToAddXML.Tables.Count;
                dsToAddXML.Tables[1].Rows.Add(drXML);
                //dsToAddXML.Tables[1].AcceptChanges();
                dsToAddXML.WriteXml(PXMLPATH);
                dsToAddXML.Dispose();
            }
            else
            {

                dtXML = dsToAddXML.Tables.Add();
                dtXML.Columns.Add(new DataColumn("RecordID", typeof(int))); //0
                dtXML.Columns.Add(new DataColumn("ProductDetailsName", typeof(string)));//1
                dtXML.Columns.Add(new DataColumn("ProductDetailsID", typeof(string)));//2
                dtXML.Columns.Add(new DataColumn("DetailBrandName", typeof(string)));//3
                dtXML.Columns.Add(new DataColumn("Detail_BrandID", typeof(string)));//4
                dtXML.Columns.Add(new DataColumn("SizeName", typeof(string)));//5
                dtXML.Columns.Add(new DataColumn("SizeID", typeof(string)));//6
                dtXML.Columns.Add(new DataColumn("ColorName", typeof(string)));//7
                dtXML.Columns.Add(new DataColumn("ColorID", typeof(string)));//8
                dtXML.Columns.Add(new DataColumn("MonthName", typeof(string)));//9
                dtXML.Columns.Add(new DataColumn("MonthID", typeof(string)));//10
                dtXML.Columns.Add(new DataColumn("YearName", typeof(string)));//11
                dtXML.Columns.Add(new DataColumn("YearID", typeof(string)));//12
                dtXML.Columns.Add(new DataColumn("QuoteCurrencyName", typeof(string)));//13
                dtXML.Columns.Add(new DataColumn("QuoteCurrencyID", typeof(string)));//14
                dtXML.Columns.Add(new DataColumn("PerchesUnit", typeof(string)));//15
                dtXML.Columns.Add(new DataColumn("LotUnit", typeof(string)));//16
                dtXML.Columns.Add(new DataColumn("UnitName", typeof(string)));//17
                dtXML.Columns.Add(new DataColumn("UnitID", typeof(string)));//18
                dtXML.Columns.Add(new DataColumn("Quantity", typeof(string)));//19
                dtXML.Columns.Add(new DataColumn("QntityunitName", typeof(string)));//20
                dtXML.Columns.Add(new DataColumn("QntityunitID", typeof(string)));//21
                dtXML.Columns.Add(new DataColumn("UnitName1", typeof(string)));//22
                dtXML.Columns.Add(new DataColumn("UnitID1", typeof(string)));//23
                dtXML.Columns.Add(new DataColumn("Remarks", typeof(string)));//24 
                dtXML.Columns.Add(new DataColumn("pOrder_ProductId", typeof(string)));//25
                dtXML.Columns.Add(new DataColumn("ProductDescription", typeof(string)));//26


                DataRow drXML = dtXML.NewRow();

                drXML[0] = 1;
                drXML[1] = txtProductDetails.Text.Trim();
                drXML[2] = txtProductDetails_hidden.Text.Trim();
                drXML[3] = txtProduct_Order_Detail_Brand.Text.Trim();
                drXML[4] = txtProduct_Order_Detail_Brand_hidden.Text.Trim();
                if (ddlSize.SelectedItem != null)
                {
                    drXML[5] = ddlSize.SelectedItem.Text.Trim();
                }
                if (ddlSize.SelectedItem != null)
                {
                    drXML[6] = ddlSize.SelectedItem.Value;
                }
                if (ddlColor.SelectedItem != null)
                {
                    drXML[7] = ddlColor.SelectedItem.Text.Trim();
                }
                if (ddlColor.SelectedItem != null)
                {
                    drXML[8] = ddlColor.SelectedItem.Value;
                }
                if (ASPxMonth.SelectedItem != null)
                {
                    drXML[9] = ASPxMonth.SelectedItem.Text.Trim();
                }
                if (ASPxMonth.SelectedItem != null)
                {
                    drXML[10] = ASPxMonth.SelectedItem.Value;
                }
                if (ASPxYear.SelectedItem != null)
                {
                    drXML[11] = ASPxYear.SelectedItem.Text.Trim();
                }
                if (ASPxYear.SelectedItem != null)
                {
                    drXML[12] = ASPxYear.SelectedItem.Value;
                }
                drXML[13] = txtQuoteCurrency.Text.Trim();
                drXML[14] = txtQuoteCurrency_hidden.Text.Trim();
                drXML[15] = txtpUnit.Text.Trim();
                drXML[16] = txtLotUnit.Text.Trim();
                drXML[17] = txtUnit.Text.Trim();
                drXML[18] = txtUnit_hidden.Text.Trim();
                drXML[19] = txtQuantity.Text.Trim();
                drXML[20] = txtQntityunit.Text.Trim();
                drXML[21] = txtQntityunit_hidden.Text.Trim();
                drXML[22] = txtUnit.Text.Trim();
                drXML[23] = txtUnit_hidden.Text.Trim();
                drXML[24] = txtAreaRemarks.Text.Trim();
                if (Convert.ToString(pOrderHiddenField.Value) != "")
                {
                    drXML[25] = Convert.ToString(pOrderHiddenField.Value);
                }
                drXML[26] = txtProductDescription.Text.Trim();
                dtXML.Rows.Add(drXML);
                dtXML.AcceptChanges();
                dsToAddXML.Tables[1].TableName = "DtPerchesOrderDetails";
                dsToAddXML.WriteXml(PXMLPATH);
            }
        }
        BindGrid(dsToAddXML, "DESC");
        dsToAddXML.Dispose();
        Session["pOrderDetailsEditId"] = null;
    }
    private void BindGrid(DataSet ds, String WhichSort)
    {
        try
        {
            if (ds.Tables.Count == 2)
            {
                DataView TempDV = new DataView(ds.Tables[1]);
                TempDV.Sort = "RecordID " + WhichSort;
                cityGrid.DataSource = TempDV;
                cityGrid.DataBind();
            }
            else
            {
                cityGrid.DataSource = null;
                cityGrid.DataBind();
            }
        }
        catch { }
    }

    private void XMLFileTracking()
    {
        string EditVal = string.Empty;
        if (Request.QueryString["ID"] != null && !isNotEditables)
        {
            EditVal = Convert.ToString(Request.QueryString["ID"]);
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
        }
        else if (isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
        }
        else
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
        }

        if (File.Exists(PXMLPATH))
        {
            File.Delete(PXMLPATH);
        }
    }

    private void CleartextBoxes(Control parent)
    {

        foreach (Control x in parent.Controls)
        {
            if ((x.GetType() == typeof(TextBox)))
            {

                ((TextBox)(x)).Text = "";
            }

            if (x.HasControls())
            {
                CleartextBoxes(x);
            }
        }
    }

    protected void btnSubmitAll_Click(object sender, EventArgs e)
    {
        Store_MasterBL oStore_MasterBL = new Store_MasterBL();
        DataTable dt = new DataTable();
        DataSet dsToInsertDB = new DataSet();
        HiddenField pOrderHiddenField = (HiddenField)Parent.FindControl("pOderId_hidden");
        string pOrderID = string.Empty;
        try
        {
            string EditVal = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                EditVal = Convert.ToString(Request.QueryString["ID"]);
            }


            if (EditVal.Equals("") && !isNotEditables)
            {

                PXMLPATH = "../../Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C";

                if (File.Exists(Server.MapPath(PXMLPATH)))
                {
                    dsToInsertDB.ReadXml(Server.MapPath(PXMLPATH));
                }
                if (dsToInsertDB.Tables[0].Rows.Count > 0)
                {
                    DataTable dtCreate = new DataTable();
                    dtCreate = dsToInsertDB.Tables[0];
                    string OrderNo = oDBEngine.ExeSclar("select [dbo].[fn_OrderNumber] ('" + Convert.ToString(dtCreate.Rows[0]["pOrderType"]) + "','" + Convert.ToString(Session["userid"]) + "','" + Convert.ToString(Session["userlastsegment"]) + "')");

                    //                    string query = string.Empty;
                    //                    query = @"'" + CurrentCompany() + "'" + "," + "'" + dtCreate.Rows[0]["pOrderBranch"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderDate"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderFinYear"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderType"] + "'" + "," + "'" + OrderNo + "'," + "'" +
                    //                        dtCreate.Rows[0]["pOrderContactID"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderRefNumber"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderAgentID"] + "'" + "," + "'" +
                    //                        dtCreate.Rows[0]["pOrderInstructions"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderPaymentTerm"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderPaymentDate"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderDeliveryDate"] + "'" + "," +
                    //                        "'" + dtCreate.Rows[0]["pOrderDeliveryAt"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderDeliveryBranch"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderDeliveryWareHouse"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderDeliveryAddress"] + "'" + ","
                    //                        + "'" + dtCreate.Rows[0]["pOrderDeliveryOther"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderVerified"] + "'" + "," + "'" + dtCreate.Rows[0]["pOrderVerifyRemarks"] + "'" + "," + "'" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "'" + ","
                    //                        + " getdate()" + ",'" + dtCreate.Rows[0]["pOrder_PaymentDays"] + "'";
                    //                    query = query.Replace("'NULL'", "NULL");

                    //                    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    //                    int ID = oGenericMethod.Insert_Table("Trans_pOrder", @"[pOrder_Company],[pOrder_Branch],[pOrder_Date],[pOrder_FinYear],[pOrder_Type],[pOrder_Number],
                    //                [pOrder_ContactID],[pOrder_RefNumber],[pOrder_AgentID],[pOrder_Instructions],[pOrder_PaymentTerm],[pOrder_PaymentDate],
                    //                [pOrder_DeliveryDate],[pOrder_DeliveryAt],[pOrder_DeliveryBranch],[pOrder_DeliveryWareHouse],[pOrder_DeliveryAddress],[pOrder_DeliveryOther],
                    //                [pOrder_Verified],[pOrder_VerifyRemarks],[pOrder_CreateUser],[pOrder_CreateTime],[pOrder_PaymentDays]"
                    //                        , query);

                    int pOrderBranch = 0;
                    string pOrderDate = string.Empty; 
                    string pOrderFinYear = string.Empty; 
                    string pOrderType = string.Empty;
                    string OrderNO = OrderNo;
                    string pOrderContactID = string.Empty;
                    string pOrderRefNumber = string.Empty;
                    string pOrderAgentID = string.Empty;
                    string pOrderInstructions = string.Empty;
                    string pOrderPaymentTerm = string.Empty;
                    string pOrderPaymentDate = string.Empty;
                    string pOrderDeliveryDate = string.Empty;
                    string pOrderDeliveryAt = string.Empty;
                    int pOrderDeliveryBranch = 0;
                    int pOrderDeliveryWareHouse = 0;
                    int pOrderDeliveryAddress = 0;
                    string pOrderDeliveryOther = string.Empty;
                    int pOrder_PaymentDays = 0;
                    string pOrderParentRefNumber = string.Empty;

                    if (Convert.ToString(dtCreate.Rows[0]["pOrderBranch"]) != null)
                    {
                        pOrderBranch = Convert.ToInt32(dtCreate.Rows[0]["pOrderBranch"]);
                    }
                    if (Convert.ToString(dtCreate.Rows[0]["pOrderDate"]) != null)
                    {
                        pOrderDate = Convert.ToDateTime(dtCreate.Rows[0]["pOrderDate"]).ToString("yyyy-MM-dd");
                    }
                    pOrderFinYear = Convert.ToString(dtCreate.Rows[0]["pOrderFinYear"]);
                    pOrderType = Convert.ToString(dtCreate.Rows[0]["pOrderType"]);
                    pOrderContactID = Convert.ToString(dtCreate.Rows[0]["pOrderContactID"]);
                    pOrderRefNumber = Convert.ToString(dtCreate.Rows[0]["pOrderRefNumber"]);

                    pOrderParentRefNumber = Convert.ToString(dtCreate.Rows[0]["pOrderParentRefNumber"]);

                    pOrderAgentID = Convert.ToString(dtCreate.Rows[0]["pOrderAgentID"]);
                    pOrderInstructions = Convert.ToString(dtCreate.Rows[0]["pOrderInstructions"]);
                    pOrderPaymentTerm = Convert.ToString(dtCreate.Rows[0]["pOrderPaymentTerm"]);
                    pOrderPaymentDate = Convert.ToString(dtCreate.Rows[0]["pOrderPaymentDate"]);
                    pOrderDeliveryDate = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryDate"]);
                    pOrderDeliveryAt = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryAt"]);
                    if (Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryBranch"]) != null)
                    {
                        pOrderDeliveryBranch = Convert.ToInt32(dtCreate.Rows[0]["pOrderDeliveryBranch"]);
                    }
                    if (Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryWareHouse"]) != null)
                    {
                        pOrderDeliveryWareHouse = Convert.ToInt32(dtCreate.Rows[0]["pOrderDeliveryWareHouse"]);
                    }
                    if (Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryAddress"]) != null)
                    {
                        pOrderDeliveryAddress = Convert.ToInt32(dtCreate.Rows[0]["pOrderDeliveryAddress"]);
                    }
                    pOrderDeliveryOther = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryOther"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(dtCreate.Rows[0]["pOrder_PaymentDays"])))
                    {
                        pOrder_PaymentDays = Convert.ToInt32(dtCreate.Rows[0]["pOrder_PaymentDays"]);
                    }
                    int ID = oStore_MasterBL.InsertOrder(CurrentCompany(), pOrderBranch, pOrderDate, pOrderFinYear, pOrderType, OrderNO, 
                        pOrderContactID, pOrderRefNumber, pOrderAgentID, pOrderInstructions,pOrderPaymentTerm, pOrderPaymentDate,pOrderDeliveryDate, 
                        pOrderDeliveryAt, pOrderDeliveryBranch, pOrderDeliveryWareHouse, pOrderDeliveryAddress, pOrderDeliveryOther,
                        Convert.ToInt32(Convert.ToString(HttpContext.Current.Session["userid"]).Trim()), pOrder_PaymentDays, pOrderParentRefNumber);

                    DataTable dtTranspOrder = oGenericMethod.GetDataTable("select top 1 pOrder_ID from Trans_pOrder order by pOrder_ID desc");
                    pOrderID = Convert.ToString(dtTranspOrder.Rows[0]["pOrder_ID"]);
                }
                if (dsToInsertDB.Tables.Contains("DtPerchesOrderDetails"))
                {
                    if (dsToInsertDB.Tables[1].Rows.Count > 0)
                    {
                        dt = dsToInsertDB.Tables[1];
                        foreach (DataRow dr in dt.Rows)
                        {
                            var value = dr.ItemArray;
                            string[] strarry = Convert.ToString(value[2]).Split('_');
                            int ProductId = 0;
                            if (!Convert.ToString(strarry[0]).Trim().Equals(""))
                            {
                                ProductId = Convert.ToInt32(strarry[0]);
                            }
                            long pOrderDetail_OrderID = 0;
                            if (!Convert.ToString(pOrderID).Trim().Equals(""))
                            {
                                pOrderDetail_OrderID = Convert.ToInt64(pOrderID);
                            }
                            int pOrderDetail_ProductID = ProductId;
                            string pOrderDetail_Brand = Convert.ToString(value[3]);
                            int pOrderDetail_Size = 0;
                            if (!Convert.ToString(value[6]).Trim().Equals(""))
                            {
                                pOrderDetail_Size = Convert.ToInt32(value[6]);
                            }
                            int pOrderDetail_Color = 0;
                            if (!Convert.ToString(value[8]).Trim().Equals(""))
                            {
                                pOrderDetail_Color = Convert.ToInt32(value[8]);
                            }
                            int pOrderDetail_BestBeforeMonth = 0;
                            if (!Convert.ToString(value[10]).Trim().Equals(""))
                            {
                                pOrderDetail_BestBeforeMonth = Convert.ToInt32(value[10]);
                            }
                            int pOrderDetail_BestBeforeYear = 0;
                            if (!Convert.ToString(value[12]).Trim().Equals(""))
                            {
                                pOrderDetail_BestBeforeYear = Convert.ToInt32(value[12]);
                            }
                            int pOrderDetail_QuoteCurrency = 0;
                            if (!Convert.ToString(value[14]).Trim().Equals(""))
                            {
                                pOrderDetail_QuoteCurrency = Convert.ToInt32(value[14]);
                            }
                            decimal pOrderDetail_UnitPrice = 0;
                            if (!Convert.ToString(value[15]).Trim().Equals(""))
                            {
                                pOrderDetail_UnitPrice = Convert.ToDecimal(value[15]);
                            }
                            int pOrderDetail_PriceLot = 0;
                            if (!Convert.ToString(value[16]).Trim().Equals(""))
                            {
                                pOrderDetail_PriceLot = Convert.ToInt32(value[16]);
                            }
                            decimal pOrderDetail_Quantity = 0;
                            if (!Convert.ToString(value[19]).Trim().Equals(""))
                            {
                                pOrderDetail_Quantity = Convert.ToDecimal(value[19]);
                            }
                            int pOrderDetail_QuantityUnit = 0;
                            if (!Convert.ToString(value[21]).Trim().Equals(""))
                            {
                                pOrderDetail_QuantityUnit = Convert.ToInt32(value[21]);
                            }
                            int pOrderDetail_PriceLotUnit = 0;
                            if (!Convert.ToString(value[18]).Trim().Equals(""))
                            {
                                pOrderDetail_PriceLotUnit = Convert.ToInt32(value[18]);
                            }
                            string pOrderDetail_Remarks = Convert.ToString(value[24]);
                            string pOrderDetail_ProductDescription = Convert.ToString(value[26]);
                            int pOrderDetail_CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);

                            oStore_MasterBL.InsertOrderDetails(Convert.ToInt32(pOrderDetail_OrderID), pOrderDetail_ProductID, pOrderDetail_Brand, pOrderDetail_Size, pOrderDetail_Color, pOrderDetail_BestBeforeMonth,
                                pOrderDetail_BestBeforeYear, pOrderDetail_QuoteCurrency, Convert.ToString(pOrderDetail_UnitPrice), pOrderDetail_PriceLot, Convert.ToString(pOrderDetail_Quantity),
                                pOrderDetail_QuantityUnit, pOrderDetail_PriceLotUnit, pOrderDetail_Remarks, pOrderDetail_CreateUser, pOrderDetail_ProductDescription);
                        }

                    }
                }
                XMLFileTracking();
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                if (isNotEditables)
                {
                    PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
                }
                else
                {
                    PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
                }
                if (File.Exists(PXMLPATH))
                {
                    dsToInsertDB.ReadXml(PXMLPATH);
                }
                if (dsToInsertDB.Tables[0].Rows.Count > 0)
                {
                    if (EditVal == "")
                    {
                        EditVal = EditIds;
                    }
                    DataTable dtCreate = new DataTable();
                    dtCreate = dsToInsertDB.Tables[0];
                    string pOrder_Company = Convert.ToString(dtCreate.Rows[0]["pOrderCompany"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderCompany"]);
                    string pOrder_Branch = Convert.ToString(dtCreate.Rows[0]["pOrderBranch"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderBranch"]);
                    string pOrder_Date = Convert.ToString(dtCreate.Rows[0]["pOrderDate"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderDate"]);
                    string pOrder_FinYear = Convert.ToString(dtCreate.Rows[0]["pOrderFinYear"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderFinYear"]);
                    string pOrder_Type = Convert.ToString(dtCreate.Rows[0]["pOrderType"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderType"]);
                    string pOrder_ContactID = Convert.ToString(dtCreate.Rows[0]["pOrderContactID"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderContactID"]);
                    string pOrder_RefNumber = Convert.ToString(dtCreate.Rows[0]["pOrderRefNumber"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderRefNumber"]);
                    string pOrder_Number = Convert.ToString(dtCreate.Rows[0]["pOrderNumber"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderNumber"]);
                    string pOrder_AgentID = Convert.ToString(dtCreate.Rows[0]["pOrderAgentID"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderAgentID"]);
                    string pOrder_Instructions = Convert.ToString(dtCreate.Rows[0]["pOrderInstructions"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderInstructions"]);
                    string pOrder_PaymentTerm = Convert.ToString(dtCreate.Rows[0]["pOrderPaymentTerm"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderPaymentTerm"]);
                    string pOrder_PaymentDate = Convert.ToString(dtCreate.Rows[0]["pOrderPaymentDate"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderPaymentDate"]);
                    string pOrder_DeliveryDate = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryDate"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryDate"]);
                    string pOrder_DeliveryAt = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryAt"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryAt"]);
                    string pOrder_DeliveryBranch = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryBranch"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryBranch"]);
                    string pOrder_DeliveryWareHouse = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryWareHouse"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryWareHouse"]);
                    string pOrder_DeliveryAddress = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryAddress"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryAddress"]);
                    string pOrder_DeliveryOther = Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryOther"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderDeliveryOther"]);
                    string pOrder_Verified = Convert.ToString(dtCreate.Rows[0]["pOrderVerified"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderVerified"]);
                    string pOrder_VerifyRemarks = Convert.ToString(dtCreate.Rows[0]["pOrderVerifyRemarks"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderVerifyRemarks"]);
                    string pOrder_VerifyUser = Convert.ToString(dtCreate.Rows[0]["pOrderVerifyUser"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderVerifyUser"]);
                    string pOrder_VerifyTime = Convert.ToString(dtCreate.Rows[0]["pOrderVerifyTime"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderVerifyTime"]);
                    string pOrder_ApprovUser1 = Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser1"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser1"]);
                    string pOrder_ApprovUser1Time = Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser1Time"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser1Time"]);
                    string pOrder_ApprovUser2 = Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser2"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser2"]);
                    string pOrder_ApprovUser2Time = Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser2Time"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser2Time"]);
                    string pOrder_ApprovUser3 = Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser3"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser3"]);
                    string pOrder_ApprovUser3Time = Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser3Time"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderApprovUser3Time"]);
                    string pOrder_PaymentDays = Convert.ToString(dtCreate.Rows[0]["pOrder_PaymentDays"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrder_PaymentDays"]);
                    string pOrder_ModifyUser = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
                    string pOrderParentRefNumber = Convert.ToString(dtCreate.Rows[0]["pOrderParentRefNumber"]) == "" ? null : Convert.ToString(dtCreate.Rows[0]["pOrderParentRefNumber"]);



                    string query = string.Empty;
                    //query = "pOrder_Branch='" + pOrder_Branch + "'," + "pOrder_Date='" + pOrder_Date + "'," + "pOrder_FinYear='" + pOrder_FinYear + "'," +
                    //   "pOrder_Type='" + pOrder_Type + "'," + "pOrder_ContactID='" + pOrder_ContactID + "'," + "pOrder_RefNumber='" + pOrder_RefNumber + "'," + "pOrder_AgentID='" + pOrder_AgentID + "'," +
                    //   "pOrder_Instructions='" + pOrder_Instructions + "'," + "pOrder_PaymentTerm='" + pOrder_PaymentTerm + "'," + "pOrder_PaymentDate='" + pOrder_PaymentDate + "'," + "pOrder_DeliveryDate='" + pOrder_DeliveryDate + "'," + "pOrder_DeliveryAt='" + pOrder_DeliveryAt + "'," +
                    //   "pOrder_DeliveryBranch='" + pOrder_DeliveryBranch + "'," + "pOrder_DeliveryWareHouse='" + pOrder_DeliveryWareHouse + "'," + "pOrder_DeliveryAddress='" + pOrder_DeliveryAddress + "'," + "pOrder_DeliveryOther='" + pOrder_DeliveryOther + "'," + "pOrder_Verified='" + pOrder_Verified + "'," +
                    //   "pOrder_VerifyRemarks='" + pOrder_VerifyRemarks + "'," + "pOrder_ModifyUser='" + pOrder_ModifyUser + "'," + "pOrder_ModifyTime=" + pOrder_ModifyTime + "," + "pOrder_PaymentDays='" + pOrder_PaymentDays + "'";
                    //query = query.Replace("'NULL'", "NULL");
                    //oGenericMethod.Update_Table("Trans_pOrder", query, "pOrder_ID='" + EditVal + "'");

                    oStore_MasterBL.UpdateOrder(Convert.ToInt32(EditVal), Convert.ToString(pOrder_Branch) == null ? 0 : Convert.ToInt32(pOrder_Branch), Convert.ToDateTime(pOrder_Date).ToString("yyyy-MM-dd"), pOrder_FinYear,
                        pOrder_Type, pOrder_ContactID, pOrder_RefNumber, pOrder_AgentID, pOrder_Instructions, pOrder_PaymentTerm, Convert.ToDateTime(pOrder_PaymentDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(pOrder_DeliveryDate).ToString("yyyy-MM-dd"), pOrder_DeliveryAt,
                        pOrder_DeliveryBranch == "NULL" ? 0 : Convert.ToInt32(pOrder_DeliveryBranch), Convert.ToString(pOrder_DeliveryWareHouse) == "NULL" ? 0 : Convert.ToInt32(pOrder_DeliveryWareHouse), Convert.ToString(pOrder_DeliveryAddress) == "NULL" ? 0 : Convert.ToInt32(pOrder_DeliveryAddress),
                        pOrder_DeliveryOther,
                        Convert.ToInt32(pOrder_ModifyUser), Convert.ToString(pOrder_PaymentDays) == "NULL" ? 0 : Convert.ToInt32(pOrder_PaymentDays), pOrderParentRefNumber);


                    DataTable dtTranspOrder = oGenericMethod.GetDataTable("select top 1 pOrder_ID from Trans_pOrder order by pOrder_ID desc");
                    pOrderID = Convert.ToString(dtTranspOrder.Rows[0]["pOrder_ID"]);
                }
                if (dsToInsertDB.Tables.Contains("DtPerchesOrderDetails"))
                {
                    if (dsToInsertDB.Tables[1].Rows.Count > 0)
                    {
                        dt = dsToInsertDB.Tables[1];
                        oGenericMethod.Delete_Table("Trans_pOrderDetail", "pOrderDetail_OrderID='" + EditVal + "'");
                        foreach (DataRow dr in dt.Rows)
                        {
                            var value = dr.ItemArray;
                            string ProductId = string.Empty;
                            if (Convert.ToString(value[2]).Contains("_"))
                            {
                                string[] ProductIdArry = Convert.ToString(value[2]).Split('_');
                                ProductId = ProductIdArry[0];
                            }
                            else
                            {
                                ProductId = Convert.ToString(value[2]);
                            }
                            long pOrderDetail_OrderID = Convert.ToInt64(pOrderID);
                            int pOrderDetail_ProductID = Convert.ToInt32(ProductId.Replace("_", ""));
                            string pOrderDetail_Brand = Convert.ToString(value[3]);
                            int pOrderDetail_Size = 0;
                            if (!Convert.ToString(value[6]).Trim().Equals(""))
                            {
                                pOrderDetail_Size = Convert.ToInt32(value[6]) == null ? 0 : Convert.ToInt32(value[6]);
                            }
                            int pOrderDetail_Color = 0;
                            if (!Convert.ToString(value[8]).Trim().Equals(""))
                            {
                                pOrderDetail_Color = Convert.ToInt32(value[8]) == null ? 0 : Convert.ToInt32(value[8]);
                            }
                            int pOrderDetail_BestBeforeMonth = 0;
                            if (!Convert.ToString(value[10]).Trim().Equals(""))
                            {
                                pOrderDetail_BestBeforeMonth = Convert.ToInt32(value[10]) == null ? 0 : Convert.ToInt32(value[10]);
                            }
                            int pOrderDetail_BestBeforeYear = 0;
                            if (!Convert.ToString(value[12]).Trim().Equals(""))
                            {
                                pOrderDetail_BestBeforeYear = Convert.ToInt32(value[12]) == null ? 0 : Convert.ToInt32(value[12]);
                            }
                            int pOrderDetail_QuoteCurrency = Convert.ToInt32(value[14]) == null ? 0 : Convert.ToInt32(value[14]);
                            decimal pOrderDetail_UnitPrice = 0;
                            if (!Convert.ToString(value[15]).Trim().Equals(""))
                            {
                                pOrderDetail_UnitPrice = Convert.ToDecimal(value[15]);
                            }
                            int pOrderDetail_PriceLot = 0;
                            if (!Convert.ToString(value[16]).Trim().Equals(""))
                            {
                                pOrderDetail_PriceLot = Convert.ToInt32(value[16]) == null ? 0 : Convert.ToInt32(value[16]);
                            }
                            decimal pOrderDetail_Quantity = 0;
                            if (!Convert.ToString(value[19]).Trim().Equals(""))
                            {
                                pOrderDetail_Quantity = Convert.ToDecimal(value[19]) == null ? 0 : Convert.ToDecimal(value[19]);
                            }
                            int pOrderDetail_QuantityUnit = 0;
                            if (!Convert.ToString(value[18]).Trim().Equals(""))
                            {
                                pOrderDetail_QuantityUnit = Convert.ToInt32(value[18]) == null ? 0 : Convert.ToInt32(value[21]);
                            }
                            int pOrderDetail_PriceLotUnit = 0;
                            if (!Convert.ToString(value[21]).Trim().Equals(""))
                            {
                                pOrderDetail_PriceLotUnit = Convert.ToInt32(value[21]) == null ? 0 : Convert.ToInt32(value[23]);
                            }
                            string pOrderDetail_Remarks = Convert.ToString(value[24]);
                            string pOrderDetail_ProductDescription = Convert.ToString(value[26]);
                            int pOrderDetail_CreateUser = 0;
                            if (!Convert.ToString(HttpContext.Current.Session["userid"]).Trim().Equals(""))
                            {
                                pOrderDetail_CreateUser = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                            }


                            string q = string.Empty;
                        //    q = "'" + EditVal + "'," + "'" + pOrderDetail_ProductID + "'," + "'" + pOrderDetail_Brand + "',"
                        //+ "'" + pOrderDetail_Size + "'," + "'" + pOrderDetail_Color + "'," + "'" + pOrderDetail_BestBeforeMonth + "'," + "'" + pOrderDetail_BestBeforeYear + "',"
                        //+ "'" + pOrderDetail_QuoteCurrency + "'," + "'" + pOrderDetail_UnitPrice + "'," + "'" + pOrderDetail_PriceLot + "'," + "'" + pOrderDetail_Quantity + "',"
                        //+ "'" + pOrderDetail_QuantityUnit + "'," + "'" + pOrderDetail_PriceLotUnit + "'," + "'" + pOrderDetail_Remarks + "'," + "'" + pOrderDetail_CreateUser + "'," + "'" + pOrderDetail_ProductDescription + "'," + "GETDATE()";

                        //    oGenericMethod.Insert_Table("Trans_pOrderDetail", "[pOrderDetail_OrderID],[pOrderDetail_ProductID],[pOrderDetail_Brand],[pOrderDetail_Size],[pOrderDetail_Color],[pOrderDetail_BestBeforeMonth],[pOrderDetail_BestBeforeYear],[pOrderDetail_QuoteCurrency],[pOrderDetail_UnitPrice],[pOrderDetail_PriceLot],[pOrderDetail_Quantity],[pOrderDetail_QuantityUnit],[pOrderDetail_PriceLotUnit],[pOrderDetail_Remarks],[pOrderDetail_ModifyUser],[pOrderDetail_ProductDescription],[pOrderDetail_ModifyTime]",
                        //        q);
                            oStore_MasterBL.InsertOrderDetails(Convert.ToInt32(EditVal), Convert.ToInt32(pOrderDetail_ProductID), pOrderDetail_Brand, Convert.ToString(pOrderDetail_Size) == null ? 0 : Convert.ToInt32(pOrderDetail_Size),
                                Convert.ToString(pOrderDetail_Color) == null ? 0 : Convert.ToInt32(pOrderDetail_Color), Convert.ToString(pOrderDetail_BestBeforeMonth) == null ? 0 : Convert.ToInt32(pOrderDetail_BestBeforeMonth),
                                Convert.ToString(pOrderDetail_BestBeforeYear) == null ? 0 : Convert.ToInt32(pOrderDetail_BestBeforeYear), Convert.ToString(pOrderDetail_QuoteCurrency) == null ? 0 : Convert.ToInt32(pOrderDetail_QuoteCurrency),
                                Convert.ToString(pOrderDetail_UnitPrice), Convert.ToString(pOrderDetail_PriceLot) == null ? 0 : Convert.ToInt32(pOrderDetail_PriceLot), Convert.ToString(pOrderDetail_Quantity),
                                Convert.ToString(pOrderDetail_QuantityUnit) == null ? 0 : Convert.ToInt32(pOrderDetail_QuantityUnit), Convert.ToString(pOrderDetail_PriceLotUnit) == null ? 0 : Convert.ToInt32(pOrderDetail_PriceLotUnit),
                                pOrderDetail_Remarks, Convert.ToInt32(pOrderDetail_CreateUser), pOrderDetail_ProductDescription);
                        }

                    }
                }

                XMLFileTracking();
                Response.Redirect(Request.RawUrl, false);
            }

        }
        catch (Exception ex) { }
    }
    protected void btnCanAll_Click(object sender, EventArgs e)
    {
    }
    
    protected void btnDiscard_Click(object sender, EventArgs e)
    {
        string EditVal = string.Empty;
        if (Request.QueryString["ID"] != null)
        {
            EditVal = Convert.ToString(Request.QueryString["ID"]);
        }
        if (EditVal.Equals("") && !isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
        }
        else if (isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
        }
        else
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
        }
        if (File.Exists(PXMLPATH))
        {
            File.Delete(PXMLPATH);
            ClearFieldsValue();
            cityGrid.DataSource = null;
            cityGrid.DataBind();
        }
        Response.Redirect(Request.RawUrl, false);
    }

    protected void btnSavenClose_Click(object sender, EventArgs e)
    {
        // for edit functionality of grid data
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        //PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
        string pOrderDetailsEditId = null;
        pOrderDetailsEditId = (string)Session["pOrderDetailsEditId"];
        string EditVal = string.Empty;
        if (Request.QueryString["ID"] != null)
        {
            EditVal = Convert.ToString(Request.QueryString["ID"]);
        }
        if (EditVal.Equals("") && !isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
        }
        else if (isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
        }
        else
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
        }
        if (pOrderDetailsEditId != null)
        {
            ds.ReadXml(PXMLPATH);
            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["RecordID"]) == pOrderDetailsEditId)
                    {
                        dt.Rows.Remove(dt.Rows[i]);
                        ds.WriteXml(PXMLPATH);
                    }
                }
            }
        }
        XMLToGridBind();
        CleartextBoxes(this);
        Popup_Empcitys.ShowOnPageLoad = false;
    }
    private bool IsFileExistance(string filePath)
    {
        bool flag = false;
        if (File.Exists(filePath))
        {
            flag = true;
        }
        return flag;
    }
    private void _pOderTableBind(DataSet ds)
    {
        DataSet dsBind = new DataSet();
        DataTable dt = new DataTable();
        try
        {
            string EditVal = string.Empty;

            if (Request.QueryString["ID"] != null)
            {
                EditVal = Convert.ToString(Request.QueryString["ID"]);
            }
            if (EditVal.Equals("") && !isNotEditables)
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
            }
            else if (isNotEditables)
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
            }
            else
            {
                PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
            }


            //  PXMLPATH = "../../Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C";
            if (File.Exists(PXMLPATH))
            {
                dsBind.ReadXml(PXMLPATH);
                if (dsBind.Tables.Count > 0)
                {
                    dt = dsBind.Tables[0];
                    FindFields(dt);
                }
            }
        }
        catch { }
    }
    private void FindFields(DataTable dt)
    {
        ASPxComboBox CmbpOrder_Branch = (ASPxComboBox)Parent.FindControl("CmbpOrder_Branch");
        ASPxComboBox ParentOrderNo = (ASPxComboBox)Parent.FindControl("ParentOrderNo");
        ASPxDateEdit txtTaxRates_DateFrom = (ASPxDateEdit)Parent.FindControl("txtTaxRates_DateFrom");
        ASPxComboBox CmbddlOrderType = (ASPxComboBox)Parent.FindControl("CmbddlOrderType");
        TextBox txttype_UserAccount = (TextBox)Parent.FindControl("txttype_UserAccount");
        TextBox txttype_UserAccount_hidden = (TextBox)Parent.FindControl("txttype_UserAccount_hidden");
        ASPxTextBox txtOrder_RefNumber = (ASPxTextBox)Parent.FindControl("txtOrder_RefNumber");
        TextBox txt_pOrder_AgentID = (TextBox)Parent.FindControl("txt_pOrder_AgentID");
        TextBox txt_pOrder_AgentID_hidden = (TextBox)Parent.FindControl("txt_pOrder_AgentID_hidden");
        ASPxMemo ASPxMemo1 = (ASPxMemo)Parent.FindControl("ASPxMemo1");
        ASPxComboBox CmbpOrder_PaymentTerm = (ASPxComboBox)Parent.FindControl("CmbpOrder_PaymentTerm");
        ASPxDateEdit ASPxDateEdit1 = (ASPxDateEdit)Parent.FindControl("ASPxDateEdit1");
        ASPxDateEdit txtpOrder_PaymentDate = (ASPxDateEdit)Parent.FindControl("txtpOrder_PaymentDate");
        ASPxComboBox ddlDeliveryAt = (ASPxComboBox)Parent.FindControl("ddlDeliveryAt_pOrderType");
        ASPxComboBox CmbpOrder_DeliveryBranch = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryBranch");
        ASPxComboBox CmbpOrder_DeliveryWareHouse = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryWareHouse");
        ASPxComboBox CmbpOrder_DeliveryAddress = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryAddress");
        ASPxMemo txtpOrder_DeliveryOther = (ASPxMemo)Parent.FindControl("txtpOrder_DeliveryOther");
        HiddenField pOderId_hidden = (HiddenField)Parent.FindControl("pOderId_hidden");
        ASPxTextBox txtpOrder_PaymentDays = (ASPxTextBox)Parent.FindControl("txtpOrder_PaymentDays");
        if (dt.Rows.Count >= 1)
        {
            pOderId_hidden.Value = Convert.ToString(dt.Rows[0]["pOrderBranch"]);
            CmbpOrder_Branch.Value = Convert.ToString(dt.Rows[0]["pOrderBranch"]);
            txtTaxRates_DateFrom.Date = Convert.ToDateTime(dt.Rows[0]["pOrderDate"]);
            CmbddlOrderType.Value = Convert.ToString(dt.Rows[0]["pOrderType"]);
            txttype_UserAccount.Text = Convert.ToString(dt.Rows[0]["contactname"]);
            txttype_UserAccount_hidden.Text = Convert.ToString(dt.Rows[0]["pOrderContactID"]);
            txtOrder_RefNumber.Value = Convert.ToString(dt.Rows[0]["pOrderRefNumber"]);
            txt_pOrder_AgentID.Text = Convert.ToString(dt.Rows[0]["agentname"]);
            txt_pOrder_AgentID_hidden.Text = Convert.ToString(dt.Rows[0]["pOrderAgentID"]);
            ASPxMemo1.Value = Convert.ToString(dt.Rows[0]["pOrderInstructions"]);
            CmbpOrder_PaymentTerm.Value = Convert.ToString(dt.Rows[0]["pOrderPaymentTerm"]);
            ASPxDateEdit1.Date = Convert.ToDateTime(dt.Rows[0]["pOrderDeliveryDate"]);
            txtpOrder_PaymentDate.Date = Convert.ToDateTime(dt.Rows[0]["pOrderPaymentDate"]);
            ddlDeliveryAt.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryAt"]);
            CmbpOrder_DeliveryBranch.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryBranch"]);
            CmbpOrder_DeliveryWareHouse.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryWareHouse"]);
            CmbpOrder_DeliveryAddress.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryAddress"]);
            txtpOrder_DeliveryOther.Value = Convert.ToString(dt.Rows[0]["pOrderDeliveryOther"]);
            txtpOrder_PaymentDays.Value = Convert.ToString(dt.Rows[0]["pOrder_PaymentDays"]);
            ParentOrderNo.Value = Convert.ToString(dt.Rows[0]["pOrderParentRefNumber"]);

            string ClientId = Convert.ToString(dt.Rows[0]["pOrderContactID"]);
            if (ClientId != "")
            {
                BindCmbpOrder_DeliveryAddress(ClientId);

            }
            DataTable dtValueSet = BindCmbpOrder_DeliveryAddress(ClientId);
            if (dtValueSet.Rows.Count > 0)
            {
                CmbpOrder_DeliveryAddress.Value = Convert.ToString(dtValueSet.Rows[0]["add_id"]);
            }
        }
    }
    private void ClearFieldsValue()
    {
        ASPxComboBox CmbpOrder_Branch = (ASPxComboBox)Parent.FindControl("CmbpOrder_Branch");
        ASPxDateEdit txtTaxRates_DateFrom = (ASPxDateEdit)Parent.FindControl("txtTaxRates_DateFrom");
        ASPxComboBox CmbddlOrderType = (ASPxComboBox)Parent.FindControl("CmbddlOrderType");
        TextBox txttype_UserAccount = (TextBox)Parent.FindControl("txttype_UserAccount");
        TextBox txttype_UserAccount_hidden = (TextBox)Parent.FindControl("txttype_UserAccount_hidden");
        ASPxTextBox txtOrder_RefNumber = (ASPxTextBox)Parent.FindControl("txtOrder_RefNumber");
        TextBox txt_pOrder_AgentID = (TextBox)Parent.FindControl("txt_pOrder_AgentID");
        TextBox txt_pOrder_AgentID_hidden = (TextBox)Parent.FindControl("txt_pOrder_AgentID_hidden");
        ASPxMemo ASPxMemo1 = (ASPxMemo)Parent.FindControl("ASPxMemo1");
        ASPxComboBox CmbpOrder_PaymentTerm = (ASPxComboBox)Parent.FindControl("CmbpOrder_PaymentTerm");
        ASPxDateEdit ASPxDateEdit1 = (ASPxDateEdit)Parent.FindControl("ASPxDateEdit1");
        ASPxDateEdit txtpOrder_PaymentDate = (ASPxDateEdit)Parent.FindControl("txtpOrder_PaymentDate");
        ASPxComboBox ddlDeliveryAt = (ASPxComboBox)Parent.FindControl("ddlDeliveryAt");
        ASPxComboBox CmbpOrder_DeliveryBranch = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryBranch");
        ASPxComboBox CmbpOrder_DeliveryWareHouse = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryWareHouse");
        ASPxComboBox CmbpOrder_DeliveryAddress = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryAddress");
        ASPxMemo txtpOrder_DeliveryOther = (ASPxMemo)Parent.FindControl("txtpOrder_DeliveryOther");
        HiddenField pOderId_hidden = (HiddenField)Parent.FindControl("pOderId_hidden");

        CmbpOrder_Branch.Value = "";
        txtTaxRates_DateFrom.Text = "";
        CmbddlOrderType.Value = "";
        txttype_UserAccount.Text = "";
        txttype_UserAccount_hidden.Text = "";
        txtOrder_RefNumber.Value = "";
        txt_pOrder_AgentID.Text = "";
        txt_pOrder_AgentID_hidden.Text = "";
        ASPxMemo1.Value = "";
        CmbpOrder_PaymentTerm.Value = "";
        ASPxDateEdit1.Text = "";
        txtpOrder_PaymentDate.Text = "";
        //ddlDeliveryAt.Value = "";
        CmbpOrder_DeliveryBranch.Value = "";
        CmbpOrder_DeliveryWareHouse.Value = "";
        CmbpOrder_DeliveryAddress.Value = "";
        txtpOrder_DeliveryOther.Value = "";
        // pOderId_hidden.Value = Convert.ToString(TableId);

    }
    protected DataTable BindCmbpOrder_DeliveryAddress(string clntid)
    {
        //  / oGenericMethod = new GenericMethod();
        string WhereClouse = string.Empty;
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        TextBox txttype_UserAccount_hidden = (TextBox)Parent.FindControl("txttype_UserAccount_hidden");
        string ClientId;// = txttype_UserAccount_hidden.Text.Trim();
        if (clntid != "" && clntid != null)
        { ClientId = clntid; }
        else
        { ClientId = txttype_UserAccount_hidden.Text.Trim(); }

        if (!ClientId.Equals(""))
        {
            WhereClouse = " and add_cntId = (select cnt_internalId from tbl_master_contact where cnt_id =" + ClientId + ")";
        }
        DataTable dtCmb = new DataTable();
        dtCmb = oGenericMethod.GetDataTable("select add_id,add_addressType + ' ('+ SUBSTRING (add_address1,1,5) +'..)' as add_addressType from tbl_master_address where add_address1<>''" + WhereClouse);
        AspxHelper oAspxHelper = new AspxHelper();
        ASPxComboBox CmbpOrder_DeliveryAddress = (ASPxComboBox)Parent.FindControl("CmbpOrder_DeliveryAddress");
        if (dtCmb.Rows.Count > 0)
        {
            oAspxHelper.Bind_Combo(CmbpOrder_DeliveryAddress, dtCmb, "add_addressType", "add_id", 0);
            CmbpOrder_DeliveryAddress.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
            CmbpOrder_DeliveryAddress.SelectedIndex = 0;
        }
        else
        {
            CmbpOrder_DeliveryAddress.DataSource = null;
            CmbpOrder_DeliveryAddress.DataBind();
        }

        return dtCmb;
    }
    private void PageloadGridBind()
    {
        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DataSet dsToAddXML = new DataSet();
        HiddenField pOrderHiddenField = (HiddenField)Parent.FindControl("pOderId_hidden");


        string EditVal = string.Empty;
        if (Request.QueryString["ID"] != null)
        {
            EditVal = Convert.ToString(Request.QueryString["ID"]);
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");

        }
        else if (isNotEditables)
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
        }
        else
        {
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
        }

        if (File.Exists(PXMLPATH))
        {
            if (dsToAddXML.Tables.Count > 1)
            {
                dsToAddXML.Tables.Remove(dsToAddXML.Tables[1]);
                dsToAddXML.Clear();
            }
            dsToAddXML.ReadXml(PXMLPATH);
            BindGrid(dsToAddXML, "DESC");
        }
    }
    private string CurrentCompany()
    {
        string CompanyInterId = string.Empty;
        DataTable dt = oGenericMethod.GetDataTable(" Select  (select top 1 cmp_internalid from tbl_master_company where cmp_internalid=ls_lastCompany)  as CompanyInternalId from  tbl_trans_LastSegment WHERE  ls_userId='" + Convert.ToString(Session["userid"]) + "'" + " and ls_lastSegment='" + Convert.ToString(Session["userlastsegment"]) + "'");
        if (dt.Rows.Count > 0)
        {
            CompanyInterId = Convert.ToString(dt.Rows[0]["CompanyInternalId"]);
        }
        return CompanyInterId;
    }

    private void CurrencyBind()
    {
        txtQuoteCurrency.Text = "Indian Rupee";
        txtQuoteCurrency_hidden.Text = "1";
    }
}






