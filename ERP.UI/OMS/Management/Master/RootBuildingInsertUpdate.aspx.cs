using System;
using System.Web.UI;
using System.Configuration;
using ClsDropDownlistNameSpace;
using System.Data;
using BusinessLogicLayer;
using System.Web.UI.WebControls;
using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Data.SqlClient;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_RootBuildingInsertUpdate : ERP.OMS.ViewState_class.VSPage
    {
        //  DBEngine objEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        clsDropDownList OclsDropDownList = new clsDropDownList();
        WarehouseConfigMasterBL objWarehouseConfigMaster = new WarehouseConfigMasterBL();
        CommonBL ComBL = new CommonBL();

        protected void Page_Init(object sender, EventArgs e)
        {
            sdLevel.ConnectionString = Convert.ToString(Session["ERPConnection"]);

            BranchdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            string ID = Request.QueryString["id"].ToString();
            hdnEditID.Value = ID;
            //  BtnAdd.Attributes.Add("onclick", "callpopup()");
            string pageAccess = objEngine.CheckPageAccessebility("RootBuilding.aspx");
            Session["PageAccess"] = pageAccess;

            if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
            {
                BtnAdd.Enabled = true;
            }
            else
            {
                BtnAdd.Enabled = false;
            }
            if (Session["PageAccess"].ToString().Trim() == "DelAdd" && Request.QueryString["id"].ToString() != "ADD")
            {
                BtnSave.Enabled = false;
            }
            else
            {
                BtnSave.Enabled = true;
            }
            if (!IsPostBack)
            {
                string WarehouseCodeRequireWHMaster = ComBL.GetSystemSettingsResult("WarehouseCodeRequireWHMaster");
                if (WarehouseCodeRequireWHMaster != null)
                {
                    if (WarehouseCodeRequireWHMaster == "No")
                    {
                        DivWHCode.Style.Add("display", "none");
                    }
                    else
                    {
                        DivWHCode.Style.Add("display", "!inline-block");
                    }
                }

                MasterSettings masterBl = new MasterSettings();
                int Multiwarehouse = Convert.ToInt32(masterBl.GetSettings("IaMultilevelWarehouse"));
                if (Multiwarehouse == 1)
                {
                    dvMultiwarehouse.Attributes.Add("class", dvMultiwarehouse.Attributes["class"].ToString().Replace("hide", ""));
                }

                SetCountry();
                PopulateBranchByBranchHierarchy();
                //if (Request.QueryString["id"].ToString() == "ADD")
                //{
                //    BindBuilding();
                //    BindState();
                //    BindCity();
                //}
                //else
                //{

                //    BindBuilding();


                //}

                BindBuilding();


            }
        }
        public void BindBuilding()
        {
            string KeyId = Request.QueryString["id"].ToString();
            //if (KeyId == "ADD")
            //{
            //    DDLState.Visible = true;
            //    DDLCity.Visible = true;
            //}
            string[,] CareTaker = objEngine.GetFieldValue("tbl_master_contact", "cnt_internalId as Id,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')+'['+isnull(cnt_shortName,'')+']' AS Name", " cnt_contactType='EM'", 2, "cnt_firstName");
            //objEngine.AddDataToDropDownList(CareTaker, DDLCareTaker);
            OclsDropDownList.AddDataToDropDownList(CareTaker, DDLCareTaker);

            string[,] Country = objEngine.GetFieldValue("tbl_master_country", "cou_id,cou_country", null, 2, "cou_country");
            //objEngine.AddDataToDropDownList(Country, DDLCountry);
            // OclsDropDownList.AddDataToDropDownList(Country, DDLCountry);
            ShowData();
        }
        //protected void DDLCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    TxtCity.Visible = false;
        //    TxtState.Visible = false;
        //    DDLState.Visible = true;
        //    DDLCity.Visible = true;
        //    BindState();
        //    BindCity();
        //}
        //protected void DDLState_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //   // BindCity();
        //}
        //public void BindState()
        //{
        //    DDLState.Items.Clear();
        //    string[,] State = objEngine.GetFieldValue("tbl_master_state", "id,state", " countryId='" + DDLCountry.SelectedItem.Value + "'", 2);
        //    if (State[0, 0] != "n")
        //    {
        //        //objEngine.AddDataToDropDownList(State, DDLState);
        //        OclsDropDownList.AddDataToDropDownList(State, DDLState);

        //    }
        //    else
        //    {
        //    }
        //}
        //public void BindCity()
        //{
        //    if (DDLState.SelectedValue != "")
        //    {
        //        DDLCity.Items.Clear();
        //        string[,] City = objEngine.GetFieldValue("tbl_master_city", "city_id,city_name", " state_id='" + DDLState.SelectedItem.Value + "'", 2);
        //        if (City[0, 0] != "n")
        //        {
        //            //objEngine.AddDataToDropDownList(City, DDLCity);
        //            OclsDropDownList.AddDataToDropDownList(City, DDLCity);

        //        }
        //        else
        //        {
        //        }
        //    }
        //    else
        //    {
        //        DDLCity.Items.Clear();
        //    }
        //}
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int ReturnId = 0;
            //DateTime Createdate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DateTime Createdate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            string KeyId = Request.QueryString["id"].ToString();
             string BranchId = "", ComponentBranch="";
            if (KeyId == "ADD")
            {
                //objEngine.InsurtFieldValue("tbl_master_building", "bui_Name,bui_contactId,bui_address1,bui_address2,bui_address3,bui_landmark,bui_country,bui_state,bui_city,bui_pin,CreateDate,CreateUser", "'" + TxtBuilding.Text + "','" + DDLCareTaker.SelectedItem.Value + "','" + TxtAdd1.Text + "','" + TxtAdd2.Text + "','" + TxtAdd3.Text + "','" + TxtLand.Text + "','" + DDLCountry.SelectedItem.Value + "','" + DDLState.SelectedItem.Value + "','" + DDLCity.SelectedItem.Value + "','" + TxtPin.Text + "','" + Createdate + "','" + Session["userid"].ToString() + "'");
                string level = "1", countryId = "0",stateid="0";
                Int64 ParentId=0;
                if (ddl_level.SelectedValue != "")
                {
                    level = ddl_level.SelectedValue;
                }
                if (Convert.ToString(txtCountry_hidden.Value)!="")
                {
                    countryId = Convert.ToString(txtCountry_hidden.Value);
                }
                if (Convert.ToString(txtState_hidden.Value)!="")
                {
                    stateid = Convert.ToString(txtState_hidden.Value);
                }
                //if (Convert.ToString(ddl_Branch.SelectedItem.Value)!="")
                //{
                //    BranchId = Convert.ToString(ddl_Branch.SelectedItem.Value);
                //}
                if (Convert.ToString(hdnWarehouse.Value)!="")
                {
                    ParentId = Convert.ToInt64(hdnWarehouse.Value);
                }

                for (int i = 0; i < BranchGridLookup.GridView.GetSelectedFieldValues("branch_id").Count; i++)
                {

                    ComponentBranch += "," + Convert.ToString(BranchGridLookup.GridView.GetSelectedFieldValues("branch_id")[i]);
                   

                }

                BranchId = ComponentBranch.TrimStart(',');
                string Action = "InsertWarehouse";
                ReturnId = SaveWarehouseDetails(Action,Convert.ToString(TxtBuilding.Text), Convert.ToString(DDLCareTaker.SelectedItem.Value), Convert.ToString(TxtAdd1.Text), Convert.ToString(TxtAdd2.Text), Convert.ToString(TxtAdd3.Text), Convert.ToString(TxtLand.Text), Convert.ToString(countryId), Convert.ToString(txtState_hidden.Value), Convert.ToString(txtCity_hidden.Value), Convert.ToString(HdPin.Value), Createdate, Convert.ToString(Session["userid"]), BranchId, level, ParentId, KeyId,Convert.ToString(txtCode.Text));

                //objEngine.InsurtFieldValue("tbl_master_building", "bui_Name,bui_contactId,bui_address1,bui_address2,bui_address3,bui_landmark,bui_country,bui_state,bui_city,bui_pin,CreateDate,CreateUser,bui_BranchId,bui_LevelId,bui_ParentId", "'" + TxtBuilding.Text + "','" + DDLCareTaker.SelectedItem.Value + "','" + TxtAdd1.Text + "','" + TxtAdd2.Text + "','" + TxtAdd3.Text + "','" + TxtLand.Text + "','" + Convert.ToString(txtCountry_hidden.Value) + "','" + Convert.ToString(txtState_hidden.Value) + "','" + Convert.ToString(txtCity_hidden.Value) + "','" + Convert.ToString(HdPin.Value) + "','" + Createdate + "','" + Session["userid"].ToString() + "','" + ddl_Branch.SelectedItem.Value + "','" + level + "','" + hdnWarehouse.Value + "'");
                string popupscript = "";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "adfa", "parent.editwin.close();", true);
                //popupscript = "<script language='javascript'>window.opener.location.href = window.opener.location.href;window.close();</script>";
                //ClientScript.RegisterStartupScript(GetType(), "JScript", popupscript);
                //Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>parent.editwin.close();</script>");
                Response.Redirect("RootBuilding.aspx");


            }
            else
            {

                //objEngine.SetFieldValue("tbl_master_building", "bui_Name='" + TxtBuilding.Text + "',bui_contactId='" + DDLCareTaker.SelectedItem.Value + "',bui_address1='" + TxtAdd1.Text + "',bui_address2='" + TxtAdd2.Text + "',bui_address3='" + TxtAdd3.Text + "',bui_landmark='" + TxtLand.Text + "',bui_country='" + DDLCountry.SelectedItem.Value + "',bui_state='" + DDLState.SelectedItem.Value + "',bui_city='" + DDLCity.SelectedItem.Value + "',bui_pin='" + TxtPin.Text + "',LastModifyDate='" + Createdate + "',LastModifyUser='" + Session["userid"].ToString() + "'", " bui_id='" + KeyId + "'");
                string level = "1";
                if (ddl_level.SelectedValue != "")
                {
                    level = ddl_level.SelectedValue;
                }

                for (int i = 0; i < BranchGridLookup.GridView.GetSelectedFieldValues("branch_id").Count; i++)
                {
                    ComponentBranch += "," + Convert.ToString(BranchGridLookup.GridView.GetSelectedFieldValues("branch_id")[i]);
                }
                BranchId = ComponentBranch.TrimStart(',');
                objEngine.SetFieldValue("tbl_master_building", "bui_Name='" + TxtBuilding.Text + "',bui_contactId='" + DDLCareTaker.SelectedItem.Value + "',bui_address1='" + TxtAdd1.Text + "',bui_address2='" + TxtAdd2.Text + "',bui_address3='" + TxtAdd3.Text + "',bui_landmark='" + TxtLand.Text + "',bui_country='" + Convert.ToString(txtCountry_hidden.Value) + "',bui_state='" + Convert.ToString(txtState_hidden.Value) + "',bui_city='" + Convert.ToString(txtCity_hidden.Value) + "',bui_pin='" + Convert.ToString(HdPin.Value) + "',LastModifyDate='" + Createdate + "',LastModifyUser='" + Session["userid"].ToString() + "',bui_levelid='" + level + "',bui_ParentId='" + hdnWarehouse.Value + "',bui_code='" + txtCode.Text + "'", " bui_id='" + KeyId + "'");
                if (BranchId != "" && BranchId != null)
                {
                    SaveBranchDetails(KeyId, BranchId);
                }
                string popupscript = "";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "adfaaa", "parent.editwin.close();", true);
                //popupscript = "<script language='javascript'>window.opener.location.href = window.opener.location.href;window.close();</script>";
                //ClientScript.RegisterStartupScript(GetType(), "JScript", popupscript);
                //Page.ClientScript.RegisterStartupScript(GetType(), "pagecall1", "<script>parent.editwin.close();</script>");
                Response.Redirect("RootBuilding.aspx");


            }
        }
        public int SaveWarehouseDetails(string Action, string bui_Name, string bui_contactId, string bui_address1, string bui_address2, string bui_address3, string bui_landmark, string bui_country, string bui_state, string bui_city, string bui_pin, DateTime Createdate, string CreateUser, string bui_BranchId, string bui_LevelId, Int64 bui_ParentId, string KeyId,string WarehouseCode)
        {
            DataSet dsInst = new DataSet();
            //    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            SqlCommand cmd = new SqlCommand("Prc_InsertWarehouse", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", Action);
            cmd.Parameters.AddWithValue("@bui_Name", bui_Name);
            cmd.Parameters.AddWithValue("@bui_contactId", bui_contactId);
            cmd.Parameters.AddWithValue("@bui_address1", bui_address1);
            cmd.Parameters.AddWithValue("bui_address2", bui_address2);
            cmd.Parameters.AddWithValue("@bui_address3", bui_address3);
            cmd.Parameters.AddWithValue("@bui_landmark", bui_landmark);
            if (bui_country != "" && bui_country != null)
            {
                cmd.Parameters.AddWithValue("@bui_country", bui_country);
            }
            if (bui_state != "" && bui_state != null)
            {
                cmd.Parameters.AddWithValue("@bui_state", bui_state);
            }
            cmd.Parameters.AddWithValue("@bui_city", bui_city);
            cmd.Parameters.AddWithValue("@bui_pin", bui_pin);
            cmd.Parameters.AddWithValue("@Createdate", Createdate);
            cmd.Parameters.AddWithValue("@CreateUser", Convert.ToInt64(CreateUser));
            if (bui_BranchId != "" && bui_BranchId != "")
            {
                //cmd.Parameters.AddWithValue("@bui_BranchId", bui_BranchId); @
                cmd.Parameters.AddWithValue("@BranchmapList", bui_BranchId);
            }
            cmd.Parameters.AddWithValue("@bui_LevelId", Convert.ToInt64(bui_LevelId));
            if (Convert.ToString(bui_ParentId) !="" && bui_ParentId != null)
            {
                cmd.Parameters.AddWithValue("@bui_ParentId", bui_ParentId);
            }
            cmd.Parameters.AddWithValue("@Bui_Id", KeyId);
            cmd.Parameters.AddWithValue("@bui_WarehouseCode", WarehouseCode);

            
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;         

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
            return idFromString;
        }

        public int SaveBranchDetails(string KeyId,string BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_InsertWarehouse");
            proc.AddVarcharPara("@Action",200,"UpdateBranchWarehousewise");
            proc.AddVarcharPara("@BranchmapList", 500, BranchId);
            proc.AddVarcharPara("@Bui_Id", 200, KeyId);
            proc.RunActionQuery();

            return 1; 
        }
        public void ShowData()
        {
            string KeyId = Request.QueryString["id"].ToString();
            if (KeyId != "ADD")
            {
                Label12.Text = "Update";
                Session["KeyId"] = KeyId.ToString();
                string[,] show = objEngine.GetFieldValue("tbl_master_building", "bui_Name,bui_contactId,bui_address1,bui_address2,bui_address3,bui_landmark,bui_country,bui_state,bui_city,bui_pin,isnull(bui_BranchId,0) bui_BranchId,isnull(bui_parentid,0) bui_parentid,isnull(bui_levelid,0) bui_levelid,isnull(bui_code,'')bui_code", " bui_id=" + KeyId, 14);
                if (show[0, 0] != "n")
                {
                    //TxtCity.Visible = true;
                    //TxtState.Visible = true;
                    TxtBuilding.Text = show[0, 0];
                    string CId = show[0, 1];
                    string[,] CId1 = objEngine.GetFieldValue("tbl_master_contact", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name", " cnt_internalId='" + CId + "'", 1);
                    DDLCareTaker.SelectedItem.Text = CId1[0, 0];
                    TxtAdd1.Text = show[0, 2];
                    TxtAdd2.Text = show[0, 3];
                    TxtAdd3.Text = show[0, 4];
                    TxtLand.Text = show[0, 5];

                    string Country = show[0, 6];
                    // DDLCountry.SelectedValue = show[0, 6];
                    txtCountry_hidden.Value = show[0, 6];
                    //string[,] Country1 = objEngine.GetFieldValue("tbl_master_country", "cou_country", " cou_id=" + Country, 1);
                    //DDLCountry.SelectedItem.Text = Country1[0, 0];
                    //string State = show[0, 7];
                    //string[,] State1 = objEngine.GetFieldValue("tbl_master_state", "state", " id=" + State, 1);
                    //TxtState.Text = State1[0, 0];
                    //string City = show[0, 8];
                    //string[,] City1 = objEngine.GetFieldValue("tbl_master_city", "city_name", " city_id=" + City, 1);
                    //TxtCity.Text = City1[0, 0];
                    //TxtPin.Text = show[0, 9];

                    //BindState();
                    //    DDLState.SelectedValue = show[0, 7];
                    txtState_hidden.Value = show[0, 7];

                    

                    //BindCity();
                    // DDLCity.SelectedValue = show[0, 8];
                    txtCity_hidden.Value = show[0, 8];
                    //string State = show[0, 7];
                    //string[,] State1 = objEngine.GetFieldValue("tbl_master_state", "state", " id=" + State, 1);
                    //TxtState.Text = State1[0, 0];
                    //string City = show[0, 8];
                    //string[,] City1 = objEngine.GetFieldValue("tbl_master_city", "city_name", " city_id=" + City, 1);
                    //TxtCity.Text = City1[0, 0];
                    //TxtPin.Text = show[0, 9];
                    HdPin.Value = show[0, 9];


                    if (show[0, 11] != "0")
                    {
                        hdnWarehouse.Value = show[0, 11];
                    }
                    if (show[0, 12] != "0")
                    {
                        ddl_level.SelectedValue = show[0, 12];
                        hdnLevel.Value = show[0, 12];
                    }
                    txtCode.Text = show[0, 13];

                    BranchdataSource.SelectCommand = @"select br.branch_id,branch_code,branch_description from tbl_master_branch br";
                    BranchGridLookup.DataBind();

                    DataTable dtBranch = oDBEngine.GetDataTable("select Branch_id from  Master_Warehouse_Branchmap where Bui_id='" + Convert.ToInt32(Session["KeyId"]) + "'");
                    if(dtBranch !=null && dtBranch.Rows.Count>0)
                    {
                        for (int i = 0; i < dtBranch.Rows.Count; i++)
                        {

                            BranchGridLookup.GridView.Selection.SelectRowByKey(dtBranch.Rows[i]["branch_id"]);

                        }
                    }

                    //if (show[0, 10] != null)
                    //{
                    //    int branchindex = 0;
                    //    int cnt = 0;
                    //    foreach (ListItem li in ddl_Branch.Items)
                    //    {
                    //        if (li.Value == show[0, 10])
                    //        {
                    //            cnt = 1;
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            branchindex += 1;
                    //        }
                    //    }
                    //    if (cnt == 1)
                    //    {
                    //        ddl_Branch.SelectedIndex = branchindex;
                    //    }
                    //    else
                    //    {
                    //        ddl_Branch.SelectedIndex = cnt;
                    //    }
                    //}

                }
            }
            else
            {
                Label12.Text = "Add";
                BtnAdd.Enabled = false;
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
        public void PopulateBranchByBranchHierarchy()
        {
            string userbranch = "";
            DataTable dt = new DataTable();
            if (Session["userbranchHierarchy"] != null)
            {
                userbranch = Convert.ToString(Session["userbranchHierarchy"]);
            }
            dt = objWarehouseConfigMaster.PopulateBranchByBranchHierarchy(userbranch);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    ddl_Branch.DataTextField = "branch_description";
            //    ddl_Branch.DataValueField = "branch_id";
            //    ddl_Branch.DataSource = dt;
            //    ddl_Branch.DataBind();
            //    ddl_Branch.Items.Insert(0, new ListItem("--ALL--", "0"));
            //    //ddl_Branch.Items.Insert(0, new ListItem("Select", "0"));
            //}
        }

        protected void ddl_level_SelectedIndexChanged(object sender, EventArgs e)
        {
            string level = ddl_level.SelectedValue;

            ProcedureExecute proc = new ProcedureExecute("PRC_WAREHOUSEPARENT");
            proc.AddVarcharPara("@LEVEL_ID", 10, level);
            DataTable dt = proc.GetTable();

            ddl_ParentWarehouse.DataSource = dt;
            ddl_ParentWarehouse.DataValueField = "bui_id";
            ddl_ParentWarehouse.DataTextField = "CategoryArrange";
            ddl_ParentWarehouse.DataBind();


        }

        [WebMethod]
        public static object GetWarehouse(string ddl_level)
        {
            string level = ddl_level;

            ProcedureExecute proc = new ProcedureExecute("PRC_WAREHOUSEPARENT");
            proc.AddVarcharPara("@LEVEL_ID", 10, level);
            DataTable dt = proc.GetTable();

            List<Warehouse> listWarehouse = new List<Warehouse>();

            listWarehouse = (from DataRow dr in dt.Rows
                             select new Warehouse()
                            {
                                id = dr["bui_id"].ToString(),
                                Name = dr["CategoryArrange"].ToString()

                            }).ToList();

            //ddl_ParentWarehouse.DataSource = dt;
            //ddl_ParentWarehouse.DataValueField = "bui_id";
            //ddl_ParentWarehouse.DataTextField = "CategoryArrange";
            //ddl_ParentWarehouse.DataBind();
            return listWarehouse;

        }
        public class Warehouse
        {
            public string Name { get; set; }
            public string id { get; set; }
        }

        [WebMethod]
        public static bool CheckUniqueName(string ProductName, string WHID)
        {
            DataTable dt = new DataTable();
            ProductName = ProductName.Replace("'", "''");
            bool IsPresent = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (WHID == "0")
            {
                dt = oGeneric.GetDataTable("SELECT COUNT(bui_code) AS bui_code FROM tbl_master_building WHERE bui_code = '" + ProductName + "' ");
            }
            else
            {
                dt = oGeneric.GetDataTable("SELECT COUNT(bui_code) AS bui_code FROM tbl_master_building WHERE bui_code = '" + ProductName + "' and bui_id<>" + WHID + "");
            }
          
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["bui_code"]) > 0)
                {
                    IsPresent = true;
                }
            }
            return IsPresent;
        }       
    }
}