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
using DevExpress.Web;
using System.Collections.Generic;


namespace ERP.OMS.Management.Master
{
    public partial class Management_Accounts_Master_TaxesLevies : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        BusinessLogicLayer.MasterTaxBl masterTaxBl = new BusinessLogicLayer.MasterTaxBl();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'

                // Replace .ToString() with Convert.ToString(..) By Sudip on 15122016

                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/TaxesLevies.aspx");

            TaxLeviesGrid.JSProperties["cpinsert"] = null;
            TaxLeviesGrid.JSProperties["cpEdit"] = null;
            TaxLeviesGrid.JSProperties["cpUpdate"] = null;
            TaxLeviesGrid.JSProperties["cpDelete"] = null;
            TaxLeviesGrid.JSProperties["cpExists"] = null;
            TaxLeviesGrid.JSProperties["cpUpdateValid"] = null;
            TaxLeviesGrid.JSProperties["cpShowInDesign"] = null;
            TaxLeviesGrid.JSProperties["cpUpdateShowInDesign"] = null;

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    Session["exportval"] = null;
                    BindCmbTaxes_ApplicableFor();
                    BindCmbTaxes_ApplicableOn();
                    BindCmbTXType();
                }
                BindGrid();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }

        protected void cmbtaxtype_CustomCallback(object source, CallbackEventArgsBase e)
        {
            
            BindCmbTXType();
        }
        protected void BindCmbTXType()
        {   
            //DataTable dt = oDBEngine.GetDataTable("select tax_Code,tax_Description from dbo.tbl_Master_TaxType");
            DataTable DT = masterTaxBl.GetTaxxDetails();
            var query = DT.AsEnumerable().Where(r => r.Field<string>("tax_Code").ToUpper() == "V" || r.Field<string>("tax_Code").ToUpper() == "G" || r.Field<string>("tax_Code").ToUpper() == "C");
            foreach (var row in query.ToList())
                row.Delete();
            DT.AcceptChanges();

            cmbtaxtype.DataSource = DT;
            cmbtaxtype.DataBind();
        }
        protected void BindCmbTaxes_ApplicableFor()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = "S";
            drsession["name"] = "Sales";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "P";
            drsession["name"] = "Purchases";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "B";
            drsession["name"] = "Both";
            dtCmb.Rows.Add(drsession);

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbTaxes_ApplicableFor, dtCmb, "name", "id", 0);

        }
        protected void BindCmbTaxes_ApplicableOn()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = "G";
            drsession["name"] = "Gross Value";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "N";
            drsession["name"] = "Net Value";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "O";
            drsession["name"] = "Other Tax Component";
            dtCmb.Rows.Add(drsession);


            drsession = dtCmb.NewRow();
            drsession["id"] = "R";
            drsession["name"] = "Running Total";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "A";
            drsession["name"] = "Any";
            dtCmb.Rows.Add(drsession);

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbTaxes_ApplicableOn, dtCmb, "name", "id", 0);

        }
        protected void BindGrid()
        {

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable(@" select b.[Taxes_ID]
                                                          ,b.[Taxes_Code]
                                                          ,b.[Taxes_Name]
                                                          ,(select a.Taxes_Name from Master_Taxes a  Where a.Taxes_ID= b.Taxes_OtherTax  ) as  OtherTax 
                                                          ,b.[Taxes_Description]
                                                          ,b.[Taxes_ApplicableFor]
                                                          , case when b.[Taxes_ApplicableFor]='S' then 'Sales' when  b.[Taxes_ApplicableFor]='P' then 'Purchases'  when  b.[Taxes_ApplicableFor]='B' then 'Both' end as Taxes_ApplicableFortxt
                                                          ,b.[Taxes_ApplicableOn]
                                                          ,b.[Taxes_OtherTax]
                                                          ,b.[Taxes_CreateUser]
                                                          ,b.[Taxes_CreateTime]
                                                          ,b.[Taxes_ModifyUser]
                                                          ,b.[Taxes_ModifyTime]
                                                            ,b.TaxTypeCode as TaxTypeCode
                                                            ,b.TaxItemlevel as TaxItemlevel
                                                            ,b.TaxCalculateMethods as TaxCalculateMethods
                                                            ,(select tax_Description from dbo.tbl_Master_TaxType where tax_Code= b.TaxTypeCode) as TaxTypeCodeG
                                                            ,case when b.TaxItemlevel='Y' then 'Yes' when b.TaxItemlevel='N' then 'No' else '' end as TaxItemlevelG
                                                            ,case when b.TaxCalculateMethods='A' then 'Added' when b.TaxCalculateMethods='L' then 'Less' else '' end as TaxCalculateMethodsG
                                                     from Master_Taxes b order by b.Taxes_ID desc");
            AspxHelper oAspxHelper = new AspxHelper();
           
                TaxLeviesGrid.DataSource = dtFillGrid;
                TaxLeviesGrid.DataBind();
             
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
            TaxLeviesGrid.Columns[8].Visible = false;
            string filename = "Tax and Levies";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Tax and Levies";
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
        protected void btnSearch(object sender, EventArgs e)
        {
            TaxLeviesGrid.Settings.ShowFilterRow = true;
        }
        protected void TaxLeviesGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 15122016

            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < TaxLeviesGrid.Columns.Count; i++)
                    if (TaxLeviesGrid.Columns[i] is GridViewCommandColumn)
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
                        //if (Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "Delete" || Session["PageAccess"].ToString().Trim() == "All")
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
        protected void TaxLeviesGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 15122016

            if (!TaxLeviesGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = TaxLeviesGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void TaxLeviesGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 15122016

            TaxLeviesGrid.JSProperties["cpinsert"] = null;
            TaxLeviesGrid.JSProperties["cpEdit"] = null;
            TaxLeviesGrid.JSProperties["cpUpdate"] = null;
            TaxLeviesGrid.JSProperties["cpDelete"] = null;
            TaxLeviesGrid.JSProperties["cpExists"] = null;
            TaxLeviesGrid.JSProperties["cpUpdateValid"] = null;

            TaxLeviesGrid.JSProperties["cpchkshowdesign"] = null;
            TaxLeviesGrid.JSProperties["cpShowInDesign"] = null;
            TaxLeviesGrid.JSProperties["cpUpdateShowInDesign"] = null;

            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;
            int showindesigncnt = 0;

            string TaxItemLevel = string.Empty;
            string TaxCalculate = string.Empty;

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

            if (e.Parameters == "s")
                TaxLeviesGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
                TaxLeviesGrid.FilterExpression = string.Empty;

            if (WhichCall == "copy")
            {
                DataTable ft = oDBEngine.GetDataTable("insert into Master_Taxes(Taxes_CreateTime,Taxes_Name,Taxes_Description,Taxes_ApplicableFor,Taxes_ApplicableOn,Taxes_OtherTax,Taxes_CreateUser,TaxTypeCode,TaxItemlevel,TaxCalculateMethods)"
            + "select GETDATE(),Taxes_Name,Taxes_Description,Taxes_ApplicableFor,Taxes_ApplicableOn,Taxes_OtherTax,'" + Convert.ToString(HttpContext.Current.Session["userid"]) + "',TaxTypeCode,TaxItemlevel,TaxCalculateMethods from Master_Taxes where Taxes_ID=" + WhichType + "");


                TaxLeviesGrid.JSProperties["cpinsert"] = "Success";
                BindGrid();


            }
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

                string[,] countrecord = oGenericMethod.GetFieldValue("Master_Taxes", "Taxes_Code", "Taxes_Code='" + txtTaxes_Code.Text + "'", 1);

                if (countrecord[0, 0] != "n")
                    TaxLeviesGrid.JSProperties["cpExists"] = "Exists";
                else
                {
                    if (Itemlevelrdp.SelectedItem != null)
                    {
                        TaxItemLevel = Convert.ToString(Itemlevelrdp.SelectedItem.Value);
                    }
                    if (Calculationmethodsrdp.SelectedItem != null)
                    {
                        TaxCalculate = Convert.ToString(Calculationmethodsrdp.SelectedItem.Value);
                    }

                    insertcount = oGenericMethod.Insert_Table("Master_Taxes", "Taxes_Code,Taxes_Name,Taxes_Description,Taxes_ApplicableFor,Taxes_ApplicableOn,Taxes_OtherTax,Taxes_CreateUser,Taxes_CreateTime,TaxTypeCode,TaxItemlevel,TaxCalculateMethods",
                       "'" + txtTaxes_Code.Text.Trim() + "','" + txtTaxes_Name.Text.Trim() + "','" + txtTaxes_Description.Text.Trim() + "','" + Convert.ToString(CmbTaxes_ApplicableFor.SelectedItem.Value) + "','" + Convert.ToString(CmbTaxes_ApplicableOn.SelectedItem.Value) + "','" + Convert.ToString(hndTaxes_OtherTax_hidden.Value) + "','" +
                        Convert.ToString(HttpContext.Current.Session["userid"]) + "',GETDATE(),'" + Convert.ToString(cmbtaxtype.SelectedItem.Value) + "','" + TaxItemLevel + "','" + TaxCalculate + "'");

                    if (insertcount > 0)
                    {
                        TaxLeviesGrid.JSProperties["cpinsert"] = "Success";
                        BindGrid();
                    }
                    else
                        TaxLeviesGrid.JSProperties["cpinsert"] = "fail";
                }
                //}
                //else
                //    cityGrid.JSProperties["cpUpdateValid"] = "StateInvalid";
            }
            if (WhichCall == "updatecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                if (Itemlevelrdp.SelectedItem != null)
                {
                    TaxItemLevel = Convert.ToString(Itemlevelrdp.SelectedItem.Value);
                }
                if (Calculationmethodsrdp.SelectedItem != null)
                {
                    TaxCalculate = Convert.ToString(Calculationmethodsrdp.SelectedItem.Value);
                }
                updtcnt = oGenericMethod.Update_Table("Master_Taxes", "TaxTypeCode='" + Convert.ToString(cmbtaxtype.SelectedItem.Value) + "',TaxItemlevel='" + TaxItemLevel + "',TaxCalculateMethods='" + TaxCalculate + "',Taxes_Code='" + txtTaxes_Code.Text.Trim() + "',Taxes_Name='" + txtTaxes_Name.Text.Trim() + "',Taxes_Description='" + txtTaxes_Description.Text.Trim() + "',Taxes_ApplicableFor='" + Convert.ToString(CmbTaxes_ApplicableFor.SelectedItem.Value) + "',Taxes_ApplicableOn='" + Convert.ToString(CmbTaxes_ApplicableOn.SelectedItem.Value) + "',Taxes_OtherTax='" + Convert.ToString(hndTaxes_OtherTax_hidden.Value) + "',Taxes_ModifyUser='" + Convert.ToString(HttpContext.Current.Session["userid"]) + "',Taxes_ModifyTime=GETDATE()", "Taxes_ID=" + WhichType + "");
                //" + txtMarkets_Code.Text + "  " + CmbState.SelectedItem.Value.ToString() + "
                if (updtcnt > 0)
                {
                    TaxLeviesGrid.JSProperties["cpUpdate"] = "Success";
                    BindGrid();
                }
                else
                    TaxLeviesGrid.JSProperties["cpUpdate"] = "fail";



            }
            if (WhichCall == "ShowInDesign")
            {
                string[,] countrecord = oGenericMethod.GetFieldValue("Master_Taxes", "ShowInDesign", "Taxes_ID='" + WhichType + "'", 1);
                TaxLeviesGrid.JSProperties["cpchkshowdesign"] = countrecord[0, 0];
                TaxLeviesGrid.JSProperties["cpShowInDesign"] = WhichType; 
            }
            if (WhichCall == "UpdateShowInDesign")
            {
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string showdesn = "";
                if (chkShowDesign.Checked == true)
                {
                    showdesn = "Y";
                }
                else
                {
                    showdesn = "N";
                }
                showindesigncnt = oGenericMethod.Update_Table("Master_Taxes", "ShowInDesign='" + Convert.ToString(showdesn) + "'", "Taxes_ID=" + WhichType + "");
                if (showindesigncnt > 0)
                {
                    TaxLeviesGrid.JSProperties["cpUpdateShowInDesign"] = "Success";
                    //BindGrid();
                }
                else
                    TaxLeviesGrid.JSProperties["cpUpdateShowInDesign"] = "fail";


            }


            if (WhichCall == "Delete")
            {
                try
                {

                    deletecnt = oGenericMethod.Delete_Table("Master_Taxes", "Taxes_ID=" + WhichType + "");

                    if (deletecnt > 0)
                    {
                        TaxLeviesGrid.JSProperties["cpDelete"] = "Success";
                        BindGrid();
                    }
                    else
                        TaxLeviesGrid.JSProperties["cpDelete"] = "Fail";
                }
                catch (Exception ex)
                {

                    TaxLeviesGrid.JSProperties["cpDelete"] = "FK";
                    TaxLeviesGrid.JSProperties["cpMsg"] = Convert.ToString(ConfigurationManager.AppSettings["DeleteErrorMessage"]);
                }
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable(@"select b.[Taxes_ID]
                                                                      ,b.[Taxes_Code]
                                                                      ,(select a.Taxes_Name from Master_Taxes a  Where a.Taxes_ID= b.Taxes_OtherTax  ) as  OtherTax 
                                                                      ,b.[Taxes_Name]       
                                                                      ,b.[Taxes_Description]
                                                                      ,b.[Taxes_ApplicableFor]
                                                                      ,b.[Taxes_ApplicableOn]
                                                                      ,b.[Taxes_OtherTax]
                                                                      ,b.[Taxes_CreateUser]
                                                                      ,b.[Taxes_CreateTime]
                                                                      ,b.[Taxes_ModifyUser]
                                                                      ,b.[Taxes_ModifyTime] 
                                                                         ,Ltrim(Rtrim(b.TaxTypeCode)) as TaxTypeCode
                                                            ,b.TaxItemlevel as TaxItemlevel
                                                            ,b.TaxCalculateMethods as TaxCalculateMethods 
                                                                 from Master_Taxes b Where Taxes_ID=" + WhichType + "");

                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string Taxes_Code = Convert.ToString(dtEdit.Rows[0]["Taxes_Code"]);
                    string Taxes_Name = Convert.ToString(dtEdit.Rows[0]["Taxes_Name"]);
                    string Taxes_Description = Convert.ToString(dtEdit.Rows[0]["Taxes_Description"]);

                    string Taxes_ApplicableFor = Convert.ToString(dtEdit.Rows[0]["Taxes_ApplicableFor"]);
                    string Taxes_ApplicableOn = Convert.ToString(dtEdit.Rows[0]["Taxes_ApplicableOn"]);
                    string Taxes_OtherTax = Convert.ToString(dtEdit.Rows[0]["Taxes_OtherTax"]);
                    string OtherTax = Convert.ToString(dtEdit.Rows[0]["OtherTax"]);

                    string Taxtype = Convert.ToString(dtEdit.Rows[0]["TaxTypeCode"]);
                    string TaxItemlevel = Convert.ToString(dtEdit.Rows[0]["TaxItemlevel"]);
                    string Taxcalculate = Convert.ToString(dtEdit.Rows[0]["TaxCalculateMethods"]);

                    TaxLeviesGrid.JSProperties["cpEdit"] = Taxes_Code + "~" + Taxes_Name + "~" + Taxes_Description + "~" + Taxes_ApplicableFor + "~" + Taxes_ApplicableOn + "~" + Taxes_OtherTax + "~" + WhichType + "~" + OtherTax + "~" + Taxcalculate + "~" + TaxItemlevel + "~" + Taxtype;


                }
            }
        }
        [WebMethod]
        public static bool CheckUniqueCode(string TaxLaviesCode)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                DataTable dtCmb = new DataTable();
                dtCmb = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Taxes] WHERE [Taxes_Code] = " + "'" + TaxLaviesCode + "'");
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

        /*Code  Added  By Sudip on 14122016 to use jquery Choosen*/
        [WebMethod]
        public static List<string> GetOtherTaxList(string reqStr)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            //DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode+'-'+MainAccount_SubLedgerType as MainAccount_AccountCode ", " MainAccount_Name like '" + reqStr + "%'");
            DT = oDBEngine.GetDataTable("Config_TaxRates", "TaxRatesSchemeName+'  ['+(select Taxes_Name  from Master_Taxes where Taxes_ID=TaxRates_TaxCode)+']' Taxes_Name, TaxRates_ID  Taxes_ID", null);
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Taxes_Name"]) + "|" + Convert.ToString(dr["Taxes_ID"]));
            }
            return obj;
        }
        /*-------------End--------------------*/

        //----------------Added By sanjib for TaxType Validation------------------------
        [WebMethod]
        public static string GetExistingValu(string reqStr)
        {
            string datas = string.Empty;
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable DT = new DataTable();
            DT.Rows.Clear();

            DT = oDBEngine.GetDataTable("Master_Taxes", "isnull(TaxItemlevel,'0') as TaxItemlevel,isnull(TaxCalculateMethods,'0') as TaxCalculateMethods ", " TaxTypeCode='" + reqStr + "' order by Taxes_CreateTime desc");
            if (DT.Rows != null && DT.Rows.Count > 0)
            {
                datas = Convert.ToString(DT.Rows[0]["TaxItemlevel"]);
                datas += "|" + Convert.ToString(DT.Rows[0]["TaxCalculateMethods"]);

            }
            return datas.Trim();
        }

        //--------------------End----------------
    }
}