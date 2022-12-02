using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using DataAccessLayer;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_Root_AddUserCompany : ERP.OMS.ViewState_class.VSPage
    {
        string[,] AllType;
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        ReadWriteMasterDatabaseBL ReadWriteMasterDatabaseBL = new ReadWriteMasterDatabaseBL();



        protected void Page_init(object s, EventArgs e)
        {
            SelectName.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // txtContact.Attributes.Add("onkeyup", "showOptions(this,'Company',event)");
            if (!IsPostBack)
            {

                BindGrid();
            }
        }
        /*Code  Added  By Priti on 06122016 to use jquery Choosen*/
        [WebMethod]
        public static List<string> ALLContact(string reqStr)
        {
            ReadWriteMasterDatabaseBL MasterDatabaseBL = new ReadWriteMasterDatabaseBL();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = MasterDatabaseBL.CompanyRead();
            //DT = oDBEngine.GetDataTable("ERP_Company_List", "Id,Name ", "Name like '" + reqStr + "%' ");
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));

            }

            return obj;
        }
        //...............code end........
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtContact_hidden.Value != "")
            {
                //DataTable dtV = oDBEngine.GetDataTable("Master_UserCompany", "*", "UserCompany_UserID='" + Convert.ToString(Request.QueryString["id"]) + "' and UserCompany_CompanyID='" + txtContact_hidden.Value + "'");
                string masterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
                // DataTable dtV = oDBEngine.GetDataTable("insert into " + masterDbanem + ".dbo.ERP_Company_List(Id,Name,DbName,IsDefault) values('" + maxColumn + "','" + companyName + "','" + dbName + "',0)");
                ProcedureExecute pro = new ProcedureExecute("Prc_MasterDatabase_Company");
                pro.AddVarcharPara("@Action", 100, "InsertCompany");
                pro.AddVarcharPara("@MasterDatabaseName", 200, masterDbname);
                pro.AddVarcharPara("@LoginId", 200, Convert.ToString(Request.QueryString["LoginID"]));
                pro.AddVarcharPara("@CompanyId", 200, txtContact_hidden.Value);
                DataTable dtV = pro.GetTable();

                if (dtV != null && dtV.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript1454", "jAlert('Already Added..!!');", true);
                    return;
                    //Response.Redirect("/OMS/Management/Master/Root_AddUserCompany.aspx?id=" + Request.QueryString["id"].ToString());
                }
                else
                {
                    oDBEngine.InsurtFieldValue("Master_UserCompany", "UserCompany_UserID,UserCompany_CompanyID,UserCompany_CreateUser,UserCompany_CreateDateTime", "'" + Convert.ToString(Request.QueryString["LoginID"]) + "','" + txtContact_hidden.Value + "','" + Convert.ToString(Session["userid"]) + "','" + DateTime.Now + "'");
                    Response.Redirect("/OMS/Management/Master/Root_AddUserCompany.aspx?id=" + Convert.ToString(Request.QueryString["id"]) + "&LoginID=" + Convert.ToString(Request.QueryString["LoginID"]));
                }
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "JScript1", "jAlert('Please Select Contact!!');", true);
            }




            txtContact_hidden.Value = "";
            // txtContact.Text = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "CallGrid();", true);
            BindGrid();

        }

        public void BindGrid()
        {
            string masterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            string id = Convert.ToString(Request.QueryString["LoginID"]);
            if (id != null)
            {
                //SelectName.SelectCommand = "select UserCompany_ID, (select user_name from tbl_master_user where user_id=UserCompany_UserID) as UserName ,(select cmp_name from tbl_master_company where cmp_internalid=UserCompany_CompanyID) as Company,UserCompany_CompanyID  from  dbo.Master_UserCompany where  UserCompany_UserID=" + id + "";
                SelectName.SelectCommand = "select Cmpnymap.Id UserCompany_ID,Cmpnymap.Id UserCompany_CompanyID,Cmpnylist.Name Company from " + masterDbname + ".dbo.user_company_map Cmpnymap inner join " + masterDbname + ".dbo.ERP_Company_List Cmpnylist on Cmpnylist.Id=Cmpnymap.CompanyId where LoginId='" + Convert.ToString(Request.QueryString["LoginID"]) + "'";

                GridName.DataBind();
            }
        }

        //protected void BtnCancel_Click(object sender, EventArgs e)
        //{


        //    ScriptManager.RegisterStartupScript(this, GetType(), "JScript41", "parent.editwin.close();", true);

        //}

        protected void GridName_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string masterDbname = ConfigurationSettings.AppSettings["MasterDBName"]; //added by chinmoy
            if (e.Parameters != "")
            {
                string tranid = Convert.ToString(e.Parameters);
                // oDBEngine.DeleteValue("Master_UserCompany ", "UserCompany_ID ='" + tranid + "'");
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");
                ProcedureExecute pro = new ProcedureExecute("Prc_MasterDatabase_Company");
                pro.AddVarcharPara("@Action", 100, "DeleteCompany");
                pro.AddVarcharPara("@MasterDatabaseName", 200, masterDbname);
                pro.AddVarcharPara("@PId", 200, tranid);
                pro.GetScalar();

            }
            BindGrid();

        }
    }
}