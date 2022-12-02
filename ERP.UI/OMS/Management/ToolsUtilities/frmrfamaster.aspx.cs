using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmrfamaster : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data = "";
        string[,] dpVal;
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        // DBEngine oDBEngine = new DBEngine(string.Empty);
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
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                DataTable dt = oDBEngine.GetDataTable("tbl_master_rfa", "rfa_id,(rfa_shortname) as Title", null);
                grdrfa.DataSource = dt;
                grdrfa.DataBind();
                lst_targetuser1.Attributes.Add("onchange", "callDoc(1)");
                lst_targetuser2.Attributes.Add("onchange", "callDoc(2)");
                lst_targetuser3.Attributes.Add("onchange", "callDoc(3)");
                lst_targetuser4.Attributes.Add("onchange", "callDoc(4)");
                lst_targetuser5.Attributes.Add("onchange", "callDoc(5)");
                lst_targetuser6.Attributes.Add("onchange", "callDoc(6)");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            }
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
        }

        #region ICallbackEventHandler Members

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] FieldValue = id.Split('~');
            string IDs = "";
            data = "";
            #region Combo
            if (FieldValue[0] == "Combo")
            {
                IDs = FieldValue[1];
                string listitems = "";
                string prop = "";
                if (FieldValue[1] == "ODH")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_costcenter ", " cost_id as Id, cost_description as Name ", " cost_costcentertype='Department' ", 2, " cost_description ");
                    prop = "1";
                }
                else if (FieldValue[1] == "SBH")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_internalid as Id, branch_description as Name ", null, 2, " branch_description ");
                    prop = "1";
                }
                else if (FieldValue[1] == "SU")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_user ", " user_contactid as Id, user_name +'['+ user_contactid +']' as Name ", null, 2, " user_name ");
                    prop = "1";
                }
                else
                {
                    prop = "0";
                }
                if (prop == "1")
                {
                    for (int i = 0; i < dpVal.Length / 2; i++)
                    {
                        if (listitems != "")
                            listitems += ";" + dpVal[i, 0] + "," + dpVal[i, 1];
                        else
                            listitems = dpVal[i, 0] + "," + dpVal[i, 1];
                    }
                }
                data = "Combo~" + listitems + "~" + FieldValue[2] + "~" + prop;
            }
            #endregion
            #region Save
            if (FieldValue[0] == "Save")
            {
                int NoOfRowsAffected = 0;
                int alert = 0;
                if (FieldValue[17] == "")
                {
                    NoOfRowsAffected = oDBEngine.InsurtFieldValue("tbl_master_rfa", "rfa_id,rfa_shortname,rfa_description,rfa_target1,rfa_target2,rfa_rule2,rfa_target3,rfa_rule3,rfa_target4,rfa_rule4,rfa_target5,rfa_rule5,rfa_target6,rfa_rule6,rfa_hoursallowed,rfa_totallevel,CreateDate,CreateUser", "'" + FieldValue[1] + "','" + FieldValue[2] + "','" + FieldValue[3] + "','" + FieldValue[4] + "','" + FieldValue[5] + "','" + FieldValue[6] + "','" + FieldValue[7] + "','" + FieldValue[8] + "','" + FieldValue[9] + "','" + FieldValue[10] + "','" + FieldValue[11] + "','" + FieldValue[12] + "','" + FieldValue[13] + "','" + FieldValue[14] + "','" + FieldValue[16] + "','" + FieldValue[15] + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                }
                else
                {
                    NoOfRowsAffected = oDBEngine.SetFieldValue("tbl_master_rfa", "rfa_shortname='" + FieldValue[2] + "',rfa_description='" + FieldValue[3] + "',rfa_target1='" + FieldValue[4] + "',rfa_target2='" + FieldValue[5] + "',rfa_rule2='" + FieldValue[6] + "',rfa_target3='" + FieldValue[7] + "',rfa_rule3='" + FieldValue[8] + "',rfa_target4='" + FieldValue[9] + "',rfa_rule4='" + FieldValue[10] + "',rfa_target5='" + FieldValue[11] + "',rfa_rule5='" + FieldValue[12] + "',rfa_target6='" + FieldValue[13] + "',rfa_rule6='" + FieldValue[14] + "',rfa_hoursallowed='" + FieldValue[16] + "',rfa_totallevel='" + FieldValue[15] + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rfa_id='" + FieldValue[17] + "'");
                    alert = 1;
                }
                if (NoOfRowsAffected != 0)
                {
                    data = "Save~Y" + "~" + alert;
                }
            }
            #endregion
            #region Edit
            if (FieldValue[0] == "Edit")
            {
                DataTable Rfadt = oDBEngine.GetDataTable("tbl_master_rfa", "*", " rfa_id='" + FieldValue[1] + "'");
                string shortName = Rfadt.Rows[0]["rfa_shortname"].ToString();
                string Description = Rfadt.Rows[0]["rfa_description"].ToString();
                string TempNo = Rfadt.Rows[0]["rfa_id"].ToString();
                string HoursAllowed = Rfadt.Rows[0]["rfa_hoursallowed"].ToString();
                int result;
                string listitems = "";
                string prop = "0";
                string targetUser1 = "";
                string subtargetUser1 = "";

                string listitems1 = "";
                string prop1 = "0";
                string targetUser2 = "";
                string subtargetUser2 = "";

                string listitems2 = "";
                string prop2 = "0";
                string targetUser3 = "";
                string subtargetUser3 = "";

                string listitems3 = "";
                string prop3 = "0";
                string targetUser4 = "";
                string subtargetUser4 = "";

                string listitems4 = "";
                string prop4 = "0";
                string targetUser5 = "";
                string subtargetUser5 = "";

                string listitems5 = "";
                string prop5 = "0";
                string targetUser6 = "";
                string subtargetUser6 = "";
                if (Int32.TryParse(Rfadt.Rows[0]["rfa_target1"].ToString(), out result))
                {
                    if (Convert.ToInt32(Rfadt.Rows[0]["rfa_target1"].ToString()) > 0)
                    {
                        dpVal = oDBEngine.GetFieldValue(" tbl_master_costcenter ", " cost_id as Id, cost_description as Name ", " cost_costcentertype='Department' ", 2, " cost_description ");
                        targetUser1 = "ODH";
                        subtargetUser1 = Rfadt.Rows[0]["rfa_target1"].ToString();
                        prop = "1";
                    }
                    else
                    {
                        targetUser1 = "0";
                    }
                }
                else if (Rfadt.Rows[0]["rfa_target1"].ToString().Substring(0, 2) == "BR")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_internalid as Id, branch_description as Name ", null, 2, " branch_description ");
                    targetUser1 = "SBH";
                    subtargetUser1 = Rfadt.Rows[0]["rfa_target1"].ToString();
                    prop = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target1"].ToString().Substring(0, 2) == "EM")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_user ", " user_contactid as Id, user_name +'['+ user_contactid +']' as Name ", null, 2, " user_name ");
                    targetUser1 = "SU";
                    subtargetUser1 = Rfadt.Rows[0]["rfa_target1"].ToString();
                    prop = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target1"].ToString() == "RH")
                {
                    targetUser1 = Rfadt.Rows[0]["rfa_target1"].ToString();
                    prop = "0";
                }
                else if (Rfadt.Rows[0]["rfa_target1"].ToString() == "HOD")
                {
                    targetUser1 = Rfadt.Rows[0]["rfa_target1"].ToString();
                    prop = "0";
                }
                else
                {
                    targetUser1 = Rfadt.Rows[0]["rfa_target1"].ToString();
                    prop = "0";
                }
                if (prop == "1")
                {
                    for (int i = 0; i < dpVal.Length / 2; i++)
                    {
                        if (listitems != "")
                            listitems += ";" + dpVal[i, 0] + "," + dpVal[i, 1];
                        else
                            listitems = dpVal[i, 0] + "," + dpVal[i, 1];
                    }
                }

                if (Int32.TryParse(Rfadt.Rows[0]["rfa_target2"].ToString(), out result))
                {
                    if (Convert.ToInt32(Rfadt.Rows[0]["rfa_target2"].ToString()) > 0)
                    {
                        dpVal = oDBEngine.GetFieldValue(" tbl_master_costcenter ", " cost_id as Id, cost_description as Name ", " cost_costcentertype='Department' ", 2, " cost_description ");
                        targetUser2 = "ODH";
                        subtargetUser2 = Rfadt.Rows[0]["rfa_target2"].ToString();
                        prop1 = "1";
                    }
                    else
                    {
                        targetUser2 = "0";
                    }
                }
                else if (Rfadt.Rows[0]["rfa_target2"].ToString().Substring(0, 2) == "BR")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_internalid as Id, branch_description as Name ", null, 2, " branch_description ");
                    targetUser2 = "SBH";
                    subtargetUser2 = Rfadt.Rows[0]["rfa_target1"].ToString();
                    prop1 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target2"].ToString().Substring(0, 2) == "EM")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_user ", " user_contactid as Id, user_name +'['+ user_contactid +']' as Name ", null, 2, " user_name ");
                    targetUser2 = "SU";
                    subtargetUser2 = Rfadt.Rows[0]["rfa_target2"].ToString();
                    prop1 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target2"].ToString() == "RH")
                {
                    targetUser2 = Rfadt.Rows[0]["rfa_target2"].ToString();
                    prop1 = "0";
                }
                else if (Rfadt.Rows[0]["rfa_target2"].ToString() == "HOD")
                {
                    targetUser2 = Rfadt.Rows[0]["rfa_target2"].ToString();
                    prop1 = "0";
                }
                else
                {
                    targetUser2 = Rfadt.Rows[0]["rfa_target2"].ToString();
                    prop1 = "0";
                }
                if (prop1 == "1")
                {
                    for (int i1 = 0; i1 < dpVal.Length / 2; i1++)
                    {
                        if (listitems1 != "")
                            listitems1 += ";" + dpVal[i1, 0] + "," + dpVal[i1, 1];
                        else
                            listitems1 = dpVal[i1, 0] + "," + dpVal[i1, 1];
                    }
                }

                if (Int32.TryParse(Rfadt.Rows[0]["rfa_target3"].ToString(), out result))
                {
                    if (Convert.ToInt32(Rfadt.Rows[0]["rfa_target3"].ToString()) > 0)
                    {
                        dpVal = oDBEngine.GetFieldValue(" tbl_master_costcenter ", " cost_id as Id, cost_description as Name ", " cost_costcentertype='Department' ", 2, " cost_description ");
                        targetUser3 = "ODH";
                        subtargetUser3 = Rfadt.Rows[0]["rfa_target3"].ToString();
                        prop2 = "1";
                    }
                    else
                    {
                        targetUser3 = "0";
                    }
                }
                else if (Rfadt.Rows[0]["rfa_target3"].ToString().Substring(0, 2) == "BR")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_internalid as Id, branch_description as Name ", null, 2, " branch_description ");
                    targetUser3 = "SBH";
                    subtargetUser3 = Rfadt.Rows[0]["rfa_target3"].ToString();
                    prop2 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target3"].ToString().Substring(0, 2) == "EM")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_user ", " user_contactid as Id, user_name +'['+ user_contactid +']' as Name ", null, 2, " user_name ");
                    targetUser3 = "SU";
                    subtargetUser3 = Rfadt.Rows[0]["rfa_target3"].ToString();
                    prop2 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target3"].ToString() == "RH")
                {
                    targetUser3 = Rfadt.Rows[0]["rfa_target3"].ToString();
                    prop2 = "0";
                }
                else if (Rfadt.Rows[0]["rfa_target3"].ToString() == "HOD")
                {
                    targetUser3 = Rfadt.Rows[0]["rfa_target3"].ToString();
                    prop2 = "0";
                }
                else
                {
                    targetUser3 = Rfadt.Rows[0]["rfa_target3"].ToString();
                    prop2 = "0";
                }
                if (prop2 == "1")
                {
                    for (int i2 = 0; i2 < dpVal.Length / 2; i2++)
                    {
                        if (listitems2 != "")
                            listitems2 += ";" + dpVal[i2, 0] + "," + dpVal[i2, 1];
                        else
                            listitems2 = dpVal[i2, 0] + "," + dpVal[i2, 1];
                    }
                }

                if (Int32.TryParse(Rfadt.Rows[0]["rfa_target4"].ToString(), out result))
                {
                    if (Convert.ToInt32(Rfadt.Rows[0]["rfa_target4"].ToString()) > 0)
                    {
                        dpVal = oDBEngine.GetFieldValue(" tbl_master_costcenter ", " cost_id as Id, cost_description as Name ", " cost_costcentertype='Department' ", 2, " cost_description ");
                        targetUser4 = "ODH";
                        subtargetUser4 = Rfadt.Rows[0]["rfa_target4"].ToString();
                        prop3 = "1";
                    }
                    else
                    {
                        targetUser4 = "0";
                    }
                }
                else if (Rfadt.Rows[0]["rfa_target4"].ToString().Substring(0, 2) == "BR")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_internalid as Id, branch_description as Name ", null, 2, " branch_description ");
                    targetUser4 = "SBH";
                    subtargetUser4 = Rfadt.Rows[0]["rfa_target4"].ToString();
                    prop3 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target4"].ToString().Substring(0, 2) == "EM")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_user ", " user_contactid as Id, user_name +'['+ user_contactid +']' as Name ", null, 2, " user_name ");
                    targetUser4 = "SU";
                    subtargetUser4 = Rfadt.Rows[0]["rfa_target4"].ToString();
                    prop3 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target4"].ToString() == "RH")
                {
                    targetUser4 = Rfadt.Rows[0]["rfa_target4"].ToString();
                    prop3 = "0";
                }
                else if (Rfadt.Rows[0]["rfa_target4"].ToString() == "HOD")
                {
                    targetUser4 = Rfadt.Rows[0]["rfa_target4"].ToString();
                    prop3 = "0";
                }
                else
                {
                    targetUser4 = Rfadt.Rows[0]["rfa_target4"].ToString();
                    prop3 = "0";
                }
                if (prop3 == "1")
                {
                    for (int i3 = 0; i3 < dpVal.Length / 2; i3++)
                    {
                        if (listitems3 != "")
                            listitems3 += ";" + dpVal[i3, 0] + "," + dpVal[i3, 1];
                        else
                            listitems3 = dpVal[i3, 0] + "," + dpVal[i3, 1];
                    }
                }

                if (Int32.TryParse(Rfadt.Rows[0]["rfa_target5"].ToString(), out result))
                {
                    if (Convert.ToInt32(Rfadt.Rows[0]["rfa_target5"].ToString()) > 0)
                    {
                        dpVal = oDBEngine.GetFieldValue(" tbl_master_costcenter ", " cost_id as Id, cost_description as Name ", " cost_costcentertype='Department' ", 2, " cost_description ");
                        targetUser5 = "ODH";
                        subtargetUser5 = Rfadt.Rows[0]["rfa_target5"].ToString();
                        prop4 = "1";
                    }
                    else
                    {
                        targetUser5 = "0";
                    }
                }
                else if (Rfadt.Rows[0]["rfa_target5"].ToString().Substring(0, 2) == "BR")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_internalid as Id, branch_description as Name ", null, 2, " branch_description ");
                    targetUser5 = "SBH";
                    subtargetUser5 = Rfadt.Rows[0]["rfa_target5"].ToString();
                    prop4 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target5"].ToString().Substring(0, 2) == "EM")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_user ", " user_contactid as Id, user_name +'['+ user_contactid +']' as Name ", null, 2, " user_name ");
                    targetUser5 = "SU";
                    subtargetUser5 = Rfadt.Rows[0]["rfa_target5"].ToString();
                    prop4 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target5"].ToString() == "RH")
                {
                    targetUser5 = Rfadt.Rows[0]["rfa_target5"].ToString();
                    prop4 = "0";
                }
                else if (Rfadt.Rows[0]["rfa_target5"].ToString() == "HOD")
                {
                    targetUser5 = Rfadt.Rows[0]["rfa_target5"].ToString();
                    prop4 = "0";
                }
                else
                {
                    targetUser5 = Rfadt.Rows[0]["rfa_target5"].ToString();
                    prop4 = "0";
                }
                if (prop4 == "1")
                {
                    for (int i4 = 0; i4 < dpVal.Length / 2; i4++)
                    {
                        if (listitems4 != "")
                            listitems4 += ";" + dpVal[i4, 0] + "," + dpVal[i4, 1];
                        else
                            listitems4 = dpVal[i4, 0] + "," + dpVal[i4, 1];
                    }
                }

                if (Int32.TryParse(Rfadt.Rows[0]["rfa_target6"].ToString(), out result))
                {
                    if (Convert.ToInt32(Rfadt.Rows[0]["rfa_target6"].ToString()) > 0)
                    {
                        dpVal = oDBEngine.GetFieldValue(" tbl_master_costcenter ", " cost_id as Id, cost_description as Name ", " cost_costcentertype='Department' ", 2, " cost_description ");
                        targetUser6 = "ODH";
                        subtargetUser6 = Rfadt.Rows[0]["rfa_target6"].ToString();
                        prop5 = "1";
                    }
                    else
                    {
                        targetUser6 = "0";
                    }
                }
                else if (Rfadt.Rows[0]["rfa_target6"].ToString().Substring(0, 2) == "BR")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_internalid as Id, branch_description as Name ", null, 2, " branch_description ");
                    targetUser6 = "SBH";
                    subtargetUser6 = Rfadt.Rows[0]["rfa_target6"].ToString();
                    prop5 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target6"].ToString().Substring(0, 2) == "EM")
                {
                    dpVal = oDBEngine.GetFieldValue(" tbl_master_user ", " user_contactid as Id, user_name +'['+ user_contactid +']' as Name ", null, 2, " user_name ");
                    targetUser6 = "SU";
                    subtargetUser6 = Rfadt.Rows[0]["rfa_target6"].ToString();
                    prop5 = "1";
                }
                else if (Rfadt.Rows[0]["rfa_target6"].ToString() == "RH")
                {
                    targetUser6 = Rfadt.Rows[0]["rfa_target6"].ToString();
                    prop5 = "0";
                }
                else if (Rfadt.Rows[0]["rfa_target6"].ToString() == "HOD")
                {
                    targetUser6 = Rfadt.Rows[0]["rfa_target6"].ToString();
                    prop5 = "0";
                }
                else
                {
                    targetUser6 = Rfadt.Rows[0]["rfa_target6"].ToString();
                    prop5 = "0";
                }
                if (prop5 == "1")
                {
                    for (int i5 = 0; i5 < dpVal.Length / 2; i5++)
                    {
                        if (listitems5 != "")
                            listitems5 += ";" + dpVal[i5, 0] + "," + dpVal[i5, 1];
                        else
                            listitems5 = dpVal[i5, 0] + "," + dpVal[i5, 1];
                    }
                }

                string rule2 = Rfadt.Rows[0]["rfa_rule2"].ToString();
                string rule3 = Rfadt.Rows[0]["rfa_rule3"].ToString();
                string rule4 = Rfadt.Rows[0]["rfa_rule4"].ToString();
                string rule5 = Rfadt.Rows[0]["rfa_rule5"].ToString();
                string rule6 = Rfadt.Rows[0]["rfa_rule6"].ToString();
                string totalLevel = Rfadt.Rows[0]["rfa_TotalLevel"].ToString();
                data = "Edit" + "~" + shortName + "~" + Description + "~" + TempNo + "~" + HoursAllowed + "~" + targetUser1 + "~" + subtargetUser1 + "~" + prop + "~" + listitems + "~" + targetUser2 + "~" + subtargetUser2 + "~" + prop1 + "~" + listitems1 + "~" + targetUser3 + "~" + subtargetUser3 + "~" + prop2 + "~" + listitems2 + "~" + targetUser4 + "~" + subtargetUser4 + "~" + prop3 + "~" + listitems3 + "~" + targetUser5 + "~" + subtargetUser5 + "~" + prop4 + "~" + listitems4 + "~" + targetUser6 + "~" + subtargetUser6 + "~" + prop5 + "~" + listitems5 + "~" + rule2 + "~" + rule3 + "~" + rule4 + "~" + rule5 + "~" + rule6 + "~" + FieldValue[1];
            }
            #endregion
            #region Delete
            if (FieldValue[0] == "Delete")
            {
                int NoofAffect = oDBEngine.DeleteValue("tbl_master_rfa", " rfa_id='" + FieldValue[1] + "'");
                if (NoofAffect != 0)
                {
                    data = "Delete~Y";
                }
            }
            #endregion
        }

        #endregion
        protected void grdrfa_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_master_rfa", "rfa_id,(rfa_shortname) as Title", null);
            grdrfa.DataSource = dt;
            grdrfa.DataBind();
        }
    }
}