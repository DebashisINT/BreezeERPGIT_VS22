using System;
using System.Web;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_category : ERP.OMS.ViewState_class.VSPage
    {

        // DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        clsDropDownList OclsDropDownList = new clsDropDownList();

        string[] lengthIndex;
        string RemarksId;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDataSourceapplicable.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/category.aspx");
           
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                   
                }
                else
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                   
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!IsPostBack)
            {
                SetDateFormat();
                bindFieldList();
                Session["exportval"] = null;
                Session["KeyVal"] = "N";
                if (Session["userlastsegment"].ToString().Trim() == "4")
                {
                    string[,] list = new string[1, 2];
                    list[0, 0] = "Em";
                    list[0, 1] = "Employee";
                    OclsDropDownList.AddDataToDropDownListToAspx(list, CboApplicableFor, false);
                }
                else
                {

                    #region "Previous"
                    //string[,] list = new string[9, 2];
                    //list[0, 0] = "Cus";
                    //list[0, 1] = "Customer";
                    //list[1, 0] = "Ld";
                    //list[1, 1] = "Lead";
                    //list[2, 0] = "Sb";
                    //list[2, 1] = "Sub Brokers";
                    //list[3, 0] = "fr";
                    //list[3, 1] = "Franchisses";
                    //list[4, 0] = "DV";
                    //list[4, 1] = "Data Vendors";
                    //list[5, 0] = "Cus";
                    //list[5, 1] = "Customer";
                    //list[6, 0] = "RP";
                    //list[6, 1] = "Relationship Partner";
                    //list[7, 0] = "BP";
                    //list[7, 1] = "Business Partner";
                    //list[8, 0] = "RA";
                    //list[8, 1] = "Recruitment Agents";
                    #endregion

                  
                    //list[0, 0] = "Cus";
                    //list[0, 1] = "Customer";
                    //list[1, 0] = "Ld";
                    //list[1, 1] = "Lead";

                    //list[2, 0] = "fr";
                    //list[2, 1] = "Franchisees";
                    //list[3, 0] = "DV";
                    //list[3, 1] = "Data Vendors";

                    //list[4, 0] = "RP";
                    //list[4, 1] = "Relationship Partner";
                    //list[5, 0] = "BP";
                    //list[5, 1] = "Business Partner";
                    //list[6, 0] = "RA";
                    //list[6, 1] = "Recruitment Agents";
                    //list[7, 0] = "Em";
                    //list[7, 1] = "Employee";
                    //list[8, 0] = "Cmp";
                    //list[8, 1] = "Companies";
                    //list[9, 0] = "Br";
                    //list[9, 1] = "Branch";
                    //list[10, 0] = "JV";
                    //list[10, 1] = "Journal Voucher";



                    //DataTable dtudf=  oDBEngine.GetDataTable("Select APP_CODE as APP_CODE,APP_NAME AS APP_NAME from tbl_master_UDFApplicable");
                    string[,] list = oDBEngine.GetFieldValue("tbl_master_UDFApplicable", "APP_CODE, APP_NAME", "IS_ACTIVE=1", 2, "ORDER_BY");
                  
                    OclsDropDownList.AddDataToDropDownListToAspx(list, CboApplicableFor, false);

                    #region "cat_applicablefor objedct data bind"
                    //  Dictionary<string, string> list1 = new Dictionary<string, string>();

                    //list1.Add("Cus", "Customer");
                    //list1.Add("Ld", "Lead");
                    //list1.Add("Sb", "Sub Brokers");
                    //list1.Add("fr", "Franchisses");

                    //list1.Add("DV", "Data Vendors");
                    //list1.Add("RP", "Relationship Partner");
                    //list1.Add("BP", "Business Partner");
                    //list1.Add("RA", "Recruitment Agents");

                    //GridViewDataComboBoxColumn col = gridCategory.Columns["cat_applicablefor"] as GridViewDataComboBoxColumn;
                    //col.PropertiesComboBox.DataSource = list1;
                    //col.PropertiesComboBox.ValueField = "Key";
                    //col.PropertiesComboBox.TextField = "Value";


                    //GridViewDataComboBoxColumn combo = gridCategory.Columns["cat_applicablefor"] as GridViewDataComboBoxColumn;
                    //combo.PropertiesComboBox.ValueType = typeof(string);
                    //combo.PropertiesComboBox.DataSource = SqlDataSource1;
                    #endregion
                    // OclsDropDownList.AddDataToDropDownListToAspx(list, col, false);

                }

                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>Action='';MakeRowInVisible();</script>");

            }
            //DataBinderSegmentSpecific();
            gridCategory.JSProperties["cpDelmsg"] = null;
            gridCategory.JSProperties["cpSaveMsg"] = null;
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
                {
                    return;
                }
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
            if (!gridCategory.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = gridCategory.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Edit" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void gridCategory_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string[] CallVal = e.Parameters.ToString().Split('~');
                gridCategory.JSProperties["cpSave"] = "";
                gridCategory.JSProperties["cpUpdate"] = "";
                gridCategory.JSProperties["cpExtraValue"] = "";
                gridCategory.JSProperties["cpDelmsg"] = "";
                string maxValue = "0";
                DateTime maxDate = new DateTime(1900, 01, 01);
                string comboOption = "";

                lengthIndex = e.Parameters.Split('~');
                if (lengthIndex[0].ToString() == "SAVE_NEW")
                {
                    string maxLength = "500";
                    string isManndatory = "0";

                    if (Convert.ToInt32(ComboFieldType.Value) == 1)
                    {
                        maxLength = txtMaxLength.Text;
                    }
                    if (Convert.ToInt32(ComboFieldType.Value) == 3)
                    {
                        if (dtMaxDate.Value != null)
                        {
                            maxDate = Convert.ToDateTime(dtMaxDate.Value);
                        }
                        else
                        {
                            maxDate = new DateTime(1900, 1, 1);
                        }
                    }
                    if (Convert.ToInt32(ComboFieldType.Value) == 4)
                    {
                        maxValue = txtMaxValue.Text;
                    }

                    if (chkIsMandatory.Checked)
                    {
                        isManndatory = "1";
                    }

                    if (IsValidUdfName(txtcat_desc.Text.Trim(), Convert.ToString(CboApplicableFor.Value), Convert.ToString(ComboUdfGroup.Value),null))
                    {
                        oDBEngine.InsurtFieldValue("tbl_master_remarksCategory", "cat_description,cat_applicablefor,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options,cat_group_id", "'" + txtcat_desc.Text + "','" + CboApplicableFor.Value + "','" + ComboFieldType.Value + "','" + maxLength + "','" + isManndatory + "','" + maxValue + "','" + maxDate + "','" + txtComboOption.Text + "','" + ComboUdfGroup.Value + "'");
                        txtcat_desc.Text = "";
                        Session["KeyVal"] = "Y";
                        gridCategory.JSProperties["cpSave"] = "Y";
                        gridCategory.JSProperties["cpSaveMsg"] = "Saved Successfully.";
                    }
                    else
                    {
                        gridCategory.JSProperties["cpDelmsg"] = "This UDF is already define.";
                    }
                }
                else if (lengthIndex[0].ToString() == "EDIT")
                {
                    DataTable dt = oDBEngine.GetDataTable("select 1 from tbl_master_contactRemarks where cat_id='" +Convert.ToString( lengthIndex[1] ) + "'");
                    if (dt.Rows.Count > 0)
                    {
                        gridCategory.JSProperties["cpDelmsg"] = "Value entered against this UDF. Cannot modify.";
                        
                     }
                    else
                    {
                        

                        Session["KeyVal"] = "Y";
                        string maxlnt = "500";
                        string isManndatory = "0";
                        if (Convert.ToInt32(ComboFieldType.Value) == 1)
                        {
                            maxlnt = txtMaxLength.Text.Trim();
                        }
                        if (Convert.ToInt32(ComboFieldType.Value) == 3)
                        {
                            maxDate = Convert.ToDateTime(dtMaxDate.Value);
                        }

                        if (Convert.ToInt32(ComboFieldType.Value) == 4)
                        {
                            maxValue = txtMaxValue.Text;
                        }
                        if (chkIsMandatory.Checked)
                        {
                            isManndatory = "1";
                        }
                        if (Convert.ToInt32(ComboFieldType.Value) == 6 || Convert.ToInt32(ComboFieldType.Value) == 8 || Convert.ToInt32(ComboFieldType.Value) == 7)
                        {
                            comboOption = txtComboOption.Text;
                        }

                        if (IsValidUdfName(txtcat_desc.Text.Trim(), Convert.ToString(CboApplicableFor.Value), Convert.ToString(ComboUdfGroup.Value), Convert.ToString(lengthIndex[1])))
                        {
                            oDBEngine.SetFieldValue("tbl_master_remarksCategory", "cat_description='" + txtcat_desc.Text + "',cat_applicablefor='" + CboApplicableFor.Value + "',cat_field_type='" + ComboFieldType.Value + "',cat_max_length='" + maxlnt + "',isMandatory='" + isManndatory + "',cat_max_value='" + maxValue + "',cat_max_date='" + maxDate + "',cat_options='" + comboOption + "',cat_group_id='" + ComboUdfGroup.Value + "'", "id='" + Convert.ToString(lengthIndex[1]) + "'");
                            txtcat_desc.Text = "";
                            gridCategory.JSProperties["cpUpdate"] = "Y";
                        }
                        else
                        {
                            gridCategory.JSProperties["cpDelmsg"] = "This UDF is already define.";
                        }
                    }
                }
                else if (lengthIndex[0].ToString() == "BEFORE_EDIT")
                {
                    Session["KeyVal"] = "N";
                    gridCategory.JSProperties["cpSave"] = "N";
                    gridCategory.JSProperties["cpUpdate"] = "N";
                    string[,] Field_Value;
                    Field_Value = oDBEngine.GetFieldValue("tbl_master_remarksCategory", "cat_description,cat_applicablefor,cat_field_type,cat_max_length,isMandatory,cat_max_value,cat_max_date,cat_options,cat_group_id", "id='" + lengthIndex[1].ToString() + "'", 9);
                    txtcat_desc.Text = Field_Value[0, 0].ToString();
                    CboApplicableFor.Value = Field_Value[0, 1].ToString();

                    //date:12-12-2016 Name :debjyoti
                    //Reason: Bellow line commented because now data pass from server via json

                    // gridCategory.JSProperties["cpEdit"] = Field_Value[0, 0].ToString() + "~" + Field_Value[0, 1].ToString() + "~" + Field_Value[0, 2].ToString() + "~" + Field_Value[0, 3].ToString() + "~" + Field_Value[0, 4].ToString() + "~" + Field_Value[0, 5].ToString() + "~" + Field_Value[0, 6].ToString();


                    gridCategory.JSProperties["cpEditJson"] = @"{""cat_description"":" + @"""" + Field_Value[0, 0].ToString() +
                                                            @""",""cat_applicablefor"":""" + Field_Value[0, 1].ToString() +
                                                            @""",""cat_field_type"":""" + Field_Value[0, 2].ToString() +
                                                            @""",""cat_max_length"":""" + Field_Value[0, 3].ToString() +
                                                            @""",""isMandatory"":""" + Field_Value[0, 4].ToString() +
                                                            @""",""cat_max_value"":""" + Field_Value[0, 5].ToString() +
                                                            @""",""cat_max_date"":""" + Field_Value[0, 6].ToString() +
                                                            @""",""cat_options"":""" + Field_Value[0, 7].ToString() +
                                                             @""",""cat_group_id"":""" + Field_Value[0, 8].ToString() +
                                                            @"""}";

                }

                if (CallVal[0].ToString() == "Delete")
                {


                    string Remarkcategorycode = Convert.ToString(CallVal[1].ToString());
                    // DataTable result = oDBEngine.GetDataTable("select * from tbl_master_remarksCategory where id in (select cat_id from tbl_master_contactRemarks where cat_id=" + Remarkcategorycode + ")");
                    int retValue = masterChecking.DeleteMasterRemarkCategory(Convert.ToInt32(Remarkcategorycode));
                    if (retValue > 0)
                    {
                        Session["KeyVal"] = "Succesfully Deleted";
                        gridCategory.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                        //  DataBinderSegmentSpecific();
                        gridCategory.DataBind();
                    }
                    else
                    {
                        Session["KeyVal"] = "Value entered against this UDF. Cannot Delete.";
                        gridCategory.JSProperties["cpDelmsg"] = "Value entered against this UDF. Cannot Delete.";

                    }
                    //if (result.Rows.Count > 0)
                    //{
                    //    Session["KeyVal"] = "Used in other modules. Cannot Delete.";
                    //    gridCategory.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                    //}
                    //else
                    //{
                    //    int i = 0;
                    //    i = oDBEngine.DeleteValue("tbl_master_remarksCategory", "id ='" + Remarkcategorycode.ToString() + "'");
                    //    if (i > 0)
                    //    {
                    //        Session["KeyVal"] = "Succesfully Deleted";
                    //        gridCategory.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                    //        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Succesfully Deleted');</script>");
                    //        //gridCategory.Settings.ShowFilterRow = true;
                    //        DataBinderSegmentSpecific();
                    //    }
                    //    else
                    //    {
                    //        Session["KeyVal"] = "Used in other modules. Cannot Delete.";
                    //        Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Used in other modules. Cannot Delete.');</script>");
                    //        gridCategory.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
                    //    }
                    //}
                }
                else
                {
                    // DataBinderSegmentSpecific();
                    gridCategory.Settings.ShowFilterRow = true;
                    //gridCategory.DataBind();
                }
                gridCategory.DataBind();
            }
            catch (Exception ex)
            {
                Session["KeyVal"] = ex.ToString() + "Error";
            }


            //try
            //{
            //    string[] CallVal = e.Parameters.ToString().Split('~');
            //    if (e.Parameters == "s")
            //    {
            //        gridCategory.Settings.ShowFilterRow = true;
            //    }
            //    if (e.Parameters == "All")
            //    {
            //        gridCategory.FilterExpression = string.Empty;
            //        gridCategory.DataBind();
            //    }
            //    if (CallVal[0].ToString() == "Delete")
            //    {
            //        string Remarkcategorycode = Convert.ToString(CallVal[1].ToString());
            //        DataTable result = oDBEngine.GetDataTable("select * from tbl_master_remarksCategory where id in (select cat_id from tbl_master_contactRemarks where id=" + Remarkcategorycode + ")");

            //        if (result.Rows.Count > 0)
            //        {
            //            gridCategory.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
            //        }
            //        else
            //        {
            //            int i = oDBEngine.DeleteValue("tbl_trans_contactbankdetails", "id ='" + Remarkcategorycode.ToString() + "'");
            //            if (i > 0)
            //            {

            //                gridCategory.JSProperties["cpDelmsg"] = "Succesfully Deleted";

            //                //gridCategory.Settings.ShowFilterRow = true;
            //                //gridCategory.DataBind();
            //            }
            //            else
            //            {
            //                gridCategory.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        gridCategory.Settings.ShowFilterRow = true;
            //        gridCategory.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{

            //}


        }
        protected void gridCategory_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "All";
        }


        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {


        }
        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpPanel"] = Session["KeyVal"];
        }
        private void DataBinderSegmentSpecific()
        {
            if (Session["userlastsegment"].ToString() == "4")
            {
                SqlDataSource1.SelectCommand = "SELECT id,cat_description,case cat_applicablefor when 'Em' then 'Employee' when 'Ld' then 'Lead' when 'Sb' then 'Sub Broker' when 'fr' then 'Franchisees' when 'DV' then 'Data Vendors' when 'Cus' then 'Customer' when 'RP' then 'Relationship Partner' when 'BP' then 'Business Partner' when 'BP' then 'Business Partner' else 'Recruitment Agents' end as cat_applicablefor FROM [tbl_master_remarksCategory] where cat_applicablefor='Em'";
            }
            else
            {
                SqlDataSource1.SelectCommand = "SELECT id,cat_description,case cat_applicablefor when 'Em' then 'Employee' when 'Ld' then 'Lead' when 'Sb' then 'Sub Broker' when 'fr' then 'Franchisees' when 'DV' then 'Data Vendors' when 'Cus' then 'Customer' when 'RP' then 'Relationship Partner' when 'BP' then 'Business Partner' when 'BP' then 'Business Partner' else 'Recruitment Agents' end as cat_applicablefor FROM [tbl_master_remarksCategory] where cat_applicablefor!='Em'";
            }
            gridCategory.DataBind();
        }

        protected void gridCategory_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

        }

        public void bindFieldList()
        {

            DataTable dt = oDBEngine.GetDataTable("tbl_master_fieldType", "id,fieldDes", null);
            ComboFieldType.Items.Clear();
            ComboFieldType.DataSource = dt;
            ComboFieldType.ValueField = "id";
            ComboFieldType.TextField = "fieldDes";
            ComboFieldType.DataBind();
        }

        public void SetDateFormat()
        {
            dtMaxDate.TimeSectionProperties.Visible = false;
            dtMaxDate.UseMaskBehavior = true;
            dtMaxDate.EditFormatString = "dd-MM-yyyy";
            dtMaxDate.DisplayFormatString = "dd-MM-yyyy";

        }

        protected bool IsValidUdfName(string udfName, string applicableFor, string group_id,string id)
        {
            bool retValue = true;
            if (id != null)
            {
                DataTable Dt = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory where cat_applicablefor='" + applicableFor + "' and cat_group_id='" + group_id + "' and cat_description='" + udfName.Trim() + "' and id !='" + id + "'");
                if (Dt.Rows.Count > 0)
                {
                    retValue = false;
                }
            }
            else
            {
                DataTable Dt = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory where cat_applicablefor='" + applicableFor + "' and cat_group_id='" + group_id + "' and cat_description='" + udfName.Trim() + "'");
                if (Dt.Rows.Count > 0)
                {
                    retValue = false;
                }
            }
            return retValue;
        }

        [WebMethod]
        public static List<string> GetUdfGroup(string AppliFor)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            if (AppliFor != "")
            {
                DT = oDBEngine.GetDataTable("tbl_master_udfGroup", " ltrim(rtrim(grp_description)) Name,ltrim(rtrim(id)) Code", "grp_applicablefor='" + AppliFor + "'");
            }
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Code"]));
            }
            return obj;
        }

        #region Export event

        public void bindexport(int Filter)
        {
            gridCategory.Columns[4].Visible = false;
            //SchemaGrid.Columns[11].Visible = false;
            //SchemaGrid.Columns[12].Visible = false;
            string filename = "UDF";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "UDF";
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
        #endregion

    }
}