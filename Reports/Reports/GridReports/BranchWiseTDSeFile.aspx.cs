using DevExpress.Web;
using DevExpress.Web.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace Reports.Reports.GridReports
{
    public partial class BranchWiseTDSeFile : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        CommonBL cbl = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["SI_ComponentData_Branch"] = null;
                BranchHoOffice();
                GetAssesmentYear();
            }
            if (IsPostBack)
            {
                BranchHoOffice();
                GetAssesmentYear();
            }
            reqMesg.Visible = false;
        }

        public void GetAssesmentYear()
        {
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("PRC_BRANCHWISEGETASSESSMENTYEARVALUE_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ASSESSMENTVALUE", "");
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlAssesYR.DataSource = ds;
                ddlAssesYR.DataTextField = "Assessment_Year";
                ddlAssesYR.DataValueField = "ASSESSMENT_VALUE";
                ddlAssesYR.DataBind();

                Session["Assessment"] = Convert.ToString(ds.Tables[0].Rows[0]["Assessment_Year"]);

                txt_finyr.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinYear"]);
                Session["Finvalue"] = Convert.ToString(ds.Tables[0].Rows[0]["FinYear"]);

                hdnAssessmentValue.Value = Convert.ToString(ds.Tables[0].Rows[0]["ASSESSMENT_VALUE"]);
                hdnFinValue.Value = Convert.ToString(ds.Tables[0].Rows[0]["FIN_VALUE"]);
            }
            cmd.Dispose();
            con.Dispose();
        }

        [WebMethod]
        public static object ddlAssesYR_SelectedIndexChanged(string ddlAssesYR)
        {
            string output = "";
            try
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_BRANCHWISEGETASSESSMENTYEARVALUE_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ASSESSMENTVALUE", ddlAssesYR);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    output = reader["FinYear"].ToString() + "~" + reader["FIN_VALUE"].ToString();
                }


                con.Close();
                con.Dispose();
            }
            catch (Exception s)
            {

            }

            return output;
        }

        protected void GenerateFile()
        {
            DataSet dsInst = new DataSet();

            string BRANCH_ID = "";

            if(Convert.ToString(lookup_branch.Value)!=null)
            {
                BRANCH_ID = Convert.ToString(lookup_branch.Value);
            }

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_BRANCHWISETDSeFILEGENERATION", con);
            cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(System.Web.HttpContext.Current.Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@FORMNO", ddl_FormNo.Value);
            cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
            cmd.Parameters.AddWithValue("@STATEMENTTYPE", statementType.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@ASSESMENTYEAR", hdnAssessmentValue.Value);
            cmd.Parameters.AddWithValue("@FINYEAR", hdnFinValue.Value);
            cmd.Parameters.AddWithValue("@QUATER", ddl_QuaterType.Value);
            cmd.Parameters.AddWithValue("@EARLIERPERIOD", rdl_SaleInvoice.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@TOKENNO", txt_tokenNo.Value);
            cmd.Parameters.AddWithValue("@MINORHEADCHALLAN", ddl_HeadChallan.Value);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();

            try
            {
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
            }
            catch (Exception e)
            {

            }

            cmd.Dispose();
            con.Dispose();

            var dir = Server.MapPath("~\\assests");
            var file = Path.Combine(dir, ddl_FormNo.Value + "R" + ddl_QuaterType.Value + ".txt");

            if (!File.Exists(file))
            {
                var myFile = File.Create(file);
                myFile.Close();
            }

            if (dsInst.Tables.Count > 2)
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    foreach (DataRow row in dsInst.Tables[0].Rows)
                    {
                        string FirstStr = "";

                        for (int i = 0; i < dsInst.Tables[0].Columns.Count - 1; i++)
                        {
                            if (i != dsInst.Tables[0].Columns.Count - 1)
                                FirstStr = FirstStr + Convert.ToString(row[i]).Trim() + "^";
                            else
                                FirstStr = FirstStr + Convert.ToString(row[i]).Trim();
                        }
                        sw.Write(FirstStr);
                        sw.WriteLine();
                    }

                    foreach (DataRow row in dsInst.Tables[1].Rows)
                    {
                        string SecondStr = "";

                        for (int i = 0; i < dsInst.Tables[1].Columns.Count - 1; i++)
                        {
                            if (i != dsInst.Tables[1].Columns.Count - 1)
                                SecondStr = SecondStr + Convert.ToString(row[i]).Trim() + "^";
                            else
                                SecondStr = SecondStr + Convert.ToString(row[i]).Trim();
                        }

                        sw.Write(SecondStr);
                        sw.WriteLine();
                    }

                    for (int J = 2; J < dsInst.Tables.Count; J++)
                    {
                        foreach (DataRow row in dsInst.Tables[J].Rows)
                        {
                            string LastStr = "";

                            for (int i = 0; i < dsInst.Tables[J].Columns.Count - 1; i++)
                            {
                                if (i != dsInst.Tables[J].Columns.Count - 1)
                                    LastStr = LastStr + Convert.ToString(row[i]).Trim() + "^";
                                else
                                    LastStr = LastStr + Convert.ToString(row[i]).Trim();
                            }
                            sw.Write(LastStr);
                            sw.WriteLine();
                        }

                    }
                }
                DownLoad(file);
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                if ((Session["Assessment"] != null) && (Session["Finvalue"] != null))
                {
                    SqlConnection con1 = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    DataSet ds = new DataSet();
                    SqlCommand comd = new SqlCommand("PRC_BRANCHWISEGETASSESSMENTYEARVALUE_REPORT", con1);
                    comd.CommandType = CommandType.StoredProcedure;
                    comd.Parameters.AddWithValue("@ASSESSMENTVALUE", "");
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = comd;
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlAssesYR.DataSource = ds;
                        ddlAssesYR.DataTextField = "Assessment_Year";
                        ddlAssesYR.DataValueField = "ASSESSMENT_VALUE";
                        ddlAssesYR.DataBind();
                    }
                    ddlAssesYR.Text = hdnAssessmentValue.Value;
                    txt_finyr.Text = hdnFinValue.Value;

                    comd.Dispose();
                    con1.Dispose();
                }
            }
        }

        public void DownLoad(string FName)
        {
            string path = FName;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "text/plain";
                response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name + ";");
                response.TransmitFile(path);
                response.Flush();
                response.End();
            }
        }

        protected void lnbGenerateFile_Click(object sender, EventArgs e)
        {
            if (lookup_branch.Value == null)
            {
                reqMesg.Visible = true;
                return;
            }
            reqMesg.Visible = false;
            GenerateFile();
        }

        #region Branch Populate

        public void BranchHoOffice()
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();
            DataTable dtBranchChild = new DataTable();
            tcbl = cbl.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (tcbl.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = tcbl;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
            }
        }

        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if(Hoid=="null")
                {
                    BranchHoOffice();
                    Hoid = "All";
                }
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                }
            }
        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion
    }
}