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

namespace ServiceManagement.ServiceManagement.Transaction.ServiceData
{
    public partial class serviceDataEntry : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        SrvServiceDataBL obj = new SrvServiceDataBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL ComBL = new CommonBL();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("status"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Mantis Issue 25172
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            hdnDbName.Value = con.Database;
            //End of Mantis Issue 25172
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/serviceData/serviceDataList.aspx");

            string ComponentWithQtyChecking = ComBL.GetSystemSettingsResult("ComponentWithQtyChecking");
            hdnComponentQty.Value = ComponentWithQtyChecking;

            if (!IsPostBack)
            {
                //string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //PopulateBranchByHierchy(userbranchHierachy);
                //ddlBranch.Value = Convert.ToString(Session["userbranchID"]);

                //if (Request.QueryString["Key"] != "ADD")
                //{
                string output = PopulateReceiptChalanDetailsById(Request.QueryString["id"]);
                hdnEntryMode.Value = "Add";
                if (Request.QueryString["status"] != null)
                {
                    DivHeader.Style.Add("display", "none");
                    hdnParentStatus.Value = "Yes";
                    divcross.Style.Add("display", "none");
                    if (output == "No")
                    {
                        btnAdd.Style.Add("display", "none");
                        btnSave.Style.Add("display", "none");
                    }
                    else
                    {
                        btnAdd.Style.Add("display", "!inline-block");
                        btnSave.Style.Add("display", "!inline-block");
                    }

                }
                else
                {

                    hdnParentStatus.Value = "No";
                    divcross.Style.Add("display", "!inline-block");
                    DivHeader.Style.Add("display", "!inline-block");

                    if (Request.QueryString["key"] == "Edit")
                    {
                        DivHeader.InnerHtml = "Edit Service Entry";
                        hdnEntryMode.Value = "Edit";
                    }

                    if (Request.QueryString["key"] == "View")
                    {
                        DivHeader.InnerHtml = "View Service Entry";
                        btnAdd.Style.Add("display", "none");
                        btnSave.Style.Add("display", "none");
                    }
                }


                // Hidden_add_edit.Value = Request.QueryString["Key"];
                hdnReceiptChalanID.Value = Request.QueryString["id"];
                DataSet ds = obj.GetServiceEntryList();
                if (ds != null)
                {
                    ddlServiceAction.DataSource = ds.Tables[1];
                    ddlServiceAction.ValueField = "SrvActionID";
                    ddlServiceAction.TextField = "SrvActionDesc";
                    ddlServiceAction.DataBind();
                    ddlServiceAction.SelectedIndex = 0;

                    //ddlComponent.DataSource = ds.Tables[2];
                    //ddlComponent.DataValueField = "sProducts_ID";
                    //ddlComponent.DataTextField = "sProducts_Description";
                    //ddlComponent.DataBind();
                    //ddlComponent.SelectedIndex = 0;

                    ddlModel.DataSource = ds.Tables[3];
                    ddlModel.DataValueField = "ModelID";
                    ddlModel.DataTextField = "ModelDesc";
                    ddlModel.DataBind();
                    ddlModel.SelectedIndex = 0;

                    ddlProblemFound.DataSource = ds.Tables[0];
                    ddlProblemFound.DataValueField = "ProblemID";
                    ddlProblemFound.DataTextField = "ProblemDesc";
                    ddlProblemFound.DataBind();
                    ddlProblemFound.SelectedIndex = 0;

                    ddlReturnReason.DataSource = ds.Tables[4];
                    ddlReturnReason.ValueField = "ReasonID";
                    ddlReturnReason.TextField = "ReasonDesc";
                    ddlReturnReason.DataBind();
                    ddlReturnReason.SelectedIndex = 0;
                }
                //}

            }
        }

