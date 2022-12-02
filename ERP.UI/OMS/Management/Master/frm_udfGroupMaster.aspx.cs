using System;
using System.Web;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Collections.Generic;
using System.Data;

namespace ERP.OMS.Management.Master
{
    public partial class management_frm_udfGroupMaster : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    { 
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.UdfGroupMasterBL udfBl = new BusinessLogicLayer.UdfGroupMasterBL();

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
                //debjyoti
                Session["exportval"] = null;
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSourceapplicable.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frm_pinMaster.aspx");
            //SqlDataSource1.ConnectionString = ConfigurationManager.AppSettings["DBConnectionDefault"];MULTI
          
           if(!IsPostBack)
           {
               //string[,] list = new string[11, 2];
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
               string[,] list = oDBEngine.GetFieldValue("tbl_master_UDFApplicable", "APP_CODE, APP_NAME", "IS_ACTIVE=1", 2, "ORDER_BY");
               OclsDropDownList.AddDataToDropDownListToAspx(list, CboApplicableFor, false);
               
           }
           
          
        }

        protected void gridudfGroup_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < gridudfGroup.Columns.Count; i++)
                    if (gridudfGroup.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                {
                    return;
                }
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex  ;
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

        protected void gridudfGroup_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                gridudfGroup.JSProperties["cpHide"] = null;
                gridudfGroup.JSProperties["cpMsg"] = null;
                gridudfGroup.JSProperties["cpEditJson"] = null;

                string[] lengthIndex;
                   lengthIndex = e.Parameters.Split('~');
                  
                if (lengthIndex[0].ToString() == "SAVE_NEW")
                   {
                       Boolean retData=udfBl.InsertGroupMaster(txtGrp_desc.Text.Trim(), Convert.ToString(CboApplicableFor.Value), chkIsVisible.Checked);
                       if (retData)
                       {
                           gridudfGroup.JSProperties["cpHide"] = "Y";
                           gridudfGroup.JSProperties["cpMsg"] = "Saved Successfully";
                       }
                   }

                else if (lengthIndex[0].ToString() == "BEFORE_EDIT")
                { 
                    string[,] Field_Value;
                    Field_Value = oDBEngine.GetFieldValue("tbl_master_udfGroup", "grp_description,grp_applicablefor,grp_isVisible ", "id='" + lengthIndex[1].ToString() + "'", 3);

                    gridudfGroup.JSProperties["cpEditJson"] = @"{""grp_description"":" + @"""" + Field_Value[0, 0].ToString() +
                                                           @""",""grp_applicablefor"":""" + Field_Value[0, 1].ToString() +
                                                           @""",""grp_isVisible"":""" + Field_Value[0, 2].ToString() + 
                                                           @"""}";
                }
                else if (lengthIndex[0].ToString() == "EDIT")
                {
                    Boolean retData = udfBl.UpdateGroupMaster(txtGrp_desc.Text.Trim(), Convert.ToString(CboApplicableFor.Value), chkIsVisible.Checked, Convert.ToInt32(lengthIndex[1]));
                    if (retData)
                    {
                        gridudfGroup.JSProperties["cpHide"] = "Y";
                        gridudfGroup.JSProperties["cpMsg"] = "Saved Successfully";
                    }
                }
                if (lengthIndex[0].ToString() == "Delete")
                {
                    int UdfGroupId = Convert.ToInt32(lengthIndex[1].ToString());
                    int retValue = masterChecking.DeleteUdfGroup(UdfGroupId);
                    if (retValue > 0)
                    {
                        gridudfGroup.JSProperties["cpMsg"] = "Deleted Successfully.";
                    }
                    else
                    {
                        gridudfGroup.JSProperties["cpMsg"] = "Used in other modules. Cannot Delete.";
                    }
                
                }
                gridudfGroup.DataBind();
                gridudfGroup.Settings.ShowFilterRow = true;

            }
            catch (Exception ex)
            {
            
            }
        }
        protected void gridudfGroup_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "All";
        } 
       
        private void DataBinderSegmentSpecific()
        {

            SqlDataSource1.SelectCommand = "select  pin_id,pin_code,d.city_name as city_id,s.state  from tbl_master_pinzip h inner join tbl_master_city d on h.city_id=d.city_id inner join tbl_master_state s on s.id=d.state_id order by pin_id";

            gridudfGroup.DataBind();

            
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
            gridudfGroup.Columns[4].Visible = false;
           

            exporter.FileName = "UDF Group Master";
            exporter.ReportHeader = "UDF Group Master";
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

    }
}