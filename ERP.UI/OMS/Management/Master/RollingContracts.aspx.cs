using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_RollingContracts : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sqlRollingContracts.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                // DropDownList drpExchnage = (DropDownList)CbpAddSpreadAccounts.FindControl("drpExchange");
                DataTable dt = oDBEngine.GetDataTable("Master_Exchange", "*", "Exchange_IsCommodity='Y'", "Exchange_Name");
                drpExchange.DataSource = dt;
                drpExchange.DataTextField = "Exchange_Name";
                drpExchange.DataValueField = "Exchange_ID";
                drpExchange.DataBind();

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "height();", true);
        }
        protected void gridRollingContracts_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {

        }
        protected void gridRollingContracts_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpheight"] = "error";
        }
        protected void gridRollingContracts_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                gridRollingContracts.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                gridRollingContracts.FilterExpression = string.Empty;
            }
            gridRollingContracts.DataBind();
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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


        protected void CbpAddSpreadAccounts_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            //Initialize cp Variable
            CbpAddSpreadAccounts.JSProperties["cpIsInsert"] = null;
            /////////////////////////////

            if (data[0] == "Add")
            {
                txtProducts.Text = string.Empty;


            }

            if (data[0] == "Save")
            {
                if (txtProducts.Text.Trim() != String.Empty && txtProducts_hidden.Value.Trim() != String.Empty)
                {
                    //string[] AssetRelatedValue = txtAsset_hidden.Value.Split('^');
                    string[] sqlParameterName = new string[5];
                    string[] sqlParameterType = new string[5];
                    string[] sqlParameterValue = new string[5];
                    string[] sqlParameterSize = new string[5];
                    sqlParameterName[0] = "AddEdit";
                    sqlParameterValue[0] = "add";
                    sqlParameterType[0] = "V";
                    sqlParameterSize[0] = "10";
                    sqlParameterName[1] = "ProductId";
                    sqlParameterValue[1] = txtProducts_hidden.Value;
                    sqlParameterType[1] = "V";
                    sqlParameterSize[1] = "200";
                    sqlParameterName[2] = "ExchangeId";
                    sqlParameterValue[2] = drpExchange.SelectedValue;
                    sqlParameterType[2] = "V";
                    sqlParameterSize[2] = "10";
                    sqlParameterName[3] = "ExchangeSegment";
                    sqlParameterValue[3] = Session["usersegid"].ToString();
                    sqlParameterType[3] = "V";
                    sqlParameterSize[3] = "10";
                    sqlParameterName[4] = "CreateUser";
                    sqlParameterValue[4] = Session["UserID"].ToString();
                    sqlParameterType[4] = "I";
                    sqlParameterSize[4] = "";


                    int GetValue = SQLProcedures.Execute_Return_StoreProcedure("Insert_RollingContracts", sqlParameterName, sqlParameterType, sqlParameterValue, sqlParameterSize);
                    if (GetValue == 2) CbpAddSpreadAccounts.JSProperties["cpIsInsert"] = "Successfully Saved";
                    else if (GetValue == 1)
                    {
                        //ddlAssetSubType.SelectedIndex = 0;
                        //ddlExercisestyle.SelectedIndex = 0;
                        //ddlOptionType.SelectedIndex = 0;
                        CbpAddSpreadAccounts.JSProperties["cpIsInsert"] = "Already Exists";
                    }
                    else CbpAddSpreadAccounts.JSProperties["cpIsInsert"] = "Unable To Save!!!";
                }
                else
                {
                    CbpAddSpreadAccounts.JSProperties["cpIsInsert"] = "Please Choose Product Properly";
                }
            }
            else if (data[0] == "Edit")
            {
                DataTable dt = oDBEngine.GetDataTable("Master_RollingContracts,Master_Products", "rtrim(ltrim(isnull(Products_Name,''))) + ' [ ' + rtrim(ltrim(isnull(Products_ShortName,''))) + ' ]' as Products,RollingContracts_ExchangeID", "RollingContracts_ProductID=Products_ID and RollingContracts_ID=" + data[1]);

                if (dt.Rows.Count > 0)
                {
                    txtProducts.Text = Convert.ToString(dt.Rows[0]["Products"]);
                    string exchid = Convert.ToString(dt.Rows[0]["RollingContracts_ExchangeID"]);
                    if (exchid != "")
                    {
                        for (int i = 0; i < drpExchange.Items.Count; i++)
                        {
                            if (drpExchange.Items[i].Value == exchid)
                                drpExchange.Items[i].Selected = true;

                        }

                    }
                    //drpExchange.SelectedValue = Convert.ToString(dt.Rows[0]["RollingContracts_ExchangeID"]);


                }

            }
            else if (data[0] == "SaveUpdate")
            {
                string[] sqlParameterName = new string[6];
                string[] sqlParameterType = new string[6];
                string[] sqlParameterValue = new string[6];
                string[] sqlParameterSize = new string[6];
                sqlParameterName[0] = "AddEdit";
                sqlParameterValue[0] = "edit";
                sqlParameterType[0] = "V";
                sqlParameterSize[0] = "10";
                sqlParameterName[1] = "ProductId";
                sqlParameterValue[1] = txtProducts_hidden.Value;
                sqlParameterType[1] = "V";
                sqlParameterSize[1] = "200";
                sqlParameterName[2] = "ExchangeId";
                sqlParameterValue[2] = drpExchange.SelectedValue;
                sqlParameterType[2] = "V";
                sqlParameterSize[2] = "10";
                sqlParameterName[3] = "ExchangeSegment";
                sqlParameterValue[3] = Session["usersegid"].ToString();
                sqlParameterType[3] = "V";
                sqlParameterSize[3] = "10";
                sqlParameterName[4] = "RollingcontractId";
                sqlParameterValue[4] = data[1];
                sqlParameterType[4] = "I";
                sqlParameterSize[4] = "";
                sqlParameterName[5] = "CreateUser";
                sqlParameterValue[5] = Session["UserID"].ToString();
                sqlParameterType[5] = "I";
                sqlParameterSize[5] = "";


                int GetValue = SQLProcedures.Execute_Return_StoreProcedure("Insert_RollingContracts", sqlParameterName, sqlParameterType, sqlParameterValue, sqlParameterSize);
                if (GetValue == 2) CbpAddSpreadAccounts.JSProperties["cpIsInsert"] = "Successfully Updated";
                else if (GetValue == 1)
                {
                    CbpAddSpreadAccounts.JSProperties["cpIsInsert"] = "Already Exists";
                }
                else CbpAddSpreadAccounts.JSProperties["cpIsInsert"] = "Unable To Save!!!";

            }

        }
    }
}