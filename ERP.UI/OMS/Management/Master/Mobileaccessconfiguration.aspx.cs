using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.IO; 
//using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;
//////using DevExpress.Web;
//using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using DevExpress.Web;
using EntityLayer.CommonELS;
using DataAccessLayer;

namespace ERP.OMS.Management.Master
{
    public partial class Mobileaccessconfiguration : ERP.OMS.ViewState_class.VSPage
    {
        public string strID;
        String strOpeningCr;
        String strOpeningDr;
        DataTable dataTable = new DataTable();
        DBEngine odebEngine = new DBEngine();

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        List<userid> UID = new List<userid>();
        public class userid
        {
            public int id { get; set; } 
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/Mobileaccessconfiguration.aspx");
            if (!IsPostBack)
            {
                
                Session.Remove("OpeningDatatable");
                if (Session["userbranchHierarchy"] != null)
                {
                    dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id not in(select branch_Id from tbl_master_MobileConfiguration)";
                    cmbBranch.DataBind();
                 

                }
                CreateSessionDatatable();
                RefreshGrid();
                //glSalesman.Enabled = false;
                //glManager.Enabled = false;
                //glFinancer.Enabled = false;
                //glTopApproval.Enabled = false;
               
            }


        }
        protected void RefreshGrid()
        {
            OpeningBalanceBl opb = new OpeningBalanceBl();
            DataTable opDetails = opb.GetOpeningBalanceDetails(Convert.ToString(Session["userbranchHierarchy"]));
            ProcedureExecute proc;
            DataTable rtrnvalue;

            using (proc = new ProcedureExecute("prc_GetMobileAccessConfiguration"))
            {
                //proc.AddVarcharPara("@BranchList", 2000, BranchHierarchy);
                rtrnvalue = proc.GetTable();
                
            }

            MobileConfigGrid.DataSource = rtrnvalue;
            Session["MobileConfigDatatable"] = rtrnvalue;
            MobileConfigGrid.DataBind();
           

           
        }
        public void CreateSessionDatatable()
        {
            DataTable MobileConfigDt = new DataTable();
            MobileConfigDt.Columns.Add("UniqueId", typeof(System.Guid));
            MobileConfigDt.Columns.Add("Active", typeof(System.Boolean));
            MobileConfigDt.Columns.Add("BranchId", typeof(System.Int32));
            MobileConfigDt.Columns.Add("Branch", typeof(System.String));
            MobileConfigDt.Columns.Add("SalesCode", typeof(System.String));
            MobileConfigDt.Columns.Add("Salesman", typeof(System.String));
            MobileConfigDt.Columns.Add("ManagerCode", typeof(System.String));
            MobileConfigDt.Columns.Add("Manager", typeof(System.String));
            MobileConfigDt.Columns.Add("TopApprovalCode", typeof(System.String));
            MobileConfigDt.Columns.Add("TopApproval", typeof(System.String));
            MobileConfigDt.Columns.Add("FinancerCode", typeof(System.String));
            MobileConfigDt.Columns.Add("Financer", typeof(System.String));
            MobileConfigDt.Columns.Add("createdbyuser", typeof(System.Int32));
            MobileConfigDt.Columns.Add("CreatedOn", typeof(System.DateTime));


            Session["MobileConfigDatatable"] = MobileConfigDt;
        }

        protected void MobileConfigGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            bool checkblankuser=false;
            if (glSalesman.Text=="")
            {
                if(glManager.Text=="")
                {
                    if (glFinancer.Text == "")
                    {
                       
                        if (glTopApproval.Text == "")
                        {
                            checkblankuser = false;
                        }
                        else
                        {
                            checkblankuser = true;
                        }
                    }
                    else
                    {
                        checkblankuser = true;
                    }

                }
                else
                {
                    checkblankuser = true;
                }
            }
            else
            {
                checkblankuser = true;
            }

         
          
