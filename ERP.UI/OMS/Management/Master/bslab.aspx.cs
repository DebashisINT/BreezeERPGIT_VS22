using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_bslab : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        OtherMasters obj = new OtherMasters();
        public string maxr;
        Int32 ID, dd;
        string ae;
        string aid;
        decimal fixedamt, minamt, rate;
        string createdate, modifydate, createuser, lastmodifyuser;
        DataTable DT = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowForm();
                txtrate.Attributes.Add("onkeyup", "disablefixedamt1()");
                txtfixedamt.Attributes.Add("onkeyup", "disableminrate()");
                txtminamt.Attributes.Add("onkeyup", "disablefixedamt()");
            }
        }

        protected void ShowForm()
        {
            if (Request.QueryString["id"] != "ADD")
            {
                if (Request.QueryString["id"] != null)
                {
                    ID = Int32.Parse(Request.QueryString["id"]);
                    HttpContext.Current.Session["KeyVal"] = ID;
                }


                string[,] InternalId;

                if (ID != 0)
                {
                    //commented by sanjib due to invalid field name BrokerageSlab_ID and now relace it as TaxSlab_ID 11012017
                    //InternalId = oDBEngine.GetFieldValue("Master_TaxSlab",
                    //                         "TaxSlab_ID,TaxSlab_AmountFrom,TaxSlab_AmountTo,TaxSlab_FlatAmount,TaxSlab_Rate",
                    //                         "BrokerageSlab_ID=" + ID, 7);

                    InternalId = oDBEngine.GetFieldValue("Master_TaxSlab",
                                             "TaxSlab_ID,TaxSlab_AmountFrom,TaxSlab_AmountTo,TaxSlab_FlatAmount,TaxSlab_Rate",
                                             "TaxSlab_ID=" + ID, 7);
                }
                else
                {
                    //commented by sanjib due to invalid field name BrokerageSlab_ID and now relace it as TaxSlab_ID 11012017
                    //InternalId = oDBEngine.GetFieldValue("Master_TaxSlab",
                    //               "TaxSlab_ID,TaxSlab_AmountFrom,TaxSlab_AmountTo,TaxSlab_FlatAmount,TaxSlab_Rate",
                    //               "BrokerageSlab_ID=" + ID, 7);

                    InternalId = oDBEngine.GetFieldValue("Master_TaxSlab",
                                  "TaxSlab_ID,TaxSlab_AmountFrom,TaxSlab_AmountTo,TaxSlab_FlatAmount,TaxSlab_Rate",
                                  "TaxSlab_ID=" + ID, 7);
                }
                if (InternalId[0, 0] !="n")
                {
                     txtslabcode.Text = InternalId[0, 0];
                ddltype.Value = InternalId[0, 1];
                txtmin.Text = InternalId[0, 2];
                txtmax.Text = InternalId[0, 3];
                txtminamt.Text = InternalId[0, 6];
                txtrate.Text = InternalId[0, 5];
                txtfixedamt.Text = InternalId[0, 4];
                txtslabcode.Enabled = false;
                txtmin.Enabled = false;
                txtmax.Enabled = false;
                ddltype.Enabled = false;

                }else{

                    txtslabcode.Text = InternalId[0, 0];
                    ddltype.Value = "Unit Price";
                    txtmin.Text = "0.000001";
                    txtmax.Text = "999999999999.999999";
                    txtmin.Enabled = false;

                    txtrate.Text = "";
                    txtfixedamt.Text = "";
                    txtminamt.Text = "";
                    txtslabcode.Enabled = false;
                    txtmin.Enabled = false;
                    txtmax.Enabled = false;
                    ddltype.Enabled = false;
                }
               


            }
            else
            {

                HttpContext.Current.Session["KeyVal"] = 0;
                ddltype.Value = "Unit Price";
                txtmin.Text = "0.000001";
                txtmax.Text = "999999999999.999999";
                txtmin.Enabled = false;

                txtrate.Text = "";
                txtfixedamt.Text = "";
                txtminamt.Text = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            Int32 userid = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
            createuser = HttpContext.Current.Session["userid"].ToString();

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
                try
                {
                    if (hd1.Value == "1")
                    {
                        InsertBrokerageSlab();
                    }
                    else
                    {
                        string id = HttpContext.Current.Session["KeyVal"].ToString();
                        if (txtfixedamt.Text != "")
                            fixedamt = Convert.ToDecimal(txtfixedamt.Text.ToString());
                        else
                            fixedamt = 0;
                        if (txtminamt.Text != "")
                            minamt = Convert.ToDecimal(txtminamt.Text.ToString());
                        else
                            minamt = 0;

                        if (txtrate.Text != "")
                            rate = Convert.ToDecimal(txtrate.Text.ToString());
                        else
                            rate = 0;

                        String value = "TaxSlab_ID='" + txtslabcode.Text + "', TaxSlab_AmountFrom=" + txtmin.Text + " ,TaxSlab_AmountTo=" + txtmax.Text + ",TaxSlab_FlatAmount=" + fixedamt + ", TaxSlab_Rate=" + rate + ", TaxSlab_CreateUser=" + createuser + ",TaxSlab_ModifyUser=" + lastmodifyuser + ",TaxSlab_ModifyTime='" + modifydate + "'";
                        if (Convert.ToDecimal(txtmin.Text.ToString()) >= Convert.ToDecimal(txtmax.Text.ToString()))
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "testjs", "<script>alert('Maximum Range Should Be Greater Than Minimum Range')</script>");
                        }
                        else
                        {
                            Int32 rowsEffected = oDBEngine.SetFieldValue("Master_TaxSlab", value, " TaxSlab_ID='" + HttpContext.Current.Session["KeyVal"].ToString() + "'");
                            string[,] CName = oDBEngine.GetFieldValue("Master_TaxSlab", "TaxSlab_AmountTo", " TaxSlab_Code='" + txtslabcode.Text + "'", 1);

                            int aa = CName.Length;
                            //int count1 = 0;
                            //for (int j = 0; j < CName.Length; j++)
                            //{
                            //    if (CName[j, 0].Contains("999999999999.999999"))
                            //    {
                            //        dd = 0;
                            //        count1 = count1 + 1;

                            //    }

                            //    else
                            //    {

                            //    }

                            //}

                            //  if (count1==1 && CName[(Convert.ToInt32(aa) - 1), 0]!= "999999999999.999999")
                            //   {
                            //        dd=1;
                            //   }
                            if (CName[(Convert.ToInt32(aa) - 1), 0] == "999999999999.999999")
                            {
                                //dd = 0;
                                hd1.Value = "0";
                            }
                            else
                            {
                                // Convert.ToString((Convert.ToDecimal(max) + Convert.ToDecimal(.000001)));
                                Page.ClientScript.RegisterStartupScript(GetType(), "tjs", "<script>alert('Please insert')</script>");
                                txtmin.Text = Convert.ToString((Convert.ToDecimal(txtmax.Text) + Convert.ToDecimal(.000001)));
                                txtmax.Text = "999999999999.999999";
                                // InsertBrokerageSlab();
                                // dd = 1;
                                hd1.Value = "1";
                            }

                            if (hd1.Value == "0")
                            {

                                string maxval = txtmax.Text.ToString();
                                Int32 rowsdelete;
                                int count = 0;
                                string[,] CName1 = oDBEngine.GetFieldValue("Master_TaxSlab", "TaxSlab_AmountFrom", "TaxSlab_Code='" + txtslabcode.Text + "' and TaxSlab_ID > " + id + "", 1);
                                for (int i = 0; i < CName1.Length; i++)
                                {
                                    if (Convert.ToDecimal(maxval) >= Convert.ToDecimal(CName1[i, 0].ToString()))
                                    {
                                        rowsdelete = oDBEngine.DeleteValue("Master_TaxSlab", " TaxSlab_Code='" + txtslabcode.Text + "'and TaxSlab_AmountTo=" + CName1[i, 0] + " and TaxSlab_ID!=" + id + "");
                                        count = count + 1;
                                    }
                                    else
                                    {

                                    }

                                }

                                if (count >= 0)
                                {
                                    string[,] a = oDBEngine.GetFieldValue("Master_TaxSlab", "TaxSlab_ID", "TaxSlab_Code='" + txtslabcode.Text + "' and TaxSlab_ID > " + id + "", 1);

                                    if (a[0, 0] != null)
                                        aid = a[0, 0];
                                    else
                                        aid = " ";
                                }

                                maxval = Convert.ToString((Convert.ToDecimal(maxval) + Convert.ToDecimal(.000001)));
                                String mvalue = "BrokerageSlab_MinRange='" + maxval + "'";
                                rowsEffected = oDBEngine.SetFieldValue("Master_TaxSlab", mvalue, " TaxSlab_ID='" + aid + "'");

                            }
                        }
                        //else
                        //{
                        //    InsertBrokerageSlab();
                        //}



                    }
                }

                catch (Exception ex)
                {

                }

            }

            else
            {
                InsertBrokerageSlab();
            }
        }

        public void InsertBrokerageSlab()
        {

            Int32 userid = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
            createuser = HttpContext.Current.Session["userid"].ToString();
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


            try
            {
                string min = txtmin.Text.ToString();
                string max = txtmax.Text.ToString();
                if (Convert.ToDecimal(txtmin.Text.ToString()) >= Convert.ToDecimal(txtmax.Text.ToString()))
                    ae = "1";
                else
                    ae = "0";

                if (ae == "1")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "testjs", "<script>alert('Maximum Range Should Be Greater Than Minimum Range')</script>");
                }

                else
                {
                    createdate = oDBEngine.GetDate().ToString();
                    Session["cdate"] = createdate;


                    /* For Tier Structrure ---------------------------- */

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //using (SqlConnection lcon = new SqlConnection(con))
                    //{
                    //    lcon.Open();
                    //    using (SqlCommand lcmdBrokerageSlab = new SqlCommand("BrokerageSlabInsert", lcon))
                    //    {
                    //        lcmdBrokerageSlab.CommandType = CommandType.StoredProcedure;

                    //        SqlParameter parameter = new SqlParameter("@ResultSlab", SqlDbType.VarChar, 20);
                    //        parameter.Direction = ParameterDirection.Output;
                    //        SqlParameter parameter1 = new SqlParameter("@maxrange", SqlDbType.Decimal);
                    //        parameter1.Direction = ParameterDirection.Output;

                    //        lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_Code", txtslabcode.Text.ToString());
                    //        lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_Type", ddltype.SelectedItem.Value);
                    //        lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_MinRange", txtmin.Text.ToString());
                    //        lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_MaxRange", txtmax.Text.ToString());
                    //        if (txtfixedamt.Text != "")
                    //            lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_FlatRate", Convert.ToDecimal(txtfixedamt.Text));
                    //        else
                    //            lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_FlatRate", Convert.ToDecimal("0"));

                    //        if (txtrate.Text != "")
                    //            lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_Rate", Convert.ToDecimal(txtrate.Text));
                    //        else
                    //            lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_Rate", Convert.ToDecimal("0"));

                    //        if (txtminamt.Text != "")
                    //            lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_MinCharge", Convert.ToDecimal(txtminamt.Text));
                    //        else
                    //            lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_MinCharge", Convert.ToDecimal("0"));

                    //        lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_CreateUser", createuser);
                    //        lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_CreateDateTime", createdate);
                    //        lcmdBrokerageSlab.Parameters.AddWithValue("@BrokerageSlab_ModifyUser", lastmodifyuser);
                    //        lcmdBrokerageSlab.Parameters.Add(parameter);
                    //        lcmdBrokerageSlab.Parameters.Add(parameter1);
                    //        lcmdBrokerageSlab.ExecuteNonQuery();

                    //        string slabname = parameter.Value.ToString();
                    //        maxr = parameter1.Value.ToString();

                    //        txtmin.Text = Convert.ToString((Convert.ToDecimal(max) + Convert.ToDecimal(.000001)));
                    //        txtmax.Text = "999999999999.999999";
                    //        txtslabcode.Enabled = false;
                    //        ddltype.Enabled = false;
                    //        txtrate.Text = "";
                    //        txtminamt.Text = "";
                    //        txtfixedamt.Text = "";

                    //        if (slabname == "1")
                    //            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Continue Insert Untill Max Range is 999999999999.999999')</script>");
                    //        else if (slabname == "0")
                    //            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Insertion Complete and Cannot able to Insert for This Code')</script>");
                    //        else if (slabname == "2")
                    //            //  Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Insertion Complete ')</script>");
                    //            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>parent.editwin.close();</script>");
                    //    }

                    //}
                    if (txtfixedamt.Text == "")
                    {
                        txtfixedamt.Text = "0";
                    }

                    if (txtrate.Text == "")
                    {
                        txtrate.Text = "0";
                    }

                    if (txtminamt.Text == "")
                    {
                        txtminamt.Text = "0";
                    }

                    string slabname = obj.Insert_BrokerageSlab(txtslabcode.Text.ToString(), ddltype.SelectedItem.Value.ToString(), txtmin.Text.ToString(),
                        txtmax.Text.ToString(), txtfixedamt.Text, txtrate.Text, txtminamt.Text, createuser, createdate, lastmodifyuser);

                    txtmin.Text = Convert.ToString((Convert.ToDecimal(max) + Convert.ToDecimal(.000001)));
                    txtmax.Text = "999999999999.999999";
                    txtslabcode.Enabled = false;
                    ddltype.Enabled = false;
                    txtrate.Text = "";
                    txtminamt.Text = "";
                    txtfixedamt.Text = "";
                    if (slabname == "1")
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Continue Insert Untill Max Range is 999999999999.999999')</script>");
                    else if (slabname == "0")
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Insertion Complete and Cannot able to Insert for This Code')</script>");
                    else if (slabname == "2")
                        //  Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Insertion Complete ')</script>");
                       // Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>parent.editwin.close();</script>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Record Inserted Successfully');window.location ='brokerageslab.aspx';", true);
                   // ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('test 9'); window.location='../Login.aspx';", true);
                }

            }

            catch (Exception ex)
            {

            }


        }
    }
}