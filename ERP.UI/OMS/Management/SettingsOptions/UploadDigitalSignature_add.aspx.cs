using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_UploadDigitalSignature_add : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        // Converter objConverter = new Converter(); 
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        GlobalSettings globalsetting = new GlobalSettings();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            // ______________________________End Script____________________________//



            if (!IsPostBack)
            {
                dtFrom.EditFormatString = objConverter.GetDateFormat("Date");
                dtTo.EditFormatString = objConverter.GetDateFormat("Date");
                //System.ComponentModel.BackgroundWorker
                txtEmpName.Attributes.Add("onkeyup", "CallAjax(this,'SearchEmployeesForDigitalSignature',event)");
                txtValidUser.Attributes.Add("onkeyup", "CallAjax(this,'SearchEmployeesForDigitalSignatureUser',event)");
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id;
            Aspose.Pdf.Kit.Certificate cert;

            id = string.Empty;
            string contentType = FileUpload1.PostedFile.ContentType;
            if (ddl_SignType.SelectedValue == "N")
            {
                if (contentType == "application/x-pkcs12")
                {
                    // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                    // {
                    // using (SqlCommand cmd = new SqlCommand("insertDigitalSignature", con))
                    //{
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("DigitalSignature_ContactID", txtEmpName_hidden.Text);
                    //cmd.Parameters.AddWithValue("DigitalSignature_ValidFrom", dtFrom.Value);
                    //cmd.Parameters.AddWithValue("DigitalSignature_ValidUntil", dtTo.Value);
                    //cmd.Parameters.AddWithValue("DigitalSignature_AuthorizedUsers", hiddenEmployee.Value);
                    //cmd.Parameters.AddWithValue("DigitalSignature_CreateUser", Session["userid"]);//Session["userid"]
                    //cmd.Parameters.AddWithValue("DigitalSignature_Password", txtPass.Text);
                    //cmd.Parameters.AddWithValue("DigitalSignature_Type", "N");
                    //cmd.Parameters.AddWithValue("DigitalSignature_Name", "NA");


                    //if (con.State != ConnectionState.Open)
                    //    con.Open();

                    //SqlTransaction trx = con.BeginTransaction();
                    //cmd.Transaction = trx;

                    try
                    {
                        // cmd.CommandTimeout = 0;

                        // id = Convert.ToString(cmd.ExecuteScalar());
                        id = Convert.ToString(globalsetting.insertDigitalSignature(txtEmpName_hidden.Text.ToString(), dtFrom.Value.ToString(), dtTo.Value.ToString(),
                            hiddenEmployee.Value.ToString(), Session["userid"].ToString(), txtPass.Text.ToString(), "N", "NA"));
                        if (id != string.Empty)
                        {
                            if (id.Substring(0, 1) != "Y")
                            {


                                string path = Server.MapPath(@"..\Documents\DigitalSignature\") + id + ".pfx";
                                FileUpload1.SaveAs(path);

                                cert = new Aspose.Pdf.Kit.Certificate(path, txtPass.Text);
                                cert = null;
                                GC.Collect();

                                //trx.Commit();

                                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Successfully Uploaded.');", true);
                                clearValues();
                            }

                            else
                            {
                                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Signature already exists upto: " + id.Substring(1) + " ');", true);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // trx.Rollback();
                        cert = null;
                        GC.Collect();
                        try
                        {
                            if (File.Exists(Server.MapPath(@"..\Documents\DigitalSignature\") + id + ".pfx"))
                            {
                                File.Delete(Server.MapPath(@"..\Documents\DigitalSignature\") + id + ".pfx");
                            }
                        }
                        catch { }

                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('" + ex.Message + "');", true);

                    }
                    finally
                    {
                        //con.Close();
                    }
                    //}
                    //}
                }
                else
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Upload .pfx File.');", true);


                }
            }
            else
            {
                // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{
                //using (SqlCommand cmd = new SqlCommand("insertDigitalSignature", con))
                // {
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("DigitalSignature_ContactID", txtEmpName_hidden.Text);
                //cmd.Parameters.AddWithValue("DigitalSignature_ValidFrom", dtFrom.Value);
                //cmd.Parameters.AddWithValue("DigitalSignature_ValidUntil", dtTo.Value);
                //cmd.Parameters.AddWithValue("DigitalSignature_AuthorizedUsers", hiddenEmployee.Value);
                //cmd.Parameters.AddWithValue("DigitalSignature_CreateUser", Session["userid"]);//Session["userid"]
                //cmd.Parameters.AddWithValue("DigitalSignature_Password", DBNull.Value);
                //cmd.Parameters.AddWithValue("DigitalSignature_Type", ddl_SignType.SelectedValue);
                //cmd.Parameters.AddWithValue("DigitalSignature_Name", TxtSignName.Text);
                //if (con.State != ConnectionState.Open)
                //    con.Open();
                //cmd.CommandTimeout = 0;
                // id = Convert.ToString(cmd.ExecuteScalar());

                id = Convert.ToString(globalsetting.insertDigitalSignature(txtEmpName_hidden.Text.ToString(), dtFrom.Value.ToString(), dtTo.Value.ToString(),
                                  hiddenEmployee.Value.ToString(), Session["userid"].ToString(), DBNull.Value.ToString(), ddl_SignType.SelectedValue.ToString(), TxtSignName.Text.ToString()));
                if (id != string.Empty)
                {
                    if (id.Substring(0, 1) != "Y")
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "S", "alert('Successfully Inserted.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "UN", "alert('This Combination Already Exists.');", true);
                        ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Something UnExpected Happened.Please Retry!!!');", true);
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                }

                // }
                //}
            }


        }


        void clearValues()
        {
            txtEmpName_hidden.Text = "";
            txtEmpName.Text = "";
            dtFrom.Text = "";
            dtTo.Text = "";
            txtPass.Text = "";
            txtRePass.Text = "";
            hiddenEmployee.Value = "";
        }
    }
}