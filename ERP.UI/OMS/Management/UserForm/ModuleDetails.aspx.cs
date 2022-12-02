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
    public partial class ModuleDetails : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadData();
                AddEdit.Value = "Add";
            }
        }

        private void loadData()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_UserDefineForm");
            proc.AddVarcharPara("@Action", 100, "GetDetails");
            proc.AddVarcharPara("@id", 500, Request.QueryString["id"]);
            DataSet ds = proc.GetDataSet();

            ModuleName.Text = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);

            FiledType.DataSource = ds.Tables[1];
            FiledType.TextField = "VissibleText";
            FiledType.ValueField = "id";
            FiledType.DataBind();

            ModuleId.Value = Request.QueryString["id"];
           
              Grid.DataBind();
            
        }


        [WebMethod]
        public static string AddField(FieldDetails ModDet)
        {
             string RetMsg = "-1~Error";
             try
             {
                 ProcedureExecute proc = new ProcedureExecute("prc_UserDefineFormDetails");
                 proc.AddVarcharPara("@Action", 100, "CreateNew");
                 proc.AddIntegerPara("@ModuleId", ModDet.ModuleId);
                 proc.AddIntegerPara("@FiledType",  ModDet.FiledType);
                 proc.AddVarcharPara("@Name", 500, ModDet.Name);
                 proc.AddBooleanPara("@VissibleInList", ModDet.VissibleInList);
                 proc.AddIntegerPara("@OrderBy", ModDet.OrderBy);
                 proc.AddBooleanPara("@Mandatory", ModDet.Mandatory);
                 proc.AddPara("@Formula", ModDet.Formula);
                 proc.AddPara("@tagId", ModDet.AddEdit);
                 proc.AddPara("@values", ModDet.Values);

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
        public static string DeleteField(string id)
        {
            string RetMsg = "-1~Error";
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_UserDefineFormDetails");
                proc.AddVarcharPara("@Action", 100, "DeleteField");
                proc.AddPara("@tagId", id);
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

        public class FieldDetails 
        {
            public int ModuleId { get; set; }
            public int FiledType { get; set; }
            public string Name { get; set; }
            public bool VissibleInList { get; set; }
            public int OrderBy { get; set; }
            public bool Mandatory { get; set; }
            public string Formula { get; set; }
            public string AddEdit { get; set; }
            public string Values { get; set; }
        
        }

        protected void Grid_DataBinding(object sender, EventArgs e)
        {
            DataTable Dt = oDBEngine.GetDataTable(@"select det.id,FieldName,VissibleText,OrderBy,case Mandatory when 1 then 'Yes' else 'No' end Mandatory  from tbl_master_UDTDetails det
                                                    inner join tbl_master_UDTFieldtype ty on det.Fieldtype = ty.id
                                                    where MainId=" + Request.QueryString["id"]);
            Grid.DataSource = Dt;
            
        }

        protected void columnList_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_UserDefineFormDetails");
            proc.AddVarcharPara("@Action", 100, "GetFormulaField");
            proc.AddVarcharPara("@tagId", 500, Request.QueryString["id"]);
            DataTable dt= proc.GetTable();
            columnList.DataSource = dt;
            columnList.ValueField = "FieldName";
            columnList.TextField = "FieldName";
            columnList.DataBind();
        }

        protected void ComponentPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string id = e.Parameter;

            ProcedureExecute proc = new ProcedureExecute("prc_UserDefineFormDetails");
            proc.AddVarcharPara("@Action", 100, "GetFieldDetails");
            proc.AddVarcharPara("@tagId", 500, id);
            DataSet Ds = proc.GetDataSet();
            DataTable dt = Ds.Tables[0];
            FiledType.Value = Convert.ToString(dt.Rows[0]["Fieldtype"]);
            txtName.Text = Convert.ToString(dt.Rows[0]["FieldName"]);
            OrderBy.Text = Convert.ToString(dt.Rows[0]["OrderBy"]);
            vissibleinList.Checked = Convert.ToBoolean(dt.Rows[0]["VissibleinList"]);
            Mandetory.Checked = Convert.ToBoolean(dt.Rows[0]["Mandatory"]);
            chkIsFormula.Checked = Convert.ToString(dt.Rows[0]["Formula"]).Trim() == "" ? false : true;
            memoFormula.Text = Convert.ToString(dt.Rows[0]["Formula"]);
            txtValues.Text = Convert.ToString(dt.Rows[0]["Value"]);
            AddEdit.Value = id;


            columnList.DataSource = Ds.Tables[1];
            columnList.ValueField = "FieldName";
            columnList.TextField = "FieldName";
            columnList.DataBind();
        }

    }
}