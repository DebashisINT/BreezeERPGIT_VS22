using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP
{
    public partial class Schemapopup : ERP.OMS.ViewState_class.VSPage
    {
        MasterSettings masterbl = new MasterSettings();
        public BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public static string ID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {

            }

            string mastersettings = masterbl.GetSettings("isServiceManagementRequred");

            if (!Page.IsPostBack)
            {
                SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
               Sqlfinyear.ConnectionString= Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
               sqlcomp.ConnectionString= Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
               sqlbranch.ConnectionString= Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
               SqlRcptChlnType.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["LastCompany"])) && !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["userbranchID"])))
                {
                    ddl_company.SelectedValue = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    //ddl_branch.SelectedValue = Convert.ToString(Session["userbranchID"]);
                }
                if (mastersettings == "0")
                {

                    SqlSchematype.SelectCommand = "select type_Id,type_name from tbl_master_idschematype where type_Isactive=1 and type_Id not in(134) order by ordernumber";
                    //trRcptChlnTyp.Style.Add("Display", "none");
                }
                else
                {
                    SqlSchematype.SelectCommand = "select type_Id,type_name from tbl_master_idschematype where type_Isactive=1 order by ordernumber";
                }

                DataTable FinMaxDate = oDBEngine.GetDataTable("select FinYear_StartDate from Master_FinYear");
                DataTable FinMinDate = oDBEngine.GetDataTable("select FinYear_EndDate from Master_FinYear");


                Session["FinYearStartDate"] = Convert.ToString(FinMaxDate.Rows[0]["FinYear_StartDate"]);
                Session["FinYearEndDate"] = Convert.ToString(FinMinDate.Rows[0]["FinYear_EndDate"]);

                validfrom.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                validfrom.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                validfrom.Date = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));



                validto.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                validto.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));

                validto.Date = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));



                cleanall();
                if (Request.QueryString["Schemaid"] != null)
                {
                    Int64 id = Convert.ToInt64(Request.QueryString["Schemaid"]);
                    filltext(id);
                    hdnPageStatus.Value = "Edit";
                    chkSupressZero.ClientEnabled = false;
                    ID = Convert.ToString(id);

                }
                else
                {
                    hdnPageStatus.Value = "Add";
                    txtdigit.Attributes.Add("readOnly", "readonly");
                    txtprefixwith.Attributes.Add("readOnly", "readonly");
                    txtstart.Attributes.Add("readOnly", "readonly");
                }


            }
        }


       
        protected void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                lblmessage.Visible = false;
                lblmessage.Text = string.Empty;

                if (!CheckTypeAndSuffixAndPreffix(Convert.ToString(drp_type.Value), txtsuffix.Text.Trim(), txtprefix.Text.Trim(), txtdigit.Text))
                {
                    DataSet dsEmail = new DataSet();
                    //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection con = new SqlConnection(conn);
                    SqlCommand cmd3 = new SqlCommand("prc_Schemamaster", con);
                    cmd3.CommandType = CommandType.StoredProcedure;

                    if (Request.QueryString["Schemaid"] != null)
                    {
                        cmd3.Parameters.AddWithValue("@schemaID", Convert.ToInt64(Request.QueryString["Schemaid"]));
                        cmd3.Parameters.AddWithValue("@SchemaName", txtschname.Text.Trim());
                        cmd3.Parameters.AddWithValue("@length", Convert.ToInt32(hddnLengthValue.Value));
                        cmd3.Parameters.AddWithValue("@schematype_id", Convert.ToInt64(drp_type.Value));
                        cmd3.Parameters.AddWithValue("@suppressZero", Convert.ToBoolean(chkSupressZero.Checked));
                        if (Convert.ToUInt64(drp_type.Value) == 21 || Convert.ToUInt64(drp_type.Value) == 24)
                        {
                            cmd3.Parameters.AddWithValue("@VoucherType", ddlVoucherType.SelectedValue);
                        }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@VoucherType","");
                        }

                        //cmd3.Parameters.AddWithValue("@financial_year_id", Convert.ToInt32(drpfinyear.SelectedValue));
                        if (Convert.ToUInt64(drp_type.Value) == 4 || Convert.ToUInt64(drp_type.Value) == 5)
                        {
                            rdpschematype.SelectedValue = "1";
                        }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@company", Convert.ToString(ddl_company.SelectedItem.Value));
                            cmd3.Parameters.AddWithValue("@Branch", Convert.ToInt64(ddl_branch.SelectedItem.Value));
                            cmd3.Parameters.AddWithValue("@financial_year_id", Convert.ToInt32(drpfinyear.SelectedValue));
                        }

                        if (rdpschematype.SelectedValue == "1")
                        {
                            if (validate())
                            {
                                cmd3.Parameters.AddWithValue("@prefix", Convert.ToString(txtprefix.Text.Trim()));
                                cmd3.Parameters.AddWithValue("@suffix", Convert.ToString(txtsuffix.Text).Trim());
                                cmd3.Parameters.AddWithValue("@digit", Convert.ToInt32(txtdigit.Text));
                                cmd3.Parameters.AddWithValue("@start", Convert.ToInt32(txtstart.Text));
                                cmd3.Parameters.AddWithValue("@ValidFrom", validfrom.Date);
                                cmd3.Parameters.AddWithValue("@ValidTo", validto.Date);
                                //if (txtprefixwith.Text != string.Empty)
                                //{
                                //    cmd3.Parameters.AddWithValue("@prefix_with", Convert.ToString(txtprefixwith.Text));
                                //}
                                //else
                                //{
                                //    //lblmessage.Visible = true;
                                //    //lblmessage.Text = "Please input Prefill with, because you have select Auto schema.";
                                //    return;
                                //}
                                string checkautoschemalength = Convert.ToString(txtprefix.Text.Trim()) + Convert.ToString(txtsuffix.Text.Trim()) + Convert.ToString(txtdigit.Text.Trim());
                                int length = Convert.ToInt32(hddnLengthValue.Value);

                                if (checkautoschemalength.Trim().Length > length)
                                {
                                    lblmessage.Visible = false;
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('The Length should not be less than Sum of prefix and suffix and Digit.')</script>");
                                    return;
                                }
                                else
                                {
                                    lblmessage.Visible = false;
                                    lblmessage.Text = string.Empty;
                                }
                            }
                            else { return; }

                        }
                        else if (rdpschematype.SelectedValue == "2")
                        {
                            if (validate())
                            {
                                cmd3.Parameters.AddWithValue("@prefix", Convert.ToString(txtprefix.Text.Trim()));
                                cmd3.Parameters.AddWithValue("@suffix", Convert.ToString(txtsuffix.Text).Trim());
                                cmd3.Parameters.AddWithValue("@digit", Convert.ToInt32(txtdigit.Text));
                                cmd3.Parameters.AddWithValue("@start", Convert.ToInt32(txtstart.Text));
                                cmd3.Parameters.AddWithValue("@ValidFrom", validfrom.Date);
                                cmd3.Parameters.AddWithValue("@ValidTo", validto.Date);
                                string checkautoschemalength = Convert.ToString(txtprefix.Text.Trim()) + Convert.ToString(txtsuffix.Text.Trim()) + Convert.ToString(txtdigit.Text.Trim());
                                int length = Convert.ToInt32(hddnLengthValue.Value);

                                if (checkautoschemalength.Trim().Length > length)
                                {
                                    lblmessage.Visible = false;
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('The Length should not be less than Sum of prefix and suffix and Digit.')</script>");
                                    return;
                                }
                                else
                                {
                                    lblmessage.Visible = false;
                                    lblmessage.Text = string.Empty;
                                }
                            }
                            else { return; }

                        }
                        else if (rdpschematype.SelectedValue == "0")
                        {
                            cmd3.Parameters.AddWithValue("@suffix", string.Empty);

                            cmd3.Parameters.AddWithValue("@prefix", string.Empty);
                            cmd3.Parameters.AddWithValue("@start", 0);
                            cmd3.Parameters.AddWithValue("@digit", 0);
                            cmd3.Parameters.AddWithValue("@ValidTo", validto.Date);
                            cmd3.Parameters.AddWithValue("@ValidFrom", validfrom.Date);
                        }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@suffix", Convert.ToString(txtsuffix.Text.Trim()));
                            cmd3.Parameters.AddWithValue("@prefix", Convert.ToString(txtprefix.Text.Trim()));
                        }

                        cmd3.Parameters.AddWithValue("@schema_type", rdpschematype.SelectedValue);

                        //if (Convert.ToString(rdpschematype.SelectedValue) == "2")
                        //{
                        //    if (string.IsNullOrEmpty(txtstart.Text.Trim()) || txtstart.Text.Trim() == "0")
                        //    {
                        //        cmd3.Parameters.AddWithValue("@start", 1);
                        //    }
                        //    else
                        //    {
                        //        cmd3.Parameters.AddWithValue("@start", txtstart.Text.Trim());
                        //    }

                        //    if (string.IsNullOrEmpty(txtdigit.Text.Trim()) || txtdigit.Text.Trim() == "0")
                        //    {
                        //        cmd3.Parameters.AddWithValue("@digit", 4);
                        //    }
                        //    else
                        //    {
                        //        cmd3.Parameters.AddWithValue("@digit", txtdigit.Text.Trim());
                        //    }

                        //    string checkautoschemalength = Convert.ToString(txtprefix.Text) + Convert.ToString(txtsuffix.Text.Trim()) + Convert.ToString(txtdigit.Text.Trim());
                        //    int length = Convert.ToInt32(hddnLengthValue.Value);

                        //    if (checkautoschemalength.Trim().Length > length)
                        //    {
                        //        lblmessage.Visible = false;
                        //        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('The Length should not be less than Sum of prefix and suffix and Digit.')</script>");

                        //        return;
                        //    }
                        //    else
                        //    {
                        //        lblmessage.Visible = false;
                        //        lblmessage.Text = string.Empty;
                        //    }
                        //}

                        cmd3.Parameters.AddWithValue("@actiontype", 1);//it is use for action like 0/1/2/3/4 ->select/UPDATE/insert/delete/selectbyid
                    }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@SchemaName", txtschname.Text.Trim());
                        cmd3.Parameters.AddWithValue("@schematype_id", Convert.ToInt64(drp_type.Value));
                        cmd3.Parameters.AddWithValue("@length", Convert.ToInt32(hddnLengthValue.Value));
                        cmd3.Parameters.AddWithValue("@ValidTo", validto.Date);
                        cmd3.Parameters.AddWithValue("@ValidFrom", validfrom.Date);
                        cmd3.Parameters.AddWithValue("@suppressZero", Convert.ToBoolean(chkSupressZero.Checked));
                        if (Convert.ToUInt64(drp_type.Value) == 21 || Convert.ToUInt64(drp_type.Value) == 24)
                        {
                            cmd3.Parameters.AddWithValue("@VoucherType", ddlVoucherType.SelectedValue);
                        }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@VoucherType", "");
                        }

                        if (Convert.ToUInt64(drp_type.Value) == 4 || Convert.ToUInt64(drp_type.Value) == 5)
                        {
                            rdpschematype.SelectedValue = "1";
                            //rdpschematypetr.Attributes.Add("display", "none");
                        }
                        else {
                            cmd3.Parameters.AddWithValue("@company", Convert.ToString(ddl_company.SelectedItem.Value));
                            cmd3.Parameters.AddWithValue("@Branch", Convert.ToInt64(ddl_branch.SelectedItem.Value));
                            cmd3.Parameters.AddWithValue("@financial_year_id", Convert.ToInt32(drpfinyear.SelectedValue));
                        }

                        if (rdpschematype.SelectedValue == "1")
                        {
                            if (validate())
                            {
                                cmd3.Parameters.AddWithValue("@prefix", Convert.ToString(txtprefix.Text.Trim()));
                                cmd3.Parameters.AddWithValue("@suffix", Convert.ToString(txtsuffix.Text.Trim()));
                                cmd3.Parameters.AddWithValue("@digit", Convert.ToInt32(txtdigit.Text.Trim()));
                                cmd3.Parameters.AddWithValue("@start", Convert.ToInt32(txtstart.Text.Trim()));

                                //if (txtprefixwith.Text != string.Empty)
                                //{
                                //    cmd3.Parameters.AddWithValue("@prefix_with", Convert.ToString(txtprefixwith.Text));
                                //    txtdigit.Attributes.Remove("ReadOnly");
                                //    txtprefixwith.Attributes.Remove("ReadOnly");
                                //    txtstart.Attributes.Remove("ReadOnly");
                                //}
                                //else
                                //{
                                //    lblmessage.Visible = true;
                                //    lblmessage.Text = "Please input Prefill with, because you have select Auto schema.";
                                //    return;
                                //}

                                string checkautoschemalength = Convert.ToString(txtprefix.Text) + Convert.ToString(txtsuffix.Text.Trim()) + Convert.ToString(txtdigit.Text.Trim());
                                int length = Convert.ToInt32(hddnLengthValue.Value);

                                if (checkautoschemalength.Trim().Length > length)
                                {
                                    lblmessage.Visible = false;
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('The Length should not be less than Sum of prefix and suffix and Digit.')</script>");

                                    return;
                                }
                                else
                                {
                                    lblmessage.Visible = false;
                                    lblmessage.Text = string.Empty;
                                }
                            }
                            else
                            {
                                if (Convert.ToUInt64(drp_type.Value) == 4 || Convert.ToUInt64(drp_type.Value) == 5)
                                {
                                    //rdpschematypetr.Attributes.Add("display", "none");
                                }
                                return;
                            }
                        }
                        else if (rdpschematype.SelectedValue == "2")
                        {
                            if (validate())
                            {
                                cmd3.Parameters.AddWithValue("@prefix", Convert.ToString(txtprefix.Text.Trim()));
                                cmd3.Parameters.AddWithValue("@suffix", Convert.ToString(txtsuffix.Text.Trim()));
                                cmd3.Parameters.AddWithValue("@digit", Convert.ToInt32(txtdigit.Text.Trim()));
                                cmd3.Parameters.AddWithValue("@start", Convert.ToInt32(txtstart.Text.Trim()));

                                string checkautoschemalength = Convert.ToString(txtprefix.Text) + Convert.ToString(txtsuffix.Text.Trim()) + Convert.ToString(txtdigit.Text.Trim());
                                int length = Convert.ToInt32(hddnLengthValue.Value);

                                if (checkautoschemalength.Trim().Length > length)
                                {
                                    lblmessage.Visible = false;
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('The Length should not be less than Sum of prefix and suffix and Digit.')</script>");
                                    return;
                                }
                                else
                                {
                                    lblmessage.Visible = false;
                                    lblmessage.Text = string.Empty;
                                }
                            }
                            else
                            {
                                if (Convert.ToUInt64(drp_type.Value) == 4 || Convert.ToUInt64(drp_type.Value) == 5)
                                {
                                    //rdpschematypetr.Attributes.Add("display", "none");
                                }
                                return;
                            }
                        }
                        else if (rdpschematype.SelectedValue == "0")
                        {
                            cmd3.Parameters.AddWithValue("@suffix", string.Empty);
                            cmd3.Parameters.AddWithValue("@prefix", string.Empty);
                            cmd3.Parameters.AddWithValue("@start", 0);
                            cmd3.Parameters.AddWithValue("@digit", 0);
                        }
                        else
                        {
                            cmd3.Parameters.AddWithValue("@prefix", Convert.ToString(txtprefix.Text.Trim()));
                            cmd3.Parameters.AddWithValue("@suffix", Convert.ToString(txtsuffix.Text.Trim()));
                        }

                        //if (Convert.ToString(rdpschematype.SelectedValue) == "2")
                        //{
                        //    if (string.IsNullOrEmpty(txtstart.Text.Trim()) || txtstart.Text.Trim() == "0")
                        //    {
                        //        cmd3.Parameters.AddWithValue("@start", 1);
                        //    }
                        //    else
                        //    {
                        //        cmd3.Parameters.AddWithValue("@start", txtstart.Text.Trim());
                        //    }

                        //    if (string.IsNullOrEmpty(txtdigit.Text.Trim()) || txtdigit.Text.Trim() == "0")
                        //    {
                        //        cmd3.Parameters.AddWithValue("@digit", 4);
                        //    }
                        //    else
                        //    {
                        //        cmd3.Parameters.AddWithValue("@digit", txtdigit.Text.Trim());
                        //    }

                        //    string checkautoschemalength = Convert.ToString(txtprefix.Text) + Convert.ToString(txtsuffix.Text.Trim()) + Convert.ToString(txtdigit.Text.Trim());
                        //    int length = Convert.ToInt32(hddnLengthValue.Value);

                        //    if (checkautoschemalength.Trim().Length > length)
                        //    {
                        //        lblmessage.Visible = false;
                        //        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('The Length should not be less than Sum of prefix and suffix and Digit.')</script>");

                        //        return;
                        //    }
                        //    else
                        //    {
                        //        lblmessage.Visible = false;
                        //        lblmessage.Text = string.Empty;
                        //    }


                        //}

                        cmd3.Parameters.AddWithValue("@schema_type", rdpschematype.SelectedValue);
                        

                        cmd3.Parameters.AddWithValue("@actiontype", 2);//it is use for action like 0/1/2/3/4 ->select/UPDATE/insert/delete/selectbyid
                    }

                    if (ChkActive.Checked)
                    { cmd3.Parameters.AddWithValue("@IsActive", 1); }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@IsActive", 0);
                    }
                    cmd3.Parameters.AddWithValue("@RecptChallanType", drp_RcptChlnTyp.Value);
                    cmd3.Parameters.AddWithValue("@DayWise", chkDateWise.Checked);

                    if (!CheckUniqueNameS(txtschname.Text, ID, Convert.ToString(drp_type.Value)))
                    {
                        cmd3.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd3;
                        Adap.Fill(dsEmail);
                        cmd3.Dispose();
                        con.Dispose();
                        GC.Collect();
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please use unique Schema name.')</script>");
                        return;

                    }
                    lblmessage.Visible = false;

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>Confirmsave();</script>");

                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Duplicate Suffix and prefix and No of Digit.')</script>");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Visible = false;
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Error exception.Please try again.')</script>");
            }

        }

        #region private method
        private void filltext(Int64 id)
        {
            txtdigit.Text = string.Empty;
            txtprefix.Text = string.Empty;
            txtprefixwith.Text = string.Empty;
            txtschname.Text = string.Empty;
            txtstart.Text = string.Empty;
            txtsuffix.Text = string.Empty;

            //Set Enable false for Edit

            drp_type.Enabled = false;
            txtschname.Enabled = false;
            rdpschematype.Enabled = false;

            //End Edit 

            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("exec prc_Schemamaster @actiontype=4,@schemaID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                hdnidschemaId.Value = Convert.ToString(dt.Rows[0]["schemaID"]);
                txtdigit.Text = Convert.ToString(dt.Rows[0]["digit"]);
                txtprefix.Text = Convert.ToString(dt.Rows[0]["prefix"]);
                txtprefixwith.Text = Convert.ToString(dt.Rows[0]["prefix_with"]);
                txtschname.Text = Convert.ToString(dt.Rows[0]["SchemaName"]);
                txtstart.Text = Convert.ToString(dt.Rows[0]["start"]);
                txtsuffix.Text = Convert.ToString(dt.Rows[0]["suffix"]);
                drp_type.Value = Convert.ToString(dt.Rows[0]["schematype_id"]);
                hdndrtType.Value = Convert.ToString(dt.Rows[0]["schematype_id"]);
                drplenght.SelectedValue = Convert.ToString(dt.Rows[0]["lenght"]);
                HdnedittLength.Value = Convert.ToString(dt.Rows[0]["lenght"]);
                chkDateWise.Checked = Convert.ToBoolean(dt.Rows[0]["DayWiseFlag"]);
                validfrom.Date = Convert.ToDateTime(dt.Rows[0]["Valid_From"]);
                validto.Date = Convert.ToDateTime(dt.Rows[0]["Valid_Upto"]);
                chkSupressZero.Checked = Convert.ToBoolean(dt.Rows[0]["suppressZero"]);
                validfrom.ClientEnabled = false;
                validto.ClientEnabled = false;
                drp_RcptChlnTyp.Value = Convert.ToString(dt.Rows[0]["Type_detailsid"]);
                hdnTypeID.Value = Convert.ToString(dt.Rows[0]["schematype_id"]);

                ChkActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]); //0019985: 'Is Active' under the numbering scheme is not working

                if (Convert.ToString(dt.Rows[0]["financial_year_id"]) != "")
                {
                    if(Convert.ToString(dt.Rows[0]["financial_year_id"]) != "0")
                    {
                        drpfinyear.SelectedValue = Convert.ToString(dt.Rows[0]["financial_year_id"]);
                    }
                }
                

                rdpschematype.SelectedValue = Convert.ToString(dt.Rows[0]["schema_type"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Company"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["branch"])))
                {
                    ddl_company.SelectedValue = Convert.ToString(dt.Rows[0]["Company"]);
                    ddl_branch.SelectedValue = Convert.ToString(dt.Rows[0]["branch"]);
                }
                if (Convert.ToString(dt.Rows[0]["schematype_id"]) == "21" || Convert.ToString(dt.Rows[0]["schematype_id"]) == "24")
                {
                    ddlVoucherType.SelectedValue = Convert.ToString(dt.Rows[0]["VoucherType"]);
                }
                if (Convert.ToString(rdpschematype.SelectedValue) != "")
                {
                    if (Convert.ToUInt32(rdpschematype.SelectedValue) == 1)
                    {

                        txtdigit.Attributes.Remove("ReadOnly");
                        txtprefixwith.Attributes.Remove("ReadOnly");
                        txtstart.Attributes.Remove("ReadOnly");


                    }
                    else if (Convert.ToUInt32(rdpschematype.SelectedValue) == 0)
                    {
                        txtdigit.Attributes.Add("readOnly", "readonly");
                        txtprefixwith.Attributes.Add("readOnly", "readonly");
                        txtstart.Attributes.Add("readOnly", "readonly");
                    }
                    else
                    {
                        txtdigit.Attributes.Add("readOnly", "readonly");
                        txtprefixwith.Attributes.Add("readOnly", "readonly");
                        txtstart.Attributes.Add("readOnly", "readonly");
                    }
                }
                else
                {
                    txtdigit.Attributes.Add("readOnly", "readonly");
                    txtprefixwith.Attributes.Add("readOnly", "readonly");
                    txtstart.Attributes.Add("readOnly", "readonly");
                }

                #region   ######## Daywise FLag ##########
                if (chkDateWise.Checked == true)
                {
                    txtprefix.Enabled = false;
                    txtsuffix.Enabled = false;
                }
                else
                {
                    txtprefix.Enabled = true;
                    txtsuffix.Enabled = true; 
                }
                #endregion
            }
        }

        private void cleanall()
        {
            txtdigit.Text = string.Empty;
            txtprefix.Text = string.Empty;
            txtprefixwith.Text = string.Empty;
            txtschname.Text = string.Empty;
            txtstart.Text = string.Empty;
            txtsuffix.Text = string.Empty;

            lblmessage.Visible = false;
            lblmessage.Text = string.Empty;
            lbl_txtprefix.Visible = false;
            //lbltxtsuffix_reqName.Visible = false;
            lbl_txtdigit.Visible = false;
            lbl_txtstart.Visible = false;
        }

        private bool validate()
        {
            bool isvalidate = true;
            //if (!string.IsNullOrEmpty(txtprefix.Text.Trim()))
            //{

            //    lbl_txtprefix.Visible = false;

            //}
            //else
            //{
            //    lbl_txtprefix.Visible = true;
            //    isvalidate = false;
            //}

            //if (!string.IsNullOrEmpty(txtsuffix.Text.Trim()))
            //{

            //    lbltxtsuffix_reqName.Visible = false;

            //}
            //else
            //{

            //    lbltxtsuffix_reqName.Visible = true;
            //    isvalidate = false;
            //}


            if (txtdigit.Text.Trim() != string.Empty && txtdigit.Text.Trim() != "0")
            {

                lbl_txtdigit.Visible = false;
            }
            else
            {
                isvalidate = false;
                lbl_txtdigit.Visible = true;
                txtdigit.Attributes.Remove("ReadOnly");
                txtprefixwith.Attributes.Remove("ReadOnly");
                txtstart.Attributes.Remove("ReadOnly");

            }

            if (txtstart.Text.Trim() != string.Empty && txtstart.Text.Trim() != "0")
            {

                lbl_txtstart.Visible = false;
            }
            else
            {
                isvalidate = false;
                lbl_txtstart.Visible = true;
                txtdigit.Attributes.Remove("ReadOnly");
                txtprefixwith.Attributes.Remove("ReadOnly");
                txtstart.Attributes.Remove("ReadOnly");

            }

            return isvalidate;


        }

        [WebMethod]
        public static bool CheckUniqueName(string clientName, string procode, string typevalue)
        {
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            BusinessLogicLayer.DBEngine oDBEngines = new BusinessLogicLayer.DBEngine(string.Empty);
            bool IsPresent = false;


            //procode = HttpContext.Current..href.split("=")[1];
            if (ID == null)
            {
                IsPresent = mshort.CheckUniqueWithtype(clientName, procode, "Masterschemaname", typevalue);
                //DataTable dt = oDBEngines.GetDataTable("SELECT COUNT(*) as cnt FROM tbl_master_idschema WHERE SchemaName ='" + clientName + "' and type_Id=" + typevalue);
                //if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["cnt"]) != "0")
                //{

                //}
            }
            else
            {
                IsPresent = mshort.CheckUniqueWithtype(clientName, procode, "Masterschemaname", typevalue);
                //DataTable dt = oDBEngines.GetDataTable("SELECT COUNT(*) as cnt FROM tbl_master_idschema WHERE SchemaName ='" + clientName + "' and type_Id=" + typevalue + " and Id<>" + ID);
                //if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["cnt"]) != "0")
                //{

                //}
            }



            return IsPresent;
        }

        [WebMethod]
        public static object BindLength(Int64 SchemaId)
        {
            List<SchemaLenth> GrpDet = new List<SchemaLenth>();
            {
                ProcedureExecute pro = new ProcedureExecute("prc_Schemamaster");
                pro.AddVarcharPara("@Action", 500, "SchemaDetails");
                pro.AddBigIntegerPara("@schemaID", SchemaId);

                DataTable addtab = pro.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new SchemaLenth
                          {
                              SchemaId = Convert.ToInt64(dr["SchemaId"]),
                              SchemaValue = Convert.ToString(dr["SchemaValue"]),
                             

                          }).ToList();

            }
            return GrpDet;
        }

        [WebMethod]
        public static object EditBindLength(Int64 SchemaId)
        {
            List<SchemaLenth> GrpDet = new List<SchemaLenth>();
            {
                ProcedureExecute pro = new ProcedureExecute("prc_Schemamaster");
                pro.AddVarcharPara("@Action", 500, "EditSchemaDetails");
                pro.AddBigIntegerPara("@schemaID", SchemaId);

                DataTable addtab = pro.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new SchemaLenth
                          {
                              SchemaId = Convert.ToInt64(dr["SchemaId"]),
                              SchemaValue = Convert.ToString(dr["SchemaValue"]),


                          }).ToList();

            }
            return GrpDet;
        }



        public bool CheckUniqueNameS(string clientName, string procode, string typevalue)
        {
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            BusinessLogicLayer.DBEngine oDBEngines = new BusinessLogicLayer.DBEngine(string.Empty);
            bool IsPresent = false;


            //procode = HttpContext.Current..href.split("=")[1];
            if (ID == null)
            {
                IsPresent = mshort.CheckUniqueWithtype(clientName, procode, "Masterschemaname", typevalue);
                //DataTable dt = oDBEngines.GetDataTable("SELECT COUNT(*) as cnt FROM tbl_master_idschema WHERE SchemaName ='" + clientName + "' and type_Id=" + typevalue);
                //if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["cnt"]) != "0")
                //{

                //}
            }
            else
            {
                IsPresent = mshort.CheckUniqueWithtype(clientName, procode, "Masterschemaname", typevalue);
                //DataTable dt = oDBEngines.GetDataTable("SELECT COUNT(*) as cnt FROM tbl_master_idschema WHERE SchemaName ='" + clientName + "' and type_Id=" + typevalue + " and Id<>" + ID);
                //if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["cnt"]) != "0")
                //{

                //}
            }



            return IsPresent;
        }

        public bool CheckTypeAndSuffixAndPreffix(string Schemetype, string Suffix, string preffix, string NoofDigit)
        {
            bool IsExist = false;

            if (!string.IsNullOrEmpty(Schemetype) && !string.IsNullOrEmpty(Suffix) && !string.IsNullOrEmpty(preffix) && !string.IsNullOrEmpty(NoofDigit))
            {
                Int64 schemaID = Convert.ToInt64(Request.QueryString["Schemaid"]);
                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable("exec prc_Schemamaster @schemaID=" + schemaID + ",@actiontype=10,@schematype_id=" + Schemetype + ",@prefix='" + preffix + "',@suffix='" + Suffix + "',@digit='" + NoofDigit + "'");

                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }

            }

            return IsExist;

        }

        #endregion

        public class SchemaLenth
        {
            public Int64 SchemaId { get; set; }
            public string SchemaValue { get; set; }
        }

    }
}