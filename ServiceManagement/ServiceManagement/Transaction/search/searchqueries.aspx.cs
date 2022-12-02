using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ServiceManagement.ServiceManagement.Transaction.search
{
    public partial class searchqueries : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        string userbranchHierachy = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SrvAssignJobBL obj = new SrvAssignJobBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/search/searchqueries.aspx");
            if (!IsPostBack)
            {
              //  string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);
               // ddlBranch.SelectedValue = Convert.ToString(Session["userbranchID"]);
                string user_id = Convert.ToString(Session["userid"]);
                DataTable dtusertyp = obj.GetUserType(user_id);
                if (dtusertyp != null && dtusertyp.Rows.Count > 0)
                {
                    hdnUserType.Value = dtusertyp.Rows[0]["contactType"].ToString();
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
        public static List<Srv_DeliveryList> SearchList(String ReceiptchallanNo, String EntityCode, String Branch)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/search/searchqueries.aspx");
            List<Srv_DeliveryList> listStatues = new List<Srv_DeliveryList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            proc.AddVarcharPara("@ACTION", 500, "SearchModule");
            proc.AddPara("@ReceiptchallanNo", ReceiptchallanNo);
            proc.AddPara("@EntityCode", EntityCode);
            if (Branch == "0")
            {
               // proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                // Mantis Issue 24265
                //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
                // End of Mantis Issue 24265
            }
            else
            {
                proc.AddPara("@BranchID", Branch);
            }
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {

                listStatues = (from DataRow item in ds.Rows
                               select new Srv_DeliveryList()
                          {
                              ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                              DocumentDate = item["DocumentDate"].ToString(),
                              ReceiptChallan = item["DocumentNumber"].ToString(),
                              Type = item["EntryType"].ToString(),
                              EntityCode = item["EntityCode"].ToString(),
                              NetworkName = item["NetworkName"].ToString(),
                              ContactPerson = item["ContactPerson"].ToString(),
                              Technician = item["Technician"].ToString(),
                              Location = item["branch_description"].ToString(),
                              Receivedby = item["Receivedby"].ToString(),
                              Receivedon = item["Receivedon"].ToString(),
                              Assignedby = item["Assignedby"].ToString(),
                              Assignedon = item["Assignedon"].ToString(),
                              ServEnteredBy = item["ServEnteredBy"].ToString(),
                              ServEnteredOn = item["ServEnteredOn"].ToString(),
                              DeliveryBy = item["Deliveryby"].ToString(),
                              DeliveryOn = item["DeliveryOn"].ToString(),
                              Status = item["Status"].ToString(),
                              Action = item["Action"].ToString(),
                              Warranty = item["Warranty"].ToString(),
                              ConfirmDelivery = item["ConfirmDelivery"].ToString(),
                              ConfirmDeliveryDate = item["ConfirmDeliveryDate"].ToString()
                          }).ToList();


                //foreach (DataRow item in ds.Rows)
                //{
                //    //String Assignedon = "";
                //    //string DeliveryOn = "";
                //    string Action = "";
                //    //String ServEnteredOn = "";
                //    //if (item["DeliveryOn"].ToString() != "")
                //    //{
                //    //    DeliveryOn = Convert.ToDateTime(item["DeliveryOn"].ToString()).ToString("dd-MM-yyyy");
                //    //}
                //    //if (item["Assignedon"].ToString() != "")
                //    //{
                //    //    Assignedon = Convert.ToDateTime(item["Assignedon"].ToString()).ToString("dd-MM-yyyy");
                //    //}
                //    //if (item["ServEnteredOn"].ToString() != "")
                //    //{
                //    //    ServEnteredOn = Convert.ToDateTime(item["ServEnteredOn"].ToString()).ToString("dd-MM-yyyy");
                //    //}
                //    //String Status = "";
                //    //if (item["Statuss"].ToString() == "P")
                //    //{
                //    //    Status = " <span class='badge badge-danger'>Unassigned</span>";
                //    //}
                //    //else if (item["Statuss"].ToString() == "DN")
                //    //{
                //    //    Status = " <span class='badge badge-info'>Ready</span>";
                //    //}
                //    //if (item["Statuss"].ToString() == "DU")
                //    //{
                //    //    Status = " <span class='badge badge-warning'>Assigned</span>";
                //    //}
                //    //else if (item["Statuss"].ToString() == "DE")
                //    //{
                //    //    Status = " <span class='badge badge-success'>Delivered</span>";
                //    //}


                //    //if (rights.CanAdd)
                //    //{
                //    //    if (item["Statuss"].ToString() != "DE")
                //    //    {
                //    //        Action = " <span class='actionInput' onclick='AddDelivery(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='left' title='Add Delivery'></i></span>";
                //    //    }
                //    //}
                //    //if (item["Statuss"].ToString() == "DE")
                //    //{


                //    //    //if (rights.CanDelete)
                //    //    //{
                //    //    //    Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                //    //    //}
                //    //}
                //    if (rights.CanPrint)
                //    {
                //        Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                //    }

                //    if (rights.CanView)
                //    {
                //        Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ",'" + item["EntryType"].ToString() + "')'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                //    }

                //    listStatues.Add(new Srv_DeliveryList
                //    {
                //        ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                //        ReceiptChallan = item["DocumentNumber"].ToString(),
                //        Type = item["EntryType"].ToString(),
                //        EntityCode = item["EntityCode"].ToString(),
                //        NetworkName = item["NetworkName"].ToString(),
                //        ContactPerson = item["ContactPerson"].ToString(),
                //        Technician = item["Technician"].ToString(),
                //        Location = item["branch_description"].ToString(),
                //        Receivedby = item["Receivedby"].ToString(),
                //        Receivedon = item["Receivedon"].ToString(),
                //        Assignedby = item["Assignedby"].ToString(),
                //        Assignedon = item["Assignedon"].ToString(),
                //        ServEnteredBy = item["ServEnteredBy"].ToString(),
                //        ServEnteredOn = item["ServEnteredOn"].ToString(),
                //        DeliveryBy = item["Deliveryby"].ToString(),
                //        DeliveryOn = item["DeliveryOn"].ToString(),
                //        Status = item["Statuss"].ToString(),
                //        Action = Action,
                //        Warranty = item["Warranty"].ToString()
                //    });
                //}
            }
            return listStatues;
        }

        [WebMethod]
        public static List<Srv_DeliveryList> ReceiptChallanSearchList(String ReceiptchallanNo)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/search/searchqueries.aspx");
            List<Srv_DeliveryList> listStatues = new List<Srv_DeliveryList>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            proc.AddVarcharPara("@ACTION", 500, "SearchModule");
            proc.AddPara("@ReceiptchallanNo", ReceiptchallanNo);
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            // Mantis Issue 24265
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            // End of Mantis Issue 24265
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {
                listStatues = (from DataRow item in ds.Rows
                               select new Srv_DeliveryList()
                               {
                                   ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                                   DocumentDate = item["DocumentDate"].ToString(),
                                   ReceiptChallan = item["DocumentNumber"].ToString(),
                                   Type = item["EntryType"].ToString(),
                                   EntityCode = item["EntityCode"].ToString(),
                                   NetworkName = item["NetworkName"].ToString(),
                                   ContactPerson = item["ContactPerson"].ToString(),
                                   Technician = item["Technician"].ToString(),
                                   Location = item["branch_description"].ToString(),
                                   Receivedby = item["Receivedby"].ToString(),
                                   Receivedon = item["Receivedon"].ToString(),
                                   Assignedby = item["Assignedby"].ToString(),
                                   Assignedon = item["Assignedon"].ToString(),
                                   ServEnteredBy = item["ServEnteredBy"].ToString(),
                                   ServEnteredOn = item["ServEnteredOn"].ToString(),
                                   DeliveryBy = item["Deliveryby"].ToString(),
                                   DeliveryOn = item["DeliveryOn"].ToString(),
                                   Status = item["Status"].ToString(),
                                   Action = item["Action"].ToString(),
                                   Warranty = item["Warranty"].ToString(),
                                   ConfirmDelivery = item["ConfirmDelivery"].ToString(),
                                   ConfirmDeliveryDate = item["ConfirmDeliveryDate"].ToString()
                               }).ToList();
                //foreach (DataRow item in ds.Rows)
                //{
                //    //String Assignedon = "";
                //    //string DeliveryOn = "";
                //    string Action = "";
                //    //String ServEnteredOn = "";
                //    //if (item["DeliveryOn"].ToString() != "")
                //    //{
                //    //    DeliveryOn = Convert.ToDateTime(item["DeliveryOn"].ToString()).ToString("dd-MM-yyyy");
                //    //}
                //    //if (item["Assignedon"].ToString() != "")
                //    //{
                //    //    Assignedon = Convert.ToDateTime(item["Assignedon"].ToString()).ToString("dd-MM-yyyy");
                //    //}
                //    //if (item["ServEnteredOn"].ToString() != "")
                //    //{
                //    //    ServEnteredOn = Convert.ToDateTime(item["ServEnteredOn"].ToString()).ToString("dd-MM-yyyy");
                //    //}
                //    //String Status = "";
                //    //if (item["Statuss"].ToString() == "P")
                //    //{
                //    //    Status = " <span class='badge badge-danger'>Unassigned</span>";
                //    //}
                //    //else if (item["Statuss"].ToString() == "DN")
                //    //{
                //    //    Status = " <span class='badge badge-info'>Ready</span>";
                //    //}
                //    //if (item["Statuss"].ToString() == "DU")
                //    //{
                //    //    Status = " <span class='badge badge-warning'>Assigned</span>";
                //    //}
                //    //else if (item["Statuss"].ToString() == "DE")
                //    //{
                //    //    Status = " <span class='badge badge-success'>Delivered</span>";
                //    //}

                //    if (rights.CanPrint)
                //    {
                //        Action =  " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                //    }

                //    if (rights.CanView)
                //    {
                //        Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                //    }

                //    listStatues.Add(new Srv_DeliveryList
                //    {
                //        ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                //        ReceiptChallan = item["DocumentNumber"].ToString(),
                //        Type = item["EntryType"].ToString(),
                //        EntityCode = item["EntityCode"].ToString(),
                //        NetworkName = item["NetworkName"].ToString(),
                //        ContactPerson = item["ContactPerson"].ToString(),
                //        Technician = item["Technician"].ToString(),
                //        Location = item["branch_description"].ToString(),
                //        Receivedby = item["Receivedby"].ToString(),
                //        Receivedon = item["Receivedon"].ToString(),
                //        Assignedby = item["Assignedby"].ToString(),
                //        Assignedon = item["Assignedon"].ToString(),
                //        ServEnteredBy = item["ServEnteredBy"].ToString(),
                //        ServEnteredOn = item["ServEnteredOn"].ToString(),
                //        DeliveryBy = item["Deliveryby"].ToString(),
                //        DeliveryOn = item["DeliveryOn"].ToString(),
                //        Status = item["Statuss"].ToString(),
                //        Action = Action,
                //        Warranty = item["Warranty"].ToString()
                //    });
                //}
            }
            return listStatues;
        }


        [WebMethod]
        public static List<SerialSearch> SerialNoSearchList(String SerialNo)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/search/searchqueries.aspx");
            List<SerialSearch> listStatues = new List<SerialSearch>();
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            proc.AddVarcharPara("@ACTION", 500, "SearchModuleSerialNo");
            proc.AddPara("@DeviceNumber", SerialNo);
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            // Mantis Issue 24265
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            // End of Mantis Issue 24265
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();

            if (ds != null && ds.Rows.Count > 0)
            {
                listStatues = APIHelperMethods.ToModelList<SerialSearch>(ds);
                //foreach (DataRow item in ds.Rows)
                //{

                //    String Warranty = "";
                //    if (item["Warrenty"].ToString() != "")
                //    {
                //        Warranty = Convert.ToDateTime(item["Warrenty"].ToString()).ToString("dd-MM-yyyy");
                //    }
                   
                //    //String Status = "";
                //    //if (item["Statuss"].ToString() == "P")
                //    //{
                //    //    Status = " <span class='badge badge-danger'>Unassigned</span>";
                //    //}
                //    //else if (item["Statuss"].ToString() == "DN")
                //    //{
                //    //    Status = " <span class='badge badge-info'>Ready</span>";
                //    //}
                //    //if (item["Statuss"].ToString() == "DU")
                //    //{
                //    //    Status = " <span class='badge badge-warning'>Assigned</span>";
                //    //}
                //    //else if (item["Statuss"].ToString() == "DE")
                //    //{
                //    //    Status = " <span class='badge badge-success'>Delivered</span>";
                //    //}

                //    //if (rights.CanPrint)
                //    //{
                //    //    Action = " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                //    //}

                //    //if (rights.CanView)
                //    //{
                //    //    Action = Action + " <span data-toggle='modal' class='actionInput' data-target='#viewDetails' onclick='ViewDetails(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span>";
                //    //}

                //    listStatues.Add(new SerialSearch
                //    {
                //        ChallanNo = item["ChallanNo"].ToString(),
                //        SearialNo = item["SearialNo"].ToString(),
                //        Model = item["Model"].ToString(),
                //        EntityCode = item["EntityCode"].ToString(),
                //        NetworkName = item["NetworkName"].ToString(),
                //        ProblemReported = item["ProblemReported"].ToString(),
                //        ServiceAction = item["ServiceAction"].ToString(),
                //        Component = item["Component"].ToString(),
                //        Warranty = Warranty,
                //        StockEntry = item["StockEntry"].ToString(),
                //        NewSerialNo = item["NewSerialNo"].ToString(),
                //        ItemModel = item["ItemModel"].ToString(),
                //        ReturnReason = item["ReturnReason"].ToString(),
                //        Billable = item["Billable"].ToString(),
                //        ProblemFound = item["ProblemFound"].ToString(),
                //        Remarks = item["Remarks"].ToString(),
                //        WarrantyStatus = item["WarrantyStatus"].ToString()
                //    });
                //}
            }
            return listStatues;
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
                   // Mantis Issue 24265
                   //ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ") order by branch_description asc");
                    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch order by branch_description asc");
                    // End of Mantis Issue 24265
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

        #region Location Populate

        protected void Location_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindLocationGrid")
            {
                DataTable LocationTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                if (Session["userbranchHierarchy"] != null)
                {
                    //LocationTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                    LocationTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ") order by branch_description asc");
                }

                if (LocationTable.Rows.Count > 0)
                {
                    Session["LocationData"] = LocationTable;
                    lookup_Location.DataSource = LocationTable;
                    lookup_Location.DataBind();
                }
                else
                {
                    Session["LocationData"] = LocationTable;
                    lookup_Location.DataSource = null;
                    lookup_Location.DataBind();
                }
            }
        }

        protected void lookup_Location_DataBinding(object sender, EventArgs e)
        {
            if (Session["LocationData"] != null)
            {
                lookup_Location.DataSource = (DataTable)Session["LocationData"];
            }
        }

        #endregion

        [WebMethod]
        public static SRV_ReceptChallanHeader ReceptDetails(String ReceiptID, String module)
        {
            SRV_ReceptChallanHeader ret = new SRV_ReceptChallanHeader();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                proc.AddVarcharPara("@ACTION", 500, "ReceiptChallanDetailsView");
                proc.AddPara("@ReceiptChallan_ID", ReceiptID);
                proc.AddPara("@Module", module);
                ds = proc.GetDataSet();

                List<srv_ReceptChallanDetails> DetailsList = new List<srv_ReceptChallanDetails>();
                
                if (ds!= null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.EntryType = item["EntryType"].ToString();
                        ret.DocumentNumber = item["DocumentNumber"].ToString();
                        ret.DocumentDate = item["DocumentDate"].ToString();
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.Branch = item["Branch"].ToString();
                        ret.ContactNo = item["ContactNo"].ToString();
                        ret.Technician = item["TECHNICIAN"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DetailsList = APIHelperMethods.ToModelList<srv_ReceptChallanDetails>(ds.Tables[1]);
                    }
                    ret.DetailsList = DetailsList;

                }
            }
            return ret;
        }

        [WebMethod]
        public static SRV_SearchServiceEntryView ServiceEntryView(String ReceiptID, String module)
        {
            SRV_SearchServiceEntryView ret = new SRV_SearchServiceEntryView();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                proc.AddVarcharPara("@ACTION", 500, "ReceiptChallanDetailsView");
                proc.AddPara("@ReceiptChallan_ID", ReceiptID);
                proc.AddPara("@Module", module);
                ds = proc.GetDataSet();

                List<SRV_SearchServiceEntryViewDetails> DetailsList = new List<SRV_SearchServiceEntryViewDetails>();

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.ChallanNo = item["DocumentNumber"].ToString();
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.ReceivedOn = item["ReceivedOn"].ToString();
                        ret.ReceivedBy = item["ReceivedBy"].ToString();
                        ret.AssignedTo = item["TECHNICIAN"].ToString();
                        ret.AssignedBy = item["AssignedBy"].ToString();
                        ret.AssignedOn = item["AssignedOn"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DetailsList = (from DataRow item in ds.Tables[1].Rows
                                       select new SRV_SearchServiceEntryViewDetails()
                                       {
                                           ModelNo = item["Model"].ToString(),
                                           SerialNo = item["DeviceNumber"].ToString(),
                                           ProblemReported = item["ProblemDesc"].ToString(),
                                           ServiceAction = item["SrvActionDesc"].ToString(),
                                           Components = item["sProducts_Description"].ToString(),
                                           Warranty = item["Warranty"].ToString(),
                                           StockEntry = item["StockEntry"].ToString(),
                                           NewModel = item["ModelDesc"].ToString(),
                                           NewSerialNo = item["NewSerialNo"].ToString(),
                                           ProblemFound = item["ProbFound"].ToString(),
                                           Remarks = item["Remarks"].ToString(),
                                           WarrantyStatus = item["WarrentyStatus"].ToString(),
                                           ReturnReason = item["ReasonDesc"].ToString(),
                                           Billable = item["Billable"].ToString(),
                                           NewWarranty = item["NewWarranty"].ToString()
                                       }).ToList();
                    }
                    ret.DetailsList = DetailsList;

                }
            }
            return ret;
        }

        [WebMethod]
        public static SRV_SearchDeliveryView DeliveryView(String ReceiptID, String module)
        {
            SRV_SearchDeliveryView ret = new SRV_SearchDeliveryView();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                proc.AddVarcharPara("@ACTION", 500, "ReceiptChallanDetailsView");
                proc.AddPara("@ReceiptChallan_ID", ReceiptID);
                proc.AddPara("@Module", module);
                ds = proc.GetDataSet();

                List<SRV_SearchDeliveryViewDetails> DetailsList = new List<SRV_SearchDeliveryViewDetails>();

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.ChallanNo = item["DocumentNumber"].ToString();
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.ReceivedOn = item["ReceivedOn"].ToString();
                        ret.ReceivedBy = item["ReceivedBy"].ToString();
                        ret.AssignedTo = item["TECHNICIAN"].ToString();
                        ret.AssignedBy = item["AssignedBy"].ToString();
                        ret.AssignedOn = item["AssignedOn"].ToString();
                        ret.DeliveredTo = item["DeliveredTo"].ToString();
                        ret.PhoneNo = item["DeliveryContact"].ToString();
                        ret.Remarks = item["DeliveryRemarks"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DetailsList = (from DataRow item in ds.Tables[1].Rows
                                       select new SRV_SearchDeliveryViewDetails()
                                       {
                                           DeviceType = item["DeviceType"].ToString(),
                                           Model = item["Model"].ToString(),
                                           DeviceNumber = item["DeviceNumber"].ToString(),
                                           Problemfound = item["ProblemDesc"].ToString(),
                                           ServiceAction = item["SrvActionDesc"].ToString(),
                                           Warranty = item["Warrenty"].ToString(),
                                           CardAdaptor = item["CordAdaptor_Status"].ToString(),
                                           Remotes = item["Remote_Status"].ToString()
                                       }).ToList();
                    }
                    ret.DetailsList = DetailsList;

                }
            }
            return ret;
        }

        [WebMethod]
        public static SRV_SearchJobSheetView JobSheetView(String ReceiptID, String module)
        {
            SRV_SearchJobSheetView ret = new SRV_SearchJobSheetView();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                proc.AddVarcharPara("@ACTION", 500, "ReceiptChallanDetailsView");
                proc.AddPara("@ReceiptChallan_ID", ReceiptID);
                proc.AddPara("@Module", module);
                ds = proc.GetDataSet();

                List<SRV_SearchJobSheetViewDetails> DetailsList = new List<SRV_SearchJobSheetViewDetails>();

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ret.ChallanNumber = item["DocumentNumber"].ToString();
                        ret.PostingDate = item["PostingDate"].ToString();
                        ret.RefJobsheet = item["RefJobsheet"].ToString();
                        ret.AssignTo = item["TECHNICIAN"].ToString();
                        ret.WorkDoneOn = item["WorkDoneOn"].ToString();
                        ret.Location = item["Branch"].ToString();
                        ret.Remarks = item["Remarks"].ToString();
                        ret.EntityCode = item["EntityCode"].ToString();
                        ret.NetworkName = item["NetworkName"].ToString();
                        ret.ContactPerson = item["ContactPerson"].ToString();
                        ret.ContactNumber = item["ContactNo"].ToString();
                        break;
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DetailsList = (from DataRow item in ds.Tables[1].Rows
                                       select new SRV_SearchJobSheetViewDetails()
                                       {
                                           SerialNumber = item["DeviceNumber"].ToString(),
                                           DeviceType = item["DeviceType_Name"].ToString(),
                                           Model = item["Model"].ToString(),
                                           Problemfound = item["ProblemDesc"].ToString(),
                                           Other = item["OtherProblem"].ToString(),
                                           ServiceAction = item["SrvActionDesc"].ToString(),
                                           Components = item["Component"].ToString(),
                                           Warranty = item["Warranty"].ToString(),
                                           ReturnReason = item["ReasonDesc"].ToString(),
                                           Remarks = item["Remarks"].ToString(),
                                           Billable = item["BILLABLE"].ToString()
                                       }).ToList();
                    }
                    ret.DetailsList = DetailsList;

                }
            }
            return ret;
        }

        [WebMethod]
        public static string UndoDelivery(String ReceiptChallanID)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                    proc.AddVarcharPara("@ACTION", 500, "UpdateUndoConfirmDelivery");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptChallanID));
                    NoOfRowEffected = proc.RunActionQuery();
                    if (NoOfRowEffected > 0)
                    {
                    }
                    output = "true";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        //Mantis Issue 24360
        [WebMethod]
        public static string SendSMS(String ReceiptID, String module)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
                    proc.AddVarcharPara("@ACTION", 500, "SendSMSManually");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptID));
                    NoOfRowEffected = proc.RunActionQuery();
                    if (NoOfRowEffected > 0)
                    {
                    }
                    output = "true";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
        //End of Mantis Issue 24360
        //Mantis Issue 24781
        [WebMethod]
        public static string SendSMSManualNo(String ReceiptID, String module, String MobileNo)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
                    proc.AddVarcharPara("@ACTION", 500, "SendSMSManually");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptID));
                    proc.AddPara("@sendSMS", MobileNo);
                    NoOfRowEffected = proc.RunActionQuery();
                    if (NoOfRowEffected > 0)
                    {
                    }
                    output = "true";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
        //End of Mantis Issue 24781
    }
}