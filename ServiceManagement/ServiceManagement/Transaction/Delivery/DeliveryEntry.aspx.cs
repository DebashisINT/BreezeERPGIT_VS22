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
using System.IO;
using System.Web.Mvc;
using BusinessLogicLayer;

namespace ServiceManagement.ServiceManagement.Transaction.Delivery
{
    public partial class DeliveryEntry : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SrvAssignJobBL obj = new SrvAssignJobBL();
        SrvDeliveryBL objDelivery = new SrvDeliveryBL();
        CommonBL ComBL = new CommonBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Delivery/DeliveryList.aspx");
            string AllowOnlinePrintinServiceManagement = ComBL.GetSystemSettingsResult("AllowOnlinePrintinServiceManagement");
            hdnOnlinePrint.Value = AllowOnlinePrintinServiceManagement;
            if (!IsPostBack)
            {
                hdnReceiptChallanID.Value = Request.QueryString["id"];
                // string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                // PopulateBranchByHierchy(userbranchHierachy);
                //ddlBranch.SelectedValue = Convert.ToString(Session["userbranchID"]);
                string user_id = Convert.ToString(Session["userid"]);
                DataTable dtusertyp = obj.GetUserType(user_id);
                if (dtusertyp != null && dtusertyp.Rows.Count > 0)
                {
                    hdnUserType.Value = dtusertyp.Rows[0]["contactType"].ToString();
                }

                if (Request.QueryString["key"] != "ADD")
                {
                    if (Request.QueryString["key"] == "Edit")
                    {
                        HeaderName.InnerText = "Edit Delivery ";
                        btnSaveExit.Style.Add("display", "!inline-block");
                        hdnDocumentType.Value = "Edit";
                    }
                    else if (Request.QueryString["key"] == "View")
                    {
                        HeaderName.InnerText = "View Delivery ";
                        btnSaveExit.Style.Add("display", "none");
                        hdnDocumentType.Value = "View";
                    }
                }
                else
                {
                    HeaderName.InnerText = "Add Delivery ";
                    btnSaveExit.Style.Add("display", "!inline-block");
                    hdnDocumentType.Value = "Add";
                }
            }
        }

        [WebMethod]
        public static srv_DeliveryHeadr ReceptDetails(String ReceiptID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            proc.AddVarcharPara("@ACTION", 500, "ShowReceiptChallanDetails");
            proc.AddPara("@ReceiptChallan_ID", ReceiptID);
            ds = proc.GetDataSet();
            srv_DeliveryHeadr ret = new srv_DeliveryHeadr();
            List<Srv_DeliveryDetails> DetailsList = new List<Srv_DeliveryDetails>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ret.DocumentNumber = item["DocumentNumber"].ToString();
                    ret.EntityCode = item["EntityCode"].ToString();
                    ret.NetworkName = item["NetworkName"].ToString();
                    ret.ContactPerson = item["ContactPerson"].ToString();
                    ret.ReceivedOn = item["Receivedon"].ToString();
                    ret.ReceivedBy = item["Receivedby"].ToString();
                    ret.AssignedTo = item["Technician"].ToString();
                    ret.AssignedBy = item["Assignedby"].ToString();
                    ret.AssignedOn = item["Assignedon"].ToString();
                    break;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        DetailsList.Add(new Srv_DeliveryDetails
                        {
                            Model = item["Model"].ToString(),
                            DeviceNumber = item["DeviceNumber"].ToString(),
                            ProblemDesc = item["ProblemDesc"].ToString(),
                            SrvActionDesc = item["SrvActionDesc"].ToString(),
                            NewSerialNo = item["NewSerialNo"].ToString(),
                            Warrenty = item["Warrenty"].ToString(),
                            CordAdaptor_Status = item["CordAdaptor_Status"].ToString(),
                            Remote_Status = item["Remote_Status"].ToString(),
                            DeviceType = item["DeviceType"].ToString()
                        });
                    }
                }
                ret.DetailsList = DetailsList;
            }
            return ret;
        }

        [WebMethod]
        public static string SaveDelivery(String DeliveredTo, String PhoneNo, String Remarks, String chkReceiptChallan, String ChallanRemarks, String ReceiptChallanDoc,
            String recept_id, String DocumentType, String DeliveryId)
        {
            string output = string.Empty;
            try
            {
                string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                DataTable dt = new DataTable();

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
                    if (DocumentType == "Add")
                    {
                        proc.AddVarcharPara("@ACTION", 500, "DeliveryInsert");
                    }
                    else if (DocumentType == "Edit")
                    {
                        proc.AddVarcharPara("@ACTION", 500, "DeliveryEdit");
                    }
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(recept_id));
                    proc.AddPara("@DeliveredTo", Convert.ToString(DeliveredTo));
                    proc.AddPara("@ContactNo", Convert.ToString(PhoneNo));
                    proc.AddPara("@Remarks", Convert.ToString(Remarks));
                    proc.AddPara("@ReceiptChallanRemarks", Convert.ToString(ChallanRemarks));
                    proc.AddPara("@AttachDocument", Convert.ToString(ReceiptChallanDoc));
                    proc.AddPara("@isRcptChallanNotReceived", Convert.ToInt32(chkReceiptChallan));
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@DeliveryId", DeliveryId);
                    proc.AddPara("@COMPANYID", strCompanyID);
                    proc.AddPara("@FINYEAR", FinYear);
                    dt = proc.GetTable();
                    if (dt!=null && dt.Rows.Count>0)
                    {
                        output = dt.Rows[0]["msg"].ToString() + "~" + dt.Rows[0]["status"].ToString() + "~" + dt.Rows[0]["ID"].ToString();
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
        public static srv_DeliveryHeadrView ViewDetailsEdit(String ReceiptID)
        {
            var DeliveryDoc = System.Configuration.ConfigurationSettings.AppSettings["DeliveryDoc"];
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_DeliveryDetails");
            proc.AddVarcharPara("@ACTION", 500, "DeliveryDetailsView");
            proc.AddPara("@ReceiptChallan_ID", ReceiptID);
            ds = proc.GetDataSet();
            srv_DeliveryHeadrView ret = new srv_DeliveryHeadrView();
            List<Srv_DeliveryDetails> DetailsList = new List<Srv_DeliveryDetails>();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ret.DocumentNumber = item["DocumentNumber"].ToString();
                    ret.EntityCode = item["EntityCode"].ToString();
                    ret.NetworkName = item["NetworkName"].ToString();
                    ret.ContactPerson = item["ContactPerson"].ToString();
                    ret.ReceivedOn = item["Receivedon"].ToString();
                    ret.ReceivedBy = item["Receivedby"].ToString();
                    ret.AssignedTo = item["Technician"].ToString();
                    ret.AssignedBy = item["Assignedby"].ToString();
                    ret.AssignedOn = item["Assignedon"].ToString();

                    ret.DeliveryTo = item["DeliveredTo"].ToString();
                    ret.ContactNo = item["ContactNo"].ToString();
                    ret.Remarks = item["Remarks"].ToString();
                    ret.isRcptChallanNotReceived = item["isRcptChallanNotReceived"].ToString();
                    ret.ReceiptRemarks = item["ReceiptChallanRemarks"].ToString();
                    ret.DeliveryID = item["DeliveryID"].ToString();
                    ret.Attachment = item["AttachDocument"].ToString();
                    break;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        DetailsList.Add(new Srv_DeliveryDetails
                        {
                            Model = item["Model"].ToString(),
                            DeviceNumber = item["DeviceNumber"].ToString(),
                            ProblemDesc = item["ProblemDesc"].ToString(),
                            SrvActionDesc = item["SrvActionDesc"].ToString(),
                            NewSerialNo = item["NewSerialNo"].ToString(),
                            Warrenty = item["Warrenty"].ToString(),
                            CordAdaptor_Status = item["CordAdaptor_Status"].ToString(),
                            Remote_Status = item["Remote_Status"].ToString(),
                            DeviceType = item["DeviceType"].ToString()
                        });
                    }
                }
                ret.DetailsList = DetailsList;
            }
            return ret;
        }
    }
}