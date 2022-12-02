using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ServiceManagement.ServiceManagement.Transaction.ServiceRegister
{
    public partial class ServiceRegister : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/ServiceRegister/ServiceRegister.aspx");

            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
            {
                if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                {
                    userbranch = EmployeeBranchMap();
                }
                else
                {
                    userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
            }
            else
            {
                userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            }
            Session["UserBranchMapID"] = userbranch;
        }

        [WebMethod]
        public static ServiceRegisterReports ServiceRegisterReport(String Action)
        {

            ServiceRegisterReports reports = new ServiceRegisterReports();
            List<Problem> problemList = new List<Problem>();
            List<EntityCodes> EntityCodeList = new List<EntityCodes>();
            List<Modeles> ModelList = new List<Modeles>();
            List<Tecchnician> TecchnicianList = new List<Tecchnician>();
            List<Branches> BranchesList = new List<Branches>();
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVServiceRegisterReport");
            proc.AddVarcharPara("@ACTION", 500, "ServiceRegisterList");
            ds = proc.GetDataSet();

            if (ds != null)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    problemList = APIHelperMethods.ToModelList<Problem>(ds.Tables[0]);
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    EntityCodeList = APIHelperMethods.ToModelList<EntityCodes>(ds.Tables[1]);
                }

                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    ModelList = APIHelperMethods.ToModelList<Modeles>(ds.Tables[2]);
                }

                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    TecchnicianList = APIHelperMethods.ToModelList<Tecchnician>(ds.Tables[3]);
                }

                if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {
                    BranchesList = APIHelperMethods.ToModelList<Branches>(ds.Tables[4]);
                }
                reports.BranchesList = BranchesList;
                reports.ProblemList = problemList;
                reports.EntityCodeList = EntityCodeList;
                reports.ModelList = ModelList;
                reports.TecchnicianList = TecchnicianList;

            }
            return reports;
        }

        [WebMethod]
        public static List<ReceiptChallanReports> ReceiptChalan(ServiceRegisterReportInput Data)
        {
            string Types = Data.Type;
            int k = 1;
            //if (Data.Type != null && Data.Type.Count > 0)
            //{
            //    foreach (string item in Data.Type)
            //    {
            //        if (k > 1)
            //            Types = Types + "," + item;
            //        else
            //            Types = item;
            //        k++;
            //    }
            //}

            string Report = "";
            k = 1;
            if (Data.Report != null && Data.Report.Count > 0)
            {
                foreach (string item in Data.Report)
                {
                    if (k > 1)
                        Report = Report + "," + item;
                    else
                        Report = item;
                    k++;
                }
            }

            string EntryType = "";
            k = 1;
            if (Data.EntryType != null && Data.EntryType.Count > 0)
            {
                foreach (string item in Data.EntryType)
                {
                    if (k > 1)
                        EntryType = EntryType + "," + item;
                    else
                        EntryType = item;
                    k++;
                }
            }


            if (Data.FromDate != "")
            {

            }


            List<ReceiptChallanReports> listStatues = new List<ReceiptChallanReports>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVServiceRegisterReport");
            proc.AddVarcharPara("@ACTION", 500, "ReceiptChalan");
            proc.AddPara("@Type", Types);
            proc.AddPara("@Report", Report);
            proc.AddPara("@EntityCode", Data.EntityCode);
            proc.AddPara("@EntryType", EntryType);
            proc.AddPara("@Model", Data.Model);
            proc.AddPara("@ProblemFound", Data.ProblemFound);
            proc.AddPara("@Technician", Data.Technician);
            if (Data.Location == "")
            {
                proc.AddPara("@Location", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@Location", Data.Location);
            }
            proc.AddPara("@FromDate", Data.FromDate);
            proc.AddPara("@ToDate", Data.ToDate);
            proc.AddPara("@IsBillable", Data.IsBillable);

            proc.AddPara("@ProblemReported", Data.ProblemReported);
            proc.AddPara("@IsProbLemReport", Data.IsProbLemReport);
            proc.AddPara("@IsDelivery", Data.IsDelivery);
            //proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                listStatues = APIHelperMethods.ToModelList<ReceiptChallanReports>(dt);
                //foreach (DataRow item in dt.Rows)
                //{
                //    String Status = "";
                //    if (item["STATUS"].ToString() == "P")
                //    {
                //        Status = " <span class='badge badge-danger'>Unassigned</span>";
                //    }
                //    else if (item["STATUS"].ToString() == "DN")
                //    {
                //        Status = " <span class='badge badge-info'>Ready</span>";
                //    }
                //    if (item["STATUS"].ToString() == "DU")
                //    {
                //        Status = " <span class='badge badge-warning'>Assigned</span>";
                //    }
                //    else if (item["STATUS"].ToString() == "DE")
                //    {
                //        Status = " <span class='badge badge-success'>Delivered</span>";
                //    }

                //    listStatues.Add(new ReceiptChallanReports
                //    {
                //        ReceiptChallan = item["ReceiptChallan"].ToString(),
                //        Date = item["Date"].ToString(),
                //        EntryType = item["EntryType"].ToString(),
                //        Location = item["Location"].ToString(),
                //        EntityCode = item["EntityCode"].ToString(),
                //        NetworkName = item["NetworkName"].ToString(),
                //        ContactName = item["ContactPerson"].ToString(),
                //        ContactNo = item["ContactNo"].ToString(),
                //        ProblemReported = item["ProblemReported"].ToString(),
                //        Cord = item["Cord"].ToString(),
                //        Adapter = item["Adapter"].ToString(),
                //        Technician = item["Technician"].ToString(),
                //        ReceivedBy = item["ReceivedBy"].ToString(),
                //        ReceivedOn = item["ReceivedOn"].ToString(),
                //        AssignedBy = item["AssignedBy"].ToString(),
                //        AssignedOn = item["AssignedOn"].ToString(),
                //        ServicedBy = item["ServicedBy"].ToString(),
                //        ServicedOn = item["ServicedOn"].ToString(),
                //        DeliveredOn = item["DeliveredOn"].ToString(),
                //        DeliveredBy = item["DeliveredBy"].ToString(),
                //        DeliveredTo = item["DeliveredTo"].ToString(),
                //        Status = Status
                //    });
                //}
            }
            return listStatues;
        }

        [WebMethod]
        public static List<DeliveryReports> DeliveryReport(ServiceRegisterReportInput Data)
        {

            string Types = Data.Type;
            int k = 1;
            //if (Data.Type != null && Data.Type.Count > 0)
            //{
            //    foreach (string item in Data.Type)
            //    {
            //        if (k > 1)
            //            Types = Types + "," + item;
            //        else
            //            Types = item;
            //        k++;
            //    }
            //}

            string Report = "";
            k = 1;
            if (Data.Report != null && Data.Report.Count > 0)
            {
                foreach (string item in Data.Report)
                {
                    if (k > 1)
                        Report = Report + "," + item;
                    else
                        Report = item;
                    k++;
                }
            }

            string EntryType = "";
            k = 1;
            if (Data.EntryType != null && Data.EntryType.Count > 0)
            {
                foreach (string item in Data.EntryType)
                {
                    if (k > 1)
                        EntryType = EntryType + "," + item;
                    else
                        EntryType = item;
                    k++;
                }
            }

            if (Data.FromDate != "")
            {

            }

            List<DeliveryReports> listStatues = new List<DeliveryReports>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVServiceRegisterReport");
            proc.AddVarcharPara("@ACTION", 500, "DeliveryReport");
            proc.AddPara("@Type", Types);
            proc.AddPara("@Report", Report);
            proc.AddPara("@EntityCode", Data.EntityCode);
            proc.AddPara("@EntryType", EntryType);
            proc.AddPara("@Model", Data.Model);
            proc.AddPara("@ProblemFound", Data.ProblemFound);
            proc.AddPara("@Technician", Data.Technician);
            //proc.AddPara("@Location", Data.Location);
            if (Data.Location == "")
            {
                proc.AddPara("@Location", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@Location", Data.Location);
            }
            proc.AddPara("@FromDate", Data.FromDate);
            proc.AddPara("@ToDate", Data.ToDate);
            proc.AddPara("@IsBillable", Data.IsBillable);

            proc.AddPara("@ProblemReported", Data.ProblemReported);
            proc.AddPara("@IsProbLemReport", Data.IsProbLemReport);
            proc.AddPara("@IsDelivery", Data.IsDelivery);
            //proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                listStatues = APIHelperMethods.ToModelList<DeliveryReports>(dt);
                //foreach (DataRow item in dt.Rows)
                //{
                //    String Status = "";
                //    if (item["STATUS"].ToString() == "P")
                //    {
                //        Status = " <span class='badge badge-danger'>Unassigned</span>";
                //    }
                //    else if (item["STATUS"].ToString() == "DN")
                //    {
                //        Status = " <span class='badge badge-info'>Ready</span>";
                //    }
                //    if (item["STATUS"].ToString() == "DU")
                //    {
                //        Status = " <span class='badge badge-warning'>Assigned</span>";
                //    }
                //    else if (item["STATUS"].ToString() == "DE")
                //    {
                //        Status = " <span class='badge badge-success'>Delivered</span>";
                //    }

                //    listStatues.Add(new DeliveryReports
                //    {
                //        ReceiptChallan = item["ReceiptChallan"].ToString(),
                //        Date = item["Date"].ToString(),
                //        EntryType = item["EntryType"].ToString(),
                //        Location = item["Location"].ToString(),
                //        EntityCode = item["EntityCode"].ToString(),
                //        Model = item["Model"].ToString(),
                //        Technician = item["Technician"].ToString(),
                //        SerialNo = item["SerialNo"].ToString(),
                //        ServiceAction = item["ServiceAction"].ToString(),
                //        ServicedOn = item["ServicedOn"].ToString(),
                //        DeliveredOn = item["DeliveredOn"].ToString(),
                //        Warranty = item["Warranty"].ToString(),
                //        WarrantyStatus = item["WarrantyStatus"].ToString(),
                //        StockEntry = item["StockEntry"].ToString(),
                //        NewModel = item["NewModel"].ToString(),
                //        NewSerialNo = item["NewSerialNo"].ToString(),
                //        Billable = item["Billable"].ToString(),
                //        ReturnReason = item["ReturnReason"].ToString(),
                //        ProblemFound = item["ProblemFound"].ToString(),
                //        Remarks = item["Remarks"].ToString(),
                //        Status = Status,
                //        Component = item["Component"].ToString(),
                //        ProblemReported = item["ProblemReported"].ToString()
                //    });
                //}
            }
            return listStatues;
        }

        #region EntityCode Populate

        protected void EntityCode_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindEntityCodeGrid")
            {
                DataTable EntityCodeTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                EntityCodeTable = oDBEngine.GetDataTable("select EntityCodeID as ID,EntityCodeDesc as EntityCode FROM SRV_Master_EntityCode order by EntityCodeDesc");

                if (EntityCodeTable.Rows.Count > 0)
                {
                    Session["EntityCodeData"] = EntityCodeTable;
                    lookup_EntityCode.DataSource = EntityCodeTable;
                    lookup_EntityCode.DataBind();
                }
                else
                {
                    Session["EntityCodeData"] = EntityCodeTable;
                    lookup_EntityCode.DataSource = null;
                    lookup_EntityCode.DataBind();
                }
            }
        }

        protected void lookup_EntityCode_DataBinding(object sender, EventArgs e)
        {
            if (Session["EntityCodeData"] != null)
            {
                lookup_EntityCode.DataSource = (DataTable)Session["EntityCodeData"];
            }
        }

        #endregion

        #region Model Populate

        protected void Model_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindModelGrid")
            {
                DataTable ModelTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                ModelTable = oDBEngine.GetDataTable("select ModelID AS ID,ModelDesc AS Model from Master_Model ORDER BY ModelDesc");

                if (ModelTable.Rows.Count > 0)
                {
                    Session["ModelData"] = ModelTable;
                    lookup_Model.DataSource = ModelTable;
                    lookup_Model.DataBind();
                }
                else
                {
                    Session["ModelData"] = ModelTable;
                    lookup_Model.DataSource = null;
                    lookup_Model.DataBind();
                }
            }
        }

        protected void lookup_Model_DataBinding(object sender, EventArgs e)
        {
            if (Session["ModelData"] != null)
            {
                lookup_Model.DataSource = (DataTable)Session["ModelData"];
            }
        }

        #endregion

        #region ProblemFound Populate

        protected void ProblemFound_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProblemFoundGrid")
            {
                DataTable ProblemFoundTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                ProblemFoundTable = oDBEngine.GetDataTable("select convert(nvarchar(10),ProblemID) AS ProblemID,ProblemDesc from master_problem");

                if (ProblemFoundTable.Rows.Count > 0)
                {
                    Session["ProblemFoundData"] = ProblemFoundTable;
                    lookup_ProblemFound.DataSource = ProblemFoundTable;
                    lookup_ProblemFound.DataBind();
                }
                else
                {
                    Session["ProblemFoundData"] = ProblemFoundTable;
                    lookup_ProblemFound.DataSource = null;
                    lookup_ProblemFound.DataBind();
                }
            }
        }

        protected void lookup_ProblemFound_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProblemFoundData"] != null)
            {
                lookup_ProblemFound.DataSource = (DataTable)Session["ProblemFoundData"];
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

                TechnicianTable = oDBEngine.GetDataTable("select DISTINCT CNT.cnt_internalId,CNT.cnt_firstName from tbl_master_contact CNT INNER JOIN Srv_master_TechnicianBranch_map MAP ON MAP.Tech_InternalId=CNT.cnt_internalId WHERE MAP.branch_id IN (" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ")  AND CNT.cnt_contactType='TM' AND CNT.Is_Active=1");

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

        //#region ProblemReported Populate

        //protected void ProblemReported_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.Split('~')[0] == "BindProblemReportedGrid")
        //    {
        //        DataTable ProblemReportedTable = new DataTable();
        //        string Hoid = e.Parameter.Split('~')[1];

        //        ProblemReportedTable = oDBEngine.GetDataTable("select convert(nvarchar(10),ProblemID) AS ProblemID,ProblemDesc from master_problem");

        //        if (ProblemReportedTable.Rows.Count > 0)
        //        {
        //            Session["ProblemReportedData"] = ProblemReportedTable;
        //            lookup_ProblemReported.DataSource = ProblemReportedTable;
        //            lookup_ProblemReported.DataBind();
        //        }
        //        else
        //        {
        //            Session["ProblemReportedData"] = ProblemReportedTable;
        //            lookup_ProblemReported.DataSource = null;
        //            lookup_ProblemReported.DataBind();
        //        }
        //    }
        //}

        //protected void lookup_ProblemReported_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["ProblemReportedData"] != null)
        //    {
        //        lookup_ProblemReported.DataSource = (DataTable)Session["ProblemReportedData"];
        //    }
        //}

        //#endregion

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

        [WebMethod]
        public static List<ReceiptChallanReports> ReceiptChalanDetails(ServiceRegisterReportInput Data)
        {
            string Types = Data.Type;
            int k = 1;
          
            string Report = "";
            k = 1;
            if (Data.Report != null && Data.Report.Count > 0)
            {
                foreach (string item in Data.Report)
                {
                    if (k > 1)
                        Report = Report + "," + item;
                    else
                        Report = item;
                    k++;
                }
            }

            string EntryType = "";
            k = 1;
            if (Data.EntryType != null && Data.EntryType.Count > 0)
            {
                foreach (string item in Data.EntryType)
                {
                    if (k > 1)
                        EntryType = EntryType + "," + item;
                    else
                        EntryType = item;
                    k++;
                }
            }


            if (Data.FromDate != "")
            {

            }


            List<ReceiptChallanReports> listStatues = new List<ReceiptChallanReports>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVServiceRegisterReport");
            proc.AddVarcharPara("@ACTION", 500, "ReceiptChalanDetails");
            proc.AddPara("@Type", Types);
            proc.AddPara("@Report", Report);
            proc.AddPara("@EntityCode", Data.EntityCode);
            proc.AddPara("@EntryType", EntryType);
            proc.AddPara("@Model", Data.Model);
            proc.AddPara("@ProblemFound", Data.ProblemFound);
            proc.AddPara("@Technician", Data.Technician);
            if (Data.Location == "")
            {
                proc.AddPara("@Location", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@Location", Data.Location);
            }
            proc.AddPara("@FromDate", Data.FromDate);
            proc.AddPara("@ToDate", Data.ToDate);
            proc.AddPara("@IsBillable", Data.IsBillable);

            proc.AddPara("@ProblemReported", Data.ProblemReported);
            proc.AddPara("@IsProbLemReport", Data.IsProbLemReport);
            proc.AddPara("@IsDelivery", Data.IsDelivery);
            //proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                listStatues = APIHelperMethods.ToModelList<ReceiptChallanReports>(dt);
                //foreach (DataRow item in dt.Rows)
                //{
                //    //String Status = "";
                //    //if (item["STATUS"].ToString() == "P")
                //    //{
                //    //    Status = " <span class='badge badge-danger'>Unassigned</span>";
                //    //}
                //    //else if (item["STATUS"].ToString() == "DN")
                //    //{
                //    //    Status = " <span class='badge badge-info'>Ready</span>";
                //    //}
                //    //if (item["STATUS"].ToString() == "DU")
                //    //{
                //    //    Status = " <span class='badge badge-warning'>Assigned</span>";
                //    //}
                //    //else if (item["STATUS"].ToString() == "DE")
                //    //{
                //    //    Status = " <span class='badge badge-success'>Delivered</span>";
                //    //}

                //    listStatues.Add(new ReceiptChallanReports
                //    {
                //        ReceiptChallan = item["ReceiptChallan"].ToString(),
                //        Date = item["Date"].ToString(),
                //        EntryType = item["EntryType"].ToString(),
                //        Location = item["Location"].ToString(),
                //        EntityCode = item["EntityCode"].ToString(),
                //        NetworkName = item["NetworkName"].ToString(),
                //        ContactName = item["ContactPerson"].ToString(),
                //        ContactNo = item["ContactNo"].ToString(),
                //        ProblemReported = item["ProblemReported"].ToString(),
                //        Cord = item["Cord"].ToString(),
                //        Adapter = item["Adapter"].ToString(),
                //        Technician = item["Technician"].ToString(),
                //        DeliveredTo = item["DeliveredTo"].ToString(),
                //        Status = Status,
                //        ModelNo = item["ModelNo"].ToString(),
                //        SerialNo = item["SerialNo"].ToString(),
                //        ReceivedBy = item["ReceivedBy"].ToString(),
                //        ReceivedOn = item["ReceivedOn"].ToString(),
                //        AssignedBy = item["AssignedBy"].ToString(),
                //        AssignedOn = item["AssignedOn"].ToString(),
                //        ServicedBy = item["ServicedBy"].ToString(),
                //        ServicedOn = item["ServicedOn"].ToString(),
                //        DeliveredOn = item["DeliveredOn"].ToString(),
                //        DeliveredBy = item["DeliveredBy"].ToString(),
                //        ServiceAction = item["ServiceAction"].ToString()
                //    });
                //}
            }
            return listStatues;
        }

    }
}