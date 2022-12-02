using System;
using System.Web;
using DevExpress.Web;
using System.Configuration;
using EntityLayer.CommonELS;
using System.Data;
using System.Web.Services;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_remindercategory : ERP.OMS.ViewState_class.VSPage
    {

        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        string[] lengthIndex;
        string RemarksId;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'

                //Purpose : Replace .ToString() with Convert.ToString(..)
                //Name : Sudip 
                // Dated : 21-12-2016

                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 21-12-2016

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/remindercategory.aspx");
           
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!IsPostBack)
            {
                Session["KeyVal"] = "N";
                //if (Session["userlastsegment"].ToString().Trim() == "4")
                //{
                //    string[,] list = new string[1, 2];
                //    list[0, 0] = "Em";
                //    list[0, 1] = "Employee";
                //    //oDBEngine.AddDataToDropDownListToAspx(list, CboApplicableFor, false);
                //}
                //else
                //{

                //    string[,] list = new string[9, 2];
                //    list[0, 0] = "Cus";
                //    list[0, 1] = "Customer";
                //    list[1, 0] = "Ld";
                //    list[1, 1] = "Lead";
                //    list[2, 0] = "Sb";
                //    list[2, 1] = "Sub Brokers";
                //    list[3, 0] = "fr";
                //    list[3, 1] = "Franchisses";
                //    list[4, 0] = "DV";
                //    list[4, 1] = "Data Vendors";
                //    list[5, 0] = "Cus";
                //    list[5, 1] = "Customer";
                //    list[6, 0] = "RP";
                //    list[6, 1] = "Relationship Partner";
                //    list[7, 0] = "BP";
                //    list[7, 1] = "Business Partner";
                //    list[8, 0] = "RA";
                //    list[8, 1] = "Recruitment Agents";
                //    //oDBEngine.AddDataToDropDownListToAspx(list, CboApplicableFor, false);

                //}

               // this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>Action='';height();MakeRowInVisible();</script>");

            }
            DataBinderSegmentSpecific();
        }
        protected void gridCategory_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < gridCategory.Columns.Count; i++)
                    if (gridCategory.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 1;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Session["PageAccess"] == "DelAdd" || Session["PageAccess"] == "Delete" || Session["PageAccess"] == "All")
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
        protected void gridCategory_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 21-12-2016

            if (!gridCategory.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = gridCategory.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Edit" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void gridCategory_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
            {
                gridCategory.Settings.ShowFilterRow = true;
            }
            if (e.Parameters == "All")
            {
                gridCategory.FilterExpression = string.Empty;
                gridCategory.DataBind();
            }


        }
        protected void gridCategory_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "All";
        }


        protected void ASPxCallbackPanel1_Callback(object source, CallbackEventArgsBase e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 21-12-2016

            string strDesc = Convert.ToString(txtcat_desc.Text).Replace(@"'", @" ");
         
            lengthIndex = e.Parameter.Split('~');
            if (Convert.ToString(lengthIndex[0]) == "SAVE_NEW")
            {
                oDBEngine.InsurtFieldValue("master_remindercategory", "Remindercategory_description,Remindercategory_shortname", "'" + strDesc.Trim() + "','" + Convert.ToString(txt_description.Text).Trim() + "'");
             
                txtcat_desc.Text = "";
                Session["KeyVal"] = "Y";
                txt_description.Text = "";
                gridCategory.FilterExpression = string.Empty;
                gridCategory.DataBind();
               
            }
            else if (Convert.ToString(lengthIndex[0]) == "EDIT")
            {
                Session["KeyVal"] = "Y";
                oDBEngine.SetFieldValue("master_remindercategory", "Remindercategory_description='" + strDesc.Trim() + "',Remindercategory_shortname='" + txt_description.Text.Trim() + "'", "Remindercategory_id='" + Convert.ToString(lengthIndex[1]) + "'");
                txtcat_desc.Text = "";
                txt_description.Text = "";


            }
            else if (Convert.ToString(lengthIndex[0]) == "BEFORE_EDIT")
            {
                Session["KeyVal"] = "N";
                string[,] Field_Value;
                Field_Value = oDBEngine.GetFieldValue("master_remindercategory", "Remindercategory_description,Remindercategory_shortname", "Remindercategory_id='" + Convert.ToString(lengthIndex[1]) + "'", 2);
                txtcat_desc.Text = Convert.ToString(Field_Value[0, 0]);
                txt_description.Text = Convert.ToString(Field_Value[0, 1]);
                //CboApplicableFor.Value = Field_Value[0, 1].ToString();
            }

        }
        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpPanel"] = Session["KeyVal"];
        }
        private void DataBinderSegmentSpecific()
        {
            //if (Session["userlastsegment"].ToString() == "4")
            //{
            //    SqlDataSource1.SelectCommand = "SELECT id,cat_description,case cat_applicablefor when 'Em' then 'Employee' when 'Ld' then 'Lead' when 'Sb' then 'Sub Broker' when 'fr' then 'Franchisses' when 'DV' then 'Data Vendors' when 'Cus' then 'Customer' when 'RP' then 'Relationship Partner' when 'BP' then 'Business Partner' when 'BP' then 'Business Partner' else 'Recruitment Agents' end as cat_applicablefor FROM [tbl_master_remarksCategory] where cat_applicablefor='Em'";
            //}
            //else
            //{
            //SqlDataSource1.SelectCommand = "SELECT id,cat_description,case cat_applicablefor when 'Em' then 'Employee' when 'Ld' then 'Lead' when 'Sb' then 'Sub Broker' when 'fr' then 'Franchisses' when 'DV' then 'Data Vendors' when 'Cus' then 'Customer' when 'RP' then 'Relationship Partner' when 'BP' then 'Business Partner' when 'BP' then 'Business Partner' else 'Recruitment Agents' end as cat_applicablefor FROM [master_remarksCategory] where cat_applicablefor!='Em'";
            SqlDataSource1.SelectCommand = "SELECT Remindercategory_id as id,Remindercategory_description as cat_description,Remindercategory_shortname as cat_applicablefor FROM [master_remindercategory]";
            //}
            gridCategory.DataBind();
        }

        [WebMethod]
        public static bool CheckUniqueCode(string CategoriesShortCode, string Code)
        {
            bool flag = false;
            try
            {
                BusinessLogicLayer.MShortNameCheckingBL obj = new BusinessLogicLayer.MShortNameCheckingBL();
                flag= obj.CheckUnique(CategoriesShortCode, Code, "ReminderCategory");
               
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

}