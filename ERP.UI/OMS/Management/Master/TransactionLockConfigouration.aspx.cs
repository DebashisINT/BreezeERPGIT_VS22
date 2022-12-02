using DataAccessLayer;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class TransactionLockConfigouration : System.Web.UI.Page
    {

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

                 DataTable dtsValue=new DataTable();
                dtsValue = oDBEngine.GetDataTable("select  top 1  convert(varchar(50),FromDate,110)  FromDate,convert(varchar(50),Todate,110)  Todate from Master_Lock where ModuleType='All'");
                if (dtsValue != null && dtsValue.Rows.Count > 0)
                {
                    hdnMasterFromdate.Value = Convert.ToString(dtsValue.Rows[0]["FromDate"]);
                    hdnMasterToDate.Value = Convert.ToString(dtsValue.Rows[0]["Todate"]);
                }

            if (!IsPostBack)
            {
              
                
            }
        }


        protected void AllSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (dt == null || dt.Rows.Count == 0)
                {
                    dt.Columns.Add("ModuleId", typeof(string));
                    dt.Columns.Add("Type", typeof(string));
                    dt.Columns.Add("FromDate", typeof(string));
                    dt.Columns.Add("ToDate", typeof(string));
                    dt.Columns.Add("TypeStatus", typeof(string));
                }
                string ModuleId = "";
                ModuleId = Convert.ToString(hdnCalcommitProductId.Value);
               
                
                dt.AcceptChanges();
                DataSet dsInst = new DataSet();

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Prc_LockConfigour_AddEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveLock");
                cmd.Parameters.AddWithValue("@SelectedComponentList", hdnCalcommitProductId.Value);
                cmd.Parameters.AddWithValue("@LockConfigure", dt);
                cmd.Parameters.AddWithValue("@AllFromDate", Convert.ToDateTime(AllFromDate.Value));
                cmd.Parameters.AddWithValue("@AllToDate", Convert.ToDateTime(AllToDate.Value));

                cmd.Parameters.AddWithValue("@UserId", Convert.ToString(Session["userid"]));
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                cmd.Dispose();
                con.Dispose();

                if (idFromString == 1)
                {
                    //cmbModule.SelectedIndex = 0;
                    AddFormDate.Value = "";
                    AddtoDate.Value = "";
                    EditFormDate.Value = "";
                    EditToDate.Value = "";
                    DeleteFormDate.Value = "";
                    DeleteToDate.Value = "";
                    AllFromDate.Value = "";
                    AllToDate.Value = "";
                    //ChkAllProduct.Checked = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>DataSaved();</script>");

                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please try again later.')</script>");
                }



            }
            catch (Exception ex)
            {

            }
        }
        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (dt == null || dt.Rows.Count == 0)
                {
                    dt.Columns.Add("ModuleId", typeof(string));
                    dt.Columns.Add("Type", typeof(string));
                    dt.Columns.Add("FromDate", typeof(string));
                    dt.Columns.Add("ToDate", typeof(string));
                    dt.Columns.Add("TypeStatus", typeof(string));
                }
                string ModuleId = "";
                if (Convert.ToString(hdnCalcommitProductId.Value) == "All")
                {
                    string Mid = "";
                    DataTable dtsr = new DataTable();
                    dtsr = oDBEngine.GetDataTable("select distinct(Module_Id) Module_Id from Trans_LockConfigouration");
                    if (dtsr != null && dtsr.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtsr.Rows.Count; i++)
                        {
                            if (Mid == "")
                            {
                                Mid = dtsr.Rows[i]["Module_Id"].ToString();
                            }
                            else
                            {
                                Mid = Mid + "," + dtsr.Rows[i]["Module_Id"];
                            }

                        }
                    }

                    ModuleId = Mid;

                }
                else
                {
                    ModuleId = Convert.ToString(hdncWiseProductId.Value);
                }
                if (ModuleId != "")
                {
                    var modId = ModuleId.Split(',');

                    foreach (var item in modId)
                    {
                        if (AddFormDate.Value != null && AddtoDate.Value != null)
                        {
                          
                                dt.Rows.Add(item, "Add", Convert.ToDateTime(AddFormDate.Value), Convert.ToDateTime(AddtoDate.Value), Convert.ToBoolean(chkAdd.Checked));
                           
                        }
                        if (EditFormDate.Value != null && EditToDate.Value != null)
                        {
                          
                                dt.Rows.Add(item, "Edit", Convert.ToDateTime(EditFormDate.Value), Convert.ToDateTime(EditToDate.Value), Convert.ToBoolean(chkEdit.Checked));
                            
                        }
                        if (DeleteFormDate.Value != null && DeleteToDate.Value != null)
                        {
                           
                                dt.Rows.Add(item, "Delete", Convert.ToDateTime(DeleteFormDate.Value), Convert.ToDateTime(DeleteToDate.Value), Convert.ToBoolean(chkDelete.Checked));
                            
                        }
                    }


                }
                dt.AcceptChanges();
                DataSet dsInst = new DataSet();

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Prc_LockConfigour_AddEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveLock");
                cmd.Parameters.AddWithValue("@SelectedComponentList", hdnCalcommitProductId.Value);
                cmd.Parameters.AddWithValue("@LockConfigure", dt);

                cmd.Parameters.AddWithValue("@UserId", Convert.ToString(Session["userid"]));
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                cmd.Dispose();
                con.Dispose();

                if (idFromString == 1)
                {
                    //cmbModule.SelectedIndex = 0;
                    AddFormDate.Value = "";
                    AddtoDate.Value = "";
                    EditFormDate.Value = "";
                    EditToDate.Value = "";
                    DeleteFormDate.Value = "";
                    DeleteToDate.Value = "";
                    //ChkAllProduct.Checked = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>DataSaved();</script>");

                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Please try again later.')</script>");
                }



            }
            catch (Exception ex)
            {

            }
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Module_Id";

        

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

             ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
             var q = from d in dc.v_LockModuleLists
                     orderby d.ParentName
                                select d;
                        e.QueryableSource = q;
                 
           
        }
        public DataTable GetModule()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_LockConfigouration");
            proc.AddVarcharPara("@Action", 500, "GetModuleDetails");

            dt = proc.GetTable();
            return dt;
        }


        [WebMethod]

        public static object GetModule(string SearchKey)
        {
            List<Moduleclass> listcwiseProducts = new List<Moduleclass>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtBankdet = new DataTable();

                dtBankdet = oDBEngine.GetDataTable(" select Module_Id,Module_Name from Trans_LockConfigouration where   Module_Name like '%" + SearchKey + "%'  order by Module_Name");


                listcwiseProducts = (from DataRow dr in dtBankdet.Rows
                                     select new Moduleclass()
                                     {
                                         Module_Id = Convert.ToInt64(dr["Module_Id"]),
                                         Module_Name = Convert.ToString(dr["Module_Name"])

                                     }).ToList();
            }

            return listcwiseProducts;
        }

        [WebMethod]
        public static object DeleteAllFreezeData()
        {
        
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_LockConfigouration");
                proc.AddVarcharPara("@Action", 500, "DeleteAllFreezeData");
                DataTable address = proc.GetTable();

            }
            return null;

        }

        public class Moduleclass
        {
            public Int64 Module_Id { get; set; }
            public string Module_Name { get; set; }

        }

        [WebMethod]
        public static String LockValue(string Val)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string LockStatus;

            DataTable dt2 = oDBEngine.GetDataTable("select Isnull(Module_Add,0)Module_Add,Isnull(Module_Edit,0)Module_Edit,Isnull(Module_Delete,0)Module_Delete,Isnull(Module_Copy,0)Module_Copy from Trans_LockConfigouration where Module_Id='" + Val + "'");

            if (dt2.Rows.Count > 0)
            {
                LockStatus = Convert.ToString(dt2.Rows[0]["Module_Add"]) + '~' + Convert.ToString(dt2.Rows[0]["Module_Edit"]) + '~' + Convert.ToString(dt2.Rows[0]["Module_Delete"]) + '~' + Convert.ToString(dt2.Rows[0]["Module_Copy"]);
                return LockStatus;
            }
            else
            {
                LockStatus = "0" + '~' + "0" + '~' + "0" + '~' + "0";
                return LockStatus;
            }
        }

        [WebMethod]
        public static String ModuleCheckBoxValue(string Val)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string retVal = "0";
            string ModVal = "0";
            string CheckBoxVal = "0";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_LockConfigouration");
            proc.AddVarcharPara("@Action", 500, "CheckBoxValue");
            proc.AddBigIntegerPara("@Doc_id", Convert.ToInt64(Val));
            dt = proc.GetTable();

            DataTable dts = oDBEngine.GetDataTable("select ISNULL(Module_Add,0) Module_Add,ISNULL(Module_Edit,0) Module_Edit,ISNULL(Module_Delete,0) Module_Delete from Trans_LockConfigouration  where Module_Id='" + Convert.ToInt64(Val) + "'");
            if (dts != null && dts.Rows.Count>0)
            {
                CheckBoxVal = Convert.ToString(dt.Rows[0]["Module_Add"]) + '~' + Convert.ToString(dt.Rows[0]["Module_Edit"]) + '~' + Convert.ToString(dt.Rows[0]["Module_Delete"]);
            }
            if (dt != null && dt.Rows.Count > 0)
            {

                retVal = Convert.ToString(dt.Rows[0]["LockDetailsVal"]);

            }
            else
            {
                retVal = "0";
            }
            return retVal + '~' + CheckBoxVal;
        }
        [WebMethod]
        public static String ModuleDateValue(string Val)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string retVal = "0";
            string ModVal = "0";
            string CheckBoxVal = "";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_LockConfigouration");
            proc.AddVarcharPara("@Action", 500, "GetModuleDateValue");
            proc.AddBigIntegerPara("@Doc_id", Convert.ToInt64(Val));
            dt = proc.GetTable();

            DataTable dts = oDBEngine.GetDataTable("select ISNULL(Module_Add,0) Module_Add,ISNULL(Module_Edit,0) Module_Edit,ISNULL(Module_Delete,0) Module_Delete from Trans_LockConfigouration  where Module_Id='" + Convert.ToInt64(Val) + "'");
            if (dts != null && dts.Rows.Count > 0)
            {
                CheckBoxVal = Convert.ToString(dts.Rows[0]["Module_Add"]) + '~' + Convert.ToString(dts.Rows[0]["Module_Edit"]) + '~' + Convert.ToString(dts.Rows[0]["Module_Delete"]);
            }

            if (dt != null && dt.Rows.Count > 0)
            {

                retVal = Convert.ToString(dt.Rows[0]["LockDetailsVal"]);

            }
            else
            {
                retVal = "0";
            }
            return retVal + '~' + CheckBoxVal;
        }
    }
}