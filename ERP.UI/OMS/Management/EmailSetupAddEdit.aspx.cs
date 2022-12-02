using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FreeTextBoxControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_EmailSetupAddEdit : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        Utilities oUtilities = new Utilities();
        clsDropDownList oclsDropDownList = new clsDropDownList();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtOPort.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtIPort.Attributes.Add("onKeypress", "return MaskMoney(event)");
            //btnSave.Attributes.Add("Onclick", "Javascript:return ValidatePage();");
            FreeTextBox FreeTextBox1 = new FreeTextBox();
            FreeTextBox1.ID = "FreeTextBox1";
            FreeTextBox1.Height = 200;
            FreeTextBox1.Width = 705;
            FreeTextBoxPlaceHolder.Controls.Add(FreeTextBox1);
            if (!Page.IsPostBack)
            {

                ShowForm();
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 15122016

            FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("FreeTextBox1") as FreeTextBox;
            string disclaimer =  Convert.ToString(textEditor.Text).Replace("'", "''");
            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                DataTable dtcom = oDBEngine.GetDataTable("tbl_master_company ", " * ", " cmp_id='" + Convert.ToString(cmbOrganization.SelectedItem.Value).Trim() + "'");
                string cmpid = Convert.ToString(dtcom.Rows[0]["cmp_internalid"]);

                //DBEngine oDEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                //BusinessLogicLayer.DBEngine oDEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDEngine = new BusinessLogicLayer.DBEngine();
                //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //SqlConnection lcon = new SqlConnection(con);
                //lcon.Open();
                //SqlCommand lcmdEmplInsert = new SqlCommand("sp_InsertEmailSetup", lcon);
                //lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_CompanyID", cmpid);
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_SegmentID", cmbSegment.SelectedItem.Value);
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_EmailID", txtRUserName.Text.ToString().Trim());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_UsedFor", cmbType.SelectedItem.Value);
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_Password", txtRPassword.Text.ToString().Trim());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_SMTP", txtOHost.Text.ToString().Trim());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_SMTPPort", txtOPort.Text.ToString().Trim());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_POP", txtIHost.Text.ToString().Trim());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_POPPort", txtIPort.Text.ToString().Trim());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_ReplyToAccount", txtReplyTo.Text.ToString().Trim());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_Disclaimer", disclaimer.ToString().Trim());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_InUse", cmbStatus.SelectedItem.Value);
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_CreateUser", HttpContext.Current.Session["userid"]);
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_SSLMode", cmbSSL.SelectedItem.Value);
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_FromName", txtFromName.Text.ToString());
                //lcmdEmplInsert.Parameters.AddWithValue("@EmailAccounts_ReplyToName", txtReplyToName.Text.ToString());
                //lcmdEmplInsert.ExecuteNonQuery();

                oUtilities.InsertEmailSetup(cmpid, Convert.ToString(cmbSegment.SelectedItem.Value), Convert.ToString(txtRUserName.Text).Trim(), cmbType.SelectedItem.Value, Convert.ToString(txtRPassword.Text).Trim(),
                   Convert.ToString(txtOHost.Text).Trim(), Convert.ToString(txtOPort.Text).Trim(), Convert.ToString(txtIHost.Text).Trim(), Convert.ToString(txtIPort.Text).Trim(), Convert.ToString(txtReplyTo.Text).Trim(), Convert.ToString(disclaimer).Trim(),
                    cmbStatus.SelectedItem.Value, Convert.ToString(HttpContext.Current.Session["userid"]), cmbSSL.SelectedItem.Value, Convert.ToString(txtFromName.Text), Convert.ToString(txtReplyToName.Text));

                //Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>parent.editwin.close();</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "Success", "alert('Inserted Successfully');window.location='../management/SettingsOptions/EmailSetup.aspx';", true);
            }
            else
            {
                DataTable dtCom = oDBEngine.GetDataTable("tbl_master_company", " * ", "cmp_id='" + cmbOrganization.SelectedItem.Value + "'");
                //DataTable dtEx = oDBEngine.GetDataTable("Config_EmailAccounts", "*", " EmailAccounts_CompanyID='" + dtCom.Rows[0]["cmp_internalid"].ToString().Trim() + "' and EmailAccounts_SegmentID ='" + cmbSegment.SelectedItem.Value + "' and EmailAccounts_UsedFor='" + cmbType.SelectedItem.Value + "'");
                //if (dtEx.Rows.Count > 0)
                //{
                //    oDBEngine.SetFieldValue("Config_EmailAccounts", " EmailAccounts_InUse='N' ", " EmailAccounts_CompanyID='" + dtCom.Rows[0]["cmp_internalid"].ToString().Trim() + "' and EmailAccounts_SegmentID ='" + cmbSegment.SelectedItem.Value + "' and EmailAccounts_UsedFor='" + cmbType.SelectedItem.Value + "'");
                //}

                oDBEngine.SetFieldValue("Config_EmailAccounts", " EmailAccounts_CompanyID= '" + Convert.ToString(dtCom.Rows[0]["cmp_internalid"]).Trim() + "',EmailAccounts_SegmentID='" + cmbSegment.SelectedItem.Value + "',EmailAccounts_EmailID='" + Convert.ToString(txtRUserName.Text).Trim() + "',EmailAccounts_UsedFor='" + cmbType.SelectedItem.Value + "',EmailAccounts_Password='" + Convert.ToString(txtRPassword.Text).Trim() + "',EmailAccounts_SMTP='" + Convert.ToString(txtOHost.Text).Trim() + "',EmailAccounts_SMTPPort='" + Convert.ToString(txtOPort.Text).Trim() + "',EmailAccounts_POP='" + Convert.ToString(txtIHost.Text).Trim() + "',EmailAccounts_POPPort='" + Convert.ToString(txtIPort.Text).Trim() + "',EmailAccounts_ReplyToAccount='" + Convert.ToString(txtReplyTo.Text).Trim() + "',EmailAccounts_Disclaimer='" + Convert.ToString(disclaimer).Trim() + "',EmailAccounts_InUse='" + cmbStatus.SelectedItem.Value + "',EmailAccounts_ModifyUser='" + Convert.ToString(HttpContext.Current.Session["userid"]) + "',EmailAccounts_ModifyDateTime=getdate(),EmailAccounts_SSLMode='" + cmbSSL.SelectedItem.Value + "' ,EmailAccounts_FromName='" + txtFromName.Text + "',EmailAccounts_ReplyToName='" + Convert.ToString(txtReplyToName.Text) + "'", " EmailAccounts_ID=" + Convert.ToString(Request.QueryString["id"]) + "");

                //Page.ClientScript.RegisterStartupScript(GetType(), "pagecalldd", "<script>parent.editwin.close();</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "Success", "alert('Updated Successfully');window.location='../management/SettingsOptions/EmailSetup.aspx';", true);
            }


        }
        private void ShowForm()
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 15122016

            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                string[,] Data = oDBEngine.GetFieldValue("tbl_master_company ", "cmp_id,cmp_name", null, 2, "cmp_name");
                //oDBEngine.AddDataToDropDownList(Data, cmbOrganization);
                oclsDropDownList.AddDataToDropDownList(Data, cmbOrganization);
                Data = oDBEngine.GetFieldValue("tbl_master_segment", "seg_id, seg_name", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 2, "seg_name");
                //oDBEngine.AddDataToDropDownList(Data, cmbSegment);
                oclsDropDownList.AddDataToDropDownList(Data, cmbSegment);
                cmbOrganization.Items.Insert(0, new ListItem("--Select--", "0"));

                //Commented and added by:Subhabrata
                //cmbSegment.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbSegment.Items.FindByValue(HttpContext.Current.Session["userlastsegment"].ToString()).Selected = true;
                //End

                
            }
            else
            {
                if (Request.QueryString["id"] != null)
                {
                    string[,] DT = oDBEngine.GetFieldValue("config_emailAccounts", "EmailAccounts_ID,(select cmp_name from tbl_master_company where cmp_internalid=EmailAccounts_CompanyID) as Company, (select cmp_id from tbl_master_company where cmp_internalid=EmailAccounts_CompanyID) as ComanyID,(select seg_name from tbl_master_segment where seg_id= EmailAccounts_SegmentID) as Segment,EmailAccounts_SegmentID as SegID,EmailAccounts_EmailID,Case when EmailAccounts_UsedFor='N' then 'Normal' when EmailAccounts_UsedFor='E' then 'ECN Email' when EmailAccounts_UsedFor='B' then 'Bulk Email'  end  as EmailType ,EmailAccounts_UsedFor,EmailAccounts_Password,EmailAccounts_SMTP,EmailAccounts_SMTPPort,EmailAccounts_POP,EmailAccounts_POPPort,EmailAccounts_ReplyToAccount,EmailAccounts_Disclaimer, EmailAccounts_InUse as ActiveInd,EmailAccounts_CreateUser,EmailAccounts_CreateDateTime,EmailAccounts_ModifyUser,EmailAccounts_ModifyDateTime,EmailAccounts_SSLMode,EmailAccounts_FromName,EmailAccounts_ReplyToName  ", "EmailAccounts_ID=" + Request.QueryString["id"], 23);
                    if (DT[0, 0] != "n")
                    {

                        string[,] Data = oDBEngine.GetFieldValue("tbl_master_segment", "seg_id, seg_name", null, 2, "seg_name");
                        if (DT[0, 4] != "")
                        {
                            //oDBEngine.AddDataToDropDownList(Data, cmbSegment, int.Parse(DT[0, 4].ToString()));
                            oclsDropDownList.AddDataToDropDownList(Data, cmbSegment, Convert.ToString(int.Parse(DT[0, 4])));
                        }
                        else
                        {
                            //oDBEngine.AddDataToDropDownList(Data, cmbSegment, 0);
                            oclsDropDownList.AddDataToDropDownList(Data, cmbSegment, 0);
                        }


                        Data = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_name", null, 2, "cmp_name");
                        if (DT[0, 2] != "")
                        {
                            //oDBEngine.AddDataToDropDownList(Data, cmbOrganization, int.Parse(DT[0, 2].ToString()));
                            oclsDropDownList.AddDataToDropDownList(Data, cmbOrganization, Convert.ToString(int.Parse(DT[0, 2])));
                        }
                        else
                        {
                            //oDBEngine.AddDataToDropDownList(Data, cmbOrganization, 0);
                            oclsDropDownList.AddDataToDropDownList(Data, cmbOrganization, 0);
                        }


                        txtRUserName.Text = DT[0, 5];
                        cmbType.SelectedValue = DT[0, 7];
                        // txtRPassword.Text = DT[0, 8];

                        txtRPassword.Text = DT[0, 8];

                        txtRPassword.Attributes.Add("value", DT[0, 8]);


                        txtOHost.Text = DT[0, 9];
                        txtOPort.Text = DT[0, 10];
                        txtIHost.Text = DT[0, 11];
                        txtIPort.Text = DT[0, 12];
                        txtReplyTo.Text = DT[0, 13];

                        FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("FreeTextBox1") as FreeTextBox;
                        textEditor.Text = Server.HtmlDecode(Convert.ToString(DT[0, 14]));
                        cmbStatus.SelectedValue = DT[0, 15];
                        cmbSSL.SelectedValue = DT[0, 20];
                        txtFromName.Text = DT[0, 21];
                        txtReplyToName.Text = DT[0, 22];

                    }
                }
            }

        }
    }
}