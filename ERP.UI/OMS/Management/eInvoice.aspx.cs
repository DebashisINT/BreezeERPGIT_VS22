﻿#region//====================================================Revision History=========================================================================
// 1.0   v2.0.37	Priti	13-03-2023	0025686:Eway Bill Cancel not working for Transit Sales Invoice & Credit Note
// 2.0   v2.0.38	Priti	18-04-2023	0025725:If an Eway Bill or IRN is cancelled from the Portal directly need to update the ERP tables accordingly next time the Document is
// 3.0   v2.0.40	Priti	10-10-2023	0026890:Error generating IRN
// 4.0   v2.0.41	Priti	10-11-2023	0026981:Need to Restrict IRN Cancellation for Sales Return(Credit Note) if the Credit Note is Adjusted
// 5.0   v2.0.41	Priti	16-11-2023	0027000:EInvoice Changes to be done due to the change in the Flynn Version from Ver 1.0 to Ver 3.0 by Vayana
// 6.0   v2.0.43	Priti	05-02-2024	0027231: An error is showing while generating IRN
// 7.0   v2.0.43	Priti	18-03-2024	0027317: An error is showing while generating IRN
//====================================================End Revision History=====================================================================
#endregion

using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
// Mantis Issuse 24030 (24/05/2021)
using BusinessLogicLayer.EmailDetails;
using EntityLayer.MailingSystem;
using UtilityLayer;
using System.Data.SqlClient;
using System.Threading;
using Newtonsoft.Json.Linq;

// End of Mantis Issuse 24030 (24/05/2021)

