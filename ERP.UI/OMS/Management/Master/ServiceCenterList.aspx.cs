using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EntityLayer.CommonELS;
using System.IO;

namespace ERP.OMS.Management.Master
{
    public partial class ServiceCenterList : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Bank objBankStatementBL = new BusinessLogicLayer.Bank();
        //string MyGlobalVariable = "ConnectionStrings:crmConnectionString"; MULTI
        //bellow code added by debjyoti 17-11-2016
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //end 17-11-2016
        protected void Page_Init(object sender, EventArgs e)
        {

        }
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/ServiceCenterList.aspx");
            gridStatusDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
          

            fillGrid();
            gridStatus.JSProperties["cpDelmsg"] = null;
        }


        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string[] CallVal = e.Parameters.ToString().Split('~');
               

                if (CallVal[0].ToString() == "s")
                {
                    gridStatus.Settings.ShowFilterRow = true;
                }
                else if (CallVal[0].ToString() == "All")
                {
                    gridStatus.FilterExpression = string.Empty;
                }
                else if (CallVal[0].ToString() == "Delete")
                {


                 
                    oDBEngine.DeleteValue("tbl_master_contact ", "cnt_id ='" + CallVal[1].ToString() + "'");
                    gridStatus.JSProperties["cpDelmsg"] = "Succesfully Deleted";
                    fillGrid();

                 

                }
            }
            catch (Exception ex)
            {

            }

        }
        public void fillGrid()
        {
            gridStatusDataSource.SelectCommand = "select tbl_master_contact.cnt_id, tbl_master_contact.cnt_internalId, tbl_master_contact.cnt_UCC, tbl_master_contact.cnt_firstName,branch_description=(select branch_description from tbl_master_branch where branch_id=tbl_master_contact.cnt_branchid),tbl_master_contact.cnt_VerifcationRemarks from  tbl_master_contact where cnt_contactType='SC' order by tbl_master_contact.cnt_id desc";
            gridStatus.DataBind();

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
            gridStatus.Columns[3].Visible = false;
            string filename = "ServiceCenter";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Service Center";
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
        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray());
           
        }

    }
}