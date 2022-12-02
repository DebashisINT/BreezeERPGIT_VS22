using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.ServiceManagement.Transaction.Jobsheet
{
    public partial class jobsheetList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SrvJobSheetEntryBL obj = new SrvJobSheetEntryBL();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/jobsheet/jobsheetList.aspx");
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
          //  string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            PopulateBranchByHierchy(userbranch);
           // ddlBranch.SelectedValue = Convert.ToString(Session["userbranchID"]);
            if (!IsPostBack)
            {
                Session["SI_ComponentData_Branch"] = null;
                Session["TechnicianData"] = null;
                string user_id = Convert.ToString(Session["userid"]);
                DataTable dtusertyp = obj.GetUserType(user_id);
                if (dtusertyp != null && dtusertyp.Rows.Count > 0)
                {
                    hdnUserType.Value = dtusertyp.Rows[0]["contactType"].ToString();
                }
                //DataTable dt;

                //dt = obj.GetAssignJobDetails(userbranch);

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
        public static string DeleteJobsheetEntry(String JobsheetID)
        {
            string output = string.Empty;
            try
            {
                MasterSettings masterbl = new MasterSettings();
                string mastersettings = masterbl.GetSettings("StkAdjSrv");
                DataTable dt = new DataTable();

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
                    proc.AddVarcharPara("@ACTION", 500, "Delete");
                    proc.AddPara("@JobsheetID", Convert.ToString(JobsheetID));
                    proc.AddPara("@UserID", user_id);
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
        public static List<SrvJobSheetList> JobSheetList(srv_SearchFilterInput model)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/jobsheet/jobsheetList.aspx");
            List<SrvJobSheetList> listStatues = new List<SrvJobSheetList>();
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVJobSheetDetails");
            proc.AddVarcharPara("@ACTION", 500, "List");
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
            proc.AddPara("@TechnicianID", model.Technician_ID);
            ds = proc.GetDataSet();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    string Action = "";

                    if (rights.CanEdit)
                    {
                        Action = Action + " <span class='actionInput text-center' onclick='Edit(" + item["JobsheetID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Edit' ></i> </span>";
                    }
                    if (rights.CanView)
                    {
                        Action = Action + " <span class='actionInput text-center' onclick='View(" + item["JobsheetID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                    }
                    if (rights.CanDelete)
                    {
                        Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["JobsheetID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Delete' ></i> </span>";
                    }
                    if (rights.CanPrint)
                    {
                        //Action = Action + " <span class='actionInput text-center'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                        Action = Action + " <span class='actionInput text-center' onclick='onPrintJv(" + item["JobsheetID"].ToString() + ")'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                    }

                    listStatues.Add(new SrvJobSheetList
                    {
                        ChallanNumber = item["ChallanNumber"].ToString(),
                        RefJobsheet = item["RefJobsheet"].ToString(),
                        AssignTo = item["AssignTo"].ToString(),
                        WorkDoneOn = item["WorkDone_Date"].ToString(),
                        Location = item["Location"].ToString(),
                        EntityCode = item["EntityCode"].ToString(),
                        NetworkName = item["NetworkName"].ToString(),
                        ContactPerson = item["ContactPerson"].ToString(),
                        ContactNumber = item["ContactNumber"].ToString(),
                        Receivedby = item["Receivedby"].ToString(),
                        Receivedon = Convert.ToDateTime(item["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                        Status = item["Status"].ToString(),
                        Action = Action,
                        PostingDate = item["PostingDate"].ToString()
                    });
                }
            }

            return listStatues;
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            //switch (Filter)
            //{
            //    case 1:
            //        exporter.WritePdfToResponse();
            //        break;
            //    case 2:
            //        exporter.WriteXlsToResponse();
            //        break;
            //    case 3:
            //        exporter.WriteRtfToResponse();
            //        break;
            //    case 4:
            //        exporter.WriteCsvToResponse();
            //        break;
            //}
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
                    DesignPath = @"Reports\Reports\RepxReportDesign\SRVJobSheet\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SRVJobSheet\DocDesign\Designes";
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
    }
}