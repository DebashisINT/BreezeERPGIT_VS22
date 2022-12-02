using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
//using DevExpress.Web.ASPxCallbackPanel;
//////using DevExpress.Web.ASPxClasses;
////using DevExpress.Web;
//using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using System.Text;
using System.Collections.Generic;
using System.Resources;
using System.Collections;
using DevExpress.Web;

namespace ERP.OMS.Management.Store.Master
{
    public partial class management_master_Store_sProductsOld : System.Web.UI.Page
    {
        public string pageAccess = "";
        //  DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }



            //new code block for showing key from resource page start

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/productVal.resx")))
            {
                ResourceReader resReader = new ResourceReader(Server.MapPath("~/Management/DailyTask/ResourceFiles/productVal.resx"));

                var i = 0;
                foreach (DictionaryEntry d in resReader)
                {


                    i = i + 1;
                    //1.Trading Lot Units
                    if (i == 1)
                    {
                        marketsGrid.Columns["sProducts_TradingLotUnit"].Caption = d.Value.ToString();
                    }
                    //2.QuoteCurrency
                    else if (i == 2)
                    {
                        marketsGrid.Columns["sProducts_TradingLotUnit"].Caption = d.Value.ToString();
                    }
                    else if (i == 3)
                    {
                        marketsGrid.Columns["sProducts_Code"].Caption = d.Value.ToString();
                    }
                    else if (i == 4)
                    {
                        marketsGrid.Columns["sProducts_TypeFull"].Caption = d.Value.ToString();
                    }
                    else if (i == 5)
                    {
                        marketsGrid.Columns["ProductClass_Code"].Caption = d.Value.ToString();
                    }
                    else if (i == 6)
                    {
                        marketsGrid.Columns["sProducts_Name"].Caption = d.Value.ToString();
                    }
                    else if (i == 7)
                    {
                        marketsGrid.Columns["sProducts_TradingLot"].Caption = d.Value.ToString();
                    }
                    //else if (i == 8)
                    //{
                    //    marketsGrid.Columns["Description"].Caption = d.Value.ToString();
                    //}
                    //else if (i == 9)
                    //{
                    //    marketsGrid.Columns["Description"].Caption = d.Value.ToString();
                    //}
                    //else if (i == 10)
                    //{
                    //    marketsGrid.Columns["Description"].Caption = d.Value.ToString();
                    //}
                    //else if (i == 11)
                    //{
                    //    marketsGrid.Columns["Description"].Caption = d.Value.ToString();
                    //}
                    //else if (i == 12)
                    //{
                    //    marketsGrid.Columns["Description"].Caption = d.Value.ToString();
                    //}
                    else if (i == 13)
                    {
                        marketsGrid.Columns["sProducts_TradingLotUnit"].Caption = d.Value.ToString();
                    }
                    else if (i == 14)
                    {
                        marketsGrid.Columns["sProducts_QuoteLot"].Caption = d.Value.ToString();
                    }
                    else if (i == 15)
                    {
                        marketsGrid.Columns["sProducts_Color"].Caption = d.Value.ToString();
                    }
                    else if (i == 16)
                    {
                        marketsGrid.Columns["sProducts_DeliveryLotUnit"].Caption = d.Value.ToString();
                    }
                    else if (i == 17)
                    {
                        marketsGrid.Columns["sProducts_DeliveryLot"].Caption = d.Value.ToString();
                    }


                    //Label currLBL = new Label();
                    //currLBL = (Label)FindControl(d.Key.ToString());

                    //if (currLBL == null) { currLBL = (Label)Parent.FindControl(d.Key.ToString()); }

                    //currLBL.Text = d.Value.ToString();
                }

                resReader.Close();
            }

            //new code block for showing key from resource page end
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlSourceProductClass_Code.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSourceProductType.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSourceSizeId.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            markets.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectUOM.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCurrency.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Session["requesttype"] = "Product";
            Session["ContactType"] = "Product";
            Session["KeyVal_InternalID"] = "";
            Session["Name"] = "";
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
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


        protected void BTNSaveUC_clicked(object sender, EventArgs e)
        {
            string[] key = Convert.ToString(KeyField.Text).Split(',');
            string[] value = Convert.ToString(ValueField.Text).Split(',');
            string RexName = Convert.ToString(RexPageName.Text).Trim();

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx")))
            {
                File.Delete(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            }

            ResourceWriter resourceWriter = new ResourceWriter(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] != null && key[i] != "")
                {
                    resourceWriter.AddResource(key[i].Trim(), value[i].Trim());
                }
            }
            resourceWriter.Generate();
            resourceWriter.Close();

            Response.Redirect("");
        }




        protected void marketsGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < marketsGrid.Columns.Count; i++)
                    if (marketsGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 16;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Session["PageAccess"].ToString() == "DelAdd" || Session["PageAccess"].ToString() == "Delete" || Session["PageAccess"].ToString() == "All")
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
        protected void marketsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!marketsGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = marketsGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void marketsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                marketsGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                marketsGrid.FilterExpression = string.Empty;
            }
        }
        protected void marketsGrid_OnInitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            e.NewValues["sProducts_Color"] = null;
            e.NewValues["sProducts_Size"] = null;
        }
    }
}