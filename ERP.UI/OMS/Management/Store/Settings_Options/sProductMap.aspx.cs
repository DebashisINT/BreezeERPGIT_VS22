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
using System.Xml.Linq;
////using DevExpress.Web.ASPxClasses;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;

namespace ERP.OMS.Management.Store.Settings_Options
{
    public partial class management_master_Store_sProductClass : System.Web.UI.Page
    {


        public string pageAccess = "";
        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/sProductMap.aspx");
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;
            cityGrid.JSProperties["cpInsertionValid"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                // BindCmbTaxSlab_Code();
                //BindCmbTaxRates_SurchargeApplicable();
                //BindCmbtxtTaxRates_SurchargeCriteria();
                //BindCmbTaxRates_SurchargeOn();
                SetEmpname();
                SetProduct();
            }
            BindGrid();
        }
        public void SetEmpname()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_contact", "  ltrim(rtrim(cnt_firstName)) Name,ltrim(rtrim(cnt_internalId)) Code ", null);
            lstEmpname.DataSource = DT;
            lstEmpname.DataMember = "Code";
            lstEmpname.DataTextField = "Name";
            lstEmpname.DataValueField = "Code";
            lstEmpname.DataBind();

        }


        public void SetProduct()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("Master_sProducts", "  ltrim(rtrim(sProducts_Name)) Name,ltrim(rtrim(sProducts_ID)) Code ", null);
            lstProduct.DataSource = DT;
            lstProduct.DataMember = "Code";
            lstProduct.DataTextField = "Name";
            lstProduct.DataValueField = "Code";
            lstProduct.DataBind();

        }



        //protected void BindCmbTaxSlab_Code()
        //{
        //    //  / oGenericMethod = new GenericMethod();
        //    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        //    DataTable dtCmb = new DataTable();
        //    dtCmb = oGenericMethod.GetDataTable("select distinct TaxSlab_Code from Master_TaxSlab order By TaxSlab_Code");
        //    AspxHelper oAspxHelper = new AspxHelper();
        //    if (dtCmb.Rows.Count > 0)
        //        oAspxHelper.Bind_Combo(CmbTaxSlab_Code, dtCmb, "TaxSlab_Code", "TaxSlab_Code", 0);

        //    CmbTaxSlab_Code.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));      ///.Insert(0, new ListItem("Select Country", "0"));
        //}




        //protected void BindCmbTaxRates_SurchargeApplicable()
        //{

        //    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        //    DataTable dtCmb = new DataTable();

        //    dtCmb.Columns.Add("id");
        //    dtCmb.Columns.Add("name");
        //    DataRow drsession = dtCmb.NewRow();
        //    drsession["id"] = "Y";
        //    drsession["name"] = "Yes";
        //    dtCmb.Rows.Add(drsession);

        //    drsession = dtCmb.NewRow();
        //    drsession["id"] = "N";
        //    drsession["name"] = "No";
        //    dtCmb.Rows.Add(drsession);
        //    //dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Code as name from Master_Taxes order By Taxes_Code");

        //    AspxHelper oAspxHelper = new AspxHelper();
        //    if (dtCmb.Rows.Count > 0)
        //        oAspxHelper.Bind_Combo(CmbTaxRates_SurchargeApplicable, dtCmb, "name", "id", 0);

        //    CmbTaxRates_SurchargeApplicable.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));

        //}

        //protected void BindCmbTaxRates_SurchargeOn()
        //{
        //    DataTable dtCmb = new DataTable();

        //    dtCmb.Columns.Add("id");
        //    dtCmb.Columns.Add("name");
        //    DataRow drsession = dtCmb.NewRow();
        //    drsession["id"] = "F";
        //    drsession["name"] = "Full amount";
        //    dtCmb.Rows.Add(drsession);

        //    drsession = dtCmb.NewRow();
        //    drsession["id"] = "D";
        //    drsession["name"] = "Differential amount";
        //    dtCmb.Rows.Add(drsession);
        //    //dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Code as name from Master_Taxes order By Taxes_Code");

        //    AspxHelper oAspxHelper = new AspxHelper();
        //    if (dtCmb.Rows.Count > 0)
        //        oAspxHelper.Bind_Combo(CmbTaxRates_SurchargeOn, dtCmb, "name", "id", 0);

        //    CmbTaxRates_SurchargeOn.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        //}

        //protected void BindCmbtxtTaxRates_SurchargeCriteria()
        //{
        //    DataTable dtCmb = new DataTable();

        //    dtCmb.Columns.Add("id");
        //    dtCmb.Columns.Add("name");
        //    DataRow drsession = dtCmb.NewRow();
        //    drsession["id"] = "G";
        //    drsession["name"] = "Gross Value";
        //    dtCmb.Rows.Add(drsession);

        //    drsession = dtCmb.NewRow();
        //    drsession["id"] = "T";
        //    drsession["name"] = "Tax component";
        //    dtCmb.Rows.Add(drsession);
        //    //dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Code as name from Master_Taxes order By Taxes_Code");

        //    AspxHelper oAspxHelper = new AspxHelper();
        //    if (dtCmb.Rows.Count > 0)
        //        oAspxHelper.Bind_Combo(CmbtxtTaxRates_SurchargeCriteria, dtCmb, "name", "id", 0);

        //    CmbtxtTaxRates_SurchargeCriteria.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        //}

        protected void BindGrid()
        {


            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable(@" SELECT A.[pCodeMap_ID],
                                                           B.[cnt_firstName],
                                                           A.[pCodeMap_ContactID],
                                                           A.[pCodeMap_ProductID],
                                                           C.[sProducts_Name],
                                                           A.[pCodeMap_Code],
                                                           A.[pCodeMap_CreateUser],
                                                           A.[pCodeMap_CreateDate],
                                                           A.[pCodeMap_ModifyUser],
                                                           A.[pCodeMap_ModifyTime] 

                                                    FROM [dbo].[Trans_pCodeMap] A 

                                                        JOIN [tbl_master_contact] B ON A.[pCodeMap_ContactID] = B.[cnt_internalId]
                                                        JOIN [dbo].[Master_sProducts] C ON A.[pCodeMap_ProductID] = C.[sProducts_ID] ");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                cityGrid.DataSource = dtFillGrid;
                cityGrid.DataBind();
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
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
            cityGrid.JSProperties["cpInsertionValid"] = null;


            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;


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

                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                if (1 == 1)
                {
                    string sTaxRates_MainAccount = txtTaxRates_MainAccount_hidden.Text.Split('-')[0] != null && txtTaxRates_MainAccount_hidden.Text.Split('-')[0] != "" ? txtTaxRates_MainAccount_hidden.Text.Split('-')[0] : "NULL";

                    string sTaxRates_SubAccount = "NULL";

                    if (txtTaxRates_SubAccount_hidden.Text != null && txtTaxRates_SubAccount_hidden.Text != "")
                    {
                        sTaxRates_SubAccount = txtTaxRates_SubAccount_hidden.Text.Trim();
                    }

                    //string TaxRates_MinAmount = txtTaxRates_MinAmount.Text == "" ? "0.0" : txtTaxRates_MinAmount.Text;

                    insertcount = oGenericMethod.Insert_Table("Trans_pCodeMap", "pCodeMap_ContactID,pCodeMap_ProductID,pCodeMap_Code,pCodeMap_CreateUser,pCodeMap_CreateDate",
                            "'" + txtTaxRates_MainAccount_hidden.Text.Trim() + "','" + txtTaxRates_SubAccount_hidden.Text.Trim() + "','" + txtTaxes_SurchargeRate.Text.Trim() + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "',getdate()");

                    if (insertcount > 0)
                    {
                        cityGrid.JSProperties["cpinsert"] = "Success";
                        BindGrid();
                    }
                    else
                        cityGrid.JSProperties["cpinsert"] = "fail";

                }
                else
                    cityGrid.JSProperties["cpUpdateValid"] = "dateInvalid";

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
                updtcnt = oGenericMethod.Update_Table("Trans_pCodeMap", "pCodeMap_ContactID ='" + txtTaxRates_MainAccount_hidden.Text.Trim() + "', pCodeMap_ProductID ='" + txtTaxRates_SubAccount_hidden.Text.Trim() + "', pCodeMap_Code ='" + txtTaxes_SurchargeRate.Text.Trim() + "', pCodeMap_ModifyUser  ='" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "', pCodeMap_ModifyTime  =getdate()", "pCodeMap_ID=" + WhichType + "");
                if (updtcnt > 0)
                {
                    cityGrid.JSProperties["cpUpdate"] = "Success";
                    BindGrid();
                }
                else
                    cityGrid.JSProperties["cpUpdate"] = "fail";
                //}
                //else
                //    cityGrid.JSProperties["cpUpdateValid"] = "StateInvalid";


            }
            if (WhichCall == "Delete")
            {
                deletecnt = oGenericMethod.Delete_Table("Trans_pCodeMap", "pCodeMap_ID=" + WhichType + "");
                if (deletecnt > 0)
                {
                    cityGrid.JSProperties["cpDelete"] = "Success";
                    BindGrid();
                }
                else
                    cityGrid.JSProperties["cpDelete"] = "Fail";
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable(@" SELECT A.[pCodeMap_ID],
                                                           B.[cnt_firstName],
                                                           A.[pCodeMap_ContactID],
                                                           A.[pCodeMap_ProductID],
                                                           C.[sProducts_Name],
                                                           A.[pCodeMap_Code],
                                                           A.[pCodeMap_CreateUser],
                                                           A.[pCodeMap_CreateDate],
                                                           A.[pCodeMap_ModifyUser],
                                                           A.[pCodeMap_ModifyTime] 

                                                    FROM [dbo].[Trans_pCodeMap] A 

                                                        JOIN [tbl_master_contact] B ON A.[pCodeMap_ContactID] = B.[cnt_internalId]
                                                        JOIN [dbo].[Master_sProducts] C ON A.[pCodeMap_ProductID] = C.[sProducts_ID] 
 
                                                      Where pCodeMap_ID=" + WhichType + "");

                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string cnt_firstName = dtEdit.Rows[0]["cnt_firstName"].ToString();
                    string pCodeMap_ContactID = dtEdit.Rows[0]["pCodeMap_ContactID"].ToString();

                    string pCodeMap_ProductID = dtEdit.Rows[0]["pCodeMap_ProductID"].ToString();

                    string sProducts_Name = dtEdit.Rows[0]["sProducts_Name"].ToString();

                    string pCodeMap_Code = dtEdit.Rows[0]["pCodeMap_Code"].ToString();

                    cityGrid.JSProperties["cpEdit"] = cnt_firstName + "~" + pCodeMap_ContactID + "~" + pCodeMap_ProductID + "~"
                        + sProducts_Name + "~" + pCodeMap_Code + "~" + WhichType;
                }
            }

            BindGrid();
        }



    }
}