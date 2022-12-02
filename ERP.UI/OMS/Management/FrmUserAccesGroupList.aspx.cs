using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_FrmUserAccesGroupList : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DBEngine oDBEngine = new DBEngine(string.Empty);
        string data;
        string Userid = string.Empty;
        string Groupid = string.Empty;
        string StrHtml = "";
        clsDropDownList cls = new clsDropDownList();
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
            //if (IsPostBack)
            //{
            //    BindGrid();
            //}

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            //for sending email
            rbOnlyClient.Attributes.Add("OnClick", "SelectUserClient('Client')");
            rbClientUser.Attributes.Add("OnClick", "SelectUserClient('User')");

            txtSelectID.Attributes.Add("onkeyup", "callAjax1(this,'GetUserAccessGroup',event)");
            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);
            FillCombo();

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
                if (idlist[0] == "Group")
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

            if (idlist[0] == "User")
            {
                Userid = str;
                data = "User~" + str;
            }
            else if (idlist[0] == "Group")
            {
                Groupid = str;
                data = "Group~" + str;
            }

        }
        private void FillCombo()
        {
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                cls.AddDataToDropDownList(r, cmbsearch);
            }
            else
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";

                cls.AddDataToDropDownList(r, cmbsearch);

            }

        }

        public void BindGrid()
        {

            string AccType = "";
            string IDS = "";
            if (cmbGroup.SelectedItem.Value.ToString() == "0")
                AccType = "U";
            else
                AccType = "A";

            if (HDNValue.Value != null)
            {
                if (rbOnlyClient.Checked == true)
                    IDS = "";
                else
                    IDS = HDNValue.Value.ToString();

            }




            DataSet ds = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("[FETCH_USER_ACCESSGROUP]", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@ACCESS_TYPE", AccType);
                    da.SelectCommand.Parameters.AddWithValue("@ID", IDS);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    // Mantis Issue 24802
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    // End of Mantis Issue 24802
                }
            }
            DataTable dtMain = new DataTable();
            DataTable dtSub = new DataTable();
            dtMain = ds.Tables[0];
            dtSub = ds.Tables[1];


            int flag = 0;

            if (cmbGroup.SelectedItem.Value.ToString() == "0")
            {
                StrHtml = "<table  width=\"100%\"  cellspacing=\"0\" cellpadding=\"0\"  border=\"1px\"  style=\"background-color: #ffffff;\">";
                StrHtml = StrHtml + "<tr style=\"background-color: #B7CEEC;height:25px;\">";
                StrHtml = StrHtml + "<td width=\"20%\"> <span style=\"font-weight: bold;\">Login ID</span></td>";
                StrHtml = StrHtml + "<td width=\"25%\"><span style=\"font-weight: bold;\">Contact Name</span></td>";
                StrHtml = StrHtml + "<td width=\"35%\"><span style=\"font-weight: bold;\">Company</span></td>";
                StrHtml = StrHtml + "<td width=\"20%\"><span style=\"font-weight: bold;\">Branch</span></td>";
                StrHtml = StrHtml + "</tr>";
                for (int i = 0; i < dtMain.Rows.Count; i++)
                {

                    StrHtml = StrHtml + "<tr style=\"font-size:10pt;font-weight:bold;color:Maroon;\"><td>&nbsp;<span style=\"font-weight: bold;\">" + dtMain.Rows[i][2].ToString() + "</span></td><td><span style=\"font-weight: bold;\">&nbsp;" + dtMain.Rows[i][3].ToString() + "</span></td><td><span style=\"font-weight: bold;\">&nbsp;" + dtMain.Rows[i][4].ToString() + "</span></td><td><span style=\"font-weight: bold;\">&nbsp;" + dtMain.Rows[i][5].ToString() + "</span></td></tr>";
                    StrHtml = StrHtml + "<tr><td colspan=\"4\">";
                    StrHtml = StrHtml + "<table  width=\"100%\"  cellspacing=\"0\" cellpadding=\"0\"  border=\"1px\">";
                    StrHtml = StrHtml + "<tr   style=\"background-color: #D1E9F4;\"><td><span style=\"font-weight: bold;\">Segment</span></td><td><span style=\"font-weight: bold;\">Access Group</span></td></tr>";
                    for (int k = 0; k < dtSub.Rows.Count; k++)
                    {

                        if (dtMain.Rows[i][0].ToString() == dtSub.Rows[k][0].ToString())
                        {
                            flag = flag + 1;

                            StrHtml = StrHtml + "<tr   style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\" ><td align=\"left\" style=\"width:100px\">&nbsp;" + dtSub.Rows[k][2].ToString() + "</td><td align=\"left\">&nbsp;" + dtSub.Rows[k][3].ToString() + "</td></tr>";
                        }


                    }
                    StrHtml = StrHtml + "</table>";
                    StrHtml = StrHtml + "</td></tr>";
                }
                StrHtml = StrHtml + "</table>";
            }
            else
            {
                StrHtml = "<table  width=\"100%\"  style=\"font-family:Verdana;background-color:#ffffff;font-size:8pt;border:solid 1px black\" cellspacing=\"0\" cellpadding=\"0\"  border=\"1px\">";


                for (int k = 0; k < dtSub.Rows.Count; k++)
                {
                    StrHtml = StrHtml + "<tr><td colspan=\"4\"><span style=\"font-weight: bold;color:maroon\">&nbsp;User Access Group Name :   " + dtSub.Rows[k][2].ToString() + " &nbsp;[" + dtSub.Rows[k][1].ToString() + "]</span></td></tr>";

                    StrHtml = StrHtml + "<tr  style=\"background-color: #D1E9F4;font-weight:bold;font-family:verdana;\">";
                    StrHtml = StrHtml + "<td width=\"20%\"> <span style=\"font-weight: bold;\">Login ID</span></td>";
                    StrHtml = StrHtml + "<td width=\"25%\"><span style=\"font-weight: bold;\">Contact Name</span></td>";
                    StrHtml = StrHtml + "<td width=\"35%\"><span style=\"font-weight: bold;\">Company</span></td>";
                    StrHtml = StrHtml + "<td width=\"20%\"><span style=\"font-weight: bold;\">Branch</span></td>";
                    StrHtml = StrHtml + "</tr>";

                    for (int i = 0; i < dtMain.Rows.Count; i++)
                    {

                        string[] s = dtMain.Rows[i][6].ToString().Split(',');
                        for (int p = 0; p < s.Length; p++)
                        {
                            if (s[p].ToString() == dtSub.Rows[k][0].ToString())
                            {
                                flag = flag + 1;
                                StrHtml = StrHtml + "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\"  ><td align=\"left\">&nbsp;" + dtMain.Rows[i][2].ToString() + "</td><td align=\"left\">&nbsp;" + dtMain.Rows[i][3].ToString() + "</td><td align=\"left\">&nbsp;" + dtMain.Rows[i][4].ToString() + "</td><td align=\"left\">&nbsp;" + dtMain.Rows[i][5].ToString() + "</td></tr>";
                                //CheckBox chk = (CheckBox)grdUserAccess.Rows[i].FindControl("chkSegmentId");
                                //chk.Checked = true;
                                //drp.SelectedValue = Convert.ToString(drp.Items.FindByValue(s[k].ToString()).Value);
                            }
                        }
                    }
                }

                StrHtml = StrHtml + "</table>";

            }
            DisplayReport.InnerHtml = StrHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HGT", "height();", true);


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BindGrid();
            DisplayReport.Visible = true;
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }

        protected void btnExoprt_Click(object sender, EventArgs e)
        {
            BindGrid();
            DisplayReport.Visible = false;
            Response.AppendHeader("Content-Disposition", "attachment; filename=Ledger.xls");
            Response.ContentType = "application/ms-excel";
            Response.Write(StrHtml.ToString());
            Response.End();
        }
    }
}