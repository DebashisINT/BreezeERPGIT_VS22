using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class frmLanguageProfi : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        string AllUserCntId;
        string InternalId;
        string str = "";
        SqlConnection oSqlConnection = new SqlConnection();
        DataTable DT = new DataTable();
        DataTable DTAvailable = new DataTable();
        DataTable DTChoosen = new DataTable();
        //SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
       // SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        IndustryMap_BL obj = new IndustryMap_BL();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Check Payroll Active Or Not

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable ConfigDT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='IsPayrollActive' AND IsActive=1");
                if (ConfigDT != null && ConfigDT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(ConfigDT.Rows[0]["Variable_Value"]).Trim();
                    if (IsMandatory == "Yes")
                    {
                        ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = true;
                        ASPxPageControl1.TabPages.FindByName("PFESI").Visible = true;
                        ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                    }
                    else
                    {
                        ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = false;
                        ASPxPageControl1.TabPages.FindByName("PFESI").Visible = false;
                        ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                    }
                }
                else
                {
                    ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = false;
                    ASPxPageControl1.TabPages.FindByName("PFESI").Visible = false;
                    ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                }

                #endregion


                SqlBank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                BankDetails.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                ViewState["GoBackTo"] = Request.UrlReferrer;
                if (HttpContext.Current.Session["KeyVal_InternalID"] != null)
                {
                    InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();
                    string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + InternalId + "'", 1);
                    if (EmployeeNameID[0, 0] != "n")
                    {
                        lblHeader.Text = EmployeeNameID[0, 0].ToString();
                    }
                }
                BindLanguages();
                LanGuage();
            }
        }
        public void LanGuage()
        {
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";        
            string[,] ListlngId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_speakLanguage,cnt_writeLanguage", "cnt_internalId='" + InternalId + "'", 2);
            string speak = ListlngId[0, 0];
          string  SpLanguage = speak;
            string write = ListlngId[0, 1];
            string WLanguage = write;
            if (speak != "")
            {
                string spk = "";
                string[] st = speak.Split(',');
                for (int i = 0; i <= st.GetUpperBound(0); i++)
                {
                    string[,] ListlngId1 = oDBEngine.GetFieldValue("tbl_master_language", "lng_language", "lng_id= '" + st[i] + "'", 1);
                    string Id = ListlngId1[0, 0];
                    spk += Id + ", ";
                }
                int spklng = spk.LastIndexOf(',');
                spk = spk.Substring(0, spklng);
                lblspeaklanguages.Text = spk;
            }
            if (write != "")
            {
                string wrt = "";
                string[] wrte = write.Split(',');
                for (int i = 0; i <= wrte.GetUpperBound(0); i++)
                {
                    string[,] ListlngId1 = oDBEngine.GetFieldValue("tbl_master_language", "lng_language", "lng_id= '" + wrte[i] + "'", 1);
                    string Id = ListlngId1[0, 0];
                    wrt += Id + ",";
                }
                int wrtlng = wrt.LastIndexOf(',');
                wrt = wrt.Substring(0, wrtlng);
                LitWrittenLanguage.Text = wrt;
            }

        }


        public void BindLanguages()
        {
            //-----------------For Speak
            //if (Request.QueryString["status"].ToString() == "speak")
            //{
            //    DataSet CntId = oDBEngine.PopulateData("cnt_speakLanguage", "tbl_master_contact", " cnt_internalId='" + InternalId + "' and cnt_speakLanguage is not null");

            //    if (CntId.Tables[0].Rows.Count > 0 && CntId.Tables[0].Rows[0][0].ToString() != "")
            //    {

            //        string chk = "";
            //        chk = CntId.Tables[0].Rows[0][0].ToString();
            //        string[] st = chk.Split(',');
            //        if (st.Length > 0)
            //        {
            //            for (int i = 0; i < st.Length; i++)
            //            {
            //                if (str.Length > 0)
            //                {
            //                    str += "," + st[i];
            //                }
            //                else
            //                {
            //                    str += st[i];
            //                }


            //            }

            //            DTAvailable = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "  tbl_master_language.lng_id not in(" + str + ") ");
            //            lbAvailable.DataSource = DTAvailable;
            //            lbAvailable.ValueField = "Id";
            //            lbAvailable.TextField = "Name";

            //            lbAvailable.DataBind();



            //            DTChoosen = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "  tbl_master_language.lng_id in(" + str + ") ");
            //            lbChoosen.DataSource = DTChoosen;
            //            lbChoosen.ValueField = "Id";
            //            lbChoosen.TextField = "Name";

            //            lbChoosen.DataBind();
            //        }

            //    }
            //    else
            //    {

            //        DTAvailable = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "lng_language is not null");
            //        lbAvailable.DataSource = DTAvailable;
            //        lbAvailable.ValueField = "Id";
            //        lbAvailable.TextField = "Name";

            //        lbAvailable.DataBind();


            //    }
            //}
            ////------------For Write
            //else
            //{
            //    DataSet CntId = oDBEngine.PopulateData("cnt_writeLanguage", "tbl_master_contact", " cnt_internalId='" + InternalId + "' and cnt_writeLanguage is not null");
            //    if (CntId.Tables[0].Rows.Count > 0 && CntId.Tables[0].Rows[0][0].ToString() != "")
            //    {

            //        string chk = "";
            //        chk = CntId.Tables[0].Rows[0][0].ToString();
            //        string[] st = chk.Split(',');
            //        if (st.Length > 0)
            //        {
            //            for (int i = 0; i < st.Length; i++)
            //            {
            //                if (str.Length > 0)
            //                {
            //                    str += "," + st[i];
            //                }
            //                else
            //                {
            //                    str += st[i];
            //                }


            //            }

            //            DTAvailable = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "  tbl_master_language.lng_id not in(" + str + ") ");
            //            lbAvailable.DataSource = DTAvailable;
            //            lbAvailable.ValueField = "Id";
            //            lbAvailable.TextField = "Name";

            //            lbAvailable.DataBind();



            //            DTChoosen = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "  tbl_master_language.lng_id in(" + str + ") ");
            //            lbChoosen.DataSource = DTChoosen;
            //            lbChoosen.ValueField = "Id";
            //            lbChoosen.TextField = "Name";

            //            lbChoosen.DataBind();
            //        }


            //    }
            //    else
            //    {

            //        DTAvailable = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "lng_language is not null");
            //        lbAvailable.DataSource = DTAvailable;
            //        lbAvailable.ValueField = "Id";
            //        lbAvailable.TextField = "Name";

            //        lbAvailable.DataBind();


            //    }
            //}




            DataSet CntId = oDBEngine.PopulateData("cnt_speakLanguage", "tbl_master_contact", " cnt_internalId='" + InternalId + "' and cnt_speakLanguage is not null");

            if (CntId.Tables[0].Rows.Count > 0 && CntId.Tables[0].Rows[0][0].ToString() != "")
            {

                string chk = "";
                chk = CntId.Tables[0].Rows[0][0].ToString();
                string[] st = chk.Split(',');
                if (st.Length > 0)
                {
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (str.Length > 0)
                        {
                            str += "," + st[i];
                        }
                        else
                        {
                            str += st[i];
                        }


                    }

                    DTAvailable = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "  tbl_master_language.lng_id >0 ");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();

                   
                        for (int i = 0; i < st.Length; i++)
                        {
                            foreach (ListEditItem item in ((ASPxListBox)lbAvailable).Items)
                            {
                                if (item.Value.ToString() == st[i].ToString())
                                {
                                    item.Selected = true;

                                }
                                else
                                {
                                   //tem.Selected = false;
                                }

                            }
                        }
                    



              
                }

            }
            else
            {

                DTAvailable = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "lng_language is not null");
                lbAvailable.DataSource = DTAvailable;
                lbAvailable.ValueField = "Id";
                lbAvailable.TextField = "Name";

                lbAvailable.DataBind();


            }



            DataSet CntIdwrite = oDBEngine.PopulateData("cnt_writeLanguage", "tbl_master_contact", " cnt_internalId='" + InternalId + "' and cnt_writeLanguage is not null");
            if (CntIdwrite.Tables[0].Rows.Count > 0 && CntIdwrite.Tables[0].Rows[0][0].ToString() != "")
            {

                string chk = "";
                chk = CntIdwrite.Tables[0].Rows[0][0].ToString();
                string[] st = chk.Split(',');
                if (st.Length > 0)
                {
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (str.Length > 0)
                        {
                            str += "," + st[i];
                        }
                        else
                        {
                            str += st[i];
                        }


                    }

                    


                    DTChoosen = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "  tbl_master_language.lng_id is not null ");
                    lbChoosen.DataSource = DTChoosen;
                    lbChoosen.ValueField = "Id";
                    lbChoosen.TextField = "Name";

                    lbChoosen.DataBind();


                   
                        for (int i = 0; i < st.Length; i++)
                        {

                            foreach (ListEditItem item in ((ASPxListBox)lbChoosen).Items)
                            {

                                if (item.Value.ToString() == st[i].ToString())
                                {
                                    item.Selected = true;

                                }
                                else
                                {
                              //      item.Selected = false;
                                }

                            }
                        }
                    


                }


            }
            else
            {

                DTAvailable = oDBEngine.GetDataTable("tbl_master_language", " ISNULL(lng_language, '')  AS Name, lng_id as Id    ", "lng_language is not null");
                lbChoosen.DataSource = DTAvailable;
                lbChoosen.ValueField = "Id";
                lbChoosen.TextField = "Name";

                lbChoosen.DataBind();


            }




        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            //string chk = Request["chk"].ToString();
            //string str = getLanguage(chk);


            string EntityIds = string.Empty;
            string variable = string.Empty;

            string variablewrite = string.Empty;
            string EntityIdswrite = string.Empty;

            foreach (ListEditItem item in ((ASPxListBox)lbAvailable).Items)
            {
                if (item.Selected == true)
                {
                    variable = item.Value.ToString() + ',' + variable;
                }
            }


            foreach (ListEditItem item in ((ASPxListBox)lbChoosen).Items)
            {
                if (item.Selected == true)
                {

                    variablewrite = item.Value.ToString() + ',' + variablewrite;

                }
            }



            if (variable.Length > 0)
            {
                EntityIds = variable.Substring(0, variable.Length - 1);
            }


            if (variablewrite.Length > 0)
            {
                EntityIdswrite = variablewrite.Substring(0, variablewrite.Length - 1);
            }



            //string popUpscript = "";
            //string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";
            //if (Request.QueryString["status"].ToString() == "speak")
            //{
            //    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_speakLanguage='" + EntityIds + "'", " cnt_internalId='" + InternalId + "'");
            //    popUpscript = "<script language='javascript'>";
            //    popUpscript += "window.opener.FillValues('" + str + "');window.close();</script>";
            //}
            //else
            //{
            //    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_writeLanguage='" + EntityIds + "'", " cnt_internalId='" + InternalId + "'");
            //    popUpscript = "<script language='javascript'>";
            //    popUpscript += "window.opener.FillValues1('" + str + "');window.close();</script>";
            //}


            string popUpscript = "";
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";
            //if (Request.QueryString["status"].ToString() == "speak")
            //{

                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_speakLanguage='" + EntityIds + "'", " cnt_internalId='" + InternalId + "'");
              
            //}
            //else
            //{
                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_writeLanguage='" + EntityIdswrite + "'", " cnt_internalId='" + InternalId + "'");
        
            //}





            ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
            Response.Redirect(ViewState["GoBackTo"].ToString());
            try
            {
                if (ViewState["GoBackTo"] != null)
                {
                    Response.Redirect(ViewState["GoBackTo"].ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btncancel_click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["GoBackTo"] != null)
                {
                    Response.Redirect(ViewState["GoBackTo"].ToString(), false);
                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void goBackCrossBtn_Click(object sender, EventArgs e)
        {

            try
            {
                if (ViewState["GoBackTo"] != null)
                {
                    Response.Redirect(ViewState["GoBackTo"].ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}