/***********************************************************************************************************************************
 * Rev 1.0      Sanchita    16/10/2024      0027747: Need to Implement existing SMS sending to Normal link instead of Bitly for GTPL
 * *********************************************************************************************************************************/
using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.ServiceManagement.Transaction.ServiceData
{
    public partial class serviceDataList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        CommonBL ComBL = new CommonBL();
        string userbranchHierachy = null;
        SrvAssignJobBL obj = new SrvAssignJobBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/serviceData/serviceDataList.aspx");
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            //Mantis Issue 25172 
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            hdnDbName.Value = con.Database;
            //End of Mantis Issue 25172
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
                //string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);
                //ddlBranch.SelectedValue = Convert.ToString(Session["userbranchID"]);

                string user_id = Convert.ToString(Session["userid"]);
                DataTable dtusertyp = obj.GetUserType(user_id);
                if (dtusertyp != null && dtusertyp.Rows.Count > 0)
                {
                    hdnUserType.Value = dtusertyp.Rows[0]["contactType"].ToString();
                }
                DataTable dt;
                //if (hdnUserType.Value == "Technician")
                //{
                    //dt = obj.GetTechnicianBind(user_id);
                //}
                //else
                   // {
                //        dt = obj.GetAssignJobDetails(userbranchHierachy);
                //  //  }

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    ddlTechnician.DataSource = dt;
                //    ddlTechnician.DataValueField = "cnt_id";
                //    ddlTechnician.DataTextField = "cnt_firstName";
                //    ddlTechnician.DataBind();
                //    ddlTechnician.SelectedIndex = 0;
                //}
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
        public static SRV_ServiceData TotalSearchJob(srv_SearchFilterInput model)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/serviceData/serviceDataList.aspx");
            SRV_ServiceData ret = new SRV_ServiceData();
            List<SRV_ServiceDataList> listStatues = new List<SRV_ServiceDataList>();
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            if (model.SearchType == "TotalAssigned")
            {
                proc.AddVarcharPara("@ACTION", 500, "SearchTotalReceiveEntryList");
            }
            else if (model.SearchType == "TotalEntered")
            {
                proc.AddVarcharPara("@ACTION", 500, "SearchServiceEnteredReceiveEntryList");
            }
            else if (model.SearchType == "TotalUnassigned")
            {
                proc.AddVarcharPara("@ACTION", 500, "SearchServicePendingReceiveEntryList");
            }
            //rev Pratik
            else if (model.SearchType == "TotalRepairingPending")
            {
                proc.AddVarcharPara("@ACTION", 500, "SearchRepairingPendingReceiveEntryList");
            }
            //End of rev Pratik
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
            if (model.Technician_ID=="0")
            {
                proc.AddPara("@USER_ID",  Convert.ToString(HttpContext.Current.Session["userid"]));
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
                    //rev Pratik
                    ret.RepairPending = item["RepairPending"].ToString();
                    //End of rev Pratik
                    break;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        string assgn_on = "";
                        string DocumentDate = "";
                        string Action = "";
                        String Status = "";                        
                        if (item["STATUS"].ToString() == "DN")
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

                        if (item["ASSIGN_ON"].ToString() != "")
                        {
                            assgn_on = Convert.ToDateTime(item["ASSIGN_ON"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        }

                        if (item["ServEnteredOn"].ToString() != "")
                        {
                            DocumentDate = Convert.ToDateTime(item["ServEnteredOn"].ToString()).ToString("dd-MM-yyyy");
                        }

                        if (item["STATUS"].ToString() == "DN")
                        {
                            if (rights.CanEdit)
                            {
                                Action = Action + " <span class='actionInput text-center' onclick='Edit(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Edit' ></i> </span>";
                            }
                            if (rights.CanView)
                            {
                                Action = Action + " <span class='actionInput text-center' onclick='View(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                            }
                            if (rights.CanDelete)
                            {
                                Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Delete' ></i> </span>";
                            }
                        }

                        if (item["STATUS"].ToString() == "DE")
                        {
                            if (rights.CanView)
                            {
                                Action = Action + " <span class='actionInput text-center' onclick='View(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                            }
                        }

                        if (item["STATUS"].ToString() == "DU")
                        {
                            if (rights.CanAdd)
                            {
                                Action = "<span class='actionInput text-center'><i class='fa fa-plus assig'  onclick='AssignServiceEntry(" + item["ReceiptChallan_ID"].ToString() + ")' data-toggle='tooltip' data-placement='bottom' title='Add Service' ></i> </span>";
                            }
                        }

                        if (rights.CanPrint)
                        {
                            Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                        }

                        listStatues.Add(new SRV_ServiceDataList
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
                            Receivedon = Convert.ToDateTime(item["Create_date"].ToString()).ToString("dd-MM-yyyy"),
                            Assignedby = item["ASSIGN_BY"].ToString(),
                            Assignedon = assgn_on,
                            //rev work start 22.06.2022 mantise no:0024978
                            RepairStatus = item["RepairStatus"].ToString(),
                            Repair_date = item["Repair_date"].ToString(),
                            //rev work closr 22.06.2022 mantise no:0024978
                            ServEnteredBy = item["ServEnteredby"].ToString(),
                            ServEnteredOn = DocumentDate,
                            Status=Status,
                            Action = Action                            
                        });
                    }
                }
                ret.DetailsList = listStatues;
            }
            return ret;
        }

        [WebMethod]
        public static SRV_ServiceData TotalServiceEntry(String model, String SearchType)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/serviceData/serviceDataList.aspx");
            SRV_ServiceData ret = new SRV_ServiceData();
            List<SRV_ServiceDataList> listStatues = new List<SRV_ServiceDataList>();
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            if (SearchType == "TotalAssigned")
            {
                proc.AddVarcharPara("@ACTION", 500, "TotalReceiveEntryList");
            }
            else if (SearchType == "TotalEntered")
            {
                proc.AddVarcharPara("@ACTION", 500, "ServiceEnteredReceiveEntryList");
            }
            else if (SearchType == "TotalUnassigned")
            {
                proc.AddVarcharPara("@ACTION", 500, "ServicePendingReceiveEntryList");
            }
            //Mantis Issue 24665
            else if (SearchType == "TotalRepairingPending")
            {
                proc.AddVarcharPara("@ACTION", 500, "RepairingPendingReceiveEntryList");
            }
            //End of Mantis Issue 24665
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            //if (model == "Technician")
            //{
            //proc.AddPara("@Technician_Id", Convert.ToString(HttpContext.Current.Session["userid"]));
            // }

            ds = proc.GetDataSet();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ret.TOTAL = item["TOTAL"].ToString();
                    ret.Unassigned = item["Unassigned"].ToString();
                    ret.Assigned = item["Assigned"].ToString();
                    //Mantis Issue 24665
                    ret.RepairPending = item["RepairPending"].ToString();
                    //End of Mantis Issue 24665
                    break;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        string assgn_on = "";
                        string DocumentDate = "";
                        string Action = "";
                        if (item["ASSIGN_ON"].ToString() != "")
                        {
                            assgn_on = Convert.ToDateTime(item["ASSIGN_ON"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        }

                        if (item["ServEnteredOn"].ToString() != "")
                        {
                            DocumentDate = Convert.ToDateTime(item["ServEnteredOn"].ToString()).ToString("dd-MM-yyyy");
                        }

                        String Status = "";
                        if (item["STATUS"].ToString() == "DN")
                        {
                            Status = " <span class='badge badge-info'>Ready</span>";
                        }
                        if (item["STATUS"].ToString() == "DU")
                        {
                            // Mantis Issue 25503
                            //Status = " <span class='badge badge-warning'>Assigned</span>";
                            if (SearchType == "TotalRepairingPending")
                            {
                                Status = " <span class='badge badge-warning'>"+ Convert.ToString(item["RepairStatus"])+"</span>";
                            }
                            else
                            {
                                Status = " <span class='badge badge-warning'>Assigned</span>";
                            }
                            // End of Mantis Issue 25503
                        }
                        else if (item["STATUS"].ToString() == "DE")
                        {
                            Status = " <span class='badge badge-success'>Delivered</span>";
                        }


                        if (item["STATUS"].ToString() == "DN")
                        {
                            if (rights.CanEdit)
                            {
                                Action = Action + " <span class='actionInput text-center' onclick='Edit(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Edit' ></i> </span>";
                            }
                            if (rights.CanView)
                            {
                                Action = Action + " <span class='actionInput text-center' onclick='View(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                            }
                            if (rights.CanDelete)
                            {
                                Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Delete' ></i> </span>";
                            }

                           // Action = Action + " <span class='actionInput text-center' onclick='SendSMS(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-envelope' data-toggle='tooltip' data-placement='bottom' title='Send SMS' ></i> </span>";
                        }

                        if (item["STATUS"].ToString() == "DE")
                        {
                            if (rights.CanView)
                            {
                                Action = Action + " <span class='actionInput text-center' onclick='View(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                            }
                        }

                        if (item["STATUS"].ToString() == "DU")
                        {
                            //Mantis Issue 24665
                            //if (rights.CanAdd)
                            //{
                            //    Action = "<span class='actionInput text-center'><i class='fa fa-plus assig'  onclick='AssignServiceEntry(" + item["ReceiptChallan_ID"].ToString() + ")' data-toggle='tooltip' data-placement='bottom' title='Add Service' ></i> </span>";
                            //}
                            if (item["RepairStatus"].ToString() == "Done")
                            {
                                if (rights.CanAdd)
                                {
                                    Action = "<span class='actionInput text-center'><i class='fa fa-plus assig'  onclick='AssignServiceEntry(" + item["ReceiptChallan_ID"].ToString() + ")' data-toggle='tooltip' data-placement='bottom' title='Add Service' ></i> </span>";
                                }
                            }
                            //rev Pratik
                            //else if (item["RepairStatus"].ToString() == "Pending" || item["RepairStatus"].ToString() == "Rejected")
                            else if (item["RepairStatus"].ToString() == "Pending" || item["RepairStatus"].ToString() == "Reject")
                            //End of rev Pratik
                            {
                                // Mantis Issue 25503
                                //if (rights.CanAssignTo)
                                if (rights.CanUnassign)
                                    // End of Mantis Issue 25503
                                {
                                    Action = Action + " <span data-toggle='modal' class='actionInput text-center' onclick='UnAssignJob(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-undo' data-toggle='tooltip' data-placement='bottom' title='Unassign'></i></span>";
                                }
                                if (rights.Verified)
                                {
                                    Action = Action + " <span data-toggle='modal' class='actionInput text-center' onclick='SendTechSMS(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-envelope' data-toggle='tooltip' data-placement='bottom' title='Send SMS' ></i> </span>";
                                }
                                if (rights.CanApproved)
                                {
                                    Action = Action + " <span class='actionInput text-center' onclick='FnAcceptReject(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Done/Reject' ></i> </span>";
                                }
                            }
                            
                        }
                        //Mantis Issue 24665
                        //if (item["STATUS"].ToString() == "DR")
                        //{
                        //    //if (rights.CanEdit)
                        //    //{
                        //    //    Action = Action + " <span class='actionInput text-center' onclick='Edit(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Edit' ></i> </span>";
                        //    //}
                        //    //if (rights.CanView)
                        //    //{
                        //    //    Action = Action + " <span class='actionInput text-center' onclick='View(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                        //    //}
                        //    //if (rights.CanDelete)
                        //    //{
                        //    //    Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Delete' ></i> </span>";
                        //    //}

                        //    Action = Action + " <span class='actionInput text-center' onclick='SendTechSMS(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-envelope' data-toggle='tooltip' data-placement='bottom' title='Send SMS' ></i> </span>";
                        //}
                        //End of Mantis Issue 24665
                        if (rights.CanPrint)
                        {
                            Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                        }


                        listStatues.Add(new SRV_ServiceDataList
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
                            Receivedon = Convert.ToDateTime(item["Create_date"].ToString()).ToString("dd-MM-yyyy"),
                            Assignedby = item["ASSIGN_BY"].ToString(),
                            Assignedon = assgn_on,
                            ServEnteredBy = item["ServEnteredby"].ToString(),
                            ServEnteredOn = DocumentDate,
                            Status=Status,
                            Action = Action,
                            //Mantis Issue 0024840
                            Repair_date = item["Repair_date"].ToString(),
                            RepairStatus = item["RepairStatus"].ToString()
                            //End of Mantis Issue 0024840
                        });
                    }
                }
                ret.DetailsList = listStatues;
            }
            return ret;
        }

        [WebMethod]
        public static SRV_JobCount CountServiceEntry(String UserType)
        {
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            proc.AddVarcharPara("@ACTION", 500, "ServiceEntryCountPageLoad");
            //if (UserType == "Technician")
            //{
           // proc.AddPara("@USER_ID", user_id);
            //}
            //else
            //{
                proc.AddPara("@USER_ID", 0);
            //}
           // proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            ds = proc.GetTable();
            SRV_JobCount ret = new SRV_JobCount();
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow item in ds.Rows)
                {
                    ret.TOTAL = item["TOTAL"].ToString();
                    ret.Assigned = item["Assigned"].ToString();
                    ret.Unassigned = item["Unassigned"].ToString();
                    //rev Pratik
                    ret.RepairPending = item["RepairPending"].ToString();
                    //End of rev Pratik
                    break;
                }
            }
            return ret;
        }

        [WebMethod]
        public static string DeleteServiceEntry(String ReceptID)
        {
            MasterSettings masterbl = new MasterSettings();
            string output = string.Empty;
            try
            {
                string mastersettings = masterbl.GetSettings("StkAdjSrv");
                DataTable dt = new DataTable();

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
                    proc.AddVarcharPara("@ACTION", 500, "ServiceEntryDelete");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceptID));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@StockAdj_Require", mastersettings);
                    dt = proc.GetTable();
                    if (dt!=null && dt.Rows.Count > 0)
                    {
                        output = dt.Rows[0]["msg"].ToString() + "~" + dt.Rows[0]["Status"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        [WebMethod]
        public static string CheckTagInWarranty(String ReceptID)
        {
            string output = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
                    proc.AddVarcharPara("@ACTION", 500, "CheckTagInWarranty");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceptID));
                    proc.AddPara("@USER_ID", user_id);
                    dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        output = "true";
                    }
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

        [WebMethod]
        public static string SendSMS(int ReceiptChallan_ID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanInsertUpdate");
                    proc.AddIntegerPara("@ReceiptChallan_ID", ReceiptChallan_ID);
                    proc.AddVarcharPara("@Action", 500, "SendSMS");
                    NoOfRowEffected = proc.RunActionQuery();

                    if (NoOfRowEffected > 0)
                    {
                        output = "true";
                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
        //Mantis Issue 24665
        [WebMethod]
        public static string SendTechnicianSMS(int ReceiptChallan_ID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                string DataBase = con.Database;

                string baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseUrl"];
                //string baseUrl = "https://3.7.30.86:85";
                // Rev 1.0
                //string LongURL = baseUrl + "/ServiceManagement/Transaction/serviceData/TechnicianAssign.aspx?id=" + ReceiptChallan_ID + "&AU=" + Convert.ToString(0)
                //                    + "&UniqueKey=" + Convert.ToString(DataBase);

                ////string LongURL = "https://stackoverflow.com/questions/366115/using-tinyurl-com-in-a-net-application-possible";
                //string tinyURL = ShortURL(LongURL);

                string LongURL = baseUrl + "/ServiceManagement/Transaction/serviceData/TechnicianAssign.aspx?id=" + ReceiptChallan_ID + "%26AU=" + Convert.ToString(0)
                                    + "%26UniqueKey=" + Convert.ToString(DataBase);
                // End of Rev 1.0

                ProcedureExecute proc1 = new ProcedureExecute("PRC_AssignJobDetails");
                proc1.AddPara("@Action", Convert.ToString("ApprovalSendSMS"));
                // Rev 1.0
                //proc1.AddPara("@tinyURL", Convert.ToString(tinyURL));
                proc1.AddPara("@longURL", Convert.ToString(LongURL));
                // End of Rev 1.0
                proc1.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptChallan_ID));
                proc1.AddPara("@TECHNICIAN_ID", Convert.ToString(0));
                dt = proc1.GetTable();
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
        private static string ShortURL(string LongUrl)
        {
            //System.Uri address = new System.Uri("http://tinyurl.com/api-create.php?url=" + LongUrl);
            //System.Net.WebClient client = new System.Net.WebClient();
            //string tinyUrl = client.DownloadString(address);
            //return tinyUrl;
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
        //End of Mantis Issue 24665

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
    }
}