using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_AddEditBranchGroup : ERP.OMS.ViewState_class.VSPage, System.Web.UI.ICallbackEventHandler
    {
        string data;
        static string BranchId;
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["userlastsegment"] = 9;
            //Session["userid"] = 20;

            if (!IsPostBack)
            {
                if (Request.QueryString["branchid"] != null)
                {
                    if (Convert.ToString(Request.QueryString["branchid"]) != "add")
                    {
                        GetValues(Convert.ToString(Request.QueryString["branchid"]));

                    }
                    getAllBranches();
                }
            }

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //// Page.ClientScript.RegisterStartupScript(GetType(), "callHight", "<script language='Javascript'>height();</script>");

        }
        public void getAllBranches()
        {
            DataTable dtbranches=new DataTable();
            if (Convert.ToString(Request.QueryString["branchid"]) != "add")
            {
                 dtbranches = oDBEngine.GetDataTable("tbl_master_branch b", "rtrim(ltrim(isnull(b.branch_description,''))) + ' -[' + rtrim(ltrim(isnull(branch_code,''))) + ']' as Branch,b.branch_id", "(branch_description Like ('%%') or branch_code Like ('%%')) and branch_id not in (select BranchGroupMembers_BranchID from trans_branchgroupmembers where BranchGroupMembers_BranchGroupID not in(select BranchGroups_ID from master_branchgroups where BranchGroups_Name='" + txtName.Text.Trim() + "' and BranchGroups_Code='" + txtCode.Text.Trim() + "'))");
                 DataTable dtbranchesselected = oDBEngine.GetDataTable("trans_branchgroupmembers bg inner join tbl_master_branch b on bg.branchgroupmembers_branchid=b.branch_id", "rtrim(ltrim(isnull(b.branch_description,''))) + ' -[' + rtrim(ltrim(isnull(branch_code,''))) + ']',b.branch_id", "branchgroupmembers_branchgroupid=" + Convert.ToString(Request.QueryString["branchid"]));

                 lstBranches.DataSource = dtbranches;
                 lstBranches.DataTextField = "Branch";
                 lstBranches.DataValueField = "branch_id";
                 lstBranches.DataBind();

                 for (int i = 0; i < dtbranchesselected.Rows.Count; i++)
                 {
                     //foreach (var item in lstBranches.Items)
                     //{
                     //    if (dtbranchesselected.Rows[i]["Branch"].ToString() == item.ToString())
                     //    {

                     //        //lstBranches.Items.FindByValue(item).Selected = true;
                     //        lstBranches.Items.FindByValue(item).Selected = true;
                     //    }
                     //}

                     for (int j = lstBranches.Items.Count - 1; j >= 0; j--)
                     {
                         if (Convert.ToString(dtbranchesselected.Rows[i]["branch_id"]) == lstBranches.Items[j].Value)
                         {

                             //lstBranches.Items.FindByValue(j).Selected = true;
                             // lstBranches.Items.FindByValue(item).Selected = true;
                             lstBranches.Items[j].Selected = true;
                         }
                     }

                 }
            }
            else
            {
                 dtbranches = oDBEngine.GetDataTable("tbl_master_branch b", "rtrim(ltrim(isnull(b.branch_description,''))) + ' -[' + rtrim(ltrim(isnull(branch_code,''))) + ']' as Branch,b.branch_id", "(branch_description Like ('%%') or branch_code Like ('%%')) and branch_id not in (select BranchGroupMembers_BranchID from trans_branchgroupmembers)");
                 lstBranches.DataSource = dtbranches;
                 lstBranches.DataTextField = "Branch";
                 lstBranches.DataValueField = "branch_id";
                 lstBranches.DataBind();
            }
           

        }
        protected void GetValues(string bgid)
        {
            DataTable dt = oDBEngine.GetDataTable("master_branchgroups", "BranchGroups_Name,BranchGroups_Code", "BranchGroups_ID=" + bgid);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = Convert.ToString(dt.Rows[0][0]);
                    txtCode.Text = Convert.ToString(dt.Rows[0][1]);

                }

            }


            DataTable dt1 = oDBEngine.GetDataTable("trans_branchgroupmembers bg inner join tbl_master_branch b on bg.branchgroupmembers_branchid=b.branch_id", "rtrim(ltrim(isnull(b.branch_description,''))) + ' -[' + rtrim(ltrim(isnull(branch_code,''))) + ']',b.branch_id", "branchgroupmembers_branchgroupid=" + bgid);
            string branchids = "";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {

                //Page.ClientScript.RegisterStartupScript(GetType(), "editbranchjs" + i.ToString(), "<script>EditBranch('" + Convert.ToString(dt1.Rows[i][0]) + "','" + Convert.ToString(dt1.Rows[i][1]) + "')</script>");
                if (branchids == "")
                    branchids = Convert.ToString(dt1.Rows[i][1]);
                else
                    branchids = branchids + "," + Convert.ToString(dt1.Rows[i][1]);
            }
            Session["KeyVal"] = branchids;


            //lstBranches.DataSource = dt1;
            //lstBranches.DataTextField = "branch_description";
            //lstBranches.DataValueField = "branch_id";
            //lstBranches.DataBind();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string[] InputName = new string[8];
            string[] InputType = new string[8];
            string[] InputValue = new string[8];
            string doneornot = "";
            //SM---------17.11.2016----------For Checking whether short name and group name already exists or not!-------------------------
            int Isduplicacy = 0;
          

            if (Convert.ToString(Request.QueryString["branchid"]) == "add")
            {
                DataTable dtcheckname = oDBEngine.GetDataTable("master_branchgroups", "BranchGroups_Name", "BranchGroups_Name='" + txtName.Text.Trim() + "'");

                if (dtcheckname.Rows.Count > 0)
                {
                    Isduplicacy = 1;
                }
                DataTable dtcheckcode = oDBEngine.GetDataTable("master_branchgroups", "BranchGroups_Code", "BranchGroups_Code='" + txtCode.Text.Trim() + "'");
                if (dtcheckcode.Rows.Count > 0)
                {
                    Isduplicacy = 1;
                }

                //if (Session["KeyVal"] != null)
                //{
                //string aa = txtBranch_hidden.Value;
                Int32 insertedrows = 0;
                int a = lstBranches.Items.Count;
                string paraOut = "";
               
                for (int i = lstBranches.Items.Count - 1; i >= 0; i--)
                {
                    if (lstBranches.Items[i].Selected == true)
                    {
                        //ListItem li = lstBranches.Items[i].Value();

                        InputName[0] = "BranchGroupName";
                        InputName[1] = "BranchGroupCode";
                        InputName[2] = "branchIds";
                        InputName[3] = "CreateUser";
                        InputName[4] = "Mode";
                        InputName[7] = "IsDuplicacy";

                        InputType[0] = "V";
                        InputType[1] = "V";
                        InputType[2] = "V";
                        InputType[3] = "I";
                        InputType[4] = "V";
                        InputType[7] = "I";

                        InputValue[0] = txtName.Text.Trim();
                        InputValue[1] = txtCode.Text.Trim();
                        //InputValue[2] = Convert.ToString(Session["KeyVal"]);
                        InputValue[2] = lstBranches.Items[i].Value;
                        InputValue[3] = Convert.ToString(Session["userid"]);
                        InputValue[4] = "add";
                        InputValue[7] = Convert.ToString(Isduplicacy);

                        try
                        {
                            //DataTable dt= SQLProcedures.InsertProcedureArr("Insert_BranchGroups", InputName, "paraOut", InputType, "V", InputValue, "");
                            DataTable dt = BusinessLogicLayer.SQLProcedures.InsertProcedureArr("Insert_BranchGroups", InputName, "paraOut", InputType, "V", InputValue, "");
                            if (dt != null)
                            {
                                if (Convert.ToString(dt.Rows[0][0]) == "bgname")
                                {
                                    doneornot = "bgname";
                                }
                                else if (Convert.ToString(dt.Rows[0][0]) == "bgcode")
                                {
                                    doneornot = "bgcode";
                                }
                                else if (Convert.ToString(dt.Rows[0][0]) == "done")
                                {
                                    doneornot = "done";
                                }
                            }

                        }

                        catch (Exception ex)
                        {
                        }

                        //using (SqlConnection objCon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                        //{
                        //    using (SqlCommand objCmd = new SqlCommand("Insert_BranchGroups", objCon))
                        //    {
                        //        objCmd.CommandType = CommandType.StoredProcedure;
                        //        objCmd.Parameters.AddWithValue("@BranchGroupName", txtName.Text.Trim());
                        //        objCmd.Parameters.AddWithValue("@BranchGroupCode", txtCode.Text.Trim());
                        //        objCmd.Parameters.AddWithValue("@branchIds", Convert.ToString(Session["KeyVal"]));
                        //        objCmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(Session["userid"]));
                        //        objCmd.Parameters.AddWithValue("@Mode", "add");
                        //        objCon.Open();
                        //        insertedrows = objCmd.ExecuteNonQuery();
                        //        if (insertedrows > 0)
                        //        {
                        //            Page.ClientScript.RegisterStartupScript(GetType(), "confmsg", "<script language='javascript'>alert('Branch Group Updated Successfully !')</script>");
                        //            Session["KeyVal"] = null;
                        //            txtName.Text = "";
                        //            txtCode.Text = "";
                        //        }
                        //    }
                        //}



                    }
                }



                //}
                ////else
                ////Page.ClientScript.RegisterStartupScript(GetType(), "alertbranch", "<script language='javascript'>alert('Please Select Branch !')</script>");
                if (doneornot == "bgname")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "bnamecheckjs", "<script>alert('BranchGroup Name Already Exists !')</script>");
                }
                else if (doneornot == "bgcode")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "bcodecheckjs", "<script>alert('BranchGroup Code Already Exists !')</script>");
                }
                else if (doneornot == "done")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "confmsg", "<script language='javascript'>alert('Branch Group Added Successfully !')</script>");
                    Session["KeyVal"] = null;
                    txtName.Text = "";
                    txtCode.Text = "";
                    //Page.ClientScript.RegisterStartupScript(GetType(), "closejs", "<script language='javascript'>ClosePage()</script>");
                    Response.Redirect("frm_BranchGroups.aspx");
                }
            }
            else
            {
                //if (Session["KeyVal"] != null)
                //{
                    //string aa = txtBranch_hidden.Value;
                    Int32 insertedrows = 0;
                    int a = lstBranches.Items.Count;
                    string paraOut = "";
                    string branchid = "";
                    for (int i = lstBranches.Items.Count - 1; i >= 0; i--)
                    {

                        if (lstBranches.Items[i].Selected == true)
                        {
                           
                            if (branchid == "")
                            {
                                branchid = lstBranches.Items[i].Value;
                            }
                            else
                            {
                                branchid = branchid + "," + lstBranches.Items[i].Value;
                            }
                        }
                    }
                            InputName[0] = "BranchGroupName";
                            InputName[1] = "BranchGroupCode";
                            InputName[2] = "branchIds";
                            InputName[3] = "CreateUser";
                            InputName[4] = "Mode";
                            InputName[5] = "BranchGrId";

                            InputType[0] = "V";
                            InputType[1] = "V";
                            InputType[2] = "V";
                            InputType[3] = "I";
                            InputType[4] = "V";
                            InputType[5] = "I";
                         
                            InputValue[0] = txtName.Text.Trim();
                            InputValue[1] = txtCode.Text.Trim();
                            InputValue[2] = branchid;
                            InputValue[3] = Convert.ToString(Session["userid"]);
                            InputValue[4] = "edit";
                            InputValue[5] = Convert.ToString(Request.QueryString["branchid"]);

                            try
                            {
                                //DataTable dt = SQLProcedures.InsertProcedureArr("Insert_BranchGroups", InputName, "paraOut", InputType, "V", InputValue, "");
                                DataTable dt = BusinessLogicLayer.SQLProcedures.InsertProcedureArr("Insert_BranchGroups", InputName, "paraOut", InputType, "V", InputValue, "");

                                if (dt != null)
                                {
                                    if (Convert.ToString(dt.Rows[0][0]) == "bgname")
                                    {
                                        doneornot = "bgname";
                                        Page.ClientScript.RegisterStartupScript(GetType(), "bnamecheckjs", "<script>alert('BranchGroup Name Already Exists !')</script>");

                                    }
                                    else if (Convert.ToString(dt.Rows[0][0]) == "bgcode")
                                    {
                                        doneornot = "bgcode";
                                        Page.ClientScript.RegisterStartupScript(GetType(), "bcodecheckjs", "<script>alert('BranchGroup Code Already Exists !')</script>");

                                    }
                                    else if (Convert.ToString(dt.Rows[0][0]) == "done")
                                    {
                                        doneornot = "done";
                                        Page.ClientScript.RegisterStartupScript(GetType(), "confmsg", "<script language='javascript'>alert('Branch Group Updated Successfully !')</script>");
                                        Session["KeyVal"] = null;
                                        txtName.Text = "";
                                        txtCode.Text = "";
                                        //Page.ClientScript.RegisterStartupScript(GetType(), "closejs", "<script language='javascript'>ClosePage()</script>");
                                        Response.Redirect("frm_BranchGroups.aspx");
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                            }

                      
              

                    

                //}
                //else
                //    Page.ClientScript.RegisterStartupScript(GetType(), "alertbranch", "<script language='javascript'>alert('Please Select Branch !')</script>");
                if (doneornot == "bgname")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "bnamecheckjs", "<script>alert('BranchGroup Name Already Exists !')</script>");

                }
                else if (doneornot == "bgcode")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "bcodecheckjs", "<script>alert('BranchGroup Code Already Exists !')</script>");

                }
                else if (doneornot == "done")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "confmsg", "<script language='javascript'>alert('Branch Group Updated Successfully !')</script>");
                    Session["KeyVal"] = null;
                    txtName.Text = "";
                    txtCode.Text = "";
                    //Page.ClientScript.RegisterStartupScript(GetType(), "closejs", "<script language='javascript'>ClosePage()</script>");
                    Response.Redirect("frm_BranchGroups.aspx");
                }

            }
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;

        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            //string id = eventArgument.ToString();
            //string[] idlist = id.Split('~');
            string[] cl = eventArgument.Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
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

            BranchId = str;
            Session["KeyVal"] = str;
            data = "Branch~" + str1;
            //if (idlist[0] == "Client")
            //{
            //    SubId = str;
            //    Session["KeyValSegment"] = str;
            //    data = "Client~" + str1;
            //}
            //if (idlist[0] == "Branch")
            //{
            //    BranchId = str;
            //    Session["KeyVal"] = str;
            //    data = "Branch~" + str1;
            //}


        }
    }
}