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

namespace Reports.Reports.GridReports
{
    public partial class TDSeFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Rev Maynak 29-10-2019
            if (!IsPostBack)
            {
                GetAssesmentYear();

                //SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //DataSet ds = new DataSet();
                //SqlCommand cmd = new SqlCommand("PRC_GETASSESSMENTYEARVALUE_REPORT", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //SqlDataAdapter da = new SqlDataAdapter();
                //try
                //{
                //    da.SelectCommand = cmd;
                //    da.Fill(ds);
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        txt_assementyr.Text = Convert.ToString(ds.Tables[0].Rows[0]["Assessment_Year"]);
                //        txt_finyr.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinYear"]);
                //        hdnAssessmentValue.Value = Convert.ToString(ds.Tables[0].Rows[0]["ASSESSMENT_VALUE"]);
                //        hdnFinValue.Value = Convert.ToString(ds.Tables[0].Rows[0]["FIN_VALUE"]);
                //        Session["Assessment"] = Convert.ToString(ds.Tables[0].Rows[0]["Assessment_Year"]);
                //        Session["Finvalue"] = Convert.ToString(ds.Tables[0].Rows[0]["FinYear"]);
                //    }
                //}

                //catch (Exception ex)
                //{
                //    throw ex;
                //}
            }
            //End of Rev Maynak 29-10-2019 
        }

        public void GetAssesmentYear()
        {
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("PRC_GETASSESSMENTYEARVALUE_REPORT", con);
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
                SqlCommand cmd = new SqlCommand("PRC_GETASSESSMENTYEARVALUE_REPORT", con);
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
            

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_TDSeFILEGENERATION", con);
            //Rev Maynak 30-10-2019
            cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(System.Web.HttpContext.Current.Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@FORMNO", ddl_FormNo.Value);
            cmd.Parameters.AddWithValue("@STATEMENTTYPE", statementType.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@ASSESMENTYEAR", hdnAssessmentValue.Value);
            cmd.Parameters.AddWithValue("@FINYEAR", hdnFinValue.Value);
            cmd.Parameters.AddWithValue("@QUATER", ddl_QuaterType.Value);
            cmd.Parameters.AddWithValue("@EARLIERPERIOD", rdl_SaleInvoice.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@TOKENNO", txt_tokenNo.Value);
            cmd.Parameters.AddWithValue("@MINORHEADCHALLAN", ddl_HeadChallan.Value);
            // End of Rev Maynak 
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
            //Rev Debashis
            //var file = Path.Combine(dir, "26QRQ2.txt");
            var file = Path.Combine(dir, ddl_FormNo.Value+"R"+ddl_QuaterType.Value+".txt");
            //End of Rev Debashis

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



                    for (int J = 2; J < dsInst.Tables.Count ; J++)
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
                //Rev Maynak 01-11-2019 0017762
                DownLoad(file);
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                if ((Session["Assessment"] != null) && (Session["Finvalue"]!=null))
                {
                    //txt_assementyr.Text = Convert.ToString(Session["Assessment"]);
                    SqlConnection con1 = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    DataSet ds = new DataSet();
                    SqlCommand comd = new SqlCommand("PRC_GETASSESSMENTYEARVALUE_REPORT", con1);
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
                    //txt_finyr.Text = Convert.ToString(Session["Finvalue"]);
                    ddlAssesYR.Text = hdnAssessmentValue.Value;
                    txt_finyr.Text = hdnFinValue.Value;

                    comd.Dispose();
                    con1.Dispose();
                }                
            }

            //DownLoad(file);
            //End of Rev Maynak
        }


        public void DownLoad(string FName)
        {
            string path = FName;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {

                //Response.Clear();
                //Response.ContentType = "text/plain";
                //Response.AddHeader("Content-Disposition",
                //                   "attachment; filename=" + file.Name + ";");
                ////file.Response.ContentType = "application/octet-stream";
                //Response.ContentType = "application/octet-stream";
                //Response.AddHeader("Content-Length", file.Length.ToString());
                ////Response.TransmitFile(FName);
                //Response.Flush();
                //Response.WriteFile(file.FullName);
                //Response.End();


                //FileInfo fileInfo = new FileInfo("file_path_here");
                //Response.Clear();
                //Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
                //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                //Response.ContentType = "application/octet-stream";
                //Response.Flush();
                //Response.WriteFile(fileInfo.FullName);
                //Response.End();


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
            GenerateFile();
        }       
    }
}

