using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_Address_Book : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        //string whereClause = "";
        //string userLeadId = "";
        DataTable dt = new DataTable();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        Utilities obj = new Utilities();
        string data;
        protected void Page_Init(object sender, EventArgs e)
        {
            SDSAddMaster.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SDSAddDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SDSState.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SDSCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SDSArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            Page.ClientScript.RegisterStartupScript(GetType(), "height1", "<script>SearchOpt();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "height2", "<script>SearchOpt1();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            btnSave.Attributes.Add("OnClick", "Javascript:return ValidatePage();");
            txtClientID1.Attributes.Add("onkeyup", "abcd(this,'lgjh',event)");

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //GridBind();

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            if (!IsPostBack)
            {
                Fncountry();
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0].ToString().Trim() == "Clients")
                {
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {

                        str = "'" + AcVal[0] + "'";
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {

                        str += ",'" + AcVal[0] + "'";
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                }

                else
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += "," + val[0];
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }

            }
            data = idlist[0] + "~" + str;


        }
        public void BindGroup()
        {
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlGroup.DataSource = DtGroup;
                ddlGroup.DataTextField = "gpm_Type";
                ddlGroup.DataValueField = "gpm_Type";
                ddlGroup.DataBind();
                DtGroup.Dispose();

            }

        }
        public void Fncountry()
        {
            ddlcountry.Items.Clear();
            DataTable Dtcountry = oDBEngine.GetDataTable("tbl_master_contacttype", "distinct cnttpy_contacttype,cnttpy_id", null, "cnttpy_id");
            if (Dtcountry.Rows.Count > 0)
            {
                ddlcountry.DataSource = Dtcountry;
                ddlcountry.DataTextField = "cnttpy_contacttype";
                ddlcountry.DataValueField = "cnttpy_id";
                ddlcountry.DataBind();
                ddlcountry.Items.Insert(0, new ListItem("--Select--", "0"));
                Dtcountry.Dispose();

            }

        }
        protected void BtnGroup_Click(object sender, EventArgs e)
        {
            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }
        public void Procedure()
        {
            DataSet ds = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[Addressbook]";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@SearchOnly", cmbDuplicate.SelectedItem.Value);
            //    cmd.Parameters.AddWithValue("@Param", txtClientID.Text.ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Param1", txtClientID1_hidden.Text.ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Addtype", cmbadd.Text.ToString().Trim());

            //if (cmbDuplicate.SelectedItem.Value == "Print")
            //{

            //    if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Branch")/////group type branch selection
            //    {
            //        if (Rdbranchselected.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@GRPTYPE", "BRANCH");

            //            if (RadioBtnOtherGroupByAll.Checked)
            //            {

            //                cmd.Parameters.AddWithValue("@Groupby", "ALL");
            //            }
            //            else
            //            {
            //                cmd.Parameters.AddWithValue("@Groupby", HiddenField_Branch.Value.ToString().Trim());
            //                cmd.Parameters.AddWithValue("@CLIENTS", "ALL");
            //            }


            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
            //            if (RadioBtnOtherGroupBySelected.Checked == true)
            //            {
            //                cmd.Parameters.AddWithValue("@Groupby", HiddenField_Branch.Value.ToString().Trim());
            //            }
            //            else
            //            {
            //                cmd.Parameters.AddWithValue("@Groupby", "All");
            //            }
            //            cmd.Parameters.AddWithValue("@CLIENTS", "ALL");
            //        }
            //    }


            //    else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")/////group type group selection
            //    {
            //        cmd.Parameters.AddWithValue("@GRPTYPE", ddlGroup.SelectedItem.Text.ToString().Trim());
            //        if (RadioBtnGroupAll.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@Groupby", "All");
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@Groupby", HiddenField_Group.Value.ToString().Trim());
            //        }
            //        cmd.Parameters.AddWithValue("@CLIENTS", "All");
            //    }
            //    else                                /////group type client selection
            //    {
            //        cmd.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
            //        cmd.Parameters.AddWithValue("@Groupby", "All");
            //        if (ddlGroupBy.SelectedItem.Value.ToString().Trim() != "Clients")
            //        {
            //            cmd.Parameters.AddWithValue("@CLIENTS", "ALL");
            //        }
            //    }

            //    if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Clients")/////group type client selection
            //    {
            //        if (RadioBtnOtherGroupByAll.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@CLIENTS", "All");
            //        }
            //        else if (RadioBtnOtherGroupBySelected.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@CLIENTS", HiddenField_Client.Value.ToString().Trim());
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@CLIENTS", "All");
            //        }
            //    }

            //    cmd.Parameters.AddWithValue("@Clienttype", ddlcountry.SelectedItem.Text);
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@CLIENTS", "ALL");
            //    cmd.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
            //    cmd.Parameters.AddWithValue("@Groupby", "ALL");
            //    cmd.Parameters.AddWithValue("@Clienttype", "");

            //}
            //cmd.CommandTimeout = 0;
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cmd;
            //da.Fill(ds);
            //Session["DatasetMain"] = ds;
            //GridBind();

            //}

            string GRPTYPE = "";
            string Groupby = "";
            string CLIENTS = "";
            string Clienttype = "";

            if (cmbDuplicate.SelectedItem.Value == "Print")
            {

                if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Branch")/////group type branch selection
                {
                    if (Rdbranchselected.Checked)
                    {
                        GRPTYPE = "BRANCH";

                        if (RadioBtnOtherGroupByAll.Checked)
                        {

                            Groupby = "ALL";
                        }
                        else
                        {
                            Groupby = HiddenField_Branch.Value.ToString().Trim();
                            CLIENTS = "ALL";
                        }


                    }
                    else
                    {
                        GRPTYPE = "BRANCH";
                        if (RadioBtnOtherGroupBySelected.Checked == true)
                        {
                            Groupby = HiddenField_Branch.Value.ToString().Trim();
                        }
                        else
                        {
                            Groupby = "All";
                        }
                        CLIENTS = "ALL";
                    }
                }


                else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")/////group type group selection
                {
                    GRPTYPE = ddlGroup.SelectedItem.Text.ToString().Trim();
                    if (RadioBtnGroupAll.Checked)
                    {
                        Groupby = "All";
                    }
                    else
                    {
                        Groupby = HiddenField_Group.Value.ToString().Trim();
                    }
                    CLIENTS = "All";
                }
                else                                /////group type client selection
                {
                    GRPTYPE = "BRANCH";
                    Groupby = "All";
                    if (ddlGroupBy.SelectedItem.Value.ToString().Trim() != "Clients")
                    {
                        CLIENTS = "ALL";
                    }
                }

                if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Clients")/////group type client selection
                {
                    if (RadioBtnOtherGroupByAll.Checked)
                    {
                        CLIENTS = "All";
                    }
                    else if (RadioBtnOtherGroupBySelected.Checked)
                    {
                        CLIENTS = HiddenField_Client.Value.ToString().Trim();
                    }
                    else
                    {
                        CLIENTS = "All";
                    }
                }

                Clienttype = ddlcountry.SelectedItem.Text.ToString();
            }
            else
            {
                CLIENTS = "ALL";
                GRPTYPE = "BRANCH";
                Groupby = "ALL";
                Clienttype = "";

            }
            ds = obj.Addressbook(cmbDuplicate.SelectedItem.Value, txtClientID.Text.ToString().Trim(), txtClientID1_hidden.Text.ToString().Trim(),
                cmbadd.Text.ToString().Trim(), GRPTYPE, Groupby, CLIENTS, Clienttype);
            Session["DatasetMain"] = ds;
            GridBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Procedure();
            exporter.WriteXlsToResponse();
        }
        protected void GridBind()
        {
            if (Session["DatasetMain"] != null)
            {
                DataSet dsNew = (DataSet)Session["DatasetMain"];
                gridContract.DataSource = dsNew.Tables[0];
                gridContract.DataBind();
            }
        }
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
        //    switch (Filter)
        //    {
        //        case 1:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;

        //    }

        //}
        protected void gridContract_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.ToString() == "s")
            {
                gridContract.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters.ToString() == "All")
            {
                gridContract.FilterExpression = string.Empty;
            }

            GridBind();
        }
        //    protected void cmbDuplicate_SelectedIndexChanged(object sender, EventArgs e)
        //    {
        //        if (cmbDuplicate.SelectedItem.Value == "Print")
        //        {
        //            Fncountry();
        //    {

        //        DataTable DtDosPrint = oDBEngine.GetDataTable("tbl_master_country", "distinct cou_country,cou_id", null);
        //        if (DtDosPrint.Rows.Count > 0)
        //        {
        //            ddlcountry.DataSource = DtDosPrint;
        //            ddlcountry.DataTextField = "cou_country";
        //            ddlcountry.DataValueField = "cou_id";
        //            ddlcountry.DataBind();

        //            DtDosPrint.Dispose();

        //        }
        //    }
        //}

        //    }
        //protected void btn_print_Click(object sender, EventArgs e)
        //{
        //    // btn_print.Attributes["onclick"] = "javascript:CallPrint('gridContract');";

        //}

        //protected void ddlcountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Fnstate(ddlcountry.SelectedItem.Value);
        //}
        //protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Fncity(ddlstate.SelectedItem.Value);
        //}

        //protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Fnarea(ddlstate.SelectedItem.Value);
        //}

        //protected void FillCityCombo(int state)
        //{
        //    string[,] cities = GetCities(state);
        //    cmbCity.Items.Clear();
        //    cmbCity.Items.Add("All", 0);
        //    for (int i = 0; i < cities.GetLength(0); i++)
        //    {
        //        cmbCity.Items.Add(cities[i, 1], cities[i, 0]);
        //    }

        //}
        //string[,] GetCities(int state)
        //{
        //    SDSCity.SelectParameters[0].DefaultValue = state.ToString();
        //    DataView view = (DataView)SDSCity.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = view[i][0].ToString();
        //        DATA[i, 1] = view[i][1].ToString();
        //    }
        //    return DATA;
        //}


        //protected void cmbArea_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    FillAreaCombo(Convert.ToInt32(e.Parameter));
        //}
        //protected void FillAreaCombo(int city)
        //{
        //    string[,] areas = GetAreas(city);
        //    cmbArea.Items.Clear();
        //    cmbArea.Items.Add("All", 0);
        //    for (int i = 0; i < areas.GetLength(0); i++)
        //    {
        //        cmbArea.Items.Add(areas[i, 1], areas[i, 0]);
        //    }
        //}
        //string[,] GetAreas(int city)
        //{
        //    SDSArea.SelectParameters[0].DefaultValue = city.ToString();
        //    DataView view = (DataView)SDSArea.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = view[i][0].ToString();
        //        DATA[i, 1] = view[i][1].ToString();
        //    }
        //    return DATA;
        //}
        ////protected void ASPx_AddDetails_BeforePerformDataSelect(object sender, EventArgs e)
        ////{
        ////    Session["cnt_internalId"] = (sender as ASPxGridView).GetMasterRowKeyValue();
        ////    userLeadId = Session["cnt_internalId"].ToString();
        ////}
        ////protected void ASPx_AddDetails_Load(object sender, EventArgs e)
        ////{
        ////    ASPxGridView ASPx_AddDetails = (ASPxGridView)sender;
        ////    userLeadId = ((DevExpress.Web.GridViewDetailRowTemplateContainer)(ASPx_AddDetails.Parent)).KeyValue.ToString();
        ////    SDSAddDetails.SelectCommand = " select add_id, add_cntId, add_entity, add_addressType, add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_area, cou_country, state, city_name, area_name, add_pin, add_activityId, CreateDate, CreateUser, LastModifyDate, LastModifyUser, cnt_internalId, Pname, contacttype, branch_description,phome,pmob " +
        ////                                    " from View_AddressReport " +
        ////                                    " where add_cntId = '" + userLeadId + "'";
        ////    ASPx_AddDetails.DataBind();
        ////}
        //protected void populateCombo()
        //{
        //    int rowNo;

        //    //State
        //    SDSState.SelectCommand = "SELECT [id], [state] FROM [tbl_master_state] ORDER BY [state]";
        //    cmbState.DataBind();

        //    //BindBranch Details
        //    dt = null;
        //    dt = oDBEngine.GetDataTable("tbl_master_branch", "branch_internalId,branch_id,branch_description", null);
        //    rowNo = dt.Rows.Count;
        //    cmbBranch.Items.Add("All", 0);
        //    for (int i = 0; i < rowNo; i++)
        //    {
        //        cmbBranch.Items.Add(dt.Rows[i]["branch_description"].ToString(), dt.Rows[i]["branch_id"].ToString());
        //    }
        //    //BindContact Type Details
        //    dt = null;
        //    dt = oDBEngine.GetDataTable("tbl_master_contactType", "cnttpy_id,cnttpy_contactType", null);
        //    rowNo = dt.Rows.Count;
        //    cmbContact.Items.Add("All", 0);
        //    for (int i = 0; i < rowNo; i++)
        //    {
        //        cmbContact.Items.Add(dt.Rows[i]["cnttpy_contactType"].ToString(), dt.Rows[i]["cnttpy_id"].ToString());
        //    }
        //    dt = null;
        //    dt = oDBEngine.GetDataTable("tbl_master_contact", "cnt_firstname,cnt_internalid", null, "cnt_firstname");
        //    rowNo = dt.Rows.Count;
        //    cmbclient.Items.Add("All", 0);
        //    for (int i = 0; i < rowNo; i++)
        //    {
        //        cmbclient.Items.Add(dt.Rows[i]["cnt_firstname"].ToString(), dt.Rows[i]["cnt_internalid"].ToString());
        //    }
        //    cmbAddressType.SelectedIndex = 0;
        //    cmbContact.SelectedIndex = 0;
        //    cmbBranch.SelectedIndex = 0;
        //    cmbclient.SelectedIndex = 0;

        //}
        protected void btnPrint_Click1(object sender, EventArgs e)
        {
            Response.Redirect("../management/frmAddressPrint_popup.aspx?");
        }
    }
}

