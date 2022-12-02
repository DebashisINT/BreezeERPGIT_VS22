using DataAccessLayer;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Followup
{
    public partial class FollowupEntry : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Followup/Followup.aspx");
            if (!IsPostBack)
            {
                hdAction.Value = "Add";
                LoadData();
            }
        }

        private void LoadData()
        {
            dtFollowDate.Date = DateTime.Now;
            dtFollowDate.MinDate = Convert.ToDateTime(Session["FinYearStart"]);
            dtFollowDate.MaxDate = Convert.ToDateTime(Session["FinYearEnd"]);

            ProcedureExecute proc = new ProcedureExecute("prc_Followup");
            proc.AddVarcharPara("@action", 100, "LoadFollowUpEntry");
            proc.AddVarcharPara("@Customer", 20, Request.QueryString["custId"]);
            proc.AddVarcharPara("@UserId", 20, Convert.ToString(Session["userid"]));
            proc.AddVarcharPara("@FromDate", 10, Request.QueryString["FromDt"]);
            proc.AddVarcharPara("@ToDate", 10, Request.QueryString["ToDt"]);
            DataSet Ds = proc.GetDataSet();
            lblFollowupBy.Text = Convert.ToString(Ds.Tables[0].Rows[0]["user_name"]) + " [Customer: " + Convert.ToString(Ds.Tables[4].Rows[0][0]) + "]";


            ASPxListBox docList = (ASPxListBox)cmbDocumentList.FindControl("documentList");
            docList.DataSource = Ds.Tables[1];
            docList.TextField = "DocNo";
            docList.ValueField = "id";
            docList.DataBind();


            if (Ds.Tables[2].Rows.Count > 0)
            {
                hdlastStatus.Value = Ds.Tables[2].Rows[0]["FollowUsing"].ToString();
            }
              if (Ds.Tables[3].Rows.Count > 0)
            {
                hdStatusNewCust.Value = Ds.Tables[3].Rows[0]["FollowUsing"].ToString();
            }

              hdNoDays.Value = Ds.Tables[5].Rows[0]["nextFollowDayLimit"].ToString();


            GridDetail.DataBind();

            
        }

        protected void ComponentPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("Id", typeof(System.String));
            Detail.Columns.Add("type", typeof(System.String));
            DataRow Ro;

            if (e.Parameter.Split('~')[0] == "Save")
            {
                ASPxListBox docList = (ASPxListBox)cmbDocumentList.FindControl("documentList");
                foreach (var itm in docList.SelectedValues)
                {
                    Ro = Detail.NewRow();
                    Ro["Id"] = itm.ToString().Split('~')[0];
                    Ro["type"] = itm.ToString().Split('~')[1];
                    Detail.Rows.Add(Ro);
                }

                ProcedureExecute proc = new ProcedureExecute("prc_followUp_addedit");
                proc.AddVarcharPara("@Action", 100, hdAction.Value);
                proc.AddVarcharPara("@custId", 100, Request.QueryString["custId"]);
                proc.AddPara("@FollowDate", dtFollowDate.Date);
                proc.AddPara("@Documents", cmbDocumentList.Text);
                proc.AddPara("@FollowUsing", cmbFollowUp.Value);
                proc.AddPara("@openClsoeStatus", cmbOpenClose.Value);
                proc.AddPara("@openClsoe", cmbOpenClose.Text);
                proc.AddPara("@NextFollowDate", dtNextFollowupdate.Value);
                proc.AddPara("@Remarks", memoRemarks.Text);
                proc.AddPara("@id", hdFollowId.Value);
                proc.AddPara("@details", Detail);
                proc.AddPara("@UserId", Convert.ToString(Session["userid"]));
                proc.AddBooleanPara("@isSuccess", false, QueryParameterDirection.Output);
                proc.AddVarcharPara("@RetMsg", 200, "", QueryParameterDirection.Output);
                proc.RunActionQuery();

                ComponentPanel.JSProperties["cpisSuccess"] = Convert.ToString(proc.GetParaValue("@isSuccess"));
                ComponentPanel.JSProperties["cpRetMsg"] = Convert.ToString(proc.GetParaValue("@RetMsg"));

                if (Convert.ToBoolean(proc.GetParaValue("@isSuccess")))
                {
                    hdAction.Value = "Add";
                    docList.UnselectAll();
                    cmbDocumentList.Text = "";
                    cmbFollowUp.Value = "Phone";
                    cmbOpenClose.Value = "O";
                    dtNextFollowupdate.Value = null;
                    memoRemarks.Text = "";
                }
            }
            else if (e.Parameter.Split('~')[0] == "Edit")
            {
                ProcedureExecute proc = new ProcedureExecute("prc_Followup");
                proc.AddVarcharPara("@action", 100, "LoadEditFollowUpEntry");
                proc.AddVarcharPara("@Customer", 20, Request.QueryString["custId"]);
                proc.AddVarcharPara("@FromDate", 10, Request.QueryString["FromDt"]);
                proc.AddVarcharPara("@ToDate", 10, Request.QueryString["ToDt"]);
                proc.AddVarcharPara("@id", 20, e.Parameter.Split('~')[1]);
                DataSet Ds = proc.GetDataSet();
                hdAction.Value = "Edit";
                dtFollowDate.Date = Convert.ToDateTime(Ds.Tables[0].Rows[0]["FollowDate"]);
                cmbFollowUp.Value = Convert.ToString(Ds.Tables[0].Rows[0]["FollowUsing"]);
                cmbDocumentList.Text = Convert.ToString(Ds.Tables[0].Rows[0]["Documents"]);
                cmbOpenClose.Value = Convert.ToString(Ds.Tables[0].Rows[0]["openClsoeStatus"]);
                memoRemarks.Text = Convert.ToString(Ds.Tables[0].Rows[0]["Remarks"]);

                if (string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["NextFollowDate"].ToString()))
                    dtNextFollowupdate.Value = null;
                else
                    dtNextFollowupdate.Date = Convert.ToDateTime(Ds.Tables[0].Rows[0]["NextFollowDate"]);

                ASPxListBox docList = (ASPxListBox)cmbDocumentList.FindControl("documentList");
                docList.DataSource = Ds.Tables[2];
                docList.TextField = "DocNo";
                docList.ValueField = "id";
                docList.DataBind();

                dtNextFollowupdate.ClientEnabled = false;



                foreach (DataRow dr in Ds.Tables[1].Rows)
                {
                    ListEditItem item = docList.Items.FindByValue(Convert.ToString(dr["ids"]));
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }


            }
            else if (e.Parameter.Split('~')[0] == "Delete")
            {
                ProcedureExecute proc = new ProcedureExecute("prc_Followup");
                proc.AddVarcharPara("@action", 100, "DelFollowUpEntry");
                proc.AddVarcharPara("@id", 100, e.Parameter.Split('~')[1]);
                proc.AddVarcharPara("@RetMsg", 200, "", QueryParameterDirection.Output);
                proc.RunActionQuery();
                 
                ComponentPanel.JSProperties["cpRetMsg"] = Convert.ToString(proc.GetParaValue("@RetMsg"));
            }
        }



        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "id";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.tbl_trans_FollowupHeaders
                    where
                    d.CustID == Request.QueryString["custId"] &&
                    (d.openClsoe=="Close" || d.NextFollowDate <=Convert.ToDateTime(Request.QueryString["ToDt"]))
                    orderby d.FollowedOn descending
                    select d;
            e.QueryableSource = q;

        }

        protected void LinqServerModeDataSource1_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            if (Request.QueryString["showZeroBal"]=="1")
            {
                List<int> branchidlist = new List<int>(Array.ConvertAll(Request.QueryString["BranchId"].Split(','), int.Parse));
                e.KeyExpression = "UniqueKey";
               // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
                string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.V_followupDetails
                        where (d.ComapreDate == null || (d.ComapreDate >= Convert.ToDateTime(Request.QueryString["FromDt"]) &&
                                  d.ComapreDate <= Convert.ToDateTime(Request.QueryString["ToDt"]))) &&
                        d.CustId == Request.QueryString["CustId"] &&
                        branchidlist.Contains(Convert.ToInt32(d.Invoice_BranchId))
                        orderby d.branch_description
                        select d;
                e.QueryableSource = q;
            }
            else {
                List<int> branchidlist = new List<int>(Array.ConvertAll(Request.QueryString["BranchId"].Split(','), int.Parse));
                e.KeyExpression = "UniqueKey";
               // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

                string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.V_followupDetails
                        where (d.ComapreDate == null || (d.ComapreDate >= Convert.ToDateTime(Request.QueryString["FromDt"]) &&
                                  d.ComapreDate <= Convert.ToDateTime(Request.QueryString["ToDt"]))) &&
                        d.CustId == Request.QueryString["CustId"] &&
                        branchidlist.Contains(Convert.ToInt32(d.Invoice_BranchId)) && d.UnPaidAmount>0
                        orderby d.branch_description
                        select d;
                e.QueryableSource = q;
            }
        }



        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);

            }
        }


        public void bindexport(int Filter)
        {


            exporter.PageHeader.Left = "Payment Follow-up";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
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