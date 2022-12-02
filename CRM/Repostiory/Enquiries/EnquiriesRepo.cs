using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using System.Data;
using DataAccessLayer;
using DataAccessLayer.Model;
using System.Data.SqlClient;
using System.Collections;

//****************************************************************************************************************************************************************************
//1.0		Subhra				v1.0.105		06/03/2019		disable  verified by control and cotacted by 
//                                                              and show by default loging user contactid.      According to Pijush da                                                               
//****************************************************************************************************************************************************************************
namespace CRM.Repostiory.Enquiries
{
    public class EnquiriesRepo:IEnquiries
    {
        public void save(EnquiriesDet apply,string uniqueID,ref int ReturnCode, ref string ReturnMsg)
        {

            string action = string.Empty;
            DataTable formula_dtls = new DataTable();
            DataSet dsInst = new DataSet();

            try
            {
               
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";

                    paramList.Add(new KeyObj("@USERID", user_id));
                    paramList.Add(new KeyObj("@ACTION_TYPE",apply.Action_type));
                    paramList.Add(new KeyObj("@DATE", apply.Date));
                    paramList.Add(new KeyObj("@CUSTNAME", apply.Customer_Name));
                    paramList.Add(new KeyObj("@CONTACTPERSON", apply.Contact_Person));
                    paramList.Add(new KeyObj("@PHONENO", apply.PhoneNo));
                    paramList.Add(new KeyObj("@EMAIL",apply.Email));
                    paramList.Add(new KeyObj("@LOCATION", apply.Location ));
                    paramList.Add(new KeyObj("@PRODUCTREQUIRED", apply.Product_Required));
                    paramList.Add(new KeyObj("@QTY", apply.Qty));
                    paramList.Add(new KeyObj("@ORDER_VALUE", apply.Order_Value));
                    paramList.Add(new KeyObj("@ENQ_DETAILS", apply.Enq_Details));
                    if (!String.IsNullOrEmpty(uniqueID))
                    {
                        paramList.Add(new KeyObj("@CRM_ID", (uniqueID)));
                    }
                    paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                    paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));
    
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public EnquiriesDet getEnquiryById(string _enquiryId, string EditFlag)
        {
            DataSet _getenq = new DataSet();
            EnquiriesDet _apply = new EnquiriesDet();
            //EnquiriesDet _header = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", EditFlag));
                paramList.Add(new KeyObj("@CRM_ID", _enquiryId));
                
                execProc.param = paramList;
                _getenq = execProc.ExecuteProcedureGetDataSet();

                if (_getenq.Tables[0].Rows.Count > 0)
                {

                    _apply.Customer_Name = _getenq.Tables[0].Rows[0]["Customer_Name"].ToString();
                    _apply.Contact_Person = _getenq.Tables[0].Rows[0]["Contact_Person"].ToString();
                    _apply.Date = Convert.ToDateTime(_getenq.Tables[0].Rows[0]["Date"]);
                    _apply.PhoneNo = _getenq.Tables[0].Rows[0]["PhoneNo"].ToString();
                    _apply.Email = _getenq.Tables[0].Rows[0]["Email"].ToString();
                    _apply.Location = _getenq.Tables[0].Rows[0]["Location"].ToString();
                    _apply.Product_Required = _getenq.Tables[0].Rows[0]["Product_Required"].ToString();
                    _apply.Qty = _getenq.Tables[0].Rows[0]["Qty"].ToString();
                    _apply.Order_Value = Convert.ToDecimal(_getenq.Tables[0].Rows[0]["Order_Value"]);
                    _apply.Enq_Details = _getenq.Tables[0].Rows[0]["Enq_Details"].ToString();
                    _apply.vend_type = _getenq.Tables[0].Rows[0]["vend_type"].ToString();
                    _apply.SUPERVISOR = Convert.ToBoolean(_getenq.Tables[0].Rows[0]["Supervisor"]);
                    _apply.SALESMAN = Convert.ToBoolean(_getenq.Tables[0].Rows[0]["salesman"]);
                    _apply.VERIFY = Convert.ToBoolean(_getenq.Tables[0].Rows[0]["verify"]);
   
                }

            }
            catch (Exception ex)
            {

            }

