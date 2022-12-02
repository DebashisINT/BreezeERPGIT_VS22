using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_ajax_list : System.Web.UI.Page
    {

        string SegmentName = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
           if (Session["ExchangeSegmentID"] == null)
            {
                SegmentName = null;


            }
            else
            {
                if (Session["ExchangeSegmentID"].ToString() == "1")
                    SegmentName = "NSE - CM";
                else if (Session["ExchangeSegmentID"].ToString() == "2")
                    SegmentName = "NSE - FO";
                else if (Session["ExchangeSegmentID"].ToString() == "3")
                    SegmentName = "NSE - CDX";
                else if (Session["ExchangeSegmentID"].ToString() == "4")
                    SegmentName = "BSE - CM";
                else if (Session["ExchangeSegmentID"].ToString() == "7")
                    SegmentName = "MCX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "9")
                    SegmentName = "NCDEX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "5")
                    SegmentName = "BSE - FO";
                else if (Session["ExchangeSegmentID"].ToString() == "6")
                    SegmentName = "BSE - CDX";
                else if (Session["ExchangeSegmentID"].ToString() == "8")
                    SegmentName = "MCXSX - CDX";
                else if (Session["ExchangeSegmentID"].ToString() == "10")
                    SegmentName = "DGCX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "11")
                    SegmentName = "NMCE - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "12")
                    SegmentName = "ICEX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "15")
                    SegmentName = "CSE - CM";
                else if (Session["ExchangeSegmentID"].ToString() == "14")
                    SegmentName = "NSEL - SPOT";
                else if (Session["ExchangeSegmentID"].ToString() == "13")
                    SegmentName = "USE - CDX";
                else if (Session["ExchangeSegmentID"].ToString() == "16")
                    SegmentName = "Accounts";
                else if (Session["ExchangeSegmentID"].ToString() == "17")
                    SegmentName = "ACE - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "18")
                    SegmentName = "INMX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "19")
                    SegmentName = "MCXSX - CM";
                else if (Session["ExchangeSegmentID"].ToString() == "20")
                    SegmentName = "MCXSX - FO";
                else if (Session["ExchangeSegmentID"].ToString() == "21")
                    SegmentName = "BFX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "22")
                    SegmentName = "INSX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "23")
                    SegmentName = "INFX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "24")
                    SegmentName = "UCX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "25")
                    SegmentName = "INBX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "26")
                    SegmentName = "INAX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "27")
                    SegmentName = "INEX - COMM";

            }
            if (Session["userlastsegment"].ToString() == "9")
            {
                SegmentName = "NSDL";
            }
            else if (Session["userlastsegment"].ToString() == "10")
            {
                SegmentName = "CDSL";

            }
            else if (Session["userlastsegment"].ToString() == "5")
            {
                SegmentName = "Accounts";

            }


            DataTable DT = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationSSettings.AppSettings["DBConnectionDefault"]); MULTI
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            #region Search by userName
            if (Request.QueryString["SearchByUser"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_user", " top 10 user_name as UserName", " user_name Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region Search by userName & InternalID
            if (Request.QueryString["SearchByUserID"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_user", " top 10 isnull(user_name,' ')+ '['+ isnull(user_loginid,' ') +']' as UserName , user_contactId, user_id", " user_name Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "," + DT.Rows[i][2].ToString() + "###" + DT.Rows[i][0].ToString() + "," + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region getProductByLetters
            if (Request.QueryString["getProductByLetters"] == "1")
            {
                try
                {
                    string param = Request.QueryString["search_param"].ToString();
                    string reqStr = Request.QueryString["letters"].ToString();
                    string pType = "";
                    if (param != "")
                    {
                        int length = param.Length;
                        if (length != 3)
                        {
                            pType = param.Substring(0, 2);
                            if (pType == "AM")
                            {
                                DT = oDBEngine.GetDataTable("tbl_master_products p,tbl_master_productsDetails pd", " top 10 p.prds_description,p.prds_internalId", "p.prds_internalId=pd.prd_internalId and pd.prd_amc='" + param + "' and p.prds_description Like '" + reqStr + "%'");
                                if (DT.Rows.Count != 0)
                                {
                                    for (int i = 0; i < DT.Rows.Count; i++)
                                    {
                                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                                    }
                                }
                                else
                                    Response.Write("No Record Found###No Record Found|");
                            }
                            else
                            {
                                if (pType == "IC")
                                {
                                    DT = oDBEngine.GetDataTable("tbl_master_products p,tbl_master_productsDetails pd", " top 10 p.prds_description,p.prds_internalId", "p.prds_internalId=pd.prd_internalId and pd.prd_insurerName='" + param + "' and  p.prds_description Like '" + reqStr + "%'");
                                    if (DT.Rows.Count != 0)
                                    {
                                        for (int i = 0; i < DT.Rows.Count; i++)
                                        {
                                            Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                                        }
                                    }
                                    else
                                        Response.Write("No Record Found###No Record Found|");
                                }
                                else
                                    Response.Write("No Record Found###No Record Found|");

                            }
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_CFProducts", " top 10 cf_pname,cf_pcode", "cf_pcode like '" + param + "%" + "' and  cf_pname Like '" + reqStr + "%'");
                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }
                    else
                    {
                        Response.Write("Please Select Company Name###Please Select Company Name|");
                    }

                }
                catch
                {

                }
            }

            #endregion
            #region refered by
            if (Request.QueryString["referedby"] == "1")
            {
                //Response.Write("<script language='javaScript'> alert('Reminder Is For You !!') </script>");
                string reqStr = Request.QueryString["letters"].ToString();
                DT.Rows.Clear();
                switch (Request.QueryString["search_param"].ToString())
                {
                    case "1":
                    case "2":
                    case "5":
                    case "6":
                    case "7":
                    case "9":
                    case "11":
                    case "12":
                    case "13":
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                        break;
                    case "0":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM') and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "3":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "  Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='DV' and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "4":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "  Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM' or cnt_contactType='CL') and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "8":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='RA' and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "10":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='RC' and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "14":
                        if (Session["KeyVal_InternalID"] != null)
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", " Top 10 (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + ISNULL(cnt_shortName, '')+']') AS cnt_firstName,cnt_internalId", " cnt_internalId='" + Session["KeyVal_InternalID"].ToString() + "' and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'");
                        }
                        break;
                    case "20":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_UCC, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='CL' and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "24":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='SB' and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "25":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName,'')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='FR' and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    default:
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='PR' and cnt_firstName Like '" + Request.QueryString["letters"].ToString() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                };
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region searchdocument1
            if (Request.QueryString["searchdocument1"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] ptype = param.Split('-');
                if (ptype[1].ToString() != "")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_document", "top 10 doc_documentName,doc_documentName as Name", " doc_documenttypeId=" + ptype[1].ToString() + " and doc_documentName Like '" + reqStr + "%'");
                }
                else
                {
                    if (ptype[0].ToString() != "")
                    {
                        switch (ptype[0].ToString())
                        {
                            case "Products MF":
                                DT = oDBEngine.GetDataTable("tbl_master_products", "top 10 prds_description,prds_description as Name", " prds_productType='MF' and prds_description Like '" + reqStr + "%'");
                                break;
                            case "Products Insurance":
                                DT = oDBEngine.GetDataTable("tbl_master_products", "top 10 prds_description,prds_description as Name", " prds_productType='IN' and prds_description Like '" + reqStr + "%'");
                                break;
                            case "Products IPOs":
                                DT = oDBEngine.GetDataTable("tbl_master_products", "top 10 prds_description,prds_description as Name", " prds_productType='IP' and prds_description Like '" + reqStr + "%'");
                                break;
                            case "Customer":
                                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='CL' and cnt_firstName Like '" + reqStr + "%'");
                                break;
                            case "Lead":
                                DT = oDBEngine.GetDataTable("tbl_master_lead", "top 10 cnt_firstName,cnt_firstName as Name", "cnt_firstName Like '" + reqStr + "%'");
                                break;
                            case "Employee":
                                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='EM' and cnt_firstName Like '" + reqStr + "%'");
                                break;
                            case "Sub Brokers":
                                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='SB' and cnt_firstName Like '" + reqStr + "%'");
                                break;
                            case "Franchisees":
                                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='FR' and cnt_firstName Like '" + reqStr + "%'");
                                break;
                            case "Data Vendors":
                                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='DV' and cnt_firstName Like '" + reqStr + "%'");
                                break;
                            case "Relationship Partner":
                                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='RA' and cnt_firstName Like '" + reqStr + "%'");
                                break;
                            case "Relationship Manager":
                                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='RC' and cnt_firstName Like '" + reqStr + "%'");
                                break;
                            case "AMCs":
                                DT = oDBEngine.GetDataTable("tbl_master_AssetsManagementCompanies", "top 10 ,amc_nameOfMutualFund as Name", " amc_nameOfMutualFund Like '" + reqStr + "%'");
                                break;
                            case "Insurance Companies":
                                DT = oDBEngine.GetDataTable("tbl_master_insurerName", "top 10  insu_nameOfCompany, insu_nameOfCompany as Name", " insu_nameOfCompany Like '" + reqStr + "%'");
                                break;
                            case "RTAs":
                                DT = oDBEngine.GetDataTable("tbl_registrarTransferAgent", "top 10 rta_name,rta_name as Name", " rta_name Like '" + reqStr + "%'");
                                break;
                            case "Branches":
                                DT = oDBEngine.GetDataTable("tbl_master_branch", "top 10 branch_description,branch_description as Name", " branch_description Like '" + reqStr + "%'");
                                break;
                            case "Companies":
                                DT = oDBEngine.GetDataTable("tbl_master_company", "top 10 cmp_Name,cmp_Name as Name", " cmp_Name Like '" + reqStr + "%'");
                                break;
                            case "Building":
                                DT = oDBEngine.GetDataTable("tbl_master_building", "top 10 bui_Name,bui_Name as Name", " bui_Name Like '" + reqStr + "%'");
                                break;
                        }
                    }
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region searchdocument
            if (Request.QueryString["searchdocument"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] parameters = param.Split('~');
                Boolean checkdata = Boolean.Parse(parameters[2].ToString());
                if (checkdata)
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_document", " top 10 doc_documentName,doc_documentName as Name ", " doc_documenttypeId='" + parameters[1] + "' and doc_documentName Like '" + reqStr + "%'");
                }
                else
                {
                    if (parameters[0] != "")
                    {
                        switch (parameters[0].ToString())
                        {
                            case "Products MF":
                                DT = oDBEngine.GetDataTable(" tbl_master_products ", " top 10 prds_description,prds_description as Name", " prds_productType='MF' and prds_description Like '" + reqStr + "'");
                                break;
                            case "Products Insurance":
                                DT = oDBEngine.GetDataTable(" tbl_master_products ", " top 10 prds_description,prds_description as Name", " prds_productType='IN' and prds_description Like '" + reqStr + "'");
                                break;
                            case "Products IPOs":
                                DT = oDBEngine.GetDataTable(" tbl_master_products ", " top 10 prds_description,prds_description as Name", " prds_productType='IP' and prds_description Like '" + reqStr + "'");
                                break;
                            case "Customer":
                                DT = oDBEngine.GetDataTable(" tbl_master_contact ", " top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='CL' and cnt_firstName Like '" + reqStr + "'");
                                break;
                            case "Lead":
                                DT = oDBEngine.GetDataTable(" tbl_master_lead ", " top 10 cnt_firstName,cnt_firstName as Name", " cnt_firstName Like '" + reqStr + "'");
                                break;
                            case "Employee":
                                DT = oDBEngine.GetDataTable(" tbl_master_contact ", " top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='EM' and cnt_firstName Like '" + reqStr + "'");
                                break;
                            case "Sub Brokers":
                                DT = oDBEngine.GetDataTable(" tbl_master_contact ", " top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='SB' and cnt_firstName Like '" + reqStr + "'");
                                break;
                            case "Franchisees":
                                DT = oDBEngine.GetDataTable(" tbl_master_contact ", " top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='FR' and cnt_firstName Like '" + reqStr + "'");
                                break;
                            case "Data Vendors":
                                DT = oDBEngine.GetDataTable(" tbl_master_contact ", " top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='DV' and cnt_firstName Like '" + reqStr + "'");
                                break;
                            case "Relationship Partner":
                                DT = oDBEngine.GetDataTable(" tbl_master_contact ", " top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='RA' and cnt_firstName Like '" + reqStr + "'");
                                break;
                            case "Relationship Manager":
                                DT = oDBEngine.GetDataTable(" tbl_master_contact ", " top 10 cnt_firstName,cnt_firstName as Name", " cnt_contactType='RC' and cnt_firstName Like '" + reqStr + "'");
                                break;
                            case "AMCs":
                                DT = oDBEngine.GetDataTable(" tbl_master_AssetsManagementCompanies ", " top 10 amc_nameOfMutualFund as Name", " amc_nameOfMutualFund Like '" + reqStr + "'");
                                break;
                            case "Insurance Companies":
                                DT = oDBEngine.GetDataTable(" tbl_master_insurerName ", " top 10 insu_nameOfCompany, insu_nameOfCompany as Name", " insu_nameOfCompany Like '" + reqStr + "'");
                                break;
                            case "RTAs":
                                DT = oDBEngine.GetDataTable(" tbl_registrarTransferAgent ", " top 10 rta_name,rta_name as Name", " rta_name Like '" + reqStr + "'");
                                break;
                            case "Branches":
                                DT = oDBEngine.GetDataTable(" tbl_master_branch ", " top 10 branch_description,branch_description as Name", " branch_description Like '" + reqStr + "'");
                                break;
                            case "Companies":
                                DT = oDBEngine.GetDataTable(" tbl_master_company ", " top 10 cmp_Name,cmp_Name as Name ", " cmp_Name Like '" + reqStr + "'");
                                break;
                            case "Building":
                                DT = oDBEngine.GetDataTable(" tbl_master_building ", " top 10 bui_Name,bui_Name as Name ", " bui_Name Like '" + reqStr + "'");
                                break;
                        }
                    }
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region bank details
            if (Request.QueryString["bankdetails"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT.Rows.Clear();
                string listitem = "";
                reqStr = Request.QueryString["letters"].ToString();
                switch (Request.QueryString["search_param"].ToString())
                {
                    case "bnk_bankName":
                        listitem = "isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~' + isnull(bnk_micrno,'') + '~0'";
                        DT = oDBEngine.GetDataTable(" tbl_master_bank", " top 10 " + listitem + " as bank, bnk_bankname", " bnk_bankName like '" + reqStr + "%'");
                        break;
                    case "bnk_Micrno":
                        listitem = "isnull(bnk_micrno,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~1'";
                        DT = oDBEngine.GetDataTable(" tbl_master_bank", " top 10 " + listitem + " as bank, bnk_bankname", " bnk_Micrno like '" + reqStr + "%'");
                        break;
                    case "bnk_branchName":
                        //listitem = "isnull(bnk_branchname,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_micrno,'') + '~2'";
                        listitem = "isnull(bnk_branchname,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_micrno,'') + '~2'";
                        DT = oDBEngine.GetDataTable(" tbl_master_bank", " top 10 " + listitem + " as bank, bnk_bankname", " bnk_branchName like '" + reqStr + "%'");
                        break;

                };

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region bank details  bankdetailsByAny: by NAME or by MICRO or by BRANCH
            if (Request.QueryString["bankdetailsByAny"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                string listitem = " bnk_bankname + ' ! ' + bnk_branchname + ' ! ' + bnk_micrno ";
                DT = oDBEngine.GetDataTable(" tbl_master_bank", " top 10 " + listitem + " as bank, bnk_id ", " bnk_bankName like '" + reqStr + "%' OR bnk_branchName like '" + reqStr + "%' OR bnk_Micrno like '" + reqStr + "%'");


                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region usergrpdetails
            if (Request.QueryString["usergrpdetails"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string[] user;
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable(" tbl_master_user INNER JOIN  tbl_trans_employeeCTC ON tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId INNER JOIN   tbl_master_designation ON tbl_trans_employeeCTC.emp_Designation = tbl_master_designation.deg_id INNER JOIN  tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id INNER JOIN  tbl_master_company ON tbl_trans_employeeCTC.emp_Organization = tbl_master_company.cmp_id ORDER BY tbl_master_user.user_name ", " user_group,tbl_master_user.user_name + '  [ ' + tbl_master_branch.branch_description + '  * ' + tbl_master_company.cmp_Name + '  *  ' + tbl_master_designation.deg_designation + '  ] ' AS User_Info", null);
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        user = DT.Rows[i][0].ToString().Split(',');
                        for (int j = 0; j < user.Length; j++)
                        {
                            if (user[0] == param)
                                Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region GetMailId
            if (Request.QueryString["GetMailId"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (param == "LD")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN tbl_master_lead ON tbl_master_email.eml_cntId = tbl_master_lead.cnt_internalId ", " top 10 IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid ", " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='" + param + "'  and cnt_firstname Like '" + reqStr + "%' ");

                }
                else if (param == "CD")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN Master_CdslClients ON tbl_master_email.eml_cntId = Master_CdslClients.CdslClients_BOID ", " top 10 IsNull(CdslClients_FirstHolderName,'') + '< '+ tbl_master_email.eml_email + '>' as name,CdslClients_BOID as contactid ", " (tbl_master_email.eml_email <> '')  and CdslClients_FirstHolderName Like  '" + reqStr + "%' ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId ", " top 10 IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid ", " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='" + param + "'  and cnt_firstname Like '" + reqStr + "%' ");
                }
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            if (Request.QueryString["ContactDetail"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "Top 10 (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + '!' + cnt_internalId) AS cnt_firstName,cnt_internalId", " (LEFT(cnt_internalId, 2) = 'CL') and (cnt_firstName Like '" + reqStr + "%' or (cnt_firstName + cnt_middlename + cnt_lastname like ' " + reqStr + "%'))");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i]["cnt_firstName"].ToString() + "###" + DT.Rows[i]["cnt_firstName"].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            if (Request.QueryString["Arbclient"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Clients")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "Top 10 (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + '[' + cnt_ucc+ ']') ,cnt_internalId", " (LEFT(cnt_internalId, 2) = 'CL') and (cnt_firstName Like '" + reqStr + "%' or (cnt_firstName + cnt_middlename + cnt_lastname like ' " + reqStr + "%') or cnt_ucc like ' " + reqStr + "%')");
                }
                else if (idlist[0] == "ArbGroup")
                {
                    DT = oDBEngine.GetDataTable("Master_ArbGroup", "top 10 rtrim(ltrim(ArbGroup_Name))+' [ '+rtrim(ltrim(ArbGroup_Code))+' ]',ArbGroup_Code ", "(ArbGroup_Code like '" + reqStr + "%' or ArbGroup_Name like '" + reqStr + "%')");

                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                        //Response.Write(DT.Rows[i]["cnt_firstName"].ToString() + "###" + DT.Rows[i]["cnt_firstName"].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            if (Request.QueryString["onlyuser"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Clients")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_user", "distinct top 10 USER_NAME+' ['+isnull(user_loginId,'')+']',user_id", "user_group not like ('" + Session["id"] + "') and ( USER_NAME like '" + reqStr + "%' or user_loginId like '" + reqStr + "%')");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                        //Response.Write(DT.Rows[i]["cnt_firstName"].ToString() + "###" + DT.Rows[i]["cnt_firstName"].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            // Select Top 10 (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + '[' + cnt_ucc+ ']') AS cnt_firstName,cnt_internalId from tbl_master_contact WHERE  (LEFT(cnt_internalId, 2) = 'CL') and (cnt_firstName Like '%%' or (cnt_firstName + cnt_middlename + cnt_lastname like ' %%'))
            #endregion
            #region getCountriesByLetters1
            if (Request.QueryString["getCountriesByLetters1"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_productsDetails INNER JOIN   tbl_master_products ON tbl_master_productsDetails.prd_internalId = tbl_master_products.prds_internalId", "Top 10  tbl_master_products.prds_internalId, tbl_master_products.prds_description", "prds_productType = " + 12 + " and prds_description Like '" + reqStr + "%' ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i]["prds_internalId"].ToString() + "###" + DT.Rows[i]["prds_description"].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region GetLeadId
            if (Request.QueryString["GetLeadId"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN tbl_master_lead ON tbl_master_email.eml_cntId = tbl_master_lead.cnt_internalId ", " top 10 IsNull(cnt_firstName,'') + IsNull(cnt_lastName,'') + '< '+ tbl_master_email.eml_email + '>' as name ", " (tbl_master_email.eml_email <> '')   and cnt_firstname Like '" + reqStr + "%'");
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region BranchName SearchByBranchName
            if (Request.QueryString["SearchByBranchName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_branch", " top 10 branch_description as BranchName, branch_internalId", " branch_description Like '" + reqStr + "%' and branch_type='Own Branch'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "," + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SubBroker SearchBySubBroker
            if (Request.QueryString["SearchBySubBroker"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_branch", " top 10 branch_description as BranchName, branch_internalId", " branch_description Like '" + reqStr + "%' and branch_type='Sub Broker'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "," + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region Franchesees SearchByFranchese
            if (Request.QueryString["SearchByFranchese"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_branch", " top 10 branch_description as BranchName, branch_internalId", " branch_description Like '" + reqStr + "%' and branch_type='Franchisee'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "," + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByEmployees
            if (Request.QueryString["SearchByEmployees"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = "";
                if (Request.QueryString["search_param"] != null)
                    param = Request.QueryString["search_param"].ToString();
                else
                    param = "";
                if (param == "1")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' ,cnt_internalId+'~'+isnull((select top 1 eml_email from tbl_master_email where eml_cntId=tbl_master_contact.cnt_internalId and eml_email<>''),'')+'~'+isnull((select top 1 phf_phonenumber from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId and phf_phonenumber<>''),'')", "  cnt_internalId like 'EM%' and cnt_firstname like '" + reqStr + "%'");
                }
                else
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid");
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion


            #region SearchByEmployeesWithSignature

            if (Request.QueryString["SearchByEmployeesWithSignature"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                string where = @" contact.cnt_firstName Like '" + reqStr + @"%' and e.emp_contactId=contact.cnt_internalid 
                              and doc_contactId=e.emp_contactid
                              and doc_documentTypeId=
	                            (select top 1 dty_id from tbl_master_documentType 
	                            where dty_documentType='Signature' 
                                and dty_applicableFor='Employee') ";

                DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e,tbl_master_document", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion


            #region SearchByEmployeesReferal
            if (Request.QueryString["SearchByEmployeesReferal"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,cnt_internalID", " contact.cnt_firstName Like '" + reqStr + "%' and contact.cnt_ContactType in ('RA','BP')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByEmployeesSubBrokerFranchise
            if (Request.QueryString["SearchByEmployeesSubBrokerFranchise"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,cnt_internalID", " contact.cnt_firstName Like '" + reqStr + "%' and contact.cnt_ContactType in ('SB','FR')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByBankName
            if (Request.QueryString["SearchByBankName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_bank b,tbl_trans_contactBankDetails c", " (b.bnk_bankName+'~'+b.bnk_micrno+'~'+b.bnk_internalId) as BankName,b.bnk_internalId", " b.bnk_bankName Like '" + reqStr + "%' and b.bnk_id=c.cbd_bankcode and cbd_cntid='" + param + "'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByBranchName
            if (Request.QueryString["BranchName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_branch", "branch_description,branch_id", "branch_description Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region BranchAgainstBankName
            if (Request.QueryString["SearchByBankBranchName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_bank", " top 10 (bnk_bankName + '~' + bnk_branchName + '~' + bnk_internalId) as BranchName, bnk_internalId", " bnk_bankName Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region employeeBranchHrchy-: employees belong to branch hierarchy!
            if (Request.QueryString["employeeBranchHrchy"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');//___[0] is for company & [1] f0r branch
                if (paramete[0] == "All" && paramete[1] == "All")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid  and  contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")");
                }
                if (paramete[0] == "All" && paramete[1] != "All")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid  and contact.cnt_branchId in (" + paramete[1] + ")");
                }
                if (paramete[0] != "All" && paramete[1] == "All")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid  and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")");
                }
                if (paramete[0] != "All" && paramete[1] != "All")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid  and contact.cnt_branchId in (" + paramete[1] + ")");
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region getCompanyByLetters
            if (Request.QueryString["getCompanyByLetters"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (param == "Mutual Fund")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_AssetsManagementCompanies", " Top 10 amc_nameOfMutualFund,amc_amcCode", "amc_nameOfMutualFund Like '" + reqStr + "%'");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    if (param == "Insurance-Life")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_insurerName", " Top 10 insu_nameOfCompany,insu_internalId", "insu_InsuranceCompType ='Life Insurers' and insu_nameOfCompany Like '" + reqStr + "%'");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else
                    {
                        if (param == "Insurance-General")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_insurerName", " Top 10 insu_nameOfCompany,insu_internalId", "insu_InsuranceCompType ='Non-Life Insurers' and insu_nameOfCompany Like '" + reqStr + "%'");
                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                }
            }

            #endregion
            #region SearchCompanyName
            if (Request.QueryString["SearchByCompany"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();

                //DT = oDBEngine.GetDataTable("tbl_master_bank", " top 10 (bnk_bankName + '~' + bnk_branchName + '~' + bnk_internalId) as BranchName, bnk_internalId", " bnk_bankName Like '" + reqStr + "%'");

                //DT = oDBEngine.GetDataTable("(SELECT a.*,b.Cmp_name FROM (SELECT * FROM tbl_master_companyExchange) AS A LEFT OUTER JOIN tbl_master_company AS B ON B.cmp_internalid=A.exch_compId ) AS C LEFT OUTER JOIN tbl_master_exchange AS D ON C.exch_exchId=D.exh_cntId", "c.exch_internalid,c.cmp_name+'--'+d.exh_shortname+'--'+ C.exch_segmentId AS Modified", " c.cmp_name Like '" + reqStr + "%'");

                DT = oDBEngine.GetDataTable("tbl_master_company comp,tbl_master_companyExchange ce,tbl_master_exchange ex", "top 10 ce.exch_internalid as internalid,comp.cmp_name+' ['+ex.exh_shortname+'-'+ce.exch_segmentId+']' as Modify ", " comp.cmp_internalId=ce.exch_compId and ce.exch_exchId=ex.exh_cntId and comp.cmp_name like '" + reqStr + "%'");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region CustomerHrchy
            if (Request.QueryString["CustomerHrchy"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //  DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 (isnull(contact.cnt_firstName,'') +' '+isnull(contact.cnt_middleName,'')+' '+isnull(contact.cnt_lastName,''))+'['+isnull(contact.cnt_UCC,'')+']' as Name,contact.cnt_internalId", "contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID  ", "cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion
            #region BrokerHrchy
            if (Request.QueryString["BrokerHrchy"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //  DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 (isnull(contact.cnt_firstName,'') +' '+isnull(contact.cnt_middleName,'')+' '+isnull(contact.cnt_lastName,''))+'['+isnull(contact.cnt_UCC,'')+']' as Name,contact.cnt_internalId", "contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID  ", "cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'BO%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }

            #endregion

            #region userHrchy
            if (Request.QueryString["user"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //  DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 (isnull(contact.cnt_firstName,'') +' '+isnull(contact.cnt_middleName,'')+' '+isnull(contact.cnt_lastName,''))+'['+isnull(contact.cnt_UCC,'')+']' as Name,contact.cnt_internalId", "contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("tbl_master_user", "distinct top 10  isnull(rtrim(user_name),'') +' ['+isnull(rtrim(user_loginid),'')+']' as user_loginid,user_id ", "user_name LIKE '" + reqStr + "%'  OR user_loginid  LIKE '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }

            #endregion
            #region Remindercategory
            if (Request.QueryString["Remindercategory"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //  DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 (isnull(contact.cnt_firstName,'') +' '+isnull(contact.cnt_middleName,'')+' '+isnull(contact.cnt_lastName,''))+'['+isnull(contact.cnt_UCC,'')+']' as Name,contact.cnt_internalId", "contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("Master_Remindercategory", "distinct top 10  isnull(rtrim(Remindercategory_shortname),'') as shortname,Remindercategory_id ", "Remindercategory_shortname LIKE '" + reqStr + "%'");//  OR user_loginid  LIKE '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }

            #endregion


            #region GroupHrchy
            if (Request.QueryString["GroupHrchy"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                //string[,] aa = oDBEngine.GetFieldValue("Master_ChargeGroup", "ChargeGroup_Type", "ChargeGroup_Code Like '" + reqStr + "%' or ChargeGroup_Name Like '" + reqStr + "%'", 1);
                //if (aa.Length >= 1 && aa[0, 0] != "n")
                //{
                DT = oDBEngine.GetDataTable("Master_ChargeGroup mcg", " top 10 (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d ,mcg.ChargeGroup_Code", "mcg.ChargeGroup_Type='1'and (mcg.ChargeGroup_Code Like '" + reqStr + "%' or mcg.ChargeGroup_Name Like '" + reqStr + "%')");

                // }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion

            #region ProductHrchy
            if (Request.QueryString["ProductHrchy"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                string[] BrokParam = param.Split('-');
                if (BrokParam[1] == "CM")
                    DT = oDBEngine.GetDataTable("Master_Products mp,Master_ProductTypes mpt", "top 10 mp.Products_Name +'['+ mpt.ProductType_Name +']' as dd,mp.Products_ID", "mpt.ProductType_ID=mp.Products_ProductTypeID and mp.Products_Name Like '" + reqStr + "%' and mpt.ProductType_Code=ltrim(rtrim('EQ'))");
                else if (BrokParam[1] == "FO")
                {
                    if (BrokParam[2] == "2")
                        //DT = oDBEngine.GetDataTable("Master_Equity", "top 10 (ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+'  '+ltrim(rtrim(isnull(Equity_FOIdentifier,'')))+'  '+ltrim(rtrim(isnull(Equity_Series,'')))+'  '+cast(cast(isnull(Equity_StrikePrice,0.00) as numeric(16,2)) as varchar)+'  '+convert(varchar(11),Equity_EffectUntil,113)) as Equity_Product,ltrim(rtrim(Equity_SeriesID)) as Equity_SeriesID  ", " Equity_FOIdentifier is not null and Equity_TickerSymbol like '" + reqStr + "%'");
                        DT = oDBEngine.GetDataTable("(select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity,Trans_CustomerTrades  WHERE CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");

                    else if (BrokParam[2] == "3")
                        DT = oDBEngine.GetDataTable("Master_Products mp,Master_ProductTypes mpt", "top 10 mp.Products_Name +'['+ mpt.ProductType_Name +']' as dd,mp.Products_ID", "mpt.ProductType_ID=mp.Products_ProductTypeID and mp.Products_Name Like '" + reqStr + "%' and mpt.ProductType_Code in('EQ','ID')");
                }
                else if (BrokParam[1] == "COMM")
                {
                    if (BrokParam[2] == "2")
                        DT = oDBEngine.GetDataTable("master_commodity", "top 10 (ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+'  '+ltrim(rtrim(isnull(Commodity_Identifier,'')))+'  '+ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+'  '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+'  '+convert(varchar(11),Commodity_EffectiveDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID ", " Commodity_TickerSymbol like '" + reqStr + "%'");
                    else if (BrokParam[2] == "3")
                        DT = oDBEngine.GetDataTable("Master_Products mp,Master_ProductTypes mpt", "top 10 mp.Products_Name +'['+ mpt.ProductType_Name +']' as dd,mp.Products_ID", "mpt.ProductType_ID=mp.Products_ProductTypeID and mp.Products_Name Like '" + reqStr + "%' and mpt.ProductType_Code=ltrim(rtrim('CO'))");
                }
                if (BrokParam[1] == "CMALL")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        DT = oDBEngine.GetDataTable("Master_Equity", "top 10 (ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+' '+ltrim(rtrim(isnull(Equity_Series,'')))) AS Equity_Product,ltrim(rtrim(Equity_SeriesID)) as Equity_SeriesID  ", "(Equity_TickerSymbol like '" + reqStr + "%' or Equity_SeriesID like '" + reqStr + "%') and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Master_Equity", "top 10 (ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+' '+ltrim(rtrim(isnull(Equity_tickercode,'')))) AS Equity_Product,ltrim(rtrim(Equity_SeriesID)) as Equity_SeriesID  ", "(Equity_TickerSymbol like '" + reqStr + "%' or Equity_SeriesID like '" + reqStr + "%') and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'");
                    }
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion
            #region SearchAccount
            if (Request.QueryString["SearchAccount"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //DT = oDBEngine.GetDataTable("tbl_master_bank", " top 10 (bnk_bankName + '~' + bnk_branchName + '~' + bnk_internalId) as BranchName, bnk_internalId", " bnk_bankName Like '" + reqStr + "%'");

                //DT = oDBEngine.GetDataTable("(SELECT a.*,b.Cmp_name FROM (SELECT * FROM tbl_master_companyExchange) AS A LEFT OUTER JOIN tbl_master_company AS B ON B.cmp_internalid=A.exch_compId ) AS C LEFT OUTER JOIN tbl_master_exchange AS D ON C.exch_exchId=D.exh_cntId", "c.exch_internalid,c.cmp_name+'--'+d.exh_shortname+'--'+ C.exch_segmentId AS Modified", " c.cmp_name Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_ReferenceID As ID, (Mainaccount_Name + '   [ ' + cast([MainAccount_AccountCode]  as varchar(100)) +'' +' ]') as AccName", "Mainaccount_Name Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region AccCSDL
            if (Request.QueryString["AccCSDL"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_ReferenceID As ID, (Mainaccount_Name + '   [ ' + cast([MainAccount_AccountCode]  as varchar(100)) +'' +' ]') as AccName", "Mainaccount_Name Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region GroupHrchyDP
            if (Request.QueryString["GroupHrchyDP"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_ChargeGroup mcg", "(isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d ,mcg.ChargeGroup_Code", "mcg.ChargeGroup_Type='3'and  (mcg.ChargeGroup_Name Like '" + reqStr + "%' or mcg.ChargeGroup_Code Like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion

            #region GroupHrchyCH
            if (Request.QueryString["GroupHrchyCH"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_ChargeGroup mcg", "(isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d ,mcg.ChargeGroup_Code", "mcg.ChargeGroup_Type='2'and  (mcg.ChargeGroup_Name Like '" + reqStr + "%'or mcg.ChargeGroup_Code like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion


            #region Acc
            if (Request.QueryString["Acc"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT.Rows.Clear();
                string listitem = "";
                reqStr = Request.QueryString["letters"].ToString();
                switch (Request.QueryString["search_param"].ToString())
                {
                    case "2":
                        DT = oDBEngine.GetDataTable("Master_CdslClients ", "(isnull(CdslClients_FirstHolderName,''))+ ' '+ '[' + substring(CdslClients_BOID,9,8)+']'as d ,substring(CdslClients_BOID,9,8) as CdslClients_BOID", " CdslClients_BOID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%'");
                        break;
                    case "1":
                        DT = oDBEngine.GetDataTable("Master_NsdlClients ", "(isnull(NsdlClients_BenFirstHolderName,''))+ ' '+ '[' + cast(NsdlClients_BenAccountID as varchar) +']'as d ,NsdlClients_BenAccountID", " NsdlClients_BenAccountID Like '" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%'");
                        break;


                };

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion

            #region ContactName
            if (Request.QueryString["ContactName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();
                //DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName, cnt_internalId", " (cnt_internalId like 'EM%' or cnt_internalId like 'SB%' or cnt_internalId like 'RA%' or cnt_internalId like 'PR%' or cnt_internalId like 'FR%' or cnt_internalId like 'BO%') and (cnt_firstName Like '" + reqStr + "%' )");
                DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName, cnt_internalId", " (cnt_internalId like 'CL%') and (cnt_firstName Like '" + reqStr + "%' or  cnt_UCC Like '" + reqStr + "%' )");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region  AllContact
            if (Request.QueryString["AllContact"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();

                string rcvid = Session["ID1"].ToString().Trim();

                rcvid = rcvid.Replace("-", " - ");
                string company = Session["ID"].ToString().Trim();

                //DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + 	ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') 	+ '] ' AS cnt_firstName, cnt_internalId", " cnt_internalid like 'CL%' and (cnt_firstName Like '" + reqStr + "%' or  cnt_UCC Like '" + reqStr + "%' )");
                DT = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + 	ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(crg_tcode)),'') 	+ '] ' AS cnt_firstName,crg_cntID", "crg_exchange='" + rcvid + "' and crg_company='" + company + "' and cnt_internalid like 'CL%' and (cnt_firstName Like '" + reqStr + "%' or  crg_tcode Like '" + reqStr + "%' )  and crg_cntID=cnt_internalId");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region  AllContactbroker
            if (Request.QueryString["AllContactbroker"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();
                string rcvid = Session["ID1"].ToString().Trim();
                rcvid = rcvid.Replace("-", " - ");
                string company = Session["ID"].ToString().Trim();

                //DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + 	ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') 	+ '] ' AS cnt_firstName, cnt_internalId", " cnt_internalid like 'CL%' and (cnt_firstName Like '" + reqStr + "%' or  cnt_UCC Like '" + reqStr + "%' )");
                DT = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + 	ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(crg_tcode)),'') 	+ '] ' AS cnt_firstName,crg_cntID", "crg_exchange='" + rcvid + "' and crg_company='" + company + "' and cnt_internalid like 'BO%' and (cnt_firstName Like '" + reqStr + "%' or  crg_tcode Like '" + reqStr + "%' )  and crg_cntID=cnt_internalId");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region  PostCLITradesContact
            if (Request.QueryString["PostCLITradesContact"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + 	ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') 	+ '] ' AS cnt_firstName, cnt_internalId", " cnt_internalid like 'CL%' and (cnt_clienttype not like 'Pro%' or cnt_clienttype is null) 	and (cnt_firstName Like '" + reqStr + "%' )");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region  PostPROTradesContact
            if (Request.QueryString["PostPROTradesContact"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName, cnt_internalId", " cnt_internalid like 'CL%' and cnt_clienttype like 'Pro%' and (cnt_firstName Like '" + reqStr + "%' )");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region  TradingMember
            if (Request.QueryString["TradingMember"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName, cnt_internalId", " cnt_internalid like 'CL%' and (cnt_clienttype='Trading Member') and (cnt_firstName Like '" + reqStr + "%' )");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region ContactDetail_UnderBranch
            if (Request.QueryString["ContactDetail_UnderBranch"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact INNER JOIN   tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id", "TOP 10 ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '')   + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '') + '!' + tbl_master_contact.cnt_internalId AS cnt_firstName, tbl_master_contact.cnt_internalId", " tbl_master_branch.branch_Id ='" + param + "' And (LEFT(cnt_internalId, 2) = 'CL') and (cnt_firstName Like '" + reqStr + "%' or (cnt_firstName + cnt_middlename + cnt_lastname like ' " + reqStr + "%'))");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region ContactDetail_UnderBranch_new
            if (Request.QueryString["ContactDetail_UnderBranch_new"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact INNER JOIN   tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id", " TOP 10 ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '')   + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '')+ ' [' + ISNULL(tbl_master_contact.cnt_ucc, '')+']'  AS cnt_firstName, tbl_master_contact.cnt_internalId", " tbl_master_branch.branch_Id ='" + param + "' And (LEFT(cnt_internalId, 2) = 'CL') and (cnt_firstName Like '" + reqStr + "%' or (cnt_firstName + cnt_middlename + cnt_lastname like ' " + reqStr + "%'))");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SchemeDetail
            if (Request.QueryString["SchemeDetail"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_products", "Top 10 prds_description + '!' + prds_internalId  AS product,prds_internalId", " prds_internalid like 'in%' And prds_description LIKE '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SchemeDetail_new
            if (Request.QueryString["SchemeDetail_new"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                DT = oDBEngine.GetDataTable(" tbl_master_products,tbl_master_productsDetails ", " Top 10 prds_description AS product,prds_internalId ", " prd_internalId=prds_internalid And prds_description LIKE '" + reqStr + "%' and prd_insurerName='" + param + "'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region DP Account Search Result
            if (Request.QueryString["DPSearch"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //DT = oDBEngine.GetDataTable("(select (DP_Name+' '+'['+DP_DepositoryID+']') as DPName,DP_DepositoryID from Master_DP) DP", " top 10 DP_DepositoryID,DPName ", " DPName Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("(select (ltrim(rtrim(DP_DPName))+' '+'['+ltrim(rtrim(DP_DPID))+']') as DPName,DP_DpID from tbl_Master_Depositoryparticipants) DP", " top 10 DP_DpID,DPName ", " (DPName Like '" + reqStr + "%' OR DP_DpID Like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchSettlement
            if (Request.QueryString["SearchBySettlement"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //DT = oDBEngine.GetDataTable("tbl_master_bank", " top 10 (bnk_bankName + '~' + bnk_branchName + '~' + bnk_internalId) as BranchName, bnk_internalId", " bnk_bankName Like '" + reqStr + "%'");

                //DT = oDBEngine.GetDataTable("(SELECT a.*,b.Cmp_name FROM (SELECT * FROM tbl_master_companyExchange) AS A LEFT OUTER JOIN tbl_master_company AS B ON B.cmp_internalid=A.exch_compId ) AS C LEFT OUTER JOIN tbl_master_exchange AS D ON C.exch_exchId=D.exh_cntId", "c.exch_internalid,c.cmp_name+'--'+d.exh_shortname+'--'+ C.exch_segmentId AS Modified", " c.cmp_name Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("Master_Settlements", "top 10 settlements_ID,isnull(rtrim(settlements_Number),'')+ ' [ ' + isnull(rtrim(settlements_typeSuffix),'') + ' ]' as SettlementName", "settlements_number Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region settNo
            if (Request.QueryString["settNo"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT.Rows.Clear();
                string listitem = "";
                reqStr = Request.QueryString["letters"].ToString();
                string[] serchp = Request.QueryString["search_param"].ToString().Split('~');
                //settNo For ICEX
                // DT = oDBEngine.GetDataTable(" Master_Settlements ", " top 5 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix),RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)", " Settlements_ExchangeSegmentID =(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + serchp[0] + " )) and Settlements_FinYear='" + serchp[1].Trim() + "' and (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)) like '" + reqStr + "%' ");
                DT = oDBEngine.GetDataTable(" Master_Settlements ", " top 5 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)+'  ' + REPLACE(CONVERT(VARCHAR(9), settlements_StartDateTime, 6), ' ', '-') AS [DD-Mon-YY],RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)", " Settlements_ExchangeSegmentID =(select ExchangeSegment_ID from Master_ExchangeSegments where (select exchange_ShortName from Master_Exchange where exchange_ID=ExchangeSegment_ExchangeID)+'-'+ExchangeSegment_Code = (select seg_name from tbl_master_segment where seg_id=" + serchp[0] + " )) and Settlements_FinYear='" + serchp[1].Trim() + "' and ((RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)) like '" + reqStr + "%' or CONVERT(VARCHAR(9), settlements_StartDateTime, 6) like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                {
                    //Response.Write("No Record Found###No Record Found|");
                    // if ((Session["ExchangeSegmentID"].ToString() == "1") || (Session["ExchangeSegmentID"].ToString() == "4") || (Session["ExchangeSegmentID"].ToString() == "15") || (Session["ExchangeSegmentID"].ToString() == "19"))
                    Response.Write("No Record Found###No Sett. found as on your criteria.<br/><strong><u>Please Click or Select here</u></strong> <br/>to open new Sett.|");
                    //else
                    //    Response.Write("No Record Found###No Record Found|");

                }

            }
            #endregion
            #region CDSLHolding
            if (Request.QueryString["CDSLHolding"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] date = paramete[0].Split(' ');

                string where = "Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";

                where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM' and '" + paramete[0].ToString() + "'";
                if (paramete[1].ToString() != "All")
                {
                    where = where + " and CdslClients_BOStatus='" + paramete[1].ToString() + "' ";
                }

                where = where + "and ( CdslClients_BOID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' )  ";

                DT = oDBEngine.GetDataTable("Trans_CdslHolding,Master_CdslClients ", "distinct top 10 CdslClients_BOID  As ID, (CdslClients_FirstHolderName + '   [ ' + cast([CdslClients_BOID]  as varchar(100)) +'' +' ]') as AccName", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region DPName
            if (Request.QueryString["DPName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
                //DT = oDBEngine.GetDataTable("(select top 10 DP_DepositoryID,DP_Name+' ['+DP_DepositoryID+']' as DPName,DP_DepositoryID+' ['+DP_Name+']' as DP_ID,(select ascii('" + reqStr + "')) as ID from Master_DP where DP_Name like '" + reqStr + "%' or DP_DepositoryID like '" + reqStr + "%') as D", "case when ID between '97' and '122' then DPName else DP_ID end as Name,DP_DepositoryID", null);
                DT = oDBEngine.GetDataTable("tbl_master_depositoryParticipants", " top 10ltrim(rtrim(dp_dpname))+' ['+dp_dpid+']' as Name,dp_dpid", "(dp_dpname like '" + reqStr + "%' or dp_dpid like '" + reqStr + "%')", "dp_dpname");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByIssuingBank
            if (Request.QueryString["SearchByIssuingBank"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //DT = oDBEngine.GetDataTable("tbl_master_bank", " top 10 (bnk_bankName + '~' + bnk_branchName + '~' + bnk_internalId) as BranchName, bnk_internalId", " bnk_bankName Like '" + reqStr + "%'");

                //DT = oDBEngine.GetDataTable("(SELECT a.*,b.Cmp_name FROM (SELECT * FROM tbl_master_companyExchange) AS A LEFT OUTER JOIN tbl_master_company AS B ON B.cmp_internalid=A.exch_compId ) AS C LEFT OUTER JOIN tbl_master_exchange AS D ON C.exch_exchId=D.exh_cntId", "c.exch_internalid,c.cmp_name+'--'+d.exh_shortname+'--'+ C.exch_segmentId AS Modified", " c.cmp_name Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("dbo.tbl_master_Bank", "top 10  bnk_id,(isnull(bnk_bankName,'')+ '-'+ isnull(bnk_micrno,'') ) as BankName", "bnk_bankName Like '" + reqStr + "%' or bnk_micrno Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region NSDLHolding
            if (Request.QueryString["NSDLHolding"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');

                string where = " Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlHolding.NsdlHolding_BenAccountNumber ";
                where = where + " and NsdlHolding_HoldingDateTime='" + paramete[0].ToString() + "'";
                if (paramete[1].ToString() != "All")
                {
                    where = where + " and NsdlClients_BenType='" + paramete[1].ToString() + "' ";
                }

                where = where + " and ( NsdlClients_BenAccountID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' )  ";
                string orderBy = " NsdlClients_BenFirstHolderName ";

                DT = oDBEngine.GetDataTable("Trans_NsdlHolding,Master_NsdlClients ", " distinct top 5 NsdlClients_BenAccountID  As ID, (NsdlClients_BenFirstHolderName + '   [ ' + cast([NsdlClients_BenAccountID]  as varchar(100)) +'' +' ]') as AccName ,NsdlClients_BenFirstHolderName", where, orderBy);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region NSDLHoldingISIN

            if (Request.QueryString["NSDLHoldingISIN"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');

                string where = " Master_NSDLISIN.NSDLISIN_Number=Trans_NsdlHolding.NsdlHolding_ISIN ";

                where = where + " and NsdlHolding_HoldingDateTime='" + paramete[0].ToString() + "'";
                //if (paramete[1].ToString() != "All")
                //{
                //    where = where + " and NsdlClients_BenType='" + paramete[1].ToString() + "' ";
                //}

                where = where + " and ( NSDLISIN_Number Like '%" + reqStr + "%' or NSDLISIN_CompanyName Like '" + reqStr + "%' )  ";

                DT = oDBEngine.GetDataTable("Trans_NsdlHolding,Master_NSDLISIN ", " distinct top 5 NSDLISIN_Number  As ID, (ltrim(rtrim(NSDLISIN_CompanyName)) + '   [ ' + convert(varchar,[NSDLISIN_Number]) + ' ]') as AccName ", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region NSDLHoldingSettlementNumber

            if (Request.QueryString["NSDLHoldingSettlementNumber"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');

                string where = " NsdlHolding_HoldingDateTime='" + paramete[0].ToString() + "'";

                where = where + " and  NsdlHolding_SettlementNumber Like '%" + reqStr + "%'  ";

                DT = oDBEngine.GetDataTable("Trans_NsdlHolding ", " distinct top 5 ltrim(rtrim(NsdlHolding_SettlementNumber))  As ID, ltrim(rtrim(NsdlHolding_SettlementNumber)) as AccName ", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion

            #region NSDLTransaction
            if (Request.QueryString["NSDLTransaction"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] stdate;
                string[] eddate;

                stdate = paramete[0].Split(' ');
                eddate = paramete[1].Split(' ');



                string where = "";//" Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlTransaction.NsdlTransaction_BenAccountNumber ";

                //where = "";//where + "and NsdlTransaction_Date between '" + stdate[1] + " " + stdate[2] + " " + stdate[6] + " 00:00:00" + "' and '" + eddate[1] + " " + eddate[2] + " " + eddate[6] + " 23:59:59" + "'";
                //if (paramete[2].ToString() != "All")
                //{
                //    where =" NsdlClients_BenType='" + paramete[2].ToString() + "' ";//where + " and NsdlClients_BenType='" + paramete[2].ToString() + "' ";
                //}

                where = where + "  ( NsdlClients_BenAccountID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' )  ";
                string orderBy = " NsdlClients_BenFirstHolderName ";

                //DT = oDBEngine.GetDataTable("Trans_NsdlTransaction,Master_NsdlClients ", " distinct top 5 NsdlClients_BenAccountID  As ID, (NsdlClients_BenFirstHolderName + '   [ ' + cast([NsdlClients_BenAccountID]  as varchar(100)) +'' +' ]') as AccName ,NsdlClients_BenFirstHolderName", where, orderBy);
                DT = oDBEngine.GetDataTable(" ( Select  distinct NsdlClients_BenFirstHolderName,NsdlClients_BenAccountID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID=(select  N.NSDLClients_ContactID from Master_NSDLClients as N where N.NsdlClients_BenAccountID=Master_NsdlClients.NsdlClients_BenAccountID)) as InterNalID from Master_NsdlClients) as DD ", " top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]' + ' ['+isnull(InterNalID,'')+']') as AccName,ID", "  ID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' ");
                //DT = oDBEngine.GetDataTable("Master_NSDLClients,tbl_master_contact", "distinct top 10 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(NsdlClients_BenAccountID  as varchar(100)) +'' +' ]'  + ' ['+(case when NSDLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,NsdlClients_BenAccountID  As ID", " (NSDLClients_ContactID=cnt_internalID or NSDLClients_ContactID is null) and (NsdlClients_BenAccountID like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region NSDLTransactionISIN
            if (Request.QueryString["NSDLTransactionISIN"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] stdate;
                string[] eddate;

                stdate = paramete[0].Split(' ');
                eddate = paramete[1].Split(' ');



                string where = "Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlTransaction.NsdlTransaction_BenAccountNumber and  Master_NSDLISIN.NSDLISIN_Number=Trans_NsdlTransaction.NsdlTransaction_ISIN ";


                where = where + "and NsdlTransaction_Date between '" + stdate[1] + " " + stdate[2] + " " + stdate[6] + " 00:00:00" + "' and '" + eddate[1] + " " + eddate[2] + " " + eddate[6] + " 23:59:59" + "'";
                if (paramete[2].ToString() != "All")
                {
                    where = where + " and NsdlClients_BenType='" + paramete[2].ToString() + "' ";
                }


                if (paramete[3].ToString() != "")
                {
                    where = where + " and  NsdlClients_BenAccountID= '" + paramete[3].ToString() + "'    ";
                }


                where = where + " and  (NSDLISIN_Number Like '%" + reqStr + "%' or NSDLISIN_CompanyName Like '" + reqStr + "%') ";



                DT = oDBEngine.GetDataTable("Trans_NsdlTransaction,Master_NsdlClients,Master_NSDLISIN ", "distinct top 10 NSDLISIN_Number  As ID, (ltrim(rtrim(NSDLISIN_CompanyName)) + '   [ ' + convert(varchar,[NSDLISIN_Number]) + ' ]') as AccName ", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }

            #endregion
            #region NSDLTransactionSettlement

            if (Request.QueryString["NSDLTransactionSettlement"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] stdate;
                string[] eddate;

                stdate = paramete[0].Split(' ');
                eddate = paramete[1].Split(' ');



                string where = "Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlTransaction.NsdlTransaction_BenAccountNumber and  Master_NSDLISIN.NSDLISIN_Number=Trans_NsdlTransaction.NsdlTransaction_ISIN ";

                where = where + "and NsdlTransaction_Date between '" + stdate[1] + " " + stdate[2] + " " + stdate[6] + " 00:00:00" + "' and '" + eddate[1] + " " + eddate[2] + " " + eddate[6] + " 23:59:59" + "'";
                if (paramete[2].ToString() != "All")
                {
                    where = where + " and NsdlClients_BenType='" + paramete[2].ToString() + "' ";
                }


                if (paramete[3].ToString() != "")
                {
                    where = where + " and  NsdlClients_BenAccountID= '" + paramete[3].ToString() + "'    ";
                }

                if (paramete[4].ToString() != "")
                {
                    where = where + " and  NsdlTransaction_ISIN= '" + paramete[4].ToString() + "'    ";
                }


                where = where + " and  NsdlTransaction_SettlementNumber Like '%" + reqStr + "%'  ";



                DT = oDBEngine.GetDataTable(" Trans_NsdlTransaction,Master_NsdlClients,Master_NSDLISIN ", "distinct top 10 NsdlTransaction_SettlementNumber As ID, NsdlTransaction_SettlementNumber as AccName", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }

            #endregion

            #region NSDLClientNames

            if (Request.QueryString["NSDLClientNames"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string paramete = Request.QueryString["search_param"].ToString();
                string where;
                where = " (NsdlClients_BenFirstHolderName Like  '" + reqStr + "%' or NsdlClients_BenAccountID Like  '%" + reqStr + "%')  and NsdlClients_DPID='" + paramete + "'";
                DT = oDBEngine.GetDataTable(" Master_NsdlClients ", "  top 10  NsdlClients_BenAccountID  As ID, NsdlClients_BenFirstHolderName + '   [ '+ convert(varchar,NsdlClients_BenAccountID) +' ]' as AccName", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");


            }
            #endregion

            #region DPAccounts
            if (Request.QueryString["DPAccounts"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();


                DT = oDBEngine.GetDataTable("Master_DPAccounts", @" top 10 ltrim(rtrim(DPAccounts_DPID))+'~'+ltrim(rtrim(DPAccounts_ClientID))
                +'~'+ case when len(ltrim(rtrim(DPAccounts_CMBPID)))=0 then 'NA' else ltrim(rtrim(DPAccounts_CMBPID)) end 
                as ID,ltrim(rtrim(DPAccounts_ShortName))+' [ '+ltrim(rtrim(DPAccounts_ClientID))+' ]'", "DPAccounts_AccountType not in ('[PLPAYOUT]','[SYSTM]') AND ltrim(rtrim(DPAccounts_CompanyID))='" + Session["LastCompany"].ToString() + "' and dpaccounts_exchangesegmentid in ('0','" + Session["usersegid"].ToString() + "') and (ltrim(rtrim(DPAccounts_ShortName)) like '" + reqStr + "%' or ltrim(rtrim(DPAccounts_ClientID)) like '%" + reqStr + "%')");


                if (DT.Rows.Count != 0)
                {

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {

                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion
            #region DPAccountsID_DPID_ClientId_CMBPID
            if (Request.QueryString["DPAccountsID_DPID_ClientId_CMBPID"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string prm = Request.QueryString["search_param"].ToString();

                string where = " ltrim(rtrim(DPAccounts_CompanyID))='" + Session["LastCompany"].ToString() + "' and Isnull(DpAccounts_IsClosed ,'A')<>'C' and (ltrim(rtrim(DPAccounts_ShortName)) like '%" + reqStr + "%' or ltrim(rtrim(DPAccounts_ClientID)) like '%" + reqStr + "%')";

                if (prm == "2")
                    where = where + "  and DpAccounts_AccountType in ('[POOL]','[PLPAYIN]') and DPAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString();

                DT = oDBEngine.GetDataTable("Master_DPAccounts", " top 10 convert(varchar,DPAccounts_ID)+'~'+ltrim(rtrim(DPAccounts_DPID))+'~'+ltrim(rtrim(DPAccounts_ClientID))+'~'+ltrim(rtrim(DPAccounts_CMBPID)) as ID,ltrim(rtrim(DPAccounts_ShortName))+' [ '+ltrim(rtrim(DPAccounts_ClientID))+' ]'", where);

                if (DT.Rows.Count != 0)
                {

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {

                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion


            #region CDSLHoldingIsin

            if (Request.QueryString["cdslholdingisin"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] date = paramete[0].Split(' ');

                string where = "Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) and Master_CDSLISIN.CDSLISIN_Number=Trans_CdslHolding.CdslHolding_ISIN ";

                where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM'  and '" + paramete[0].ToString() + "'";
                if (paramete[1].ToString() != "All")
                {
                    where = where + " and CdslClients_BOStatus='" + paramete[1].ToString() + "' ";
                }


                if (paramete[4].ToString() == "0")
                {
                    if (paramete[2].ToString() != "")
                    {
                        where = where + " and  CdslClients_BranchID= '" + paramete[2].ToString() + "'    ";
                    }
                }
                else if (paramete[4].ToString() == "1")
                {
                    if (paramete[2].ToString() != "")
                    {
                        where = where + " and  CdslClients_ContactID IN(Select grp_ContactId from tbl_trans_group where grp_groupMaster='" + paramete[2].ToString() + "')";
                    }
                }


                where = where + " and  (CdslHolding_ISIN Like '%" + reqStr + "%' or CDSLISIN_ShortName Like '%" + reqStr + "%') ";



                DT = oDBEngine.GetDataTable("Trans_CdslHolding,Master_CdslClients,Master_CDSLISIN ", "distinct top 10 CDSLISIN_Number  As ID, (CDSLISIN_ShortName + '   [ ' + cast([CDSLISIN_Number]  as varchar(100)) +'' +' ]') as AccName", where);
                // DT = oDBEngine.GetDataTable("Master_CdslClients ", "distinct top 10 CdslClients_BOID  As ID, (CdslClients_FirstHolderName + '   [ ' + cast([CdslClients_BOID]  as varchar(100)) +'' +' ]') as AccName", "(CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }

            #endregion

            #region CDSLTransction

            if (Request.QueryString["CDSLTransction"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] stdate;
                string[] eddate;

                stdate = paramete[0].Split(' ');
                eddate = paramete[1].Split(' ');



                string where = "Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslTransaction_DPID))+ LTRIM(RTRIM(CdslTransaction_BenAccountNumber)) ";

                where = where + "and CdslTransaction_Date between '" + stdate[1] + " " + stdate[2] + " " + stdate[6] + " 00:00:00" + "' and '" + eddate[1] + " " + eddate[2] + " " + eddate[6] + " 23:59:59" + "'";
                if (paramete[2].ToString() != "All")
                {
                    where = where + " and CdslClients_BOStatus='" + paramete[2].ToString() + "' ";
                }

                where = where + "and ( CdslClients_BOID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' )  ";

                //DT = oDBEngine.GetDataTable("Trans_CdslTransaction,Master_CdslClients ", "distinct top 10 CdslClients_BOID  As ID, (CdslClients_FirstHolderName + '   [ ' + cast([CdslClients_BOID]  as varchar(100)) +'' +' ]') as AccName", where);
                //DT = oDBEngine.GetDataTable("Master_CdslClients ", "distinct top 10 CdslClients_BOID  As ID, (CdslClients_FirstHolderName + '   [ ' + cast([CdslClients_BOID]  as varchar(100)) +'' +' ]') as AccName", "(CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%')");
                DT = oDBEngine.GetDataTable("(Select distinct CdslClients_BOID  As ID, CdslClients_FirstHolderName,(select  cnt_ucc from tbl_master_contact where cnt_internalID=(select  N.CDSLClients_ContactID from Master_CDSLClients as N where N.CdslClients_BOID=Master_CdslClients.CdslClients_BOID)) as InterNalID from Master_CdslClients  ) as DD", " top 10 ID,(CdslClients_FirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName", " ID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                // DT = oDBEngine.GetDataTable("Master_CdslClients,tbl_master_contact", " distinct top 10 CdslClients_BOID  As ID,(isnull(CdslClients_FirstHolderName,'') + '   [ ' + cast(CdslClients_BOID  as varchar(100)) +'' +' ]'  + ' ['+(case when CDSLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName ", " (CDSLClients_ContactID=cnt_internalID or CDSLClients_ContactID is null) and (CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }


            #endregion

            #region CDSLTransctionIsin

            if (Request.QueryString["cdslTransctionisin"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] stdate;
                string[] eddate;

                stdate = paramete[0].Split(' ');
                eddate = paramete[1].Split(' ');



                string where = "Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslTransaction_DPID))+ LTRIM(RTRIM(CdslTransaction_BenAccountNumber)) and Master_CDSLISIN.CDSLISIN_Number=Trans_CdslTransaction.CdslTransaction_ISIN ";

                where = where + "and CdslTransaction_Date between '" + stdate[1] + " " + stdate[2] + " " + stdate[6] + " 00:00:00" + "' and '" + eddate[1] + " " + eddate[2] + " " + eddate[6] + " 23:59:59" + "'";
                if (paramete[2].ToString() != "All")
                {
                    where = where + " and CdslClients_BOStatus='" + paramete[2].ToString() + "' ";
                }


                if (paramete[3].ToString() != "")
                {
                    where = where + " and  CdslClients_BOID= '" + paramete[3].ToString() + "'    ";
                }


                where = where + " and  (CdslTransaction_ISIN Like '%" + reqStr + "%' or CDSLISIN_ShortName Like '%" + reqStr + "%') ";



                DT = oDBEngine.GetDataTable("Trans_CdslTransaction,Master_CdslClients,Master_CDSLISIN ", "distinct top 10 CDSLISIN_Number  As ID, (CDSLISIN_ShortName + '   [ ' + cast([CDSLISIN_Number]  as varchar(100)) +'' +' ]') as AccName", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }

            #endregion
            #region MainAccount
            if (Request.QueryString["MainAccount"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                if (param == "C")
                {
                    if (Session["MainAccountContra"] == null)
                    {
                        DT = oDBEngine.GetDataTable("Master_MainAccount", "top 10 MainAccount_Name as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar(20))+'~'+MainAccount_AccountType+'~'+MainAccount_SubLedgerType as CashBank_MainAccountID1", " ((MainAccount_BankCashType='Bank')or (MainAccount_BankCashType='Cash')) and MainAccount_BankCompany='" + Session["LastCompany"].ToString() + "' and MainAccount_Name like '" + reqStr + "%'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Master_MainAccount", "top 10 MainAccount_Name as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar(20))+'~'+MainAccount_AccountType+'~'+MainAccount_SubLedgerType as CashBank_MainAccountID1", " ((MainAccount_BankCashType='Bank')or (MainAccount_BankCashType='Cash')) and MainAccount_BankCompany='" + Session["LastCompany"].ToString() + "' and MainAccount_Name like '" + reqStr + "%' and MainAccount_ReferenceID not in(" + Session["MainAccountContra"].ToString() + ")");
                    }
                }
                else
                {
                    DT = oDBEngine.GetDataTable("Master_MainAccount", "top 10 MainAccount_Name as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar(20))+'~'+MainAccount_AccountType+'~'+MainAccount_SubLedgerType as CashBank_MainAccountID1", " ((MainAccount_BankCashType<>'Bank')and (MainAccount_BankCashType<>'Cash')) and MainAccount_Name like '" + reqStr + "%'");
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion



            #region SubAccount
            if (Request.QueryString["SubAccountMod"] == "1")
            {
                DataSet DS = new DataSet();
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                string[] param1 = param.Split('~');
                if (Session["ExchangeSegmentID"] == null)
                    Session["ExchangeSegmentID"] = "0";
                string Branch = null;
                if (param1[1].ToString() == "N")
                {
                    Branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                }
                else
                    Branch = param1[1].ToString();


                SqlCommand cmd = new SqlCommand("SubAccountSelect", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CashBank_MainAccountID", param1[0].ToString());
                cmd.Parameters.AddWithValue("@clause", reqStr);
                cmd.Parameters.AddWithValue("@branch", Branch);
                cmd.Parameters.AddWithValue("@exchSegment", Session["ExchangeSegmentID"].ToString());
                cmd.Parameters.AddWithValue("@SegmentN", "'" + SegmentName.ToString() + "'");
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(DS);
                cmd.Dispose();
                GC.Collect();

                DT = DS.Tables[0];
                //string[,] Data1 = { { "@CashBank_MainAccountID", SqlDbType.VarChar.ToString(), param1[0].ToString() }, { "@clause", SqlDbType.VarChar.ToString(), reqStr }, { "@branch", SqlDbType.VarChar.ToString(), Branch }, { "@exchSegment", SqlDbType.VarChar.ToString(), Session["ExchangeSegmentID"].ToString() } };
                //DT = oDBEngine.GetDatatable_StoredProcedure("SubAccountSelect", Data1);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                {
                    if (DS.Tables[1].Rows.Count > 0)
                        if (DS.Tables[1].Rows[0][0] == "1")
                            Response.Write("Suspended Client###Suspended Client|");
                        else
                            Response.Write("No Record Found###No Record Found|");
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion


            //Newly added for TDS/TCS sub account on 19.08.2015

            if (Request.QueryString["SubAccount"] == "1")
            {
                DataSet DS = new DataSet();
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                string[] param1 = param.Split('~');
                if (Session["ExchangeSegmentID"] == null)
                    Session["ExchangeSegmentID"] = "0";
                string Branch = null;
                //if (param1[1].ToString() == "N")
                //{
                //    Branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                //}
                //else
                //    Branch = param1[1].ToString();

                Branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();

                SqlCommand cmd = new SqlCommand("SubAccountSelect", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CashBank_MainAccountID", param1[0].ToString());
                cmd.Parameters.AddWithValue("@clause", reqStr);
                cmd.Parameters.AddWithValue("@branch", Branch);
                cmd.Parameters.AddWithValue("@exchSegment", Session["ExchangeSegmentID"].ToString());
                cmd.Parameters.AddWithValue("@SegmentN", "");//"'" + SegmentName.ToString() + "'"
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(DS);
                cmd.Dispose();
                GC.Collect();

                DT = DS.Tables[0];
                //string[,] Data1 = { { "@CashBank_MainAccountID", SqlDbType.VarChar.ToString(), param1[0].ToString() }, { "@clause", SqlDbType.VarChar.ToString(), reqStr }, { "@branch", SqlDbType.VarChar.ToString(), Branch }, { "@exchSegment", SqlDbType.VarChar.ToString(), Session["ExchangeSegmentID"].ToString() } };
                //DT = oDBEngine.GetDatatable_StoredProcedure("SubAccountSelect", Data1);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                {
                    if (DS.Tables[1].Rows.Count > 0)
                        if (DS.Tables[1].Rows[0][0] == "1")
                            Response.Write("Suspended Client###Suspended Client|");
                        else
                            Response.Write("No Record Found###No Record Found|");
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }


            #region CDSLHoldingSettlement
            if (Request.QueryString["CDSLHoldingSettlement"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] date = paramete[0].Split(' ');

                string where = "Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) and Master_CDSLISIN.CDSLISIN_Number=Trans_CdslHolding.CdslHolding_ISIN ";

                where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM'  and '" + paramete[0].ToString() + "'";
                if (paramete[1].ToString() != "All")
                {
                    where = where + " and CdslClients_BOStatus='" + paramete[1].ToString() + "' ";
                }


                if (paramete[2].ToString() != "")
                {
                    where = where + " and  CdslClients_BOID= '" + paramete[2].ToString() + "'    ";
                }

                if (paramete[3].ToString() != "")
                {
                    where = where + " and  CdslHolding_ISIN= '" + paramete[3].ToString() + "'    ";
                }



                where = where + " and  CdslHolding_SettlementID Like '%" + reqStr + "%'  ";



                DT = oDBEngine.GetDataTable("Trans_CdslHolding,Master_CdslClients,Master_CDSLISIN ", "distinct top 10 CdslHolding_SettlementID  As ID, CdslHolding_SettlementID as AccName", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }


            #endregion

            #region CDSLTransctionSettlement

            if (Request.QueryString["cdslTransctionSettlement"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] paramete = Request.QueryString["search_param"].ToString().Split('~');
                string[] stdate;
                string[] eddate;

                stdate = paramete[0].Split(' ');
                eddate = paramete[1].Split(' ');



                string where = "Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslTransaction_DPID))+ LTRIM(RTRIM(CdslTransaction_BenAccountNumber)) and Master_CDSLISIN.CDSLISIN_Number=Trans_CdslTransaction.CdslTransaction_ISIN ";

                where = where + "and CdslTransaction_Date between '" + stdate[1] + " " + stdate[2] + " " + stdate[6] + " 00:00:00" + "' and '" + eddate[1] + " " + eddate[2] + " " + eddate[6] + " 23:59:59" + "'";
                if (paramete[2].ToString() != "All")
                {
                    where = where + " and CdslClients_BOStatus='" + paramete[2].ToString() + "' ";
                }


                if (paramete[3].ToString() != "")
                {
                    where = where + " and  CdslClients_BOID= '" + paramete[3].ToString() + "'    ";
                }

                if (paramete[4].ToString() != "")
                {
                    where = where + " and  CdslTransaction_ISIN= '" + paramete[4].ToString() + "'    ";
                }


                where = where + " and  CdslTransaction_SettlementID Like '%" + reqStr + "%'  ";



                DT = oDBEngine.GetDataTable("Trans_CdslTransaction,Master_CdslClients,Master_CDSLISIN ", "distinct top 10 CdslTransaction_SettlementID  As ID, CdslTransaction_SettlementID as AccName", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }

            #endregion

            #region MainAccount For Journal
            if (Request.QueryString["MainAccountJournal"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_MainAccount", "top 10 MainAccount_Name as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+'~'+MainAccount_SubLedgerType as MainAccount_ReferenceID", " MainAccount_Name like '" + reqStr + "%' and MainAccount_BankCashType not in('Cash','Bank')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region CDSLContactClientNames
            if (Request.QueryString["CDSLContactClientNames"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string paramete = Request.QueryString["search_param"].ToString();
                string where;
                where = " cnt_internalId in (select dpd_cntId from tbl_master_contactDPDetails where RTRIM(LTRIM(dpd_dpCode))+LTRIM(RTRIM(dpd_ClientId))='" + paramete + "' ) and (cnt_firstName Like '%" + reqStr + "%' or cnt_middleName Like '%" + reqStr + "%' or cnt_lastName Like '%" + reqStr + "%' or cnt_UCC Like '%" + reqStr + "%')";
                DT = oDBEngine.GetDataTable(" tbl_master_contact ", "  top 10  cnt_firstName +' '+cnt_middleName+' '+cnt_lastName +'['+ isnull(cnt_UCC,' ') +']' as AccName,cnt_internalId as ID  ", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");


            }
            #endregion
            #region CDSLbranch
            if (Request.QueryString["CDSLbranch"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string paramete = Request.QueryString["search_param"].ToString();
                string where;
                where = " cnt_internalId in (select dpd_cntId from tbl_master_contactDPDetails where RTRIM(LTRIM(dpd_dpCode))+LTRIM(RTRIM(dpd_ClientId))='" + paramete + "' ) and (cnt_firstName Like '%" + reqStr + "%' or cnt_middleName Like '%" + reqStr + "%' or cnt_lastName Like '%" + reqStr + "%' or cnt_UCC Like '%" + reqStr + "%')";
                DT = oDBEngine.GetDataTable(" tbl_master_branch ", "  top 10 ltrim(rtrim(branch_id)) as ID, ltrim(rtrim(branch_description)) +'-'+ ltrim(rtrim(branch_code)) as branch_description ", "branch_description Like '%" + reqStr + "%' or branch_code Like '%" + reqStr + "%' ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");


            }
            #endregion

            #region CDSLClientNames

            if (Request.QueryString["CDSLClientNames"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string paramete = Request.QueryString["search_param"].ToString();
                string where;
                where = " (CdslClients_FirstHolderName Like  '%" + reqStr + "%' or CdslClients_BOID Like  '%" + reqStr + "%')  and substring(CdslClients_BOID,1,8)='" + paramete + "'";
                DT = oDBEngine.GetDataTable(" Master_CdslClients ", "  top 10  CdslClients_BOID  As ID, CdslClients_FirstHolderName + '   [ '+ CdslClients_BOID +' ]' as AccName", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");


            }
            #endregion
            #region settlement
            if (Request.QueryString["settlement"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable(" Master_Settlements ", "Settlements_Number,Settlements_Number ", " Settlements_Number Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");


            }
            # endregion

            #region SearchSettlementWithoutbracket
            if (Request.QueryString["SearchSettlementWithoutbracket"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //DT = oDBEngine.GetDataTable("tbl_master_bank", " top 10 (bnk_bankName + '~' + bnk_branchName + '~' + bnk_internalId) as BranchName, bnk_internalId", " bnk_bankName Like '" + reqStr + "%'");

                //DT = oDBEngine.GetDataTable("(SELECT a.*,b.Cmp_name FROM (SELECT * FROM tbl_master_companyExchange) AS A LEFT OUTER JOIN tbl_master_company AS B ON B.cmp_internalid=A.exch_compId ) AS C LEFT OUTER JOIN tbl_master_exchange AS D ON C.exch_exchId=D.exh_cntId", "c.exch_internalid,c.cmp_name+'--'+d.exh_shortname+'--'+ C.exch_segmentId AS Modified", " c.cmp_name Like '" + reqStr + "%'");
                DT = oDBEngine.GetDataTable("Master_Settlements", "top 10 settlements_ID,isnull(rtrim(settlements_Number),'')+  isnull(rtrim(settlements_typeSuffix),'')  as SettlementName", "settlements_number Like '" + reqStr + "%' or settlements_number+Settlements_TypeSuffix like '" + reqStr + "%' and Settlements_exchangesegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchBankSegmentBranch
            if (Request.QueryString["SearchBankSegmentBranch"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                if (parameter == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", "EXCHANGENAME Like '" + reqStr + "%'");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_id,branch_description+'-'+branch_code", "(branch_description Like '" + reqStr + "%' OR branch_code Like '" + reqStr + "%') and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    DT = oDBEngine.GetDataTable("(Select MainAccount_Name+' [ '+MainAccount_BankAcNumber+' ]' as CashBank_MainAccountID, MainAccount_Name+' [ '+MainAccount_BankAcNumber+' ]' as MainAccount_Name from Master_MainAccount where MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash') as D  ", "top 10 *", "MainAccount_Name Like '" + reqStr + "%'");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion
            #region SearchBankNameFromMainAccount
            if (Request.QueryString["SearchBankNameFromMainAccount"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string CompanyName = Session["LastCompany"].ToString();
                string segid = Session["usersegid"].ToString();
                DT = oDBEngine.GetDataTable(" master_mainaccount ", "MainAccount_AccountCode,MainAccount_AccountCode+' - '+MainAccount_Name+' ['+MainAccount_BankAcNumber+']' ", " (MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash') and (MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_BankCompany='" + CompanyName.ToString() + "'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            if (Request.QueryString["SearchBankNameFromMainAccountForBrs"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();

                if (parameter.ToString().Trim() == "ALL")
                {
                    DT = oDBEngine.GetDataTable(@"(Select MainAccount_AccountCode,MainAccount_AccountCode+' - '+MainAccount_Name+' ['+MainAccount_BankAcNumber+']' BDetail,
                MainAccount_Name,MainAccount_BankAcNumber  from  master_mainaccount Where  MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash') T1 ",
                    " top 10 * ", "MainAccount_AccountCode like ('%" + reqStr + "%') or MainAccount_Name like ('%" + reqStr + "%') or MainAccount_BankAcNumber like ('%" + reqStr + "%')");
                }
                else if (parameter.ToString().Trim() == "Current")
                {
                    DT = oDBEngine.GetDataTable(@"(Select MainAccount_AccountCode,MainAccount_AccountCode+' - '+MainAccount_Name+' ['+MainAccount_BankAcNumber+']' BDetail,
                MainAccount_Name,MainAccount_BankAcNumber  from  master_mainaccount Where  MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash'
                and MainAccount_BankCompany='" + Session["LastCompany"].ToString() + "') T1 ",
                    " top 10 * ", "MainAccount_AccountCode like ('%" + reqStr + "%') or MainAccount_Name like ('%" + reqStr + "%') or MainAccount_BankAcNumber like ('%" + reqStr + "%')");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(@"(Select MainAccount_AccountCode,MainAccount_AccountCode+' - '+MainAccount_Name+' ['+MainAccount_BankAcNumber+']' BDetail,
                MainAccount_Name,MainAccount_BankAcNumber  from  master_mainaccount Where  MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash'
                and MainAccount_BankCompany=" + parameter.ToString() + ") T1 ",
                    " top 10 * ", "MainAccount_AccountCode like ('%" + reqStr + "%') or MainAccount_Name like ('%" + reqStr + "%') or MainAccount_BankAcNumber like ('%" + reqStr + "%')");
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchMainAccountBranchSegment
            if (Request.QueryString["SearchMainAccountBranchSegment"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                if (parameter == "Segment")
                {
                    if (Session["LastCompany"] == null)
                    {
                        DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + ' - ' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", "EXCHANGENAME Like '" + reqStr + "%'");

                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + ' - ' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", "EXCHANGENAME Like '" + reqStr + "%'");
                    }
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_id,branch_description+'-'+branch_code", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%') and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "MainAcc")
                {
                    DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_AccountCode,MainAccount_Name + ' ['+MainAccount_AccountCode+']' as MainAccount_Name", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) and MainAccount_SubLedgerType<>'None'");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }

                else if (parameter == "Ac Name")
                {
                    DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_AccountCode,MainAccount_Name", "MainAccount_Name Like '" + reqStr + "%' and MainAccount_AccountCode not in('SYSTM00001','SYSTM00002')");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "Ac Name1")
                {
                    DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_AccountCode+'~'+MainAccount_SubLedgerType,MainAccount_Name", "MainAccount_Name Like '" + reqStr + "%' and MainAccount_AccountCode not in('SYSTM00001','SYSTM00002')");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "EntryUser")
                {
                    DT = oDBEngine.GetDataTable("select top 10 user_id,ltrim(rtrim(user_name))+ ' [ '+ltrim(rtrim(user_loginId))+' ]' as textfield from tbl_master_user where (user_loginId like '" + reqStr + "%' or user_name like '" + reqStr + "%')");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }

            }
            #endregion
            #region selectSubAccountForMainAccount
            if (Request.QueryString["selectSubAccountForMainAccount"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                if (parameter == "0")
                {
                    DT = oDBEngine.GetDataTable("(SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers') ) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid  ", "distinct top 10 (ISNULL(ltrim(rtrim(b.cnt_firstname)),'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) +' ['+isnull(ltrim(rtrim(b.cnt_UCC)),'')+']' as Contact_Name,b.cnt_internalId as SubAccount_ReferenceID ", "b.cnt_firstname like '" + reqStr + "%' or b.cnt_UCC like '" + reqStr + "%'");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_id,branch_description+'-'+branch_code", "branch_description Like '" + reqStr + "%' and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "")
                {
                    DT = oDBEngine.GetDataTable("master_subaccount ", "distinct top 10 SubAccount_Code,SubAccount_Name+' ['+SubAccount_Code+']' as SubAccount_Name ", "(SubAccount_Name like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%')");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    if (parameter == "('SYSTM00042')")
                    {
                        DT = oDBEngine.GetDataTable("(Select  cdslclients_benaccountnumber,ltrim(rtrim(cdslclients_firstholdername))+' ['+cast(cdslclients_benaccountnumber as varchar)+']' as cdslclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=CdslClients_ContactID) as UCC 	from master_cdslclients  ) as DD ", " top 10 cdslclients_benaccountnumber,cdslclients_firstholdername", " cdslclients_firstholdername like '" + reqStr + "%' or cdslclients_benaccountnumber like '%" + reqStr + "%' or UCC like '" + reqStr + "%'");
                    }
                    else if (parameter == "('SYSTM00043')")
                    {
                        DT = oDBEngine.GetDataTable("(Select  nsdlclients_benaccountid,ltrim(rtrim(nsdlclients_benfirstholdername))+' ['+cast(nsdlclients_benaccountid as varchar)+']' as nsdlclients_benfirstholdername,(select cnt_ucc from tbl_master_contact where cnt_internalID=NSDLClients_ContactID) as UCC from master_nsdlclients  ) as DD  ", "top 10 nsdlclients_benaccountid,nsdlclients_benfirstholdername ", "nsdlclients_benfirstholdername like '" + reqStr + "%' or nsdlclients_benaccountid like '%" + reqStr + "%' or UCC like '" + reqStr + "%'");

                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_subaccount ", "distinct top 10 SubAccount_Code,SubAccount_Name+' ['+SubAccount_Code+']' as SubAccount_Name ", "(SubAccount_Name like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%') and SubAccount_MainAcReferenceId in " + parameter + "");
                    }
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }

            if (Request.QueryString["selectSubAccountForMainAccountAndBranch"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                string[] CheckParameter = parameter.ToString().Split('~');
                if (parameter == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_id,branch_description+'-'+branch_code", "branch_description Like '" + reqStr + "%' and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                //FOR NSECM AND NSCFO
                if (CheckParameter[0].ToString() == "7" || CheckParameter[0].ToString() == "8")
                {
                    if (CheckParameter[1].ToString() == "")
                    {

                        DT = oDBEngine.GetDataTable("(SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers') ) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid  ", "distinct top 10 (ISNULL(ltrim(rtrim(b.cnt_firstname)),'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) +' ['+isnull(ltrim(rtrim(b.cnt_UCC)),'')+']' as Contact_Name,b.cnt_internalId as SubAccount_ReferenceID ", "(b.cnt_firstname like '" + reqStr + "%' or b.cnt_UCC like '" + reqStr + "%')");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("(SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers') ) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid  ", "distinct top 10 (ISNULL(ltrim(rtrim(b.cnt_firstname)),'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) +' ['+isnull(ltrim(rtrim(b.cnt_UCC)),'')+']' as Contact_Name,b.cnt_internalId as SubAccount_ReferenceID ", "(b.cnt_firstname like '" + reqStr + "%' or b.cnt_UCC like '" + reqStr + "%') and cnt_branchid in" + CheckParameter[1].ToString() + "");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                }

                //FOR CDSL
                if (CheckParameter[0].ToString() == "10")
                {
                    if (CheckParameter[1].ToString() == "")
                    {

                        //DT = oDBEngine.GetDataTable("(SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers') ) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid  ", "distinct top 10 (ISNULL(ltrim(rtrim(b.cnt_firstname)),'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) +' ['+isnull(ltrim(rtrim(b.cnt_UCC)),'')+']' as Contact_Name,b.cnt_internalId as SubAccount_ReferenceID ", "(b.cnt_firstname like '" + reqStr + "%' or b.cnt_UCC like '" + reqStr + "%')");
                        DT = oDBEngine.GetDataTable("master_cdslclients", "top 10 cdslclients_firstholdername+'['+cdslclients_benaccountnumber+']',cdslclients_benaccountnumber", "(cdslclients_firstholdername like '" + reqStr + "%' or cdslclients_benaccountnumber like '" + reqStr + "%')", "cdslclients_benaccountnumber");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_cdslclients", "top 10 cdslclients_firstholdername+'['+cdslclients_benaccountnumber+']',cdslclients_benaccountnumber", "(cdslclients_firstholdername like '" + reqStr + "%' or cdslclients_benaccountnumber like '" + reqStr + "%') and cdslclients_branchid in " + CheckParameter[1].ToString() + "", "cdslclients_benaccountnumber");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                }

                //FOR NSDL
                if (CheckParameter[0].ToString() == "9")
                {
                    if (CheckParameter[1].ToString() == "")
                    {

                        //DT = oDBEngine.GetDataTable("(SELECT * FROM tbl_master_contact WHERE cnt_contactType=(SELECT cnt_prefix FROM tbl_master_contactType WHERE cnttpy_contactType='Customers') ) AS b Left outer JOIN Master_SubAccount ON Master_SubAccount.subAccount_code=b.cnt_internalid  ", "distinct top 10 (ISNULL(ltrim(rtrim(b.cnt_firstname)),'') + ' '+ ISNULL(b.cnt_middlename,'')+ ' ' + ISNULL(b.cnt_lastName,'')) +' ['+isnull(ltrim(rtrim(b.cnt_UCC)),'')+']' as Contact_Name,b.cnt_internalId as SubAccount_ReferenceID ", "(b.cnt_firstname like '" + reqStr + "%' or b.cnt_UCC like '" + reqStr + "%')");
                        DT = oDBEngine.GetDataTable("master_nsdlclients", "top 10 ltrim(rtrim(nsdlclients_benfirstholdername))+'['+ cast(nsdlclients_benaccountid as varchar)+']',nsdlclients_benaccountid", "(nsdlclients_benfirstholdername like '" + reqStr + "%' or nsdlclients_benaccountid like '" + reqStr + "%')", "nsdlclients_benaccountid");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_nsdlclients", "top 10 ltrim(rtrim(nsdlclients_benfirstholdername))+'['+ cast(nsdlclients_benaccountid as varchar)+']',nsdlclients_benaccountid", "(nsdlclients_benfirstholdername like '" + reqStr + "%' or nsdlclients_shortname like '" + reqStr + "%') and nsdlclients_branchid in " + CheckParameter[1].ToString() + "", "nsdlclients_benaccountid");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                }

            }
            #endregion

            #region cdslBillClientSelection
            if (Request.QueryString["cdslBillClientSelection"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                DT = oDBEngine.GetDataTable(" Master_CdslClients", "distinct top 10 CdslClients_FirstHolderName +'['+CdslClients_BOID+']' as name, right(CdslClients_BOID,8) as ID", "(CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");



            }

            #endregion

            #region Vendor Name  VendorName
            if (Request.QueryString["VendorName"] == "1")
            {
                //string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalid,cnt_firstName+''+cnt_lastName as VendorName", "cnt_ContactType='VR' And cnt_firstName like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            //#region DPName
            //if (Request.QueryString["DPName"] == "1")
            //{
            //    string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
            //    DT = oDBEngine.GetDataTable("(select top 10 DP_DepositoryID,DP_Name+' ['+DP_DepositoryID+']' as DPName,DP_DepositoryID+' ['+DP_Name+']' as DP_ID,(select ascii('" + reqStr + "')) as ID from Master_DP where DP_Name like '" + reqStr + "%' or DP_DepositoryID like '" + reqStr + "%') as D", "case when ID between '97' and '122' then DPName else DP_ID end as Name,DP_DepositoryID", null);
            //    if (DT.Rows.Count != 0)
            //    {
            //        for (int i = 0; i < DT.Rows.Count; i++)
            //        {
            //            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
            //        }
            //    }
            //    else
            //        Response.Write("No Record Found###No Record Found|");
            //}
            //#endregion

            #endregion

            #region ServiceProvider Name  ServiceProviderName
            if (Request.QueryString["ServiceProviderName"] == "1")
            {
                //string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalid,cnt_firstName+''+cnt_lastName as ServiceProviderName", "cnt_ContactType='VR' And cnt_firstName like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region InsurerName InsurerName
            if (Request.QueryString["InsurerName"] == "1")
            {
                //string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalid,cnt_firstName+''+cnt_lastName as InsurerName", "cnt_ContactType='VR' And cnt_firstName like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region Location Name  LocationName
            if (Request.QueryString["LocationName"] == "1")
            {
                //string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
                DT = oDBEngine.GetDataTable("tbl_master_branch", "top 10 branch_id,branch_description+'('+branch_code+')' as BranchName", "branch_description like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    //DataRow[] dr = DT.Select("branch_id=Max(branch_id)");
                    //int Imaxid = Convert.ToInt32(dr[0].ItemArray[0]);
                    //int Mmaxid = Imaxid + 1;
                    //DT.Rows.Add(Mmaxid, "ALL");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                {
                    DT.Rows.Add(1, "ALL");
                    DT.Rows.Add(2, "No Record Found");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
            }
            #endregion


            #region Used By  UsedBy
            if (Request.QueryString["UsedBy"] == "1")
            {
                //string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalid,cnt_firstName+''+cnt_lastName as UsedBy", "cnt_firstName like '" + reqStr + "%' And  cnt_firstName IN ( SELECT cnt_firstName FROM tbl_master_contact WHERE cnt_ContactType='EM' or cnt_ContactType='SB' or cnt_ContactType='BP' or cnt_ContactType='FR' or cnt_ContactType='RA')");
                if (DT.Rows.Count != 0)
                {
                    //DataRow[] dr2 = DT.Select("cnt_internalid=Max(cnt_internalid)");
                    //string Umaxid = dr2[0].ItemArray[0].ToString();
                    //DT.Rows.Add(Umaxid, "ALL");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                {
                    DT.Rows.Add("ALL", "ALL");
                    DT.Rows.Add("No Record Found", "No Record Found");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
            }
            #endregion

            #region SearchForTradeRegister
            if (Request.QueryString["SearchForTradeRegister"] == "1")
            {

                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                string[] idlist = parameter.Split('~');

                if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select exch_internalId,(select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId as Comp from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' ) as D ", "top 10 *", "Comp Like '" + reqStr + "%'");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_id,branch_description+'-'+branch_code", "branch_description Like '" + reqStr + "%' and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (idlist[0] == "Clients")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact", "  distinct top 10 (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name,contact.cnt_internalId", "cnt_branchid in(" + Session["userbranchHierarchy"] + ") and (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%')");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else
                    {
                        //DT = oDBEngine.GetDataTable("tbl_master_contact contact,Trans_ComCustomerTrades", "  distinct top 10 (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name,contact.cnt_internalId", "contact.cnt_internalId=ComCustomerTrades_CustomerID and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%')");
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID  ", "cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");

                    }

                }
                else if (idlist[0] == "Instruments")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                            DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");

                        else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4")
                            DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_tickercode),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");

                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        //DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 (case when Equity_StrikePrice=0.0 then isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']' else isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']'+'['+  cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");
                        DT = oDBEngine.GetDataTable("(select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity,Trans_CustomerTrades  WHERE CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");

                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Master_Commodity,Trans_ComCustomerTrades ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']'+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqStr + "%'  and ComCustomerTrades_ProductSeriesID=Commodity_ProductSeriesID and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");

                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }

                }
                else if (idlist[0] == "SettlementNo")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Number)),'')", "Settlements_StartDateTime between '" + idlist[1] + "' and '" + idlist[2] + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Settlements_Number Like '" + reqStr + "%' ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (idlist[0] == "SettlementType")
                {

                    if (idlist[4] == "All")
                    {
                        DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_StartDateTime between '" + idlist[1] + "' and '" + idlist[2] + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");

                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_StartDateTime between '" + idlist[1] + "' and '" + idlist[2] + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Settlements_Number in(" + idlist[3] + ") and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }

                }

            }
            #endregion
            #region SelectMainAccountName
            if (Request.QueryString["SelectMainAccountName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_AccountCode,MainAccount_Name + ' ['+MainAccount_AccountCode+']' as MainAccount_Name", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) and MainAccount_SubLedgerType<>'None'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region       MainAccountName For Only Fixed Assets
            if (Request.QueryString["AssetMain"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_AccountCode,MainAccount_Name", "MainAccount_AccountGroup='fixed Assets' and MainAccount_AccountCode Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    //DataRow[] dr3 = DT.Select("MainAccount_AccountCode=Max(MainAccount_AccountCode)");
                    //string Umaxid = dr3[0].ItemArray[0].ToString();
                    //DT.Rows.Add("ALL", Umaxid);
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                {
                    //DataRow[] dr3 = DT.Select("MainAccount_AccountCode=Max(MainAccount_AccountCode)");
                    //string Umaxid = dr2[0].ItemArray[0].ToString();
                    DT.Rows.Add("ALL", "ALL");
                    DT.Rows.Add("No Record Found", "No Record Found");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                    //Response.Write("ALL");
                    //Response.Write("No Record Found###No Record Found|");
                }

            }

            #endregion
            #region   SubAccount Name Respective MainAccountName
            if (Request.QueryString["AssetSub"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_SubAccount S,Master_MainAccount M ", "top 10 S.SubAccount_MainAcReferenceID,S.SubAccount_Code", "M.MainAccount_SubLedgerType='Custom' And S.SubAccount_MainAcReferenceID=M.MainAccount_AccountCode AND S.SubAccount_MainAcReferenceID='" + param + "' and S.SubAccount_Code Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    //DataRow[] dr4 = DT.Select("SubAccount_MainAcReferenceID=Max(SubAccount_MainAcReferenceID)");
                    //string Umaxid = dr4[0].ItemArray[0].ToString();
                    //DT.Rows.Add(Umaxid, "ALL");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                {
                    DT.Rows.Add("ALL", "ALL");
                    DT.Rows.Add("No Record Found", "No Record Found");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
            }
            #endregion
            #region SearchTransInsurance
            if (Request.QueryString["SearchTransInsurance"] == "1")
            {
                string wherecon = "";
                string reqStr = Request.QueryString["letters"].ToString();
                string[] param = Request.QueryString["search_param"].ToString().Split('~');
                switch (param[0])
                {
                    case "Branch":
                        DT = oDBEngine.GetDataTable(" tbl_master_branch ", " top 10 branch_id,branch_description+'['+branch_code+']' ", " branch_description like '" + reqStr + "%' or branch_code like '" + reqStr + "%'", " branch_description ");
                        break;
                    case "Clients":
                        if (param.Length > 1)
                            wherecon = " and cnt_branchid in (" + param[1] + ")";
                        //DT = oDBEngine.GetDataTable("tbl_master_contact contact,Trans_CustomerTrades", " distinct top 10 contact.cnt_internalId, (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name", "contact.cnt_internalId=CustomerTrades_CustomerID and (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%') " + wherecon);
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact ", " distinct top 10 contact.cnt_internalId, (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name", " (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%') " + wherecon);
                        break;
                    case "Ins.Comp":
                        string cond = "";
                        if (param[1] == "Insurance-Life")
                            cond = " and insu_InsuranceCompType ='Life Insurers' ";
                        else if (param[1] == "Insurance-General")
                            cond = " and insu_InsuranceCompType ='Non-Life Insurers' ";
                        DT = oDBEngine.GetDataTable(" tbl_master_insurerName ", " top 10 insu_internalId,insu_nameOfCompany", " insu_nameOfCompany like'" + reqStr + "%'" + cond);
                        break;
                    case "Products":
                        if (param.Length > 1)
                            wherecon = " and  prd_insurerName in (" + param[1] + ")";
                        DT = oDBEngine.GetDataTable("tbl_master_products,tbl_master_productsDetails", "Top 10 prds_internalId,prds_description AS product ", " prd_internalId=prds_internalId and prds_internalid like 'in%' And prds_description LIKE '" + reqStr + "%' " + wherecon);
                        break;
                    case "TeleCaller":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 e.emp_contactid,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid");
                        break;
                    case "Sales Rep.":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 e.emp_contactid,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid");
                        break;
                    case "Associate":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 cnt_internalID,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and contact.cnt_ContactType in ('RA','BP')");
                        break;
                    case "Sub Broker":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 cnt_internalID,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and contact.cnt_ContactType in ('SB','FR')");
                        break;
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchTransInsuranceWithAll
            if (Request.QueryString["SearchTransInsuranceWithAll"] == "1")
            {
                string wherecon = "";
                string reqStr = Request.QueryString["letters"].ToString();
                string[] param = Request.QueryString["search_param"].ToString().Split('~');
                switch (param[0])
                {
                    case "Branch":
                        DT = oDBEngine.GetDataTable(" tbl_master_branch ", " top 10 branch_id,branch_description+'['+branch_code+']' ", " branch_description like '" + reqStr + "%' or branch_code like '" + reqStr + "%'", " branch_description ");
                        break;
                    case "Clients":
                        if (param.Length > 1)
                            wherecon = " and cnt_branchid in (" + param[1] + ")";
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,Trans_CustomerTrades", " distinct contact.cnt_internalId, (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name", "contact.cnt_internalId=CustomerTrades_CustomerID and (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%') " + wherecon);
                        break;
                    case "Ins.Comp":
                        DT = oDBEngine.GetDataTable(" tbl_master_insurerName ", " insu_internalId,insu_nameOfCompany", " insu_nameOfCompany like'" + reqStr + "%'");
                        break;
                    case "Products":
                        if (param.Length > 1)
                            wherecon = " and  prd_insurerName in (" + param[1] + ")";
                        DT = oDBEngine.GetDataTable("tbl_master_products,tbl_master_productsDetails", "Top 10 prds_internalId,prds_description AS product ", " prd_internalId=prds_internalId and prds_description LIKE '" + reqStr + "%' " + wherecon);
                        break;
                    case "TeleCaller":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 e.emp_contactid,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid");
                        break;
                    case "Sales Rep.":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 e.emp_contactid,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid");
                        break;
                    case "Associate":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 cnt_internalID,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and contact.cnt_ContactType in ('RA','BP')");
                        break;
                    case "Sub Broker":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact", " top 10 cnt_internalID,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and contact.cnt_ContactType in ('SB','FR')");
                        break;
                }
                if (DT.Rows.Count != 0)
                {
                    Response.Write("All###All|");
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchForTradeProcessing
            if (Request.QueryString["SearchForTradeProcessing"] == "1")
            {

                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();


                if (parameter == "Clients")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,Trans_ExchangeTrades", "  distinct top 10 (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name,contact.cnt_internalId", "ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and contact.cnt_internalId=ExchangeTrades_CustomerID  and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%')");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,Trans_ComExchangeTrades", "  distinct top 10 (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name,contact.cnt_internalId", "ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and contact.cnt_internalId=ComExchangeTrades_CustomerID and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%')");
                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }


                }
                else if (parameter == "Instruments")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                            DT = oDBEngine.GetDataTable("Master_Equity,Trans_ExchangeTrades ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and ExchangeTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")   and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' ");
                        else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4")
                            DT = oDBEngine.GetDataTable("Master_Equity,Trans_ExchangeTrades ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_tickercode),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and ExchangeTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")   and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' ");

                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Master_Equity,Trans_ExchangeTrades ", " distinct top 10 (case when Equity_StrikePrice=0.0 then isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']' else isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']'+'['+  cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and ExchangeTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' ");

                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Master_Commodity,Trans_ComExchangeTrades ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']'+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqStr + "%'  and ComExchangeTrades_ProductSeriesID=Commodity_ProductSeriesID and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' ");

                        if (DT.Rows.Count != 0)
                        {
                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                            }
                        }
                        else
                            Response.Write("No Record Found###No Record Found|");
                    }

                }

                else if (parameter == "TerminalId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades,Master_TradingTerminal,tbl_master_contact", " distinct top 10 (isnull(rtrim(ExchangeTrades_TerminalID),'')+' - '+isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_shortName),'')+']' as Name,ExchangeTrades_TerminalID", "(ExchangeTrades_TerminalID like  '" + reqStr + "%' or cnt_firstName like '" + reqStr + "%' or  cnt_shortName like '" + reqStr + "%')  and cast(ExchangeTrades_TerminalID as char)=TradingTerminal_TerminalID and TradingTerminal_ContactID=cnt_internalId");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "CTCLID")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades,Master_TradingTerminal,tbl_master_contact", " distinct top 10 (isnull(rtrim(ExchangeTrades_TerminalID),'')+' - '+isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_shortName),'')+']' as Name,ExchangeTrades_TerminalID", "(ExchangeTrades_TerminalID like  '" + reqStr + "%' or cnt_firstName like '" + reqStr + "%' or  cnt_shortName like '" + reqStr + "%')  and cast(ExchangeTrades_TerminalID as char)=TradingTerminal_TerminalID and TradingTerminal_ContactID=cnt_internalId");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion
            #region SerachDefault
            if (Request.QueryString["SerachDefault"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_ChargeGroup", "top 10 rtrim(ChargeGroup_Name)+' ['+ltrim(rtrim(ChargeGroup_Code))+']' as ChargeGroup_Name,ChargeGroup_Code ", " ltrim(rtrim(ChargeGroup_Code)) in(select distinct ltrim(rtrim(brokerageMain_customerid)) from config_brokerageMain where brokerageMain_Type='G' and brokerageMain_SegmentID in(select exch_internalId from (select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS segment_name from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as D where segment_Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"].ToString() + ")) and brokerageMain_CompanyID='" + Session["LastCompany"].ToString() + "') and (ChargeGroup_Name like '" + reqStr + "%' or ChargeGroup_Code like '" + reqStr + "%') and ChargeGroup_Type=1");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region MainAccountForTDS
            if (Request.QueryString["MainAccountForTDS"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_MainAccount", " top 10 MainAccount_Name+' ['+MainAccount_AccountCode+']',MainAccount_AccountCode+'~'+MainAccount_SubLedgerType+'~'+cast(MainAccount_ReferenceID as varchar)", " MainAccount_Name like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchEmployeesForDigitalSignatureUser
            if (Request.QueryString["SearchEmployeesForDigitalSignatureUser"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                // DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e, tbl_master_user u", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,u.user_id", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid and e.emp_contactId=u.user_contactId");
                DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e, tbl_master_user u", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+u.user_loginId+']') as Name ,u.user_id", " (contact.cnt_firstName Like '" + reqStr + "%' or contact.cnt_middleName Like '" + reqStr + "%' or contact.cnt_lastName Like '" + reqStr + "%' or u.user_loginId Like '" + reqStr + "%') and e.emp_contactId=contact.cnt_internalid and e.emp_contactId=u.user_contactId");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchEmployeesForDigitalSignature
            if (Request.QueryString["SearchEmployeesForDigitalSignature"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //  DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e, tbl_master_user u", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid and e.emp_contactId=u.user_contactId");
                DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchTdsTcsCode
            if (Request.QueryString["SearchTdsTcsCode"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_tdstcs", " top 10 ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']',ltrim(rtrim(tdstcs_code))", " tdstcs_description Like '" + reqStr + "%' or tdstcs_code Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchEmployeesNameWithDigitalSign
            if (Request.QueryString["SearchEmployeesNameWithDigitalSign"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_DigitalSignature,tbl_master_contact"
                    , " top 10 (cnt_firstName +' '+cnt_middleName+' '+cnt_lastName +'['+cnt_shortName+']') as Name,DigitalSignature_ID"
                    , " cnt_firstName Like '" + reqStr + "%' and cnt_internalid=DigitalSignature_ContactID " +
                    "and '" + Session["userid"] + "' in ( " +
                        "select * from dbo.fnSplitReturnTable(DigitalSignature_AuthorizedUsers,',')) " +
                        " and DigitalSignature_ValidUntil>=cast(convert(varchar(9),getdate(),06) as datetime)");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region Country
            if (Request.QueryString["Country"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_country", " top 10 ltrim(rtrim(cou_country)),ltrim(rtrim(cou_id))", " cou_country Like '" + reqStr + "%' ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region State
            if (Request.QueryString["State"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_state", " top 10 ltrim(rtrim(state)),ltrim(rtrim(id))", " state Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region City
            if (Request.QueryString["City"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_city", " top 10 ltrim(rtrim(city_name)),ltrim(rtrim(city_id))", " city_name Like '" + reqStr + "%' and state_id=" + param + " ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region Area
            if (Request.QueryString["Area"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_area", " top 10 ltrim(rtrim(area_name)),ltrim(rtrim(area_id))", " area_name Like '" + reqStr + "%' and city_id=" + param + " ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByEmployeesSubBroker
            if (Request.QueryString["SearchByEmployeesSubBroker"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact", " top 10 (isnull(ltrim(rtrim(cnt_firstName)),'') +' '+isnull(ltrim(rtrim(cnt_middleName)),'')+' '+isnull(ltrim(rtrim(cnt_lastName)),'') +'['+isnull(ltrim(rtrim(cnt_shortName)),'')+']') as Name ,cnt_internalID", " cnt_firstName Like '" + reqStr + "%' and cnt_ContactType in ('SB')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByContact
            if (Request.QueryString["SearchByContact"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact", " top 10 (isnull(ltrim(rtrim(cnt_firstName)),'') +' '+isnull(ltrim(rtrim(cnt_middleName)),'')+' '+isnull(ltrim(rtrim(cnt_lastName)),'') +'['+isnull(ltrim(rtrim(cnt_ucc)),'')+']') as Name ,cnt_internalID", " cnt_firstName Like '" + reqStr + "%' and cnt_ContactType in ('EM')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByCustodian
            if (Request.QueryString["SearchByCustodian"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_custodians", " top 10 Custodian_name+' ['+isnull(custodian_shortname,'')+']',custodian_internalid", " custodian_name Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByFundManager
            if (Request.QueryString["SearchByFundManager"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_fundmanagers", " top 10 fundmanager_firstname+' '+isnull(fundmanager_middlename,'')+' '+isnull(fundmanager_lastname,''),fundmanager_internalid", " fundmanager_firstname Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchByGroupName
            if (Request.QueryString["SearchByGroupName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("(select (select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalid=tbl_trans_group.grp_contactId) as Name,grp_contactId from tbl_trans_group where ltrim(rtrim(grp_groupType))='Family' ) as D ", " distinct top 10 * ", " Name Like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchCdslSlipNumber
            if (Request.QueryString["SearchCdslSlipNumber"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] param = Request.QueryString["search_param"].ToString().Split('~');

                DT = oDBEngine.GetDataTable(" Trans_DpSlipsUsage ", " distinct top 10 DpSlipsUsage_SlipNumber, DpSlipsUsage_SlipNumber ",
                    "DpSlipsUsage_SlipNumber like '" + reqStr + "%' and DpSlipsUsage_SlipType='" + param[0] + "' and DpSlipsUsage_Status='0' and DpSlipsUsage_DPID='" + param[1] + "'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Slip Already Used or Not Exists###No Slip Already Used or Not Exists|");
            }
            #endregion
            #region Company
            if (Request.QueryString["Company"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
                DT = oDBEngine.GetDataTable(" tbl_master_company ", "top 10 cmp_internalid,cmp_Name ", "  cmp_Name like '" + reqStr + "%' ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region branchCustomer
            if (Request.QueryString["branchCustomer"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string Param = Request.QueryString["search_param"].ToString();
                if (Param == "0")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_branch ", "top 10 branch_id,branch_description+' ['+branch_code+']' ", "  branch_description like '" + reqStr + "%' or branch_code like '" + reqStr + "%'");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_contact ", "top 10 cnt_internalId,isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+ ' ]' ", "  cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchByGroup
            if (Request.QueryString["SearchByGroup"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact", " top 10 (isnull(ltrim(rtrim(cnt_firstName)),'') +' '+isnull(ltrim(rtrim(cnt_middleName)),'')+' '+isnull(ltrim(rtrim(cnt_lastName)),'') +'['+isnull(ltrim(rtrim(cnt_ucc)),'')+']') as Name ,cnt_internalID", " cnt_firstName Like '" + reqStr + "%' and cnt_ContactType in ('EM')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region ClientName
            if (Request.QueryString["ClientSelection"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Convert.ToDateTime(Request.QueryString["search_param"]).ToString("yyyy-MM-dd");
                string[] idlist = parameter.Split('~');


                //            Select C.cnt_firstName+isnull(C.cnt_middleName,'')+cnt_LastName AS CustomerName,CN.ContractNotes_CustomerID from trans_ContractNotes CN,tbl_master_contact C where CN.ContractNotes_TradeDate='2009-12-08 00:00:00.000' And CN.ContractNotes_SegmentId=5 AND CN.ContractNotes_CompanyID='COR0000005'
                //AND C.cnt_internalId=CN.ContractNotes_CustomerID




                //if (idlist[0] == "Segment")
                //{
                DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "distinct top 10 (C.cnt_firstName+' '+isnull(C.cnt_middleName,'')+' '+isnull(C.cnt_LastName,'')+'['+isnull(C.cnt_UCC,'')+']') as CustomerName,cnt_internalID", "CN.ContractNotes_TradeDate='" + parameter + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' AND C.cnt_internalId=CN.ContractNotes_CustomerID AND (C.cnt_firstName Like '" + reqStr + "%'OR C.cnt_UCC Like '" + reqStr + "%')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        //DT.Rows[i][1]= DT.Rows[i][0].ToString() + "~" + DT.Rows[i][1].ToString();
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
                //}
                //else if (idlist[0] == "Branch")
                //{
                //    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_id,branch_description+'-'+branch_code", "branch_description Like '" + reqStr + "%' and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                //    if (DT.Rows.Count != 0)
                //    {
                //        for (int i = 0; i < DT.Rows.Count; i++)
                //        {
                //            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                //        }
                //    }
                //    else
                //        Response.Write("No Record Found###No Record Found|");
                //}
                //else if (idlist[0] == "Clients")
                //{
                //    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                //    {
                //        DT = oDBEngine.GetDataTable("tbl_master_contact contact,Trans_CustomerTrades", "  distinct top 10 (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name,contact.cnt_internalId", "contact.cnt_internalId=CustomerTrades_CustomerID and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%')");
                //        if (DT.Rows.Count != 0)
                //        {
                //            for (int i = 0; i < DT.Rows.Count; i++)
                //            {
                //                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                //            }
                //        }
                //        else
                //            Response.Write("No Record Found###No Record Found|");
                //    }
                //    else
                //    {
                //        DT = oDBEngine.GetDataTable("tbl_master_contact contact,Trans_ComCustomerTrades", "  distinct top 10 (isnull(rtrim(contact.cnt_firstName),'') +' '+isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name,contact.cnt_internalId", "contact.cnt_internalId=ComCustomerTrades_CustomerID and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and (contact.cnt_UCC  Like '" + reqStr + "%' or contact.cnt_firstName Like '" + reqStr + "%')");
                //        if (DT.Rows.Count != 0)
                //        {
                //            for (int i = 0; i < DT.Rows.Count; i++)
                //            {
                //                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                //            }
                //        }
                //        else
                //            Response.Write("No Record Found###No Record Found|");

                //    }

                //}
                //else if (idlist[0] == "Instruments")
                //{
                //    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1")
                //    {
                //        DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");
                //        if (DT.Rows.Count != 0)
                //        {
                //            for (int i = 0; i < DT.Rows.Count; i++)
                //            {
                //                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                //            }
                //        }
                //        else
                //            Response.Write("No Record Found###No Record Found|");
                //    }
                //    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                //    {
                //        DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 (case when Equity_StrikePrice=0.0 then isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']' else isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']'+'['+  cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");

                //        if (DT.Rows.Count != 0)
                //        {
                //            for (int i = 0; i < DT.Rows.Count; i++)
                //            {
                //                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                //            }
                //        }
                //        else
                //            Response.Write("No Record Found###No Record Found|");
                //    }
                //    else
                //    {
                //        DT = oDBEngine.GetDataTable("Master_Commodity,Trans_ComCustomerTrades ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']'+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqStr + "%'  and ComCustomerTrades_ProductSeriesID=Commodity_ProductSeriesID and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");

                //        if (DT.Rows.Count != 0)
                //        {
                //            for (int i = 0; i < DT.Rows.Count; i++)
                //            {
                //                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                //            }
                //        }
                //        else
                //            Response.Write("No Record Found###No Record Found|");
                //    }

                //}
                //else if (idlist[0] == "SettlementNo")
                //{
                //    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Number)),'')", "Settlements_StartDateTime between '" + idlist[1] + "' and '" + idlist[2] + "' and Settlements_ExchangeSegmentID='"+HttpContext.Current.Session["ExchangeSegmentID"]+"' and Settlements_Number Like '" + reqStr + "%' ");
                //    if (DT.Rows.Count != 0)
                //    {
                //        for (int i = 0; i < DT.Rows.Count; i++)
                //        {
                //            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                //        }
                //    }
                //    else
                //        Response.Write("No Record Found###No Record Found|");
                //}
                //else if (idlist[0] == "SettlementType")
                //{

                //    if (idlist[4] == "All")
                //    {
                //        DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_StartDateTime between '" + idlist[1] + "' and '" + idlist[2] + "' and Settlements_ExchangeSegmentID='"+HttpContext.Current.Session["ExchangeSegmentID"]+"' and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                //        if (DT.Rows.Count != 0)
                //        {
                //            for (int i = 0; i < DT.Rows.Count; i++)
                //            {
                //                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                //            }
                //        }
                //        else
                //            Response.Write("No Record Found###No Record Found|");

                //    }
                //    else
                //    {
                //        DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_StartDateTime between '" + idlist[1] + "' and '" + idlist[2] + "' and Settlements_ExchangeSegmentID='"+HttpContext.Current.Session["ExchangeSegmentID"]+"' and Settlements_Number in(" + idlist[3] + ") and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                //        if (DT.Rows.Count != 0)
                //        {
                //            for (int i = 0; i < DT.Rows.Count; i++)
                //            {
                //                Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                //            }
                //        }
                //        else
                //            Response.Write("No Record Found###No Record Found|");
                //    }

                //}


            }
            #endregion
            #region GetGroupName
            if (Request.QueryString["GetGroupName"] == "1")
            {
                //  string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupMaster", "top 10 gpm_Description,gpm_id ", "  gpm_Description Like '" + reqStr + "%' and gpm_Type in ('Relationship Officer','Relationship Manager') ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupMaster", "top 10 gpm_Description,gpm_id ", "  gpm_Description Like '" + reqStr + "%' ");
                }

                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region Serch Vender, Datavender and Agents
            if (Request.QueryString["VenderAgent"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalid,RTRIM(cnt_firstName)+''+ISNULL(RTRIM(cnt_lastName),'')+' ['+ISNULL(RTRIM(cnt_shortName),'')+']' as VendorName", " cnt_ContactType in ('VR','DV','RC') And cnt_firstName like '" + reqStr + "%'");
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region All Record from contact
            if (Request.QueryString["AllDataContact"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();

                if (Request.QueryString["search_param"] != null)
                {
                    string param = Request.QueryString["search_param"].ToString();
                    DT = oDBEngine.GetDataTable(" (select top 10 cnt_internalid,LTRIM(RTRIM(cnt_firstName))+' '+isnull(LTRIM(RTRIM(cnt_middleName)),'')+' '+LTRIM(RTRIM(isnull(cnt_lastName,'')))+' ['+LTRIM(RTRIM(isnull(cnt_UCC,cnt_shortName)))+']' as UsedBy from tbl_master_contact where cnt_firstName like '" + reqStr + "%' or cnt_UCC like  '" + reqStr + "%' or  cnt_shortName like  '" + reqStr + "%'   union select cast(femrel_id as varchar(10)),femrel_memberName from tbl_master_contactFamilyRelationship where femrel_memberName like '" + reqStr + "%' and femrel_cntid='" + param + "' ) as D", " * ", null);
                }
                else
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalid,cnt_internalid,LTRIM(RTRIM(cnt_firstName))+' '+isnull(LTRIM(RTRIM(cnt_middleName)),'')+' '+LTRIM(RTRIM(isnull(cnt_lastName,'')))+' ['+LTRIM(RTRIM(isnull(cnt_UCC,cnt_shortName)))+']' as UsedBy ", " cnt_firstName like '" + reqStr + "%'  or cnt_UCC like  '" + reqStr + "%' or  cnt_shortName like  '" + reqStr + "%'  ");
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region Family Members
            if (Request.QueryString["AllFamily"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString().Trim().ToLower();
                DT = oDBEngine.GetDataTable(" tbl_master_contactFamilyRelationship ", "top 10 femrel_id,femrel_memberName ", " femrel_cntID='" + param + "' and femrel_memberName like '" + reqStr + "%' ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region GetNameForEmail
            if (Request.QueryString["GetNameForEmail"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (param == "EM" || param == "RA")
                    DT = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", " TOP 10 CNT_INTERNALID , LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_SHORTNAME,CNT_UCC)))+'] '", " (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%') AND CNT_INTERNALID LIKE  '" + param + "%' ");
                else if (param == "CD")
                    DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS", " TOP 10 CDSLCLIENTS_BOID,LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ' ", " (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') ");
                else if (param == "ND")
                    DT = oDBEngine.GetDataTable("MASTER_NSDLCLIENTS  ", " TOP 10 NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ,LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']'  ", "( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%')");
                else
                    DT = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", " TOP 10 CNT_INTERNALID , LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+'] '", " (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%') AND CNT_INTERNALID LIKE  '" + param + "%' ");

                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchUnderLying
            if (Request.QueryString["SearchUnderLying"] == "1")
            {

                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();


                if (parameter == "0")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 isnull(Equity_TickerSymbol,''),Equity_ProductID", "equity_exchsegmentid=1 and Equity_TickerSymbol Like '" + reqStr + "%' ");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 isnull(Equity_TickerSymbol,''),Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_TickerSymbol Like '" + reqStr + "%' ");
                    }
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "1")
                {
                    DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 isnull(Equity_TickerSymbol,''),Equity_ProductID", "(Equity_FOIdentifier='FUTSTK' or Equity_FOIdentifier='OPTSTK') and equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_TickerSymbol Like '" + reqStr + "%' ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "2")
                {
                    DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 isnull(Equity_TickerSymbol,''),Equity_ProductID", "Equity_FOIdentifier='FUTSTK' and equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_TickerSymbol Like '" + reqStr + "%' ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "3")
                {
                    DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 isnull(Equity_TickerSymbol,''),Equity_ProductID", "Equity_FOIdentifier='OPTSTK' and equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_TickerSymbol Like '" + reqStr + "%' ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "4")
                {
                    DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 isnull(Equity_TickerSymbol,''),Equity_ProductID", "(Equity_FOIdentifier='FUTIDX' or Equity_FOIdentifier='OPTIDX') and equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_TickerSymbol Like '" + reqStr + "%' ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "5")
                {
                    DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 isnull(Equity_TickerSymbol,''),Equity_ProductID", "Equity_FOIdentifier='FUTIDX' and equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_TickerSymbol Like '" + reqStr + "%' ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (parameter == "6")
                {
                    DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 isnull(Equity_TickerSymbol,''),Equity_ProductID", "Equity_FOIdentifier='OPTIDX' and equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_TickerSymbol Like '" + reqStr + "%' ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }

            }
            #endregion
            #region GetGroupOwnerName
            if (Request.QueryString["GetGroupOwnerName"] == "1")
           {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();

                if (param == "Sub Broker" || param == "Broker" || param == "Relationship Partner" || param == "Franchisee" || param == "Relationship Manager")
                {
                    //DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalId, isnull(cnt_firstName,'') + ' ' + isnull(cnt_middleName,'') + ' ' + isnull(cnt_lastName,'') + '['+ case when cnt_internalId like 'CL%' then   isnull(cnt_UCC,'') else isnull(cnt_shortname,'') end +']'  AS GroupOwner ", " (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%' )  Order by cnt_firstName ");
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalId, isnull(cnt_firstName,'') + ' ' + isnull(cnt_middleName,'') + ' ' + isnull(cnt_lastName,'') + '['+ case when cnt_internalId like 'CL%' then   isnull(cnt_UCC,'') else isnull(cnt_shortname,'') end +']'  AS GroupOwner ", " cnt_internalId like   (select prefix_Name from tbl_master_prefix where  ltrim(rtrim(prefix_Type))= '" + param + "')+'%' and cnt_firstName like '" + reqStr + "%'  Order by cnt_firstName ");
                    // DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalId, isnull(cnt_firstName,'') + ' ' + isnull(cnt_middleName,'') + ' ' + isnull(cnt_lastName,'') + '['+ case when cnt_internalId like 'CL%' then   isnull(cnt_UCC,'') else isnull(cnt_shortname,'') end +']'  AS GroupOwner ", " cnt_internalId like '" + param + "%'  and cnt_firstName like '" + reqStr + "%'  Order by cnt_firstName ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_internalId, isnull(cnt_firstName,'') + ' ' + isnull(cnt_middleName,'') + ' ' + isnull(cnt_lastName,'') + '['+ case when cnt_internalId like 'CL%' then   isnull(cnt_UCC,'') else isnull(cnt_shortname,'') end +']'  AS GroupOwner ", " (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%' )  Order by cnt_firstName ");
                }

                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region userdetails
            if (Request.QueryString["userdetails"] == "1")
            {
                // string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_user", "top 10 user_contactId,user_name", "user_name like '" + reqStr + "%'  Order by user_name ");
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchClinet
            if (Request.QueryString["SearchClinet"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                string[] idlist = parameter.Split('~');
                if (idlist[1] == "Customer")
                {
                    DT = oDBEngine.GetDataTable("Trans_ComCustomerTrades,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalId=ComCustomerTrades_CustomerID and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and ComCustomerTrades_TradeDate='" + idlist[0] + "' and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }

                    }
                    else
                        Response.Write("Trade Not Process For This Client in This Date ###Trade Not Process For This Client in This Date |");
                }
                if (idlist[1] == "Exchange")
                {
                    DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalId=ComExchangeTrades_CustomerID and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and ComExchangeTrades_TradeDate='" + idlist[0] + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion

            #region SEARCHCLIENTTOFORTRADECHANGE
            if (Request.QueryString["SEARCHCLIENTTOFORTRADECHANGE"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contactexchange,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalid<>'" + parameter + "' and crg_cntid=cnt_internalid and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  and crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId)");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion

            #region SEARCHOTHERFORTRADECHANGE
            if (Request.QueryString["SEARCHOTHERFORTRADECHANGE"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                string[] idlist = parameter.Split('~');

                if (idlist[3] == "Customer")
                {
                    if (idlist[0] == "Instruments")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                        {
                            DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),Equity_Tickercode)+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and CustomerTrades_CustomerID='" + idlist[1] + "' and CustomerTrades_TradeDate='" + idlist[2] + "' and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                        {
                            DT = oDBEngine.GetDataTable("(select (case when isnull(Equity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity,Trans_CustomerTrades  WHERE  CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and CustomerTrades_CustomerID='" + idlist[1] + "' and CustomerTrades_TradeDate='" + idlist[2] + "' and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "')as tb", " distinct top 12 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");
                            //DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 (case when Equity_StrikePrice=0.0 then isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']' else isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']'+'['+  cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and CustomerTrades_CustomerID='" + idlist[1] + "' and CustomerTrades_TradeDate='" + idlist[2] + "' and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                        {
                            //DT = oDBEngine.GetDataTable("Master_Commodity,Trans_ComCustomerTrades ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']'+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqStr + "%'  and ComCustomerTrades_ProductSeriesID=Commodity_ProductSeriesID and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ComCustomerTrades_CustomerID='" + idlist[1] + "' and ComCustomerTrades_TradeDate='" + idlist[2] + "' and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  ComCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                            DT = oDBEngine.GetDataTable("(select (case when isnull(Commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(Commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Commodity_TickerCode)),'')+' '+convert(varchar(9),Commodity_ExpiryDate,6) else isnull(rtrim(ltrim(Commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Commodity_TickerCode)),'')+' '+convert(varchar(9),Commodity_ExpiryDate,6)+' '+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_Commodity,Trans_ComCustomerTrades  WHERE  COmCustomerTrades_ProductSeriesID=Commodity_ProductSeriesID and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and COmCustomerTrades_CustomerID='" + idlist[1] + "' and COmCustomerTrades_TradeDate='" + idlist[2] + "' and COmCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and COmCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  COmCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "')as tb", " distinct top 12 TickerSymbol,Commodity_ProductSeriesID", "TickerSymbol like  '" + reqStr + "%'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }
                    else if (idlist[0] == "TerminalId")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                        {
                            DT = oDBEngine.GetDataTable("(select ExchangeTrades_TerminalID,CustTransactionID from Trans_ExchangeTrades," +
                                                        "(select min(ExchangeTrades_ID) as ExchangeTradesID,ExchangeTrades_CustTransactionID" +
                                                        " as CustTransactionID from Trans_ExchangeTrades where " +
                                                        "ExchangeTrades_CustomerID='" + idlist[1] + "' " +
                                                        "and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ExchangeTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'" +
                                                        "group by ExchangeTrades_CustTransactionID) as tab " +
                                                        "where ExchangeTrades_ID=ExchangeTradesID)as tb,Trans_CustomerTrades",
                                                        "distinct top 10 ExchangeTrades_TerminalID",
                                                        "ExchangeTrades_TerminalID like  '" + reqStr + "%'" +
                                                        " and (CustTransactionID=CustomerTrades_ID or  CustomerTrades_OriginalTransactionID=CustTransactionID)" +
                                                        "and CustomerTrades_CustomerID='" + idlist[1] + "'" +
                                                        "and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and CustomerTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("(select ComExchangeTrades_TerminalID,CustTransactionID from Trans_ComExchangeTrades," +
                                                        "(select min(ComExchangeTrades_ID) as ComExchangeTradesID,ComExchangeTrades_CustTransactionID" +
                                                        " as CustTransactionID from Trans_ComExchangeTrades where " +
                                                        "ComExchangeTrades_CustomerID='" + idlist[1] + "' " +
                                                        "and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ComExchangeTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'" +
                                                        "group by ComExchangeTrades_CustTransactionID) as tab " +
                                                        "where ComExchangeTrades_ID=ComExchangeTradesID)as tb,Trans_ComCustomerTrades",
                                                        "distinct top 10 ComExchangeTrades_TerminalID",
                                                        "ComExchangeTrades_TerminalID like  '" + reqStr + "%'" +
                                                        " and (CustTransactionID=ComCustomerTrades_ID or  ComCustomerTrades_OriginalTransactionID=CustTransactionID)" +
                                                        "and ComCustomerTrades_CustomerID='" + idlist[1] + "'" +
                                                        "and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ComCustomerTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ComCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ComCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }
                    else if (idlist[0] == "CTCLId")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                        {
                            DT = oDBEngine.GetDataTable("(select ExchangeTrades_CTCLID,CustTransactionID from Trans_ExchangeTrades," +
                                                        "(select min(ExchangeTrades_ID) as ExchangeTradesID,ExchangeTrades_CustTransactionID" +
                                                        " as CustTransactionID from Trans_ExchangeTrades where " +
                                                        " ExchangeTrades_CustomerID='" + idlist[1] + "' " +
                                                        "and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ExchangeTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'" +
                                                        "group by ExchangeTrades_CustTransactionID) as tab " +
                                                        "where ExchangeTrades_ID=ExchangeTradesID)as tb,Trans_CustomerTrades",
                                                        "distinct top 10 ExchangeTrades_CTCLID",
                                                        "ExchangeTrades_CTCLID like  '" + reqStr + "%'" +
                                                        " and (CustTransactionID=CustomerTrades_ID or  CustomerTrades_OriginalTransactionID=CustTransactionID)" +
                                                        "and CustomerTrades_CustomerID='" + idlist[1] + "'" +
                                                        "and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and CustomerTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                        {
                            if (idlist[4] == "ALLTER")
                            {
                                DT = oDBEngine.GetDataTable("(select ComExchangeTrades_CTCLID,CustTransactionID from Trans_ComExchangeTrades," +
                                                        "(select min(ComExchangeTrades_ID) as ComExchangeTradesID,ComExchangeTrades_CustTransactionID" +
                                                        " as CustTransactionID from Trans_ComExchangeTrades where " +
                                                        "ComExchangeTrades_CustomerID='" + idlist[1] + "' " +
                                                        "and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ComExchangeTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'" +
                                                        "group by ComExchangeTrades_CustTransactionID) as tab " +
                                                        "where ComExchangeTrades_ID=ComExchangeTradesID)as tb,Trans_ComCustomerTrades",
                                                        "distinct top 10 ComExchangeTrades_CTCLID",
                                                        "ComExchangeTrades_CTCLID like  '" + reqStr + "%'" +
                                                        " and (CustTransactionID=ComCustomerTrades_ID or  ComCustomerTrades_OriginalTransactionID=CustTransactionID)" +
                                                        "and ComCustomerTrades_CustomerID='" + idlist[1] + "'" +
                                                        "and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ComCustomerTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ComCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ComCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            }
                            else
                            {
                                DT = oDBEngine.GetDataTable("(select ComExchangeTrades_CTCLID,CustTransactionID from Trans_ComExchangeTrades," +
                                                        "(select min(ComExchangeTrades_ID) as ComExchangeTradesID,ComExchangeTrades_CustTransactionID" +
                                                        " as CustTransactionID from Trans_ComExchangeTrades where ComExchangeTrades_TerminalID in(" + idlist[4] + ")" +
                                                        "and ComExchangeTrades_CustomerID='" + idlist[1] + "' " +
                                                        "and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ComExchangeTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'" +
                                                        "group by ComExchangeTrades_CustTransactionID) as tab " +
                                                        "where ComExchangeTrades_ID=ComExchangeTradesID)as tb,Trans_ComCustomerTrades",
                                                        "distinct top 10 ComExchangeTrades_CTCLID",
                                                        "ComExchangeTrades_CTCLID like  '" + reqStr + "%'" +
                                                        " and (CustTransactionID=ComCustomerTrades_ID or  ComCustomerTrades_OriginalTransactionID=CustTransactionID)" +
                                                        "and ComCustomerTrades_CustomerID='" + idlist[1] + "'" +
                                                        "and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ComCustomerTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ComCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ComCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            }

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }
                    else if (idlist[0] == "OrderNo")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                        {
                            DT = oDBEngine.GetDataTable("(select ExchangeTrades_OrderNumber,CustTransactionID from Trans_ExchangeTrades," +
                                                        "(select min(ExchangeTrades_ID) as ExchangeTradesID,ExchangeTrades_CustTransactionID" +
                                                        " as CustTransactionID from Trans_ExchangeTrades where " +
                                                        "ExchangeTrades_CustomerID='" + idlist[1] + "' " +
                                                        "and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ExchangeTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'" +
                                                        "group by ExchangeTrades_CustTransactionID) as tab " +
                                                        "where ExchangeTrades_ID=ExchangeTradesID)as tb,Trans_CustomerTrades",
                                                        "distinct top 10 ExchangeTrades_OrderNumber",
                                                        "ExchangeTrades_OrderNumber like  '" + reqStr + "%'" +
                                                        " and (CustTransactionID=CustomerTrades_ID or  CustomerTrades_OriginalTransactionID=CustTransactionID)" +
                                                        "and CustomerTrades_CustomerID='" + idlist[1] + "'" +
                                                        "and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and CustomerTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("(select ComExchangeTrades_OrderNumber,CustTransactionID from Trans_ComExchangeTrades," +
                                                        "(select min(ComExchangeTrades_ID) as ComExchangeTradesID,ComExchangeTrades_CustTransactionID" +
                                                        " as CustTransactionID from Trans_ComExchangeTrades where " +
                                                        "ComExchangeTrades_CustomerID='" + idlist[1] + "' " +
                                                        "and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ComExchangeTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'" +
                                                        "group by ComExchangeTrades_CustTransactionID) as tab " +
                                                        "where ComExchangeTrades_ID=ComExchangeTradesID)as tb,Trans_ComCustomerTrades",
                                                        "distinct top 10 ComExchangeTrades_OrderNumber",
                                                        "ComExchangeTrades_OrderNumber like  '" + reqStr + "%'" +
                                                        " and (CustTransactionID=ComCustomerTrades_ID or  ComCustomerTrades_OriginalTransactionID=CustTransactionID)" +
                                                        "and ComCustomerTrades_CustomerID='" + idlist[1] + "'" +
                                                        "and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") " +
                                                        "and ComCustomerTrades_TradeDate='" + idlist[2] + "' " +
                                                        "and ComCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'" +
                                                        "and ComCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }


                }
                else
                {
                    if (idlist[0] == "Instruments")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                        {
                            DT = oDBEngine.GetDataTable("Master_Equity,Trans_ExchangeTrades ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and ExchangeTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ExchangeTrades_CustomerID='" + idlist[1] + "' and ExchangeTrades_TradeDate='" + idlist[2] + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                        {
                            //DT = oDBEngine.GetDataTable("Master_Equity,Trans_ExchangeTrades ", " distinct top 10 (case when Equity_StrikePrice=0.0 then isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']' else isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']'+'['+  cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and ExchangeTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ExchangeTrades_CustomerID='" + idlist[1] + "' and ExchangeTrades_TradeDate='" + idlist[2] + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                            DT = oDBEngine.GetDataTable("(select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity,Trans_ExchangeTrades  WHERE  ExchangeTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ExchangeTrades_CustomerID='" + idlist[1] + "' and ExchangeTrades_TradeDate='" + idlist[2] + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "')as tb", " distinct top 12 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                        {
                            //DT = oDBEngine.GetDataTable("Master_Commodity,Trans_ComExchangeTrades ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']'+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqStr + "%'  and ComExchangeTrades_ProductSeriesID=Commodity_ProductSeriesID and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ComExchangeTrades_CustomerID='" + idlist[1] + "' and ComExchangeTrades_TradeDate='" + idlist[2] + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                            DT = oDBEngine.GetDataTable("(select (case when isnull(Commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(Commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Commodity_TickerCode)),'')+' '+convert(varchar(9),Commodity_ExpiryDate,6) else isnull(rtrim(ltrim(Commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Commodity_TickerCode)),'')+' '+convert(varchar(9),Commodity_ExpiryDate,6)+' '+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_Commodity,Trans_ComExchangeTrades  WHERE  ComExchangeTrades_ProductSeriesID=Commodity_ProductSeriesID and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and ComExchangeTrades_CustomerID='" + idlist[1] + "' and ComExchangeTrades_TradeDate='" + idlist[2] + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "')as tb", " distinct top 12 TickerSymbol,Commodity_ProductSeriesID", "TickerSymbol like  '" + reqStr + "%'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }
                    else if (idlist[0] == "OrderNo")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                        {
                            DT = oDBEngine.GetDataTable("Trans_ExchangeTrades", "distinct top 10 ExchangeTrades_OrderNumber", "ExchangeTrades_OrderNumber like  '" + reqStr + "%' and ExchangeTrades_CustomerID='" + idlist[1] + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ExchangeTrades_TradeDate='" + idlist[2] + "' and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");


                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades", "distinct top 10 ComExchangeTrades_OrderNumber", "ComExchangeTrades_OrderNumber like  '" + reqStr + "%' and ComExchangeTrades_CustomerID='" + idlist[1] + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_TradeDate='" + idlist[2] + "' and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }
                    else if (idlist[0] == "TerminalId")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                        {
                            DT = oDBEngine.GetDataTable("Trans_ExchangeTrades", "distinct top 10 ExchangeTrades_TerminalID", "ExchangeTrades_TerminalID like  '" + reqStr + "%' and ExchangeTrades_CustomerID='" + idlist[1] + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ExchangeTrades_TradeDate='" + idlist[2] + "' and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");


                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades", "distinct top 10 ComExchangeTrades_TerminalID", "ComExchangeTrades_TerminalID like  '" + reqStr + "%' and ComExchangeTrades_CustomerID='" + idlist[1] + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_TradeDate='" + idlist[2] + "' and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }
                    else if (idlist[0] == "CTCLId")
                    {
                        if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                        {
                            DT = oDBEngine.GetDataTable("Trans_ExchangeTrades", "distinct top 10 ExchangeTrades_CTCLID", "ExchangeTrades_TerminalID in(" + idlist[4] + ") and ExchangeTrades_CTCLID like  '" + reqStr + "%' and ExchangeTrades_CustomerID='" + idlist[1] + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ExchangeTrades_TradeDate='" + idlist[2] + "' and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");


                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                        else
                        {
                            //if (idlist[4] == "ALLTER")
                            //{
                            DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades", "distinct top 10 ComExchangeTrades_CTCLID", "ComExchangeTrades_CTCLID like  '" + reqStr + "%' and ComExchangeTrades_CustomerID='" + idlist[1] + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_TradeDate='" + idlist[2] + "' and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                            //}
                            //else
                            //{
                            //    DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades", "distinct top 10 ComExchangeTrades_CTCLID", "ComExchangeTrades_TerminalID in(" + idlist[4] + ") and ComExchangeTrades_CTCLID like  '" + reqStr + "%' and ComExchangeTrades_CustomerID='" + idlist[1] + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_TradeDate='" + idlist[2] + "' and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                            //}

                            if (DT.Rows.Count != 0)
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                                }
                            }
                            else
                                Response.Write("No Record Found###No Record Found|");
                        }
                    }

                }
            }
            #endregion

            #region SEARCHCLIENTTOFORTRADEENTRY
            if (Request.QueryString["SEARCHCLIENTTOFORTRADEENTRY"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                if (parameter == "ALL")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contactexchange,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "crg_cntid=cnt_internalid and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  and crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId)");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contactexchange,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalid<>'" + parameter + "' and crg_cntid=cnt_internalid and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  and crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId)");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");

                }
            }
            #endregion

            #region SEARCHINSTRUMENTTOFORTRADEENTRY
            if (Request.QueryString["SEARCHINSTRUMENTTOFORTRADEENTRY"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                reqStr = reqStr.Replace("_", "&");
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                {
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1")
                    //    DT = oDBEngine.GetDataTable("Master_Equity ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");

                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4")
                    //    DT = oDBEngine.GetDataTable("Master_Equity ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Tickercode),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");


                    DT = oDBEngine.GetDataTable("Master_Equity ", " distinct top 10 case when equity_exchsegmentid=1 then isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' else isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Tickercode),'')+']' end as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and Equity_ExchSegmentID in(1,4) ");

                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                {
                    //DT = oDBEngine.GetDataTable("Master_Equity ", " distinct top 10 (case when Equity_StrikePrice=0.0 then isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']' else isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']'+'['+  cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_EffectUntil>='" + parameter + "'");
                    DT = oDBEngine.GetDataTable("(select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity  WHERE  Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_EffectUntil>='" + parameter + "')as tb", " distinct top 12 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");

                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    //DT = oDBEngine.GetDataTable("Master_Commodity", " distinct top 10 Commodity_ProductSeriesID,(case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']' else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+'['+convert(varchar(9),Commodity_ExpiryDate,6)+']'+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol", "Commodity_TickerSymbol like  '" + reqStr + "%' and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Commodity_ExpiryDate>='" + parameter + "'");
                    DT = oDBEngine.GetDataTable("(select (case when isnull(commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Commodity_TickerCode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'') else isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Commodity_TickerCode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'')+' '+cast(cast(round(commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_commodity  WHERE commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'  and Commodity_ExpiryDate>='" + parameter + "' )as tb", " distinct top 15 Commodity_ProductSeriesID,TickerSymbol", "TickerSymbol like  '" + reqStr + "%'");

                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");

                }
            }
            #endregion

            #region SearchClinetNSE
            if (Request.QueryString["SearchClinetNSE"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                string[] idlist = parameter.Split('~');
                if (idlist[1] == "Customer")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_CustomerTrades,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalId=CustomerTrades_CustomerID and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and  CustomerTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and CustomerTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and CustomerTrades_TradeDate='" + idlist[0] + "' and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_comCustomerTrades,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalId=comCustomerTrades_CustomerID and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and  comCustomerTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and comCustomerTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and comCustomerTrades_TradeDate='" + idlist[0] + "' and comCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and comCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and comCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                    }
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }

                    }
                    else
                        Response.Write("Trade Not Process For This Client in This Date ###Trade Not Process For This Client in This Date |");
                }
                if (idlist[1] == "Exchange")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalId=ExchangeTrades_CustomerID and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_TradeDate='" + idlist[0] + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalId=ComExchangeTrades_CustomerID and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_TradeDate='" + idlist[0] + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion

            #region SearchClinetsmsemailreport
            if (Request.QueryString["SearchClinetsmsemailreport"] == "1")
            {
                string parameter = Request.QueryString["search_param"].ToString();


                SqlCommand com = new SqlCommand("sp_fetch_report_sms_email_ajax", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@contact", Request.QueryString["letters"].ToString());
                com.Parameters.AddWithValue("@branchid", Session["userbranchHierarchy"].ToString());
                com.Parameters.AddWithValue("@segment", parameter);
                SqlDataAdapter ad = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //Response.Write(dt.Rows[i][0].ToString());
                        Response.Write(dt.Rows[i][1].ToString() + "###" + dt.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion




            #region ShowClientScrip
            if (Request.QueryString["ShowClientScrip"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (param == "Clients")
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                else if (param == "Scrips")
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID in(1,4)");
                else if (param == "ScripsAllSeg")
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID in(1,4)");
                else if (param == "SettType")
                    DT = oDBEngine.GetDataTable("master_settlements", " distinct top 10 rtrim(Settlements_Number)+ltrim(Settlements_TypeSuffix),Settlements_Number", " Settlements_Number like '" + reqStr + "%' and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + "");
                else if (param == "Segment")
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='COR0000001' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                else if (param == "Branch")
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_id,branch_description+'-'+branch_code", "branch_description Like '" + reqStr + "%' and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");
            }
            #endregion



            #region SearchSegName
            if (Request.QueryString["SearchSegName"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();

                //DT = oDBEngine.GetDataTable("tbl_master_bank", " top 10 (bnk_bankName + '~' + bnk_branchName + '~' + bnk_internalId) as BranchName, bnk_internalId", " bnk_bankName Like '" + reqStr + "%'");

                //DT = oDBEngine.GetDataTable("(SELECT a.*,b.Cmp_name FROM (SELECT * FROM tbl_master_companyExchange) AS A LEFT OUTER JOIN tbl_master_company AS B ON B.cmp_internalid=A.exch_compId ) AS C LEFT OUTER JOIN tbl_master_exchange AS D ON C.exch_exchId=D.exh_cntId", "c.exch_internalid,c.cmp_name+'--'+d.exh_shortname+'--'+ C.exch_segmentId AS Modified", " c.cmp_name Like '" + reqStr + "%'");

                DT = oDBEngine.GetDataTable("tbl_master_companyExchange ce ,tbl_master_exchange ex", "top 10 ce.exch_internalid as internalid, case when ce.exch_segmentId is not null then ex.exh_shortname+'-'+isnull(ce.exch_segmentId,'') else ex.exh_shortname end as Modify ", " exch_compId='" + param + "' and  ce.exch_exchId=ex.exh_cntId and ex.exh_shortname like '" + reqStr + "%'");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion


            #region SearchByCustomerCL
            if (Request.QueryString["SearchByCustomerCL"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact", " top 10 (isnull(ltrim(rtrim(cnt_firstName)),'') +' '+isnull(ltrim(rtrim(cnt_middleName)),'')+' '+isnull(ltrim(rtrim(cnt_lastName)),'') +'['+isnull(ltrim(rtrim(cnt_ucc)),'')+']') as Name ,cnt_internalID", "(cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and cnt_ContactType in ('CL')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchEquityTicker
            if (Request.QueryString["SearchEquityTicker"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "distinct top 10  isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as scrip,cast(Equity_SeriesID as varchar(50)) +'~'+(Select top 1 ISIN_Number from master_isin WHERE ISIN_ProductSeriesID=Equity_SeriesID and ISIN_IsActive='Y') as eqity", "(equity_tickersymbol Like '" + reqStr + "%' or Equity_Series Like '" + reqStr + "%')and Equity_FOIdentifier is null and equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'");
                }
                else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "distinct top 10  isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Tickercode),'')+']' as scrip,cast(Equity_SeriesID as varchar(50)) +'~'+(Select top 1 ISIN_Number from master_isin WHERE ISIN_ProductSeriesID=Equity_SeriesID and ISIN_IsActive='Y') as eqity", " (equity_tickersymbol Like '" + reqStr + "%' or Equity_Tickercode Like '" + reqStr + "%') and Equity_FOIdentifier is null and equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'");
                }
                else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "distinct top 10  isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as scrip,cast(Equity_SeriesID as varchar(50)) +'~'+(Select top 1 ISIN_Number from master_isin WHERE ISIN_ProductSeriesID=Equity_SeriesID and ISIN_IsActive='Y') as eqity", "(equity_tickersymbol Like '" + reqStr + "%' or Equity_Series Like '" + reqStr + "%')and Equity_FOIdentifier is null and equity_exchsegmentid in (1,4) ");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion


            #region SearchProdISINNumber
            if (Request.QueryString["SearchProdISINNumber"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_isin", "ISIN_Number,ISIN_ProductID", "ISIN_ProductSeriesID='" + param + "' and ISIN_IsActive='Y'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion





            #region SearchDpAccount
            {
                if (Request.QueryString["SearchDpAccount"] == "1")
                {
                    string parameter = Request.QueryString["search_param"].ToString();
                    string reqStr = Request.QueryString["letters"].ToString();
                    if (parameter == "P")
                        DT = oDBEngine.GetDataTable("Master_DPAccounts", " top 10 DPAccounts_ID,DPAccounts_ShortName+' ['+DPAccounts_ClientID+']'", " rtrim(DPAccounts_AccountType) in ('[POOL]','[PLPAYIN]','[PLPAYOUT]') and DPAccounts_CompanyID='" + Session["LastCompany"].ToString() + "' and (DPAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + " Or DPAccounts_ExchangeSegmentID=0) and (DPAccounts_ShortName like '" + reqStr + "%' or DPAccounts_ClientID like '" + reqStr + "%')");
                    else if (parameter == "M")
                        DT = oDBEngine.GetDataTable("Master_DPAccounts", " top 10 DPAccounts_ID,DPAccounts_ShortName+' ['+DPAccounts_ClientID+']'", " (Left(DPAccounts_AccountType,6)='[MRGIN' or Left(DPAccounts_AccountType,7)='[HOLDBK') and DPAccounts_CompanyID='" + Session["LastCompany"].ToString() + "' and (DPAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + " Or DPAccounts_ExchangeSegmentID=0) and (DPAccounts_ShortName like '" + reqStr + "%' or DPAccounts_ClientID like '" + reqStr + "%')");
                    else if (parameter == "O")
                        DT = oDBEngine.GetDataTable("Master_DPAccounts", " top 10 DPAccounts_ID,DPAccounts_ShortName+' ['+DPAccounts_ClientID+']'", " Left(DPAccounts_AccountType,4)='[OWN' and DPAccounts_CompanyID='" + Session["LastCompany"].ToString() + "' and (DPAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + " Or DPAccounts_ExchangeSegmentID=0)  and (DPAccounts_ShortName like '" + reqStr + "%' or DPAccounts_ClientID like '" + reqStr + "%')");

                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");

                }
            }
            #endregion

            #region SearchClinetFORBRKGCHANGE
            if (Request.QueryString["SearchClinetFORBRKGCHANGE"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = Request.QueryString["search_param"].ToString();
                string[] idlist = parameter.Split('~');
                if (idlist[1] == "CLIENT")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_CustomerTrades,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalId=CustomerTrades_CustomerID and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and CustomerTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and CustomerTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and CustomerTrades_TradeDate='" + idlist[0] + "' and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_ComCustomerTrades,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "cnt_internalId=ComCustomerTrades_CustomerID and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and ComCustomerTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComCustomerTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComCustomerTrades_TradeDate='" + idlist[0] + "' and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and ComCustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComCustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }

                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and CustomerTrades_CustomerID='" + idlist[1] + "' and CustomerTrades_TradeDate='" + idlist[0] + "' and CustomerTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and CustomerTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("(select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity,Trans_CustomerTrades  WHERE CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and CustomerTrades_CustomerID='" + idlist[1] + "' and CustomerTrades_TradeDate='" + idlist[0] + "' and CustomerTrades_BranchID in(" + Session["userbranchHierarchy"] + ") and CustomerTrades_ExchangeSegment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and  CustomerTrades_CompanyID='" + Session["LastCompany"].ToString() + "')as tb", " distinct top 15 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),'')+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'') else isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),'')+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'')+' '+cast(cast(round(commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_commodity  WHERE commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Commodity_ProductSeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");

                }
            }
            #endregion

            #region SearchByInstruments
            if (Request.QueryString["SearchByInstruments"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] reqstr1 = new string[0];
                string reqstr2 = "";
                string reqstr3 = "";
                if (reqStr.ToString().Contains(" "))
                {
                    reqstr1 = reqStr.ToString().Split(' ');

                    if (reqstr1.Length > 2)
                    {
                        if (reqstr1.Length > 3)
                        {
                            reqstr3 = reqstr1[3].ToString();
                        }
                        reqstr2 = reqstr1[2].ToString();
                    }

                }
                string param = Request.QueryString["search_param"].ToString();
                string[] parameter = param.ToString().Split('-');
                string startdate = parameter[1].ToString() + "/" + parameter[0].ToString() + "/" + parameter[2].ToString();
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                {
                    DT = oDBEngine.GetDataTable("Master_Equity", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                {
                    //DT = oDBEngine.GetDataTable("Master_Equity,Trans_CustomerTrades ", " distinct top 10 (case when Equity_StrikePrice=0.0 then isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']' else isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),'')+']'+'['+convert(varchar(9),Equity_EffectUntil,6)+']'+'['+  cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");
                    //DT = oDBEngine.GetDataTable("(select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity,Trans_CustomerTrades  WHERE CustomerTrades_ProductSeriesID=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    DT = oDBEngine.GetDataTable("(select (case when Equity_StrikePrice=0.0 then  isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID,Equity_EffectUntil from Master_Equity WHERE  Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb ", " distinct top 15 TickerSymbol,Equity_SeriesID ", " TickerSymbol like  '" + reqStr + "%' and   (CAST(Equity_EffectUntil AS datetime) >= CONVERT(varchar,'" + startdate + "', 101))");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    if (reqstr1.Length > 0)
                    {
                        if (reqstr2.Length > 0)
                        {
                            if (reqstr3.Length > 0)
                            {

                                DT = oDBEngine.GetDataTable("Master_Commodity ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end) when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end) else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end)+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqstr1[0] + "%' and convert(varchar(9),Commodity_ExpiryDate,6 ) like  '" + reqstr1[1] + " " + reqstr2 + " " + reqstr3 + "%' and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and  (CAST(Commodity_EffectiveDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) ");
                            }
                            else
                            {
                                DT = oDBEngine.GetDataTable("Master_Commodity ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end) when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end) else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end)+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqstr1[0] + "%' and convert(varchar(9),Commodity_ExpiryDate,6 ) like  '" + reqstr1[1] + " " + reqstr2 + "%' and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and  (CAST(Commodity_EffectiveDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) ");
                            }

                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("Master_Commodity ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end) when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end) else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end)+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqstr1[0] + "%' and convert(varchar(9),Commodity_ExpiryDate,6 ) like  '" + reqstr1[1] + "%' and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and  (CAST(Commodity_EffectiveDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) ");
                        }

                    }


                    else
                    {

                        DT = oDBEngine.GetDataTable("Master_Commodity ", " distinct top 10 (case when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end) when isnull(Commodity_StrikePrice,0.0)=0.0 and Commodity_TickerSeries is not null then isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end) else isnull(Commodity_TickerSymbol,'')+'['+isnull(rtrim(Commodity_Identifier),'')+']'+'['+isnull(rtrim(Commodity_TickerSeries),'')+']'+(case when Commodity_ExpiryDate is null then '' else   '[' + convert(varchar(9),Commodity_ExpiryDate,6 ) + ']' end)+'['+cast(cast(round(Commodity_StrikePrice,2) as numeric(28,2)) as varchar)+']' end) as TickerSymbol,Commodity_ProductSeriesID", "Commodity_TickerSymbol like  '" + reqStr + "%'  and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and  (CAST(Commodity_EffectiveDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) ");
                    }

                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion

            #region InterSettlementForDeliveryPosition
            if (Request.QueryString["InterSettlementForDeliveryPosition"] == "1")
            {
                string parameter = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                reqStr = reqStr.Replace("_", "&");
                if (parameter == "Client")
                    DT = oDBEngine.GetDataTable("tbl_master_contact", " distinct top 10 cnt_internalid,isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'", "  (cnt_firstname like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                else if (parameter == "ISIN")
                    DT = oDBEngine.GetDataTable("Trans_DematPosition", " distinct top 10 DematPosition_ISIN,DematPosition_ISIN", " DematPosition_CustomerID like 'CL%' and DematPosition_SegmentID=" + Session["usersegid"].ToString() + " and DematPosition_ISIN like '" + reqStr + "%'");
                else if (parameter == "Product")
                    DT = oDBEngine.GetDataTable("master_equity", " distinct top 10 cast(Equity_SeriesID as varchar)+'~'+(select top 1 isin_number from master_isin where isin_productseriesid=Equity_SeriesID and isin_existenceUntil is null),isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']'", "  (Equity_TickerSymbol like '" + reqStr + "%' or Equity_Series like '" + reqStr + "%') and Equity_FOIdentifier is null");
                else if (parameter == "SettSource")
                    DT = oDBEngine.GetDataTable("(select  rtrim(Settlements_Number)+rtrim(Settlements_TypeSuffix) as ID,rtrim(Settlements_Number)+rtrim(Settlements_TypeSuffix) as Number from Master_Settlements where Settlements_FinYear='" + Session["LastFinYear"].ToString() + "' and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ") as KK ", " distinct top 10 ID,Number", " Number like '" + reqStr + "%'");
                else if (parameter == "ISINNew")
                    DT = oDBEngine.GetDataTable("Master_ISIN", " distinct top 10 substring(ISIN_Number,1,12),ISIN_Number", " ISIN_Number like '" + reqStr + "%' and ISIN_ProductSeriesID='" + parameter + "' and ISIN_IsActive='Y'");
                else if (parameter == "ISINOld")
                    DT = oDBEngine.GetDataTable("Master_ISIN", " distinct top 10 substring(ISIN_Number,1,12),ISIN_Number", " ISIN_Number like '" + reqStr + "%' and ISIN_ProductSeriesID='" + parameter + "'");
                else if (parameter == "AccountName")
                    DT = oDBEngine.GetDataTable("Master_DPAccounts", " distinct top 10 DPAccounts_ID,DPAccounts_ShortName", " DPAccounts_ShortName like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {

                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region InstructionSlipsMethods(Nsdl/Cdsl)
            #region Searchsettlementfrom
            if (Request.QueryString["Searchsettlementfrom"] == "1")
            {
                try
                {
                    string tran = "";
                    string mkt = "";
                    string ex = "";
                    string parameter = "";
                    parameter = Request.QueryString["search_param"].ToString();
                    mkt = parameter.Split('~')[0];
                    ex = parameter.Split('~')[1];
                    tran = parameter.Split('~')[2];

                    string reqStr = Request.QueryString["letters"].ToString();
                    string month = System.DateTime.Now.ToShortDateString().Split('/')[0];
                    if (month.Length == 1)
                    {
                        month = "0" + month;
                    }

                    string day = System.DateTime.Now.ToShortDateString().Split('/')[1];
                    if (day.Length == 1)
                    {
                        day = "0" + day;
                    }
                    string year = System.DateTime.Now.ToShortDateString().Split('/')[2];
                    string date = month + "/" + day + "/" + year;
                    if (Session["dp"].ToString() == "NSDL")
                    {
                        DT = oDBEngine.GetDataTable("Master_NsdlCalendar", "distinct top 10 NsdlCalendar_SettlementNumber,NsdlCalendar_SettlementNumber+'+none+none+settlementfrom' ", "(NsdlCalendar_CCID = '" + ex + "') and NsdlCalendar_MarketType='" + mkt + "' and NsdlCalendar_SettlementNumber like '%" + reqStr + "%'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Master_CdslCalendar join Master_cdslmarkettypes on Master_CdslCalendar.CdslCalendar_ExchangeID=Master_cdslmarkettypes.cdslmarkettypes_exchangeid", "distinct top 10 CdslCalendar_settlementid,(cast(CdslCalendar_settlementid as varchar(40))+'+none+none+settlementfrom') ", "substring(Master_CdslCalendar.CdslCalendar_SettlementID,5,2)in (select case when len(cdslmarkettypes_typeid)=1 then('0'+cdslmarkettypes_typeid) else cdslmarkettypes_typeid end as cdslmarkettypes_typeid from Master_cdslmarkettypes where cdslmarkettypes_typeid='" + mkt + "' and cdslmarkettypes_exchangeid='" + ex + "') and Master_cdslmarkettypes.cdslmarkettypes_exchangeid='" + ex + "' and substring(CdslCalendar_settlementid,len(CdslCalendar_settlementid)-6,7) like '%" + reqStr + "%'");
                    }
                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                catch
                {
                }

            }
            #endregion

            #region Searchsettlementto
            if (Request.QueryString["Searchsettlementto"] == "1")
            {
                try
                {
                    string mkt = "";
                    string ex = "";
                    string parameter = "";
                    string CCIDConvertCDSLExch = "";
                    string ExcIDConvertCCId = "";
                    parameter = Request.QueryString["search_param"].ToString();
                    string trantype = parameter.Split('~')[3];
                    mkt = parameter.Split('~')[2];
                    ex = parameter.Split('~')[1];
                    string reqStr = Request.QueryString["letters"].ToString();
                    string month = System.DateTime.Now.ToShortDateString().Split('/')[0];
                    if (month.Length == 1)
                    {
                        month = "0" + month;
                    }

                    string day = System.DateTime.Now.ToShortDateString().Split('/')[1];
                    if (day.Length == 1)
                    {
                        day = "0" + day;
                    }
                    string year = System.DateTime.Now.ToShortDateString().Split('/')[2];
                    string date = month + "/" + day + "/" + year;
                    try
                    {
                        if (Session["dp"].ToString() == "NSDL")
                        {
                            if (trantype != "5")
                            {
                                if (trantype == "3")
                                {
                                    if (ex == "IN001002")
                                    {
                                        CCIDConvertCDSLExch = "12";
                                    }
                                    else if (ex == "IN001150")
                                    {
                                        CCIDConvertCDSLExch = "11";
                                    }
                                    else if (ex == "IN001027")
                                    {
                                        CCIDConvertCDSLExch = "13";
                                    }
                                    else
                                    {
                                        CCIDConvertCDSLExch = ex;
                                    }

                                    DT = oDBEngine.GetDataTable("Master_CdslCalendar join Master_cdslmarkettypes on Master_CdslCalendar.CdslCalendar_ExchangeID=Master_cdslmarkettypes.cdslmarkettypes_exchangeid", "distinct top 10 substring(CdslCalendar_settlementid,7,7), (cast(CdslCalendar_settlementid as varchar(40))+'+none+none+settlementto') ", "substring(Master_CdslCalendar.CdslCalendar_SettlementID,5,2)in (select case when len(cdslmarkettypes_typeid)=1 then('0'+cdslmarkettypes_typeid) else cdslmarkettypes_typeid end as cdslmarkettypes_typeid from Master_cdslmarkettypes where (cdslmarkettypes_typeid is null or cdslmarkettypes_typeid='" + mkt + "') and cdslmarkettypes_exchangeid='" + CCIDConvertCDSLExch + "') and Master_cdslmarkettypes.cdslmarkettypes_exchangeid='" + CCIDConvertCDSLExch + "' and substring(CdslCalendar_settlementid,len(CdslCalendar_settlementid)-6,7) like '%" + reqStr + "%' and CdslCalendar_EarmarkDate>getdate()");
                                }
                                else
                                {
                                    //DT = oDBEngine.GetDataTable("Master_NsdlCalendar", "distinct top 10 NsdlCalendar_SettlementNumber,NsdlCalendar_SettlementNumber+'t' ", "(NsdlCalendar_CCID = '" + ex + "') and NsdlCalendar_MarketType='" + mkt + "' and NsdlCalendar_DeadlineDate >='" + date + "' and NsdlCalendar_SettlementNumber like '%" + reqStr + "%'");
                                    DT = oDBEngine.GetDataTable("Master_NsdlCalendar", "distinct top 10 NsdlCalendar_SettlementNumber,NsdlCalendar_SettlementNumber+'t' ", "(NsdlCalendar_CCID = '" + ex + "') and NsdlCalendar_MarketType='" + mkt + "'  and NsdlCalendar_SettlementNumber like '%" + reqStr + "%' and Cast(Convert(varchar,Convert(varchar,NsdlCalendar_DeadlineDate,101)+' '+Convert(varchar,NsdlCalendar_Deadlinetime,108)) as datetime)>getdate()");
                                }
                            }
                            else
                            {
                                //DT = oDBEngine.GetDataTable("Master_CdslCalendar join Master_cdslmarkettypes on Master_CdslCalendar.CdslCalendar_ExchangeID=Master_cdslmarkettypes.cdslmarkettypes_exchangeid", "distinct top 10 substring(CdslCalendar_settlementid,len(CdslCalendar_settlementid)-6,7), (cast(CdslCalendar_settlementid as varchar(40))+'t') ", "substring(Master_CdslCalendar.CdslCalendar_SettlementID,5,2)in (select case when len(cdslmarkettypes_typeid)=1 then('0'+cdslmarkettypes_typeid) else cdslmarkettypes_typeid end as cdslmarkettypes_typeid from Master_cdslmarkettypes where cdslmarkettypes_type='" + mkt + "' and cdslmarkettypes_exchangeid='" + ex + "') and Master_cdslmarkettypes.cdslmarkettypes_exchangeid='" + ex + "' and datediff(day,'" + date + "',convert(varchar(15),CdslCalendar_settlementdate,1)) >=0 and substring(CdslCalendar_settlementid,len(CdslCalendar_settlementid)-6,7) like '%" + reqStr + "%'");
                                DT = oDBEngine.GetDataTable("Master_NsdlCalendar", "distinct top 10 NsdlCalendar_SettlementNumber,NsdlCalendar_SettlementNumber+'t' ", "(NsdlCalendar_CCID = '" + ex + "') and NsdlCalendar_MarketType='" + mkt + "' and NsdlCalendar_SettlementNumber like '%" + reqStr + "%'  and Cast(Convert(varchar,Convert(varchar,NsdlCalendar_DeadlineDate,101)+' '+Convert(varchar,NsdlCalendar_Deadlinetime,108)) as datetime)>getdate()");
                            }
                        }
                        else
                        {
                            if (trantype == "5")
                            {
                                //DT = oDBEngine.GetDataTable("Master_NsdlCalendar", "distinct top 10 NsdlCalendar_SettlementNumber,NsdlCalendar_SettlementNumber+'t' ", "(NsdlCalendar_CCID = '" + ex + "') and NsdlCalendar_MarketType='" + mkt + "' and NsdlCalendar_DeadlineDate >='" + date + "' and NsdlCalendar_SettlementNumber like '%" + reqStr + "%'");
                                //DT = oDBEngine.GetDataTable("Master_CdslCalendar join Master_cdslmarkettypes on Master_CdslCalendar.CdslCalendar_ExchangeID=Master_cdslmarkettypes.cdslmarkettypes_exchangeid", "distinct top 10 substring(CdslCalendar_settlementid,len(CdslCalendar_settlementid)-6,7), (cast(CdslCalendar_settlementid as varchar(40))+'t') ", "substring(Master_CdslCalendar.CdslCalendar_SettlementID,5,2)in (select case when len(cdslmarkettypes_typeid)=1 then('0'+cdslmarkettypes_typeid) else cdslmarkettypes_typeid end as cdslmarkettypes_typeid from Master_cdslmarkettypes where cdslmarkettypes_type='" + mkt + "' and cdslmarkettypes_exchangeid='" + ex + "') and Master_cdslmarkettypes.cdslmarkettypes_exchangeid='" + ex + "' and datediff(day,'" + date + "',convert(varchar(15),CdslCalendar_settlementdate,1)) >=0 and substring(CdslCalendar_settlementid,len(CdslCalendar_settlementid)-6,7) like '%" + reqStr + "%'");
                                DT = oDBEngine.GetDataTable("Master_CdslCalendar join Master_cdslmarkettypes on Master_CdslCalendar.CdslCalendar_ExchangeID=Master_cdslmarkettypes.cdslmarkettypes_exchangeid", "distinct top 10 CdslCalendar_settlementid,(cast(CdslCalendar_settlementid as varchar(40))+'t') ", "substring(Master_CdslCalendar.CdslCalendar_SettlementID,5,2)in (select case when len(cdslmarkettypes_typeid)=1 then('0'+cdslmarkettypes_typeid) else cdslmarkettypes_typeid end as cdslmarkettypes_typeid from Master_cdslmarkettypes where cdslmarkettypes_typeid='" + mkt + "' and cdslmarkettypes_exchangeid='" + ex + "') and Master_cdslmarkettypes.cdslmarkettypes_exchangeid='" + ex + "' and CdslCalendar_EarmarkDate>getdate() and substring(CdslCalendar_settlementid,len(CdslCalendar_settlementid)-6,7) like '%" + reqStr + "%'");

                            }
                            else
                            {

                                if (ex == "IN001002")
                                {
                                    CCIDConvertCDSLExch = "12";
                                }
                                else if (ex == "IN001150")
                                {
                                    CCIDConvertCDSLExch = "11";
                                }
                                else if (ex == "IN001027")
                                {
                                    CCIDConvertCDSLExch = "13";
                                }
                                else
                                {
                                    CCIDConvertCDSLExch = ex;
                                }

                                DT = oDBEngine.GetDataTable("Master_CdslCalendar join Master_cdslmarkettypes on Master_CdslCalendar.CdslCalendar_ExchangeID=Master_cdslmarkettypes.cdslmarkettypes_exchangeid", "distinct top 10 substring(CdslCalendar_settlementid,7,7), (cast(CdslCalendar_settlementid as varchar(40))+'+none+none+settlementto') ", "substring(Master_CdslCalendar.CdslCalendar_SettlementID,5,2)in (select case when len(cdslmarkettypes_typeid)=1 then('0'+cdslmarkettypes_typeid) else cdslmarkettypes_typeid end as cdslmarkettypes_typeid from Master_cdslmarkettypes where (cdslmarkettypes_typeid is null or cdslmarkettypes_typeid='" + mkt + "') and cdslmarkettypes_exchangeid='" + CCIDConvertCDSLExch + "') and Master_cdslmarkettypes.cdslmarkettypes_exchangeid='" + CCIDConvertCDSLExch + "' and substring(CdslCalendar_settlementid,len(CdslCalendar_settlementid)-6,7) like '%" + reqStr + "%' and CdslCalendar_EarmarkDate>getdate()");

                            }
                        }
                    }
                    catch
                    {

                    }
                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                catch
                {
                }

            }
            #endregion

            #region Searchcmbpid
            {
                if (Request.QueryString["Searchcmbpid"] == "1")
                {
                    string tran = "";
                    string EarlyOrNormal = "";
                    string parameter = Request.QueryString["search_param"].ToString();
                    tran = parameter.Split('~')[0];
                    EarlyOrNormal = parameter.Split('~')[1];
                    string reqStr = Request.QueryString["letters"].ToString();
                    try
                    {
                        if (Session["dp"].ToString() == "NSDL")
                        {
                            if (tran == "3")
                            {
                                if (EarlyOrNormal == "N")
                                {
                                    DT = oDBEngine.GetDataTable("Master_CdslClearingMember", "Top 10 case when  CdslClearingMember_PrincipalAccount is not null then cast(CdslClearingMember_ExchangeID as varchar(50))+'+' +cast(substring(CdslClearingMember_PrincipalAccount,1,8) as varchar(50))+'+'+(select cdslexchange_name from Master_CdslExchange where cdslexchange_exchangeid=CdslClearingMember_ExchangeID) +'+'+'cmbpid'+'+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+cast(CdslClearingMember_CMID as varchar)  else cast(CdslClearingMember_ExchangeID as varchar(50))+'+'+cast(substring(CdslClearingMember_UnifiedAccount,1,8) as varchar(50))+'+'+(select cdslexchange_name from Master_CdslExchange where cdslexchange_exchangeid=CdslClearingMember_ExchangeID) +'+'+'cmbpid'+'+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+cast(CdslClearingMember_CMID as varchar) end as ex,case when  CdslClearingMember_PrincipalAccount is not null then cast(CdslClearingMember_PrincipalAccount as varchar(50))+'['+CdslClearingMember_Name1+']' else cast(CdslClearingMember_UnifiedAccount as varchar(50))+'['+CdslClearingMember_Name1+']' end as name ", "CdslClearingMember_Name1 LIKE '%" + reqStr + "%' or CdslClearingMember_UnifiedAccount like '%" + reqStr + "%' or CdslClearingMember_PrincipalAccount like '%" + reqStr + "%' ");
                                }
                                else
                                {
                                    DT = oDBEngine.GetDataTable("Master_CdslClearingMember", "Top 10 cast(CdslClearingMember_ExchangeID as varchar(50))+'+' +cast(substring(CdslClearingMember_EarlyPayinAccount,1,8) as varchar(50))+'+'+(select cdslexchange_name from Master_CdslExchange where cdslexchange_exchangeid=CdslClearingMember_ExchangeID) +'+'+'cmbpid'+'+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+cast(CdslClearingMember_CMID as varchar) ,cast(CdslClearingMember_EarlyPayinAccount as varchar(50))+'['+CdslClearingMember_Name1+']' as name", "len(CdslClearingMember_EarlyPayinAccount)>0 and (CdslClearingMember_Name1 LIKE '%" + reqStr + "%' or CdslClearingMember_EarlyPayinAccount like '%" + reqStr + "%')");
                                }
                            }
                            else
                            {
                                DT = oDBEngine.GetDataTable("(Select nsdlbplist_associatedccid,nsdlbplist_associateddpid,nsdlbplist_bpname,nsdlbplist_bpid from master_nsdlbplist WHERE nsdlbplist_bprole= 3 AND nsdlbplist_bpcategory in (1,2,8,4) and Nsdlbplist_bpstat not in (4,5) and (nsdlbplist_bpid like '%" + reqStr + "%' or nsdlbplist_bpname like '%" + reqStr + "%')) as t1 join (select nsdlbplist_bpid,nsdlbplist_bpname from master_nsdlbplist where nsdlbplist_bprole=2)as  t2 on t1.nsdlbplist_associatedccid=t2.nsdlbplist_bpid ", "top 10 (cast(t1.nsdlbplist_associatedccid as varchar(20))+'+'+t1.nsdlbplist_associateddpid+'+'+t2.nsdlbplist_bpname+'+cmbpid+') as nsdlbplist_bpid ,(cast(t1.nsdlbplist_bpid as varchar(20))+' ['+rtrim(ltrim(t1.nsdlbplist_bpname))+']' )as nsdlbplist_bpname ", null);
                            }
                        }
                        else
                        {
                            if (tran == "3")
                            {
                                DT = oDBEngine.GetDataTable("(Select nsdlbplist_associatedccid,nsdlbplist_associateddpid,nsdlbplist_bpname,nsdlbplist_bpid from master_nsdlbplist WHERE nsdlbplist_bprole= 3 AND nsdlbplist_bpcategory in (1,2,8,4) and Nsdlbplist_bpstat not in (4,5) and (nsdlbplist_bpid like '%" + reqStr + "%' or nsdlbplist_bpname like '%" + reqStr + "%')) as t1 join (select nsdlbplist_bpid,nsdlbplist_bpname from master_nsdlbplist where nsdlbplist_bprole=2)as  t2 on t1.nsdlbplist_associatedccid=t2.nsdlbplist_bpid ", "top 10 (cast(t1.nsdlbplist_associatedccid as varchar(20))+'+'+t1.nsdlbplist_associateddpid+'+'+t2.nsdlbplist_bpname+'+cmbpid+') as nsdlbplist_bpid ,(cast(t1.nsdlbplist_bpid as varchar(20))+' ['+rtrim(ltrim(t1.nsdlbplist_bpname))+']' )as nsdlbplist_bpname ", null);
                            }
                            else
                            {
                                if (tran == "7" || tran == "1")
                                {
                                    if (EarlyOrNormal == "N")
                                    {
                                        DT = oDBEngine.GetDataTable("Master_CdslClearingMember", "Top 10 case when  CdslClearingMember_PrincipalAccount is not null then cast(CdslClearingMember_ExchangeID as varchar(50))+'+' +cast(substring(CdslClearingMember_PrincipalAccount,1,8) as varchar(50))+'+'+(select cdslexchange_name from Master_CdslExchange where cdslexchange_exchangeid=CdslClearingMember_ExchangeID) +'+'+'cmbpid'+'+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+cast(CdslClearingMember_CMID as varchar) else cast(CdslClearingMember_ExchangeID as varchar(50))+'+' +cast(substring(CdslClearingMember_UnifiedAccount,1,8) as varchar(50))+'+'+(select cdslexchange_name from Master_CdslExchange where cdslexchange_exchangeid=CdslClearingMember_ExchangeID) +'+'+'cmbpid'+'+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+cast(CdslClearingMember_CMID as varchar) end as ex,case when  CdslClearingMember_PrincipalAccount is not null then cast(CdslClearingMember_PrincipalAccount as varchar(50))+'['+CdslClearingMember_Name1+']' else cast(CdslClearingMember_UnifiedAccount as varchar(50))+'['+CdslClearingMember_Name1+']' end as name ", "CdslClearingMember_Name1 LIKE '%" + reqStr + "%' or CdslClearingMember_UnifiedAccount like '%" + reqStr + "%' or CdslClearingMember_PrincipalAccount like '%" + reqStr + "%' ");
                                    }
                                    else if (EarlyOrNormal == "E")
                                    {
                                        DT = oDBEngine.GetDataTable("Master_CdslClearingMember", "Top 10 cast(CdslClearingMember_ExchangeID as varchar(50))+'+' +cast(substring(CdslClearingMember_EarlyPayinAccount,1,8) as varchar(50))+'+'+(select cdslexchange_name from Master_CdslExchange where cdslexchange_exchangeid=CdslClearingMember_ExchangeID) +'+'+'cmbpid'+'+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+cast(CdslClearingMember_CMID as varchar) ,cast(CdslClearingMember_EarlyPayinAccount as varchar(50))+'['+CdslClearingMember_Name1+']' as name", "len(CdslClearingMember_EarlyPayinAccount)>0 and CdslClearingMember_Name1 LIKE '%" + reqStr + "%' or CdslClearingMember_PrincipalAccount like '%" + reqStr + "%'");
                                    }
                                    else
                                    {
                                        DT = oDBEngine.GetDataTable("Master_CdslClearingMember", "Top 10 cast(CdslClearingMember_ExchangeID as varchar)+'++'+(select CdslExchange_Name from Master_CDSLExchange where CdslExchange_ExchangeID=CdslClearingMember_ExchangeID)+'+cmbpid+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+ltrim(rtrim(cast(CdslClearingMember_CMID as varchar))) as CM_EXC_CH_ID,ltrim(rtrim(cdslclearingmember_CMID))+' ['+CdslClearingMember_Name1+']' as CMIDName ", "CdslClearingMember_CMID LIKE '%" + reqStr + "%' or CdslClearingMember_Name1 like '%" + reqStr + "%'");
                                    }
                                }
                                else
                                {
                                    DT = oDBEngine.GetDataTable("Master_CdslClearingMember", "Top 10 case when  CdslClearingMember_PrincipalAccount is not null then cast(CdslClearingMember_ExchangeID as varchar(50))+'+' +cast(substring(CdslClearingMember_PrincipalAccount,1,8) as varchar(50))+'+'+(select cdslexchange_name from Master_CdslExchange where cdslexchange_exchangeid=CdslClearingMember_ExchangeID) +'+'+'cmbpid'+'+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+cast(CdslClearingMember_CMID as varchar)  else cast(CdslClearingMember_ExchangeID as varchar(50))+'+' +cast(substring(CdslClearingMember_UnifiedAccount,1,8) as varchar(50))+'+'+(select cdslexchange_name from Master_CdslExchange where cdslexchange_exchangeid=CdslClearingMember_ExchangeID) +'+'+'cmbpid'+'+'+cast(CdslClearingMember_ClearingHouseID as varchar)+'+'+cast(CdslClearingMember_CMID as varchar) end as ex,case when  CdslClearingMember_PrincipalAccount is not null then cast(CdslClearingMember_PrincipalAccount as varchar(50))+'['+CdslClearingMember_Name1+']' else cast(CdslClearingMember_UnifiedAccount as varchar(50))+'['+CdslClearingMember_Name1+']' end as name ", "CdslClearingMember_Name1 LIKE '%" + reqStr + "%' or CdslClearingMember_UnifiedAccount like '%" + reqStr + "%' or CdslClearingMember_PrincipalAccount like '%" + reqStr + "%' ");
                                }
                            }
                        }
                    }
                    catch (SqlException sqlexcp)
                    {

                    }

                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }

            }
            #endregion

            #region Searchdpid
            {
                if (Request.QueryString["Searchdpid"] == "1")
                {
                    string parameter = Request.QueryString["search_param"].ToString();
                    string dpid3 = parameter.Split('~')[2];
                    string tran = parameter.Split('~')[3];


                    string reqStr = Request.QueryString["letters"].ToString();
                    if (Session["dp"].ToString() == "NSDL")
                    {
                        if (tran == "4")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslbplist", "Top 10 (('1'+ltrim(rtrim(cdslbplist_dptype))+'0'+right(replicate(0,5)+isnull(convert(varchar,cdslbplist_dpId),0),5))+' ['+cdslbplist_firstname+']' )as cdslbplist_firstname,('1'+ltrim(rtrim(cdslbplist_dptype))+'0'+right(replicate(0,5)+isnull(convert(varchar,cdslbplist_dpId),0),5))+'+none+none+dpid' as cdslbplist_dpid", "(cdslbplist_dptype in (2,3,6) and cdslbplist_dptype=substring('" + reqStr + "',2,1) and '1'+ltrim(rtrim(cdslbplist_dptype))+'0'+cast(cdslbplist_dpid as varchar) like '%" + reqStr + "%') or cdslbplist_firstName like '%" + reqStr + "%'");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlbplist", "Top 10 (cast(nsdlbplist_bpid as varchar(20))+' ['+rtrim(ltrim(nsdlbplist_bpname))+']' )as nsdlbplist_bpname,(cast(nsdlbplist_bpid as varchar(20))+'+none+none+dpid') as nsdlbplist_bpid ", "nsdlbplist_bprole= 1 and (nsdlbplist_bpid like '%" + reqStr + "%' or nsdlbplist_bpname like '%" + reqStr + "%') ");
                        }
                    }
                    else
                    {
                        if (tran == "4")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlbplist", "Top 10 (cast(nsdlbplist_bpid as varchar(20))+' ['+rtrim(ltrim(nsdlbplist_bpname))+']' )as nsdlbplist_bpname,(cast(nsdlbplist_bpid as varchar(20))+'+none+none+dpid') as nsdlbplist_bpid ", "nsdlbplist_bprole= 1 and (nsdlbplist_bpid like '%" + reqStr + "%' or nsdlbplist_bpname like '%" + reqStr + "%') ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("master_cdslbplist", "Top 10 (('1'+ltrim(rtrim(cdslbplist_dptype))+'0'+right(replicate(0,5)+isnull(convert(varchar,cdslbplist_dpId),0),5))+' ['+cdslbplist_firstname+']' )as cdslbplist_firstname,('1'+ltrim(rtrim(cdslbplist_dptype))+'0'+right(replicate(0,5)+isnull(convert(varchar,cdslbplist_dpId),0),5))+'+none+none+dpid' as cdslbplist_dpid", "(cdslbplist_dptype in (2,3,6) and cdslbplist_dptype=substring('" + reqStr + "',2,1) and '1'+ltrim(rtrim(cdslbplist_dptype))+'0'+cast(cdslbplist_dpid as varchar) like '%" + reqStr + "%') or cdslbplist_firstName like '%" + reqStr + "%'");
                        }
                    }

                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");

                }
            }
            #endregion


            #region Searchclientid
            {
                if (Request.QueryString["Searchclientid"] == "1")
                {
                    string parameter = Request.QueryString["search_param"].ToString();
                    string dpid = "";
                    string trans = parameter.Split('~')[2];
                    string Client = parameter.Split('~')[3];
                    string BenAccID = Client.Substring(Client.IndexOf("[") + 2, 8);
                    try
                    {
                        if (Session["dp"].ToString() == "NSDL")
                        {
                            dpid = parameter.Split('~')[1].ToString();
                        }
                        else
                        {
                            dpid = parameter.Split('~')[1];
                        }
                        string reqStr = Request.QueryString["letters"].ToString();
                        if (Session["dp"].ToString() == "NSDL")
                        {
                            if (trans == "4" || trans == "3")
                            {
                                DT = oDBEngine.GetDataTable("Master_CdslClients", "Top 10 CdslClients_boid+'+none+none+ClientId'+CdslClients_ExchangeID,(substring(CdslClients_boid,len(CdslClients_boid)-7,8)+' ['+CdslClients_firstholdername+']') as CdslClients_firstholdername", "(substring(CdslClients_boid,len(CdslClients_boid)-7,8)like '%" + reqStr + "%' or CdslClients_firstholdername like '%" + reqStr + "%') and (substring(CdslClients_boid,1,8)= '" + dpid + "' and CdslClients_bostatus not like '%Clearing Member%' and CdslClients_accountstatus='Active')");
                            }
                            else
                            {
                                DT = oDBEngine.GetDataTable("Master_NsdlClients", "Top 10 (NsdlClients_DPID+'+'+cast(NsdlClients_BenAccountID as varchar(20))+'+none+ClientId') as NsdlClients_BenAccountID, (cast(NsdlClients_BenAccountID as varchar(20))+' ['+ltrim(rtrim(NsdlClients_BenFirstHolderName))+' ]') as NsdlClients_BenFirstHolderName", "(NsdlClients_BenAccountID like  '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like  '%" + reqStr + "%' )and NsdlClients_DPID='" + dpid + "' and NsdlClients_BenAccountID <> '" + BenAccID.Trim() + "' and NsdlClients_bentype<>6 ");
                            }
                        }
                        else
                        {
                            if (trans == "4" || trans == "3")
                            {
                                DT = oDBEngine.GetDataTable("Master_NsdlClients", "Top 10 (NsdlClients_DPID+cast(NsdlClients_BenAccountID as varchar(20))) as NsdlClients_BenAccountID, (cast(NsdlClients_BenAccountID as varchar(20))+' ['+ltrim(rtrim(NsdlClients_BenFirstHolderName))+' ]') as NsdlClients_BenFirstHolderName", "(NsdlClients_BenAccountID like  '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like  '%" + reqStr + "%' )and NsdlClients_DPID='" + dpid + "' and NsdlClients_bentype<>6 ");
                            }
                            else
                            {
                                DT = oDBEngine.GetDataTable("Master_CdslClients", "Top 10 CdslClients_boid+'$'+CdslClients_ExchangeID,(substring(CdslClients_boid,len(CdslClients_boid)-7,8)+' ['+CdslClients_firstholdername+']') as CdslClients_firstholdername", "(substring(CdslClients_boid,len(CdslClients_boid)-7,8)like '%" + reqStr + "%' or CdslClients_firstholdername like '%" + reqStr + "%') and (substring(CdslClients_boid,1,8)= '" + dpid + "' and substring(CdslClients_boid,len(CdslClients_boid)-7,8)<>'" + BenAccID.Trim() + "'and CdslClients_bostatus not like '%Clearing Member%' and CdslClients_accountstatus='Active')");
                            }
                        }
                    }
                    catch (SqlException exp)
                    {

                    }

                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    //else
                    // Response.Write("No Record Found###No Record Found|");

                }
            }
            #endregion

            #region Searchslip
            if (Request.QueryString["Searchslip"] == "1")
            {
                string tran = "";
                string reqStr = Request.QueryString["letters"].ToString();
                string parameter = "";
                parameter = Request.QueryString["search_param"].ToString();
                tran = parameter.Split('~')[2];
                try
                {
                    if (Session["dp"].ToString() == "CDSL")
                    {
                        DT = oDBEngine.GetDataTable("master_cdslisin", "distinct top 10 cdslisin_number+'+none+'+ Case cdslisin_isinstatusdescription When 'Active' then 'Active+isin' When 'Inactive' then 'To be Closed+isin' End as name,(cdslisin_number+'[ '+cdslisin_shortname+' ]') as cdslisin_number", "cdslisin_isinstatusdescription in ('Active','Inactive') and (cdslisin_number like '%" + reqStr + "%' or cdslisin_shortname like '%" + reqStr + "%')");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_nsdlisin", "distinct top 10 nsdlisin_number+'+none+'+CASE WHEN NSDLISIN_SecurityStatus = 1 THEN 'Active+isin' WHEN NSDLISIN_SecurityStatus = 3 THEN 'Suspended for Debit+isin' WHEN NSDLISIN_SecurityStatus = 4	THEN 'To be Closed+isin'	END as NSDLISIN_SecurityStatus,(nsdlisin_number+'[ '+nsdlisin_companyname+' ]') as cdslisin_number ", "NSDLISIN_SecurityStatus in(1,3,4) and (nsdlisin_number like '%" + reqStr + "%' or nsdlisin_companyname like '%" + reqStr + "%')");
                    }
                }
                catch (SqlException exp)
                {
                }

                //DT = oDBEngine.GetDataTable("master_cdslisin join Trans_CdslHolding on Trans_CdslHolding.CdslHolding_ISIN=master_cdslisin.cdslisin_number WHERE (cdslisin_number like '%" + reqStr + "%' or cdslisin_shortname like '%" + reqStr + "%') and CdslHolding_BenAccountNumber='" + Session["BenAccountNumber"].ToString().Trim() + "' and CdslHolding_dpid='" + Session["usersegid"].ToString() + "' order by CdslHolding_HoldingDateTime desc)b on a.CdslHolding_HoldingDateTime=b.CdslHolding_HoldingDateTime ", "b.cdslisin_number,b.name from (Select distinct top 10  (cdslisin_number+'[ '+cdslisin_shortname+' ]') as cdslisin_number ,max(CdslHolding_HoldingDateTime) as CdslHolding_HoldingDateTime", "master_cdslisin join Trans_CdslHolding on Trans_CdslHolding.CdslHolding_ISIN=master_cdslisin.cdslisin_number WHERE (cdslisin_number like '%" + reqStr + "%' or cdslisin_shortname like '%" + reqStr + "%') and CdslHolding_BenAccountNumber='" + Session["BenAccountNumber"].ToString().Trim() + "' and CdslHolding_dpid='" + Session["usersegid"].ToString() + "' group by cdslisin_number,cdslisin_shortname) a join (Select distinct top 10  (cdslisin_number+'[ '+cdslisin_shortname+' ]') as cdslisin_number ,(cdslisin_shortname +'~'+cast(cdslholding_freebalance as varchar(20))) as name ,CdslHolding_HoldingDateTime from master_cdslisin join Trans_CdslHolding on Trans_CdslHolding.CdslHolding_ISIN=master_cdslisin.cdslisin_number ");
                if (DT.Rows.Count != 0)
                {

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {

                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion
            #endregion



            #region ISIN
            if (Request.QueryString["ISIN"] == "1")
            {
                string parameter = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_isin", " distinct top 10 left(ISIN_Number,12) as ID,left(ISIN_Number,12) as Number", " isin_productseriesid='" + parameter + "' and ISIN_ExistenceUntil is null and ISIN_Number like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {

                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region Searchcmbpidcdsl
            {
                if (Request.QueryString["Searchcmbpidcdsl"] == "1")
                {

                    string reqStr = Request.QueryString["letters"].ToString();

                    DT = oDBEngine.GetDataTable("  Master_CdslClearingMember", "Top 10 case when  CdslClearingMember_PrincipalAccount is not null then cast(CdslClearingMember_PrincipalAccount as varchar(50)) +'$'+cast(CdslClearingMember_ExchangeID as varchar(50)) else cast(CdslClearingMember_UnifiedAccount as varchar(50)) +'$'+cast(CdslClearingMember_ExchangeID as varchar(50)) end as ex,case when  CdslClearingMember_PrincipalAccount is not null then cast(CdslClearingMember_PrincipalAccount as varchar(50))+'['+CdslClearingMember_Name1+']' else cast(CdslClearingMember_UnifiedAccount as varchar(50))+'['+CdslClearingMember_Name1+']' end as name ", "CdslClearingMember_Name1 LIKE '%" + reqStr + "%' or CdslClearingMember_UnifiedAccount like '%" + reqStr + "%' or CdslClearingMember_PrincipalAccount like '%" + reqStr + "%' ");
                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }


            #endregion


            #region SearchExchangeClients

            if (Request.QueryString["SearchExchangeClients"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                string[] parameter = param.ToString().Split('-');
                string startdate = parameter[1].ToString() + "/" + parameter[0].ToString() + "/" + parameter[2].ToString();
                if (parameter[3].ToString() == "S")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contactexchange,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "crg_cntid=cnt_internalid and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  and crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId) and (CAST(crg_regisDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101))");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contactexchange,tbl_master_contact", "distinct top 10  cnt_internalId ,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "crg_cntid=cnt_internalid and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  and crg_exchange in (select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where  exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId) ");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }

            }
            #endregion

            #region CLIENTRISK
            if (Request.QueryString["CLIENTRISK"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string Param = Request.QueryString["search_param"].ToString();
                string[] idlist = Param.Split('~');

                if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select exch_internalId,(select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId as Comp from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' ) as D ", "top 10 *", "Comp Like '" + reqStr + "%'");
                }

                if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_branch ", "top 10 branch_id,branch_description+' ['+branch_code+']' ", "  branch_description like '" + reqStr + "%' or branch_code like '" + reqStr + "%'");
                }

                if (idlist[0] == "Client")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "  distinct top 10 cnt_internalId,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "(cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')");

                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "  distinct top 10 cnt_internalId,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "(cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and cnt_branchid in('" + idlist[1] + "')");
                    }
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchSettlementNumber
            if (Request.QueryString["SearchSettlementNumber"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_Settlements", "distinct top 10 rtrim(Settlements_Number) as Number,rtrim(Settlements_Number) as Number1", " Settlements_FinYear='" + Session["LastFinYear"].ToString() + "' and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and  Settlements_Number like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchSettlementType
            if (Request.QueryString["SearchSettlementType"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                DT = oDBEngine.GetDataTable("Master_Settlements", "distinct top 10 rtrim(Settlements_TypeSuffix) as Number,rtrim(Settlements_TypeSuffix) as Number1", " Settlements_FinYear='" + Session["LastFinYear"].ToString() + "' and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + "  and Settlements_Number='" + param + "'  and  Settlements_TypeSuffix like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchBankNameFromMainAccountOFBANKTYPE
            if (Request.QueryString["BankAccountSelection"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string CompanyName = Session["LastCompany"].ToString();
                string segid = Session["usersegid"].ToString();
                DT = oDBEngine.GetDataTable(" master_mainaccount ", "MainAccount_AccountCode,MainAccount_AccountCode+' - '+MainAccount_Name+' ['+MainAccount_BankAcNumber+']' ", " MainAccount_BankCashType='Bank' and MainAccount_AccountType='Asset' and (MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_BankCompany='" + CompanyName + "'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region ShowClientFORMarginStocks
            if (Request.QueryString["ShowClientFORMarginStocks"] == "1")
            {
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string CombinedQuery = null;

                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                string where = null;
                if (idlist[0] == "Clients")
                {
                    //   DT = oDBEngine.GetDataTable("tbl_master_contact,TBL_MASTER_CONTACTEXCHANGE", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' AND crg_cntid=cnt_internalid AND (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  AND crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId) AND cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") AND isnull(crg_suspensiondate,'1900-01-01 00:00:00.000')='1900-01-01 00:00:00.000' ");
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ClientsWithoutExchange")
                {
                    string strcomp = Session["LastCompany"].ToString();
                    //   DT = oDBEngine.GetDataTable("tbl_master_contact,TBL_MASTER_CONTACTEXCHANGE", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' AND crg_cntid=cnt_internalid AND (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  AND crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId) AND cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") AND isnull(crg_suspensiondate,'1900-01-01 00:00:00.000')='1900-01-01 00:00:00.000' ");
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%'  AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "Broker")
                {

                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'BO%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ArbGroup")
                {
                    DT = oDBEngine.GetDataTable("Master_ArbGroup", "top 10 rtrim(ltrim(ArbGroup_Name))+' [ '+rtrim(ltrim(ArbGroup_Code))+' ]',ArbGroup_Code ", "(ArbGroup_Code like '" + reqStr + "%' or ArbGroup_Name like '" + reqStr + "%')");

                }
                else if (idlist[0] == "ClientsSelction")
                {
                    if (idlist[1] == "ALL")
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%')  and crg_company='" + Session["LastCompany"].ToString() + "' and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                    else
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%')  and crg_company='" + Session["LastCompany"].ToString() + "' and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");

                }
                else if (idlist[0] == "ClientsNotIn")
                {
                    //DT = oDBEngine.GetDataTable("tbl_master_contact,TBL_MASTER_CONTACTEXCHANGE", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalId not in (" + idlist[1] + ") AND crg_cntid=cnt_internalid AND (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  AND crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId) and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') AND isnull(crg_suspensiondate,'1900-01-01 00:00:00.000')='1900-01-01 00:00:00.000' and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and cnt_internalId not in (" + idlist[1] + ") and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");

                }
                else if (idlist[0] == "ProClients")
                {
                    //DT = oDBEngine.GetDataTable("tbl_master_contact,TBL_MASTER_CONTACTEXCHANGE", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_clienttype like 'Pro%' AND crg_cntid=cnt_internalid AND (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')  AND crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId) and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') AND isnull(crg_suspensiondate,'1900-01-01 00:00:00.000')='1900-01-01 00:00:00.000' and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%')  and cnt_clienttype like 'Pro%' and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "BranchGroup")
                {
                    DT = oDBEngine.GetDataTable("master_branchgroups", "top 10 rtrim(ltrim(branchgroups_name))+' [ '+rtrim(ltrim(branchgroups_code))+' ]',branchgroups_id ", "(branchgroups_name like '" + reqStr + "%' or branchgroups_code like '" + reqStr + "%')");
                }
                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ClientsBranchGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        // DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+'] ['+isnull(rtrim(branch_code),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers)");
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers)");
                    }
                    else
                    {
                        // DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+'] ['+isnull(rtrim(branch_code),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + idlist[2] + "))");
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + idlist[2] + "))");
                    }
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                    {
                        string WhichDP = Session["userlastsegment"].ToString() == "9" ? "NSDL" : "CDSL";
                        string WhichCall_Indicator = WhichDP + "Client";
                        if (idlist[1] == "ALL")
                            DT = oGenericMethod.GetClient_NSDLCDSL("D", ref CombinedQuery, 10, "TextField,ValueField+'~" + WhichCall_Indicator + "',ContactID,BranchID,GPMID,GRPID,GType,GroupName,GDescription", "ContactID is not null and GType in ('" + idlist[2] + "') and TextField Like '%" + reqStr + "%'", 1, WhichDP);
                        else
                            DT = oGenericMethod.GetClient_NSDLCDSL("D", ref CombinedQuery, 10, "TextField,ValueField+'~" + WhichCall_Indicator + "',ContactID,BranchID,GPMID,GRPID,GType,GroupName,GDescription", "ContactID is not null and GPMID in (" + idlist[2] + ") and TextField Like '%" + reqStr + "%'", 1, WhichDP);

                    }
                    else if (Session["userlastsegment"].ToString() == "1")
                    {
                        if (idlist[1] == "ALL")
                        {
                            if (idlist.Length > 2)
                            {
                                if (idlist[2] != null)
                                {
                                    string RefStr = string.Empty;
                                    DataTable DtSegments = null;
                                    if (idlist[2].Split(',')[0] == "SelectedSegmentID")
                                    {
                                        DtSegments = oGenericMethod.GetCompanyDetail(Session["LastCompany"].ToString(), idlist[2].Split(',')[1]);
                                        SegmentName = "'" + DtSegments.Rows[0][4].ToString() + " - " + DtSegments.Rows[0][5].ToString() + "'";
                                    }
                                    else
                                    {
                                        DtSegments = oGenericMethod.GetSegments("D", ref RefStr, 100, Session["LastCompany"].ToString(), idlist[2]);
                                        SegmentName = string.Empty;
                                        foreach (DataRow Dr in DtSegments.Rows)
                                            if (SegmentName == String.Empty)
                                                SegmentName = "'" + Dr[0].ToString() + "'";
                                            else
                                                SegmentName = SegmentName + ",'" + Dr[0].ToString() + "'";
                                    }
                                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in (" + SegmentName + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                                }
                            }
                            else
                                DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 30  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");

                        }
                        else if (idlist[1] == "ProSelected")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 30  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_clienttype like 'Pro%' and branch_id in (" + idlist[2] + ")");
                        }
                        else
                        {
                            // DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+'] ['+isnull(rtrim(branch_code),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 30  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%'  AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + idlist[2] + ")");
                        }
                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            if (idlist.Length > 3)
                            {
                                if (idlist[3] != null)
                                {
                                    string RefStr = string.Empty;
                                    DataTable DtSegments = null;
                                    if (idlist[3].Split(',')[0] == "SelectedSegmentID")
                                    {
                                        DtSegments = oGenericMethod.GetCompanyDetail(Session["LastCompany"].ToString(), idlist[2].Split(',')[3]);
                                        SegmentName = "'" + DtSegments.Rows[0][4].ToString() + " - " + DtSegments.Rows[0][5].ToString() + "'";
                                    }
                                    else
                                    {
                                        DtSegments = oGenericMethod.GetSegments("D", ref RefStr, 100, Session["LastCompany"].ToString(), idlist[3]);
                                        SegmentName = string.Empty;
                                        foreach (DataRow Dr in DtSegments.Rows)
                                            if (SegmentName == String.Empty)
                                                SegmentName = "'" + Dr[0].ToString() + "'";
                                            else
                                                SegmentName = SegmentName + ",'" + Dr[0].ToString() + "'";
                                    }
                                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in (" + SegmentName + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                                }
                            }
                            else
                                DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else if (idlist[1] == "ProALL")
                        {
                            //  DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_clienttype like 'Pro%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else if (idlist[1] == "ProSelected")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_clienttype like 'Pro%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                        else
                        {
                            // DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                    {
                        string WhichDP = Session["userlastsegment"].ToString() == "9" ? "NSDL" : "CDSL";
                        string WhichCall_Indicator = WhichDP + "Client";
                        if (idlist[1] == "ALL")
                            DT = oGenericMethod.GetClient_NSDLCDSL("D", ref CombinedQuery, 10, "TextField,ValueField+'~" + WhichCall_Indicator + "',ContactID,BranchID,GPMID,GRPID,Gtype,GroupName,GDescription", "ContactID is not null and BranchID in (" + Session["userbranchHierarchy"].ToString() + ") and TextField Like '%" + reqStr + "%'", 1, WhichDP);
                        else
                            DT = oGenericMethod.GetClient_NSDLCDSL("D", ref CombinedQuery, 10, "TextField,ValueField+'~" + WhichCall_Indicator + "',ContactID,BranchID,GPMID,GRPID,Gtype,GroupName,GDescription", "ContactID is not null and BranchID in (" + idlist[2] + ") and TextField Like '%" + reqStr + "%'", 1, WhichDP);

                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            if (idlist.Length > 2)
                            {
                                if (idlist[2] != null)
                                {
                                    string RefStr = string.Empty;
                                    DataTable DtSegments = null;
                                    if (idlist[2].Split(',')[0] == "SelectedSegmentID")
                                    {
                                        DtSegments = oGenericMethod.GetCompanyDetail(Session["LastCompany"].ToString(), idlist[2].Split(',')[1]);
                                        SegmentName = "'" + DtSegments.Rows[0][4].ToString() + " - " + DtSegments.Rows[0][5].ToString() + "'";
                                    }
                                    else
                                    {
                                        DtSegments = oGenericMethod.GetSegments("D", ref RefStr, 100, Session["LastCompany"].ToString(), idlist[2]);
                                        SegmentName = string.Empty;
                                        foreach (DataRow Dr in DtSegments.Rows)
                                            if (SegmentName == String.Empty)
                                                SegmentName = "'" + Dr[0].ToString() + "'";
                                            else
                                                SegmentName = SegmentName + ",'" + Dr[0].ToString() + "'";
                                    }
                                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in (" + SegmentName + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                                }
                            }
                            else
                                DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");

                        }
                        else if (idlist[1] == "ProSelected")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_clienttype like 'Pro%' and branch_id in (" + idlist[2] + ")");
                        }
                        else
                        {
                            // DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+'] ['+isnull(rtrim(branch_code),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + idlist[2] + ")");
                        }
                    }
                }
                else if (idlist[0] == "Product")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "16")
                    {
                        DT = oDBEngine.GetDataTable("master_products", " top 10 isnull(rtrim(products_name),'')+' '+'['+isnull(rtrim(products_shortname),'')+']' as  Name, products_id as Id", "products_producttypeid=1 and  (products_shortname Like '" + reqStr + "%' or products_name like '" + reqStr + "%')");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_products", " top 10 isnull(rtrim(products_name),'')+' '+'['+isnull(rtrim(products_shortname),'')+']' as  Name, products_id as Id", "products_producttypeid in (10,9,13) and  (products_shortname Like '" + reqStr + "%' or products_name like '" + reqStr + "%')");
                    }
                }
                else if (idlist[0] == "Scrips")
                {
                    //DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID=1");
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        DT = oDBEngine.GetDataTable("Master_Equity", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),Equity_Tickercode)+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%' or Equity_Tickercode like '" + reqStr + "%') and Equity_ExchSegmentID in (1,4,15)");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(Equity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity  WHERE Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'') else isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'')+' '+cast(cast(round(commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_commodity  WHERE commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'  )as tb", " distinct top 15 TickerSymbol,Commodity_ProductSeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }
                }
                else if (idlist[0] == "ScripsExchange")
                {
                    if (idlist[1] == "Date")
                    {
                        where = idlist[2];
                    }
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        DT = oDBEngine.GetDataTable("Master_Equity", " distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),Equity_Tickercode)+']' as TickerSymbol,Equity_SeriesID", "(Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%' or Equity_Tickercode like '" + reqStr + "%') and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' ");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(Equity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity  WHERE Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' " + where + " )as tb", " distinct top 15 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'') else isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'')+' '+cast(cast(round(commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_commodity  WHERE commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' " + where + " )as tb", " distinct top 15 TickerSymbol,Commodity_ProductSeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }
                }
                else if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                }
                else if (idlist[0] == "SegmentCM")
                {
                    DT = oDBEngine.GetDataTable("(select isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,exch_internalId from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid like 'CM%') as D", "*", null);
                }
                else if (idlist[0] == "UserID")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_TerminalID,ExchangeTrades_TerminalID", " " + idlist[1] + " AND ExchangeTrades_TerminalID Like '" + reqStr + "%'  and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades ", "distinct top 10  ComExchangeTrades_TerminalID,ComExchangeTrades_TerminalID", " " + idlist[1] + " AND ComExchangeTrades_TerminalID Like '" + reqStr + "%'  and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "'");
                    }
                }
                else if (idlist[0] == "TerminalId")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_TerminalID,ExchangeTrades_TerminalID", "ExchangeTrades_TerminalID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades ", "distinct top 10  ComExchangeTrades_TerminalID,ComExchangeTrades_TerminalID", "ComExchangeTrades_TerminalID Like '" + reqStr + "%' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                    }
                }
                else if (idlist[0] == "TradeCode")
                {

                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  rtrim(ltrim(ExchangeTrades_customerucc)),rtrim(ltrim(ExchangeTrades_customerucc))", " ExchangeTrades_customerucc Like '" + reqStr + "%'  " + idlist[1] + " and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_COMExchangeTrades ", "distinct top 10  rtrim(ltrim(ComExchangeTrades_customerucc)),rtrim(ltrim(ComExchangeTrades_customerucc))", " ComExchangeTrades_customerucc Like '" + reqStr + "%'   " + idlist[1] + " and COMExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and COMExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and COMExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                    }

                }
                else if (idlist[0] == "TerminalIdDate")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_TerminalID,ExchangeTrades_TerminalID", "ExchangeTrades_TerminalID Like '" + reqStr + "%' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and exchangetrades_tradedate between " + idlist[1] + " and   len(ExchangeTrades_TerminalID)<>0 and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades ", "distinct top 10  ComExchangeTrades_TerminalID,ComExchangeTrades_TerminalID", "ComExchangeTrades_TerminalID Like '" + reqStr + "%' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and Comexchangetrades_tradedate between " + idlist[1] + " and   len(ComExchangeTrades_TerminalID)<>0  and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                    }
                }
                else if (idlist[0] == "CTCLId")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades ", "distinct top 10  ComExchangeTrades_CTCLID,ComExchangeTrades_CTCLID", "ComExchangeTrades_CTCLID Like '" + reqStr + "%' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                    }
                }
                else if (idlist[0] == "TERMINALSELECTEDCTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_TerminalID in(" + idlist[1] + ")");
                }

                else if (idlist[0] == "SettlementType")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                }
                else if (idlist[0] == "SettlementNo")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Number)),''),isnull(ltrim(rtrim(Settlements_Number)),'')", "Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Settlements_Number like  '" + reqStr + "%'");
                }
                else if (idlist[0] == "Commission")
                {
                    if (idlist[1] == "Sub Broker")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and cnt_internalid like 'SB%' and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (idlist[1] == "Relationship Partner")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and cnt_internalid like 'RA%' and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (idlist[1] == "Both")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and (cnt_internalid like 'SB%' or cnt_internalid like 'RA%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                }
                else if (idlist[0] == "Clientsstpprovider")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_stpprovider is not null");
                }

                else if (idlist[0] == "TradeCategory")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("trans_CustomerTrades", "distinct customertrades_tradecategory ,customertrades_tradecategory", " customertrades_tradecategory is not null and  CustomerTrades_ContractNoteNumber IS NULL and CustomerTrades_Customerid='" + idlist[1] + "' and CustomerTrades_productseriesid='" + idlist[2] + "' and CustomerTrades_TradeDate='" + idlist[3] + "' and CustomerTrades_IsLocked is null and customertrades_tradecategory like '" + reqStr + "%' and customertrades_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("trans_ComCustomerTrades", "distinct Comcustomertrades_tradecategory ,Comcustomertrades_tradecategory", " Comcustomertrades_tradecategory is not null and ComCustomerTrades_ContractNoteNumber IS NULL and ComCustomerTrades_Customerid='" + idlist[1] + "' and ComCustomerTrades_productseriesid='" + idlist[2] + "' and ComCustomerTrades_TradeDate='" + idlist[3] + "' and ComCustomerTrades_IsLocked is null and Comcustomertrades_tradecategory like '" + reqStr + "%' and Comcustomertrades_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                }
                else if (idlist[0] == "ScripRespectToClient")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        DT = oDBEngine.GetDataTable("trans_CustomerTrades,Master_Equity", "distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']' as TickerSymbol,Equity_SeriesID", " CustomerTrades_ContractNoteNumber IS NULL and CustomerTrades_Customerid='" + idlist[1] + "' and CustomerTrades_TradeDate='" + idlist[2] + "' and CustomerTrades_IsLocked is null and CustomerTrades_productseriesid=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%' or Equity_TickerCode like '" + reqStr + "%') and customertrades_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(Equity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity,trans_CustomerTrades  WHERE CustomerTrades_ContractNoteNumber IS NULL and CustomerTrades_Customerid='" + idlist[1] + "' and CustomerTrades_TradeDate='" + idlist[2] + "' and CustomerTrades_IsLocked is null and CustomerTrades_productseriesid=Equity_SeriesID and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+convert(varchar(9),commodity_ExpiryDate,6) else isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Commodity_TickerCode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+convert(varchar(9),commodity_ExpiryDate,6)+' '+cast(cast(round(commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_commodity,trans_COMCustomerTrades  WHERE ComCustomerTrades_ContractNoteNumber IS NULL and comCustomerTrades_Customerid='" + idlist[1] + "' and comCustomerTrades_TradeDate='" + idlist[2] + "' and comCustomerTrades_IsLocked is null and comCustomerTrades_productseriesid=Commodity_ProductSeriesID and commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Commodity_ProductSeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }

                }

                else if (idlist[0] == "ScripCriteria")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
                    {
                        DT = oDBEngine.GetDataTable("Master_Equity", "distinct top 10 isnull(Equity_TickerSymbol,'')+'['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']' as TickerSymbol,Equity_SeriesID", "Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and " + idlist[1] + " and (Equity_TickerSymbol like  '" + reqStr + "%' or Equity_Series like '" + reqStr + "%' or Equity_TickerCode like '" + reqStr + "%') ");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(Equity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity  WHERE  " + idlist[1] + " and Equity_ExchSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Equity_SeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("(select (case when isnull(commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+convert(varchar(9),commodity_ExpiryDate,6) else isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Commodity_TickerCode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+convert(varchar(9),commodity_ExpiryDate,6)+' '+cast(cast(round(commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_commodity  WHERE  " + idlist[1] + " and commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "')as tb", " distinct top 15 TickerSymbol,Commodity_ProductSeriesID", "TickerSymbol like  '" + reqStr + "%'");
                    }

                }
                else if (idlist[0] == "MAILEMPLOYEE")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId ", " top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid ", " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='EM'  and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%' or tbl_master_email.eml_email like '" + reqStr + "%') ");
                }
                else if (idlist[0] == "OrdernoCriteria")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_ORDERNUMBER,ExchangeTrades_ORDERNUMBER", " " + idlist[1] + " AND ExchangeTrades_ORDERNUMBER Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_COMExchangeTrades ", "distinct top 10  COMExchangeTrades_ORDERNUMBER,COMExchangeTradess_ORDERNUMBER", " " + idlist[1] + " AND COMExchangeTrades_ORDERNUMBER Like '" + reqStr + "%' and COMExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and COMExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and COMExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and COMExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and COMExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                    }
                }

                else if (idlist[0] == "TerminalIdCriteria")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_TerminalID,ExchangeTrades_TerminalID", " " + idlist[1] + " AND ExchangeTrades_TerminalID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("trans_comexchangetrades", "distinct top 10  comexchangetrades_terminalid,comexchangetrades_terminalid", " " + idlist[1] + " AND comexchangetrades_terminalid Like '" + reqStr + "%' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                }
                else if (idlist[0] == "CTCLIdCriteria")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", " " + idlist[1] + " AND ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("trans_comexchangetrades", "distinct top 10  ComExchangeTrades_CTCLID,ComExchangeTrades_CTCLID", " " + idlist[1] + " AND ComExchangeTrades_CTCLID Like '" + reqStr + "%' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }

                }
                else if (idlist[0] == "CliCustomerucc")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  rtrim(ltrim(ExchangeTrades_customerucc)),rtrim(ltrim(ExchangeTrades_customerucc))", " " + idlist[1] + " AND ExchangeTrades_customerucc Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_COMExchangeTrades ", "distinct top 10  rtrim(ltrim(ComExchangeTrades_customerucc)),rtrim(ltrim(ComExchangeTrades_customerucc))", " " + idlist[1] + " AND ComExchangeTrades_customerucc Like '" + reqStr + "%' and COMExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and COMExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and COMExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and COMExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and COMExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");

                    }

                }

                else if (idlist[0] == "CommDeliveryPositionSettlementNumber")
                {
                    DT = oDBEngine.GetDataTable("Trans_CommDeliveryPosition", "distinct top 10  CommDeliveryPosition_SettlementNumber,CommDeliveryPosition_SettlementNumber", " CommDeliveryPosition_BranchID in(" + Session["userbranchHierarchy"] + ")  and CommDeliveryPosition_SegmentID='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and CommDeliveryPosition_CompanyID='" + Session["LastCompany"].ToString() + "' and (CommDeliveryPosition_SettlementNumber like '" + reqStr + "%')");
                }
                else if (idlist[0] == "FetchSettlementTypeSpot")
                {
                    DT = oDBEngine.GetDataTable("MASTER_SETTLEMENTTYPE", "distinct top 10  rtrim(SETTLEMENTTYPE_Description)+' [ '+SETTLEMENTTYPE_TypeSuffix+' ]',SETTLEMENTTYPE_TypeSuffix", " SETTLEMENTTYPE_Exchangesegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "' and Settlementtype_isT2t is not null and (SETTLEMENTTYPE_TypeSuffix like '" + reqStr + "%')");
                }
                else if (idlist[0] == "ICIN")
                {
                    DT = oDBEngine.GetDataTable("master_icin", "distinct top 10  rtrim(icin_number),rtrim(icin_number)", " icin_productseriesid='" + idlist[1] + "' and (icin_number like '" + reqStr + "%')");
                }
                else if (idlist[0] == "ClientDP")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contactdpdetails,tbl_master_depositoryparticipants", "distinct top 10  rtrim(replace(dp_dpname,char(160),''))+' ['+rtrim(dpd_clientid)+']' +'['+rtrim(dpd_accounttype)+']',dpd_id", " dpd_CNTID='" + idlist[1] + "' and dpd_dpcode=dp_dpid");
                }
                if (idlist[0] == "NsdlClients")
                {
                    DT = oDBEngine.GetDataTable("master_nsdlclients", "top 10 rtrim(nsdlclients_benfirstholdername)+' ['+isnull(rtrim(nsdlclients_benaccountid),'')+']' ,rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid)", "  (nsdlclients_benaccountid  Like '" + reqStr + "%' or nsdlclients_benfirstholdername Like '" + reqStr + "%')  ");
                }
                else if (idlist[0] == "NsdlClientsBranchGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("master_nsdlclients,tbl_master_branch", "top 10 rtrim(nsdlclients_benfirstholdername)+' ['+isnull(rtrim(nsdlclients_benaccountid),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid)", " nsdlclients_branchid=branch_id and (nsdlclients_benaccountid like '" + reqStr + "%' or nsdlclients_benfirstholdername like '" + reqStr + "%') and nsdlclients_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and nsdlclients_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers)");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_nsdlclients,tbl_master_branch", "top 10 rtrim(nsdlclients_benfirstholdername)+' ['+isnull(rtrim(nsdlclients_benaccountid),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid)", " nsdlclients_branchid=branch_id and (nsdlclients_benaccountid like '" + reqStr + "%' or nsdlclients_benfirstholdername like '" + reqStr + "%') and nsdlclients_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and nsdlclients_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + idlist[2] + "))");
                    }
                }
                else if (idlist[0] == "NsdlClientsGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("master_nsdlclients", "top 10 rtrim(nsdlclients_benfirstholdername)+' ['+isnull(rtrim(nsdlclients_benaccountid),'')+']' ,rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid)", "  (nsdlclients_benaccountid like '" + reqStr + "%' or nsdlclients_benfirstholdername like '" + reqStr + "%') and rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid) in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_nsdlclients", "top 10 rtrim(nsdlclients_benfirstholdername)+' ['+isnull(rtrim(nsdlclients_benaccountid),'')+']' ,rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid)", "  (nsdlclients_benaccountid like '" + reqStr + "%' or nsdlclients_benfirstholdername like '" + reqStr + "%') and rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid) in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                    }
                }
                else if (idlist[0] == "NsdlClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("master_nsdlclients,tbl_master_branch", "top 10 rtrim(nsdlclients_benfirstholdername)+' ['+isnull(rtrim(nsdlclients_benaccountid),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid)", " nsdlclients_branchid=branch_id and (nsdlclients_benaccountid like '" + reqStr + "%' or nsdlclients_benfirstholdername like '" + reqStr + "%') and nsdlclients_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_nsdlclients,tbl_master_branch", "top 10 rtrim(nsdlclients_benfirstholdername)+' ['+isnull(rtrim(nsdlclients_benaccountid),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid)", " nsdlclients_branchid=branch_id and (nsdlclients_benaccountid like '" + reqStr + "%' or nsdlclients_benfirstholdername like '" + reqStr + "%') and nsdlclients_branchid in(" + idlist[2] + ")");
                    }
                }
                else if (idlist[0] == "CdslClients")
                {
                    DT = oDBEngine.GetDataTable("master_cdslclients", "top 10 rtrim(cdslclients_firstholdername)+' ['+isnull(rtrim(cdslclients_benaccountnumber),'')+']' ,rtrim(cdslclients_BOID)", "  (cdslclients_benaccountnumber  Like '" + reqStr + "%' or cdslclients_firstholdername Like '" + reqStr + "%')  ");
                }
                else if (idlist[0] == "CdslClientsBranchGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("master_cdslclients,tbl_master_branch", "top 10 rtrim(cdslclients_firstholdername)+' ['+isnull(rtrim(cdslclients_benaccountnumber),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,cdslclients_BOID", "  and cdslclients_branchid=branch_id and (cdslclients_benaccountnumber  Like '" + reqStr + "%' or cdslclients_firstholdername Like '" + reqStr + "%') and cdslclients_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cdslclients_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers)");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_cdslclients,tbl_master_branch", "top 10 rtrim(cdslclients_firstholdername)+' ['+isnull(rtrim(cdslclients_benaccountnumber),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,cdslclients_BOID", " and cdslclients_branchid=branch_id and (cdslclients_benaccountnumber  Like '" + reqStr + "%' or cdslclients_firstholdername Like '" + reqStr + "%') and cdslclients_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cdslclients_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + idlist[2] + "))");
                    }
                }
                else if (idlist[0] == "CdslClientsGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("master_cdslclients", "top 10 rtrim(cdslclients_firstholdername)+' ['+isnull(rtrim(cdslclients_benaccountnumber),'')+']' ,cdslclients_BOID", " (cdslclients_benaccountnumber  Like '" + reqStr + "%' or cdslclients_firstholdername Like '" + reqStr + "%') and cdslclients_BOID in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_cdslclients", "top 10 rtrim(cdslclients_firstholdername)+' ['+isnull(rtrim(cdslclients_benaccountnumber),'')+']' ,cdslclients_BOID", " (cdslclients_benaccountnumber  Like '" + reqStr + "%' or cdslclients_firstholdername Like '" + reqStr + "%') and cdslclients_BOID in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                    }
                }
                else if (idlist[0] == "CdslClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("master_cdslclients,tbl_master_branch", "top 10 rtrim(cdslclients_firstholdername)+' ['+isnull(rtrim(cdslclients_benaccountnumber),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,cdslclients_BOID", " cdslclients_branchid=branch_id and (cdslclients_benaccountnumber  Like '" + reqStr + "%' or cdslclients_firstholdername Like '" + reqStr + "%') and cdslclients_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_cdslclients,tbl_master_branch", "top 10 rtrim(cdslclients_firstholdername)+' ['+isnull(rtrim(cdslclients_benaccountnumber),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,cdslclients_BOID", " cdslclients_branchid=branch_id and (cdslclients_benaccountnumber  Like '" + reqStr + "%' or cdslclients_firstholdername Like '" + reqStr + "%') and cdslclients_branchid in(" + idlist[2] + ")");
                    }
                }
                else if (idlist[0] == "SubAccount")
                {
                    DT = oDBEngine.GetDataTable("master_subaccount ", "distinct top 10 SubAccount_Name+' ['+(case when SubAccount_Code like 'CL%' then (Select cnt_ucc from tbl_master_contact where cnt_internalid=SubAccount_Code) else SubAccount_Code end)+']' as SubAccount_Name,SubAccount_Code ", "(SubAccount_Name like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%') and " + idlist[1] + " ");
                }
                else if (idlist[0] == "SettlementNoCriteria")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable(" Master_Settlements ", " top 10 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)+'  ' + REPLACE(CONVERT(VARCHAR(9), settlements_StartDateTime, 6), ' ', '-') AS [DD-Mon-YY],RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)", " " + idlist[1] + " Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ((RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)) like '" + reqStr + "%' or CONVERT(VARCHAR(9), settlements_StartDateTime, 6) like '" + reqStr + "%')");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("Trans_CommDeliveryPosition", "distinct top 10  rtrim(CommDeliveryPosition_SettlementNumber)+rtrim(CommDeliveryPosition_settlementtype),rtrim(CommDeliveryPosition_SettlementNumber)+rtrim(CommDeliveryPosition_settlementtype)", " CommDeliveryPosition_BranchID in(" + Session["userbranchHierarchy"] + ")  and CommDeliveryPosition_SegmentID='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and CommDeliveryPosition_CompanyID='" + Session["LastCompany"].ToString() + "' and (CommDeliveryPosition_settlementtype like '" + reqStr + "%' or CommDeliveryPosition_Settlementnumber like '" + reqStr + "%')");
                    }
                }
                else if (idlist[0] == "ClientDPOtherAc")
                {
                    if (idlist[1] == "R")
                        DT = oDBEngine.GetDataTable("(Select distinct rtrim(replace(dp_dpname,char(160),''))+' ['+rtrim(dpd_clientid)+']' +'['+rtrim(dpd_accounttype)+']' as Acname,dpd_id as DpId,Case When dpd_accounttype='Default' Then 0 Else 3 End As TxtColor  From tbl_master_contactdpdetails,tbl_master_depositoryparticipants Where dpd_CNTID='" + idlist[2] + "' and dpd_dpcode=dp_dpid Union All Select distinct rtrim(Dpaccounts_Shortname),Dpaccounts_id,Case When rtrim(Dpaccounts_AccountType)='[MRGIN]' Then 1 Else 2 End As TxtColor From Master_Dpaccounts Where Dpaccounts_AccountType in ('[MRGIN]','[HOLDBK]') and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "' ) as Kk", "Acname,'TxtColor~'+Cast(DpId as Varchar)+'~'+Cast(TxtColor as Varchar)", " (RTRIM(Acname) like '" + reqStr + "%' or DpId like '" + reqStr + "%' )");
                    if (idlist[1] == "P")
                        DT = oDBEngine.GetDataTable("Master_Dpaccounts", "rtrim(Dpaccounts_Shortname) as Acname,Dpaccounts_id as DpId", "Dpaccounts_AccountType='[Own]' and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "' ", " (RTRIM(Dpaccounts_Shortname) like '" + reqStr + "%' or Dpaccounts_id like '" + reqStr + "%' )");
                }
                else if (idlist[0] == "ClientDPOtherAcSpot")
                {
                    if (idlist[1] == "R")
                        DT = oDBEngine.GetDataTable("(Select distinct rtrim(replace(dp_dpname,char(160),''))+' ['+rtrim(dpd_clientid)+']' +'['+rtrim(dpd_accounttype)+']' as Acname,dpd_id as DpId,Case When dpd_accounttype='Default' Then 0 Else 3 End As TxtColor  From tbl_master_contactdpdetails,tbl_master_depositoryparticipants Where dpd_CNTID='" + idlist[2] + "' and rtrim(dpd_accounttype) in ('CommodityDP','CommodityDP Sec') and dpd_dpcode=dp_dpid Union All Select distinct rtrim(Dpaccounts_Shortname),Dpaccounts_id,Case When rtrim(Dpaccounts_AccountType)='[MRGIN]' Then 1 Else 2 End As TxtColor From Master_Dpaccounts Where Dpaccounts_AccountType in ('[MRGIN]','[HOLDBK]') and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "' ) as Kk", "Acname,'TxtColor~'+Cast(DpId as Varchar)+'~'+Cast(TxtColor as Varchar)", " (RTRIM(Acname) like '" + reqStr + "%' or DpId like '" + reqStr + "%' )");
                    if (idlist[1] == "P")
                        DT = oDBEngine.GetDataTable("Master_Dpaccounts", "rtrim(Dpaccounts_Shortname) as Acname,Dpaccounts_id as DpId", "Dpaccounts_AccountType='[Own]' and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "' ", " (RTRIM(Dpaccounts_Shortname) like '" + reqStr + "%' or Dpaccounts_id like '" + reqStr + "%' )");
                }
                else if (idlist[0] == "SettlementNoAll")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements ", " top 10 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)+'  ' + REPLACE(CONVERT(VARCHAR(9), settlements_StartDateTime, 6), ' ', '-') AS [DD-Mon-YY],RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)", "Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Settlements_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and ((RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)) like '" + reqStr + "%' or CONVERT(VARCHAR(9), settlements_StartDateTime, 6) like '" + reqStr + "%')");
                }
                else if (idlist[0] == "Company")
                {
                    DT = oDBEngine.GetDataTable("tbl_Master_Company ", " top 10  LTRIM(RTrim(cmp_Name))+' ['+LTRIM(Rtrim(cmp_internalid))+']',LTRIM(Rtrim(Cmp_Internalid))", "cmp_Name like '%" + reqStr + "%'");
                }
                else if (idlist[0] == "ClientParam")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'+' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchID=branch_id and " + idlist[1] + "='" + idlist[2] + "' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }

                else
                    Response.Write("0###No Record Found|");

            }
            #endregion
            #region AcSearch
            if (Request.QueryString["AcSearch"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');

                if (idlist[0] == "MainAcc")
                {
                    DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_AccountCode+'~'+MainAccount_SubLedgerType,MainAccount_Name + ' ['+MainAccount_AccountCode+']'", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_SubLedgerType in ('Customers','CDSL Clients','NSDL Clients','None','Brokers','Custom','Sub Brokers','Creditors','Debtors')");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region AcSearchClientOnly
            if (Request.QueryString["AcSearchClientOnly"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');

                if (idlist[0] == "MainAcc")
                {
                    DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_AccountCode+'~'+MainAccount_SubLedgerType,MainAccount_Name + ' ['+MainAccount_AccountCode+']'", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_SubLedgerType in ('Customers')");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion


            #region settNoForAuction
            if (Request.QueryString["settNoForAuction"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable(" Master_Settlements ", " top 10 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix),RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)", " Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + "  and (RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)) like '" + reqStr + "%' ");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion



            #region Searchdematclientid
            if (Request.QueryString["Searchdematclientid"] == "1")
            {
                string parameter = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (Session["dp"].ToString().ToString() == "CDSL")
                {
                    DT = oDBEngine.GetDataTable("master_cdslisin", "distinct top 10 (cdslisin_number+'[ '+cdslisin_shortname+' ]') as cdslisin_number, (cdslisin_number+'~'+'[ '+cdslisin_shortname+' ]') as name", "cdslisin_number like '%" + reqStr + "%' or cdslisin_shortname like '%" + reqStr + "%'");
                }
                else
                {
                    DT = oDBEngine.GetDataTable("Master_NsdlClients", "Top 10 (NsdlClients_DPID+cast(NsdlClients_BenAccountID as varchar(20))) as NsdlClients_BenAccountID, (cast(NsdlClients_BenAccountID as varchar(20))+' ['+ltrim(rtrim(NsdlClients_benfirstholderName))+' ]') as NsdlClients_ShortName", "(NsdlClients_BenAccountID like  '%" + reqStr + "%' or NsdlClients_ShortName like  '%" + reqStr + "%' ) and NsdlClients_bentype<>6 ");
                }

                if (DT.Rows.Count != 0)
                {

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {

                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");

            }
            #endregion

            #region SearchCustomer
            if (Request.QueryString["SearchCustomer"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("tbl_master_contact", "  distinct top 10 cnt_internalId,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "(cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and cnt_internalid like 'CL%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region ShowClientFORMarginStocksMainHead
            if (Request.QueryString["ShowClientFORMarginStocksMainHead"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Clients")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'+' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                }
                else if (idlist[0] == "Scrips")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID=1");
                }
                else if (idlist[0] == "ScripsExchange")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID='" + Session["ExchangeSegmentID"].ToString() + "'");
                }
                else if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                }
                else if (idlist[0] == "SegmentCM")
                {
                    DT = oDBEngine.GetDataTable("(select isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,exch_internalId from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid like 'CM%') as D", "*", null);
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "TerminalId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_TerminalID,ExchangeTrades_TerminalID", "ExchangeTrades_TerminalID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "CTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "TERMINALSELECTEDCTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_TerminalID in(" + idlist[1] + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[3] == "'SYSTM00042'")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("(Select  cdslclients_benaccountnumber,ltrim(rtrim(cdslclients_firstholdername))+' ['+cast(cdslclients_benaccountnumber as varchar)+']' +' ['+rtrim(branch_description)+']' as cdslclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=CdslClients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='CDSL')+ cdslclients_benaccountnumber as Number	from master_cdslclients,tbl_master_branch where CdslClients_BranchID=branch_id ) as DD", "top 10 cdslclients_firstholdername,cdslclients_benaccountnumber", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "')) and (cdslclients_benaccountnumber like '" + reqStr + "%' or UCC like '" + reqStr + "%' or cdslclients_benaccountnumber like '" + reqStr + "%')");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("(Select cdslclients_benaccountnumber,ltrim(rtrim(cdslclients_firstholdername))+' ['+cast(cdslclients_benaccountnumber as varchar)+']' +' ['+rtrim(branch_description)+']' as cdslclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=CdslClients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='CDSL')+ cdslclients_benaccountnumber as Number	from master_cdslclients,tbl_master_branch where CdslClients_BranchID=branch_id ) as DD", "top 10 cdslclients_firstholdername,cdslclients_benaccountnumber", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(" + idlist[2] + ")) and (cdslclients_benaccountnumber like '" + reqStr + "%' or UCC like '" + reqStr + "%'  or cdslclients_benaccountnumber like '" + reqStr + "%')");
                        }
                    }
                    else if (idlist[3] == "'SYSTM00043'")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("(Select  nsdlclients_benaccountid,ltrim(rtrim(nsdlclients_benfirstholdername))+' ['+cast(nsdlclients_benaccountid as varchar)+']' +' ['+rtrim(branch_description)+']' as nsdlclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=nsdlclients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='NSDL')+ cast(nsdlclients_benaccountid as varchar) as Number	from master_nsdlclients,tbl_master_branch where NsdlClients_BranchID=branch_id) as DD", "top 10 nsdlclients_firstholdername,nsdlclients_benaccountid", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "')) and (nsdlclients_benaccountid like '" + reqStr + "%' or UCC like '" + reqStr + "%' or nsdlclients_firstholdername like '" + reqStr + "%')");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("(Select  nsdlclients_benaccountid,ltrim(rtrim(nsdlclients_benfirstholdername))+' ['+cast(nsdlclients_benaccountid as varchar)+']' +' ['+rtrim(branch_description)+']' as nsdlclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=nsdlclients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='NSDL')+ cast(nsdlclients_benaccountid as varchar) as Number	from master_nsdlclients,tbl_master_branch where NsdlClients_BranchID=branch_id) as DD", "top 10 nsdlclients_firstholdername,nsdlclients_benaccountid", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(" + idlist[2] + ")) and (nsdlclients_benaccountid like '" + reqStr + "%' or UCC like '" + reqStr + "%' or nsdlclients_firstholdername like '" + reqStr + "%')");
                        }
                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' +' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_BranchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' +' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_BranchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        if (idlist[3] == "")
                        {
                            DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code)else SubAccount_Code end as InterNalID from Master_SubAccount) as DD ", " top 10 SubAccount_Name+' ['+InterNalID+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        }
                        else if (idlist[3] != "")
                        {
                            if (idlist[3] == "'SYSTM00043'")
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,(select cnt_ucc from tbl_master_contact where cnt_internalID=(select NSDLClients_ContactID from Master_NSDLClients where NSDLClients_BenAccountID=SubAccount_Code)) as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(SubAccount_Code),'')+']'+'['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select NsdlClients_BranchID from Master_NsdlClients where NsdlClients_BenAccountID=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code ", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%'");
                            else if (idlist[3] == "'SYSTM00042'")
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,(select cnt_ucc from tbl_master_contact where cnt_internalID=(select CDSLClients_ContactID from Master_CDSLClients where CDSLClients_BenAccountNumber=SubAccount_Code)) as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(SubAccount_Code),'')+']'+'['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select CdslClients_BranchID from Master_CdslClients where CdslClients_BenAccountNumber=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code ", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%'");
                            else
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code)else SubAccount_Code end as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        }
                    }
                    else
                    {
                        if (idlist[3] == "")
                        {
                            DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code) else SubAccount_Code end as InterNalID,AccountsLedger_BranchID from Master_SubAccount,Trans_AccountsLedger where AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and AccountsLedger_BranchID in(" + idlist[2] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+rtrim(InterNalID)+']'+' ['+(select rtrim(branch_description) from tbl_master_branch where AccountsLedger_BranchID=branch_id)+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        }
                        else if (idlist[3] != "")
                        {
                            DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code) else SubAccount_Code end as InterNalID,AccountsLedger_BranchID from Master_SubAccount,Trans_AccountsLedger where AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and AccountsLedger_BranchID in(" + idlist[2] + ") and SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+rtrim(InterNalID)+']'+' ['+(select rtrim(branch_description) from tbl_master_branch where AccountsLedger_BranchID=branch_id)+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        }
                    }
                }
                else if (idlist[0] == "SettlementType")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                }
                else if (idlist[0] == "MainAcc")
                {
                    DataTable dtBr = new DataTable();
                    dtBr = oDBEngine.GetDataTable("tbl_master_branch", "branch_parentid", " branch_id IN (select cnt_branchid from tbl_master_contact where cnt_internalid='" + Session["usercontactID"].ToString() + "')");
                    if (dtBr.Rows.Count > 0)
                    {

                        if (dtBr.Rows[0][0].ToString() == "0")
                        {

                            //DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) and MainAccount_SubLedgerType<>'None' ");
                            DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_SubLedgerType<>'None' ");
                        }
                        else
                        {
                            //DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) and MainAccount_SubLedgerType<>'None' AND   MainAccount_SubLedgerType IN  ('CDSL Clients','Customers ','NSDL Clients')");
                            DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_SubLedgerType<>'None' AND   MainAccount_SubLedgerType IN  ('CDSL Clients','Customers ','NSDL Clients')");
                        }
                    }
                    else
                    {
                        //DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) and MainAccount_SubLedgerType<>'None' AND   MainAccount_SubLedgerType IN  ('CDSL Clients','Customers ','NSDL Clients')");
                        DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_SubLedgerType<>'None' AND   MainAccount_SubLedgerType IN  ('CDSL Clients','Customers ','NSDL Clients')");
                    }
                    //  DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) and MainAccount_SubLedgerType<>'None'");
                }
                else if (idlist[0] == "Sub Ac")
                {
                    DT = oDBEngine.GetDataTable("master_subaccount ", "distinct top 10 SubAccount_Name+' ['+SubAccount_Code+']' as SubAccount_Name,SubAccount_Code ", "(SubAccount_Name like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%') and SubAccount_MainAcReferenceID=" + idlist[2] + "");
                }
                else if (idlist[0] == "Employee")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId ", " top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid ", " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='EM'  and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%' or tbl_master_email.eml_email like '" + reqStr + "%') ");
                }
                else if (idlist[0] == "MailToEmployee")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId ", " top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId +'^'+tbl_master_email.eml_email as contactid ", " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='EM'  and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%' or tbl_master_email.eml_email like '" + reqStr + "%') ");
                }
                else if (idlist[0] == "Company")
                {
                    DT = oDBEngine.GetDataTable("tbl_Master_Company ", " top 10  LTRIM(RTrim(cmp_Name))+' ['+LTRIM(Rtrim(cmp_internalid))+']',LTRIM(Rtrim(Cmp_Internalid))", "cmp_Name like '%" + reqStr + "%'");
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion

            #region ShowClientFORMCDSL
            if (Request.QueryString["ShowClientFORMCDSL"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "' and  gpm_id in (select distinct grp_groupMaster  from tbl_trans_group where grp_ContactId like 'IN%')");
                }
                else if (idlist[0] == "Clients")
                {

                    if (idlist[4] != "")
                    {
                        string where = null; //" Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlHolding.NsdlHolding_BenAccountNumber ";
                        //where = where + " and NsdlHolding_HoldingDateTime='" + idlist[1].ToString() + "'";
                        where = where + "  NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupType='" + idlist[4].ToString() + "' and grp_contactId like 'IN%'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " and NsdlClients_BenType='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + " and ( NsdlClients_BenAccountID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' )  ";
                        string orderBy = " NsdlClients_BenFirstHolderName ";
                        // DT = oDBEngine.GetDataTable("Trans_NsdlHolding,Master_NsdlClients, tbl_trans_group ", " distinct top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast([NsdlClients_BenAccountID]  as varchar(100)) +'' +' ]') as AccName ,NsdlClients_BenAccountID  As ID, NsdlClients_BenFirstHolderName", where, orderBy);
                        DT = oDBEngine.GetDataTable(" ( Select  distinct NsdlClients_BenFirstHolderName,NsdlClients_BenAccountID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID=(select  N.NSDLClients_ContactID from Master_NSDLClients as N where N.NsdlClients_BenAccountID=Master_NsdlClients.NsdlClients_BenAccountID)) as InterNalID from Master_NsdlClients,tbl_trans_group WHERE  " + where + " ) as DD ", " top 5 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", "  ID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' ");
                        //DT = oDBEngine.GetDataTable("Master_NSDLClients,tbl_master_contact,tbl_trans_group", " distinct top 10 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(NsdlClients_BenAccountID  as varchar(100)) +'' +' ]'  + ' ['+(case when NSDLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,NsdlClients_BenAccountID  As ID", " (NSDLClients_ContactID=cnt_internalID or NSDLClients_ContactID is null) and (NsdlClients_BenAccountID like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");

                    }
                    else
                    {
                        string where = null;//" Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlHolding.NsdlHolding_BenAccountNumber ";
                        //where = where + " and NsdlHolding_HoldingDateTime='" + idlist[1].ToString() + "'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " where NsdlClients_BenType='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + " and ( NsdlClients_BenAccountID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' )  ";

                        string orderBy = " NsdlClients_BenFirstHolderName ";
                        //DT = oDBEngine.GetDataTable("Trans_NsdlHolding,Master_NsdlClients ", " distinct top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast([NsdlClients_BenAccountID]  as varchar(100)) +'' +' ]') as AccName ,NsdlClients_BenAccountID  As ID, NsdlClients_BenFirstHolderName", where, orderBy);
                        DT = oDBEngine.GetDataTable(" ( Select  distinct NsdlClients_BenFirstHolderName,NsdlClients_BenAccountID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID=(select  N.NSDLClients_ContactID from Master_NSDLClients as N where N.NsdlClients_BenAccountID=Master_NsdlClients.NsdlClients_BenAccountID)) as InterNalID from Master_NsdlClients  " + where + " ) as DD ", " top 5 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", "  ID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' ");
                        //DT = oDBEngine.GetDataTable("Master_NSDLClients,tbl_master_contact", " distinct top 10 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(NsdlClients_BenAccountID  as varchar(100)) +'' +' ]'  + ' ['+(case when NSDLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,NsdlClients_BenAccountID  As ID", " (NSDLClients_ContactID=cnt_internalID or NSDLClients_ContactID is null) and (NsdlClients_BenAccountID like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");
                    }
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[4] == "ALL")
                    {
                        // DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        string where = null;//" Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlHolding.NsdlHolding_BenAccountNumber ";
                        //where = where + " and NsdlHolding_HoldingDateTime='" + idlist[1].ToString() + "'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " where NsdlClients_BenType='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + " and ( NsdlClients_BenAccountID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' )  ";
                        string orderBy = " NsdlClients_BenFirstHolderName ";
                        //DT = oDBEngine.GetDataTable("Trans_NsdlHolding,Master_NsdlClients ", " distinct top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast([NsdlClients_BenAccountID]  as varchar(100)) +'' +' ]') as AccName ,NsdlClients_BenAccountID  As ID, NsdlClients_BenFirstHolderName", where, orderBy);
                        DT = oDBEngine.GetDataTable(" ( Select  distinct NsdlClients_BenFirstHolderName,NsdlClients_BenAccountID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID=(select  N.NSDLClients_ContactID from Master_NSDLClients as N where N.NsdlClients_BenAccountID=Master_NsdlClients.NsdlClients_BenAccountID)) as InterNalID from Master_NsdlClients  " + where + " ) as DD ", " top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", "  ID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' ");
                        // DT = oDBEngine.GetDataTable("Master_NSDLClients,tbl_master_contact", " distinct top 10 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(NsdlClients_BenAccountID  as varchar(100)) +'' +' ]'  + ' ['+(case when NSDLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,NsdlClients_BenAccountID  As ID", " (NSDLClients_ContactID=cnt_internalID or NSDLClients_ContactID is null) and (NsdlClients_BenAccountID like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");

                    }
                    else
                    {
                        // DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        string where = null;//" Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlHolding.NsdlHolding_BenAccountNumber ";
                        //where = where + " and NsdlHolding_HoldingDateTime='" + idlist[1].ToString() + "'";
                        where = where + " NsdlClients_DPID +Cast(NsdlClients_BenAccountID as Varchar(10))=grp_contactId and grp_groupMaster in (" + idlist[5] + ") and grp_contactId like 'IN%'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " and NsdlClients_BenType='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + " and ( NsdlClients_BenAccountID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' )  ";
                        string orderBy = " NsdlClients_BenFirstHolderName ";
                        //DT = oDBEngine.GetDataTable("Trans_NsdlHolding,Master_NsdlClients, tbl_trans_group ", " distinct top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast([NsdlClients_BenAccountID]  as varchar(100)) +'' +' ]') as AccName ,NsdlClients_BenAccountID  As ID, NsdlClients_BenFirstHolderName", where, orderBy);
                        DT = oDBEngine.GetDataTable(" ( Select  distinct NsdlClients_BenFirstHolderName,NsdlClients_BenAccountID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID=(select  N.NSDLClients_ContactID from Master_NSDLClients as N where N.NsdlClients_BenAccountID=Master_NsdlClients.NsdlClients_BenAccountID)) as InterNalID from Master_NsdlClients,tbl_trans_group WHERE  " + where + " ) as DD ", " top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", "  ID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' ");
                        //DT = oDBEngine.GetDataTable("Master_NSDLClients,tbl_master_contact,tbl_trans_group", " distinct top 10 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(NsdlClients_BenAccountID  as varchar(100)) +'' +' ]'  + ' ['+(case when NSDLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,NsdlClients_BenAccountID  As ID", " (NSDLClients_ContactID=cnt_internalID or NSDLClients_ContactID is null) and (NsdlClients_BenAccountID like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[4] == "ALL")
                    {

                        //   DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        string where = null;//" Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlHolding.NsdlHolding_BenAccountNumber ";
                        //where = where + " and NsdlHolding_HoldingDateTime='" + idlist[1].ToString() + "'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " where NsdlClients_BenType='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + " and ( NsdlClients_BenAccountID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' )  ";
                        string orderBy = " NsdlClients_BenFirstHolderName ";
                        // DT = oDBEngine.GetDataTable("Trans_NsdlHolding,Master_NsdlClients ", " distinct top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast([NsdlClients_BenAccountID]  as varchar(100)) +'' +' ]') as AccName ,NsdlClients_BenAccountID  As ID, NsdlClients_BenFirstHolderName", where, orderBy);
                        DT = oDBEngine.GetDataTable(" ( Select  distinct NsdlClients_BenFirstHolderName,NsdlClients_BenAccountID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID=(select  N.NSDLClients_ContactID from Master_NSDLClients as N where N.NsdlClients_BenAccountID=Master_NsdlClients.NsdlClients_BenAccountID)) as InterNalID from Master_NsdlClients  " + where + " ) as DD ", " top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", "  ID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' ");
                        //DT = oDBEngine.GetDataTable("Master_NSDLClients,tbl_master_contact", " distinct top 10 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(NsdlClients_BenAccountID  as varchar(100)) +'' +' ]'  + ' ['+(case when NSDLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,NsdlClients_BenAccountID  As ID", " (NSDLClients_ContactID=cnt_internalID or NSDLClients_ContactID is null) and (NsdlClients_BenAccountID like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");
                    }
                    else
                    {
                        // DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");


                        string where = null;//" Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlHolding.NsdlHolding_BenAccountNumber ";
                        //where = where + " and NsdlHolding_HoldingDateTime='" + idlist[1].ToString() + "'";
                        where = where + " NsdlClients_Branchid in ('" + idlist[5].ToString() + "')";

                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " and NsdlClients_BenType='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + " and ( NsdlClients_BenAccountID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' )  ";
                        string orderBy = " NsdlClients_BenFirstHolderName ";
                        //DT = oDBEngine.GetDataTable("Trans_NsdlHolding,Master_NsdlClients ", " distinct top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast([NsdlClients_BenAccountID]  as varchar(100)) +'' +' ]') as AccName ,NsdlClients_BenAccountID  As ID, NsdlClients_BenFirstHolderName", where, orderBy);
                        DT = oDBEngine.GetDataTable(" ( Select  distinct NsdlClients_BenFirstHolderName,NsdlClients_BenAccountID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID=(select  N.NSDLClients_ContactID from Master_NSDLClients as N where N.NsdlClients_BenAccountID=Master_NsdlClients.NsdlClients_BenAccountID)) as InterNalID from Master_NsdlClients WHERE  " + where + " ) as DD ", " top 5 (NsdlClients_BenFirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", "  ID Like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' ");
                        //DT = oDBEngine.GetDataTable("Master_NSDLClients,tbl_master_contact", " distinct top 10 (rtrim(NsdlClients_BenFirstHolderName) + '   [ ' + cast(NsdlClients_BenAccountID  as varchar(100)) +'' +' ]'  + ' ['+(case when NSDLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,NsdlClients_BenAccountID  As ID", " (NSDLClients_ContactID=cnt_internalID or NSDLClients_ContactID is null) and (NsdlClients_BenAccountID like '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");
                    }
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion

            #region CDSLHoldingClientsGroups
            if (Request.QueryString["CDSLHoldingClientsGroups"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                string[] date = idlist[1].Split(' ');
                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "' and  gpm_id in (select distinct grp_groupMaster  from tbl_trans_group where len(grp_ContactId)=16)");
                }
                else if (idlist[0] == "Clients")
                {

                    if (idlist[4] != "")
                    {

                        string where = null;

                        //where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM' and '" + idlist[1].ToString() + "'";

                        where = where + "  cdslclients_BOID =grp_contactId and  grp_groupType='" + idlist[4].ToString() + "' and grp_contactID like '" + Session["usersegid"].ToString() + "%'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " and CdslClients_BOStatus='" + idlist[2].ToString() + "' ";
                        }


                        DT = oDBEngine.GetDataTable("(Select  distinct CdslClients_FirstHolderName,CdslClients_BOID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID in (select  N.CDSLClients_ContactID from Master_CDSLClients as N where N.CdslClients_BOID=Master_CdslClients.CdslClients_BOID)) as InterNalID from Master_CdslClients,tbl_trans_group where " + where + ") as DD", " top 10 (CdslClients_FirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", " ID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");

                        //DT = oDBEngine.GetDataTable("Master_CdslClients,tbl_master_contact,tbl_trans_group", " distinct top 10 (CdslClients_FirstHolderName + '   [ ' + cast(CdslClients_BOID  as varchar(100)) +'' +' ]'  + ' ['+(case when CDSLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,CdslClients_BOID  As ID", " (CDSLClients_ContactID=cnt_internalID or CDSLClients_ContactID is null) and (CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");

                    }
                    else
                    {

                        string where = null;//"Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";

                        //where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM' and '" + idlist[1].ToString() + "'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " where CdslClients_BOStatus='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + "and ( CdslClients_BOID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' )  ";

                        //DT = oDBEngine.GetDataTable("Trans_CdslHolding,Master_CdslClients ", "distinct top 10  (CdslClients_FirstHolderName + '   [ ' + cast([CdslClients_BOID]  as varchar(100)) +'' +' ]') as AccName, CdslClients_BOID  As ID", where);

                        DT = oDBEngine.GetDataTable("(Select  distinct CdslClients_FirstHolderName,CdslClients_BOID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID in (select  N.CDSLClients_ContactID from Master_CDSLClients as N where N.CdslClients_BOID=Master_CdslClients.CdslClients_BOID)) as InterNalID from Master_CdslClients " + where + ") as DD", " top 10 (CdslClients_FirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", " ID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");

                        //DT = oDBEngine.GetDataTable("Master_CdslClients,tbl_master_contact", " distinct top 10 (CdslClients_FirstHolderName + '   [ ' + cast(CdslClients_BOID  as varchar(100)) +'' +' ]'  + ' ['+(case when CDSLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,CdslClients_BOID  As ID", " (CDSLClients_ContactID=cnt_internalID or CDSLClients_ContactID is null) and (CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                    }
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[4] == "ALL")
                    {
                        // DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");

                        string where = null;//"Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";

                        //where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM' and '" + idlist[1].ToString() + "'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " where CdslClients_BOStatus='" + idlist[2].ToString() + "' ";
                        }

                        DT = oDBEngine.GetDataTable("(Select  distinct CdslClients_FirstHolderName,CdslClients_BOID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID in (select  N.CDSLClients_ContactID from Master_CDSLClients as N where N.CdslClients_BOID=Master_CdslClients.CdslClients_BOID)) as InterNalID from Master_CdslClients  " + where + ") as DD", " top 10 (CdslClients_FirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", " ID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        //DT = oDBEngine.GetDataTable("Master_CdslClients,tbl_master_contact", " distinct top 10 (CdslClients_FirstHolderName + '   [ ' + cast(CdslClients_BOID  as varchar(100)) +'' +' ]'  + ' ['+(case when CDSLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,CdslClients_BOID  As ID", " (CDSLClients_ContactID=cnt_internalID or CDSLClients_ContactID is null) and (CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");


                    }
                    else
                    {
                        // DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");


                        string where = null;//"Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";

                        //where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM' and '" + idlist[1].ToString() + "'";

                        where = where + "cdslclients_BOID =grp_contactId and  grp_groupMaster in (" + idlist[5] + ") and grp_contactID like '" + Session["usersegid"].ToString() + "%'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " and CdslClients_BOStatus='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + "and ( CdslClients_BOID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' )  ";

                        //DT = oDBEngine.GetDataTable("Trans_CdslHolding,Master_CdslClients, tbl_trans_group ", "distinct top 10  (CdslClients_FirstHolderName + '   [ ' + cast([CdslClients_BOID]  as varchar(100)) +'' +' ]') as AccName, CdslClients_BOID  As ID", where);

                        DT = oDBEngine.GetDataTable("(Select  distinct CdslClients_FirstHolderName,CdslClients_BOID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID in (select  N.CDSLClients_ContactID from Master_CDSLClients as N where N.CdslClients_BOID=Master_CdslClients.CdslClients_BOID)) as InterNalID from Master_CdslClients,tbl_trans_group where " + where + ") as DD", " top 10 (CdslClients_FirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", " ID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        //DT = oDBEngine.GetDataTable("Master_CdslClients,tbl_master_contact,tbl_trans_group", " distinct top 10 (CdslClients_FirstHolderName + '   [ ' + cast(CdslClients_BOID  as varchar(100)) +'' +' ]'  + ' ['+(case when CDSLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,CdslClients_BOID  As ID", " (CDSLClients_ContactID=cnt_internalID or CDSLClients_ContactID is null) and (CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");

                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[4] == "ALL")
                    {

                        //   DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");

                        string where = null;//"Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";

                        //where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM' and '" + idlist[1].ToString() + "'";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " where CdslClients_BOStatus='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + "and ( CdslClients_BOID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' )  ";

                        //DT = oDBEngine.GetDataTable("Trans_CdslHolding,Master_CdslClients ", "distinct top 10  (CdslClients_FirstHolderName + '   [ ' + cast([CdslClients_BOID]  as varchar(100)) +'' +' ]') as AccName, CdslClients_BOID  As ID", where);

                        DT = oDBEngine.GetDataTable("(Select  distinct CdslClients_FirstHolderName,CdslClients_BOID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID in (select  N.CDSLClients_ContactID from Master_CDSLClients as N where N.CdslClients_BOID=Master_CdslClients.CdslClients_BOID)) as InterNalID from Master_CdslClients " + where + ") as DD", " top 10 (CdslClients_FirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", " ID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        //DT = oDBEngine.GetDataTable("Master_CdslClients,tbl_master_contact", " distinct top 10 (CdslClients_FirstHolderName + '   [ ' + cast(CdslClients_BOID  as varchar(100)) +'' +' ]'  + ' ['+(case when CDSLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,CdslClients_BOID  As ID", " (CDSLClients_ContactID=cnt_internalID or CDSLClients_ContactID is null) and (CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");

                    }
                    else
                    {
                        // DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");


                        string where = null;//"Master_CdslClients.CdslClients_BOID=LTRIM(RTRIM(CdslHolding_DPID))+ LTRIM(RTRIM(CdslHolding_BenAccountNumber)) ";

                        //where = where + "and CdslHolding_HoldingDateTime between '" + date[0] + " 00:00:00 AM' and '" + idlist[1].ToString() + "'";
                        where = where + " cdslClients_BranchID in ('" + idlist[5].ToString() + "')";
                        if (idlist[2].ToString() != "All")
                        {
                            where = where + " and CdslClients_BOStatus='" + idlist[2].ToString() + "' ";
                        }

                        //where = where + "and ( CdslClients_BOID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' )  ";

                        //DT = oDBEngine.GetDataTable("Trans_CdslHolding,Master_CdslClients ", "distinct top 10  (CdslClients_FirstHolderName + '   [ ' + cast([CdslClients_BOID]  as varchar(100)) +'' +' ]') as AccName, CdslClients_BOID  As ID", where);

                        DT = oDBEngine.GetDataTable("(Select  distinct CdslClients_FirstHolderName,CdslClients_BOID  As ID,(select  cnt_ucc from tbl_master_contact where cnt_internalID in (select  N.CDSLClients_ContactID from Master_CDSLClients as N where N.CdslClients_BOID=Master_CdslClients.CdslClients_BOID)) as InterNalID from Master_CdslClients where " + where + ") as DD", " top 10 (CdslClients_FirstHolderName + '   [ ' + cast(ID  as varchar(100)) +'' +' ]'  + ' ['+isnull(InterNalID,'')+']') as AccName,ID", " ID Like '%" + reqStr + "%' or CdslClients_FirstHolderName Like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");

                        //DT = oDBEngine.GetDataTable("Master_CdslClients,tbl_master_contact", " distinct top 10 (CdslClients_FirstHolderName + '   [ ' + cast(CdslClients_BOID  as varchar(100)) +'' +' ]'  + ' ['+(case when CDSLClients_ContactID is not null then isnull(rtrim(cnt_ucc),'') else '' end)+']') as AccName,CdslClients_BOID  As ID", " (CDSLClients_ContactID=cnt_internalID or CDSLClients_ContactID is null) and (CdslClients_BOID like '%" + reqStr + "%' or CdslClients_FirstHolderName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') " + where + "");
                    }
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion

            #region PortFolio
            if (Request.QueryString["PortFolio"] == "1")
            {
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string ExSegName = oGenericMethod.GetExchangeSegmentName();
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                if (param == "T")
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_contactExchange", " top 10 isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+']',cnt_internalID", " crg_exchange='" + ExSegName + "' and crg_company='" + Session["LastCompany"].ToString() + "' and (cnt_firstname like '" + reqStr + "%' or cnt_UCC like '" + reqStr + "%') and cnt_internalID like 'CL%' and cnt_clienttype='Pro Trading' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                else if (param == "I")
                    DT = oDBEngine.GetDataTable("tbl_master_contact", " top 10 isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+']',cnt_internalID", "  (cnt_firstname like '" + reqStr + "%' or cnt_UCC like '" + reqStr + "%') and cnt_internalID like 'CL%' and cnt_clienttype='Pro Investment' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                else
                    DT = oDBEngine.GetDataTable("tbl_master_contact", " top 10 isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+']',cnt_internalID", "  (cnt_firstname like '" + reqStr + "%' or cnt_UCC like '" + reqStr + "%') and cnt_internalID like 'CL%' and (cnt_clienttype not in('Pro Trading','Pro Investment') or cnt_clienttype is null) and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");
            }
            #endregion
            #region ProductIDForPortFolio
            if (Request.QueryString["ProductIDForPortFolio"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_Equity", " top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']',cast(Equity_ProductID as varchar)+'~'+cast(Equity_SeriesID as varchar)", "  Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID in(1,4) ");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");
            }
            #endregion
            #region PortFolioISIN
            if (Request.QueryString["PortFolioISIN"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                DT = oDBEngine.GetDataTable("Master_ISIN", " top 10 ISIN_Number,ISIN_Number", "  ISIN_ProductSeriesID=" + param + " and ISIN_Number like '" + reqStr + "%' ");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");
            }
            #endregion
            #region InterAccountTransferOtherAccount
            if (Request.QueryString["InterAccountTransferOtherAccount"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_mainaccount", " top 10 mainaccount_Name,mainaccount_AccountCode", "  mainaccount_SubLedgerType='None' and MainAccount_Name like '" + reqStr + "%' ");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");
            }
            #endregion

            #region InterSettlementForISINAccounts
            if (Request.QueryString["InterSettlementForISINAccounts"] == "1")
            {
                string parameter = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                reqStr = reqStr.Replace("_", "&");
                string ISIN = null;
                string[] param = parameter.Split('~');
                if (param.Length > 1)
                {
                    ISIN = param[1];
                }
                parameter = param[0];
                if (parameter == "Client")
                    DT = oDBEngine.GetDataTable("tbl_master_contact", " distinct top 10 cnt_internalid,isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'", " cnt_internalid in(select DematPosition_CustomerID from Trans_DematPosition where DematPosition_CustomerID like 'CL%' and DematPosition_SegmentID=" + Session["usersegid"].ToString() + ") and (cnt_firstname like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                else if (parameter == "ISIN")
                    DT = oDBEngine.GetDataTable("Trans_DematPosition", " distinct top 10 DematPosition_ISIN,DematPosition_ISIN", " DematPosition_CustomerID like 'CL%' and DematPosition_SegmentID=" + Session["usersegid"].ToString() + " and DematPosition_ISIN like '" + reqStr + "%'");
                else if (parameter == "Product")
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 equity_seriesid,isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']'", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID in(1,4)");
                else if (parameter == "SettSource")
                    DT = oDBEngine.GetDataTable("(select  rtrim(Settlements_Number)+rtrim(Settlements_TypeSuffix) as ID,rtrim(Settlements_Number)+rtrim(Settlements_TypeSuffix) as Number from Master_Settlements where Settlements_FinYear='" + Session["LastFinYear"].ToString() + "' and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + ") as KK ", " distinct top 10 ID,Number", " Number like '" + reqStr + "%'");
                else if (parameter == "ISINNew")
                    DT = oDBEngine.GetDataTable("Master_ISIN", " distinct top 10 substring(ISIN_Number,1,12),ISIN_Number", " ISIN_Number like '" + reqStr + "%' and ISIN_ProductSeriesID='" + ISIN + "' and ISIN_IsActive='Y'");
                else if (parameter == "ISINOld")
                    DT = oDBEngine.GetDataTable("Master_ISIN", " distinct top 10 substring(ISIN_Number,1,12),ISIN_Number", " ISIN_Number like '" + reqStr + "%' and ISIN_ProductSeriesID='" + ISIN + "'");
                else if (parameter == "AccountName")
                    DT = oDBEngine.GetDataTable("Master_DPAccounts", " distinct top 10 DPAccounts_ID,DPAccounts_ShortName", " DPAccounts_ShortName like '" + reqStr + "%' and DPAccounts_AccountType not in('[POOL]')");
                else if (parameter == "AccountNameCheck")
                    DT = oDBEngine.GetDataTable("Master_DPAccounts", " distinct top 10 cast(DPAccounts_ID as varchar)+'~'+rtrim(DPAccounts_AccountType),DPAccounts_ShortName", " DPAccounts_ShortName like '" + reqStr + "%'");
                else if (parameter == "SettNumber")
                    DT = oDBEngine.GetDataTable("(select rtrim(Settlements_Number)+Settlements_TypeSuffix as SettNum,rtrim(Settlements_Number)+Settlements_TypeSuffix as SettType from Master_Settlements) as DD ", " distinct top 10 SettNum,SettType ", " SettNum like '" + reqStr + "%'");
                else if (parameter == "ClientAdj")
                    DT = oDBEngine.GetDataTable("tbl_master_contact", " distinct top 10 cnt_internalid+'~'+cast(cnt_branchID as varchar)+'~'+'Customer',isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastname),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'", " cnt_internalid in(select DematPosition_CustomerID from Trans_DematPosition where DematPosition_CustomerID like 'CL%' and DematPosition_SegmentID=" + Session["usersegid"].ToString() + ") and (cnt_firstname like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                else if (parameter == "AccountNameE")
                    DT = oDBEngine.GetDataTable("Master_DPAccounts", " distinct top 10 cast(DPAccounts_ID as varchar)+'~'+cast(DPAccounts_ExchangeSegmentID as varchar),DPAccounts_ShortName", " DPAccounts_ShortName like '" + reqStr + "%' and DPAccounts_AccountType not in('[POOL]')");
                if (DT.Rows.Count != 0)
                {

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {

                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region GetAllGroup
            if (Request.QueryString["GetAllGroup"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (param == "ALL")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_groupmaster ", " top 10 isnull(gpm_Description,'')+'['+ rtrim(ltrim(isnull(gpm_Code,'')))+']' as GroupName,gpm_id", "gpm_Description like '" + reqStr + "%'  or  gpm_code Like '" + reqStr + "%' ");

                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_groupmaster ", " top 10 isnull(gpm_Description,'')+'['+ rtrim(ltrim(isnull(gpm_Code,'')))+']' as GroupName,gpm_id ", " gpm_Type='" + param + "' AND (gpm_Description like '" + reqStr + "%'  or gpm_code Like '" + reqStr + "%' )");
                }
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion



            #region GetBrokerageName
            if (Request.QueryString["GetBrokerageName"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (param == "ALL")
                {
                    DT = oDBEngine.GetDataTable(" Master_ChargeGroup ", "  top 10 ChargeGroup_Name+ '['+rtrim(ltrim(isnull(ChargeGroup_Code,'')))+']',ChargeGroup_Code ", " ChargeGroup_Name like '" + reqStr + "%'  or  ChargeGroup_Code Like '" + reqStr + "%' ");

                }
                else
                {
                    DT = oDBEngine.GetDataTable(" Master_ChargeGroup ", " top 10 ChargeGroup_Name+ '['+rtrim(ltrim(isnull(ChargeGroup_Code,'')))+']',ChargeGroup_Code ", " ChargeGroup_Type='" + param + "' AND (ChargeGroup_Name like '" + reqStr + "%'  or  ChargeGroup_Code Like '" + reqStr + "%' )");
                }
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion

            #region GetHeaderFooter
            if (Request.QueryString["GetHeaderFooter"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable(" Master_HeaderFooter ", "  top 10 HeaderFooter_ShortName,HeaderFooter_ID ", " HeaderFooter_ShortName like '" + reqStr + "%' and HeaderFooter_Type='" + param + "' ");
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region SubBrokerageName
            if (Request.QueryString["SubBrokerageName"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (param == "SUBBROKER")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_shortname),'')+']' ,cnt_internalID", " cnt_internalId like 'Sb%' and (cnt_firstName like '" + reqStr + "%' or cnt_shortname like '" + reqStr + "%')");
                }
                else if (param == "PARTNERDEALER")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_shortname),'')+']' ,cnt_internalID", " cnt_internalId like 'Ra%' and (cnt_firstName like '" + reqStr + "%' or cnt_shortname like '" + reqStr + "%')");
                }
                else if (param == "ALL")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact,config_subbrokeragemain", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_shortname),'')+']' ,cnt_internalID", "subbrokeragemain_CompanyID='" + Session["LastCompany"].ToString() + "' and subbrokeragemain_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and cnt_internalId=subbrokeragemain_contactid and (cnt_internalId like 'Ra%' OR cnt_internalId like 'Sb%')and (cnt_firstName like '" + reqStr + "%' or cnt_shortname like '" + reqStr + "%')");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");
            }
            #endregion
            #region ShowClienthavingcontractnotenumber
            if (Request.QueryString["ShowClienthavingcontractnotenumber"] == "1")
            {
                //string parameter = Convert.ToDateTime(Request.QueryString["search_param"]).ToString("yyyy-MM-dd");
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                int number = 0;
                string strdate = "";
                if (HttpContext.Current.Session["Tradedate"] != null)
                {
                    strdate = HttpContext.Current.Session["Tradedate"].ToString();

                }
                else
                {
                    number = idlist.Length - 1;
                    strdate = idlist[number].ToString();
                }


                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Clients")
                {
                    if (idlist[1] == "Dealers")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_trans_group,tbl_master_groupMaster", "top 10 Customername ,cnt_internalID from(Select Distinct isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' as Customername ,cnt_internalID from (Select * from (Select grp_ContactID,grp_groupmaster,grp_grouptype,gpm_code,gpm_Type,gpm_Description,gpm_id", "grp_groupType='" + idlist[1] + "' and gpm_id=grp_groupmaster)as a1 Inner join tbl_master_contact on(a1.grp_contactid=cnt_internalID and cnt_contactType like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')))as a2 Inner Join trans_contractnotes on(ContractNotes_CustomerID=a2.cnt_internalID and contractnotes_tradedate='" + idlist[2] + "' and ContractNotes_Status is null))as a3");

                    }

                }
                else if (idlist[0] == "Scrips")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID=1");
                }
                else if (idlist[0] == "ScripsExchange")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID='" + Session["ExchangeSegmentID"].ToString() + "'");
                }
                else if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                }
                else if (idlist[0] == "SegmentCM")
                {
                    DT = oDBEngine.GetDataTable("(select isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,exch_internalId from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid like 'CM%') as D", "*", null);
                }
                else if (idlist[0] == "Branch")
                {
                    if (Session["userbranchHierarchy"].ToString() != "")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_branch", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                }
                else if (idlist[0] == "TerminalId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_TerminalID,ExchangeTrades_TerminalID", "ExchangeTrades_TerminalID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "CTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "TERMINALSELECTEDCTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_TerminalID in(" + idlist[1] + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,trans_contractnotes", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "ContractNotes_TradeDate='" + strdate + "' and ContractNotes_SegmentId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ContractNotes_Status is null and ContractNotes_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ContractNotes_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ContractNotes_CompanyID='" + Session["LastCompany"].ToString() + "' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                    }
                    else if (idlist[1] == "Selected")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,trans_contractnotes", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "ContractNotes_TradeDate='" + strdate + "' and ContractNotes_SegmentId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ContractNotes_Status is null and ContractNotes_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ContractNotes_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ContractNotes_CompanyID='" + Session["LastCompany"].ToString() + "' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupMaster in(" + idlist[2].Substring(1, idlist[2].Length - 2) + "))");
                    }
                    else if (idlist[1] == "DeSelected")
                    {
                        if (idlist[2].Contains("CL"))
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,trans_contractnotes", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " ContractNotes_TradeDate='" + strdate + "' and ContractNotes_SegmentId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ContractNotes_Status is null and ContractNotes_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ContractNotes_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ContractNotes_CompanyID='" + Session["LastCompany"].ToString() + "' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_contactid not in(" + idlist[2] + "))");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,trans_contractnotes", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " ContractNotes_TradeDate='" + strdate + "' and ContractNotes_SegmentId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ContractNotes_Status is null and ContractNotes_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ContractNotes_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ContractNotes_CompanyID='" + Session["LastCompany"].ToString() + "' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster not in(" + idlist[2].Substring(1, idlist[2].Length - 2) + "))");

                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        //DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "CN.ContractNotes_TradeDate='" + strdate + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' and CN.ContractNotes_Status is null and ContractNotes_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ContractNotes_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' AND C.cnt_internalId=CN.ContractNotes_CustomerID AND cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (idlist[1] == "Selected")
                    {
                        //DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                        //DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "CN.ContractNotes_TradeDate='" + parameter + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' AND C.cnt_internalId=CN.ContractNotes_CustomerID AND cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "CN.ContractNotes_TradeDate='" + strdate + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' and CN.ContractNotes_Status is null and ContractNotes_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ContractNotes_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' AND C.cnt_internalId=CN.ContractNotes_CustomerID AND cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in (" + idlist[2] + ")");

                    }
                    else if (idlist[1] == "DeSelected")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "CN.ContractNotes_TradeDate='" + strdate + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' and CN.ContractNotes_Status is null and ContractNotes_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ContractNotes_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' AND C.cnt_internalId=CN.ContractNotes_CustomerID AND cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid not in (" + idlist[2] + ")");

                    }
                }
                else if (idlist[0] == "SettlementType")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                }
                else if (idlist[0] == "Commission")
                {
                    if (idlist[1] == "Sub Broker")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and cnt_internalid like 'SB%' and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (idlist[1] == "Relationship Partner")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and cnt_internalid like 'RA%' and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (idlist[1] == "Both")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and (cnt_internalid like 'SB%' or cnt_internalid like 'RA%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }

                else
                    Response.Write("0###No Record Found|");

            }
            #endregion
            #region SearchByEmp
            if (Request.QueryString["SearchByEmp"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

               // DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId  and tbl_master_employee.emp_contactId not in('" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "') and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
                DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId   and cnt_contactType='EM'  and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");

                // DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and tbl_master_employee.emp_contactId not in('" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "') and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchByEmpRportto
            if (Request.QueryString["SearchByEmpRportto"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                // DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId  and tbl_master_employee.emp_contactId not in('" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "') and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
                DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId   and cnt_contactType='EM'  and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");

                // DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and tbl_master_employee.emp_contactId not in('" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "') and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchSUBBRKGSETUP_BRKGCLIENTGROUP
            if (Request.QueryString["SearchSUBBRKGSETUP_BRKGCLIENTGROUP"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                string[] BrokParam = param.Split('-');
                if (BrokParam[1] == "2")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", " top 10 rtrim(ltrim(ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, ''))) +'['+ISNULL(rtrim(cnt_ucc), '')+']' AS Name,cnt_internalid as id", "cnt_internalid in (SELECT GRP_CONTACTID FROM tbl_trans_group WHERE GRP_GROUPMASTER=(select gpm_id from tbl_master_groupmaster WHERE gpm_owner='" + BrokParam[0] + "') and GRP_CONTACTID like 'CL%') and (cnt_firstName Like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                }
                else if (BrokParam[1] == "3")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster", " top 10 ISNULL(rtrim(GPM_DESCRIPTION), '')+'['+GPM_CODE+']' AS Name,GPM_ID as id", "GPM_ID IN (SELECT DISTINCT GRP_GROUPMASTER FROM tbl_trans_group WHERE GRP_CONTACTID IN (SELECT GRP_CONTACTID FROM tbl_trans_group WHERE GRP_GROUPMASTER=(select gpm_id from tbl_master_groupmaster WHERE gpm_owner='" + BrokParam[0] + "')) AND GRP_GROUPTYPE='Family') and (GPM_DESCRIPTION Like '" + reqStr + "%' or GPM_CODE like '" + reqStr + "%')");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region Searchproductandeffectuntil
            if (Request.QueryString["Searchproductandeffectuntil"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');

                if (idlist[0] == "Product")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("master_products", " top 10 isnull(rtrim(products_name),'')+' '+'['+isnull(rtrim(products_shortname),'')+']' as  Name, products_id as Id", "products_producttypeid in(1,5) and  (products_shortname Like '" + reqStr + "%' or products_name like '" + reqStr + "%')");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "7" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "9" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "10" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "11" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "12")
                    {
                        DT = oDBEngine.GetDataTable("master_products", " top 10 isnull(rtrim(products_name),'')+' '+'['+isnull(rtrim(products_shortname),'')+']' as  Name, products_id as Id", "products_producttypeid in(10) and  (products_shortname Like '" + reqStr + "%' or products_name like '" + reqStr + "%')");
                    }
                    else if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "3" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "6" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "8")
                    {
                        DT = oDBEngine.GetDataTable("master_products", " top 10 isnull(rtrim(products_name),'')+' '+'['+isnull(rtrim(products_shortname),'')+']' as  Name, products_id as Id", "products_producttypeid in(8) and  (products_shortname Like '" + reqStr + "%' or products_name like '" + reqStr + "%')");
                    }
                }
                else if (idlist[0] == "Expiry")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
                    {
                        DT = oDBEngine.GetDataTable("master_equity", " distinct top 10  CONVERT(VARCHAR(11),CAST(equity_effectuntil AS DATETIME),106) AS EXPIRY , equity_effectuntil as Id", "CONVERT(VARCHAR(11),CAST(equity_effectuntil AS DATETIME),106) Like '" + reqStr + "%' and equity_effectuntil>='" + idlist[1] + "'", "equity_effectuntil");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("master_commodity", " distinct top 10  CONVERT(VARCHAR(11),CAST(commodity_expirydate AS DATETIME),106) AS EXPIRY , commodity_expirydate as Id", "CONVERT(VARCHAR(11),CAST(commodity_expirydate AS DATETIME),106) Like '" + reqStr + "%' and Commodity_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and commodity_expirydate>='" + idlist[1] + "'", "commodity_expirydate");

                    }
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchSettlementNumberAll
            if (Request.QueryString["SearchSettlementNumberAll"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                if (param == "S")
                    DT = oDBEngine.GetDataTable("Master_Settlements", "distinct top 10 rtrim(Settlements_Number) as Number,rtrim(Settlements_Number) as Number1", " Settlements_FinYear='" + Session["LastFinYear"].ToString() + "' and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and  Settlements_Number like '" + reqStr + "%'");
                else
                    DT = oDBEngine.GetDataTable("Master_Settlements", "distinct top 10 rtrim(Settlements_Number) as Number,rtrim(Settlements_Number) as Number1", " Settlements_FinYear='" + Session["LastFinYear"].ToString() + "' and Settlements_ExchangeSegmentID in(1,4) and  Settlements_Number like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region SearchClientIdbyDpId
            {
                if (Request.QueryString["SearchClientIdbyDpId"] == "1")
                {
                    string dpid;
                    string parameter = Request.QueryString["search_param"].ToString();
                    string reqStr = Request.QueryString["letters"].ToString();
                    //if (Session["dp"].ToString() == "NSDL" || Session["dp"].ToString() == "9")
                    //{
                    //    dpid = parameter.Split('~')[1].ToString();
                    //    DT = oDBEngine.GetDataTable("Master_NsdlClients", "Top 10 (NsdlClients_DPID+cast(NsdlClients_BenAccountID as varchar(20))) as NsdlClients_BenAccountID, (cast(NsdlClients_BenAccountID as varchar(20))+' ['+ltrim(rtrim(NsdlClients_ShortName))+' ]') as NsdlClients_ShortName", "(NsdlClients_BenAccountID like  '%" + reqStr + "%' or NsdlClients_ShortName like  '%" + reqStr + "%' )and NsdlClients_DPID='" + dpid + "'");
                    //}
                    //else if (Session["dp"].ToString() == "CDSL" || Session["dp"].ToString() == "10")
                    //{
                    //    dpid = parameter.Split('~')[1].ToString();
                    //    DT = oDBEngine.GetDataTable("Master_CdslClients", "Top 10 (cast(CdslClients_DPID as varchar(20))+cast(substring([CdslClients_BOID],9,8) as varchar(20))) as [CdslClients_BOID], (cast(substring([CdslClients_BOID],9,8) as varchar(20))+' ['+ltrim(rtrim([CdslClients_FirstHolderName]))+' ]') as CdslClients_ShortName ", "([CdslClients_BOID] like  '%" + reqStr + "%' or [CdslClients_FirstHolderName] like  '%" + reqStr + "%')and CdslClients_DPID=substring('" + dpid + "',4,5)");
                    //}

                    DataTable DT1 = new DataTable();
                    DataTable DT2 = new DataTable();
                    int rCount1 = 0;
                    int rCount2 = 0;
                    dpid = parameter.Split('~')[1].ToString();
                    //DT1 = oDBEngine.GetDataTable("Master_NsdlClients", "Top 10 (NsdlClients_DPID+cast(NsdlClients_BenAccountID as varchar(20)) + '_n') as AccountID, (cast(NsdlClients_BenAccountID as varchar(20))+' ['+ltrim(rtrim(NsdlClients_ShortName))+' ]n') as ShortName", "(NsdlClients_BenAccountID like  '%" + reqStr + "%' or NsdlClients_ShortName like  '%" + reqStr + "%' )and NsdlClients_DPID='" + dpid + "'");

                    //DT2 = oDBEngine.GetDataTable("Master_CdslClients", "Top 10 (cast(CdslClients_DPID as varchar(20))+cast(substring([CdslClients_BOID],9,8) as varchar(20)) + '_c') as [AccountID], (cast(substring([CdslClients_BOID],9,8) as varchar(20))+' ['+ltrim(rtrim([CdslClients_FirstHolderName]))+' ]c') as ShortName ", "([CdslClients_BOID] like  '%" + reqStr + "%' or [CdslClients_FirstHolderName] like  '%" + reqStr + "%')and CdslClients_DPID=substring('" + dpid + "',4,5)");

                    DT1 = oDBEngine.GetDataTable("Master_NsdlClients", "Top 10 (NsdlClients_DPID+cast(NsdlClients_BenAccountID as varchar(20)) + '_n') as AccountID, (cast(NsdlClients_BenAccountID as varchar(20))+' ['+ltrim(rtrim(NsdlClients_BenFirstHolderName))+' ] ['  + isnull(NsdlClients_TradingUCC,'') + ']' ) as ShortName", "(NsdlClients_BenAccountID like  '%" + reqStr + "%' or NsdlClients_BenFirstHolderName like  '%" + reqStr + "%' or NsdlClients_TradingUCC like '%" + reqStr + "%' )and NsdlClients_DPID=(select exch_TMCode from tbl_master_companyexchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_membershipType='NSDL')");

                    DT2 = oDBEngine.GetDataTable("Master_CdslClients", "Top 10 (cast(CdslClients_DPID as varchar(20))+cast(substring([CdslClients_BOID],9,8) as varchar(20)) + '_c') as [AccountID], (cast(substring([CdslClients_BOID],9,8) as varchar(20))+' ['+ltrim(rtrim([CdslClients_FirstHolderName]))+' ] [' + isnull(CdslClients_TradingUCC,'') + ']') as ShortName ", "([CdslClients_BOID] like  '%" + reqStr + "%' or [CdslClients_FirstHolderName] like  '%" + reqStr + "%' or CdslClients_TradingUCC like  '%" + reqStr + "%')and CdslClients_DPID=(select exch_TMCode from tbl_master_companyexchange where exch_compId='" + Session["LastCompany"].ToString() + "' and exch_membershipType='CDSL')");

                    DT = DT1.Clone();
                    if (DT1.Rows.Count > 5)
                    {
                        for (rCount1 = 0; rCount1 < 5; rCount1++)
                        {
                            DataRow dr = DT.NewRow();
                            dr[0] = DT1.Rows[rCount1][0].ToString();
                            dr[1] = DT1.Rows[rCount1][1].ToString();
                            DT.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        for (rCount1 = 0; rCount1 < DT1.Rows.Count; rCount1++)
                        {
                            DataRow dr = DT.NewRow();
                            dr[0] = DT1.Rows[rCount1][0].ToString();
                            dr[1] = DT1.Rows[rCount1][1].ToString();
                            DT.Rows.Add(dr);
                        }
                    }

                    for (rCount2 = 0; rCount2 < DT2.Rows.Count; rCount2++)
                    {
                        if (DT.Rows.Count < 10)
                        {
                            DataRow dr = DT.NewRow();
                            dr[0] = DT2.Rows[rCount2][0].ToString();
                            dr[1] = DT2.Rows[rCount2][1].ToString();
                            DT.Rows.Add(dr);
                        }
                        else break;
                    }
                    //if (DT2.Rows.Count > 5)
                    //{
                    //    for (rCount2 = 0; rCount2 < 5; rCount2++)
                    //    {
                    //        DataRow dr = DT.NewRow();
                    //        dr[0] = DT2.Rows[rCount2][0].ToString();
                    //        dr[1] = DT2.Rows[rCount2][1].ToString();
                    //        DT.Rows.Add(dr);
                    //    }
                    //}
                    //else
                    //{
                    //    for (rCount2 = 0; rCount2 < DT2.Rows.Count; rCount2++)
                    //    {
                    //        DataRow dr = DT.NewRow();
                    //        dr[0] = DT2.Rows[rCount2][0].ToString();
                    //        dr[1] = DT2.Rows[rCount2][1].ToString();
                    //        DT.Rows.Add(dr);
                    //    }
                    //}

                    if (DT.Rows.Count < 10)
                    {
                        if (DT1.Rows.Count > (rCount1))
                        {
                            for (int i = rCount1; i < DT1.Rows.Count; i++)
                            {
                                //if (DT1.Rows[i] != null)
                                //{
                                if (DT.Rows.Count < 10)
                                {
                                    DataRow dr = DT.NewRow();
                                    dr[0] = DT1.Rows[i][0].ToString();
                                    dr[1] = DT1.Rows[i][1].ToString();
                                    DT.Rows.Add(dr);
                                }
                                //}
                                else
                                {
                                    break;
                                }

                            }
                        }
                    }
                    // DT.Rows.Add(
                    //   DT.Merge(DT1);
                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                        }
                    }
                    //else
                    // Response.Write("No Record Found###No Record Found|");

                }
            }
            #endregion


            #region SearchByEmpCont
            if (Request.QueryString["SearchByEmpCont"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                //DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId  and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
                DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC ", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId    and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region Narration
            if (Request.QueryString["Narration"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_Narration", " top 10 Narration_Description,Narration_ID", "  Narration_Description like '" + reqStr + "%'");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }

            }
            #endregion

            #region SettlementForExchSegmentWise
            if (Request.QueryString["SettlementForExchSegmentWise"] == "1")
            {
                string parameter = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string ExchID = null;
                string[] Param = parameter.Split('~');
                if (Param[1] == "S")
                    ExchID = Param[0].ToString();
                else if (Param[1] == "T")
                    ExchID = Session["ExchangeSegmentID"].ToString();
                DT = oDBEngine.GetDataTable("(select rtrim(Settlements_Number)+ltrim(rtrim(Settlements_TypeSuffix)) as SettNum from master_settlements where Settlements_ExchangeSegmentID=" + ExchID + ") as DD ", "top 10 SettNum,SettNum", " SettNum like '" + reqStr + "%'");
                if (DT.Rows.Count != 0)
                {

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {

                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region        ScripName
            if (Request.QueryString["ScripName"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();

                string Fields = @" distinct top 10 Equity_TickerSymbol + ' [ '+ rtrim(case when Equity_TickerSymbol like '"
                    + reqStr + @"%' then Equity_Series
                else Equity_TickerCode end)+' ] ' as ScripName, Equity_SeriesID as ID ";

                string where = " Equity_TickerSymbol like '" + reqStr + "%' or Equity_TickerCode like '" + reqStr + "%'";

                DT = oDBEngine.GetDataTable("Master_Equity", Fields, where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region ShowClientFORClientMaster
            if (Request.QueryString["ShowClientFORClientMaster"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Clients")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                }

                else if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                }
                else if (idlist[0] == "SegmentCM")
                {
                    DT = oDBEngine.GetDataTable("(select isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,exch_internalId from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid like 'CM%') as D", "*", null);
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }

                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                    }
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }

                else
                    Response.Write("0###No Record Found|");

            }
            #endregion
            #region SearcBranchWiseEmployee
            if (Request.QueryString["SearcBranchWiseEmployee"] == "1")
            {
                string wherecon = "";
                string reqStr = Request.QueryString["letters"].ToString();
                string[] param = Request.QueryString["search_param"].ToString().Split('~');
                switch (param[0])
                {
                    case "Branch":
                        DT = oDBEngine.GetDataTable(" tbl_master_branch ", " top 10 branch_id,branch_description+'['+branch_code+']' ", " branch_description like '" + reqStr + "%' or branch_code like '" + reqStr + "%'", " branch_description ");
                        break;

                    case "Company":

                        DT = oDBEngine.GetDataTable(" tbl_master_company ", " top 10 cmp_internalid,cmp_Name ", " cmp_Name like'" + reqStr + "%'");
                        break;
                    case "Employee":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 e.emp_contactid,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid");
                        break;
                    case "ReportTo":
                        DT = oDBEngine.GetDataTable("tbl_master_contact contact,tbl_master_employee e", " top 10 emp_id,(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ", " contact.cnt_firstName Like '" + reqStr + "%' and e.emp_contactId=contact.cnt_internalid");
                        break;

                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchGroupCustomer
            if (Request.QueryString["SearchGroupCustomer"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string[] param = Request.QueryString["search_param"].ToString().Split('~');


                string id = param[0].ToString();

                DataTable dtGrp = oDBEngine.GetDataTable("tbl_master_groupmaster", "gpm_MemberType,gpm_type", "gpm_id='" + id + "'");
                string MType = dtGrp.Rows[0]["gpm_MemberType"].ToString();
                string GType = dtGrp.Rows[0]["gpm_type"].ToString();

                string cong = "";
                string con1 = "";
                string con2 = "";
                string con3 = "";
                con2 = " in (select emp_contactId from tbl_master_employee where (emp_DateofLeaving is null OR emp_DateofLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving >=getdate()) and emp_dateOfjoining <= getdate())";
                con1 = " select cnt_firstName+isnull(cnt_middleName,'')+isnull(cnt_lastName,'')+'['+case cnt_contactType when 'CL' then cnt_UCC else cnt_shortName end+']' from tbl_master_contact where cnt_internalid=";
                switch (MType)
                {
                    case "Contacts":
                        con3 = " cnt_internalid ";
                        break;
                    case "Addresses":
                        con3 = " add_cntId ";
                        break;
                    case "Emails":
                        con3 = " eml_cntId ";
                        break;
                    case "Phones":
                        con3 = " phf_cntId ";
                        break;
                }
                switch (param[2].ToString().Trim())
                {
                    case "EM":
                        cong = con3 + con2 + " and cnt_internalid like '" + param[3].ToString().Trim() + "%' and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') ";
                        break;
                    default:
                        cong = con3 + " like '" + param[3].ToString().Trim() + "%' and " + con3 + " not in (SELECT grp_contactId FROM tbl_trans_group WHERE  grp_groupType='" + GType + "') and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')";
                        break;

                }
                if (cong != "" && con1 != "")
                {
                    switch (MType)
                    {
                        case "Contacts":
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+'['+case cnt_contactType when 'CL' then cnt_UCC else cnt_shortName end+']'  as Name, cnt_InternalId as Id", "  " + cong + " and cnt_internalid not in (SELECT grp_contactId FROM tbl_trans_group WHERE  grp_groupType='" + GType + "')   order by cnt_firstName");
                            break;
                        case "Addresses":
                            DT = oDBEngine.GetDataTable("tbl_master_address", " top 10 add_cntId as Id, ((" + con1 + " add_cntId ) + ' | ' + add_addressType + ' | ' + case when add_address1 is null then '' else add_address1 end) as Name", cong);
                            break;
                        case "Emails":
                            DT = oDBEngine.GetDataTable("tbl_master_email", " top 10 eml_cntId as Id, ((" + con1 + " eml_cntId ) + ' | ' + eml_type + ' | ' + case when eml_email is null then '' else eml_email end) as Name", cong);
                            break;
                        case "Phones":
                            DT = oDBEngine.GetDataTable("tbl_master_phonefax", " top 10 phf_cntId as Id, ((" + con1 + " phf_cntId ) + ' | ' + phf_type + ' | ' + case when phf_phoneNumber is null then '' else phf_phoneNumber end) as Name", cong);
                            break;
                    }

                }
                if (param[2].ToString().Trim() == "CDSL Client")
                {
                    DT = oDBEngine.GetDataTable("Master_CDSLClients", " top 10 CDSLClients_FirstHolderName as Name,CDSLClients_BOID as Id", " CDSLClients_FirstHolderName Like '" + reqStr + "%'");

                }
                if (param[2].ToString().Trim() == "NSDL Client")
                {
                    DT = oDBEngine.GetDataTable("Master_NSDLClients", " top 10 NSDLClients_BenFirstHolderName as Name, (NsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))) as Id", " NSDLClients_BenFirstHolderName Like '" + reqStr + "%'");
                }






                //DT = oDBEngine.GetDataTable("tbl_master_contact", "  distinct top 10 cnt_internalId,(isnull(rtrim(cnt_firstName),'') +' '+isnull(rtrim(cnt_middleName),'')+' '+isnull(rtrim(cnt_lastName),''))+'['+isnull(rtrim(cnt_UCC),'')+']' as Name", "(cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') and cnt_internalid like 'CL%'");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region CDSLClientsGroupMember
            if (Request.QueryString["CDSLClientsGroupMember"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                DT = oDBEngine.GetDataTable("master_cdslclients ", " top 10 ltrim(rtrim(CdslClients_FirstHolderName +'['+ right(CdslClients_BOID,8) +']')) +' ' + case when ltrim(rtrim((select  top 1  DPChargeMembers_GroupCode from  trans_DPChargeMembers where DPChargeMembers_BenAccountNumber =right(CdslClients_BOID,8) ))) is null then '' else   ltrim(rtrim((select  top 1 ' Existing GroupCode: '+ DPChargeMembers_GroupCode from  trans_DPChargeMembers where DPChargeMembers_BenAccountNumber =right(CdslClients_BOID,8) ))) end ,right(CdslClients_BOID,8)     ", "  (CdslClients_FirstHolderName Like '" + reqStr + "%' or right(CdslClients_BOID,8) like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region NSDLClientsGroupMember
            if (Request.QueryString["NSDLClientsGroupMember"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                DT = oDBEngine.GetDataTable("master_nsdlclients ", " top 10  rtrim(ltrim( NsdlClients_BenFirstHolderName + '['+ cast(NsdlClients_BenAccountID as varchar(20))+']' )) + +' ' + case when ltrim(rtrim((select  top 1  DPChargeMembers_GroupCode from  trans_DPChargeMembers where DPChargeMembers_BenAccountNumber =NsdlClients_BenAccountID))) is null then '' else   ltrim(rtrim((select  top 1 ' Existing GroupCode: '+ DPChargeMembers_GroupCode from  trans_DPChargeMembers where DPChargeMembers_BenAccountNumber =NsdlClients_BenAccountID))) end ,NsdlClients_BenAccountID      ", "  (NsdlClients_BenFirstHolderName Like '" + reqStr + "%' or NsdlClients_BenAccountID like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region ShowClienthavingcontractnotenumberForCommodity
            if (Request.QueryString["ShowClienthavingcontractnotenumberForCommodity"] == "1")
            {
                //string parameter = Convert.ToDateTime(Request.QueryString["search_param"]).ToString("yyyy-MM-dd");
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                int number = 0;
                string strdate = "";
                if (HttpContext.Current.Session["Tradedate"] != null)
                {
                    strdate = HttpContext.Current.Session["Tradedate"].ToString();

                }
                else
                {
                    number = idlist.Length - 1;
                    strdate = idlist[number].ToString();
                }


                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Clients")
                {
                    if (idlist[1] == "Dealers")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_trans_group,tbl_master_groupMaster", "top 10 Customername ,cnt_internalID from(Select Distinct isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' as Customername ,cnt_internalID from (Select * from (Select grp_ContactID,grp_groupmaster,grp_grouptype,gpm_code,gpm_Type,gpm_Description,gpm_id", "grp_groupType='" + idlist[1] + "' and gpm_id=grp_groupmaster)as a1 Inner join tbl_master_contact on(a1.grp_contactid=cnt_internalID and cnt_contactType like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')))as a2 Inner Join trans_contractnotes on(ContractNotes_CustomerID=a2.cnt_internalID and contractnotes_tradedate='" + idlist[2] + "' and ContractNotes_Status is null))as a3");

                    }

                }
                else if (idlist[0] == "Scrips")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID=1");
                }
                else if (idlist[0] == "ScripsExchange")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID='" + Session["ExchangeSegmentID"].ToString() + "'");
                }
                else if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                }
                else if (idlist[0] == "SegmentCM")
                {
                    DT = oDBEngine.GetDataTable("(select isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,exch_internalId from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid like 'CM%') as D", "*", null);
                }
                else if (idlist[0] == "Branch")
                {
                    if (Session["userbranchHierarchy"].ToString() != "")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_branch", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                }
                else if (idlist[0] == "TerminalId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades ", "distinct top 10  ComExchangeTrades_TerminalID,ComExchangeTrades_TerminalID", "ComExchangeTrades_TerminalID Like '" + reqStr + "%' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "CTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades ", "distinct top 10  ComExchangeTrades_CTCLID,ComExchangeTrades_CTCLID", "ComExchangeTrades_CTCLID Like '" + reqStr + "%' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "TERMINALSELECTEDCTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ComExchangeTrades ", "distinct top 10  ComExchangeTrades_CTCLID,ComExchangeTrades_CTCLID", "ComExchangeTrades_CTCLID Like '" + reqStr + "%' and ComExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ComExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ComExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ComExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ComExchangeTrades_TerminalID in(" + idlist[1] + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,trans_contractnotes", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "ContractNotes_TradeDate='" + strdate + "' and ContractNotes_SegmentId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ContractNotes_Status is null and ContractNotes_CompanyID='" + Session["LastCompany"].ToString() + "' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                    }
                    else if (idlist[1] == "Selected")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,trans_contractnotes", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "ContractNotes_TradeDate='" + strdate + "' and ContractNotes_SegmentId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ContractNotes_Status is null and ContractNotes_CompanyID='" + Session["LastCompany"].ToString() + "' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupMaster in(" + idlist[2].Substring(1, idlist[2].Length - 2) + "))");
                    }
                    else if (idlist[1] == "DeSelected")
                    {
                        if (idlist[2].Contains("CL"))
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,trans_contractnotes", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " ContractNotes_TradeDate='" + strdate + "' and ContractNotes_SegmentId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ContractNotes_Status is null and ContractNotes_CompanyID='" + Session["LastCompany"].ToString() + "' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_contactid not in(" + idlist[2] + "))");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,trans_contractnotes", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " ContractNotes_TradeDate='" + strdate + "' and ContractNotes_SegmentId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ContractNotes_Status is null and ContractNotes_CompanyID='" + Session["LastCompany"].ToString() + "' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster not in(" + idlist[2].Substring(1, idlist[2].Length - 2) + "))");

                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        //DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "CN.ContractNotes_TradeDate='" + strdate + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' and CN.ContractNotes_Status is null AND C.cnt_internalId=CN.ContractNotes_CustomerID AND cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (idlist[1] == "Selected")
                    {
                        //DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                        //DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "CN.ContractNotes_TradeDate='" + parameter + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' AND C.cnt_internalId=CN.ContractNotes_CustomerID AND cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "CN.ContractNotes_TradeDate='" + strdate + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' and CN.ContractNotes_Status is null AND C.cnt_internalId=CN.ContractNotes_CustomerID AND cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in (" + idlist[2] + ")");

                    }
                    else if (idlist[1] == "DeSelected")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact C,trans_ContractNotes CN", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", "CN.ContractNotes_TradeDate='" + strdate + "' And CN.ContractNotes_SegmentId=" + HttpContext.Current.Session["usersegid"] + " AND CN.ContractNotes_CompanyID='" + HttpContext.Current.Session["LastCompany"] + "' and CN.ContractNotes_Status is null AND C.cnt_internalId=CN.ContractNotes_CustomerID AND cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid not in (" + idlist[2] + ")");

                    }
                }
                else if (idlist[0] == "SettlementType")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                }
                else if (idlist[0] == "Commission")
                {
                    if (idlist[1] == "Sub Broker")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and cnt_internalid like 'SB%' and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (idlist[1] == "Relationship Partner")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and cnt_internalid like 'RA%' and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else if (idlist[1] == "Both")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_SHORTNAME),'')+']' ,cnt_internalID", " (cnt_firstName like '" + reqStr + "%' or cnt_SHORTNAME like '" + reqStr + "%') and (cnt_internalid like 'SB%' or cnt_internalid like 'RA%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }

                else
                    Response.Write("0###No Record Found|");

            }
            #endregion


            #region GetTeplate
            if (Request.QueryString["GetTeplate"] == "1")
            {
                //   string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable(" master_templateDetails ", "  top 10 Tmplt_ShortName,Tmplt_ID ", " Tmplt_ShortName like '" + reqStr + "%'");
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchBroker
            if (Request.QueryString["SearchBroker"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();
                //DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName, cnt_internalId", " (cnt_internalId like 'EM%' or cnt_internalId like 'SB%' or cnt_internalId like 'RA%' or cnt_internalId like 'PR%' or cnt_internalId like 'FR%' or cnt_internalId like 'BO%') and (cnt_firstName Like '" + reqStr + "%' )");
                DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName, cnt_internalId", " (cnt_internalId like 'BO%') and (cnt_firstName Like '" + reqStr + "%' or  cnt_UCC Like '" + reqStr + "%' )");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchMainAccount
            if (Request.QueryString["SearchMainAccount"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();

                DT = oDBEngine.GetDataTable("master_mainaccount", "TOP 10 MainAccount_AccountCode +'~'+ isnull(MainAccount_SubLedgerType,''),MainAccount_Name +'['+ isnull(MainAccount_AccountCode,'') +']' ", "  MainAccount_AccountCode not like 'SYS%' and (MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchSubAccount
            if (Request.QueryString["SearchSubAccount"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();
                string param = Request.QueryString["search_param"].ToString();
                string[] SplTx = param.ToString().Split('~');

                DT = oDBEngine.GetDataTable("master_subaccount", "TOP 10 SubAccount_Code , SubAccount_Name + '['+ isnull(SubAccount_Code,'') +']' ", "  subaccount_mainacreferenceid='" + SplTx[0].ToString().Trim() + "'  and (SubAccount_Name Like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region SearchGroupID  // Done By Sudeshna Kundu 15 Dec 2010
            {
                if (Request.QueryString["SearchGroupID"] == "1")
                {
                    string parameter = Request.QueryString["search_param"].ToString();
                    string groupType = parameter;


                    string reqStr = Request.QueryString["letters"].ToString();


                    DT = oDBEngine.GetDataTable("tbl_master_groupMaster", "Top 10 (gpm_Description+' ['+gpm_code+']' )as gpm_des,('!'+cast(gpm_id as varchar(20))+'!gpm_id') as gpm_id", "(gpm_Description like '" + reqStr + "%'  or  gpm_code like '" + reqStr + "%' ) AND ( gpm_Type = '" + groupType + "')");


                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion

            #region SearchBankID
            {         // ==================== Done By Sudeshna Kundu 21 Jan 2011 =====
                if (Request.QueryString["SearchBankID"] == "1")
                {
                    string parameter = Request.QueryString["search_param"].ToString();

                    string reqStr = Request.QueryString["letters"].ToString();

                    DT = oDBEngine.GetDataTable("tbl_master_Bank", "Top 10 (bnk_bankName+' ['+bnk_micrno+']' )as bnk_des,('!'+cast(bnk_id as varchar(20))+'!bnk_id') as bnk_id", "(bnk_bankName like '" + reqStr + "%'  or  bnk_micrno like '" + reqStr + "%' ) ");


                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion

            #region SearchBranchID  // Done By Sudeshna Kundu 15 Dec 2010
            {
                if (Request.QueryString["SearchBranchID"] == "1")
                {
                    string parameter = Request.QueryString["search_param"].ToString();
                    string branchN = parameter;

                    string reqStr = Request.QueryString["letters"].ToString();

                    DT = oDBEngine.GetDataTable("tbl_master_branch", "Top 10 (branch_description+' ['+branch_code+']' )as brn_des,('!'+cast(branch_id as varchar(20))+'!branch_id') as branch_id", "branch_Description like '" + reqStr + "%'  or  branch_code like '" + reqStr + "%' ");

                    if (DT.Rows.Count != 0)
                    {

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {

                            Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion

            #region ShowNSDLClientTypeSubType
            if (Request.QueryString["ShowNSDLClientTypeSubType"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Type")
                {
                    DT = oDBEngine.GetDataTable("(select distinct CASE WHEN nsdlstaticdata_TypeCode=1 THEN 'Resident' when nsdlstaticdata_TypeCode =2 then 'FI' when nsdlstaticdata_TypeCode =3 then 'FII' when nsdlstaticdata_TypeCode =4 then 'NRI' when nsdlstaticdata_TypeCode =5 then 'Body Coprporate' when nsdlstaticdata_TypeCode =6 then 'Clearing Member' when nsdlstaticdata_TypeCode =7 then 'Foreign National' when nsdlstaticdata_TypeCode =8 then 'Mutual Fund' when nsdlstaticdata_TypeCode =10 then 'Bank' when nsdlstaticdata_TypeCode =21 then 'Intermediary' end as AccType,nsdlstaticdata_TypeCode as Code   from master_nsdlstaticdatacode  where  nsdlstaticdata_FieldName like 'Beneficiary Type / Sub type%' ) as B    ", "top 10  *   ", " B.AccType  Like '" + reqStr + "%'  order by  B.AccType  asc ");
                }
                else if (idlist[0] == "SubType")
                {
                    DT = oDBEngine.GetDataTable("master_nsdlstaticdatacode ", "top 10 nsdlstaticdata_Description ,nsdlstaticdata_SubTypeCode ", " nsdlstaticdata_TypeCode=" + idlist[1] + "  and  nsdlstaticdata_FieldName like 'Beneficiary Type / Sub type%' and  nsdlstaticdata_Description like '" + reqStr + "%'");
                }
                else if (idlist[0] == "Client")
                {
                    string where = "";
                    where = " (NsdlClients_BenFirstHolderName Like  '" + reqStr + "%' or NsdlClients_BenAccountID Like  '%" + reqStr + "%')";
                    DT = oDBEngine.GetDataTable(" Master_NsdlClients ", "  top 10  NsdlClients_BenFirstHolderName + '   [ '+ convert(varchar,NsdlClients_BenAccountID) +' ]' as AccName, NsdlClients_BenAccountID  As ID", where);

                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion

            #region ShowCDSLClientTypeSubType
            if (Request.QueryString["ShowCDSLClientTypeSubType"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Type")
                {
                    //  DT = oDBEngine.GetDataTable("(select distinct CASE WHEN nsdlstaticdata_TypeCode=1 THEN 'Resident' when nsdlstaticdata_TypeCode =2 then 'FI' when nsdlstaticdata_TypeCode =3 then 'FII' when nsdlstaticdata_TypeCode =4 then 'NRI' when nsdlstaticdata_TypeCode =5 then 'Body Coprporate' when nsdlstaticdata_TypeCode =6 then 'Clearing Member' when nsdlstaticdata_TypeCode =7 then 'Foreign National' when nsdlstaticdata_TypeCode =8 then 'Mutual Fund' when nsdlstaticdata_TypeCode =10 then 'Bank' when nsdlstaticdata_TypeCode =21 then 'Intermediary' end as AccType,nsdlstaticdata_TypeCode as Code   from master_nsdlstaticdatacode  where  nsdlstaticdata_FieldName like 'Beneficiary Type / Sub type%' ) as B    ", "top 10  *   ", " B.AccType  Like '" + reqStr + "%'  order by  B.AccType  asc ");

                    DT = oDBEngine.GetDataTable("master_cdslclients ", "distinct  CdslClients_BOStatus,CdslClients_BOStatus ", " CdslClients_BOStatus is not null and  CdslClients_BOStatus  like  '" + reqStr + "%'  ");
                }
                else if (idlist[0] == "SubType")
                {
                    //  DT = oDBEngine.GetDataTable("master_nsdlstaticdatacode ", "top 10 nsdlstaticdata_Description ,nsdlstaticdata_SubTypeCode ", " nsdlstaticdata_TypeCode=" + idlist[1] + "  and  nsdlstaticdata_FieldName like 'Beneficiary Type / Sub type%' and  nsdlstaticdata_Description like '" + reqStr + "%'");
                    DT = oDBEngine.GetDataTable("master_cdslclients ", "distinct  CdslClients_BOSubStatus,CdslClients_BOSubStatus ", " CdslClients_BOSubStatus is not null and  CdslClients_BOSubStatus  like  '" + reqStr + "%'  and  CdslClients_BOStatus =" + idlist[1] + "");

                }
                else if (idlist[0] == "Client")
                {
                    string where = "";
                    //where = " (NsdlClients_BenFirstHolderName Like  '" + reqStr + "%' or NsdlClients_BenAccountID Like  '%" + reqStr + "%')";
                    //DT = oDBEngine.GetDataTable(" Master_NsdlClients ", "  top 10  NsdlClients_BenFirstHolderName + '   [ '+ convert(varchar,NsdlClients_BenAccountID) +' ]' as AccName, NsdlClients_BenAccountID  As ID", where);
                    where = " (CdslClients_FirstHolderName Like  '%" + reqStr + "%' or CdslClients_BOID Like  '%" + reqStr + "%'   or substring(CdslClients_BOID,1,8)='" + reqStr + "')   ";
                    DT = oDBEngine.GetDataTable(" Master_CdslClients ", "  top 10  CdslClients_FirstHolderName + '   [ '+ CdslClients_BOID +' ]' as AccName, CdslClients_BOID  As ID ", where);

                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion

            #region ShowEmployeeByFilter
            if (Request.QueryString["ShowEmployeeByFilter"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Employee")
                {
                    DT = oDBEngine.GetDataTable(@"tbl_Master_Contact) cnt
                on emp_contactId=cnt_internalID
                and (Name Like '%" + reqStr + "%' Or FirstName Like '%" + reqStr + "%' Or MiddleName Like '%" + reqStr + @"%' Or
                LastName Like '%" + reqStr + "%' Or ShortName Like '%" + reqStr + "%')", @"Top 10 cnt_internalId ID,Name,FirstName,
                MiddleName,LastName from (Select * from tbl_master_employee) emp 
                inner Join
                (Select Ltrim(Rtrim(isnull(cnt_firstName,'')))+' '+Ltrim(Rtrim(isnull(cnt_middleName,'')))+
                ' '+Ltrim(Rtrim(isnull(cnt_lastName,'')))+' ['+Ltrim(Rtrim(isnull(cnt_shortName,'')))+']' Name,
                cnt_firstName FirstName,cnt_middleName MiddleName,cnt_lastName LastName,cnt_shortName ShortName,cnt_internalId",
                   null);
                }
                else if (idlist[0] == "Company")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_company ", " top 10 cmp_internalid,cmp_Name ", " cmp_Name like'" + reqStr + "%'");
                }
                else if (idlist[0] == "Organization")//Intentially Wrote this For Getting Cmp_ID Rather Than Cmp_InternalID
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_company ", " top 10 cmp_id,cmp_Name ", " cmp_Name like'" + reqStr + "%'");
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_branch ", " top 10 branch_id,branch_description+'['+branch_code+']' ", " branch_description like '" + reqStr + "%' or branch_code like '" + reqStr + "%'", " branch_description ");
                }
                else if (idlist[0] == "ReportTo")
                {
                    DT = oDBEngine.GetDataTable("(   select  emp_id,(select  ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + '['+ltrim(rtrim(isnull(cnt_ucc,cnt_shortname)))+']'  from tbl_master_contact where cnt_internalid=emp_contactID  )   as  EmployeeName,EMP_UNIQUECODE   from tbl_master_employee where (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM') )AS C  ", " top 10  emp_id ,EmployeeName,EMP_UNIQUECODE    ", "   ( C.EmployeeName Like '" + reqStr + "%' OR  C.EMP_UNIQUECODE Like '" + reqStr + "%' )");
                }
                else if (idlist[0] == "Type")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_employeeType ", " top 10  EMPTPY_ID, EMPTPY_TYPE  ", "EMPTPY_TYPE Like '" + reqStr + "%' ");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString().ToUpper() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion
            #region NSDLClientNamesPOA

            if (Request.QueryString["NSDLClientNamesPOA"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string paramete = Request.QueryString["search_param"].ToString();
                string where;
                where = " (NsdlCorporatePOA_ShortName Like  '" + reqStr + "%' or NsdlCorporatePOA_POAID Like  '%" + reqStr + "%')  and NsdlCorporatePOA_DPID='" + paramete + "'";
                DT = oDBEngine.GetDataTable(" master_nsdlCorporatePOA ", "  top 10  NsdlCorporatePOA_POAID  As ID, NsdlCorporatePOA_ShortName + '   [ '+ convert(varchar,NsdlCorporatePOA_POAID) +' ]' as AccName", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");


            }
            #endregion
            #region CdslClientNamesPOA

            if (Request.QueryString["CDSLClientNamesPOA"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                string paramete = Request.QueryString["search_param"].ToString();
                string where;
                where = " CdslClients_AccountCategory='POA' and (CdslClients_BOID like  '" + reqStr + "%' or CdslClients_FirstHolderName like  '" + reqStr + "%')";
                DT = oDBEngine.GetDataTable(" Master_CdslClients ", "  top 10 CdslClients_BOID,CdslClients_FirstHolderName +'[ '+CdslClients_BOID+' ]'", where);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");


            }
            #endregion

            #region ShowClientForNotification
            if (Request.QueryString["ShowClientForNotification"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');

                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        if (Session["userlastsegment"].ToString() == "9")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlclients ", "top 10 LTRIM(RTRIM(NSDLCLIENTS_BENFIRSTHOLDERNAME))+'['+LTRIM(RTRIM(NSDLCLIENTS_BENACCOUNTID)) +']' ,NSDLCLIENTS_BENACCOUNTID", "  (NSDLCLIENTS_BENFIRSTHOLDERNAME like '" + reqStr + "%' or NSDLCLIENTS_BENACCOUNTID like '" + reqStr + "%') and NSDLCLIENTS_DPID+''+CAST(NSDLCLIENTS_BENACCOUNTID AS VARCHAR) in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else if (Session["userlastsegment"].ToString() == "10")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslclients ", "top 10 CDSLCLIENTS_FIRSTHOLDERNAME+'['+LTRIM(RTRIM( RIGHT(CDSLCLIENTS_BOID,8)))+']',CDSLCLIENTS_BOID ", "  (CDSLCLIENTS_FIRSTHOLDERNAME like '" + reqStr + "%' or RIGHT(CDSLCLIENTS_BOID,8) like '" + reqStr + "%') and CDSLCLIENTS_BOID in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else if (Session["userlastsegment"].ToString() == "4")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'EM%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                    }
                    else
                    {

                        if (Session["userlastsegment"].ToString() == "9")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlclients ", "top 10 LTRIM(RTRIM(NSDLCLIENTS_BENFIRSTHOLDERNAME))+'['+LTRIM(RTRIM(NSDLCLIENTS_BENACCOUNTID)) +']' ,NSDLCLIENTS_BENACCOUNTID", "  (NSDLCLIENTS_BENFIRSTHOLDERNAME like '" + reqStr + "%' or NSDLCLIENTS_BENACCOUNTID like '" + reqStr + "%') and NSDLCLIENTS_DPID+''+CAST(NSDLCLIENTS_BENACCOUNTID AS VARCHAR) in in (select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                        else if (Session["userlastsegment"].ToString() == "10")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslclients ", "top 10 CDSLCLIENTS_FIRSTHOLDERNAME+'['+LTRIM(RTRIM( RIGHT(CDSLCLIENTS_BOID,8)))+']',CDSLCLIENTS_BOID ", "  (CDSLCLIENTS_FIRSTHOLDERNAME like '" + reqStr + "%' or RIGHT(CDSLCLIENTS_BOID,8) like '" + reqStr + "%') and CDSLCLIENTS_BOID in (select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                        else if (Session["userlastsegment"].ToString() == "4")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'EM%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        if (Session["userlastsegment"].ToString() == "9")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlclients ", "top 10 LTRIM(RTRIM(NSDLCLIENTS_BENFIRSTHOLDERNAME))+'['+LTRIM(RTRIM(NSDLCLIENTS_BENACCOUNTID)) +']' ,NSDLCLIENTS_BENACCOUNTID", "  (NSDLCLIENTS_BENFIRSTHOLDERNAME like '" + reqStr + "%' or NSDLCLIENTS_BENACCOUNTID like '" + reqStr + "%') and NSDLCLIENTS_BRANCHID in (" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                        else if (Session["userlastsegment"].ToString() == "10")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslclients ", "top 10 CDSLCLIENTS_FIRSTHOLDERNAME +'['+LTRIM(RTRIM( RIGHT(CDSLCLIENTS_BOID,8)))+']',CDSLCLIENTS_BOID ", "  (CDSLCLIENTS_FIRSTHOLDERNAME like '" + reqStr + "%' or RIGHT(CDSLCLIENTS_BOID,8) like '" + reqStr + "%') and CDSLCLIENTS_BRANCHID in(" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                        else if (Session["userlastsegment"].ToString() == "4")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'EM%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                    }
                    else
                    {
                        if (Session["userlastsegment"].ToString() == "9")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlclients ", "top 10 LTRIM(RTRIM(NSDLCLIENTS_BENFIRSTHOLDERNAME))+'['+LTRIM(RTRIM(NSDLCLIENTS_BENACCOUNTID)) +']' ,NSDLCLIENTS_BENACCOUNTID", "  (NSDLCLIENTS_BENFIRSTHOLDERNAME like '" + reqStr + "%' or NSDLCLIENTS_BENACCOUNTID like '" + reqStr + "%') and NSDLCLIENTS_BRANCHID in (" + idlist[2] + ")");
                        }
                        else if (Session["userlastsegment"].ToString() == "10")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslclients ", "top 10 CDSLCLIENTS_FIRSTHOLDERNAME+'['+LTRIM(RTRIM( RIGHT(CDSLCLIENTS_BOID,8)))+']',CDSLCLIENTS_BOID ", "  (CDSLCLIENTS_FIRSTHOLDERNAME like '" + reqStr + "%' or RIGHT(CDSLCLIENTS_BOID,8) like '" + reqStr + "%') and CDSLCLIENTS_BRANCHID in(" + idlist[2] + ")");
                        }
                        else if (Session["userlastsegment"].ToString() == "4")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'EM%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                        }
                    }
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion


            #region ShowClientForDocument
            if (Request.QueryString["ShowClientForDocument"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');

                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        if (idlist[3] == "NSDL Clients")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlclients ", "top 10 LTRIM(RTRIM(NSDLCLIENTS_BENFIRSTHOLDERNAME))+'['+LTRIM(RTRIM(NSDLCLIENTS_BENACCOUNTID)) +']' ,NSDLCLIENTS_BENACCOUNTID", "  (NSDLCLIENTS_BENFIRSTHOLDERNAME like '" + reqStr + "%' or NSDLCLIENTS_BENACCOUNTID like '" + reqStr + "%') and NSDLCLIENTS_DPID+''+CAST(NSDLCLIENTS_BENACCOUNTID AS VARCHAR) in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else if (idlist[3] == "CDSL Clients")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslclients ", "top 10 CDSLCLIENTS_FIRSTHOLDERNAME+'['+LTRIM(RTRIM( RIGHT(CDSLCLIENTS_BOID,8)))+']',CDSLCLIENTS_BOID ", "  (CDSLCLIENTS_FIRSTHOLDERNAME like '" + reqStr + "%' or RIGHT(CDSLCLIENTS_BOID,8) like '" + reqStr + "%') and CDSLCLIENTS_BOID in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else if (idlist[3] == "Employee")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'EM%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                    }
                    else
                    {

                        if (idlist[3] == "NSDL Clients")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlclients ", "top 10 LTRIM(RTRIM(NSDLCLIENTS_BENFIRSTHOLDERNAME))+'['+LTRIM(RTRIM(NSDLCLIENTS_BENACCOUNTID)) +']' ,NSDLCLIENTS_BENACCOUNTID", "  (NSDLCLIENTS_BENFIRSTHOLDERNAME like '" + reqStr + "%' or NSDLCLIENTS_BENACCOUNTID like '" + reqStr + "%') and NSDLCLIENTS_DPID+''+CAST(NSDLCLIENTS_BENACCOUNTID AS VARCHAR) in in (select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                        else if (idlist[3] == "CDSL Clients")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslclients ", "top 10 CDSLCLIENTS_FIRSTHOLDERNAME+'['+LTRIM(RTRIM( RIGHT(CDSLCLIENTS_BOID,8)))+']',CDSLCLIENTS_BOID ", "  (CDSLCLIENTS_FIRSTHOLDERNAME like '" + reqStr + "%' or RIGHT(CDSLCLIENTS_BOID,8) like '" + reqStr + "%') and CDSLCLIENTS_BOID in (select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                        else if (idlist[3] == "Employee")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'EM%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        if (idlist[3] == "NSDL Clients")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlclients ", "top 10 LTRIM(RTRIM(NSDLCLIENTS_BENFIRSTHOLDERNAME))+'['+LTRIM(RTRIM(NSDLCLIENTS_BENACCOUNTID)) +']' ,NSDLCLIENTS_BENACCOUNTID", "  (NSDLCLIENTS_BENFIRSTHOLDERNAME like '" + reqStr + "%' or NSDLCLIENTS_BENACCOUNTID like '" + reqStr + "%') and NSDLCLIENTS_BRANCHID in (" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                        else if (idlist[3] == "CDSL Clients")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslclients ", "top 10 CDSLCLIENTS_FIRSTHOLDERNAME +'['+LTRIM(RTRIM( RIGHT(CDSLCLIENTS_BOID,8)))+']',CDSLCLIENTS_BOID ", "  (CDSLCLIENTS_FIRSTHOLDERNAME like '" + reqStr + "%' or RIGHT(CDSLCLIENTS_BOID,8) like '" + reqStr + "%') and CDSLCLIENTS_BRANCHID in(" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                        else if (idlist[3] == "Employee")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'EM%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                    }
                    else
                    {
                        if (idlist[3] == "NSDL Clients")
                        {
                            DT = oDBEngine.GetDataTable("master_nsdlclients ", "top 10 LTRIM(RTRIM(NSDLCLIENTS_BENFIRSTHOLDERNAME))+'['+LTRIM(RTRIM(NSDLCLIENTS_BENACCOUNTID)) +']' ,NSDLCLIENTS_BENACCOUNTID", "  (NSDLCLIENTS_BENFIRSTHOLDERNAME like '" + reqStr + "%' or NSDLCLIENTS_BENACCOUNTID like '" + reqStr + "%') and NSDLCLIENTS_BRANCHID in (" + idlist[2] + ")");
                        }
                        else if (idlist[3] == "CDSL Clients")
                        {
                            DT = oDBEngine.GetDataTable("master_cdslclients ", "top 10 CDSLCLIENTS_FIRSTHOLDERNAME+'['+LTRIM(RTRIM( RIGHT(CDSLCLIENTS_BOID,8)))+']',CDSLCLIENTS_BOID ", "  (CDSLCLIENTS_FIRSTHOLDERNAME like '" + reqStr + "%' or RIGHT(CDSLCLIENTS_BOID,8) like '" + reqStr + "%') and CDSLCLIENTS_BRANCHID in(" + idlist[2] + ")");
                        }
                        else if (idlist[3] == "Employee")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'EM%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_branchid in(" + idlist[2] + ")");
                        }
                    }
                }
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion

            #region bankdetailsSearch
            if (Request.QueryString["bankdetailsSearch"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT.Rows.Clear();
                string listitem = "";
                reqStr = Request.QueryString["letters"].ToString();
                switch (Request.QueryString["search_param"].ToString())
                {
                    case "bnk_bankName":
                        listitem = "isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'') + '~' + isnull(bnk_micrno,'') ";
                        DT = oDBEngine.GetDataTable(" tbl_master_bank", " top 10 " + listitem + " as bank, bnk_id", " bnk_bankName like '" + reqStr + "%'");
                        break;
                    case "bnk_Micrno":
                        listitem = "isnull(bnk_micrno,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_branchname,'')";
                        DT = oDBEngine.GetDataTable(" tbl_master_bank", " top 10 " + listitem + " as bank, bnk_id", " bnk_Micrno like '" + reqStr + "%'");
                        break;
                    case "bnk_branchName":
                        //listitem = "isnull(bnk_branchname,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_micrno,'') + '~2'";
                        listitem = "isnull(bnk_branchname,'') + '~' + isnull(bnk_bankname,'') + '~' + isnull(bnk_micrno,'')";
                        DT = oDBEngine.GetDataTable(" tbl_master_bank", " top 10 " + listitem + " as bank, bnk_id", " bnk_branchName like '" + reqStr + "%'");
                        break;

                };

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region ShowFilterForLedger
            if (Request.QueryString["ShowFilterForLedger"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                string[] seg = idlist[4].Split(',');
                string ExchSeg = "";
                string ExchSegid = "";
                if (idlist[0] == "Settlement")
                {
                    if (idlist[4].ToString() != "")
                    {

                        for (int k = 0; k < seg.Length; k++)
                        {

                            ExchSeg = "";
                            if (seg[k].ToString() == "'NSE - CM'")
                                ExchSeg = "1";
                            else if (seg[k].ToString() == "'NSE - FO'")
                                ExchSeg = "2";
                            else if (seg[k].ToString() == "'NSE - CDX")
                                ExchSeg = "3";
                            else if (seg[k].ToString() == "'BSE - CM'")
                                ExchSeg = "4";
                            else if (seg[k].ToString() == "'BSE - FO'")
                                ExchSeg = "5";
                            else if (seg[k].ToString() == "'BSE - CDX'")
                                ExchSeg = "6";
                            else if (seg[k].ToString() == "'MCX - COMM'")
                                ExchSeg = "7";
                            else if (seg[k].ToString() == "'MCXSX - CDX'")
                                ExchSeg = "8";
                            else if (seg[k].ToString() == "'NCDEX - COMM'")
                                ExchSeg = "9";
                            else if (seg[k].ToString() == "'DGCX - COMM'")
                                ExchSeg = "10";
                            else if (seg[k].ToString() == "'NMCE - COMM'")
                                ExchSeg = "11";
                            else if (seg[k].ToString() == "'ICEX - COMM'")
                                ExchSeg = "12";
                            else if (seg[k].ToString() == "'USE - CDX'")
                                ExchSeg = "13";
                            else if (seg[k].ToString() == "'NSEL - SPOT'")
                                ExchSeg = "14";
                            else if (seg[k].ToString() == "'CSE - CM'")
                                ExchSeg = "15";
                            else if (seg[k].ToString() == "'MCXSX - CM'")
                                ExchSeg = "19";
                            else if (seg[k].ToString() == "'MCXSX - FO")
                                ExchSeg = "20";
                            else if (seg[k].ToString() == "'BFX - COMM")
                                ExchSeg = "21";
                            else
                                ExchSeg = "1";

                            if (k == 0)
                                ExchSegid = ExchSeg;
                            else
                                ExchSegid = ExchSegid + "," + ExchSeg;

                        }
                    }
                }
                if (idlist[0] == "Settlement")
                {
                    DT = oDBEngine.GetDataTable("MASTER_SETTLEMENTS", " DISTINCT TOP 10 SettlementS_Number,SettlementS_Number ", "SETTLEMENTS_FINYEAR='" + Session["LastFinYear"].ToString() + "' AND SETTLEMENTS_EXCHANGESEGMENTID IN (" + ExchSegid + ") AND SettlementS_Number Like '" + reqStr + "%' ");
                }
                else if (idlist[0] == "Employee")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId ", " top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid ", " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='EM'  and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%' or tbl_master_email.eml_email like '" + reqStr + "%') ");
                }
                else if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Clients")
                {
                    if (Session["userlastsegment"].ToString() == "5")
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'+' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                    else
                        DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", "cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%'  and CRG_EXCHANGE in (" + idlist[4] + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "Scrips")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID=1");
                }
                else if (idlist[0] == "ScripsExchange")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID='" + Session["ExchangeSegmentID"].ToString() + "'");
                }
                else if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                }
                else if (idlist[0] == "SegmentCM")
                {
                    DT = oDBEngine.GetDataTable("(select isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,exch_internalId from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid like 'CM%') as D", "*", null);
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "TerminalId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_TerminalID,ExchangeTrades_TerminalID", "ExchangeTrades_TerminalID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "CTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "TERMINALSELECTEDCTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_TerminalID in(" + idlist[1] + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[3] == "'SYSTM00042'")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("(Select  cdslclients_benaccountnumber,ltrim(rtrim(cdslclients_firstholdername))+' ['+cast(cdslclients_benaccountnumber as varchar)+']' +' ['+rtrim(branch_description)+']' as cdslclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=CdslClients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='CDSL')+ cdslclients_benaccountnumber as Number	from master_cdslclients,tbl_master_branch where CdslClients_BranchID=branch_id and cdslclients_BranchID in(" + Session["userbranchHierarchy"] + ")) as DD", "top 10 cdslclients_firstholdername,cdslclients_benaccountnumber", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "')) and (cdslclients_benaccountnumber like '" + reqStr + "%' or UCC like '" + reqStr + "%' or cdslclients_benaccountnumber like '" + reqStr + "%')");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("(Select cdslclients_benaccountnumber,ltrim(rtrim(cdslclients_firstholdername))+' ['+cast(cdslclients_benaccountnumber as varchar)+']' +' ['+rtrim(branch_description)+']' as cdslclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=CdslClients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='CDSL')+ cdslclients_benaccountnumber as Number	from master_cdslclients,tbl_master_branch where CdslClients_BranchID=branch_id and cdslclients_BranchID in(" + Session["userbranchHierarchy"] + ")) as DD", "top 10 cdslclients_firstholdername,cdslclients_benaccountnumber", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(" + idlist[2] + ")) and (cdslclients_benaccountnumber like '" + reqStr + "%' or UCC like '" + reqStr + "%'  or cdslclients_benaccountnumber like '" + reqStr + "%')");
                        }
                    }
                    else if (idlist[3] == "'SYSTM00043'")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("(Select  nsdlclients_benaccountid,ltrim(rtrim(nsdlclients_benfirstholdername))+' ['+cast(nsdlclients_benaccountid as varchar)+']' +' ['+rtrim(branch_description)+']' as nsdlclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=nsdlclients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='NSDL')+ cast(nsdlclients_benaccountid as varchar) as Number	from master_nsdlclients,tbl_master_branch where NsdlClients_BranchID=branch_id and Nsdlclients_BranchID in(" + Session["userbranchHierarchy"] + ")) as DD", "top 10 nsdlclients_firstholdername,nsdlclients_benaccountid", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "')) and (nsdlclients_benaccountid like '" + reqStr + "%' or UCC like '" + reqStr + "%' or nsdlclients_firstholdername like '" + reqStr + "%')");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("(Select  nsdlclients_benaccountid,ltrim(rtrim(nsdlclients_benfirstholdername))+' ['+cast(nsdlclients_benaccountid as varchar)+']' +' ['+rtrim(branch_description)+']' as nsdlclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=nsdlclients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='NSDL')+ cast(nsdlclients_benaccountid as varchar) as Number	from master_nsdlclients,tbl_master_branch where NsdlClients_BranchID=branch_id and Nsdlclients_BranchID in(" + Session["userbranchHierarchy"] + ")) as DD", "top 10 nsdlclients_firstholdername,nsdlclients_benaccountid", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(" + idlist[2] + ")) and (nsdlclients_benaccountid like '" + reqStr + "%' or UCC like '" + reqStr + "%' or nsdlclients_firstholdername like '" + reqStr + "%')");
                        }
                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            if (Session["userlastsegment"].ToString() == "5")
                                DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' +' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_BranchID=branch_id and cnt_BranchID in(" + Session["userbranchHierarchy"] + ") and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                            else
                                DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", "cnt_branchid=branch_id and cnt_BranchID in(" + Session["userbranchHierarchy"] + ") and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%'  and CRG_EXCHANGE in (" + idlist[4] + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else
                        {
                            if (Session["userlastsegment"].ToString() == "5")
                                DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' +' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_BranchID=branch_id and cnt_BranchID in(" + Session["userbranchHierarchy"] + ") and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                            else
                                DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", "cnt_branchid=branch_id and cnt_BranchID in(" + Session["userbranchHierarchy"] + ") and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%'  and CRG_EXCHANGE in (" + idlist[4] + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        if (idlist[3] == "")
                        {
                            if (Session["userlastsegment"].ToString() == "5")
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code)else SubAccount_Code end as InterNalID from Master_SubAccount) as DD ", " top 10 SubAccount_Name+' ['+InterNalID+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                            else
                                DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", "cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%'  and CRG_EXCHANGE in (" + idlist[4] + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                        }
                        else if (idlist[3] != "")
                        {
                            if (idlist[3] == "'SYSTM00043'")
                            {
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,(select cnt_ucc from tbl_master_contact where cnt_internalID in (select NSDLClients_ContactID from Master_NSDLClients where NSDLClients_BenAccountID=SubAccount_Code)) as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(SubAccount_Code),'')+']'+'['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select NsdlClients_BranchID from Master_NsdlClients where NsdlClients_BenAccountID=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code ", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%'");
                            }
                            else if (idlist[3] == "'SYSTM00042'")
                            {
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,(select cnt_ucc from tbl_master_contact where cnt_internalID in (select CDSLClients_ContactID from Master_CDSLClients where CDSLClients_BenAccountNumber=SubAccount_Code)) as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(SubAccount_Code),'')+']'+'['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select CdslClients_BranchID from Master_CdslClients where CdslClients_BenAccountNumber=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code ", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%'");
                            }
                            else
                            {
                                // DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code)else SubAccount_Code end as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                                //DT = oDBEngine.GetDataTable("TBL_MASTER_CONTACT, Master_SubAccount ", "DISTINCT TOP 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(CNT_UCC),'')+']'+' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=CNT_BRANCHID)+']',SubAccount_Code ", " SubAccount_MainAcReferenceID in (" + idlist[3] + ") AND SubAccount_Code like 'CL%' AND TBL_MASTER_CONTACT.CNT_INTERNALID=Master_SubAccount.SubAccount_Code  AND CNT_BRANCHID IN (" + Session["userbranchHierarchy"].ToString() + ")  and (SubAccount_Name like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%' )");
                                DataTable dtSubledger = oDBEngine.GetDataTable("master_mainaccount ", "Top 1 MainAccount_SubLedgerType ", " MainAccount_AccountCode in (" + idlist[3].ToString() + ")");
                                if (dtSubledger.Rows.Count > 0)
                                {
                                    if (dtSubledger.Rows[0][0].ToString() == "Sub Brokers ")
                                    {
                                        DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch  ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')  +' ['+isnull(rtrim(cnt_shortname),cnt_ucc)+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID     ", "cnt_branchid=branch_id   and cnt_internalid like 'SB%'  AND (cnt_ucc LIKE '" + reqStr + "%' or  cnt_shortname LIKE '" + reqStr + "%'   OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                                    }
                                    else if (dtSubledger.Rows[0][0].ToString() == "Brokers")
                                    {
                                        DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch  ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')  +' ['+isnull(rtrim(cnt_shortname),cnt_ucc)+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID     ", "cnt_branchid=branch_id   and cnt_internalid like 'BO%'  AND (cnt_ucc LIKE '" + reqStr + "%' or  cnt_shortname LIKE '" + reqStr + "%'   OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                                    }
                                    else if (dtSubledger.Rows[0][0].ToString() == "Employees")
                                    {
                                        DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch  ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')  +' ['+isnull(rtrim(cnt_shortname),cnt_ucc)+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID     ", "cnt_branchid=branch_id   and cnt_internalid like 'EM%'  AND (cnt_ucc LIKE '" + reqStr + "%' or  cnt_shortname LIKE '" + reqStr + "%'   OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                                    }

                                    else if (dtSubledger.Rows[0][0].ToString() == "Business Partners")
                                    {
                                        DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch  ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')  +' ['+isnull(rtrim(cnt_shortname),cnt_ucc)+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID     ", "cnt_branchid=branch_id   and cnt_internalid like 'BP%'  AND (cnt_ucc LIKE '" + reqStr + "%' or  cnt_shortname LIKE '" + reqStr + "%'   OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                                    }

                                    else if (dtSubledger.Rows[0][0].ToString() == "Relationship Partners")
                                    {
                                        DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch  ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')  +' ['+isnull(rtrim(cnt_shortname),cnt_ucc)+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID     ", "cnt_branchid=branch_id   and cnt_internalid like 'RA%'  AND (cnt_ucc LIKE '" + reqStr + "%' or  cnt_shortname LIKE '" + reqStr + "%'   OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                                    }
                                    else
                                    {
                                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID  ", "cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in (" + idlist[4] + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (idlist[3] == "")
                        {
                            if (Session["userlastsegment"].ToString() == "5")
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code) else SubAccount_Code end as InterNalID,AccountsLedger_BranchID from Master_SubAccount,Trans_AccountsLedger where AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and AccountsLedger_BranchID in(" + idlist[2] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+rtrim(InterNalID)+']'+' ['+(select rtrim(branch_description) from tbl_master_branch where AccountsLedger_BranchID=branch_id)+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                            else
                                DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", "cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%'  and CRG_EXCHANGE in (" + idlist[4] + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in(" + idlist[2] + ")");
                        }
                        else if (idlist[3] != "")
                        {
                            if (Session["userlastsegment"].ToString() == "5")
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code) else SubAccount_Code end as InterNalID,AccountsLedger_BranchID from Master_SubAccount,Trans_AccountsLedger where AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and AccountsLedger_BranchID in(" + idlist[2] + ") and SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+rtrim(InterNalID)+']'+' ['+(select rtrim(branch_description) from tbl_master_branch where AccountsLedger_BranchID=branch_id)+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                            else
                                DT = oDBEngine.GetDataTable(" tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange ", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", "cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%'  and CRG_EXCHANGE in (" + idlist[4] + ") AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in(" + idlist[2] + ")");
                        }
                    }
                }
                else if (idlist[0] == "SettlementType")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                }
                else if (idlist[0] == "MainAcc")
                {
                    DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) and MainAccount_SubLedgerType<>'None'");
                }
                else if (idlist[0] == "Sub Ac")
                {
                    DT = oDBEngine.GetDataTable("master_subaccount ", "distinct top 10 SubAccount_Name+' ['+SubAccount_Code+']' as SubAccount_Name,SubAccount_Code ", "(SubAccount_Name like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%') and SubAccount_MainAcReferenceID=" + idlist[2] + "");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion

            #region SearchContactByType
            if (Request.QueryString["SearchContactByType"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString().Trim();
                string param = Request.QueryString["search_param"].ToString();
                string[] SplTx = param.ToString().Split('~');

                DT = oDBEngine.GetDataTable("tbl_master_contact", "TOP 10 cnt_internalid , cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+ '['+case cnt_contactType when 'CL' then ltrim(rtrim(isnull(cnt_UCC,''))) else ltrim(rtrim(isnull(cnt_shortName,''))) end+']'  as [Name] ", "  cnt_contactType='" + SplTx[1].ToString().Trim() + "'  and (cnt_firstName Like '" + reqStr + "%' or cnt_UCC like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][1].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region MonthlyTrialAjaxList
            if (Request.QueryString["MonthlyTrialAjaxList"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Clients")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'+' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                }
                else if (idlist[0] == "Scrips")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID=1");
                }
                else if (idlist[0] == "ScripsExchange")
                {
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID='" + Session["ExchangeSegmentID"].ToString() + "'");
                }
                else if (idlist[0] == "Segment")
                {
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                }
                else if (idlist[0] == "SegmentCM")
                {
                    DT = oDBEngine.GetDataTable("(select isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,exch_internalId from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid like 'CM%') as D", "*", null);
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "TerminalId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_TerminalID,ExchangeTrades_TerminalID", "ExchangeTrades_TerminalID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "CTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "'");
                }
                else if (idlist[0] == "TERMINALSELECTEDCTCLId")
                {
                    DT = oDBEngine.GetDataTable("Trans_ExchangeTrades ", "distinct top 10  ExchangeTrades_CTCLID,ExchangeTrades_CTCLID", "ExchangeTrades_CTCLID Like '" + reqStr + "%' and ExchangeTrades_SettlementType='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and ExchangeTrades_SettlementNumber='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ExchangeTrades_BranchID in(" + Session["userbranchHierarchy"] + ")  and ExchangeTrades_Segment='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and ExchangeTrades_CompanyID='" + Session["LastCompany"].ToString() + "' and ExchangeTrades_TerminalID in(" + idlist[1] + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[3] == "'SYSTM00042'")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("(Select  cdslclients_benaccountnumber,ltrim(rtrim(cdslclients_firstholdername))+' ['+cast(cdslclients_benaccountnumber as varchar)+']' +' ['+rtrim(branch_description)+']' as cdslclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=CdslClients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='CDSL')+ cdslclients_benaccountnumber as Number	from master_cdslclients,tbl_master_branch where CdslClients_BranchID=branch_id ) as DD", "top 10 cdslclients_firstholdername,cdslclients_benaccountnumber", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "')) and (cdslclients_benaccountnumber like '" + reqStr + "%' or UCC like '" + reqStr + "%' or cdslclients_benaccountnumber like '" + reqStr + "%')");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("(Select cdslclients_benaccountnumber,ltrim(rtrim(cdslclients_firstholdername))+' ['+cast(cdslclients_benaccountnumber as varchar)+']' +' ['+rtrim(branch_description)+']' as cdslclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=CdslClients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='CDSL')+ cdslclients_benaccountnumber as Number	from master_cdslclients,tbl_master_branch where CdslClients_BranchID=branch_id ) as DD", "top 10 cdslclients_firstholdername,cdslclients_benaccountnumber", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(" + idlist[2] + ")) and (cdslclients_benaccountnumber like '" + reqStr + "%' or UCC like '" + reqStr + "%'  or cdslclients_benaccountnumber like '" + reqStr + "%')");
                        }
                    }
                    else if (idlist[3] == "'SYSTM00043'")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("(Select  nsdlclients_benaccountid,ltrim(rtrim(nsdlclients_benfirstholdername))+' ['+cast(nsdlclients_benaccountid as varchar)+']' +' ['+rtrim(branch_description)+']' as nsdlclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=nsdlclients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='NSDL')+ cast(nsdlclients_benaccountid as varchar) as Number	from master_nsdlclients,tbl_master_branch where NsdlClients_BranchID=branch_id) as DD", "top 10 nsdlclients_firstholdername,nsdlclients_benaccountid", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "')) and (nsdlclients_benaccountid like '" + reqStr + "%' or UCC like '" + reqStr + "%' or nsdlclients_firstholdername like '" + reqStr + "%')");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("(Select  nsdlclients_benaccountid,ltrim(rtrim(nsdlclients_benfirstholdername))+' ['+cast(nsdlclients_benaccountid as varchar)+']' +' ['+rtrim(branch_description)+']' as nsdlclients_firstholdername,	(select cnt_ucc from tbl_master_contact where cnt_internalID=nsdlclients_ContactID) as UCC,	(select exch_TMCode from tbl_master_companyExchange where exch_membershiptype='NSDL')+ cast(nsdlclients_benaccountid as varchar) as Number	from master_nsdlclients,tbl_master_branch where NsdlClients_BranchID=branch_id) as DD", "top 10 nsdlclients_firstholdername,nsdlclients_benaccountid", " Number in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(" + idlist[2] + ")) and (nsdlclients_benaccountid like '" + reqStr + "%' or UCC like '" + reqStr + "%' or nsdlclients_firstholdername like '" + reqStr + "%')");
                        }
                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' +' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_BranchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' +' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_BranchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        if (idlist[3] == "")
                        {
                            DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code)else SubAccount_Code end as InterNalID from Master_SubAccount) as DD ", " top 10 SubAccount_Name+' ['+InterNalID+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        }
                        else if (idlist[3] != "")
                        {
                            if (idlist[3] == "'SYSTM00043'")
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,(select cnt_ucc from tbl_master_contact where cnt_internalID=(select NSDLClients_ContactID from Master_NSDLClients where NSDLClients_BenAccountID=SubAccount_Code)) as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(SubAccount_Code),'')+']'+'['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select NsdlClients_BranchID from Master_NsdlClients where NsdlClients_BenAccountID=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code ", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%'");
                            else if (idlist[3] == "'SYSTM00042'")
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,(select cnt_ucc from tbl_master_contact where cnt_internalID=(select CDSLClients_ContactID from Master_CDSLClients where CDSLClients_BenAccountNumber=SubAccount_Code)) as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(SubAccount_Code),'')+']'+'['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select CdslClients_BranchID from Master_CdslClients where CdslClients_BenAccountNumber=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code ", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%'");
                            else
                                DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code)else SubAccount_Code end as InterNalID from Master_SubAccount where SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+isnull(rtrim(InterNalID),'')+']' +' ['+(select rtrim(branch_description) from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=SubAccount_Code))+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        }
                    }
                    else
                    {
                        if (idlist[3] == "")
                        {
                            DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code) else SubAccount_Code end as InterNalID,AccountsLedger_BranchID from Master_SubAccount,Trans_AccountsLedger where AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and AccountsLedger_BranchID in(" + idlist[2] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+rtrim(InterNalID)+']'+' ['+(select rtrim(branch_description) from tbl_master_branch where AccountsLedger_BranchID=branch_id)+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        }
                        else if (idlist[3] != "")
                        {
                            DT = oDBEngine.GetDataTable("(select distinct SubAccount_Code,SubAccount_Name,case when SubAccount_Code like 'CL%' then (select cnt_ucc from tbl_master_contact where cnt_internalID=SubAccount_Code) else SubAccount_Code end as InterNalID,AccountsLedger_BranchID from Master_SubAccount,Trans_AccountsLedger where AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and AccountsLedger_BranchID in(" + idlist[2] + ") and SubAccount_MainAcReferenceID in(" + idlist[3] + ")) as DD  ", " top 10 rtrim(SubAccount_Name)+' ['+rtrim(InterNalID)+']'+' ['+(select rtrim(branch_description) from tbl_master_branch where AccountsLedger_BranchID=branch_id)+']' as SubAccount_Name,SubAccount_Code", " SubAccount_Name like '" + reqStr + "%' or InterNalID like '" + reqStr + "%'");
                        }
                    }
                }
                else if (idlist[0] == "SettlementType")
                {
                    DT = oDBEngine.GetDataTable("Master_Settlements", " distinct top 10 isnull(ltrim(rtrim(Settlements_Type)),'')+'['+isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')+']',isnull(ltrim(rtrim(Settlements_TypeSuffix)),'')", "Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Settlements_Type like  '" + reqStr + "%' or Settlements_TypeSuffix like  '" + reqStr + "%')");
                }
                else if (idlist[0] == "MainAcc")
                {
                    DataTable dtBr = new DataTable();
                    dtBr = oDBEngine.GetDataTable("tbl_master_branch", "branch_parentid", " branch_id IN (select cnt_branchid from tbl_master_contact where cnt_internalid='" + Session["usercontactID"].ToString() + "')");
                    if (dtBr.Rows.Count > 0)
                    {

                        if (dtBr.Rows[0][0].ToString() == "0")
                        {

                            DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger)  ");
                        }
                        else
                        {

                            DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) ");
                        }
                    }
                    else
                    {

                        DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) ");
                    }

                    // DT = oDBEngine.GetDataTable("master_mainAccount ", "top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) and MainAccount_SubLedgerType<>'None'");
                }
                else if (idlist[0] == "Sub Ac")
                {
                    DT = oDBEngine.GetDataTable("master_subaccount ", "distinct top 10 SubAccount_Name+' ['+SubAccount_Code+']' as SubAccount_Name,SubAccount_Code ", "(SubAccount_Name like '" + reqStr + "%' or SubAccount_Code like '" + reqStr + "%') and SubAccount_MainAcReferenceID=" + idlist[2] + "");
                }
                else if (idlist[0] == "Employee")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId ", " top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid ", " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='EM'  and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%' or tbl_master_email.eml_email like '" + reqStr + "%') ");
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion

            ////Journal Voucher New Module Region
            #region SubAccount_New
            if (Request.QueryString["SubAccountMod_New"] == "1")
            {
                DataSet DS = new DataSet();
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                string[] param1 = param.Split('~');
                if (Session["ExchangeSegmentID"] == null)
                    Session["ExchangeSegmentID"] = "0";
                string Branch = null;
                if (param1[1].ToString() == "N")
                {
                    Branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                }
                else
                    Branch = param1[1].ToString();


                //SqlCommand cmd = new SqlCommand("SubAccountSelect", con);
                SqlCommand cmd = new SqlCommand("SubAccountSelect_New", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CashBank_MainAccountID", param1[0].ToString());
                cmd.Parameters.AddWithValue("@clause", reqStr);
                cmd.Parameters.AddWithValue("@branch", Branch);
                cmd.Parameters.AddWithValue("@exchSegment", Session["ExchangeSegmentID"].ToString());
                cmd.Parameters.AddWithValue("@SegmentN", "'" + SegmentName.ToString() + "'");
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(DS);
                cmd.Dispose();
                GC.Collect();

                DT = DS.Tables[0];
                //string[,] Data1 = { { "@CashBank_MainAccountID", SqlDbType.VarChar.ToString(), param1[0].ToString() }, { "@clause", SqlDbType.VarChar.ToString(), reqStr }, { "@branch", SqlDbType.VarChar.ToString(), Branch }, { "@exchSegment", SqlDbType.VarChar.ToString(), Session["ExchangeSegmentID"].ToString() } };
                //DT = oDBEngine.GetDatatable_StoredProcedure("SubAccountSelect", Data1);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                {
                    if (DS.Tables[1].Rows.Count > 0)
                        if (DS.Tables[1].Rows[0][0] == "1")
                            Response.Write("Suspended Client###Suspended Client|");
                        else
                            Response.Write("No Record Found###No Record Found|");
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion


            #region SubAccount Edit
            if (Request.QueryString["SubAccountModE"] == "1")
            {
                DataSet DS = new DataSet();
                string reqStr = Request.QueryString["letters"].ToString();
                string param = Request.QueryString["search_param"].ToString();
                string[] param1 = param.Split('~');
                if (Session["ExchangeSegmentID"] == null)
                    Session["ExchangeSegmentID"] = "0";
                string Branch = null;
                if (param1[1].ToString() == "N")
                {
                    Branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                }
                else
                    Branch = param1[1].ToString();


                //SqlCommand cmd = new SqlCommand("SubAccountSelect", con);
                SqlCommand cmd = new SqlCommand("SubAccountSelect_New", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CashBank_MainAccountID", param1[0].ToString());
                cmd.Parameters.AddWithValue("@clause", reqStr);
                cmd.Parameters.AddWithValue("@branch", Branch);
                cmd.Parameters.AddWithValue("@exchSegment", Session["ExchangeSegmentID"].ToString());
                cmd.Parameters.AddWithValue("@SegmentN", "'" + SegmentName.ToString() + "'");
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(DS);
                cmd.Dispose();
                GC.Collect();

                DT = DS.Tables[0];
                //string[,] Data1 = { { "@CashBank_MainAccountID", SqlDbType.VarChar.ToString(), param1[0].ToString() }, { "@clause", SqlDbType.VarChar.ToString(), reqStr }, { "@branch", SqlDbType.VarChar.ToString(), Branch }, { "@exchSegment", SqlDbType.VarChar.ToString(), Session["ExchangeSegmentID"].ToString() } };
                //DT = oDBEngine.GetDatatable_StoredProcedure("SubAccountSelect", Data1);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "~Edit" + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                {
                    if (DS.Tables[1].Rows.Count > 0)
                        if (DS.Tables[1].Rows[0][0] == "1")
                            Response.Write("Suspended Client###Suspended Client|");
                        else
                            Response.Write("No Record Found###No Record Found|");
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
            }
            #endregion



            #region MainAccount For Journal New
            if (Request.QueryString["MainAccountJournal_New"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_MainAccount", "top 10 MainAccount_Name as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+'~'+MainAccount_SubLedgerType+'~MAINAC' as MainAccount_ReferenceID", " MainAccount_Name like '" + reqStr + "%' and isnull(MainAccount_BankCashType,'') not in('Cash','Bank')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region MainAccount For Journal Edit
            if (Request.QueryString["MainAccountJournalE"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("Master_MainAccount", "top 10 MainAccount_Name as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+'~'+MainAccount_SubLedgerType+'~MAINAC' as MainAccount_ReferenceID", " MainAccount_Name like '" + reqStr + "%' and MainAccount_BankCashType not in('Cash','Bank')");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "~Edit" + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            ////EndJournal Voucher New Module Region
            #region GetUserAccessGroup
            if (Request.QueryString["GetUserAccessGroup"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                if (param == "0")
                {
                    DT = oDBEngine.GetDataTable(" TBL_MASTER_USER ,TBL_MASTER_CONTACT ", "top 10   USER_NAME  + '  [' + LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))  + ' ' + LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,'')))   + ' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,'')))  + '] [' + LTRIM(RTRIM(ISNULL(CNT_SHORTNAME,''))) +']' ,user_id   ", " TBL_MASTER_CONTACT.CNT_INTERNALID=TBL_MASTER_USER.USER_CONTACTID  AND (CNT_FIRSTNAME LIKE '" + reqStr + "%'  OR USER_NAME LIKE  '" + reqStr + "%'  OR CNT_SHORTNAME LIKE  '" + reqStr + "%' ) ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable("TBL_MASTER_USERGROUP  ,TBL_MASTER_SEGMENT", " top 10 ISNULL(GRP_NAME,'')+' [ '+ ISNULL(SEG_NAME,'') + ' ]' , grp_id ", " TBL_MASTER_USERGROUP.GRP_SEGMENTID=TBL_MASTER_SEGMENT.SEG_ID AND (GRP_NAME LIKE '" + reqStr + "%' OR SEG_NAME LIKE '" + reqStr + "%')");
                }

                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region SearchAllMainAccount
            if (Request.QueryString["SearchAllMainAccount"] == "1")
            {

                string reqStr = Request.QueryString["letters"].ToString();
                DT = oDBEngine.GetDataTable("master_mainAccount ", " top 10 MainAccount_Name  + ' ['+MainAccount_AccountCode+']' as MainAccount_Name,MainAccount_AccountCode", "(MainAccount_Name Like '" + reqStr + "%' or MainAccount_AccountCode like '" + reqStr + "%') and MainAccount_AccountCode in(select distinct accountsledger_mainaccountid from trans_accountsledger) ");
                if (DT.Rows.Count > 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }

            #endregion
            #region ShowDeliveryPosition
            if (Request.QueryString["ShowDeliveryPosition"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Clients")
                    DT = oDBEngine.GetDataTable("tbl_master_contact", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' ,cnt_internalID", " cnt_internalId like 'CL%' and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                else if (idlist[0] == "Scrips")
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID in(1,4)");
                else if (idlist[0] == "ScripsAllSeg")
                    DT = oDBEngine.GetDataTable("master_equity", "top 10 isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']',Equity_SeriesID", " Equity_TickerSymbol like '" + reqStr + "%' and Equity_ExchSegmentID in(1,4)");
                else if (idlist[0] == "SettType")
                    DT = oDBEngine.GetDataTable("master_settlements", " distinct top 10 rtrim(Settlements_Number)+ltrim(Settlements_TypeSuffix),Settlements_Number", " Settlements_Number like '" + reqStr + "%' and Settlements_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + "");
                else if (idlist[0] == "Segment")
                    DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='COR0000001' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "EXCHANGENAME Like '" + reqStr + "%'");
                else if (idlist[0] == "Branch")
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "branch_description Like '" + reqStr + "%' and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                else if (idlist[0] == "Group")
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                else if (idlist[0] == "ClientsBranchGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers)");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in(select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + idlist[2] + "))");
                    }
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + idlist[2] + "'))");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[1] == "ALL")
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange", "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID ", " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND CRG_COMPANY ='" + Session["LastCompany"].ToString() + "'  and CRG_EXCHANGE in ('" + SegmentName + "') AND (crg_tcode LIKE '" + reqStr + "%'  OR CNT_FIRSTNAME  LIKE '" + reqStr + "%') and branch_id in (" + idlist[2] + ")");
                    }
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");
            }
            #endregion
            #region ShowClientSendEmail
            if (Request.QueryString["ShowClientSendEmail"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Clients")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'+' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[3] == "CD")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS,TBL_MASTER_EMAIL ", " TOP 10 LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ['+ltrim(rtrim(isnull(eml_email,'')))+']',CDSLCLIENTS_BOID ", " MASTER_CDSLCLIENTS.CDSLCLIENTS_BOID=tbl_master_email.eml_cntid and eml_email !='' and  eml_type='Official' and  (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') ");

                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS,TBL_MASTER_EMAIL ", " TOP 10 LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ['+ltrim(rtrim(isnull(eml_email,'')))+']',CDSLCLIENTS_BOID ", " MASTER_CDSLCLIENTS.CDSLCLIENTS_BOID=tbl_master_email.eml_cntid and eml_email !='' and  eml_type='Official' and  (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') and CDSLCLIENTS_BOID in (select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + idlist[2] + ")) ");
                        }
                    }
                    else if (idlist[3] == "ND")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS,TBL_MASTER_EMAIL ", " TOP 10 LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ", " NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)=eml_cntid and eml_email !='' and  eml_type='Official' and  ( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS,TBL_MASTER_EMAIL ", " TOP 10 LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ", " NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)=eml_cntid and eml_email !='' and  eml_type='Official' and  ( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%') and NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) in (select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))  ");
                        }
                    }
                    else if (idlist[3] == "EM")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CNT_INTERNALID  ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'EM%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']['+ltrim(rtrim(isnull(eml_email,'')))+']'  ,CNT_INTERNALID  ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'EM%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%') and cnt_internalid in(select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + idlist[2] + ")) ");
                        }
                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CNT_INTERNALID   ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'CL%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CNT_INTERNALID   ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'CL%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%') and cnt_internalid in(select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[3] == "CD")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS,TBL_MASTER_EMAIL ", " TOP 10 LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CDSLCLIENTS_BOID", " MASTER_CDSLCLIENTS.CDSLCLIENTS_BOID=tbl_master_email.eml_cntid and eml_email !='' and  eml_type='Official' and  (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') ");

                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS,TBL_MASTER_EMAIL ", " TOP 10 LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + '] ['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CDSLCLIENTS_BOID ", " MASTER_CDSLCLIENTS.CDSLCLIENTS_BOID=tbl_master_email.eml_cntid and eml_email !='' and  eml_type='Official' and  (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') and  cdslclients_branchid in(" + idlist[2] + ") ");
                        }
                    }
                    else if (idlist[3] == "ND")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS,TBL_MASTER_EMAIL ", " TOP 10 LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ", " NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)=eml_cntid and eml_email !='' and  eml_type='Official' and  ( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS,TBL_MASTER_EMAIL ", " TOP 10 LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ", " NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR)=eml_cntid and eml_email !='' and  eml_type='Official' and  ( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%') and nsdlclients_branchid in (" + idlist[2] + ")  ");
                        }
                    }
                    else if (idlist[3] == "EM")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CNT_INTERNALID  ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'EM%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CNT_INTERNALID  ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'EM%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')  and cnt_branchid in (" + idlist[2] + ") ");
                        }
                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CNT_INTERNALID   ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'CL%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')   ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT,tbl_master_email", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']['+ltrim(rtrim(isnull(eml_email,'')))+']' ,CNT_INTERNALID  ", " TBL_MASTER_CONTACT.CNT_INTERNALID=tbl_master_email.eml_cntid and eml_email !='' and eml_type='Official' and cnt_internalid like 'CL%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')   and cnt_branchid in (" + idlist[2] + ") ");
                        }
                    }
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion


            #region ShowClientTemplatePDF
            if (Request.QueryString["ShowClientTemplatePDF"] == "1")
            {
                string param = Request.QueryString["search_param"].ToString();
                string reqStr = Request.QueryString["letters"].ToString();
                string[] idlist = param.Split('~');
                if (idlist[0] == "Group")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_groupmaster ", "top 10 gpm_description+'-'+gpm_code ,gpm_id", "(gpm_description Like '" + reqStr + "%' or gpm_code Like '" + reqStr + "%' ) and gpm_type='" + idlist[1] + "'");
                }
                else if (idlist[0] == "Clients")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch", "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']'+' ['+rtrim(branch_description)+']' ,cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchID=branch_id and (cnt_firstName like '" + reqStr + "%' or cnt_ucc like '" + reqStr + "%')");
                }
                else if (idlist[0] == "Branch")
                {
                    DT = oDBEngine.GetDataTable("tbl_master_branch ", "top 10 branch_description+'-'+branch_code,branch_id", "(branch_description Like '" + reqStr + "%' or branch_code Like '" + reqStr + "%')and branch_id in(" + Session["userbranchHierarchy"].ToString() + ")");
                }
                else if (idlist[0] == "ClientsGroup")
                {
                    if (idlist[3] == "CD")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS ", " TOP 10 LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + ']',CDSLCLIENTS_BOID ", "   (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') ");

                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS ", " TOP 10 LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + ']',CDSLCLIENTS_BOID ", "   (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') and CDSLCLIENTS_BOID in (select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + idlist[2] + ")) ");
                        }
                    }
                    else if (idlist[3] == "ND")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS    ", " TOP 10 LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ", " ( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS ", " TOP 10 LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ", " ( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%') and NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) in (select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))  ");
                        }
                    }
                    else if (idlist[3] == "EM")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' ,CNT_INTERNALID  ", "  cnt_internalid like 'EM%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT  ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']'  ,CNT_INTERNALID  ", " cnt_internalid like 'EM%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%') and cnt_internalid in(select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + idlist[2] + ")) ");
                        }
                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' ,CNT_INTERNALID   ", " cnt_internalid like 'CL%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' ,CNT_INTERNALID   ", " cnt_internalid like 'CL%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%') and cnt_internalid in(select grp_contactid from	tbl_trans_group where grp_groupmaster in(" + idlist[2] + "))");
                        }
                    }
                }
                else if (idlist[0] == "ClientsBranch")
                {
                    if (idlist[3] == "CD")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS    ", " TOP 10 LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + ']' ,CDSLCLIENTS_BOID", "   (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') ");

                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS    ", " TOP 10 LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+' ['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) + ']' ,CDSLCLIENTS_BOID ", "  (RIGHT(CDSLCLIENTS_BOID,8) LIKE  '" + reqStr + "%'  OR CDSLCLIENTS_FIRSTHOLDERNAME LIKE '" + reqStr + "%') and  cdslclients_branchid in(" + idlist[2] + ") ");
                        }
                    }
                    else if (idlist[3] == "ND")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS    ", " TOP 10 LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ", "  ( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" MASTER_NSDLCLIENTS ", " TOP 10 LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) + ' ['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,''))) +']' ,NSDLCLIENTS_DPID +''+ CAST(NSDLCLIENTS_BENACCOUNTID AS  VARCHAR) ", "  ( NSDLCLIENTS_BENACCOUNTID LIKE '" + reqStr + "%' OR NSDLCLIENTS_BENFIRSTHOLDERNAME LIKE  '" + reqStr + "%') and nsdlclients_branchid in (" + idlist[2] + ")  ");
                        }
                    }
                    else if (idlist[3] == "EM")
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' ,CNT_INTERNALID  ", "  cnt_internalid like 'EM%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')  ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' ,CNT_INTERNALID  ", " cnt_internalid like 'EM%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')  and cnt_branchid in (" + idlist[2] + ") ");
                        }
                    }
                    else
                    {
                        if (idlist[1] == "ALL")
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' ,CNT_INTERNALID   ", "  cnt_internalid like 'CL%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')   ");
                        }
                        else
                        {
                            DT = oDBEngine.GetDataTable(" TBL_MASTER_CONTACT ", " TOP 10  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+' '+ LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,''))) +' ' + LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) + ' ['+ LTRIM(RTRIM(ISNULL(CNT_UCC,CNT_SHORTNAME)))+']' ,CNT_INTERNALID  ", " s cnt_internalid like 'CL%' and (CNT_FIRSTNAME LIKE  '" + reqStr + "%' OR CNT_UCC LIKE  '" + reqStr + "%' OR  CNT_SHORTNAME LIKE  '" + reqStr + "%')   and cnt_branchid in (" + idlist[2] + ") ");
                        }
                    }
                }

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("0###No Record Found|");

            }
            #endregion
            #region SearchByEmployee
            if (Request.QueryString["SearchByEmployee"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();

                DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact", "distinct top 10  ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalid as Id    ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and tbl_master_employee.emp_contactId not in('" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "') and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            #region  GetAccountTypes
            if (Request.QueryString["CDSLAccountType"] == "1")
            {

                string reqStr = Request.QueryString["letters"].ToString();


                DT = oDBEngine.GetDataTable(" MASTER_CDSLCLIENTS ", " distinct cdslclients_BoSubstatus ", " cdslclients_BoSubstatus like '%" + reqStr + "%' ", " cdslclients_BoSubstatus");

                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][0].ToString() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");


            }
            #endregion
            //Need to Be Change By Subhadeep Old Format
            #region Serachclientwithexchange
            if (Request.QueryString["Serachclientwithexchange"] == "1")
            {
                string reqStr = Request.QueryString["letters"].ToString();
                //string[] param = Request.QueryString["search_param"].ToString();//.Split('~');
                DT = oDBEngine.GetDataTable("select distinct top 5 ltrim(rtrim(isnull(cnt_firstName,''))) + ltrim(rtrim(isnull(cnt_middleName,''))) +ltrim(rtrim(isnull(cnt_lastName,''))) +' ['+ltrim(rtrim(cnt_UCC))+']' as Name,cnt_internalId from tbl_master_contact,Config_BrokerageMain where cnt_internalId in (select BrokerageMain_CustomerID from Config_BrokerageMain where BrokerageMain_CustomerID=cnt_internalId and brokerageMain_SegmentID in(select exch_internalId from (select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS segment_name from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as D where segment_Name in(select seg_name  from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"].ToString() + ")) and brokerageMain_CompanyID='" + Session["LastCompany"].ToString() + "' and BrokerageMain_SegmentID='" + HttpContext.Current.Session["usersegid"].ToString() + "') and cnt_internalId=BrokerageMain_CustomerID and (cnt_firstName like '" + reqStr + "%' or cnt_UCC like '" + reqStr + "%') ");
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion





            /////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////Generic Ajax List Call////////////////////////////////////////
            #region GenericAjaxList
            if (Request.QueryString["GenericAjaxList"] == "1")
            {
                string RequestLetter = Request.QueryString["letters"].ToString();
                string[] param = Request.QueryString["search_param"].ToString().Replace("--", "+").Replace("^^", "%").Split('$');
                string strQuery_Table = param[0].Trim() != String.Empty ? param[0] : null;
                string strQuery_FieldName = param[1].Trim() != String.Empty ? param[1] : null;
                string strQuery_WhereClause = param[2].Trim() != String.Empty ? param[2] : null;
                string strQuery_OrderBy = param[3].Trim() != String.Empty ? param[3] : null;
                string strQuery_GroupBy = param[4].Trim() != String.Empty ? param[4] : null;
                if (strQuery_Table != null)
                {
                    strQuery_Table = strQuery_Table.Replace("RequestLetter", RequestLetter);
                }
                if (strQuery_FieldName != null)
                {
                    strQuery_FieldName = strQuery_FieldName.Replace("RequestLetter", RequestLetter);
                }
                if (strQuery_WhereClause != null)
                {
                    strQuery_WhereClause = strQuery_WhereClause.Replace("RequestLetter", RequestLetter);
                }
                DT = GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy, strQuery_GroupBy);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region GenericAjaxListSP
            if (Request.QueryString["GenericAjaxListSP"] == "1")
            {
                string RequestLetter = Convert.ToString(Request.QueryString["letters"]);
                string[] param = Convert.ToString(Request.QueryString["search_param"]).Split('$');
                char SplitChar = Convert.ToChar(param[4]);
                string ProcedureName = Convert.ToString(param[0]);
                string[] InputName = param[1].Split(SplitChar);
                string[] InputType = param[2].Split(SplitChar);
                string SetRequestLetter = param[3].Replace("RequestLetter", RequestLetter);
                string[] InputValue = SetRequestLetter.Split(SplitChar);
                if (ProcedureName.Trim() != String.Empty && (InputName.Length == InputType.Length) && (InputType.Length == InputValue.Length))
                {
                    DT = SelectProcedureArr(ProcedureName, InputName, InputType, InputValue);
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            Response.Write(Convert.ToString(DT.Rows[i][1]).ToUpper() + "###" + Convert.ToString(DT.Rows[i][0]) + "|");
                        }
                    }
                    else
                        Response.Write("No Record Found###No Record Found|");
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion
            #region GenericAjaxListQuery
            if (Request.QueryString["GenericAjaxListQuery"] == "1")
            {
                string RequestLetter = Request.QueryString["letters"].ToString();
                string[] param = Request.QueryString["search_param"].ToString().Replace("--", "+").Replace("^^", "%").Split('$');
                string strQuery = param[0].Trim() != String.Empty ? param[0] : null;
                if (strQuery != null)
                {
                    strQuery = strQuery.Replace("RequestLetter", RequestLetter);
                }

                DT = GetDataTable(strQuery);
                if (DT.Rows.Count != 0)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Response.Write(DT.Rows[i][1].ToString().ToUpper() + "###" + DT.Rows[i][0].ToString() + "|");
                    }
                }
                else
                    Response.Write("No Record Found###No Record Found|");
            }
            #endregion

            ////////////////////////////End Generic Ajax List Call////////////////////////////////////////
        }
        ////////////////////////////Generic Ajax List Variable////////////////////////////////////////

        SqlConnection oSqlConnection = new SqlConnection();

        //////////////////////////////End Generic Ajax List Variable//////////////////////////////////

        public void GetConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
            }
            else
            {
                //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                //DBConnectionRead  //DBConnectionDefault
                //oSqlConnection.ConnectionString = ConfigurationManager.AppSettings["DBConnectionDefault"]; MULTI
                oSqlConnection.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                oSqlConnection.Open();
            }
            //return lcon;
        }
        public void CloseConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
                oSqlConnection.Close();
            }
        }


        //////////////////////////Start Generic Ajax List Method/////////////////////////////////////////

        public DataTable GetDataTable(
                        String cTableName,     // TableName from which the field value is to be fetched
                        String cFieldName,     // The name if the field whose value needs to be returned
                        String cWhereClause)   // WHERE condition [if any]
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable        
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }
        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string cOrderBy)       // Order by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (cOrderBy != null)
            {
                lcSql += " Order By " + cOrderBy;
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }
        public DataTable GetDataTableGroup(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string groupBy)       // Group by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (groupBy != null)
            {
                lcSql += " group By " + groupBy;
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }
        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string groupBy,         // Gropu by condition
                            string cOrderBy)        // Order by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (groupBy != null)
            {
                lcSql += " group By " + groupBy;
            }
            if (cOrderBy != null)
            {
                lcSql += " Order By " + cOrderBy;
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }
        public DataTable GetDataTable(
                         String query)    // Full Query from which the value is to be fetched    
        {
            // Now we construct a SQL command that will fetch fields from the Suplied Query

            String lcSql;
            lcSql = query;

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable        
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;

        }
        public DataTable SelectProcedureArr(string ProcedureName, string[] InputName, string[] InputType, string[] InputValue)
        {
            GetConnection();
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, oSqlConnection);
            DataTable DT = new DataTable();

            try
            {

                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "C")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Char, 10));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToString(InputValue[LoopCnt]);
                    }
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, 255));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, 2000000));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToDateTime(InputValue[LoopCnt]);
                    }
                    else if (InputType[LoopCnt] == "DE")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Decimal, 14));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    //MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@musicname", SqlDbType.Decimal, 23));
                }
                //Create and add an output parameter to the Parameters collection. 

                //Set the direction for the parameter. This parameter returns the Rows that are returned.
                //MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Direction = ParameterDirection.Output;
                MyDataAdapter.SelectCommand.CommandTimeout = 0;
                MyDataAdapter.Fill(DT);
            }

            catch (Exception ex)
            {


            }
            finally
            {
                MyDataAdapter.Dispose();
                oSqlConnection.Close();
            }
            oSqlConnection.Close();
            return DT;
        }
        /////////////////////////End Generic Ajax List Method///////////////////////////////////////////

    }
}