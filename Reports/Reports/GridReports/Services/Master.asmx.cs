using DataAccessLayer;
//using ERP.OMS.Management.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Reports.Reports.GridReports.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class Master : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetUser(string SearchKey)
        {
            List<UserModel> listuser = new List<UserModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable user = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetUserBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(user);

                cmd.Dispose();
                con.Dispose();

                listuser = (from DataRow dr in user.Rows
                            select new UserModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString()
                            }).ToList();
            }

            return listuser;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(1));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cust);

                cmd.Dispose();
                con.Dispose();

                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["ID"].ToString(),
                                Na=dr["Name"].ToString(),
                                Cntno=dr["Contact No."].ToString(),
                                Altrno = dr["Alternate No."].ToString(),
                                add = Convert.ToString(dr["Address"])

                            }).ToList();
            }

            return listCust;
        }

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTaxComponent(string SearchKey)
        {
            List<TaxComponentModel> listTaxCompnant = new List<TaxComponentModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable TCompnent = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_TAXCOMPONENTSEARCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(TCompnent);

                cmd.Dispose();
                con.Dispose();

                listTaxCompnant = (from DataRow dr in TCompnent.Rows
                            select new TaxComponentModel()
                            {
                                ID = dr["TaxRates_ID"].ToString(),
                                Desc = dr["TaxRatesSchemeName"].ToString(),
                                Code = dr["Taxes_Code"].ToString(),
                                Name = dr["Taxes_Name"].ToString()
                            }).ToList();
            }

            return listTaxCompnant;
        }
        //End of Rev Debashis

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTCSCustomer(string SearchKey)
        {
            List<TCSCustomerModel> listCust = new List<TCSCustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_TCSCUSTOMERBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cust);

                cmd.Dispose();
                con.Dispose();

                listCust = (from DataRow dr in cust.Rows
                            select new TCSCustomerModel()
                            {
                                id = dr["ID"].ToString(),
                                Na = dr["Name"].ToString(),
                                Cntno = dr["Contact No."].ToString(),
                                Altrno = dr["Alternate No."].ToString(),
                                add = Convert.ToString(dr["Address"])

                            }).ToList();
            }

            return listCust;
        }
        //End of Rev Debashis

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTechnician(string SearchKey)
        {
            List<TechnicianModel> listTech = new List<TechnicianModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable tech = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_GETTECHNICIANLIST_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(tech);

                cmd.Dispose();
                con.Dispose();

                listTech = (from DataRow dr in tech.Rows
                            select new TechnicianModel()
                            {
                                id = dr["ID"].ToString(),
                                Na = dr["Name"].ToString(),
                                Cntno = dr["Contact No."].ToString(),
                                Altrno = dr["Alternate No."].ToString(),
                                add = Convert.ToString(dr["Address"])

                            }).ToList();
            }

            return listTech;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInfluencer(string SearchKey)
        {
            List<InfluencerModel> listInflu = new List<InfluencerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable influ = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_INFLUENCERSEARCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(influ);

                cmd.Dispose();
                con.Dispose();

                listInflu = (from DataRow dr in influ.Rows
                            select new InfluencerModel()
                            {
                                id = dr["ID"].ToString(),
                                Na = dr["Name"].ToString(),
                                Cntno = dr["Contact No."].ToString(),
                                Altrno = dr["Alternate No."].ToString(),
                                add = Convert.ToString(dr["Address"])

                            }).ToList();
            }

            return listInflu;
        }
        //End of Rev Debashis

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSalesManAgent(string SearchKey)
        {
            List<SalesmanAgentModel> listSm = new List<SalesmanAgentModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable sm = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_SALESMANSEARCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FILTERTEXT", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(sm);

                cmd.Dispose();
                con.Dispose();

                listSm = (from DataRow dr in sm.Rows
                          select new SalesmanAgentModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString()
                            }).ToList();
            }

            return listSm;
        }
        //End of Rev Debashis

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetOnlyCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(6));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cust);

                cmd.Dispose();
                con.Dispose();

                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["ID"].ToString(),
                                Na = dr["Name"].ToString(),
                                Cntno = dr["Contact No."].ToString(),
                                Altrno = dr["Alternate No."].ToString(),
                                add = Convert.ToString(dr["Address"])

                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustVend(string SearchKey)
        {
            List<CustVendModel> listCustvend = new List<CustVendModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable custvend = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(0));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(custvend);

                cmd.Dispose();
                con.Dispose();

                listCustvend = (from DataRow dr in custvend.Rows
                            select new CustVendModel()
                            {
                                id = dr["ID"].ToString(),
                                Na = dr["Name"].ToString(),
                                Cntno = dr["Contact No."].ToString()
                            }).ToList();
            }

            return listCustvend;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTransporterVendor(string SearchKey)
        {
            List<TransporterVendorModel> listCust = new List<TransporterVendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(3));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cust);

                cmd.Dispose();
                con.Dispose();

                listCust = (from DataRow dr in cust.Rows
                            select new TransporterVendorModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString()
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetLedger(string SearchKey)
        {
            List<LedgerModel> listLedg = new List<LedgerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable ledg = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Proc_GetLedger", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                //cmd.Parameters.AddWithValue("@BranchId", "");
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ledg);


                cmd.Dispose();
                con.Dispose();

                listLedg = (from DataRow dr in ledg.Rows
                            select new LedgerModel()
                            {
                                id = dr["ID"].ToString(),
                                Description = dr["AccountName"].ToString()

                            }).ToList();
            }

            return listLedg;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSubLedger(string SearchKey, string LedgerID, string AllSubLedg)
        {
            List<SubLedgerModel> listSubLedg = new List<SubLedgerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable subledg = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("Proc_GetSubLedger", con);
                SqlCommand cmd = new SqlCommand("PRC_SUBACCOUNTLIST_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@LedgerId", LedgerID);
                cmd.Parameters.AddWithValue("@AllSubLedger", AllSubLedg);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(subledg);


                cmd.Dispose();
                con.Dispose();

                listSubLedg = (from DataRow dr in subledg.Rows
                            select new SubLedgerModel()
                            {
                                DocCode = dr["SubAccount_ReferenceID"].ToString(),
                                Description = dr["Contact_Name"].ToString(),
                                Type = dr["MainAccount_SubLedgerType"].ToString()

                            }).ToList();
            }

            return listSubLedg;
        }

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPartyLedger(string SearchKey)
        {
            List<PartyLedgerModel> listPartyLedg = new List<PartyLedgerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable partyledg = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PARTYLEDGERLIST_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(partyledg);

                cmd.Dispose();
                con.Dispose();

                listPartyLedg = (from DataRow dr in partyledg.Rows
                               select new PartyLedgerModel()
                               {
                                   DocCode = dr["SubAccount_ReferenceID"].ToString(),
                                   Description = dr["Contact_Name"].ToString(),
                                   Type = dr["MainAccount_SubLedgerType"].ToString()

                               }).ToList();
            }

            return listPartyLedg;
        }
        //End of Rev Debashis

        //Rev Debashis PARTY LEDGER - ALL
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPartyLedgerAll(string SearchKey,string Type)
        {
            List<PartyLedgerAllModel> listPartyLedg = new List<PartyLedgerAllModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable partyledgall = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PARTYLEDGERALLLIST_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CRITERIA", Type);
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(partyledgall);

                cmd.Dispose();
                con.Dispose();

                listPartyLedg = (from DataRow dr in partyledgall.Rows
                                 select new PartyLedgerAllModel()
                                 {
                                     DocCode = dr["SubAccount_ReferenceID"].ToString(),
                                     Description = dr["Contact_Name"].ToString(),
                                     Type = dr["MainAccount_SubLedgerType"].ToString(),
                                     Address = dr["Address"].ToString()

                                 }).ToList();
            }

            return listPartyLedg;
        }
        //End of Rev Debashis

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetOnlyFinancer(string SearchKey, string BranchID)
        {
            List<FinancerModel> listFin = new List<FinancerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable fin = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetFinancerBranchwise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@Branch", BranchID);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(fin);

                cmd.Dispose();
                con.Dispose();

                listFin = (from DataRow dr in fin.Rows
                           select new FinancerModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString(),
                            }).ToList();
            }

            return listFin;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendor(string SearchKey)
        {
            List<VendorModel> listVend = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable vend = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(2));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(vend);

                cmd.Dispose();
                con.Dispose();

                listVend = (from DataRow dr in vend.Rows
                            select new VendorModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString()
                            }).ToList();
            }

            return listVend;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetOnlyVendor(string SearchKey)
        {
            List<VendorModel> listVend = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable vend = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(7));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(vend);

                cmd.Dispose();
                con.Dispose();

                listVend = (from DataRow dr in vend.Rows
                            select new VendorModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString()
                            }).ToList();
            }

            return listVend;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTransporter(string SearchKey)
        {
            List<TransporterModel> listVend = new List<TransporterModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(4));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cust);

                cmd.Dispose();
                con.Dispose();

                listVend = (from DataRow dr in cust.Rows
                            select new TransporterModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString(),
                            }).ToList();
            }

            return listVend;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProduct(string SearchKey)
        {
            List<ProductModel> listProd = new List<ProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable prod = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetProductBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(prod);

                cmd.Dispose();
                con.Dispose();

                listProd = (from DataRow dr in prod.Rows
                            select new ProductModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString(),
                                Description = dr["Description"].ToString()
                            }).ToList();
            }

            return listProd;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetGroup(string SearchKey)
        
        {
            List<GroupModel> listgroup = new List<GroupModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtgroup = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetGroupBind_Search_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dtgroup);

                cmd.Dispose();
                con.Dispose();

                listgroup = (from DataRow dr in dtgroup.Rows
                            select new GroupModel()
                            {
                                GroupCode = dr["GroupCode"].ToString(),
                                Description = dr["GroupDescription"].ToString()
                            }).ToList();
            }

            return listgroup;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEntityGroupMaster(string SearchKey,string Entitytype)
        {
            List<EntityGroupModel> listgroup = new List<EntityGroupModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtgroup = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_GROUPMASTERSEARCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ENTITYTYPE", Entitytype);
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dtgroup);

                cmd.Dispose();
                con.Dispose();

                listgroup = (from DataRow dr in dtgroup.Rows
                             select new EntityGroupModel()
                             {
                                 GroupCode = dr["GroupCode"].ToString(),
                                 Description = dr["GroupDescription"].ToString()
                             }).ToList();
            }

            return listgroup;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetGroupWiseVendor(string SearchKey, string GroupIDs, string Type)
        {
            List<GrpWiseVendorModel> listVend = new List<GrpWiseVendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable grpvend = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("GetGroupWiseVendorBind_Search_Report", con);
                SqlCommand cmd = new SqlCommand("prc_FetchCustomerVendorByGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartyType", Type);
                cmd.Parameters.AddWithValue("@srctxt", SearchKey);
                cmd.Parameters.AddWithValue("@GroupList", GroupIDs);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(grpvend);

                cmd.Dispose();
                con.Dispose();

                listVend = (from DataRow dr in grpvend.Rows
                            select new GrpWiseVendorModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString(),
                                Cntno = dr["Contact"].ToString(),
                                Partytype = dr["Type"].ToString()

                            }).ToList();
            }

            return listVend;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetGroupWiseCustomer(string SearchKey, string GroupIDs, string Type)
        {
            List<GrpWiseCustomerModel> listCust = new List<GrpWiseCustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable grpvend = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_FetchCustomerVendorByGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartyType", Type);
                cmd.Parameters.AddWithValue("@srctxt", SearchKey);
                cmd.Parameters.AddWithValue("@GroupList", GroupIDs);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(grpvend);

                cmd.Dispose();
                con.Dispose();

                listCust = (from DataRow dr in grpvend.Rows
                            select new GrpWiseCustomerModel()
                            {
                                id = dr["ID"].ToString(),
                                Na = dr["Name"].ToString(),
                                Cntno = dr["Contact No."].ToString(),
                                Altrno = dr["Alternate No."].ToString(),
                                add = Convert.ToString(dr["Address"])

                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSalesman(string SearchKey)
        {
            List<SalesmanModel> listSalesman = new List<SalesmanModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable salesman = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(5));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(salesman);

                cmd.Dispose();
                con.Dispose();

                listSalesman = (from DataRow dr in salesman.Rows
                            select new SalesmanModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Salesman_Name"].ToString(),
                            }).ToList();
            }

            return listSalesman;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetClass(string SearchKey)
        {
            List<ClassModel> listClass = new List<ClassModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable classes = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CLASSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(classes);

                cmd.Dispose();
                con.Dispose();

                listClass = (from DataRow dr in classes.Rows
                                select new ClassModel()
                                {
                                    id = dr["ID"].ToString(),
                                    Name = dr["Name"].ToString(),
                                }).ToList();
            }

            return listClass;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetClassBind(string SearchKey)
        {
            List<ClassModel> listClass = new List<ClassModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable classes = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_INDUSCLASSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@type", Convert.ToString(1));
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(classes);

                cmd.Dispose();
                con.Dispose();

                listClass = (from DataRow dr in classes.Rows
                             select new ClassModel()
                             {
                                 id = dr["ID"].ToString(),
                                 Name = dr["Name"].ToString(),
                             }).ToList();
            }

            return listClass;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetClassWiseProduct(string SearchKey, string ClassID)
        {
            List<ClassWiseProductModel> listcwiseProducts = new List<ClassWiseProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cwiseProduct = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("Proc_GetSubLedger", con);
                SqlCommand cmd = new SqlCommand("PRC_PRODUCTSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@ProductClass", ClassID);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cwiseProduct);


                cmd.Dispose();
                con.Dispose();

                listcwiseProducts = (from DataRow dr in cwiseProduct.Rows
                               select new ClassWiseProductModel()
                               {
                                   id = dr["ID"].ToString(),
                                   Code = dr["Code"].ToString(),
                                   Name = dr["Name"].ToString(),
                                   Hsn = dr["Hsn"].ToString()

                               }).ToList();
            }

            return listcwiseProducts;
        }

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNormalProduct(string SearchKey)
        {
            List<NormalProductModel> listnormalProducts = new List<NormalProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable normalProduct = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_NORMALPRODUCTSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(normalProduct);

                cmd.Dispose();
                con.Dispose();

                listnormalProducts = (from DataRow dr in normalProduct.Rows
                                     select new NormalProductModel()
                                     {
                                         id = dr["ID"].ToString(),
                                         Code = dr["Code"].ToString(),
                                         Name = dr["Name"].ToString(),
                                         Hsn = dr["Hsn"].ToString()

                                     }).ToList();
            }

            return listnormalProducts;
        }

        //Work Center
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetWorkCenter(string SearchKey)
        {
            List<WorkCenterModel> listWorkCenter = new List<WorkCenterModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable WorkCenter = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_WORKCENTERBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(WorkCenter);

                cmd.Dispose();
                con.Dispose();

                listWorkCenter = (from DataRow dr in WorkCenter.Rows
                                  select new WorkCenterModel()
                                      {
                                          id = dr["ID"].ToString(),
                                          Code = dr["Code"].ToString(),
                                          Name = dr["Name"].ToString()

                                      }).ToList();
            }

            return listWorkCenter;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetComponentProduct(string SearchKey)
        {
            List<NormalProductModel> listnormalProducts = new List<NormalProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable normalProduct = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_COMPONENTPRODUCTSLIST_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(normalProduct);

                cmd.Dispose();
                con.Dispose();

                listnormalProducts = (from DataRow dr in normalProduct.Rows
                                      select new NormalProductModel()
                                      {
                                          id = dr["ID"].ToString(),
                                          Code = dr["Code"].ToString(),
                                          Name = dr["Name"].ToString(),
                                          Hsn = dr["Hsn"].ToString()

                                      }).ToList();
            }

            return listnormalProducts;
        }
        //End of Rev Debashis

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetBatchBind(string SearchKey)
        {
            List<BatchBindModel> listnormalProducts = new List<BatchBindModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable Batch = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_BATCHBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Batch);

                cmd.Dispose();
                con.Dispose();

                listnormalProducts = (from DataRow dr in Batch.Rows
                                      select new BatchBindModel()
                                      {
                                          id = dr["ID"].ToString(),
                                          Batch_No = dr["BATCH_NO"].ToString()

                                      }).ToList();
            }

            return listnormalProducts;
        }
        //End of Rev Debashis

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetBrand(string SearchKey)
        {
            List<BrandModel> listBrand = new List<BrandModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable brand = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_BRANDBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(brand);

                cmd.Dispose();
                con.Dispose();

                listBrand = (from DataRow dr in brand.Rows
                             select new BrandModel()
                             {
                                 id = dr["ID"].ToString(),
                                 Name = dr["Name"].ToString(),
                             }).ToList();
            }
            return listBrand;
        }

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetBOM(string SearchKey)
        {
            List<BOMModel> listBOM = new List<BOMModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable bom = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_BOMCOMPONENTBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", "BOMNO");
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(bom);

                cmd.Dispose();
                con.Dispose();

                listBOM = (from DataRow dr in bom.Rows
                           select new BOMModel()
                             {
                                 id = dr["ID"].ToString(),
                                 BOM_NO = dr["BOM_NO"].ToString(),
                             }).ToList();
            }
            return listBOM;
        }
        //End of Rev Debashis

        //Rev Debashis
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPRDI(string SearchKey)
        {
            List<PRDIModel> listPRDI = new List<PRDIModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable prdi = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PRDICOMPONENTBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", "PRDINO");
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(prdi);

                cmd.Dispose();
                con.Dispose();

                listPRDI = (from DataRow dr in prdi.Rows
                            select new PRDIModel()
                           {
                               id = dr["ID"].ToString(),
                               ISSUE_NO = dr["ISSUE_NO"].ToString(),
                           }).ToList();
            }
            return listPRDI;
        }
        //End of Rev Debashis

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetLedgerBind(string SearchKey, string BranchID)
        {
            List<LedgerBindModel> listLedg = new List<LedgerBindModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable ledg = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Proc_GetLedgerBind_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@BranchId", BranchID);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ledg);


                cmd.Dispose();
                con.Dispose();

                listLedg = (from DataRow dr in ledg.Rows
                            select new LedgerBindModel()
                            {
                                ID = dr["ID"].ToString(),
                                LedgerDescription = dr["LedgerDescription"].ToString()

                            }).ToList();
            }

            return listLedg;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public object GetSubLedgerBind(string SearchKey, int LedgerId)
        {
            List<SubLedgerBindModel> listLedg = new List<SubLedgerBindModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable ledg = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Proc_GetSubLedgerBind_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@BranchId", Convert.ToString(Session["userbranchHierarchy"]));
                cmd.Parameters.AddWithValue("@LedgerId", LedgerId);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ledg);


                cmd.Dispose();
                con.Dispose();

                listLedg = (from DataRow dr in ledg.Rows
                            select new SubLedgerBindModel()
                            {
                                ID = dr["ID"].ToString(),
                                //Doc_Code = dr["cnt_internlId"].ToString(),
                                Description = dr["Description"].ToString(),
                                Type = dr["Type"].ToString()

                            }).ToList();
            }

            return listLedg;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetRecipientsBind(string SearchKey)
        {
            List<RecipientsBindModel> listRecpt = new List<RecipientsBindModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable recpt = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_EMAILSEARCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(recpt);


                cmd.Dispose();
                con.Dispose();

                listRecpt = (from DataRow dr in recpt.Rows
                             select new RecipientsBindModel()
                            {
                                ID = dr["ID"].ToString(),
                                Email = dr["Email"].ToString()

                            }).ToList();
            }

            return listRecpt;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomerSalesInvVendTransporter(string SearchKey)
        {
            List<CustvendTranspModel> listCustVendTransp = new List<CustvendTranspModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(8));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cust);

                cmd.Dispose();
                con.Dispose();

                listCustVendTransp = (from DataRow dr in cust.Rows
                            select new CustvendTranspModel()
                            {
                                id = dr["ID"].ToString(),
                                Name = dr["Name"].ToString(),
                                Cntno = dr["Contact No."].ToString(),
                                Partytype = dr["Type"].ToString()


                            }).ToList();
            }

            return listCustVendTransp;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorPurinvCustTransporter(string SearchKey)
        {
            List<CustvendTranspModel> listCustVendTransp = new List<CustvendTranspModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetCustomerBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", Convert.ToString(9));
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cust);

                cmd.Dispose();
                con.Dispose();

                listCustVendTransp = (from DataRow dr in cust.Rows
                                      select new CustvendTranspModel()
                                      {
                                          id = dr["ID"].ToString(),
                                          Name = dr["Name"].ToString(),
                                          Cntno = dr["Contact No."].ToString(),
                                          Partytype = dr["Type"].ToString()


                                      }).ToList();
            }

            return listCustVendTransp;
        }

        // Rev Deep
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEmpLeaveRegisterLists(string SearchKey)
        {
            List<EmpLeaveRegList> list = new List<EmpLeaveRegList>();
            string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                string cntType = "EM";
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(@"select distinct top 10 tmc.cnt_internalId ID,(tmc.cnt_firstName +' '+ tmc.cnt_lastName)  as Name from tbl_master_contact tmc
                                                        inner join tbl_master_employee tme on tmc.cnt_internalId = tme.emp_contactId
                                                        inner join proll_Leave_Transaction plt on tme.emp_contactId = plt.EmployeeCode
                                                        Where cnt_contactType='" + cntType + "' AND cnt_firstName like '%" + SearchKey + "%'");


                list = (from DataRow dr in dt.Rows
                        select new EmpLeaveRegList()
                        {
                            id = Convert.ToString(dr["ID"]),
                            Name = Convert.ToString(dr["Name"]),
                        }).ToList();
            }

            return list;
        }
        // End Rev Deep
        public class UserModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class EmpLeaveRegList
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class CustvendTranspModel
        {
            public string id { get; set; }
            public string Name { get; set; }
            public string Cntno { get; set; }
            public string Partytype { get; set; }
        }
        public class CustomerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Cntno { get; set; }
            public string Altrno { get; set; }
            public string add { get; set; }
        }

        public class TCSCustomerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Cntno { get; set; }
            public string Altrno { get; set; }
            public string add { get; set; }
        }
        public class TechnicianModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Cntno { get; set; }
            public string Altrno { get; set; }
            public string add { get; set; }
        }
        public class InfluencerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Cntno { get; set; }
            public string Altrno { get; set; }
            public string add { get; set; }
        }

        public class FinancerModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class TransporterVendorModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class LedgerModel
        {
            public string id { get; set; }
            public string Description { get; set; }
        }
        public class SubLedgerModel
        {
            public string DocCode { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
        }

        public class PartyLedgerModel
        {
            public string DocCode { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
        }

        //Rev Debashis
        public class PartyLedgerAllModel
        {
            public string DocCode { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public string Address { get; set; }
        }
        //End of Rev Debashis
        public class VendorModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class GrpWiseVendorModel
        {
            public string id { get; set; }
            public string Name { get; set; }
            public string Cntno { get; set; }
            public string Partytype { get; set; }
        }

        public class GrpWiseCustomerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Cntno { get; set; }
            public string Altrno { get; set; }
            public string add { get; set; }
        }

        public class TransporterModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class ProductModel
        {
            public string id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class GroupModel
        {
            public string GroupCode { get; set; }
            public string Description { get; set; }
        }

        public class EntityGroupModel
        {
            public string GroupCode { get; set; }
            public string Description { get; set; }
        }
        public class CustVendModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Cntno { get; set; }
           
        }
        public class SalesmanModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }

        //Rev Debashis
        public class SalesmanAgentModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        //End of Rev Debashis
        public class ClassModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class ClassWiseProductModel
        {
            public string id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Hsn { get; set; }

        }
        public class NormalProductModel
        {
            public string id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Hsn { get; set; }

        }

        public class WorkCenterModel
        {
            public string id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }        

        public class BatchBindModel
        {
            public string id { get; set; }
            public string Batch_No { get; set; }
        }
        public class BrandModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }

        public class BOMModel
        {
            public string id { get; set; }
            public string BOM_NO { get; set; }
        }

        public class PRDIModel
        {
            public string id { get; set; }
            public string ISSUE_NO { get; set; }
        }
        public class LedgerBindModel
        {
            public string ID { get; set; }
            public string LedgerDescription { get; set; }
        }
        public class SubLedgerBindModel
        {
            public string ID { get; set; }
            //public string Doc_Code { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
        }
        public class RecipientsBindModel
        {
            public string ID { get; set; }
            public string Email { get; set; }
        }

        //Rev Debashis
        public class TaxComponentModel
        {
            public string ID { get; set; }
            public string Desc { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }
        //End of Rev Debashis
    }
}
