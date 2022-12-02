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
    public partial class SystemSettings : ERP.OMS.ViewState_class.VSPage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/category.aspx");

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!IsPostBack)
            {
               
            
                Session["exportval"] = null;
                Session["KeyVal"] = "N";
                

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

                    

                    if (chkIsMandatory.Checked)
                    {
                        isManndatory = "1";
                    }

                    
                }
                else if (lengthIndex[0].ToString() == "EDIT")
                {
                    DataTable dt = oDBEngine.GetDataTable("select * from Config_SystemSettings where Variable_Name='" + Convert.ToString(lengthIndex[1]) + "'");
                    if (dt.Rows.Count <= 0)
                    {
                        gridCategory.JSProperties["cpDelmsg"] = "Variable Name does not match with our database.";

                    }
                    else
                    {


                        Session["KeyVal"] = "Y";
                        string maxlnt = "500";
                        string isManndatory = "0";



                        if (Convert.ToString(lengthIndex[1]) !="")
                        {
                            string valeedit = "",ModuleName="";
                            ModuleName = txtModuleName.Text.Trim();
                            if (Convert.ToString(lengthIndex[2]) == "Text")
                            {
                                valeedit = txtVal_Value.Text;
                                

                            }
                            else if (Convert.ToString(lengthIndex[2]) == "Dropdown")
                            {
                                valeedit = ComboValue.SelectedItem.Text;

                            }
                            //oDBEngine.SetFieldValue("Config_SystemSettings", "Variable_Description='" + txtVal_Desc.Text + "',Variable_Value='" + valeedit + "',IsActive='" + chkIsMandatory.Checked + "',UpdatedOn='" + DateTime.Now + "'", "Variable_Name='" + Convert.ToString(lengthIndex[1]) + "'");
                            oDBEngine.SetFieldValue("Config_SystemSettings", "Variable_Value='" + valeedit + "',ModuleName='" + ModuleName + "',UpdatedOn='" + DateTime.Now + "'", "Variable_Name='" + Convert.ToString(lengthIndex[1]) + "'");

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


                     

                    ///Comment on 03/04/2017 by sudip pal///
                    ///

                    Field_Value = oDBEngine.GetFieldValue("Config_SystemSettings", "Variable_Name,Variable_Description,Variable_Value,IsActive,FieldType,ModuleName", "Variable_Name='" + lengthIndex[1].ToString() + "'", 6);
                    gridCategory.JSProperties["cpEditJson"] = @"{""Variable_Name"":" + @"""" + Field_Value[0, 0].ToString() +
                                                      @""",""Variable_Description"":""" + Field_Value[0, 1].ToString() +
                                                      @""",""Variable_Value"":""" + Field_Value[0, 2].ToString() +
                                                      @""",""IsActive"":""" + Field_Value[0, 3].ToString() +
                                                      @""",""FieldType"":""" + Field_Value[0, 4].ToString() +
                                                      @""",""ModuleName"":""" + Field_Value[0, 5].ToString() + @"""}";

                }

               
                gridCategory.DataBind();
            }
            catch (Exception ex)
            {
                Session["KeyVal"] = ex.ToString() + "Error";
            }
            

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
    

      

      

      

     

     
        #region Export event

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
            gridCategory.Columns[4].Visible = false;
            string filename = "System Settings";
            exporter.FileName = filename;
            exporter.FileName = "SystemSettings";

            exporter.PageHeader.Left = "System Settings";
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

        //public void bindexport(int Filter)
        //{
        //    gridCategory.Columns[4].Visible = false;
           
        //    string filename = "UDF";
        //    exporter.FileName = filename;

        //    exporter.PageHeader.Left = "UDF";
        //    exporter.PageFooter.Center = "[Page # of Pages #]";
        //    exporter.PageFooter.Right = "[Date Printed]";

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
     
        #endregion

    }
}