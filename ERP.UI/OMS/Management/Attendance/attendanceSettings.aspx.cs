using DataAccessLayer;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Attendance
{
    public partial class attendanceSettings : System.Web.UI.Page
    {
      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Grid.DataBind();
            }
        }



        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_internalId";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            var q = from d in dc.v_employee_details
                    orderby d.Name
                    select d;
            e.QueryableSource = q;

        }

        protected void bioGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = oDBEngine.GetDataTable("select * from tbl_master_empBioIdMap where EmpId='" + EmpId.Value + "'");
            bioGrid.DataSource = dt;
        }

        protected void bioGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }


        [WebMethod]
        public static string BioIdUpdateedit(string EmpId, string Id, string BioId)
        {
            string RetMsg = "-1~Error";
            ProcedureExecute proc;
            try
            {
                proc = new ProcedureExecute("prc_biometricSettings");
                proc.AddVarcharPara("@Action", 100, "AddEdit");
                proc.AddVarcharPara("@EmpId", 500, EmpId);
                proc.AddPara("@id", Id);
                proc.AddPara("@BioId", BioId);
                proc.AddVarcharPara("@outputMsg", 200, "", QueryParameterDirection.Output);
                proc.AddIntegerPara("@status", null, QueryParameterDirection.Output);
                proc.RunActionQuery();

                RetMsg = proc.GetParaValue("@status").ToString() + "~" + proc.GetParaValue("@outputMsg").ToString();

            }
            catch (Exception ex)
            {
                RetMsg = "-1~" + ex.Message;
            }

            return RetMsg;
        }


        [WebMethod]
        public static string BioIdDelete(string BioId)
        {
            string RetMsg = "-1~Error";
            ProcedureExecute proc;
            try
            {
                proc = new ProcedureExecute("prc_biometricSettings");
                proc.AddVarcharPara("@Action", 100, "Delete");  
                proc.AddPara("@BioId", BioId);
                proc.AddVarcharPara("@outputMsg", 200, "", QueryParameterDirection.Output);
                proc.AddIntegerPara("@status", null, QueryParameterDirection.Output);
                proc.RunActionQuery();

                RetMsg = proc.GetParaValue("@status").ToString() + "~" + proc.GetParaValue("@outputMsg").ToString();

            }
            catch (Exception ex)
            {
                RetMsg = "-1~" + ex.Message;
            }

            return RetMsg;
        }

        protected void payBranch_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            payBranch.JSProperties["cpSave"] = null;
            ProcedureExecute proc;
            if (e.Parameter.Split('~')[0] == "Save")
            {
                proc = new ProcedureExecute("prc_biometricSettings");
                proc.AddVarcharPara("@Action", 100, "updateBranch");
                proc.AddVarcharPara("@EmpId", 500, EmpId.Value);
                proc.AddPara("@id", e.Parameter.Split('~')[1]);
                proc.RunActionQuery();
                payBranch.JSProperties["cpSave"] = "Yes";

            }
            else { 
                proc = new ProcedureExecute("prc_biometricSettings");
                proc.AddVarcharPara("@Action", 100, "GetAllBranch");
                proc.AddVarcharPara("@EmpId", 500, EmpId.Value);
                DataSet DT = proc.GetDataSet();
                payBranch.DataSource = DT.Tables[0];
                payBranch.ValueField = "branch_id";
                payBranch.TextField = "branch_description";
                payBranch.DataBind();

                payBranch.Value =Convert.ToString(DT.Tables[1].Rows[0][0]);
            }
        }


    }
}