            return _apply;
        }
        public string Delete(string ActionType, string uniqueid, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;

            DataTable formula_dtls = new DataTable();

            try
            {
               
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                    paramList.Add(new KeyObj("@ACTION_TYPE", ActionType));
                    paramList.Add(new KeyObj("@CRM_ID", uniqueid));//ADDEDITCATEGORYIMPORT
                   

                    paramList.Add(new KeyObj("@ReturnMessage", output, true));
                    paramList.Add(new KeyObj("@ReturnCode", ReturnCode, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    output = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);
                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }
        public string MassDelete(string ActionType, string uniqueid, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;

            DataTable formula_dtls = new DataTable();

            try
            {

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                    paramList.Add(new KeyObj("@ACTION_TYPE", ActionType));
                    paramList.Add(new KeyObj("@CRM_IDS", uniqueid));//ADDEDITCATEGORYIMPORT


                    paramList.Add(new KeyObj("@ReturnMessage", output, true));
                    paramList.Add(new KeyObj("@ReturnCode", ReturnCode, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    output = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);
                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }
        public void GetListing(string EnquiriesFrom,string FromDate,string ToDate)
        {

            //string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            //string TODATE = dtTo.ToString("yyyy-MM-dd");

            string action = string.Empty;
            DataTable formula_dtls = new DataTable();
            DataSet dsInst = new DataSet();

            try
            {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_ENQUIRIES_LISTING";
                    paramList.Add(new KeyObj("@USERID", user_id));
                    paramList.Add(new KeyObj("@ENQUIRIESFROM", EnquiriesFrom));
                    paramList.Add(new KeyObj("@FROMDATE", FromDate));
                    paramList.Add(new KeyObj("@TODATE", ToDate));

                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public List<Industry> getIndustry()
        {
           
            DataSet _getenq = new DataSet();
            List<Industry> indlist = new List<Industry>();
            Industry ind = null;
            EnquiriesDet _apply = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_IndustryBinding";
                execProc.param = paramList;
                _getenq = execProc.ExecuteProcedureGetDataSet();
                foreach(DataRow dr in _getenq.Tables[0].Rows)
                {
                    ind = new Industry();
                    ind.Industry_Id = Convert.ToInt32(dr[0]);
                    ind.Industry_Name = dr[1].ToString();
                    indlist.Add(ind);
                }

            }
            catch (Exception ex)
            {

            }
            return indlist;
        }
        
        public List<Employee> getEmployee()
        {

            DataSet _getenq = new DataSet();
            List<Employee> emplist = new List<Employee>();
            Employee emp = null;
            EnquiriesDet _apply = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CUSTOMER";
                paramList.Add(new KeyObj("@TYPE", "CUSTOMER"));
                execProc.param = paramList;
                _getenq = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getenq.Tables[0].Rows)
                {
                    emp = new Employee();
                    emp.ID = dr[0].ToString();
                    emp.Name = dr[1].ToString();
                    emplist.Add(emp);
                }

            }
            catch (Exception ex)
            {

            }
            return emplist;
        }

        public List<Salesman> getSalesman()
        {

            DataSet _getenq = new DataSet();
            List<Salesman> saleslist = new List<Salesman>();
            Salesman salesman = null;
            EnquiriesDet _apply = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CUSTOMER";
                //Rev 1.0 details in Enquiries Controller
                //paramList.Add(new KeyObj("@TYPE", "SALESMAN"));
                paramList.Add(new KeyObj("@TYPE", "CUSTOMER_USER"));
                //End of Rev 1.0 details in Enquiries Controller
                execProc.param = paramList;
                _getenq = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getenq.Tables[0].Rows)
                {
                    salesman = new Salesman();
                    salesman.Name = dr[0].ToString();
                    salesman.ID = Convert.ToInt32(dr[1]);
                    saleslist.Add(salesman);
                }

            }
            catch (Exception ex)
            {

            }
            return saleslist;
        }
        public EnquiriesSupervisorFeedback getSuperVisorById(string _enquiryId)
        {
            DataSet _getenqsupervisor = new DataSet();
            EnquiriesSupervisorFeedback _applysupervisor = new EnquiriesSupervisorFeedback();
            //EnquiriesDet _header = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_ENQUIRIES_SUPERVISOR";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GETSUPERVISOR"));
                paramList.Add(new KeyObj("@ENQ_CRM_ID", _enquiryId));

                execProc.param = paramList;
                _getenqsupervisor = execProc.ExecuteProcedureGetDataSet();

                if (_getenqsupervisor.Tables[0].Rows.Count > 0)
                {
                    _applysupervisor.Unique_ID = _getenqsupervisor.Tables[0].Rows[0]["enq_crm_id"].ToString();
                    _applysupervisor.date = Convert.ToDateTime(_getenqsupervisor.Tables[0].Rows[0]["date"]);
                    _applysupervisor.source = _getenqsupervisor.Tables[0].Rows[0]["source"].ToString();
                    _applysupervisor.IndustryId = Convert.ToInt32(_getenqsupervisor.Tables[0].Rows[0]["IndustryId"]);
                    _applysupervisor.Misc_comments = _getenqsupervisor.Tables[0].Rows[0]["Misc_comments"].ToString();
                    _applysupervisor.enq_priorityID = Convert.ToInt32(_getenqsupervisor.Tables[0].Rows[0]["enq_priorityID"]);
                    _applysupervisor.checkedcustomer = _getenqsupervisor.Tables[0].Rows[0]["Is_Exist_Customer"].ToString();

                }

            }
            catch (Exception ex)
            {

            }

            return _applysupervisor;
        }
        public EnquiriesSalesmanFeedback getSalesmanById(string _enquiryId)
        {
            DataSet _getenqsalesman = new DataSet();
            EnquiriesSalesmanFeedback _applysalesman = new EnquiriesSalesmanFeedback();
            //EnquiriesDet _header = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_ENQUIRIES_SALESMAN";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GETSALESMAN"));
                paramList.Add(new KeyObj("@ENQ_CRM_ID", _enquiryId));

                execProc.param = paramList;
                _getenqsalesman = execProc.ExecuteProcedureGetDataSet();

                if (_getenqsalesman.Tables[0].Rows.Count > 0)
                {
                    _applysalesman.Unique_ID = _getenqsalesman.Tables[0].Rows[0]["enq_crm_id"].ToString();
                    _applysalesman.enq_prodreq = _getenqsalesman.Tables[0].Rows[0]["enq_prodreq"].ToString();
                    _applysalesman.feedback = _getenqsalesman.Tables[0].Rows[0]["feedback"].ToString();
                    _applysalesman.Final_IndustryId = Convert.ToInt32(_getenqsalesman.Tables[0].Rows[0]["final_industry"]);
                    _applysalesman.usefullid = _getenqsalesman.Tables[0].Rows[0]["Is_useful"].ToString();
                    _applysalesman.last_contactdate = Convert.ToDateTime(_getenqsalesman.Tables[0].Rows[0]["last_contactdate"]);
                    _applysalesman.next_contactdate = Convert.ToDateTime(_getenqsalesman.Tables[0].Rows[0]["next_contactdate"]);
                    _applysalesman.Contractedby = Convert.ToInt32(_getenqsalesman.Tables[0].Rows[0]["contactedby"]);
                }
                else
                {
                    //Rev 1.0
                    _applysalesman.Contractedby = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    //End of Rev 1.0
                }

            }
            catch (Exception ex)
            {

            }

