using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using DevExpress.Web;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.XtraEditors;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_heads : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        Int32 ID;
        string createdate, modifydate, createuser, lastmodifyuser;
        DataTable DT = new DataTable();
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);multi
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.OtherMasters obj = new BusinessLogicLayer.OtherMasters();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowForm();
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
                    InternalId = oDBEngine.GetFieldValue("Master_OtherCharges",
                                             "OtherCharges_Name,OtherCharges_Code,OtherCharges_ChargeOn,OtherCharges_Frequency",
                                             "OtherCharges_ID=" + ID, 4);
                }
                else
                {
                    InternalId = oDBEngine.GetFieldValue("Master_OtherCharges",
                                   "OtherCharges_Name,OtherCharges_Code,OtherCharges_ChargeOn,OtherCharges_Frequency",
                                   "OtherCharges_ID=" + ID, 4);
                }

                txtdesc.Text = InternalId[0, 0];
                txtchargename.Text = InternalId[0, 1];
                ddlfreq.Value = InternalId[0, 3];
                ddlchargeon.Value = InternalId[0, 2];
                txtchargename.Enabled = false;
            }
            else
            {
                HttpContext.Current.Session["KeyVal"] = 0;
                ddlchargeon.Value = "Turnover";
                ddlfreq.Value = "Daily";

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {


            Int32 userid = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
            createuser = HttpContext.Current.Session["userid"].ToString();
            if (txtchargename.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1", "<script>alert('Charge Code is Required.')</script>");
                return;
            }
            //if (txtdesc.Text == "")
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript1", "<script>alert('Please enter .')</script>");
            //    return;
            //}
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
                String value = "OtherCharges_Name='" + txtdesc.Text + "',OtherCharges_ChargeOn='" + ddlchargeon.SelectedItem.Value + "',OtherCharges_Frequency='" + ddlfreq.SelectedItem.Value + "',OtherCharges_CreateUser=" + createuser + ",OtherCharges_ModifyUser=" + lastmodifyuser + ",OtherCharges_ModifyDateTime='" + modifydate + "'";
                string[,] CName = oDBEngine.GetFieldValue("Master_OtherCharges", "OtherCharges_Code", " OtherCharges_ID !=" + int.Parse(HttpContext.Current.Session["KeyVal"].ToString()), 1);

                for (int i = 0; i < CName.Length; i++)
                {

                    if (txtchargename.Text.Trim() == CName[i, 0].ToString().Trim())
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript1", "<script>alert('Charge Code Already Exists')</script>");
                        return;
                    }
                    else
                    {
                    }
                }


                Int32 rowsEffected = oDBEngine.SetFieldValue("Master_OtherCharges", value, " OtherCharges_ID='" + HttpContext.Current.Session["KeyVal"].ToString() + "'");
                // Response.Redirect("heads.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);

            }
            else
            {
                try
                {

                    createdate = oDBEngine.GetDate().ToString();
                    Session["cdate"] = createdate;

                    /* For Tier Structrure ---------------------------- */

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //using (SqlConnection lcon = new SqlConnection(con))
                    //{
                    //    lcon.Open();
                    //    using (SqlCommand lcmdChargehead = new SqlCommand("ChargeheadsInsert", lcon))
                    //    {
                    //        lcmdChargehead.CommandType = CommandType.StoredProcedure;

                    //        SqlParameter parameter = new SqlParameter("@ResultChargeName", SqlDbType.VarChar, 20);
                    //        parameter.Direction = ParameterDirection.Output;

                    //        lcmdChargehead.Parameters.AddWithValue("@OtherCharges_Code", txtchargename.Text.ToString());
                    //        if (txtdesc.Text != "")
                    //            lcmdChargehead.Parameters.AddWithValue("@OtherCharges_Name", txtdesc.Text.ToString());
                    //        else
                    //            lcmdChargehead.Parameters.AddWithValue("@OtherCharges_Name", " ");
                    //        lcmdChargehead.Parameters.AddWithValue("@OtherCharges_ChargeOn", ddlchargeon.SelectedItem.Value);
                    //        lcmdChargehead.Parameters.AddWithValue("@OtherCharges_Frequency", ddlfreq.SelectedItem.Value);

                    //        lcmdChargehead.Parameters.AddWithValue("@OtherCharges_CreateUser", createuser);
                    //        lcmdChargehead.Parameters.AddWithValue("@OtherCharges_CreateDateTime", createdate);
                    //        lcmdChargehead.Parameters.AddWithValue("@OtherCharges_ModifyUser", lastmodifyuser);
                    //        lcmdChargehead.Parameters.Add(parameter);
                    //        lcmdChargehead.ExecuteNonQuery();

                    //        string chname = parameter.Value.ToString();
                    //        if (chname == "0")
                    //            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Charge Code Already Exists')</script>");

                    //    }
                    //}
                    if (txtdesc.Text == "")
                    {
                        txtdesc.Text = " ";
                    }
                    string chname = obj.Insert_Chargeheads(txtchargename.Text.ToString(), txtdesc.Text.ToString(), ddlchargeon.SelectedItem.Value.ToString(),
                         ddlfreq.SelectedItem.Value.ToString(), createuser, createdate, lastmodifyuser);

                    if (chname == "0")
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Charge Code Already Exists')</script>");
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