        private string PopulateReceiptChalanDetailsById(string receiptID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ds = obj.GetServiceEntryByReceptChallanid(receiptID.ToString().Trim(), Convert.ToString(HttpContext.Current.Session["userid"]));

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        
                        tdChallanNo.InnerText = ds.Tables[0].Rows[0]["DocumentNumber"].ToString();
                        tdEntityCode.InnerText = ds.Tables[0].Rows[0]["EntityCode"].ToString();
                        tdNetworkName.InnerText = ds.Tables[0].Rows[0]["NetworkName"].ToString();
                        tdContactPerson.InnerText = ds.Tables[0].Rows[0]["ContactPerson"].ToString();
                        tdReceivedOn.InnerText = Convert.ToDateTime(ds.Tables[0].Rows[0]["DocumentDate"].ToString()).ToString("dd-MM-yyyy");
                        tdReceivedBy.InnerText = ds.Tables[0].Rows[0]["CREATE_BY"].ToString();
                        tdAssignedTo.InnerText = ds.Tables[0].Rows[0]["Technician"].ToString();
                        tdAssignedBy.InnerText = ds.Tables[0].Rows[0]["ASSIGN_BY"].ToString();
                        tdAssignedOn.InnerText = Convert.ToDateTime(ds.Tables[0].Rows[0]["ASSIGN_ON"].ToString()).ToString("dd-MM-yyyy");
                        tdChallanIDF.InnerText = ds.Tables[0].Rows[0]["ReceiptChallan_ID"].ToString();
                        output = ds.Tables[0].Rows[0]["ISSAVE"].ToString();

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
        public static List<srv_RcptChallanDtls> JobDetails(String model)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/serviceData/serviceDataList.aspx");
            List<srv_RcptChallanDtls> listStatues = new List<srv_RcptChallanDtls>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            proc.AddVarcharPara("@ACTION", 500, "ReceiptChalanDetailsList");
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddPara("@ReceiptChallan_ID", model);

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    string Action = "";

                    if (item["ADDSTATUS"].ToString() != "DN")
                    {
                        Action = Action + "<span class='badge badge-warning'>Pending</span>";
                    }
                    else
                    {
                        Action = Action + " <span class='badge badge-success'>Done</span> ";
                    }

                    if (rights.CanAdd)
                    {
                        Action = Action + "<span class='actionInput text-center'><i class='fa fa-pencil-square-o assig'  onclick='AssignServiceEntry(" + item["RcptChallanDtls_ID"].ToString() + ")' data-toggle='tooltip' data-placement='bottom' title='Edit' ></i> </span>";
                    }

                    if (rights.CanDelete)
                    {
                        Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["RcptChallanDtls_ID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Delete' ></i> </span>";
                    }

                    listStatues.Add(new srv_RcptChallanDtls
                    {
                        RcptChallan_ID = item["RcptChallan_ID"].ToString(),
                        RcptChallanDtls_ID = item["RcptChallanDtls_ID"].ToString(),
                        Rcpt_Model = item["Rcpt_Model"].ToString(),
                        SerialNo = item["SerialNo"].ToString(),
                        ProblemReported = item["ProblemReported"].ToString(),
                        ServiceAction = item["ServiceAction"].ToString(),
                        Component = item["Component"].ToString(),
                        Warrenty = item["Warrenty"].ToString(),
                        StockEntry = item["StockEntry"].ToString(),
                        Entry_Model = item["Entry_Model"].ToString(),
                        NewSerialNo = item["NewSerialNo"].ToString(),
                        ProblemFound = item["ProblemFound"].ToString(),
                        Remarks = item["Remarks"].ToString(),
                        WarrentyStatus = item["WarrentyStatus"].ToString(),
                        Action = Action,
                        ReturnReasonID = item["ReturnReasonID"].ToString(),
                        IsBillable = item["IsBillable"].ToString(),
                        ReturnReason = item["ReturnReason"].ToString(),
                        Billable = item["Billable"].ToString(),
                        Reason = item["Reason"].ToString(),
                        LevelID = item["LevelID"].ToString(),
                        LevelDesc = item["LevelDesc"].ToString()
                    });
                }
            }
            return listStatues;
        }

        [WebMethod]
        public static srv_RcptChallanDtls ServiceEntry(String ReceptDtlsID, String ReceptID)
        {
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            proc.AddVarcharPara("@ACTION", 500, "ServiceEntryAdd");
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddPara("@RcptChallanDtls_ID", ReceptDtlsID);
            proc.AddPara("@ReceiptChallan_ID", ReceptID);
            dt = proc.GetTable();


            srv_RcptChallanDtls ret = new srv_RcptChallanDtls();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ret.Rcpt_Model = item["Rcpt_Model"].ToString();
                    ret.SerialNo = item["SerialNo"].ToString();
                    ret.ProblemReported = item["ProblemReported"].ToString();
                    ret.ServiceAction = item["ServiceActionID"].ToString();
                    ret.Component = item["ComponentID"].ToString();
                    ret.Warrenty = item["Warrenty"].ToString();
                    ret.StockEntry = item["StockEntryID"].ToString();
                    ret.Entry_Model = item["Entry_ModelID"].ToString();
                    ret.NewSerialNo = item["NewSerialNo"].ToString();
                    ret.ProblemFound = item["ProblemFound_ID"].ToString();
                    ret.Remarks = item["Remarks"].ToString();
                    ret.WarrentyStatus = item["WarrentyStatusID"].ToString();
                    ret.ReturnReasonID = item["ReturnReasonID"].ToString();
                    ret.IsBillable = item["IsBillable"].ToString();
                    ret.ServiceActionText = item["ServiceAction"].ToString();
                    ret.Reason = item["Reason"].ToString();
                    // Rev Sanchita
                    ret.LevelID = item["LevelID"].ToString();
                    ret.LevelDesc = item["LevelDesc"].ToString();
                    // End of Rev Sanchita
                    break;
                }
            }
            return ret;
        }


        [WebMethod]
        public static string AddNewServiceEntry(srv_AddServiceEntryInput model)
        {
            string output = string.Empty;
            try
            {
                MasterSettings masterbl = new MasterSettings();
                string mastersettings = masterbl.GetSettings("StkAdjSrv");
                DataTable dt = new DataTable();

                DataTable dtable = new DataTable();
                dtable.Clear();
                dtable.Columns.Add("ComponentId", typeof(System.String));
                dtable.Columns.Add("Qty", typeof(System.String));

                if (model.com_qty != null && model.com_qty.Count > 0)
                {
                    foreach (var s2 in model.com_qty)
                    {
                        object[] trow = { s2.id.ToString(), s2.Value.ToString() };
                        dtable.Rows.Add(trow);
                    }
                }


               DataTable dt2 = null;
               dt2 = dtable;
               string []data = model.ComponentID.Split(',');
               for (int i = 0; i < data.Length; i++)
               {
                   if (dtable!=null && dtable.Rows.Count>0)
                   {
                       var row = dt2.AsEnumerable().FirstOrDefault(r => r.Field<string>("ComponentId") == data[i]);
                       if (row == null)
                       {
                           object[] trow = { data[i], "0" };
                           dtable.Rows.Add(trow);
                       }
                   }
                 
               }
                

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
                    proc.AddVarcharPara("@ACTION", 500, "AddNewServiceEntry");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(model.RcptChallan_ID));
                    proc.AddPara("@RcptChallanDtls_ID", Convert.ToString(model.RcptChallanDtls_ID));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@ServiceActionID", model.ServiceActionID);
                    proc.AddPara("@ComponentID", model.ComponentID);
                    proc.AddPara("@StockEntryID", model.StockEntryID);
                    proc.AddPara("@Entry_ModelID", Convert.ToString(model.Entry_ModelID));
                    proc.AddPara("@ProblemFound_ID", Convert.ToString(model.ProblemFound_ID));
                    proc.AddPara("@WarrentyStatusID", model.WarrentyStatusID);
                    proc.AddPara("@ServiceAction", Convert.ToString(model.ServiceAction));
                    proc.AddPara("@Component", Convert.ToString(model.Component));
                    proc.AddPara("@Warrenty", model.Warrenty);
                    proc.AddPara("@StockEntry", Convert.ToString(model.StockEntry));
                    proc.AddPara("@Entry_Model", Convert.ToString(model.Entry_Model));
                    proc.AddPara("@NewSerialNo", model.NewSerialNo);
                    proc.AddPara("@ProblemFound", Convert.ToString(model.ProblemFound));
                    proc.AddPara("@Remarks", Convert.ToString(model.Remarks));
                    proc.AddPara("@WarrentyStatus", model.WarrentyStatus);
                    proc.AddPara("@ReturnReasonID", model.ReturnReasonID);
                    proc.AddPara("@IsBillable", model.IsBillable);
                    proc.AddPara("@ReturnReason", model.ReturnReason);
                    proc.AddPara("@Billable", model.Billable);
                    proc.AddPara("@StockAdj_Require", mastersettings);
                    // Rev Sanchita
                    proc.AddPara("@LevelID", model.LevelID);
                    proc.AddPara("@LevelDesc", model.LevelDesc);
                    // End of Rev Sanchita

                    proc.AddPara("@Reason", model.Reason);
                    proc.AddPara("@UDT_ServiceComponents", dtable);

                    dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
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
        public static string SaveServiceEntry(String ReceptID, String EntryMode, String sendSMS)
        {
            string output = string.Empty;
            try
            {
                MasterSettings masterbl = new MasterSettings();
                string mastersettings = masterbl.GetSettings("StkAdjSrv");

                CommonBL SrvEntry = new CommonBL();
                string SendSMSforServiceManagement = SrvEntry.GetSystemSettingsResult("SendSMSforServiceManagement");
                if (!String.IsNullOrEmpty(SendSMSforServiceManagement))
                {
                    if (SendSMSforServiceManagement == "No")
                    {
                        sendSMS = "No";
                    }
                }
                else
                {
                    sendSMS = "No";
                }

                string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                int NoOfRowEffected = 0;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
                    if (EntryMode == "Add")
                    {
                        proc.AddVarcharPara("@ACTION", 500, "InsertNewServiceEntry");
                    }
                    else if (EntryMode == "Edit")
                    {
                        proc.AddVarcharPara("@ACTION", 500, "UpdateServiceEntry");
                    }
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceptID));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@COMPANYID", strCompanyID);
                    proc.AddPara("@FINYEAR", FinYear);
                    proc.AddPara("@sendSMS", sendSMS);
                    proc.AddPara("@StockAdj_Require", mastersettings);
                    DataTable dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        output = dt.Rows[0]["msg"].ToString();
                    }
                    //  output = "true";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        [WebMethod]
        public static List<Srv_ServiceEntryHistory> ServiceEntryHistory(String model, String DeviceNumber)
        {
            List<Srv_ServiceEntryHistory> listStatues = new List<Srv_ServiceEntryHistory>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            proc.AddVarcharPara("@ACTION", 500, "ServiceEntryHistory");
            proc.AddPara("@ModelNumber", model);
            proc.AddPara("@DeviceNumber", DeviceNumber);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    listStatues.Add(new Srv_ServiceEntryHistory
                    {
                        EntityCode = item["EntityCode"].ToString(),
                        ReceiptNo = item["DocumentNumber"].ToString(),
                        ServiceAction = item["SrvActionDesc"].ToString(),
                        Remarks = item["Remarks"].ToString(),
                        Billable = item["Billable"].ToString()
                    });
                }
            }
            return listStatues;
        }


        [WebMethod]
        public static string DeleteLineServiceEntry(String ReceptDtlsID, String ReceptID)
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
                    ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
                    proc.AddVarcharPara("@ACTION", 500, "DeleteLineServiceEntry");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceptID));
                    proc.AddPara("@RcptChallanDtls_ID", Convert.ToString(ReceptDtlsID));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@StockAdj_Require", mastersettings);
                    dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
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

        #region Component Populate

        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                ComponentTable = oDBEngine.GetDataTable("select sProducts_ID as ID,sProducts_Name,sProducts_Code,CASE WHEN sProduct_IsReplaceable=1 THEN 'Yes' ELSE 'No' END AS Replaceable from master_sproducts where isComponentService=1 AND prod.sProduct_Status!='D' order by sProducts_Name asc");
                if (ComponentTable.Rows.Count > 0)
                {
                    Session["JobSheetComponentData"] = ComponentTable;
                    lookup_Component.DataSource = ComponentTable;
                    lookup_Component.DataBind();
                }
                else
                {
                    Session["JobSheetComponentData"] = ComponentTable;
                    lookup_Component.DataSource = null;
                    lookup_Component.DataBind();
                }
            }
            else if (e.Parameter.Split('~')[0] == "SetComponentGrid")
            {
                string ModelDesc = e.Parameter.Split('~')[2];
                BindLookUp(ModelDesc);
                string userid = Convert.ToString(HttpContext.Current.Session["userid"]);
                DataTable Componentdt = new DataTable();
                string dtlsID = e.Parameter.Split('~')[1];
                string rcptid = hdnReceiptChalanID.Value;
                Componentdt = oDBEngine.GetDataTable("select Component_id from SRV_ServiceComponentEditUpdate where RcptChallan_ID=" + rcptid + " and RcptChallanDtls_ID=" + dtlsID + " and user_id=" + userid + "");
                if (Componentdt != null && Componentdt.Rows.Count > 0)
                {
                    lookup_Component.GridView.Selection.CancelSelection();
                    lookup_Component.GridView.Selection.CancelSelection();
                    foreach (DataRow item in Componentdt.Rows)
                    {
                        lookup_Component.GridView.Selection.SelectRowByKey(Convert.ToInt32(item["Component_id"].ToString()));
                    }
                }
            }
            else if (e.Parameter.Split('~')[0] == "BindComponentModelGrid")
            {
                DataTable ComponentTable = new DataTable();
                string ModelDesc = e.Parameter.Split('~')[1];
                ComponentTable = oDBEngine.GetDataTable("select prod.sProducts_ID as ID,prod.sProducts_Name,prod.sProducts_Code,CASE WHEN prod.sProduct_IsReplaceable=1 THEN 'Yes' ELSE 'No' END AS Replaceable from master_sproducts prod INNER JOIN SRV_ProductModelMap MAP ON MAP.Product_id=prod.sProducts_ID INNER JOIN Master_Model MDL ON MDL.ModelID=MAP.Model_id WHERE prod.isComponentService=1 AND MDL.ModelDesc='" + ModelDesc + "' AND prod.sProduct_Status!='D' order by sProducts_Name asc");
                if (ComponentTable.Rows.Count > 0)
                {
                    Session["JobSheetComponentData"] = ComponentTable;
                    lookup_Component.DataSource = ComponentTable;
                    lookup_Component.DataBind();
                }
                else
                {
                    Session["JobSheetComponentData"] = ComponentTable;
                    lookup_Component.DataSource = null;
                    lookup_Component.DataBind();
                }
            }
        }

        protected void lookup_Component_DataBinding(object sender, EventArgs e)
        {
            if (Session["JobSheetComponentData"] != null)
            {
                lookup_Component.DataSource = (DataTable)Session["JobSheetComponentData"];
            }
        }

        protected void BindLookUp(String ModelDesc)
        {
            DataTable ComponentTable = oDBEngine.GetDataTable("select prod.sProducts_ID as ID,prod.sProducts_Name,prod.sProducts_Code,CASE WHEN prod.sProduct_IsReplaceable=1 THEN 'Yes' ELSE 'No' END AS Replaceable from master_sproducts prod INNER JOIN SRV_ProductModelMap MAP ON MAP.Product_id=prod.sProducts_ID INNER JOIN Master_Model MDL ON MDL.ModelID=MAP.Model_id WHERE prod.isComponentService=1 AND MDL.ModelDesc='" + ModelDesc + "' AND prod.sProduct_Status!='D' order by sProducts_Name asc");
            lookup_Component.GridView.Selection.CancelSelection();

            lookup_Component.GridView.Selection.CancelSelection();
            lookup_Component.DataSource = ComponentTable;
            lookup_Component.DataBind();

            Session["JobSheetComponentData"] = ComponentTable;
        }

        #endregion

        [WebMethod]
        public static List<Srv_ServiceComponentList> ShowComponentQty(String ComponentID, String RcptChallan_ID, String RcptChallanDtls_ID, String EntryMode)
        {
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            if (EntryMode == "Add")
            {
                proc.AddVarcharPara("@ACTION", 500, "ComponentListAdd");
            }
            else if (EntryMode == "Edit")
            {
                proc.AddVarcharPara("@ACTION", 500, "ComponentListEdit");
            }
            proc.AddPara("@ComponentID", ComponentID);
            proc.AddPara("@ReceiptChallan_ID", Convert.ToString(RcptChallan_ID));
            proc.AddPara("@RcptChallanDtls_ID", Convert.ToString(RcptChallanDtls_ID));
            proc.AddPara("@USER_ID", user_id);
            dt = proc.GetTable();

            List<Srv_ServiceComponentList> listStatues = new List<Srv_ServiceComponentList>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    listStatues.Add(new Srv_ServiceComponentList
                    {
                        ProductCode = item["ProductCode"].ToString(),
                        ProductName = item["ProductName"].ToString(),
                        Replaceable = item["Replaceable"].ToString(),
                        TEXTBOX = item["TEXTBOX"].ToString(),
                        Productid = item["ID"].ToString()
                    });
                }
            }
            return listStatues;
        }
    }
}