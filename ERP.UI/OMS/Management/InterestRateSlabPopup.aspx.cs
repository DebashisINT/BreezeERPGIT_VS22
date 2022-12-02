using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_InterestRateSlabPopup : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        Management_BL oManagement_BL = new Management_BL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowForm();

            }
        }
        void ShowForm()
        {
            txtmin.Text = "1";
            txtmax.Text = "9999999999999";
            txtmin.Enabled = false;
            txtrate.Text = "";
        }
        public void InsertIntSlab()
        {
            string min = txtmin.Text.ToString();
            string max = txtmax.Text.ToString();

            if (Convert.ToDecimal(txtmin.Text.ToString()) >= Convert.ToDecimal(txtmax.Text.ToString()))
                Page.ClientScript.RegisterStartupScript(GetType(), "testjs", "<script>alert('Maximum Range Should Be Greater Than Minimum Range')</script>");

            else
            {

                //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //SqlConnection lcon = new SqlConnection(con);
                //lcon.Open();
                //SqlCommand lcmdIntSlab = new SqlCommand("[Insert_IntSlab]", lcon);
                //lcmdIntSlab.CommandType = CommandType.StoredProcedure;

                //SqlParameter parameter = new SqlParameter("@ResultIntSlab", SqlDbType.VarChar, 20);
                //parameter.Direction = ParameterDirection.Output;
                //SqlParameter parameter1 = new SqlParameter("@IntSlabmaxRange", SqlDbType.Decimal);
                //parameter1.Direction = ParameterDirection.Output;

                //lcmdIntSlab.Parameters.AddWithValue("@IntSlab_Code", txtslabcode.Text.ToString());
                //lcmdIntSlab.Parameters.AddWithValue("@IntSlab_AmntFrom", txtmin.Text.ToString());
                //lcmdIntSlab.Parameters.AddWithValue("@IntSlab_AmntTo", txtmax.Text.ToString());

                //if (txtrate.Text.ToString().Trim() != "")
                //    lcmdIntSlab.Parameters.AddWithValue("@IntSlab_Rate", Convert.ToDecimal(txtrate.Text));
                //else
                //    lcmdIntSlab.Parameters.AddWithValue("@IntSlab_Rate", Convert.ToDecimal("0"));

                //lcmdIntSlab.Parameters.AddWithValue("@IntSlab_CreateUser", HttpContext.Current.Session["userid"].ToString().Trim());

                //lcmdIntSlab.Parameters.Add(parameter);
                //lcmdIntSlab.Parameters.Add(parameter1);
                //lcmdIntSlab.ExecuteNonQuery();
                decimal IntSlab_Rate = 0;
                string parameter = string.Empty;
                decimal parameter1 = 0;
                if (txtrate.Text.ToString().Trim() != "")
                    IntSlab_Rate = Convert.ToDecimal(txtrate.Text);
                else
                    IntSlab_Rate = Convert.ToDecimal("0");

                oManagement_BL.Insert_IntSlab(out parameter, out parameter1, txtslabcode.Text.ToString(), Convert.ToDecimal(txtmin.Text.ToString()),
                     Convert.ToDecimal(txtmax.Text.ToString()), IntSlab_Rate, Convert.ToInt32(HttpContext.Current.Session["userid"]));

                string slabname = parameter.ToString();
                string maxr = parameter1.ToString();

                txtmin.Text = Convert.ToString((Convert.ToDecimal(max)));
                txtmax.Text = "9999999999999";
                txtslabcode.Enabled = false;
                txtmin.Enabled = false;
                txtrate.Text = "";


                if (slabname == "1")
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Continue Insert Untill Max Range is 9999999999999')</script>");
                else if (slabname == "0")
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Already Exists');parent.editwin.close();</script>");
                else if (slabname == "2")
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Insertion Complete and Cannot able to Insert for This Code');parent.editwin.close();</script>");


            }


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            InsertIntSlab();
        }
    }
}