            string returnPara = Convert.ToString(e.Parameters);
            DataTable MobileconfigDt = (DataTable)Session["MobileConfigDatatable"];
            #region Add new
            if (returnPara.Split('~')[0] == "Add")
            {
               
                userid userid1 = new userid();

                if (checkblankuser == false)
                {
                    MobileConfigGrid.JSProperties["cpBlankAlert"] = "You should select at least one user for Salesman/Manager/Approver/Financer to Add data.";
                }
                else
                {

                string BranchCode = "";
                if (cmbBranch.Value != null)
                {
                    BranchCode = Convert.ToString(cmbBranch.Value);
                }
                //string salesman = Convert.ToString(cmbSalesman.Value);
                //string manager = Convert.ToString(cmbManager.Value);
                //string topapproval = Convert.ToString(cmbTopApproval.Value);
                //string financer = Convert.ToString(cmbFinancer.Value);
                //DataRow[] filterRow = MobileconfigDt.Select("BranchId='" + BranchCode + "' and SalesCode='" + salesman + "' and ManagerCode='" + manager + "' and FinancerCode='" + financer +"'");
                DataRow[] filterRow = MobileconfigDt.Select("BranchId='" + BranchCode + "'");
                if (filterRow.Length > 0)
                {
                    string activate;
                    if (checkIsactive.Checked==true)
                    {
                        activate = "Yes";
                    }
                    else
                    {
                        activate = "No";
                    }
                    filterRow[0]["Active"] = activate;
                    filterRow[0]["BranchId"] = Convert.ToInt32(cmbBranch.Value);
                    filterRow[0]["Branch"] = cmbBranch.Text;
                    string scode="";
                    if (glSalesman.GridView.GetSelectedFieldValues("Code").Count>0)
                    {
                        for (int i = 0; i <= glSalesman.GridView.GetSelectedFieldValues("Code").Count-1; i++)
                      {
                            if(scode==""){
                                scode = Convert.ToString(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                scode = scode+","+Convert.ToString(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                                
                           
                      }
                    }

                    filterRow[0]["SalesCode"] = scode;
                    filterRow[0]["Salesman"] = glSalesman.Text;

                    string mcode = "";
                    if (glManager.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glManager.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (mcode == "")
                            {
                                mcode = Convert.ToString(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                mcode = mcode + "," + Convert.ToString(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }

                    filterRow[0]["ManagerCode"] = mcode;
                    filterRow[0]["Manager"] = glManager.Text;


                    string tcode = "";
                    if (glTopApproval.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glTopApproval.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (tcode == "")
                            {
                                tcode = Convert.ToString(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                tcode = tcode + "," + Convert.ToString(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }
                    filterRow[0]["TopApprovalCode"] = tcode;
                    filterRow[0]["TopApproval"] = glTopApproval.Text;

                    string fcode = "";
                    if (glFinancer.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glFinancer.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (fcode == "")
                            {
                                fcode = Convert.ToString(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                fcode = fcode + "," + Convert.ToString(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }
                    filterRow[0]["FinancerCode"] = fcode;
                    filterRow[0]["Financer"] = glFinancer.Text;
                    filterRow[0]["createdbyuser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    filterRow[0]["CreatedOn"] = DateTime.Today;
                 
                }
                else
                {
                    Boolean active;
                    active = false;


                    string activate;
                    if (checkIsactive.Checked == true)
                    {
                        activate = "Yes";
                    }
                    else
                    {
                        activate = "No";
                    }

                    DataRow newRow = MobileconfigDt.NewRow();
                    newRow["UniqueId"] = Guid.NewGuid();
                    newRow["Active"] = activate;
                    newRow["BranchId"] = Convert.ToInt32(cmbBranch.Value);
                    newRow["Branch"] = cmbBranch.Text;

                    string scode = "";
                    if (glSalesman.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glSalesman.GridView.GetSelectedFieldValues("Code").Count-1; i++)
                        {
                            if (scode == "")
                            {
                                scode = Convert.ToString(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                scode = scode +","+ Convert.ToString(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }

                    newRow["SalesCode"] = scode;
                    newRow["Salesman"] = glSalesman.Text;
                    //newRow["SalesCode"] =cmbSalesman.Value;
                    //newRow["Salesman"] = cmbSalesman.Text;


                    string mcode = "";
                    if (glManager.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glManager.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (mcode == "")
                            {
                                mcode = Convert.ToString(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                mcode = mcode + "," + Convert.ToString(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }

                    newRow["ManagerCode"] = mcode;
                    newRow["Manager"] = glManager.Text;

                    //newRow["ManagerCode"] = cmbManager.Value;
                    //newRow["Manager"] = cmbManager.Text;

                    string tcode = "";
                    if (glTopApproval.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glTopApproval.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (tcode == "")
                            {
                                tcode = Convert.ToString(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                tcode = tcode + "," + Convert.ToString(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }

                    newRow["TopApprovalCode"] = tcode;
                    newRow["TopApproval"] = glTopApproval.Text;
                    //newRow["TopApprovalCode"] = cmbTopApproval.Value;
                    //newRow["TopApproval"] = cmbTopApproval.Text;


                    string fcode = "";
                    if (glFinancer.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glFinancer.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (fcode == "")
                            {
                                fcode = Convert.ToString(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                fcode = fcode + "," + Convert.ToString(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }

                    newRow["FinancerCode"] = fcode;
                    newRow["Financer"] = glFinancer.Text;
                    //newRow["FinancerCode"] = cmbFinancer.Value;
                    //newRow["Financer"] = cmbFinancer.Text;
                    newRow["createdbyuser"] = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    newRow["CreatedOn"] = Convert.ToDateTime(DateTime.Today);

                    MobileconfigDt.Rows.Add(newRow);
                }
                //For avoid duplicacy in different profile -----------------------------------------
                bool check_duid = false;
                #region 
                if (UID.Count > 0)
                {
                    if (UID.Count>1)
                    {
                        for (int i = 0; i < UID.Count - 1; i++)
                        {
                            int fuid;
                            fuid = UID[i].id;
                            for (int j = 0; j <= UID.Count - 1; j++)
                            {
                                if (i == j)
                                {

                                }
                                else
                                {
                                    if (fuid == UID[j].id)
                                    {
                                        check_duid = true;
                                    }
                                }
                            }
                        }
                    }
               
                   
                }

                #endregion
               //-----------------------------------------------------------------------------------
                if (check_duid==false)
                {
                  Session["MobileConfigDatatable"] = MobileconfigDt;
                  MobileConfigGrid.DataSource = MobileconfigDt;
                  MobileConfigGrid.DataBind();
                }
                else
                {
                    MobileConfigGrid.JSProperties["cpProfileDuplicacy"] = "One User Can be saved Only in One Profile.";
                }
               

                //OpeningGrid.JSProperties["cpTotalAmount"] = ComputeTotalDrCr(OpeningDt);
            }
        }
            #endregion
            #region Edit
            else if (returnPara.Split('~')[0] == "Edit")
            {
                DataRow[] filterRow = MobileconfigDt.Select("UniqueId='" + returnPara.Split('~')[1] + "'");
                if (filterRow.Length > 0)
                {
                    //string BranchId = Convert.ToString(filterRow[0]["BranchId"]);
                    //string AccountCode = GetAccountCode(Convert.ToString(filterRow[0]["AccountCode"]));
                    //string SubAccountCode = Convert.ToString(filterRow[0]["SubAccountCode"]);
                    //string DrCr = Convert.ToString(filterRow[0]["DrCr"]);
                    //string Amount;
                    //if (DrCr == "D")
                    //    Amount = Convert.ToString(filterRow[0]["DebitAmount"]);
                    //else
                    //    Amount = Convert.ToString(filterRow[0]["CreditAmount"]);
                  


                    string checkactive = Convert.ToString(filterRow[0]["Active"]);
                    string BranchId = Convert.ToString(filterRow[0]["BranchId"]);
                    string salescode =Convert.ToString( filterRow[0]["SalesCode"]);
                    Session["sCode"] = Convert.ToString(filterRow[0]["SalesCode"]);
                    string manager = Convert.ToString(filterRow[0]["ManagerCode"]);
                    Session["mCode"] = Convert.ToString(filterRow[0]["ManagerCode"]);
                    string topapproval = Convert.ToString(filterRow[0]["TopApprovalCode"]);
                    Session["tCode"] = Convert.ToString(filterRow[0]["TopApprovalCode"]);
                    string financer = Convert.ToString(filterRow[0]["FinancerCode"]);
                    Session["fCode"] = Convert.ToString(filterRow[0]["FinancerCode"]);

                    MobileConfigGrid.JSProperties["cpBeforeEdit"] = returnPara.Split('~')[1] +
                                                              "~" + checkactive +
                                                              "~" + BranchId +
                                                              "~" + salescode +
                                                              "~" + manager +
                                                              "~" + topapproval +
                                                              "~" + financer;

                }


            }
            #endregion
            #region AfterEdit
            else if (returnPara.Split('~')[0] == "EditDone")
            {
                string uniqueId = returnPara.Split('~')[1];
                DataRow[] filterRow = MobileconfigDt.Select("UniqueId='" + returnPara.Split('~')[1] + "'");
                if (filterRow.Length > 0)
                {
                    string activate;
                    if (checkIsactive.Checked == true)
                    {
                        activate = "Yes";
                    }
                    else
                    {
                        activate = "No";
                    }
                    filterRow[0]["Active"] = activate;
                    filterRow[0]["UniqueId"] = returnPara.Split('~')[1];
                    filterRow[0]["BranchId"] = Convert.ToInt32(cmbBranch.Value);
                    filterRow[0]["Branch"] = cmbBranch.Text;
                    List<userid> UID = new List<userid>();
                    userid userid1 = new userid();
                    string scode = "";
                    if (glSalesman.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                       
                        for (int i = 0; i <= glSalesman.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                           
                            if (scode == "")
                            {
                                scode = Convert.ToString(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                scode = scode + "," + Convert.ToString(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glSalesman.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }

                        }
                    }

                    filterRow[0]["SalesCode"] = scode;
                    filterRow[0]["Salesman"] = glSalesman.Text;

                  
                    string mcode = "";
                    if (glManager.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glManager.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (mcode == "")
                            {
                                mcode = Convert.ToString(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                mcode = mcode + "," + Convert.ToString(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glManager.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }

                    filterRow[0]["ManagerCode"] = mcode;
                    filterRow[0]["Manager"] = glManager.Text;


                    string tcode = "";
                    if (glTopApproval.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glTopApproval.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (tcode == "")
                            {
                                tcode = Convert.ToString(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                tcode = tcode + "," + Convert.ToString(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glTopApproval.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }
                    filterRow[0]["TopApprovalCode"] = tcode;
                    filterRow[0]["TopApproval"] = glTopApproval.Text;

                    string fcode = "";
                    if (glFinancer.GridView.GetSelectedFieldValues("Code").Count > 0)
                    {
                        for (int i = 0; i <= glFinancer.GridView.GetSelectedFieldValues("Code").Count - 1; i++)
                        {
                            if (fcode == "")
                            {
                                fcode = Convert.ToString(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }
                            else
                            {
                                fcode = fcode + "," + Convert.ToString(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                userid1 = new userid();
                                userid1.id = Convert.ToInt32(glFinancer.GridView.GetSelectedFieldValues("Code")[i]);
                                UID.Add(userid1);
                            }


                        }
                    }
                    filterRow[0]["FinancerCode"] = fcode;
                    filterRow[0]["Financer"] = glFinancer.Text;

                    //For avoid duplicacy in different profile -----------------------------------------
                    bool check_duid = false;
                    #region
                    if (UID.Count > 0)
                    {
                        for (int i = 0; i < UID.Count - 1; i++)
                        {
                            int fuid;
                            fuid = UID[i].id;
                            for (int j = 0; j < UID.Count - 1; j++)
                            {
                                if (i == j)
                                {

                                }
                                else
                                {
                                    if (fuid == UID[j].id)
                                    {
                                        //MobileConfigGrid.JSProperties["cpProfileDuplicacy"] = "One User Can be saved Only in One Profile.";
                                        check_duid = true;
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                    //-----------------------------------------------------------------------------------

                    if (check_duid == false)
                    {
                        Session["MobileConfigDatatable"] = MobileconfigDt;
                        MobileConfigGrid.DataSource = MobileconfigDt;
                        MobileConfigGrid.DataBind();
                    }
                    else
                    {
                        MobileConfigGrid.JSProperties["cpProfileDuplicacy"] = "One User Can be Saved Only in One Profile.";
                    }
                  
                }
            }
            #endregion
            #region SaveAllRecord
            else if (returnPara.Split('~')[0] == "SaveAllRecord")
            {
                if (checkblankuser == false)
                {
                    MobileConfigGrid.JSProperties["cpBlankAlert"] = "You should select at least one user for Salesman/Manager/Approver/Financer to Add data.";
                }
                else { 
                SaveAllRecord();
                MobileConfigGrid.JSProperties["cpClientMsg"] = "Updated Successfully.";
                }
            }
            #endregion
            #region Delete
            if (returnPara.Split('~')[0] == "Delete")
            {
                 //DataRow[] filterRow = MobileconfigDt.Select("UniqueId='" + returnPara.Split('~')[1] + "'");
                 //if (filterRow.Length > 0)
                 //{
                 //    int BranchId = Convert.ToInt32(filterRow[0]["BranchId"]);

                 //    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                 //    //SqlCommand cmd = new SqlCommand("prc_DelMobileAccessConfiguration", con);
                 //    //cmd.CommandType = CommandType.StoredProcedure;
                 //    //cmd.Parameters.AddWithValue("@branch_id", BranchId);
                 //    //con.Open();
                 //    //cmd.ExecuteNonQuery();
                 //    //con.Close();
                  


                 //}


                DataRow[] filterRow = MobileconfigDt.Select("UniqueId='" + returnPara.Split('~')[1] + "'");
                if (filterRow.Length > 0)
                {
                    filterRow[0].Delete();
                }
                MobileconfigDt.AcceptChanges();
                SaveAllRecord();
                Session["MobileConfigDatatable"] = MobileconfigDt;
                MobileConfigGrid.DataSource = MobileconfigDt;
                MobileConfigGrid.DataBind();
               // Response.Redirect("Mobileaccessconfiguration.aspx");
                MobileConfigGrid.JSProperties["cpRedirect"] = "Mobileaccessconfiguration.aspx";
                dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id not in(select branch_Id from tbl_master_MobileConfiguration)";
                cmbBranch.DataBind();
                MobileConfigGrid.JSProperties["cpClientMsg"] = "Deleted Successfully.";
              
            }
            #endregion
        
            


        }
        protected void SaveAllRecord()
        {
            //DataTable OpeningDt = (DataTable)Session["MobileConfigDatatable"];
            //DataTable finalDt = OpeningDt.Clone();
            //finalDt.Merge(OpeningDt);
            ////Remove Extra Columns
            //finalDt.Columns.Remove("UniqueId");
            //finalDt.Columns.Remove("Branch");
            //finalDt.Columns.Remove("Account");
            //finalDt.Columns.Remove("SubAccount");
            //finalDt.Columns.Remove("DrCr");
            //OpeningBalanceBl opb = new OpeningBalanceBl();
            //opb.UpdateOpeningBalance(finalDt);
            //OpeningGrid.JSProperties["cpClientMsg"] = "Updated Successfully.";
            try
            {

                DataTable MobileDt = (DataTable)Session["MobileConfigDatatable"];
                DataTable finalDt = MobileDt.Clone();
                finalDt.Merge(MobileDt);
                //Remove Extra Columns
                finalDt.Columns.Remove("UniqueId");
                finalDt.Columns.Remove("Branch");
                finalDt.Columns.Remove("Salesman");
                finalDt.Columns.Remove("Manager");
                finalDt.Columns.Remove("TopApproval");
                finalDt.Columns.Remove("Financer");

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_MobileAccessConfiguration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@opTable", finalDt);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                //OpeningBalanceBl opb = new OpeningBalanceBl();
                //opb.UpdateOpeningBalance(finalDt);
               
            }
            catch(Exception ex){

            }
        }
        protected void MobileConfigGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["MobileConfigDatatable"] != null)
            {
                MobileConfigGrid.DataSource = (DataTable)Session["MobileConfigDatatable"];
            }

        }

        protected void CustPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            glSalesman.Enabled = true;
            glManager.Enabled = true;
            glFinancer.Enabled = true;
            glTopApproval.Enabled = true;
            string[] param = e.Parameter.Split('~');

            int branchid = Convert.ToInt32(param[0]);
           
            if (param.Length>1)
            {
                string status = Convert.ToString(param[1]);
                if (status=="Edit")
                {
                    //DataTable dtCmb = oDBEngine.GetDataTable("select c.cnt_firstName+' '+c.cnt_middleName+' '+c.cnt_lastName as Name,c.cnt_internalId as 'Code' from tbl_master_contact c inner join tbl_master_employee e on c.cnt_internalId=e.emp_contactId where c.cnt_branchid='" + branchid + "'");
                   
                    //Commented By Subhra on 20/04/2018
                   DataTable dtCmb = oDBEngine.GetDataTable("select user_name as 'Name',user_id as 'Code' from tbl_master_user  where  user_branchId='" + branchid + "'");    

                    //Rev  1.0   V 1.0.102   16/01/2019  at the suggestion of Pijush Bhattacharya     mail:Issue in TAB Configuration module(Review (Approved)) 
                    //DataTable dtCmb = oDBEngine.GetDataTable("Select cnt_id as Code,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name "+
                    //    "from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG' and cnt_branchid='" + branchid + "' "+
                    //    "union all select emp_id as Code,isnull(cnt_firstName,'')+SPACE(1)+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name "+
                    //    "from (select row_number() over (partition by emp_cntId order by emp_id desc ) as Row, emp_cntId,emp_id "+
                    //    "from tbl_trans_employeeCTC where emp_type=19) ctc inner join tbl_master_contact cnt on ctc.emp_cntId=cnt.cnt_internalId "+
                    //    "where ctc.Row=1  and cnt_branchid='" + branchid + "'");   



                    //DataTable dtCmb = oDBEngine.GetDataTable("select t.Name,ur.user_id as Code from tbl_master_user ur inner join"+
                    //    "(Select cnt_internalId,cnt_id as Code,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name " +
                    //    "from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG' and cnt_branchid='" + branchid + "' " +
                    //    "union all "+
                    //    "select cnt_internalId,emp_id as Code,isnull(cnt_firstName,'')+SPACE(1)+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name " +
                    //    "from (select row_number() over (partition by emp_cntId order by emp_id desc ) as Row, emp_cntId,emp_id " +
                    //    "from tbl_trans_employeeCTC where emp_type=19) ctc inner join tbl_master_contact cnt on ctc.emp_cntId=cnt.cnt_internalId " +
                    //    "where ctc.Row=1  and cnt_branchid='" + branchid + "' ) as t on ur.user_contactId=t.cnt_internalId "
                    //    );   




                    //End of Rev 1.0
                    Session["CustSession"] = dtCmb;

                    DataTable dtTopApprover = oDBEngine.GetDataTable("select user_name as 'Name',user_id as 'Code' from tbl_master_user");
                    Session["TopApproverSession"] = dtTopApprover;

                    DataTable dtCmbFinancer = oDBEngine.GetDataTable("select user_name as 'Name',user_id as 'Code' from tbl_master_user  where user_contactId in(SELECT c.cnt_internalId from tbl_master_contact c inner join tbl_master_FinancerExecutive f on c.cnt_id=f.executive_id) and user_branchId='" + branchid + "'");
                    Session["CustFinanceSession"] = dtCmbFinancer;
                   
                  
                    //----------------------------------------------sales---------------------------------------------------------
                    glSalesman.DataSource = dtCmb;
                    glSalesman.DataBind();
                   
                    string[] sales = Convert.ToString(Session["sCode"]).Split(',');
                    if (sales.Length > 0)
                    {
                        for (int s = 0; s <= sales.Length - 1; s++)
                        {
                            glSalesman.GridView.Selection.SelectRowByKey(sales[s]);
                        }
                    }
                    //-------------------------------------------------------------------------------------------------------------

                    //----------------------------------------------Manager--------------------------------------------------------
                    glManager.DataSource = dtCmb;
                    glManager.DataBind();
                    string[] Manager = Convert.ToString(Session["mCode"]).Split(',');
                    if (Manager.Length > 0)
                    {
                        for (int m = 0; m <= Manager.Length - 1; m++)
                        {
                            glManager.GridView.Selection.SelectRowByKey(Manager[m]);
                        }
                    }
                    //---------------------------------------------------------------------------------------------------------------
                    //----------------------------------------------Financer--------------------------------------------------------
                    glFinancer.DataSource = dtCmbFinancer;
                    glFinancer.DataBind();
                    string[] Financer = Convert.ToString(Session["fCode"]).Split(',');
                    if (Financer.Length > 0)
                    {
                        for (int f = 0; f <= Financer.Length - 1; f++)
                        {
                            glFinancer.GridView.Selection.SelectRowByKey(Financer[f]);
                        }
                    }
                    //---------------------------------------------------------------------------------------------------------------

                    //----------------------------------------------TopApproval--------------------------------------------------------
                    glTopApproval.DataSource = dtTopApprover;
                    glTopApproval.DataBind();
                    string[] TopApproval = Convert.ToString(Session["tCode"]).Split(',');
                    if (TopApproval.Length > 0)
                    {
                        for (int t = 0; t <= TopApproval.Length - 1; t++)
                        {
                            glTopApproval.GridView.Selection.SelectRowByKey(TopApproval[t]);
                        }
                    }
                    //---------------------------------------------------------------------------------------------------------------

                }
            }
            else
            {
                //DataTable dtCmb = oDBEngine.GetDataTable("select c.cnt_firstName+' '+c.cnt_middleName+' '+c.cnt_lastName as Name,c.cnt_internalId as 'Code' from tbl_master_contact c inner join tbl_master_employee e on c.cnt_internalId=e.emp_contactId where c.cnt_branchid='" + branchid + "'");
               
                //Below line commented by Jitendra on 06-06-2017 at the suggestion of Pijush Bhattacharya- Remove branch from filteration   
                //Commented By Subhra on 20/04/2018
               DataTable dtCmb = oDBEngine.GetDataTable("select user_name as 'Name',user_id as 'Code' from tbl_master_user  where user_branchId='" + branchid + "'");

                //Rev  1.0   V 1.0.102   16/01/2019  at the suggestion of Pijush Bhattacharya     mail:Issue in TAB Configuration module(Review (Approved)) 
                //DataTable dtCmb = oDBEngine.GetDataTable("Select cnt_id as Code,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG' and cnt_branchid='" + branchid + "' union all select emp_id as emp_cntId,isnull(cnt_firstName,'')+SPACE(1)+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Salesman_Name from (select row_number() over (partition by emp_cntId order by emp_id desc ) as Row, emp_cntId,emp_id from tbl_trans_employeeCTC where emp_type=19) ctc inner join tbl_master_contact cnt on ctc.emp_cntId=cnt.cnt_internalId where ctc.Row=1  and cnt_branchid='" + branchid + "'");   
                //DataTable dtCmb = oDBEngine.GetDataTable("select t.Name,ur.user_id as Code from tbl_master_user ur inner join" +
                //       "(Select cnt_internalId,cnt_id as Code,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name " +
                //       "from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG' and cnt_branchid='" + branchid + "' " +
                //       "union all " +
                //       "select cnt_internalId,emp_id as Code,isnull(cnt_firstName,'')+SPACE(1)+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name " +
                //       "from (select row_number() over (partition by emp_cntId order by emp_id desc ) as Row, emp_cntId,emp_id " +
                //       "from tbl_trans_employeeCTC where emp_type=19) ctc inner join tbl_master_contact cnt on ctc.emp_cntId=cnt.cnt_internalId " +
                //       "where ctc.Row=1  and cnt_branchid='" + branchid + "' ) as t on ur.user_contactId=t.cnt_internalId "
                //       );   
                //End of Rev 1.0
                Session["CustSession"] = dtCmb;

                DataTable dtTopApprover = oDBEngine.GetDataTable("select user_name as 'Name',user_id as 'Code' from tbl_master_user");               
                Session["TopApproverSession"] = dtTopApprover;

                DataTable dtCmbFinancer = oDBEngine.GetDataTable("select user_name as 'Name',user_id as 'Code' from tbl_master_user  where user_contactId in(SELECT c.cnt_internalId from tbl_master_contact c inner join tbl_master_FinancerExecutive f on c.cnt_id=f.executive_id) and user_branchId='" + branchid + "'");
                Session["CustFinanceSession"] = dtCmbFinancer;

                glSalesman.DataSource = dtCmb;
                glSalesman.DataBind();

                glManager.DataSource = dtCmb;
                glManager.DataBind();

                glTopApproval.DataSource = dtTopApprover;
                glTopApproval.DataBind();

                glFinancer.DataSource = dtCmbFinancer;
                glFinancer.DataBind();
            }
           
        }

        protected void glSalesman_DataBinding(object sender, EventArgs e)
        {
           if (Session["CustSession"] != null)
            {
                glSalesman.DataSource = (DataTable)Session["CustSession"];
            }
        }

        protected void glManager_DataBinding(object sender, EventArgs e)
        {
            if (Session["CustSession"] != null)
            {
                glManager.DataSource = (DataTable)Session["CustSession"];
            }
        }

        protected void glTopApproval_DataBinding(object sender, EventArgs e)
        {         


            if (Session["TopApproverSession"] != null)
            {
                glTopApproval.DataSource = (DataTable)Session["TopApproverSession"];
            }
        }

        protected void glFinancer_DataBinding(object sender, EventArgs e)
        {
            if (Session["CustFinanceSession"] != null)
            {
                glFinancer.DataSource = (DataTable)Session["CustFinanceSession"];
            }
        }

        protected void cmbBranch_Callback(object sender, CallbackEventArgsBase e)
        {
            string branchid = e.Parameter;
            if(Convert.ToInt32(e.Parameter)>0){
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id not in(select branch_Id from tbl_master_MobileConfiguration where branch_Id!=" + branchid + ")";
            cmbBranch.DataBind();
            cmbBranch.Value = branchid;
            }
            else
            {
                dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id not in(select branch_Id from tbl_master_MobileConfiguration)";
                cmbBranch.DataBind();
                 
            }
            
        }

      

      

    }
}