using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_chargemaster : ERP.OMS.ViewState_class.VSPage
    {
        Int32 ID;
        string createdate, rd, createuser, lastmodifyuser, modifydate, ar1;
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList OclsDropDownList = new clsDropDownList();
        Brokerage_SchemesBL OBrokerage_SchemesBL = new Brokerage_SchemesBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowForm();
            }
        }

        protected void ShowForm()
        {

            string aa = Request.QueryString["id"].ToString();
            string aa1 = Request.QueryString["Type"].ToString();
            if (aa.Contains("ADD"))
            {
                string ar1 = HttpContext.Current.Session["userlastsegment"].ToString();

                HttpContext.Current.Session["KeyVal"] = 0;
                txtname.Text = "";
                txtcode.Text = "";

                if (ar1 == "9" || ar1 == "10")
                {

                    string[,] r = new string[1, 2];
                    r[0, 0] = "3";
                    r[0, 1] = "DP Charges";
                    // oDBEngine.AddDataToDropDownListToAspx(r, ddltype, false);
                    OclsDropDownList.AddDataToDropDownListToAspx(r, ddltype, false);
                    ddltype.SelectedIndex = 0;
                    ddltype.Enabled = false;

                    // oDBEngine.AddDataToDropDown(r, ddltype, true);
                }
                else
                {

                    string[,] r = new string[1, 2];
                    if (aa1 == "BR")
                    {
                        r[0, 0] = "1";
                        r[0, 1] = "Brokerage";
                        txtcode.Text = "GRPBR";
                    }
                    else if (aa1 == "CH")
                    {
                        r[0, 0] = "2";
                        r[0, 1] = "Charges";
                        txtcode.Text = "GRPCH";
                    }
                    // oDBEngine.AddDataToDropDownListToAspx(r, ddltype, false);
                    OclsDropDownList.AddDataToDropDownListToAspx(r, ddltype, false);
                    ddltype.SelectedIndex = 0;
                    ddltype.Enabled = false;


                    //oDBEngine.AddDataToDropDownListToAspx(r5, ddltype, false);
                    ////ddltype.Value = "DP Charges";
                }



            }

            else
            {
                if (Request.QueryString["id"] != null)
                {
                    ID = Int32.Parse(Request.QueryString["id"]);
                    HttpContext.Current.Session["KeyVal"] = ID;
                }

                string[,] InternalId;

                if (ID != 0)
                {
                    InternalId = oDBEngine.GetFieldValue("Master_ChargeGroup",
                                             "ChargeGroup_Type,ChargeGroup_Code,ChargeGroup_Name,ChargeGroup_IsDefault",
                                             "ChargeGroup_ID=" + ID, 4);
                }
                else
                {
                    InternalId = oDBEngine.GetFieldValue("Master_ChargeGroup",
                                   "ChargeGroup_Type,ChargeGroup_Code,ChargeGroup_Name,ChargeGroup_IsDefault",
                                   "ChargeGroup_ID=" + ID, 4);
                }



                if (InternalId[0, 0] == "1" || InternalId[0, 0] == "2")
                {
                    string[,] r4 = new string[2, 2];

                    if (aa1 == "BR")
                    {
                        r4[0, 0] = "1";
                        r4[0, 1] = "Brokerage";
                    }
                    else if (aa1 == "CH")
                    {
                        r4[0, 0] = "2";
                        r4[0, 1] = "Charges";
                    }
                    //oDBEngine.AddDataToDropDownListToAspx(r4, ddltype, false);
                    OclsDropDownList.AddDataToDropDownListToAspx(r4, ddltype, false);
                    //ddltype.SelectedIndex = 0;
                    //ddltype.Enabled = false;
                }
                else
                {
                    string[,] r = new string[1, 2];
                    r[0, 0] = "3";
                    r[0, 1] = "DP Charges";
                    //  oDBEngine.AddDataToDropDownListToAspx(r6, ddltype, false);
                    // oDBEngine.AddDataToDropDownListToAspx(r, ddltype, false);
                    OclsDropDownList.AddDataToDropDownListToAspx(r, ddltype, false);
                }
                ddltype.Value = InternalId[0, 0];
                txtcode.Text = InternalId[0, 1];
                txtname.Text = InternalId[0, 2];
                string rdvalue = InternalId[0, 3];
                if (rdvalue.ToString().Trim() == "No")
                    rdno.Checked = true;
                else
                    rdyes.Checked = true;



                txtcode.Enabled = false;
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            Int32 userid = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
            createuser = HttpContext.Current.Session["userid"].ToString();
            if (txtcode.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1", "<script>alert('Code is required.')</script>");
                return;
            }

            if (int.Parse(HttpContext.Current.Session["KeyVal"].ToString()) != 0)        //________For Update
            {
                lastmodifyuser = HttpContext.Current.Session["userid"].ToString();
            }
            else
                lastmodifyuser = "";
            if (int.Parse(HttpContext.Current.Session["KeyVal"].ToString()) != 0)        //________For Update
            {
                modifydate = oDBEngine.GetDate().ToString();
            }
            else
                modifydate = "";
            if (int.Parse(HttpContext.Current.Session["KeyVal"].ToString()) != 0)        //________For Update
            {
                if (rdno.Checked == true)
                    rd = "No";
                else
                    rd = "Yes";

                String value = "ChargeGroup_Type=" + ddltype.SelectedItem.Value + ", ChargeGroup_Name='" + txtname.Text + "',ChargeGroup_IsDefault='" + rd + "',ChargeGroup_CreateUser=" + createuser + ",ChargeGroup_ModifyUser=" + lastmodifyuser + ",ChargeGroup_ModifyDateTime='" + modifydate + "'";
                string[,] CName = oDBEngine.GetFieldValue("Master_ChargeGroup", "ChargeGroup_Code", " ChargeGroup_ID !=" + int.Parse(HttpContext.Current.Session["KeyVal"].ToString()), 1);
                string[,] ww = oDBEngine.GetFieldValue("Master_ChargeGroup", "ChargeGroup_IsDefault", "ChargeGroup_Type=" + ddltype.SelectedItem.Value + " and ChargeGroup_ID !=" + int.Parse(HttpContext.Current.Session["KeyVal"].ToString()), 1);
                for (int i = 0; i < CName.Length; i++)
                {

                    if (txtcode.Text == CName[i, 0].ToString().Trim())
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript1", "<script>alert('Code Already Exists')</script>");
                        return;
                    }
                    else
                    {
                    }
                }
                for (int i = 0; i < ww.Length; i++)
                {

                    if (ww[i, 0].Contains("Yes") && rdyes.Checked == true)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JSt", "<script>alert('There is Already a Default " + ddltype.SelectedItem.Text + " Group')</script>");
                        return;

                    }
                }

                Int32 rowsEffected = oDBEngine.SetFieldValue("Master_ChargeGroup", value, " ChargeGroup_ID='" + HttpContext.Current.Session["KeyVal"].ToString() + "'");
                //Response.Redirect("chargemaster.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);


            }
            else
            {
                try
                {

                    createdate = oDBEngine.GetDate().ToString();
                    Session["cdate"] = createdate;
                    int count = 0;
                    string ee = ddltype.SelectedItem.Value.ToString();
                    string a1 = ddltype.SelectedItem.Text.ToString();
                    string[,] CName1 = oDBEngine.GetFieldValue("Master_ChargeGroup", "ChargeGroup_IsDefault", " ChargeGroup_Type =" + ee, 1);

                    for (int i = 0; i < CName1.Length; i++)
                    {

                        if (CName1[i, 0].Contains("Yes"))
                        {
                            count = count + 1;
                        }
                    }

                    if (count == 1 && rdyes.Checked == true)
                    {
                        // hddval.Value = "1";
                        rdno.Checked = true;

                        Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script>alert('There is Already a Default " + a1 + " Group ')</script>");

                    }
                    else
                    {
                        /* For Tier Structure
                        String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                        using (SqlConnection lcon = new SqlConnection(con))
                        {
                            lcon.Open();
                            using (SqlCommand lcmdgroupmasterInsert = new SqlCommand("MasterChargeGroupInsert", lcon))
                            {
                                lcmdgroupmasterInsert.CommandType = CommandType.StoredProcedure;

                                SqlParameter parameter = new SqlParameter("@ResultCode", SqlDbType.Char, 10);
                                parameter.Direction = ParameterDirection.Output;

                           

                                lcmdgroupmasterInsert.Parameters.AddWithValue("@ChargeGroup_Code", txtcode.Text);
                                lcmdgroupmasterInsert.Parameters.AddWithValue("@ChargeGroup_Name", txtname.Text);
                                lcmdgroupmasterInsert.Parameters.AddWithValue("@ChargeGroup_Type", ddltype.SelectedItem.Value);

                                if (rdno.Checked == true)
                                    lcmdgroupmasterInsert.Parameters.AddWithValue("@ChargeGroup_IsDefault", "No");
                                else
                                    lcmdgroupmasterInsert.Parameters.AddWithValue("@ChargeGroup_IsDefault", "Yes");

                                lcmdgroupmasterInsert.Parameters.AddWithValue("@ChargeGroup_CreateUser", createuser);
                                lcmdgroupmasterInsert.Parameters.AddWithValue("@ChargeGroup_CreateDateTime", createdate);
                                lcmdgroupmasterInsert.Parameters.AddWithValue("@ChargeGroup_ModifyUser", lastmodifyuser);
                                lcmdgroupmasterInsert.Parameters.Add(parameter);
                                lcmdgroupmasterInsert.ExecuteNonQuery();

                                string chname = parameter.Value.ToString();

                        */

                        string vIsDefault = "";

                        if (rdno.Checked == true)
                            vIsDefault = "No";
                        else
                            vIsDefault = "Yes";


                        string chname = OBrokerage_SchemesBL.Insert_MasterChargeGroup(
                             txtcode.Text.Trim(), txtname.Text.Trim(), ddltype.SelectedItem.Value.ToString(), vIsDefault, createuser,
                               createdate, lastmodifyuser);


                        if (chname.ToString().Trim() == "0")
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Code Already Exists')</script>");
                    }
                    // }

                    //}
                }

                catch (Exception ex)
                {


                }
            }


            string popUpscript = "";
            popUpscript = "<script language='javascript'>parent.editwin.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);

        }
    }
}