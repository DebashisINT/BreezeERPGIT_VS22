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
    public partial class management_master_pin : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    { 
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frm_pinMaster.aspx");
          //  SqlDataSource1.ConnectionString = ConfigurationManager.AppSettings["DBConnectionDefault"]; MULTI
            SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           if(!IsPostBack)
           {
               SetCountry();
               
           }
           
          
        }

        protected void gridPin_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < gridPin.Columns.Count; i++)
                    if (gridPin.Columns[i] is GridViewCommandColumn)
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

        protected void gridPin_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                gridPin.JSProperties["cpSave"] = "";
                gridPin.JSProperties["cpEdit"] = "";
                gridPin.JSProperties["cpDelmsg"] = "";
                gridPin.JSProperties["cpErrorMsg"] = "";
                string[] CallVal = e.Parameters.ToString().Split('~');
                lengthIndex = e.Parameters.Split('~');
                if (lengthIndex[0].ToString() == "SAVE_NEW")
                {
                    string[,] Field_Value;
                    Field_Value = oDBEngine.GetFieldValue("tbl_master_pinzip", "pin_code", "pin_code='" + txtpincode.Text.Trim() + "'", 1);
                    if (Field_Value[0, 0] !=  "n")
                    {
                        gridPin.JSProperties["cpSave"] = "N";
                        gridPin.JSProperties["cpErrorMsg"] = "Pin code already exists!!";
                    }
                    else
                    {
                        oDBEngine.InsurtFieldValue("tbl_master_pinzip", "pin_code,city_id", "'" + txtpincode.Text.Trim() + "','" + HdlstCity.Value + "'");
                        txtpincode.Text = "";
                        gridPin.JSProperties["cpSave"] = "Y";
                        DataBinderSegmentSpecific();
                    }
                }
                if (lengthIndex[0].ToString() == "BEFORE_EDIT")
                {
                    string pinid = lengthIndex[1].ToString();
                    string[,] Field_Value;
                    Field_Value = oDBEngine.GetFieldValue("tbl_master_pinzip p inner join tbl_master_city c on p.city_id=c.city_id inner join tbl_master_state s on c.state_id= s.id", "p.pin_code,p.city_id,c.state_id,s.countryId ", "pin_id='" + pinid + "'", 4);
                    txtpincode.Text = Field_Value[0, 0];
                    gridPin.JSProperties["cpSave"] = "N";
                    gridPin.JSProperties["cpEdit"] = Field_Value[0, 0].ToString() + "~" + Field_Value[0, 1].ToString() + "~" + Field_Value[0, 2].ToString() + "~" + Field_Value[0, 3].ToString();
                }
                if (lengthIndex[0].ToString() == "EDIT")
                {
                     string[,] Field_Value;
                     Field_Value = oDBEngine.GetFieldValue("tbl_master_pinzip", "pin_code", "pin_code='" + txtpincode.Text.Trim() + "' and pin_id <> " + Convert.ToString(lengthIndex[1]) , 1);
                    if ((Field_Value[0, 0] != "n"))
                    {
                        gridPin.JSProperties["cpSave"] = "N";
                        gridPin.JSProperties["cpErrorMsg"] = "Pin code already exists!!";
                    }
                    else
                    {
                        string pinid = lengthIndex[1].ToString();
                        oDBEngine.SetFieldValue("tbl_master_pinzip", "pin_code='" + txtpincode.Text.Trim() + "',city_id='" + HdlstCity.Value + "'", "pin_id='" + lengthIndex[1].ToString() + "'");
                        gridPin.JSProperties["cpSave"] = "Y";
                        DataBinderSegmentSpecific();
                    }
                }
                if (CallVal[0].ToString() == "Delete")
                {
                    string PinId = Convert.ToString(CallVal[1].ToString());
                    int retValue = masterChecking.DeleteMasterPinCode(Convert.ToInt32(PinId));
                    if (retValue > 0)
                    {
                        Session["KeyVal"] = "Succesfully Deleted";
                        gridPin.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                        DataBinderSegmentSpecific();
                    }
                    else
                    {
                        Session["KeyVal"] = "Used in other modules. Cannot Delete.";
                        gridPin.JSProperties["cpDelmsg"] = "Used in other modules. Cannot Delete.";

                    }
                }
                else
                {
                    DataBinderSegmentSpecific();
                    gridPin.Settings.ShowFilterRow = true;
                }
            }
            catch (Exception ex)
            {
            
            }
        }
        protected void gridPin_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "All";
        } 
       
        private void DataBinderSegmentSpecific()
        {

            SqlDataSource1.SelectCommand = "select  pin_id,pin_code,d.city_name as city_id,s.state  from tbl_master_pinzip h inner join tbl_master_city d on h.city_id=d.city_id inner join tbl_master_state s on s.id=d.state_id order by pin_id";
           
            gridPin.DataBind();

            
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

        public void SetCountry()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_country", "  ltrim(rtrim(cou_country)) Name,ltrim(rtrim(cou_id)) Code ", null);
            lstCountry.DataSource = DT;
            lstCountry.DataMember = "Code";
            lstCountry.DataTextField = "Name";
            lstCountry.DataValueField = "Code";
            lstCountry.DataBind();
             
        }

    }
}