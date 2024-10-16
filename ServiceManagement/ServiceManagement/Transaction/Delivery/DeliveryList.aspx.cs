/***********************************************************************************************************************************
 * Rev 1.0      Sanchita    16/10/2024      0027747: Need to Implement existing SMS sending to Normal link instead of Bitly for GTPL
 * *********************************************************************************************************************************/
using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net;

namespace ServiceManagement.ServiceManagement.Transaction.Delivery
{
    public partial class DeliveryList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        CommonBL ComBL = new CommonBL();
        string userbranchHierachy = null;
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Delivery/DeliveryList.aspx");
            if (!IsPostBack)
            {
                Session["TechnicianData"] = null;
                Session["SI_ComponentData_Branch"] = null;
                // string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);
                // ddlBranch.SelectedValue = Convert.ToString(Session["userbranchID"]);
                string user_id = Convert.ToString(Session["userid"]);
                DataTable dtusertyp = obj.GetUserType(user_id);
                if (dtusertyp != null && dtusertyp.Rows.Count > 0)
                {
                    hdnUserType.Value = dtusertyp.Rows[0]["contactType"].ToString();
                }
                DataTable dt;
                //if (hdnUserType.Value == "Technician")
                //{
                //    dt = obj.GetTechnicianBind(user_id);
                //}
                //else
                // //{
                // dt = obj.GetAssignJobDetails(userbranchHierachy);
                //// }

                // if (dt != null && dt.Rows.Count > 0)
                // {
                //     ddlTechnician.DataSource = dt;
                //     ddlTechnician.DataValueField = "cnt_id";
                //     ddlTechnician.DataTextField = "cnt_firstName";
                //     ddlTechnician.DataBind();
                //     ddlTechnician.SelectedIndex = 0;
                // }
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
        public static Srv_deliveryCount CountJob(String UserType)
        {
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            proc.AddVarcharPara("@ACTION", 500, "DeliveryCount");
            //if (UserType == "Technician")
            //{
            //    proc.AddPara("@USER_ID", user_id);
            //}
            //else
            //{
            proc.AddPara("@USER_ID", 0);
            //}
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            ds = proc.GetTable();
            Srv_deliveryCount ret = new Srv_deliveryCount();
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow item in ds.Rows)
                {
                    ret.TotalEntered = item["TOTAL"].ToString();
                    ret.TotalDelivered = item["Assigned"].ToString();
                    ret.Pendingdelivery = item["Unassigned"].ToString();
                    break;
                }
            }
            return ret;
        }

        [WebMethod]
        public static srv_DeliverySearch BindTotalList(string Type)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Delivery/DeliveryList.aspx");
            srv_DeliverySearch ret = new srv_DeliverySearch();
            List<Srv_DeliveryList> listStatues = new List<Srv_DeliveryList>();
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            if (Type == "TotalEntered")
            {
                proc.AddVarcharPara("@ACTION", 500, "TotalEntered");
            }
            else if (Type == "TotalDelivered")
            {
                proc.AddVarcharPara("@ACTION", 500, "TotalDelivered");
            }
            else if (Type == "Pendingdelivery")
            {
                proc.AddVarcharPara("@ACTION", 500, "Pendingdelivery");
            }

            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddPara("@CanEdit", rights.CanEdit);
            proc.AddPara("@CanView", rights.CanView);
            proc.AddPara("@CanDelete", rights.CanDelete);
            proc.AddPara("@CanAdd", rights.CanAdd);
            proc.AddPara("@CanAddUpdateDocuments", rights.CanAddUpdateDocuments);
            proc.AddPara("@CanPrint", rights.CanPrint);
            proc.AddPara("@CanApproved", rights.CanApproved);
            ds = proc.GetDataSet();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ret.TotalEntered = item["TOTAL"].ToString();
                    ret.TotalDelivered = item["Assigned"].ToString();
                    ret.Pendingdelivery = item["Unassigned"].ToString();
                    break;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    listStatues = (from DataRow item in ds.Tables[1].Rows
                                   select new Srv_DeliveryList()
                                   {
                                       ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
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
                                       UpdateBy = item["Updatedby"].ToString(),
                                       UpdateOn = item["UpdatedOn"].ToString(),
                                       Status = item["Statuss"].ToString(),
                                       Action = item["Action_Add"].ToString() + item["Action_Edit"].ToString() + item["Action_View"].ToString() + item["Action_Delete"].ToString() + item["Action_Print"].ToString() + item["Action_Documents"].ToString() + item["Confirm_Delivery"].ToString()
                                   }).ToList();

                    #region Not Used
                    //foreach (DataRow item in ds.Tables[1].Rows)
                    //{
                    //    string DeliveryOn = "";
                    //    string UpdateOn = "";
                    //    string Action = "";
                    //    if (item["DeliveryOn"].ToString() != "")
                    //    {
                    //        DeliveryOn = Convert.ToDateTime(item["DeliveryOn"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    //    }
                    //    if (item["UpdatedOn"].ToString() != "")
                    //    {
                    //        UpdateOn = Convert.ToDateTime(item["UpdatedOn"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    //    }
                    //    String Status = "";
                    //    if (item["Statuss"].ToString() == "P")
                    //    {
                    //         Status = " <span class='badge badge-info'>Ready</span>";
                    //    }
                    //    else if (item["Statuss"].ToString() == "DE")
                    //    {
                    //        Status = " <span class='badge badge-success'>Delivered</span>";
                    //    }


                    //    if (rights.CanAdd)
                    //    {
                    //        if (Type != "TotalDelivered")
                    //        {
                    //            if (item["Statuss"].ToString() != "DE")
                    //            {
                    //                Action = " <span class='actionInput' onclick='AddDelivery(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-plus assig' data-toggle='tooltip' data-placement='left' title='Add Delivery'></i></span>";
                    //            }
                    //        }
                    //    }
                    //    if (item["Statuss"].ToString() == "DE")
                    //    {
                    //        if (rights.CanEdit)
                    //        {
                    //            Action = Action + " <span class='actionInput text-center' onclick='EditDelivery(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Edit' ></i> </span>";
                    //        }
                    //        if (rights.CanView)
                    //        {
                    //            Action = Action + " <span class='actionInput text-center' onclick='ViewDelivery(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                    //        }
                    //        if (rights.CanDelete)
                    //        {
                    //            Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Delete' ></i> </span>";
                    //        }
                    //        if (rights.CanAddUpdateDocuments)
                    //        {
                    //            Action = Action + " <span class='actionInput text-center' onclick='OnclickViewAttachment(" + item["DeliveryID"].ToString() + ")'><i class='fa fa-paperclip det' data-toggle='tooltip' data-placement='bottom' title='Attachment' ></i> </span>";
                    //        }
                    //    }
                    //    if (rights.CanPrint)
                    //    {
                    //        //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                    //        Action = Action + " <span class='actionInput text-center' onclick='onPrintJv(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
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
                    //        Receivedon = Convert.ToDateTime(item["Receivedon"].ToString()).ToString("dd-MM-yyyy"),
                    //        Assignedby = item["Assignedby"].ToString(),
                    //        Assignedon = Convert.ToDateTime(item["Assignedon"].ToString()).ToString("dd-MM-yyyy"),
                    //        ServEnteredBy = item["ServEnteredBy"].ToString(),
                    //        ServEnteredOn = Convert.ToDateTime(item["ServEnteredOn"].ToString()).ToString("dd-MM-yyyy"),
                    //        DeliveryBy = item["Deliveryby"].ToString(),
                    //        DeliveryOn = DeliveryOn,
                    //        UpdateBy = item["Updatedby"].ToString(),
                    //        UpdateOn = UpdateOn,
                    //        Status = Status,
                    //        Action = Action

                    //    });
                    //}
                    #endregion
                }
                ret.DetailsList = listStatues;
            }
            return ret;
        }

        [WebMethod]
        public static string DeleteDelivery(String ReceiptChallanID)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                    proc.AddVarcharPara("@ACTION", 500, "DeliveryDelete");
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

        [WebMethod]
        public static srv_DeliverySearch SearchData(srv_SearchFilterInput model)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Delivery/DeliveryList.aspx");
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            if (model.SearchType == "TotalEntered")
            {
                proc.AddVarcharPara("@ACTION", 500, "SearchTotalEntered");
            }
            else if (model.SearchType == "TotalDelivered")
            {
                proc.AddVarcharPara("@ACTION", 500, "SearchTotalDelivered");
            }
            else if (model.SearchType == "Pendingdelivery")
            {
                proc.AddVarcharPara("@ACTION", 500, "SearchPendingdelivery");
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
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddPara("@CanEdit", rights.CanEdit);
            proc.AddPara("@CanView", rights.CanView);
            proc.AddPara("@CanDelete", rights.CanDelete);
            proc.AddPara("@CanAdd", rights.CanAdd);
            proc.AddPara("@CanAddUpdateDocuments", rights.CanAddUpdateDocuments);
            proc.AddPara("@CanPrint", rights.CanPrint);
            proc.AddPara("@CanApproved", rights.CanApproved);
            ds = proc.GetDataSet();
            srv_DeliverySearch ret = new srv_DeliverySearch();
            List<Srv_DeliveryList> DetailsList = new List<Srv_DeliveryList>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ret.TotalEntered = item["TOTAL"].ToString();
                    ret.TotalDelivered = item["Assigned"].ToString();
                    ret.Pendingdelivery = item["Unassigned"].ToString();
                    break;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    DetailsList = (from DataRow item in ds.Tables[1].Rows
                                   select new Srv_DeliveryList()
                                   {
                                       ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
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
                                       UpdateBy = item["Updatedby"].ToString(),
                                       UpdateOn = item["UpdatedOn"].ToString(),
                                       Status = item["Statuss"].ToString(),
                                       Action = item["Action_Add"].ToString() + item["Action_Edit"].ToString() + item["Action_View"].ToString() + item["Action_Delete"].ToString() + item["Action_Print"].ToString() + item["Action_Documents"].ToString() + item["Confirm_Delivery"].ToString()
                                   }).ToList();
                    #region Not Used

                    //foreach (DataRow item in ds.Tables[1].Rows)
                    //{
                    //    string DeliveryOn = "";
                    //    string UpdateOn = "";
                    //    string Action = "";
                    //    if (item["DeliveryOn"].ToString() != "")
                    //    {
                    //        DeliveryOn = Convert.ToDateTime(item["DeliveryOn"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    //    }
                    //    if (item["UpdatedOn"].ToString() != "")
                    //    {
                    //        UpdateOn = Convert.ToDateTime(item["UpdatedOn"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    //    }
                    //    String Status = "";
                    //    if (item["Statuss"].ToString() == "P")
                    //    {
                    //        Status = " <span class='badge badge-info'>Ready</span>";
                    //    }
                    //    else if (item["Statuss"].ToString() == "DE")
                    //    {
                    //        Status = " <span class='badge badge-success'>Delivered</span>";
                    //    }


                    //    if (rights.CanAdd)
                    //    {
                    //        if (model.SearchType != "TotalDelivered")
                    //        {
                    //            if (item["Statuss"].ToString() != "DE")
                    //            {
                    //                Action = " <span class='actionInput' onclick='AddDelivery(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-plus assig' data-toggle='tooltip' data-placement='left' title='Add Delivery'></i></span>";
                    //            }
                    //        }
                    //    }

                    //    if (item["Statuss"].ToString() == "DE")
                    //    {
                    //        if (rights.CanEdit)
                    //        {
                    //            Action = Action + " <span class='actionInput text-center' onclick='EditDelivery(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Edit' ></i> </span>";
                    //        }
                    //        if (rights.CanView)
                    //        {
                    //            Action = Action + " <span class='actionInput text-center' onclick='ViewDelivery(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                    //        }
                    //        if (rights.CanDelete)
                    //        {
                    //            Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Delete' ></i> </span>";
                    //        }
                    //        if (rights.CanAddUpdateDocuments)
                    //        {
                    //            Action = Action + " <span class='actionInput text-center' onclick='OnclickViewAttachment(" + item["DeliveryID"].ToString() + ")'><i class='fa fa-paperclip det' data-toggle='tooltip' data-placement='bottom' title='Attachment' ></i> </span>";
                    //        }
                    //    }
                    //    if (rights.CanPrint)
                    //    {
                    //        //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                    //        Action = Action + " <span class='actionInput text-center' onclick='onPrintJv(" + item["ReceiptChallan_ID"].ToString() + ")'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                    //    }

                    //    DetailsList.Add(new Srv_DeliveryList
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
                    //        Receivedon = Convert.ToDateTime(item["Receivedon"].ToString()).ToString("dd-MM-yyyy"),
                    //        Assignedby = item["Assignedby"].ToString(),
                    //        Assignedon = Convert.ToDateTime(item["Assignedon"].ToString()).ToString("dd-MM-yyyy"),
                    //        ServEnteredBy = item["ServEnteredBy"].ToString(),
                    //        ServEnteredOn = Convert.ToDateTime(item["ServEnteredOn"].ToString()).ToString("dd-MM-yyyy"),
                    //        DeliveryBy = item["Deliveryby"].ToString(),
                    //        DeliveryOn = DeliveryOn,
                    //        UpdateBy = item["Updatedby"].ToString(),
                    //        UpdateOn = UpdateOn,
                    //        Status = Status,
                    //        Action = Action

                    //    });
                    //}
                    #endregion
                }
                ret.DetailsList = DetailsList;
            }
            return ret;
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\SRVDeliveryChallan\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SRVDeliveryChallan\DocDesign\Designes";
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
        public static string ConfirmDelivery(String ReceiptChallanID)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    //Mantis Issue 24713
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    string DataBase = con.Database;
                    string @COMPANYID = Convert.ToString(System.Web.HttpContext.Current.Session["LastCompany"]);
                    string @FINYEAR = Convert.ToString(System.Web.HttpContext.Current.Session["LastFinYear"]).Trim();

                    string baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseUrl"];
                    //string baseUrl = "https://3.7.30.86:85";
                    //string LongURL = baseUrl + "/ServiceManagement/Transaction/Delivery/DeliveryServiceDetails.aspx?id=" + Convert.ToString(ReceiptChallanID) + "&UniqueKey=" + Convert.ToString(DataBase);
                    // Rev 1.0
                    //string LongURL = baseUrl + "/ServiceManagement/Transaction/Delivery/DeliveryServiceDetails.aspx?COMPANYID=" + @COMPANYID + "&FINYEAR=" + @FINYEAR + "&RCID=" + Convert.ToString(ReceiptChallanID) +"&ISCREATEORPREVIEW=P" + "&DataBase=" + Convert.ToString(DataBase);

                    ////string LongURL = "https://stackoverflow.com/questions/366115/using-tinyurl-com-in-a-net-application-possible";
                    //string tinyURL = ShortURL(LongURL);

                    string LongURL = baseUrl + "/ServiceManagement/Transaction/Delivery/DeliveryServiceDetails.aspx?COMPANYID=" + @COMPANYID + "%26FINYEAR=" + @FINYEAR + "%26RCID=" + Convert.ToString(ReceiptChallanID) + "%26ISCREATEORPREVIEW=P" + "%26DataBase=" + Convert.ToString(DataBase);

                    // End of Rev 1.0

                    //End of Mantis Issue 24713
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                    proc.AddVarcharPara("@ACTION", 500, "UpdateConfirmDelivery");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptChallanID));
                    // Rev 1.0
                    //proc.AddPara("@tinyURL", Convert.ToString(tinyURL));
                    proc.AddPara("@longURL", Convert.ToString(LongURL));
                    // End of Rev 1.0
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
        //Mantis Issue 24713
        private static string ShortURL(string LongUrl)
        {
            //System.Uri address = new System.Uri("http://tinyurl.com/api-create.php?url=" + LongUrl);
            //System.Net.WebClient client = new System.Net.WebClient();
            //string tinyUrl = client.DownloadString(address);
            //return tinyUrl;

            //try
            //{
            //    if (LongUrl.Length <= 30)
            //    {
            //        return LongUrl;
            //    }
            //    if (!LongUrl.ToLower().StartsWith("http") && !LongUrl.ToLower().StartsWith("ftp"))
            //    {
            //        LongUrl = "http://" + LongUrl;
            //    }
            //    System.Uri address = new System.Uri("http://tinyurl.com/api-create.php?url=" + LongUrl);
            //    System.Net.WebClient client = new System.Net.WebClient();
            //    string tinyUrl = client.DownloadString(address);
            //    return tinyUrl;
            //}
            //catch (Exception)
            //{
            //    return LongUrl;
            //}
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
        //End of Mantis Issue 24713
        [WebMethod]
        public static List<SRV_MassConfirmShow> bindMassAssign(string ReceiptType)
        {

            List<SRV_MassConfirmShow> listStatues = new List<SRV_MassConfirmShow>();

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            proc.AddVarcharPara("@ACTION", 500, "MassConfirmShow");
            //proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddPara("@BranchID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            ds = proc.GetTable();
            listStatues = (from DataRow item in ds.Rows
                           select new SRV_MassConfirmShow()
                           {
                               ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString(),
                               EntryType = item["EntryType"].ToString(),
                               DocumentNumber = item["DocumentNumber"].ToString(),
                               DocumentDate = item["DocumentDate"].ToString(),
                               EntityCode = item["EntityCode"].ToString(),
                               NetworkName = item["NetworkName"].ToString(),
                               ContactPerson = item["ContactPerson"].ToString(),
                               DeliveredTo = item["DeliveredTo"].ToString(),
                               DELIVERY_DATE = item["DELIVERY_DATE"].ToString()
                           }).ToList();

            return listStatues;
        }

        [WebMethod]
        public static string MassDeliveryConfirm(SRV_MassInsert model)
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
                    ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                    proc.AddVarcharPara("@Action", 500, "MassConfirmDelivery");
                    proc.AddPara("@RcptId", Convert.ToString(RecptID));
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
    }
}