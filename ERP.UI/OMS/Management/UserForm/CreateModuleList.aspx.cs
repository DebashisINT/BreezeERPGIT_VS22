using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.UserForm
{
    public partial class CreateModuleList : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Init(object sender, EventArgs e)
        {
            userDataControl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack) 
            {
                Grid.DataBind();
                userGrid.DataBind();
            }
            userGrid.JSProperties["cpRetMsg"] = null;
        }

        protected void gridAttendance_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = oDBEngine.GetDataTable("select id,Name,case Userwise when 1 then 'Yes' else 'No' end Userwise  from tbl_master_UDTMain");
            Grid.DataSource = dt;
        }

        [WebMethod]
        public static string CreateModule(string ModName)
        {
            string RetMsg = "-1~Error";
            ProcedureExecute proc;
            try
            {
                 proc = new ProcedureExecute("prc_UserDefineForm");
                proc.AddVarcharPara("@Action", 100, "ModCount");
                proc.AddIntegerPara("@status", null, QueryParameterDirection.Output);
                proc.RunActionQuery();

                string CM = Convert.ToString(ConfigurationManager.AppSettings["CM"]);
                int no=999;
                switch (CM)
                {
                    case "A":
                        no = 2;
                        break;
                    case "B":
                        no = 3;
                        break;
                    case "C":
                        no = 4;
                        break;
                    case "D":
                        no = 5;
                        break;
                    case "E":
                        no = 6;
                        break;
                    case "F":
                        no = 7;
                        break;
                    case "G":
                        no = 8;
                        break;
                    case "H":
                        no = 9;
                        break;
                    case "J":
                        no = 10;
                        break;
                    case "K":
                        no = 11;
                        break;
                }

                if (Convert.ToInt32(proc.GetParaValue("@status")) < no)
                {


                proc = new ProcedureExecute("prc_UserDefineForm");
                proc.AddVarcharPara("@Action", 100, "CreateNew");
                proc.AddVarcharPara("@ModName", 500, ModName);
                proc.AddVarcharPara("@Userid", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
                proc.AddVarcharPara("@outputMsg", 200, "", QueryParameterDirection.Output);
                proc.AddIntegerPara("@status", null, QueryParameterDirection.Output);
                proc.RunActionQuery();

                RetMsg = proc.GetParaValue("@status").ToString() + "~" + proc.GetParaValue("@outputMsg").ToString();
                }
                else
                {
                    RetMsg = "-1~Cannot Create more than 2 custom Module.";
                }
            }
            catch (Exception ex) {
                RetMsg = "-1~"+ex.Message;
            }

            return RetMsg;
        }


        [WebMethod]
        public static string CreateMenu(string ModName)
        {
            string RetMsg = "-1~Error";
            ProcedureExecute proc;
            try
            {
                
                    proc = new ProcedureExecute("prc_UserDefineForm");
                    proc.AddVarcharPara("@Action", 100, "CraeteMenu");
                    proc.AddVarcharPara("@ModName", 500, ModName);
                    proc.AddVarcharPara("@Userid", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
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
        public static string MakeUserwise(string id)
        {
            string RetMsg = "-1~Error";
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_UserDefineForm");
                proc.AddVarcharPara("@Action", 100, "MakeUserwise");
                proc.AddVarcharPara("@id", 500, id);
                proc.AddVarcharPara("@Userid", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
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

        protected void FiledType_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string Id = e.Parameter;
            ProcedureExecute proc = new ProcedureExecute("prc_UserDefineForm");
            proc.AddVarcharPara("@Action", 100, "GetDateFieldFilter");
            proc.AddPara("@id", Id);
            DataSet Ds = proc.GetDataSet();

            FiledType.DataSource = Ds.Tables[0];
            FiledType.ValueField = "id";
            FiledType.TextField = "FieldName";
            FiledType.DataBind();

            if (Convert.ToString(Ds.Tables[1].Rows[0][0]) != "")
                FiledType.Value=Convert.ToString(Ds.Tables[1].Rows[0][0]);
        }



        [WebMethod]
        public static string UpdateDateFilter(string fieldName, string hdModId)
        {
            string RetMsg = "-1~Error";
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_UserDefineForm");
                proc.AddVarcharPara("@Action", 100, "UpdateDateFilter");
                proc.AddVarcharPara("@id", 500, hdModId);
                proc.AddVarcharPara("@fieldName", 500, fieldName);
                proc.AddVarcharPara("@Userid", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
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

        protected void userGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            userGrid.ClientSideEvents.EndCallback = null;
            string GetParam = e.Parameters;
            if (GetParam.Split('~')[0] == "BindGrid")
            {
               
                DataTable userList = oDBEngine.GetDataTable("select Userid from tbl_master_customModule_UserMap where ModuleId=" + hdModId.Value);
                userGrid.Selection.UnselectAll();
                foreach (DataRow dr in userList.Rows)
                {
                    userGrid.Selection.SelectRowByKey(Convert.ToString(dr["Userid"]));
                }

            }
            else if (GetParam.Split('~')[0] == "Save")
            {
                string ListOfSelectedUser = "";
                var listSelect =  userGrid.GetSelectedFieldValues("user_id");
                foreach (var obj in listSelect)
                {
                    ListOfSelectedUser = ListOfSelectedUser + Convert.ToString(obj) + ",";
                }
                ListOfSelectedUser = ListOfSelectedUser.TrimEnd(',');


                ProcedureExecute proc = new ProcedureExecute("prc_UserDefineForm");
                proc.AddVarcharPara("@Action", 100, "UpdateUserMapTable");
                proc.AddVarcharPara("@id", 500, hdModId.Value);
                proc.AddVarcharPara("@UserList", -1, ListOfSelectedUser);
                proc.AddVarcharPara("@Userid", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
                proc.AddVarcharPara("@outputMsg", 200, "", QueryParameterDirection.Output);
                proc.AddIntegerPara("@status", null, QueryParameterDirection.Output);
                proc.RunActionQuery();

                userGrid.JSProperties["cpRetMsg"] = "1";
                //userGrid.ClientSideEvents.EndCallback = "Saveandcloseuserpopup";

            }
        }


    }
}