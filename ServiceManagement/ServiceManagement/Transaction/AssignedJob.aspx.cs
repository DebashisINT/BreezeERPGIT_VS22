/***********************************************************************************************************************************
 * Rev 1.0      Sanchita    16/10/2024      0027747: Need to Implement existing SMS sending to Normal link instead of Bitly for GTPL
 * *********************************************************************************************************************************/
using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogicLayer.ServiceManagement;
using System.Web.Services;
using DataAccessLayer;
using EntityLayer.CommonELS;
using DevExpress.Web;
using System.Configuration;
using System.IO;
using UtilityLayer;
using System.Data.SqlClient;
using System.Net;

namespace ServiceManagement.ServiceManagement.Transaction
{
    public partial class AssignedJob : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        string userbranchHierachy = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SrvAssignJobBL obj = new SrvAssignJobBL();
        CommonBL ComBL = new CommonBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/AssignedJob.aspx");
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            string AllowOnlinePrintinServiceManagement = ComBL.GetSystemSettingsResult("AllowOnlinePrintinServiceManagement");
            hdnOnlinePrint.Value = AllowOnlinePrintinServiceManagement;
            if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
            {
                if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                {
                    userbranchHierachy = EmployeeBranchMap();
                }
                else
                {
                    userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
            }
            else
            {
                userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            }
            Session["UserBranchMapID"] = userbranchHierachy;
            if (!IsPostBack)
            {
                Session["TechnicianData"] = null;
                Session["SI_ComponentData_Branch"] = null;
                // string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);
                // ddlBranch.SelectedValue = Convert.ToString(Session["userbranchID"]);
                string user_id = Convert.ToString(Session["userid"]);
                DataTable dtusertyp = obj.GetUserType(user_id);
                Session["UserType"] = "";
                if (dtusertyp != null && dtusertyp.Rows.Count > 0)
                {
                    // hdnUserType.Value = dtusertyp.Rows[0]["contactType"].ToString();
                    Session["UserType"] = dtusertyp.Rows[0]["contactType"].ToString();
                }


                DataTable dt;
                if (Session["UserType"].ToString() == "Technician")
                {
                    //Session["UserType"] = "Technician";
                    dt = obj.GetTechnicianBind(user_id);
                }
                else
                {
                    Session["UserType"] = "All";
                    dt = obj.GetAssignJobDetails(userbranchHierachy);
                }

                DataTable dtTech = obj.GetAssignJobDetails(userbranchHierachy);


                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    ddlTechnician.DataSource = dt;
                //    ddlTechnician.DataValueField = "cnt_id";
                //    ddlTechnician.DataTextField = "cnt_firstName";
                //    ddlTechnician.DataBind();
                //    ddlTechnician.SelectedIndex = 0;
                //}

                if (dtTech != null && dtTech.Rows.Count > 0)
                {
                    //ddlTechnician.DataSource = dtTech;
                    //ddlTechnician.DataValueField = "cnt_id";
                    //ddlTechnician.DataTextField = "cnt_firstName";
                    //ddlTechnician.DataBind();
                    //ddlTechnician.SelectedIndex = 0;

                    //ddlAssignTechnician.DataSource = dtTech;
                    //ddlAssignTechnician.DataValueField = "cnt_id";
                    //ddlAssignTechnician.DataTextField = "cnt_firstName";
                    //ddlAssignTechnician.DataBind();
                    //ddlAssignTechnician.SelectedIndex = 0;

                    ddlTechnicianMass.DataSource = dtTech;
                    ddlTechnicianMass.DataValueField = "cnt_id";
                    ddlTechnicianMass.DataTextField = "cnt_firstName";
                    ddlTechnicianMass.DataBind();
                    ddlTechnicianMass.SelectedIndex = 0;
                }
            }
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            //PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            //DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            //ddlBranch.DataSource = branchtable;
            //ddlBranch.DataValueField = "branch_id";
            //ddlBranch.DataTextField = "branch_description";
            //ddlBranch.DataBind();
            //ddlBranch.SelectedIndex = 0;
        }