namespace ERP.OMS.Management
{
    public partial class eInvoice : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/eInvoice.aspx");
            if (!IsPostBack)
            {
                string EinvoiceType = ConfigurationManager.AppSettings["EinvoiceType"];
                //rights.CanIRN
                //rights.CanEWayBill



                #region Set Today Date
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                //ASPxDateEdit14.Date = DateTime.Now;
                //ASPxDateEdit13.Date = DateTime.Now;
                //ASPxDateEdit12.Date = DateTime.Now;
                //ASPxDateEdit11.Date = DateTime.Now;
                ASPxDateEdit20.Date = DateTime.Now;
                ASPxDateEdit19.Date = DateTime.Now;
                ASPxDateEdit10.Date = DateTime.Now;
                ASPxDateEdit9.Date = DateTime.Now;
                ASPxDateEdit8.Date = DateTime.Now;
                ASPxDateEdit7.Date = DateTime.Now;
                ASPxDateEdit18.Date = DateTime.Now;
                ASPxDateEdit17.Date = DateTime.Now;
                ASPxDateEdit6.Date = DateTime.Now;
                ASPxDateEdit5.Date = DateTime.Now;
                ASPxDateEdit4.Date = DateTime.Now;
                ASPxDateEdit3.Date = DateTime.Now;
                ASPxDateEdit22.Date = DateTime.Now;
                ASPxDateEdit21.Date = DateTime.Now;
                ASPxDateEdit16.Date = DateTime.Now;
                ASPxDateEdit15.Date = DateTime.Now;
                ASPxDateEdit2.Date = DateTime.Now;
                ASPxDateEdit1.Date = DateTime.Now;

                ASPxDateEdit21.Date = DateTime.Now;
                ASPxDateEdit22.Date = DateTime.Now;
                ASPxDateEdit23.Date = DateTime.Now;
                ASPxDateEdit24.Date = DateTime.Now;
                ASPxDateEdit25.Date = DateTime.Now;
                ASPxDateEdit26.Date = DateTime.Now;
                ASPxDateEdit27.Date = DateTime.Now;
                ASPxDateEdit28.Date = DateTime.Now;
                ASPxDateEdit11.Date = DateTime.Now;
                ASPxDateEdit12.Date = DateTime.Now;
                ASPxDateEdit13.Date = DateTime.Now;
                ASPxDateEdit14.Date = DateTime.Now;



                #endregion



                string userbranchhierchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
                DataTable branchtable = posSale.getBranchListByHierchyEInvoice(userbranchhierchy);
                DBEngine objDB = new DBEngine();
                //DataTable branchtable = objDB.GetDataTable("select * from dbo.fn_GetEinvoiceBranchDetails()");

                cmbBranchfilter.DataSource = branchtable;
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataBind();
                cmbBranchfilter.SelectedIndex = 0;

                brPendinSI.DataSource = branchtable;
                brPendinSI.ValueField = "branch_id";
                brPendinSI.TextField = "branch_description";
                brPendinSI.DataBind();
                brPendinSI.SelectedIndex = 0;

                brCancelledSI.DataSource = branchtable;
                brCancelledSI.ValueField = "branch_id";
                brCancelledSI.TextField = "branch_description";
                brCancelledSI.DataBind();
                brCancelledSI.SelectedIndex = 0;


                brPendingTSI.DataSource = branchtable;
                brPendingTSI.ValueField = "branch_id";
                brPendingTSI.TextField = "branch_description";
                brPendingTSI.DataBind();
                brPendingTSI.SelectedIndex = 0;

                brTSI.DataSource = branchtable;
                brTSI.ValueField = "branch_id";
                brTSI.TextField = "branch_description";
                brTSI.DataBind();
                brTSI.SelectedIndex = 0;


                brCancelTSI.DataSource = branchtable;
                brCancelTSI.ValueField = "branch_id";
                brCancelTSI.TextField = "branch_description";
                brCancelTSI.DataBind();
                brCancelTSI.SelectedIndex = 0;

                crBR.DataSource = branchtable;
                crBR.ValueField = "branch_id";
                crBR.TextField = "branch_description";
                crBR.DataBind();
                crBR.SelectedIndex = 0;


                brPendingCR.DataSource = branchtable;
                brPendingCR.ValueField = "branch_id";
                brPendingCR.TextField = "branch_description";
                brPendingCR.DataBind();
                brPendingCR.SelectedIndex = 0;


                brCancelBR.DataSource = branchtable;
                brCancelBR.ValueField = "branch_id";
                brCancelBR.TextField = "branch_description";
                brCancelBR.DataBind();
                brCancelBR.SelectedIndex = 0;

                brewaybillSI.DataSource = branchtable;
                brewaybillSI.ValueField = "branch_id";
                brewaybillSI.TextField = "branch_description";
                brewaybillSI.DataBind();
                brewaybillSI.SelectedIndex = 0;

                brewaybillSI.DataSource = branchtable;
                brewaybillSI.ValueField = "branch_id";
                brewaybillSI.TextField = "branch_description";
                brewaybillSI.DataBind();
                brewaybillSI.SelectedIndex = 0;


                brCancelEwaybill.DataSource = branchtable;
                brCancelEwaybill.ValueField = "branch_id";
                brCancelEwaybill.TextField = "branch_description";
                brCancelEwaybill.DataBind();
                brCancelEwaybill.SelectedIndex = 0;

                brewaybillTSI.DataSource = branchtable;
                brewaybillTSI.ValueField = "branch_id";
                brewaybillTSI.TextField = "branch_description";
                brewaybillTSI.DataBind();
                brewaybillTSI.SelectedIndex = 0;

                brCancelewaybillTSI.DataSource = branchtable;
                brCancelewaybillTSI.ValueField = "branch_id";
                brCancelewaybillTSI.TextField = "branch_description";
                brCancelewaybillTSI.DataBind();
                brCancelewaybillTSI.SelectedIndex = 0;

                brewaybillSR.DataSource = branchtable;
                brewaybillSR.ValueField = "branch_id";
                brewaybillSR.TextField = "branch_description";
                brewaybillSR.DataBind();
                brewaybillSR.SelectedIndex = 0;

                brCancelewaybillSR.DataSource = branchtable;
                brCancelewaybillSR.ValueField = "branch_id";
                brCancelewaybillSR.TextField = "branch_description";
                brCancelewaybillSR.DataBind();
                brCancelewaybillSR.SelectedIndex = 0;


            }
        }
        public DataTable getBranchListByHierchy(string userbranchhierchy)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
            proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
            ds = proc.GetTable();
            return ds;
        }
        [WebMethod]
        public static object generateMultiEinvoiceJSON()
        {
            List<EinvoiceModel> obj = new List<EinvoiceModel>();

            obj = (List<EinvoiceModel>)HttpContext.Current.Session["obj"];
            return obj;
        }

        [WebMethod]
        public static object UpdatePin(string PINtoPINDistance, string ID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_UpdatePin");
            proc.AddVarcharPara("@Action", 50, "UpdatePin");
            proc.AddVarcharPara("@DOC_ID", 20, ID);
            proc.AddVarcharPara("@Distance", 1000, PINtoPINDistance);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        [WebMethod]
        public static object UpdatePinTSI(string PINtoPINDistance, string ID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_UpdatePin");
            proc.AddVarcharPara("@Action", 50, "UpdatePinTSI");
            proc.AddVarcharPara("@DOC_ID", 20, ID);
            proc.AddVarcharPara("@Distance", 1000, PINtoPINDistance);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        [WebMethod]
        public static object GetDistancePin(string ID)
        {
            string PINtoPINDistance = "";
            ProcedureExecute proc = new ProcedureExecute("PRC_UpdatePin");
            proc.AddVarcharPara("@Action", 50, "GetDistancePin");
            proc.AddVarcharPara("@DOC_ID", 20, ID);
            DataTable dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                PINtoPINDistance = Convert.ToString(dt.Rows[0]["Transporter_Distance"]);
            }

            return Convert.ToString(PINtoPINDistance);

        }
        [WebMethod]
        public static object GetDistancePinTSI(string ID)
        {
            string PINtoPINDistance = "";
            ProcedureExecute proc = new ProcedureExecute("PRC_UpdatePin");
            proc.AddVarcharPara("@Action", 50, "GetDistancePinTSI");
            proc.AddVarcharPara("@DOC_ID", 20, ID);
            DataTable dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                PINtoPINDistance = Convert.ToString(dt.Rows[0]["Transporter_Distance"]);
            }

            return Convert.ToString(PINtoPINDistance);

        }

        [WebMethod]
        //Rev 4.0
        //public static object CancelIRN(string irn, string type, string cancelReason, string cancelRemarks)
        public static object CancelIRN(string irn, string type, string cancelReason, string cancelRemarks, string InvoiceId)
        //Rev 4.0 End
        {


            string output = "";

            if (type == "SILine")
            {

                CancelDetails objCancelDetails = new CancelDetails();
                objCancelDetails.Irn = irn;
                objCancelDetails.CnlRem = cancelRemarks;
                objCancelDetails.CnlRsn = cancelReason;

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_SALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]); ;
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                //Rev 4.0
                string SRIDISExistsInvoice = "0";
                DataTable SRIDISExistsdt = GetCHECKAdjustment(InvoiceId, "CHECKSALESINVOICE");
                if (SRIDISExistsdt.Rows.Count > 0)
                {
                    SRIDISExistsInvoice = Convert.ToString(SRIDISExistsdt.Rows[0]["ISEXIST"]);
                }
                if (SRIDISExistsInvoice != "0")
                {
                    output = "The Document is Adjusted, Please delete the adjustment and then Proceed with IRN Cancellation";
                }
                //Rev 4.0 End

                else
                {
                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                    EinvoiceToken.Expiry = date;
                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            DBEngine objDB = new DBEngine();
                            string id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_ID FROM TBL_TRANS_SALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_CANCEL'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','0','" + err.Message + "')");
                            }
                            output = "Error occurs while IRN Cancellation.";
                        }
                    }
                    try
                    {
                        //Rev 5.0
                        //Rev Cancel IRN (v1.0)
                        //IRN objIRN = new IRN();
                        //Rev Cancel IRN (v1.0) End
                        //Rev Cancel IRN (v3.0) 
                        IRNV3 objIRN = new IRNV3();
                        //Rev Cancel IRN (v3.0) End
                        //Rev 5.0 End
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objCancelDetails, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0
                            //Rev Cancel IRN (v1.0)
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            //Rev Cancel IRN (v1.0) End
                            //Rev Cancel IRN (v3.0)
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                            //Rev Cancel IRN (v3.0) End
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnCancelUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                //Rev 5.0
                                //Rev Cancel IRN (v3.0)
                                objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                                if (Convert.ToString(objIRN.status) == "1")
                                {

                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRN.data.CancelDate + "' WHERE Irn='" + objIRN.data.Irn + "'");
                                    string id = Convert.ToString(objDb.GetDataTable("select invoice_id from TBL_TRANS_SALESINVOICE WHERE Irn='" + objIRN.data.Irn + "'").Rows[0][0]);
                                    objDb.GetDataTable("EXEC PRC_CANCELIRNSI " + id + "");
                                    output = "IRN Cancelled successfully.";
                                }
                                //Rev Cancel IRN (v3.0) End
                                //Rev Cancel IRN (v1.0) 
                                //objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                //{
                                //    // Deserialization from JSON  
                                //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelIRNOutput));
                                //    CancelIRNOutput objIRNDetails = (CancelIRNOutput)deserializer.ReadObject(ms);
                                //    DBEngine objDb = new DBEngine();
                                //    objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRNDetails.CancelDate + "' WHERE Irn='" + objIRNDetails.Irn + "'");
                                //    string id = Convert.ToString(objDb.GetDataTable("select invoice_id from TBL_TRANS_SALESINVOICE WHERE Irn='" + objIRNDetails.Irn + "'").Rows[0][0]);
                                //    objDb.GetDataTable("EXEC PRC_CANCELIRNSI " + id + "");
                                //    output = "IRN Cancelled successfully.";
                                //}
                                //Rev Cancel IRN (v1.0) End
                                //Rev 5.0 End

                            }
                            else
                            {
                                //REV 5.0
                                ////Rev Cancel IRN (v1.0)
                                //EinvoiceError err = new EinvoiceError();
                                ////Rev Cancel IRN (v1.0) End
                                //Rev Cancel IRN (v3.0)
                                EinvoiceErrorV3 err = new EinvoiceErrorV3();
                                //Rev Cancel IRN (v3.0) End
                                //REV 5.0 END
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);

                                //REV 5.0
                                ////Rev Cancel IRN (v1.0)
                                //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                ////Rev Cancel IRN (v1.0) End
                                //Rev Cancel IRN (v3.0)
                                err = response.Content.ReadAsAsync<EinvoiceErrorV3>().Result;
                                //Rev Cancel IRN (v3.0) End
                                //REV 5.0 END
                                DBEngine objDB = new DBEngine();
                                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_SALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);

                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' and ERROR_TYPE='IRN_CANCEL'");

                                //REV 5.0
                                //Rev Cancel IRN (v3.0)
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.details)
                                    {                                        
                                        if (item.ErrorCode == "9999")
                                        {
                                            DBEngine objDb = new DBEngine();
                                            objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + DateTime.Now.ToString("MM/dd/yyyy H:mm") + "' WHERE Irn='" + objCancelDetails.Irn + "'");
                                            objDb.GetDataTable("EXEC PRC_CANCELIRNSI " + id + "");
                                            output = "IRN Cancelled successfully.";
                                        }                                      
                                        else
                                        {
                                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                            output = "Error occurs while IRN Cancellation.";
                                        }
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','" + "0" + "','" + item + "')");
                                    }
                                    output = "Error occurs while IRN Cancellation.";
                                }
                                //Rev Cancel IRN (v1.0) End

                                //Rev Cancel IRN (v3.0)
                                //if (err.error.type != "ClientRequest")
                                //{
                                //    foreach (errorlog item in err.error.args.irp_error.details)
                                //    {
                                //        //Rev 2.0
                                //        if (item.ErrorCode == "9999")
                                //        {
                                //            DBEngine objDb = new DBEngine();
                                //            objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + DateTime.Now.ToString("MM/dd/yyyy H:mm") + "' WHERE Irn='" + objCancelDetails.Irn + "'");
                                //            objDb.GetDataTable("EXEC PRC_CANCELIRNSI " + id + "");
                                //            output = "IRN Cancelled successfully.";

                                //        }
                                //        //Rev 2.0 End
                                //        else
                                //        {
                                //            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                //            output = "Error occurs while IRN Cancellation.";

                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                //    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                //    foreach (string item in cErr.error.args.errors)
                                //    {
                                //        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','" + "0" + "','" + item + "')");
                                //    }

                                //    output = "Error occurs while IRN Cancellation.";

                                //}
                                //Rev Cancel IRN (v1.0) End
                                //REV 5.0 END
                            }


                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        string id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_ID FROM TBL_TRANS_SALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','0','" + err.Message + "')");
                        }
                        output = "Error occurs while IRN Cancellation.";
                    }
                }
            }

            return output;
        }

        [WebMethod]
        //Rev 4.0
        //public static object CancelIRNTSI(string irn, string type, string cancelReason, string cancelRemarks)
        public static object CancelIRNTSI(string irn, string type, string cancelReason, string cancelRemarks, string InvoiceId)
        //Rev 4.0 End
        {


            string output = "";

            if (type == "SILineTSI")
            {

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];

                CancelDetails objCancelDetails = new CancelDetails();
                objCancelDetails.Irn = irn;
                objCancelDetails.CnlRem = cancelRemarks;
                objCancelDetails.CnlRsn = cancelReason;

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]); ;
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                //Rev 4.0
                string SRIDISExistsInvoice = "0";
                DataTable SRIDISExistsdt = GetCHECKAdjustment(InvoiceId, "CHECKTRANSITSALESINVOICE");
                if (SRIDISExistsdt.Rows.Count > 0)
                {
                    SRIDISExistsInvoice = Convert.ToString(SRIDISExistsdt.Rows[0]["ISEXIST"]);
                }
                if (SRIDISExistsInvoice != "0")
                {
                    output = "The Document is Adjusted, Please delete the adjustment and then Proceed with IRN Cancellation";
                }
                //Rev 4.0 End

                else
                {

                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                    EinvoiceToken.Expiry = date;
                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            DBEngine objDB = new DBEngine();
                            string id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_CANCEL'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','0','" + err.Message + "')");
                            }
                            output = "Error occurs while IRN Cancellation.";
                        }
                    }
                    try
                    {
                        //Rev 5.0
                        //Rev Cancel IRN (v1.0)
                        //IRN objIRN = new IRN();
                        //Rev Cancel IRN (v1.0) End
                        //Rev Cancel IRN (v3.0) 
                        IRNV3 objIRN = new IRNV3();
                        //Rev Cancel IRN (v3.0) End
                        //Rev 5.0 End
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objCancelDetails, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0
                            //Rev Cancel IRN (v1.0)
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            //Rev Cancel IRN (v1.0) End
                            //Rev Cancel IRN (v3.0)
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                            //Rev Cancel IRN (v3.0) End
                            //Rev 5.0 End

                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnCancelUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                //Rev 5.0
                                //Rev Cancel IRN (v3.0)
                                objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                                if (Convert.ToString(objIRN.status) == "1")
                                {

                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRN.data.CancelDate + "' WHERE Irn='" + objIRN.data.Irn + "'");
                                    string id = Convert.ToString(objDb.GetDataTable("select invoice_id from TBL_TRANS_TRANSITSALESINVOICE WHERE Irn='" + objIRN.data.Irn + "'").Rows[0][0]);
                                    objDb.GetDataTable("EXEC PRC_CANCELIRNTSI " + id + "");
                                    output = "IRN Cancelled successfully.";
                                }
                                //Rev Cancel IRN (v3.0) End

                                //Rev Cancel IRN (v1.0)
                                //objIRN = response.Content.ReadAsAsync<IRN>().Result;
                                //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                //{
                                //    // Deserialization from JSON  
                                //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelIRNOutput));
                                //    CancelIRNOutput objIRNDetails = (CancelIRNOutput)deserializer.ReadObject(ms);
                                //    DBEngine objDb = new DBEngine();
                                //    objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRNDetails.CancelDate + "' WHERE Irn='" + objIRNDetails.Irn + "'");

                                //    string id = Convert.ToString(objDb.GetDataTable("select invoice_id from TBL_TRANS_TRANSITSALESINVOICE WHERE Irn='" + objIRNDetails.Irn + "'").Rows[0][0]);
                                //    objDb.GetDataTable("EXEC PRC_CANCELIRNTSI " + id + "");


                                //    output = "IRN Cancelled successfully.";
                                //}
                                //Rev Cancel IRN (v1.0) END
                                //Rev 5.0 END

                            }
                            else
                            {
                                //REV 5.0
                                ////Rev Cancel IRN (v1.0)
                                //EinvoiceError err = new EinvoiceError();
                                ////Rev Cancel IRN (v1.0) End
                                //Rev Cancel IRN (v3.0)
                                EinvoiceErrorV3 err = new EinvoiceErrorV3();
                                //Rev Cancel IRN (v3.0) End
                                //REV 5.0 END

                                
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                //REV 5.0
                                ////Rev Cancel IRN (v1.0)
                                //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                ////Rev Cancel IRN (v1.0) End
                                //Rev Cancel IRN (v3.0)
                                err = response.Content.ReadAsAsync<EinvoiceErrorV3>().Result;
                                //Rev Cancel IRN (v3.0) End
                                //REV 5.0 END

                                DBEngine objDB = new DBEngine();
                                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);

                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' and ERROR_TYPE='IRN_CANCEL'");
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','" + "0" + "','" + item + "')");
                                    }
                                }

                                output = "Error occurs while IRN Cancellation.";

                                //Rev Cancel IRN (v1.0)
                                //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                //DBEngine objDB = new DBEngine();
                                //string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);

                                //objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' and ERROR_TYPE='IRN_CANCEL'");
                                //if (err.error.type != "ClientRequest")
                                //{
                                //    foreach (errorlog item in err.error.args.irp_error.details)
                                //    {
                                //        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                //    }
                                //}
                                //else
                                //{
                                //    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                //    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                //    foreach (string item in cErr.error.args.errors)
                                //    {
                                //        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','" + "0" + "','" + item + "')");
                                //    }
                                //}

                                //output = "Error occurs while IRN Cancellation.";
                                //Rev Cancel IRN (v1.0) END
                            }


                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        string id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','0','" + err.Message + "')");
                        }
                        output = "Error occurs while IRN Cancellation.";
                    }
                }
            }

            return output;
        }

        [WebMethod]

        //Rev 4.0 
        //public static object CancelIRNSR(string irn, string type, string cancelReason, string cancelRemarks)
        public static object CancelIRNSR(string irn, string type, string cancelReason, string cancelRemarks, string SRId)
        //Rev 4.0 End
        {
            string output = "";

            if (type == "SILineSR")
            {

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];


                CancelDetails objCancelDetails = new CancelDetails();
                objCancelDetails.Irn = irn;
                objCancelDetails.CnlRem = cancelRemarks;
                objCancelDetails.CnlRsn = cancelReason;

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT RETURN_BranchId FROM TBL_TRANS_SALESRETURN WHERE Irn='" + irn + "'").Rows[0][0]); ;

                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                //Rev 4.0
                string SRIDISExistsInvoice = "0";
                DataTable SRIDISExistsdt = GetCHECKAdjustment(SRId, "CHECKCREDITNOTE");
                if (SRIDISExistsdt.Rows.Count > 0)
                {
                    SRIDISExistsInvoice = Convert.ToString(SRIDISExistsdt.Rows[0]["ISEXIST"]);
                }
                if (SRIDISExistsInvoice != "0")
                {
                    output = "The Document is Adjusted, Please delete the adjustment and then Proceed with IRN Cancellation";
                }
                //Rev 4.0 End
                else
                {
                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                    EinvoiceToken.Expiry = date;

                                }
                            }
                        }

                        catch (AggregateException err)
                        {
                            DBEngine objDB = new DBEngine();
                            string id = Convert.ToString(objDB.GetDataTable("SELECT RETURN_ID FROM TBL_TRANS_SALESRETURN WHERE Irn='" + irn + "'").Rows[0][0]);
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_CANCEL'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','0','" + err.Message + "')");
                            }
                            output = "Error occurs while IRN Cancellation.";
                        }
                    }

                    try
                    {
                        //Rev 5.0
                        //Rev Cancel IRN (v1.0)
                        //IRN objIRN = new IRN();
                        //Rev Cancel IRN (v1.0) End
                        //Rev Cancel IRN (v3.0) 
                        IRNV3 objIRN = new IRNV3();
                        //Rev Cancel IRN (v3.0) End
                        //Rev 5.0 End
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objCancelDetails, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                           
                            //Rev 5.0
                            //Rev Cancel IRN (v1.0)
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            //Rev Cancel IRN (v1.0) End
                            //Rev Cancel IRN (v3.0)
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                            //Rev Cancel IRN (v3.0) End
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnCancelUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                //Rev Cancel IRN (v1.0)
                                objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                                if (Convert.ToString(objIRN.status) == "1")
                                {
                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRN.data.CancelDate + "' WHERE Irn='" + objIRN.data.Irn + "'");

                                    string id = Convert.ToString(objDb.GetDataTable("select return_id from TBL_TRANS_SALESreturn WHERE Irn='" + objIRN.data.Irn + "'").Rows[0][0]);
                                    objDb.GetDataTable("EXEC PRC_CANCELIRNSR " + id + "");
                                    output = "IRN Cancelled successfully.";
                                }
                                //Rev Cancel IRN (v1.0)
                                //Rev Cancel IRN (v3.0)
                                //objIRN = response.Content.ReadAsAsync<IRN>().Result;
                                //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                //{
                                //    // Deserialization from JSON  
                                //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelIRNOutput));
                                //    CancelIRNOutput objIRNDetails = (CancelIRNOutput)deserializer.ReadObject(ms);
                                //    DBEngine objDb = new DBEngine();
                                //    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRNDetails.CancelDate + "' WHERE Irn='" + objIRNDetails.Irn + "'");

                                //    string id = Convert.ToString(objDb.GetDataTable("select return_id from TBL_TRANS_SALESreturn WHERE Irn='" + objIRNDetails.Irn + "'").Rows[0][0]);
                                //    objDb.GetDataTable("EXEC PRC_CANCELIRNSR " + id + "");
                                //    output = "IRN Cancelled successfully.";
                                //}
                                //Rev Cancel IRN (v3.0) End
                            }
                            else
                            {
                                //REV 5.0
                                ////Rev Cancel IRN (v1.0)
                                //EinvoiceError err = new EinvoiceError();
                                ////Rev Cancel IRN (v1.0) End
                                //Rev Cancel IRN (v3.0)
                                EinvoiceErrorV3 err = new EinvoiceErrorV3();
                                //Rev Cancel IRN (v3.0) End
                                //REV 5.0 END
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                //REV 5.0
                                ////Rev Cancel IRN (v1.0)
                                //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                ////Rev Cancel IRN (v1.0) End
                                //Rev Cancel IRN (v3.0)
                                err = response.Content.ReadAsAsync<EinvoiceErrorV3>().Result;
                                //Rev Cancel IRN (v3.0) End
                                //REV 5.0 END

                                
                                DBEngine objDB = new DBEngine();
                                string id = Convert.ToString(objDB.GetDataTable("SELECT RETURN_ID FROM TBL_TRANS_SALESRETURN WHERE Irn='" + irn + "'").Rows[0][0]);

                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' and ERROR_TYPE='IRN_CANCEL'");
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','" + "0" + "','" + item + "')");
                                    }
                                }
                                ////Rev Cancel IRN (v1.0)
                                //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                //DBEngine objDB = new DBEngine();
                                //string id = Convert.ToString(objDB.GetDataTable("SELECT RETURN_ID FROM TBL_TRANS_SALESRETURN WHERE Irn='" + irn + "'").Rows[0][0]);

                                //objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' and ERROR_TYPE='IRN_CANCEL'");
                                //if (err.error.type != "ClientRequest")
                                //{
                                //    foreach (errorlog item in err.error.args.irp_error.details)
                                //    {
                                //        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                //    }
                                //}
                                //else
                                //{
                                //    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                //    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                //    foreach (string item in cErr.error.args.errors)
                                //    {
                                //        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','" + "0" + "','" + item + "')");
                                //    }
                                //}
                                ////Rev Cancel IRN (v1.0) End
                                output = "Error occurs while IRN Cancellation.";
                            }
                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        string id = Convert.ToString(objDB.GetDataTable("SELECT RETURN_ID FROM TBL_TRANS_SALESRETURN WHERE Irn='" + irn + "'").Rows[0][0]);
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','0','" + err.Message + "')");
                        }
                        output = "Error occurs while IRN Cancellation.";
                    }
                }
            }

            return output;
        }
        //REV 4.0
        public static DataTable GetCHECKAdjustment(string DOCID, string actiontype)
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_EInvoiceDetails");
            proc.AddVarcharPara("@Action", 100, actiontype);
            proc.AddIntegerPara("@DOC_ID", Convert.ToInt32(DOCID));
            return proc.GetTable();
        }
        //REV 4.0 END

        //[WebMethod]
        //public static object CancelIRN(string irn, string type, string cancelReason, string cancelRemarks)
        //{
        //    string output = "";
        //    if (type == "SILine")
        //    {
        //        CancelDetails objCancelDetails = new CancelDetails();
        //        objCancelDetails.Irn = irn;
        //        objCancelDetails.CnlRem = cancelRemarks;
        //        objCancelDetails.CnlRsn = cancelReason;

        //        string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
        //        string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
        //        string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
        //        string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
        //        string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];

        //        DBEngine objDBEngineCredential = new DBEngine();
        //        string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_SALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]); ;
        //        DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
        //        string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
        //        string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
        //        string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);






        //        authtokensOutput authObj = new authtokensOutput();
        //        if (DateTime.Now > EinvoiceToken.Expiry)
        //        {
        //            try
        //            {
        //                using (HttpClient client = new HttpClient())
        //                {
        //                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
        //                                       SecurityProtocolType.Tls11 |
        //                                       SecurityProtocolType.Tls12;
        //                    authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
        //                    var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
        //                    var stringContent = new StringContent(json);
        //                    var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
        //                    var response = client.PostAsync(IrnBaseURL, stringContent).Result;

        //                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //                    {
        //                        var jsonString = response;
        //                        var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
        //                        authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
        //                        EinvoiceToken.token = authObj.data.token;
        //                        long unixDate = authObj.data.expiry;
        //                        DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //                        DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

        //                        EinvoiceToken.Expiry = date;
        //                    }
        //                }
        //            }
        //            catch (AggregateException err)
        //            {
        //                DBEngine objDB = new DBEngine();
        //                string id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_ID FROM TBL_TRANS_SALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);
        //                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_CANCEL'");

        //                foreach (var errInner in err.InnerExceptions)
        //                {
        //                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','0','" + err.Message + "')");
        //                }
        //                output = "Error occurs while IRN Cancellation.";
        //            }
        //        }
        //        try
        //        {
        //            IRN objIRN = new IRN();
        //            using (var client = new HttpClient())
        //            {
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
        //                SecurityProtocolType.Tls11 |
        //                SecurityProtocolType.Tls12;
        //                client.DefaultRequestHeaders.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                var json = JsonConvert.SerializeObject(objCancelDetails, Formatting.Indented);
        //                var stringContent = new StringContent(json);
        //                client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
        //                client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
        //                client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
        //                client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
        //                client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
        //                client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
        //                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
        //                // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
        //                var response = client.PostAsync(IrnCancelUrl, stringContent).Result;

        //                if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //                {
        //                    var jsonString = response.Content.ReadAsStringAsync().Result;
        //                    objIRN = response.Content.ReadAsAsync<IRN>().Result;

        //                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
        //                    {
        //                        // Deserialization from JSON  
        //                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelIRNOutput));
        //                        CancelIRNOutput objIRNDetails = (CancelIRNOutput)deserializer.ReadObject(ms);
        //                        DBEngine objDb = new DBEngine();
        //                        objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRNDetails.CancelDate + "' WHERE Irn='" + objIRNDetails.Irn + "'");
        //                        string id = Convert.ToString(objDb.GetDataTable("select invoice_id from TBL_TRANS_SALESINVOICE WHERE Irn='" + objIRNDetails.Irn + "'").Rows[0][0]);
        //                        objDb.GetDataTable("EXEC PRC_CANCELIRNSI " + id + "");
        //                        output = "IRN Cancelled successfully.";
        //                    }



        //                }
        //                else
        //                {
        //                    EinvoiceError err = new EinvoiceError();
        //                    var jsonString = response.Content.ReadAsStringAsync().Result;
        //                    // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
        //                    err = response.Content.ReadAsAsync<EinvoiceError>().Result;
        //                    DBEngine objDB = new DBEngine();
        //                    string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_SALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);

        //                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' and ERROR_TYPE='IRN_CANCEL'");
        //                    if (err.error.type != "ClientRequest")
        //                    {
        //                        foreach (errorlog item in err.error.args.irp_error.details)
        //                        {
        //                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ClientEinvoiceError cErr = new ClientEinvoiceError();
        //                        cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
        //                        foreach (string item in cErr.error.args.errors)
        //                        {
        //                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','" + "0" + "','" + item + "')");
        //                        }
        //                    }


        //                    output = "Error occurs while IRN Cancellation.";
        //                }


        //            }
        //        }
        //        catch (AggregateException err)
        //        {
        //            DBEngine objDB = new DBEngine();
        //            string id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_ID FROM TBL_TRANS_SALESINVOICE WHERE Irn='" + irn + "'").Rows[0][0]);
        //            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_CANCEL'");

        //            foreach (var errInner in err.InnerExceptions)
        //            {
        //                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','0','" + err.Message + "')");
        //            }
        //            output = "Error occurs while IRN Cancellation.";
        //        }
        //    }

        //    return output;
        //}


        [WebMethod]
        public static object generateEinvoiceJSONTSI(string id)
        {


            DataSet ds = GetInvoiceDetailsTSI(id);


            DataTable Header = ds.Tables[0];
            DataTable SellerDetails = ds.Tables[1];
            DataTable BuyerDetails = ds.Tables[2];
            DataTable ValueDetails = ds.Tables[3];
            DataTable Products = ds.Tables[4];




            EinvoiceModel objInvoice = new EinvoiceModel("1.01");

            TrasporterDetails objTransporter = new TrasporterDetails();
            objTransporter.EcmGstin = null;
            objTransporter.IgstOnIntra = "N";
            if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
            {
                objTransporter.RegRev = "Y";     /// From table mantis id 23407
            }
            else
            {
                objTransporter.RegRev = "N";
            }
            if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "")
                objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
            objTransporter.TaxSch = "GST";
            objInvoice.TranDtls = objTransporter;


            DocumentsDetails objDoc = new DocumentsDetails();
            objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
            objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
            objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
            objInvoice.DocDtls = objDoc;


            SellerDetails objSeller = new SellerDetails();
            objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
            objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
            if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
            objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master
            objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
            objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]); ;     /// Based on settings Branch/Company master
            if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]); ;      /// Based on settings Branch/Company master
            objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
            objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
            objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
            objInvoice.SellerDtls = objSeller;


            BuyerDetails objBuyer = new BuyerDetails();
            objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objInvoice.BuyerDtls = objBuyer;


            objInvoice.DispDtls = null;  // for now 
            objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

            ValueDetails objValue = new ValueDetails();
            objValue.AssVal = Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]);   // Taxable value
            objValue.CesVal = 0;
            objValue.CgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]); ;
            objValue.Discount = 0;
            objValue.IgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]); ;
            objValue.OthChrg = Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]);   // Global Tax
            objValue.RndOffAmt = 0;
            objValue.SgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]); ;
            objValue.StCesVal = 0;
            objValue.TotInvVal = Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]);
            objValue.TotInvValFc = 0;
            objInvoice.ValDtls = objValue;


            //ExportDetails objExport = new ExportDetails();
            //objExport.CntCode = ""; ///optional for now
            //objExport.ExpDuty = 0;  ///optional for now
            //objExport.ForCur = "";  ///optional for now
            //objExport.Port = "";    ///optional for now
            //objExport.RefClm = "";  ///optional for now
            //objExport.ShipBDt = ""; ///optional for now
            //objExport.ShipBNo = ""; ///optional for now
            //objInvoice.ExpDtls = objExport;

            //EwayBillDetails objEway = new EwayBillDetails();
            //if (Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
            //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
            //else
            //    objEway.Distance = 0;
            /////
            //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
            //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
            //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
            //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
            //objInvoice.EwbDtls = objEway;









            //PaymentDetails objPayment = new PaymentDetails();
            //objPayment.AccDet = "";   ///Optional For Now
            //objPayment.CrDay = 0;     ///Optional For Now
            //objPayment.CrTrn = "";    ///Optional For Now
            //objPayment.DirDr = "";    ///Optional For Now
            //objPayment.FinInsBr = ""; ///Optional For Now
            //objPayment.Mode = "";     ///Optional For Now
            //objPayment.Nm = "";       ///Optional For Now
            //objPayment.PaidAmt = 0;   ///Optional For Now
            //objPayment.PayInstr = ""; ///Optional For Now
            //objPayment.PaymtDue = 0;  ///Optional For Now
            //objPayment.PayTerm = "";  ///Optional For Now
            //objInvoice.PayDtls = objPayment;


            //ReferenceDetails objRef = new ReferenceDetails();

            //List<ContractDetails> onjListContact = new List<ContractDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    ContractDetails onjContact = new ContractDetails();
            //    onjContact.ContrRefr = "";
            //    onjContact.ExtRefr = "";
            //    onjContact.PORefDt = "";
            //    onjContact.PORefr = "";
            //    onjContact.ProjRefr = "";
            //    onjContact.RecAdvDt = "";
            //    onjContact.RecAdvRefr = "";
            //    onjContact.TendRefr = "";
            //    onjListContact.Add(onjContact);
            //}
            //objRef.ContrDtls = onjListContact;


            //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
            //    onjPrecDoc.InvDt = "";
            //    onjPrecDoc.InvNo = "";
            //    onjPrecDoc.OthRefNo = "";
            //    onjListPrecDoc.Add(onjPrecDoc);
            //}
            //objRef.PrecDocDtls = onjListPrecDoc;

            //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
            //objdocPre.InvEndDt = "";
            //objdocPre.InvStDt = "";
            //objRef.DocPerdDtls = objdocPre;

            //objRef.InvRm = "";  // Remarks from invoice
            //objInvoice.RefDtls = objRef;   ///////////// Optional For now



            //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
            //    objAddl.Docs = "";
            //    objAddl.Info = "";
            //    objAddl.Url = "";
            //    objListAddl.Add(objAddl);
            //}
            //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


            List<ProductList> objListProd = new List<ProductList>();

            foreach (DataRow dr in Products.Rows)
            {
                ProductList objProd = new ProductList();
                objProd.AssAmt = 0;

                //**************Commented for now -- This is foer Attribute adding ********************************//

                //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                //for (int j = 0; j < 1; j++)
                //{
                //    AttributeDetails objAttr = new AttributeDetails();
                //    objAttr.Nm = "";
                //    objAttr.Val = "";
                //    objListAttr.Add(objAttr);
                //}
                //objProd.AttribDtls = objListAttr;

                //**************End Commented for now -- This is foer Attribute adding ******************************//

                objProd.AssAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]);
                objProd.Barcde = null;
                objProd.BchDtls = null;
                objProd.CesAmt = 0;
                objProd.CesNonAdvlAmt = 0;
                objProd.CesRt = 0;
                objProd.CgstAmt = Convert.ToDecimal(dr["CGSTAmount"]);
                objProd.Discount = Convert.ToDecimal(dr["InvoiceDetails_Discount"]);
                objProd.FreeQty = 0;
                objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                objProd.IgstAmt = Convert.ToDecimal(dr["IGSTAmount"]); ;
                if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                    objProd.IsServc = "N";
                else
                    objProd.IsServc = "Y";
                objProd.OrdLineRef = null;
                objProd.OrgCntry = null;
                objProd.OthChrg = Convert.ToDecimal(dr["OtherAmount"]); ;
                objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                objProd.PrdSlNo = null;
                objProd.PreTaxVal = Convert.ToDecimal(dr["InvoiceDetails_Amount"]); ;
                objProd.Qty = Convert.ToDecimal(dr["InvoiceDetails_Quantity"]); ;
                objProd.SgstAmt = Convert.ToDecimal(dr["SGSTAmount"]); ;
                objProd.SlNo = Convert.ToString(dr["SL"]);
                objProd.StateCesAmt = 0;
                objProd.StateCesNonAdvlAmt = 0;
                objProd.StateCesRt = 0;
                objProd.TotAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]); ;
                objProd.TotItemVal = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]); ;
                if (Convert.ToString(dr["GST_Print_Name"]) != "")
                    objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                //else
                //    objProd.Unit = "BAG";
                objProd.UnitPrice = Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]); ;
                objListProd.Add(objProd);
            }
            objInvoice.ItemList = objListProd;




            return objInvoice;
        }
        [WebMethod]
        public static object generateEinvoiceJSONSR(string id)
        {


            DataSet ds = GetInvoiceDetailsSR(id);


            DataTable Header = ds.Tables[0];
            DataTable SellerDetails = ds.Tables[1];
            DataTable BuyerDetails = ds.Tables[2];
            DataTable ValueDetails = ds.Tables[3];
            DataTable Products = ds.Tables[4];




            EinvoiceModel objInvoice = new EinvoiceModel("1.01");

            TrasporterDetails objTransporter = new TrasporterDetails();
            objTransporter.EcmGstin = null;
            objTransporter.IgstOnIntra = "N";
            if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
            {
                objTransporter.RegRev = "Y";     /// From table mantis id 23407
            }
            else
            {
                objTransporter.RegRev = "N";
            }
            if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "")
                objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
            objTransporter.TaxSch = "GST";
            objInvoice.TranDtls = objTransporter;


            DocumentsDetails objDoc = new DocumentsDetails();
            objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
            objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
            objDoc.Typ = "CRN";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
            objInvoice.DocDtls = objDoc;


            SellerDetails objSeller = new SellerDetails();
            objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
            objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
            if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
            objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master
            objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
            objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]); ;     /// Based on settings Branch/Company master
            if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]); ;      /// Based on settings Branch/Company master
            objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
            objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
            objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
            objInvoice.SellerDtls = objSeller;


            BuyerDetails objBuyer = new BuyerDetails();
            objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objInvoice.BuyerDtls = objBuyer;


            objInvoice.DispDtls = null;  // for now 
            objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trajs_salesinvoiceadddress

            ValueDetails objValue = new ValueDetails();
            objValue.AssVal = Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]);   // Taxable value
            objValue.CesVal = 0;
            objValue.CgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]); ;
            objValue.Discount = 0;
            objValue.IgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]); ;
            objValue.OthChrg = Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]);   // Global Tax
            objValue.RndOffAmt = 0;
            objValue.SgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]); ;
            objValue.StCesVal = 0;
            objValue.TotInvVal = Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]);
            objValue.TotInvValFc = 0;
            objInvoice.ValDtls = objValue;


            //ExportDetails objExport = new ExportDetails();
            //objExport.CntCode = ""; ///optional for now
            //objExport.ExpDuty = 0;  ///optional for now
            //objExport.ForCur = "";  ///optional for now
            //objExport.Port = "";    ///optional for now
            //objExport.RefClm = "";  ///optional for now
            //objExport.ShipBDt = ""; ///optional for now
            //objExport.ShipBNo = ""; ///optional for now
            //objInvoice.ExpDtls = objExport;

            //EwayBillDetails objEway = new EwayBillDetails();
            //if (Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
            //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
            //else
            //    objEway.Distance = 0;
            /////
            //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
            //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
            //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
            //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
            //objInvoice.EwbDtls = objEway;









            //PaymentDetails objPayment = new PaymentDetails();
            //objPayment.AccDet = "";   ///Optional For Now
            //objPayment.CrDay = 0;     ///Optional For Now
            //objPayment.CrTrn = "";    ///Optional For Now
            //objPayment.DirDr = "";    ///Optional For Now
            //objPayment.FinInsBr = ""; ///Optional For Now
            //objPayment.Mode = "";     ///Optional For Now
            //objPayment.Nm = "";       ///Optional For Now
            //objPayment.PaidAmt = 0;   ///Optional For Now
            //objPayment.PayInstr = ""; ///Optional For Now
            //objPayment.PaymtDue = 0;  ///Optional For Now
            //objPayment.PayTerm = "";  ///Optional For Now
            //objInvoice.PayDtls = objPayment;


            //ReferenceDetails objRef = new ReferenceDetails();

            //List<ContractDetails> onjListContact = new List<ContractDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    ContractDetails onjContact = new ContractDetails();
            //    onjContact.ContrRefr = "";
            //    onjContact.ExtRefr = "";
            //    onjContact.PORefDt = "";
            //    onjContact.PORefr = "";
            //    onjContact.ProjRefr = "";
            //    onjContact.RecAdvDt = "";
            //    onjContact.RecAdvRefr = "";
            //    onjContact.TendRefr = "";
            //    onjListContact.Add(onjContact);
            //}
            //objRef.ContrDtls = onjListContact;


            //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
            //    onjPrecDoc.InvDt = "";
            //    onjPrecDoc.InvNo = "";
            //    onjPrecDoc.OthRefNo = "";
            //    onjListPrecDoc.Add(onjPrecDoc);
            //}
            //objRef.PrecDocDtls = onjListPrecDoc;

            //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
            //objdocPre.InvEndDt = "";
            //objdocPre.InvStDt = "";
            //objRef.DocPerdDtls = objdocPre;

            //objRef.InvRm = "";  // Remarks from invoice
            //objInvoice.RefDtls = objRef;   ///////////// Optional For now



            //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
            //    objAddl.Docs = "";
            //    objAddl.Info = "";
            //    objAddl.Url = "";
            //    objListAddl.Add(objAddl);
            //}
            //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


            List<ProductList> objListProd = new List<ProductList>();

            foreach (DataRow dr in Products.Rows)
            {
                ProductList objProd = new ProductList();
                objProd.AssAmt = 0;

                //**************Commented for now -- This is foer Attribute adding ********************************//

                //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                //for (int j = 0; j < 1; j++)
                //{
                //    AttributeDetails objAttr = new AttributeDetails();
                //    objAttr.Nm = "";
                //    objAttr.Val = "";
                //    objListAttr.Add(objAttr);
                //}
                //objProd.AttribDtls = objListAttr;

                //**************End Commented for now -- This is foer Attribute adding ******************************//

                objProd.AssAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]);
                objProd.Barcde = null;
                objProd.BchDtls = null;
                objProd.CesAmt = 0;
                objProd.CesNonAdvlAmt = 0;
                objProd.CesRt = 0;
                objProd.CgstAmt = Convert.ToDecimal(dr["CGSTAmount"]);
                objProd.Discount = Convert.ToDecimal(dr["InvoiceDetails_Discount"]);
                objProd.FreeQty = 0;
                objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                objProd.IgstAmt = Convert.ToDecimal(dr["IGSTAmount"]); ;
                if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                    objProd.IsServc = "N";
                else
                    objProd.IsServc = "Y";
                objProd.OrdLineRef = null;
                objProd.OrgCntry = null;
                objProd.OthChrg = Convert.ToDecimal(dr["OtherAmount"]); ;
                objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                objProd.PrdSlNo = null;
                objProd.PreTaxVal = Convert.ToDecimal(dr["InvoiceDetails_Amount"]); ;
                objProd.Qty = Convert.ToDecimal(dr["InvoiceDetails_Quantity"]); ;
                objProd.SgstAmt = Convert.ToDecimal(dr["SGSTAmount"]); ;
                objProd.SlNo = Convert.ToString(dr["SL"]);
                objProd.StateCesAmt = 0;
                objProd.StateCesNonAdvlAmt = 0;
                objProd.StateCesRt = 0;
                objProd.TotAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]); ;
                objProd.TotItemVal = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]); ;
                if (Convert.ToString(dr["GST_Print_Name"]) != "")
                    objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                //else
                //    objProd.Unit = "BAG";
                objProd.UnitPrice = Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]); ;
                objListProd.Add(objProd);
            }
            objInvoice.ItemList = objListProd;




            return objInvoice;
        }

        [WebMethod]
        public static object generateEinvoiceJSON(string id)
        {


            DataSet ds = GetInvoiceDetails(id);


            DataTable Header = ds.Tables[0];
            DataTable SellerDetails = ds.Tables[1];
            DataTable BuyerDetails = ds.Tables[2];
            DataTable ValueDetails = ds.Tables[3];
            DataTable Products = ds.Tables[4];


            string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];

            EinvoiceModel objInvoice = new EinvoiceModel("1.01");



            TrasporterDetails objTransporter = new TrasporterDetails();
            objTransporter.EcmGstin = null;
            objTransporter.IgstOnIntra = "N";
            if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
            {
                objTransporter.RegRev = "Y";     /// From table mantis id 23407
            }
            else
            {
                objTransporter.RegRev = "N";
            }
            if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "")
                objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
            objTransporter.TaxSch = "GST";
            objInvoice.TranDtls = objTransporter;


            DocumentsDetails objDoc = new DocumentsDetails();
            objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
            objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
            objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
            objInvoice.DocDtls = objDoc;


            SellerDetails objSeller = new SellerDetails();
            objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
            objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
            if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
            objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master
            objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
            objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]); ;     /// Based on settings Branch/Company master
            if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]); ;      /// Based on settings Branch/Company master
            objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
            objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
            objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
            objInvoice.SellerDtls = objSeller;


            BuyerDetails objBuyer = new BuyerDetails();
            objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objInvoice.BuyerDtls = objBuyer;


            objInvoice.DispDtls = null;  // for now 
            objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

            ValueDetails objValue = new ValueDetails();
            objValue.AssVal = Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]);   // Taxable value
            objValue.CesVal = 0;
            objValue.CgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]); ;
            objValue.Discount = 0;
            objValue.IgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]); ;
            objValue.OthChrg = Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]);   // Global Tax
            objValue.RndOffAmt = 0;
            objValue.SgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]); ;
            objValue.StCesVal = 0;
            objValue.TotInvVal = Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]);
            objValue.TotInvValFc = 0;
            objInvoice.ValDtls = objValue;


            //ExportDetails objExport = new ExportDetails();
            //objExport.CntCode = ""; ///optional for now
            //objExport.ExpDuty = 0;  ///optional for now
            //objExport.ForCur = "";  ///optional for now
            //objExport.Port = "";    ///optional for now
            //objExport.RefClm = "";  ///optional for now
            //objExport.ShipBDt = ""; ///optional for now
            //objExport.ShipBNo = ""; ///optional for now
            //objInvoice.ExpDtls = objExport;

            //EwayBillDetails objEway = new EwayBillDetails();
            //if (Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
            //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
            //else
            //    objEway.Distance = 0;
            /////
            //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
            //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
            //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
            //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
            //objInvoice.EwbDtls = objEway;









            //PaymentDetails objPayment = new PaymentDetails();
            //objPayment.AccDet = "";   ///Optional For Now
            //objPayment.CrDay = 0;     ///Optional For Now
            //objPayment.CrTrn = "";    ///Optional For Now
            //objPayment.DirDr = "";    ///Optional For Now
            //objPayment.FinInsBr = ""; ///Optional For Now
            //objPayment.Mode = "";     ///Optional For Now
            //objPayment.Nm = "";       ///Optional For Now
            //objPayment.PaidAmt = 0;   ///Optional For Now
            //objPayment.PayInstr = ""; ///Optional For Now
            //objPayment.PaymtDue = 0;  ///Optional For Now
            //objPayment.PayTerm = "";  ///Optional For Now
            //objInvoice.PayDtls = objPayment;


            //ReferenceDetails objRef = new ReferenceDetails();

            //List<ContractDetails> onjListContact = new List<ContractDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    ContractDetails onjContact = new ContractDetails();
            //    onjContact.ContrRefr = "";
            //    onjContact.ExtRefr = "";
            //    onjContact.PORefDt = "";
            //    onjContact.PORefr = "";
            //    onjContact.ProjRefr = "";
            //    onjContact.RecAdvDt = "";
            //    onjContact.RecAdvRefr = "";
            //    onjContact.TendRefr = "";
            //    onjListContact.Add(onjContact);
            //}
            //objRef.ContrDtls = onjListContact;


            //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
            //    onjPrecDoc.InvDt = "";
            //    onjPrecDoc.InvNo = "";
            //    onjPrecDoc.OthRefNo = "";
            //    onjListPrecDoc.Add(onjPrecDoc);
            //}
            //objRef.PrecDocDtls = onjListPrecDoc;

            //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
            //objdocPre.InvEndDt = "";
            //objdocPre.InvStDt = "";
            //objRef.DocPerdDtls = objdocPre;

            //objRef.InvRm = "";  // Remarks from invoice
            //objInvoice.RefDtls = objRef;   ///////////// Optional For now



            //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
            //    objAddl.Docs = "";
            //    objAddl.Info = "";
            //    objAddl.Url = "";
            //    objListAddl.Add(objAddl);
            //}
            //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


            List<ProductList> objListProd = new List<ProductList>();

            foreach (DataRow dr in Products.Rows)
            {
                ProductList objProd = new ProductList();
                objProd.AssAmt = 0;

                //**************Commented for now -- This is foer Attribute adding ********************************//

                //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                //for (int j = 0; j < 1; j++)
                //{
                //    AttributeDetails objAttr = new AttributeDetails();
                //    objAttr.Nm = "";
                //    objAttr.Val = "";
                //    objListAttr.Add(objAttr);
                //}
                //objProd.AttribDtls = objListAttr;

                //**************End Commented for now -- This is foer Attribute adding ******************************//

                //  objProd.AssAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]);
                objProd.Barcde = null;
                objProd.BchDtls = null;
                objProd.CesAmt = 0;
                objProd.CesNonAdvlAmt = 0;
                objProd.CesRt = 0;
                objProd.CgstAmt = Convert.ToDecimal(dr["CGSTAmount"]);
                //  objProd.Discount = Convert.ToDecimal(dr["InvoiceDetails_Discount"]);
                objProd.FreeQty = 0;
                objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                objProd.IgstAmt = Convert.ToDecimal(dr["IGSTAmount"]); ;
                if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                    objProd.IsServc = "N";
                else
                    objProd.IsServc = "Y";
                objProd.OrdLineRef = null;
                objProd.OrgCntry = null;
                objProd.OthChrg = Convert.ToDecimal(dr["OtherAmount"]); ;
                objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                objProd.PrdSlNo = null;
                objProd.PreTaxVal = Convert.ToDecimal(dr["InvoiceDetails_Amount"]); ;
                objProd.Qty = Convert.ToDecimal(dr["InvoiceDetails_Quantity"]); ;
                objProd.SgstAmt = Convert.ToDecimal(dr["SGSTAmount"]); ;
                objProd.SlNo = Convert.ToString(dr["SL"]);
                objProd.StateCesAmt = 0;
                objProd.StateCesNonAdvlAmt = 0;
                objProd.StateCesRt = 0;
                // objProd.TotAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]);
                objProd.TotAmt = Convert.ToDecimal((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])).ToString("0.00"));
                objProd.Discount = Convert.ToDecimal(dr["InvoiceDetails_Discount"]);
                objProd.AssAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]);

                //if (Convert.ToDecimal(dr["InvoiceDetails_Discount"])<0)
                //{
                //    objProd.Discount = (Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"]))-Convert.ToDecimal(dr["InvoiceDetails_Amount"])  ;
                //    objProd.AssAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]) - ((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])) - Convert.ToDecimal(dr["InvoiceDetails_Amount"]));
                //}

                if (Convert.ToDecimal(dr["InvoiceDetails_Discount"]) < 0)
                {
                    objProd.Discount = Convert.ToDecimal(((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])) - Convert.ToDecimal(dr["InvoiceDetails_Amount"])).ToString("0.00"));
                    Decimal Discount_Amount = Convert.ToDecimal(((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])) - Convert.ToDecimal(dr["InvoiceDetails_Amount"])).ToString("0.00"));
                    objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])) - Discount_Amount).ToString("0.00"));
                }


                objProd.TotItemVal = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]); ;
                if (Convert.ToString(dr["GST_Print_Name"]) != "")
                    objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                //else
                //    objProd.Unit = "BAG";
                objProd.UnitPrice = Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]); ;
                objListProd.Add(objProd);
            }
            objInvoice.ItemList = objListProd;




            return objInvoice;
        }

        [WebMethod]
        public static object UploadIRN(string id, string type)
        {


            DataSet ds = new DataSet();

            if (type == "SI")
            {
                ds = GetInvoiceDetails(id);
            }

            DataTable Header = ds.Tables[0];
            DataTable SellerDetails = ds.Tables[1];
            DataTable BuyerDetails = ds.Tables[2];
            DataTable ValueDetails = ds.Tables[3];
            DataTable Products = ds.Tables[4];





            EinvoiceModel objInvoice = new EinvoiceModel("1.01");

            TrasporterDetails objTransporter = new TrasporterDetails();
            objTransporter.EcmGstin = null;
            objTransporter.IgstOnIntra = "N";
            if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
            {
                objTransporter.RegRev = "Y";     /// From table mantis id 23407
            }
            else
            {
                objTransporter.RegRev = "N";
            }
            if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "")
                objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
            objTransporter.TaxSch = "GST";
            objInvoice.TranDtls = objTransporter;


            DocumentsDetails objDoc = new DocumentsDetails();
            objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
            objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
            objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
            objInvoice.DocDtls = objDoc;


            SellerDetails objSeller = new SellerDetails();
            objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
            objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
            if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
            objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master
            objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
            objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]); ;     /// Based on settings Branch/Company master
            if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]); ;      /// Based on settings Branch/Company master
            objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
            objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
            objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
            objInvoice.SellerDtls = objSeller;


            BuyerDetails objBuyer = new BuyerDetails();
            objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
            objInvoice.BuyerDtls = objBuyer;


            objInvoice.DispDtls = null;  // for now 
            objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

            ValueDetails objValue = new ValueDetails();
            objValue.AssVal = Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]);   // Taxable value
            objValue.CesVal = 0;
            objValue.CgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]); ;
            objValue.Discount = 0;
            objValue.IgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]); ;
            objValue.OthChrg = Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]);   // Global Tax
            objValue.RndOffAmt = 0;
            objValue.SgstVal = Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]); ;
            objValue.StCesVal = 0;
            objValue.TotInvVal = Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]);
            objValue.TotInvValFc = 0;
            objInvoice.ValDtls = objValue;


            //ExportDetails objExport = new ExportDetails();
            //objExport.CntCode = ""; ///optional for now
            //objExport.ExpDuty = 0;  ///optional for now
            //objExport.ForCur = "";  ///optional for now
            //objExport.Port = "";    ///optional for now
            //objExport.RefClm = "";  ///optional for now
            //objExport.ShipBDt = ""; ///optional for now
            //objExport.ShipBNo = ""; ///optional for now
            //objInvoice.ExpDtls = objExport;

            //EwayBillDetails objEway = new EwayBillDetails();
            //if (Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
            //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
            //else
            //    objEway.Distance = 0;
            /////
            //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
            //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
            //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
            //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
            //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
            //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
            //objInvoice.EwbDtls = objEway;









            //PaymentDetails objPayment = new PaymentDetails();
            //objPayment.AccDet = "";   ///Optional For Now
            //objPayment.CrDay = 0;     ///Optional For Now
            //objPayment.CrTrn = "";    ///Optional For Now
            //objPayment.DirDr = "";    ///Optional For Now
            //objPayment.FinInsBr = ""; ///Optional For Now
            //objPayment.Mode = "";     ///Optional For Now
            //objPayment.Nm = "";       ///Optional For Now
            //objPayment.PaidAmt = 0;   ///Optional For Now
            //objPayment.PayInstr = ""; ///Optional For Now
            //objPayment.PaymtDue = 0;  ///Optional For Now
            //objPayment.PayTerm = "";  ///Optional For Now
            //objInvoice.PayDtls = objPayment;


            //ReferenceDetails objRef = new ReferenceDetails();

            //List<ContractDetails> onjListContact = new List<ContractDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    ContractDetails onjContact = new ContractDetails();
            //    onjContact.ContrRefr = "";
            //    onjContact.ExtRefr = "";
            //    onjContact.PORefDt = "";
            //    onjContact.PORefr = "";
            //    onjContact.ProjRefr = "";
            //    onjContact.RecAdvDt = "";
            //    onjContact.RecAdvRefr = "";
            //    onjContact.TendRefr = "";
            //    onjListContact.Add(onjContact);
            //}
            //objRef.ContrDtls = onjListContact;


            //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
            //    onjPrecDoc.InvDt = "";
            //    onjPrecDoc.InvNo = "";
            //    onjPrecDoc.OthRefNo = "";
            //    onjListPrecDoc.Add(onjPrecDoc);
            //}
            //objRef.PrecDocDtls = onjListPrecDoc;

            //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
            //objdocPre.InvEndDt = "";
            //objdocPre.InvStDt = "";
            //objRef.DocPerdDtls = objdocPre;

            //objRef.InvRm = "";  // Remarks from invoice
            //objInvoice.RefDtls = objRef;   ///////////// Optional For now



            //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
            //for (int i = 0; i < 1; i++)
            //{
            //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
            //    objAddl.Docs = "";
            //    objAddl.Info = "";
            //    objAddl.Url = "";
            //    objListAddl.Add(objAddl);
            //}
            //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


            List<ProductList> objListProd = new List<ProductList>();

            foreach (DataRow dr in Products.Rows)
            {
                ProductList objProd = new ProductList();
                objProd.AssAmt = 0;

                //**************Commented for now -- This is foer Attribute adding ********************************//

                //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                //for (int j = 0; j < 1; j++)
                //{
                //    AttributeDetails objAttr = new AttributeDetails();
                //    objAttr.Nm = "";
                //    objAttr.Val = "";
                //    objListAttr.Add(objAttr);
                //}
                //objProd.AttribDtls = objListAttr;

                //**************End Commented for now -- This is foer Attribute adding ******************************//

                objProd.AssAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]);
                objProd.Barcde = null;
                objProd.BchDtls = null;
                objProd.CesAmt = 0;
                objProd.CesNonAdvlAmt = 0;
                objProd.CesRt = 0;
                objProd.CgstAmt = Convert.ToDecimal(dr["CGSTAmount"]);
                objProd.Discount = Convert.ToDecimal(dr["InvoiceDetails_Discount"]);
                objProd.FreeQty = 0;
                objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                objProd.IgstAmt = Convert.ToDecimal(dr["IGSTAmount"]); ;
                if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                    objProd.IsServc = "N";
                else
                    objProd.IsServc = "Y";
                objProd.OrdLineRef = null;
                objProd.OrgCntry = null;
                objProd.OthChrg = Convert.ToDecimal(dr["OtherAmount"]); ;
                objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                objProd.PrdSlNo = null;
                objProd.PreTaxVal = Convert.ToDecimal(dr["InvoiceDetails_Amount"]); ;
                objProd.Qty = Convert.ToDecimal(dr["InvoiceDetails_Quantity"]); ;
                objProd.SgstAmt = Convert.ToDecimal(dr["SGSTAmount"]); ;
                objProd.SlNo = Convert.ToString(dr["SL"]);
                objProd.StateCesAmt = 0;
                objProd.StateCesNonAdvlAmt = 0;
                objProd.StateCesRt = 0;
                objProd.TotAmt = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]); ;
                objProd.TotItemVal = Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]); ;
                if (Convert.ToString(dr["GST_Print_Name"]) != "")
                    objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                //else
                //    objProd.Unit = "BAG";
                objProd.UnitPrice = Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]); ;
                objListProd.Add(objProd);
            }
            objInvoice.ItemList = objListProd;




            return objInvoice;
        }

        private static DataSet GetInvoiceDetails(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_Einvoice");
            proc.AddVarcharPara("@Action", 100, "GetInvoiceDetails");
            proc.AddVarcharPara("@id", 4000, id);
            ds = proc.GetDataSet();
            return ds;
        }

        private static DataSet GetInvoiceDetailsTSI(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_Einvoice");
            proc.AddVarcharPara("@Action", 100, "GetTInvoiceDetails");
            proc.AddVarcharPara("@id", 4000, id);
            ds = proc.GetDataSet();
            return ds;
        }


        private static DataSet GetInvoiceDetailsSR(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_Einvoice");
            proc.AddVarcharPara("@Action", 100, "GetSRDetails");
            proc.AddVarcharPara("@id", 4000, id);
            ds = proc.GetDataSet();
            return ds;
        }

        protected void GrdQuotation_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            GrdQuotation.JSProperties["cpSuccessMsg"] = null;
            GrdQuotation.JSProperties["cpFalureMsg"] = null;

            string success = "";
            string error = "";
            #region download
            if (e.Parameters.Split('~')[0] == "DownloadJSON")
            {
                var ids = GrdQuotation.GetSelectedFieldValues("Invoice_Id");
                List<EinvoiceModel> obj = new List<EinvoiceModel>();
                foreach (var id in ids)
                {

                    DataSet ds = GetInvoiceDetails(id.ToString());
                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];

                    string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                    string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                    string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                    string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                    string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(Header.Rows[0]["Invoice_BranchId"]); ;
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);



                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;

                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }

                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;

                    obj.Add(objInvoice);
                    GrdQuotation.JSProperties["cpJson"] = "Yes";
                }


                Session["obj"] = obj;




            }

            #endregion
            #region Cancel
            else if (e.Parameters.Split('~')[0] == "CancelIRN")
            {
                string output = "";
                var ids = GrdQuotation.GetSelectedFieldValues("Invoice_Id");

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];


                foreach (var id in ids)
                {
                    DBEngine objDB = new DBEngine();
                    string irn = Convert.ToString(objDB.GetDataTable("SELECT IRN FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_SALESInvoice WHERE Irn='" + irn + "'").Rows[0][0]); ;
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);




                    CancelDetails objCancelDetails = new CancelDetails();
                    objCancelDetails.Irn = irn;
                    objCancelDetails.CnlRem = e.Parameters.Split('~')[2];
                    objCancelDetails.CnlRsn = e.Parameters.Split('~')[1];

                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {

                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                    EinvoiceToken.Expiry = date;

                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_CANCEL'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','0','" + err.Message + "')");
                            }

                            error = error + "," + irn;


                        }
                    }
                    try
                    {
                        IRN objIRN = new IRN();
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objCancelDetails, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnCancelUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                {
                                    // Deserialization from JSON  
                                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelIRNOutput));
                                    CancelIRNOutput objIRNDetails = (CancelIRNOutput)deserializer.ReadObject(ms);
                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRNDetails.CancelDate + "' WHERE Irn='" + objIRNDetails.Irn + "'");
                                    objDb.GetDataTable("EXEC PRC_CANCELIRNSI " + id + "");

                                    output = "IRN Cancelled successfully.";
                                    success = success + "," + irn;
                                }



                            }
                            else
                            {

                                EinvoiceError err = new EinvoiceError();
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' and ERROR_TYPE='IRN_CANCEL'");
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.irp_error.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','" + "0" + "','" + item + "')");
                                    }

                                }

                                error = error + "," + irn;


                                output = "Error occurs while IRN Cancellation.";
                            }



                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','0','" + err.Message + "')");
                        }

                        error = error + "," + irn;
                    }
                }
                GrdQuotation.JSProperties["cpJSON"] = "Cancel";
            }
            #endregion
            else if (e.Parameters.Split('~')[0] == "GenEwayBill")
            {

                string output = "";

                DBEngine objDB = new DBEngine();
                string irn = e.Parameters.Split('~')[1];
                string id = e.Parameters.Split('~')[2];
                DataSet ds = GetInvoiceDetails(id);
                DataTable Header = ds.Tables[0];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(Header.Rows[0]["Invoice_BranchId"]); ;
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];

                EwayBillGeneration objEwaybill = new EwayBillGeneration();
                objEwaybill.Irn = irn;
                if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    objEwaybill.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                else
                    objEwaybill.Distance = 0;
                ///
                if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    objEwaybill.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 





                authtokensOutput authObj = new authtokensOutput();
                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {

                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                EinvoiceToken.Expiry = date;

                            }
                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAY_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','0','" + err.Message + "')");
                        }

                        error = error + "," + irn;


                    }
                }
                try
                {

                    //Rev 5.0
                    //Rev  7.0
                    //IRNV3 objIRN = new IRNV3();
                    EWAYBILLV3 objIRN = new EWAYBILLV3();
                    //Rev  7.0 End
                    //IRN objIRN = new IRN();
                    //Rev 5.0 End



                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objEwaybill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        //Rev 5.0
                        //Rev Generate EWB by IRN (v1.0)
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                        //Rev Generate EWB by IRN (v1.0) End
                        //Rev Generate EWB by IRN (v3.0)
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                        //Rev Generate EWB by IRN (v3.0) End
                        //Rev 5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync(IrnEwaybillUrl, stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            //Rev 5.0
                            //Rev Generate EWB (v3.0)
                            objIRN = JsonConvert.DeserializeObject<EWAYBILLV3>(jsonString);

                            if (Convert.ToString(objIRN.status) == "1")
                            {
                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET EWayBillNumber = '" + objIRN.data.EwbNo + "',EWayBillDate='" + objIRN.data.EwbDt + "',EwayBill_ValidTill='" + objIRN.data.EwbValidTill + "',ISEWAYBILLCANCEL=0 where invoice_id='" + id.ToString() + "'");
                                output = "E-Way bill genererated successfully .";
                                success = success + "," + irn;
                            }
                            //Rev Generate EWB (v3.0) End
                            //Rev Generate EWB (v1.0)
                            //objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                            //    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                            //    DBEngine objDb = new DBEngine();
                            //    objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "',ISEWAYBILLCANCEL=0 where invoice_id='" + id.ToString() + "'");
                            //    output = "IRN Cancelled successfully.";
                            //    success = success + "," + irn;
                            //}
                            //Rev Generate EWB (v1.0) End
                            //Rev 5.0 End

                        }
                        else
                        {
                            //Rev 5.0
                            EinvoiceErrorV3 err = new EinvoiceErrorV3();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' and ERROR_TYPE='EWAY_GEN'");
                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                if (cErr.error.args.errors != null)
                                {
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + "0" + "','" + item + "')");
                                    }
                                }
                                else
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + "0" + "','Invalid request body.')");
                                }

                            }

                            //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            //objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' and ERROR_TYPE='EWAY_GEN'");
                            //if (err.error.type != "ClientRequest")
                            //{
                            //    foreach (errorlog item in err.error.args.irp_error.details)
                            //    {
                            //        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                            //    }
                            //}
                            //else
                            //{
                            //    ClientEinvoiceError cErr = new ClientEinvoiceError();
                            //    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                            //    if (cErr.error.args.errors != null)
                            //    {
                            //        foreach (string item in cErr.error.args.errors)
                            //        {
                            //            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + "0" + "','" + item + "')");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + "0" + "','Invalid request body.')");
                            //    }

                            //}
                            //Rev 5.0 End
                            error = error + "," + irn;


                            output = "Error occurs while IRN Cancellation.";
                        }



                    }
                }
                catch (AggregateException err)
                {
                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_CANCEL'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','0','" + err.Message + "')");
                    }

                    error = error + "," + irn;
                }

                GrdQuotation.JSProperties["cpJSON"] = "Ewaybill";
            }
            else if (e.Parameters.Split('~')[0] == "GenEwayBillBulk")
            {
                string output = "";
                var ids = GrdQuotation.GetSelectedFieldValues("Invoice_Id");
                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];

                foreach (var id in ids)
                {



                    DBEngine objDB = new DBEngine();
                    string irn = Convert.ToString(objDB.GetDataTable("SELECT IRN FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataSet ds = GetInvoiceDetails(id.ToString());
                    DataTable Header = ds.Tables[0];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(Header.Rows[0]["Invoice_BranchId"]); ;
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                    EwayBillGeneration objEwaybill = new EwayBillGeneration();
                    objEwaybill.Irn = irn;
                    if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                        objEwaybill.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    else
                        objEwaybill.Distance = 0;
                    ///
                    if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                        objEwaybill.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 





                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {

                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                    EinvoiceToken.Expiry = date;

                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAY_GEN'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','0','" + err.Message + "')");
                            }

                            error = error + "," + irn;


                        }
                    }
                    try
                    {
                        IRN objIRN = new IRN();
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objEwaybill, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnEwaybillUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                {
                                    // Deserialization from JSON  
                                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                                    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "',ISEWAYBILLCANCEL=0 where invoice_id='" + id.ToString() + "'");
                                    output = "IRN Cancelled successfully.";
                                    success = success + "," + irn;
                                }



                            }
                            else
                            {

                                EinvoiceError err = new EinvoiceError();
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' and ERROR_TYPE='EWAY_GEN'");
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.irp_error.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    if (cErr.error.args.errors != null)
                                    {
                                        foreach (string item in cErr.error.args.errors)
                                        {
                                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + "0" + "','" + item + "')");
                                        }
                                    }
                                    else
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAY_GEN','" + "0" + "','Invalid request body.')");
                                    }

                                }

                                error = error + "," + irn;


                                output = "Error occurs while IRN Cancellation.";
                            }



                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_CANCEL','0','" + err.Message + "')");
                        }

                        error = error + "," + irn;
                    }
                }

                GrdQuotation.JSProperties["cpJSON"] = "Ewaybill";
            }


            GrdQuotation.JSProperties["cpSuccessMsg"] = success;
            GrdQuotation.JSProperties["cpFalureMsg"] = error;


        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) == "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) == "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoices
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void LinqServerModeDataSourceCancelSI_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {


            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.IRNCancel_BranchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == true
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.IRNCancel_BranchId)) &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == true
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoices
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }

        }

        protected void LinqServerModeDataSourcePendinSI_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) == "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) == "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoices
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void ASPxGridView2_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdQuotationPendinSI.JSProperties["cpJson"] = "No";
            GrdQuotationPendinSI.JSProperties["cpSuccessMsg"] = null;
            GrdQuotationPendinSI.JSProperties["cpFalureMsg"] = null;

            string success = "";
            string error = "";


            string IRNsuccess = "";
            string IRNerror = "";

            if (e.Parameters.Split('~')[0] == "UploadSingleIRN")
            {

                DataSet ds = new DataSet();

                string id = e.Parameters.Split('~')[2];
                if (e.Parameters.Split('~')[1] == "SI")
                {
                    ds = GetInvoiceDetails(id);
                }

                DataTable Header = ds.Tables[0];
                DataTable SellerDetails = ds.Tables[1];
                DataTable BuyerDetails = ds.Tables[2];
                DataTable ValueDetails = ds.Tables[3];
                DataTable Products = ds.Tables[4];
                DataTable ShipDetails = ds.Tables[5];
                DataTable DispatchFrom = ds.Tables[6];


                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(Header.Rows[0]["Invoice_BranchId"]); ;
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);



                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];

                EinvoiceModel objInvoice = new EinvoiceModel("1.1");




                TrasporterDetails objTransporter = new TrasporterDetails();
                objTransporter.EcmGstin = null;
                objTransporter.IgstOnIntra = "N";
                if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                {
                    objTransporter.RegRev = "Y";     /// From table mantis id 23407
                }
                else
                {
                    objTransporter.RegRev = "N";
                }
                if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                    objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                else
                    objTransporter.SupTyp = "B2B";
                objTransporter.TaxSch = "GST";
                objInvoice.TranDtls = objTransporter;


                DocumentsDetails objDoc = new DocumentsDetails();
                objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                objInvoice.DocDtls = objDoc;


                SellerDetails objSeller = new SellerDetails();
                objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                    objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                else
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                /// 
                if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                    objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                objInvoice.SellerDtls = objSeller;


                BuyerDetails objBuyer = new BuyerDetails();
                objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                    objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                else
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                    objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objInvoice.BuyerDtls = objBuyer;


                objInvoice.DispDtls = null;  // for now 
                objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                ValueDetails objValue = new ValueDetails();
                objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                objValue.CesVal = 0.00M;
                objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                objValue.Discount = 0.00M;
                objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                objValue.RndOffAmt = 0.00M;
                objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                objValue.StCesVal = 0.00M;
                objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                objValue.TotInvValFc = 0.00M;
                objInvoice.ValDtls = objValue;


                //ExportDetails objExport = new ExportDetails();
                //objExport.CntCode = ""; ///optional for now
                //objExport.ExpDuty = 0;  ///optional for now
                //objExport.ForCur = "";  ///optional for now
                //objExport.Port = "";    ///optional for now
                //objExport.RefClm = "";  ///optional for now
                //objExport.ShipBDt = ""; ///optional for now
                //objExport.ShipBNo = ""; ///optional for now
                //objInvoice.ExpDtls = objExport;

                //EwayBillDetails objEway = new EwayBillDetails();
                //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                //else
                //    objEway.Distance = 0;
                /////
                //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                //objInvoice.EwbDtls = objEway;


                // Mantis Issue 24608
                if (DispatchFrom.Rows.Count > 0)
                {
                    // End of Mantis Issue 24608
                    DispatchDetails objDisp = new DispatchDetails();
                    objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                    objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                    objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                    objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                    objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                    objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                    objInvoice.DispDtls = objDisp;
                }

                // Mantis Issue 24608
                if (ShipDetails.Rows.Count > 0)
                {
                    // End of Mantis Issue 24608
                    ShipToDetails objShip = new ShipToDetails();
                    objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                    objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                    objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                    objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                    objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                    objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                    objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                    objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                    objInvoice.ShipDtls = objShip;
                }

                //PaymentDetails objPayment = new PaymentDetails();
                //objPayment.AccDet = "";   ///Optional For Now
                //objPayment.CrDay = 0;     ///Optional For Now
                //objPayment.CrTrn = "";    ///Optional For Now
                //objPayment.DirDr = "";    ///Optional For Now
                //objPayment.FinInsBr = ""; ///Optional For Now
                //objPayment.Mode = "";     ///Optional For Now
                //objPayment.Nm = "";       ///Optional For Now
                //objPayment.PaidAmt = 0;   ///Optional For Now
                //objPayment.PayInstr = ""; ///Optional For Now
                //objPayment.PaymtDue = 0;  ///Optional For Now
                //objPayment.PayTerm = "";  ///Optional For Now
                //objInvoice.PayDtls = objPayment;


                //ReferenceDetails objRef = new ReferenceDetails();

                //List<ContractDetails> onjListContact = new List<ContractDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    ContractDetails onjContact = new ContractDetails();
                //    onjContact.ContrRefr = "";
                //    onjContact.ExtRefr = "";
                //    onjContact.PORefDt = "";
                //    onjContact.PORefr = "";
                //    onjContact.ProjRefr = "";
                //    onjContact.RecAdvDt = "";
                //    onjContact.RecAdvRefr = "";
                //    onjContact.TendRefr = "";
                //    onjListContact.Add(onjContact);
                //}
                //objRef.ContrDtls = onjListContact;


                //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                //    onjPrecDoc.InvDt = "";
                //    onjPrecDoc.InvNo = "";
                //    onjPrecDoc.OthRefNo = "";
                //    onjListPrecDoc.Add(onjPrecDoc);
                //}
                //objRef.PrecDocDtls = onjListPrecDoc;

                //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                //objdocPre.InvEndDt = "";
                //objdocPre.InvStDt = "";
                //objRef.DocPerdDtls = objdocPre;

                //objRef.InvRm = "";  // Remarks from invoice
                //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                //    objAddl.Docs = "";
                //    objAddl.Info = "";
                //    objAddl.Url = "";
                //    objListAddl.Add(objAddl);
                //}
                //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                List<ProductList> objListProd = new List<ProductList>();

                foreach (DataRow dr in Products.Rows)
                {
                    ProductList objProd = new ProductList();
                    // objProd.AssAmt = 0.00M;

                    //**************Commented for now -- This is foer Attribute adding ********************************//

                    //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                    //for (int j = 0; j < 1; j++)
                    //{
                    //    AttributeDetails objAttr = new AttributeDetails();
                    //    objAttr.Nm = "";
                    //    objAttr.Val = "";
                    //    objListAttr.Add(objAttr);
                    //}
                    //objProd.AttribDtls = objListAttr;

                    //**************End Commented for now -- This is foer Attribute adding ******************************//

                    objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Barcde = null;
                    objProd.BchDtls = null;
                    objProd.CesAmt = 0.00M;
                    objProd.CesNonAdvlAmt = 0.00M;
                    objProd.CesRt = 0.00M;
                    objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                    objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                    objProd.FreeQty = 0.00M;
                    objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                    objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                    objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                    if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                        objProd.IsServc = "N";
                    else
                        objProd.IsServc = "Y";
                    objProd.OrdLineRef = null;
                    objProd.OrgCntry = null;
                    objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                    objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                    objProd.PrdSlNo = null;
                    objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                    objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                    objProd.SlNo = Convert.ToString(dr["SL"]);
                    objProd.StateCesAmt = 0.00M;
                    objProd.StateCesNonAdvlAmt = 0.00M;
                    objProd.StateCesRt = 0.00M;
                    objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));

                    objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                    if (Convert.ToString(dr["GST_Print_Name"]) != "")
                        objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                    //else
                    //    objProd.Unit = "BAG";
                    objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00"));



                    if (Convert.ToDecimal(dr["InvoiceDetails_Discount"]) < 0)
                    {
                        objProd.TotAmt = Convert.ToDecimal((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])) - Convert.ToDecimal(dr["InvoiceDetails_Amount"])).ToString("0.00"));
                        Decimal Discount_Amount = Convert.ToDecimal(((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])) - Convert.ToDecimal(dr["InvoiceDetails_Amount"])).ToString("0.00"));
                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]) * Convert.ToDecimal(dr["InvoiceDetails_Quantity"])) - Discount_Amount).ToString("0.00"));
                    }
                    objListProd.Add(objProd);
                }
                objInvoice.ItemList = objListProd;


                //if (IrnOrgId != "webtel")
                //{
                authtokensOutput authObj = new authtokensOutput();
                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','0','" + err.Message + "')");
                        }
                        error = error + "," + objInvoice.DocDtls.No;
                    }
                }
                //   }

                try
                {
                    //Rev 5.0
                    IRNV3 objIRN = new IRNV3();
                    //IRN objIRN = new IRN();
                    //Rev 5.0 End
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;

                        var json = JsonConvert.SerializeObject(objInvoice, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        //Rev 5.0
                        //Rev Generate IRN (v1.0)
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                        //Rev Generate IRN (v1.0) End
                        //Rev Generate IRN (v3.0)
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                        //Rev Generate IRN (v3.0)
                        //Rev 5.0 End
                        var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            //Rev 5.0
                            //Rev Generate IRN (v3.0)
                            objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);

                            if (Convert.ToString(objIRN.data.Irn) != "")
                            {
                                DBEngine objDb = new DBEngine();

                                if (Convert.ToString(objIRN.data.EwbNo) != "" && objIRN.data.EwbNo != null)
                                {

                                    objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + objIRN.data.AckNo + "',AckDt='" + objIRN.data.AckDt + "',Irn='" + objIRN.data.Irn + "',SignedInvoice='" + objIRN.data.SignedInvoice + "',SignedQRCode='" + objIRN.data.SignedQRCode + "',Status='" + objIRN.data.Status + "',EWayBillNumber = '" + objIRN.data.EwbNo + "',EWayBillDate='" + objIRN.data.EwbDt + "',EwayBill_ValidTill='" + objIRN.data.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");
                                    //grid.JSProperties["cpSucessIRN"] = "Yes";

                                    IRNsuccess = IRNsuccess + "," + objInvoice.DocDtls.No;
                                }

                                else
                                {
                                    objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + objIRN.data.AckNo + "',AckDt='" + objIRN.data.AckDt + "',Irn='" + objIRN.data.Irn + "',SignedInvoice='" + objIRN.data.SignedInvoice + "',SignedQRCode='" + objIRN.data.SignedQRCode + "',Status='" + objIRN.data.Status + "' where invoice_id='" + id.ToString() + "'");

                                    IRNerror = IRNerror + "," + objInvoice.DocDtls.No;
                                }

                                success = success + "," + objInvoice.DocDtls.No;

                                // Mantis Issuse 24030 (24/05/2021)
                                string Customer_id = "", IsMailSend = "";

                                DataTable dtInvDetail = objDBEngineCredential.GetDataTable("SELECT Customer_Id,IsMailSend FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'");

                                if (dtInvDetail != null && dtInvDetail.Rows.Count > 0)
                                {
                                    Customer_id = Convert.ToString(dtInvDetail.Rows[0]["Customer_Id"]);
                                    IsMailSend = Convert.ToString(dtInvDetail.Rows[0]["IsMailSend"]);
                                }

                                if (IsMailSend == "False")
                                {
                                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                                    string msgBody = " <a href='" + baseUrl + "OMS/Management/Activities/ViewSIPDF.aspx?key=" + id.ToString() + "&dbname=" + con.Database + "'>Click here </a> to get your bill";

                                    SendMail(Convert.ToString(id.ToString()), msgBody, Customer_id);
                                }
                            }
                            //Rev Generate IRN (v3.0) End
                            //Rev 5.0 End
                            //Generate IRN (v1.0)
                            // objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                            //    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                            //    DBEngine objDb = new DBEngine();

                            //    if (Convert.ToString(objIRNDetails.EwbNo) != "" && objIRNDetails.EwbNo != null)
                            //    {

                            //        objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "',EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");
                            //        //grid.JSProperties["cpSucessIRN"] = "Yes";

                            //        IRNsuccess = IRNsuccess + "," + objInvoice.DocDtls.No;
                            //    }

                            //    else
                            //    {
                            //        objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "' where invoice_id='" + id.ToString() + "'");

                            //        IRNerror = IRNerror + "," + objInvoice.DocDtls.No;
                            //    }

                            //    success = success + "," + objInvoice.DocDtls.No;

                            //    // Mantis Issuse 24030 (24/05/2021)
                            //    string Customer_id = "", IsMailSend = "";

                            //    DataTable dtInvDetail = objDBEngineCredential.GetDataTable("SELECT Customer_Id,IsMailSend FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'");

                            //    if (dtInvDetail != null && dtInvDetail.Rows.Count > 0)
                            //    {
                            //        Customer_id = Convert.ToString(dtInvDetail.Rows[0]["Customer_Id"]);
                            //        IsMailSend = Convert.ToString(dtInvDetail.Rows[0]["IsMailSend"]);
                            //    }

                            //    if (IsMailSend == "False")
                            //    {
                            //        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                            //        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                            //        string msgBody = " <a href='" + baseUrl + "OMS/Management/Activities/ViewSIPDF.aspx?key=" + id.ToString() + "&dbname=" + con.Database + "'>Click here </a> to get your bill";

                            //        SendMail(Convert.ToString(id.ToString()), msgBody, Customer_id);
                            //    }
                            //    // End of Mantis Issuse 24030 (24/05/2021)
                            //}
                            //Generate IRN (v1.0) End
                        }
                        else
                        {

                            //Rev 6.0
                            IRNERRORV3 err = new IRNERRORV3();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            err = JsonConvert.DeserializeObject<IRNERRORV3>(jsonString);

                            DBEngine objDB = new DBEngine();
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_GEN'");
                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.details)
                                {
                                    if (Convert.ToString(item.ErrorCode) == "2150")
                                    {
                                        if (err.additionalInfo.details.AckNo != null)
                                        {
                                            //objDB.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + err.error.args.additionalDetails.AckNo + "',AckDt='" + err.error.args.additionalDetails.AckDt + "',Irn='" + err.error.args.additionalDetails.Irn + "',SignedInvoice='" + err.error.args.additionalDetails.SignedInvoice + "',SignedQRCode='" + err.error.args.additionalDetails.SignedQRCode + "',Status='" + err.error.args.additionalDetails.Status + "',EWayBillNumber = '" + err.error.args.additionalDetails.EwbNo + "',EWayBillDate='" + err.error.args.additionalDetails.EwbDt + "',EwayBill_ValidTill='" + err.error.args.additionalDetails.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");
                                            objDB.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + err.additionalInfo.details.AckNo + "',AckDt='" + err.additionalInfo.details.AckDt + "',Irn='" + err.additionalInfo.details.Irn + "',SignedInvoice='" + err.additionalInfo.details.SignedInvoice + "',SignedQRCode='" + err.additionalInfo.details.SignedQRCode + "',Status='" + err.additionalInfo.details.Status + "',EWayBillNumber = '" + err.additionalInfo.details.EwbNo + "',EWayBillDate='" + err.additionalInfo.details.EwbDt + "',EwayBill_ValidTill='" + err.additionalInfo.details.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");

                                        }
                                        else
                                        {
                                            foreach (infologV3 item1 in err.info)
                                            {                                                
                                                if (item1.Desc != null)
                                                {
                                                    objDB.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + item1.Desc.AckNo + "',AckDt='" + item1.Desc.AckDt + "',Irn='" + item1.Desc.Irn + "' where invoice_id='" + id.ToString() + "'");
                                                }
                                                else
                                                {
                                                    objDB.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + item1.Desc.AckNo + "',AckDt='" + item1.Desc.AckDt + "',Irn='" + item1.Desc.Irn + "' where invoice_id='" + id.ToString() + "'");
                                                }                                                
                                            }
                                        }
                                        success = success + "," + objInvoice.DocDtls.No;

                                    }
                                    else
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                        error = error + "," + objInvoice.DocDtls.No;
                                        IRNerror = IRNerror + "," + objInvoice.DocDtls.No;
                                    }
                                }

                                //Cancel IRN (v1.0)
                                //foreach (errorlog item in err.error.args.irp_error.details)
                                //{
                                //    if (Convert.ToString(item.ErrorCode) == "2150")
                                //    {
                                //        if(err.error.args.irp_error.additionalDetails.AckNo!=null)
                                //        {
                                //            objDB.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + err.error.args.irp_error.additionalDetails.AckNo + "',AckDt='" + err.error.args.irp_error.additionalDetails.AckDt + "',Irn='" + err.error.args.irp_error.additionalDetails.Irn + "',SignedInvoice='" + err.error.args.irp_error.additionalDetails.SignedInvoice + "',SignedQRCode='" + err.error.args.irp_error.additionalDetails.SignedQRCode + "',Status='" + err.error.args.irp_error.additionalDetails.Status + "',EWayBillNumber = '" + err.error.args.irp_error.additionalDetails.EwbNo + "',EWayBillDate='" + err.error.args.irp_error.additionalDetails.EwbDt + "',EwayBill_ValidTill='" + err.error.args.irp_error.additionalDetails.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");
                                //        }
                                //        else
                                //        {
                                //            foreach (infolog item1 in err.error.args.irp_error.info)
                                //            {
                                //                //REV 3.0
                                //                if (item1.InfoDesc!=null)
                                //                {
                                //                    objDB.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + item1.InfoDesc.AckNo + "',AckDt='" + item1.InfoDesc.AckDt + "',Irn='" + item1.InfoDesc.Irn + "' where invoice_id='" + id.ToString() + "'");
                                //                }
                                //                else
                                //                {
                                //                    objDB.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + item1.Desc.AckNo + "',AckDt='" + item1.Desc.AckDt + "',Irn='" + item1.Desc.Irn + "' where invoice_id='" + id.ToString() + "'");
                                //                }
                                //                //REV 3.0 END
                                //        }
                                //    }
                                //        success = success + "," + objInvoice.DocDtls.No;

                                //    }
                                //    else
                                //    {
                                //        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                //        error = error + "," + objInvoice.DocDtls.No;
                                //        IRNerror = IRNerror + "," + objInvoice.DocDtls.No;
                                //    }                                        
                                //}
                                //Cancel IRN (v1.0) End
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','" + "0" + "','" + item + "')");
                                }

                                error = error + "," + objInvoice.DocDtls.No;
                                IRNerror = IRNerror + "," + objInvoice.DocDtls.No;
                            }

                            //Rev 6.0 End


                            //grid.JSProperties["cpSucessIRN"] = "No";
                        }


                    }

                }
                catch (AggregateException err)
                {
                    DBEngine objDB = new DBEngine();
                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_GEN'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','0','" + err.Message + "')");
                    }
                    error = error + "," + objInvoice.DocDtls.No;
                }
            }
            else if (e.Parameters.Split('~')[0] == "UploadBulkIRN")
            {
                var ids = GrdQuotationPendinSI.GetSelectedFieldValues("Invoice_Id");
                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];
                foreach (var id in ids)
                {


                    DataSet ds = new DataSet();
                    if (e.Parameters.Split('~')[1] == "SI")
                    {
                        ds = GetInvoiceDetails(id.ToString());
                    }

                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];


                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(Header.Rows[0]["Invoice_BranchId"]); ;
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;

                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }

                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;



                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                    EinvoiceToken.Expiry = date;

                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            DBEngine objDB = new DBEngine();
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_GEN'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','0','" + err.Message + "')");
                            }
                        }
                    }
                    try
                    {
                        IRN objIRN = new IRN();
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objInvoice, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0
                            //Rev Cancel IRN (v1.0)
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            //Rev Cancel IRN (v1.0) End
                            //Rev Cancel IRN (v3.0)
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                            //Rev Cancel IRN (v3.0) End
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                {
                                    // Deserialization from JSON  
                                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                                    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                                    DBEngine objDb = new DBEngine();

                                    if (Convert.ToString(objIRNDetails.EwbNo) != "")
                                    {

                                        objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "',EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");
                                        //grid.JSProperties["cpSucessIRN"] = "Yes";

                                        IRNsuccess = IRNsuccess + "," + objInvoice.DocDtls.No;

                                        // Mantis Issuse 24030 (24/05/2021)
                                        string Customer_id = "", IsMailSend = "";

                                        DataTable dtInvDetail = objDBEngineCredential.GetDataTable("SELECT Customer_Id,IsMailSend FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'");

                                        if (dtInvDetail != null && dtInvDetail.Rows.Count > 0)
                                        {
                                            Customer_id = Convert.ToString(dtInvDetail.Rows[0]["Customer_Id"]);
                                            IsMailSend = Convert.ToString(dtInvDetail.Rows[0]["IsMailSend"]);
                                        }

                                        if (IsMailSend == "False")
                                        {
                                            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                                            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                                            string msgBody = " <a href='" + baseUrl + "OMS/Management/Activities/ViewSIPDF.aspx?key=" + id.ToString() + "&dbname=" + con.Database + "'>Click here </a> to get your bill";

                                            SendMail(Convert.ToString(id.ToString()), msgBody, Customer_id);
                                        }
                                        // End of Mantis Issuse 24030 (24/05/2021)
                                    }

                                    else
                                    {
                                        objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "' where invoice_id='" + id.ToString() + "'");

                                        IRNerror = IRNerror + "," + objInvoice.DocDtls.No;
                                    }
                                    //grid.JSProperties["cpSucessIRN"] = "Yes";
                                    success = success + "," + objInvoice.DocDtls.No;
                                }
                            }
                            else
                            {
                                EinvoiceError err = new EinvoiceError();
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                error = error + "," + objInvoice.DocDtls.No;
                                IRNerror = IRNerror + "," + objInvoice.DocDtls.No;
                                DBEngine objDB = new DBEngine();
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_GEN'");

                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.irp_error.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''").Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','" + "0" + "','" + item + "')");
                                    }

                                }
                            }


                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','0','" + err.Message + "')");
                        }
                    }
                }

            }
            else if (e.Parameters.Split('~')[0] == "DownloadJSON")
            {
                GrdQuotationPendinSI.JSProperties["cpJson"] = "Yes";

                var ids = GrdQuotationPendinSI.GetSelectedFieldValues("Invoice_Id");
                List<EinvoiceModel> obj = new List<EinvoiceModel>();
                foreach (var id in ids)
                {

                    DataSet ds = GetInvoiceDetails(id.ToString());
                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(Header.Rows[0]["Invoice_BranchId"]); ;
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;

                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }

                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;

                    obj.Add(objInvoice);
                    GrdQuotationPendinSI.JSProperties["cpJson"] = "Yes";
                }


                Session["obj"] = obj;




            }
            GrdQuotationPendinSI.JSProperties["cpSuccessMsg"] = success;
            GrdQuotationPendinSI.JSProperties["cpFalureMsg"] = error;
            GrdQuotationPendinSI.JSProperties["cpIRNSuccessMsg"] = IRNsuccess;
            GrdQuotationPendinSI.JSProperties["cpIRNFalureMsg"] = IRNerror;
        }

        protected void LinqServerModeDataSourceTSI_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceTSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceTSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoiceTSIs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void LinqServerModeDataSourcePendingTSI_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceTSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) == "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceTSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) == "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoiceTSIs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void LinqServerModeDataSourceCancelTSI_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceTSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.IRNCancel_BranchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == true
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceTSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.IRNCancel_BranchId)) &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == true
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoiceTSIs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GrdQuotationTSI_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdQuotationTSI.JSProperties["cpSuccessMsg"] = null;
            GrdQuotationTSI.JSProperties["cpFalureMsg"] = null;

            string success = "";
            string error = "";
            if (e.Parameters.Split('~')[0] == "DownloadJSON")
            {
                var ids = GrdQuotationTSI.GetSelectedFieldValues("Invoice_Id");
                List<EinvoiceModel> obj = new List<EinvoiceModel>();
                foreach (var id in ids)
                {

                    DataSet ds = GetInvoiceDetailsTSI(id.ToString());
                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(Header.Rows[0]["Invoice_BranchId"]); ;
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;

                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }


                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;

                    obj.Add(objInvoice);
                    GrdQuotationTSI.JSProperties["cpJson"] = "Yes";
                }


                Session["obj"] = obj;




            }
            else if (e.Parameters.Split('~')[0] == "CancelIRN")
            {
                GrdQuotationTSI.JSProperties["cpJson"] = "No";
                string output = "";
                var ids = GrdQuotationTSI.GetSelectedFieldValues("Invoice_Id");
                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];

                foreach (var id in ids)
                {
                    DBEngine objDB = new DBEngine();
                    string irn = Convert.ToString(objDB.GetDataTable("SELECT IRN FROM TBL_TRANS_TRANSITSALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                    CancelDetails objCancelDetails = new CancelDetails();
                    objCancelDetails.Irn = irn;
                    objCancelDetails.CnlRem = e.Parameters.Split('~')[2];
                    objCancelDetails.CnlRsn = e.Parameters.Split('~')[1];

                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;


                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                    EinvoiceToken.Expiry = date;
                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_CANCEL'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','0','" + err.Message + "')");
                            }
                            error = error + "," + irn;
                        }
                    }
                    try
                    {
                        IRN objIRN = new IRN();
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objCancelDetails, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0
                            //Rev Cancel IRN (v1.0)
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            //Rev Cancel IRN (v1.0) End
                            //Rev Cancel IRN (v3.0)
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                            //Rev Cancel IRN (v3.0) End
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnCancelUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                {
                                    // Deserialization from JSON  
                                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelIRNOutput));
                                    CancelIRNOutput objIRNDetails = (CancelIRNOutput)deserializer.ReadObject(ms);
                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRNDetails.CancelDate + "' WHERE Irn='" + objIRNDetails.Irn + "'");
                                    objDb.GetDataTable("EXEC PRC_CANCELIRNTSI " + id + "");
                                    output = "IRN Cancelled successfully.";
                                    success = success + "," + irn;
                                }



                            }
                            else
                            {

                                EinvoiceError err = new EinvoiceError();
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' and ERROR_TYPE='IRN_CANCEL'");
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.irp_error.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','" + "0" + "','" + item + "')");
                                    }
                                }

                                error = error + "," + irn;


                                output = "Error occurs while IRN Cancellation.";
                            }

                            // GrdQuotationTSI.JSProperties["cpJSON"] = "Cancel";

                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_CANCEL','0','" + err.Message + "')");
                        }
                        error = error + "," + irn;
                    }
                }
            }
            else if (e.Parameters.Split('~')[0] == "GenEwayBill")
            {

                string output = "";

                DBEngine objDB = new DBEngine();
                string irn = e.Parameters.Split('~')[1];
                string id = e.Parameters.Split('~')[2];
                DataSet ds = GetInvoiceDetailsTSI(id);
                DataTable Header = ds.Tables[0];


                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];
                EwayBillGeneration objEwaybill = new EwayBillGeneration();
                objEwaybill.Irn = irn;
                if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    objEwaybill.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                else
                    objEwaybill.Distance = 0;
                ///
                if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    objEwaybill.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 





                authtokensOutput authObj = new authtokensOutput();
                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {

                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                EinvoiceToken.Expiry = date;

                            }
                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAY_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','0','" + err.Message + "')");
                        }

                        error = error + "," + irn;


                    }
                }
                try
                {
                    //Rev 5.0
                    //IRN objIRN = new IRN();
                    //Rev 7.0
                    // IRNV3 objIRN = new IRNV3();
                    EWAYBILLV3 objIRN = new EWAYBILLV3();
                    //Rev 7.0 ENd
                    //Rev 5.0 End
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objEwaybill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        //Rev 5.0
                        //Rev Cancel IRN (v1.0)
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                        //Rev Cancel IRN (v1.0) End
                        //Rev Cancel IRN (v3.0)
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                        //Rev Cancel IRN (v3.0) End
                        //Rev 5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync(IrnEwaybillUrl, stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            //Rev 5.0
                            //objIRN = response.Content.ReadAsAsync<IRN>().Result;
                            //Rev 7.0
                            //objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                            objIRN = JsonConvert.DeserializeObject<EWAYBILLV3>(jsonString);
                            //Rev 7.0 End
                            if (Convert.ToString(objIRN.status) == "1")
                            {
                                
                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET EWayBillNumber = '" + objIRN.data.EwbNo + "',EWayBillDate='" + objIRN.data.EwbDt + "',EwayBill_ValidTill='" + objIRN.data.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");
                                output = "IRN Cancelled successfully.";
                                success = success + "," + irn;
                            }

                            //    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                            //    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                            //    DBEngine objDb = new DBEngine();
                            //    objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");
                            //    output = "IRN Cancelled successfully.";
                            //    success = success + "," + irn;
                            //}
                            //Rev 5.0 End
                        }
                        else
                        {
                            EinvoiceErrorV3 err = new EinvoiceErrorV3();
                            //EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' and ERROR_TYPE='EWAY_GEN'");
                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.details)
                                //foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                if (cErr.error.args.errors != null)
                                {
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','" + "0" + "','" + item + "')");
                                    }
                                }
                                else
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','" + "0" + "','Invalid request body.')");
                                }
                            }
                            error = error + "," + irn;
                            output = "Error occurs while IRN Cancellation.";
                        }



                    }
                }
                catch (AggregateException err)
                {
                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAY_GEN'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','0','" + err.Message + "')");
                    }

                    error = error + "," + irn;
                }

                GrdQuotationTSI.JSProperties["cpJSON"] = "Ewaybill";
            }
            else if (e.Parameters.Split('~')[0] == "GenEwayBillBulk")
            {
                string output = "";
                var ids = GrdQuotationTSI.GetSelectedFieldValues("Invoice_Id");
                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];

                foreach (var id in ids)
                {



                    DBEngine objDB = new DBEngine();
                    string irn = Convert.ToString(objDB.GetDataTable("SELECT IRN FROM TBL_TRANS_TRANSITSALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataSet ds = GetInvoiceDetailsTSI(id.ToString());
                    DataTable Header = ds.Tables[0];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDB.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                    EwayBillGeneration objEwaybill = new EwayBillGeneration();
                    objEwaybill.Irn = irn;
                    if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                        objEwaybill.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    else
                        objEwaybill.Distance = 0;
                    ///
                    if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                        objEwaybill.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 





                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {

                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                    EinvoiceToken.Expiry = date;

                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAY_GEN'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','0','" + err.Message + "')");
                            }

                            error = error + "," + irn;


                        }
                    }
                    try
                    {
                        IRN objIRN = new IRN();
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objEwaybill, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0
                            //Rev Cancel IRN (v1.0)
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            //Rev Cancel IRN (v1.0) End
                            //Rev Cancel IRN (v3.0)
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                            //Rev Cancel IRN (v3.0) End
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnEwaybillUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                {
                                    // Deserialization from JSON  
                                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                                    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "' where invoice_id='" + id.ToString() + "'");
                                    output = "IRN Cancelled successfully.";
                                    success = success + "," + irn;
                                }



                            }
                            else
                            {

                                EinvoiceError err = new EinvoiceError();
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' and ERROR_TYPE='EWAY_GEN'");
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.irp_error.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    if (cErr.error.args.errors != null)
                                    {
                                        foreach (string item in cErr.error.args.errors)
                                        {
                                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','" + "0" + "','" + item + "')");
                                        }
                                    }
                                    else
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','" + "0" + "','Invalid request body.')");
                                    }

                                }

                                error = error + "," + irn;


                                output = "Error occurs while IRN Cancellation.";
                            }



                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAY_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAY_GEN','0','" + err.Message + "')");
                        }

                        error = error + "," + irn;
                    }
                }

                GrdQuotationTSI.JSProperties["cpJSON"] = "Ewaybill";
            }

            GrdQuotationTSI.JSProperties["cpSuccessMsg"] = success;
            GrdQuotationTSI.JSProperties["cpFalureMsg"] = error;

        }

        protected void GrdQuotationPendingTSI_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdQuotationPendingTSI.JSProperties["cpJson"] = "No";
            GrdQuotationPendingTSI.JSProperties["cpSuccessMsg"] = null;
            GrdQuotationPendingTSI.JSProperties["cpFalureMsg"] = null;

            string success = "";
            string error = "";
            if (e.Parameters.Split('~')[0] == "UploadSingleIRN")
            {
                DataSet ds = new DataSet();

                string id = e.Parameters.Split('~')[2];
                if (e.Parameters.Split('~')[1] == "SI")
                {
                    ds = GetInvoiceDetailsTSI(id);
                }

                DataTable Header = ds.Tables[0];
                DataTable SellerDetails = ds.Tables[1];
                DataTable BuyerDetails = ds.Tables[2];
                DataTable ValueDetails = ds.Tables[3];
                DataTable Products = ds.Tables[4];
                DataTable ShipDetails = ds.Tables[5];
                DataTable DispatchFrom = ds.Tables[6];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];



                EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                TrasporterDetails objTransporter = new TrasporterDetails();
                objTransporter.EcmGstin = null;
                objTransporter.IgstOnIntra = "N";
                if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                {
                    objTransporter.RegRev = "Y";     /// From table mantis id 23407
                }
                else
                {
                    objTransporter.RegRev = "N";
                }
                if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                    objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                else
                    objTransporter.SupTyp = "B2B";
                objTransporter.TaxSch = "GST";
                objInvoice.TranDtls = objTransporter;


                DocumentsDetails objDoc = new DocumentsDetails();
                objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                objInvoice.DocDtls = objDoc;


                SellerDetails objSeller = new SellerDetails();
                objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                    objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                else
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                /// 
                if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                    objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                objInvoice.SellerDtls = objSeller;


                BuyerDetails objBuyer = new BuyerDetails();
                objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                    objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                else
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                    objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objInvoice.BuyerDtls = objBuyer;


                objInvoice.DispDtls = null;  // for now 
                objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                ValueDetails objValue = new ValueDetails();
                objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                objValue.CesVal = 0.00M;
                objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                objValue.Discount = 0.00M;
                objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                objValue.RndOffAmt = 0.00M;
                objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                objValue.StCesVal = 0.00M;
                objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                objValue.TotInvValFc = 0.00M;
                objInvoice.ValDtls = objValue;


                //ExportDetails objExport = new ExportDetails();
                //objExport.CntCode = ""; ///optional for now
                //objExport.ExpDuty = 0;  ///optional for now
                //objExport.ForCur = "";  ///optional for now
                //objExport.Port = "";    ///optional for now
                //objExport.RefClm = "";  ///optional for now
                //objExport.ShipBDt = ""; ///optional for now
                //objExport.ShipBNo = ""; ///optional for now
                //objInvoice.ExpDtls = objExport;

                //EwayBillDetails objEway = new EwayBillDetails();
                //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                //else
                //    objEway.Distance = 0;
                /////
                //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                //objInvoice.EwbDtls = objEway;

                // Mantis Issue 24608
                if (DispatchFrom.Rows.Count > 0)
                {
                    // End of Mantis Issue 24608
                    DispatchDetails objDisp = new DispatchDetails();
                    objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                    objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                    objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                    objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                    objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                    objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                    objInvoice.DispDtls = objDisp;
                }

                // Mantis Issue 24608
                if (ShipDetails.Rows.Count > 0)
                {
                    // End of Mantis Issue 24608
                    ShipToDetails objShip = new ShipToDetails();
                    objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                    objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                    objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                    objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                    objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                    objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                    objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                    objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                    objInvoice.ShipDtls = objShip;
                }

                //PaymentDetails objPayment = new PaymentDetails();
                //objPayment.AccDet = "";   ///Optional For Now
                //objPayment.CrDay = 0;     ///Optional For Now
                //objPayment.CrTrn = "";    ///Optional For Now
                //objPayment.DirDr = "";    ///Optional For Now
                //objPayment.FinInsBr = ""; ///Optional For Now
                //objPayment.Mode = "";     ///Optional For Now
                //objPayment.Nm = "";       ///Optional For Now
                //objPayment.PaidAmt = 0;   ///Optional For Now
                //objPayment.PayInstr = ""; ///Optional For Now
                //objPayment.PaymtDue = 0;  ///Optional For Now
                //objPayment.PayTerm = "";  ///Optional For Now
                //objInvoice.PayDtls = objPayment;


                //ReferenceDetails objRef = new ReferenceDetails();

                //List<ContractDetails> onjListContact = new List<ContractDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    ContractDetails onjContact = new ContractDetails();
                //    onjContact.ContrRefr = "";
                //    onjContact.ExtRefr = "";
                //    onjContact.PORefDt = "";
                //    onjContact.PORefr = "";
                //    onjContact.ProjRefr = "";
                //    onjContact.RecAdvDt = "";
                //    onjContact.RecAdvRefr = "";
                //    onjContact.TendRefr = "";
                //    onjListContact.Add(onjContact);
                //}
                //objRef.ContrDtls = onjListContact;


                //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                //    onjPrecDoc.InvDt = "";
                //    onjPrecDoc.InvNo = "";
                //    onjPrecDoc.OthRefNo = "";
                //    onjListPrecDoc.Add(onjPrecDoc);
                //}
                //objRef.PrecDocDtls = onjListPrecDoc;

                //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                //objdocPre.InvEndDt = "";
                //objdocPre.InvStDt = "";
                //objRef.DocPerdDtls = objdocPre;

                //objRef.InvRm = "";  // Remarks from invoice
                //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                //    objAddl.Docs = "";
                //    objAddl.Info = "";
                //    objAddl.Url = "";
                //    objListAddl.Add(objAddl);
                //}
                //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                List<ProductList> objListProd = new List<ProductList>();

                foreach (DataRow dr in Products.Rows)
                {
                    ProductList objProd = new ProductList();
                    // objProd.AssAmt = 0.00M;

                    //**************Commented for now -- This is foer Attribute adding ********************************//

                    //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                    //for (int j = 0; j < 1; j++)
                    //{
                    //    AttributeDetails objAttr = new AttributeDetails();
                    //    objAttr.Nm = "";
                    //    objAttr.Val = "";
                    //    objListAttr.Add(objAttr);
                    //}
                    //objProd.AttribDtls = objListAttr;

                    //**************End Commented for now -- This is foer Attribute adding ******************************//

                    objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Barcde = null;
                    objProd.BchDtls = null;
                    objProd.CesAmt = 0.00M;
                    objProd.CesNonAdvlAmt = 0.00M;
                    objProd.CesRt = 0.00M;
                    objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                    objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                    objProd.FreeQty = 0.00M;
                    objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                    objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                    objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                    if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                        objProd.IsServc = "N";
                    else
                        objProd.IsServc = "Y";
                    objProd.OrdLineRef = null;
                    objProd.OrgCntry = null;
                    objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                    objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                    objProd.PrdSlNo = null;
                    objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                    objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                    objProd.SlNo = Convert.ToString(dr["SL"]);
                    objProd.StateCesAmt = 0.00M;
                    objProd.StateCesNonAdvlAmt = 0.00M;
                    objProd.StateCesRt = 0.00M;
                    objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                    if (Convert.ToString(dr["GST_Print_Name"]) != "")
                        objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                    //else
                    //    objProd.Unit = "BAG";
                    objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                    objListProd.Add(objProd);
                }
                objInvoice.ItemList = objListProd;



                authtokensOutput authObj = new authtokensOutput();
                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_GEN','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    //Rev 5.0
                    //IRN objIRN = new IRN();
                    IRNV3 objIRN = new IRNV3();
                    //Rev 5.0 End
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objInvoice, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        //Rev 5.0                       
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");                       
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");                       
                        //Rev 5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            //Rev 5.0 
                            objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                            if (Convert.ToString(objIRN.data.Irn) != "")
                            {
                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET AckNo='" + objIRN.data.AckNo + "',AckDt='" + objIRN.data.AckDt + "',Irn='" + objIRN.data.Irn + "',SignedInvoice='" + objIRN.data.SignedInvoice + "',SignedQRCode='" + objIRN.data.SignedQRCode + "',Status='" + objIRN.data.Status + "' where invoice_id='" + id.ToString() + "'");
                              
                                success = success + "," + objInvoice.DocDtls.No;
                            }

                            //objIRN = response.Content.ReadAsAsync<IRN>().Result;
                            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                            //    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                            //    DBEngine objDb = new DBEngine();
                            //    objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "' where invoice_id='" + id.ToString() + "'");
                            //    //grid.JSProperties["cpSucessIRN"] = "Yes";
                            //    success = success + "," + objInvoice.DocDtls.No;
                            //}
                            //Rev 5.0 End
                        }
                        else
                        {
                            //Rev 5.0
                            EinvoiceErrorV3 err = new EinvoiceErrorV3();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);

                            //EinvoiceError err = new EinvoiceError();
                            //var jsonString = response.Content.ReadAsStringAsync().Result;                            
                            //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            //Rev 5.0 End
                            error = error + "," + objInvoice.DocDtls.No;

                            DBEngine objDB = new DBEngine();
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_GEN'");

                            if (err.error.type != "ClientRequest")
                            {
                                //Rev 5.0
                                foreach (errorlog item in err.error.args.details)
                                //foreach (errorlog item in err.error.args.irp_error.details)
                                //Rev 5.0 End
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_GEN','" + "0" + "','" + item + "')");
                                }
                            }




                            //grid.JSProperties["cpSucessIRN"] = "No";
                        }


                    }
                }
                catch (AggregateException err)
                {
                    DBEngine objDB = new DBEngine();
                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_GEN'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_GEN','0','" + err.Message + "')");
                    }
                }
            }
            else if (e.Parameters.Split('~')[0] == "UploadBulkIRN")
            {
                var ids = GrdQuotationPendingTSI.GetSelectedFieldValues("Invoice_Id");
                foreach (var id in ids)
                {


                    DataSet ds = new DataSet();
                    ds = GetInvoiceDetailsTSI(id.ToString());

                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                    string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                    string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                    string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                    string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                    string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                    string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                    string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];


                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;

                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }

                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;



                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                    EinvoiceToken.Expiry = date;

                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            DBEngine objDB = new DBEngine();
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_GEN'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_GEN','0','" + err.Message + "')");
                            }
                        }
                    }

                    try
                    {
                        //Rev 5.0
                        //IRN objIRN = new IRN();
                        IRNV3 objIRN = new IRNV3();
                        //Rev 5.0 End
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objInvoice, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0                          
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");                            
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");                            
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;

                                //Rev 5.0 
                                objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                                if (Convert.ToString(objIRN.data.Irn) != "")
                                {
                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET AckNo='" + objIRN.data.AckNo + "',AckDt='" + objIRN.data.AckDt + "',Irn='" + objIRN.data.Irn + "',SignedInvoice='" + objIRN.data.SignedInvoice + "',SignedQRCode='" + objIRN.data.SignedQRCode + "',Status='" + objIRN.data.Status + "' where invoice_id='" + id.ToString() + "'");                                   
                                    success = success + "," + objInvoice.DocDtls.No;
                                }


                                //objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                //{
                                //    // Deserialization from JSON  
                                //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                                //    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                                //    DBEngine objDb = new DBEngine();
                                //    objDb.GetDataTable("update TBL_TRANS_TRANSITSALESINVOICE SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "' where invoice_id='" + id.ToString() + "'");
                                //    //grid.JSProperties["cpSucessIRN"] = "Yes";
                                //    success = success + "," + objInvoice.DocDtls.No;
                                //}
                                //Rev 5.0 End
                            }
                            else
                            {
                                //Rev 5.0
                                EinvoiceErrorV3 err = new EinvoiceErrorV3();
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);


                                //EinvoiceError err = new EinvoiceError();
                                //var jsonString = response.Content.ReadAsStringAsync().Result;
                                //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                //Rev 5.0 End
                                error = error + "," + objInvoice.DocDtls.No;


                                DBEngine objDB = new DBEngine();
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_GEN'");
                                if (err.error.type != "ClientRequest")
                                {
                                    //Rev 5.0 
                                    foreach (errorlog item in err.error.args.details)
                                    //foreach (errorlog item in err.error.args.irp_error.details)
                                    //Rev 5.0 End
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_GEN','" + "0" + "','" + item + "')");
                                    }
                                }
                            }


                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='IRN_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','IRN_GEN','0','" + err.Message + "')");
                        }
                    }
                }
            }
            else if (e.Parameters.Split('~')[0] == "DownloadJSON")
            {
                var ids = GrdQuotationPendingTSI.GetSelectedFieldValues("Invoice_Id");
                List<EinvoiceModel> obj = new List<EinvoiceModel>();
                foreach (var id in ids)
                {

                    DataSet ds = GetInvoiceDetails(id.ToString());
                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;

                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }

                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;

                    obj.Add(objInvoice);
                    GrdQuotationPendingTSI.JSProperties["cpJson"] = "Yes";
                }


                Session["obj"] = obj;




            }

            GrdQuotationPendingTSI.JSProperties["cpSuccessMsg"] = success;
            GrdQuotationPendingTSI.JSProperties["cpFalureMsg"] = error;
        }

        protected void LinqServerModeDataSourceCR_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceSRs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceSRs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoiceSRs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GrdQuotationCR_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdQuotationCR.JSProperties["cpSuccessMsg"] = null;
            GrdQuotationCR.JSProperties["cpFalureMsg"] = null;

            string success = "";
            string error = "";
            if (e.Parameters.Split('~')[0] == "DownloadJSON")
            {
                var ids = GrdQuotationCR.GetSelectedFieldValues("Invoice_Id");
                List<EinvoiceModel> obj = new List<EinvoiceModel>();
                foreach (var id in ids)
                {



                    DataSet ds = GetInvoiceDetailsTSI(id.ToString());
                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "CRN";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;

                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }

                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;

                    obj.Add(objInvoice);
                    GrdQuotationCR.JSProperties["cpJson"] = "Yes";
                }


                Session["obj"] = obj;




            }
            else if (e.Parameters.Split('~')[0] == "CancelIRN")
            {
                GrdQuotationCR.JSProperties["cpJson"] = "No";
                string output = "";
                var ids = GrdQuotationCR.GetSelectedFieldValues("Invoice_Id");
                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];

                foreach (var id in ids)
                {
                    DBEngine objDB = new DBEngine();
                    string irn = Convert.ToString(objDB.GetDataTable("SELECT IRN FROM TBL_TRANS_SALESRETURN WHERE RETURN_ID='" + id.ToString() + "'").Rows[0][0]);

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT RETURN_BranchId FROM TBL_TRANS_SALESRETURN WHERE RETURN_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                    CancelDetails objCancelDetails = new CancelDetails();
                    objCancelDetails.Irn = irn;
                    objCancelDetails.CnlRem = e.Parameters.Split('~')[2];
                    objCancelDetails.CnlRsn = e.Parameters.Split('~')[1];

                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                    EinvoiceToken.Expiry = date;
                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_CANCEL'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','0','" + err.Message + "')");
                            }
                        }
                    }
                    try
                    {
                        IRN objIRN = new IRN();
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objCancelDetails, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0
                            //Rev Cancel IRN (v1.0)
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            //Rev Cancel IRN (v1.0) End
                            //Rev Cancel IRN (v3.0)
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                            //Rev Cancel IRN (v3.0) End
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnCancelUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                {
                                    // Deserialization from JSON  
                                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelIRNOutput));
                                    CancelIRNOutput objIRNDetails = (CancelIRNOutput)deserializer.ReadObject(ms);
                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET IsIRNCancelled=1,IRN_Cancell_Date='" + objIRNDetails.CancelDate + "' WHERE Irn='" + objIRNDetails.Irn + "'");
                                    objDb.GetDataTable("EXEC PRC_CANCELIRNSR " + id + "");
                                    output = "IRN Cancelled successfully.";

                                    success = success + "," + irn;
                                }



                            }
                            else
                            {
                                EinvoiceError err = new EinvoiceError();
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' and ERROR_TYPE='IRN_CANCEL'");
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.irp_error.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','" + "0" + "','" + item + "')");
                                    }
                                }

                                error = error + "," + irn;

                                output = "Error occurs while IRN Cancellation.";
                            }

                            GrdQuotation.JSProperties["cpJSON"] = "Cancel";

                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_CANCEL'");
                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_CANCEL','0','" + err.Message + "')");
                        }
                    }
                }
            }


            else if (e.Parameters.Split('~')[0] == "GenEwayBill")
            {

                string output = "";
                GrdQuotationCR.JSProperties["cpJSON"] = "Ewaybill";
                DBEngine objDB = new DBEngine();
                string irn = e.Parameters.Split('~')[1];
                string id = e.Parameters.Split('~')[2];
                DataSet ds = GetInvoiceDetailsSR(id);
                DataTable Header = ds.Tables[0];


                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT RETURN_BranchId FROM TBL_TRANS_SALESRETURN WHERE RETURN_ID='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];


                EwayBillGeneration objEwaybill = new EwayBillGeneration();
                objEwaybill.Irn = irn;
                if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    objEwaybill.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                else
                    objEwaybill.Distance = 0;
                ///
                if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    objEwaybill.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objEwaybill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 





                authtokensOutput authObj = new authtokensOutput();
                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {

                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                EinvoiceToken.Expiry = date;

                            }
                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAY_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','0','" + err.Message + "')");
                        }

                        error = error + "," + irn;


                    }
                }
                try
                {
                    //REV 5.0
                    IRNV3 objIRN = new IRNV3();
                    //IRN objIRN = new IRN();
                    //REV 5.0 END
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objEwaybill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        //Rev 5.0                        
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");                       
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");                       
                        //Rev 5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync(IrnEwaybillUrl, stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            //REV 5.0
                            objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                            if (Convert.ToString(objIRN.status) == "1")
                            {
                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET EWayBillNumber = '" + objIRN.data.EwbNo + "',EWayBillDate='" + objIRN.data.EwbDt + "',EwayBill_ValidTill='" + objIRN.data.EwbValidTill + "' where return_id='" + id.ToString() + "'");
                                output = "IRN Cancelled successfully.";
                                success = success + "," + irn;
                            }
                            //objIRN = response.Content.ReadAsAsync<IRN>().Result;
                            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                            //    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                            //    DBEngine objDb = new DBEngine();
                            //    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "' where return_id='" + id.ToString() + "'");
                            //    output = "IRN Cancelled successfully.";
                            //    success = success + "," + irn;
                            //}
                            //REV 5.0 END


                        }
                        else
                        {
                            //REV 5.0 
                            EinvoiceErrorV3 err = new EinvoiceErrorV3();
                            //EinvoiceError err = new EinvoiceError();
                            //REV 5.0 end
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            //REV 5.0 
                            //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);
                            //REV 5.0  END
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' and ERROR_TYPE='EWAY_GEN'");
                            if (err.error.type != "ClientRequest")
                            {
                                //REV 5.0
                                foreach (errorlog item in err.error.args.details)
                                //foreach (errorlog item in err.error.args.irp_error.details)
                                //REV 5.0
                                {

                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                if (cErr.error.args.errors != null)
                                {
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','" + "0" + "','" + item + "')");
                                    }
                                }
                                else
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','" + "0" + "','Invalid request body.')");
                                }

                            }

                            error = error + "," + irn;


                            output = "Error occurs while IRN Cancellation.";
                        }



                    }
                }
                catch (AggregateException err)
                {
                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAY_GEN'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','0','" + err.Message + "')");
                    }

                    error = error + "," + irn;
                }

                GrdQuotationTSI.JSProperties["cpJSON"] = "Ewaybill";
            }
            else if (e.Parameters.Split('~')[0] == "GenEwayBillBulk")
            {
                string output = "";
                var ids = GrdQuotationCR.GetSelectedFieldValues("Invoice_Id");

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];
                foreach (var id in ids)
                {



                    DBEngine objDB = new DBEngine();
                    string irn = Convert.ToString(objDB.GetDataTable("SELECT IRN FROM TBL_TRANS_SALESRETURN WHERE RETURN_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataSet ds = GetInvoiceDetailsSR(id.ToString());
                    DataTable Header = ds.Tables[0];

                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT RETURN_BranchId FROM TBL_TRANS_SALESRETURN WHERE RETURN_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                    EwayBillGeneration objEwaybill = new EwayBillGeneration();
                    objEwaybill.Irn = irn;
                    if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                        objEwaybill.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    else
                        objEwaybill.Distance = 0;
                    ///
                    if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                        objEwaybill.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                        objEwaybill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 





                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {

                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                    EinvoiceToken.Expiry = date;

                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAY_GEN'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','0','" + err.Message + "')");
                            }

                            error = error + "," + irn;


                        }
                    }
                    try
                    {
                        IRN objIRN = new IRN();
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objEwaybill, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0
                            //Rev Cancel IRN (v1.0)
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            //Rev Cancel IRN (v1.0) End
                            //Rev Cancel IRN (v3.0)
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");
                            //Rev Cancel IRN (v3.0) End
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            // var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            var response = client.PostAsync(IrnEwaybillUrl, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                objIRN = response.Content.ReadAsAsync<IRN>().Result;

                                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                {
                                    // Deserialization from JSON  
                                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                                    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                                    DBEngine objDb = new DBEngine();
                                    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET EWayBillNumber = '" + objIRNDetails.EwbNo + "',EWayBillDate='" + objIRNDetails.EwbDt + "',EwayBill_ValidTill='" + objIRNDetails.EwbValidTill + "' where return_id='" + id.ToString() + "'");
                                    output = "IRN Cancelled successfully.";
                                    success = success + "," + irn;
                                }



                            }
                            else
                            {

                                EinvoiceError err = new EinvoiceError();
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' and ERROR_TYPE='EWAY_GEN'");
                                if (err.error.type != "ClientRequest")
                                {
                                    foreach (errorlog item in err.error.args.irp_error.details)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    if (cErr.error.args.errors != null)
                                    {
                                        foreach (string item in cErr.error.args.errors)
                                        {
                                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','" + "0" + "','" + item + "')");
                                        }
                                    }
                                    else
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','" + "0" + "','Invalid request body.')");
                                    }

                                }

                                error = error + "," + irn;


                                output = "Error occurs while IRN Cancellation.";
                            }



                        }
                    }
                    catch (AggregateException err)
                    {
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAY_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAY_GEN','0','" + err.Message + "')");
                        }

                        error = error + "," + irn;
                    }
                }

                GrdQuotationCR.JSProperties["cpJSON"] = "Ewaybill";
            }



            GrdQuotationCR.JSProperties["cpSuccessMsg"] = success;
            GrdQuotationCR.JSProperties["cpFalureMsg"] = error;
        }

        protected void LinqServerModeDataSourcePendingCR_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceSRs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) == "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceSRs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) == "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoiceSRs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GrdQuotationPendingCR_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdQuotationPendingCR.JSProperties["cpJson"] = "No";
            GrdQuotationPendingCR.JSProperties["cpSuccessMsg"] = null;
            GrdQuotationPendingCR.JSProperties["cpFalureMsg"] = null;

            string success = "";
            string error = "";

            if (e.Parameters.Split('~')[0] == "UploadSingleIRN")
            {
                DataSet ds = new DataSet();

                string id = e.Parameters.Split('~')[2];
                if (e.Parameters.Split('~')[1] == "SI")
                {
                    ds = GetInvoiceDetailsSR(id);
                }

                DataTable Header = ds.Tables[0];
                DataTable SellerDetails = ds.Tables[1];
                DataTable BuyerDetails = ds.Tables[2];
                DataTable ValueDetails = ds.Tables[3];
                DataTable Products = ds.Tables[4];
                DataTable ShipDetails = ds.Tables[5];
                DataTable DispatchFrom = ds.Tables[6];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT RETURN_BranchId FROM TBL_TRANS_SALESRETURN WHERE RETURN_ID='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];

                EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                TrasporterDetails objTransporter = new TrasporterDetails();
                objTransporter.EcmGstin = null;
                objTransporter.IgstOnIntra = "N";
                if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                {
                    objTransporter.RegRev = "Y";     /// From table mantis id 23407
                }
                else
                {
                    objTransporter.RegRev = "N";
                }
                if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                    objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                else
                    objTransporter.SupTyp = "B2B";
                objTransporter.TaxSch = "GST";
                objInvoice.TranDtls = objTransporter;


                DocumentsDetails objDoc = new DocumentsDetails();
                objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                objDoc.Typ = "CRN";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                objInvoice.DocDtls = objDoc;


                SellerDetails objSeller = new SellerDetails();
                objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                    objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                else
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                /// 
                if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                    objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                objInvoice.SellerDtls = objSeller;


                BuyerDetails objBuyer = new BuyerDetails();
                objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                    objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                else
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                    objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objInvoice.BuyerDtls = objBuyer;


                objInvoice.DispDtls = null;  // for now 
                objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                ValueDetails objValue = new ValueDetails();
                objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                objValue.CesVal = 0.00M;
                objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                objValue.Discount = 0.00M;
                objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                objValue.RndOffAmt = 0.00M;
                objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                objValue.StCesVal = 0.00M;
                objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                objValue.TotInvValFc = 0.00M;
                objInvoice.ValDtls = objValue;


                //ExportDetails objExport = new ExportDetails();
                //objExport.CntCode = ""; ///optional for now
                //objExport.ExpDuty = 0;  ///optional for now
                //objExport.ForCur = "";  ///optional for now
                //objExport.Port = "";    ///optional for now
                //objExport.RefClm = "";  ///optional for now
                //objExport.ShipBDt = ""; ///optional for now
                //objExport.ShipBNo = ""; ///optional for now
                //objInvoice.ExpDtls = objExport;

                //EwayBillDetails objEway = new EwayBillDetails();
                //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                //else
                //    objEway.Distance = 0;
                /////
                //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                //objInvoice.EwbDtls = objEway;

                // Mantis Issue 24608
                if (DispatchFrom.Rows.Count > 0)
                {
                    // End of Mantis Issue 24608

                    DispatchDetails objDisp = new DispatchDetails();
                    objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                    objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                    objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                    objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                    objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                    objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                    objInvoice.DispDtls = objDisp;
                }

                if (ShipDetails.Rows.Count > 0)
                {

                    ShipToDetails objShip = new ShipToDetails();
                    objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                    objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                    objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                    objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                    objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                    objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                    objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                    objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                    objInvoice.ShipDtls = objShip;
                }

                //PaymentDetails objPayment = new PaymentDetails();
                //objPayment.AccDet = "";   ///Optional For Now
                //objPayment.CrDay = 0;     ///Optional For Now
                //objPayment.CrTrn = "";    ///Optional For Now
                //objPayment.DirDr = "";    ///Optional For Now
                //objPayment.FinInsBr = ""; ///Optional For Now
                //objPayment.Mode = "";     ///Optional For Now
                //objPayment.Nm = "";       ///Optional For Now
                //objPayment.PaidAmt = 0;   ///Optional For Now
                //objPayment.PayInstr = ""; ///Optional For Now
                //objPayment.PaymtDue = 0;  ///Optional For Now
                //objPayment.PayTerm = "";  ///Optional For Now
                //objInvoice.PayDtls = objPayment;


                //ReferenceDetails objRef = new ReferenceDetails();

                //List<ContractDetails> onjListContact = new List<ContractDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    ContractDetails onjContact = new ContractDetails();
                //    onjContact.ContrRefr = "";
                //    onjContact.ExtRefr = "";
                //    onjContact.PORefDt = "";
                //    onjContact.PORefr = "";
                //    onjContact.ProjRefr = "";
                //    onjContact.RecAdvDt = "";
                //    onjContact.RecAdvRefr = "";
                //    onjContact.TendRefr = "";
                //    onjListContact.Add(onjContact);
                //}
                //objRef.ContrDtls = onjListContact;


                //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                //    onjPrecDoc.InvDt = "";
                //    onjPrecDoc.InvNo = "";
                //    onjPrecDoc.OthRefNo = "";
                //    onjListPrecDoc.Add(onjPrecDoc);
                //}
                //objRef.PrecDocDtls = onjListPrecDoc;

                //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                //objdocPre.InvEndDt = "";
                //objdocPre.InvStDt = "";
                //objRef.DocPerdDtls = objdocPre;

                //objRef.InvRm = "";  // Remarks from invoice
                //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                //    objAddl.Docs = "";
                //    objAddl.Info = "";
                //    objAddl.Url = "";
                //    objListAddl.Add(objAddl);
                //}
                //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                List<ProductList> objListProd = new List<ProductList>();

                foreach (DataRow dr in Products.Rows)
                {
                    ProductList objProd = new ProductList();
                    // objProd.AssAmt = 0.00M;

                    //**************Commented for now -- This is foer Attribute adding ********************************//

                    //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                    //for (int j = 0; j < 1; j++)
                    //{
                    //    AttributeDetails objAttr = new AttributeDetails();
                    //    objAttr.Nm = "";
                    //    objAttr.Val = "";
                    //    objListAttr.Add(objAttr);
                    //}
                    //objProd.AttribDtls = objListAttr;

                    //**************End Commented for now -- This is foer Attribute adding ******************************//

                    objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Barcde = null;
                    objProd.BchDtls = null;
                    objProd.CesAmt = 0.00M;
                    objProd.CesNonAdvlAmt = 0.00M;
                    objProd.CesRt = 0.00M;
                    objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                    objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                    objProd.FreeQty = 0.00M;
                    objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                    objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                    objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                    if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                        objProd.IsServc = "N";
                    else
                        objProd.IsServc = "Y";
                    objProd.OrdLineRef = null;
                    objProd.OrgCntry = null;
                    objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                    objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                    objProd.PrdSlNo = null;
                    objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                    objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                    objProd.SlNo = Convert.ToString(dr["SL"]);
                    objProd.StateCesAmt = 0.00M;
                    objProd.StateCesNonAdvlAmt = 0.00M;
                    objProd.StateCesRt = 0.00M;
                    objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                    if (Convert.ToString(dr["GST_Print_Name"]) != "")
                        objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                    //else
                    //    objProd.Unit = "BAG";
                    objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                    objListProd.Add(objProd);
                }
                objInvoice.ItemList = objListProd;



                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    //REV 5.0
                    IRNV3 objIRN = new IRNV3();
                    //IRN objIRN = new IRN();
                    //REV 5.0 END
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objInvoice, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        //Rev 5.0                       
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");                        
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");                       
                        //Rev 5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            
                            //REV 5.0
                            objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                            if (Convert.ToString(objIRN.data.Irn) != "")
                            {
                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET AckNo='" + objIRN.data.AckNo + "',AckDt='" + objIRN.data.AckDt + "',Irn='" + objIRN.data.Irn + "',SignedInvoice='" + objIRN.data.SignedInvoice + "',SignedQRCode='" + objIRN.data.SignedQRCode + "',Status='" + objIRN.data.Status + "' where return_id='" + id.ToString() + "'");                            
                                success = success + "," + objInvoice.DocDtls.No;
                            }


                            //objIRN = response.Content.ReadAsAsync<IRN>().Result;
                            //    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                            //    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                            //    DBEngine objDb = new DBEngine();
                            //    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "' where return_id='" + id.ToString() + "'");
                            //    //grid.JSProperties["cpSucessIRN"] = "Yes";
                            //    success = success + "," + objInvoice.DocDtls.No;
                            //}
                            //REV 5.0 END
                        }
                        else
                        {
                            //REV 5.0
                            EinvoiceErrorV3 err = new EinvoiceErrorV3();
                            //EinvoiceError err = new EinvoiceError();
                            //REV 5.0 END

                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            //REV 5.0
                            //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);
                            //REV 5.0 END
                            error = error + "," + objInvoice.DocDtls.No;
                            DBEngine objDB = new DBEngine();
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_GEN'");

                            if (err.error.type != "ClientRequest")
                            {
                                //REV 5.0
                                foreach (errorlog item in err.error.args.details)
                                //foreach (errorlog item in err.error.args.irp_error.details)
                                //REV 5.0 END
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','" + "0" + "','" + item + "')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {
                    DBEngine objDB = new DBEngine();
                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_GEN'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','0','" + err.Message + "')");
                    }
                }
            }
            else if (e.Parameters.Split('~')[0] == "UploadBulkIRN")
            {

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];

                var ids = GrdQuotationPendingCR.GetSelectedFieldValues("Invoice_Id");
                foreach (var id in ids)
                {


                    DataSet ds = new DataSet();
                    ds = GetInvoiceDetailsSR(id.ToString());

                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];


                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT RETURN_BranchId FROM TBL_TRANS_SALESRETURN WHERE RETURN_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;


                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }

                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;



                    authtokensOutput authObj = new authtokensOutput();
                    if (DateTime.Now > EinvoiceToken.Expiry)
                    {
                        try
                        {


                            using (HttpClient client = new HttpClient())
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
                                authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                                var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                                var stringContent = new StringContent(json);
                                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var jsonString = response;
                                    var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                    authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                    EinvoiceToken.token = authObj.data.token;
                                    long unixDate = authObj.data.expiry;
                                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                                    EinvoiceToken.Expiry = date;
                                }
                            }
                        }
                        catch (AggregateException err)
                        {
                            DBEngine objDB = new DBEngine();
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_GEN'");

                            foreach (var errInner in err.InnerExceptions)
                            {
                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','0','" + err.Message + "')");
                            }
                        }
                    }

                    try
                    {
                        //REV 5.0
                        IRNV3 objIRN = new IRNV3();
                        //  IRN objIRN = new IRN();
                        //REV 5.0 END
                        using (var client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                            SecurityProtocolType.Tls11 |
                            SecurityProtocolType.Tls12;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var json = JsonConvert.SerializeObject(objInvoice, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                            //Rev 5.0                            
                            //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");                            
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "vay");                           
                            //Rev 5.0 End
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                
                                //REV 5.0
                                objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                                DBEngine objDb = new DBEngine();
                                if (Convert.ToString(objIRN.data.Irn) != "")
                                {
                                    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET AckNo='" + objIRN.data.AckNo + "',AckDt='" + objIRN.data.AckDt + "',Irn='" + objIRN.data.Irn + "',SignedInvoice='" + objIRN.data.SignedInvoice + "',SignedQRCode='" + objIRN.data.SignedQRCode + "',Status='" + objIRN.data.Status + "' where RETURN_id='" + id.ToString() + "'");                                   
                                    success = success + "," + objInvoice.DocDtls.No;
                                }



                                ////objIRN = response.Content.ReadAsAsync<IRN>().Result;
                                //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                                //{
                                //    // Deserialization from JSON  
                                //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                                //    IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                                //    DBEngine objDb = new DBEngine();
                                //    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "' where RETURN_id='" + id.ToString() + "'");
                                //    //grid.JSProperties["cpSucessIRN"] = "Yes";
                                //    success = success + "," + objInvoice.DocDtls.No;
                                //}
                                //REV 5.0 END
                            }
                            else
                            {
                                //REV 5.0
                                //EinvoiceError err = new EinvoiceError();
                                EinvoiceErrorV3 err = new EinvoiceErrorV3();
                                //REV 5.0 END

                                var jsonString = response.Content.ReadAsStringAsync().Result;
                                // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);

                                //REV 5.0
                                //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                                err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);
                                //REV 5.0 END
                                error = error + "," + objInvoice.DocDtls.No;
                                DBEngine objDB = new DBEngine();
                                objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_GEN'");
                                if (err.error.type != "ClientRequest")
                                {
                                    //REV 5.0
                                    foreach (errorlog item in err.error.args.details)
                                    //foreach (errorlog item in err.error.args.irp_error.details)
                                    //REV 5.0 END
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                    }
                                }
                                else
                                {
                                    ClientEinvoiceError cErr = new ClientEinvoiceError();
                                    cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','" + "0" + "','" + item + "')");
                                    }
                                }
                            }


                        }
                    }
                    catch (AggregateException err)
                    {
                        DBEngine objDB = new DBEngine();
                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_GEN'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','0','" + err.Message + "')");
                        }
                    }
                }
            }
            else if (e.Parameters.Split('~')[0] == "DownloadJSON")
            {
                var ids = GrdQuotationPendingCR.GetSelectedFieldValues("Invoice_Id");
                List<EinvoiceModel> obj = new List<EinvoiceModel>();
                foreach (var id in ids)
                {

                    DataSet ds = GetInvoiceDetails(id.ToString());
                    DataTable Header = ds.Tables[0];
                    DataTable SellerDetails = ds.Tables[1];
                    DataTable BuyerDetails = ds.Tables[2];
                    DataTable ValueDetails = ds.Tables[3];
                    DataTable Products = ds.Tables[4];
                    DataTable ShipDetails = ds.Tables[5];
                    DataTable DispatchFrom = ds.Tables[6];


                    DBEngine objDBEngineCredential = new DBEngine();
                    string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                    DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                    string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                    string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                    string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                    EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                    TrasporterDetails objTransporter = new TrasporterDetails();
                    objTransporter.EcmGstin = null;
                    objTransporter.IgstOnIntra = "N";
                    if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                    {
                        objTransporter.RegRev = "Y";     /// From table mantis id 23407
                    }
                    else
                    {
                        objTransporter.RegRev = "N";
                    }
                    if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                        objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                    else
                        objTransporter.SupTyp = "B2B";
                    objTransporter.TaxSch = "GST";
                    objInvoice.TranDtls = objTransporter;


                    DocumentsDetails objDoc = new DocumentsDetails();
                    objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                    objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                    objDoc.Typ = "CRN";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                    objInvoice.DocDtls = objDoc;


                    SellerDetails objSeller = new SellerDetails();
                    objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                    objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                        objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                    //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                    objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                    objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                    if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                    else
                        objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                    /// 
                    if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                        objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                    objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                    objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                    objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                    objInvoice.SellerDtls = objSeller;


                    BuyerDetails objBuyer = new BuyerDetails();
                    objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                        objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    else
                        objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                    if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                        objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                    objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                    objInvoice.BuyerDtls = objBuyer;


                    objInvoice.DispDtls = null;  // for now 
                    objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                    ValueDetails objValue = new ValueDetails();
                    objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                    objValue.CesVal = 0.00M;
                    objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                    objValue.Discount = 0.00M;
                    objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                    objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                    objValue.RndOffAmt = 0.00M;
                    objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                    objValue.StCesVal = 0.00M;
                    objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                    objValue.TotInvValFc = 0.00M;
                    objInvoice.ValDtls = objValue;


                    //ExportDetails objExport = new ExportDetails();
                    //objExport.CntCode = ""; ///optional for now
                    //objExport.ExpDuty = 0;  ///optional for now
                    //objExport.ForCur = "";  ///optional for now
                    //objExport.Port = "";    ///optional for now
                    //objExport.RefClm = "";  ///optional for now
                    //objExport.ShipBDt = ""; ///optional for now
                    //objExport.ShipBNo = ""; ///optional for now
                    //objInvoice.ExpDtls = objExport;

                    //EwayBillDetails objEway = new EwayBillDetails();
                    //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                    //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                    //else
                    //    objEway.Distance = 0;
                    /////
                    //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                    //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                    //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                    //objInvoice.EwbDtls = objEway;


                    // Mantis Issue 24608
                    if (DispatchFrom.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        DispatchDetails objDisp = new DispatchDetails();
                        objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                        objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                        objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                        objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                        objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                        objInvoice.DispDtls = objDisp;
                    }

                    // Mantis Issue 24608
                    if (ShipDetails.Rows.Count > 0)
                    {
                        // End of Mantis Issue 24608
                        ShipToDetails objShip = new ShipToDetails();
                        objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                        objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                        objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                        objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                        objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                        objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                        objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                        objInvoice.ShipDtls = objShip;
                    }

                    //PaymentDetails objPayment = new PaymentDetails();
                    //objPayment.AccDet = "";   ///Optional For Now
                    //objPayment.CrDay = 0;     ///Optional For Now
                    //objPayment.CrTrn = "";    ///Optional For Now
                    //objPayment.DirDr = "";    ///Optional For Now
                    //objPayment.FinInsBr = ""; ///Optional For Now
                    //objPayment.Mode = "";     ///Optional For Now
                    //objPayment.Nm = "";       ///Optional For Now
                    //objPayment.PaidAmt = 0;   ///Optional For Now
                    //objPayment.PayInstr = ""; ///Optional For Now
                    //objPayment.PaymtDue = 0;  ///Optional For Now
                    //objPayment.PayTerm = "";  ///Optional For Now
                    //objInvoice.PayDtls = objPayment;


                    //ReferenceDetails objRef = new ReferenceDetails();

                    //List<ContractDetails> onjListContact = new List<ContractDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    ContractDetails onjContact = new ContractDetails();
                    //    onjContact.ContrRefr = "";
                    //    onjContact.ExtRefr = "";
                    //    onjContact.PORefDt = "";
                    //    onjContact.PORefr = "";
                    //    onjContact.ProjRefr = "";
                    //    onjContact.RecAdvDt = "";
                    //    onjContact.RecAdvRefr = "";
                    //    onjContact.TendRefr = "";
                    //    onjListContact.Add(onjContact);
                    //}
                    //objRef.ContrDtls = onjListContact;


                    //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                    //    onjPrecDoc.InvDt = "";
                    //    onjPrecDoc.InvNo = "";
                    //    onjPrecDoc.OthRefNo = "";
                    //    onjListPrecDoc.Add(onjPrecDoc);
                    //}
                    //objRef.PrecDocDtls = onjListPrecDoc;

                    //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                    //objdocPre.InvEndDt = "";
                    //objdocPre.InvStDt = "";
                    //objRef.DocPerdDtls = objdocPre;

                    //objRef.InvRm = "";  // Remarks from invoice
                    //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                    //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                    //    objAddl.Docs = "";
                    //    objAddl.Info = "";
                    //    objAddl.Url = "";
                    //    objListAddl.Add(objAddl);
                    //}
                    //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                    List<ProductList> objListProd = new List<ProductList>();

                    foreach (DataRow dr in Products.Rows)
                    {
                        ProductList objProd = new ProductList();
                        // objProd.AssAmt = 0.00M;

                        //**************Commented for now -- This is foer Attribute adding ********************************//

                        //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                        //for (int j = 0; j < 1; j++)
                        //{
                        //    AttributeDetails objAttr = new AttributeDetails();
                        //    objAttr.Nm = "";
                        //    objAttr.Val = "";
                        //    objListAttr.Add(objAttr);
                        //}
                        //objProd.AttribDtls = objListAttr;

                        //**************End Commented for now -- This is foer Attribute adding ******************************//

                        objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Barcde = null;
                        objProd.BchDtls = null;
                        objProd.CesAmt = 0.00M;
                        objProd.CesNonAdvlAmt = 0.00M;
                        objProd.CesRt = 0.00M;
                        objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                        objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                        objProd.FreeQty = 0.00M;
                        objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00"));
                        objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                        objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                        if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                            objProd.IsServc = "N";
                        else
                            objProd.IsServc = "Y";
                        objProd.OrdLineRef = null;
                        objProd.OrgCntry = null;
                        objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                        objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                        objProd.PrdSlNo = null;
                        objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                        objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                        objProd.SlNo = Convert.ToString(dr["SL"]);
                        objProd.StateCesAmt = 0.00M;
                        objProd.StateCesNonAdvlAmt = 0.00M;
                        objProd.StateCesRt = 0.00M;
                        objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                        objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                        if (Convert.ToString(dr["GST_Print_Name"]) != "")
                            objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                        //else
                        //    objProd.Unit = "BAG";
                        objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                        objListProd.Add(objProd);
                    }
                    objInvoice.ItemList = objListProd;

                    obj.Add(objInvoice);
                    GrdQuotationPendingCR.JSProperties["cpJson"] = "Yes";
                }


                Session["obj"] = obj;




            }
            GrdQuotationPendingCR.JSProperties["cpSuccessMsg"] = success;
            GrdQuotationPendingCR.JSProperties["cpFalureMsg"] = error;

        }

        protected void LinqServerModeDataSourceCancelCR_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceSRs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.IRNCancel_BranchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == true
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceSRs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.IRNCancel_BranchId)) &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == true
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoiceSRs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void LinqServerModeDataSourceewaybillSI_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoices
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }


        [WebMethod]
        public static object GetTSITabBox(string frmdate, string todate)
        {

            List<TSITabBoxClass> lEfficency = new List<TSITabBoxClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_EINVOICE_DASHBOARD");
            proc.AddVarcharPara("@MODULE_TYPE", 100, "TSI");
            proc.AddVarcharPara("@FROM_DATE", 100, frmdate);
            proc.AddVarcharPara("@TO_DATE", 100, todate);
            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TSITabBoxClass()
                          {
                              TOTAL_COUNTS = Convert.ToString(dr["TOTAL_COUNTS"]),
                              TOTAL_AMOUNT = Convert.ToString(dr["TOTAL_AMOUNT"]),
                              TOTAL_PENDING = Convert.ToString(dr["TOTAL_PENDING"]),
                              TOTAL_PENDING_AMOUNT = Convert.ToString(dr["TOTAL_PENDING_AMOUNT"]),
                              TOTAL_CANCEL = Convert.ToString(dr["TOTAL_CANCEL"]),
                              TOTAL_CANCEL_AMOUNT = Convert.ToString(dr["TOTAL_CANCEL_AMOUNT"]),
                              TOTAL_GENERATED = Convert.ToString(dr["TOTAL_GENERATED"]),
                              TOTAL_GENERATED_AMOUNT = Convert.ToString(dr["TOTAL_GENERATED_AMOUNT"]),
                              TOTAL_EWAY = Convert.ToString(dr["TOTAL_EWAY"]),
                              TOTAL_EWAY_AMOUNT = Convert.ToString(dr["TOTAL_EWAY_AMOUNT"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        public static object GetSRTabBox(string frmdate, string todate)
        {

            List<TSITabBoxClass> lEfficency = new List<TSITabBoxClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_EINVOICE_DASHBOARD");
            proc.AddVarcharPara("@MODULE_TYPE", 100, "SR");
            proc.AddVarcharPara("@FROM_DATE", 100, frmdate);
            proc.AddVarcharPara("@TO_DATE", 100, todate);
            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TSITabBoxClass()
                          {
                              TOTAL_COUNTS = Convert.ToString(dr["TOTAL_COUNTS"]),
                              TOTAL_AMOUNT = Convert.ToString(dr["TOTAL_AMOUNT"]),
                              TOTAL_PENDING = Convert.ToString(dr["TOTAL_PENDING"]),
                              TOTAL_PENDING_AMOUNT = Convert.ToString(dr["TOTAL_PENDING_AMOUNT"]),
                              TOTAL_CANCEL = Convert.ToString(dr["TOTAL_CANCEL"]),
                              TOTAL_CANCEL_AMOUNT = Convert.ToString(dr["TOTAL_CANCEL_AMOUNT"]),
                              TOTAL_GENERATED = Convert.ToString(dr["TOTAL_GENERATED"]),
                              TOTAL_GENERATED_AMOUNT = Convert.ToString(dr["TOTAL_GENERATED_AMOUNT"]),
                              TOTAL_EWAY = Convert.ToString(dr["TOTAL_EWAY"]),
                              TOTAL_EWAY_AMOUNT = Convert.ToString(dr["TOTAL_EWAY_AMOUNT"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        public static object GetSITabBox(string frmdate, string todate)
        {

            List<TSITabBoxClass> lEfficency = new List<TSITabBoxClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_EINVOICE_DASHBOARD");
            proc.AddVarcharPara("@MODULE_TYPE", 100, "SI");
            proc.AddVarcharPara("@FROM_DATE", 100, frmdate);
            proc.AddVarcharPara("@TO_DATE", 100, todate);
            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TSITabBoxClass()
                          {
                              TOTAL_COUNTS = Convert.ToString(dr["TOTAL_COUNTS"]),
                              TOTAL_AMOUNT = Convert.ToString(dr["TOTAL_AMOUNT"]),
                              TOTAL_PENDING = Convert.ToString(dr["TOTAL_PENDING"]),
                              TOTAL_PENDING_AMOUNT = Convert.ToString(dr["TOTAL_PENDING_AMOUNT"]),
                              TOTAL_CANCEL = Convert.ToString(dr["TOTAL_CANCEL"]),
                              TOTAL_CANCEL_AMOUNT = Convert.ToString(dr["TOTAL_CANCEL_AMOUNT"]),
                              TOTAL_GENERATED = Convert.ToString(dr["TOTAL_GENERATED"]),
                              TOTAL_GENERATED_AMOUNT = Convert.ToString(dr["TOTAL_GENERATED_AMOUNT"]),
                              TOTAL_EWAY = Convert.ToString(dr["TOTAL_EWAY"]),
                              TOTAL_EWAY_AMOUNT = Convert.ToString(dr["TOTAL_EWAY_AMOUNT"])
                          }).ToList();
            return lEfficency;
        }

        protected void GrdQuotationewaybillSI_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string success = "";
            string error = "";
            GrdQuotationewaybillSI.JSProperties["cpJson"] = null;
            GrdQuotationewaybillSI.JSProperties["cpSuccessMsg"] = null;
            GrdQuotationewaybillSI.JSProperties["cpFalureMsg"] = null;
            string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
            string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
            string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
            string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
            string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
            string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
            string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];
            string IrnEwaybillDownloadUrl = ConfigurationManager.AppSettings["IrnEwaybillDownloadUrl"];
            string IrnEwaybilCancellUrl = ConfigurationManager.AppSettings["IrnEwaybilCancellUrl"];
            //Rev
            string IrnUpdateEwayBillTransporter = ConfigurationManager.AppSettings["IrnUpdateEwayBillTransporter"];

            #region cancel e way bill
            if (e.Parameters.Split('~')[0] == "CancelEwayBill")
            {
                GrdQuotationewaybillSI.JSProperties["cpJson"] = "cancelEwayBill";
                CancelEwayBill objCancelEwayBill = new CancelEwayBill();
                objCancelEwayBill.ewbNo = Convert.ToInt64(e.Parameters.Split('~')[1]);
                objCancelEwayBill.cancelRsnCode = Convert.ToInt32(e.Parameters.Split('~')[2]);
                objCancelEwayBill.cancelRmrk = Convert.ToString(e.Parameters.Split('~')[3]);
                DBEngine objDB = new DBEngine();
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_SALESINVOICE WHERE  EWayBillNumber='" + objCancelEwayBill.ewbNo + "'").Rows[0][0]);

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);




                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_CANCEL','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    //Rev 5.0
                    IRNV3 objIRN = new IRNV3();
                    //IRN objIRN = new IRN();
                    //Rev 5.0 End
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objCancelEwayBill, Formatting.Indented);
                        var stringContent = new StringContent(json);

                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        //rev 5.0
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        //rev 5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(IrnEwaybilCancellUrl, stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            //Rev 5.0
                            objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                            //objIRN = response.Content.ReadAsAsync<IRN>().Result;
                            //Rev 5.0 End


                            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            // Deserialization from JSON  
                            //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                            //CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);
                            //DBEngine objDb = new DBEngine();
                            //objDb.GetDataTable("update TBL_TRANS_SALESINVOICE EWayBillNumber=NULL,ISEWAYBILLCANCEL=1 SET  where EWayBillNumber='" + objCancelEwayBill.ewbNo + "'");                                
                            //success = success + "," + objCancelEwayBill.ewbNo;                                
                            //objDb.GetDataTable("INSERT INTO EWAYBILL_CANCELHOSTORY(DOC_ID,DOC_TYPE,EWAYBILL_NO,CANCEL_DATE) VALUES ('" + id.ToString() + "','SI','" + objIRNDetails.ewayBillNo + "','" + objIRNDetails.cancelDate + "')");
                            int i = 0;
                            ProcedureExecute proc = new ProcedureExecute("PRC_UpdatePin");
                            proc.AddVarcharPara("@Action", 500, "UpdateCancelEWayBill");
                            proc.AddVarcharPara("@EWayBillNumber", 100, Convert.ToString(objCancelEwayBill.ewbNo));
                            proc.AddVarcharPara("@DOC_TYPE", 100, Convert.ToString("SI"));
                            proc.AddVarcharPara("@DOC_ID", 100, Convert.ToString(id));
                            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                            i = proc.RunActionQuery();

                            success = success + "," + objCancelEwayBill.ewbNo;

                            //}
                        }
                        else
                        {

                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;

                            error = error + "," + objCancelEwayBill.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_GEN'");

                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_CANCEL','" + "0" + "','" + item + "')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_CANCEL','0','" + err.Message + "')");
                    }
                }



            }
            #endregion
            #region update e way bill
            else if (e.Parameters.Split('~')[0] == "UpdateEwayBill")
            {
                DBEngine objDB = new DBEngine();
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_SALESINVOICE WHERE EWayBillNumber='" + Convert.ToInt64(e.Parameters.Split('~')[1]) + "'").Rows[0][0]);

                DataSet ds = GetInvoiceDetails(id);
                DataTable Header = ds.Tables[0];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                GrdQuotationewaybillSI.JSProperties["cpJson"] = "UpdateEwaybill";
                UpdateEwayBill objUpdateEwayBill = new UpdateEwayBill();
                objUpdateEwayBill.ewbNo = Convert.ToInt64(e.Parameters.Split('~')[1]);
                objUpdateEwayBill.reasonCode = Convert.ToString(e.Parameters.Split('~')[2]);
                objUpdateEwayBill.reasonRem = Convert.ToString(e.Parameters.Split('~')[3]);
                if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null && Header.Rows[0]["Transporter_DocDate"] != "")
                    objUpdateEwayBill.transDocDate = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ;
                if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null && Header.Rows[0]["Transporter_DocNo"] != "") objUpdateEwayBill.transDocNo = "";
                if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);
                if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.vehicleNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]); ;
                if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);


                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_UPDATE'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATE','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objUpdateEwayBill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        //rev 5.0
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        //rev 5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        //var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync("https://solo.enriched-api.vayana.com/basic/ewb/v1.0/v1.03/update-part-b", stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            {
                                // Deserialization from JSON  
                                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                                CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);

                                DBEngine objDb = new DBEngine();


                                objDb.GetDataTable("INSERT INTO EWAYBILL_CANCELHOSTORY(DOC_ID,DOC_TYPE,EWAYBILL_NO,CANCEL_DATE) VALUES ('" + ID + "','SI','" + objIRNDetails.ewayBillNo + "','" + objIRNDetails.cancelDate + "')");

                                objDb.GetDataTable("update TBL_TRANS_SALESINVOICE EWayBillNumber=NULL,ISEWAYBILLCANCEL=1 SET  where EWayBillNumber='" + objUpdateEwayBill.ewbNo + "'");
                                //grid.JSProperties["cpSucessIRN"] = "Yes";
                                success = success + "," + objUpdateEwayBill.ewbNo;
                            }
                        }
                        else
                        {

                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;

                            error = error + "," + objUpdateEwayBill.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATE','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                if (cErr.error.args.errors != null)
                                {
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATE','" + "0" + "','" + item + "')");
                                    }
                                }
                                else
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATE','" + "0" + "','Invalid request value.')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_CANCEL','0','" + err.Message + "')");
                    }
                }
            }
            #endregion
            #region update e way bill transporter
            else if (e.Parameters.Split('~')[0] == "UpdateTransporterEwayBill")
            {
                DBEngine objDB = new DBEngine();
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_SALESINVOICE WHERE EWayBillNumber='" + Convert.ToString(e.Parameters.Split('~')[1]) + "'").Rows[0][0]);


                DataSet ds = GetInvoiceDetails(id);
                DataTable Header = ds.Tables[0];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                GrdQuotationewaybillSI.JSProperties["cpJson"] = "UpdateEwaybillTr";
                UpdateEwayBillTransporter objUpdateEwayBillTransporter = new UpdateEwayBillTransporter();
                objUpdateEwayBillTransporter.ewbNo = Convert.ToString(e.Parameters.Split('~')[1]);
                objUpdateEwayBillTransporter.transporterId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);



                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATETR','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    //Rev 5.0
                    IRNV3 objIRN = new IRNV3();
                    //IRN objIRN = new IRN();
                    //Rev 5.0 End
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objUpdateEwayBillTransporter, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        //Rev  5.0
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        //Rev  5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        //Rev  5.0
                        var response = client.PostAsync(IrnUpdateEwayBillTransporter, stringContent).Result;
                        //var response = client.PostAsync("https://live.enriched-api.vayana.com/basic/ewb/v1.0/v1.03/update-part-b", stringContent).Result;
                        //Rev  5.0 End
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            //Rev  5.0
                            objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                            if (Convert.ToString(objIRN.status) == "1")
                            {
                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_SALESINVOICE set FinalTransporter_GSTIN='" + objIRN.data.transporterId + "' where EWayBillNumber='" + objUpdateEwayBillTransporter.ewbNo + "'");
                                success = success + "," + objUpdateEwayBillTransporter.ewbNo;
                            }

                            //objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                            //    CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);

                            //    DBEngine objDb = new DBEngine();


                            //    objDb.GetDataTable("INSERT INTO EWAYBILL_CANCELHOSTORY(DOC_ID,DOC_TYPE,EWAYBILL_NO,CANCEL_DATE) VALUES ('" + ID + "','SI','" + objIRNDetails.ewayBillNo + "','" + objIRNDetails.cancelDate + "')");

                            //    //  objDb.GetDataTable("update TBL_TRANS_SALESINVOICE EWayBillNumber=NULL,ISEWAYBILLCANCEL=1 SET  where EWayBillNumber='" + objUpdateEwayBillTransporter.ewbNo + "'");
                            //    //grid.JSProperties["cpSucessIRN"] = "Yes";
                            //    success = success + "," + objUpdateEwayBillTransporter.ewbNo;
                            //}
                            //Rev  5.0 End
                        }
                        else
                        {
                            //REV 5.0
                            EinvoiceErrorV3 err = new EinvoiceErrorV3();
                            //EinvoiceError err = new EinvoiceError();
                            //REV 5.0 END
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            //REV 5.0
                            err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);
                            //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            //REV 5.0 END
                            error = error + "," + objUpdateEwayBillTransporter.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                            if (err.error.type != "ClientRequest")
                            {

                                //REV 5.0
                                foreach (errorlog item in err.error.args.details)
                                //foreach (errorlog item in err.error.args.irp_error.details)
                                //REV 5.0 END
                                {
                                    if(item.ErrorMessage!=null)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATETR','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");

                                    }
                                    else
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATETR','" + item.ErrorCode + "','" + item.ErrorMessage + "')");

                                    }
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                if (cErr.error.args.errors != null)
                                {
                                    foreach (string item in cErr.error.args.errors)
                                    {
                                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATETR','" + "0" + "','" + item + "')");
                                    }
                                }
                                else
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATETR','" + "0" + "','Invalid Request body.')");
                                }

                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATETR','0','" + err.Message + "')");
                    }
                }
            }
            #endregion
            #region Download PDF

            else if (e.Parameters.Split('~')[0] == "DownloadEwayBill")
            {
                DBEngine objDB = new DBEngine();
                authtokensOutput authObj = new authtokensOutput();
                GrdQuotationewaybillSI.JSProperties["cpJson"] = "DownloadEwaybill";

                string eWaybillNumber = e.Parameters.Split('~')[1];
                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_SALESINVOICE WHERE EWayBillNumber='" + eWaybillNumber.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);



                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", "19b13665-525d-482f-be09-325cca313155");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);

                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        var response = client.GetAsync(IrnEwaybillDownloadUrl + eWaybillNumber).Result;
                        //var file = client.GetStreamAsync("https://live.enriched-api.vayana.com/enriched/tasks/v1.0/download/" + objIRN.data.task_id).Result;
                        //var response = await client.GetAsync(uri);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            //using (var fs = new FileStream(
                            //    HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.pdf", eWaybillNumber.ToString())),
                            //    FileMode.Create))
                            //{
                            //    response.Content.CopyToAsync(fs);
                            //}

                            System.Net.Http.HttpContent content = response.Content; // actually a System.Net.Http.StreamContent instance but you do not need to cast as the actual type does not matter in this case

                            using (var file = System.IO.File.Create(HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.pdf", eWaybillNumber.ToString()))))
                            { // create a new file to write to
                                var contentStream = content.ReadAsStreamAsync(); // get the actual content stream
                                contentStream.Result.CopyTo(file); // copy that stream to the file stream


                                file.Close();

                                // GrdQuotationewaybillSI.JSProperties["cpeWaybillNumber"] = eWaybillNumber;
                                //string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString("Commonfolder/") + eWaybillNumber.ToString() + ".pdf");

                                string strPath = (Convert.ToString("/Commonfolder/") + eWaybillNumber.ToString() + ".pdf");

                                //Response.ContentType = "application/pdf";
                                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + eWaybillNumber.ToString()+".pdf");
                                //Response.TransmitFile(strPath);

                                //try
                                //{
                                //    Response.End();
                                //}
                                //catch (ThreadAbortException ex)
                                //{
                                //}

                                GrdQuotationewaybillSI.JSProperties["cpeWaybillNumber"] = strPath;

                            }

                        }
                        else
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                        }


                    }
                }
                catch (AggregateException err)
                {


                }
            }
            #endregion


            GrdQuotationewaybillSI.JSProperties["cpSuccessMsg"] = success;
            GrdQuotationewaybillSI.JSProperties["cpFalureMsg"] = error;

        }

        protected void LinqServerModeDataSourceCancelewaybill_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoices
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void LinqServerModeDataSourceewaybillTSI_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceTSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceTSIs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoiceTSIs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GrdQuotationewaybillTSI_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string success = "";
            string error = "";
            GrdQuotationewaybillTSI.JSProperties["cpJson"] = null;
            GrdQuotationewaybillTSI.JSProperties["cpSuccessMsg"] = null;
            GrdQuotationewaybillTSI.JSProperties["cpFalureMsg"] = null;

            string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
            string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
            string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
            string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
            string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
            string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
            string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];
            string IrnEwaybillDownloadUrl = ConfigurationManager.AppSettings["IrnEwaybillDownloadUrl"];
            string IrnEwaybilCancellUrl = ConfigurationManager.AppSettings["IrnEwaybilCancellUrl"];

            #region cancel e way bill
            if (e.Parameters.Split('~')[0] == "CancelEwayBill")
            {
                GrdQuotationewaybillTSI.JSProperties["cpJson"] = "cancelEwayBill";
                CancelEwayBill objCancelEwayBill = new CancelEwayBill();
                objCancelEwayBill.ewbNo = Convert.ToInt64(e.Parameters.Split('~')[1]);
                objCancelEwayBill.cancelRsnCode = Convert.ToInt32(e.Parameters.Split('~')[2]);
                objCancelEwayBill.cancelRmrk = Convert.ToString(e.Parameters.Split('~')[3]);
                DBEngine objDB = new DBEngine();
                //Rev 1.0
                //string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE where EWayBillNumber='" + objCancelEwayBill.ewbNo + "'").Rows[0][0]);
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE  EWayBillNumber='" + objCancelEwayBill.ewbNo + "'").Rows[0][0]);
                //Rev 1.0 End
                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE EWayBillNumber='" + objCancelEwayBill.ewbNo.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);




                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAYBILL_CANCEL','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    //rev 5.0
                    IRNV3 objIRN = new IRNV3();
                    //IRN objIRN = new IRN();
                    //rev 5.0 end
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objCancelEwayBill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        //Rev 1.0 
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);

                        //Rev 5.0
                        //Rev Cancel EwayBill (v1.0)
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        //Rev Cancel EwayBill (v1.0) End
                        //Rev Cancel EwayBill (v3.0)
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        //Rev Cancel EwayBill (v3.0) End
                        //Rev 5.0 End
                        //Rev 1.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        //var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync(IrnEwaybilCancellUrl, stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;

                            objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                            //objIRN = response.Content.ReadAsAsync<IRN>().Result;


                            //Rev 1.0 
                            //  using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            // {
                            // Deserialization from JSON  
                            //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                            //CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);
                            //DBEngine objDb = new DBEngine();
                            //objDb.GetDataTable("INSERT INTO EWAYBILL_CANCELHOSTORY(DOC_ID,DOC_TYPE,EWAYBILL_NO,CANCEL_DATE) VALUES ('" + ID + "','TSI','" + objIRNDetails.ewayBillNo + "','" + objIRNDetails.cancelDate + "')");
                            //objDb.GetDataTable("update TBL_TRANS_SALESINVOICE EWayBillNumber=NULL,ISEWAYBILLCANCEL=1 SET  where EWayBillNumber='" + objCancelEwayBill.ewbNo + "'");


                            int i = 0;
                            ProcedureExecute proc = new ProcedureExecute("PRC_UpdatePin");
                            proc.AddVarcharPara("@Action", 500, "UpdateCancelEWayBill");
                            proc.AddVarcharPara("@EWayBillNumber", 100, Convert.ToString(objCancelEwayBill.ewbNo));
                            proc.AddVarcharPara("@DOC_TYPE", 100, Convert.ToString("TSI"));
                            proc.AddVarcharPara("@DOC_ID", 100, Convert.ToString(id));
                            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                            i = proc.RunActionQuery();

                            //Rev 1.0 End
                            success = success + "," + objCancelEwayBill.ewbNo;

                            //Rev 1.0 
                            //grid.JSProperties["cpSucessIRN"] = "Yes";
                            //success = success + "," + objCancelEwayBill.ewbNo;
                            //}
                            //Rev 1.0 End
                        }
                        else
                        {
                            EinvoiceErrorV3 err = new EinvoiceErrorV3();
                            //EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            err = JsonConvert.DeserializeObject<EinvoiceErrorV3>(jsonString);
                            error = error + "," + objCancelEwayBill.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.details)
                                //foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAYBILL_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAYBILL_CANCEL','" + "0" + "','" + item + "')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_CANCEL','0','" + err.Message + "')");
                    }
                }



            }
            #endregion
            #region update e way bill
            else if (e.Parameters.Split('~')[0] == "UpdateEwayBill")
            {



                DBEngine objDB = new DBEngine();
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE where EWayBillNumber='" + Convert.ToInt64(e.Parameters.Split('~')[1]) + "'").Rows[0][0]);

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE Invoice_Id='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);




                DataSet ds = GetInvoiceDetailsTSI(id);
                DataTable Header = ds.Tables[0];
                GrdQuotationewaybillTSI.JSProperties["cpJson"] = "UpdateEwaybill";
                UpdateEwayBill objUpdateEwayBill = new UpdateEwayBill();
                objUpdateEwayBill.ewbNo = Convert.ToInt64(e.Parameters.Split('~')[1]);
                objUpdateEwayBill.reasonCode = Convert.ToString(e.Parameters.Split('~')[2]);
                objUpdateEwayBill.reasonRem = Convert.ToString(e.Parameters.Split('~')[3]);
                if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null && Header.Rows[0]["Transporter_DocDate"] != "")
                    objUpdateEwayBill.transDocDate = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ;
                if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null && Header.Rows[0]["Transporter_DocNo"] != "") objUpdateEwayBill.transDocNo = "";
                if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);
                if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.vehicleNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]); ;
                if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);


                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAYBILL_UPDATE'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAYBILL_UPDATE','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objUpdateEwayBill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        //rev 5.0
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        //rev 5.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        //var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync("/basic/ewb/v1.0/v1.03/update-part-b", stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            {
                                // Deserialization from JSON  
                                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                                CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);
                                success = success + "," + objUpdateEwayBill.ewbNo;
                            }
                        }
                        else
                        {

                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;

                            error = error + "," + objUpdateEwayBill.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAYBILL_UPDATE','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAYBILL_UPDATE','" + "0" + "','" + item + "')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAYBILL_CANCEL','0','" + err.Message + "')");
                    }
                }
            }
            #endregion
            #region update e way bill transporter
            else if (e.Parameters.Split('~')[0] == "UpdateTransporterEwayBill")
            {
                DBEngine objDB = new DBEngine();
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE where EWayBillNumber='" + Convert.ToString(e.Parameters.Split('~')[1]) + "'").Rows[0][0]);

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE Invoice_Id='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);

                DataSet ds = GetInvoiceDetailsSR(id);
                DataTable Header = ds.Tables[0];

                GrdQuotationewaybillTSI.JSProperties["cpJson"] = "UpdateEwaybillTr";
                UpdateEwayBillTransporter objUpdateEwayBillTransporter = new UpdateEwayBillTransporter();
                objUpdateEwayBillTransporter.ewbNo = Convert.ToString(e.Parameters.Split('~')[1]);
                objUpdateEwayBillTransporter.transporterId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);


                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','TSI','EWAYBILL_UPDATETR','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objUpdateEwayBillTransporter, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        //Rev Cancel EwayBill (v3.0)
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        //Rev Cancel EwayBill (v3.0) End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        //var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync("/basic/ewb/v1.0/v1.03/update-part-b", stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            {
                                // Deserialization from JSON  
                                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                                CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);

                                DBEngine objDb = new DBEngine();
                                success = success + "," + objUpdateEwayBillTransporter.ewbNo;
                            }
                        }
                        else
                        {

                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;

                            error = error + "," + objUpdateEwayBillTransporter.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATETR','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATE','" + "0" + "','" + item + "')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','EWAYBILL_UPDATETR','0','" + err.Message + "')");
                    }
                }
            }
            #endregion
            #region Download PDF

            else if (e.Parameters.Split('~')[0] == "DownloadEwayBill")
            {
                DBEngine objDB = new DBEngine();
                authtokensOutput authObj = new authtokensOutput();
                GrdQuotationewaybillTSI.JSProperties["cpJson"] = "DownloadEwaybill";

                string eWaybillNumber = e.Parameters.Split('~')[1];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT INVOICE_BranchId FROM TBL_TRANS_TRANSITSALESINVOICE WHERE EWayBillNumber='" + eWaybillNumber.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        //Rev DownloadEwayBill EwayBill (v3.0)
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        //Rev DownloadEwayBill EwayBill (v3.0) End
                        var response = client.GetAsync(IrnEwaybillDownloadUrl + eWaybillNumber).Result;
                        //var file = client.GetStreamAsync("https://solo.enriched-api.vayana.com/enriched/tasks/v1.0/download/" + objIRN.data.task_id).Result;
                        //var response = await client.GetAsync(uri);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            //using (var fs = new FileStream(
                            //    HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.pdf", eWaybillNumber.ToString())),
                            //    FileMode.Create))
                            //{
                            //    response.Content.CopyToAsync(fs);
                            //}

                            System.Net.Http.HttpContent content = response.Content; // actually a System.Net.Http.StreamContent instance but you do not need to cast as the actual type does not matter in this case

                            using (var file = System.IO.File.Create(HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.pdf", eWaybillNumber.ToString()))))
                            { // create a new file to write to
                                var contentStream = content.ReadAsStreamAsync(); // get the actual content stream
                                contentStream.Result.CopyTo(file); // copy that stream to the file stream                                
                                file.Close();
                                string strPath = (Convert.ToString("/Commonfolder/") + eWaybillNumber.ToString() + ".pdf");

                                GrdQuotationewaybillTSI.JSProperties["cpeWaybillNumber"] = strPath;
                            }

                        }
                        else
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                        }


                    }
                }
                catch (AggregateException err)
                {


                }
            }
            #endregion


            GrdQuotationewaybillTSI.JSProperties["cpSuccessMsg"] = success;
            GrdQuotationewaybillTSI.JSProperties["cpFalureMsg"] = error;
        }

        protected void LinqServerModeDataSourceCancelewaybillTSI_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoices
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void LinqServerModeDataSourceewaybillSR_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceSRs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoiceSRs
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoiceSRs
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GrdQuotationewaybillSR_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string success = "";
            string error = "";
            GrdQuotationewaybillSR.JSProperties["cpJson"] = null;
            GrdQuotationewaybillSR.JSProperties["cpSuccessMsg"] = null;
            GrdQuotationewaybillSR.JSProperties["cpFalureMsg"] = null;


            #region cancel e way bill
            if (e.Parameters.Split('~')[0] == "CancelEwayBill")
            {
                GrdQuotationewaybillSR.JSProperties["cpJson"] = "cancelEwayBill";
                CancelEwayBill objCancelEwayBill = new CancelEwayBill();
                objCancelEwayBill.ewbNo = Convert.ToInt64(e.Parameters.Split('~')[1]);
                objCancelEwayBill.cancelRsnCode = Convert.ToInt32(e.Parameters.Split('~')[2]);
                objCancelEwayBill.cancelRmrk = Convert.ToString(e.Parameters.Split('~')[3]);
                DBEngine objDB = new DBEngine();
                //Rev 1.0
                //string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_SALESRETURN WHERE where EWayBillNumber='" + objCancelEwayBill.ewbNo + "'").Rows[0][0]);
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_SALESRETURN WHERE  EWayBillNumber='" + objCancelEwayBill.ewbNo + "'").Rows[0][0]);

                //Rev 1.0 End
                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Return_BranchId FROM TBL_TRANS_TRANSITSALESReturn WHERE Return_id='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];
                string IrnEwaybilCancellUrl = ConfigurationManager.AppSettings["IrnEwaybilCancellUrl"];

                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_CANCEL','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    //Rev 5.0
                    IRNV3 objIRN = new IRNV3();
                    //IRN objIRN = new IRN();
                    //Rev 5.0 End
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objCancelEwayBill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        //Rev 1.0 
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");

                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);                        
                        //rev 5.0
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "vay");
                        //rev 5.0 End
                        //Rev 1.0 End
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        //var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync(IrnEwaybilCancellUrl, stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            //REV 5.0
                           // objIRN = response.Content.ReadAsAsync<IRN>().Result;
                            objIRN = JsonConvert.DeserializeObject<IRNV3>(jsonString);
                            //REV 5.0 END
                            //Rev 1.0 
                            int i = 0;
                            ProcedureExecute proc = new ProcedureExecute("PRC_UpdatePin");
                            proc.AddVarcharPara("@Action", 500, "UpdateCancelEWayBill");
                            proc.AddVarcharPara("@EWayBillNumber", 100, Convert.ToString(objCancelEwayBill.ewbNo));
                            proc.AddVarcharPara("@DOC_TYPE", 100, Convert.ToString("SR"));
                            proc.AddVarcharPara("@DOC_ID", 100, Convert.ToString(id));
                            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                            i = proc.RunActionQuery();
                            success = success + "," + objCancelEwayBill.ewbNo;


                            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                            //    CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);
                            //    DBEngine objDb = new DBEngine();
                            //    objDb.GetDataTable("INSERT INTO EWAYBILL_CANCELHOSTORY(DOC_ID,DOC_TYPE,EWAYBILL_NO,CANCEL_DATE) VALUES ('" + ID + "','SR','" + objIRNDetails.ewayBillNo + "','" + objIRNDetails.cancelDate + "')");
                            //    objDb.GetDataTable("update TBL_TRANS_SALESRETURN EWayBillNumber=NULL,ISEWAYBILLCANCEL=1 SET  where EWayBillNumber='" + objCancelEwayBill.ewbNo + "'");
                            //    //grid.JSProperties["cpSucessIRN"] = "Yes";
                            //    success = success + "," + objCancelEwayBill.ewbNo;
                            //}
                            //Rev 1.0 End
                        }
                        else
                        {

                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;

                            error = error + "," + objCancelEwayBill.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='TSI' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_CANCEL','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_CANCEL','" + "0" + "','" + item + "')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_CANCEL','0','" + err.Message + "')");
                    }
                }



            }
            #endregion
            #region update e way bill
            else if (e.Parameters.Split('~')[0] == "UpdateEwayBill")
            {
                DBEngine objDB = new DBEngine();
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_TRANSITSALESINVOICE WHERE where EWayBillNumber='" + Convert.ToInt64(e.Parameters.Split('~')[1]) + "'").Rows[0][0]);

                DataSet ds = GetInvoiceDetailsTSI(id);
                DataTable Header = ds.Tables[0];

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];


                GrdQuotationewaybillSR.JSProperties["cpJson"] = "UpdateEwaybill";
                UpdateEwayBill objUpdateEwayBill = new UpdateEwayBill();
                objUpdateEwayBill.ewbNo = Convert.ToInt64(e.Parameters.Split('~')[1]);
                objUpdateEwayBill.reasonCode = Convert.ToString(e.Parameters.Split('~')[2]);
                objUpdateEwayBill.reasonRem = Convert.ToString(e.Parameters.Split('~')[3]);


                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Return_BranchId FROM TBL_TRANS_SALESReturn WHERE Return_id='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);




                if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null && Header.Rows[0]["Transporter_DocDate"] != "")
                    objUpdateEwayBill.transDocDate = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ;
                if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null && Header.Rows[0]["Transporter_DocNo"] != "") objUpdateEwayBill.transDocNo = "";
                if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);
                if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.vehicleNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]); ;
                if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                    objUpdateEwayBill.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);


                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAYBILL_UPDATE'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_UPDATE','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objUpdateEwayBill, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        //var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync("/basic/ewb/v1.0/v1.03/update-part-b", stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            {
                                // Deserialization from JSON  
                                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                                CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);
                                success = success + "," + objUpdateEwayBill.ewbNo;
                            }
                        }
                        else
                        {

                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;

                            error = error + "," + objUpdateEwayBill.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_UPDATE','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_UPDATE','" + "0" + "','" + item + "')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAYBILL_CANCEL'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_CANCEL','0','" + err.Message + "')");
                    }
                }
            }
            #endregion
            #region update e way bill transporter
            else if (e.Parameters.Split('~')[0] == "UpdateTransporterEwayBill")
            {
                DBEngine objDB = new DBEngine();
                string id = Convert.ToString(objDB.GetDataTable("SELECT INVOICE_ID FROM TBL_TRANS_SALESRETURN WHERE where EWayBillNumber='" + Convert.ToString(e.Parameters.Split('~')[1]) + "'").Rows[0][0]);


                DataSet ds = GetInvoiceDetailsSR(id);
                DataTable Header = ds.Tables[0];

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];


                GrdQuotationewaybillSR.JSProperties["cpJson"] = "UpdateEwaybillTr";
                UpdateEwayBillTransporter objUpdateEwayBillTransporter = new UpdateEwayBillTransporter();
                objUpdateEwayBillTransporter.ewbNo = Convert.ToString(e.Parameters.Split('~')[1]);
                objUpdateEwayBillTransporter.transporterId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Return_BranchId FROM TBL_TRANS_SALESReturn WHERE Return_id='" + id.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                authtokensOutput authObj = new authtokensOutput();

                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                        objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                        foreach (var errInner in err.InnerExceptions)
                        {
                            objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_UPDATETR','0','" + err.Message + "')");
                        }
                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objUpdateEwayBillTransporter, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        //var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        var response = client.PostAsync("/basic/ewb/v1.0/v1.03/update-part-b", stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            {
                                // Deserialization from JSON  
                                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CancelEwayBillOutput));
                                CancelEwayBillOutput objIRNDetails = (CancelEwayBillOutput)deserializer.ReadObject(ms);

                                DBEngine objDb = new DBEngine();
                                success = success + "," + objUpdateEwayBillTransporter.ewbNo;
                            }
                        }
                        else
                        {

                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;

                            error = error + "," + objUpdateEwayBillTransporter.ewbNo;

                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_UPDATETR','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_UPDATE','" + "0" + "','" + item + "')");
                                }
                            }

                        }


                    }
                }
                catch (AggregateException err)
                {

                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='EWAYBILL_UPDATETR'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','EWAYBILL_UPDATETR','0','" + err.Message + "')");
                    }
                }
            }
            #endregion
            #region Download PDF

            else if (e.Parameters.Split('~')[0] == "DownloadEwayBill")
            {
                DBEngine objDB = new DBEngine();
                authtokensOutput authObj = new authtokensOutput();
                GrdQuotationewaybillSR.JSProperties["cpJson"] = "DownloadEwaybill";

                string eWaybillNumber = e.Parameters.Split('~')[1];

                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];
                string IrnEwaybillDownloadUrl = ConfigurationManager.AppSettings["IrnEwaybillDownloadUrl"];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Return_BranchId FROM TBL_TRANS_SALESReturn WHERE EWayBillNumber='" + eWaybillNumber.ToString() + "'").Rows[0][0]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EwayBill_Userid"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EwayBill_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);





                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser, IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;
                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {

                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-PWD", IRN_API_Password);
                        //client.DefaultRequestHeaders.Add("X-FLYNN-N-EWB-GSP-CODE", "clayfin");
                        
                        var response = client.GetAsync(IrnEwaybillDownloadUrl + eWaybillNumber).Result;
                        //var file = client.GetStreamAsync("https://solo.enriched-api.vayana.com/enriched/tasks/v1.0/download/" + objIRN.data.task_id).Result;
                        //var response = await client.GetAsync(uri);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            //using (var fs = new FileStream(
                            //    HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.pdf", eWaybillNumber.ToString())),
                            //    FileMode.Create))
                            //{
                            //    response.Content.CopyToAsync(fs);
                            //}

                            System.Net.Http.HttpContent content = response.Content; // actually a System.Net.Http.StreamContent instance but you do not need to cast as the actual type does not matter in this case

                            using (var file = System.IO.File.Create(HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.pdf", eWaybillNumber.ToString()))))
                            { // create a new file to write to
                                var contentStream = content.ReadAsStreamAsync(); // get the actual content stream
                                contentStream.Result.CopyTo(file); // copy that stream to the file stream
                            }

                        }
                        else
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                        }


                    }
                }
                catch (AggregateException err)
                {


                }
            }
            #endregion


            GrdQuotationewaybillSR.JSProperties["cpSuccessMsg"] = success;
            GrdQuotationewaybillSR.JSProperties["cpFalureMsg"] = error;
        }

        protected void LinqServerModeDataSourceCancelewaybillSR_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_EInvoices
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId)) &&
                                  Convert.ToString(d.Irn) != "" &&
                                  Convert.ToString(d.EWayBillNumber) != "" &&
                                  Convert.ToBoolean(d.IsIRNCancelled) == false
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_EInvoices
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        // Mantis Issuse 24030 (24/05/2021)
        public int SendMail(string Output, string url, string customerid)
        {
            int stat = 0;

            Employee_BL objemployeebal = new Employee_BL();
            DataTable dt2 = new DataTable();
            dt2 = objemployeebal.GetSystemsettingmail("Show Email in SI");
            if (Convert.ToString(dt2.Rows[0]["Variable_Value"]) == "Yes")
            {
                ExceptionLogging mailobj = new ExceptionLogging();
                EmailSenderHelperEL emailSenderSettings = new EmailSenderHelperEL();
                DataTable dt_EmailConfig = new DataTable();
                DataTable dt_EmailConfigpurchase = new DataTable();

                DataTable dt_Emailbodysubject = new DataTable();
                SalesOrderEmailTags fetchModel = new SalesOrderEmailTags();
                string Subject = "";
                string Body = "";
                string emailTo = "";
                int MailStatus = 0;
                // var customerid = Convert.ToString(hdnCustomerId.Value);  // now obtained from parameter

                dt_EmailConfig = objemployeebal.Getemailids(customerid);
                // string FilePath = string.Empty;
                string path = System.Web.HttpContext.Current.Server.MapPath("~");
                string path1 = string.Empty;
                string DesignPath = "";

                if (dt_EmailConfig.Rows.Count > 0)
                {
                    emailTo = Convert.ToString(dt_EmailConfig.Rows[0]["eml_email"]);
                    dt_Emailbodysubject = objemployeebal.Getemailtemplates("17");

                    if (dt_Emailbodysubject.Rows.Count > 0)
                    {
                        Body = Convert.ToString(dt_Emailbodysubject.Rows[0]["body"]) + url;
                        Subject = Convert.ToString(dt_Emailbodysubject.Rows[0]["subjct"]);

                        dt_EmailConfigpurchase = objemployeebal.Getemailtagsforpurchase(Output, "SalesInvoiceEmailTags");

                        if (dt_EmailConfigpurchase.Rows.Count > 0)
                        {
                            fetchModel = DbHelpers.ToModel<SalesOrderEmailTags>(dt_EmailConfigpurchase);
                            Body = Employee_BL.GetFormattedString<SalesOrderEmailTags>(fetchModel, Body);
                            Subject = Employee_BL.GetFormattedString<SalesOrderEmailTags>(fetchModel, Subject);
                        }

                        emailSenderSettings = mailobj.GetEmailSettingsforAllreport(emailTo, "", "", null, Body, Subject);

                        if (emailSenderSettings.IsSuccess)
                        {
                            string Message = "";
                            EmailSenderEL obj2 = new EmailSenderEL();
                            stat = SendEmailUL.sendMailInHtmlFormat(emailSenderSettings.ModelCast<EmailSenderEL>(), out Message);
                            if (stat == 1)
                            {
                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET IsMailSend=1 where invoice_id='" + Output.ToString() + "'");
                            }
                        }
                    }
                }
            }
            return stat;
        }
        // End of Mantis Issuse 24030 (24/05/2021)
    }

    public class TSITabBoxClass
    {
        public string TOTAL_COUNTS { get; set; }
        public string TOTAL_AMOUNT { get; set; }
        public string TOTAL_PENDING { get; set; }
        public string TOTAL_PENDING_AMOUNT { get; set; }
        public string TOTAL_CANCEL { get; set; }
        public string TOTAL_CANCEL_AMOUNT { get; set; }
        public string TOTAL_GENERATED { get; set; }
        public string TOTAL_GENERATED_AMOUNT { get; set; }
        public string TOTAL_EWAY { get; set; }
        public string TOTAL_EWAY_AMOUNT { get; set; }
    }

}