            return _applysalesman;
        }
        public List<EnquiriesShowHistorySalesman> getShowHistorySalesmanById(string _enquiryId)
        {
            DataSet _getenqsalesman = new DataSet();
            EnquiriesShowHistorySalesman _applysalesman = new EnquiriesShowHistorySalesman();
            List<EnquiriesShowHistorySalesman> objList = new List<EnquiriesShowHistorySalesman>();
            //EnquiriesDet _header = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_ENQUIRIES_SALESMAN";
                paramList.Add(new KeyObj("@ACTION_TYPE", "SHOWHISTORY"));
                paramList.Add(new KeyObj("@ENQ_CRM_ID", _enquiryId));

                execProc.param = paramList;
                _getenqsalesman = execProc.ExecuteProcedureGetDataSet();


                if (_getenqsalesman.Tables[0].Rows.Count > 0)
                {


                    foreach (DataRow dr in _getenqsalesman.Tables[0].Rows)
                    {
                        _applysalesman = new EnquiriesShowHistorySalesman();
                       
                        _applysalesman.Unique_ID = dr["enq_crm_id"].ToString();
                        _applysalesman.enq_prodreq = dr["enq_prodreq"].ToString();
                        _applysalesman.feedback = dr["feedback"].ToString();
                        _applysalesman.Final_IndustryId = dr["final_industry"].ToString();
                        _applysalesman.usefullid = dr["Is_useful"].ToString();
                        _applysalesman.last_contactdate = Convert.ToDateTime(dr["last_contactdate"]);
                        _applysalesman.next_contactdate = Convert.ToDateTime(dr["next_contactdate"]);
                        _applysalesman.Contactedby = dr["contactedby"].ToString();
                        _applysalesman.created_date = Convert.ToDateTime(dr["created_date"]);
                        objList.Add(_applysalesman);

                    }


                }

            }
            catch (Exception ex)
            {

            }

            return objList;
        }
        public EnquiriesVerify getVerifyId(string _enquiryId)
        {
            DataSet _getenqverify = new DataSet();
            EnquiriesVerify _applyverify = new EnquiriesVerify();
            //EnquiriesDet _header = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_ENQUIRIES_VERIFIED";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GETVERIFY"));
                paramList.Add(new KeyObj("@ENQ_CRM_ID", _enquiryId));

                execProc.param = paramList;
                _getenqverify = execProc.ExecuteProcedureGetDataSet();

                if (_getenqverify.Tables[0].Rows.Count > 0)
                {
                    _applyverify.Unique_ID = _getenqverify.Tables[0].Rows[0]["enq_crm_id"].ToString();
                    _applyverify.verify_by = _getenqverify.Tables[0].Rows[0]["verify_by"].ToString();
                    _applyverify.verified_on = Convert.ToDateTime(_getenqverify.Tables[0].Rows[0]["verified_on"]);
                    _applyverify.closure_date =Convert.ToDateTime(_getenqverify.Tables[0].Rows[0]["closure_date"]);
                    //_applyverify.created_by = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                }
                else {
                        DataSet _getenqverifyenq = new DataSet();
                        ExecProcedure execProc1 = new ExecProcedure();
                        List<KeyObj> paramList1 = new List<KeyObj>();
                        execProc.ProcedureName = "PRC_ENQUIRIES_VERIFIED";
                        paramList1.Add(new KeyObj("@ACTION_TYPE", "GETCONTACTID"));
                        paramList1.Add(new KeyObj("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"])));
                        execProc.param = paramList1;
                        _getenqverifyenq = execProc.ExecuteProcedureGetDataSet();
                        _applyverify.verify_by = _getenqverifyenq.Tables[0].Rows[0]["user_contactId"].ToString();
                }

            }
            catch (Exception ex)
            {

            }

            return _applyverify;
        }
        public void Supervisorsave(EnquiriesSupervisorFeedback supervisorapply, ref int ReturnCode, ref string ReturnMsg)
        {

            string action = string.Empty;
            DataTable formula_dtls = new DataTable();
            DataSet dsInst = new DataSet();

            try
            {

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_ENQUIRIES_SUPERVISOR";

                    paramList.Add(new KeyObj("@USERID", user_id));
                    paramList.Add(new KeyObj("@ACTION_TYPE", "SAVE"));
                    paramList.Add(new KeyObj("@ENQ_CRM_ID", supervisorapply.Unique_ID));
                    paramList.Add(new KeyObj("@SOURCE", supervisorapply.source));
                    paramList.Add(new KeyObj("@INDUSTRYID", supervisorapply.IndustryId));
                    paramList.Add(new KeyObj("@MISC_COMMENTS", supervisorapply.Misc_comments));
                    paramList.Add(new KeyObj("@ENQ_PRIORITYID", supervisorapply.enq_priorityID));
                    paramList.Add(new KeyObj("@IS_EXIST_CUSTOMER", supervisorapply.checkedcustomer));
                    paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                    paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));

                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Salesmansave(EnquiriesSalesmanFeedback salesmanapply, ref int ReturnCode, ref string ReturnMsg)
        {

            string action = string.Empty;
            DataTable formula_dtls = new DataTable();
            DataSet dsInst = new DataSet();

            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_ENQUIRIES_SALESMAN";

                    paramList.Add(new KeyObj("@USERID", user_id));
                    paramList.Add(new KeyObj("@ACTION_TYPE", "SAVESALESMAN"));
                    paramList.Add(new KeyObj("@ENQ_CRM_ID", salesmanapply.Unique_ID));
                    paramList.Add(new KeyObj("@ENQ_PRODREQ", salesmanapply.enq_prodreq));
                    paramList.Add(new KeyObj("@FEEDBACK", salesmanapply.feedback));
                    paramList.Add(new KeyObj("@FINALINDUSTRYID", salesmanapply.Final_IndustryId));
                    paramList.Add(new KeyObj("@IS_USEFUL", salesmanapply.usefullid));
                    paramList.Add(new KeyObj("@LAST_CONTACTDATE", DateTime.Today));
                    paramList.Add(new KeyObj("@NEXT_CONTACTDATE", salesmanapply.next_contactdate));
                    paramList.Add(new KeyObj("@CONTRACTEDBY", salesmanapply.Contractedby));
                    paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                    paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));

                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Verifiedsave(EnquiriesVerify verifyapply, ref int ReturnCode, ref string ReturnMsg)
        {

            string action = string.Empty;
            DataTable formula_dtls = new DataTable();
            DataSet dsInst = new DataSet();

            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_ENQUIRIES_VERIFIED";

                    paramList.Add(new KeyObj("@USERID", user_id));
                    paramList.Add(new KeyObj("@ACTION_TYPE", "SAVEVERIFY"));
                    paramList.Add(new KeyObj("@ENQ_CRM_ID", verifyapply.Unique_ID));
                    paramList.Add(new KeyObj("@VERIFY_BY", verifyapply.verify_by));
                    paramList.Add(new KeyObj("@VERIFIED_ON", verifyapply.verified_on));
                    paramList.Add(new KeyObj("@CLOSURE_DATE", verifyapply.closure_date));
                    paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                    paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));

                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

       

    }
}