using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Store.Master
{
    public partial class management_master_Store_sMarkets : System.Web.UI.Page
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
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/store/Master/sMarkets.aspx");

            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                BindCountry();
                // BindState(1);
                
            }
            BindGrid();
        }

        protected void BindCountry()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select cou_id as id,cou_country as name from tbl_master_country order By cou_country");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbCountryName, dtCmb, "name", "id", "");//oAspxHelper.Bind_Combo(CmbCountryName, dtCmb, "name", "id", "India");

        }

        protected void BindState(int countryID)
        {
            CmbState.Items.Clear();

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("Select id,state as name From tbl_master_STATE Where countryID=" + countryID + " Order By Name");//+ " Order By state "
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //CmbState.Enabled = true;
                oAspxHelper.Bind_Combo(CmbState, dtCmb, "name", "id");
            }
            else
                CmbState.Enabled = false;
        }
        protected void BindCity(int stateID)
        {
            //CmbCity.Items.Clear();

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("Select city_id,city_name From tbl_master_city Where state_id=" + stateID + " Order By city_name");//+ " Order By state "
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //CmbState.Enabled = true;
                oAspxHelper.Bind_Combo(CmbCity, dtCmb, "city_name", "city_id",null);
            }
            else
                CmbCity.Enabled = false;
        }

        protected void BindGrid()
        {

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();

            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oStore_MasterBL.GetMarketList();
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                cityGrid.DataSource = dtFillGrid;
                cityGrid.DataBind();
            }
        }
        public void bindexport(int Filter)
        {
            cityGrid.Columns[10].Visible = false;
            //SchemaGrid.Columns[11].Visible = false;
            //SchemaGrid.Columns[12].Visible = false;
            string filename = "Markets";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Markets";
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
            cityGrid.JSProperties["cpUpdateValid"] = null;

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
                cityGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
                cityGrid.FilterExpression = string.Empty;

            if (WhichCall == "savecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();


                int cityID = 0;
                if (CmbCity.Items.Count > 0)
                    if (CmbCity.SelectedItem != null)
                        cityID = Convert.ToInt32(Convert.ToString(CmbCity.SelectedItem.Value));
                //if (cityID != 0)
                //{

                string[,] countrecord = oGenericMethod.GetFieldValue("Master_Markets", "Markets_Code", "Markets_Code='" + txtMarkets_Code.Text + "'", 1);

                if (countrecord[0, 0] != "n")
                    cityGrid.JSProperties["cpExists"] = "Exists";
                else
                {
                    //if (CmbCountryName.SelectedIndex!=-1)
                    //{
                    //    if()
                    //}
                    insertcount = oStore_MasterBL.InsertsMarket(txtMarkets_Code.Text, Convert.ToInt32(CmbCountryName.SelectedItem.Value), Convert.ToInt32(hdnStateId.Value), Convert.ToInt32(hdnCityId.Value),
                                            txtMarkets_Name.Text, txtMarkets_Description.Text, txtMarkets_Address.Text, txtMarkets_Email.Text,
                                            txtMarkets_Phones.Text, txtMarkets_WebSite.Text, txtMarkets_ContactPerson.Text, Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    if (insertcount > 0)
                    {
                        cityGrid.JSProperties["cpinsert"] = "Success";
                        BindGrid();
                    }
                    else
                        cityGrid.JSProperties["cpinsert"] = "fail";
                }
                //}
                //else
                //{
                //    //cityGrid.JSProperties["cpUpdateValid"] = "StateInvalid";
                //}
            }
            if (WhichCall == "updatecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                int stateID = 0;
                if (Convert.ToString(hdnStateId.Value) != string.Empty)
                {
                    stateID = Convert.ToInt32(hdnStateId.Value);
                }


                if (stateID != 0)
                {

                    updtcnt = oStore_MasterBL.UpdatesMarket(Convert.ToInt32(WhichType), txtMarkets_Code.Text, Convert.ToInt32(CmbCountryName.SelectedItem.Value)
                                            , Convert.ToInt32(hdnStateId.Value), Convert.ToInt32(hdnCityId.Value),
                                            txtMarkets_Name.Text, txtMarkets_Description.Text, txtMarkets_Address.Text, txtMarkets_Email.Text,
                                            txtMarkets_Phones.Text, txtMarkets_WebSite.Text, txtMarkets_ContactPerson.Text,
                                            Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    if (updtcnt > 0)
                    {
                        cityGrid.JSProperties["cpUpdate"] = "Success";
                        BindGrid();
                    }
                    else
                        cityGrid.JSProperties["cpUpdate"] = "fail";
                }
                else
                    cityGrid.JSProperties["cpUpdateValid"] = "StateInvalid";


            }
            if (WhichCall == "Delete")
            {
                deletecnt = oStore_MasterBL.InsertsMarketLog(Convert.ToInt32(WhichType));
                deletecnt = oGenericMethod.Delete_Table("Master_Markets", "Markets_ID=" + WhichType + "");
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
                DataTable dtEdit = oStore_MasterBL.GetMarketListById(Convert.ToInt32(WhichType));
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string Markets_Code = Convert.ToString(dtEdit.Rows[0]["Markets_Code"]);
                    string Markets_Name = Convert.ToString(dtEdit.Rows[0]["Markets_Name"]);
                    string Markets_Description = Convert.ToString(dtEdit.Rows[0]["Markets_Description"]);

                    string Markets_Address = Convert.ToString(dtEdit.Rows[0]["Markets_Address"]);
                    string Markets_Email = Convert.ToString(dtEdit.Rows[0]["Markets_Email"]);
                    string Markets_Phones = Convert.ToString(dtEdit.Rows[0]["Markets_Phones"]);
                    string Markets_WebSite = Convert.ToString(dtEdit.Rows[0]["Markets_WebSite"]);
                    string Markets_ContactPerson = Convert.ToString(dtEdit.Rows[0]["Markets_ContactPerson"]);

                    string Markets_State = Convert.ToString(dtEdit.Rows[0]["Markets_State"]);
                    string Markets_City = Convert.ToString(dtEdit.Rows[0]["Markets_City"]);

                    string Markets_Country = Convert.ToString(dtEdit.Rows[0]["Markets_Country"]);

                    cityGrid.JSProperties["cpEdit"] = Markets_Code + "~" + Markets_Name + "~" + Markets_Description + "~" + Markets_Address + "~" + Markets_Email + "~" + Markets_Phones + "~" + Markets_WebSite + "~" + Markets_ContactPerson + "~" + Markets_State + "~" + Markets_City + "~" + Markets_Country + "~" + WhichType;
                }
            }
        }

        protected void CmbState_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindState")
            {
                int countryID = Convert.ToInt32(Convert.ToString(e.Parameter.Split('~')[1]));
                BindState(countryID);
            }
        }

        protected void CmbCity_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindCity")
            {
                int stateId = Convert.ToInt32(Convert.ToString(e.Parameter.Split('~')[1]));
                BindCity(stateId);
            }
        }

        [WebMethod]
        public static bool CheckUniqueName(string MarketsCode, int procode)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                DataTable dtCmb = new DataTable();
                if(procode==1)
                { 
                dtCmb = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Markets] WHERE [Markets_Code] = " + "'" + MarketsCode + "'");
                }
                else
                {
                    dtCmb = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Markets] WHERE [Markets_Code] = " + "'" + MarketsCode + "' and Markets_ID!=" + procode + " ");
                }
                    int cnt = dtCmb.Rows.Count;
                if (cnt > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

    }







    //using System;
    //using System.Web;
    //using DevExpress.Web;
    //using BusinessLogicLayer;
    //using System.Data;
    //using System.Web.UI;
    //////using DevExpress.Web.ASPxClasses;
    //using System.Web.Services;
    //using System.Text;

    //public partial class management_master_Store_sMarkets : System.Web.UI.Page
    //{
    //    public string pageAccess = "";
    //  //  DBEngine oDBEngine = new DBEngine(string.Empty);
    //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
    //    BusinessLogicLayer.GenericMethod oGenericMethod;
    //    protected void Page_PreInit(object sender, EventArgs e)
    //    {
    //        if (!IsPostBack)
    //        {
    //            //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
    //            string sPath = HttpContext.Current.Request.Url.ToString();
    //            oDBEngine.Call_CheckPageaccessebility(sPath);
    //        }
    //    }
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
    //    }
    //    protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
    //        switch (Filter)
    //        {
    //            case 1:
    //                exporter.WritePdfToResponse();
    //                break;
    //            case 2:
    //                exporter.WriteXlsToResponse();
    //                break;
    //            case 3:
    //                exporter.WriteRtfToResponse();
    //                break;
    //            case 4:
    //                exporter.WriteCsvToResponse();
    //                break;
    //        }
    //    }
    //    protected void marketsGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
    //    {
    //        if (e.RowType == GridViewRowType.Data)
    //        {
    //            int commandColumnIndex = -1;
    //            for (int i = 0; i < marketsGrid.Columns.Count; i++)
    //                if (marketsGrid.Columns[i] is GridViewCommandColumn)
    //                {
    //                    commandColumnIndex = i;
    //                    break;
    //                }
    //            if (commandColumnIndex == -1)
    //                return;
    //            //____One colum has been hided so index of command column will be leass by 1 
    //            commandColumnIndex = commandColumnIndex - 5;
    //            DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
    //            for (int i = 0; i < cell.Controls.Count; i++)
    //            {
    //                DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
    //                if (button == null) return;
    //                DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

    //                if (hyperlink.Text == "Delete")
    //                {
    //                    if (Session["PageAccess"].ToString() == "DelAdd" || Session["PageAccess"].ToString() == "Delete" || Session["PageAccess"].ToString() == "All")
    //                    {
    //                        hyperlink.Enabled = true;
    //                        continue;
    //                    }
    //                    else
    //                    {
    //                        hyperlink.Enabled = false;
    //                        continue;
    //                    }
    //                }


    //            }

    //        }

    //    }
    //    protected void marketsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
    //    {
    //        if (!marketsGrid.IsNewRowEditing)
    //        {
    //            ASPxGridViewTemplateReplacement RT = marketsGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
    //            if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
    //                RT.Visible = true;
    //            else
    //                RT.Visible = false;
    //        }

    //    }
    //    protected void marketsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    //    {
    //        if (e.Parameters == "s")
    //            marketsGrid.Settings.ShowFilterRow = true;

    //        if (e.Parameters == "All")
    //        {
    //            marketsGrid.FilterExpression = string.Empty;
    //        }
    //    }

    //     [WebMethod]

    //    public static string GetStateListBycountryId(string countryid)
    //    {
    //        StringBuilder strStates = new StringBuilder();
    //        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
    //        try
    //        {
    //            DataTable dtCmb = new DataTable();
    //            dtCmb = oGenericMethod.GetDataTable("select [State_ID],[State_Name] from Master_States ms  inner join tbl_master_country tmc on tmc.cou_id=ms.[State_CountryID]  where tmc.[cou_country]=LTRIM (RTRIM (" + countryid + ")) order by State_Name");
    //            int cnt = dtCmb.Rows.Count;
    //            if (dtCmb.Rows.Count>0)
    //            {
    //                int i = 0;
    //                strStates.Append("[");
    //                foreach (DataRow dr in dtCmb.Rows)
    //                {
    //                    if (i == cnt - 1)
    //                    {
    //                        strStates.Append("{");
    //                        strStates.Append("\"statename\":\"" + dr["State_Name"] + "\",");
    //                        strStates.Append("\"ID\":\"" + dr["State_ID"] + "\"");
    //                        strStates.Append("}");
    //                    }
    //                    else
    //                    {
    //                        strStates.Append("{");
    //                        strStates.Append("\"statename\":\"" + dr["State_Name"] + "\",");
    //                        strStates.Append("\"ID\":\"" + dr["State_ID"] + "\"");
    //                        strStates.Append("},");
    //                    }
    //                    i++;
    //                }
    //            }
    //            strStates.Append("]");
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //        finally
    //        {
    //        }

    //        return strStates.ToString();
    //    }
    //}
}