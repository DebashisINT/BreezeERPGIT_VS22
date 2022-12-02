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

namespace ServiceManagement.ServiceManagement.Transaction.Warranty
{
    public partial class WarrantyEntry : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Warranty/WarrantyList.aspx");
            if (!IsPostBack)
            {
                if (Request.QueryString["Key"] != "Add")
                {
                    PopulateWarrantyUpdatebyID(Request.QueryString["id"]);
                    txtSerialNo.Attributes.Add("disabled", "disabled");
                    if (Request.QueryString["key"] == "Edit")
                    {
                        DivHeader.InnerHtml = "Edit Warranty";
                        hdnAddEditAction.Value = "Edit";
                        btnSearchSerial.Style.Add("display", "none");
                        hdnWarrantyUpdateId.Value = Request.QueryString["id"].ToString();
                    }

                    if (Request.QueryString["key"] == "View")
                    {
                        DivHeader.InnerHtml = "View Warranty";
                        btnUpdateWarranty.Style.Add("display", "none");
                        btnSearchSerial.Style.Add("display", "none");
                        hdnWarrantyUpdateId.Value = Request.QueryString["id"].ToString();
                    }
                }
                else
                {
                    DivHeader.InnerHtml = "Add Warranty";
                    hdnAddEditAction.Value = "Add";
                }
            }
        }

        private string PopulateWarrantyUpdatebyID(string warrantyID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    DataTable dt = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
                    proc.AddVarcharPara("@ACTION", 500, "EditWarranty");
                    proc.AddVarcharPara("@WarrantyUpdateID", 50, warrantyID);
                    dt = proc.GetTable();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        txtSerialNo.Value = dt.Rows[0]["DeviceNumber"].ToString();
                        txtReceiptChallanNo.Value = dt.Rows[0]["DocumentNumber"].ToString();
                        txtDate.Value = dt.Rows[0]["DocumentDate"].ToString();
                        txtEntityCode.Value = dt.Rows[0]["EntityCode"].ToString();
                        txtNetworkName.Value = dt.Rows[0]["NetworkName"].ToString();
                        txtdtlsSerialNo.Value = dt.Rows[0]["DeviceNumber"].ToString();
                        txtNewSerialNo.Value = dt.Rows[0]["NewSerialNo"].ToString();
                        txtWarrantyStatus.Value = dt.Rows[0]["WarrantyStatus"].ToString();
                        if (dt.Rows[0]["Warranty"].ToString() != "01-01-1900")
                        {
                            txtWarrantyDate.Value = dt.Rows[0]["Warranty"].ToString();
                        }
                        else
                        {
                            txtWarrantyDate.Value = "";
                        }
                        txtProblemFound.Value = dt.Rows[0]["ProblemFound"].ToString();
                        dtUpdateWarranty.Value = dt.Rows[0]["Warranty"].ToString();
                        // Mantis Issue 24290
                        ddlWarrentyStatus.Value = dt.Rows[0]["WarrentyStatusID"].ToString();
                        // End of Mantis Issue 24290
                        hdnWarrantyForEdit.Value = dt.Rows[0]["Warranty"].ToString();
                        txtRemarks.Value = dt.Rows[0]["Remarks"].ToString();

                        hdnReceiptChallanID.Value = dt.Rows[0]["ReceiptChallan_ID"].ToString();
                        hdnEntryID.Value = dt.Rows[0]["Entry_Id"].ToString();
                        hdnEntryDtlsId.Value = dt.Rows[0]["EntryDtls_Id"].ToString();
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
        public static srv_SerialNoDetails SerialNoFetch(String SerialNo)
        {
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
            proc.AddVarcharPara("@ACTION", 500, "FetchSerial");
            proc.AddVarcharPara("@DeviceNumber", 50, SerialNo);
            dt = proc.GetTable();


            srv_SerialNoDetails ret = new srv_SerialNoDetails();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ret.ReceiptChallan_ID = item["ReceiptChallan_ID"].ToString();
                    ret.Entry_Id = item["Entry_Id"].ToString();
                    ret.EntryDtls_Id = item["EntryDtls_Id"].ToString();
                    ret.DocumentNumber = item["DocumentNumber"].ToString();
                    ret.DocumentDate = item["DocumentDate"].ToString();
                    ret.EntityCode = item["EntityCode"].ToString();
                    ret.NetworkName = item["NetworkName"].ToString();
                    ret.DeviceNumber = item["DeviceNumber"].ToString();
                    ret.NewSerialNo = item["NewSerialNo"].ToString();
                    ret.WarrantyStatus = item["WarrantyStatus"].ToString();
                    ret.Warranty = item["Warranty"].ToString();
                    ret.ProblemFound = item["ProblemFound"].ToString();
                    ret.STATUS = item["STATUS"].ToString();
                    // Mantis Issue 24290
                    ret.WarrentyStatusID = item["WarrentyStatusID"].ToString();
                    // End of Mantis Issue 24290
                    break;
                }
            }
            return ret;
        }

        [WebMethod]
        public static String UpdateWarranty(srv_WarrantyUpdateInput data)
        {
            string ret = string.Empty;
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
            if (data.AddEditAction == "Add")
            {
                proc.AddVarcharPara("@ACTION", 500, "InsertWarranty");
            }
            else if (data.AddEditAction == "Edit")
            {
                proc.AddVarcharPara("@ACTION", 500, "UpdateWarranty");
                proc.AddPara("@WarrantyUpdateID", data.WarrantyUpdateId);
            }
            proc.AddPara("@ReceiptChallan_ID", data.ReceiptChallan_ID);
            proc.AddPara("@SrvEntryID", data.SrvEntryID);
            proc.AddPara("@SrvEntryDtlsId", data.SrvEntryDtlsId);
            proc.AddPara("@ReceiptChallanNo", data.ReceiptChallanNo);
            proc.AddPara("@SerialNo", data.SerialNo);
            proc.AddPara("@NewSerialNo", data.NewSerialNo);
            proc.AddPara("@Old_WarrantyDate", data.Old_WarrantyDate);
            proc.AddPara("@UpdateWarrantyDate", data.UpdateWarrantyDate);
            // Mantis Issue 24290
            proc.AddPara("@WarrentyStatusID", data.WarrentyStatusID);
            // End of Mantis Issue 24290
            proc.AddPara("@Remarks", data.Remarks);
            proc.AddPara("@UserId", user_id);

            int i = proc.RunActionQuery();
            if (i > 0)
            {
                ret = "Sucess";
            }

            return ret;
        }


        public class srv_SerialNoDetails
        {
            public String ReceiptChallan_ID { get; set; }
            public String Entry_Id { get; set; }
            public String EntryDtls_Id { get; set; }
            public String DocumentNumber { get; set; }
            public String DocumentDate { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String DeviceNumber { get; set; }
            public String NewSerialNo { get; set; }
            public String WarrantyStatus { get; set; }
            public String Warranty { get; set; }
            public String ProblemFound { get; set; }
            public String STATUS { get; set; }
            // Mantis Issue 24290
            public String WarrentyStatusID { get; set; }
            // End of Mantis Issue 24290
        }

        public class srv_WarrantyUpdateInput
        {
            public String ReceiptChallan_ID { get; set; }
            public String SrvEntryID { get; set; }
            public String SrvEntryDtlsId { get; set; }
            public String ReceiptChallanNo { get; set; }
            public String SerialNo { get; set; }
            public String NewSerialNo { get; set; }
            public String Old_WarrantyDate { get; set; }
            public String UpdateWarrantyDate { get; set; }
            public String Remarks { get; set; }
            public String WarrantyUpdateId { get; set; }
            public String AddEditAction { get; set; }
            // Mantis Issue 24290
            public String WarrentyStatusID { get; set; }
            // End of Mantis Issue 24290
        }
    }
}