        [WebMethod]
        public static List<SRV_MassJobAssign> bindMassAssign(string ReceiptType)
        {

            List<SRV_MassJobAssign> listStatues = new List<SRV_MassJobAssign>();

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
            proc.AddVarcharPara("@ACTION", 500, "MassJobAssign");
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            ds = proc.GetTable();
            foreach (DataRow item in ds.Rows)
            {
                listStatues.Add(new SRV_MassJobAssign
                {
                    ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                    EntryType = item["EntryType"].ToString(),
                    DocumentNumber = item["DocumentNumber"].ToString(),
                    DocumentDate = Convert.ToDateTime(item["DocumentDate"].ToString()).ToString("dd-MM-yyyy"),
                    EntityCode = item["EntityCode"].ToString(),
                    NetworkName = item["NetworkName"].ToString(),
                    ContactPerson = item["ContactPerson"].ToString()
                });
            }

            return listStatues;
        }

        [WebMethod]
        public static SRV_AssignMyJobData bindTotalJobs(string Type)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/AssignedJob.aspx");
            List<SRV_TotalJobsList> listStatues = new List<SRV_TotalJobsList>();
            SRV_AssignMyJobData ret = new SRV_AssignMyJobData();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                if (Type == "TotalJob")
                {
                    proc.AddVarcharPara("@ACTION", 500, "TotalJobs");
                }
                else
                {
                    proc.AddVarcharPara("@ACTION", 500, "AssignedJobs");
                }
                //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
                ds = proc.GetDataSet();

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.TOTAL = item["TOTAL"].ToString();
                        ret.Unassigned = item["Unassigned"].ToString();
                        ret.Assigned = item["Assigned"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            string assgn_on = "";
                            string Action = "";
                            if (item["ASSIGN_ON"].ToString() != "")
                            {
                                assgn_on = Convert.ToDateTime(item["ASSIGN_ON"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            }
                            String Status = "";
                            //if (item["STATUS"].ToString() == "P")
                            //{
                            //    Status = " <span class='badge badge-warning'>Pending</span>";
                            //}
                            //else if (item["STATUS"].ToString() == "DU")
                            //{
                            //    Status = " <span class='badge badge-danger'>Due</span>";
                            //}
                            //else if (item["STATUS"].ToString() == "DN")
                            //{
                            //    Status = "<span class='badge badge-success'>Done</span>";
                            //}
                            //else
                            //{
                            //    Status = "";
                            //}


                            if (item["STATUS"].ToString() == "P")
                            {
                                Status = " <span class='badge badge-danger'>Unassigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DN")
                            {
                                Status = " <span class='badge badge-info'>Ready</span>";
                            }
                            if (item["STATUS"].ToString() == "DU")
                            {
                                Status = " <span class='badge badge-warning'>Assigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DE")
                            {
                                Status = " <span class='badge badge-success'>Delivered</span>";
                            }

                            //if (rights.CanAssignTo)
                            //{
                            if (Type == "TotalJob")
                            {
                                if (item["STATUS"].ToString() == "P")
                                {
                                    if (rights.CanAssignTo)
                                    {
                                        Action = " <span data-toggle='modal' class='actionInput' data-target='#assignpop' onclick='AssignJob(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-user-plus assig' data-toggle='tooltip' data-placement='left' title='Assign'></i></span>";
                                    }
                                }
                            }
                            if (item["STATUS"].ToString() == "DU")
                            {
                                // Mantis Issue 25503
                                //if (rights.CanAssignTo)
                                if (rights.CanUnassign)
                                    // End of Mantis Issue 25503
                                {
                                    Action = Action + " <span data-toggle='modal' class='actionInput' onclick='UnAssignJob(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-undo' data-toggle='tooltip' data-placement='left' title='Unassign'></i></span>";
                                }
                                //rev Pratik
                                //if (rights.CanAdd)
                                //{ 
                                //    Action = Action + " <span class='actionInput' onclick='ServiceEntry(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-plus assig' data-toggle='tooltip' data-placement='left' title='Service Entry'></i></span>";
                                //}
                                //End of rev Pratik
                            }
                            //  }

                            if (rights.CanView)
                            {
                                Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                            }
                            if (rights.CanPrint)
                            {
                                if (item["STATUS"].ToString() != "P")
                                {
                                    //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                    Action = Action + " <span class='actionInput' onclick='onPrintJv(" + item["ReceiptChallan_ID"].ToString() + ")' text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                }
                            }

                            listStatues.Add(new SRV_TotalJobsList
                            {
                                ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                                ReceiptChallan = item["DocumentNumber"].ToString(),
                                Type = item["EntryType"].ToString(),
                                EntityCode = item["EntityCode"].ToString(),
                                NetworkName = item["NetworkName"].ToString(),
                                ContactPerson = item["ContactPerson"].ToString(),
                                Technician = item["Technician"].ToString(),
                                Location = item["branch_description"].ToString(),
                                Receivedby = item["CREATE_BY"].ToString(),
                                Receivedon = Convert.ToDateTime(item["Create_date"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                                Assignedby = item["ASSIGN_BY"].ToString(),
                                Assignedon = assgn_on,
                                Status = Status,
                                Action = Action

                            });
                        }
                    }
                    ret.DetailsList = listStatues;
                }
            }
            return ret;
        }

        [WebMethod]
        public static string save(SRV_MassInsert model)
        {
            string output = string.Empty;
            try
            {
                string RecptID = "";
                int NoOfRowEffected = 0;

                int k = 1;

                if (model.RecptID != null && model.RecptID.Count > 0)
                {
                    foreach (string item in model.RecptID)
                    {
                        if (k > 1)
                            RecptID = RecptID + "," + item;
                        else
                            RecptID = item;
                        k++;
                    }
                }

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                    proc.AddVarcharPara("@Action", 500, "MassInsert");
                    proc.AddPara("@RcptId", Convert.ToString(RecptID));
                    proc.AddVarcharPara("@Technician_Id", 100, Convert.ToString(model.Technician_ID));
                    proc.AddPara("@USER_ID", user_id);
                    NoOfRowEffected = proc.RunActionQuery();
                    if (NoOfRowEffected > 0)
                    {

                    }
                    output = "true";
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        [WebMethod]
        public static SRV_JobCount CountJob(String UserType)
        {
            SRV_JobCount ret = new SRV_JobCount();
            if (HttpContext.Current.Session["userid"] != null)
            {
                int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                proc.AddVarcharPara("@ACTION", 500, "JobAssignCount");
                if (UserType == "Technician")
                {
                    proc.AddPara("@USER_ID", user_id);
                }
                else
                {
                    proc.AddPara("@USER_ID", 0);
                }
                //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
                ds = proc.GetTable();

                if (ds != null && ds.Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Rows)
                    {
                        ret.TOTAL = item["TOTAL"].ToString();
                        ret.Assigned = item["Assigned"].ToString();
                        ret.Unassigned = item["Unassigned"].ToString();
                        break;
                    }
                }
            }
            return ret;
        }

        [WebMethod]
        public static SRV_UnAssignJobData BindUnassignedJob(string Type)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/AssignedJob.aspx");
            List<SRV_UnassignedJobsList> listStatues = new List<SRV_UnassignedJobsList>();
            SRV_UnAssignJobData ret = new SRV_UnAssignJobData();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                proc.AddVarcharPara("@ACTION", 500, "UnAssignedJobs");
                // proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
                ds = proc.GetDataSet();

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.TOTAL = item["TOTAL"].ToString();
                        ret.Unassigned = item["Unassigned"].ToString();
                        ret.Assigned = item["Assigned"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            string Action = "";
                            String Status = "";
                            if (item["STATUS"].ToString() == "P")
                            {
                                Status = " <span class='badge badge-danger'>Unassigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DN")
                            {
                                Status = " <span class='badge badge-info'>Ready</span>";
                            }
                            if (item["STATUS"].ToString() == "DU")
                            {
                                Status = " <span class='badge badge-warning'>Assigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DE")
                            {
                                Status = " <span class='badge badge-success'>Delivered</span>";
                            }

                            if (rights.CanAssignTo)
                            {
                                Action = " <span data-toggle='modal' class='actionInput' data-target='#assignpop' onclick='AssignJob(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-user-plus assig' data-toggle='tooltip' data-placement='left' title='Assign'></i></span>";
                            }

                            if (rights.CanView)
                            {
                                Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                            }
                            if (rights.CanPrint)
                            {
                                if (item["STATUS"].ToString() != "P")
                                {
                                    //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                    Action = Action + " <span class='actionInput' onclick='onPrintJv(" + item["ReceiptChallan_ID"].ToString() + ")' text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                }
                            }

                            listStatues.Add(new SRV_UnassignedJobsList
                            {
                                ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                                ReceiptChallan = item["DocumentNumber"].ToString(),
                                Type = item["EntryType"].ToString(),
                                EntityCode = item["EntityCode"].ToString(),
                                NetworkName = item["NetworkName"].ToString(),
                                ContactPerson = item["ContactPerson"].ToString(),
                                Location = item["branch_description"].ToString(),
                                Receivedby = item["CREATE_BY"].ToString(),
                                Receivedon = Convert.ToDateTime(item["Create_date"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                                Status = Status,
                                Action = Action

                            });
                        }
                    }
                    ret.DetailsList = listStatues;
                }
            }
            return ret;
        }

        [WebMethod]
        public static srv_ReceptChallanDtls ReceptDetails(String ReceiptID)
        {
            srv_ReceptChallanDtls ret = new srv_ReceptChallanDtls();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                proc.AddVarcharPara("@ACTION", 500, "ReceiptChallanDetails");
                proc.AddPara("@ReceiptChallan_ID", ReceiptID);
                ds = proc.GetDataSet();

                List<srv_ReceptChallanDtlsList> DetailsList = new List<srv_ReceptChallanDtlsList>();
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.ReceivedBy = item["CREATE_BY"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        int k = 0;
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            k = k + 1;
                            DetailsList.Add(new srv_ReceptChallanDtlsList
                            {
                                SLNO = Convert.ToString(k),
                                DeviceNumber = item["DeviceNumber"].ToString(),
                                ModelNumber = item["Model"].ToString(),
                                Problem = item["ProblemDesc"].ToString(),
                                CordAdaptor = item["CardAdaptor_STATUS"].ToString(),
                                Remote = item["REMOTE_STATUS"].ToString(),
                                Others = ""
                            });
                        }
                    }
                    ret.DetailsList = DetailsList;

                }
            }
            return ret;
        }

        [WebMethod]
        public static SRV_AssignMyJobData BindMyJobs(string Type)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/AssignedJob.aspx");
            List<SRV_TotalJobsList> listStatues = new List<SRV_TotalJobsList>();
            SRV_AssignMyJobData ret = new SRV_AssignMyJobData();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                proc.AddVarcharPara("@ACTION", 500, "MyJobs");
                proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
                ds = proc.GetDataSet();

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.TOTAL = item["TOTAL"].ToString();
                        ret.Unassigned = item["Unassigned"].ToString();
                        ret.Assigned = item["Assigned"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            string assgn_on = "";
                            String Status = "";
                            String Action = "";
                            if (item["ASSIGN_ON"].ToString() != "")
                            {
                                assgn_on = Convert.ToDateTime(item["ASSIGN_ON"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            }

                            if (item["STATUS"].ToString() == "P")
                            {
                                Status = " <span class='badge badge-danger'>Unassigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DN")
                            {
                                Status = " <span class='badge badge-info'>Ready</span>";
                            }
                            if (item["STATUS"].ToString() == "DU")
                            {
                                Status = " <span class='badge badge-warning'>Assigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DE")
                            {
                                Status = " <span class='badge badge-success'>Delivered</span>";
                            }

                            if (Convert.ToString(HttpContext.Current.Session["UserType"]) == "Technician")
                            {
                                if (item["STATUS"].ToString() != "DN")
                                {
                                    if (rights.CanAdd)
                                    {
                                        Action = " <span class='actionInput' onclick='ServiceEntry(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-plus assig' data-toggle='tooltip' data-placement='left' title='Service Entry'></i></span>";
                                    }
                                }
                            }
                            if (rights.CanView)
                            {
                                Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                            }
                            if (rights.CanPrint)
                            {
                                if (item["STATUS"].ToString() != "P")
                                {
                                    //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                    Action = Action + " <span class='actionInput' onclick='onPrintJv(" + item["ReceiptChallan_ID"].ToString() + ")' text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                }
                            }
                            listStatues.Add(new SRV_TotalJobsList
                            {
                                ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                                ReceiptChallan = item["DocumentNumber"].ToString(),
                                Type = item["EntryType"].ToString(),
                                EntityCode = item["EntityCode"].ToString(),
                                NetworkName = item["NetworkName"].ToString(),
                                ContactPerson = item["ContactPerson"].ToString(),
                                Technician = item["Technician"].ToString(),
                                Location = item["branch_description"].ToString(),
                                Receivedby = item["CREATE_BY"].ToString(),
                                Receivedon = Convert.ToDateTime(item["Create_date"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                                Assignedby = item["ASSIGN_BY"].ToString(),
                                Assignedon = assgn_on,
                                Status = Status,
                                Action = Action
                            });
                        }
                    }
                    ret.DetailsList = listStatues;
                }
            }
            return ret;
        }

        [WebMethod]
        public static string SingleAssign(String Technician_ID, String ReceiptChallan_ID, String Remarks)
        {
            string output = string.Empty;
            try
            {
                DataTable dt = new DataTable();

                if (HttpContext.Current.Session["userid"] != null)
                {
                    //Mantis Issue 24675
                    string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    //string strBranchID = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                    string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                    //End of Mantis Issue 24675
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                    proc.AddVarcharPara("@Action", 500, "SingleJobInsert");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptChallan_ID));
                    proc.AddPara("@Remarks", Convert.ToString(Remarks));
                    proc.AddVarcharPara("@Technician_Id", 100, Convert.ToString(Technician_ID));
                    proc.AddPara("@USER_ID", user_id);
                    //Mantis Issue 24675
                    proc.AddPara("@CompanyID", Convert.ToString(strCompanyID));
                    proc.AddPara("@FinYear", Convert.ToString(FinYear));
                    //End of Mantis Issue 24675
                    dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        output = dt.Rows[0]["msg"].ToString() + "~" + dt.Rows[0]["statuss"].ToString() + "~" + dt.Rows[0]["ID"].ToString();
                        //Mantis Issue 24664
                        
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                        string DataBase = con.Database;

                        string baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseUrl"];
                        //string baseUrl = "https://3.7.30.86:85";
                        //rev Pratik
                        //string LongURL = baseUrl + "/ServiceManagement/Transaction/serviceData/TechnicianAssign.aspx?id=" + dt.Rows[0]["ID"].ToString() + "&AU=" + Convert.ToString(Technician_ID)
                        //                    + "&UniqueKey=" + Convert.ToString(DataBase);
                        
                        string LongURL = baseUrl + "/ServiceManagement/Transaction/serviceData/TechnicianAssign.aspx?id=" + dt.Rows[0]["ID"].ToString() + "&AU=" + Convert.ToString(Technician_ID)
                                            + "&UniqueKey=" + Convert.ToString(DataBase) + "&user_id=" + Convert.ToString(user_id);
                        //End of rev Pratik

                        //string LongURL = "https://stackoverflow.com/questions/366115/using-tinyurl-com-in-a-net-application-possible";

                        // Rev 1.0
                        //string tinyURL = ShortURL(LongURL);
                        // End of Rev 1.0

                        ProcedureExecute proc1 = new ProcedureExecute("PRC_AssignJobDetails");
                        proc1.AddPara("@Action", Convert.ToString("ApprovalSendSMS"));
                        // Rev 1.0
                        //proc1.AddPara("@tinyURL", Convert.ToString(tinyURL));
                        proc1.AddPara("@longURL", Convert.ToString(LongURL));
                        proc1.AddPara("@baseUrl", Convert.ToString(baseUrl));
                        proc1.AddPara("@DataBase", Convert.ToString(DataBase));
                        // End of Rev 1.0
                        //Mantis Issue 24897
                        //proc1.AddPara("@DocumentNumber", Convert.ToString(ReceiptChallan_ID));
                        proc1.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptChallan_ID));
                        //End of Mantis Issue 24897
                        proc1.AddPara("@TECHNICIAN_ID", Convert.ToString(Technician_ID));
                        dt = proc1.GetTable();
                       
                        //End of Mantis Issue 24664
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
        //Mantis Issue 24664
        private static string ShortURL(string LongUrl)
        {
            //System.Uri address = new System.Uri("http://tinyurl.com/api-create.php?url=" + LongUrl);
            //System.Net.WebClient client = new System.Net.WebClient();
            //string tinyUrl = client.DownloadString(address);
            //return tinyUrl;

            //string URL;
            //URL = "http://tinyurl.com/api-create.php?url=" +
            //   LongUrl.ToLower();

            //System.Net.HttpWebRequest objWebRequest;
            //System.Net.HttpWebResponse objWebResponse;

            //System.IO.StreamReader srReader;

            //string strHTML;

            //objWebRequest = (System.Net.HttpWebRequest)System.Net
            //   .WebRequest.Create(URL);
            //objWebRequest.Method = "GET";

            //objWebResponse = (System.Net.HttpWebResponse)objWebRequest
            //   .GetResponse();
            //srReader = new System.IO.StreamReader(objWebResponse
            //   .GetResponseStream());

            //strHTML = srReader.ReadToEnd();

            //srReader.Close();
            //objWebResponse.Close();
            //objWebRequest.Abort();

            //return (strHTML);

            try
            {
                if (LongUrl.Length <= 30)
                {
                    return LongUrl;
                }
                if (!LongUrl.ToLower().StartsWith("http") && !LongUrl.ToLower().StartsWith("ftp"))
                {
                    LongUrl = "http://" + LongUrl;
                }
                var request = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + LongUrl);
                var res = request.GetResponse();
                string text;
                using (var reader = new StreamReader(res.GetResponseStream()))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception)
            {
                return LongUrl;
            }
        }
        //End of Mantis Issue 24664

        [WebMethod]
        public static SRV_AssignMyJobData SearchDataMyJob(srv_SearchFilterInput model)
        {
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/AssignedJob.aspx");
            List<SRV_TotalJobsList> listStatues = new List<SRV_TotalJobsList>();
            SRV_AssignMyJobData ret = new SRV_AssignMyJobData();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                proc.AddVarcharPara("@ACTION", 500, "SearchMyJobs");
                proc.AddPara("@FromDate", model.FromDate);
                proc.AddPara("@ToDate", model.ToDate);
                if (model.Branch == "")
                {
                    // proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                    proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
                }
                else
                {
                    proc.AddPara("@BranchID", model.Branch);
                }
                if (model.Technician_ID == "")
                {
                    proc.AddPara("@USER_ID", user_id);
                }
                else
                {
                    proc.AddPara("@Technician_Id", model.Technician_ID);
                }

                ds = proc.GetDataSet();

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.TOTAL = item["TOTAL"].ToString();
                        ret.Unassigned = item["Unassigned"].ToString();
                        ret.Assigned = item["Assigned"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            string assgn_on = "";
                            String Status = "";
                            String Action = "";
                            if (item["ASSIGN_ON"].ToString() != "")
                            {
                                assgn_on = Convert.ToDateTime(item["ASSIGN_ON"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            }

                            if (item["STATUS"].ToString() == "P")
                            {
                                Status = " <span class='badge badge-danger'>Unassigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DN")
                            {
                                Status = " <span class='badge badge-info'>Ready</span>";
                            }
                            if (item["STATUS"].ToString() == "DU")
                            {
                                Status = " <span class='badge badge-warning'>Assigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DE")
                            {
                                Status = " <span class='badge badge-success'>Delivered</span>";
                            }

                            if (Convert.ToString(HttpContext.Current.Session["UserType"]) == "Technician")
                            {
                                if (item["STATUS"].ToString() != "DN")
                                {
                                    if (rights.CanAdd)
                                    {
                                        Action = " <span class='actionInput' onclick='ServiceEntry(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-plus assig' data-toggle='tooltip' data-placement='left' title='Service Entry'></i></span>";
                                    }
                                }
                            }
                            if (rights.CanView)
                            {
                                Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                            }
                            if (rights.CanPrint)
                            {
                                if (item["STATUS"].ToString() != "P")
                                {
                                    //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                    Action = Action + " <span class='actionInput' onclick='onPrintJv(" + item["ReceiptChallan_ID"].ToString() + ")' text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                }
                            }
                            listStatues.Add(new SRV_TotalJobsList
                            {
                                ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                                ReceiptChallan = item["DocumentNumber"].ToString(),
                                Type = item["EntryType"].ToString(),
                                EntityCode = item["EntityCode"].ToString(),
                                NetworkName = item["NetworkName"].ToString(),
                                ContactPerson = item["ContactPerson"].ToString(),
                                Technician = item["Technician"].ToString(),
                                Location = item["branch_description"].ToString(),
                                Receivedby = item["CREATE_BY"].ToString(),
                                Receivedon = Convert.ToDateTime(item["Create_date"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                                Assignedby = item["ASSIGN_BY"].ToString(),
                                Assignedon = assgn_on,
                                Status = Status,
                                Action = Action
                            });
                        }

                    }
                    ret.DetailsList = listStatues;
                }
            }
            return ret;
        }

        [WebMethod]
        public static SRV_AssignMyJobData TotalSearchJob(srv_SearchFilterInput model)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/AssignedJob.aspx");

            List<SRV_TotalJobsList> listStatues = new List<SRV_TotalJobsList>();
            SRV_AssignMyJobData ret = new SRV_AssignMyJobData();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                if (model.SearchType == "TotalJob")
                {
                    proc.AddVarcharPara("@ACTION", 500, "TotalSearchJobs");
                }
                else
                {
                    proc.AddVarcharPara("@ACTION", 500, "AssignedSearchJobs");
                }
                proc.AddPara("@FromDate", model.FromDate);
                proc.AddPara("@ToDate", model.ToDate);
                if (model.Branch == "0")
                {
                    //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                    proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
                }
                else
                {
                    proc.AddPara("@BranchID", model.Branch);
                }
                proc.AddPara("@Technician_Id", model.Technician_ID);

                ds = proc.GetDataSet();

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.TOTAL = item["TOTAL"].ToString();
                        ret.Unassigned = item["Unassigned"].ToString();
                        ret.Assigned = item["Assigned"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            string assgn_on = "";
                            string Action = "";
                            if (item["ASSIGN_ON"].ToString() != "")
                            {
                                assgn_on = Convert.ToDateTime(item["ASSIGN_ON"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                            }
                            String Status = "";
                            if (item["STATUS"].ToString() == "P")
                            {
                                Status = " <span class='badge badge-danger'>Unassigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DN")
                            {
                                Status = " <span class='badge badge-info'>Ready</span>";
                            }
                            if (item["STATUS"].ToString() == "DU")
                            {
                                Status = " <span class='badge badge-warning'>Assigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DE")
                            {
                                Status = " <span class='badge badge-success'>Delivered</span>";
                            }

                            //if (rights.CanAssignTo)
                            //{
                            if (model.SearchType == "TotalJob")
                            {
                                if (item["STATUS"].ToString() == "P")
                                {
                                    if (rights.CanAssignTo)
                                    {
                                        Action = " <span data-toggle='modal' class='actionInput' data-target='#assignpop' onclick='AssignJob(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-user-plus assig' data-toggle='tooltip' data-placement='left' title='Assign'></i></span>";
                                    }
                                }
                            }
                            if (item["STATUS"].ToString() == "DU")
                            {
                                // Mantis Issue 25503
                                //if (rights.CanAssignTo)
                                if (rights.CanUnassign)
                                    // End of Mantis Issue 25503
                                {
                                    Action = Action + " <span data-toggle='modal' class='actionInput' onclick='UnAssignJob(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-undo' data-toggle='tooltip' data-placement='left' title='Unassign'></i></span>";
                                }
                                if (rights.CanAdd)
                                {
                                    Action = Action + " <span class='actionInput' onclick='ServiceEntry(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-plus assig' data-toggle='tooltip' data-placement='left' title='Service Entry'></i></span>";
                                }
                            }
                            //}

                            if (rights.CanView)
                            {
                                Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                            }
                            if (rights.CanPrint)
                            {
                                if (item["STATUS"].ToString() != "P")
                                {
                                    //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                    Action = Action + " <span class='actionInput' onclick='onPrintJv(" + item["ReceiptChallan_ID"].ToString() + ")' text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                }
                            }

                            listStatues.Add(new SRV_TotalJobsList
                            {
                                ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                                ReceiptChallan = item["DocumentNumber"].ToString(),
                                Type = item["EntryType"].ToString(),
                                EntityCode = item["EntityCode"].ToString(),
                                NetworkName = item["NetworkName"].ToString(),
                                ContactPerson = item["ContactPerson"].ToString(),
                                Technician = item["Technician"].ToString(),
                                Location = item["branch_description"].ToString(),
                                Receivedby = item["CREATE_BY"].ToString(),
                                Receivedon = Convert.ToDateTime(item["Create_date"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                                Assignedby = item["ASSIGN_BY"].ToString(),
                                Assignedon = assgn_on,
                                Status = Status,
                                Action = Action

                            });
                        }
                    }
                    ret.DetailsList = listStatues;
                }
            }
            return ret;
        }

        [WebMethod]
        public static SRV_UnAssignJobData BindUnassignedSearchJob(srv_SearchFilterInput model)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/AssignedJob.aspx");
            List<SRV_UnassignedJobsList> listStatues = new List<SRV_UnassignedJobsList>();
            SRV_UnAssignJobData ret = new SRV_UnAssignJobData();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                proc.AddVarcharPara("@ACTION", 500, "UnAssignedSearchJobs");
                proc.AddPara("@FromDate", model.FromDate);
                proc.AddPara("@ToDate", model.ToDate);
                if (model.Branch == "")
                {
                    //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                    proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
                }
                else
                {
                    proc.AddPara("@BranchID", model.Branch);
                }
                ds = proc.GetDataSet();

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.TOTAL = item["TOTAL"].ToString();
                        ret.Unassigned = item["Unassigned"].ToString();
                        ret.Assigned = item["Assigned"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            string Action = "";
                            String Status = "";
                            if (item["STATUS"].ToString() == "P")
                            {
                                Status = " <span class='badge badge-danger'>Unassigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DN")
                            {
                                Status = " <span class='badge badge-info'>Ready</span>";
                            }
                            if (item["STATUS"].ToString() == "DU")
                            {
                                Status = " <span class='badge badge-warning'>Assigned</span>";
                            }
                            else if (item["STATUS"].ToString() == "DE")
                            {
                                Status = " <span class='badge badge-success'>Delivered</span>";
                            }

                            if (rights.CanAssignTo)
                            {
                                Action = " <span data-toggle='modal' class='actionInput' data-target='#assignpop' onclick='AssignJob(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-user-plus assig' data-toggle='tooltip' data-placement='left' title='Assign'></i></span>";
                            }

                            if (rights.CanView)
                            {
                                Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                            }
                            if (rights.CanPrint)
                            {
                                if (item["STATUS"].ToString() != "P")
                                {
                                    //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                    Action = Action + " <span class='actionInput' onclick='onPrintJv(" + item["ReceiptChallan_ID"].ToString() + ")' text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                                }
                            }

                            listStatues.Add(new SRV_UnassignedJobsList
                            {
                                ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                                ReceiptChallan = item["DocumentNumber"].ToString(),
                                Type = item["EntryType"].ToString(),
                                EntityCode = item["EntityCode"].ToString(),
                                NetworkName = item["NetworkName"].ToString(),
                                ContactPerson = item["ContactPerson"].ToString(),
                                Location = item["branch_description"].ToString(),
                                Receivedby = item["CREATE_BY"].ToString(),
                                Receivedon = Convert.ToDateTime(item["Create_date"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                                Status = Status,
                                Action = Action

                            });
                        }
                    }
                    ret.DetailsList = listStatues;
                }
            }

            return ret;
        }

        [WebMethod]
        public static string UnAssign(String ReceiptChallan_ID)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                    proc.AddVarcharPara("@Action", 500, "JobUnAssign");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptChallan_ID));
                    proc.AddPara("@USER_ID", user_id);
                    NoOfRowEffected = proc.RunActionQuery();
                    if (NoOfRowEffected > 0)
                    {
                        output = "true";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        public String EmployeeBranchMap()
        {
            String branches = null;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMap");
            proc.AddVarcharPara("@USER_ID", 100, Session["userid"].ToString());
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\AssignJob\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\AssignJob\DocDesign\Designes";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    CmbDesignName.Items.Add(name, reportValue);
                }
                CmbDesignName.SelectedIndex = 0;
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Session["userbranchHierarchy"] != null)
                {
                    // ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ") order by branch_description asc");
                }
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
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }
        #endregion

        #region Technician Populate

        protected void Technician_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindTechnicianGrid")
            {
                DataTable TechnicianTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                TechnicianTable = oDBEngine.GetDataTable("select distinct CNT.cnt_internalId,CNT.cnt_firstName from tbl_master_contact CNT INNER JOIN Srv_master_TechnicianBranch_map MAP ON MAP.Tech_InternalId=CNT.cnt_internalId WHERE MAP.branch_id IN (" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ")  AND CNT.cnt_contactType='TM' AND CNT.Is_Active=1");

                if (TechnicianTable.Rows.Count > 0)
                {
                    Session["TechnicianData"] = TechnicianTable;
                    lookup_Technician.DataSource = TechnicianTable;
                    lookup_Technician.DataBind();
                }
                else
                {
                    Session["TechnicianData"] = TechnicianTable;
                    lookup_Technician.DataSource = null;
                    lookup_Technician.DataBind();
                }
            }
        }

        protected void lookup_Technician_DataBinding(object sender, EventArgs e)
        {
            if (Session["TechnicianData"] != null)
            {
                lookup_Technician.DataSource = (DataTable)Session["TechnicianData"];
            }
        }

        #endregion

        [WebMethod]
        public static ServiceRegisterReports SingleAssignTechnician(String ChallanID)
        {

            ServiceRegisterReports reports = new ServiceRegisterReports();
            List<Tecchnician> TecchnicianList = new List<Tecchnician>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
            proc.AddVarcharPara("@Action", 500, "SingleAssignTechnician");
            proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ChallanID));
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                TecchnicianList = APIHelperMethods.ToModelList<Tecchnician>(dt);
                reports.TecchnicianList = TecchnicianList;
            }
            return reports;
        }
    }
}