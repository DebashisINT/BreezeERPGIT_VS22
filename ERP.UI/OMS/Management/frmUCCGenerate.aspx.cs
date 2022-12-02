using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frmUCCGenerate : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        int num;
        string[] s;
        Management_BL ObjManagement = new Management_BL();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ShowList()
        {
            string prefx = Request.QueryString["ucc"].ToString().ToUpper();

            /* For Tier Structure-----------
            */
            //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
            SqlCommand lcmdEmplInsert = new SqlCommand("GenerateUCC", lcon);
            lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
            lcmdEmplInsert.Parameters.AddWithValue("@prifixIdN", prefx);
            SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 50);
            parameter.Direction = ParameterDirection.Output;
            lcmdEmplInsert.Parameters.Add(parameter);
            lcmdEmplInsert.ExecuteNonQuery();
            // Mantis Issue 24802
            if (lcon.State == ConnectionState.Open)
            {
                lcon.Close();
            }
            // End of Mantis Issue 24802

            string InternalID = parameter.Value.ToString();




            //string InternalID =  ObjManagement.GenerateUCC(prefx);

            if (InternalID == "")
            {
                num = 0;
            }
            else
            {
                // Response.Write(InternalID);
                //num = Convert.ToInt32(InternalID.Substring(1, 3).ToString());
                s = InternalID.ToString().Split('~');
                if (s[1] == "")
                {
                    num = 0;
                }
                else
                {
                    num = Convert.ToInt32(s[1]);
                }

            }
            string id = "";
            //num = num + 1;
            //if (num < 9)
            //{
            //    id = prefx + "00";
            //}
            //else if (num > 9 && num < 99)
            //{
            //    id = prefx + "0";
            //}
            //else if (num > 99 && num <= 999)
            //{
            //    id = prefx;
            //}
            //string secondUCC = id + num;
            //Response.Write(secondUCC);
            //num = num + 1;
            //if (num < 9)
            //{
            //    id = prefx + "00";
            //}
            //else if (num > 9 && num < 99)
            //{
            //    id = prefx + "0";
            //}
            //else if (num > 99 && num <= 999)
            //{
            //    id = prefx;
            //}
            //string lastUCC = id + num;
            //Response.Write(lastUCC);
            string str = "";
            string check_status = "";
            for (int i = 0; i < 3; i++)
            {

                num = num + 1;
                if (num < 9)
                {
                    id = s[0] + "00";
                }
                else if (num > 9 && num < 99)
                {
                    id = s[0] + "0";
                }
                else if (num > 99 && num <= 999)
                {
                    id = s[0];
                }
                else
                {
                    id = s[0] + num;
                }
                string lastUCC = id;
                check_status = Convert.ToBoolean(Language_Status(str, lastUCC)).ToString();
                if (Convert.ToBoolean(check_status) == false)
                {
                    check_status = "";
                }
                else
                {
                    check_status = "checked='CHECKED'";
                }
                Response.Write("<tr><td><input " + check_status + " type='checkbox' id='chk'  name='chk' value='" + lastUCC + "'  onclick='javascript:chkclicked(this);'></td><td>" + lastUCC + "</td></tr>");
            }
            Response.Write("</table>");

        }

        public bool Language_Status(string lng_collection, string lng)
        {
            bool status = false;
            string[] menus = null;
            if (lng_collection != null)
            {
                menus = lng_collection.Split(',');
                for (int j = 0; j <= menus.Length - 1; j++)
                {
                    if (menus[j] == lng)
                        status = true;
                }
            }
            return status;
        }
        //public string genrateUCC()
        //   {

        //       string chars = "0123456789";
        //       StringBuilder pass = new StringBuilder();
        //       string prefx = Request.QueryString["ucc"].ToString();
        //       int k = prefx.ToString().Length;
        //       int reql = 5 - k;
        //       Random ran = new Random();
        //       for (int i = 0; i < reql; i++)
        //       {
        //           pass.Append(chars[ran.Next(0, chars.Length)]);

        //       }
        //       string randno = prefx.ToString().ToUpper() + "" + pass.ToString();
        //       Response.Write(randno.ToString());
        //       return randno;
        //   }
        //public void UCCWrit()
        //{
        //    string randnum=genrateUCC();
        //    DataTable dtUCC = oDBEngine.GetDataTable("tbl_master_contact", " cnt_UCC", "cnt_UCC ='" + randnum + "'");
        //    if (dtUCC.Rows.Count == 0)
        //    {
        //        for (int i = 0; i < 3; i++)
        //        {
        //            randnum = genrateUCC();
        //        }
        //    }

        //}

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            string chk = Request["chk"].ToString();
            //string str = getLanguage(chk);
            string popUpscript = "";
            //string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";
            popUpscript = "<script language='javascript'>";
            popUpscript += "window.opener.FillUCCCode('" + chk + "');window.close();</script>";
            //if (Request.QueryString["status"].ToString() == "speak")
            //{
            //    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_speakLanguage='" + chk + "'", " cnt_internalId='" + InternalId + "'");
            //    popUpscript = "<script language='javascript'>";
            //    popUpscript += "window.opener.FillValues('" + str + "');window.close();</script>";
            //}
            //else
            //{
            //    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_writeLanguage='" + chk + "'", " cnt_internalId='" + InternalId + "'");
            //    popUpscript = "<script language='javascript'>";
            //    popUpscript += "window.opener.FillValues1('" + str + "');window.close();</script>";
            //}
            ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
        }

    }
}